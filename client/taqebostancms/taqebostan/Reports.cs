using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace taqebostan
{
    public partial class frmReports : Form
    {
        public frmReports()
        {
            InitializeComponent();
        }

        private void cboxClose_MouseEnter(object sender, EventArgs e)
        {
            cboxClose.ImageIndex = 1;
        }

        private void cboxClose_MouseDown(object sender, MouseEventArgs e)
        {
            cboxClose.ImageIndex = 0;
        }

        private void cboxClose_MouseLeave(object sender, EventArgs e)
        {
            cboxClose.ImageIndex = 2;
        }

        private void cboxClose_MouseUp(object sender, MouseEventArgs e)
        {
            cboxClose.ImageIndex = 1;
        }

        private void cboxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReports_Shown(object sender, EventArgs e)
        {
            this.Activate();

            rptPagesRank.Focus();
            this.PageRanksTableAdapter.Fill(this.reportsDataSet.PageRanks);
            this.rptPagesRank.RefreshReport();
            rptPagesRank.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.FullPage;
        }
    }
}