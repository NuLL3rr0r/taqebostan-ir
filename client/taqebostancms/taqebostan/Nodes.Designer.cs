namespace taqebostan
{
    partial class frmNodes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNodes));
            this.imglstCboxClose = new System.Windows.Forms.ImageList(this.components);
            this.cboxClose = new System.Windows.Forms.Button();
            this.trvPages = new System.Windows.Forms.TreeView();
            this.ctxNodeManager = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mItemInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.mItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.mItemErase = new System.Windows.Forms.ToolStripMenuItem();
            this.miNodeManagerSpacer01 = new System.Windows.Forms.ToolStripSeparator();
            this.mItemMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.mItemMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.miNodeManagerSpacer02 = new System.Windows.Forms.ToolStripSeparator();
            this.mItemCopyURL = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxNodeManager.SuspendLayout();
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
            this.cboxClose.TabIndex = 2;
            this.cboxClose.TabStop = false;
            this.cboxClose.UseVisualStyleBackColor = false;
            this.cboxClose.MouseLeave += new System.EventHandler(this.cboxClose_MouseLeave);
            this.cboxClose.Click += new System.EventHandler(this.cboxClose_Click);
            this.cboxClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cboxClose_MouseDown);
            this.cboxClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cboxClose_MouseUp);
            this.cboxClose.MouseEnter += new System.EventHandler(this.cboxClose_MouseEnter);
            // 
            // trvPages
            // 
            this.trvPages.ContextMenuStrip = this.ctxNodeManager;
            this.trvPages.Location = new System.Drawing.Point(12, 84);
            this.trvPages.Name = "trvPages";
            this.trvPages.Size = new System.Drawing.Size(410, 250);
            this.trvPages.TabIndex = 3;
            this.trvPages.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvPages_NodeMouseClick);
            // 
            // ctxNodeManager
            // 
            this.ctxNodeManager.Font = new System.Drawing.Font("B Traffic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.ctxNodeManager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mItemInsert,
            this.mItemRename,
            this.mItemErase,
            this.miNodeManagerSpacer01,
            this.mItemMoveUp,
            this.mItemMoveDown,
            this.miNodeManagerSpacer02,
            this.mItemCopyURL});
            this.ctxNodeManager.Name = "ctxNodeManager";
            this.ctxNodeManager.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ctxNodeManager.Size = new System.Drawing.Size(177, 194);
            this.ctxNodeManager.Opening += new System.ComponentModel.CancelEventHandler(this.ctxNodeManager_Opening);
            // 
            // mItemInsert
            // 
            this.mItemInsert.Name = "mItemInsert";
            this.mItemInsert.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.mItemInsert.Size = new System.Drawing.Size(176, 26);
            this.mItemInsert.Text = "درج";
            this.mItemInsert.Click += new System.EventHandler(this.mItemInsert_Click);
            // 
            // mItemRename
            // 
            this.mItemRename.Name = "mItemRename";
            this.mItemRename.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Insert)));
            this.mItemRename.Size = new System.Drawing.Size(176, 26);
            this.mItemRename.Text = "ويرايش";
            this.mItemRename.Click += new System.EventHandler(this.mItemRename_Click);
            // 
            // mItemErase
            // 
            this.mItemErase.Name = "mItemErase";
            this.mItemErase.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mItemErase.Size = new System.Drawing.Size(176, 26);
            this.mItemErase.Text = "حذف";
            this.mItemErase.Click += new System.EventHandler(this.mItemErase_Click);
            // 
            // miNodeManagerSpacer01
            // 
            this.miNodeManagerSpacer01.Name = "miNodeManagerSpacer01";
            this.miNodeManagerSpacer01.Size = new System.Drawing.Size(173, 6);
            // 
            // mItemMoveUp
            // 
            this.mItemMoveUp.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.mItemMoveUp.Name = "mItemMoveUp";
            this.mItemMoveUp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.mItemMoveUp.Size = new System.Drawing.Size(176, 26);
            this.mItemMoveUp.Text = "▲";
            this.mItemMoveUp.Click += new System.EventHandler(this.mItemMoveUp_Click);
            // 
            // mItemMoveDown
            // 
            this.mItemMoveDown.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mItemMoveDown.Name = "mItemMoveDown";
            this.mItemMoveDown.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.mItemMoveDown.Size = new System.Drawing.Size(176, 26);
            this.mItemMoveDown.Text = "▼";
            this.mItemMoveDown.Click += new System.EventHandler(this.mItemMoveDown_Click);
            // 
            // miNodeManagerSpacer02
            // 
            this.miNodeManagerSpacer02.Name = "miNodeManagerSpacer02";
            this.miNodeManagerSpacer02.Size = new System.Drawing.Size(173, 6);
            // 
            // mItemCopyURL
            // 
            this.mItemCopyURL.Name = "mItemCopyURL";
            this.mItemCopyURL.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mItemCopyURL.Size = new System.Drawing.Size(176, 26);
            this.mItemCopyURL.Text = "کپی آدرس";
            this.mItemCopyURL.Click += new System.EventHandler(this.mItemCopyURL_Click);
            // 
            // frmNodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(434, 346);
            this.ControlBox = false;
            this.Controls.Add(this.trvPages);
            this.Controls.Add(this.cboxClose);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmNodes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "مدیر صفحات";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Load += new System.EventHandler(this.frmNodes_Load);
            this.Shown += new System.EventHandler(this.frmNodes_Shown);
            this.ctxNodeManager.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imglstCboxClose;
        private System.Windows.Forms.Button cboxClose;
        private System.Windows.Forms.TreeView trvPages;
        private System.Windows.Forms.ContextMenuStrip ctxNodeManager;
        private System.Windows.Forms.ToolStripMenuItem mItemInsert;
        private System.Windows.Forms.ToolStripMenuItem mItemRename;
        private System.Windows.Forms.ToolStripMenuItem mItemErase;
        private System.Windows.Forms.ToolStripSeparator miNodeManagerSpacer01;
        private System.Windows.Forms.ToolStripMenuItem mItemMoveUp;
        private System.Windows.Forms.ToolStripMenuItem mItemMoveDown;
        private System.Windows.Forms.ToolStripSeparator miNodeManagerSpacer02;
        private System.Windows.Forms.ToolStripMenuItem mItemCopyURL;
    }
}