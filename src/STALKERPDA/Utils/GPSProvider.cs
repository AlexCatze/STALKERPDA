using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace STALKERPDA.Utils
{
    public class GPSProvider
    {
        public GPSProvider()
        {
        }

        public LatLon GetLatLon()
        {
            return new LatLon(0,0);
        }

        public bool IsGpsAvailable()
        {
            return false;
        }

        public struct LatLon
        {
            public double Lat, Lon;

            public LatLon(double lat, double lon)
            {
                Lat = lat; Lon = lon;
            }
        }
    }
}
