using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace taqebostan
{
    public partial class frmLoading : Form
    {
        public frmLoading()
        {
            InitializeComponent();
        }

        private bool _allowClose = false;

        public bool allowClose
        {
            set
            {
                _allowClose = value;
            }
        }

        private void frmLoading_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_allowClose)
                e.Cancel = true;
        }

        private void frmLoading_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}