using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace STALKERPDA.Utils
{
    public class RenderEntry
    {
        public string Image { get; set; }

        public Pivot Start = new Pivot(), End = null;//, Anchor = new Pivot();
    }

    public class Vector2
    {
        public Vector2() { }
        public Vector2(int _x, int _y) { X = _x; Y = _y; }

        public int X = 0;
        public int Y = 0;

        public static Vector2 operator -(Vector2 a, Vector2 b) { return new Vector2(a.X - b.X, a.Y - b.Y);}
        public static Vector2 operator +(Vector2 a, Vector2 b) { return new Vector2(a.X + b.X, a.Y + b.Y); }
    }


    public class Vector2D
    {
        public Vector2D() { }
        public Vector2D(double _x, double _y) { X = _x; Y = _y; }

        public double X = 0;
        public double Y = 0;

        public static Vector2D operator -(Vector2D a, Vector2D b) { return new Vector2D(a.X - b.X, a.Y - b.Y); }
        public static Vector2D operator +(Vector2D a, Vector2D b) { return new Vector2D(a.X + b.X, a.Y + b.Y); }
    }

    public class Pivot : Vector2
    {
        public HorizontalPosition HorizontalAnchor = HorizontalPosition.Left;
        public VerticalPosition VerticalAnchor = VerticalPosition.Top;

        //public Vector2 GetPosition(int _x, int _y) { return GetPosition(new Vector2(_x, _y)); }

        public Vector2 GetPosition(Vector2 frame, Vector2 size)
        {
            var pos = new Vector2();

            switch (this.HorizontalAnchor)
            {
                case HorizontalPosition.Left:
                    pos.X = X;
                    break;
                /*case HorizontalPosition.Center:
                    pos.X = X + (frame.X/2);
                    break;*/
                case HorizontalPosition.Right:
                    pos.X = frame.X - X;
                    break;
            }

            switch (this.VerticalAnchor)
            {
                case VerticalPosition.Top:
                    pos.Y = Y;
                    break;
                /*case VerticalPosition.Center:
                    pos.Y = Y + (frame.Y/2);
                    break;*/
                case VerticalPosition.Bottom:
                    pos.Y = frame.Y - Y;
                    break;
            }

            return pos;
        }
    }

    public enum VerticalPosition
    {
        Top,
        //Center,
        Bottom
    }

    public enum HorizontalPosition
    {
        Left,
        //Center,
        Right
    }
}
