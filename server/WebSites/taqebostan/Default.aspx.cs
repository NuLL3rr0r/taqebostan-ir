using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    private Master core = new Master();

    private bool isBrowser = false;
    private string browser = string.Empty;

    private string homePage = "7VoxVCLWySgDi148Q6hY.html";
    private string langPage = "7VoxVCLWySgDi148Q6hY_Lang.html";

    private string websiteMenus = string.Empty;
    private string lang = string.Empty;

    private bool isMenuDivOpened = false;
    private bool redirectIfFailed = false;

    string path = string.Empty;

    public static string rootTitleFa = "منوهاي وب سايت";
    public static string rootTitleEn = "Website Menus";

    public static string urlHashKey = "§";

    private string GetQueryString(string query)
    {
        return Request.QueryString[query];
    }

    private string GetDomainName()
    {
        return Request.Url.Host.ToString();
    }

    private void WriteCookie(string name, string value)
    {
        HttpCookie cookie = new HttpCookie(name);
        cookie.HttpOnly = false;
        cookie.Value = value;

        /*DateTime dtNow = DateTime.Now;
        TimeSpan tsMinute = new TimeSpan(0, 0, 1, 0);
        cookie.Expires = dtNow + tsMinute;*/
        cookie.Expires = DateTime.Now.AddDays(365);

        Response.Cookies.Add(cookie);
    }

    private string ReadCookie(string name)
    {
        string value = string.Empty;

        try
        {
            HttpCookie cookie = Request.Cookies[name];
            cookie.HttpOnly = false;

            if (null != cookie)
                value = cookie.Value.ToString();
        }
        catch
        {
        }
        finally
        {
        }

        return value;
    }

    private void DetectLang()
    {
        lang = GetQueryString("lang");

        switch (lang)
        {
            case "fa":
                break;
            case "en":
                break;
            case "home":
                break;
            default:
                lang = ReadCookie("lang");
                switch (lang)
                {
                    case "fa":
                        break;
                    case "en":
                        break;
                    case "home":
                        break;
                    default:
                        lang = "home";
                        WriteCookie("lang", lang);
                        break;
                }
                Response.Redirect("./?lang=" + lang, true);
                break;
        }

        WriteCookie("lang", lang);
    }

    private string DecryptReqID(string reqType)
    {
        try
        {
            return EncDec.Decrypt(GetQueryString(reqType), urlHashKey);
        }
        catch
        {
            redirectIfFailed = true;
        }
        finally
        {
        }

        return string.Empty;
    }

    private string GetLayoutFile(string file)
    {
        string pg = string.Empty;

        try
        {
            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                pg = sr.ReadToEnd();
            }
        }
        catch
        {
        }

        return pg;
    }

    private string GetLayout()
    {
        return GetLayoutFile(homePage);
    }

    private string GetLayoutLangPage()
    {
        return GetLayoutFile(langPage);
    }

    /*private string GetRecommends()
    {
        string rec = string.Empty;

        if (!browser.Contains("Gecko"))
        {
            rec = "<div class=\"dvRecommends\">{0}</div>";

            switch (lang)
            {
                case "fa":
                    rec = string.Format(rec, "كاربر گرامي جهت استفاده ي بهينه از وب سايت مرورگر فاير فاكس را توصيه مي نمائيم؛<a href=\"http://www.getfirefox.com/\" target=\"_blank\">دريافت فاير فاكس</a>");
                    break;
                case "en":
                    rec = string.Format(rec, "Tips: for best performance, we recommends <a href=\"http://www.getfirefox.com/\" target=\"_blank\" title=\"Get Firefox\">Firefox</a>.");
                    break;
                default:
                    break;
            }
        }

        return rec;
    }*/

    private string FixGoogleLayout(string pg)
    {
        if (isBrowser)
        {
            pg = pg.Replace("{Google JS Header}", "<script type=\"text/javascript\" src=\"layout.js\"></script>");
            pg = pg.Replace("{Google Preloader Div}", "<div id=\"dvPreLoader\">{No Script Section}<img src=\"preprogress.gif\" id=\"loadingImage\" /></div>");
            pg = pg.Replace("{Google Main Contents}", "<script language=\"javascript\">document.write(loadingText);</script>");
            //pg = pg.Replace("{Google Recommends Scripts}", "<script language=\"javascript\">fixRecommends();</script>");
            pg = pg.Replace("{Google Last Scripts}", "<script type=\"text/javascript\">fixPreLoader();reqType={ReqType};fetchX={FetchX};</script>");
        }
        else
        {
            pg = pg.Replace("{Google JS Header}", string.Empty);
            pg = pg.Replace("{Google Preloader Div}", string.Empty);
            pg = pg.Replace("{Contex Div}", string.Empty);
            pg = pg.Replace("{Google Main Contents}", "{GooglePage}");

            //pg = pg.Replace("{Google Recommends Scripts}", string.Empty);
            pg = pg.Replace("{Google Last Scripts}", string.Empty);
        }

        //pg = pg.Replace("{Recommends FF}", GetRecommends());

        return pg;
    }

    private string ReadMainPage()
    {
        if (lang == "home")
            //return GetLayoutLangPage();
            return core.LayoutHomeRead();

        string pg = string.Empty;

        try
        {
            pg = FixGoogleLayout(core.LayoutRead());
            //pg = FixGoogleLayout(GetLayout());

            GetPageMenus();

            pg = pg.Replace("{Title}", GetWTitle(lang));

            pg = pg.Replace("{Menu Area}", websiteMenus);
            pg = pg.Replace("{Top of page Link}", GetTopOfPageLink());
            pg = pg.Replace("{Navigation Area}", GetNavigation());

            pg = pg.Replace("{No Script Section}", GetNoScript());
            //pg = pg.Replace("{Eng Lang Style}", GetSpecLangStyles());
            pg = pg.Replace("{Eng Lang Style}", GetEngStyles());
            pg = pg.Replace("{IP}", ClientIP());




            if (isBrowser)
            {
                pg = pg.Replace("{Contex Div}", "<div id=\"ie5menu\" class=\"skinContexMenu\" onMouseover=\"highlightie5(event)\" onMouseout=\"lowlightie5(event)\" onClick=\"jumptoie5(event)\" display:none>{Contex Area}</div>");
                pg = pg.Replace("{Contex Area}", GetContex());
            }




            string req = GetQueryString("req");
            string curPage = string.Empty;

            switch (req)
            {
                case "fetchpage":
                    curPage = DecryptReqID("page");
                    break;
                case "fetchgallery":
                    //curPage = DecryptReqID("gallery");
                    break;
                case "sitemap":
                    break;
                case "search":
                    break;
                case "links":
                    switch (lang)
                    {
                        case "fa":
                            curPage = "سایت های مرتبط";
                            break;
                        case "en":
                            curPage = "Links";
                            break;
                        default:
                            break;
                    }
                    break;
                case "contactus":
                    break;
                case "aboutus":
                    switch (lang)
                    {
                        case "fa":
                            curPage = "درباره ی ما";
                            break;
                        case "en":
                            curPage = "About us";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    redirectIfFailed = true;
                    break;
            }

            if (isBrowser)
            {
                if (!redirectIfFailed)
                {
                    pg = pg.Replace("{ReqType}", string.Format("'{0}'", req));
                    pg = pg.Replace("{FetchX}", string.Format("'{0}'", curPage));
                }
                else
                {
                    pg = pg.Replace("{ReqType}", "'fetchpage'");
                    pg = pg.Replace("{FetchX}", "mainPage");
                }
            }
            else
            {
                switch (req)
                {
                    case "fetchpage":
                        pg = pg.Replace("{GooglePage}", core.FetchPage(curPage, "pages" + lang));
                        break;
                    case "fetchgallery":
                        pg = pg.Replace("{GooglePage}", core.FetchGallery("gallery" + lang));
                        break;
                    case "sitemap":
                        pg = pg.Replace("{GooglePage}", core.SiteMapGoogle("pages" + lang, true, lang));
                        break;
                    case "search":
                        pg = pg.Replace("{GooglePage}", core.GetSearchFormGoogle(lang));
                        break;
                    case "links":
                        pg = pg.Replace("{GooglePage}", core.FetchPage(curPage, "pages" + lang));
                        break;
                    case "contactus":
                        pg = pg.Replace("{GooglePage}", core.ContactUSForm("contactlist" + lang));
                        break;
                    case "aboutus":
                        pg = pg.Replace("{GooglePage}", core.FetchPage(curPage, "pages" + lang));
                        break;
                    default:
                        switch (lang)
                        {
                            case "fa":
                                curPage = "صفحه اصلی";
                                break;
                            case "en":
                                curPage = "Home Page";
                                break;
                            default:
                                curPage = "صفحه اصلی";
                                break;
                        }
                        pg = pg.Replace("{GooglePage}", core.FetchPage(curPage, "pages" + lang));
                        break;
                }
            }
        }
        catch
        {
            pg = "<center><br/><br/><br/>" +
                 "<h5 style=\"color:#990000;font-family:Tahoma;direction:ltr;letter-spacing:2px;\">Internal Server Error! Please try again later; or, Contact Administrator...</h5>" +
                 "<h6 style=\"color:#990000;font-family:Tahoma;direction:rtl;letter-spacing:1px;\">کاربر گرامی در حال حاضر به دلیل خطای داخلی در وب سرور امکان دسترسی به وب سایت وجود ندارد. لطفا بعدا مراجعه نمائید. در صورت لزوم مدیر وب سایت را آگاه نمائید&hellip;</h6>" +
                 "<h6 style=\"color:#990000;font-family:Tahoma;direction:rtl;letter-spacing:1px;\">خطأ خادم داخلي! الرجاء المحاولة مرة أخرى في وقت لاحق ؛ أو ، اتصل بمدير&hellip;</h6>" +
                 "</center>";
        }
        finally
        {
        }

        return pg;
    }

    private string GetEngStyles()
    {
        string styles = string.Empty;

        if (lang == "en")
            styles = "<link rel=\"stylesheet\" type=\"text/css\" href=\"layout_en.css\" /><link rel=\"stylesheet\" type=\"text/css\" href=\"layout.add_en.css\" /><!--[if lt IE 7]><style type=\"text/css\">@import \"layout-lt-ie-7_en.css\";</style><![endif]-->";

        return styles;
    }

/*    private string GetSpecLangStyles()
    {
        string styles = string.Empty;

        switch (lang)
        {
            case "en":
                styles = "<link rel=\"stylesheet\" type=\"text/css\" href=\"layout_en.css\" /><link rel=\"stylesheet\" type=\"text/css\" href=\"layout.add_en.css\" /><!--[if lt IE 7]><style type=\"text/css\">@import \"layout-lt-ie-7_en.css\";</style><![endif]-->";
                break;
            default:
                break;
        }

        return styles;
    }*/

    private string GetNoScript()
    {
        string noscript = "<noscript><div id=\"dvNoScript\">{0}</div></noscript>";
        switch (lang)
        {
            case "fa":
                noscript = string.Format(noscript, "<h5 style=\"direction:rtl;font-family:Tahoma;font-size:12px;line-height:23px;\">كاربر گرامي مرورگر شما از جاوا اسكريپت پشتيباني نمي نمايد، و يا اينكه جاوا اسكريپت در مرورگر شما فعال نمي باشد...<br/>جهت ادامه ما مرورگر فاير فاكس را توصيه مي نمائيم؛ <a href=\"http://www.getfirefox.com/\" target=\"_blank\">دريافت فاير فاكس</a></h5>");
                break;
            case "en":
                noscript = string.Format(noscript, "<h4 style=\"direction:ltr;\">Your browser doesn't support javascript, or javascript doesn't enabled on your browser...<br/>For continue we recommends <a href=\"http://www.getfirefox.com/\" target=\"_blank\" title=\"Get Firefox\">Firefox</a>.</h4>");
                break;
            default:
                break;
        }

        return noscript;
    }

    private string GetTopOfPageLink()
    {
        string link = "<a href=\"#top\" title=\"{0}\"><img src=\"up.png\" width=\"24\" height=\"24\" alt=\"{0}\" /></a>";
        string top = string.Empty;

        switch (lang)
        {
            case "fa":
                top = "بالای صفحه";
                break;
            case "en":
                top = "Top of page";
                break;
            default:
                break;
        }

        return string.Format(link, top);
    }

    private void GetNodesFromTable(TreeNode node, DataTable dt)
    {
        TreeNodeCollection nodes = node.ChildNodes;
        DataRow dr;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dr = dt.Rows[i];
            string name = dr["pg"].ToString().Trim();

            if (dr["parent"].ToString().Trim() == node.Text && dr["fullpath"].ToString().Trim().Replace("\\", "/") == node.ValuePath + "/" + name)
            {
                TreeNode tn = new TreeNode();
                tn.Text = name;
                tn.Value = name;
                nodes.Add(tn);

                GetNodesFromTable(tn, dt);
            }
        }
    }

    private void GetMenusFromNodes(TreeNode node, string lang, bool hasChild, string[] lastChild)
    {
        TreeNodeCollection nodes = node.ChildNodes;

        foreach (TreeNode n in nodes)
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

                    websiteMenus += string.Format("<li><a href=\"javascript:;\">{0}</a><ul>", name);

                    GetMenusFromNodes(n, lang, hasChild, lastChild);
                }
                else
                {
                    if (isBrowser)
                        websiteMenus += string.Format("<li><a href=\"javascript:;\" onclick=\"fetchPage('{1}');\">{0}</a></li>", name, path);
                    else
                        websiteMenus += string.Format("<li><a href=\"{1}\">{0}</a></li>", name, GenerateURL("fetchpage", "page", path, this.lang));
                }

                if (len > 0)
                {
                    for (int i = 0; i < lastChild.Length; i++)
                    {
                        if (lastChild[i] == n.ValuePath)
                        {
                            websiteMenus += "</ul></li>";
                        }
                    }
                }

            }
            else
            {
                if (hasChild)
                {
                    websiteMenus += "</ul></div>";
                    isMenuDivOpened = false;
                    hasChild = false;
                }
                websiteMenus += string.Format("<div class=\"mnuTitle\">{0}</div>", name);
                if (n.ChildNodes.Count > 0)
                {
                    websiteMenus += "<div class=\"mlmenu vertical blindh arrow blackwhite left\"><ul>";
                    isMenuDivOpened = true;
                    hasChild = true;

                    GetMenusFromNodes(n, lang, hasChild, lastChild);
                }
            }
        }
    }

    private void GetPageMenus()
    {
        string mnuTitle = string.Empty;
        string tbl = string.Empty;
        string gallery = string.Empty;

        switch (lang)
        {
            case "fa":
                mnuTitle = rootTitleFa;
                tbl = "pagesfa";
                gallery = "galleryfa";
                break;
            case "en":
                mnuTitle = rootTitleEn;
                tbl = "pagesen";
                gallery = "galleryen";
                break;
            default:
                break;
        }

        DataTable dt = core.NodesAll(tbl);

        TreeView trv = new TreeView();
        TreeNode root = new TreeNode();

        root.Text = "root";
        root.Value = mnuTitle;
        trv.Nodes.Add(root);

        GetNodesFromTable(trv.Nodes[0], dt);

        GetMenusFromNodes(trv.Nodes[0], tbl, false, new string[] { });
        if (isMenuDivOpened)
        {
            websiteMenus += "</ul></div>";
            isMenuDivOpened = false;
        }

        dt = core.GalleryDefAll(gallery);
        websiteMenus += GetGalleryMenues(gallery, dt);
    }

    private string ReplaceMenu(string msg)
    {
        switch (lang)
        {
            case "fa":
                msg = string.Format(msg, "صفحه اصلی", "نقشه سایت", "جستجو", "سایت های مرتبط", "تماس با ما", "درباره ی ما", "چاپ");
                break;
            case "en":
                msg = string.Format(msg, "Home Page", "Site Map", "Search", "Links", "Contact us", "About us", "Print");
                break;
            default:
                break;
        }

        if (!isBrowser)
            msg = msg.Replace("[lang]", lang);

        return msg;
    }

    private string GetContex()
    {
        string msg = "<div class=\"menuitems\" url=\"#top\" onclick=\"fetchPage('{0}');\">{0}</div>" +
                     "<hr />" +
                     "<div class=\"menuitems\" url=\"#top\" onclick=\"siteMap();\">{1}</div>" +
                     "<div class=\"menuitems\" url=\"#top\" onclick=\"searchForm();\">{2}</div>" +
                     "<div class=\"menuitems\" url=\"#top\" onclick=\"fetchPage('{3}');\">{3}</div>" +
                     "<hr />" +
                     "<div class=\"menuitems\" url=\"#top\" onclick=\"contactForm();\">{4}</div>" +
                     "<div class=\"menuitems\" url=\"#top\" onclick=\"fetchPage('{5}');\">{5}</div>" +
                     "<hr />" +
                     "<div class=\"menuitems\" url=\"#top\" onclick=\"docPrint();\">{6}</div>";

        msg = ReplaceMenu(msg);

        string top = string.Empty;
        switch (lang)
        {
            case "fa":
                top = "بالای صفحه";
                break;
            case "en":
                top = "Top of page";
                break;
            default:
                break;
        }

        msg = string.Format("<div class=\"menuitems\" url=\"#top\">{0}</div>", top) + msg;

        return msg;
    }
    
    private string GetNavigation()
    {
        string msg = string.Empty;

        if (isBrowser)
            msg = "<li><a href=\"#top\" onclick=\"fetchPage('{0}');\">{0}</a></li>" +
                  "<li><a href=\"#top\" onclick=\"siteMap();\">{1}</a></li>" +
                  "<li><a href=\"#top\" onclick=\"searchForm();\">{2}</a></li>" +
                  "<li><a href=\"#top\" onclick=\"fetchPage('{3}');\">{3}</a></li>" +
                  "<li><a href=\"#top\" onclick=\"contactForm();\">{4}</a></li>" +
                  "<li><a href=\"#top\" onclick=\"fetchPage('{5}');\">{5}</a></li>" +
                  "<li><a href=\"javascript:;\" onclick=\"docPrint();\">{6}</a></li>";
        else
            msg = "<li><a href=\".?lang=[lang]\">{0}</a></li>" +
                  "<li><a href=\".?lang=[lang]&req=sitemap\">{1}</a></li>" +
                  "<li><a href=\".?lang=[lang]&req=search\">{2}</a></li>" +
                  //"<li><a href=\"javascript:;\">{2}</a></li>" +
                  "<li><a href=\".?lang=[lang]&req=links\">{3}</a></li>" +
                  "<li><a href=\".?lang=[lang]&req=contactus\">{4}</a></li>" +
                  "<li><a href=\".?lang=[lang]&req=aboutus\">{5}</a></li>" +
                  "<li><a href=\"javascript:;\" onclick=\"docPrint();\">{6}</a></li>";

        return ReplaceMenu(msg);
    }

    private string GetGalleryMenues(string tbl, DataTable dt)
    {
        string msg = string.Empty;

        if (dt.Rows.Count > 0)
        {
            string title = string.Empty;

            switch (lang)
            {
                case "fa":
                    title = "گالری";
                    break;
                case "en":
                    title = "Gallery";
                    break;
                default:
                    break;
            }

            msg = string.Format("<div class=\"mnuTitle\">{0}</div>", title);
            msg += "<div class=\"mlmenu vertical blindh arrow blackwhite left\"><ul>";

            if (isBrowser)
                for (int i = 0; i < dt.Rows.Count; i++)
                    msg += string.Format("<li><a href=\"javascript:;\" onclick=\"fetchGallery('{1}');\">{0}</a></li>", dt.Rows[i][0].ToString().Trim(), tbl + "/" + dt.Rows[i][0].ToString().Trim());
            else
                for (int i = 0; i < dt.Rows.Count; i++)
                    msg += string.Format("<li><a href=\"{1}\">{0}</a></li>", dt.Rows[i][0].ToString().Trim(), GenerateURL("fetchgallery", "gallery", tbl + "/" + dt.Rows[i][0].ToString().Trim(), lang));

            msg += "</ul></div>";
        }

        return msg;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~");
        path += path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? string.Empty : Path.DirectorySeparatorChar.ToString();

        browser = Browser();
        isBrowser = DetBrowser();
        DetectLang();

        homePage = path + homePage;
        langPage = path + langPage;

        Response.Write(ReadMainPage());

        /*if (!isBrowser)
            GoGoogle();*/
    }

    private string ClientIP()
    {
        return Request.ServerVariables["REMOTE_ADDR"];
    }

    private string Browser()
    {
        return Request.ServerVariables["HTTP_USER_AGENT"];
    }

    private bool DetBrowser()
    {
        bool isGecko = browser.Contains("Gecko");
        bool isKHTML = browser.Contains("KHTML");
        bool isMSIE = browser.Contains("MSIE");
        bool isOpera = browser.Contains("Opera");

        //return (isKHTML || isMSIE || isOpera);
        return (isGecko || isKHTML || isMSIE || isOpera);
    }

/*    private void GoGoogle()
    {
        string browser = Browser();
        bool isGoogle = browser.Contains("Googlebot");
        //bool isGoogle = true;

        if (!isGoogle)
            return;

        string url = "http://www.kermanshahchhto.ir/?" + Request.QueryString.ToString();

        core.SendGoogleWarn(url, ClientIP());
    }*/



    public static string GenerateURL(string req, string var, string value, string lang)
    {
        return string.Format("./?lang={0}&req={1}&{2}={3}", lang, req, var, EncDec.Encrypt(value, urlHashKey).Replace("+", "%2B").Replace("/", "%2F").Replace("=", "%3D"));
    }

    public string GetWTitle(string lang)
    {
        string title = string.Empty;

        switch (lang)
        {
            case "fa":
                title = "تاق بستان";
                break;
            case "en":
                title = "Taq-e Bostan";
                break;
            default:
                break;
        }

        return title;
    }
}
