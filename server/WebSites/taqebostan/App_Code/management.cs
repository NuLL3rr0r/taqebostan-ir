using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Net.Mail;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for Management
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Management : System.Web.Services.WebService
{

    private string path;
    private string fileDb = @"oracledb\master.oracle";
    private string picsDb = @"oracledb\pics.oracle";

    private string dBpw = "LJ1GL54gHP8X0bUiN0cy";

    private string hashKey = "g0t0Z86CkMxlV430GpXNNea";

    private string cnnStr;
    private string cnnStrPics;

    private string srvMsgErr = "err:";
    private string srvMsgSuccess = "res:";
    //private int srvMsgLen = 4;
    private string tLegal;

    private string errInvalidLegal = "Illegal Access...";

    private string[] pgImages = { };

    public static string rootTitleFa = "منوهاي وب سايت";
    public static string rootTitleEn = "Website Menus";


    public Management()
    {
        Server.ScriptTimeout = 2147483647;

        path = Server.MapPath("~");
        path += path.EndsWith("\\") ? string.Empty : "\\";

        fileDb = String.Concat(path, fileDb);
        picsDb = String.Concat(path, picsDb);

        cnnStr = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};", fileDb, dBpw);
        cnnStrPics = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};", picsDb, dBpw);

        FillLegal();

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    #region Security

    private void FillLegal()
    {
        string tbl = "admin";
        string sqlStr = "SELECT * FROM " + tbl;

        try
        {
            OleDbConnection cnn = new OleDbConnection(cnnStr);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
            cnn.Open();
            OleDbDataReader drr = cmd.ExecuteReader();

            DataSet ds = new DataSet();

            oda.Fill(ds, "admin");

            while (drr.Read())
            {
                tLegal = EncDec.Decrypt(drr["legal"].ToString(), hashKey);
                break;
            }

            drr.Close();
            cnn.Close();

            cmd.Dispose();
            drr.Dispose();
            ds.Dispose();
            oda.Dispose();
            cnn.Dispose();

            cmd = null;
            drr = null;
            ds = null;
            oda = null;
            cnn = null;
        }
        catch
        {
        }
        finally
        {
            tbl = null;
            sqlStr = null;
        }
    }

    private bool IsDaysLeft()
    {
        bool result = false;
        string tbl = "admin";
        string sqlStr = "SELECT * FROM " + tbl;

        try
        {
            OleDbConnection cnn = new OleDbConnection(cnnStr);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
            cnn.Open();
            OleDbDataReader drr = cmd.ExecuteReader();

            DataSet ds = new DataSet();

            oda.Fill(ds, tbl);

            while (drr.Read())
            {
                if (EncDec.Decrypt(drr["daysleft"].ToString(), hashKey) != "0")
                    result = true;
                else
                    result = false;
                break;
            }

            drr.Close();
            cnn.Close();

            cmd.Dispose();
            drr.Dispose();
            ds.Dispose();
            oda.Dispose();
            cnn.Dispose();

            cmd = null;
            drr = null;
            ds = null;
            oda = null;
            cnn = null;
        }
        catch
        {
            result = false;
        }
        finally
        {
            tbl = null;
            sqlStr = null;
        }

        return result;
    }

/*
    private string[] LegalFilesList()
    {
        string[] fList = { };
        string tbl = "cpr";
        string sqlStr = "SELECT * FROM " + tbl;

        OleDbConnection cnn = new OleDbConnection(cnnStr);
        OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
        OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataRow dr;

        try
        {
            cnn.Open();
            ocb.QuotePrefix = "[";
            ocb.QuoteSuffix = "]";
            oda.Fill(ds, tbl);
            dt = ds.Tables[tbl];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                int len = fList.Length;
                Array.Resize(ref fList, len + 1);
                fList[len] = EncDec.Decrypt(dr["filename"].ToString(), hashKey);
            }
        }
        catch
        {
        }
        finally
        {
            sqlStr = null;

            cnn.Close();

            ds.Dispose();
            ocb.Dispose();
            oda.Dispose();
            cnn.Dispose();
            dt.Dispose();

            ds = null;
            ocb = null;
            oda = null;
            dr = null;
            dt = null;
            cnn = null;
        }

        return fList;
    }

    private bool CheckLegalContents(string filename)
    {
        string tbl = "cpr";
        string sqlStr = "SELECT * FROM " + tbl;

        OleDbConnection cnn = new OleDbConnection(cnnStr);
        OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
        OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataRow dr;

        try
        {
            cnn.Open();
            ocb.QuotePrefix = "[";
            ocb.QuoteSuffix = "]";
            oda.Fill(ds, tbl);
            dt = ds.Tables[tbl];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                if (EncDec.Decrypt(dr["filename"].ToString(), hashKey) == filename)
                {
                    string cprFilePath = path + filename;
                    string cprFile = filename;
                    string cprContent;
                    using (FileStream fs = new FileStream(cprFilePath, FileMode.Open))
                    {
                        int fileSize = (int)new FileInfo(cprFilePath).Length;
                        byte[] buffer = new byte[fileSize];
                        fs.Read(buffer, 0, fileSize);
                        fs.Close();
                        cprContent = Encoding.UTF8.GetString(buffer);
                    }

                    if (cprContent == EncDec.Decrypt(dr["content"].ToString(), hashKey))
                        return true;
                    else
                        return false;
                }
            }
        }
        catch
        {
            return false;
        }
        finally
        {
            sqlStr = null;

            cnn.Close();

            ds.Dispose();
            ocb.Dispose();
            oda.Dispose();
            cnn.Dispose();
            dt.Dispose();

            ds = null;
            ocb = null;
            oda = null;
            dr = null;
            dt = null;
            cnn = null;
        }

        return false;
    }

    private bool WriteLegalFile(string fileName)
    {
        string tbl = "cpr";
        string sqlStr = "SELECT * FROM " + tbl;

        OleDbConnection cnn = new OleDbConnection(cnnStr);
        OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
        OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataRow dr;

        try
        {
            cnn.Open();
            ocb.QuotePrefix = "[";
            ocb.QuoteSuffix = "]";
            oda.Fill(ds, tbl);
            dt = ds.Tables[tbl];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                if (EncDec.Decrypt(dr["filename"].ToString(), hashKey) == fileName)
                {
                    string cprFilePath = path + fileName;
                    string cprFile = fileName;

                    if (File.Exists(path + fileName))
                    {
                        File.SetAttributes(path + fileName, FileAttributes.Normal);
                        File.Delete(path + fileName);
                    }

                    using (StreamWriter sw = new StreamWriter(path + fileName))
                    {
                        sw.Write(EncDec.Decrypt(dr["content"].ToString(), hashKey));
                        sw.Close();
                    }
                }
            }
        }
        catch
        {
            return false;
        }
        finally
        {
            sqlStr = null;

            cnn.Close();

            ds.Dispose();
            ocb.Dispose();
            oda.Dispose();
            cnn.Dispose();
            dt.Dispose();

            ds = null;
            ocb = null;
            oda = null;
            dr = null;
            dt = null;
            cnn = null;
        }

        return true;
    }

    private bool ChkMyAds()
    {
        return true;

        try
        {
            string[] lstExt = new string[] { "html", "htm", "xml", "shtml", "mht", "xsd", "xsl", "xslt", "xul", "asmx", "asp", "aspx", "asbx", "ascx", "as", "asr", "asc", "php", "php3", "php4", "php5", "jsp", "cfm", "cfml", "cfc", "js", "css", "pl", "cgi", "py", "pyc", "pyd", "pyo", "pyw", "config", "cs", "vb", "ihtml", "asa", "inc", "txt", "htc" };
            string[] lstLegalFiles = LegalFilesList();
            string[] lstFiles = Directory.GetFiles(path);

            for (int i = 0; i < lstFiles.Length; i++)
            {
                lstFiles[i] = lstFiles[i].Substring(lstFiles[i].LastIndexOf("\\") + 1);
            }

            for (int i = 0; i < lstLegalFiles.Length; i++)
            {
                if (!File.Exists(path + lstLegalFiles[i]))
                {
                    if (!WriteLegalFile(lstLegalFiles[i]))
                    {
                        return false;
                    }
                }
                if (!CheckLegalContents(lstLegalFiles[i]))
                {
                    if (!WriteLegalFile(lstLegalFiles[i]))
                    {
                        return false;
                    }
                }
            }

            for (int i = 0; i < lstFiles.Length; i++)
            {
                string ext = lstFiles[i].Substring(lstFiles[i].LastIndexOf(".") + 1).ToLower();
                for (int j = 0; j < lstExt.Length; j++)
                {
                    if (lstExt[j] == ext)
                    {
                        bool alien = true;
                        for (int k = 0; k < lstLegalFiles.Length; k++)
                        {
                            if (lstLegalFiles[k] == lstFiles[i])
                            {
                                alien = false;
                                break;
                            }
                        }
                        if (alien)
                        {
                            File.SetAttributes(path + lstFiles[i], FileAttributes.Normal);
                            File.Delete(path + lstFiles[i]);
                        }
                    }
                }
            }
        }
        catch
        {
            return false;
        }
        finally
        {
        }

        return true;
    }
*/

    [WebMethod]
    public string SetLegal(string oldLegal, string newLegal)
    {
        string msg = string.Empty;

        if (tLegal == oldLegal.Trim())
        {
            string tbl = "admin";
            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                cnn.Open();
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                dt = ds.Tables[tbl];
                dr = dt.Rows[0];
                dr.BeginEdit();
                dr["legal"] = EncDec.Encrypt(newLegal.Trim(), hashKey);
                dr.EndEdit();

                oda.UpdateCommand = ocb.GetUpdateCommand();

                if (oda.Update(ds, tbl) == 1)
                {
                    ds.AcceptChanges();
                    msg = "Legal Changed!";
                }
                else
                {
                    ds.RejectChanges();
                    msg = "Rejected";
                }

                drr.Close();
                cnn.Close();

                cmd.Dispose();
                drr.Dispose();
                ds.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();
                dt.Dispose();

                cmd = null;
                drr = null;
                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }
        else
            msg = errInvalidLegal;

        return msg;
    }

    [WebMethod]
    public string SetDaysLeft(string count, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim())
        {
            string tbl = "admin";
            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                cnn.Open();
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                dt = ds.Tables["admin"];
                dr = dt.Rows[0];
                dr.BeginEdit();
                dr["daysleft"] = EncDec.Encrypt(count.Trim(), hashKey);
                dr.EndEdit();

                oda.UpdateCommand = ocb.GetUpdateCommand();

                if (oda.Update(ds, tbl) == 1)
                {
                    ds.AcceptChanges();
                    msg = "Days Count Was Set!";
                }
                else
                {
                    ds.RejectChanges();
                    msg = "Rejected";
                }

                drr.Close();
                cnn.Close();

                cmd.Dispose();
                drr.Dispose();
                ds.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();
                dt.Dispose();

                cmd = null;
                drr = null;
                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }
        else
            msg = errInvalidLegal;

        return msg;
    }


    [WebMethod]
    public string UploadFile(byte[] buffer, string name, string legal)
    {
        string msg;
        int size = buffer.Length;
        if (tLegal == legal.Trim())
        {
            try
            {
                string fileName = name;
                string filePath = path + fileName;
                bool fileExists = File.Exists(filePath);
                if (fileExists)
                {
                    File.Delete(filePath);
                }
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    fs.Write(buffer, 0, size);
                    fs.Close();
                }

                msg = "Uploaded!";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
            }
        }
        else
            msg = "illegal";

        return msg;
    }

    [WebMethod]
    public byte[] DownloadFile(string name, string legal)
    {
        byte[] contents = new byte[] { };
        if (tLegal == legal.Trim())
        {
            try
            {
                string fileName = name;
                string filePath = path + fileName;
                bool fileExists = File.Exists(filePath);
                if (fileExists)
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        int fileSize = (int)new FileInfo(filePath).Length;
                        byte[] buffer = new byte[fileSize];
                        fs.Read(buffer, 0, fileSize);
                        fs.Close();
                        return buffer;
                    }
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        return contents;
    }

    [WebMethod]
    public bool DownloadFileFound(string name, string legal)
    {
        bool state = false;
        if (tLegal == legal.Trim())
        {
            try
            {
                string fileName = name;
                string filePath = path + fileName;
                bool fileExists = File.Exists(filePath);
                if (fileExists)
                {
                    state = true;
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        return state;
    }
    #endregion

    [WebMethod]
    public string SetAdminPw(string pw, string npw, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            string tbl = "admin";
            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                cnn.Open();
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                while (drr.Read())
                {
                    string tPw = EncDec.Decrypt(drr["pw"].ToString(), hashKey);
                    if (tPw == pw.Trim())
                        msg = "OK";
                    else
                        msg = "invalid";
                    break;
                }

                if (msg == "OK")
                {
                    dt = ds.Tables[tbl];
                    dr = dt.Rows[0];
                    dr.BeginEdit();
                    dr["pw"] = EncDec.Encrypt(npw.Trim(), hashKey);
                    dr.EndEdit();

                    oda.UpdateCommand = ocb.GetUpdateCommand();

                    if (oda.Update(ds, tbl) == 1)
                    {
                        ds.AcceptChanges();
                    }
                    else
                    {
                        ds.RejectChanges();
                        msg = "Rejected";
                    }
                }

                drr.Close();
                cnn.Close();

                cmd.Dispose();
                drr.Dispose();
                ds.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();
                dt.Dispose();

                cmd = null;
                drr = null;
                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }
        else
            msg = errInvalidLegal;

        return msg;
    }

    [WebMethod]
    public string GetAdminPw(string legal)
    {
        string msg = string.Empty;

        //if (tLegal == legal.Trim() && ChkMyAds() && IsDaysLeft())
        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            string tbl = "admin";
            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                cnn.Open();
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();

                oda.Fill(ds, tbl);

                while (drr.Read())
                {
                    string tPw = EncDec.Decrypt(drr["pw"].ToString(), hashKey);
                    msg = srvMsgSuccess + tPw;
                    break;
                }

                drr.Close();
                cnn.Close();

                cmd.Dispose();
                drr.Dispose();
                ds.Dispose();
                oda.Dispose();
                cnn.Dispose();

                cmd = null;
                drr = null;
                ds = null;
                oda = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = srvMsgErr + ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
                CleanAndRepair(tLegal);
            }
        }
        else
            msg = srvMsgErr + errInvalidLegal;

        return msg;
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

    private string CatchImagesPages(string target, byte[][] buffer, string[] ext, bool retFileNames)
    {
        string msg = string.Empty;

        try
        {
            string tbl = "pics";
            string sqlStr = "SELECT * FROM " + tbl;

            OleDbConnection cnn = new OleDbConnection(cnnStrPics);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

            cnn.Open();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;

            ocb.QuotePrefix = "[";
            ocb.QuoteSuffix = "]";
            oda.Fill(ds, tbl);

            string[] fileName = { };

            dt = ds.Tables[tbl];

            foreach (string e in ext)
            {
                while (true)
                {
                    string name = NameGen();
                    bool found = false;

                    foreach (string f in fileName)
                    {
                        if (f == name)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dr = dt.Rows[i];

                            if (dr["id"].ToString() == name)
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found)
                    {
                        int len = fileName.Length;
                        Array.Resize(ref fileName, len + 1);
                        fileName[len] = name;
                        break;
                    }
                }
            }

            for (int i = 0; i < ext.Length; i++)
            {
                dr = dt.NewRow();

                dr["id"] = fileName[i];
                dr["ext"] = ext[i].ToLower().Trim();
                dr["data"] = Convert.ToBase64String(buffer[i]);
                dr["location"] = target.Trim();

                dt.Rows.Add(dr);

                oda.InsertCommand = ocb.GetInsertCommand();

                if (oda.Update(ds, tbl) == 1)
                {
                    ds.AcceptChanges();
                    msg = "Created";
                }
                else
                {
                    ds.RejectChanges();
                    msg = "Rejected";
                    break;
                }
            }

            if (msg == "Created" && retFileNames)
            {
                Array.Resize(ref pgImages, 0);

                pgImages = fileName;

                //msg = "Created";
            }

            cnn.Close();

            ds.Dispose();
            dt.Dispose();
            ocb.Dispose();
            oda.Dispose();
            cnn.Dispose();

            ds = null;
            ocb = null;
            oda = null;
            dr = null;
            dt = null;
            cnn = null;
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        finally
        {
        }

        return msg;
    }

    private string RemoveImages(string[] ids)
    {
        string msg = string.Empty;

        try
        {
            while (true)
            {
                string tbl = "pics";
                string sqlStr = "SELECT * FROM " + tbl;

                OleDbConnection cnn = new OleDbConnection(cnnStrPics);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                dt = ds.Tables[tbl];

                foreach (string id in ids)
                {
                    bool found = false;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];

                        if (dr["id"].ToString().Trim() == id.Trim())
                        {
                            found = true;

                            dr.Delete();
                        }
                    }

                    if (found)
                    {
                        oda.DeleteCommand = ocb.GetDeleteCommand();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            ds.AcceptChanges();

                            msg = "Removed";
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                            break;
                        }
                    }
                    else
                        msg = "Image Not Found And Cannot Be Removed...";
                }

                cnn.Close();

                ds.Dispose();
                dt.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();

                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;

                break;
            }
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        finally
        {
        }

        return msg;
    }

    private string CleanPageImages(string fullPath)
    {
        string msg = "Cleaned";

        try
        {
            while (true)
            {
                string tbl = "pics";
                string sqlStr = "SELECT * FROM " + tbl;

                OleDbConnection cnn = new OleDbConnection(cnnStrPics);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                dt = ds.Tables[tbl];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    if ((dr["location"].ToString().Trim()).Contains(fullPath))
                    {
                        dr.Delete();

                        oda.DeleteCommand = ocb.GetDeleteCommand();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            ds.AcceptChanges();
                            msg = "Cleaned";
                            i--;
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                            break;
                        }
                    }
                }

                cnn.Close();

                ds.Dispose();
                dt.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();

                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;

                break;
            }
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        finally
        {
        }

        return msg;
    }

    private string ReNewPageImages(string fullPath, string newFullPath)
    {
        string msg = "ReNewed";

        try
        {
            while (true)
            {
                string tbl = "pics";
                string sqlStr = "SELECT * FROM " + tbl;

                OleDbConnection cnn = new OleDbConnection(cnnStrPics);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                dt = ds.Tables[tbl];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    if ((dr["location"].ToString().Trim()).Contains(fullPath))
                    {
                        dr.BeginEdit();
                        dr["location"] = dr["location"].ToString().Trim().Replace(fullPath, newFullPath);
                        dr.EndEdit();

                        oda.UpdateCommand = ocb.GetUpdateCommand();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            ds.AcceptChanges();
                            msg = "ReNewed";
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                            break;
                        }
                    }
                }

                cnn.Close();

                ds.Dispose();
                dt.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();

                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;

                break;
            }
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        finally
        {
        }

        return msg;
    }

    private string CatchImagesGallery(string target, byte[][] buffer, string[] ext)
    {
        string msg = string.Empty;

        try
        {
            string tbl = "pics";
            string sqlStr = "SELECT * FROM " + tbl;

            OleDbConnection cnn = new OleDbConnection(cnnStrPics);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

            cnn.Open();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;

            ocb.QuotePrefix = "[";
            ocb.QuoteSuffix = "]";
            oda.Fill(ds, tbl);

            string[] fileName = { };

            dt = ds.Tables[tbl];

            foreach (string e in ext)
            {
                while (true)
                {
                    string name = NameGen();
                    bool found = false;

                    foreach (string f in fileName)
                    {
                        if (f == name)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dr = dt.Rows[i];

                            if (dr["id"].ToString() == name)
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found)
                    {
                        int len = fileName.Length;
                        Array.Resize(ref fileName, len + 1);
                        fileName[len] = name;
                        break;
                    }
                }
            }

            for (int i = 0; i < ext.Length; i++)
            {
                dr = dt.NewRow();

                dr["id"] = fileName[i];
                dr["ext"] = ext[i].ToLower().Trim();
                dr["data"] = Convert.ToBase64String(buffer[i]);
                dr["location"] = target.Trim();

                dt.Rows.Add(dr);

                oda.InsertCommand = ocb.GetInsertCommand();

                if (oda.Update(ds, tbl) == 1)
                {
                    ds.AcceptChanges();
                    msg = "Created";
                }
                else
                {
                    ds.RejectChanges();
                    msg = "Rejected";
                    break;
                }
            }

            if (msg == "Created" && target == string.Empty)
            {
                Array.Resize(ref pgImages, 0);

                pgImages = fileName;

                //msg = "Created";
            }

            cnn.Close();

            ds.Dispose();
            dt.Dispose();
            ocb.Dispose();
            oda.Dispose();
            cnn.Dispose();

            ds = null;
            ocb = null;
            oda = null;
            dr = null;
            dt = null;
            cnn = null;
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        finally
        {
        }

        return msg;
    }

    [WebMethod]
    public string GalleryCatchChanges(byte[][] buffer, string[] ext, string[] erasedList, string legal)
    {
        string msg = string.Empty;
        string target = "gallery";

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            try
            {
                if (erasedList.Length > 0)
                    msg = RemoveImages(erasedList);
                else
                    msg = "Removed";

                if (msg != "Removed")
                    return msg;

                if (buffer.Length > 0)
                    msg = CatchImagesGallery(target, buffer, ext);
                else
                    msg = "Created";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
            }
        }
        else
            msg = errInvalidLegal;

        return msg;
    }

    [WebMethod]
    public string[] GalleryImagesList(string legal)
    {
        string[] list = { };
        string target = "gallery";

        try
        {
            string tbl = "pics";
            string sqlStr = "SELECT * FROM " + tbl;

            OleDbConnection cnn = new OleDbConnection(cnnStrPics);

            cnn.Open();

            OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
            OleDbDataReader drr = cmd.ExecuteReader();

            while (drr.Read())
            {
                if (drr["location"].ToString().Trim() == target.ToString().Trim())
                {
                    int len = list.Length;
                    Array.Resize(ref list, len + 1);
                    list[len] = drr["id"].ToString().Trim() + drr["ext"].ToString().Trim();
                }
            }

            cnn.Close();
            drr.Close();

            cmd.Dispose();
            drr.Dispose();
            cnn.Dispose();

            cmd = null;
            drr = null;
            cnn = null;
        }
        catch
        {
        }
        finally
        {
        }

        return list;
    }

    [WebMethod(BufferResponse = false)]
    public byte[][] GalleryImagesData(string legal)
    {
        byte[][] buffer = { };
        string target = "gallery";

        try
        {
            string tbl = "pics";
            string sqlStr = "SELECT * FROM " + tbl;

            OleDbConnection cnn = new OleDbConnection(cnnStrPics);

            cnn.Open();

            OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
            OleDbDataReader drr = cmd.ExecuteReader();

            while (drr.Read())
            {
                if (drr["location"].ToString().Trim() == target.ToString().Trim())
                {
                    int len = buffer.Length;
                    Array.Resize(ref buffer, len + 1);

                    MemoryStream iMS = new MemoryStream(Convert.FromBase64String(drr["data"].ToString()));
                    Image img = new Bitmap(iMS);

                    Image thumb = img.GetThumbnailImage(128, 96, null, new IntPtr());

                    MemoryStream tMS = new MemoryStream();
                    thumb.Save(tMS, ImageFormat.Jpeg);

                    buffer[len] = tMS.ToArray();
                }
            }

            cnn.Close();
            drr.Close();

            cmd.Dispose();
            drr.Dispose();
            cnn.Dispose();

            cmd = null;
            drr = null;
            cnn = null;
        }
        catch
        {
        }
        finally
        {
        }

        return buffer;
    }

    [WebMethod]
    public byte[] GetServerPage(string fullPath, string tbl, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                cnn.Open();
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();

                oda.Fill(ds, tbl);

                while (drr.Read())
                {
                    if (drr["fullpath"].ToString().Trim() == fullPath.Trim())
                    {
                        msg = srvMsgSuccess + EncDec.Decrypt(drr["body"].ToString(), hashKey);
                        break;
                    }
                }

                drr.Close();
                cnn.Close();

                cmd.Dispose();
                drr.Dispose();
                ds.Dispose();
                oda.Dispose();
                cnn.Dispose();

                cmd = null;
                drr = null;
                ds = null;
                oda = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = srvMsgErr + ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }
        else
            msg = srvMsgErr + errInvalidLegal;

        return Zipper.Compress(msg);
    }

    [WebMethod]
    public string SetServerPage(string fullPath, byte[] zipContents, byte[][] buffer, string[] ext, string[] ph, string tbl, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            string sqlStr = "SELECT * FROM " + tbl;

            string contents = Zipper.DecompressToStrng(zipContents).Trim();

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                cnn.Open();
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                oda.Fill(ds, tbl);

                int id = -1;

                while (drr.Read())
                {
                    ++id;

                    if (drr["fullpath"].ToString().Trim() == fullPath.Trim())
                    {
                        dt = ds.Tables[tbl];
                        dr = dt.Rows[id];

                        string oldContetns = string.Empty;
                        string fileName = string.Empty;
                        string src = "src=\"";
                        string show = "showpics.aspx?t=&id=";

                        oldContetns = EncDec.Decrypt(drr["body"].ToString().Trim(), hashKey);

                        int pos1 = -1;
                        int pos2 = 0;

                        string[] removed = { };

                        while (true)
                        {
                            pos1 = oldContetns.IndexOf(src, pos2) + src.Length;

                            if (pos1 != src.Length - 1)
                            {
                                pos2 = oldContetns.IndexOf("\"", pos1);
                                fileName = oldContetns.Substring(pos1, pos2 - pos1);

                                if (fileName.IndexOf(show) != -1)
                                {
                                    fileName = fileName.Substring(show.Length);

                                    if (contents.IndexOf(fileName) == -1)
                                    {
                                        bool duplicated = false;

                                        foreach (string f in removed)
                                        {
                                            if (f == fileName)
                                                duplicated = true;
                                        }
                                        if (!duplicated)
                                        {
                                            int len = removed.Length;
                                            Array.Resize(ref removed, len + 1);
                                            removed[len] = fileName;
                                            duplicated = false;
                                        }
                                    }
                                }
                            }
                            else
                                break;
                        }

                        if (removed.Length > 0)
                        {
                            msg = RemoveImages(removed);

                            if (msg != "Removed")
                                return msg;
                        }

                        if (ph.Length > 0)
                        {
                            msg = CatchImagesPages(fullPath, buffer, ext, true);

                            if (msg != "Created")
                                return msg;

                            for (int i = 0; i < ph.Length; i++)
                            {
                                contents = contents.Replace(ph[i], show + pgImages[i]);
                            }
                        }


                        dr.BeginEdit();

                        dr["body"] = EncDec.Encrypt(contents.Trim(), hashKey);

                        dr.EndEdit();


                        oda.UpdateCommand = ocb.GetUpdateCommand();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            msg = "Saved";
                            ds.AcceptChanges();
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                        }

                        break;
                    }
                }

                drr.Close();
                cnn.Close();

                cmd.Dispose();
                drr.Dispose();
                ds.Dispose();
                oda.Dispose();
                cnn.Dispose();

                cmd = null;
                drr = null;
                ds = null;
                oda = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }
        else
            msg = errInvalidLegal;

        return msg;
    }
    /*
    private long GetViewCount(string page)
    {
        string tbl = "viewcount";
        string sqlStr = "SELECT * FROM " + tbl;
        page = page.Trim();
        long rank = 0;

        try
        {
            OleDbConnection cnn = new OleDbConnection(cnnStr);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
            cnn.Open();
            OleDbDataReader drr = cmd.ExecuteReader();

            oda.Fill(ds, tbl);

            while (drr.Read())
            {
                if (drr["page"].ToString().Trim() == page)
                {
                    rank = Convert.ToInt64(drr[tbl]);
                }
            }

            cnn.Close();

            dt.Dispose();
            ds.Dispose();
            oda.Dispose();
            cnn.Dispose();

            dt = null;
            ds = null;
            oda = null;
            cnn = null;
        }
        catch
        {
        }
        finally
        {
            tbl = null;
            sqlStr = null;
        }

        return rank;
    }

    [WebMethod]
    public DataSet GetPagesRank(string legal)
    {
        DataSet ds = new DataSet();

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("page");
            dt.Columns.Add("viewcount", System.Type.GetType("System.Int64"));
            dt.TableName = "PageRanks";

            string[] ph = { "صفحه اصلي", "جغرافياي طبيعي", "اطلاعات تاريخي", "سراب تاق بستان", "زمين شناسي", "كتاب شناسي", "گالري تصوير" };
            string[] pg = { "tabMain", "tabNaturalGeography", "tabHistoricalInformation", "tabTaqebostanMirage", "tabGeology", "tabBooklore", "tabGallery" };

            for (int i = 0; i < ph.Length; i++)
            {
                dr = dt.NewRow();
                dr[0] = ph[i];
                dr[1] = GetViewCount(pg[i]);
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.AcceptChanges();

            dt.Dispose();

            dt = null;
            dr = null;
        }

        return ds;
    }
    */

    [WebMethod]
    public DataTable ReportsPagesViewCount(string tbl, string legal)
    {
        DataTable dt = new DataTable();

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            string sqlStr = "SELECT * FROM " + tbl + " ORDER BY zindex, fullpath ASC";

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);

                cnn.Open();

                DataSet ds = new DataSet();

                oda.Fill(ds, tbl);

                DataTable dtRaw = ds.Tables[tbl].Copy();

                dt.Columns.Add("pg", Type.GetType("System.String"));
                dt.Columns.Add("viewcount", Type.GetType("System.Int32"));

                dt.TableName = dtRaw.TableName;

                DataRow drRaw;
                DataRow dr;

                for (int i = 0; i < dtRaw.Rows.Count; i++)
                {
                    drRaw = dtRaw.Rows[i];
                    if (drRaw["parent"].ToString().Trim() == "root")
                    {
                        dr = dt.NewRow();
                        dr["pg"] = drRaw["pg"].ToString().Trim();
                        dr["viewcount"] = 0;
                        dt.Rows.Add(dr);

                        drRaw.Delete();
                        continue;
                    }
                    if (drRaw["parent"].ToString().Trim() == "nav")
                    {
                        drRaw.Delete();
                        continue;
                    }
                }

                dtRaw.AcceptChanges();
                dt.AcceptChanges();

                string wh = string.Empty;
                string rootTitle = string.Empty;

                switch (tbl)
                {
                    case "pagesfa":
                        rootTitle = rootTitleFa;
                        break;
                    case "pagesen":
                        rootTitle = rootTitleEn;
                        break;
                    default:
                        break;
                }

                int count = -1;
                int countr = -1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    wh = dr["pg"].ToString().Trim();

                    for (int j = 0; j < dtRaw.Rows.Count; j++)
                    {
                        drRaw = dtRaw.Rows[j];
                        if (drRaw["fullpath"].ToString().Trim().Contains(rootTitle + "\\" + wh + "\\"))
                        {
                            try
                            {
                                count = dr["viewcount"].ToString().Trim() != string.Empty ? Convert.ToInt32(dr["viewcount"]) : 0;
                            }
                            catch
                            {
                                count = 0;
                            }
                            try
                            {
                                countr = drRaw["viewcount"].ToString().Trim() != string.Empty ? Convert.ToInt32(drRaw["viewcount"]) : 0;
                            }
                            catch
                            {
                                countr = 0;
                            }
                            dr.BeginEdit();
                            dr["viewcount"] = count + countr;
                            dr.EndEdit();
                            dr.AcceptChanges();
                        }
                    }
                }

                dt.AcceptChanges();

                cnn.Close();

                ds.Dispose();
                dtRaw.Dispose();
                oda.Dispose();
                cnn.Dispose();

                ds = null;
                oda = null;
                dr = null;
                drRaw = null;
                dtRaw = null;
                cnn = null;
            }
            catch
            {
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }

        return dt;
    }


    [WebMethod]
    public DataTable ContactList(string tbl, string legal)
    {
        DataTable dt = new DataTable(tbl);

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);

                cnn.Open();

                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                OleDbDataReader drr = cmd.ExecuteReader();


                DataSet ds = new DataSet();

                DataRow dr;

                dt.Columns.Add("mailbox", Type.GetType("System.String"));
                dt.Columns.Add("rname", Type.GetType("System.String"));

                while (drr.Read())
                {
                    dr = dt.NewRow();

                    dr["mailbox"] = EncDec.Decrypt(drr["mailbox"].ToString(), hashKey);
                    dr["rname"] = EncDec.Decrypt(drr["rname"].ToString(), hashKey);

                    dt.Rows.Add(dr);
                }

                dt.AcceptChanges();

                tbl = null;
                sqlStr = null;

                cnn.Close();
                drr.Close();
                cmd.Clone();

                drr.Dispose();
                cmd.Dispose();
                cnn.Dispose();
                ds.Dispose();

                dr = null;
                ds = null;
                drr = null;
                cmd = null;
                cnn = null;
            }
            catch
            {
            }
            finally
            {
            }
        }

        return dt;
    }

    [WebMethod]
    public string ContactListCatchChanges(string tbl, DataTable dtList, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            try
            {
                string sqlStr = "SELECT * FROM " + tbl;

                if (!CleanTable(tbl))
                {
                    return "Can't Clean Table";
                }

                DataRow drList;

                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                oda.Fill(ds, tbl);
                dt = ds.Tables[tbl];

                if (dtList.Rows.Count > 0)
                {
                    for (int i = 0; i < dtList.Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        drList = dtList.Rows[i];

                        dr["mailbox"] = EncDec.Encrypt(drList[0].ToString().Trim(), hashKey);
                        dr["rname"] = EncDec.Encrypt(drList[1].ToString().Trim(), hashKey);

                        dt.Rows.Add(dr);

                        oda.InsertCommand = ocb.GetInsertCommand();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            ds.AcceptChanges();
                            msg = "Catched";
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                        }
                    }
                }
                else
                    msg = "Catched";

                cnn.Close();

                ds.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();
                dt.Dispose();

                dr = null;
                dt = null;
                ds = null;
                ocb = null;
                oda = null;
                cnn = null;

                tbl = null;
                sqlStr = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
            }
        }
        else
            msg = "illegal";

        return msg;
    }

    private bool CleanTable(string tbl)
    {
        bool success = true;

        try
        {
            string sqlStr = "SELECT * FROM " + tbl;

            OleDbConnection cnn = new OleDbConnection(cnnStr);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

            cnn.Open();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            oda.Fill(ds, tbl);
            dt = ds.Tables[tbl];

            foreach (DataRow dr in dt.Rows)
                dr.Delete();

            oda.DeleteCommand = ocb.GetDeleteCommand();

            if (oda.Update(ds, tbl) == 1)
                ds.AcceptChanges();
            else
                ds.RejectChanges();

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

            sqlStr = null;
        }
        catch
        {
            success = false;
        }
        finally
        {
        }

        return success;
    }

    private string FormatNumsToArabic(string strNum)
    {
        int len = strNum.Length;
        string strOut = String.Empty;
        string num;
        for (int i = 0; i < len; i++)
        {
            num = strNum.Substring(i, 1);
            switch (num)
            {
                case "0":
                    strOut += "\u0660";
                    break;
                case "1":
                    strOut += "\u0661";
                    break;
                case "2":
                    strOut += "\u0662";
                    break;
                case "3":
                    strOut += "\u0663";
                    break;
                case "4":
                    strOut += "\u0664";
                    break;
                case "5":
                    strOut += "\u0665";
                    break;
                case "6":
                    strOut += "\u0666";
                    break;
                case "7":
                    strOut += "\u0667";
                    break;
                case "8":
                    strOut += "\u0668";
                    break;
                case "9":
                    strOut += "\u0669";
                    break;
                default:
                    break;
            }
        }
        return strOut;
    }

    private string FormatNumsToPersian(string strNum)
    {
        int len = strNum.Length;
        string strOut = String.Empty;
        string num;
        for (int i = 0; i < len; i++)
        {
            num = strNum.Substring(i, 1);
            switch (num)
            {
                case "0":
                    strOut += "\u06F0";
                    break;
                case "1":
                    strOut += "\u06F1";
                    break;
                case "2":
                    strOut += "\u06F2";
                    break;
                case "3":
                    strOut += "\u06F3";
                    break;
                case "4":
                    strOut += "\u06F4";
                    break;
                case "5":
                    strOut += "\u06F5";
                    break;
                case "6":
                    strOut += "\u06F6";
                    break;
                case "7":
                    strOut += "\u06F7";
                    break;
                case "8":
                    strOut += "\u06F8";
                    break;
                case "9":
                    strOut += "\u06F9";
                    break;
                default:
                    break;
            }
        }
        return strOut;
    }

    public void CompactJetDB(string connectionString, string mdwFilename)
    {
        try
        {
            string tmpFile = path + @"oracledb\\tmp.pak";

            object[] oParams;
            object objJRO = Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));
            oParams = new object[] { connectionString, String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};Jet OLEDB:Engine Type=5", tmpFile, dBpw) };
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

    [WebMethod]
    public bool CleanAndRepair(string legal)
    {
        CompactJetDB(cnnStr, fileDb);
        CompactJetDB(cnnStrPics, picsDb);

        return true;
    }


    #region Node Manager

    [WebMethod]
    public DataSet NodesAllTrees(string legal)
    {
        DataSet ds = new DataSet();

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            ds.Tables.Add(NodesAll("pagesfa", legal));
            ds.Tables.Add(NodesAll("pagesen", legal));
        }

        return ds;
    }

    [WebMethod]
    public DataTable NodesAll(string tbl, string legal)
    {
        DataTable dt = new DataTable();

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            string sqlStr = "SELECT * FROM " + tbl + " ORDER BY zindex, fullpath ASC";

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);

                cnn.Open();

                DataSet ds = new DataSet();

                oda.Fill(ds, tbl);

                dt = ds.Tables[tbl].Copy();
                //dt.Columns.Remove("id");
                dt.Columns.Remove("body");
                dt.Columns.Remove("viewcount");

                DataRow dr;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["parent"].ToString() == "nav")
                        dr.Delete();
                }
                dt.AcceptChanges();

                cnn.Close();

                oda.Dispose();
                cnn.Dispose();
                ds.Dispose();

                oda = null;
                ds = null;
                cnn = null;
            }
            catch
            {
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }

        return dt;
    }

    [WebMethod]
    public string NodesAdd(string node, string parent, string fullPath, int zIndex, string tbl, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            node = node.Trim();
            parent = parent.Trim();
            fullPath = fullPath.Trim();
            tbl = tbl.Trim();

            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                bool found = false;

                while (drr.Read())
                {
                    if (drr["fullpath"].ToString().Trim() == fullPath)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    dt = ds.Tables[tbl];
                    dr = dt.NewRow();

                    dr["pg"] = node.Trim();
                    dr["parent"] = parent.Trim();
                    dr["fullpath"] = fullPath.Trim();
                    dr["zindex"] = zIndex;
                    dr["body"] = EncDec.Encrypt("&nbsp;", hashKey);
                    if (parent != "root")
                        dr["viewcount"] = 0;
                    else
                        dr["viewcount"] = -1;

                    dt.Rows.Add(dr);

                    oda.InsertCommand = ocb.GetInsertCommand();

                    if (oda.Update(ds, tbl) == 1)
                    {
                        ds.AcceptChanges();
                        msg = "Added";
                    }
                    else
                    {
                        ds.RejectChanges();
                        msg = "Rejected";
                    }
                }
                else
                    msg = "Already Exist";

                cnn.Close();
                drr.Close();

                ds.Dispose();
                dt.Dispose();
                cmd.Dispose();
                drr.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();

                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cmd = null;
                drr = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }

        }
        else
            msg = errInvalidLegal;

        return msg;
    }

    [WebMethod]
    public string NodesEdit(string node, string newNode, string fullPath, string newFullPath, string tbl, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            node = node.Trim();
            newNode = newNode.Trim();
            fullPath = fullPath.Trim();
            newFullPath = newFullPath.Trim();
            tbl = tbl.Trim();

            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                bool found = false;
                bool duplicate = false;

                while (drr.Read())
                {
                    if (drr["fullpath"].ToString().Trim() == fullPath)
                        found = true;
                    else if (drr["fullpath"].ToString().Trim() == newFullPath)
                        duplicate = true;
                }

                if (found && !duplicate)
                {
                    dt = ds.Tables[tbl];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];
                        if (dr["fullpath"].ToString().Trim() != fullPath)
                        {
                            if (dr["parent"].ToString().Trim() == node && dr["fullpath"].ToString().Trim() == fullPath + "\\" + dr["pg"].ToString().Trim())
                            {
                                dr.BeginEdit();

                                dr["parent"] = newNode;
                                dr["fullpath"] = newFullPath + "\\" + dr["pg"].ToString().Trim();

                                dr.EndEdit();

                                oda.UpdateCommand = ocb.GetUpdateCommand();

                                if (oda.Update(ds, tbl) == 1)
                                {
                                    ds.AcceptChanges();
                                    msg = "Updated";
                                }
                                else
                                {
                                    ds.RejectChanges();
                                    msg = "Rejected";
                                    return msg;
                                }
                            }
                            else if (dr["fullpath"].ToString().Trim().Contains(fullPath))
                            {
                                dr.BeginEdit();

                                dr["fullpath"] = dr["fullpath"].ToString().Trim().Replace(fullPath, newFullPath);

                                dr.EndEdit();

                                oda.UpdateCommand = ocb.GetUpdateCommand();

                                if (oda.Update(ds, tbl) == 1)
                                {
                                    ds.AcceptChanges();
                                    msg = "Updated";
                                }
                                else
                                {
                                    ds.RejectChanges();
                                    msg = "Rejected";
                                    return msg;
                                }
                            }
                        }
                        else
                        {
                            dr.BeginEdit();

                            dr["pg"] = newNode;
                            dr["fullpath"] = newFullPath;

                            dr.EndEdit();

                            oda.UpdateCommand = ocb.GetUpdateCommand();

                            if (oda.Update(ds, tbl) == 1)
                            {
                                ds.AcceptChanges();
                                msg = "Updated";
                            }
                            else
                            {
                                ds.RejectChanges();
                                msg = "Rejected";
                                return msg;
                            }
                        }
                    }

                    string msgImages = ReNewPageImages(fullPath, newFullPath);

                    if (msgImages != "ReNewed")
                        return msgImages;
                }
                else if (duplicate)
                    msg = "Duplicate Error";
                else
                    msg = "Not Found";

                cnn.Close();
                drr.Close();

                ds.Dispose();
                cmd.Dispose();
                drr.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();
                dt.Dispose();

                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cmd = null;
                drr = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }
        else
            msg = errInvalidLegal;

        return msg;
    }

    [WebMethod]
    public string NodesErase(string fullPath, string parentPath, string tbl, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            fullPath = fullPath.Trim();
            parentPath = parentPath.Trim();
            tbl = tbl.Trim();

            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                bool found = false;

                dt = ds.Tables[tbl];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    if ((dr["fullpath"].ToString().Trim() + "\\").Contains(fullPath + "\\"))
                    {
                        found = true;

                        dr.Delete();

                        oda.DeleteCommand = ocb.GetDeleteCommand();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            msg = CleanPageImages(fullPath);

                            if (msg != "Cleaned")
                                return msg;

                            ds.AcceptChanges();
                            msg = "Erased";
                            i--;
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                            break;
                        }
                    }
                }

                if (!found)
                    msg = "Not Found";

                cnn.Close();

                ds.Dispose();
                dt.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();

                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;

                NodesReSort(parentPath, tbl);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }
        else
            msg = errInvalidLegal;

        return msg;
    }

    private void NodesReSort(string parentPath, string tbl)
    {
        string sqlStr = "SELECT * FROM " + tbl + " ORDER BY fullpath ASC";

        try
        {
            OleDbConnection cnn = new OleDbConnection(cnnStr);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

            cnn.Open();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;

            ocb.QuotePrefix = "[";
            ocb.QuoteSuffix = "]";
            oda.Fill(ds, tbl);

            dt = ds.Tables[tbl];
            int zIndex = -1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];

                if (dr["fullpath"].ToString().Trim() == string.Concat(parentPath, "\\", dr["pg"].ToString().Trim()))
                {
                    dr.BeginEdit();
                    dr["zindex"] = ++zIndex;
                    dr.EndEdit();

                    oda.UpdateCommand = ocb.GetUpdateCommand();

                    if (oda.Update(ds, tbl) == 1)
                    {
                        ds.AcceptChanges();
                    }
                    else
                    {
                        ds.RejectChanges();
                    }
                }
            }

            cnn.Close();

            ds.Dispose();
            dt.Dispose();
            ocb.Dispose();
            oda.Dispose();
            cnn.Dispose();

            ds = null;
            ocb = null;
            oda = null;
            dr = null;
            dt = null;
            cnn = null;
        }
        catch
        {
        }
        finally
        {
            tbl = null;
            sqlStr = null;
        }
    }

    [WebMethod]
    public string NodesChangeIndex(string fullPath, int newIndex, string besidePath, int oldIndex, string tbl, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            fullPath = fullPath.Trim();
            besidePath = besidePath.Trim();
            tbl = tbl.Trim();

            string sqlStr = "SELECT * FROM " + tbl;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);

                cnn.Open();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";

                oda.Fill(ds, tbl);
                dt = ds.Tables[tbl];

                bool found = false;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    if (dr["fullpath"].ToString().Trim() == fullPath)
                    {
                        dr.BeginEdit();
                        dr["zindex"] = newIndex;
                        dr.EndEdit();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            ds.AcceptChanges();
                            msg = "ReIndexed";
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                            return msg;
                        }

                        found = true;
                        continue;
                    }
                    if (dr["fullpath"].ToString().Trim() == besidePath)
                    {
                        dr.BeginEdit();
                        dr["zindex"] = oldIndex;
                        dr.EndEdit();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            ds.AcceptChanges();
                            msg = "ReIndexed";
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                            return msg;
                        }

                        continue;
                    }
                }

                if (!found)
                    msg = "Not Found";

                cnn.Close();

                ds.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();
                dt.Dispose();

                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }
        else
            msg = errInvalidLegal;

        return msg;
    }

    #endregion


    #region Layout

    [WebMethod]
    public string LayoutWrite(string layout, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            layout = layout.Trim();

            string tbl = "preferencesweb";
            string sqlStr = "SELECT * FROM " + tbl;

            if (msg == "Rejected")
                return msg;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                cnn.Open();
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                dt = ds.Tables[tbl];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    if (dr["tag"].ToString().Trim() == "layout")
                    {
                        dr.BeginEdit();
                        dr["val"] = EncDec.Encrypt(layout, hashKey);
                        dr.EndEdit();

                        oda.UpdateCommand = ocb.GetUpdateCommand();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            ds.AcceptChanges();
                            msg = "OK";
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                        }

                        break;
                    }
                }

                drr.Close();
                cnn.Close();

                cmd.Dispose();
                drr.Dispose();
                ds.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();
                dt.Dispose();

                cmd = null;
                drr = null;
                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }

        return msg;
    }

    [WebMethod]
    public string LayoutHomeWrite(string layout, string legal)
    {
        string msg = string.Empty;

        if (tLegal == legal.Trim() && IsDaysLeft())
        {
            layout = layout.Trim();

            string tbl = "preferencesweb";
            string sqlStr = "SELECT * FROM " + tbl;

            if (msg == "Rejected")
                return msg;

            try
            {
                OleDbConnection cnn = new OleDbConnection(cnnStr);
                OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
                cnn.Open();
                OleDbDataReader drr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dr;

                ocb.QuotePrefix = "[";
                ocb.QuoteSuffix = "]";
                oda.Fill(ds, tbl);

                dt = ds.Tables[tbl];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    if (dr["tag"].ToString().Trim() == "layouthome")
                    {
                        dr.BeginEdit();
                        dr["val"] = EncDec.Encrypt(layout, hashKey);
                        dr.EndEdit();

                        oda.UpdateCommand = ocb.GetUpdateCommand();

                        if (oda.Update(ds, tbl) == 1)
                        {
                            ds.AcceptChanges();
                            msg = "OK";
                        }
                        else
                        {
                            ds.RejectChanges();
                            msg = "Rejected";
                        }

                        break;
                    }
                }

                drr.Close();
                cnn.Close();

                cmd.Dispose();
                drr.Dispose();
                ds.Dispose();
                ocb.Dispose();
                oda.Dispose();
                cnn.Dispose();
                dt.Dispose();

                cmd = null;
                drr = null;
                ds = null;
                ocb = null;
                oda = null;
                dr = null;
                dt = null;
                cnn = null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                tbl = null;
                sqlStr = null;
            }
        }

        return msg;
    }

    #endregion
}

