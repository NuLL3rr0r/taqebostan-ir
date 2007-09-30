using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace taqebostan
{
    public partial class frmContactList : FormBase
    {
        private string tblName = "contactlist";

        private bool goForChange = false;
        private DataTable _cList = new DataTable();
        private bool _retryMode = false;

        private string msgTitle = "taqebostan.ir CMS v1.0";


        public frmContactList()
        {
            Draggable = true;
            ExcludeList = "dgvMailBox, btnReturn, btnSave";

            InitializeComponent();
        }

        public DataTable cList
        {
            get
            {
                return _cList;
            }
            set
            {
                _cList = value;
                _cList.AcceptChanges();
            }
        }

        public bool hasChanged
        {
            get
            {
                return btnSave.Enabled;
            }
            set
            {
                btnSave.Enabled = value;
            }
        }

        public bool retryMode
        {
            get
            {
                return _retryMode;
            }
            set
            {
                _retryMode = value;
            }
        }

        private void doReturn()
        {
            this.Close();
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
            doReturn();
        }

        private void frmContactList_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = _cList;
            dt.Columns[0].ColumnName = "¬œ—” «Ì„Ì·";
            dt.Columns[1].ColumnName = "‰«„ »Œ‘";

            dgvMailBox.DataSource = dt;
            dgvMailBox.Columns[0].Width = 149;
            dgvMailBox.Columns[1].Width = 149;
            //dgvMailBox.Sort(dgvMailBox.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

            if (!_retryMode)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            doReturn();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int len = dgvMailBox.Rows.Count - 1;

            //trim spaces
            for (int i = 0; i < len; i++)
            {
                string mail = dgvMailBox.Rows[i].Cells[0].Value.ToString().Trim();
                dgvMailBox.Rows[i].Cells[0].Value = mail;
            }

            bool hasDuplicate = false;
            bool isZeroFillMail = false;
            bool isZeroFillName = false;

            string zeroFillMail = string.Empty;

            for (int i = 0; i < len; i++)
            {
                string cMail = dgvMailBox.Rows[i].Cells[0].Value.ToString().Trim();
                string nMail;

                if (i != len - 1)
                {
                    nMail = dgvMailBox.Rows[i + 1].Cells[0].Value.ToString().Trim();

                    if (cMail == nMail)
                    {
                        hasDuplicate = true;
                        break;
                    }
                }

                string cName = dgvMailBox.Rows[i].Cells[1].Value.ToString().Trim();

                if (cMail == string.Empty)
                {
                    isZeroFillMail = true;
                    break;
                }

                if (cName == string.Empty)
                {
                    isZeroFillName = true;
                    zeroFillMail = cMail;
                    break;
                }
            }

            if (!hasDuplicate && !isZeroFillMail && !isZeroFillName)
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("mailbox");
                dt.Columns.Add("name");

                dt.TableName = tblName;

                DataRow dr;

                for (int i = 0; i < len; i++)
                {
                    string mail = dgvMailBox.Rows[i].Cells[0].Value.ToString().Trim();
                    string name = dgvMailBox.Rows[i].Cells[1].Value.ToString().Trim();

                    dr = dt.NewRow();

                    dr[0] = mail;
                    dr[1] = name;

                    dt.Rows.Add(dr);
                }

                dt.AcceptChanges();

                _cList = dt;
                _cList.AcceptChanges();

                goForChange = true;
                doReturn();
            }
            else if (hasDuplicate)
                MessageBox.Show("œ— ·Ì”  «Ì„Ì· Â« ¬œ—”  ﬂ—«—Ì ÊÃÊœ œ«—œ∫ «„ﬂ«‰ œ—Ã ÊÃÊœ ‰œ«—œ", msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (isZeroFillMail)
                MessageBox.Show("Œÿ« œ— ¬œ—” «Ì„Ì· Â«", msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (isZeroFillName)
            {
                MessageBox.Show(String.Format("»—«Ì ¬œ—” «Ì„Ì· –Ì· Ê«Õœ „—»ÊÿÂ Ê«—œ ‰‘œÂ «” ∫ «„ﬂ«‰ œ—Ã ÊÃÊœ ‰œ«—œ\n\n\t{0}", zeroFillMail), msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            }
        }

        private void dgvMailBox_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            btnSave.Enabled = true;
        }

        private void dgvMailBox_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            btnSave.Enabled = true;
        }

        private void frmContactList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.Enabled)
            {
                if (!goForChange)
                {
                    if (MessageBox.Show(" €ÌÌ—«  –ŒÌ—Â ‰‘œÂ «” ° ¬Ì« „«Ì· »Â ·€Ê  €ÌÌ—«  Ê »«“ê‘  »Â ’›ÕÂ Ì «’·Ì „Ì »«‘Ìœø", msgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                    {
                        dgvMailBox.Focus();
                        e.Cancel = true;
                    }
                    else
                        btnSave.Enabled = false;
                }
            }
        }

        private void frmContactList_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}