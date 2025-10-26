using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using STALKERPDA.Controls;
using System.IO;
using System.Threading;
using OpenNETCF.Drawing.Imaging;

namespace STALKERPDA.Utils
{
    public class MapTileProvider
    {
        private string MAP_FOLDER;

        private static string[] MAP_PREFIXES = { "/", "/Storage Card/", "/CF Card/", "/PCMCIA Card/", "/SD Card/" };

        private const int CACHE_LIMIT = 80;

        private Dictionary<long, Bitmap> BitmapCache = new Dictionary<long, Bitmap>();

        private long CenterTileId = 0;

        private List<long> CacheList = new List<long>();

        private Bitmap EmptyTile;

        private List<long> WaitList = new List<long>();

        public event EventHandler OnWaitedTileArrived;

        public MapTileProvider()
        {
            /*EmptyTile = new Bitmap(MapView.TILE_SIDE, MapView.TILE_SIDE);

            var color = Color.FromArgb(0, 0, 255);

            using (var g = Graphics.FromImage(EmptyTile))
            {
                g.FillRectangle(new SolidBrush(color), 0, 0, MapView.TILE_SIDE, MapView.TILE_SIDE);
            }*/

            var stream = GetType().Assembly.GetManifestResourceStream("STALKERPDA.Images.Ui.emptyTile.jpg");

            EmptyTile = new Bitmap(stream);


            foreach (var pref in MAP_PREFIXES)
            {
                var fullPath = pref + "map/";
                if (Directory.Exists(fullPath))
                {
                    MAP_FOLDER = fullPath;
                    break;
                }
            }

            var workerThreadStart = new ThreadStart(WorkerThread);
            var workerThread = new Thread(workerThreadStart);
            workerThread.IsBackground = true;
            workerThread.Priority = ThreadPriority.BelowNormal;
            workerThread.Start();
        }

        private long GetTileId(int x, int y, int z)
        {
            return (x << 28) + y + (z << 56);//FIXME add mask
        }

        private string GetMapPath(int x, int y, int z)
        {
            return MAP_FOLDER + z.ToString() + '/'+x.ToString()+'/'+y.ToString()+".jpg";
        }

        private List<TileCoords> ToLoad = new List<TileCoords>();

        private void WorkerThread()
        {
            while (true)
            {
                if (ToLoad.Count > 0)
                {
                    var item = ToLoad.First();
                    ToLoad.Remove(item);

                    PutBitmapToCache(item);

                    var longId = GetTileId(item.x, item.y, item.z);

                    if (WaitList.Contains(longId))
                    {
                        WaitList.Remove(longId);
                        OnWaitedTileArrived.Invoke(this, null);
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }
            }

        }

        /*
        private void EnsureBitmapInCache(int x, int y, int z)
        {
            if (BitmapCache.Keys.Contains(GetTileId(x, y, z))) return;



            if (BitmapCache.Count > CACHE_LIMIT)
            {
                var el = BitmapCache.FirstOrDefault(b=>!CacheList.Contains(b.Key));
                if (el.Value != null)
                {
                    el.Value.Dispose();
                    BitmapCache.Remove(el.Key);
                }
            }

            BitmapCache.Add(GetTileId(x, y, z), LoadBitmap(x, y, z));
        }*/

        private struct TileCoords
        {
            public int x, y, z;
        }

        public void LoadBitmap(int x, int y, int z)
        {
            TileCoords coords = new TileCoords();
            coords.x = x ;
            coords.y = y;
            coords.z = z;

            CacheList.Add(GetTileId(x, y, z));
            //System.Threading.ThreadPool.QueueUserWorkItem(PutBitmapToCache, coords);
            //PutBitmapToCache(coords);
            ToLoad.Add(coords);
        }

        public void UpdateCacheList(int x, int y, int z)
        {
            if(CenterTileId==GetTileId(x, y, z)) return;
            CenterTileId = GetTileId(x, y, z);

            CacheList.Clear();

            for (int i = -2; i <= 2; i++)
                for (int k = -2; k <= 2; k++)
                    LoadBitmap(x + i, y + k, z);

            for (int i = -2; i <= 2; i++)
                for (int k = -2; k <= 2; k++)
                LoadBitmap((x / 2) + i, (y / 2) + k, z - 1);

            for (int i = -2; i <= 2; i++)
                for (int k = -2; k <= 2; k++)
                LoadBitmap((x * 2) + i, (y * 2) + k, z + 1);

            List<TileCoords> toRemove = new List<TileCoords>();

            foreach(var i in ToLoad)
                if(!CacheList.Contains(GetTileId(i.x,i.y,i.z)))
                    toRemove.Add(i);

            foreach(var i in toRemove)
                ToLoad.Remove(i);

        }

        public Bitmap GetBitmap(int x, int y, int z)
        {
            //EnsureBitmapInCache(x,y,z);
            if (BitmapCache.ContainsKey(GetTileId(x, y, z)))
                return BitmapCache[GetTileId(x, y, z)];
            else
            {
                WaitList.Add(GetTileId(x, y, z));
                return EmptyTile;
            }
        }

        public void PutBitmapToCache(object _coords)
        {
            TileCoords coords = (TileCoords)_coords;
            if(BitmapCache.Keys.Contains(GetTileId(coords.x, coords.y, coords.z))) return;

            if (BitmapCache.Count > CACHE_LIMIT)
            {
                var el = BitmapCache.FirstOrDefault(b => !CacheList.Contains(b.Key));
                if (el.Value != null)
                {
                    if(el.Value != EmptyTile)
                        el.Value.Dispose();
                    BitmapCache.Remove(el.Key);
                }
            }

            var tilePath = GetMapPath(coords.x, coords.y, coords.z);

            if (File.Exists(tilePath))
            {
                BitmapCache.Add(GetTileId(coords.x, coords.y, coords.z), new Bitmap(tilePath));
            }
            else
            {
                BitmapCache.Add(GetTileId(coords.x, coords.y, coords.z), EmptyTile);
            }
        }
        /*
        public Bitmap LoadBitmap(int x, int y, int z)
        {
            var tilePath = GetMapPath( x, y, z);

            if (File.Exists(tilePath))
            {
                return new Bitmap(tilePath);
            }
            else
            {
                return null;
                /*
                var i = new Bitmap(MapView.TILE_SIDE, MapView.TILE_SIDE);

                var color = (((x + y) % 2) > 0) ? Color.FromArgb(0, 0, 255) : Color.FromArgb(0, 255, 0);
                var font = new Font(FontFamily.GenericSerif, 6, FontStyle.Bold);
                string text = x + " " + y + " " + z;

                using (var g = Graphics.FromImage(i))
                {
                    var mes = g.MeasureString(text, font);

                    g.FillRectangle(new SolidBrush(color), 0, 0, MapView.TILE_SIDE, MapView.TILE_SIDE);

                    g.DrawString(text, font, new SolidBrush(Color.Black), (MapView.TILE_SIDE - mes.Width) / 2, (MapView.TILE_SIDE - mes.Height) / 2);
                }

                return i;*/
            //}
        //}
    }
}
