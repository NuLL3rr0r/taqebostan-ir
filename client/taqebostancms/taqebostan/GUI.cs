using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web.Services.Protocols;
using System.IO;
using System.Data.OleDb;
using System.Threading;
using System.IO.Compression;
using System.Diagnostics;

namespace taqebostan
{
    public partial class frmGUI : FormBase
    {
        #region Declare global variable & properties

        //string url = "http://www.taqebostan.ir/";
        string url = "http://localhost:50559/taqebostan/";

        private string legal = "coLorado1963";
        private const string srvMsgErr = "err:";
        private const string srvMsgSuccess = "res:";
        private const string srvMsgInvalidLegal = "Illegal Access...";
        private const string srvMsgDSReject = "Rejected";
        private int srvMsgLen = 4;
        private string _pw;
        private string wMnuActived = string.Empty;

        private taq.Management wsrv = new taqebostan.taq.Management();
        private frmLoading frmLoading = new frmLoading();

        private string wPage;
        string lang = string.Empty;
        string wList = string.Empty;
        private string membersSaveStatus = string.Empty;
        private string[] galleryFiles = { };
        private string[] galleryServerFiles = { };
        private DataTable cList = new DataTable();
        private DataTable dtMembers = new DataTable();

        private string path;
        private string dBpw = string.Empty;
        private string fileRpt = "reports.rpt";
        private string cnnStr;
        string tmpPath = string.Empty;

        private string msgTitle = "taqebostan.ir CMS v1.0";
        //private string msgErrTitle = "خطاي زمان اجرا";
        private string errPrefix = "Error:\n\t";
        private string errDSReject = "سرور قادر به بروز رساني اطلاعات ورودي نمي باشد";
//        private string errInvalidLegal = "دسترسي غير مجاز";
        private string errServer = "امكان دسترسي به وب سرور به دليل خطاي ذيل وجود ندارد";
        private string errServerHeader = "خطا در اتصال به سايت";
        private string errReport = "به دليل خطاي ذيل امكان گزارش گيري وجود ندارد\n\n";
        private string errReportHeader = "خطا در اعلام گزارشات";
        private string errConfirmPw = "لطفا كلمه ي عبور جديد را تائيد نمائيد";
        private string errCurrentPw = "لطفا كلمه عبور فعلي را وارد نمائيد";
        private string errGalleryNoPic = "تصويري يافت نشد";

        private bool hasChanged = false;
        private ErrorProvider errProvider;

        private string rootTitleFa = "منوهاي وب سايت";
        private string rootTitleEn = "Website Menus";

        private string rptTarget = string.Empty;


        public string pw
        {
            set
            {
                _pw = value;
            }
        }

        #endregion

        #region FormLoad & Async Methods Declarations & cnn Definition

        public frmGUI()
        {
            path = Environment.CurrentDirectory;
            path += path.EndsWith("\\") ? string.Empty : "\\";;
            fileRpt = String.Concat(path, fileRpt);
            cnnStr = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};", fileRpt, dBpw);

            Draggable = true;
            ExcludeList = "cboxClose, cboxMax, cboxMin, mnuAbout, mnuEditor, mnuExit, mnuGallery, mnuMain, mnuMembers, mnuPw, mnuReports, btnProtect, stabsNavigation";

            InitializeComponent();

            errProvider = new ErrorProvider();
            errProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            //for sync call
            //wsrv.Timeout = 2147483647;
            //or
            /*
            public Management() {
                this.Url = global::taqebostan.Properties.Settings.Default.taqebostan_taq_Management;
                this.Timeout = 2147483647;
                .
                .
                .
            }
            */

            wsrv.GetAdminPwCompleted += new taq.GetAdminPwCompletedEventHandler(GetAdminPwCompleted);
            wsrv.SetAdminPwCompleted += new taq.SetAdminPwCompletedEventHandler(SetAdminPwCompleted);
            wsrv.GetServerPageCompleted += new taq.GetServerPageCompletedEventHandler(GetServerPageCompleted);
            wsrv.SetServerPageCompleted += new taq.SetServerPageCompletedEventHandler(SetServerPageCompleted);
            wsrv.ReportsPagesViewCountCompleted += new taq.ReportsPagesViewCountCompletedEventHandler(ReportsPagesViewCountCompleted);
            wsrv.GalleryCatchChangesCompleted += new taq.GalleryCatchChangesCompletedEventHandler(GalleryCatchChangesCompleted);
            wsrv.GalleryImagesListCompleted += new taq.GalleryImagesListCompletedEventHandler(GalleryImagesListCompleted);
            wsrv.GalleryImagesDataCompleted += new taq.GalleryImagesDataCompletedEventHandler(GalleryImagesDataCompleted);
            wsrv.ContactListCompleted += new taq.ContactListCompletedEventHandler(ContactListCompleted);
            wsrv.ContactListCatchChangesCompleted += new taq.ContactListCatchChangesCompletedEventHandler(ContactListCatchChangesCompleted);
            wsrv.NodesAllTreesCompleted += new taq.NodesAllTreesCompletedEventHandler(NodesAllTreesCompleted);
           
            //DocClose();
            FormGallery(false);
            //FormMembers(false);
            //ClearFormPw();
        }

        private void frmGUI_Load(object sender, EventArgs e)
        {

            NavigateHomePage();
            SetEditorMnuState(false);
        }

        private void frmGUI_Shown(object sender, EventArgs e)
        {
            this.Activate();
            RegenMenues();
            this.Activate();
        }

        #endregion

        #region Common Form Operations

        private void DoExit()
        {
            this.Hide();
            frmSplashScreen frm = new frmSplashScreen();
            frm.shutdown = true;
            frm.Show();
        }

        private void DoSafeExit()
        {
            if (hasChanged)
            {
                if (MessageBox.Show("تغييرات ذخيره نشده است، آيا مايل به لغو تغييرات و خروج مي باشيد؟", msgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                {
                    return;
                }
            }

            mnuExit.ImageIndex = 2;
            cboxClose.ImageIndex = 2;

            DoExit();
        }

        private bool StayForChanges()
        {
            if (hasChanged)
            {
                if (MessageBox.Show("تغييرات ذخيره نشده است، آيا مايل به لغو تغييرات مي باشيد؟", msgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                {
                    return true;
                }
                else
                {
                    hasChanged = false;
                }
            }

            return false;
        }

        private void frmGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            DoExit();
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
            DoSafeExit();
        }

        private void cboxMin_MouseEnter(object sender, EventArgs e)
        {
            cboxMin.ImageIndex = 1;
        }

        private void cboxMin_MouseDown(object sender, MouseEventArgs e)
        {
            cboxMin.ImageIndex = 0;
        }

        private void cboxMin_MouseLeave(object sender, EventArgs e)
        {
            cboxMin.ImageIndex = 2;
        }

        private void cboxMin_MouseUp(object sender, MouseEventArgs e)
        {
            cboxMin.ImageIndex = 1;
        }

        private void cboxMin_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (_pw == string.Empty)
                ctxExit.Enabled = true;
            else
                ctxExit.Enabled = false;
            ntfyMinimize.Visible = true;
        }

        private void RestoreForm()
        {
            ntfyMinimize.Visible = false;
            if (_pw != string.Empty)
            {
                using (frmPw dlg = new frmPw())
                {
                    dlg.pw = _pw;
                    dlg.cboxCloseVisible = false;
                    dlg.ShowDialog(this);
                }
            }
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void ntfyMinimize_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreForm();
        }

        private void ctxReturn_Click(object sender, EventArgs e)
        {
            RestoreForm();
        }

        private void ctxTimeSinceReboot_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Concat("حدودا ", ((Int32)((Environment.TickCount / 1000) / 60)).ToString(), " دقيقه از زمان بوت ويندوز مي گذرد"), "آخرين بوت", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ctxCurrentOSVersion_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Concat("OS Version: ", Environment.OSVersion.ToString()), "Operating System", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ctxFrameworkVersion_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Concat("Framework Version: ", Environment.Version.ToString()), ".NET Framework Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ctxCurrentTimeZone_Click(object sender, EventArgs e)
        {
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
                MessageBox.Show(String.Concat(TimeZone.CurrentTimeZone.DaylightName, " :منطقه زماني فعلي سيستم"), "منطقه زماني", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(String.Concat(TimeZone.CurrentTimeZone.StandardName, " :منطقه زماني فعلي سيستم"), "منطقه زماني", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetPersianDate()
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            DateTime dt = DateTime.Now;

            //{0} = Year
            //{1} = Month
            //{2} = Day
            return String.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
        }

        private void ctxCurrentDate_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Concat(DateTime.Now.ToLongDateString(), "   ", DateTime.Now.ToLongTimeString(), "       ", GetPersianDate(), " :تاريخ شمسي"), "تاريخ و ساعت", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ctxExit_Click(object sender, EventArgs e)
        {
            ntfyMinimize.Visible = false;
            DoExit();
        }

        private void btnProtect_MouseDown(object sender, MouseEventArgs e)
        {
            btnProtect.ImageIndex = 0;
        }

        private void btnProtect_MouseUp(object sender, MouseEventArgs e)
        {
            btnProtect.ImageIndex = 1;
        }

        private void btnProtect_Click(object sender, EventArgs e)
        {
            if (_pw != string.Empty)
            {
                btnProtect.ImageIndex = 1;
                this.Hide();
                using (frmPw dlg = new frmPw())
                {
                    dlg.pw = _pw;
                    dlg.cboxCloseVisible = false;
                    dlg.ShowDialog(this);
                    this.Show();
                }
            }
        }

        private void NavigateHomePage()
        {
            Loading(false);
            SetMnuActived("Editor");
            wMnuActived = "Editor";
            mnuEditor.ImageIndex = 0;
            stabsNavigation.SelectedTab = stabEditor;
        }

        private void Loading(bool status)
        {
            try
            {
                frmLoading.allowClose = !status;
                if (status)
                {
                    if (!IsLoading())
                        frmLoading.ShowDialog(this);
                    frmLoading.Activate();
                }
                else
                {
                    if (IsLoading())
                        frmLoading.Close();
                    this.Activate();
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        private bool IsLoading()
        {
            if (frmLoading.Visible)
                return true;
            else
                return false;
        }

        private void RenderTable(DataGridView dgv, DataTable dt, int col, TextBox textBox)
        {
            string phrase = textBox.Text.Trim();
            if (phrase != string.Empty)
            {
                DataTable dtr = dt.Clone();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][col].ToString().Trim().IndexOf(phrase) > -1)
                    {
                        object[] row = dt.Rows[i].ItemArray;
                        dtr.Rows.Add(row);
                    }
                }
                dgv.DataSource = dtr;
            }
            else
                dgv.DataSource = dt;
        }

        private DataTable RenderTable(DataTable dt, int col, string phrase)
        {
            if (phrase != string.Empty)
            {
                DataTable dtr = dt.Clone();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][col].ToString().Trim().IndexOf(phrase) > -1)
                    {
                        object[] row = dt.Rows[i].ItemArray;
                        dtr.Rows.Add(row);
                    }
                }
                return dtr;
            }
            else
                return dt;
        }

        private string NameGen()
        {
            Random rnd = new Random();
            String key = String.Empty;
            int min = -1, max = -1;

            for (int i = 0; i < 33; i++)
            {
                switch (rnd.Next(2))
                {
                    case 0:
                        min = 48;
                        max = 58;
                        break;
                    case 1:
                        min = 97;
                        max = 123;
                        break;
                    default:
                        break;
                }
                key = String.Concat(key, Convert.ToChar(rnd.Next(min, max)));
            }

            return key;
        }

        #endregion

        #region Left Side Menu Operations & Functions

        private void SetMnuActived(string mnu)
        {
            switch (wMnuActived)
            {
                case "Editor":
                    mnuEditor.ImageIndex = 2;
                    DocClose();
                    break;
                case "Gallery":
                    mnuGallery.ImageIndex = 2;
                    FormGallery(false);
                    break;
                case "Report":
                    mnuReports.ImageIndex = 2;
                    break;
                case "Pw":
                    mnuPw.ImageIndex = 2;
                    ClearFormPw();
                    break;
                case "About":
                    mnuAbout.ImageIndex = 2;
                    break;
                case "Exit":
                    mnuExit.ImageIndex = 2;
                    break;
                default:
                    mnuEditor.ImageIndex = 2;
                    mnuGallery.ImageIndex = 2;
                    mnuReports.ImageIndex = 2;
                    mnuPw.ImageIndex = 2;
                    mnuAbout.ImageIndex = 2;
                    mnuExit.ImageIndex = 2;
                    break;
            }

            wMnuActived = mnu;

            // for StayForChanges() function
            switch (wMnuActived)
            {
                case "Editor":
                    mnuEditor.ImageIndex = 0;
                    break;
                case "Gallery":
                    mnuGallery.ImageIndex = 0;
                    break;
                case "Report":
                    mnuReports.ImageIndex = 0;
                    break;
                case "Pw":
                    mnuPw.ImageIndex = 0;
                    break;
                case "About":
                    mnuAbout.ImageIndex = 0;
                    break;
                case "Exit":
                    mnuExit.ImageIndex = 0;
                    break;
                default:
                    break;
            }
            // end for StayForChanges() function
        }

        private void mnuExit_MouseEnter(object sender, EventArgs e)
        {
            if (wMnuActived != "Exit")
                mnuExit.ImageIndex = 1;
        }

        private void mnuExit_MouseDown(object sender, MouseEventArgs e)
        {
            mnuExit.ImageIndex = 0;
        }

        private void mnuExit_MouseLeave(object sender, EventArgs e)
        {
            if (wMnuActived != "Exit")
                mnuExit.ImageIndex = 2;
        }

        private void mnuExit_MouseUp(object sender, MouseEventArgs e)
        {
            if (wMnuActived != "Exit" && !hasChanged)
                mnuExit.ImageIndex = 1;
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (wMnuActived != "Exit")
            {
                DoSafeExit();
//                SetMnuActived("Exit");
//                stabsNavigation.SelectedTab = stabExit;
            }
        }

        private void mnuAbout_MouseEnter(object sender, EventArgs e)
        {
            if (wMnuActived != "About")
                mnuAbout.ImageIndex = 1;
        }

        private void mnuAbout_MouseDown(object sender, MouseEventArgs e)
        {
            mnuAbout.ImageIndex = 0;
        }

        private void mnuAbout_MouseLeave(object sender, EventArgs e)
        {
            if (wMnuActived != "About")
                mnuAbout.ImageIndex = 2;
        }

        private void mnuAbout_MouseUp(object sender, MouseEventArgs e)
        {
            if (wMnuActived != "About" && !hasChanged)
                mnuAbout.ImageIndex = 1;
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            if (!StayForChanges())
            {
                if (wMnuActived != "About")
                {
                    SetMnuActived("About");
                    stabsNavigation.SelectedTab = stabAbout;
                }
            }
        }

        private void mnuPw_MouseEnter(object sender, EventArgs e)
        {
            if (wMnuActived != "Pw")
                mnuPw.ImageIndex = 1;
        }

        private void mnuPw_MouseDown(object sender, MouseEventArgs e)
        {
            mnuPw.ImageIndex = 0;
        }

        private void mnuPw_MouseLeave(object sender, EventArgs e)
        {
            if (wMnuActived != "Pw")
                mnuPw.ImageIndex = 2;
        }

        private void mnuPw_MouseUp(object sender, MouseEventArgs e)
        {
            if (wMnuActived != "Pw" && !hasChanged)
                mnuPw.ImageIndex = 1;
        }

        private void mnuPw_Click(object sender, EventArgs e)
        {
            if (!StayForChanges())
            {
                if (wMnuActived != "Pw")
                {
                    SetMnuActived("Pw");
                    stabsNavigation.SelectedTab = stabPw;
                    SendRequest("GetAdminPw");
                }
            }
        }

        private void mnuReports_MouseEnter(object sender, EventArgs e)
        {
            if (wMnuActived != "Report")
                mnuReports.ImageIndex = 1;
        }

        private void mnuReports_MouseDown(object sender, MouseEventArgs e)
        {
            mnuReports.ImageIndex = 0;
        }

        private void mnuReports_MouseLeave(object sender, EventArgs e)
        {
            if (wMnuActived != "Report")
                mnuReports.ImageIndex = 2;
        }

        private void mnuReports_MouseUp(object sender, MouseEventArgs e)
        {
            if (wMnuActived != "Report" && !hasChanged)
                mnuReports.ImageIndex = 1;
        }

        private void mnuReports_Click(object sender, EventArgs e)
        {
            if (!StayForChanges())
            {
                if (wMnuActived != "Report")
                {
                    SetMnuActived("Report");
                    stabsNavigation.SelectedTab = stabReports;
                }
            }
        }

        private void mnuGallery_MouseEnter(object sender, EventArgs e)
        {
            if (wMnuActived != "Gallery")
                mnuGallery.ImageIndex = 1;
        }

        private void mnuGallery_MouseDown(object sender, MouseEventArgs e)
        {
            mnuGallery.ImageIndex = 0;
        }

        private void mnuGallery_MouseLeave(object sender, EventArgs e)
        {
            if (wMnuActived != "Gallery")
                mnuGallery.ImageIndex = 2;
        }

        private void mnuGallery_MouseUp(object sender, MouseEventArgs e)
        {
            if (wMnuActived != "Gallery" && !hasChanged)
                mnuGallery.ImageIndex = 1;
        }

        private void mnuGallery_Click(object sender, EventArgs e)
        {
            if (!StayForChanges())
            {
                if (wMnuActived != "Gallery")
                {
                    SetMnuActived("Gallery");
                    stabsNavigation.SelectedTab = stabGallery;
                }
            }
        }

        private void mnuEditor_MouseEnter(object sender, EventArgs e)
        {
            if (wMnuActived != "Editor")
                mnuEditor.ImageIndex = 1;
        }

        private void mnuEditor_MouseDown(object sender, MouseEventArgs e)
        {
            mnuEditor.ImageIndex = 0;
        }

        private void mnuEditor_MouseLeave(object sender, EventArgs e)
        {
            if (wMnuActived != "Editor")
                mnuEditor.ImageIndex = 2;
        }

        private void mnuEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (wMnuActived != "Editor" && !hasChanged)
                mnuEditor.ImageIndex = 1;
        }

        private void mnuEditor_Click(object sender, EventArgs e)
        {
            if (!StayForChanges())
            {
                if (wMnuActived != "Editor")
                {
                    SetMnuActived("Editor");
                    stabsNavigation.SelectedTab = stabEditor;
                }
            }
        }

        #endregion

        #region Calling Methods Async

        private void TryRequest(string req, string err)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(errServer);
            sb.Append("\n\n\n");
            sb.Append(err);
            DialogResult result = MessageBox.Show(sb.ToString(), errServerHeader, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            switch (result)
            {
                case DialogResult.Retry:
                    SendRequest(req);
                    break;
                case DialogResult.Cancel:
                    if (req == "ContactListCatchChanges")
                    {
                        using (frmContactList frm = new frmContactList())
                        {
                            frm.cList = cList;
                            cList = new DataTable();
                            frm.retryMode = true;
                            frm.ShowDialog(this);

                            if (frm.hasChanged)
                            {
                                cList = frm.cList;
                                SendRequest("ContactListCatchChanges");
                            }
                            else
                                Loading(false);
                        }
                        return;
                    }
                    Loading(false);
                    //NavigateHomePage();
                    break;
                default:
                    break;
            }
        }

        private bool SendRequestSetServerPage()
        {
            try
            {
                string pg = editor.BodyHtml.Trim();
                string[] edImages = { };
                string[] ext = { };
                byte[][] buffer = { };
                string[] ph = { };

                string src = "src=\"";
                string edFilePath = string.Empty;

                int pos1 = -1;
                int pos2 = 0;

                while (true)
                {
                    if (pos2 > pg.Length)
                        break;

                    pos1 = pg.ToLower().IndexOf(src, pos2) + src.Length;

                    if (pos1 != src.Length - 1)
                    {
                        pos2 = pg.ToLower().IndexOf("\"", pos1);
                        edFilePath = pg.Substring(pos1, pos2 - pos1);

                        pos2 = pos1 + 1;

                        if (edFilePath.IndexOf(url) == -1)
                        {
                            if (edFilePath.IndexOf("http://") != -1)
                                continue;

                            if (edFilePath.Substring(0, 1) != "{")
                            {
                                int len = ph.Length;
                                Array.Resize(ref ph, len + 1);
                                Array.Resize(ref ext, len + 1);
                                Array.Resize(ref buffer, len + 1);

                                using (FileStream fs = new FileStream(edFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8))
                                {
                                    int edFileSize = (int)new FileInfo(edFilePath).Length;
                                    ext[len] = edFilePath.Substring(edFilePath.LastIndexOf(".")).ToLower().Trim();
                                    buffer[len] = new byte[edFileSize];
                                    fs.Read(buffer[len], 0, edFileSize);
                                    fs.Close();
                                    ph[len] = String.Format("{{PlaceHolder/{0}}}", len);
                                }

                                pg = pg.Replace(edFilePath, String.Format("{{PlaceHolder/{0}}}", len));
                            }
                        }
                        else
                        {
                            pg = pg.Remove(pos1, url.Length);
                        }
                    }
                    else
                        break;
                }

                pg = pg.Replace("about:blank", "");
                pg = pg.Replace("##", "#");
                pg = pg.Replace("&amp;", "&");


                wsrv.SetServerPageAsync(wPage, Zipper.Compress(pg), buffer, ext, ph, lang, legal);
                return true;
            }
            catch (IOException ex)
            {
                MessageBox.Show(errPrefix + ex.Message, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Loading(false);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(errPrefix + ex.Message, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Loading(false);
                return false;
            }
            finally
            {
            }
        }

        private bool SendRequestGalleryCatchChanges()
        {
            try
            {
                int len = -1;
                string[] ext = { };
                byte[][] buffer = { };
                string[] erased = { };

                foreach (string e in galleryServerFiles)
                {
                    bool isErased = true;
                    foreach (string f in galleryFiles)
                    {
                        if (e == f)
                        {
                            isErased = false;
                            break;
                        }
                    }
                    if (isErased)
                    {
                        len = erased.Length;
                        Array.Resize(ref erased, len + 1);
                        string id = e.Substring(e.LastIndexOf("\\") + 1);
                        id = id.Substring(0, id.LastIndexOf("."));
                        erased[len] = id;
                    }
                }

                for (int i = 0; i < galleryFiles.Length; i++)
                {
                    string filePath = galleryFiles[i];

                    bool isServerSide = false;
                    foreach (string f in galleryServerFiles)
                    {
                        if (filePath.IndexOf(tmpPath) != -1)
                        {
                            isServerSide = true;
                            break;
                        }
                    }

                    if (isServerSide)
                        continue;

                    len = ext.Length;
                    Array.Resize(ref ext, len + 1);
                    Array.Resize(ref buffer, len + 1);

                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8))
                    {
                        int fileSize = (int)new FileInfo(filePath).Length;
                        ext[len] = filePath.Substring(filePath.LastIndexOf(".")).ToLower().Trim();
                        buffer[len] = new byte[fileSize];
                        fs.Read(buffer[len], 0, fileSize);
                        fs.Close();
                    }
                }

                wsrv.GalleryCatchChangesAsync(buffer, ext, erased, legal);
                return true;
            }
            catch (IOException ex)
            {
                MessageBox.Show(errPrefix + ex.Message, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Loading(false);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(errPrefix + ex.Message, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Loading(false);
                return false;
            }
            finally
            {
            }
        }

        private void SendRequest(string req)
        {
            try
            {
                switch (req)
                {
                    case "GetAdminPw":
                        wsrv.GetAdminPwAsync(legal);
                        break;
                    case "SetAdminPw":
                        wsrv.SetAdminPwAsync(_pw, txtPwNew.Text, legal);
                        break;
                    case "GetServerPage":
                        if (!StayForChanges())
                        {
                            DocClose();
                            wsrv.GetServerPageAsync(wPage, lang, legal);
                        }
                        else
                            return;
                        break;
                    case "SetServerPage":
                        if (!SendRequestSetServerPage())
                            return;
                        break;
                    case "ReportsPagesViewCount":
                        wsrv.ReportsPagesViewCountAsync(rptTarget, legal);
                        break;
                    case "GalleryCatchChanges":
                        if (!SendRequestGalleryCatchChanges())
                            return;
                        break;
                    case "GalleryImagesList":
                        wsrv.GalleryImagesListAsync(legal);
                        break;
                    case "GalleryImagesData":
                        wsrv.GalleryImagesDataAsync(legal);
                        break;
                    case "ContactList":
                        if (!StayForChanges())
                        {
                            DocClose();
                            wsrv.ContactListAsync(wList, legal);
                        }
                        else
                            return;
                        break;
                    case "ContactListCatchChanges":
                        wsrv.ContactListCatchChangesAsync(wList, cList, legal);
                        break;
                    case "NodesAllTrees":
                        wsrv.NodesAllTreesAsync(legal);
                        break;
                    default:
                        break;
                }
                Loading(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(errPrefix + ex.Message, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Loading(false);
            }
            finally
            {
            }
        }

        private void GetAdminPwCompleted(Object sender, taq.GetAdminPwCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;
                switch (result.Substring(0, srvMsgLen))
                {
                    case srvMsgSuccess:
                        _pw = result.Substring(srvMsgLen);
                        Loading(false);
                        txtPwCurrent.Focus();
                        break;
                    case srvMsgErr:
                        //An erorr ocurred
                        result = result.Substring(srvMsgLen);
                        if (result == srvMsgInvalidLegal)
                        {
                            MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            DoExit();
                        }
                        else
                        {
                            TryRequest("GetAdminPw", result);
                            return;
                        }
                        break;
                    default:
                        break;
                }
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

        private void SetAdminPwCompleted(Object sender, taq.SetAdminPwCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;
                switch (result)
                {
                    case "OK":
                        _pw = txtPwNew.Text;
                        MessageBox.Show("كلمه ي عبور جديد با موفقيت جايگزين شد", msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Loading(false);
                        ClearFormPw();
                        NavigateHomePage();
                        break;
                    case srvMsgDSReject:
                        TryRequest("SetAdminPw", errDSReject);
                        break;
                    case srvMsgInvalidLegal:
                        MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DoExit();
                        break;
                    default:
                        TryRequest("SetAdminPw", result);
                        break;
                }
            }
            catch (SoapException ex)
            {
                TryRequest("SetAdminPw", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("SetAdminPw", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void GetServerPageCompleted(Object sender, taq.GetServerPageCompletedEventArgs Completed)
        {
            try
            {
                string result = Zipper.DecompressToStrng(Completed.Result);
                switch (result.Substring(0, srvMsgLen))
                {
                    case srvMsgSuccess:
                        result = result.Substring(srvMsgLen);

                        int pos1 = -1;
                        int pos2 = 0;
                        string src = "src=\"";

                        while (true)
                        {
                            pos1 = result.IndexOf(src, pos2) + src.Length;
                            if (pos1 != src.Length - 1)
                            {
                                pos2 = result.IndexOf("\"", pos1);

                                if (result.IndexOf("http://", pos1, pos2 - pos1) != -1)
                                    continue;

                                result = result.Insert(pos1, url);
                            }
                            else
                                break;
                        }

                        if (result == string.Empty)
                            result = "<p>&nbsp;</p>";


                        string start = "<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"fa\" xml:lang=\"fa\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><title></title></head><style type=\"text/css\">html, body {direction: rtl; text-align: justify; font-family: Tahoma; line-height: 33px;}</style><body>";
                        string stop = "</body></html>";
                        editor.Document.Write(string.Empty);
                        editor.DocumentText = start + result + stop;
                        editor.Tick += new Editor.TickDelegate(editor_Tick);

                        Loading(false);
                        break;
                    case srvMsgErr:
                        //An erorr ocurred
                        result = result.Substring(srvMsgLen);
                        if (result == srvMsgInvalidLegal)
                        {
                            MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            DoExit();
                        }
                        else
                        {
                            TryRequest("GetServerPage", result);
                            return;
                        }
                        break;
                    default:
                        break;
                }
                SetEditorMnuState(true);
            }
            catch (SoapException ex)
            {
                TryRequest("GetServerPage", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("GetServerPage", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void SetServerPageCompleted(Object sender, taq.SetServerPageCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;

                switch (result)
                {
                    case "Saved":
                        hasChanged = false;
                        DocClose();
                        MessageBox.Show("تغييرات با موفقيت اعمال شد", "ويرايشگر صفحات", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        Loading(false);
                        break;
                    case srvMsgDSReject:
                        MessageBox.Show(errPrefix + errDSReject, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case srvMsgInvalidLegal:
                        MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DoExit();
                        break;
                    default:
                        TryRequest("SetServerPage", result);
                        break;
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(errPrefix + ex.Message, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Loading(false);
            }
            catch (SoapException ex)
            {
                TryRequest("SetServerPage", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("SetServerPage", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        /*private void GetPagesRankCompleted(Object sender, taq.GetPagesRankCompletedEventArgs Completed)
        {
            try
            {
                DataSet result = Completed.Result;

                Loading(false);

                if (GetPagesRank(result))
                {
                    frmReports frm = new frmReports();
                    frm.ShowDialog(this);
                    CleanReports();
                }
            }
            catch (SoapException ex)
            {
                TryRequest("GetPagesRank", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("GetPagesRank", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }*/

        private void ReportsPagesViewCountCompleted(Object sender, taq.ReportsPagesViewCountCompletedEventArgs Completed)
        {
            try
            {
                DataTable result = Completed.Result;

                Loading(false);

                if (GetPagesRank(result))
                {
                    frmReports frm = new frmReports();
                    frm.ShowDialog(this);
                    CleanReports();
                }
            }
            catch (SoapException ex)
            {
                TryRequest("ReportsPagesViewCount", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("ReportsPagesViewCount", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void GalleryCatchChangesCompleted(Object sender, taq.GalleryCatchChangesCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;

                switch (result)
                {
                    case "Created":
                        FormGallery(false);
                        GalleryChanged(false);
                        MessageBox.Show("تغييرات با موفقيت اعمال شد", "گالري", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        Loading(false);
                        break;
                    case srvMsgInvalidLegal:
                        MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DoExit();
                        break;
                    default:
                        TryRequest("FixServerPageImage", result);
                        break;
                }
            }
            catch (SoapException ex)
            {
                TryRequest("GalleryCatchChanges", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("GalleryCatchChanges", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void GalleryImagesListCompleted(Object sender, taq.GalleryImagesListCompletedEventArgs Completed)
        {
            try
            {
                Array.Resize(ref galleryServerFiles, 0);

                galleryServerFiles = Completed.Result;


                if (galleryServerFiles.Length > 0)
                    SendRequest("GalleryImagesData");
                else
                    Loading(false);

            }
            catch (SoapException ex)
            {
                TryRequest("GalleryImagesList", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("GalleryImagesList", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void GalleryImagesDataCompleted(Object sender, taq.GalleryImagesDataCompletedEventArgs Completed)
        {
            try
            {
                byte[][] buffer = Completed.Result;

                tmpPath = System.IO.Path.GetTempPath();
                tmpPath += tmpPath.EndsWith("\\") ? string.Empty : "\\";

                while (true)
                {
                    tmpPath += NameGen() + "\\";
                    if (!Directory.Exists(tmpPath))
                    {
                        Directory.CreateDirectory(tmpPath);
                        break;
                    }
                }

                for (int i = 0; i < galleryServerFiles.Length; i++)
                {
                    galleryServerFiles[i] = tmpPath + galleryServerFiles[i];

                    using (FileStream fs = new FileStream(galleryServerFiles[i], FileMode.Create))
                    {
                        fs.Write(buffer[i], 0, buffer[i].Length);
                        fs.Close();
                    }
                }

                AddImagesToGallery("FromServer");

                Loading(false);
            }
            catch (SoapException ex)
            {
                TryRequest("GalleryImagesData", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("GalleryImagesData", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void ContactListCompleted(Object sender, taq.ContactListCompletedEventArgs Completed)
        {
            try
            {
                DataTable result = Completed.Result;

                using (frmContactList frm = new frmContactList())
                {
                    frm.cList = result;
                    frm.ShowDialog(this);

                    if (frm.hasChanged)
                    {
                        cList = frm.cList;
                        SendRequest("ContactListCatchChanges");
                    }
                    else
                        Loading(false);
                }
            }
            catch (SoapException ex)
            {
                TryRequest("ContactList", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("ContactList", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void ContactListCatchChangesCompleted(Object sender, taq.ContactListCatchChangesCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;
                switch (result)
                {
                    case "Catched":
                        MessageBox.Show("ليست تماس ها با موفقيت ذخيره شد", msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cList = new DataTable();
                        Loading(false);
                        break;
                    case "Can't Clean Table":
                        TryRequest("ContactListCatchChanges", "در حال حاضر سرور قادر به ذخيره تغييرات مربوط به ليست تماس ها نمي باشد");
                        break;
                    case srvMsgDSReject:
                        TryRequest("ContactListCatchChanges", errDSReject);
                        break;
                    case srvMsgInvalidLegal:
                        MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DoExit();
                        break;
                    default:
                        TryRequest("ContactListCatchChanges", result);
                        break;
                }
            }
            catch (SoapException ex)
            {
                TryRequest("ContactListCatchChanges", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("ContactListCatchChanges", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void NodesAllTreesCompleted(Object sender, taq.NodesAllTreesCompletedEventArgs Completed)
        {
            try
            {
                DataSet ds = Completed.Result;

                RegenMenu(mItemOpenPersian, ds.Tables["pagesfa"], rootTitleFa);
                RegenMenu(mItemOpenEnglish, ds.Tables["pagesen"], rootTitleEn);

                Loading(false);
            }
            catch (SoapException ex)
            {
                TryRequest("NodesAllTrees", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("NodesAllTrees", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        #endregion

        #region Compress / Decompress Section

        /*        private byte[] Compress(string data)
        {
            return Compress(Encoding.Unicode.GetBytes(data));
        }

        private string DecompressToStr(byte[] data)
        {
            return Encoding.Unicode.GetString(Decompress(data));
        }

        private byte[] Compress(byte[] data)
        {
            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

                MemoryStream ms = new MemoryStream();

                Stream s = new DeflateStream(ms, CompressionMode.Compress);
                s.Write(data, 0, data.Length);
                s.Close();

                byte[] cData = { };
                cData = (byte[])ms.ToArray();

                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;

                return cData;
            }
            catch
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                return null;
            }
        }

        private byte[] Decompress(byte[] data)
        {
            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

                string result = string.Empty;
                int tLen = 0;
                byte[] wData = new byte[4096];

                MemoryStream ms = new MemoryStream(data);
                Stream s = new DeflateStream(ms, CompressionMode.Decompress);

                while (true)
                {
                    int size = s.Read(wData, 0, wData.Length);
                    if (size > 0)
                    {
                        tLen += size;
                        result += System.Text.Encoding.Unicode.GetString(wData, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }
                s.Close();

                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;

                return Encoding.Unicode.GetBytes(result);
            }
            catch
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                return null;
            }
        }
*/
        #endregion

        # region Change Password Section

        private void ClearFormPw()
        {
            txtPwCurrent.Clear();
            txtPwNew.Clear();
            txtPwConfirm.Clear();

            hasChanged = false;
        }

        private void txtPwCurrent_TextChanged(object sender, EventArgs e)
        {
            hasChanged = true;
        }

        private void txtPwNew_TextChanged(object sender, EventArgs e)
        {
            hasChanged = true;
        }

        private void txtPwConfirm_TextChanged(object sender, EventArgs e)
        {
            hasChanged = true;
        }

        private void btnPwCancel_Click(object sender, EventArgs e)
        {
            ClearFormPw();
            NavigateHomePage();
        }

        private void ShowPwErr(Control obj, string msg)
        {
            errProvider.SetIconAlignment(obj, ErrorIconAlignment.MiddleLeft);
            errProvider.SetIconPadding(obj, 8);
            errProvider.SetError(obj, msg);
        }

        private void ClearPwErr()
        {
            errProvider.SetError(txtPwCurrent, string.Empty);
            errProvider.SetError(txtPwConfirm, string.Empty);
        }

        private void btnPwOK_Click(object sender, EventArgs e)
        {
            if (txtPwCurrent.Text == _pw)
            {
                if (txtPwNew.Text == txtPwConfirm.Text)
                {
                    ClearPwErr();
                    SendRequest("SetAdminPw");
                }
                else
                {
                    ClearPwErr();
                    MessageBox.Show(errConfirmPw, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ShowPwErr(txtPwConfirm, errConfirmPw);
                    txtPwConfirm.Focus();
                    txtPwConfirm.SelectAll();
                }
            }
            else
            {
                ClearPwErr();
                MessageBox.Show(errCurrentPw, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ShowPwErr(txtPwCurrent, errCurrentPw);
                txtPwCurrent.Focus();
                txtPwCurrent.SelectAll();
            }
        }
        #endregion

        #region Report Section Buttons Actions

        private void sbmnuReportsVCount_MouseEnter(object sender, EventArgs e)
        {
            sbmnuReportsVCount.BackgroundImage = global::taqebostan.Properties.Resources.sbmnu_reports_vcount_over;
        }

        private void sbmnuReportsVCount_MouseDown(object sender, MouseEventArgs e)
        {
            sbmnuReportsVCount.BackgroundImage = global::taqebostan.Properties.Resources.sbmnu_reports_vcount_down;
        }

        private void sbmnuReportsVCount_MouseLeave(object sender, EventArgs e)
        {
            sbmnuReportsVCount.BackgroundImage = global::taqebostan.Properties.Resources.sbmnu_reports_vcount_up;
        }

        private void sbmnuReportsVCount_MouseUp(object sender, MouseEventArgs e)
        {
            sbmnuReportsVCount.BackgroundImage = global::taqebostan.Properties.Resources.sbmnu_reports_vcount_over;
        }

        private void sbmnuReportsVCount_Click(object sender, EventArgs e)
        {
            rptTarget = "pagesfa";
            SendRequest("ReportsPagesViewCount");
        }


        private void sbmnuReportsVCountEn_Click(object sender, EventArgs e)
        {
            rptTarget = "pagesen";
            SendRequest("ReportsPagesViewCount");
        }

        private void sbmnuReportsVCountEn_MouseDown(object sender, MouseEventArgs e)
        {
            sbmnuReportsVCountEn.BackgroundImage = global::taqebostan.Properties.Resources.sbmnu_reports_vcount_down_en;
        }

        private void sbmnuReportsVCountEn_MouseEnter(object sender, EventArgs e)
        {
            sbmnuReportsVCountEn.BackgroundImage = global::taqebostan.Properties.Resources.sbmnu_reports_vcount_over_en;
        }

        private void sbmnuReportsVCountEn_MouseLeave(object sender, EventArgs e)
        {
            sbmnuReportsVCountEn.BackgroundImage = global::taqebostan.Properties.Resources.sbmnu_reports_vcount_up_en;
        }

        private void sbmnuReportsVCountEn_MouseUp(object sender, MouseEventArgs e)
        {
            sbmnuReportsVCountEn.BackgroundImage = global::taqebostan.Properties.Resources.sbmnu_reports_vcount_over_en;
        }

        #endregion

        #region Generate Report Section

        private bool CleanReports()
        {
            try
            {
                string sqlStr = "SELECT * FROM PageRanks";

                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                oda.Fill(ds, "PageRanks");
                dt = ds.Tables["PageRanks"];

                int id = -1;
                while (drr.Read())
                {
                    dr = dt.Rows[++id];
                    dr.Delete();
                }

                oda.DeleteCommand = ocb.GetDeleteCommand();

                if (oda.Update(ds, "PageRanks") == 1)
                    ds.AcceptChanges();
                else
                    ds.RejectChanges();

                sqlStr = null;

                cnn.Close();
                drr.Close();

                dt.Dispose();
                ds.Dispose();
                cmd.Dispose();
                drr.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();

                dt = null;
                ds = null;
                cmd = null;
                drr = null;
                ocb = null;
                oda = null;
                cnn = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(errReport + e.Message, errReportHeader, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                return false;
            }
            finally
            {
            }
            return true;
        }

        private bool GetPagesRank(DataTable rptDT)
        {
            if (!CleanReports())
                return false;

            try
            {
                //DataTable rptDT = rptDS.Tables[0];
                DataRow rptDR;

                string sqlStr = "SELECT * FROM PageRanks";

                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                oda.Fill(ds, "PageRanks");
                dt = ds.Tables["PageRanks"];

                for (int i = 0; i < rptDT.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    rptDR = rptDT.Rows[i];
                    dr[0] = rptDR[0];
                    dr[1] = rptDR[1];
                    dt.Rows.Add(dr);
                }

                oda.DeleteCommand = ocb.GetDeleteCommand();

                if (oda.Update(ds, "PageRanks") == 1)
                    ds.AcceptChanges();
                else
                    ds.RejectChanges();

                sqlStr = null;

                cnn.Close();

                dt.Dispose();
                ds.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();

                dt = null;
                ds = null;
                ocb = null;
                oda = null;
                cnn = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(errReport + e.Message, errReportHeader, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                return false;
            }
            finally
            {
            }
            return true;
        }

        #endregion

        #region Page Editor Section

        private void editor_Tick()
        {
            mItemUndo.Enabled = editor.CanUndo();
            mItemRedo.Enabled = editor.CanRedo();
            mItemCut.Enabled = editor.CanCut();
            mItemCopy.Enabled = editor.CanCopy();
            mItemPaste.Enabled = editor.CanPaste();
            if (editor.CanUndo())
                hasChanged = true;
            else
                hasChanged = false;
        }

        public void SetEditorMnuState(bool state)
        {
            mItemClose.Enabled = state;
            mItemSave.Enabled = state;
            mItemPrint.Enabled = state;
            mItemEdit.Enabled = state;
            mItemView.Enabled = state;
            mItemInsert.Enabled = state;
            editor.Enabled2 = state;
            mItemPagesManagementEn.Enabled = !state;
            mItemPagesManagementFa.Enabled = !state;
        }

        private void DocClose()
        {
            editor.Document.Write(string.Empty);
            editor.DocumentText = null;
            editor.Tick += null;
            SetEditorMnuState(false);
        }

        private void mItemSave_Click(object sender, EventArgs e)
        {
            SendRequest("SetServerPage");
        }

        private void mItemClose_Click(object sender, EventArgs e)
        {
            if (!StayForChanges())
            {
                DocClose();
            }
        }

        private void mItemPrint_Click(object sender, EventArgs e)
        {
            editor.Print();
        }

        private void mItemUndo_Click(object sender, EventArgs e)
        {
            editor.Undo();
        }

        private void mItemRedo_Click(object sender, EventArgs e)
        {
            editor.Redo();
        }

        private void mItemCut_Click(object sender, EventArgs e)
        {
            editor.Cut();
        }

        private void mItemCopy_Click(object sender, EventArgs e)
        {
            editor.Copy();
        }

        private void mItemPaste_Click(object sender, EventArgs e)
        {
            editor.Paste();
        }

        private void mItemSelectAll_Click(object sender, EventArgs e)
        {
            editor.SelectAll();
        }

        private void mItemFind_Click(object sender, EventArgs e)
        {
            using (SearchDialog dlg = new SearchDialog(editor))
            {
                dlg.ShowDialog(this);
            }
        }

        private void mItemTextView_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, editor.BodyText);
        }

        private void mItemHtmlView_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, editor.BodyHtml);
        }

        private void mItemInsertBreak_Click(object sender, EventArgs e)
        {
            editor.InsertBreak();
        }

        private void mItemInsertHTML_Click(object sender, EventArgs e)
        {
            using (TextInsertForm form = new TextInsertForm(editor.BodyHtml))
            {
                form.ShowDialog(this);
                if (form.Accepted)
                {
                    editor.BodyHtml = form.HTML;
                }
            }
        }

        private void mItemInsertParagraph_Click(object sender, EventArgs e)
        {
            editor.InsertParagraph();
        }

        private void frmPageEditor_Load(object sender, EventArgs e)
        {
            SetEditorMnuState(false);
        }

        private void ServerPageGet(object sender, EventArgs e)
        {
            if (!StayForChanges())
            {
                DocClose();

                ToolStripMenuItem mnu = (ToolStripMenuItem)sender;
                string tag = (string)mnu.Tag;
                int pos = tag.IndexOf("==>");
                lang = tag.Substring(0, pos);
                wPage = tag.Substring(pos + 3);
                SendRequest("GetServerPage");
            }
        }

        private void GetContactList(object sender, EventArgs e)
        {
            if (!StayForChanges())
            {
                DocClose();

                ToolStripMenuItem mnu = (ToolStripMenuItem)sender;
                wList = (string)mnu.Tag;

                SendRequest("ContactList");
            }
        }

        private void RootMessage(object sender, EventArgs e)
        {
            MessageBox.Show("کاربر گرامی منوهای اصلی قادر به نگهداری اطلاعات نمی باشند\n\nلطفا برای آن زیر منو ایجاد نمائید", msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        #endregion

        #region Gallery Section

        private void ClearGallery()
        {
            hasChanged = true;
            lstvGallery.Items.Clear();
            imglstGallery.Images.Clear();
            Array.Resize(ref galleryFiles, 0);

            FormGalleryChErAllRep();
        }

        private void FormGallery(bool state)
        {
            btnGallerySave.Enabled = state;
            btnGalleryCancel.Enabled = state;
            btnGalleryAdd.Enabled = state;
            cmiGalleryAddFolder.Enabled = state;
            btnGalleryAddFiles.Enabled = state;
            cmiGalleryAddFiles.Enabled = state;
            btnGalleryReplace.Enabled = false;
            cmiGalleryReplaceFolder.Enabled = false;
            cmiGalleryReplaceFiles.Enabled = false;
            btnGalleryErase.Enabled = false;
            cmiGalleryErase.Enabled = false;
            btnGalleryEraseAll.Enabled = false;
            cmiGalleryEraseAll.Enabled = false;
            cmiGallerySelectAll.Enabled = false;

            if (state)
            {
                btnFetchGallery.Enabled = false;

                btnGallerySave.Enabled = false;

/*                if (lstvGallery.Items.Count <= 0)
                {
                    btnGalleryReplace.Enabled = false;
                    cmiGalleryReplaceFolder.Enabled = false;
                    cmiGalleryReplaceFiles.Enabled = false;
                    cmiGallerySelectAll.Enabled = false;
                }*/
            }
            else
            {
                btnFetchGallery.Enabled = true;
                ClearGallery();
            }

            lstvGallery.Enabled = state;

            hasChanged = false;

            if (galleryServerFiles.Length > 0)
            {
                try
                {
                    foreach (string f in galleryServerFiles)
                    {
                        File.Delete(f);
                    }
                    Directory.Delete(tmpPath);
                }
                catch { }
                finally { }

                tmpPath = string.Empty;
                Array.Resize(ref galleryServerFiles, 0);
            }
        }

        private void FormGalleryChErAllRep(bool state)
        {
            btnGalleryEraseAll.Enabled = state;
            cmiGalleryEraseAll.Enabled = state;
            btnGalleryReplace.Enabled = state;
            cmiGalleryReplaceFolder.Enabled = state;
            cmiGalleryReplaceFiles.Enabled = state;
            cmiGallerySelectAll.Enabled = state;
        }

        private void FormGalleryChErAllRep()
        {
            if (lstvGallery.Items.Count > 0)
                FormGalleryChErAllRep(true);
            else
                FormGalleryChErAllRep(false);
        }

        private void btnFetchGallery_Click(object sender, EventArgs e)
        {
            FormGallery(true);
            SendRequest("GalleryImagesList");
        }

        private void FormGalleryChEr(bool state)
        {
            btnGalleryErase.Enabled = state;
            cmiGalleryErase.Enabled = state;
        }

        private void lstvGallery_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstvGallery.SelectedItems.Count > 0)
                FormGalleryChEr(true);
            else
                FormGalleryChEr(false);
        }

        private void GalleryChanged(bool flag)
        {
            btnGallerySave.Enabled = flag;
        }

        private bool GalleryChanged()
        {
            return btnGallerySave.Enabled;
        }

        private void AddImagesToGallery(string op)
        {
            try
            {
                DialogResult result;

                if (op != "FromServer")
                {
                    if (op != "AddFiles" && op != "ReplaceFiles")
                    {
                        if (op == "ReplaceFolder")
                            if (MessageBox.Show("آيا مايل به حذف تمامي تصاوير و جايگريني آن با يك پوشه مي باشيد؟", msgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                                return;
                        result = fbd.ShowDialog(this.Parent);
                    }
                    else
                    {
                        if (op == "ReplaceFolder")
                            if (MessageBox.Show("آيا مايل به حذف تمامي تصاوير و جايگريني آن با تصاوير جديد مي باشيد؟", msgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                                return;
                        ofd.Filter = "All Web Images Format | *.png;*.jpg;*.gif; | All Files (*.*) | *.*";
                        ofd.Multiselect = true;
                        result = ofd.ShowDialog(this.Parent);
                    }
                }
                else
                    result = DialogResult.OK;

                if (result == DialogResult.OK)
                {
                    string[] files = { };

                    if (op != "FromServer")
                    {
                        if (op != "AddFiles")
                        {
                            if (op == "ReplaceFolder" || op == "ReplaceFiles")
                                ClearGallery();

                            string path = fbd.SelectedPath;
                            path += path.EndsWith("\\") ? string.Empty : "\\";

                            files = Directory.GetFiles(path);
                        }
                        else
                        {
                            files = ofd.FileNames;
                        }
                    }
                    else
                        files = galleryServerFiles;

                    ListViewItem[] items = { };
                    int len = -1;
                    bool hasPic = false;
                    int dot = -1;
                    string ext = string.Empty;

                    foreach (string file in files)
                    {
                        if (op != "AddFiles")
                        {
                            dot = file.LastIndexOf(".");
                            ext = dot > -1 ? file.Substring(dot) : string.Empty;
                        }

                        if (ext == ".png" || ext == ".jpg" || ext == ".gif" || op == "AddFiles")
                        {
                            imglstGallery.Images.Add(Bitmap.FromFile(file));

                            len = items.Length;
                            Array.Resize(ref items, len + 1);
                            items[len] = new ListViewItem("", imglstGallery.Images.Count - 1);

                            len = galleryFiles.Length;
                            Array.Resize(ref galleryFiles, len + 1);
                            galleryFiles[len] = file;

                            hasPic = true;
                        }
                    }

                    if (hasPic)
                    {
                        hasChanged = true;
                        lstvGallery.Items.AddRange(items);
                        FormGalleryChErAllRep();
                        GalleryChanged(true);
                    }
                    else
                        MessageBox.Show(errGalleryNoPic);

                    if (op == "FromServer") {
                        GalleryChanged(false);
                        hasChanged = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(errPrefix + ex.Message, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void btnGalleryAdd_Click(object sender, EventArgs e)
        {
            AddImagesToGallery("AddFolder");
        }

        private void btnGalleryReplace_Click(object sender, EventArgs e)
        {
            AddImagesToGallery("ReplaceFolder");
        }

        private void btnGalleryAddFiles_Click(object sender, EventArgs e)
        {
            AddImagesToGallery("AddFiles");
        }

        private void DoGalleryErase()
        {
            if (MessageBox.Show("آيا مايل به حذف موارد انتخابي مي باشيد؟", msgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                hasChanged = true;
                foreach (int index in lstvGallery.SelectedIndices)
                    galleryFiles[index] = null;

                string[] tempFiles = galleryFiles;

                Array.Resize(ref galleryFiles, 0);
                int len = -1;

                foreach (string t in tempFiles)
                {
                    if (t != null)
                    {
                        len = galleryFiles.Length;
                        Array.Resize(ref galleryFiles, len + 1);

                        galleryFiles[len] = t;
                    }
                }

                foreach (ListViewItem item in lstvGallery.SelectedItems)
                    lstvGallery.Items.Remove(item);

                FormGalleryChErAllRep();

                GalleryChanged(true);
            }
        }

        private void btnGalleryErase_Click(object sender, EventArgs e)
        {
            DoGalleryErase();
        }

        private void DoGalleryEraseAll()
        {
            if (MessageBox.Show("آيا مايل به حذف تمامي تصاوير مي باشيد؟", msgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                ClearGallery();
                GalleryChanged(true);
            }
        }

        private void btnGalleryEraseAll_Click(object sender, EventArgs e)
        {
            DoGalleryEraseAll();
        }


        private void btnGallerySave_Click(object sender, EventArgs e)
        {
            SendRequest("GalleryCatchChanges");
        }

        private void btnGalleryCancel_Click(object sender, EventArgs e)
        {
            if (GalleryChanged())
            {
                if (MessageBox.Show("آيا مايل به لغو تغييرات مي باشيد؟", msgTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    FormGallery(false);
                    GalleryChanged(false);
                }
            }
            else
            {
                FormGallery(false);
            }
        }

        private void cmiGallerySelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstvGallery.Items.Count; i++)
                lstvGallery.SelectedIndices.Add(i);
        }

        private void cmiGalleryAddFolder_Click(object sender, EventArgs e)
        {
            AddImagesToGallery("AddFolder");
        }

        private void cmiGalleryAddFiles_Click(object sender, EventArgs e)
        {
            AddImagesToGallery("AddFiles");
        }

        private void cmiGalleryReplaceFolder_Click(object sender, EventArgs e)
        {
            AddImagesToGallery("ReplaceFolder");
        }

        private void cmiGalleryReplaceFiles_Click(object sender, EventArgs e)
        {
            AddImagesToGallery("ReplaceFiles");
        }

        private void cmiGalleryErase_Click(object sender, EventArgs e)
        {
            DoGalleryErase();
        }

        private void cmiGalleryEraseAll_Click(object sender, EventArgs e)
        {
            DoGalleryEraseAll();
        }
        #endregion

        #region Nodes Manager

        private void ShowNodes(string lang)
        {
            frmNodes frm = new frmNodes();
            frm.LangSelect = lang;
            frm.ShowDialog();
            RegenMenues();
        }

        private void mItemPagesManagementFa_Click(object sender, EventArgs e)
        {
            ShowNodes("fa");
        }

        private void mItemPagesManagementEn_Click(object sender, EventArgs e)
        {
            ShowNodes("en");
        }

        #endregion

        #region Draw Menues

        private void RegenMenues()
        {
            SendRequest("NodesAllTrees");
        }

        private void ClearMenues(ToolStripMenuItem menu)
        {
            for (int i = menu.DropDownItems.Count - 4; i > 0; i--)
                menu.DropDownItems.RemoveAt(i);
        }

        private void RegenMenu(ToolStripMenuItem menu, DataTable dt, string rootString)
        {
            ClearMenues(menu);
            TreeView trv = new TreeView();
            TreeNode root = new TreeNode(rootString);
            root.Name = "root";
            trv.Nodes.Add(root);
            DrawMenues(menu, trv.Nodes["root"], dt, 1);
            SetMenuEvents(menu, dt.TableName);
        }

        private void DrawMenues(ToolStripMenuItem mItem, TreeNode node, DataTable dt, int rootIndex)
        {
            TreeNodeCollection nodes = node.Nodes;
            DataRow dr;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                string name = dr["pg"].ToString().Trim();

                if (dr["parent"].ToString().Trim() == node.Name && dr["fullpath"].ToString().Trim() == node.FullPath + "\\" + name)
                {
                    TreeNode tn = new TreeNode();
                    tn.Name = name;
                    tn.Text = name;
                    nodes.Add(tn);

                    ToolStripMenuItem newItem = new ToolStripMenuItem(name);

                    if (dr["parent"].ToString().Trim() != "root")
                    {
                        mItem.DropDownItems.Add(newItem);
                        newItem.Tag = "{lang}==>" + dr["fullpath"].ToString().Trim();
                    }
                    else
                    {
                        mItem.DropDownItems.Insert(rootIndex, newItem);
                        ++rootIndex;
                        newItem.Tag = "root";
                    }

                    DrawMenues(newItem, tn, dt, rootIndex);
                }
            }
        }

        private void SetMenuEvents(ToolStripMenuItem mItem, string lang)
        {
            ToolStripItemCollection menues = mItem.DropDownItems;

            foreach (ToolStripMenuItem m in menues)
            {
                string tag = ((string)m.Tag);
                if (m.DropDownItems.Count < 1 && tag.Contains("{lang}"))
                {
                    m.Tag = tag.Replace("{lang}", lang);
                    m.Click += new System.EventHandler(this.ServerPageGet);
                }
                else if (m.DropDownItems.Count < 1 && tag.Contains("root"))
                {
                    m.Click += new System.EventHandler(this.RootMessage);
                }

                SetMenuEvents(m, lang);
            }
        }

        #endregion
    }
}