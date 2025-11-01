namespace STALKERPDA
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.customButton2 = new STALKERPDA.Controls.CustomButton();
            this.mapView1 = new STALKERPDA.Controls.MapView();
            this.gpsIcon = new STALKERPDA.Controls.EnumVisualizer();
            this.batteryIcon = new STALKERPDA.Controls.EnumVisualizer();
            this.timeLabel = new STALKERPDA.Controls.CustomLabel();
            this.tabSelector1 = new STALKERPDA.Controls.TabSelector();
            this.SuspendLayout();
            // 
            // customButton2
            // 
            this.customButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.customButton2.DefaultImage = "STALKERPDA.Images.Ui.Buttons.ButtonPower.png";
            this.customButton2.Location = new System.Drawing.Point(600, 3);
            this.customButton2.Name = "customButton2";
            this.customButton2.PressedImage = "STALKERPDA.Images.Ui.Buttons.ButtonPower_Pressed.png";
            this.customButton2.Size = new System.Drawing.Size(37, 50);
            this.customButton2.TabIndex = 1;
            this.customButton2.Click += new System.EventHandler(this.customButton2_Click);
            // 
            // mapView1
            // 
            this.mapView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mapView1.Location = new System.Drawing.Point(7, 99);
            this.mapView1.Name = "mapView1";
            this.mapView1.Size = new System.Drawing.Size(622, 369);
            this.mapView1.TabIndex = 2;
            // 
            // gpsIcon
            // 
            this.gpsIcon.Location = new System.Drawing.Point(520, 12);
            this.gpsIcon.Name = "gpsIcon";
            this.gpsIcon.Size = new System.Drawing.Size(16, 16);
            this.gpsIcon.TabIndex = 5;
            // 
            // batteryIcon
            // 
            this.batteryIcon.Location = new System.Drawing.Point(483, 12);
            this.batteryIcon.Name = "batteryIcon";
            this.batteryIcon.Size = new System.Drawing.Size(32, 16);
            this.batteryIcon.TabIndex = 6;
            // 
            // timeLabel
            // 
            this.timeLabel.Font = new System.Drawing.Font("Tahoma", 5F, System.Drawing.FontStyle.Bold);
            this.timeLabel.ForeColor = System.Drawing.Color.Silver;
            this.timeLabel.Location = new System.Drawing.Point(542, 10);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(44, 20);
            this.timeLabel.TabIndex = 7;
            this.timeLabel.Value = "00:00";
            // 
            // tabSelector1
            // 
            this.tabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSelector1.Font = new System.Drawing.Font("Courier New", 6F, System.Drawing.FontStyle.Regular);
            this.tabSelector1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.tabSelector1.ItemOffset = 23;
            this.tabSelector1.Location = new System.Drawing.Point(6, 6);
            this.tabSelector1.MidWidthClosed = 27;
            this.tabSelector1.MidWidthOpen = 126;
            this.tabSelector1.Name = "tabSelector1";
            this.tabSelector1.Size = new System.Drawing.Size(471, 27);
            this.tabSelector1.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.tabSelector1);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.batteryIcon);
            this.Controls.Add(this.gpsIcon);
            this.Controls.Add(this.mapView1);
            this.Controls.Add(this.customButton2);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private STALKERPDA.Controls.CustomButton customButton2;
        private STALKERPDA.Controls.MapView mapView1;
        private STALKERPDA.Controls.EnumVisualizer gpsIcon;
        private STALKERPDA.Controls.EnumVisualizer batteryIcon;
        private STALKERPDA.Controls.CustomLabel timeLabel;
        private STALKERPDA.Controls.TabSelector tabSelector1;



    }
}

