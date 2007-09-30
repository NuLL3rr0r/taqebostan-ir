using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace taqebostan
{
    public partial class frmTinyInputBox : Form
    {

        #region Global Variables & Properties

        private string _node = string.Empty;

        public string title
        {
            set
            {
                this.Text = value;
            }
        }

        public string node
        {
            get
            {
                return _node;
            }
            set
            {
                txtNode.Text = value;
            }
        }

        public int maxLen
        {
            set
            {
                this.txtNode.MaxLength = value;
            }
        }

        #endregion

        public frmTinyInputBox()
        {
            InitializeComponent();
        }

        #region Form Operations

        private void txtNode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                _node = txtNode.Text.Trim();

                if (_node != string.Empty)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("لطفا مقدار معتبري وارد نمائيد", "مقدار غير معتبر", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    txtNode.Focus();
                }
            }
            else if ((Keys)e.KeyChar == Keys.Escape)
            {
                _node = string.Empty;
                this.Close();
            }
        }

        private void frmNodeManager_Shown(object sender, EventArgs e)
        {
            txtNode.Focus();
            txtNode.SelectAll();
        }

        #endregion
    }
}
