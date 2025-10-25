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

        private string GetMapPath(int x, int y, int z)
        {
            return MAP_FOLDER + z.ToString() + '/'+x.ToString()+'/'+y.ToString()+".jpg";
        }

        public Bitmap GetBitmap(int x, int y, int z)
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
