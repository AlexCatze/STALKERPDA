using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using STALKERPDA.Controls;
using System.IO;

namespace STALKERPDA.Utils
{
    public class MapTileProvider
    {
        private const string MAP_FOLDER = "/Storage Card/map/";

        private const int CACHE_LIMIT = 32;

        private Dictionary<long, Bitmap> BitmapCache = new Dictionary<long, Bitmap>();

        private long CenterTileId = 0;

        private List<long> CacheList = new List<long>();

        private long GetTileId(int x, int y, int z)
        {
            return (x << 28) + y + (z << 56);//FIXME add mask
        }

        private string GetMapPath(int x, int y, int z)
        {
            return MAP_FOLDER + z.ToString() + '/'+x.ToString()+'/'+y.ToString()+".jpg";
        }

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
            }//FIXME remove most useless

            BitmapCache.Add(GetTileId(x, y, z), LoadBitmap(x, y, z));
        }

        public void UpdateCacheList(int x, int y, int z)
        {
            if(CenterTileId==GetTileId(x, y, z)) return;
            CenterTileId = GetTileId(x, y, z);

            CacheList.Clear();
            for (int i = -3; i <= 3; i++)
                for (int k = -3; k <= 3; k++)
                    CacheList.Add(GetTileId(x + i, y + k, z));
        }

        public Bitmap GetBitmap(int x, int y, int z)
        {
            EnsureBitmapInCache(x,y,z);

            return BitmapCache[GetTileId(x, y, z)];
        }

        public Bitmap LoadBitmap(int x, int y, int z)
        {
            var tilePath = GetMapPath( x, y, z);

            if (File.Exists(tilePath))
            {
                return new Bitmap(tilePath);
            }
            else
            {
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

                return i;
            }
        }
    }
}
