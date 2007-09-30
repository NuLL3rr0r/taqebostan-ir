using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web.Services.Protocols;

namespace taqebostan
{
    public partial class frmNodes : FormBase
    {
        #region Global Variables & Properties

        private string legal = "coLorado1963";

        private taq.Management wsrv = new taq.Management();
        private frmLoading frmLoading = new frmLoading();

        private TreeNode nodeServer = new TreeNode();
        private string nodeServerNew = string.Empty;
        private TreeNode nodeServerParent = new TreeNode();
        private string nodeServerLang = string.Empty;
        private int nodeServerZIndex = -1;
        private string nodeServerParentPath = string.Empty;
        private TreeNode nodeServerBeside = new TreeNode();

        private string errDuplicateNode = "صفحه اي با نام تعيين شده در سطح جاري قبلا ثبت شده است";
        private string errDuplicateNodeHeader = "صفحه تكراري";
        private string errOverflow = "حداکثر طول مجاز برای هر شاخه ۲۵۵ حرف می باشد";
        private string errOverflowHeader = "سرریز در ساختار درختی";
        private string errInvalidChar = "در نام گذاری از کاراکتر غیر مجاز استفاده نموده اید";
        private string errInvalidCharHeader = "تشخیص کاراکتر غیر مجاز";
        private string errServer = "امكان دسترسي به وب سرور به دليل خطاي ذيل وجود ندارد";
        private string errServerHeader = "خطا در اتصال به سايت";

        private string msgTitle = "taqebostan.ir CMS v1.0";
        private string errPrefix = "Error:\n\t";
        private string errDSReject = "سرور قادر به بروز رساني اطلاعات ورودي نمي باشد";

        private const string srvMsgInvalidLegal = "Illegal Access...";
        private const string srvMsgDSReject = "Rejected";

        private string _rootTitle = string.Empty;
        private string _tblPages = string.Empty;
        private string _lang = string.Empty;

        private string rootTitleFa = "منوهاي وب سايت";
        private string rootTitleEn = "Website Menus";

        private static string urlPages = "http://www.taqebostan.ir/?lang={0}&req={1}&{2}={3}";

        private string urlHashKey = "§";

        public string LangSelect
        {
            set
            {
                if (value == "fa")
                {
                    _rootTitle = rootTitleFa;
                    _tblPages = "pagesfa";
                    _lang = "fa";
                    trvPages.RightToLeft = RightToLeft.Yes;
                    trvPages.RightToLeftLayout = true;
                    SetRootNode(trvPages, _rootTitle);
                }
                else if (value == "en")
                {
                    _rootTitle = rootTitleEn;
                    _tblPages = "pagesen";
                    _lang = "en";
                    trvPages.RightToLeft = RightToLeft.No;
                    trvPages.RightToLeftLayout = false;
                    SetRootNode(trvPages, _rootTitle);
                }
            }
        }

        #endregion

        public frmNodes()
        {
            Draggable = true;
            ExcludeList = "trvPages, cboxClose";

            InitializeComponent();

            wsrv.NodesAllCompleted += new taq.NodesAllCompletedEventHandler(NodesAllCompleted);
            wsrv.NodesAddCompleted += new taq.NodesAddCompletedEventHandler(NodesAddCompleted);
            wsrv.NodesEditCompleted += new taq.NodesEditCompletedEventHandler(NodesEditCompleted);
            wsrv.NodesEraseCompleted += new taq.NodesEraseCompletedEventHandler(NodesEraseCompleted);
            wsrv.NodesChangeIndexCompleted += new taq.NodesChangeIndexCompletedEventHandler(NodesChangeIndexCompleted);
        }

        #region Setting Default Values

        private void SetRootNode(TreeView trv, string root)
        {
            TreeNode rootNode = new TreeNode();
            rootNode.Name = "root";
            rootNode.Text = root;
            trv.Nodes.Add(rootNode);
        }

        #endregion

        #region Correct Contex Menu

        private void CheckStatus(TreeView trv)
        {
            if (trv.SelectedNode.Name == "root")
            {
                mItemErase.Enabled = false;
                mItemRename.Enabled = false;
                mItemMoveUp.Enabled = false;
                mItemMoveDown.Enabled = false;
                mItemCopyURL.Enabled = false;
            }
            else
            {
                mItemErase.Enabled = true;
                mItemRename.Enabled = true;
                mItemCopyURL.Enabled = trv.SelectedNode.Nodes.Count == 0 ? true : false;

                if (trv.SelectedNode.Index != 0)
                {
                    mItemMoveUp.Enabled = true;
                }
                else
                {
                    mItemMoveUp.Enabled = false;
                }

                if (trv.SelectedNode.Index + 1 != trv.SelectedNode.Parent.Nodes.Count)
                {
                    mItemMoveDown.Enabled = true;
                }
                else
                {
                    mItemMoveDown.Enabled = false;
                }
            }
        }

        private void ctxNodeManager_Opening(object sender, CancelEventArgs e)
        {
            CheckStatus(trvPages);
        }

        private void trvPages_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            trvPages.SelectedNode = e.Node;
            CheckStatus(trvPages);
        }

        #endregion

        #region Add

        private void AddNode(TreeView trv, string lang)
        {
            using (frmTinyInputBox frm = new frmTinyInputBox())
            {
                frm.title = "افزودن صفحه";
                while (true)
                {
                    frm.ShowDialog(this);

                    if (frm.node != string.Empty)
                    {
                        TreeNode node = new TreeNode();
                        node.Name = frm.node;
                        node.Text = frm.node;

                        bool found = false;

                        if (frm.node.Contains("\\") || frm.node.Contains("/"))
                        {
                            MessageBox.Show(errInvalidChar, errInvalidCharHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                            continue;
                        }

                        if ((trv.SelectedNode.FullPath + "\\" + node.Name).Length > 255)
                        {
                            MessageBox.Show(errOverflow, errOverflowHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                            continue;
                        }

                        foreach (TreeNode tn in trv.SelectedNode.Nodes)
                        {
                            if (tn.Name == node.Name)
                            {
                                MessageBox.Show(errDuplicateNode, errDuplicateNodeHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                                found = true;
                                break;
                            }
                        }

                        if (found)
                            continue;

                        if ((trv.SelectedNode.FullPath + "\\" + node.Name).Length > 255)
                        {
                            MessageBox.Show(errOverflow, errOverflowHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                            continue;
                        }

                        nodeServer = node;
                        nodeServerParent = trv.SelectedNode;
                        nodeServerLang = lang;
                        nodeServerZIndex = trv.SelectedNode.Nodes.Count;
                        SendRequest("NodesAdd");
                        break;
                    }
                    else
                        break;
                }
            }
        }

        private void mItemInsert_Click(object sender, EventArgs e)
        {
            AddNode(trvPages, _tblPages);
        }

        #endregion

        #region Edit

        private void EditNode(TreeView trv, string lang)
        {
            if (trv.SelectedNode.Name != "root")
            {
                using (frmTinyInputBox frm = new frmTinyInputBox())
                {
                    frm.title = "ويرايش نام صفحه";
                    while (true)
                    {
                        frm.node = trv.SelectedNode.Text;
                        frm.ShowDialog(this);

                        if (frm.node != string.Empty)
                        {
                            DialogResult result = MessageBox.Show("آيا مايل به تغيير نام صفحه موردنظر مي باشيد", "ويرايش نام صفحه", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                            if (result == DialogResult.OK)
                            {
                                if (frm.node.Contains("\\") || frm.node.Contains("/"))
                                {
                                    MessageBox.Show(errInvalidChar, errInvalidCharHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                                    continue;
                                }

                                bool found = false;

                                foreach (TreeNode tn in trv.SelectedNode.Parent.Nodes)
                                {
                                    if (tn.Name == frm.node && frm.node != trv.SelectedNode.Name)
                                    {
                                        MessageBox.Show(errDuplicateNode, errDuplicateNodeHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                                        found = true;
                                        break;
                                    }
                                }

                                if (found)
                                    continue;

                                if ((trv.SelectedNode.Parent.FullPath + "\\" + frm.node).Length > 255)
                                {
                                    MessageBox.Show(errOverflow, errOverflowHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                                    continue;
                                }

                                nodeServer = trv.SelectedNode;
                                nodeServerParent = trv.SelectedNode.Parent;
                                nodeServerLang = lang;
                                nodeServerNew = frm.node;
                                SendRequest("NodesEdit");
                            }

                            break;
                        }
                        else
                            break;
                    }
                }
            }
        }

        private void mItemRename_Click(object sender, EventArgs e)
        {
            EditNode(trvPages, _tblPages);
        }

        #endregion

        #region Remove

        private void RemoveNode(TreeView trv, string lang)
        {
            if (trv.SelectedNode.Name != "root")
            {
                DialogResult result = MessageBox.Show("آيا مايل به حذف صفحه موردنظر با تمامي اطلاعات آن مي باشيد\n\nدقت نمائيد كه پس از حذف هيچگونه امكان بازگشتي وجود ندارد", "حذف صفحه", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                if (result == DialogResult.OK)
                {
                    nodeServer = trv.SelectedNode;
                    nodeServerParent = trv.SelectedNode.Parent;
                    nodeServerLang = lang;
                    SendRequest("NodesErase");
                }
            }
        }


        private void mItemErase_Click(object sender, EventArgs e)
        {
            RemoveNode(trvPages, _tblPages);
        }

        #endregion

        #region UP / Down

        private void MoveUpNode(TreeView trv, string lang)
        {
            if (trv.SelectedNode.Name != "root")
            {
                if (trv.SelectedNode.Index != 0)
                {
                    /*TreeNode parent = trv.SelectedNode.Parent;
                    TreeNode node = trv.SelectedNode;
                    parent.Nodes.Remove(node);
                    parent.Nodes.Insert(node.Index - 1, node);

                    trv.SelectedNode = node;*/

                    nodeServer = trv.SelectedNode;
                    nodeServerBeside = trv.SelectedNode.Parent.Nodes[trv.SelectedNode.Index - 1];
                    nodeServerLang = lang;

                    SendRequest("NodesChangeIndex");
                }
            }
        }

        private void MoveDownNode(TreeView trv, string lang)
        {
            if (trv.SelectedNode.Name != "root")
            {
                if (trv.SelectedNode.Index + 1 != trv.SelectedNode.Parent.Nodes.Count)
                {
                    /*TreeNode parent = trv.SelectedNode.Parent;
                    TreeNode node = trv.SelectedNode;
                    parent.Nodes.Remove(node);
                    parent.Nodes.Insert(node.Index + 1, node);

                    trv.SelectedNode = node;*/

                    nodeServer = trv.SelectedNode;
                    nodeServerBeside = trv.SelectedNode.Parent.Nodes[trv.SelectedNode.Index + 1];
                    nodeServerLang = lang;

                    SendRequest("NodesChangeIndex");
                }
            }
        }

        private void mItemMoveUp_Click(object sender, EventArgs e)
        {
            MoveUpNode(trvPages, _tblPages);
        }

        private void mItemMoveDown_Click(object sender, EventArgs e)
        {
            MoveDownNode(trvPages, _tblPages);
        }

        #endregion

        #region Copy URL

        private void CopyURL(string req, string var, string value, string lang)
        {
            // use - and _ indtead of + and / ;;;;;;;;;; leave =
            // or escape it
            //using array for var/value
            //?lang={0}&req={1}&{2}={3}
            Clipboard.SetText(string.Format(urlPages, lang, req, var, EncDec.Encrypt(value, urlHashKey).Replace("+", "%2B").Replace("/", "%2F").Replace("=", "%3D")), TextDataFormat.UnicodeText);
        }

        private void CopyURL(TreeView trv, string lang)
        {
            CopyURL("fetchpage", "page", trv.SelectedNode.FullPath.Replace("\\", "/"), lang);
        }

        private void mItemCopyURL_Click(object sender, EventArgs e)
        {
            CopyURL(trvPages, _lang);
        }

        #endregion

        #region loading

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

        #endregion

        #region Form Operations

        private void DoExit()
        {
            Environment.Exit(0);
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

        private void frmNodes_Shown(object sender, EventArgs e)
        {
            SendRequest("NodesAll");
        }

        private void frmNodes_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region AsyncCalls

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
                    Loading(false);
                    break;
                default:
                    break;
            }
        }

        private void SendRequest(string req)
        {
            try
            {
                switch (req)
                {
                    case "NodesAll":
                        wsrv.NodesAllAsync(_tblPages, legal);
                        break;
                    case "NodesAdd":
                        wsrv.NodesAddAsync(nodeServer.Name, nodeServerParent.Name, nodeServerParent.FullPath + "\\" + nodeServer.Name, nodeServerZIndex, nodeServerLang, legal);
                        break;
                    case "NodesEdit":
                        wsrv.NodesEditAsync(nodeServer.Name, nodeServerNew, nodeServer.FullPath, nodeServerParent.FullPath + "\\" + nodeServerNew, nodeServerLang, legal);
                        break;
                    case "NodesErase":
                        wsrv.NodesEraseAsync(nodeServer.FullPath, nodeServerParent.FullPath, nodeServerLang, legal);
                        break;
                    case "NodesChangeIndex":
                        wsrv.NodesChangeIndexAsync(nodeServer.FullPath, nodeServerBeside.Index, nodeServerBeside.FullPath, nodeServer.Index, nodeServerLang, legal);
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

        #endregion

        #region AsyncCalls Response

        private void NodesAllCompleted(Object sender, taq.NodesAllCompletedEventArgs Completed)
        {
            try
            {
                DataTable dt = Completed.Result;

                SetNodes(trvPages.Nodes["root"], dt);

                Loading(false);
            }
            catch (SoapException ex)
            {
                TryRequest("NodesAll", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("NodesAll", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void NodesAddCompleted(Object sender, taq.NodesAddCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;
                switch (result)
                {
                    case "Added":
                        nodeServerParent.Nodes.Add(nodeServer);
                        nodeServerParent.ExpandAll();
                        Loading(false);
                        break;
                    case "Already Exist":
                        MessageBox.Show(errDuplicateNode, errDuplicateNodeHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        Loading(false);
                        break;
                    case srvMsgDSReject:
                        TryRequest("NodesAdd", errDSReject);
                        break;
                    case srvMsgInvalidLegal:
                        MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DoExit();
                        break;
                    default:
                        TryRequest("NodesAdd", result);
                        break;
                }
            }
            catch (SoapException ex)
            {
                TryRequest("NodesAdd", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("NodesAdd", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void NodesEditCompleted(Object sender, taq.NodesEditCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;
                switch (result)
                {
                    case "Updated":
                        nodeServer.Text = nodeServerNew;
                        nodeServer.Name = nodeServerNew;

                        nodeServer.ExpandAll();

                        Loading(false);
                        break;
                    case "Not Found":
                        MessageBox.Show("صفحه ای با نام موردنظر جهت ویرایش یافت نشد", msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        Loading(false);
                        break;
                    case "Duplicate Error":
                        MessageBox.Show(errDuplicateNode, errDuplicateNodeHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        Loading(false);
                        break;
                    case srvMsgDSReject:
                        TryRequest("NodesEdit", errDSReject);
                        break;
                    case srvMsgInvalidLegal:
                        MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DoExit();
                        break;
                    default:
                        TryRequest("NodesEdit", result);
                        break;
                }
            }
            catch (SoapException ex)
            {
                TryRequest("NodesEdit", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("NodesEdit", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void NodesEraseCompleted(Object sender, taq.NodesEraseCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;
                switch (result)
                {
                    case "Erased":
                        nodeServer.Remove();
                        Loading(false);
                        break;
                    case "Not Found":
                        MessageBox.Show("صفحه ای با نام موردنظر جهت حذف یافت نشد", msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        Loading(false);
                        break;
                    case srvMsgDSReject:
                        TryRequest("NodesErase", errDSReject);
                        break;
                    case srvMsgInvalidLegal:
                        MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DoExit();
                        break;
                    default:
                        TryRequest("NodesErase", result);
                        break;
                }
            }
            catch (SoapException ex)
            {
                TryRequest("NodesErase", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("NodesErase", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        private void NodesChangeIndexCompleted(Object sender, taq.NodesChangeIndexCompletedEventArgs Completed)
        {
            try
            {
                string result = Completed.Result;
                switch (result)
                {
                    case "ReIndexed":
                        TreeView trv = new TreeView();

                        trv = trvPages;

                        int index = nodeServerBeside.Index;

                        TreeNode parent = trv.SelectedNode.Parent;
                        TreeNode node = trv.SelectedNode;
                        parent.Nodes.Remove(node);
                        parent.Nodes.Insert(index, node);
                        trv.SelectedNode = node;

                        Loading(false);
                        break;
                    case "Not Found":
                        MessageBox.Show("صفحه ای با نام موردنظر جهت تغییر موقعیت یافت نشد", msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        Loading(false);
                        break;
                    case srvMsgDSReject:
                        TryRequest("NodesChangeIndex", errDSReject);
                        break;
                    case srvMsgInvalidLegal:
                        MessageBox.Show(errPrefix + result, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DoExit();
                        break;
                    default:
                        TryRequest("NodesChangeIndex", result);
                        break;
                }
            }
            catch (SoapException ex)
            {
                TryRequest("NodesChangeIndex", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                TryRequest("NodesChangeIndex", ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
            }
        }

        #endregion

        #region Nodes

        private void GetNodes(TreeNode node, DataTable dt)
        {
            TreeNodeCollection nodes = node.Nodes;
            DataRow dr;

            foreach (TreeNode n in nodes)
            {
                dr = dt.NewRow();
                dr["pg"] = n.Name;
                dr["parent"] = n.Parent.Name;
                dr["fullpath"] = n.FullPath;
                dt.Rows.Add(dr);
                GetNodes(n, dt);
            }
        }

        private void SetNodes(TreeNode node, DataTable dt)
        {
            TreeNodeCollection nodes = node.Nodes;
            DataRow dr;

            dt.DefaultView.Sort = "[fullpath] asc";

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
                    node.ExpandAll();
                    SetNodes(tn, dt);
                }
            }
        }

        #endregion
    }
}
