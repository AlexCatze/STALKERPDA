using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STALKERPDA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            var orient = Microsoft.WindowsCE.Forms.SystemSettings.ScreenOrientation;

            Microsoft.WindowsCE.Forms.SystemSettings.ScreenOrientation = Microsoft.WindowsCE.Forms.ScreenOrientation.Angle270;

            Application.Run(new MainForm());

            Microsoft.WindowsCE.Forms.SystemSettings.ScreenOrientation = orient;
        }
    }
}