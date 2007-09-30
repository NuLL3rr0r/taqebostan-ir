namespace taqebostan
{
    partial class frmPw
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPw));
            this.txtPw = new System.Windows.Forms.TextBox();
            this.cboxClose = new System.Windows.Forms.Button();
            this.imglstCboxClose = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // txtPw
            // 
            this.txtPw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPw.Location = new System.Drawing.Point(23, 141);
            this.txtPw.Name = "txtPw";
            this.txtPw.Size = new System.Drawing.Size(387, 21);
            this.txtPw.TabIndex = 1;
            this.txtPw.UseSystemPasswordChar = true;
            this.txtPw.TextChanged += new System.EventHandler(this.txtPw_TextChanged);
            // 
            // cboxClose
            // 
            this.cboxClose.BackColor = System.Drawing.Color.Transparent;
            this.cboxClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cboxClose.FlatAppearance.BorderSize = 0;
            this.cboxClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cboxClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cboxClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxClose.ForeColor = System.Drawing.Color.Transparent;
            this.cboxClose.ImageIndex = 2;
            this.cboxClose.ImageList = this.imglstCboxClose;
            this.cboxClose.Location = new System.Drawing.Point(397, 7);
            this.cboxClose.Name = "cboxClose";
            this.cboxClose.Size = new System.Drawing.Size(27, 27);
            this.cboxClose.TabIndex = 0;
            this.cboxClose.TabStop = false;
            this.cboxClose.UseVisualStyleBackColor = false;
            this.cboxClose.MouseLeave += new System.EventHandler(this.cboxClose_MouseLeave);
            this.cboxClose.Click += new System.EventHandler(this.cboxClose_Click);
            this.cboxClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cboxClose_MouseDown);
            this.cboxClose.MouseEnter += new System.EventHandler(this.cboxClose_MouseEnter);
            this.cboxClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cboxClose_MouseUp);
            // 
            // imglstCboxClose
            // 
            this.imglstCboxClose.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstCboxClose.ImageStream")));
            this.imglstCboxClose.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstCboxClose.Images.SetKeyName(0, "cbox.close.down.png");
            this.imglstCboxClose.Images.SetKeyName(1, "cbox.close.over.png");
            this.imglstCboxClose.Images.SetKeyName(2, "cbox.close.up.png");
            // 
            // frmPw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::taqebostan.Properties.Resources.skinpw;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(434, 346);
            this.ControlBox = false;
            this.Controls.Add(this.cboxClose);
            this.Controls.Add(this.txtPw);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPw";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ﬂ·„Â Ì ⁄»Ê—";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Shown += new System.EventHandler(this.frmPw_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPw_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPw;
        private System.Windows.Forms.ImageList imglstCboxClose;
        private System.Windows.Forms.Button cboxClose;
    }
}