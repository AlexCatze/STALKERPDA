using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.IO.Serial;
using OpenNETCF.IO.Serial.GPS;

namespace STALKERPDA.Utils
{
    public class GPSProvider
    {
        private readonly object _syncObject = new object();

        private const string serialPortName = "COM9:";

        BaudRates baudRate = BaudRates.CBR_115200;

        GPS gps;

        private LatLon _LatLon = new LatLon(0, 0);

        public GPSProvider()
        {
            gps = new GPS();

            gps.BaudRate = baudRate;
            gps.ComPort = serialPortName;
            gps.AutoDiscovery = false;

            gps.Position += OnPos;
            gps.Error += new GPS.ErrorEventHandler(gps_Error);

            gps.Start();
        }

        void gps_Error(object sender, Exception exception, string message, string gps_data)
        {
            //throw new NotImplementedException();
        }

        public LatLon GetLatLon()
        {
            return _LatLon;
        }

        public bool IsGpsAvailable()
        {
            return gps.State == States.Running;
        }

        private DateTime lastUpdated;
        private int updCount = 0;
        private double _lat = 0, _lon = 0;

        public event EventHandler OnPosUpdated;

        protected void OnPos(object sender, Position pos)
        {
            updCount++;
            _lat += (double)pos.Latitude_Decimal;
            _lon += (double)pos.Longitude_Decimal;

            if (DateTime.Now - lastUpdated > TimeSpan.FromSeconds(1))
            {
                _LatLon = new LatLon(_lat/updCount,_lon/updCount);

                updCount = 0;
                _lat = 0;
                _lon = 0;
                lastUpdated = DateTime.Now;

                OnPosUpdated.Invoke(this,null);
            }
        }

        public struct LatLon
        {
            public double Lat, Lon;

            public LatLon(double lat, double lon)
            {
                Lat = lat; Lon = lon;
            }
        }

        public enum GpsState
        {
            Ok,
            Warning,
            Bad,
            Error
        }

        public GpsState GetGpsState()
        {
            if (gps.State != States.Running) return GpsState.Error;

            if (gps.GpsState == StatusType.Warning) return GpsState.Warning;

            return GpsState.Ok;
        }
    }
}
