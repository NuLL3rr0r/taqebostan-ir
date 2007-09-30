namespace taqebostan
{
    partial class frmContactList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmContactList));
            this.imglstCboxClose = new System.Windows.Forms.ImageList(this.components);
            this.cboxClose = new System.Windows.Forms.Button();
            this.dgvMailBox = new System.Windows.Forms.DataGridView();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMailBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imglstCboxClose
            // 
            this.imglstCboxClose.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstCboxClose.ImageStream")));
            this.imglstCboxClose.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstCboxClose.Images.SetKeyName(0, "cbox.close.down.png");
            this.imglstCboxClose.Images.SetKeyName(1, "cbox.close.over.png");
            this.imglstCboxClose.Images.SetKeyName(2, "cbox.close.up.png");
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
            this.cboxClose.TabIndex = 1;
            this.cboxClose.TabStop = false;
            this.cboxClose.UseVisualStyleBackColor = false;
            this.cboxClose.MouseLeave += new System.EventHandler(this.cboxClose_MouseLeave);
            this.cboxClose.Click += new System.EventHandler(this.cboxClose_Click);
            this.cboxClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cboxClose_MouseDown);
            this.cboxClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cboxClose_MouseUp);
            this.cboxClose.MouseEnter += new System.EventHandler(this.cboxClose_MouseEnter);
            // 
            // dgvMailBox
            // 
            this.dgvMailBox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMailBox.Location = new System.Drawing.Point(12, 84);
            this.dgvMailBox.Name = "dgvMailBox";
            this.dgvMailBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvMailBox.Size = new System.Drawing.Size(410, 221);
            this.dgvMailBox.TabIndex = 2;
            this.dgvMailBox.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMailBox_CellValueChanged);
            this.dgvMailBox.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvMailBox_RowsRemoved);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(12, 311);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(75, 23);
            this.btnReturn.TabIndex = 3;
            this.btnReturn.Text = "»«“ê‘ ";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(93, 311);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "–ŒÌ—Â";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmContactList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::taqebostan.Properties.Resources.skin_contactlist;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(434, 346);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.dgvMailBox);
            this.Controls.Add(this.cboxClose);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmContactList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "·Ì”   „«”";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Load += new System.EventHandler(this.frmContactList_Load);
            this.Shown += new System.EventHandler(this.frmContactList_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmContactList_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMailBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imglstCboxClose;
        private System.Windows.Forms.Button cboxClose;
        private System.Windows.Forms.DataGridView dgvMailBox;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnSave;
    }
}