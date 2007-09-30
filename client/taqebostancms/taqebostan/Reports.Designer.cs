namespace taqebostan
{
    partial class frmReports
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReports));
            this.PageRanksBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportsDataSet = new taqebostan.reportsDataSet();
            this.pctTopRight = new System.Windows.Forms.PictureBox();
            this.pctBottomLeft = new System.Windows.Forms.PictureBox();
            this.pctBottomRight = new System.Windows.Forms.PictureBox();
            this.pctBottomRepeat = new System.Windows.Forms.PictureBox();
            this.pctLeftRepeat = new System.Windows.Forms.PictureBox();
            this.pctTopRepeat = new System.Windows.Forms.PictureBox();
            this.pctRightRepeat = new System.Windows.Forms.PictureBox();
            this.pctTopLeft = new System.Windows.Forms.PictureBox();
            this.pnlReportContainer = new System.Windows.Forms.Panel();
            this.rptPagesRank = new Microsoft.Reporting.WinForms.ReportViewer();
            this.PageRanksTableAdapter = new taqebostan.reportsDataSetTableAdapters.PageRanksTableAdapter();
            this.cboxClose = new System.Windows.Forms.Button();
            this.imglstCboxClose = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PageRanksBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTopRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBottomLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBottomRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBottomRepeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctLeftRepeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTopRepeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctRightRepeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTopLeft)).BeginInit();
            this.pnlReportContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PageRanksBindingSource
            // 
            this.PageRanksBindingSource.DataMember = "PageRanks";
            this.PageRanksBindingSource.DataSource = this.reportsDataSet;
            // 
            // reportsDataSet
            // 
            this.reportsDataSet.DataSetName = "reportsDataSet";
            this.reportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pctTopRight
            // 
            this.pctTopRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pctTopRight.BackColor = System.Drawing.Color.Transparent;
            this.pctTopRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pctTopRight.Image = global::taqebostan.Properties.Resources.reports_top_right;
            this.pctTopRight.Location = new System.Drawing.Point(597, 0);
            this.pctTopRight.Name = "pctTopRight";
            this.pctTopRight.Size = new System.Drawing.Size(43, 65);
            this.pctTopRight.TabIndex = 1;
            this.pctTopRight.TabStop = false;
            // 
            // pctBottomLeft
            // 
            this.pctBottomLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pctBottomLeft.BackColor = System.Drawing.Color.Transparent;
            this.pctBottomLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pctBottomLeft.Image = global::taqebostan.Properties.Resources.reports_bottom_left;
            this.pctBottomLeft.Location = new System.Drawing.Point(0, 447);
            this.pctBottomLeft.Name = "pctBottomLeft";
            this.pctBottomLeft.Size = new System.Drawing.Size(33, 33);
            this.pctBottomLeft.TabIndex = 2;
            this.pctBottomLeft.TabStop = false;
            // 
            // pctBottomRight
            // 
            this.pctBottomRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pctBottomRight.BackColor = System.Drawing.Color.Transparent;
            this.pctBottomRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pctBottomRight.Image = global::taqebostan.Properties.Resources.reports_bottom_right;
            this.pctBottomRight.Location = new System.Drawing.Point(607, 447);
            this.pctBottomRight.Name = "pctBottomRight";
            this.pctBottomRight.Size = new System.Drawing.Size(33, 33);
            this.pctBottomRight.TabIndex = 3;
            this.pctBottomRight.TabStop = false;
            // 
            // pctBottomRepeat
            // 
            this.pctBottomRepeat.BackColor = System.Drawing.Color.Transparent;
            this.pctBottomRepeat.BackgroundImage = global::taqebostan.Properties.Resources.reports_bottom_repeat;
            this.pctBottomRepeat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pctBottomRepeat.Location = new System.Drawing.Point(33, 447);
            this.pctBottomRepeat.Name = "pctBottomRepeat";
            this.pctBottomRepeat.Size = new System.Drawing.Size(574, 33);
            this.pctBottomRepeat.TabIndex = 4;
            this.pctBottomRepeat.TabStop = false;
            // 
            // pctLeftRepeat
            // 
            this.pctLeftRepeat.BackColor = System.Drawing.Color.Transparent;
            this.pctLeftRepeat.BackgroundImage = global::taqebostan.Properties.Resources.reports_left_repeat;
            this.pctLeftRepeat.Dock = System.Windows.Forms.DockStyle.Left;
            this.pctLeftRepeat.Location = new System.Drawing.Point(0, 0);
            this.pctLeftRepeat.Name = "pctLeftRepeat";
            this.pctLeftRepeat.Size = new System.Drawing.Size(33, 480);
            this.pctLeftRepeat.TabIndex = 4;
            this.pctLeftRepeat.TabStop = false;
            // 
            // pctTopRepeat
            // 
            this.pctTopRepeat.BackColor = System.Drawing.Color.Transparent;
            this.pctTopRepeat.BackgroundImage = global::taqebostan.Properties.Resources.reports_top_repeat;
            this.pctTopRepeat.Dock = System.Windows.Forms.DockStyle.Top;
            this.pctTopRepeat.Location = new System.Drawing.Point(33, 0);
            this.pctTopRepeat.Name = "pctTopRepeat";
            this.pctTopRepeat.Size = new System.Drawing.Size(574, 65);
            this.pctTopRepeat.TabIndex = 4;
            this.pctTopRepeat.TabStop = false;
            // 
            // pctRightRepeat
            // 
            this.pctRightRepeat.BackColor = System.Drawing.Color.Transparent;
            this.pctRightRepeat.BackgroundImage = global::taqebostan.Properties.Resources.reports_right_repeat;
            this.pctRightRepeat.Dock = System.Windows.Forms.DockStyle.Right;
            this.pctRightRepeat.Location = new System.Drawing.Point(607, 0);
            this.pctRightRepeat.Name = "pctRightRepeat";
            this.pctRightRepeat.Size = new System.Drawing.Size(33, 480);
            this.pctRightRepeat.TabIndex = 4;
            this.pctRightRepeat.TabStop = false;
            // 
            // pctTopLeft
            // 
            this.pctTopLeft.BackColor = System.Drawing.Color.Transparent;
            this.pctTopLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pctTopLeft.Image = global::taqebostan.Properties.Resources.reports_top_left;
            this.pctTopLeft.Location = new System.Drawing.Point(0, 0);
            this.pctTopLeft.Name = "pctTopLeft";
            this.pctTopLeft.Size = new System.Drawing.Size(328, 98);
            this.pctTopLeft.TabIndex = 0;
            this.pctTopLeft.TabStop = false;
            // 
            // pnlReportContainer
            // 
            this.pnlReportContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlReportContainer.BackColor = System.Drawing.Color.Transparent;
            this.pnlReportContainer.Controls.Add(this.rptPagesRank);
            this.pnlReportContainer.Location = new System.Drawing.Point(33, 87);
            this.pnlReportContainer.Name = "pnlReportContainer";
            this.pnlReportContainer.Size = new System.Drawing.Size(574, 360);
            this.pnlReportContainer.TabIndex = 5;
            // 
            // rptPagesRank
            // 
            this.rptPagesRank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptPagesRank.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            reportDataSource2.Name = "reportsDataSet_PageRanks";
            reportDataSource2.Value = this.PageRanksBindingSource;
            this.rptPagesRank.LocalReport.DataSources.Add(reportDataSource2);
            this.rptPagesRank.LocalReport.ReportEmbeddedResource = "taqebostan.ReportViewCount.rdlc";
            this.rptPagesRank.Location = new System.Drawing.Point(0, 0);
            this.rptPagesRank.Name = "rptPagesRank";
            this.rptPagesRank.Size = new System.Drawing.Size(574, 360);
            this.rptPagesRank.TabIndex = 0;
            // 
            // PageRanksTableAdapter
            // 
            this.PageRanksTableAdapter.ClearBeforeFill = true;
            // 
            // cboxClose
            // 
            this.cboxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(116)))), ((int)(((byte)(234)))));
            this.cboxClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cboxClose.FlatAppearance.BorderSize = 0;
            this.cboxClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cboxClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cboxClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxClose.ForeColor = System.Drawing.Color.Transparent;
            this.cboxClose.ImageIndex = 2;
            this.cboxClose.ImageList = this.imglstCboxClose;
            this.cboxClose.Location = new System.Drawing.Point(602, 8);
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
            // frmReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::taqebostan.Properties.Resources.reports_back;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.ControlBox = false;
            this.Controls.Add(this.cboxClose);
            this.Controls.Add(this.pnlReportContainer);
            this.Controls.Add(this.pctTopLeft);
            this.Controls.Add(this.pctTopRight);
            this.Controls.Add(this.pctBottomLeft);
            this.Controls.Add(this.pctBottomRight);
            this.Controls.Add(this.pctTopRepeat);
            this.Controls.Add(this.pctBottomRepeat);
            this.Controls.Add(this.pctRightRepeat);
            this.Controls.Add(this.pctLeftRepeat);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReports";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reports";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.frmReports_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.PageRanksBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTopRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBottomLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBottomRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBottomRepeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctLeftRepeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTopRepeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctRightRepeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTopLeft)).EndInit();
            this.pnlReportContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pctTopRight;
        private System.Windows.Forms.PictureBox pctBottomLeft;
        private System.Windows.Forms.PictureBox pctBottomRight;
        private System.Windows.Forms.PictureBox pctBottomRepeat;
        private System.Windows.Forms.PictureBox pctLeftRepeat;
        private System.Windows.Forms.PictureBox pctTopRepeat;
        private System.Windows.Forms.PictureBox pctRightRepeat;
        private System.Windows.Forms.PictureBox pctTopLeft;
        private System.Windows.Forms.Panel pnlReportContainer;
        private Microsoft.Reporting.WinForms.ReportViewer rptPagesRank;
        private System.Windows.Forms.BindingSource PageRanksBindingSource;
        private reportsDataSet reportsDataSet;
        private taqebostan.reportsDataSetTableAdapters.PageRanksTableAdapter PageRanksTableAdapter;
        private System.Windows.Forms.Button cboxClose;
        private System.Windows.Forms.ImageList imglstCboxClose;
    }
}