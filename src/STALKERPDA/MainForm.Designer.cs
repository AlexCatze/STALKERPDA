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
            this.customButton1 = new STALKERPDA.Controls.CustomButton();
            this.customButton2 = new STALKERPDA.Controls.CustomButton();
            this.SuspendLayout();
            // 
            // customButton1
            // 
            this.customButton1.Caption = "Мапа";
            this.customButton1.DefaultImage = "STALKERPDA.Images.Ui.Buttons.Button1.png";
            this.customButton1.Font = new System.Drawing.Font("Courier New", 6F, System.Drawing.FontStyle.Regular);
            this.customButton1.ForeColor = System.Drawing.Color.White;
            this.customButton1.IsPressed = true;
            this.customButton1.Location = new System.Drawing.Point(6, 6);
            this.customButton1.Name = "customButton1";
            this.customButton1.PressedImage = "STALKERPDA.Images.Ui.Buttons.Button1_Pressed.png";
            this.customButton1.Size = new System.Drawing.Size(172, 27);
            this.customButton1.TabIndex = 0;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.customButton2);
            this.Controls.Add(this.customButton1);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private STALKERPDA.Controls.CustomButton customButton1;
        private STALKERPDA.Controls.CustomButton customButton2;



    }
}

