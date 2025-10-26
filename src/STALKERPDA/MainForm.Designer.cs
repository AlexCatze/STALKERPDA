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
            this.mapButton = new STALKERPDA.Controls.CustomButton();
            this.customButton2 = new STALKERPDA.Controls.CustomButton();
            this.mapView1 = new STALKERPDA.Controls.MapView();
            this.chatButton = new STALKERPDA.Controls.CustomButton();
            this.settingsButton = new STALKERPDA.Controls.CustomButton();
            this.gpsIcon = new STALKERPDA.Controls.EnumVisualizer();
            this.batteryIcon = new STALKERPDA.Controls.EnumVisualizer();
            this.SuspendLayout();
            // 
            // mapButton
            // 
            this.mapButton.Caption = "Мапа";
            this.mapButton.DefaultImage = "STALKERPDA.Images.Ui.Buttons.Button1.png";
            this.mapButton.Font = new System.Drawing.Font("Courier New", 6F, System.Drawing.FontStyle.Regular);
            this.mapButton.ForeColor = System.Drawing.Color.White;
            this.mapButton.IsPressed = true;
            this.mapButton.Location = new System.Drawing.Point(6, 6);
            this.mapButton.Name = "mapButton";
            this.mapButton.PressedImage = "STALKERPDA.Images.Ui.Buttons.Button1_Pressed.png";
            this.mapButton.Size = new System.Drawing.Size(172, 27);
            this.mapButton.TabIndex = 0;
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
            // chatButton
            // 
            this.chatButton.Caption = "Чат";
            this.chatButton.DefaultImage = "STALKERPDA.Images.Ui.Buttons.Button1.png";
            this.chatButton.Font = new System.Drawing.Font("Courier New", 6F, System.Drawing.FontStyle.Regular);
            this.chatButton.ForeColor = System.Drawing.Color.White;
            this.chatButton.IsPressed = true;
            this.chatButton.Location = new System.Drawing.Point(156, 6);
            this.chatButton.Name = "chatButton";
            this.chatButton.PressedImage = "STALKERPDA.Images.Ui.Buttons.Button1_Pressed.png";
            this.chatButton.Size = new System.Drawing.Size(172, 27);
            this.chatButton.TabIndex = 3;
            this.chatButton.Visible = false;
            // 
            // settingsButton
            // 
            this.settingsButton.Caption = "Мапа";
            this.settingsButton.DefaultImage = "STALKERPDA.Images.Ui.Buttons.Button1.png";
            this.settingsButton.Font = new System.Drawing.Font("Courier New", 6F, System.Drawing.FontStyle.Regular);
            this.settingsButton.ForeColor = System.Drawing.Color.White;
            this.settingsButton.IsPressed = true;
            this.settingsButton.Location = new System.Drawing.Point(306, 6);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.PressedImage = "STALKERPDA.Images.Ui.Buttons.Button1_Pressed.png";
            this.settingsButton.Size = new System.Drawing.Size(172, 27);
            this.settingsButton.TabIndex = 4;
            this.settingsButton.Visible = false;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.batteryIcon);
            this.Controls.Add(this.gpsIcon);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.chatButton);
            this.Controls.Add(this.mapView1);
            this.Controls.Add(this.customButton2);
            this.Controls.Add(this.mapButton);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private STALKERPDA.Controls.CustomButton mapButton;
        private STALKERPDA.Controls.CustomButton customButton2;
        private STALKERPDA.Controls.MapView mapView1;
        private STALKERPDA.Controls.CustomButton chatButton;
        private STALKERPDA.Controls.CustomButton settingsButton;
        private STALKERPDA.Controls.EnumVisualizer gpsIcon;
        private STALKERPDA.Controls.EnumVisualizer batteryIcon;



    }
}

