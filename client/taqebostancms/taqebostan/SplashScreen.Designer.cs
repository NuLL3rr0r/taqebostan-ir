namespace taqebostan
{
    partial class frmSplashScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplashScreen));
            this.pctLoading = new System.Windows.Forms.PictureBox();
            this.tmrLoading = new System.Windows.Forms.Timer(this.components);
            this.lblLoading = new System.Windows.Forms.Label();
            this.tmrDoExit = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pctLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // pctLoading
            // 
            this.pctLoading.BackColor = System.Drawing.Color.Transparent;
            this.pctLoading.Image = global::taqebostan.Properties.Resources.ajaxloadersplash;
            this.pctLoading.Location = new System.Drawing.Point(529, 12);
            this.pctLoading.Name = "pctLoading";
            this.pctLoading.Size = new System.Drawing.Size(31, 31);
            this.pctLoading.TabIndex = 0;
            this.pctLoading.TabStop = false;
            // 
            // tmrLoading
            // 
            this.tmrLoading.Interval = 200;
            this.tmrLoading.Tick += new System.EventHandler(this.tmrLoading_Tick);
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.BackColor = System.Drawing.Color.Transparent;
            this.lblLoading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblLoading.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblLoading.Location = new System.Drawing.Point(428, 12);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(0, 13);
            this.lblLoading.TabIndex = 1;
            // 
            // tmrDoExit
            // 
            this.tmrDoExit.Interval = 3113;
            this.tmrDoExit.Tick += new System.EventHandler(this.tmrDoExit_Tick);
            // 
            // frmSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::taqebostan.Properties.Resources.splashscreen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(572, 270);
            this.ControlBox = false;
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.pctLoading);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSplashScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash Screen";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.frmSplashScreen_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSplashScreen_FormClosing);
            this.Load += new System.EventHandler(this.frmSplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pctLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctLoading;
        private System.Windows.Forms.Timer tmrLoading;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.Timer tmrDoExit;

    }
}

