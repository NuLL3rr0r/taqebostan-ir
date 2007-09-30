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

/// <summary>
/// Summary description for Master
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Master : System.Web.Services.WebService {
    private string path;
    private string fileDb = @"oracledb\master.oracle";
    private string picsDb = @"oracledb\pics.oracle";

    private string dBpw = "LJ1GL54gHP8X0bUiN0cy";

    private string hashKey = "g0t0Z86CkMxlV430GpXNNea";
    public static string urlHashKey = "§";

    private string cnnStr;
    private string cnnStrPics;


    private string msgPageExpired = "<p style=\"direction: ltr;\">&nbsp;&nbsp;&nbsp;Permission has expired!!!</p>";


    private string siteMap = string.Empty;
    private bool isMenuULOpened = false;

    private bool isGoogle = false;
    private string langGoogle = "fa";

    public static string rootTitleFa = "منوهاي وب سايت";
    public static string rootTitleEn = "Website Menus";


    public Master () {
        path = Server.MapPath("~");
        path += path.EndsWith("\\") ? string.Empty : "\\";

        fileDb = String.Concat(path, fileDb);
        picsDb = String.Concat(path, picsDb);

        cnnStr = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};", fileDb, dBpw);
        cnnStrPics = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};", picsDb, dBpw);

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
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

            string msg = string.Empty;

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

    public static string GenerateURL(string req, string var, string value, string lang)
    {
        return string.Format("./?lang={0}&req={1}&{2}={3}", lang, req, var, EncDec.Encrypt(value, urlHashKey).Replace("+", "%2B").Replace("/", "%2F").Replace("=", "%3D"));
    }

    private string GenSrcURLFetchPage(string value, string tbl)
    {
        string lang = string.Empty;
        string urlWord = string.Empty;

        switch (tbl)
        {
            case "pagesfa":
                lang = "fa";
                urlWord = "آدرس مستقیم این صفحه";
                break;
            case "pagesen":
                lang = "en";
                urlWord = "Direct URL of the page";
                break;
            default:
                break;
        }

        string url = "<div id=\"dvURLSourceTop\"><a href=\"{0}\" title=\"{2}\">{1}</a></div>";

        if (value == "صفحه اصلی" || value == "Home Page")
            //url = string.Format(url, string.Format("./?lang={0}", lang), value, urlWord);
            url = string.Empty;
        else if (value == "سایت های مرتبط" || value == "Links")
            url = string.Format(url, string.Format("./?lang={0}&req={1}", lang, "links"), value, urlWord);
        else if (value == "درباره ی ما" || value == "About us")
            url = string.Format(url, string.Format("./?lang={0}&req={1}", lang, "aboutus"), value, urlWord);
        else
            url = GenerateSourceURL(value.Substring(value.IndexOf("\\") + 1).Replace("\\", " > "), urlWord, "fetchpage", "page", value.Replace("\\", "/"), lang);

        return url;
    }

    private string GenSrcURLContactUSForm(string tbl)
    {
        string lang = string.Empty;
        string urlWord = string.Empty;
        string cfTitle = string.Empty;

        switch (tbl)
        {
            case "contactlistfa":
                lang = "fa";
                urlWord = "آدرس مستقیم این صفحه";
                cfTitle = "تماس با ما";
                break;
            case "contactlisten":
                lang = "en";
                urlWord = "Direct URL of the page";
                cfTitle = "Contact us";
                break;
            default:
                break;
        }

        string url = "<div id=\"dvURLSourceTop\"><a href=\"{0}\" title=\"{2}\">{1}</a></div>";
        return string.Format(url, string.Format("./?lang={0}&req=contactus", lang), cfTitle, urlWord);
    }

    private string GenSrcURLFetchGallery(string tbl)
    {
        string lang = string.Empty;
        string urlWord = string.Empty;
        string galTitle = string.Empty;

        switch (tbl)
        {
            case "galleryfa":
                lang = "fa";
                urlWord = "آدرس مستقیم این صفحه";
                galTitle = "گالری";
                break;
            case "galleryen":
                lang = "en";
                urlWord = "Direct URL of the page";
                galTitle = "Gallery";
                break;
            default:
                break;
        }

        return string.Format("<div id=\"dvURLSourceTop\"><a href=\"{0}\" title=\"{2}\">{1}</a></div>", string.Format("./?lang={0}&req={1}", lang, "fetchgallery"), galTitle, urlWord);
    }

    private string GenSrcURLSiteMap(string tbl)
    {
        string lang = string.Empty;
        string urlWord = string.Empty;
        string smTitle = string.Empty;

        switch (tbl)
        {
            case "pagesfa":
                lang = "fa";
                urlWord = "آدرس مستقیم این صفحه";
                smTitle = "نقشه سایت";
                break;
            case "pagesen":
                lang = "en";
                urlWord = "Direct URL of the page";
                smTitle = "Site Map";
                break;
            default:
                break;
        }

        string url = "<div id=\"dvURLSourceTop\"><a href=\"{0}\" title=\"{2}\">{1}</a></div>";
        return string.Format(url, string.Format("./?lang={0}&req=sitemap", lang), smTitle, urlWord);
    }

    private string GenSrcURLSearch(string tbl)
    {
        string lang = string.Empty;
        string urlWord = string.Empty;
        string smTitle = string.Empty;

        switch (tbl)
        {
            case "pagesfa":
                lang = "fa";
                urlWord = "آدرس مستقیم این صفحه";
                smTitle = "جستجو";
                break;
            case "pagesen":
                lang = "en";
                urlWord = "Direct URL of the page";
                smTitle = "Search";
                break;
            default:
                break;
        }

        string url = "<div id=\"dvURLSourceTop\"><a href=\"{0}\" title=\"{2}\">{1}</a></div>";
        return string.Format(url, string.Format("./?lang={0}&req=search", lang), smTitle, urlWord);
    }

    private string GenerateSourceURL(string title, string urlWord, string req, string var, string value, string lang)
    {
        string url = "<div id=\"dvURLSourceTop\"><a href=\"{0}\" title=\"{2}\">{1}</a></div>";

        url = string.Format(url, GenerateURL(req, var, value, lang), title, urlWord);

        return url;
    }

    private void AddViewCount(string pg, string tbl)
    {
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
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                if (dr["fullpath"].ToString().Trim() == pg.Trim())
                {
                    dr.BeginEdit();

                    int count = dr["viewcount"].ToString().Trim() != string.Empty ? Convert.ToInt32(dr["viewcount"]) : 0;
                    dr["viewcount"] = ++count;

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
            ocb.Dispose();
            oda.Dispose();
            cnn.Dispose();
            dt.Dispose();

            ds = null;
            ocb = null;
            oda = null;
            dr = null;
            dt = null;
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
    public string FetchPage(string pg, string tbl)
    {
        if (!IsDaysLeft())
            return msgPageExpired;

        string sqlStr = "SELECT * FROM " + tbl;

        string pageContent = string.Empty;

        pg = pg.Replace("/", "\\").Trim();

        string lang = string.Empty;
        string urlWord = string.Empty;
        try
        {
            OleDbConnection cnn = new OleDbConnection(cnnStr);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
            cnn.Open();
            OleDbDataReader drr = cmd.ExecuteReader();

            DataSet ds = new DataSet();

            oda.Fill(ds, tbl);

            string parent = string.Empty;

            //bool found = false;


            switch (tbl)
            {
                case "pagesfa":
                    lang = "fa";
                    urlWord = "آدرس";
                    break;
                case "pagesen":
                    lang = "en";
                    urlWord = "URL";
                    break;
                default:
                    break;
            }

            while (drr.Read())
            {
                if (drr["fullpath"].ToString().Trim() == pg)
                {
                    pageContent = GenSrcURLFetchPage(pg, tbl);
                    pageContent += EncDec.Decrypt(drr["body"].ToString(), hashKey);
                    parent = drr["parent"].ToString().Trim();
                    //found = true;
                    break;
                }
            }

            /*if (!found)
                return "{Invalid Request!!!}";*/

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

            if (parent != "nav")
                AddViewCount(pg, tbl);
        }
        catch (Exception ex)
        {
            pageContent = ex.Message;
        }
        finally
        {
            sqlStr = null;
            tbl = null;
        }

        return pageContent;
    }

    public string GetPersianName(string tab)
    {
        string result = string.Empty;
        string[] ph = { "صفحه اصلي", "جغرافياي طبيعي", "اطلاعات تاريخي", "سراب تاق بستان", "زمين شناسي", "كتاب شناسي", "گالري تصوير" };
        string[] pg = { "tabMain", "tabNaturalGeography", "tabHistoricalInformation", "tabTaqebostanMirage", "tabGeology", "tabBooklore", "tabGallery" };

        for (int i = 0; i < pg.Length; i++)
        {
            if (tab.Trim() == pg[i].Trim())
            {
                result = ph[i];
                break;
            }
        }

        return result;
    }

    [WebMethod]
    public string FetchGallery(string lang)
    {
        string wch = "gallery";

        if (!IsDaysLeft())
            return msgPageExpired;

        string tbl = "pics";
        string sqlStr = "SELECT * FROM " + tbl;
        string tab = string.Empty;

        string galleryContent = string.Empty;

        try
        {
            OleDbConnection cnn = new OleDbConnection(cnnStrPics);
            OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
            OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
            cnn.Open();
            OleDbDataReader drr = cmd.ExecuteReader();

            DataSet ds = new DataSet();

            oda.Fill(ds, tbl);

            while (drr.Read())
            {
                if (drr["location"].ToString().Trim() == wch.Trim())
                {
                    galleryContent += String.Format("<a href=\"showpics.aspx?id={0}&t=\" rel=\"thumbnail\" title=\"{1}\"><img src=\"showpics.aspx?id={0}&t=t\" class=\"thumbs\" /></a>", drr["id"], GetPersianName(wch.Trim()));
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

            string start = "<center><div class=\"dvGallery\">";
            string stop = "</div></center>";

            galleryContent = start + galleryContent + stop;

            galleryContent = GenSrcURLFetchGallery("gallery" + lang) + start + galleryContent + stop;
        }
        catch (Exception ex)
        {
            galleryContent = ex.Message;
        }
        finally
        {
            tbl = null;
            sqlStr = null;
        }

        return galleryContent;
    }

    private string FilterTags(string str)
    {
        int pos1 = -1;
        int pos2 = 0;

        while (true)
        {
            pos1 = str.IndexOf("<");
            if (pos1 != -1)
            {
                pos2 = str.IndexOf(">");
                str = str.Remove(pos1, pos2 - pos1 + 1);
            }
            else
                break;
        }

        return str;
    }

    private string Find(string[] keywords, string tbl)
    {
        string noRoot = string.Empty;
        switch (tbl)
        {
            case "pagesfa":
                noRoot = "منوهاي وب سايت\\";
                break;
            case "pagesen":
                noRoot = "Website Menus\\";
                break;
            default:
                break;
        }

        string result = string.Empty;
        string notFound = "Not Found!!!";

        string sqlStr = "SELECT * FROM " + tbl;

        OleDbConnection cnn = new OleDbConnection(cnnStr);
        OleDbDataAdapter oda = new OleDbDataAdapter(sqlStr, cnn);
        OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
        cnn.Open();
        OleDbDataReader drr = cmd.ExecuteReader();

        DataSet ds = new DataSet();

        try
        {
            oda.Fill(ds, tbl);

            bool found = false;

            while (drr.Read())
            {
                string srch = FilterTags(EncDec.Decrypt(drr["body"].ToString(), hashKey));

                foreach (string kw in keywords)
                {
                    int posSrch = srch.IndexOf(kw);

                    if (posSrch != -1)
                    {
                        string header = drr["fullpath"].ToString().Trim().Replace(noRoot, string.Empty).Replace("\\", " > ");
                        result += String.Format("<a href=\"#top\" onclick=\"fetchPage('{0}');\">{1}</a><br /><br />", drr["fullpath"].ToString().Trim().Replace("\\", "/"), header);
                        found = true;
                        break;
                    }
                }
            }
            result = found ? result : notFound;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            sqlStr = null;

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


        return result;
    }

    [WebMethod]
    public string Find(string keywords, string tbl)
    {
        if (!IsDaysLeft())
            return msgPageExpired;

        keywords = keywords.Trim();

        string result = string.Empty;

        string[] kws = keywords.Split(' ');

        string start = "<div class=\"dvSearchResult\"><h5>{0}</h5><blockquote>";
        string stop = "</blockquote></div>";
        string msgNotFound = string.Empty;
        string notFound = "Not Found!!!";

        switch (tbl)
        {
            case "pagesfa":
                start = string.Format(start, "نتايج جستجو");
                msgNotFound = "عبارت مورد نظر يافت نشد";
                break;
            case "pagesen":
                start = string.Format(start, "Search Results");
                msgNotFound = "Your search phrase did not match any pages";
                break;
            default:
                break;
        }

        msgNotFound += "&hellip;";

        if (keywords != string.Empty)
        {
            result = Find(kws, tbl);

            if (result == notFound)
                result = msgNotFound;
        }
        else
            result = msgNotFound;

        return start + result + stop;
    }

    [WebMethod]
    public string ContactUSForm(string tbl)
    {
        if (!IsDaysLeft())
            return msgPageExpired;

        string cList = string.Empty;

        try
        {
            string sqlStr = "SELECT * FROM " + tbl;

            OleDbConnection cnn = new OleDbConnection(cnnStr);

            cnn.Open();

            OleDbCommand cmd = new OleDbCommand(sqlStr, cnn);
            OleDbDataReader drr = cmd.ExecuteReader();

            string start = "<center>" +
                                "<div class=\"dvContactForm\">" +
                                    "<table border=\"0\" align=\"center\" width=\"100%\">" +
                                        "<tr>" +
                                            "<td style=\"text-align: center;\" colspan=\"2\">" +
                                                "<h6>{0}</h6>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td width=\"30%\">" +
                                                "{1}" +
                                            "</td>" +
                                            "<td width=\"70%\">" +
                                                "<select id=\"cmbWMail\" onChange=\"setFormStatus('dvStatusContactForm', document.getElementById('cmbWMail').value != '' ? true : false, 'reset');\" class=\"comboBox\" style=\"width: 100%;\"><option value=\"\">{2}</option>";
            string stop = "</select>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "{0}" +
                                            "</td>" +
                                            "<td>" +
                                                "<input type=\"text\" class=\"textBox\" id=\"txtName\" style=\"width: 98%;\" disabled=\"disabled\" />" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "{1}" +
                                            "</td>" +
                                            "<td>" +
                                                "<input type=\"text\" class=\"textBox\" id=\"txtEmail\" style=\"text-align: left; width: 98%;\" disabled=\"disabled\" />" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "{2}" +
                                            "</td>" +
                                            "<td>" +
                                                "<input type=\"text\" class=\"textBox\" id=\"txtURL\" style=\"text-align: left; width: 98%;\" disabled=\"disabled\" />" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "{3}" +
                                            "</td>" +
                                            "<td>" +
                                                "<input type=\"text\" class=\"textBox\" id=\"txtMsgSbjct\" style=\"width: 98%;\" disabled=\"disabled\" />" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td valign=\"top\">" +
                                                "{4}" +
                                            "</td>" +
                                            "<td>" +
                                                "<textarea class=\"textArea\" id=\"txtMsgBdy\" style=\"width: 98%\" disabled=\"disabled\"></textarea>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td colspan=\"2\">" +
                                                "<div id=\"dvStatusContactForm\"></div>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                            "</td>" +
                                            "<td style=\"text-align:center\">" +
                                                "<input type=\"button\" value=\"{5}\" class=\"button\" disabled=\"disabled\" onclick=\"sendMessage();\" id=\"btnSend\" style=\"width: 47%;\" />" +
                                                "&nbsp;&nbsp;&nbsp;" +
                                                "<input type=\"reset\" value=\"{6}\" class=\"button\" disabled=\"disabled\" onclick=\"clearForm('dvStatusContactForm', true);\" id=\"btnClear\" style=\"width: 47%;\" />" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</div>" +
                            "</center>";

            switch (tbl)
            {
                case "contactlistfa":
                    start = string.Format(start, "شما مي توانيد پيام خود را در قسمت زير وارد نمائيد:", "تماس با", ".:: تماس با... ::.");
                    stop = string.Format(stop, "نام فرستنده:", "آدرس ايميل:", "صفحه ي وب:", "موضوع پيام:", "متن پيام", "ارسال", "پيام جديد");
                    break;
                case "contactlisten":
                    start = string.Format(start, "Please enter your message:", "Contact with", ".:: Contact with... ::.");
                    stop = string.Format(stop, "Your name:", "Email:", "Website:", "Subject:", "Body", "Send", "Clear");
                    break;
                default:
                    break;
            }

            while (drr.Read())
            {
                string mail = EncDec.Decrypt(drr["mailbox"].ToString(), hashKey);
                string name = EncDec.Decrypt(drr["rname"].ToString(), hashKey);

                cList += String.Format("<option value=\"{0}\">{1}</option>", mail, name);
            }

            cList = GenSrcURLContactUSForm(tbl) + start + cList + stop;

            cnn.Close();

            cmd.Dispose();
            drr.Dispose();
            cnn.Dispose();

            cmd = null;
            drr = null;
            cnn = null;

            sqlStr = null;
            tbl = null;
            start = null;
            stop = null;
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
        }

        return cList;
    }

    [WebMethod]
    public string SendMessage(string to, string sender, string from, string url, string subject, string body)
    {
        string msg = String.Empty;

        try
        {
            string mailServer = "webmail.taqebostan.ir";
            string user = "admin@taqebostan.ir";
            string pw = "smtpserver";
            int port = 25;

            to = to.Trim();
            sender = sender.Trim();
            from = from.Trim();
            url = url.Trim();
            subject = subject.Trim();
            body = body.Trim();

            StringBuilder sb = new StringBuilder();
            sb.Append("<center><div style=\"position: relative; width: 89%; direction: rtl; text-align: justify; font-family:Tahoma; font-size: 14px; line-height: 33px;\">");
            sb.Append("<p>");
            sb.Append("<h6 style=\"color: #0000FF; font-size: 11px;\">");
            sb.Append("مشخصات فرستنده پيغام");
            sb.Append("</h6>");
            sb.Append("نام فرستنده:&nbsp;&nbsp;&nbsp;");
            sb.Append(sender);
            sb.Append("<br />");
            sb.Append("آدرس ايميل:&nbsp;&nbsp;&nbsp;");
            sb.Append(from);
            sb.Append("<br />");
            sb.Append("وب سايت:&nbsp;&nbsp;&nbsp;");
            sb.Append(url);
            sb.Append("<br />");
            sb.Append("</p>");
            sb.Append("<br />");
            sb.Append("<p>");
            sb.Append("<h6 style=\"color: #0000FF; font-size: 11px;\">");
            sb.Append("موضوع پيام");
            sb.Append("</h6>");
            sb.Append(subject);
            sb.Append("</p>");
            sb.Append("<br />");
            sb.Append("<p>");
            sb.Append("<h6 style=\"color: #0000FF; font-size: 11px;\">");
            sb.Append("متن  پيام");
            sb.Append("</h6>");
            sb.Append(body);
            sb.Append("</p>");
            sb.Append("<br />");
            sb.Append("</div></center>");

            body = sb.ToString();
            subject = "user of taqebostan.ir - " + subject.Trim();

            using (MailMessage message = new MailMessage(from, to, subject, body))
            {
                message.BodyEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient(mailServer);
                //smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(user, pw);
                smtp.Port = port;
                smtp.Send(message);
                msg = "sent";
            }
        }
        catch (FormatException ex)
        {
            msg = ex.Message;
        }
        catch (SmtpException ex)
        {
            msg = ex.Message;
        }
        finally
        {
        }

        return msg;
    }



    public string LayoutRead()
    {
        string layout = string.Empty;

        string tbl = "preferencesweb";
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
                if (drr["tag"].ToString().Trim() == "layout")
                {
                    layout = EncDec.Decrypt(drr["val"].ToString().Trim(), hashKey);
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
        catch
        {
        }
        finally
        {
            tbl = null;
            sqlStr = null;
        }

        return layout;
    }

    public string LayoutHomeRead()
    {
        string layout = string.Empty;

        string tbl = "preferencesweb";
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
                if (drr["tag"].ToString().Trim() == "layouthome")
                {
                    layout = EncDec.Decrypt(drr["val"].ToString().Trim(), hashKey);
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
        catch
        {
        }
        finally
        {
            tbl = null;
            sqlStr = null;
        }

        return layout;
    }

    public void GetNodesFromTable(System.Web.UI.WebControls.TreeNode node, DataTable dt)
    {
        System.Web.UI.WebControls.TreeNodeCollection nodes = node.ChildNodes;
        DataRow dr;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dr = dt.Rows[i];
            string name = dr["pg"].ToString().Trim();

            if (dr["parent"].ToString().Trim() == node.Text && dr["fullpath"].ToString().Trim().Replace("\\", "/") == node.ValuePath + "/" + name)
            {
                System.Web.UI.WebControls.TreeNode tn = new System.Web.UI.WebControls.TreeNode();
                tn.Text = name;
                tn.Value = name;
                nodes.Add(tn);

                GetNodesFromTable(tn, dt);
            }
        }
    }

    private void GetMenusFromNodes(System.Web.UI.WebControls.TreeNode node, string lang, bool hasChild, string[] lastChild)
    {
        System.Web.UI.WebControls.TreeNodeCollection nodes = node.ChildNodes;

        foreach (System.Web.UI.WebControls.TreeNode n in nodes)
        {
            string name = n.Value;
            string path = n.ValuePath;

            if (n.Parent.Text != "root")
            {
                int len = lastChild.Length;

                if (n.ChildNodes.Count > 0)
                {
                    Array.Resize(ref lastChild, len + 1);
                    lastChild[len] = n.ChildNodes[n.ChildNodes.Count - 1].ValuePath;

                    siteMap += string.Format("<li>{0}<ul>", name);

                    GetMenusFromNodes(n, lang, hasChild, lastChild);
                }
                else
                {
                    if (!isGoogle)
                        siteMap += string.Format("<li><a href=\"javascript:;\" onclick=\"fetchPage('{1}');\">{0}</a></li>", name, path);
                    else
                        siteMap += string.Format("<li><a href=\"{1}\">{0}</a></li>", name, GenerateURL("fetchpage", "page", path, langGoogle));
                }

                if (len > 0)
                {
                    for (int i = 0; i < lastChild.Length; i++)
                    {
                        if (lastChild[i] == n.ValuePath)
                        {
                            siteMap += "</ul></li>";
                        }
                    }
                }

            }
            else
            {
                if (hasChild)
                {
                    siteMap += "</ul></li>";
                    isMenuULOpened = false;
                    hasChild = false;
                }
                if (n.ChildNodes.Count > 0)
                {
                    siteMap += string.Format("<li>{0}<ul>", name);
                    isMenuULOpened = true;
                    hasChild = true;

                    GetMenusFromNodes(n, lang, hasChild, lastChild);
                }
                else if (!hasChild)
                {
                    siteMap += string.Format("<li>{0}<ul></ul></li>", name);
                    isMenuULOpened = false;
                }
            }
        }
    }

    private string GetGalleryMenues(string tbl, DataTable dt)
    {
        string msg = string.Empty;

        if (dt.Rows.Count > 0)
        {
            string title = string.Empty;

            switch (tbl)
            {
                case "galleryfa":
                    title = "گالری";
                    break;
                case "galleryen":
                    title = "Gallery";
                    break;
                case "galleryar":
                    title = "معارض";
                    break;
                default:
                    break;
            }

            msg = string.Format("<li>{0}<ul>", title);

            if (!isGoogle)
                for (int i = 0; i < dt.Rows.Count; i++)
                    msg += string.Format("<li><a href=\"javascript:;\" onclick=\"fetchGallery('{1}');\">{0}</a></li>", dt.Rows[i][0].ToString().Trim(), tbl + "/" + dt.Rows[i][0].ToString().Trim());
            else
                for (int i = 0; i < dt.Rows.Count; i++)
                    msg += string.Format("<li><a href=\"{1}\">{0}</a></li>", dt.Rows[i][0].ToString().Trim(), GenerateURL("fetchgallery", "gallery", tbl + "/" + dt.Rows[i][0].ToString().Trim(), langGoogle));

            msg += "</ul></li>";
        }

        return msg;
    }

    public DataSet NodesAllTrees()
    {
        DataSet ds = new DataSet();

        ds.Tables.Add(NodesAll("pagesfa"));
        ds.Tables.Add(NodesAll("pagesen"));

        return ds;
    }

    public DataTable NodesAll(string tbl)
    {
        DataTable dt = new DataTable();

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

        return dt;
    }

    public DataSet GalleryDefAllTables()
    {
        DataSet ds = new DataSet();

        ds.Tables.Add(GalleryDefAll("galleryfa"));
        ds.Tables.Add(GalleryDefAll("galleryen"));

        return ds;
    }

    public DataTable GalleryDefAll(string tbl)
    {
        DataTable dt = new DataTable();
        tbl = tbl.Trim();

        try
        {
            dt.TableName = tbl;
            dt.Columns.Add("id", Type.GetType("System.String"));

            DataRow dr = dt.NewRow();

            switch (tbl)
            {
                case "galleryfa":
                    dr[0] = "گالری";
                    break;
                case "galleryen":
                    dr[0] = "Gallery";
                    break;
                default:
                    break;
            }

            dt.Rows.Add(dr);

            dt.AcceptChanges();
        }
        catch
        {
        }
        finally
        {
        }

        return dt;
    }

    [WebMethod]
    public string SiteMap(string tbl)
    {
        string mnuTitle = string.Empty;
        string gallery = string.Empty;

        string expandStr = string.Empty;
        string collapseStr = string.Empty;

        switch (tbl)
        {
            case "pagesfa":
                mnuTitle = rootTitleFa;
                gallery = "galleryfa";
                expandStr = "<img src=\"smopen.fa.gif\" title=\"گشودن تمامی\" />";
                collapseStr = "<img src=\"smclosed.fa.gif\" title=\"بستن تمامی\" />";
                break;
            case "pagesen":
                mnuTitle = rootTitleEn;
                gallery = "galleryen";
                expandStr = "<img src=\"smopen.gif\" title=\"Expand All\" />";
                collapseStr = "<img src=\"smclosed.gif\" title=\"Collapse All\" />";
                break;
            default:
                break;
        }

        DataTable dt = NodesAll(tbl);

        System.Web.UI.WebControls.TreeView trv = new System.Web.UI.WebControls.TreeView();
        System.Web.UI.WebControls.TreeNode root = new System.Web.UI.WebControls.TreeNode();

        root.Text = "root";
        root.Value = mnuTitle;
        trv.Nodes.Add(root);

        siteMap = GenSrcURLSiteMap(tbl) + string.Format("<div class=\"dvSiteMap\">" +
                  "<center>" +
                  "<a href=\"javascript:;\" onclick=\"ddtreemenu.flatten('siteMapTree', 'contact');\">{0}</a>" +
                  "&nbsp;&nbsp;&nbsp;" +
                  "<a href=\"javascript:;\" onclick=\"ddtreemenu.flatten('siteMapTree', 'expand');\">{1}</a>" +
                  "</center>" +
                  "<ul id=\"siteMapTree\" class=\"treeview\">", collapseStr, expandStr);

        GetNodesFromTable(trv.Nodes[0], dt);

        GetMenusFromNodes(trv.Nodes[0], tbl, false, new string[] { });

        if (isMenuULOpened)
        {
            siteMap += "</ul></li>";
            isMenuULOpened = false;
        }

        dt = GalleryDefAll(gallery);
        siteMap += GetGalleryMenues(gallery, dt);

        siteMap += "</ul></div>";

        return siteMap;
    }

    public string SiteMapGoogle(string tbl, bool isGoogle, string langGoogle)
    {
        this.isGoogle = isGoogle;
        this.langGoogle = langGoogle;
        return SiteMap(tbl);
    }

    public string GetSearchFormGoogle(string lang)
    {
        string frm = GenSrcURLSearch("pages" + lang);

        switch (lang)
        {
            case "fa":
                frm += "<div class=\"dvSearchForm\">" +
                      "<h5>جهت جستجو عبارت مورد نظرتان را وارد نمائيد:</h5>" +
                      "عبارت مورد نظر:&nbsp;" +
                      "<input type=\"text\" class=\"textBox\" id=\"txtKeywords\" onkeypress=\"javascript: if (event.which == 13) findKeywords();\" />&nbsp;<input type=\"button\" value=\"جستجو\" class=\"button\" onclick=\"findKeywords();\" />" +
                      "</div>";
                break;
            case "en":
                frm += "<div class=\"dvSearchForm\">" +
                      "<h5>Please enter your phrase to find:</h5>" +
                      "Phrase:&nbsp;" +
                      "<input type=\"text\" class=\"textBox\" id=\"txtKeywords\" onkeypress=\"javascript: if (event.which == 13) findKeywords();\" />&nbsp;<input type=\"button\" value=\"Search\" class=\"button\" onclick=\"findKeywords();\" />" +
                      "</div>";
                break;
            default:
                break;
        }

        return frm;
    }
}

