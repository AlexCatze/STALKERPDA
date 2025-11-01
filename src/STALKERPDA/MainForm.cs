using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Drawing.Imaging;
using STALKERPDA.Utils;
using Microsoft.WindowsMobile.Status;

namespace STALKERPDA
{
    public partial class MainForm : DoubleBufferedForm
    {
        private const int BORDER_MARGIN = 2;

        private const int FIRST_ROW_HEIHGT = 19, SECOND_ROW_Y = BORDER_MARGIN + FIRST_ROW_HEIHGT;
        private const int SECOND_ROW_HEIGHT = 40, THIRD_ROW_Y = SECOND_ROW_Y + SECOND_ROW_HEIGHT;
        private const int THIRD_ROW_HEIGHT = 11, FOURTH_ROW_Y = THIRD_ROW_Y + THIRD_ROW_HEIGHT;

        private const int SIDE_WIDTH = 20, SIDE_X = BORDER_MARGIN + SIDE_WIDTH;


        private const int MAP_FIRST_ROW_Y = 66 + 28;
        private const int MAP_FIRST_ROW_HEIHGT = 13, MAP_SECOND_ROW_Y = MAP_FIRST_ROW_Y + MAP_FIRST_ROW_HEIHGT;
        private const int MAP_THIRD_ROW_HEIGHT = 16, MAP_THIRD_ROW_Y = BORDER_MARGIN + MAP_THIRD_ROW_HEIGHT;
        //private const int MAP_SECOND_ROW_HEIGHT = 40, THIRD_ROW_Y = MAP_SECOND_ROW_Y + MAP_SECOND_ROW_HEIGHT;
       
        private const int MAP_LEFT_SIDE_WIDTH = 16 + BORDER_MARGIN;
        private const int MAP_RIGHT_SIDE_WIDTH = 19 + BORDER_MARGIN;

        public MainForm()
        {
            m_factory = new ImagingFactoryClass();
            InitializeComponent();

            gpsIcon.Images = new List<string> { 
                "STALKERPDA.Images.Ui.Statusbar.Gps.Ok.png",
                "STALKERPDA.Images.Ui.Statusbar.Gps.Warning.png",
                "STALKERPDA.Images.Ui.Statusbar.Gps.Bad.png",
                "STALKERPDA.Images.Ui.Statusbar.Gps.Error.png"};

            batteryIcon.Images = new List<string>
            {
                "STALKERPDA.Images.Ui.Statusbar.Battery.1.png",
                "STALKERPDA.Images.Ui.Statusbar.Battery.2.png",
                "STALKERPDA.Images.Ui.Statusbar.Battery.3.png",
                "STALKERPDA.Images.Ui.Statusbar.Battery.4.png",
            };

            tabSelector1.TabNames = new List<string>() { "Мапа", "Чат","Мережа","Музика", "Налаштування" };
            tabSelector1.TabIcons = new List<string>() { 
                "STALKERPDA.Images.Ui.Taskbar.Map.png",
                "STALKERPDA.Images.Ui.Taskbar.Chat.png",
                "STALKERPDA.Images.Ui.Taskbar.Network.png",
                "STALKERPDA.Images.Ui.Taskbar.Music.png",
                "STALKERPDA.Images.Ui.Taskbar.Settings.png",
            };
            tabSelector1.Invalidate();

            var timer = new Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += (a,b) => {
                gpsIcon.Value =(int) mapView1.GetGpsState();

                timeLabel.Value = DateTime.Now.ToString("HH:mm");

                switch (SystemState.PowerBatteryStrength)
                {
                    case BatteryLevel.VeryHigh:
                        batteryIcon.Value = 3;
                        break;
                    case BatteryLevel.High:
                        batteryIcon.Value = 2;
                        break;
                    case BatteryLevel.Medium:
                        batteryIcon.Value = 1;
                        break;
                    case BatteryLevel.VeryLow:
                    case BatteryLevel.Low:
                        batteryIcon.Value = 0;
                        break;

                }
            };
        }

        protected override void SetupBackground()
        {
            var lst = new List<RenderEntry> { 
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Background.png" },//TODO: Change render strategy to support other resolutions
                
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.LeftTop.png", Start = new Pivot{X = BORDER_MARGIN, Y = BORDER_MARGIN} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.Top.png", Start = new Pivot{X = SIDE_X, Y = BORDER_MARGIN}, End = new Pivot{X = SIDE_X, Y = SECOND_ROW_Y, HorizontalAnchor = HorizontalPosition.Right} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.RightTop.png", Start = new Pivot{X = SIDE_X, Y = BORDER_MARGIN, HorizontalAnchor = HorizontalPosition.Right}},
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.Left.png", Start = new Pivot{X = BORDER_MARGIN, Y = SECOND_ROW_Y}, End = new Pivot{X = SIDE_X, Y = THIRD_ROW_Y} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.Back.png", Start = new Pivot{X = SIDE_X, Y = SECOND_ROW_Y}, End = new Pivot{X = SIDE_X, Y = THIRD_ROW_Y, HorizontalAnchor = HorizontalPosition.Right} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.Right.png", Start = new Pivot{X = SIDE_X, Y = SECOND_ROW_Y, HorizontalAnchor = HorizontalPosition.Right}, End = new Pivot{X = BORDER_MARGIN, Y = THIRD_ROW_Y, HorizontalAnchor = HorizontalPosition.Right}},
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.LeftBottom.png", Start = new Pivot{X = BORDER_MARGIN, Y = THIRD_ROW_Y} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.Bottom.png", Start = new Pivot{X = SIDE_X, Y = THIRD_ROW_Y}, End = new Pivot{X = SIDE_X, Y = FOURTH_ROW_Y, HorizontalAnchor = HorizontalPosition.Right} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Head.RightBottom.png", Start = new Pivot{X = SIDE_X, Y = THIRD_ROW_Y, HorizontalAnchor = HorizontalPosition.Right}},

                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Mid.Left.png", Start = new Pivot{X = BORDER_MARGIN, Y = 66} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Mid.Right.png", Start = new Pivot{X = BORDER_MARGIN + 41, Y = 65, HorizontalAnchor = HorizontalPosition.Right} },

                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.LeftTop.png", Start = new Pivot{X = BORDER_MARGIN, Y = MAP_FIRST_ROW_Y} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.Top.png", Start = new Pivot{X = MAP_LEFT_SIDE_WIDTH, Y = MAP_FIRST_ROW_Y}, End = new Pivot{X = MAP_RIGHT_SIDE_WIDTH, Y = MAP_SECOND_ROW_Y, HorizontalAnchor = HorizontalPosition.Right} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.RightTop.png", Start = new Pivot{X = MAP_RIGHT_SIDE_WIDTH, Y = MAP_FIRST_ROW_Y, HorizontalAnchor = HorizontalPosition.Right}},
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.Left.png", Start = new Pivot{X = BORDER_MARGIN, Y = MAP_SECOND_ROW_Y}, End = new Pivot{X = MAP_LEFT_SIDE_WIDTH, Y = MAP_THIRD_ROW_Y, VerticalAnchor = VerticalPosition.Bottom} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.Back.png", Start = new Pivot{X = MAP_LEFT_SIDE_WIDTH, Y = MAP_SECOND_ROW_Y}, End = new Pivot{X = MAP_RIGHT_SIDE_WIDTH, Y = MAP_THIRD_ROW_Y, HorizontalAnchor = HorizontalPosition.Right, VerticalAnchor = VerticalPosition.Bottom} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.Right.png", Start = new Pivot{X = MAP_RIGHT_SIDE_WIDTH, Y = MAP_SECOND_ROW_Y, HorizontalAnchor = HorizontalPosition.Right}, End = new Pivot{X = BORDER_MARGIN, Y = MAP_THIRD_ROW_Y, HorizontalAnchor = HorizontalPosition.Right, VerticalAnchor = VerticalPosition.Bottom}},
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.LeftBottom.png", Start = new Pivot{X = BORDER_MARGIN, Y = MAP_THIRD_ROW_Y, VerticalAnchor = VerticalPosition.Bottom} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.Bottom.png", Start = new Pivot{X = MAP_LEFT_SIDE_WIDTH, Y = MAP_THIRD_ROW_Y, VerticalAnchor = VerticalPosition.Bottom}, End = new Pivot{X = MAP_RIGHT_SIDE_WIDTH, Y = BORDER_MARGIN, HorizontalAnchor = HorizontalPosition.Right, VerticalAnchor = VerticalPosition.Bottom} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Background.Map.RightBottom.png", Start = new Pivot{X = MAP_RIGHT_SIDE_WIDTH, Y = MAP_THIRD_ROW_Y, HorizontalAnchor = HorizontalPosition.Right, VerticalAnchor = VerticalPosition.Bottom}},

                new RenderEntry { Image = "STALKERPDA.Images.Ui.Statusbar.Frame.Left.png", Start = new Pivot{X = 184, Y = 6, HorizontalAnchor = HorizontalPosition.Right} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Statusbar.Frame.Mid.png", Start = new Pivot{X = 157, Y = 6, HorizontalAnchor = HorizontalPosition.Right}, End = new Pivot{X = 54, Y = 6 + 27, HorizontalAnchor = HorizontalPosition.Right} },
                new RenderEntry { Image = "STALKERPDA.Images.Ui.Statusbar.Frame.Right.png", Start = new Pivot{X = 54, Y = 6, HorizontalAnchor = HorizontalPosition.Right} },
            };

            IntPtr hdc = m_gBuffer.GetHdc();

            var frame = new Vector2(this.Width, this.Height);

            foreach (var i in lst)
            {
                StreamOnFile sof = new StreamOnFile(GetType().Assembly.GetManifestResourceStream(i.Image));
                IImage imgBlank;
                m_factory.CreateImageFromStream(sof, out imgBlank);
                ImageInfo ii;
                imgBlank.GetImageInfo(out ii);
                var imgsize = new Vector2((int)ii.Width, (int)ii.Height);

                Vector2 start = i.Start.GetPosition(frame, imgsize);
                Vector2 end;
                if (i.End != null)
                {
                    end = i.End.GetPosition(frame, imgsize);
                }
                else
                {
                    end = start + imgsize;
                }

                imgBlank.Draw(hdc, new RECT(start.X, start.Y, end.X, end.Y), null);
            }


            m_gBuffer.ReleaseHdc(hdc);
        }

        private void customButton2_Click(object sender, EventArgs e)
        {
            Close();   
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            mapView1.ProcessKeyCode(e.KeyCode);
        }
    }
}