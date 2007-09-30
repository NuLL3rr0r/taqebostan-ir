using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web.Services.Protocols;
using System.IO;

namespace taqebostan
{
    public partial class frmSplashScreen : Form
    {
        private string legal = "coLorado1963";
        //Nevada Kanzas
        private const string srvMsgErr = "err:";
        private const string srvMsgSuccess = "res:";
        private const string srvMsgInvalidLegal = "Illegal Access...";
        private int srvMsgLen = 4;
        
        private bool allowClose = false;
        private taq.Management wsrv = new taqebostan.taq.Management();
        private string loadingStr = "Shutting Down ";
        private string loadingChr = "●";
        private string errPrefix = "Error:\n\t";
        private string errFile = "امكان دسترسي به فايل ذيل، از منابع برنامه وجود ندارد\n\n{0}\n\n\nپيشنهاد مي شود اقدام به نصب مجدد برنامه نمائيد\n\nجهت خروج از برنامه كليد تائيد را بفشاريد";
        private string errFileHeader = "عدم دسترسي به منابع برنامه";
        private string errُُُُServer = "امكان دسترسي به وب سرور به دليل خطاي ذيل وجود ندارد";
        private string errServerHeader = "خطا در اتصال به سايت";
        private string msgTitle = "taqebostan.ir CMS v1.0";
        private string[] fileList = { "Microsoft.mshtml.dll", "Microsoft.ReportViewer.Common.dll", "Microsoft.ReportViewer.ProcessingObjectModel.dll", "Microsoft.ReportViewer.WinForms.dll", "reports.rpt" };

        private string path;
        private string cnnStr;
        private string dBpw = string.Empty;
        private string fileRpt = "reports.rpt";

        private bool _shutdown = false;

        public bool shutdown
        {
            set
            {
                _shutdown = value;
            }
        }


        public frmSplashScreen()
        {
            path = Environment.CurrentDirectory;
            path += path.EndsWith("\\") ? string.Empty : "\\"; ;
            fileRpt = String.Concat(path, fileRpt);
            cnnStr = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};", fileRpt, dBpw);

            InitializeComponent();
        }

        private bool ChkFiles()
        {
            bool found = true;

            foreach (string file in fileList)
            {
                found &= File.Exists(path + file);
                if (!found)
                    MessageBox.Show(String.Format(errFile, file), errFileHeader, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            }
            return found;
        }

        private void frmSplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allowClose)
                e.Cancel = true;
        }

        private void DoExit()
        {
            Environment.Exit(0);
        }

        private void ShowGUI(string pw)
        {
            frmGUI frm = new frmGUI();
            frm.pw = pw;
            frm.Show();
        }

        private void TryRequest(string req, string err)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(errُُُُServer);
            sb.Append("\n\n\n");
            sb.Append(err);
            DialogResult result = MessageBox.Show(sb.ToString(), errServerHeader, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            switch (result)
            {
                case DialogResult.Retry:
                    switch (req)
                    {
                        case "GetAdminPw":
                            this.Show();
                            GetAdminPw();
                            break;
                        default:
                            break;
                    }
                    break;
                case DialogResult.Cancel:
                    DoExit();
                    break;
                default:
                    break;
            }
        }

        private void GetAdminPw()
        {
            wsrv.GetAdminPwAsync(legal);
        }

        private void GetAdminPwCompleted(Object sender, taq.GetAdminPwCompletedEventArgs Completed)
        {
            try
            {
                this.Hide();
                string pw = Completed.Result;
                switch (pw.Substring(0, srvMsgLen))
                {
                    case srvMsgSuccess:
                        pw = pw.Substring(srvMsgLen);
                        break;
                    case srvMsgErr:
                        //An erorr ocurred
                        pw = pw.Substring(srvMsgLen);
                        if (pw == srvMsgInvalidLegal)
                        {
                            MessageBox.Show(errPrefix + pw, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            DoExit();
                        }
                        else
                        {
                            TryRequest("GetAdminPw", pw);
                            return;
                        }
                        break;
                    default:
                        break;
                }
                if (pw != string.Empty)
                {
                    using (frmPw dlg = new frmPw())
                    {
                        dlg.pw = pw;
                        dlg.ShowDialog(this.ParentForm);
                        if (dlg.isValid)
                            ShowGUI(pw);
                        else
                            //Password is inavlid - User closed the form
                            DoExit();
                    }
                }
                else
                    ShowGUI(pw);

            }
            catch (SoapException ex)
            {
                TryRequest("GetAdminPw", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("GetAdminPw", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void CleanAndRepairCompleted(Object sender, taq.CleanAndRepairCompletedEventArgs CleanAndRepairCompleted)
        {
            //Application.DoEvents();
            //System.Threading.Thread.Sleep(3000);
            //DoExit();

            tmrDoExit.Enabled = true;
        }

        private void frmSplashScreen_Shown(object sender, EventArgs e)
        {
            this.Activate();

            if (!_shutdown)
            {
                lblLoading.Visible = false;
                if (!ChkFiles())
                    DoExit();

                CompactJetDB(cnnStr, "reports.rpt");

                GetAdminPw();
            }
            else
            {
                pctLoading.Visible = false;
                lblLoading.Visible = true;
                lblLoading.Text = loadingStr;
                tmrLoading.Enabled = true;

                CompactJetDB(cnnStr, "reports.rpt");

                wsrv.CleanAndRepairAsync(legal);
            }
        }

        private void frmSplashScreen_Load(object sender, EventArgs e)
        {
            wsrv.GetAdminPwCompleted += new taq.GetAdminPwCompletedEventHandler(GetAdminPwCompleted);
            wsrv.CleanAndRepairCompleted += new taq.CleanAndRepairCompletedEventHandler(CleanAndRepairCompleted);
        }

        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            if (lblLoading.Text.Length < loadingStr.Length + 6)
                lblLoading.Text += loadingChr;
            else
                lblLoading.Text = loadingStr;
        }

        private void tmrDoExit_Tick(object sender, EventArgs e)
        {
            DoExit();
        }

        public void CompactJetDB(string connectionString, string mdwFilename)
        {
            try
            {
                string tmpFile = path + @"oracledb\\tmp.pak";

                object[] oParams;
                object objJRO = Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));
                oParams = new object[] { connectionString, String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5", tmpFile) };
                objJRO.GetType().InvokeMember("CompactDatabase", System.Reflection.BindingFlags.InvokeMethod, null, objJRO, oParams);

                System.IO.File.Delete(mdwFilename);
                System.IO.File.Move(tmpFile, mdwFilename);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(objJRO);
                objJRO = null;
            }
            catch
            {
            }
            finally
            {
            }
        }
    }
}