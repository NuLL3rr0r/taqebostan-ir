namespace taqebostan
{
    partial class frmLoading
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
            this.pctLoading = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pctLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // pctLoading
            // 
            this.pctLoading.BackColor = System.Drawing.Color.Transparent;
            this.pctLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pctLoading.Image = global::taqebostan.Properties.Resources.loading;
            this.pctLoading.Location = new System.Drawing.Point(331, 205);
            this.pctLoading.Name = "pctLoading";
            this.pctLoading.Size = new System.Drawing.Size(100, 100);
            this.pctLoading.TabIndex = 0;
            this.pctLoading.TabStop = false;
            // 
            // frmLoading
            // 
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::taqebostan.Properties.Resources.loadingback;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(762, 509);
            this.ControlBox = false;
            this.Controls.Add(this.pctLoading);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoading";
            this.Opacity = 0.75;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "loading...";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Shown += new System.EventHandler(this.frmLoading_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLoading_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pctLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pctLoading;
    }
}