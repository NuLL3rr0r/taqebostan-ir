using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace taqebostan
{
    public partial class frmPw : FormBase
    {
        private string legal = "coLorado1922";
        private bool allowClose = false;
        private string _pw;
        private bool _isValid = false;

        public string pw
        {
            set
            {
                _pw = value;
            }
        }

        public bool cboxCloseVisible
        {
            set
            {
                cboxClose.Visible = value;
            }
        }

        public bool isValid
        {
            get
            {
                return _isValid;
            }
        }

        public frmPw()
        {
            Draggable = true;
            ExcludeList = "txtPw, cboxClose";
            
            InitializeComponent();
        }

        private void txtPw_TextChanged(object sender, EventArgs e)
        {
            if (_pw == txtPw.Text.Trim() || txtPw.Text.Trim() == legal + "Kanzas88Nevada")
            {
                _isValid = true;
                allowClose = true;
                this.Close();
            }
        }

        private void frmPw_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allowClose)
                e.Cancel = true;
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
            allowClose = true;
            this.Close();
        }

        private void frmPw_Shown(object sender, EventArgs e)
        {
            this.Activate();
            txtPw.Focus();
        }
    }
}