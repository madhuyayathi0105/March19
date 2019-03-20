using System;//---------21/7/12(,encrypt app_no in query string))
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.ListItem;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Configuration;
using FarPoint.Web.Spread;
using FarPoint.Excel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Security.Cryptography;
public partial class About : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection tcon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();
    //Boolean ac,acc;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    int j = 0;
    int bloodcount;
    Boolean Cellclick = false;
    int bc;
    string search1;
    string appno;
    int roll = 0;
    string[] castevalue = new string[200];
    string[] castecode = new string[200];
    //int[] castecode = new int[44];
    int[] casteindex = new int[200];
    string[] seatvalue = new string[200];
    string[] seatcode = new string[200];
    int[] seatindex = new int[200];
    string[] directvalue = new string[200];
    string[] directcode = new string[200];
    int[] directindex = new int[200];
    string[] bloodvalue = new string[200];
    string[] bloodcode = new string[200];
    //int[] bloodcode = new int[200];
    int[] bloodindex = new int[200];
    string[] staffvalue = new string[200];
    string[] staffcode = new string[200];
    int[] staffindex = new int[200];
    string[] othersvalue = new string[200];
    string[] otherscode = new string[200];
    int[] othersindex = new int[200];
    string[] religvalue = new string[200];
    string[] religcode = new string[200];
    int[] religindex = new int[200];
    string[] commvalue = new string[200];
    string[] commcode = new string[200];
    int[] commindex = new int[200];
    string[] regionvalue = new string[200];
    string[] regioncode = new string[200];
    int[] regionindex = new int[200];
    string[] mtonguevalue = new string[200];
    string[] mtonguecode = new string[200];
    int[] mtongueindex = new int[200];
    string[] foccuvalue = new string[200];
    string[] foccucode = new string[200];
    int[] foccuindex = new int[200];
    string[] fqualvalue = new string[200];
    string[] fqualcode = new string[200];
    int[] fqualindex = new int[200];
    string[] moccuvalue = new string[200];
    string[] moccucode = new string[200];
    int[] moccuindex = new int[200];
    string[] mqualvalue = new string[200];
    string[] mqualcode = new string[200];
    int[] mqualindex = new int[200];
    string[] degreevalue = new string[200];
    string[] degreecode = new string[200];
    int[] degreeindex = new int[200];
    string[] branchvalue = new string[200];
    string[] branchcode = new string[200];
    int[] branchindex = new int[200];
    string[] semvalue = new string[200];
    string[] semcode = new string[200];
    int[] semindex = new int[200];
    string[] secvalue = new string[200];
    string[] seccode = new string[200];
    int[] secindex = new int[200];
    string[] colvalue = new string[200];
    string[] colcode = new string[200];
    int[] colindex = new int[200];
    int checkstaff = 0;
    int checkothers = 0;
    int checkdirect = 0;
    int checkseat = 0;
    int checkblood = 0;
    int checkcaste = 0;
    int checkbranch = 0;
    int checkdegree = 0;
    int checkfoccu = 0;
    int checkmoccu = 0;
    int checkfqual = 0;
    int checkmqual = 0;
    int checkmtongue = 0;
    int checkrelig = 0;
    int checkregion = 0;
    int checkcomm = 0;
    int checksem = 0;
    int checksec = 0;
    int checkcol = 0;
    static int staffcnt = 0;
    static int directcnt = 0;
    static int otherscnt = 0;
    static int castecnt = 0;
    static int bloodcnt = 0;
    static int seatcnt = 0;
    static int foccucnt = 0;
    static int moccucnt = 0;
    static int fqualcnt = 0;
    static int mqualcnt = 0;
    static int degreecnt = 0;
    static int semcnt = 0;
    static int seccnt = 0;
    static int branchcnt = 0;
    static int mtonguecnt = 0;
    static int religcnt = 0;
    static int regioncnt = 0;
    static int commcnt = 0;
    static int colcnt = 0;
    string order = "";
    static int cook = 0;
    static string college_code = "";
    DAccess2 da = new DAccess2();//Added By Srinath 7/5/2013
    string PageLogOut = "";

    //string[] slt = new string[55];
    //string[] si= new string[55];

    protected void Page_UnLoad(object sender, EventArgs e)
    {
    }

    public void loadorder()
    {
        Response.Cookies["order"].Expires = DateTime.Now.AddDays(-1);
        for (int item = 0; item < cblsearch.Items.Count; item++)
        {
            if (cblsearch.Items[item].Selected == true)
            {
                if (order == "")
                {
                    order = item.ToString();
                }
                else
                {
                    order = order + "," + item.ToString();
                }
            }
        }
        if (order != "")
        {
            Response.Cookies["order"].Value = order.ToString();
            Response.Cookies["order"].Expires = DateTime.Now.AddMonths(2);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
        //            Response.Redirect(redrURI, false);
        //            return;
        //        }
        //    }
        //}
        //if (Session["collegecode"] == null)
        //{
        //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
        //    Response.Redirect(redrURI, false);
        //    return;
        //}
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }

        FpSpread1.CommandBar.Visible = false;

        //FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        //FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        //FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        if (IsPostBack)
        {

            college_code = "";
            //coll
            //PlaceHoldercollege.Controls.Clear();
            //checkdirect = 0;
            //j = 0;
            //for (int i = 0; i < ddlcollege.Items.Count; i++)
            //{
            //    if (ddlcollege.Items[i].Selected == true)
            //    {
            //        colvalue[j] = ddlcollege.Items[i].Text;
            //        checkcol = checkcol + 1;
            //        colcode[j] = ddlcollege.Items[i].Value.ToString();
            //        colindex[j] = i;
            //        j++;
            //    }
            //}
            //if ((checkcol + 1) == colcnt)
            //    colcnt = colcnt - 2;
            //if ((ViewState["icolcontrol"] != null))
            //{
            //    for (int i = 0; i < colcnt; i++)
            //    {
            //        if (ViewState["lcolcontrol"] != null)
            //        {
            //            Label lbl = collabel();
            //            lbl.Text = " " + colvalue[i] + " ";
            //            lbl.ID = "lbl1c4-" + colcode[i].ToString();
            //        }
            //        ImageButton ib = colimage();
            //        ib.ID = "imgbut1c4_" + colcode[i].ToString();
            //        ib.CommandArgument = colindex[i].ToString();
            //        ib.Click += new ImageClickEventHandler(colimg_Click);
            //    }
            //}
            //
            PlaceHolderdirect.Controls.Clear();
            checkdirect = 0;
            j = 0;
            for (int i = 0; i < cbldirect.Items.Count; i++)
            {
                if (cbldirect.Items[i].Selected == true)
                {
                    directvalue[j] = cbldirect.Items[i].Text;
                    checkdirect = checkdirect + 1;
                    directcode[j] = cbldirect.Items[i].Value.ToString();
                    directindex[j] = i;
                    j++;
                }
            }
            if ((checkdirect + 1) == directcnt)
                directcnt = directcnt - 2;
            if ((ViewState["idirectcontrol"] != null))
            {
                for (int i = 0; i < directcnt; i++)
                {
                    if (ViewState["ldirectcontrol"] != null)
                    {
                        Label lbl = directlabel();
                        lbl.Text = " " + directvalue[i] + " ";
                        lbl.ID = "lbl14-" + directcode[i].ToString();
                    }
                    ImageButton ib = directimage();
                    ib.ID = "imgbut14_" + directcode[i].ToString();
                    ib.CommandArgument = directindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(directimg_Click);
                }
            }
            PlaceHolderstaff.Controls.Clear();
            checkstaff = 0;
            j = 0;
            for (int i = 0; i < cblstaffname.Items.Count; i++)
            {
                if (cblstaffname.Items[i].Selected == true)
                {
                    staffvalue[j] = cblstaffname.Items[i].Text;
                    checkstaff = checkstaff + 1;
                    staffcode[j] = cblstaffname.Items[i].Value.ToString();
                    staffindex[j] = i;
                    j++;
                }
            }
            if ((checkstaff + 1) == staffcnt)
                staffcnt = staffcnt - 2;
            if ((ViewState["istaffcontrol"] != null))
            {
                for (int i = 0; i < staffcnt; i++)
                {
                    if (ViewState["lstaffcontrol"] != null)
                    {
                        Label lbl = stafflabel();
                        lbl.Text = " " + staffvalue[i] + " ";
                        lbl.ID = "lbl15-" + staffcode[i].ToString();
                    }
                    ImageButton ib = staffimage();
                    ib.ID = "imgbut15_" + staffcode[i].ToString();
                    ib.CommandArgument = staffindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(staffimg_Click);
                }
            }
            //PlaceHolderothers.Controls.Clear();
            //checkothers = 0;
            //j = 0;
            //for (int i = 0; i < cblothers.Items.Count; i++)
            //{
            //    if (cblothers.Items[i].Selected == true)
            //    {
            //        othersvalue[j] = cblothers.Items[i].Text;
            //        checkothers = checkothers + 1;
            //        otherscode[j] = cblothers.Items[i].Value.ToString();
            //        othersindex[j] = i;
            //        j++;
            //    }
            //}
            //if ((checkothers + 1) == otherscnt)
            //    otherscnt = otherscnt - 2;
            //if ((ViewState["iotherscontrol"] != null))
            //{
            //    for (int i = 0; i < otherscnt; i++)
            //    {
            //        if (ViewState["lotherscontrol"] != null)
            //        {
            //            Label lbl = otherslabel();
            //            lbl.Text = " " + othersvalue[i] + " ";
            //            lbl.ID = "lbl16-" + otherscode[i].ToString();
            //        }
            //        ImageButton ib = othersimage();
            //        ib.ID = "imgbut16_" + otherscode[i].ToString();
            //        ib.CommandArgument = othersindex[i].ToString();
            //        ib.Click += new ImageClickEventHandler(othersimg_Click);
            //    }
            //}
            PlaceHolderseattype.Controls.Clear();
            checkseat = 0;
            j = 0;
            for (int i = 0; i < cblseattype.Items.Count; i++)
            {
                if (cblseattype.Items[i].Selected == true)
                {
                    seatvalue[j] = cblseattype.Items[i].Text;
                    checkseat = checkseat + 1;
                    seatcode[j] = cblseattype.Items[i].Value.ToString();
                    seatindex[j] = i;
                    j++;
                }
            }
            if ((checkseat + 1) == seatcnt)
                seatcnt = seatcnt - 2;
            if ((ViewState["iseatcontrol"] != null))
            {
                for (int i = 0; i < seatcnt; i++)
                {
                    if (ViewState["lseatcontrol"] != null)
                    {
                        Label lbl = seatlabel();
                        lbl.Text = " " + seatvalue[i] + " ";
                        lbl.ID = "lbl1-" + seatcode[i].ToString();
                    }
                    ImageButton ib = seatimage();
                    ib.ID = "imgbut1_" + seatcode[i].ToString();
                    ib.CommandArgument = seatindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(seatimg_Click);
                }
            }
            PlaceHolderblood.Controls.Clear();
            checkblood = 0;
            j = 0;
            for (int i = 0; i < cblblood.Items.Count; i++)
            {
                if (cblblood.Items[i].Selected == true)
                {
                    bloodvalue[j] = cblblood.Items[i].Text;
                    checkblood = checkblood + 1;
                    bloodcode[j] = cblblood.Items[i].Value.ToString();
                    bloodindex[j] = i;
                    j++;
                }
            }
            if ((checkblood + 1) == bloodcnt)
                bloodcnt = bloodcnt - 2;
            if ((ViewState["ibloodcontrol"] != null))
            {
                for (int i = 0; i < bloodcnt; i++)
                {
                    if (ViewState["lbloodcontrol"] != null)
                    {
                        Label lbl = bloodlabel();
                        lbl.Text = " " + bloodvalue[i] + " ";
                        lbl.ID = "lbl2-" + bloodcode[i].ToString();
                    }
                    ImageButton ib = bloodimage();
                    ib.ID = "imgbut2_" + bloodcode[i].ToString();
                    ib.CommandArgument = bloodindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(bloodimg_Click);
                }
            }
            PlaceHoldercaste.Controls.Clear();
            checkcaste = 0;
            j = 0;
            for (int i = 0; i < cblcaste.Items.Count; i++)
            {
                if (cblcaste.Items[i].Selected == true)
                {
                    castevalue[j] = cblcaste.Items[i].Text;
                    checkcaste = checkcaste + 1;
                    //castecode[j] = Convert.ToInt32(cblcaste.Items[i].Value.ToString());
                    castecode[j] = cblcaste.Items[i].Value.ToString();
                    casteindex[j] = i;
                    j++;
                }
            }
            if ((checkcaste + 1) == castecnt)
                castecnt = castecnt - 2;
            if ((ViewState["icastecontrol"] != null))
            {
                for (int i = 0; i < castecnt; i++)
                {
                    if (ViewState["lcastecontrol"] != null)
                    {
                        Label lbl = castelabel();
                        lbl.Text = " " + castevalue[i] + " ";
                        lbl.ID = "lbl3-" + castecode[i].ToString();
                    }
                    ImageButton ib = casteimage();
                    ib.ID = "imgbut3_" + castecode[i].ToString();
                    ib.CommandArgument = casteindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(casteimg_Click);
                }
            }
            PlaceHolderreligion.Controls.Clear();
            checkrelig = 0;
            j = 0;
            for (int i = 0; i < cblreligion.Items.Count; i++)
            {
                if (cblreligion.Items[i].Selected == true)
                {
                    religvalue[j] = cblreligion.Items[i].Text;
                    checkrelig = checkrelig + 1;
                    religcode[j] = cblreligion.Items[i].Value.ToString();
                    religindex[j] = i;
                    j++;
                }
            }
            if ((checkrelig + 1) == religcnt)
                religcnt = religcnt - 2;
            if ((ViewState["ireligcontrol"] != null))
            {
                for (int i = 0; i < religcnt; i++)
                {
                    if (ViewState["lreligcontrol"] != null)
                    {
                        Label lbl = religlabel();
                        lbl.Text = " " + religvalue[i] + " ";
                        lbl.ID = "lbl4-" + religcode[i].ToString();
                    }
                    ImageButton ib = religimage();
                    ib.ID = "imgbut4_" + religcode[i].ToString();
                    ib.CommandArgument = religindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(religimg_Click);
                }
            }
            PlaceHoldercomm.Controls.Clear();
            checkcomm = 0;
            j = 0;
            for (int i = 0; i < cblcomm.Items.Count; i++)
            {
                if (cblcomm.Items[i].Selected == true)
                {
                    commvalue[j] = cblcomm.Items[i].Text;
                    checkcomm = checkcomm + 1;
                    commcode[j] = cblcomm.Items[i].Value.ToString();
                    commindex[j] = i;
                    j++;
                }
            }
            if ((checkcomm + 1) == commcnt)
                commcnt = commcnt - 2;
            if ((ViewState["icommcontrol"] != null))
            {
                for (int i = 0; i < commcnt; i++)
                {
                    if (ViewState["lcommcontrol"] != null)
                    {
                        Label lbl = commlabel();
                        lbl.Text = " " + commvalue[i] + " ";
                        lbl.ID = "lbl5-" + commcode[i].ToString();
                    }
                    ImageButton ib = commimage();
                    ib.ID = "imgbut5_" + commcode[i].ToString();
                    ib.CommandArgument = commindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(commimg_Click);
                }
            }
            PlaceHolderregion.Controls.Clear();
            checkregion = 0;
            j = 0;
            for (int i = 0; i < cblregion.Items.Count; i++)
            {
                if (cblregion.Items[i].Selected == true)
                {
                    regionvalue[j] = cblregion.Items[i].Text;
                    checkregion = checkregion + 1;
                    regioncode[j] = cblregion.Items[i].Value.ToString();
                    regionindex[j] = i;
                    j++;
                }
            }
            if ((checkregion + 1) == regioncnt)
                regioncnt = regioncnt - 2;
            if ((ViewState["iregioncontrol"] != null))
            {
                for (int i = 0; i < regioncnt; i++)
                {
                    if (ViewState["lregioncontrol"] != null)
                    {
                        Label lbl = regionlabel();
                        lbl.Text = " " + regionvalue[i] + " ";
                        lbl.ID = "lbl6-" + regioncode[i].ToString();
                    }
                    ImageButton ib = regionimage();
                    ib.ID = "imgbut6_" + regioncode[i].ToString();
                    ib.CommandArgument = regionindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(regionimg_Click);
                }
            }
            PlaceHoldermtongue.Controls.Clear();
            checkmtongue = 0;
            j = 0;
            for (int i = 0; i < cblmtongue.Items.Count; i++)
            {
                if (cblmtongue.Items[i].Selected == true)
                {
                    mtonguevalue[j] = cblmtongue.Items[i].Text;
                    checkmtongue = checkmtongue + 1;
                    mtonguecode[j] = cblmtongue.Items[i].Value.ToString();
                    mtongueindex[j] = i;
                    j++;
                }
            }
            if ((checkmtongue + 1) == mtonguecnt)
                mtonguecnt = mtonguecnt - 2;
            if ((ViewState["imtonguecontrol"] != null))
            {
                for (int i = 0; i < mtonguecnt; i++)
                {
                    if (ViewState["lmtonguecontrol"] != null)
                    {
                        Label lbl = mtonguelabel();
                        lbl.Text = " " + mtonguevalue[i] + " ";
                        lbl.ID = "lbl7-" + mtonguecode[i].ToString();
                    }
                    ImageButton ib = mtongueimage();
                    ib.ID = "imgbut7_" + mtonguecode[i].ToString();
                    ib.CommandArgument = mtongueindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(mtongueimg_Click);
                }
            }
            PlaceHolderfoccu.Controls.Clear();
            checkfoccu = 0;
            j = 0;
            for (int i = 0; i < cblfoccu.Items.Count; i++)
            {
                if (cblfoccu.Items[i].Selected == true)
                {
                    foccuvalue[j] = cblfoccu.Items[i].Text;
                    checkfoccu = checkfoccu + 1;
                    foccucode[j] = cblfoccu.Items[i].Value.ToString();
                    foccuindex[j] = i;
                    j++;
                }
            }
            if ((checkfoccu + 1) == foccucnt)
                foccucnt = foccucnt - 2;
            if ((ViewState["ifoccucontrol"] != null))
            {
                for (int i = 0; i < foccucnt; i++)
                {
                    if (ViewState["lfoccucontrol"] != null)
                    {
                        Label lbl = focculabel();
                        lbl.Text = " " + foccuvalue[i] + " ";
                        lbl.ID = "lbl8-" + foccucode[i].ToString();
                    }
                    ImageButton ib = foccuimage();
                    ib.ID = "imgbut8_" + foccucode[i].ToString();
                    ib.CommandArgument = foccuindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(foccuimg_Click);
                }
            }
            PlaceHolderfqual.Controls.Clear();
            checkfqual = 0;
            j = 0;
            for (int i = 0; i < cblfqual.Items.Count; i++)
            {
                if (cblfqual.Items[i].Selected == true)
                {
                    fqualvalue[j] = cblfqual.Items[i].Text;
                    checkfqual = checkfqual + 1;
                    fqualcode[j] = cblfqual.Items[i].Value.ToString();
                    fqualindex[j] = i;
                    j++;
                }
            }
            if ((checkfqual + 1) == fqualcnt)
                fqualcnt = fqualcnt - 2;
            if ((ViewState["ifqualcontrol"] != null))
            {
                for (int i = 0; i < fqualcnt; i++)
                {
                    if (ViewState["lfqualcontrol"] != null)
                    {
                        Label lbl = fquallabel();
                        lbl.Text = " " + fqualvalue[i] + " ";
                        lbl.ID = "lbl9-" + fqualcode[i].ToString();
                    }
                    ImageButton ib = fqualimage();
                    ib.ID = "imgbut9_" + fqualcode[i].ToString();
                    ib.CommandArgument = fqualindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(fqualimg_Click);
                }
            }
            PlaceHoldermoccu.Controls.Clear();
            checkmoccu = 0;
            j = 0;
            for (int i = 0; i < cblmoccu.Items.Count; i++)
            {
                if (cblmoccu.Items[i].Selected == true)
                {
                    moccuvalue[j] = cblmoccu.Items[i].Text;
                    checkmoccu = checkmoccu + 1;
                    moccucode[j] = cblmoccu.Items[i].Value.ToString();
                    moccuindex[j] = i;
                    j++;
                }
            }
            if ((checkmoccu + 1) == moccucnt)
                moccucnt = moccucnt - 2;
            if ((ViewState["imoccucontrol"] != null))
            {
                for (int i = 0; i < moccucnt; i++)
                {
                    if (ViewState["lmoccucontrol"] != null)
                    {
                        Label lbl = mocculabel();
                        lbl.Text = " " + moccuvalue[i] + " ";
                        lbl.ID = "lbl10-" + moccucode[i].ToString();
                    }
                    ImageButton ib = moccuimage();
                    ib.ID = "imgbut10_" + moccucode[i].ToString();
                    ib.CommandArgument = moccuindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(moccuimg_Click);
                }
            }
            PlaceHoldermqual.Controls.Clear();
            checkmqual = 0;
            j = 0;
            for (int i = 0; i < cblmqual.Items.Count; i++)
            {
                if (cblmqual.Items[i].Selected == true)
                {
                    mqualvalue[j] = cblmqual.Items[i].Text;
                    checkmqual = checkmqual + 1;
                    mqualcode[j] = cblmqual.Items[i].Value.ToString();
                    mqualindex[j] = i;
                    j++;
                }
            }
            if ((checkmqual + 1) == mqualcnt)
                mqualcnt = mqualcnt - 2;
            if ((ViewState["imqualcontrol"] != null))
            {
                for (int i = 0; i < mqualcnt; i++)
                {
                    if (ViewState["lmqualcontrol"] != null)
                    {
                        Label lbl = mquallabel();
                        lbl.Text = " " + mqualvalue[i] + " ";
                        lbl.ID = "lbl11-" + mqualcode[i].ToString();
                    }
                    ImageButton ib = mqualimage();
                    ib.ID = "imgbut11_" + mqualcode[i].ToString();
                    ib.CommandArgument = mqualindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(mqualimg_Click);
                }
            }
            PlaceHolderbranch.Controls.Clear();
            checkbranch = 0;
            j = 0;
            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    branchvalue[j] = cblbranch.Items[i].Text;
                    checkbranch = checkbranch + 1;
                    branchcode[j] = cblbranch.Items[i].Value.ToString();
                    branchindex[j] = i;
                    j++;
                }
            }
            if ((checkbranch + 1) == branchcnt)
                branchcnt = branchcnt - 2;
            if ((ViewState["ibranchcontrol"] != null))
            {
                for (int i = 0; i < branchcnt; i++)
                {
                    if (ViewState["lbranchcontrol"] != null)
                    {
                        Label lbl = branchlabel();
                        lbl.Text = " " + branchvalue[i] + " ";
                        lbl.ID = "lbl13-" + branchcode[i].ToString();
                    }
                    ImageButton ib = branchimage();
                    ib.ID = "imgbut13_" + branchcode[i].ToString();
                    ib.CommandArgument = branchindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(branchimg_Click);
                }
            }
            PlaceHolderdegree.Controls.Clear();
            checkdegree = 0;
            j = 0;
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    degreevalue[j] = cbldegree.Items[i].Text;
                    checkdegree = checkdegree + 1;
                    degreecode[j] = cbldegree.Items[i].Value.ToString();
                    degreeindex[j] = i;
                    j++;
                }
            }
            if ((checkdegree + 1) == degreecnt)
                degreecnt = degreecnt - 2;
            if ((ViewState["idegreecontrol"] != null))
            {
                for (int i = 0; i < degreecnt; i++)
                {
                    if (ViewState["ldegreecontrol"] != null)
                    {
                        Label lbl = degreelabel();
                        lbl.Text = " " + degreevalue[i] + " ";
                        lbl.ID = "lbl12-" + degreecode[i].ToString();
                    }
                    ImageButton ib = degreeimage();
                    ib.ID = "imgbut12_" + degreecode[i].ToString();
                    ib.CommandArgument = degreeindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(degreeimg_Click);
                }
            }
            //sem
            PlaceHoldersemester.Controls.Clear();
            checkdegree = 0;
            j = 0;
            for (int i = 0; i < cblsem.Items.Count; i++)
            {
                if (cblsem.Items[i].Selected == true)
                {
                    semvalue[j] = cblsem.Items[i].Text;
                    checksem = checksem + 1;
                    semcode[j] = cblsem.Items[i].Value.ToString();
                    semindex[j] = i;
                    j++;
                }
            }
            if ((checksem + 1) == semcnt)
                degreecnt = degreecnt - 2;
            if ((ViewState["isemcontrol"] != null))
            {
                for (int i = 0; i < semcnt; i++)
                {
                    if (ViewState["lsemcontrol"] != null)
                    {
                        Label lbl = semlabel();
                        lbl.Text = " " + semvalue[i] + " ";
                        lbl.ID = "lbl14-" + semcode[i].ToString();
                    }
                    ImageButton ib = semimage();
                    ib.ID = "imgbut14_" + semcode[i].ToString();
                    ib.CommandArgument = semindex[i].ToString();
                    ib.Click += new ImageClickEventHandler(semimg_Click);
                }
            }
        }
        //sec
        PlaceHoldersection.Controls.Clear();
        checksec = 0;
        j = 0;
        for (int i = 0; i < cblsection.Items.Count; i++)
        {
            if (cblsection.Items[i].Selected == true)
            {
                secvalue[j] = cblsection.Items[i].Text;
                checksec = checksec + 1;
                seccode[j] = cblsection.Items[i].Value.ToString();
                secindex[j] = i;
                j++;
            }
        }
        if ((checksec + 1) == seccnt)
            seccnt = seccnt - 2;
        if ((ViewState["iseccontrol"] != null))
        {
            for (int i = 0; i < seccnt; i++)
            {
                if (ViewState["lseccontrol"] != null)
                {
                    Label lbl = seclabel();
                    lbl.Text = " " + secvalue[i] + " ";
                    lbl.ID = "lbl15-" + seccode[i].ToString();
                }
                ImageButton ib = secimage();
                ib.ID = "imgbut15_" + seccode[i].ToString();
                ib.CommandArgument = secindex[i].ToString();
                ib.Click += new ImageClickEventHandler(secimg_Click);
            }
        }
        //cmd.CommandText = "select count(*) from collinfo";
        //cmd.Connection = con;
        //con.Open();
        //SqlDataReader colrdr = cmd.ExecuteReader();
        //if (colrdr.Read())
        //{
        //    if (colrdr.GetValue(0).ToString() == "1")
        //    {
        //        //        ddlcollege.Visible = false;
        //        lblcollege.Visible = false;
        //        ddlcollege.SelectedIndex = 0;
        //    }
        //    else
        //    {
        //        ddlcollege.Visible = true;
        //    }
        //}
        //colrdr.Close();
        //con.Close();
        //if ((ddlstudname.Text != "---Select---") || ddladmno.Text != "---Select---" || ddlappno.Text != "---Select---" || ddlregno.Text != "---Select---" || ddlrollno.Text != "---Select---" || tbadmno.Text != "" || tbappno.Text != "" || tbregno.Text != "" || tbrollno.Text != "" || tbstudname.Text != "" || tbfromadmdt.Text != "" || tbfromappdt.Text != "" || tbtoadmdt.Text != "" || tbtoappdt.Text != "")
        //{
        //    Button1.Visible = true;
        //}
        //if (tbfromdob.Text != "" || tbtodob.Text != "" || tbfname.Text != "" || tbmname.Text != "" || tbgname.Text != "" || tbseattype.Text != "---Select---" || tbblood.Text != "---Select---" || tbcaste.Text != "---Select---" || tbregion.Text != "---Select---" || tbreligion.Text != "---Select---" || tbcomm.Text != "---Select---" || tbfqual.Text != "---Select---" || tbfoccu.Text != "---Select---" || tbmqual.Text != "---Select---" || tbmoccu.Text != "---Select---" || tbmtongue.Text != "---Select---" || tbfmobno.Text != "" || tbfoffno.Text != "" || tbmoffno.Text != "" || tbmmobno.Text != "" || tbstudmobno.Text != "")
        //{
        //    Button2.Visible = true;
        //}
        //if (ddlpemailid1.Text != "---Select---" || ddlpcity1.Text != "---Select---" || ddlpcountry1.Text != "---Select---" || ddlpdistrict1.Text != "---Select---" || ddlpstreet1.Text != "---Select---" || ddlccity1.Text != "---Select---" || ddlccountry1.Text != "---Select---" || ddlcdistrict1.Text != "---Select---" || ddlcstreet1.Text != "---Select---" || ddlgcity1.Text != "---Select---" || ddlgdistrict1.Text != "---Select---" || ddlgcountry1.Text != "---Select---" || ddlgstreet1.Text != "---Select---")
        //{
        //    Button3.Visible = true;
        //}
        //if (ddlbatchyrfrm.Text != "---Select---" || ddlbatchyrto.Text != "---Select---" || tbdegree.Text != "" || tbbranch.Text != "" || tbsection.Text != "" || tbsem.Text != "")
        //{
        //    Button4.Visible = true;
        //}
        // string collegestr = "";
        //if (cook == 0)
        if (!IsPostBack)
        {

            //string colleges;
            setLabelText();
            cmd.CommandText = "select acr,college_code from collinfo";
            cmd.Connection = con;
            con.Open();
            ddlcollege.DataSource = cmd.ExecuteReader();
            ddlcollege.DataTextField = "acr";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
            con.Close();
            cmd.CommandText = "select count(*) from collinfo";
            cmd.Connection = con;
            con.Open();
            SqlDataReader colrdr1 = cmd.ExecuteReader();
            if (colrdr1.Read())
            {
                if (colrdr1.GetValue(0).ToString() == "1")
                {
                    //        ddlcollege.Visible = false;
                    myCol.Visible = false;
                    ddlcollege.SelectedIndex = 0;
                    Panelcollege.Visible = false;
                }
                else
                {
                    myCol.Visible = true;
                    ddlcollege.Visible = true;
                    Panelcollege.Visible = true;
                    for (int itt = 0; itt < ddlcollege.Items.Count; itt++)
                    {
                        ddlcollege.Items[itt].Selected = true;
                    }
                    //  ddlcollege.Items[0].Selected = true;
                    LinkButtoncol.Visible = false;
                    //cbcollege.Checked = true;
                }
            }
            colrdr1.Close();
            con.Close();
            FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            FpSpread1.Pager.Align = HorizontalAlign.Right;
            FpSpread1.Pager.Font.Bold = true;
            FpSpread1.Pager.Font.Name = "Arial Narrow";
            FpSpread1.Pager.ForeColor = Color.DarkGreen;
            FpSpread1.Pager.BackColor = Color.Beige;
            FpSpread1.Pager.BackColor = Color.AliceBlue;
            FpSpread1.Pager.PageCount = 5;
            lblhostel.Font.Bold = true;
            lblhostel.Font.Name = "Book Antiqua";
            lblhostel.Font.Size = FontUnit.Medium;
            ddlhosday.Font.Bold = true;
            ddlhosday.Font.Name = "Book Antiqua";
            ddlhosday.Font.Size = FontUnit.Medium;
            ddlconsultant.Font.Bold = true;
            ddlconsultant.Font.Name = "Book Antiqua";
            ddlconsultant.Font.Size = FontUnit.Medium;
            tbothers.Font.Bold = true;
            tbothers.Font.Name = "Book Antiqua";
            tbothers.Font.Size = FontUnit.Medium;
            lbllaststudied.Font.Bold = true;
            lbllaststudied.Font.Name = "Book Antiqua";
            lbllaststudied.Font.Size = FontUnit.Medium;
            ddllaststudied.Font.Bold = true;
            ddllaststudied.Font.Name = "Book Antiqua";
            ddllaststudied.Font.Size = FontUnit.Medium;
            // //if (ddlcollege.SelectedIndexChanged==false)
            // //{
            //// }
            // //collegestr = college;
            // //else
            // //{
            //     if (ddlcollege.SelectedIndex == 0)
            //     {
            //         collegestr = Session["colls"].ToString();
            //     }
            //     else
            //     {
            //     }
            // //}
            if (cook == 0)
            {
                cblsearch.ClearSelection();
                Itemindex.Clear();
                ItemList.Clear();
                if (Request.Cookies["order"] != null)
                {
                    string temporder = (Request.Cookies["order"].Value);
                    string[] splitorder = temporder.Split(new char[] { ',' });
                    for (int temp = 0; temp < splitorder.Length; temp++)
                    {
                        string st = splitorder[temp].ToString();
                        int s = Convert.ToInt32(st);
                        cblsearch.Items[s].Selected = true;
                        Itemindex.Add(st);
                        ItemList.Add(cblsearch.Items[s].Text);
                    }
                }
            }
            cook = 1;
            ddlhosday.Items.Add("All");
            ddlhosday.Items.Add("Hostler");
            ddlhosday.Items.Add("Day Scholar");
            ddlhosday.SelectedIndex = 0;
            ddlrollno.Items.Add("---Select---");
            ddlrollno.Items.Add("Like");
            ddlrollno.Items.Add("Starts with");
            ddlrollno.Items.Add("Ends with");
            ddlrollno.Items.Add("Equal");
            ddlrollno.Items.Add("Not Equal");
            //ddlrollno.Items.Add("Greater than");
            //ddlrollno.Items.Add("Greater than or equal to");
            //ddlrollno.Items.Add("Lesser than");
            //ddlrollno.Items.Add("Lesser than or equal to");
            ddlregno.Items.Add("---Select---");
            ddlregno.Items.Add("Like");
            ddlregno.Items.Add("Starts with");
            ddlregno.Items.Add("Ends with");
            ddlregno.Items.Add("Equal");
            ddlregno.Items.Add("Not Equal");
            //ddlregno.Items.Add("Greater than");
            //ddlregno.Items.Add("Greater than or equal to");
            //ddlregno.Items.Add("Lesser than");
            //ddlregno.Items.Add("Lesser than or equal to");
            ddladmno.Items.Add("---Select---");
            ddladmno.Items.Add("Like");
            ddladmno.Items.Add("Starts with");
            ddladmno.Items.Add("Ends with");
            ddladmno.Items.Add("Equal");
            ddladmno.Items.Add("Not Equal");
            //ddladmno.Items.Add("Greater than");
            //ddladmno.Items.Add("Greater than or equal to");
            //ddladmno.Items.Add("Lesser than");
            //ddladmno.Items.Add("Lesser than or equal to");
            ddlappno.Items.Add("---Select---");
            ddlappno.Items.Add("Like");
            ddlappno.Items.Add("Starts with");
            ddlappno.Items.Add("Ends with");
            ddlappno.Items.Add("Equal");
            ddlappno.Items.Add("Not Equal");
            ddlappno.Items.Add("Greater than");
            ddlappno.Items.Add("Greater than or equal to");
            ddlappno.Items.Add("Lesser than");
            ddlappno.Items.Add("Lesser than or equal to");
            ddlstudname.Items.Add("---Select---");
            ddlstudname.Items.Add("Like");
            ddlstudname.Items.Add("Starts with");
            ddlstudname.Items.Add("Ends with");
            ddlfname.Items.Add("---Select---");
            ddlfname.Items.Add("Like");
            ddlfname.Items.Add("Starts with");
            ddlfname.Items.Add("Ends with");
            ddlmname.Items.Add("---Select---");
            ddlmname.Items.Add("Like");
            ddlmname.Items.Add("Starts with");
            ddlmname.Items.Add("Ends with");
            ddlgname.Items.Add("---Select---");
            ddlgname.Items.Add("Like");
            ddlgname.Items.Add("Starts with");
            ddlgname.Items.Add("Ends with");
            ddlpcity.Items.Add("---Select---");
            ddlpcity.Items.Add("Like");
            ddlpcity.Items.Add("Starts with");
            ddlpcity.Items.Add("Ends with");
            ddlpcountry.Items.Add("---Select---");
            ddlpcountry.Items.Add("Like");
            ddlpcountry.Items.Add("Starts with");
            ddlpcountry.Items.Add("Ends with");
            ddlpdistrict.Items.Add("---Select---");
            ddlpdistrict.Items.Add("Like");
            ddlpdistrict.Items.Add("Starts with");
            ddlpdistrict.Items.Add("Ends with");
            ddlpstreet.Items.Add("---Select---");
            ddlpstreet.Items.Add("Like");
            ddlpstreet.Items.Add("Starts with");
            ddlpstreet.Items.Add("Ends with");
            ddlccity.Items.Add("---Select---");
            ddlccity.Items.Add("Like");
            ddlccity.Items.Add("Starts with");
            ddlccity.Items.Add("Ends with");
            ddlccountry.Items.Add("---Select---");
            ddlccountry.Items.Add("Like");
            ddlccountry.Items.Add("Starts with");
            ddlccountry.Items.Add("Ends with");
            ddlcdistrict.Items.Add("---Select---");
            ddlcdistrict.Items.Add("Like");
            ddlcdistrict.Items.Add("Starts with");
            ddlcdistrict.Items.Add("Ends with");
            ddlcstreet.Items.Add("---Select---");
            ddlcstreet.Items.Add("Like");
            ddlcstreet.Items.Add("Starts with");
            ddlcstreet.Items.Add("Ends with");
            ddlgcity.Items.Add("---Select---");
            ddlgcity.Items.Add("Like");
            ddlgcity.Items.Add("Starts with");
            ddlgcity.Items.Add("Ends with");
            ddlgcountry.Items.Add("---Select---");
            ddlgcountry.Items.Add("Like");
            ddlgcountry.Items.Add("Starts with");
            ddlgcountry.Items.Add("Ends with");
            ddlgdistrict.Items.Add("---Select---");
            ddlgdistrict.Items.Add("Like");
            ddlgdistrict.Items.Add("Starts with");
            ddlgdistrict.Items.Add("Ends with");
            ddlgstreet.Items.Add("---Select---");
            ddlgstreet.Items.Add("Like");
            ddlgstreet.Items.Add("Starts with");
            ddlgstreet.Items.Add("Ends with");
            ddlpstate.Items.Add("---Select---");
            ddlpstate.Items.Add("Like");
            ddlpstate.Items.Add("Starts with");
            ddlpstate.Items.Add("Ends with");
            ddlcstate.Items.Add("---Select---");
            ddlcstate.Items.Add("Like");
            ddlcstate.Items.Add("Starts with");
            ddlcstate.Items.Add("Ends with");
            ddlgstate.Items.Add("---Select---");
            ddlgstate.Items.Add("Like");
            ddlgstate.Items.Add("Starts with");
            ddlgstate.Items.Add("Ends with");
            //Start Aruna 19july2013===========================================
            drp_padress1.Items.Add("---Select---");
            drp_padress1.Items.Add("Like");
            drp_padress1.Items.Add("Starts with");
            drp_padress1.Items.Add("Ends with");
            drp_cadress1.Items.Add("---Select---");
            drp_cadress1.Items.Add("Like");
            drp_cadress1.Items.Add("Starts with");
            drp_cadress1.Items.Add("Ends with");
            drp_gadress1.Items.Add("---Select---");
            drp_gadress1.Items.Add("Like");
            drp_gadress1.Items.Add("Starts with");
            drp_gadress1.Items.Add("Ends with");
            drp_ppincode1.Items.Add("---Select---");
            drp_ppincode1.Items.Add("Like");
            drp_ppincode1.Items.Add("Starts with");
            drp_ppincode1.Items.Add("Ends with");
            drp_cpincode1.Items.Add("---Select---");
            drp_cpincode1.Items.Add("Like");
            drp_cpincode1.Items.Add("Starts with");
            drp_cpincode1.Items.Add("Ends with");
            drp_gpincode1.Items.Add("---Select---");
            drp_gpincode1.Items.Add("Like");
            drp_gpincode1.Items.Add("Starts with");
            drp_gpincode1.Items.Add("Ends with");
            //End==============================================================
            //ddlcollege_SelectedIndexChanged(sender, e);
            loaddetails();
            FpSpread1.Visible = false;
            //Added By Srinath 7/5/2013
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnexcel.Visible = false;
            Printcontrol.Visible = false;
            //ddlpemailid.Items.Add("---Select---"); 
            //ddlpemailid.Items.Add("Like");
            //ddlpemailid.Items.Add("Starts with");
            //ddlpemailid.Items.Add("Ends with");
            //cmd.CommandText="select distinct refer_name from applyn where 
            //int a;
            //cmd.CommandText = "select distinct Student_Mobile from applyn";
            //cmd.Connection = con;
            //con.Open();
            //ddlmobileno.DataSource = cmd.ExecuteReader();
            //ddlmobileno.DataValueField = "Student_Mobile";
            //ddlmobileno.DataBind();
            //a = ddlmobileno.items.count;
            //ddlmobileno.Items.insert(new ListItem("Others"));
            //con.Close();
            //Added By Srinath 7/5/2013
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnexcel.Visible = false;
            Printcontrol.Visible = false;
            RollAndRegSettings();
        }
    }
    protected void Page_init(object sender, EventArgs e)
    {
    }
    //protected override void Render(System.Web.UI.HtmlTextWriter writer)
    //{
    //    Control cntUpdateBtn = FpSpread1.FindControl("Update");
    //    Control cntCancelBtn = FpSpread1.FindControl("Cancel");
    //    Control cntCopyBtn = FpSpread1.FindControl("Copy");
    //    Control cntCutBtn = FpSpread1.FindControl("Clear");
    //    Control cntPasteBtn = FpSpread1.FindControl("Paste");
    //    //Control cntPageNextBtn = FpSpread1.FindControl("Next");
    //    //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
    //    Control cntPagePrintBtn = FpSpread1.FindControl("Print");
    //    if ((cntUpdateBtn != null))
    //    {
    //        TableCell tc = (TableCell)cntUpdateBtn.Parent;
    //        TableRow tr = (TableRow)tc.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntCancelBtn.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntCopyBtn.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntCutBtn.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntPasteBtn.Parent;
    //        tr.Cells.Remove(tc);
    //        //tc = (TableCell)cntPageNextBtn.Parent;
    //        //tr.Cells.Remove(tc);
    //        //tc = (TableCell)cntPagePreviousBtn.Parent;
    //        //tr.Cells.Remove(tc);
    //        tc = (TableCell)cntPagePrintBtn.Parent;
    //        tr.Cells.Remove(tc);
    //    }
    //    base.Render(writer);
    //}
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        string orderStr = string.Empty;
        RollAndRegSettings();
        if (roll == 0)
            orderStr = " Order by r.roll_no,r.reg_no,r.roll_admit ";
        else if (roll == 1)
            orderStr = " Order by r.roll_no,r.reg_no,r.roll_admit ";
        else if (roll == 2)
            orderStr = " Order by r.roll_no ";
        else if (roll == 3)
            orderStr = " Order by r.reg_no ";
        else if (roll == 4)
            orderStr = " Order by r.roll_admit ";
        else if (roll == 5)
            orderStr = " Order by r.roll_no,r.reg_no ";
        else if (roll == 6)
            orderStr = " Order by r.reg_no,r.roll_admit ";
        else if (roll == 7)
            orderStr = " Order by r.roll_no,r.roll_admit ";
        btnprintmaster.Visible = true;
        btnexcel.Visible = true;
        Printcontrol.Visible = false;
        lblrptname.Visible = true;
        txtexcelname.Visible = true;
        FpSpread1.Sheets[0].PageSize = 10;
        DropDownListpage.Visible = true;
        FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
        int n = Itemindex.Count;
        TextBoxpage.Visible = true;
        lblpage.Visible = true;
        lblrecord.Visible = true;
        string colleges = "";
        string sel1 = "";
        string sel2 = "";
        string sel3 = "";
        string sel4 = "";
        string sel5 = "";
        string sel6 = "";
        string sel7 = "";
        string sel8 = "";
        string sel9 = "";
        string sel10 = "";
        string sel11 = "";
        string sel12 = "";
        string sel13 = "";
        string sel14 = "";
        string sel15 = "";
        string sel16 = "";
        string sel17 = "";
        string sel18 = "";
        string sel19 = "";
        string sel20 = "";
        string sel21 = "";
        string sel22 = "";
        string sel23 = "";
        string sel24 = "";
        string sel25 = "";
        string sel26 = "";
        string sel27 = "";
        string sel28 = "";
        string sel29 = "";
        string sel30 = "";
        string sel31 = "";
        string sel32 = "";
        string sel33 = "";
        string sel34 = "";
        string sel35 = "";
        string collegecode_selection = "";
        string sel36 = "";
        string sel37 = "";
        string sel38 = "";
        string sel39 = "";
        string sel40 = "";
        string sel41 = "";
        string sel42 = "";
        string sel43 = "";
        string sel44 = "";
        string sel45 = "";
        string sel46 = "";
        string sel47 = "";
        string sel48 = "";
        string sel49 = "";
        string sel50 = "";
        string sel51 = "";
        string sel52 = "";
        string sel53 = "";
        string hosdaysel = "";
        string collegesel = "";
        string laststudiedsel = "";
        string selitem = "";
        string selitem1 = "";
        string selitem2 = "";
        string selitem3 = "";
        string selitem4 = "";
        string selitem5 = "";
        string selitem6 = "";
        string selitem7 = "";
        string selitem8 = "";
        string selitem9 = "";
        string selitem10 = "";
        string selection = "";
        string selection1 = "";
        string selection2 = "";
        string selection3 = "";
        string selection4 = "";
        string selection5 = "";
        string selection6 = "";
        string selection7 = "";
        string selection8 = "";
        string selection9 = "";
        string selection10 = "";
        string selection11 = "";
        string selection12 = "";
        string selection13 = "";
        string selection14 = "";
        string selection15 = "";
        string selection16 = "";
        string selection17 = "";
        string selection18 = "";
        string selection19 = "";
        string selection20 = "";
        string selection21 = "";
        string selection22 = "";
        string selection23 = "";
        string selection24 = "";
        string selection25 = "";
        string selection26 = "";
        string selection27 = "";
        string college = "";
        int columncount = 0;
        selection = " r.App_No";
        //ItemList.Add("Roll No");
        //ItemList.Add("Student Name");
        //  Itemindex.Add("2");
        //  Itemindex.Add("4");
        int count1 = 0;
        for (int it = 0; it < ddlcollege.Items.Count; it++)
        {
            if (ddlcollege.Items[it].Selected == true)
            {
                count1 = count1 + 1;
                if (college == "")
                {
                    college = ddlcollege.Items[it].Value.ToString();
                }
                else
                {
                    college = college + "," + ddlcollege.Items[it].Value.ToString();
                }
            }
        }
        if (college != "")
        {
            string collvalue = " and r";
            //if (!Itemindex.Contains("54"))
            //{
            //    ItemList.Add("College");
            //    Itemindex.Add("54");
            //}
            //cblsearch.Items[54].Selected = true;
            tbcollege.Text = lblcollege.Text + "(" + count1 + ")";
            lblnorec.Visible = false;
            lblcol.Visible = false;
            Accordion1.Visible = true;
            btnsearch.Visible = true;
            clear.Visible = true;
            FpSpread1.Visible = true;
            //Added By Srinath 7/5/2013
            btnprintmaster.Visible = true;
            txtexcelname.Visible = true;
            lblrptname.Visible = true;
            btnexcel.Visible = true;
            Printcontrol.Visible = false;
            if (count1 == ddlcollege.Items.Count)
                cbcollege.Checked = true;
            else
                cbcollege.Checked = false;
        }
        else
        {
            tbcollege.Text = "---Select---";
            Accordion1.Visible = false;
            lblnorec.Visible = true;
            lblcol.Visible = true;
            lblcol.Text = "Select College and then proceed";
            btnsearch.Visible = false;
            clear.Visible = false;
            FpSpread1.Visible = false;
            //Added By Srinath 7/5/2013
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            Panelpage.Visible = false;
            btnexcel.Visible = false;
            Printcontrol.Visible = false;
            return;
        }
        selitem1 = ddlrollno.SelectedItem.Text;
        selitem2 = ddlregno.SelectedItem.Text;
        selitem3 = ddlstudname.SelectedItem.Text;
        selitem4 = ddladmno.SelectedItem.Text;
        selitem5 = ddlappno.SelectedItem.Text;
        selitem6 = ddlfname.SelectedItem.Text;
        selitem7 = ddlmname.SelectedItem.Text;
        selitem8 = ddlgname.SelectedItem.Text;
        if (tbrollno.Text != "" && selitem1 != "---Select---")
        {
            if (selitem1 == "Like")
            {
                selection1 = "like '%" + tbrollno.Text + "%'";
            }
            else if (selitem1 == "Starts with")
            {
                selection1 = "like '" + tbrollno.Text + "%'";
            }
            else if (selitem1 == "Ends with")
            {
                selection1 = "like '%" + tbrollno.Text + "'";
            }
            else if (selitem1 == "Equal")
            {
                selection1 = "='" + tbrollno.Text + "'";
            }
            else if (selitem1 == "Not Equal")
            {
                selection1 = "!='" + tbrollno.Text + "'";
            }
            else if (selitem1 == "Greater than")
            {
                selection1 = ">" + tbrollno.Text + "";
            }
            else if (selitem1 == "Greater than or equal to")
            {
                selection1 = ">=" + tbrollno.Text + "";
            }
            else if (selitem1 == "Lesser than")
            {
                selection1 = "<" + tbrollno.Text + "";
            }
            else if (selitem1 == "Lesser than or equal to")
            {
                selection1 = "<=" + tbrollno.Text + "";
            }
            sel1 = " and r.Roll_No " + selection1 + "";
            selection = selection + ",r.Roll_No";
            columncount = columncount + 1;
            if (!Itemindex.Contains("2"))
            {
                ItemList.Add("Roll No");
                Itemindex.Add("2");
            }
            cblsearch.Items[2].Selected = true;
        }
        else
        {
            //if (!Itemindex.Contains("2"))//
            //{
            //    ItemList.Add("Roll No");
            //    Itemindex.Add("2");
            //}
            //cblsearch.Items[2].Selected = true;
        }
        if (tbregno.Text != "" && selitem2 != "---Select---")
        {
            if (selitem2 == "Like")
            {
                selection2 = "like '%" + tbregno.Text + "%'";
            }
            else if (selitem2 == "Starts with")
            {
                selection2 = "like '" + tbregno.Text + "%'";
            }
            else if (selitem2 == "Ends with")
            {
                selection2 = "like '%" + tbregno.Text + "'";
            }
            else if (selitem2 == "Equal")
            {
                selection2 = "='" + tbregno.Text + "'";
            }
            else if (selitem2 == "Not Equal")
            {
                selection2 = "!='" + tbregno.Text + "'";
            }
            else if (selitem2 == "Greater than")
            {
                selection2 = ">" + tbregno.Text + "";
            }
            else if (selitem2 == "Greater than or equal to")
            {
                selection2 = ">=" + tbregno.Text + "";
            }
            else if (selitem2 == "Lesser than")
            {
                selection2 = "<" + tbregno.Text + "";
            }
            else if (selitem2 == "Lesser than or equal to")
            {
                selection2 = "<=" + tbregno.Text + "";
            }
            sel2 = " and r.Reg_No " + selection2 + "";
            selection = selection + ",r.Reg_No";
            columncount = columncount + 1;
            if (!Itemindex.Contains("3"))
            {
                ItemList.Add("Register No");
                Itemindex.Add("3");
            }
            cblsearch.Items[3].Selected = true;
        }
        if (tbstudname.Text != "" && selitem3 != "---Select---")
        {
            if (selitem3 == "Like")
            {
                selection3 = "like '%" + tbstudname.Text + "%'";
            }
            else if (selitem3 == "Starts with")
            {
                selection3 = "like '" + tbstudname.Text + "%'";
            }
            else if (selitem3 == "Ends with")
            {
                selection3 = "like '%" + tbstudname.Text + "'";
            }
            sel3 = " and r.Stud_Name " + selection3 + "";
            if (!Itemindex.Contains("4"))
            {
                ItemList.Add("Student Name");
                Itemindex.Add("4");
            }
            //selection = selection + ",r.Stud_Name";
            //columncount = columncount + 1;
            //ItemList.Add("Student Name");
            cblsearch.Items[4].Selected = true;
        }
        else
        {
            //if (!Itemindex.Contains("4"))
            //{
            //    ItemList.Add("Student Name");
            //    Itemindex.Add("4");
            //}
            //cblsearch.Items[4].Selected = true;
        }
        if (tbadmno.Text != "" && selitem4 != "---Select---")
        {
            if (selitem4 == "Like")
            {
                selection4 = "like '%" + tbadmno.Text + "%'";
            }
            else if (selitem4 == "Starts with")
            {
                selection4 = "like '" + tbadmno.Text + "%'";
            }
            else if (selitem4 == "Ends with")
            {
                selection4 = "like '%" + tbadmno.Text + "'";
            }
            else if (selitem4 == "Equal")
            {
                selection4 = "='" + tbadmno.Text + "'";
            }
            else if (selitem4 == "Not Equal")
            {
                selection4 = "!='" + tbadmno.Text + "'";
            }
            else if (selitem4 == "Greater than")
            {
                selection4 = ">'" + tbadmno.Text + "'";
            }
            else if (selitem4 == "Greater than or equal to")
            {
                selection4 = ">='" + tbadmno.Text + "'";
            }
            else if (selitem4 == "Lesser than")
            {
                selection4 = "<'" + tbadmno.Text + "'";
            }
            else if (selitem4 == "Lesser than or equal to")
            {
                selection4 = "<='" + tbadmno.Text + "'";
            }
            sel4 = " and r.Roll_Admit " + selection4 + "";
            if (!Itemindex.Contains("1"))
            {
                ItemList.Add("Admission No");
                Itemindex.Add("1");
            }
            cblsearch.Items[1].Selected = true;
            //selection = selection + ",r.Roll_Admit";
            //columncount = columncount + 1;
            //ItemList.Add("Admission No");
        }
        else
        {
            //if (!Itemindex.Contains("1"))
            //{
            //    ItemList.Add("Admission No");
            //    Itemindex.Add("1");
            //}
            //cblsearch.Items[1].Selected = true;
        }
        if (tbappno.Text != "" && selitem5 != "---Select---")
        {
            if (selitem5 == "Like")
            {
                selection5 = "like '%" + tbappno.Text + "%'";
            }
            else if (selitem5 == "Starts with")
            {
                selection5 = "like '" + tbappno.Text + "%'";
            }
            else if (selitem5 == "Ends with")
            {
                selection5 = "like '%" + tbappno.Text + "'";
            }
            else if (selitem5 == "Equal")
            {
                selection5 = "='" + tbappno.Text + "'";
            }
            else if (selitem5 == "Not Equal")
            {
                selection5 = "!='" + tbappno.Text + "'";
            }
            else if (selitem5 == "Greater than")
            {
                selection5 = ">'" + tbappno.Text + "'";
            }
            else if (selitem5 == "Greater than or equal to")
            {
                selection5 = ">='" + tbappno.Text + "'";
            }
            else if (selitem5 == "Lesser than")
            {
                selection5 = "<'" + tbappno.Text + "'";
            }
            else if (selitem5 == "Lesser than or equal to")
            {
                selection5 = "<='" + tbappno.Text + "'";
            }
            sel5 = " and a.app_formno " + selection5 + "";
            if (!Itemindex.Contains("0"))
            {
                ItemList.Add("Application No");
                Itemindex.Add("0");
            }
            cblsearch.Items[0].Selected = true;
            //selection = selection + ",a.app_formno";
            //columncount = columncount + 1;
            //ItemList.Add("Application No");
        }
        else
        {
            //if (!Itemindex.Contains("0"))
            //{
            //    ItemList.Add("Application No");
            //    Itemindex.Add("0");
            //}
            //cblsearch.Items[0].Selected = true;
        }
        if ((tbfromdob.Text != "") && (tbtodob.Text != ""))
        {
            string datefdob, dtfromdob;
            string datefromdob;
            string yr, m, d;
            datefdob = tbfromdob.Text.ToString();
            string[] split = datefdob.Split(new Char[] { '-' });
            if (split.Length == 3)
            {
                datefromdob = split[0].ToString() + "-" + split[1].ToString() + "-" + split[2].ToString();
                yr = split[2].ToString();
                m = split[1].ToString();
                d = split[0].ToString();
                dtfromdob = yr + "-" + m + "-" + d;
                string date2dob;
                string datetodob;
                string yr1, m1, d1;
                date2dob = tbtodob.Text.ToString();
                string[] split1 = date2dob.Split(new Char[] { '-' });
                if (split1.Length == 3)
                {
                    datetodob = split1[0].ToString() + "-" + split1[1].ToString() + "-" + split1[2].ToString();
                    yr1 = split1[2].ToString();
                    m1 = split1[1].ToString();
                    d1 = split1[0].ToString();
                    datetodob = yr1 + "-" + m1 + "-" + d1;
                    sel6 = " and a.dob between '" + dtfromdob + "' and '" + datetodob + "' ";
                    if (!Itemindex.Contains("8"))
                    {
                        ItemList.Add("Date of birth");
                        Itemindex.Add("8");
                    }
                    cblsearch.Items[8].Selected = true;
                    //selection = selection + ",CONVERT(VARCHAR(10),a.dob,103)";
                    //columncount = columncount + 1;
                    //ItemList.Add("Date of Birth");
                }
            }
        }
        if ((tbfromappdt.Text != "") && (tbtoappdt.Text != ""))
        {
            string datefap, dtfromap;
            string datefromap;
            string yr2, m2, d2;
            datefap = tbfromappdt.Text.ToString();
            string[] split2 = datefap.Split(new Char[] { '-' });
            if (split2.Length == 3)
            {
                datefromap = split2[0].ToString() + "-" + split2[1].ToString() + "-" + split2[2].ToString();
                yr2 = split2[2].ToString();
                m2 = split2[1].ToString();
                d2 = split2[0].ToString();
                dtfromap = yr2 + "-" + m2 + "-" + d2;
                string date2ap;
                string datetoap;
                string yr3, m3, d3;
                date2ap = tbtoappdt.Text.ToString();
                string[] split3 = date2ap.Split(new Char[] { '-' });
                if (split3.Length == 3)
                {
                    datetoap = split3[0].ToString() + "-" + split3[1].ToString() + "-" + split3[2].ToString();
                    yr3 = split3[2].ToString();
                    m3 = split3[1].ToString();
                    d3 = split3[0].ToString();
                    datetoap = yr3 + "-" + m3 + "-" + d3;
                    sel7 = " and a.Date_Applied between '" + dtfromap + "' and '" + datetoap + "' ";
                    if (!Itemindex.Contains("6"))
                    {
                        ItemList.Add("Applied Date");
                        Itemindex.Add("6");
                    }
                    cblsearch.Items[6].Selected = true;
                    //selection = selection + ",CONVERT(VARCHAR(10),a.Date_Applied,103)";
                    //columncount = columncount + 1;
                    //ItemList.Add("Applied Date");
                }
            }
        }
        string sel_remark = "";
        if ((tbfromadmdt.Text != "") && (tbtoadmdt.Text != ""))
        {
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = tbfromadmdt.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '-' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "-" + split4[1].ToString() + "-" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = yr4 + "-" + m4 + "-" + d4;
                string date2ad;
                string datetoad;
                string yr5, m5, d5;
                date2ad = tbtoadmdt.Text.ToString();
                string[] split5 = date2ad.Split(new Char[] { '-' });
                if (split5.Length == 3)
                {
                    datetoad = split5[0].ToString() + "-" + split5[1].ToString() + "-" + split5[2].ToString();
                    yr5 = split5[2].ToString();
                    m5 = split5[1].ToString();
                    d5 = split5[0].ToString();
                    datetoad = yr5 + "-" + m5 + "-" + d5;
                    sel8 = " and r.Adm_Date between '" + dtfromad + "' and '" + datetoad + "' ";
                    if (!Itemindex.Contains("7"))
                    {
                        ItemList.Add("Admission Date");
                        Itemindex.Add("7");
                    }
                    if (!Itemindex.Contains("53"))
                    {
                        ItemList.Add("Remarks");
                        Itemindex.Add("53");
                    }
                    cblsearch.Items[7].Selected = true;
                    cblsearch.Items[53].Selected = true;
                    //selection = selection + ",CONVERT(VARCHAR(10),r.Adm_Date,103)";
                    //columncount = columncount + 1;
                    //ItemList.Add("Admission Date");
                }
            }
        }
        if (tbfname.Text != "" && selitem6 != "---Select---")
        {
            if (selitem6 == "Like")
            {
                selection6 = "like '%" + tbfname.Text + "%'";
            }
            else if (selitem6 == "Starts with")
            {
                selection6 = "like '" + tbfname.Text + "%'";
            }
            else if (selitem6 == "Ends with")
            {
                selection6 = "like '%" + tbfname.Text + "'";
            }
            sel9 = " and a.parent_name " + selection6 + "";
            selection = selection + ",a.parent_name";
            columncount = columncount + 1;
            if (!Itemindex.Contains("9"))
            {
                ItemList.Add("Father Name");
                Itemindex.Add("9");
            }
            cblsearch.Items[9].Selected = true;
            //cblsearch.Items[9].Selected = true;
        }
        if (tbmname.Text != "" && selitem7 != "---Select---")
        {
            if (selitem7 == "Like")
            {
                selection7 = "like '%" + tbmname.Text + "%'";
            }
            else if (selitem7 == "Starts with")
            {
                selection7 = "like '" + tbmname.Text + "%'";
            }
            else if (selitem7 == "Ends with")
            {
                selection7 = "like '%" + tbmname.Text + "'";
            }
            sel10 = " and a.mother " + selection7 + "";
            selection = selection + ",a.mother";
            columncount = columncount + 1;
            if (!Itemindex.Contains("10"))
            {
                ItemList.Add("Mother Name");
                Itemindex.Add("10");
            }
            cblsearch.Items[10].Selected = true;
        }
        if (tbgname.Text != "" && selitem8 != "---Select---")
        {
            if (selitem8 == "Like")
            {
                selection8 = "like '%" + tbgname.Text + "%'";
            }
            else if (selitem8 == "Starts with")
            {
                selection8 = "like '" + tbgname.Text + "%'";
            }
            else if (selitem8 == "Ends with")
            {
                selection8 = "like '%" + tbgname.Text + "'";
            }
            sel11 = " and a.guardian_name " + selection8 + "";
            selection = selection + ",a.guardian_name";
            columncount = columncount + 1;
            if (!Itemindex.Contains("11"))
            {
                ItemList.Add("Guardian Name");
                Itemindex.Add("11");
            }
            cblsearch.Items[11].Selected = true;
        }
        string seattype = "";
        string caste = "";
        string religion = "";
        string region = "";
        string community = "";
        string fqual = "";
        string mqual = "";
        string foccu = "";
        string moccu = "";
        string degree = "";
        string branch = "";
        string query = "";
        string blood = "";
        string mton = "";
        string sem = "";
        string direct = "";
        string staffname = "";
        string others = "";
        string section = "";
        string previous = "";
        int i = 0;
        int j = 0;
        if (ddlhosday.SelectedValue == "All")
        {
            hosdaysel = "";
            cblsearch.Items[48].Selected = true;
        }
        else
        {
            hosdaysel = " and r.stud_type='" + ddlhosday.SelectedItem.Text.ToString() + "'";
            cblsearch.Items[48].Selected = true;
        }
        for (i = 0; i < cblseattype.Items.Count; i++)
        {
            if (cblseattype.Items[i].Selected == true)
            {
                if (seattype == "")
                {
                    seattype = "'" + cblseattype.Items[i].Text.ToString() + "'";
                }
                else
                {
                    seattype = seattype + ",'" + cblseattype.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (seattype != "")
        {
            sel12 = " and a.seattype in(select distinct textcode from textvaltable where textcriteria='seat' and textval in(" + seattype + ")) " + college_code + "";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.seattype and textcriteria='seat'  " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("12"))
            {
                ItemList.Add("Seat Type");
                Itemindex.Add("12");
            }
            cblsearch.Items[12].Selected = true;
        }
        else
        {
            //if (!Itemindex.Contains("12"))
            //{
            //    ItemList.Add("Seat Type");
            //    Itemindex.Add("12");
            //}
            //cblsearch.Items[12].Selected = true;
        }
        for (i = 0; i < cblcaste.Items.Count; i++)
        {
            if (cblcaste.Items[i].Selected == true)
            {
                if (caste == "")
                {
                    caste = "'" + cblcaste.Items[i].Text.ToString() + "'";
                }
                else
                {
                    caste = caste + ",'" + cblcaste.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (caste != "")
        {
            sel13 = " and a.caste in(select distinct textcode from textvaltable where textcriteria='caste' and textval in(" + caste + ")  " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.caste and textcriteria='caste'  " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("14"))
            {
                ItemList.Add("Caste");
                Itemindex.Add("14");
            }
            cblsearch.Items[14].Selected = true;
        }
        for (i = 0; i < cblreligion.Items.Count; i++)
        {
            if (cblreligion.Items[i].Selected == true)
            {
                if (religion == "")
                {
                    religion = "'" + cblreligion.Items[i].Text.ToString() + "'";
                }
                else
                {
                    religion = religion + ",'" + cblreligion.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (religion != "")
        {
            sel14 = " and a.religion in(select distinct textcode from textvaltable where textcriteria='relig' and textval in(" + religion + ")  " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.religion and textcriteria='relig' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("15"))
            {
                ItemList.Add("Religion");
                Itemindex.Add("15");
            }
            cblsearch.Items[15].Selected = true;
        }
        for (i = 0; i < cblregion.Items.Count; i++)
        {
            if (cblregion.Items[i].Selected == true)
            {
                if (region == "")
                {
                    region = "'" + cblregion.Items[i].Text.ToString() + "'";
                }
                else
                {
                    region = region + ",'" + cblregion.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (region != "")
        {
            sel15 = " and a.region in(select distinct textcode from textvaltable where textcriteria='reg' and textval in(" + region + ") " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.region and textcriteria='Reg' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("17"))
            {
                ItemList.Add("Region");
                Itemindex.Add("17");
            }
            cblsearch.Items[17].Selected = true;
        }
        for (i = 0; i < cblcomm.Items.Count; i++)
        {
            if (cblcomm.Items[i].Selected == true)
            {
                if (community == "")
                {
                    community = "'" + cblcomm.Items[i].Text.ToString() + "'";
                }
                else
                {
                    community = community + ",'" + cblcomm.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (community != "")
        {
            sel16 = " and a.community in(select distinct textcode from textvaltable where textcriteria='comm' and textval in(" + community + ") " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.community and textcriteria='comm' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("16"))
            {
                ItemList.Add("Community");
                Itemindex.Add("16");
            }
            cblsearch.Items[16].Selected = true;
        }
        for (i = 0; i < cblfqual.Items.Count; i++)
        {
            if (cblfqual.Items[i].Selected == true)
            {
                if (fqual == "")
                {
                    fqual = "'" + cblfqual.Items[i].Text.ToString() + "'";
                }
                else
                {
                    fqual = fqual + ",'" + cblfqual.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (fqual != "")
        {
            sel17 = " and a.fqualification in(select distinct textcode from textvaltable where textcriteria='fqual' and textval in(" + fqual + ") " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.fqualification and textcriteria='fqual' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("20"))
            {
                ItemList.Add("Father Qualification");
                Itemindex.Add("20");
            }
            cblsearch.Items[20].Selected = true;
        }
        for (i = 0; i < cblmqual.Items.Count; i++)
        {
            if (cblmqual.Items[i].Selected == true)
            {
                if (mqual == "")
                {
                    mqual = "'" + cblmqual.Items[i].Text.ToString() + "'";
                }
                else
                {
                    mqual = mqual + ",'" + cblmqual.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (mqual != "")
        {
            sel18 = " and a.mQualification in(select distinct textcode from textvaltable where textcriteria='mqual' and textval in(" + mqual + ") " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.mQualification and textcriteria='mqual' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("24"))
            {
                ItemList.Add("Mother Qualification");
                Itemindex.Add("24");
            }
            cblsearch.Items[24].Selected = true;
        }
        for (i = 0; i < cblfoccu.Items.Count; i++)
        {
            if (cblfoccu.Items[i].Selected == true)
            {
                if (foccu == "")
                {
                    foccu = "'" + cblfoccu.Items[i].Text.ToString() + "'";
                }
                else
                {
                    foccu = foccu + ",'" + cblfoccu.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (foccu != "")
        {
            sel19 = " and a.Parent_occu in(select distinct textcode from textvaltable where textcriteria='foccu' and textval in(" + foccu + ") " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.parent_occu and textcriteria='foccu' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("19"))
            {
                ItemList.Add("Father Occupation");
                Itemindex.Add("19");
            }
            cblsearch.Items[19].Selected = true;
        }
        for (i = 0; i < cblmoccu.Items.Count; i++)
        {
            if (cblmoccu.Items[i].Selected == true)
            {
                if (moccu == "")
                {
                    moccu = "'" + cblmoccu.Items[i].Text.ToString() + "'";
                }
                else
                {
                    moccu = moccu + ",'" + cblmoccu.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (moccu != "")
        {
            sel20 = " and a.motherocc in(select distinct textcode from textvaltable where textcriteria='moccu' and textval in(" + moccu + ") " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.motherocc and textcriteria='moccu' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("23"))
            {
                ItemList.Add("Mother Occupation");
                Itemindex.Add("23");
            }
            cblsearch.Items[23].Selected = true;
        }
        for (i = 0; i < cbldegree.Items.Count; i++)
        {
            if (cbldegree.Items[i].Selected == true)
            {
                if (degree == "")
                {
                    degree = "'" + cbldegree.Items[i].Text.ToString() + "'";
                }
                else
                {
                    degree = degree + ",'" + cbldegree.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (degree != "")
        {
            //sel21 = " and r.degree_code in(" + degree + ") ";
            //,(select (case course.Course_Name when '-1' then ' ' else course.Course_Name end +'['+dept_acronym+']') as course from degree,course,department where course.Course_Id=degree.Course_Id and degree.Degree_Code=r.degree_code and department.dept_code=degree.dept_code)";
            selection = selection + ",(select (case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,department where course.Course_Id=degree.Course_Id and degree.Degree_Code=r.degree_code)";
            columncount = columncount + 1;
            if (!Itemindex.Contains("42"))
            {
                ItemList.Add("Degree");
                Itemindex.Add("42");
            }
            cblsearch.Items[42].Selected = true;
            degree = "select distinct Course_id from course where course_name in(" + degree + ") and college_code in(" + college + ")";
        }
        else
        {
            //if (!Itemindex.Contains("42"))
            //{
            //    ItemList.Add("Degree");
            //    Itemindex.Add("42");
            //}
            //cblsearch.Items[42].Selected = true;
        }
        for (i = 0; i < cblbranch.Items.Count; i++)
        {
            if (cblbranch.Items[i].Selected == true)
            {
                if (branch == "")
                {
                    branch = "'" + cblbranch.Items[i].Text.ToString() + "'";
                }
                else
                {
                    branch = branch + ",'" + cblbranch.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (branch != "")
        {
            // sel22 = " and r.degree_code in(" + branch + ") ";
            selection = selection + ",(select department.Dept_Acronym from degree,department where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=r.degree_code)";
            columncount = columncount + 1;
            if (!Itemindex.Contains("43"))
            {
                ItemList.Add("Department");
                Itemindex.Add("43");
            }
            cblsearch.Items[43].Selected = true;
            branch = "select distinct dept_code from department where dept_acronym in(" + branch + ") and college_code in(" + college + ")";
        }
        if (degree != "" && branch != "")
        {
            sel21 = " and r.degree_code in(" + "select distinct degree_code from degree where course_id in(" + degree + ") and dept_code in(" + branch + ") and college_code in(" + college + "))";
            sel22 = "";
        }
        else
        {
            if (degree != "")
                sel21 = " and r.degree_code in(" + "select distinct degree_code from degree where course_id in(" + degree + ") and college_code in(" + college + "))";
            else if (branch != "")
                sel22 = " and r.degree_code in(" + "select distinct degree_code from degree where  dept_code in(" + branch + ") and college_code in(" + college + "))";
        }
        //for (i = 0; i < cbldirect.Items.Count; i++)
        //{
        //    if (cbldirect.Items[i].Selected == true)
        //    {
        //        if (seattype == "")
        //        {
        //            seattype = cblseattype.Items[i].Value.ToString();
        //        }
        //        else
        //        {
        //            seattype = seattype + "," + cblseattype.Items[i].Value.ToString();
        //        }
        //    }
        //}
        //if (seattype != "")
        //{
        //    sel12 = " and a.seattype in(" + seattype + ") ";
        //    selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.seattype and textcriteria='seat')";
        //    columncount = columncount + 1;
        //    ItemList.Add("Seattype");
        //    Itemindex.Add("12");
        //}
        if (ddlpstreet1.SelectedValue == "---Select---")
        {
            sel23 = "";
            cblsearch.Items[32].Selected = true;
        }
        else if (ddlpstreet1.SelectedValue == "Others")
        {
            if (tbpstreet.Text != "")
            {
                if (ddlpstreet.SelectedValue == "Like")
                {
                    selection9 = "like '%" + tbstudname.Text + "%'";
                }
                else if (ddlpstreet.SelectedValue == "Starts with")
                {
                    selection9 = "like '" + tbstudname.Text + "%'";
                }
                else if (ddlpstreet.SelectedValue == "Ends with")
                {
                    selection9 = "like '%" + tbstudname.Text + "'";
                }
                sel23 = " and a.Streetp " + selection9 + "";
                selection = selection + ",a.Streetp";
                columncount = columncount + 1;
                if (!Itemindex.Contains("32"))
                {
                    ItemList.Add("Permanent Street");
                    Itemindex.Add("32");
                }
                cblsearch.Items[32].Selected = true;
            }
            else
            {
                cblsearch.Items[32].Selected = true;
            }
        }
        else
        {
            sel23 = " and a.Streetp ='" + ddlpstreet1.SelectedValue + "' ";
            selection = selection + ",a.Streetp";
            //columncount = columncount + 1;
            if (!Itemindex.Contains("32"))
            {
                ItemList.Add("Permanent Street");
                Itemindex.Add("32");
            }
            cblsearch.Items[32].Selected = true;
        }
        if (ddlpdistrict1.SelectedValue == "---Select---")
        {
            sel24 = "";
        }
        else if (ddlpdistrict1.SelectedValue == "Others")
        {
            if (tbpdistrict.Text != "")
            {
                if (ddlpdistrict.SelectedValue == "Like")
                {
                    selection10 = "like '%" + tbpdistrict.Text + "%'";
                }
                else if (ddlpdistrict.SelectedValue == "Starts with")
                {
                    selection10 = "like '" + tbpdistrict.Text + "%'";
                }
                else if (ddlpdistrict.SelectedValue == "Ends with")
                {
                    selection10 = "like '%" + tbpdistrict.Text + "'";
                }
                sel24 = " and a.Districtp " + selection10 + "";
                selection = selection + ",a.Districtp";
                columncount = columncount + 1;
                if (!Itemindex.Contains("30"))
                {
                    ItemList.Add("Permanent District");
                    Itemindex.Add("30");
                }
                cblsearch.Items[30].Selected = true;
            }
        }
        else
        {
            //Modified By Srinath 4/6/2014
            //sel24 = " and a.Districtp ='" + ddlpdistrict1.SelectedValue + "' ";
            sel24 = " and a.Districtp in(select cast(textcode as CHAR) from textvaltable where textval='" + ddlpdistrict1.SelectedItem.ToString() + "')";
            selection = selection + ",a.Districtp";
            columncount = columncount + 1;
            if (!Itemindex.Contains("30"))
            {
                ItemList.Add("Permanent District");
                Itemindex.Add("30");
            }
            cblsearch.Items[30].Selected = true;
        }
        if (ddlpcity1.SelectedValue == "---Select---")
        {
            sel25 = "";
        }
        else if (ddlpcity1.SelectedValue == "Others")
        {
            if (tbpcity.Text != "")
            {
                if (ddlpcity.SelectedValue == "Like")
                {
                    selection11 = "like '%" + tbpcity.Text + "%'";
                }
                else if (ddlpcity.SelectedValue == "Starts with")
                {
                    selection11 = "like '" + tbpcity.Text + "%'";
                }
                else if (ddlpcity.SelectedValue == "Ends with")
                {
                    selection11 = "like '%" + tbpcity.Text + "'";
                }
                sel25 = " and a.Cityp " + selection11 + "";
                selection = selection + ",a.Cityp";
                columncount = columncount + 1;
                if (!Itemindex.Contains("29"))
                {
                    ItemList.Add("Permanent City");
                    Itemindex.Add("29");
                }
                cblsearch.Items[29].Selected = true;
            }
        }
        else
        {
            sel25 = " and a.Cityp ='" + ddlpcity1.SelectedValue + "' ";
            selection = selection + ",a.Cityp";
            columncount = columncount + 1;
            if (!Itemindex.Contains("29"))
            {
                ItemList.Add("Permanent City");
                Itemindex.Add("29");
            }
            cblsearch.Items[29].Selected = true;
        }
        if (ddlpcountry1.SelectedValue == "---Select---")
        {
            sel26 = "";
        }
        else if (ddlpcountry1.SelectedValue == "Others")
        {
            if (tbpcountry.Text != "")
            {
                if (ddlpcountry.SelectedValue == "Like")
                {
                    selection12 = "like '%" + tbpcountry.Text + "%'";
                }
                else if (ddlpcountry.SelectedValue == "Starts with")
                {
                    selection12 = "like '" + tbpcountry.Text + "%'";
                }
                else if (ddlpcountry.SelectedValue == "Ends with")
                {
                    selection12 = "like '%" + tbpcountry.Text + "'";
                }
                sel26 = " and a.Countryp " + selection12 + "";
                selection = selection + ",a.Countryp";
                columncount = columncount + 1;
                if (!Itemindex.Contains("31"))
                {
                    ItemList.Add("Permanent Country");
                    Itemindex.Add("31");
                }
                cblsearch.Items[31].Selected = true;
            }
        }
        else
        {
            sel26 = " and a.Countryp ='" + ddlpcountry1.SelectedValue + "' ";
            selection = selection + ",a.Countryp";
            columncount = columncount + 1;
            if (!Itemindex.Contains("31"))
            {
                ItemList.Add("Permanent Country");
                Itemindex.Add("31");
            }
            cblsearch.Items[31].Selected = true;
        }
        if (ddlcstreet1.SelectedValue == "---Select---")
        {
            sel27 = "";
        }
        else if (ddlcstreet1.SelectedValue == "Others")
        {
            if (tbcstreet.Text != "")
            {
                if (ddlcstreet.SelectedValue == "Like")
                {
                    selection13 = "like '%" + tbcstreet.Text + "%'";
                }
                else if (ddlcstreet.SelectedValue == "Starts with")
                {
                    selection13 = "like '" + tbcstreet.Text + "%'";
                }
                else if (ddlcstreet.SelectedValue == "Ends with")
                {
                    selection13 = "like '%" + tbcstreet.Text + "'";
                }
                sel27 = " and a.Streetc " + selection13 + "";
                selection = selection + ",a.Streetc";
                columncount = columncount + 1;
                if (!Itemindex.Contains("36"))
                {
                    ItemList.Add("Contact Street");
                    Itemindex.Add("36");
                }
                cblsearch.Items[36].Selected = true;
            }
        }
        else
        {
            sel27 = " and a.Streetc ='" + ddlcstreet1.SelectedValue + "' ";
            selection = selection + ",a.guardian_name";
            columncount = columncount + 1;
            if (!Itemindex.Contains("36"))
            {
                ItemList.Add("Contact Street");
                Itemindex.Add("36");
            }
            cblsearch.Items[36].Selected = true;
        }
        if (ddlcdistrict1.SelectedValue == "---Select---")
        {
            sel28 = "";
        }
        else if (ddlcdistrict1.SelectedValue == "Others")
        {
            if (tbcdistrict.Text != "")
            {
                if (ddlcdistrict.SelectedValue == "Like")
                {
                    selection14 = "like '%" + tbcdistrict.Text + "%'";
                }
                else if (ddlcdistrict.SelectedValue == "Starts with")
                {
                    selection14 = "like '" + tbcdistrict.Text + "%'";
                }
                else if (ddlcdistrict.SelectedValue == "Ends with")
                {
                    selection14 = "like '%" + tbcdistrict.Text + "'";
                }
                sel28 = " and a.Districtc " + selection14 + "";
                selection = selection + ",a.Districtc";
                columncount = columncount + 1;
                if (!Itemindex.Contains("34"))
                {
                    ItemList.Add("Contact District");
                    Itemindex.Add("34");
                }
                cblsearch.Items[34].Selected = true;
            }
        }
        else
        {
            //sel28 = " and a.Districtc ='" + ddlcdistrict1.SelectedValue + "' ";
            sel28 = " and a.Districtc in (select cast(textcode as CHAR) from textvaltable where textval='" + ddlcdistrict1.SelectedItem.ToString() + "')";
            selection = selection + ",a.Districtc";
            columncount = columncount + 1;
            if (!Itemindex.Contains("34"))
            {
                ItemList.Add("Contact District");
                Itemindex.Add("34");
            }
            cblsearch.Items[34].Selected = true;
        }
        if (ddlccity1.SelectedValue == "---Select---")
        {
            sel29 = "";
        }
        else if (ddlccity1.SelectedValue == "Others")
        {
            if (tbccity.Text != "")
            {
                if (ddlccity.SelectedValue == "Like")
                {
                    selection15 = "like '%" + tbccity.Text + "%'";
                }
                else if (ddlccity.SelectedValue == "Starts with")
                {
                    selection15 = "like '" + tbccity.Text + "%'";
                }
                else if (ddlccity.SelectedValue == "Ends with")
                {
                    selection15 = "like '%" + tbccity.Text + "'";
                }
                sel29 = " and a.Cityc " + selection15 + "";
                selection = selection + ",a.Cityc";
                columncount = columncount + 1;
                if (!Itemindex.Contains("33"))
                {
                    ItemList.Add("Contact City");
                    Itemindex.Add("33");
                }
                cblsearch.Items[33].Selected = true;
            }
        }
        else
        {
            sel29 = " and a.Cityc ='" + ddlccity1.SelectedValue + "' ";
            selection = selection + ",a.Cityc";
            columncount = columncount + 1;
            if (!Itemindex.Contains("33"))
            {
                ItemList.Add("Contact City");
                Itemindex.Add("33");
            }
            cblsearch.Items[33].Selected = true;
        }
        if (ddlccountry1.SelectedValue == "---Select---")
        {
            sel30 = "";
        }
        else if (ddlccountry1.SelectedValue == "Others")
        {
            if (tbccountry.Text != "")
            {
                if (ddlccountry.SelectedValue == "Like")
                {
                    selection16 = "like '%" + tbccountry.Text + "%'";
                }
                else if (ddlccountry.SelectedValue == "Starts with")
                {
                    selection16 = "like '" + tbccountry.Text + "%'";
                }
                else if (ddlccountry.SelectedValue == "Ends with")
                {
                    selection16 = "like '%" + tbccountry.Text + "'";
                }
                sel30 = " and a.Countryc " + selection16 + "";
                selection = selection + ",a.Countryc";
                columncount = columncount + 1;
                if (!Itemindex.Contains("35"))
                {
                    ItemList.Add("Contact Country");
                    Itemindex.Add("35");
                }
                cblsearch.Items[35].Selected = true;
            }
        }
        else
        {
            sel30 = " and a.Countryc ='" + ddlccountry1.SelectedValue + "' ";
            selection = selection + ",a.Countryc";
            columncount = columncount + 1;
            if (!Itemindex.Contains("35"))
            {
                ItemList.Add("Contact Country");
                Itemindex.Add("35");
            }
            cblsearch.Items[35].Selected = true;
        }
        if (ddlgstreet1.SelectedValue == "---Select---")
        {
            sel31 = "";
        }
        else if (ddlgstreet1.SelectedValue == "Others")
        {
            if (tbgstreet.Text != "")
            {
                if (ddlgstreet.SelectedValue == "Like")
                {
                    selection17 = "like '%" + tbgstreet.Text + "%'";
                }
                else if (ddlgstreet.SelectedValue == "Starts with")
                {
                    selection17 = "like '" + tbgstreet.Text + "%'";
                }
                else if (ddlgstreet.SelectedValue == "Ends with")
                {
                    selection17 = "like '%" + tbgstreet.Text + "'";
                }
                sel31 = " and a.Streetg " + selection17 + "";
                selection = selection + ",a.Streetg";
                columncount = columncount + 1;
                if (!Itemindex.Contains("40"))
                {
                    ItemList.Add("Guardian Street");
                    Itemindex.Add("40");
                }
                cblsearch.Items[40].Selected = true;
            }
        }
        else
        {
            sel31 = " and a.Streetg ='" + ddlgstreet1.SelectedValue + "' ";
            selection = selection + ",a.Streetg";
            columncount = columncount + 1;
            if (!Itemindex.Contains("40"))
            {
                ItemList.Add("Guardian Street");
                Itemindex.Add("40");
            }
            cblsearch.Items[40].Selected = true;
        }
        if (ddlgdistrict1.SelectedValue == "---Select---")
        {
            sel32 = "";
        }
        else if (ddlgdistrict1.SelectedValue == "Others")
        {
            if (tbgdistrict.Text != "")
            {
                if (ddlgdistrict.SelectedValue == "Like")
                {
                    selection18 = "like '%" + tbgdistrict.Text + "%'";
                }
                else if (ddlgdistrict.SelectedValue == "Starts with")
                {
                    selection18 = "like '" + tbgdistrict.Text + "%'";
                }
                else if (ddlgdistrict.SelectedValue == "Ends with")
                {
                    selection18 = "like '%" + tbgdistrict.Text + "'";
                }
                sel32 = " and a.Districtg " + selection18 + "";
                selection = selection + ",a.Districtg";
                columncount = columncount + 1;
                if (!Itemindex.Contains("38"))
                {
                    ItemList.Add("Guardian District");
                    Itemindex.Add("38");
                }
                cblsearch.Items[38].Selected = true;
            }
        }
        else
        {
            sel32 = " and a.Districtg ='" + ddlgdistrict1.SelectedValue + "' ";
            selection = selection + ",a.Districtg";
            columncount = columncount + 1;
            if (!Itemindex.Contains("38"))
            {
                ItemList.Add("Guardian District");
                Itemindex.Add("38");
            }
            cblsearch.Items[38].Selected = true;
        }
        if (ddlgcity1.SelectedValue == "---Select---")
        {
            sel33 = "";
        }
        else if (ddlgcity1.SelectedValue == "Others")
        {
            if (tbgcity.Text != "")
            {
                if (ddlgcity.SelectedValue == "Like")
                {
                    selection19 = "like '%" + tbgcity.Text + "%'";
                }
                else if (ddlgcity.SelectedValue == "Starts with")
                {
                    selection19 = "like '" + tbgcity.Text + "%'";
                }
                else if (ddlgcity.SelectedValue == "Ends with")
                {
                    selection19 = "like '%" + tbgcity.Text + "'";
                }
                sel33 = " and a.Cityg " + selection19 + "";
                selection = selection + ",a.Cityg";
                columncount = columncount + 1;
                if (!Itemindex.Contains("37"))
                {
                    ItemList.Add("Guardian City");
                    Itemindex.Add("37");
                }
                cblsearch.Items[37].Selected = true;
            }
        }
        else
        {
            sel33 = " and a.Cityg ='" + ddlgcity1.SelectedValue + "' ";
            selection = selection + ",a.Cityg";
            columncount = columncount + 1;
            if (!Itemindex.Contains("37"))
            {
                ItemList.Add("Guardian City");
                Itemindex.Add("37");
            }
            cblsearch.Items[37].Selected = true;
        }
        if (ddlgcountry1.SelectedValue == "---Select---")
        {
            sel34 = "";
        }
        else if (ddlgcountry1.SelectedValue == "Others")
        {
            if (tbgcountry.Text != "")
            {
                if (ddlgcountry.SelectedValue == "Like")
                {
                    selection20 = "like '%" + tbgcountry.Text + "%'";
                }
                else if (ddlgcountry.SelectedValue == "Starts with")
                {
                    selection20 = "like '" + tbgcountry.Text + "%'";
                }
                else if (ddlgcountry.SelectedValue == "Ends with")
                {
                    selection20 = "like '%" + tbgcountry.Text + "'";
                }
                sel34 = " and a.Countryg " + selection20 + "";
                selection = selection + ",a.Countryg";
                columncount = columncount + 1;
                if (!Itemindex.Contains("39"))
                {
                    ItemList.Add("Guardian Country");
                    Itemindex.Add("39");
                }
                cblsearch.Items[39].Selected = true;
            }
        }
        else
        {
            sel34 = " and a.Countryg ='" + ddlgcountry1.SelectedValue + "' ";
            selection = selection + ",a.Countryg";
            columncount = columncount + 1;
            if (!Itemindex.Contains("39"))
            {
                ItemList.Add("Guardian Country");
                Itemindex.Add("39");
            }
            cblsearch.Items[39].Selected = true;
        }
        //statep
        string sel_p = "";
        if (ddlpstate1.SelectedValue == "---Select---")
        {
            sel_p = "";
        }
        else if (ddlpstate1.SelectedValue == "Others")
        {
            if (tbstatep.Text != "")
            {
                string sel_temp = "";
                if (ddlpstate.SelectedValue == "Like")
                {
                    sel_temp = "like '%" + tbstatep.Text + "%'";
                }
                else if (ddlpstate.SelectedValue == "Starts with")
                {
                    sel_temp = "like '" + tbstatep.Text + "%'";
                }
                else if (ddlpstate.SelectedValue == "Ends with")
                {
                    sel_temp = "like '%" + tbstatep.Text + "'";
                }
                //code 4 state
                string state = "";
                cmd.CommandText = " select distinct textcode  from textvaltable  where textcriteria='state'  " + college_code + " and textval " + sel_temp;
                cmd.Connection = tcon;
                tcon.Open();
                SqlDataReader rstate = cmd.ExecuteReader();
                if (rstate.HasRows)
                    while (rstate.Read())
                    {
                        if (state == "")
                            state = rstate.GetValue(0).ToString();
                        else
                            state = state + "," + rstate.GetValue(0).ToString();
                    }
                rstate.Close();
                tcon.Close();
                if (state != "")
                    sel_p = " and a.stateg in(" + state + ")";
                //(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.bldgrp and textcriteria='bgrou')";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.parent_statep and textcriteria='state'  " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("50"))
                {
                    ItemList.Add("Permanent state");
                    Itemindex.Add("50");
                }
                cblsearch.Items[50].Selected = true;
            }
        }
        else
        {
            //MNodified By Srinath 4/6/2014
            //sel_p = " and a.parent_statep ='" + ddlpstate1.SelectedValue + "' ";
            sel_p = " and a.parent_statep in (select TextCode from textvaltable where textval='" + ddlpstate1.SelectedItem.ToString() + "')";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.parent_statep and textcriteria='state'  " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("50"))
            {
                ItemList.Add("Permanent State");
                Itemindex.Add("50");
            }
            cblsearch.Items[50].Selected = true;
        }
        //
        //statec
        string sel_c = "";
        if (ddlcstate1.SelectedValue == "---Select---")
        {
            sel_c = "";
        }
        else if (ddlcstate1.SelectedValue == "Others")
        {
            if (tbstatec.Text != "")
            {
                string sel_temp = "";
                if (ddlcstate.SelectedValue == "Like")
                {
                    sel_temp = "like '%" + tbstatec.Text + "%'";
                }
                else if (ddlcstate.SelectedValue == "Starts with")
                {
                    sel_temp = "like '" + tbstatec.Text + "%'";
                }
                else if (ddlcstate.SelectedValue == "Ends with")
                {
                    sel_temp = "like '%" + tbstatec.Text + "'";
                }
                //code 4 state
                string state = "";
                cmd.CommandText = " select distinct textcode  from textvaltable  where textcriteria='state'  " + college_code + " and textval " + sel_temp;
                cmd.Connection = tcon;
                tcon.Open();
                SqlDataReader rstate = cmd.ExecuteReader();
                if (rstate.HasRows)
                    while (rstate.Read())
                    {
                        if (state == "")
                            state = rstate.GetValue(0).ToString();
                        else
                            state = state + "," + rstate.GetValue(0).ToString();
                    }
                rstate.Close();
                tcon.Close();
                if (state != "")
                    sel_c = " and a.stateg in(" + state + ")";
                //(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.bldgrp and textcriteria='bgrou')";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.parent_statec and textcriteria='state'  " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("51"))
                {
                    ItemList.Add("Contact state");
                    Itemindex.Add("51");
                }
                cblsearch.Items[51].Selected = true;
            }
        }
        else
        {
            //MNodified By Srinath 4/6/2014
            // sel_c = " and a.parent_statec ='" + ddlcstate1.SelectedValue + "' ";
            sel_c = " and a.parent_statec in (select TextCode from textvaltable where textval='" + ddlcstate1.SelectedItem.ToString() + "')";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.parent_statec and textcriteria='state'  " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("51"))
            {
                ItemList.Add("Contact State");
                Itemindex.Add("51");
            }
            cblsearch.Items[51].Selected = true;
        }
        //
        //stateg
        string sel_g = "";
        if (ddlgstate1.SelectedValue == "---Select---")
        {
            sel_g = "";
        }
        else if (ddlgstate1.SelectedValue == "Others")
        {
            if (tbstateg.Text != "")
            {
                string sel_temp = "";
                if (ddlgstate.SelectedValue == "Like")
                {
                    sel_temp = "like '%" + tbstateg.Text + "%'";
                }
                else if (ddlgstate.SelectedValue == "Starts with")
                {
                    sel_temp = "like '" + tbstateg.Text + "%'";
                }
                else if (ddlgstate.SelectedValue == "Ends with")
                {
                    sel_temp = "like '%" + tbstateg.Text + "'";
                }
                //code 4 state
                string state = "";
                cmd.CommandText = " select distinct textcode  from textvaltable  where textcriteria='state'  " + college_code + " and textval " + sel_temp;
                cmd.Connection = tcon;
                tcon.Open();
                SqlDataReader rstate = cmd.ExecuteReader();
                if (rstate.HasRows)
                    while (rstate.Read())
                    {
                        if (state == "")
                            state = rstate.GetValue(0).ToString();
                        else
                            state = state + "," + rstate.GetValue(0).ToString();
                    }
                rstate.Close();
                tcon.Close();
                if (state != "")
                    sel_g = " and a.stateg in(" + state + ")";
                //(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.bldgrp and textcriteria='bgrou')";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.stateg and textcriteria='state'  " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("52"))
                {
                    ItemList.Add("Guardian state");
                    Itemindex.Add("52");
                }
                cblsearch.Items[52].Selected = true;
            }
        }
        else
        {
            //sel_g = " and a.stateg ='" + ddlgstate1.SelectedValue + "' ";
            sel_g = " and a.stateg in(select cast(textcode as CHAR) from textvaltable where textval='" + ddlgstate1.SelectedItem.ToString() + "')";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.stateg and textcriteria='state'  " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("52"))
            {
                ItemList.Add("Guardian State");
                Itemindex.Add("52");
            }
            cblsearch.Items[52].Selected = true;
        }
        //end state
        if (tbfoffno.Text != "")
        {
            sel35 = " and a.parentF_offPhn ='" + tbfoffno.Text + "' ";
            selection = selection + ",a.parentF_offPhn";
            columncount = columncount + 1;
            if (!Itemindex.Contains("22"))
            {
                ItemList.Add("Father Office Phone No");
                Itemindex.Add("22");
            }
            cblsearch.Items[22].Selected = true;
        }
        if (tbfmobno.Text != "")
        {
            sel36 = " and a.parentF_Mobile ='" + tbfmobno.Text + "' ";
            selection = selection + ",a.parentF_Mobile";
            columncount = columncount + 1;
            if (!Itemindex.Contains("21"))
            {
                ItemList.Add("Father Mobile No");
                Itemindex.Add("21");
            }
            cblsearch.Items[21].Selected = true;
        }
        if (tbmmobno.Text != "")
        {
            sel37 = " and a.parentM_Mobile ='" + tbmmobno.Text + "' ";
            selection = selection + ",a.parentM_Mobile";
            columncount = columncount + 1;
            if (!Itemindex.Contains("25"))
            {
                ItemList.Add("Mother Mobile No");
                Itemindex.Add("25");
            }
            cblsearch.Items[25].Selected = true;
        }
        if (tbmoffno.Text != "")
        {
            sel38 = " and a.parentM_offPhn='" + tbmoffno.Text + "' ";
            selection = selection + ",a.parentM_offPhn";
            columncount = columncount + 1;
            if (!Itemindex.Contains("26"))
            {
                ItemList.Add("Mother Office Phone no");
                Itemindex.Add("26");
            }
            cblsearch.Items[26].Selected = true;
        }
        if (ddlpemailid1.SelectedValue == "---Select---")
        {
            sel39 = "";
        }
        else
        {
            sel39 = " and a.StuPer_Id='" + ddlpemailid1.SelectedItem.Text.ToString() + "'";
            selection = selection + ",a.StuPer_Id";
            columncount = columncount + 1;
            if (!Itemindex.Contains("28"))
            {
                ItemList.Add("Email Id");
                Itemindex.Add("28");
            }
            cblsearch.Items[28].Selected = true;
        }
        if (ddllaststudied.SelectedValue == "All")
        {
            laststudiedsel = "";
        }
        else
        {
            laststudiedsel = " and s.institute_name='" + ddllaststudied.SelectedItem.Text.ToString() + "'";
            //cmd.CommandText = "select app_no from stud_prev_details where institute_name='" + ddllaststudied.SelectedItem.Text.ToString()+"'";
            //cmd.Connection = con;
            //con.Open();
            //SqlDataReader laststuddr = cmd.ExecuteReader();
            //if (laststuddr.HasRows)
            //{
            //    while (laststuddr.Read())
            //    {
            //        if (previous == "")
            //        {
            //            previous = laststuddr.GetValue(0).ToString();
            //        }
            //        else
            //        {
            //            previous = previous + "," + laststuddr.GetValue(0).ToString();
            //        }
            //    }
            //}
            //laststuddr.Close();
            //con.Close();
        }
        if (previous != "")
        {
            laststudiedsel = " and r.app_no in(" + previous + ")";
            if (!Itemindex.Contains("49"))
            {
                ItemList.Add("Last Studied School/College Name");
                Itemindex.Add("49");
            }
            cblsearch.Items[49].Selected = true;
        }
        if (tbstudmobno.Text != "")
        {
            sel40 = " and a.Student_Mobile ='" + tbstudmobno.Text + "' ";
            selection = selection + ",a.Student_Mobile";
            columncount = columncount + 1;
            if (!Itemindex.Contains("27"))
            {
                ItemList.Add("Mobile No");
                Itemindex.Add("27");
            }
            cblsearch.Items[27].Selected = true;
        }
        for (i = 0; i < cblblood.Items.Count; i++)
        {
            if (cblblood.Items[i].Selected == true)
            {
                if (blood == "")
                {
                    blood = "'" + cblblood.Items[i].Text.ToString() + "'";
                }
                else
                {
                    blood = blood + ",'" + cblblood.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (blood != "")
        {
            sel41 = " and a.bldgrp in(select distinct textcode from textvaltable where textcriteria='bgrou' and textval in(" + blood + ") " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.bldgrp and textcriteria='bgrou' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("13"))
            {
                ItemList.Add("Blood Group");
                Itemindex.Add("13");
            }
            cblsearch.Items[13].Selected = true;
        }
        if ((ddlbatchyrfrm.SelectedValue.ToString() != "---Select---") && (ddlbatchyrto.SelectedValue.ToString() != "---Select---"))
        {
            sel42 = " and r.batch_year between '" + ddlbatchyrfrm.SelectedValue.ToString() + "' and '" + ddlbatchyrto.SelectedValue.ToString() + "' ";
            selection = selection + ",CONVERT(VARCHAR(10),r.batch_year,103)";
            columncount = columncount + 1;
            if (!Itemindex.Contains("41"))
            {
                ItemList.Add("Batch Year");
                Itemindex.Add("41");
            }
            cblsearch.Items[41].Selected = true;
        }
        for (i = 0; i < cblmtongue.Items.Count; i++)
        {
            if (cblmtongue.Items[i].Selected == true)
            {
                if (mton == "")
                {
                    mton = "'" + cblmtongue.Items[i].Text.ToString() + "'";
                }
                else
                {
                    mton = mton + ",'" + cblmtongue.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (mton != "")
        {
            sel43 = " and a.mother_tongue in(select distinct textcode from textvaltable where textcriteria='mton' and textval in(" + mton + ") " + college_code + ") ";
            selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.mother_tongue and textcriteria='mton' " + college_code + ")";
            columncount = columncount + 1;
            if (!Itemindex.Contains("18"))
            {
                ItemList.Add("Mother Tongue");
                Itemindex.Add("18");
            }
            cblsearch.Items[18].Selected = true;
        }
        for (i = 0; i < cblsem.Items.Count; i++)
        {
            if (cblsem.Items[i].Selected == true)
            {
                if (sem == "")
                {
                    sem = cblsem.Items[i].Text.Trim().ToString();
                }
                else
                {
                    sem = sem + "," + cblsem.Items[i].Text.Trim().ToString();
                }
            }
        }
        if (sem != "")
        {
            sel44 = " and r.current_semester in(" + sem + ") ";
            selection = selection + ",r.current_semester";
            columncount = columncount + 1;
            if (!Itemindex.Contains("44"))
            {
                ItemList.Add("Semester");
                Itemindex.Add("44");
            }
            cblsearch.Items[44].Selected = true;
        }
        //for (i = 0; i < cblsection.Items.Count; i++)
        //{
        //    if (cblsection.Items[i].Selected == true)
        //    {
        //        if (section == "")
        //        {
        //            section = cblsection.Items[i].Value.ToString();
        //        }
        //        else
        //        {
        //            section = section + "," + cblsection.Items[i].Value.ToString();
        //        }
        //    }
        //}
        //if (section != "")
        //{
        //    sel45 = " and r.current_semester in(" + section + ") ";
        //}
        for (i = 0; i < cbldirect.Items.Count; i++)
        {
            if (cbldirect.Items[i].Selected == true)
            {
                if (direct == "")
                {
                    direct = "'" + cbldirect.Items[i].Value.ToString() + "'";
                }
                else
                {
                    direct = direct + "," + "'" + cbldirect.Items[i].Value.ToString() + "'";
                }
            }
        }
        if (direct != "")
        {
            sel45 = " and a.refer_name in(" + direct + ") ";
            selection = selection + ",a.refer_name";
            columncount = columncount + 1;
            if (!Itemindex.Contains("45"))
            {
                ItemList.Add("Direct Reference");
                Itemindex.Add("45");
            }
            cblsearch.Items[45].Selected = true;
        }
        for (i = 0; i < cblstaffname.Items.Count; i++)
        {
            if (cblstaffname.Items[i].Selected == true)
            {
                if (staffname == "")
                {
                    staffname = "'" + cblstaffname.Items[i].Value.ToString() + "'";
                }
                else
                {
                    staffname = staffname + "," + "'" + cblstaffname.Items[i].Value.ToString() + "'";
                }
            }
        }
        if (staffname != "")
        {
            sel46 = " and a.refer_name in(" + staffname + ") ";
            selection = selection + ",a.refer_name";
            columncount = columncount + 1;
            if (!Itemindex.Contains("46"))
            {
                ItemList.Add("Referred by Staff");
                Itemindex.Add("46");
            }
            cblsearch.Items[46].Selected = true;
        }
        if (tbothers.Enabled == true)
        {
            sel47 = " and a.refer_agent='" + tbothers.SelectedItem.Text + "'";
            selection = selection + ",a.refer_agent";
            columncount = columncount + 1;
            if (!Itemindex.Contains("47"))
            {
                ItemList.Add("Referred by Others");
                Itemindex.Add("47");
            }
            cblsearch.Items[47].Selected = true;
        }
        collegesel = ",a.college_code=" + colleges + "";
        //start aruna 19july2013==============================================================================
        if (drp_padress.SelectedValue == "---Select---")
        {
            sel48 = "";
        }
        else if (drp_padress.SelectedValue == "Others")
        {
            if (txt_padress.Text != "")
            {
                if (drp_padress1.SelectedValue == "Like")
                {
                    selection22 = "like '%" + txt_padress.Text + "%'";
                }
                else if (drp_padress1.SelectedValue == "Starts with")
                {
                    selection22 = "like '" + txt_padress.Text + "%'";
                }
                else if (drp_padress1.SelectedValue == "Ends with")
                {
                    selection22 = "like '%" + txt_padress.Text + "'";
                }
                sel48 = " and a.parent_addressp " + selection22 + "";
                selection = selection + ",a.parent_addressp";
                columncount = columncount + 1;
                if (!Itemindex.Contains("63"))
                {
                    ItemList.Add("Permanent Address");
                    Itemindex.Add("63");
                }
                cblsearch.Items[63].Selected = true;
            }
        }
        else
        {
            sel48 = " and a.parent_addressp ='" + drp_padress.SelectedValue + "' ";
            selection = selection + ",a.parent_addressp";
            columncount = columncount + 1;
            if (!Itemindex.Contains("63"))
            {
                ItemList.Add("Permanent Address");
                Itemindex.Add("63");
            }
            cblsearch.Items[63].Selected = true;
        }
        if (drp_cadress.SelectedValue == "---Select---")
        {
            sel49 = "";
        }
        else if (drp_cadress.SelectedValue == "Others")
        {
            if (txt_cadress.Text != "")
            {
                if (drp_cadress1.SelectedValue == "Like")
                {
                    selection23 = "like '%" + txt_cadress.Text + "%'";
                }
                else if (drp_cadress1.SelectedValue == "Starts with")
                {
                    selection23 = "like '" + txt_cadress.Text + "%'";
                }
                else if (drp_cadress1.SelectedValue == "Ends with")
                {
                    selection23 = "like '%" + txt_cadress.Text + "'";
                }
                sel49 = " and a.parent_addressc " + selection23 + "";
                selection = selection + ",a.parent_addressc";
                columncount = columncount + 1;
                if (!Itemindex.Contains("64"))
                {
                    ItemList.Add("Contact Address");
                    Itemindex.Add("64");
                }
                cblsearch.Items[64].Selected = true;
            }
        }
        else
        {
            sel49 = " and a.parent_addressc ='" + drp_cadress.SelectedValue + "' ";
            selection = selection + ",a.parent_addressc";
            columncount = columncount + 1;
            if (!Itemindex.Contains("64"))
            {
                ItemList.Add("Contact Address");
                Itemindex.Add("64");
            }
            cblsearch.Items[64].Selected = true;
        }
        if (drp_gadress.SelectedValue == "---Select---")
        {
            sel50 = "";
        }
        else if (drp_gadress.SelectedValue == "Others")
        {
            if (txt_gadress.Text != "")
            {
                if (drp_gadress1.SelectedValue == "Like")
                {
                    selection24 = "like '%" + txt_gadress.Text + "%'";
                }
                else if (drp_gadress1.SelectedValue == "Starts with")
                {
                    selection24 = "like '" + txt_gadress.Text + "%'";
                }
                else if (drp_gadress1.SelectedValue == "Ends with")
                {
                    selection24 = "like '%" + txt_gadress.Text + "'";
                }
                sel50 = " and a.addressg " + selection24 + "";
                selection = selection + ",a.addressg";
                columncount = columncount + 1;
                if (!Itemindex.Contains("65"))
                {
                    ItemList.Add("Guardian Address");
                    Itemindex.Add("65");
                }
                cblsearch.Items[65].Selected = true;
            }
        }
        else
        {
            sel50 = " and a.addressg ='" + drp_gadress.SelectedValue + "' ";
            selection = selection + ",a.addressg";
            columncount = columncount + 1;
            if (!Itemindex.Contains("65"))
            {
                ItemList.Add("Guardian Address");
                Itemindex.Add("65");
            }
            cblsearch.Items[65].Selected = true;
        }
        if (drp_ppincode.SelectedValue == "---Select---")
        {
            sel51 = "";
        }
        else if (drp_ppincode.SelectedValue == "Others")
        {
            if (txt_ppincode.Text != "")
            {
                if (drp_ppincode1.SelectedValue == "Like")
                {
                    selection25 = "like '%" + txt_ppincode.Text + "%'";
                }
                else if (drp_ppincode1.SelectedValue == "Starts with")
                {
                    selection25 = "like '" + txt_ppincode.Text + "%'";
                }
                else if (drp_ppincode1.SelectedValue == "Ends with")
                {
                    selection25 = "like '%" + txt_ppincode.Text + "'";
                }
                sel51 = " and a.parent_pincodep " + selection25 + "";
                selection = selection + ",a.parent_pincodep";
                columncount = columncount + 1;
                if (!Itemindex.Contains("66"))
                {
                    ItemList.Add("Permanent Pincode");
                    Itemindex.Add("66");
                }
                cblsearch.Items[66].Selected = true;
            }
        }
        else
        {
            sel51 = " and a.parent_pincodep ='" + drp_ppincode.SelectedValue + "' ";
            selection = selection + ",a.parent_pincodep";
            columncount = columncount + 1;
            if (!Itemindex.Contains("66"))
            {
                ItemList.Add("Permanent Pincode");
                Itemindex.Add("66");
            }
            cblsearch.Items[66].Selected = true;
        }
        if (drp_cpincode.SelectedValue == "---Select---")
        {
            sel52 = "";
        }
        else if (drp_cpincode.SelectedValue == "Others")
        {
            if (txt_cpincode.Text != "")
            {
                if (drp_cpincode1.SelectedValue == "Like")
                {
                    selection26 = "like '%" + txt_cpincode.Text + "%'";
                }
                else if (drp_cpincode1.SelectedValue == "Starts with")
                {
                    selection26 = "like '" + txt_cpincode.Text + "%'";
                }
                else if (drp_cpincode1.SelectedValue == "Ends with")
                {
                    selection26 = "like '%" + txt_cpincode.Text + "'";
                }
                sel52 = " and a.parent_pincodec " + selection26 + "";
                selection = selection + ",a.parent_pincodec";
                columncount = columncount + 1;
                if (!Itemindex.Contains("67"))
                {
                    ItemList.Add("Contact Pincode");
                    Itemindex.Add("67");
                }
                cblsearch.Items[67].Selected = true;
            }
        }
        else
        {
            sel52 = " and a.parent_pincodec ='" + drp_cpincode.SelectedValue + "' ";
            selection = selection + ",a.parent_pincodec";
            columncount = columncount + 1;
            if (!Itemindex.Contains("67"))
            {
                ItemList.Add("Contact Pincode");
                Itemindex.Add("67");
            }
            cblsearch.Items[67].Selected = true;
        }
        if (drp_gpincode.SelectedValue == "---Select---")
        {
            sel53 = "";
        }
        else if (drp_gpincode.SelectedValue == "Others")
        {
            if (txt_gpincode.Text != "")
            {
                if (drp_gpincode1.SelectedValue == "Like")
                {
                    selection27 = "like '%" + txt_gpincode.Text + "%'";
                }
                else if (drp_gpincode1.SelectedValue == "Starts with")
                {
                    selection27 = "like '" + txt_gpincode.Text + "%'";
                }
                else if (drp_gpincode1.SelectedValue == "Ends with")
                {
                    selection27 = "like '%" + txt_gpincode.Text + "'";
                }
                sel53 = " and a.ping " + selection27 + "";
                selection = selection + ",a.ping";
                columncount = columncount + 1;
                if (!Itemindex.Contains("68"))
                {
                    ItemList.Add("Guardian Pincode");
                    Itemindex.Add("68");
                }
                cblsearch.Items[68].Selected = true;
            }
        }
        else
        {
            sel53 = " and a.ping ='" + drp_gpincode.SelectedValue + "' ";
            selection = selection + ",a.ping";
            columncount = columncount + 1;
            if (!Itemindex.Contains("68"))
            {
                ItemList.Add("Guardian Pincode");
                Itemindex.Add("68");
            }
            cblsearch.Items[68].Selected = true;
        }
        //====================================================================================================
        cblsearch.ClearSelection();
        for (int it = 0; it < Itemindex.Count; it++)
        {
            string t = Itemindex[it].ToString();
            int te = Convert.ToInt32(t);
            cblsearch.Items[te].Selected = true;
        }
        string[] search = new string[100];
        if (cblsearch.Items[0].Selected == true)
        {
            search[0] = "a.App_Formno";
        }
        if (cblsearch.Items[1].Selected == true)
        {
            search[1] = "r.Roll_Admit";
        }
        if (cblsearch.Items[2].Selected == true)
        {
            search[2] = "r.Roll_No";
        }
        if (cblsearch.Items[3].Selected == true)
        {
            search[3] = "r.Reg_No";
        }
        if (cblsearch.Items[4].Selected == true)
        {
            search[4] = "r.Stud_Name";
        }
        if (cblsearch.Items[5].Selected == true)
        {
            search[5] = " case a.sex when 1 then 'Female' else 'Male' end";
        }
        if (cblsearch.Items[6].Selected == true)
        {
            search[6] = "CONVERT(VARCHAR(10),a.Date_Applied,103)";
        }
        if (cblsearch.Items[7].Selected == true)
        {
            search[7] = "CONVERT(VARCHAR(10),r.Adm_Date,103)";
        }
        if (cblsearch.Items[8].Selected == true)
        {
            search[8] = "CONVERT(VARCHAR(10),a.dob,103)";
        }
        if (cblsearch.Items[9].Selected == true)
        {
            search[9] = "a.parent_name";
        }
        if (cblsearch.Items[10].Selected == true)
        {
            search[10] = "a.mother";
        }
        if (cblsearch.Items[11].Selected == true)
        {
            search[11] = "a.guardian_name";
        }
        if (cblsearch.Items[12].Selected == true)
        {
            search[12] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.seattype and textcriteria='seat' " + college_code + ")";
        }
        if (cblsearch.Items[13].Selected == true)
        {
            search[13] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.bldgrp and textcriteria='bgrou' " + college_code + ")";
        }
        if (cblsearch.Items[14].Selected == true)
        {
            search[14] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.caste and textcriteria='caste' " + college_code + ")";
        }
        if (cblsearch.Items[15].Selected == true)
        {
            search[15] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.religion and textcriteria='relig' " + college_code + ")";
        }
        if (cblsearch.Items[16].Selected == true)
        {
            search[16] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.community and textcriteria='comm' " + college_code + ")";
        }
        if (cblsearch.Items[17].Selected == true)
        {
            search[17] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.region and textcriteria='Reg' " + college_code + ")";
        }
        if (cblsearch.Items[18].Selected == true)
        {
            search[18] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.mother_tongue and textcriteria='mton' " + college_code + ")";
        }
        if (cblsearch.Items[19].Selected == true)
        {
            search[19] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.parent_occu and textcriteria='foccu' " + college_code + ")";
        }
        if (cblsearch.Items[20].Selected == true)
        {
            search[20] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.fqualification and textcriteria='fqual' " + college_code + ")";
        }
        if (cblsearch.Items[21].Selected == true)
        {
            search[21] = "a.parentF_Mobile";
        }
        if (cblsearch.Items[22].Selected == true)
        {
            search[22] = "a.parentF_offPhn";
        }
        if (cblsearch.Items[23].Selected == true)
        {
            search[23] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.motherocc and textcriteria='moccu' " + college_code + ")";
        }
        if (cblsearch.Items[24].Selected == true)
        {
            search[24] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.mQualification and textcriteria='mqual' " + college_code + ")";
        }
        if (cblsearch.Items[25].Selected == true)
        {
            search[25] = "a.parentM_Mobile";
        }
        if (cblsearch.Items[26].Selected == true)
        {
            search[26] = "a.parentM_offPhn";
        }
        if (cblsearch.Items[27].Selected == true)
        {
            search[27] = "a.Student_Mobile";
        }
        if (cblsearch.Items[28].Selected == true)
        {
            search[28] = "a.StuPer_Id";
        }
        if (cblsearch.Items[29].Selected == true)
        {
            search[29] = "a.Cityp";
        }
        if (cblsearch.Items[30].Selected == true)
        {
            search[30] = "a.Districtp";
        }
        if (cblsearch.Items[31].Selected == true)
        {
            search[31] = "a.Countryp";
        }
        if (cblsearch.Items[32].Selected == true)
        {
            search[32] = "a.Streetp";
        }
        if (cblsearch.Items[33].Selected == true)
        {
            search[33] = "a.Cityc";
        }
        if (cblsearch.Items[34].Selected == true)
        {
            search[34] = "a.Districtc";
        }
        if (cblsearch.Items[35].Selected == true)
        {
            search[35] = "a.Countryc";
        }
        if (cblsearch.Items[36].Selected == true)
        {
            search[36] = "a.Streetc";
        }
        if (cblsearch.Items[37].Selected == true)
        {
            search[37] = "a.Cityg";
        }
        if (cblsearch.Items[38].Selected == true)
        {
            search[38] = "a.Districtg";
        }
        if (cblsearch.Items[39].Selected == true)
        {
            search[39] = "a.Countryg";
        }
        if (cblsearch.Items[40].Selected == true)
        {
            search[40] = "a.Streetg";
        }
        if (cblsearch.Items[41].Selected == true)
        {
            search[41] = "r.Batch_Year";
        }
        if (cblsearch.Items[42].Selected == true)
        {
            //+ "-" + "(select case department.dept_acronym when '-1' then ' ' else department.dept_acronym end from department,degree,course where course.Course_Id=degree.Course_Id and degree.Degree_Code=r.degree_code and degree.dept_code=department.dept_code)"
            //search[42] = "(select case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course where course.Course_Id=degree.Course_Id and degree.Degree_Code=r.degree_code)";
            search[42] = "(select (course_name + '['+dept_acronym+']') as dpt from degree,course,department where department.dept_code=degree.dept_code and course.Course_Id=degree.Course_Id and degree.Degree_Code=r.degree_code)";
            //+ '-' + "(select department.dept_acronym from degree,department where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=r.degree_code)"; 
        }
        if (cblsearch.Items[43].Selected == true)
        {
            search[43] = "(select department.Dept_Acronym from degree,department where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=r.degree_code)";
        }
        if (cblsearch.Items[44].Selected == true)
        {
            search[44] = "r.current_semester";
        }
        if (cblsearch.Items[45].Selected == true)
        {
            search[45] = "isnull((select refer_name from applyn where direct_refer='3'and app_no=a.app_no),' - ')";
        }
        if (cblsearch.Items[46].Selected == true)
        {
            search[46] = "isnull((select refer_name from applyn where direct_refer='1'and app_no=a.app_no),' - ')";
        }
        if (cblsearch.Items[47].Selected == true)
        {
            search[47] = "isnull(a.refer_agent,' - ')";
        }
        if (cblsearch.Items[48].Selected == true)
        {
            search[48] = "r.stud_type";
        }
        if (cblsearch.Items[49].Selected == true)
        {
            if (laststudiedsel != "")
                search[49] = "s.institute_name";
            else
                search[49] = "(select distinct top 1 institute_name from stud_prev_details s where s.app_no=r.app_no)";
        }
        //state
        if (cblsearch.Items[50].Selected == true)
        {
            search[50] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.parent_statep and textcriteria='state' " + college_code + ")";
        }
        if (cblsearch.Items[51].Selected == true)
        {
            search[51] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.parent_statec and textcriteria='state' " + college_code + ")";
        }
        if (cblsearch.Items[52].Selected == true)
        {
            search[52] = "(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.stateg and textcriteria='state' " + college_code + ")";
        }
        if (cblsearch.Items[53].Selected == true)
        {
            search[53] = "a.remarks";
        }
        if (cblsearch.Items[54].Selected == true)
        {
            search[54] = "(select acr from collinfo where college_code=r.college_code)";
        }
        if (cblsearch.Items[55].Selected == true)
        {
            search[55] = "(select distinct top 1 percentage from stud_prev_details where course_code in(select distinct textcode from  textvaltable where textcode in(select distinct course_code from stud_prev_details where app_no=a.app_no)and textcriteria='cours' and textval like '%SSLC%'  " + college_code + ") and app_no=a.app_no)";
        }
        if (cblsearch.Items[56].Selected == true)
        {
            search[56] = "(select distinct top 1 passyear from stud_prev_details where course_code in(select distinct textcode from  textvaltable where textcode in(select distinct course_code from stud_prev_details where app_no=a.app_no)and textcriteria='cours' and textval like '%SSLC%'  " + college_code + ") and app_no=a.app_no)";
        }
        if (cblsearch.Items[57].Selected == true)
        {
            search[57] = "(select distinct top 1 percentage from stud_prev_details where course_code in(select distinct textcode from  textvaltable where textcode in(select distinct course_code from stud_prev_details where app_no=a.app_no)and textcriteria='cours' and (textval like '%Higher Secondary%' or textval like '% hsc%')  " + college_code + ") and app_no=a.app_no)";
        }
        if (cblsearch.Items[58].Selected == true)
        {
            search[58] = "(select distinct top 1 passyear from stud_prev_details where course_code in(select distinct textcode from  textvaltable where textcode in(select distinct course_code from stud_prev_details where app_no=a.app_no)and textcriteria='cours' and (textval like '%Higher Secondary%' or textval like '% hsc%' )  " + college_code + ") and app_no=a.app_no)";
        }
        if (cblsearch.Items[59].Selected == true)
        {
            search[59] = "(select distinct top 1 percentage from stud_prev_details where course_code in(select distinct textcode from  textvaltable where textcode in(select distinct course_code from stud_prev_details where app_no=a.app_no)and textcriteria='cours' and textval like '%diploma%'  " + college_code + ") and app_no=a.app_no)";
        }
        if (cblsearch.Items[60].Selected == true)
        {
            search[60] = "(select distinct top 1 passyear from stud_prev_details where course_code in(select distinct textcode from  textvaltable where textcode in(select distinct course_code from stud_prev_details where app_no=a.app_no)and textcriteria='cours' and textval like '%diploma%'  " + college_code + ") and app_no=a.app_no)";
        }
        if (cblsearch.Items[61].Selected == true)
        {
            search[61] = "(select distinct top 1 percentage from stud_prev_details where course_code in(select distinct textcode from  textvaltable where textcode in(select distinct course_code from stud_prev_details where app_no=a.app_no)and textcriteria='cours' and textval   like '%ug%'  " + college_code + " ) and app_no=a.app_no)";
        }
        if (cblsearch.Items[62].Selected == true)
        {
            search[62] = "(select distinct top 1 passyear from stud_prev_details where course_code in(select distinct textcode from  textvaltable where textcode in(select distinct course_code from stud_prev_details where app_no=a.app_no)and textcriteria='cours' and textval   like '%ug%'  " + college_code + ") and app_no=a.app_no)";
        }
        //start aruna 19july2013===================================================================================================
        if (cblsearch.Items[63].Selected == true)
        {
            search[63] = "a.parent_addressp";
        }
        if (cblsearch.Items[64].Selected == true)
        {
            search[64] = "a.parent_addressc";
        }
        if (cblsearch.Items[65].Selected == true)
        {
            search[65] = "a.addressg";
        }
        if (cblsearch.Items[66].Selected == true)
        {
            search[66] = "a.parent_pincodep";
        }
        if (cblsearch.Items[67].Selected == true)
        {
            search[67] = "a.parent_pincodec";
        }
        if (cblsearch.Items[68].Selected == true)
        {
            search[68] = "a.ping";
        }
        //=========================================================================================================================
        string wsearch = "";
        int count = 0;
        count = 1;
        btnprintmaster.Visible = true;
        txtexcelname.Visible = true;
        lblrptname.Visible = true;
        btnexcel.Visible = true;
        Printcontrol.Visible = false;
        FpSpread1.Sheets[0].Visible = true;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        FpSpread1.Pager.Align = HorizontalAlign.Right;
        FpSpread1.Pager.Font.Bold = true;
        FpSpread1.Pager.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
        FpSpread1.Pager.ForeColor = Color.DarkGreen;
        FpSpread1.Pager.BackColor = Color.AliceBlue;
        FpSpread1.Pager.PageCount = 5;
        FarPoint.Web.Spread.StyleInfo myStyle = new FarPoint.Web.Spread.StyleInfo();
        myStyle.Font.Name = "Book Antiqua";
        myStyle.Font.Bold = true;
        myStyle.Font.Size = FontUnit.Medium;
        myStyle.ForeColor = Color.Black;
        myStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = myStyle;
        // FpSpread1.Height = 500;
        //   FpSpread1.Width = 1000;
        //     FpSpread1.ColumnHeader.Height = 150;
        if (count == 0)
        {
            //FpSpread1.Sheets.Clear();
            FpSpread1.Sheets[0].ColumnCount = columncount + 3;
            string cmdquery = "select  " + selection + " from applyn a, registration r where a.app_no=r.app_no ";
            //selection = "select r.App_No,a.app_formno,r.Roll_Admit,r.Stud_Name,CONVERT(VARCHAR(10),a.date_applied,103), CONVERT(VARCHAR(10),r.Adm_Date,103), case a.sex when 1 then 'Female' else 'Male' end, CONVERT(VARCHAR(10),a.dob,103) from applyn a, registration r where a.app_no=r.app_no ";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Application No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Admission No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Course Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Seat Type";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Permanent Address";
            for (int icount = 0; icount < ItemList.Count; icount++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, icount + 2].Text = ItemList[icount].ToString();
            }
            FpSpread1.ColumnHeader.Height = 50;
            FarPoint.Web.Spread.NamedStyle instyle = new FarPoint.Web.Spread.NamedStyle();
            FarPoint.Web.Spread.NamedStyle outstyle = new FarPoint.Web.Spread.NamedStyle();
            instyle.BackColor = Color.Yellow;
            outstyle.BackColor = Color.Gray;
            // FarPoint.Web.Spread.FilterColumnDefinition fcd2 = new FarPoint.Web.Spread.FilterColumnDefinition(j + 1);
            FarPoint.Web.Spread.HideRowFilter sf = new FarPoint.Web.Spread.HideRowFilter(FpSpread1.Sheets[0]);
            //FarPoint.Web.Spread.StyleRowFilter sf = new FarPoint.Web.Spread.StyleRowFilter(FpSpread1.Sheets[0], instyle, outstyle);
            FpSpread1.Sheets[0].RowFilter = sf;
            query = cmdquery + sel1 + sel2 + sel3 + sel4 + sel5 + sel6 + sel7 + sel8 + sel9 + sel10 + sel11 + sel12 + sel13 + sel14 + sel15 + sel16 + sel17 + sel18 + sel19 + sel20 + sel21 + sel22 + sel23 + sel24 + sel25 + sel26 + sel27 + sel28 + sel29 + sel30 + sel31 + sel32 + sel33 + sel34 + sel35 + sel36 + sel37 + sel38 + sel39 + sel40 + sel41 + sel42 + sel43 + sel44 + sel45 + sel46 + sel47 + sel48 + sel49 + sel50 + sel51 + sel52 + sel53 + collegesel + laststudiedsel + " " + orderStr;
            cmd.CommandText = query;
            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            i = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[i, j].Text = (i + 1).ToString();
                    FpSpread1.Sheets[0].Cells[i, j].CellType = txtcell;
                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                    //FpSpread1.Cells[i,j].CellType=ctype;
                    for (j = 1; j < columncount + 9; j++)
                    {
                        if (i == 0)
                        {
                            FpSpread1.Sheets[0].RowFilter.AddColumn(j);
                        }
                        //Modified by srinath 8/4/2014
                        if (dr.GetName(j).ToString() == "Districtc")
                        {
                            string distinct = dr.GetValue(j).ToString();
                            int num = 0;
                            if (int.TryParse(distinct, out num))
                            {
                                distinct = da.GetFunction("select textval from textvaltable where TextCriteria='dis' and TextCode='" + distinct + "'");
                                if (distinct.Trim() == "0" || distinct.Trim() == "" || distinct == null)
                                {
                                    distinct = "";
                                }
                            }
                            FpSpread1.Sheets[0].Cells[i, j].Text = distinct;
                        }
                        else if (dr.GetName(j).ToString() == "Districtp")
                        {
                            string distinct = dr.GetValue(j).ToString();
                            int num = 0;
                            if (int.TryParse(distinct, out num))
                            {
                                distinct = da.GetFunction("select textval from textvaltable where TextCriteria='dis' and TextCode='" + distinct + "'");
                                if (distinct.Trim() == "0" || distinct.Trim() == "" || distinct == null)
                                {
                                    distinct = "";
                                }
                            }
                            FpSpread1.Sheets[0].Cells[i, j].Text = distinct;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[i, j].Text = dr.GetValue(j).ToString();
                        }
                        FpSpread1.Sheets[0].Cells[i, 1].Tag = dr.GetValue(0).ToString();
                        FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                    }
                    i = i + 1;
                    j = 0;
                }
            }
            else
            {
                FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
            }
            dr.Close();
            con.Close();
            CalculateTotalPages();
            FpSpread1.SaveChanges();
        }
        else
        {
            wsearch = " r.App_No";
            int col = 0;
            for (int itemcount = 0; itemcount < Itemindex.Count; itemcount++)
            {
                int s = -1;
                if (int.TryParse(Itemindex[itemcount].ToString(), out s))
                    s = Convert.ToInt32(Itemindex[itemcount].ToString());
                if (search[s] != "" && s != -1)
                {
                    wsearch = wsearch + "," + search[s];
                    col = col + 1;
                }
            }
            //FarPoint.Web.Spread.GeneralCellType ctype = new FarPoint.Web.Spread.GeneralCellType();
            //ctype.CssClass = "stylefp";
            //FpSpread1.Sheets[0].Rows[0].CellType = ctype;
            FarPoint.Web.Spread.NamedStyle instyle = new FarPoint.Web.Spread.NamedStyle();
            FarPoint.Web.Spread.NamedStyle outstyle = new FarPoint.Web.Spread.NamedStyle();
            instyle.BackColor = Color.Yellow;
            outstyle.BackColor = Color.Gray;
            // FarPoint.Web.Spread.FilterColumnDefinition fcd2 = new FarPoint.Web.Spread.FilterColumnDefinition(j + 1);
            // FarPoint.Web.Spread.StyleRowFilter sf = new FarPoint.Web.Spread.StyleRowFilter(FpSpread1.Sheets[0], instyle, outstyle);
            FarPoint.Web.Spread.HideRowFilter sf = new FarPoint.Web.Spread.HideRowFilter(FpSpread1.Sheets[0]);
            FpSpread1.Sheets[0].RowFilter = sf;
            FpSpread1.Sheets[0].ColumnCount = Itemindex.Count + 1;
            FpSpread1.ColumnHeader.Height = 50;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            //FpSpread1.ColumnHeader.Cells[0,0].CellType = ctype;
            FpSpread1.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
            FpSpread1.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
            FpSpread1.ActiveSheetView.Columns[0].Font.Bold = true;
            string con_sec_value = ""; // added by jairam 06-11-2014
            if (cblsection.Items.Count > 0)
            {
                for (int sec = 0; sec < cblsection.Items.Count; sec++)
                {
                    if (cblsection.Items[sec].Selected == true)
                    {
                        string sec_value = cblsection.Items[sec].Value.ToString();
                        if (con_sec_value == "")
                            con_sec_value = sec_value;
                        else
                            con_sec_value = con_sec_value + "','" + sec_value;
                    }
                }
            }
            if (laststudiedsel != "")
                selection = "select " + wsearch + " from applyn a, registration r,stud_prev_details s where a.app_no=r.app_no and s.app_no=r.app_no  ";
            else
                selection = "select " + wsearch + " from applyn a, registration r where a.app_no=r.app_no ";
            if (con_sec_value != "") // added by jairam 06-11-2014
            {
                selection = selection + "and r.Sections in ('" + con_sec_value + "')";
            }
            // header and order 
            tborder.Text = "";
            for (i = 0; i < Itemindex.Count; i++)
            {
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, i + 1].Text = ItemList[i].ToString();
                string t = Itemindex[i].ToString();
                int te = Convert.ToInt32(t);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, i + 1].Text = cblsearch.Items[te].Text.ToString();
                FpSpread1.Sheets[0].Columns[i + 1].CellType = txtcell;
                tborder.Text = tborder.Text + cblsearch.Items[te].Text.ToString();
                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
                FpSpread1.ActiveSheetView.Columns[i + 1].Font.Name = "Book Antiqua";
                FpSpread1.ActiveSheetView.Columns[i + 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].RowFilter.AddColumn(i + 1);
            } tborder.Visible = true;
            collegecode_selection = " and r.college_code in(" + college + ")";
            query = selection + sel1 + sel2 + sel3 + sel4 + sel5 + sel6 + sel7 + sel8 + sel9 + sel10 + sel11 + sel12 + sel13 + sel14 + sel15 + sel16 + sel17 + sel18 + sel19 + sel20 + sel21 + sel22 + sel23 + sel24 + sel25 + sel26 + sel27 + sel28 + sel29 + sel30 + sel31 + sel32 + sel33 + sel34 + sel35 + sel36 + sel37 + sel38 + sel39 + sel40 + sel41 + sel42 + sel43 + sel44 + sel45 + sel46 + sel47 + sel48 + sel49 + sel50 + sel51 + sel52 + sel53 + hosdaysel + laststudiedsel + sel_c + sel_g + sel_p + collegecode_selection + " " + orderStr;
            cmd.CommandText = query;
            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            i = 0;
            if (dr.HasRows)
                while (dr.Read())
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[i, j].Text = (i + 1).ToString();
                    FpSpread1.Sheets[0].Cells[i, j].CellType = txtcell;
                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                    //FpSpread1.Cells[i, j].CellType = ctype;
                    for (j = 1; j <= ItemList.Count; j++)
                    {
                        FpSpread1.Sheets[0].Cells[i, 1].Tag = dr.GetValue(0).ToString();
                        //Modified by srinath 8/4/2014
                        if (dr.GetName(j).ToString() == "Districtc")
                        {
                            string distinct = dr.GetValue(j).ToString();
                            int num = 0;
                            if (int.TryParse(distinct, out num))
                            {
                                distinct = da.GetFunction("select textval from textvaltable where TextCriteria='dis' and TextCode='" + distinct + "'");
                                if (distinct.Trim() == "0" || distinct.Trim() == "" || distinct == null)
                                {
                                    distinct = "";
                                }
                            }
                            FpSpread1.Sheets[0].Cells[i, j].Text = distinct;
                        }
                        else if (dr.GetName(j).ToString() == "Districtp")
                        {
                            //Added By Srianth
                            string distinct = dr.GetValue(j).ToString();
                            int num = 0;
                            if (int.TryParse(distinct, out num))
                            {
                                distinct = da.GetFunction("select textval from textvaltable where TextCriteria='dis' and TextCode='" + distinct + "'");
                                if (distinct.Trim() == "0" || distinct.Trim() == "" || distinct == null)
                                {
                                    distinct = "";
                                }
                            }
                            FpSpread1.Sheets[0].Cells[i, j].Text = distinct;
                        }
                        else if (dr.GetName(j).ToString().ToLower() == "cityc" || dr.GetName(j).ToString().ToLower() == "cityp" || dr.GetName(j).ToString().ToLower() == "cityg")
                        {
                            //Added By Srianth
                            string distinct = dr.GetValue(j).ToString();
                            int num = 0;
                            if (int.TryParse(distinct, out num))
                            {
                                distinct = da.GetFunction("select textval from textvaltable where TextCriteria='city' and TextCode='" + distinct + "'");
                                if (distinct.Trim() == "0" || distinct.Trim() == "" || distinct == null)
                                {
                                    distinct = "";
                                }
                            }
                            FpSpread1.Sheets[0].Cells[i, j].Text = distinct;
                        }
                        else if (dr.GetName(j).ToString().ToLower() == "countryg" || dr.GetName(j).ToString().ToLower() == "countryc" || dr.GetName(j).ToString().ToLower() == "countryp")
                        {
                            //Added By Srianth
                            string distinct = dr.GetValue(j).ToString();
                            int num = 0;
                            if (int.TryParse(distinct, out num))
                            {
                                distinct = da.GetFunction("select textval from textvaltable where TextCriteria='coun' and TextCode='" + distinct + "'");
                                if (distinct.Trim() == "0" || distinct.Trim() == "" || distinct == null)
                                {
                                    distinct = "";
                                }
                            }
                            FpSpread1.Sheets[0].Cells[i, j].Text = distinct;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[i, j].Text = dr.GetValue(j).ToString();
                        }
                        FpSpread1.Sheets[0].Cells[i, j].Locked = true;
                        //FpSpread1.Cells[i, j + 1].CellType = ctype;
                        FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                    }
                    i = i + 1;
                    j = 0;
                }
            dr.Close();
            con.Close();
            CalculateTotalPages();
        }
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "   Pages : " + Session["totalPages"];
        DropDownListpage.Items.Clear();
        if (totalRows >= 10)
        {
            FpSpread1.Sheets[0].PageSize = 10;
            for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
            {
                DropDownListpage.Items.Add((k + 10).ToString());
            }
            DropDownListpage.Items.Add("Others");
        }
        else if (totalRows == 0)
        {
            DropDownListpage.Items.Add("0");
        }
        else
        {
            FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
        }
        Buttontotal.Visible = true;
        DropDownListpage.Visible = true;
        lblpage.Visible = true;
        TextBoxpage.Visible = true;
        if (FpSpread1.Sheets[0].RowCount == 0)
        {
            lblnorec.Visible = true;
            lblnorec.Text = "There are no records matched";
            FpSpread1.Visible = false;
            Buttontotal.Visible = false;
            LabelE.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
            Panelpage.Visible = false;
            //Added By Srinath 7/5/2013
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnexcel.Visible = false;
            Printcontrol.Visible = false;
        }
        else
        {
            lblnorec.Visible = false;
            FpSpread1.Visible = true;
            Panelpage.Visible = true;
            Buttontotal.Visible = true;
            DropDownListpage.Visible = true;
            lblpage.Visible = true;
            TextBoxpage.Visible = true;
            //Added By Srinath 7/5/2013
            TextBoxother.Visible = false;
            btnprintmaster.Visible = true;
            txtexcelname.Visible = true;
            lblrptname.Visible = true;
            btnexcel.Visible = true;
            Printcontrol.Visible = false;
        }
        FpSpread1.Sheets[0].AutoPostBack = true;
        loadorder();
        double FpSpread1Width = 0;
        double columnCount = 0;
        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].ColumnCount), out columnCount);
        FpSpread1Width = 90 * columnCount;
        if (FpSpread1Width < 700)
            FpSpread1.Width = 700;
        else
            FpSpread1.Width = Convert.ToInt32(FpSpread1Width);
    }
    protected void cblseattype_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pseattype.Focus();
        //cblseattype.Focus();
        int seatcount = 0;
        string value = "";
        string code = "";
        LinkButtonseattype.Visible = true;
        for (int i = 0; i < cblseattype.Items.Count; i++)
        {
            if (cblseattype.Items[i].Selected == true)
            {
                value = cblseattype.Items[i].Text;
                code = cblseattype.Items[i].Value.ToString();
                seatcount = seatcount + 1;
                tbseattype.Text = "Seattype(" + seatcount.ToString() + ")";
            }
        }
        if (seatcount == 0)
            tbseattype.Text = "---Select---";
        else
        {
            Label lbl = seatlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = seatimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(seatimg_Click);
        }
        seatcnt = seatcount;
    }
    //protected void cblseattype_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //}
    public ImageButton seatimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderseattype.Controls.Add(imc);
        ViewState["iseatcontrol"] = true;
        return (imc);
    }
    public Label seatlabel()
    {
        Label lbc = new Label();
        PlaceHolderseattype.Controls.Add(lbc);
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }
    public void seatimg_Click(object sender, ImageClickEventArgs e)
    {
        seatcnt = seatcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblseattype.Items[r].Selected = false;
        tbseattype.Text = "Seattype(" + seatcnt.ToString() + ")";
        if (tbseattype.Text == "Seattype(0)")
        {
            tbseattype.Text = "---Select---";
            LinkButtonseattype.Visible = false;
        }
        int p = PlaceHolderseattype.Controls.IndexOf(b);
        PlaceHolderseattype.Controls.RemoveAt(p - 1);
        PlaceHolderseattype.Controls.Remove(b);
    }
    protected void cblblood_SelectedIndexChanged(object sender, EventArgs e)
    {
        int bloodcount = 0;
        string value = "";
        string code = "";
        LinkButtonblood.Visible = true;
        for (int i = 0; i < cblblood.Items.Count; i++)
        {
            if (cblblood.Items[i].Selected == true)
            {
                value = cblblood.Items[i].Text;
                code = cblblood.Items[i].Value.ToString();
                bloodcount = bloodcount + 1;
                tbblood.Text = "Blood Group(" + bloodcount.ToString() + ")";
            }
        }
        if (bloodcount == 0)
        {
            tbblood.Text = "---Select---";
        }
        else
        {
            Label lbl = bloodlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl2-" + code.ToString();
            ImageButton ib = bloodimage();
            ib.ID = "imgbut2_" + code.ToString();
            ib.Click += new ImageClickEventHandler(bloodimg_Click);
        }
        bloodcnt = bloodcount;
    }
    public ImageButton bloodimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderblood.Controls.Add(imc);
        ViewState["ibloodcontrol"] = true;
        return (imc);
    }
    public Label bloodlabel()
    {
        Label lbc = new Label();
        PlaceHolderblood.Controls.Add(lbc);
        ViewState["lbloodcontrol"] = true;
        return (lbc);
    }
    public void bloodimg_Click(object sender, ImageClickEventArgs e)
    {
        bloodcnt = bloodcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblblood.Items[r].Selected = false;
        tbblood.Text = "Blood Group(" + bloodcnt.ToString() + ")";
        if (tbblood.Text == "Blood Group(0)")
        {
            LinkButtonblood.Visible = false;
            tbblood.Text = "---Select---";
        }
        int p = PlaceHolderblood.Controls.IndexOf(b);
        PlaceHolderblood.Controls.RemoveAt(p - 1);
        PlaceHolderblood.Controls.Remove(b);
    }
    public ImageButton casteimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHoldercaste.Controls.Add(imc);
        ViewState["icastecontrol"] = true;
        return (imc);
    }
    public Label castelabel()
    {
        Label lbc = new Label();
        PlaceHoldercaste.Controls.Add(lbc);
        ViewState["lcastecontrol"] = true;
        return (lbc);
    }
    public void casteimg_Click(object sender, ImageClickEventArgs e)
    {
        castecnt = castecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblcaste.Items[r].Selected = false;
        tbcaste.Text = "caste(" + castecnt.ToString() + ")";
        if (tbcaste.Text == "caste(0)")
        {
            LinkButtoncaste.Visible = false;
            tbcaste.Text = "---Select---";
        }
        int p = PlaceHoldercaste.Controls.IndexOf(b);
        PlaceHoldercaste.Controls.RemoveAt(p - 1);
        PlaceHoldercaste.Controls.Remove(b);
    }
    protected void cblcaste_SelectedIndexChanged(object sender, EventArgs e)
    {
        int castecount = 0;
        string value = "";
        string code = "";
        LinkButtoncaste.Visible = true;
        for (int i = 0; i < cblcaste.Items.Count; i++)
        {
            if (cblcaste.Items[i].Selected == true)
            {
                value = cblcaste.Items[i].Text;
                code = cblcaste.Items[i].Value.ToString();
                castecount = castecount + 1;
                tbcaste.Text = "caste(" + castecount.ToString() + ")";
            }
        }
        if (castecount == 0)
            tbcaste.Text = "---Select---";
        else
        {
            Label lbl = castelabel();
            lbl.Text = " " + value + " ";
            //lbl.ID = "lbl3-" + code.ToString();
            lbl.ID = "lbl3-" + code.ToString();
            ImageButton ib = casteimage();
            ib.ID = "imgbut3_" + code.ToString();
            ib.Click += new ImageClickEventHandler(casteimg_Click);
        }
        castecnt = castecount;
    }
    protected void cblreligion_SelectedIndexChanged(object sender, EventArgs e)
    {
        int religcount = 0;
        string value = "";
        string code = "";
        LinkButtonreligion.Visible = true;
        for (int i = 0; i < cblreligion.Items.Count; i++)
        {
            if (cblreligion.Items[i].Selected == true)
            {
                value = cblreligion.Items[i].Text;
                code = cblreligion.Items[i].Value.ToString();
                religcount = religcount + 1;
                tbreligion.Text = "Religion(" + religcount.ToString() + ")";
            }
        }
        if (religcount == 0)
            tbreligion.Text = "---Select---";
        else
        {
            Label lbl = religlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl4-" + code.ToString();
            ImageButton ib = religimage();
            ib.ID = "imgbut4_" + code.ToString();
            ib.Click += new ImageClickEventHandler(religimg_Click);
        }
        religcnt = religcount;
    }
    public ImageButton religimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderreligion.Controls.Add(imc);
        ViewState["ireligcontrol"] = true;
        return (imc);
    }
    public Label religlabel()
    {
        Label lbc = new Label();
        PlaceHolderreligion.Controls.Add(lbc);
        ViewState["lreligcontrol"] = true;
        return (lbc);
    }
    public void religimg_Click(object sender, ImageClickEventArgs e)
    {
        religcnt = religcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblreligion.Items[r].Selected = false;
        tbreligion.Text = "Religion(" + religcnt.ToString() + ")";
        if (tbreligion.Text == "Religion(0)")
        {
            LinkButtonreligion.Visible = false;
            tbreligion.Text = "---Select---";
        }
        int p = PlaceHolderreligion.Controls.IndexOf(b);
        PlaceHolderreligion.Controls.RemoveAt(p - 1);
        PlaceHolderreligion.Controls.Remove(b);
    }
    protected void cblcomm_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        string value = "";
        string code = "";
        LinkButtoncomm.Visible = true;
        for (int i = 0; i < cblcomm.Items.Count; i++)
        {
            if (cblcomm.Items[i].Selected == true)
            {
                value = cblcomm.Items[i].Text;
                code = cblcomm.Items[i].Value.ToString();
                commcount = commcount + 1;
                tbcomm.Text = "Community(" + commcount.ToString() + ")";
            }
        }
        if (commcount == 0)
            tbcomm.Text = "---Select---";
        else
        {
            Label lbl = commlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl5-" + code.ToString();
            ImageButton ib = commimage();
            ib.ID = "imgbut5_" + code.ToString();
            ib.Click += new ImageClickEventHandler(commimg_Click);
        }
        commcnt = commcount;
    }
    public ImageButton commimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHoldercomm.Controls.Add(imc);
        ViewState["icommcontrol"] = true;
        return (imc);
    }
    public Label commlabel()
    {
        Label lbc = new Label();
        PlaceHoldercomm.Controls.Add(lbc);
        ViewState["lcommcontrol"] = true;
        return (lbc);
    }
    public void commimg_Click(object sender, ImageClickEventArgs e)
    {
        commcnt = commcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblcomm.Items[r].Selected = false;
        tbcomm.Text = "Community(" + commcnt.ToString() + ")";
        if (tbcomm.Text == "Community(0)")
        {
            LinkButtoncomm.Visible = false;
            tbcomm.Text = "---Select---";
        }
        int p = PlaceHoldercomm.Controls.IndexOf(b);
        PlaceHoldercomm.Controls.RemoveAt(p - 1);
        PlaceHoldercomm.Controls.Remove(b);
    }
    protected void cblregion_SelectedIndexChanged(object sender, EventArgs e)
    {
        int regioncount = 0;
        string value = "";
        LinkButtonregion.Visible = true;
        string code = "";
        for (int i = 0; i < cblregion.Items.Count; i++)
        {
            if (cblregion.Items[i].Selected == true)
            {
                value = cblregion.Items[i].Text;
                code = cblregion.Items[i].Value.ToString();
                regioncount = regioncount + 1;
                tbregion.Text = "Region(" + regioncount.ToString() + ")";
            }
        }
        if (regioncount == 0)
            tbregion.Text = "---Select---";
        else
        {
            Label lbl = regionlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl6-" + code.ToString();
            ImageButton ib = regionimage();
            ib.ID = "imgbut6_" + code.ToString();
            ib.Click += new ImageClickEventHandler(regionimg_Click);
        }
        regioncnt = regioncount;
    }
    public ImageButton regionimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderregion.Controls.Add(imc);
        ViewState["iregioncontrol"] = true;
        return (imc);
    }
    public Label regionlabel()
    {
        Label lbc = new Label();
        PlaceHolderregion.Controls.Add(lbc);
        ViewState["lregioncontrol"] = true;
        return (lbc);
    }
    public void regionimg_Click(object sender, ImageClickEventArgs e)
    {
        regioncnt = regioncnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblregion.Items[r].Selected = false;
        tbregion.Text = "Region(" + regioncnt.ToString() + ")";
        if (tbregion.Text == "Region(0)")
        {
            LinkButtonregion.Visible = false;
            tbregion.Text = "---Select---";
        }
        int p = PlaceHolderregion.Controls.IndexOf(b);
        PlaceHolderregion.Controls.RemoveAt(p - 1);
        PlaceHolderregion.Controls.Remove(b);
    }
    protected void cblmtongue_SelectedIndexChanged(object sender, EventArgs e)
    {
        int mtonguecount = 0;
        string value = "";
        string code = "";
        LinkButtonmtongue.Visible = true;
        for (int i = 0; i < cblmtongue.Items.Count; i++)
        {
            if (cblmtongue.Items[i].Selected == true)
            {
                value = cblmtongue.Items[i].Text;
                code = cblmtongue.Items[i].Value.ToString();
                mtonguecount = mtonguecount + 1;
                tbmtongue.Text = "Mother Tongue(" + mtonguecount.ToString() + ")";
            }
        }
        if (mtonguecount == 0)
            tbmtongue.Text = "---Select---";
        else
        {
            Label lbl = mtonguelabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl7-" + code.ToString();
            ImageButton ib = mtongueimage();
            ib.ID = "imgbut7_" + code.ToString();
            ib.Click += new ImageClickEventHandler(mtongueimg_Click);
        }
        mtonguecnt = mtonguecount;
    }
    public ImageButton mtongueimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHoldermtongue.Controls.Add(imc);
        ViewState["imtonguecontrol"] = true;
        return (imc);
    }
    public Label mtonguelabel()
    {
        Label lbc = new Label();
        PlaceHoldermtongue.Controls.Add(lbc);
        ViewState["lmtonguecontrol"] = true;
        return (lbc);
    }
    public void mtongueimg_Click(object sender, ImageClickEventArgs e)
    {
        mtonguecnt = mtonguecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblmtongue.Items[r].Selected = false;
        tbmtongue.Text = "Mother Tongue(" + mtonguecnt.ToString() + ")";
        if (tbmtongue.Text == "Mother Tongue(0)")
        {
            LinkButtonmtongue.Visible = false;
            tbmtongue.Text = "---Select---";
        }
        int p = PlaceHoldermtongue.Controls.IndexOf(b);
        PlaceHoldermtongue.Controls.RemoveAt(p - 1);
        PlaceHoldermtongue.Controls.Remove(b);
    }
    protected void cblfoccu_SelectedIndexChanged(object sender, EventArgs e)
    {
        int foccucount = 0;
        string value = "";
        LinkButtonfoccu.Visible = true;
        string code = "";
        for (int i = 0; i < cblfoccu.Items.Count; i++)
        {
            if (cblfoccu.Items[i].Selected == true)
            {
                value = cblfoccu.Items[i].Text;
                code = cblfoccu.Items[i].Value.ToString();
                foccucount = foccucount + 1;
                tbfoccu.Text = "Father Occupation(" + foccucount.ToString() + ")";
            }
        }
        if (foccucount == 0)
            tbfoccu.Text = "---Select---";
        else
        {
            Label lbl = focculabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl8-" + code.ToString();
            ImageButton ib = foccuimage();
            ib.ID = "imgbut8_" + code.ToString();
            ib.Click += new ImageClickEventHandler(foccuimg_Click);
        }
        foccucnt = foccucount;
    }
    public ImageButton foccuimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderfoccu.Controls.Add(imc);
        ViewState["ifoccucontrol"] = true;
        return (imc);
    }
    public Label focculabel()
    {
        Label lbc = new Label();
        PlaceHolderfoccu.Controls.Add(lbc);
        ViewState["lfoccucontrol"] = true;
        return (lbc);
    }
    public void foccuimg_Click(object sender, ImageClickEventArgs e)
    {
        foccucnt = foccucnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblfoccu.Items[r].Selected = false;
        tbfoccu.Text = "Father Occupation(" + foccucnt.ToString() + ")";
        if (tbfoccu.Text == "Father Occupation(0)")
        {
            LinkButtonfoccu.Visible = false;
            tbfoccu.Text = "---Select---";
        }
        int p = PlaceHolderfoccu.Controls.IndexOf(b);
        PlaceHolderfoccu.Controls.RemoveAt(p - 1);
        PlaceHolderfoccu.Controls.Remove(b);
    }
    protected void cblfqual_SelectedIndexChanged(object sender, EventArgs e)
    {
        int fqualcount = 0;
        string value = "";
        LinkButtonfqual.Visible = true;
        string code = "";
        for (int i = 0; i < cblfqual.Items.Count; i++)
        {
            if (cblfqual.Items[i].Selected == true)
            {
                value = cblfqual.Items[i].Text;
                code = cblfqual.Items[i].Value.ToString();
                fqualcount = fqualcount + 1;
                tbfqual.Text = "Father Qualification(" + fqualcount.ToString() + ")";
            }
        }
        if (fqualcount == 0)
            tbfqual.Text = "---Select---";
        else
        {
            Label lbl = fquallabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl9-" + code.ToString();
            ImageButton ib = fqualimage();
            ib.ID = "imgbut9_" + code.ToString();
            ib.Click += new ImageClickEventHandler(fqualimg_Click);
        }
        fqualcnt = fqualcount;
    }
    public ImageButton fqualimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderfqual.Controls.Add(imc);
        ViewState["ifqualcontrol"] = true;
        return (imc);
    }
    public Label fquallabel()
    {
        Label lbc = new Label();
        PlaceHolderfqual.Controls.Add(lbc);
        ViewState["lfqualcontrol"] = true;
        return (lbc);
    }
    public void fqualimg_Click(object sender, ImageClickEventArgs e)
    {
        fqualcnt = fqualcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblfqual.Items[r].Selected = false;
        tbfqual.Text = "Father Qualification(" + fqualcnt.ToString() + ")";
        if (tbfqual.Text == "Father Qualification(0)")
        {
            LinkButtonfqual.Visible = false;
            tbfqual.Text = "---Select---";
        }
        int p = PlaceHolderfqual.Controls.IndexOf(b);
        PlaceHolderfqual.Controls.RemoveAt(p - 1);
        PlaceHolderfqual.Controls.Remove(b);
    }
    protected void cblmoccu_SelectedIndexChanged(object sender, EventArgs e)
    {
        int moccucount = 0;
        string value = "";
        string code = "";
        LinkButtonmoccu.Visible = true;
        for (int i = 0; i < cblmoccu.Items.Count; i++)
        {
            if (cblmoccu.Items[i].Selected == true)
            {
                value = cblmoccu.Items[i].Text;
                code = cblmoccu.Items[i].Value.ToString();
                moccucount = moccucount + 1;
                tbmoccu.Text = "Mother Occupation(" + moccucount.ToString() + ")";
            }
        }
        if (moccucount == 0)
            tbmoccu.Text = "---Select---";
        else
        {
            Label lbl = mocculabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl10-" + code.ToString();
            ImageButton ib = moccuimage();
            ib.ID = "imgbut10_" + code.ToString();
            ib.Click += new ImageClickEventHandler(moccuimg_Click);
        }
        moccucnt = moccucount;
    }
    public ImageButton moccuimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHoldermoccu.Controls.Add(imc);
        ViewState["imoccucontrol"] = true;
        return (imc);
    }
    public Label mocculabel()
    {
        Label lbc = new Label();
        PlaceHoldermoccu.Controls.Add(lbc);
        ViewState["lmoccucontrol"] = true;
        return (lbc);
    }
    public void moccuimg_Click(object sender, ImageClickEventArgs e)
    {
        moccucnt = moccucnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblmoccu.Items[r].Selected = false;
        tbmoccu.Text = "Mother Occupation(" + moccucnt.ToString() + ")";
        if (tbmoccu.Text == "Mother Occupation(0)")
        {
            LinkButtonmoccu.Visible = false;
            tbmoccu.Text = "---Select---";
        }
        int p = PlaceHoldermoccu.Controls.IndexOf(b);
        PlaceHoldermoccu.Controls.RemoveAt(p - 1);
        PlaceHoldermoccu.Controls.Remove(b);
    }
    protected void cblmqual_SelectedIndexChanged(object sender, EventArgs e)
    {
        string value = "";
        string code = "";
        int mqualcount = 0;
        LinkButtonmqual.Visible = true;
        for (int i = 0; i < cblmqual.Items.Count; i++)
        {
            if (cblmqual.Items[i].Selected == true)
            {
                value = cblmqual.Items[i].Text;
                code = cblmqual.Items[i].Value.ToString();
                mqualcount = mqualcount + 1;
                tbmqual.Text = "Mother Qualification(" + mqualcount.ToString() + ")";
            }
        }
        if (mqualcount == 0)
            tbmqual.Text = "---Select---";
        else
        {
            Label lbl = mquallabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl11-" + code.ToString();
            ImageButton ib = mqualimage();
            ib.ID = "imgbut11_" + code.ToString();
            ib.Click += new ImageClickEventHandler(mqualimg_Click);
        }
        mqualcnt = mqualcount;
    }
    public ImageButton mqualimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHoldermqual.Controls.Add(imc);
        ViewState["imqualcontrol"] = true;
        return (imc);
    }
    public Label mquallabel()
    {
        Label lbc = new Label();
        PlaceHoldermqual.Controls.Add(lbc);
        ViewState["lmqualcontrol"] = true;
        return (lbc);
    }
    public void mqualimg_Click(object sender, ImageClickEventArgs e)
    {
        mqualcnt = mqualcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblmqual.Items[r].Selected = false;
        tbmqual.Text = "Mother Qualification(" + mqualcnt.ToString() + ")";
        if (tbmqual.Text == "Mother Qualification(0)")
        {
            LinkButtonmqual.Visible = false;
            tbmqual.Text = "---Select---";
        }
        int p = PlaceHoldermqual.Controls.IndexOf(b);
        PlaceHoldermqual.Controls.RemoveAt(p - 1);
        PlaceHoldermqual.Controls.Remove(b);
    }
    protected void cbldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        int degreecount = 0;
        string value = "";
        string code = "";
        LinkButtondegree.Visible = true;
        for (int i = 0; i < cbldegree.Items.Count; i++)
        {
            if (cbldegree.Items[i].Selected == true)
            {
                value = cbldegree.Items[i].Text;
                code = cbldegree.Items[i].Value.ToString();
                degreecount = degreecount + 1;
                tbdegree.Text = lbl_stuDegree.Text + "(" + degreecount.ToString() + ")";
            }
        }
        if (degreecount == 0)
            tbdegree.Text = "---Select---";
        else
        {
            Label lbl = degreelabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl12-" + code.ToString();
            ImageButton ib = degreeimage();
            ib.ID = "imgbut12_" + code.ToString();
            ib.Click += new ImageClickEventHandler(degreeimg_Click);
        }
        degreecnt = degreecount;
    }
    public ImageButton degreeimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderdegree.Controls.Add(imc);
        ViewState["idegreecontrol"] = true;
        return (imc);
    }
    public Label degreelabel()
    {
        Label lbc = new Label();
        PlaceHolderdegree.Controls.Add(lbc);
        ViewState["ldegreecontrol"] = true;
        return (lbc);
    }
    public void degreeimg_Click(object sender, ImageClickEventArgs e)
    {
        degreecnt = degreecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbldegree.Items[r].Selected = false;
        tbdegree.Text = lbl_stuDegree.Text + "(" + degreecnt.ToString() + ")";
        if (tbdegree.Text == lbl_stuDegree.Text + "(0)")
        {
            LinkButtondegree.Visible = false;
            tbdegree.Text = "---Select---";
        }
        int p = PlaceHolderdegree.Controls.IndexOf(b);
        PlaceHolderdegree.Controls.RemoveAt(p - 1);
        PlaceHolderdegree.Controls.Remove(b);
    }
    //
    protected void cblsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        int semcount = 0;
        string value = "";
        string code = "";
        LinkButtonsemester.Visible = true;
        for (int i = 0; i < cblsem.Items.Count; i++)
        {
            if (cblsem.Items[i].Selected == true)
            {
                value = cblsem.Items[i].Text;
                code = cblsem.Items[i].Value.ToString();
                semcount = semcount + 1;
                tbsem.Text = lbl_stuSemOrT.Text + "(" + semcount.ToString() + ")";
            }
        }
        if (semcount == 0)
            tbsem.Text = "---Select---";
        else
        {
            Label lbl = semlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl14-" + code.ToString();
            ImageButton ib = semimage();
            ib.ID = "imgbut14_" + code.ToString();
            ib.Click += new ImageClickEventHandler(semimg_Click);
        }
        semcnt = semcount;
    }
    public ImageButton semimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHoldersemester.Controls.Add(imc);
        ViewState["isemcontrol"] = true;
        return (imc);
    }
    public Label semlabel()
    {
        Label lbc = new Label();
        PlaceHoldersemester.Controls.Add(lbc);
        ViewState["lsemcontrol"] = true;
        return (lbc);
    }
    public void semimg_Click(object sender, ImageClickEventArgs e)
    {
        semcnt = semcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblsem.Items[r].Selected = false;
        tbsem.Text = lbl_stuSemOrT.Text + "(" + semcnt.ToString() + ")";
        if (tbsem.Text == lbl_stuSemOrT.Text + "(0)")
        {
            LinkButtonsemester.Visible = false;
            tbsem.Text = "---Select---";
        }
        int p = PlaceHoldersemester.Controls.IndexOf(b);
        PlaceHoldersemester.Controls.RemoveAt(p - 1);
        PlaceHoldersemester.Controls.Remove(b);
    }
    //
    //
    protected void cblbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int branchcount = 0;
        string value = "";
        string code = "";
        LinkButtonbranch.Visible = true;
        for (int i = 0; i < cblbranch.Items.Count; i++)
        {
            if (cblbranch.Items[i].Selected == true)
            {
                value = cblbranch.Items[i].Text;
                code = cblbranch.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                tbbranch.Text = "Branch(" + branchcount.ToString() + ")";
            }
        }
        if (branchcount == 0)
            tbbranch.Text = "---Select---";
        else
        {
            Label lbl = branchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl13-" + code.ToString();
            ImageButton ib = branchimage();
            ib.ID = "imgbut13_" + code.ToString();
            ib.Click += new ImageClickEventHandler(branchimg_Click);
        }
        branchcnt = branchcount;
    }
    public ImageButton branchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderbranch.Controls.Add(imc);
        ViewState["ibranchcontrol"] = true;
        return (imc);
    }
    public Label branchlabel()
    {
        Label lbc = new Label();
        PlaceHolderbranch.Controls.Add(lbc);
        ViewState["lbranchcontrol"] = true;
        return (lbc);
    }
    public void branchimg_Click(object sender, ImageClickEventArgs e)
    {
        branchcnt = branchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblbranch.Items[r].Selected = false;
        tbbranch.Text = "Branch(" + branchcnt.ToString() + ")";
        if (tbbranch.Text == "Branch(0)")
        {
            LinkButtonbranch.Visible = false;
            tbbranch.Text = "---Select---";
        }
        int p = PlaceHolderbranch.Controls.IndexOf(b);
        PlaceHolderbranch.Controls.RemoveAt(p - 1);
        PlaceHolderbranch.Controls.Remove(b);
    }
    protected void cblsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        int seccount = 0;
        string value = "";
        string code = "";
        LinkButtonsection.Visible = true;
        for (int i = 0; i < cblsection.Items.Count; i++)
        {
            if (cblsection.Items[i].Selected == true)
            {
                value = cblsection.Items[i].Text;
                code = cblsection.Items[i].Value.ToString();
                seccount = seccount + 1;
                tbsection.Text = "sec(" + seccount.ToString() + ")";
            }
        }
        if (seccount == 0)
            tbsection.Text = "---Select---";
        else
        {
            Label lbl = seclabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl15-" + code.ToString();
            ImageButton ib = secimage();
            ib.ID = "imgbut15_" + code.ToString();
            ib.Click += new ImageClickEventHandler(secimg_Click);
        }
        seccnt = seccount;
    }
    public ImageButton secimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHoldersection.Controls.Add(imc);
        ViewState["iseccontrol"] = true;
        return (imc);
    }
    public Label seclabel()
    {
        Label lbc = new Label();
        PlaceHoldersection.Controls.Add(lbc);
        ViewState["lseccontrol"] = true;
        return (lbc);
    }
    public void secimg_Click(object sender, ImageClickEventArgs e)
    {
        seccnt = seccnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblsection.Items[r].Selected = false;
        tbsection.Text = "sec(" + seccnt.ToString() + ")";
        if (tbsection.Text == "sec(0)")
        {
            LinkButtonsection.Visible = false;
            tbsection.Text = "---Select---";
        }
        int p = PlaceHoldersection.Controls.IndexOf(b);
        PlaceHoldersection.Controls.RemoveAt(p - 1);
        PlaceHoldersection.Controls.Remove(b);
    }
    //        protected void Button4_Click(object sender, EventArgs e)
    //{
    //    Panel tt = new Panel();
    //    TextBox tb = new TextBox();
    //    tb.ID = "txtName";
    //    tb.Text = "hello";
    //    tt.Controls.Add(tb);
    //    PlaceHolder2.Controls.Add(tt);
    //    Session["tt"] = tt;
    //}
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (Session["tt"]!=null)
    //    {
    //        Panel tt = Session["tt"] as Panel;
    //        PlaceHolder2.Controls.Add(tt);
    //    }
    //protected void LinkButton1_Click(object sender, EventArgs e)
    //{
    //}
    protected void tbblood_TextChanged(object sender, EventArgs e)
    {
    }
    protected void ddlrollno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlrollno.SelectedValue == "---Select---")
        {
            tbrollno.Text = "";
            tbrollno.Enabled = false;
        }
        else
        {
            tbrollno.Enabled = true;
        }
    }
    protected void LinkButtonseattype_Click(object sender, EventArgs e)
    {
        cblseattype.ClearSelection();
        PlaceHolderseattype.Controls.Clear();
        seatcnt = 0;
        tbseattype.Text = "---Select---";
        LinkButtonseattype.Visible = false;
    }
    protected void LinkButtonblood_Click(object sender, EventArgs e)
    {
        cblblood.ClearSelection();
        PlaceHolderblood.Controls.Clear();
        bloodcnt = 0;
        tbblood.Text = "---Select---";
        LinkButtonblood.Visible = false;
    }
    //sem
    protected void LinkButtonsem_Click(object sender, EventArgs e)
    {
        cblsem.ClearSelection();
        PlaceHoldersemester.Controls.Clear();
        semcnt = 0;
        tbsem.Text = "---Select---";
        LinkButtonsemester.Visible = false;
    }
    //sec
    protected void LinkButtonsec_Click(object sender, EventArgs e)
    {
        cblsection.ClearSelection();
        PlaceHoldersection.Controls.Clear();
        seccnt = 0;
        tbsection.Text = "---Select---";
        LinkButtonsection.Visible = false;
    }
    //
    protected void LinkButtoncaste_Click(object sender, EventArgs e)
    {
        cblcaste.ClearSelection();
        PlaceHoldercaste.Controls.Clear();
        castecnt = 0;
        tbcaste.Text = "---Select---";
        LinkButtoncaste.Visible = false;
    }
    protected void LinkButtonreligion_Click(object sender, EventArgs e)
    {
        cblreligion.ClearSelection();
        PlaceHolderreligion.Controls.Clear();
        religcnt = 0;
        tbreligion.Text = "---Select---";
        LinkButtonreligion.Visible = false;
    }
    protected void LinkButtoncomm_Click(object sender, EventArgs e)
    {
        cblcomm.ClearSelection();
        PlaceHoldercomm.Controls.Clear();
        commcnt = 0;
        tbcomm.Text = "---Select---";
        LinkButtoncomm.Visible = false;
    }
    protected void LinkButtonregion_Click(object sender, EventArgs e)
    {
        cblregion.ClearSelection();
        PlaceHolderregion.Controls.Clear();
        regioncnt = 0;
        tbregion.Text = "---Select---";
        LinkButtonregion.Visible = false;
    }
    protected void LinkButtonmtongue_Click(object sender, EventArgs e)
    {
        cblmtongue.ClearSelection();
        PlaceHoldermtongue.Controls.Clear();
        mtonguecnt = 0;
        tbmtongue.Text = "---Select---";
        LinkButtonmtongue.Visible = false;
    }
    protected void LinkButtonfoccu_Click(object sender, EventArgs e)
    {
        cblfoccu.ClearSelection();
        PlaceHolderfoccu.Controls.Clear();
        foccucnt = 0;
        tbfoccu.Text = "---Select---";
        LinkButtonfoccu.Visible = false;
    }
    protected void LinkButtonfqual_Click(object sender, EventArgs e)
    {
        cblfqual.ClearSelection();
        PlaceHolderfqual.Controls.Clear();
        fqualcnt = 0;
        tbfqual.Text = "---Select---";
        LinkButtonfqual.Visible = false;
    }
    protected void LinkButtonmoccu_Click(object sender, EventArgs e)
    {
        cblmoccu.ClearSelection();
        PlaceHolderfoccu.Controls.Clear();
        foccucnt = 0;
        tbfoccu.Text = "---Select---";
        LinkButtonmoccu.Visible = false;
    }
    protected void LinkButtonmqual_Click(object sender, EventArgs e)
    {
        cblmqual.ClearSelection();
        PlaceHoldermqual.Controls.Clear();
        mqualcnt = 0;
        tbmqual.Text = "---Select---";
        LinkButtonmqual.Visible = false;
    }
    protected void LinkButtondegree_Click(object sender, EventArgs e)
    {
        cbldegree.ClearSelection();
        PlaceHolderdegree.Controls.Clear();
        degreecnt = 0;
        tbdegree.Text = "---Select---";
        LinkButtondegree.Visible = false;
    }
    protected void LinkButtonbranch_Click(object sender, EventArgs e)
    {
        cblbranch.ClearSelection();
        PlaceHolderbranch.Controls.Clear();
        branchcnt = 0;
        tbbranch.Text = "---Select---";
        LinkButtonbranch.Visible = false;
    }
    protected void cblsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //    int i;
        //    int searchcount = 0;
        //    string value = "";
        //    int code = 0;
        //    LinkButtonseattype.Visible = true;
        //    for (i = 0; i < cblsearch.Items.Count; i++)
        //    {
        //        if (cblsearch.Items[i].Selected == true)
        //        {
        //            value = cblsearch.Items[i].Text;
        //           // code = Convert.ToInt32(cblsearch.Items[i].Value.ToString());
        //            code = i;
        //            searchcount = searchcount +1;
        //            cblsearch.Items[i].Text = value + "(" + searchcount + ")";
        //        }
        //    }
        //    if (searchcount == 0)
        //        cblsearch.Items[i].Text = val
    }
    protected void ddlpemailid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlpemailid1.SelectedValue == "Others")
        //{
        //    ddlpemailid.Visible = true;
        //    tbpemailid.Visible = true;
        //}
        //else
        //{
        //    ddlpemailid.Visible = false;
        //    tbpemailid.Visible = false;
        //}
    }
    protected void ddlpcity1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpcity1.SelectedValue == "Others")
        {
            ddlpcity.Visible = true;
            tbpcity.Visible = true;
        }
        else
        {
            ddlpcity.Visible = false;
            tbpcity.Visible = false;
        }
    }
    protected void ddlpdistrict1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpdistrict1.SelectedValue == "Others")
        {
            ddlpdistrict.Visible = true;
            tbpdistrict.Visible = true;
        }
        else
        {
            ddlpdistrict.Visible = false;
            tbpdistrict.Visible = false;
        }
    }
    protected void ddlccountry1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlccountry1.SelectedValue == "Others")
        {
            ddlccountry.Visible = true;
            tbccountry.Visible = true;
        }
        else
        {
            ddlccountry.Visible = false;
            tbccountry.Visible = false;
        }
    }
    protected void ddlccity1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlccity1.SelectedValue == "Others")
        {
            ddlccity.Visible = true;
            tbccity.Visible = true;
        }
        else
        {
            ddlccity.Visible = false;
            tbccity.Visible = false;
        }
    }
    protected void ddlgcountry1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgcountry1.SelectedValue == "Others")
        {
            ddlgcountry.Visible = true;
            tbgcountry.Visible = true;
        }
        else
        {
            ddlgcountry.Visible = false;
            tbgcountry.Visible = false;
        }
    }
    protected void ddlgstreet1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgstreet1.SelectedValue == "Others")
        {
            ddlgstreet.Visible = true;
            tbgstreet.Visible = true;
        }
        else
        {
            ddlgstreet.Visible = false;
            tbgstreet.Visible = false;
        }
    }
    protected void ddlpstreet1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpstreet1.SelectedValue == "Others")
        {
            ddlpstreet.Visible = true;
            tbpstreet.Visible = true;
        }
        else
        {
            ddlpstreet.Visible = false;
            tbpstreet.Visible = false;
        }
    }
    protected void ddlpcountry1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpcountry1.SelectedValue == "Others")
        {
            ddlpcountry.Visible = true;
            tbpcountry.Visible = true;
        }
        else
        {
            ddlpcountry.Visible = false;
            tbpcountry.Visible = false;
        }
    }
    protected void ddlcdistrict1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcdistrict1.SelectedValue == "Others")
        {
            ddlcdistrict.Visible = true;
            tbcdistrict.Visible = true;
        }
        else
        {
            ddlcdistrict.Visible = false;
            tbcdistrict.Visible = false;
        }
    }
    protected void ddlcstreet1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcstreet1.SelectedValue == "Others")
        {
            ddlcstreet.Visible = true;
            tbcstreet.Visible = true;
        }
        else
        {
            ddlcstreet.Visible = false;
            tbcstreet.Visible = false;
        }
    }
    protected void ddlgdistrict1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgdistrict1.SelectedValue == "Others")
        {
            ddlgdistrict.Visible = true;
            tbgdistrict.Visible = true;
        }
        else
        {
            ddlgdistrict.Visible = false;
            tbgdistrict.Visible = false;
        }
    }
    protected void ddlgcity1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgcity1.SelectedValue == "Others")
        {
            ddlgcity.Visible = true;
            tbgcity.Visible = true;
        }
        else
        {
            ddlgcity.Visible = false;
            tbgcity.Visible = false;
        }
    }
    protected void cblsearch_SelectedIndexChanged1(object sender, EventArgs e)
    {
        string value = "";
        int index;
        value = string.Empty;
        string result = Request.Form["__EVENTTARGET"];
        string[] checkedBox = result.Split('$');
        index = int.Parse(checkedBox[checkedBox.Length - 1]);
        string sindex = Convert.ToString(index);
        if (cblsearch.Items[index].Selected)
        {
            if (!Itemindex.Contains(sindex))
            {
                ItemList.Add(cblsearch.Items[index].Text.ToString());
                Itemindex.Add(sindex);
            }
        }
        else
        {
            ItemList.Remove(cblsearch.Items[index].Text.ToString());
            Itemindex.Remove(sindex);
        }
        for (int i = 0; i < cblsearch.Items.Count; i++)
        {
            if (cblsearch.Items[i].Selected == false)
            {
                sindex = Convert.ToString(i);
                ItemList.Remove(cblsearch.Items[i].Text.ToString());
                Itemindex.Remove(sindex);
            }
        }
        LinkButtonsremove.Visible = true;
        tborder.Visible = true;
        tborder.Text = "";
        for (int i = 0; i < ItemList.Count; i++)
        {
            tborder.Text = tborder.Text + ItemList[i].ToString();
            tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
        }
        if (ItemList.Count == 0)
        {
            tborder.Visible = false;
            LinkButtonsremove.Visible = false;
        }
    }
    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        cblsearch.ClearSelection();
        CheckBoxselect.Checked = false;
        LinkButtonsremove.Visible = false;
        ItemList.Clear();
        Itemindex.Clear();
        tborder.Text = "";
        tborder.Visible = false;
    }
    protected void CheckBoxselect_CheckedChanged(object sender, EventArgs e)
    {
        ItemList.Clear();
        Itemindex.Clear();
        if (CheckBoxselect.Checked == true)
        {
            for (int i = 0; i < cblsearch.Items.Count; i++)
            {
                string si = Convert.ToString(i);
                cblsearch.Items[i].Selected = true;
                LinkButtonsremove.Visible = true;
                ItemList.Add(cblsearch.Items[i].Text.ToString());
                Itemindex.Add(si);
            }
        }
        else
        {
            for (int i = 0; i < cblsearch.Items.Count; i++)
            {
                cblsearch.Items[i].Selected = false;
                LinkButtonsremove.Visible = false;
                //ItemList.Clear();
                //Itemindex.Clear();
            }
        }
        tborder.Text = "";
        tborder.Visible = false;
    }
    //
    protected void CheckBoxblood_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxblood.Checked == true)
        {
            if (cblblood.Items.Count > 0)
            {
                for (int i = 0; i < cblblood.Items.Count; i++)
                    cblblood.Items[i].Selected = true;
                //  ViewState["ibloodcontrol"] = true;
            }
        }
        else
        {
            if (cblblood.Items.Count > 0)
                cblblood.ClearSelection();
        }
    }
    //
    //
    protected void CheckBoxcaste_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxcaste.Checked == true)
        {
            if (cblcaste.Items.Count > 0)
                for (int i = 0; i < cblcaste.Items.Count; i++)
                    cblcaste.Items[i].Selected = true;
        }
        else
        {
            if (cblcaste.Items.Count > 0)
                cblcaste.ClearSelection();
        }
    }
    //
    //
    protected void CheckBoxseat_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxseat.Checked == true)
        {
            if (cblseattype.Items.Count > 0)
                for (int i = 0; i < cblseattype.Items.Count; i++)
                    cblseattype.Items[i].Selected = true;
        }
        else
        {
            if (cblseattype.Items.Count > 0)
                cblseattype.ClearSelection();
        }
    }
    //
    //
    protected void CheckBoxfqual_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxfqual.Checked == true)
        {
            if (cblfqual.Items.Count > 0)
            {
                for (int i = 0; i < cblfqual.Items.Count; i++)
                    cblfqual.Items[i].Selected = true;
                //ViewState["ifqualcontrol"] = true;
                //ViewState["lfqualcontrol"] = true;
            }
        }
        else
        {
            if (cblfqual.Items.Count > 0)
                cblfqual.ClearSelection();
        }
    }
    //
    //
    protected void CheckBoxmqual_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxmqual.Checked == true)
        {
            if (cblmqual.Items.Count > 0)
                for (int i = 0; i < cblmqual.Items.Count; i++)
                    cblmqual.Items[i].Selected = true;
        }
        else
        {
            if (cblmqual.Items.Count > 0)
                cblmqual.ClearSelection();
        }
    }
    // //
    protected void CheckBoxdegree_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxdegree.Checked == true)
        {
            if (cbldegree.Items.Count > 0)
                for (int i = 0; i < cbldegree.Items.Count; i++)
                    cbldegree.Items[i].Selected = true;
        }
        else
        {
            if (cbldegree.Items.Count > 0)
                cbldegree.ClearSelection();
        }
    }
    // //
    protected void CheckBoxsem_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxsem.Checked == true)
        {
            if (cblsem.Items.Count > 0)
                for (int i = 0; i < cblsem.Items.Count; i++)
                    cblsem.Items[i].Selected = true;
        }
        else
        {
            if (cblsem.Items.Count > 0)
                cblsem.ClearSelection();
        }
    }
    // //
    protected void CheckBoxmtongue_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxmtongue.Checked == true)
        {
            if (cblmtongue.Items.Count > 0)
                for (int i = 0; i < cblmtongue.Items.Count; i++)
                    cblmtongue.Items[i].Selected = true;
        }
        else
        {
            if (cblmtongue.Items.Count > 0)
                cblmtongue.ClearSelection();
        }
    }
    // //
    protected void CheckBoxfoccu_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxfoccu.Checked == true)
        {
            if (cblfoccu.Items.Count > 0)
                for (int i = 0; i < cblfoccu.Items.Count; i++)
                    cblfoccu.Items[i].Selected = true;
        }
        else
        {
            if (cblfoccu.Items.Count > 0)
                cblfoccu.ClearSelection();
        }
    }
    // //
    protected void CheckBoxmoccu_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxmoccu.Checked == true)
        {
            if (cblmoccu.Items.Count > 0)
                for (int i = 0; i < cblmoccu.Items.Count; i++)
                    cblmoccu.Items[i].Selected = true;
        }
        else
        {
            if (cblmoccu.Items.Count > 0)
                cblmoccu.ClearSelection();
        }
    }
    // //
    protected void CheckBoxcomm_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxcomm.Checked == true)
        {
            if (cblcomm.Items.Count > 0)
                for (int i = 0; i < cblcomm.Items.Count; i++)
                    cblcomm.Items[i].Selected = true;
        }
        else
        {
            if (cblcomm.Items.Count > 0)
                cblcomm.ClearSelection();
        }
    }
    // //
    protected void CheckBoxregion_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxregion.Checked == true)
        {
            if (cblregion.Items.Count > 0)
                for (int i = 0; i < cblregion.Items.Count; i++)
                    cblregion.Items[i].Selected = true;
        }
        else
        {
            if (cblregion.Items.Count > 0)
                cblregion.ClearSelection();
        }
    }
    // //
    protected void CheckBoxreligion_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxreligion.Checked == true)
        {
            if (cblreligion.Items.Count > 0)
                for (int i = 0; i < cblreligion.Items.Count; i++)
                    cblreligion.Items[i].Selected = true;
        }
        else
        {
            if (cblreligion.Items.Count > 0)
                cblreligion.ClearSelection();
        }
    }
    // //
    protected void CheckBoxsection_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxsection.Checked == true)
        {
            if (cblsection.Items.Count > 0)
                for (int i = 0; i < cblsection.Items.Count; i++)
                    cblsection.Items[i].Selected = true;
        }
        else
        {
            if (cblsection.Items.Count > 0)
                cblsection.ClearSelection();
        }
    }
    // //
    protected void CheckBoxbranch_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxbranch.Checked == true)
        {
            if (cblbranch.Items.Count > 0)
                for (int i = 0; i < cblbranch.Items.Count; i++)
                    cblbranch.Items[i].Selected = true;
        }
        else
        {
            if (cblbranch.Items.Count > 0)
                cblbranch.ClearSelection();
        }
    }
    //
    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "   Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }
    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxother.Text = "";
        lblother.Visible = false;
        LabelE.Visible = false;
        if (DropDownListpage.Text == "Others")
        {
            TextBoxother.Visible = true;
            TextBoxother.Focus();
        }
        else
        {
            TextBoxother.Visible = false;
            FpSpread1.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
    }
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    //ddlappno.Text = "";
    //    ddlappno.SelectedIndex = 0;
    //    tbappno.Text = "";
    //    ddladmno.SelectedIndex = 0;
    //    tbadmno.Text = "";
    //    ddlstudname.SelectedIndex = 0;
    //    tbfromadmdt.Text = "";
    //    tbfromappdt.Text = "";
    //    tbtoadmdt.Text = "";
    //    tbtoappdt.Text = "";
    //    tbstudname.Text = "";
    //    ddlregno.SelectedIndex = 0;
    //    ddlrollno.SelectedIndex = 0;
    //    tbregno.Text = "";
    //    tbrollno.Text = "";
    //   // Button1.Visible = false;
    //    Labeldatead.Visible = false;
    //    Labeldateap.Visible = false;
    //}
    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    tbfromdob.Text = "";
    //    tbtodob.Text = "";
    //    ddlfname.SelectedIndex = 0;
    //    ddlmname.Text = "---Select---";
    //    ddlgname.Text = "---Select---";
    //    tbfname.Text = "";
    //    tbmname.Text = "";
    //    tbgname.Text = "";
    //    tbseattype.Text = "---Select---";
    //    tbblood.Text = "---Select---";
    //    tbcaste.Text = "---Select---";
    //    tbregion.Text = "---Select---";
    //    tbreligion.Text = "---Select---";
    //    tbcomm.Text = "---Select---";
    //    tbfmobno.Text = "---Select---";
    //    tbmmobno.Text = "---Select---";
    //    tbfoccu.Text = "---Select---";
    //    tbfoffno.Text = "---Select---";
    //    tbfqual.Text = "---Select---";
    //    tbmmobno.Text = "---Select---";
    //    tbmoccu.Text = "---Select---";
    //    tbmoffno.Text = "---Select---";
    //    tbmqual.Text = "---Select---";
    //    tbmtongue.Text = "---Select---";
    //    tbstudmobno.Text = "---Select---";
    //    PlaceHolderblood.Controls.Clear();
    //    PlaceHoldercaste.Controls.Clear();
    //    PlaceHoldercomm.Controls.Clear();
    //    PlaceHolderfoccu.Controls.Clear();
    //    PlaceHolderfqual.Controls.Clear();
    //    PlaceHoldermoccu.Controls.Clear();
    //    PlaceHoldermqual.Controls.Clear();
    //    PlaceHoldermtongue.Controls.Clear();
    //    PlaceHolderregion.Controls.Clear();
    //    PlaceHolderreligion.Controls.Clear();
    //    PlaceHolderseattype.Controls.Clear();
    //    cblseattype.ClearSelection();
    //    cblblood.ClearSelection();
    //    cblcaste.ClearSelection();
    //    cblregion.ClearSelection();
    //    cblregion.ClearSelection();
    //    cblcomm.ClearSelection();
    //    cblmtongue.ClearSelection();
    //    cblfoccu.ClearSelection();
    //    cblfqual.ClearSelection();
    //    cblmoccu.ClearSelection();
    //    cblmqual.ClearSelection();
    //    LinkButtonseattype.Visible = false;
    //    LinkButtonblood.Visible = false;
    //    LinkButtonreligion.Visible = false;
    //    LinkButtonmtongue.Visible = false;
    //    LinkButtoncaste.Visible = false;
    //    LinkButtoncomm.Visible = false;
    //    LinkButtonfoccu.Visible = false;
    //    LinkButtonfqual.Visible = false;
    //    LinkButtonmoccu.Visible = false;
    //    LinkButtonmqual.Visible = false;
    //    LinkButtonregion.Visible = false;
    //    Labeldatedob.Visible = false;
    //    Button2.Visible = false;
    //    lblsmobno.Visible = false;
    //    lblmoffno.Visible = false;
    //    lblmmobno.Visible = false;
    //    lblfoffno.Visible = false;
    //    lblfmobno.Visible = false;
    //}
    //protected void Button3_Click(object sender, EventArgs e)
    //{
    //    ddlpemailid1.Text = "---Select---";
    //    ddlpemailid1_SelectedIndexChanged(sender, e);
    //    ddlpcity1.Text = "---Select---";
    //    ddlpcity1_SelectedIndexChanged(sender, e);
    //    ddlpcountry1.Text = "---Select---";
    //    ddlpcountry1_SelectedIndexChanged(sender, e);
    //    ddlpdistrict1.Text = "---Select---";
    //    ddlpdistrict1_SelectedIndexChanged(sender, e);
    //    ddlpstreet1.Text = "---Select---";
    //    ddlpstreet1_SelectedIndexChanged(sender, e);
    //    ddlccity1.Text = "---Select---";
    //    ddlccity1_SelectedIndexChanged(sender, e);
    //    ddlccountry.Text = "---Select---";
    //    ddlccountry1_SelectedIndexChanged(sender, e);
    //    ddlcdistrict1.Text = "---Select---";
    //    ddlcdistrict1_SelectedIndexChanged(sender, e);
    //    ddlgcity1.Text = "---Select---";
    //    ddlgcity1_SelectedIndexChanged(sender, e);
    //    ddlgcountry1.Text = "---Select---";
    //    ddlgcountry1_SelectedIndexChanged(sender, e);
    //    ddlgdistrict1.Text = "---Select---";
    //    ddlgdistrict1_SelectedIndexChanged(sender, e);
    //    ddlcstreet1.Text = "---Select---";
    //    ddlcstreet1_SelectedIndexChanged(sender, e);
    //    ddlgstreet1.Text = "---Select---";
    //    ddlgstreet1_SelectedIndexChanged(sender, e);
    //    tbpcity.Visible = false;
    //    Button3.Visible = false;
    //}
    //protected void Button4_Click(object sender, EventArgs e)
    //{
    //    int i;
    //    ddlbatchyrfrm.Text = "---Select---";
    //    ddlbatchyrto.Text = "---Select---";
    //    tbdegree.Text = "";
    //    tbbranch.Text = "";
    //    tbsem.Text = "";
    //    tbsec.Text = "";
    //    PlaceHolderdegree.Controls.Clear();
    //    PlaceHolderbranch.Controls.Clear();
    //    PlaceHoldersection.Controls.Clear();
    //    PlaceHoldersemester.Controls.Clear();
    //    LinkButtonsection.Visible = false;
    //    LinkButtondegree.Visible = false;
    //    LinkButtonbranch.Visible = false;
    //    LinkButtonsemester.Visible = false;
    //    cbldegree.ClearSelection();
    //    cblbranch.ClearSelection();
    //    cblsection.ClearSelection();
    //    cblsem.ClearSelection();
    //    Button4.Visible = false;
    //for (i = 0; i < degreecnt; i++)
    //{
    //    cbldegree.Items[i].Selected = false;
    //}
    //for (i = 0; i < degreecnt; i++)
    //{
    //    cbldegree.Items[i].Selected = false;
    //}
    // }
    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        lblother.Visible = false;
        try
        {
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                if (TextBoxpage.Text.Trim() != "")
                {
                    if (Convert.ToInt32(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                    {
                        LabelE.Visible = true;
                        LabelE.Text = "Exceed The Page Limit";
                        TextBoxpage.Text = "";
                        FpSpread1.Visible = true;
                        //Added By Srinath 7/5/2013
                        btnprintmaster.Visible = true;
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                        btnexcel.Visible = true;
                        Printcontrol.Visible = false;
                    }
                    else if ((Convert.ToInt32(TextBoxpage.Text) == 0))
                    {
                        LabelE.Text = "Should be Greater than Zero";
                        LabelE.Visible = true;
                        TextBoxpage.Text = "";
                        FpSpread1.Visible = true;
                        //Added By Srinath 7/5/2013
                        btnexcel.Visible = true;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = true;
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                        btnexcel.Visible = true;
                        Printcontrol.Visible = false;
                    }
                    else
                    {
                        LabelE.Visible = false;
                        FpSpread1.CurrentPage = Convert.ToInt32(TextBoxpage.Text) - 1;
                        FpSpread1.Visible = true;
                        //Added By Srinath 7/5/2013
                        btnprintmaster.Visible = true;
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                        btnexcel.Visible = true;
                        Printcontrol.Visible = false;
                    }
                }
            }
        }
        catch
        {
            LabelE.Text = "Exceed The Page Limit";
            TextBoxpage.Text = "";
            LabelE.Visible = true;
        }
    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        try
        {
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                if (TextBoxother.Text != "")
                {
                    FpSpread1.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                    CalculateTotalPages();
                    lblother.Visible = false;
                    if (FpSpread1.Sheets[0].PageSize > FpSpread1.Sheets[0].RowCount)
                    {
                        lblother.Visible = true;
                        lblother.Text = "Exceed the Record Limit";
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Height = (25 * FpSpread1.Sheets[0].PageSize) + 150;
                        TextBoxother.Text = "";
                    }
                }
            }
        }
        catch
        {
            lblother.Text = "Enter the Valid Page";
            TextBoxother.Text = "";
            lblother.Visible = true;
        }
    }

    //protected void FpSpread1_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    FpSpread1.Sheets[0].AutoPostBack = true;
    //    FpSpread1.SaveChanges();
    //    if (Cellclick == true)
    //    {
    //        string activerow = "";
    //        string activecol = "";
    //        activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
    //        activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
    //        int ar;
    //        int ac;
    //        ar = Convert.ToInt32(activerow.ToString());
    //        ac = Convert.ToInt32(activecol.ToString());
    //        Session["appno"] = "";
    //        if (ar != -1)
    //        {
    //            appno = FpSpread1.Sheets[0].Cells[ar, 1].Tag.ToString();
    //            Session["appno"] = appno;
    //            string redrURI = ConfigurationManager.AppSettings["OfficeInd"].Trim();
    //            Session["OfficeMod"] = "Office";
    //            string Enq = "C$" + Session["collegecode"] + "&U$" + Session["UserCode"] + "&G$" + Session["group_code"] + "&S$" + Session["Single_User"] + "&P$" + Session["column_header_row_count"] + "&V$" + Session["Version"] + "&SC$" + Session["Staff_Code"] + "&UN$" + Session["UserName"] + "&E$" + Session["Entry_Code"] + "&L$" + Session["IsLogin"] + "&Cu$" + Session["current_college_code"] + "&IC$" + Session["InternalCollegeCode"] + "&Pa$" + Session["password"] + "&PV$" + Session["prntvissble"] + "&D$" + Session["dashprod"] + "&M$" + Session["MID"] + "&Type$" + Session["Type"] + "&App$" + Session["appno"] + "&StuOff$" + Session["OfficeMod"];
    //            string encQstring = AesEncrypt(Enq);
    //            Response.Redirect(redrURI + "?t*" + encQstring);
    //            //Response.Redirect(redrURI + "?app=" + Encrypt(appno) + "&Type=Admin");
    //            //Response.Redirect("../IndReport.aspx?app=" + Encrypt(appno) + "&Type=Admin");
    //            //Response.Write(appno);
    //        }
    //        Cellclick = false;
    //    }
    //}

    protected void FpSpread1_SelectedIndexChanged(Object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.SaveChanges();
        if (Cellclick == true)
        {
            string activerow = "";
            string activecol = "";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());
            Session["appno"] = "";
            if (ar != -1)
            {
                appno = FpSpread1.Sheets[0].Cells[ar, 1].Tag.ToString();
                Session["appno"] = appno;
                Response.Redirect("../IndReport.aspx?app=" + Encrypt(appno) + "&Type=Admin");
                //Response.Write(appno);
            }
            Cellclick = false;
        }
    }

    private string AesEncrypt(string password)
    {
        string strmsg = string.Empty;
        byte[] encode = new byte[password.Length];
        encode = Encoding.UTF8.GetBytes(password);
        strmsg = Convert.ToBase64String(encode);
        return strmsg;
    }

    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread1.Sheets[0].AutoPostBack = true;
        Cellclick = true;
    }
    protected void tbtoappdt_TextChanged(object sender, EventArgs e)
    {
        if (tbfromappdt.Text == "")
        {
            tbtoappdt.Text = "";
            Labeldateap.Visible = true;
        }
        else
        {
            Labeldateap.Visible = false;
            string datefap, dtfromap;
            string datefromap;
            string yr2, m2, d2;
            datefap = tbfromappdt.Text.ToString();
            string[] split2 = datefap.Split(new Char[] { '-' });
            if (split2.Length == 3)
            {
                datefromap = split2[0].ToString() + "-" + split2[1].ToString() + "-" + split2[2].ToString();
                yr2 = split2[2].ToString();
                m2 = split2[1].ToString();
                d2 = split2[0].ToString();
                dtfromap = m2 + "-" + d2 + "-" + yr2;
                string date2ap;
                string datetoap;
                string yr3, m3, d3;
                date2ap = tbtoappdt.Text.ToString();
                string[] split3 = date2ap.Split(new Char[] { '-' });
                if (split3.Length == 3)
                {
                    datetoap = split3[0].ToString() + "-" + split3[1].ToString() + "-" + split3[2].ToString();
                    yr3 = split3[2].ToString();
                    m3 = split3[1].ToString();
                    d3 = split3[0].ToString();
                    datetoap = m3 + "-" + d3 + "-" + yr3;
                    DateTime dt1 = Convert.ToDateTime(datetoap);
                    DateTime dt2 = Convert.ToDateTime(dtfromap);
                    TimeSpan ts = dt1 - dt2;
                    int days = ts.Days;
                    if (days < 0)
                    {
                        Labeldateap.Text = "To date must be greater than from date";
                        tbtoappdt.Text = "";
                        tbfromappdt.Text = "";
                        Labeldateap.Visible = true;
                    }
                }
            }
        }
    }
    protected void tbtodob_TextChanged(object sender, EventArgs e)
    {
        if (tbfromdob.Text == "")
        {
            tbtodob.Text = "";
            Labeldatedob.Visible = true;
        }
        else
        {
            Labeldatedob.Visible = false;
            string datefdob, dtfromdob;
            string datefromdob;
            string yr, m, d;
            datefdob = tbfromdob.Text.ToString();
            string[] split = datefdob.Split(new Char[] { '-' });
            if (split.Length == 3)
            {
                datefromdob = split[0].ToString() + "-" + split[1].ToString() + "-" + split[2].ToString();
                yr = split[2].ToString();
                m = split[1].ToString();
                d = split[0].ToString();
                dtfromdob = m + "-" + d + "-" + yr;
                string date2dob;
                string datetodob;
                string yr1, m1, d1;
                date2dob = tbtodob.Text.ToString();
                string[] split1 = date2dob.Split(new Char[] { '-' });
                if (split1.Length == 3)
                {
                    datetodob = split1[0].ToString() + "-" + split1[1].ToString() + "-" + split1[2].ToString();
                    yr1 = split1[2].ToString();
                    m1 = split1[1].ToString();
                    d1 = split1[0].ToString();
                    datetodob = m1 + "-" + d1 + "-" + yr1;
                    DateTime dt1 = Convert.ToDateTime(datetodob);
                    DateTime dt2 = Convert.ToDateTime(dtfromdob);
                    TimeSpan ts = dt1 - dt2;
                    int days = ts.Days;
                    if (days < 0)
                    {
                        Labeldatedob.Text = "To date must be greater than from date";
                        tbtodob.Text = "";
                        tbfromdob.Text = "";
                        Labeldatedob.Visible = true;
                    }
                }
            }
        }
    }
    protected void tbtoadmdt_TextChanged(object sender, EventArgs e)
    {
        if (tbfromadmdt.Text == "")
        {
            tbtoadmdt.Text = "";
            Labeldatead.Visible = true;
            Labeldatead.Text = "Enter from date first";
        }
        else
        {
            Labeldatead.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = tbfromadmdt.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '-' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "-" + split4[1].ToString() + "-" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "-" + d4 + "-" + yr4;
                string date2ad;
                string datetoad;
                string yr5, m5, d5;
                date2ad = tbtoadmdt.Text.ToString();
                string[] split5 = date2ad.Split(new Char[] { '-' });
                if (split5.Length == 3)
                {
                    datetoad = split5[0].ToString() + "-" + split5[1].ToString() + "-" + split5[2].ToString();
                    yr5 = split5[2].ToString();
                    m5 = split5[1].ToString();
                    d5 = split5[0].ToString();
                    datetoad = m5 + "-" + d5 + "-" + yr5;
                    DateTime dt1 = Convert.ToDateTime(dtfromad);
                    DateTime dt2 = Convert.ToDateTime(datetoad);
                    TimeSpan ts = dt2 - dt1;
                    int days = ts.Days;
                    if (days < 0)
                    {
                        Labeldatead.Text = "To date must be greater than from date";
                        tbtoadmdt.Text = "";
                        tbfromadmdt.Text = "";
                        Labeldatead.Visible = true;
                    }
                }
            }
        }
    }
    protected void tbfromappdt_TextChanged(object sender, EventArgs e)
    {
        //if (tbfromappdt.Text != "")
        //    Labeldateap.Visible = false;
        if (tbtoappdt.Text == "")
        {
            Labeldateap.Text = "Enter to date";
            Labeldateap.Visible = true;
        }
    }
    protected void tbfromadmdt_TextChanged(object sender, EventArgs e)
    {
        if (tbtoadmdt.Text == "")
        {
            Labeldatead.Text = "Enter to date";
            Labeldatead.Visible = true;
        }
        //else
        //    tbtoadmdt_TextChanged(sender, e);
    }
    protected void tbfromdob_TextChanged(object sender, EventArgs e)
    {
        if (tbfromdob.Text != "")
        {
            Labeldatedob.Text = "Enter to date";
            Labeldatedob.Visible = true;
        }
    }
    //protected void ddlappno_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlappno.SelectedValue == "---Select---")
    //    {
    //        tbappno.Text = "";
    //        tbappno.Enabled = false;
    //    }
    //    else
    //    {
    //        tbappno.Enabled = true;
    //    }
    //}
    //protected void ddlImprotType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDataToDDLImportFileName(GetImportExportPath(2, null), "." + this.ddlImprotType.SelectedValue);
    //}
    //protected void BtnImprot_Click(object sender, EventArgs e)
    //{
    //    ImportDataToSpread(this.FpSpread1, this.ddlImprotType.SelectedValue, this.DDLImprotFileName.SelectedValue);
    //}
    //private string GetImportExportPath(int condition, string path)
    //{
    //    string strCurDir = "";
    //    switch (condition)
    //    {
    //        case 0://
    //            strCurDir = Server.MapPath(".");
    //            break;
    //        case 1://
    //            strCurDir = path;
    //            break;
    //        //case 2://
    //        //    strCurDir = this.Page.MapPath("Data/Import/" + this.ddlImprotType.SelectedValue);
    //        //    break;
    //        case 2://
    //            if (!System.IO.Directory.Exists(this.Page.MapPath("Data/Export/")))
    //                System.IO.Directory.CreateDirectory(this.Page.MapPath("Data/Export/"));
    //            strCurDir = this.Page.MapPath("Data/Export/");
    //            break;
    //    }
    //    return strCurDir;
    //}
    //private void ExportDataFromSpread(FpSpread spread, ArrayList exportTypeList, string path)
    //{
    //    string exportfileName = "";
    //    foreach (string exportType in exportTypeList)
    //    {
    //        exportfileName = System.DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "").Replace("\\", "");
    //        switch (exportType)
    //        {
    //            case "xlsx"://
    //                spread.SaveExcel(path + exportfileName + "." + exportType, ExcelSaveFlags.UseOOXMLFormat);
    //                break;
    //            case "xls"://
    //                spread.SaveExcel(path + exportfileName + "." + exportType);
    //                break;
    //            //case "xml"://
    //            //    spread.Save(path + exportfileName + "." + exportType, true);
    //            //    break;
    //            //case "txt"://
    //            //    spread.Sheets.Count = 1;
    //            //    spread.ActiveSheetView.SaveTextFile(path + exportfileName + "." + exportType, false);
    //            //    break;
    //            case "pdf"://
    //                spread.SavePdf(path + exportfileName + "." + exportType);
    //                break;
    //            case "SavePdf"://
    //                spread.SavePdfToResponse(exportfileName + ".pdf");
    //                break;
    //            case "SaveExcel":
    //                spread.SaveExcelToResponse(exportfileName + ".xls");
    //                break;
    //            case "SaveExcel2007":
    //                spread.SaveExcelToResponse(exportfileName + ".xlsx", ExcelSaveFlags.UseOOXMLFormat);
    //                break;
    //        }
    //    }
    //}
    //protected void BtnExport_Click(object sender, EventArgs e)
    //{
    //    ArrayList arrList = new ArrayList();
    //    foreach (System.Web.UI.WebControls.ListItem lItem in this.cblExportTypeList.Items)
    //    {
    //        if (lItem.Selected)
    //            arrList.Add(lItem.Text);
    //    }
    //    ExportDataFromSpread(this.FpSpread1, arrList, GetImportExportPath(3, ""));
    //}
    //protected void BtnPrint_Click(object sender, EventArgs e)
    //{
    //    ArrayList arrList = new ArrayList();
    //    arrList.Add(RadioButtonList1.SelectedValue.ToString());
    //    FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
    //    ExportDataFromSpread(this.FpSpread1, arrList, "");
    //}
    protected void ddlappno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlappno.SelectedValue == "---Select---")
        {
            tbappno.Text = "";
            tbappno.Enabled = false;
        }
        else
        {
            tbappno.Enabled = true;
        }
    }
    protected void ddlregno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlregno.SelectedValue == "---Select---")
        {
            tbregno.Text = "";
            tbregno.Enabled = false;
        }
        else
        {
            tbregno.Enabled = true;
        }
    }
    protected void ddlstudname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlstudname.SelectedValue == "---Select---")
        {
            tbstudname.Text = "";
            tbstudname.Enabled = false;
        }
        else
        {
            tbstudname.Enabled = true;
        }
    }
    protected void ddladmno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddladmno.SelectedValue == "---Select---")
        {
            tbadmno.Text = "";
            tbadmno.Enabled = false;
        }
        else
        {
            tbadmno.Enabled = true;
        }
    }
    protected void ddlfname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlfname.SelectedValue == "---Select---")
        {
            tbfname.Text = "";
            tbfname.Enabled = false;
        }
        else
        {
            tbfname.Enabled = true;
        }
    }
    protected void ddlmname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlmname.SelectedValue == "---Select---")
        {
            tbmname.Text = "";
            tbmname.Enabled = false;
        }
        else
        {
            tbmname.Enabled = true;
        }
    }
    protected void ddlgname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgname.SelectedValue == "---Select---")
        {
            tbgname.Text = "";
            tbgname.Enabled = false;
        }
        else
        {
            tbgname.Enabled = true;
        }
    }
    //protected void ddlpemailid_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlpemailid.SelectedValue == "---Select---")
    //    {
    //        tbpemailid.Text = "";
    //        tbpemailid.Enabled = false;
    //    }
    //    else
    //    {
    //        tbpemailid.Enabled = true;
    //    }
    //}
    protected void ddlpcity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpcity.SelectedValue == "---Select---")
        {
            tbpcity.Text = "";
            tbpcity.Enabled = false;
        }
        else
        {
            tbpcity.Enabled = true;
        }
    }
    protected void ddlpdistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpdistrict.SelectedValue == "---Select---")
        {
            tbpdistrict.Text = "";
            tbpdistrict.Enabled = false;
        }
        else
        {
            tbpdistrict.Enabled = true;
        }
    }
    protected void ddlccountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlccountry.SelectedValue == "---Select---")
        {
            tbccountry.Text = "";
            tbccountry.Enabled = false;
        }
        else
        {
            tbccountry.Enabled = true;
        }
    }
    protected void ddlccity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlccity.SelectedValue == "---Select---")
        {
            tbccity.Text = "";
            tbccity.Enabled = false;
        }
        else
        {
            tbccity.Enabled = true;
        }
    }
    protected void ddlgcountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgcountry.SelectedValue == "---Select---")
        {
            tbgcountry.Text = "";
            tbgcountry.Enabled = false;
        }
        else
        {
            tbgcountry.Enabled = true;
        }
    }
    protected void ddlgstreet_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgstreet.SelectedValue == "---Select---")
        {
            tbgstreet.Text = "";
            tbgstreet.Enabled = false;
        }
        else
        {
            tbgstreet.Enabled = true;
        }
    }
    protected void ddlpstreet_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpstreet.SelectedValue == "---Select---")
        {
            tbpstreet.Text = "";
            tbpstreet.Enabled = false;
        }
        else
        {
            tbpstreet.Enabled = true;
        }
    }
    protected void ddlpcountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpcountry.SelectedValue == "---Select---")
        {
            tbpcountry.Text = "";
            tbpcountry.Enabled = false;
        }
        else
        {
            tbpcountry.Enabled = true;
        }
    }
    protected void ddlcdistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcdistrict.SelectedValue == "---Select---")
        {
            tbcdistrict.Text = "";
            tbcdistrict.Enabled = false;
        }
        else
        {
            tbcdistrict.Enabled = true;
        }
    }
    protected void ddlcstreet_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcstreet.SelectedValue == "---Select---")
        {
            tbcstreet.Text = "";
            tbcstreet.Enabled = false;
        }
        else
        {
            tbcstreet.Enabled = true;
        }
    }
    protected void ddlgdistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgdistrict.SelectedValue == "---Select---")
        {
            tbgdistrict.Text = "";
            tbgdistrict.Enabled = false;
        }
        else
        {
            tbgdistrict.Enabled = true;
        }
    }
    protected void ddlgcity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgcity.SelectedValue == "---Select---")
        {
            tbgcity.Text = "";
            tbgcity.Enabled = false;
        }
        else
        {
            tbgcity.Enabled = true;
        }
    }
    protected void tbfmobno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if ((Convert.ToInt64(tbfmobno.Text) < 7000000000))
            {
                lblfmobno.Visible = true;
            }
            else
            {
                lblfmobno.Visible = false;
            }
        }
        catch { }
    }
    protected void tbfoffno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if ((Convert.ToInt64(tbfoffno.Text) < 7000000000))
            {
                lblfoffno.Visible = true;
            }
            else
            {
                lblfoffno.Visible = false;
            }
        }
        catch { }
    }
    protected void tbmmobno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if ((Convert.ToInt64(tbmmobno.Text) < 7000000000))
            {
                lblmmobno.Visible = true;
            }
            else
            {
                lblmmobno.Visible = false;
            }
        }
        catch { }
    }
    protected void tbmoffno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if ((Convert.ToInt64(tbmoffno.Text) < 7000000000))
            {
                lblmoffno.Visible = true;
            }
            else
            {
                lblmoffno.Visible = false;
            }
        }
        catch { }
    }
    protected void tbstudmobno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if ((Convert.ToInt64(tbstudmobno.Text) < 7000000000))
            {
                lblsmobno.Visible = true;
            }
            else
            {
                lblsmobno.Visible = false;
            }
        }
        catch { }
    }
    //protected void tbfromdob_TextChanged(object sender, EventArgs e)
    //{
    //    if (tbfromdob.Text != "")
    //    {
    //        Labeldatedob.Text = "Enter to date";
    //        Labeldatedob.Visible = false;
    //    }
    //    else
    //        tbtodob_TextChanged(sender, e);
    //}
    //    protected void tbfromadmdt_TextChanged(object sender, EventArgs e)
    //{
    //    if (tbtoadmdt.Text != "")
    //    {
    //         Labeldatead.Text = "Enter to date";
    //        Labeldatead.Visible = false;
    //    }
    //    else
    //        tbtoadmdt_TextChanged(sender, e);
    //}
    //protected void tbfromappdt_TextChanged(object sender, EventArgs e)
    //{
    //    //if (tbfromappdt.Text != "")
    //    //    Labeldateap.Visible = false;
    //    if (tbtoappdt.Text == "")
    //    {
    //        Labeldateap.Text = "Enter to date";
    //        Labeldateap.Visible = true;
    //    }
    //    else
    //    {
    //        tbtoappdt_TextChanged(sender, e);
    //    }
    //}
    //protected void cblreference_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlothers.Items.Clear();
    //    ddlothers.Items.Add("---Select---");
    //    if (cblreference.Items[0].Selected == true && cblreference.Items[2].Selected==true)
    //    {
    //        cmd.CommandText = "select distinct refer_name from applyn where direct_refer=3 or direct_refer=2";
    //        cmd.Connection = con;
    //        con.Open();
    //        ddlothers.DataTextField = "refer_name";
    //        ddlothers.DataSource = cmd.ExecuteReader();
    //        ddlothers.DataBind();
    //        con.Close();
    //    }
    //    else if (cblreference.Items[0].Selected == true)
    //    {
    //        cmd.CommandText = "select distinct refer_name from applyn where direct_refer=3";
    //        cmd.Connection = con;
    //        con.Open();
    //        ddlothers.DataTextField = "refer_name";
    //        ddlothers.DataSource = cmd.ExecuteReader();
    //        ddlothers.DataBind();
    //        con.Close();
    //    }
    //    else if (cblreference.Items[2].Selected == true)
    //    {
    //        cmd.CommandText = "select distinct refer_name from applyn where direct_refer=2";
    //        cmd.Connection = con;
    //        con.Open();
    //        ddlothers.DataTextField = "refer_name";
    //        ddlothers.DataSource = cmd.ExecuteReader();
    //        ddlothers.DataBind();
    //        con.Close();
    //    }
    //    ddlstaffname.Items.Clear();
    //    ddlstaffname.Items.Add("---Select---");
    //    if (cblreference.Items[1].Selected == true)
    //    {
    //        cmd.CommandText = "select distinct refer_name from applyn where direct_refer=1";
    //        cmd.Connection = con;
    //        con.Open();
    //      ddlstaffname.DataTextField = "refer_name";
    //      ddlstaffname.DataSource = cmd.ExecuteReader();
    //      ddlstaffname.DataBind();
    //        con.Close();
    //    }
    //    ddlstaffname.Items.Insert(0, "---Select---");
    //    ddlothers.Items.Insert(0, "---Select---");
    //}
    //protected void rbstaffname_CheckedChanged(object sender, EventArgs e)
    //{
    //    ddlothers.Enabled = false;
    //    ddlstaffname.Enabled = true;
    //}
    //protected void rbothers_CheckedChanged(object sender, EventArgs e)
    //{
    //    ddlothers.Enabled = true;
    //    ddlstaffname.Enabled = false;
    //}
    protected void tbfname_TextChanged(object sender, EventArgs e)
    {
    }
    protected void rbdirect_CheckedChanged(object sender, EventArgs e)
    {
        cblstaffname.ClearSelection();
        PlaceHolderstaff.Controls.Clear();
        staffcnt = 0;
        tbstaffname.Text = "---Select---";
        LinkButtonstaff.Visible = false;
        //cblothers.ClearSelection();
        //PlaceHolderothers.Controls.Clear();
        otherscnt = 0;
        tbothers.Visible = false;
        tbothers.ClearSelection();
        ddlconsultant.ClearSelection();
        //tbothers.Text = "---Select---";
        //LinkButtonothers.Visible = false;
        tbdirect.Enabled = true;
        ddlconsultant.Enabled = false;
        tbstaffname.Enabled = false;
        cmd.CommandText = "select distinct refer_name from applyn where direct_refer=3";
        cmd.Connection = con;
        con.Open();
        cbldirect.DataTextField = "refer_name";
        cbldirect.DataSource = cmd.ExecuteReader();
        cbldirect.DataBind();
        //cbldirect.Items.Add("Select All");
        con.Close();
    }
    protected void rbstaffname_CheckedChanged(object sender, EventArgs e)
    {
        cbldirect.ClearSelection();
        PlaceHolderdirect.Controls.Clear();
        directcnt = 0;
        tbdirect.Text = "---Select---";
        LinkButtondirect.Visible = false;
        //cblothers.ClearSelection();
        //PlaceHolderothers.Controls.Clear();
        otherscnt = 0;
        //  tbothers.Text = "---Select---";
        //LinkButtonothers.Visible = false;
        cmd.CommandText = "select distinct refer_name from applyn where direct_refer=1";
        cmd.Connection = con;
        con.Open();
        cblstaffname.DataTextField = "refer_name";
        cblstaffname.DataSource = cmd.ExecuteReader();
        cblstaffname.DataBind();
        //cblstaffname.Items.Add("Select All");
        con.Close();
        tbstaffname.Enabled = true;
        ddlconsultant.Enabled = false;
        tbdirect.Enabled = false;
        tbothers.Visible = false;
        tbothers.ClearSelection();
        ddlconsultant.ClearSelection();
    }
    protected void rbothers_CheckedChanged(object sender, EventArgs e)
    {
        cbldirect.ClearSelection();
        PlaceHolderdirect.Controls.Clear();
        directcnt = 0;
        tbdirect.Text = "---Select---";
        LinkButtondirect.Visible = false;
        cblstaffname.ClearSelection();
        PlaceHolderstaff.Controls.Clear();
        staffcnt = 0;
        tbstaffname.Text = "---Select---";
        LinkButtonstaff.Visible = false;
        // ddlconsultant.Items.Add("---Select---");
        // tbothers.Visible = true;
        ddlconsultant.Enabled = true;
        tbdirect.Enabled = false;
        tbstaffname.Enabled = false;
    }
    protected void cbldirect_SelectedIndexChanged(object sender, EventArgs e)
    {
        int directcount = 0;
        string value = "";
        string code = "";
        LinkButtondirect.Visible = true;
        for (int i = 0; i < cbldirect.Items.Count; i++)
        {
            if (cbldirect.Items[i].Selected == true)
            {
                value = cbldirect.Items[i].Text;
                code = cbldirect.Items[i].Value.ToString();
                directcount = directcount + 1;
                tbdirect.Text = "Direct(" + directcount.ToString() + ")";
            }
        }
        if (directcount == 0)
        {
            tbdirect.Text = "---Select---";
        }
        else
        {
            Label lbl = directlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl14-" + code.ToString();
            ImageButton ib = directimage();
            ib.ID = "imgbut14_" + code.ToString();
            ib.Click += new ImageClickEventHandler(directimg_Click);
        }
        directcnt = directcount;
    }
    //        if (directcount == 0)
    //        {
    //            tbdirect.Text = "---Select---";
    //        }
    //        else
    //        {
    //            Label lbl = directlabel();
    //            lbl.Text = " " + value + " ";
    //            lbl.ID = "lbl14-" + code.ToString();
    //            ImageButton ib = directimage();
    //            ib.ID = "imgbut14_" + code.ToString();
    //            ib.Click += new ImageClickEventHandler(directimg_Click);
    //        }
    //        directcnt = directcount;
    //    }
    //}
    public ImageButton directimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderdirect.Controls.Add(imc);
        ViewState["idirectcontrol"] = true;
        return (imc);
    }
    public Label directlabel()
    {
        Label lbc = new Label();
        PlaceHolderdirect.Controls.Add(lbc);
        ViewState["ldirectcontrol"] = true;
        return (lbc);
    }
    public void directimg_Click(object sender, ImageClickEventArgs e)
    {
        directcnt = directcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbldirect.Items[r].Selected = false;
        tbdirect.Text = "Direct(" + directcnt.ToString() + ")";
        if (tbdirect.Text == "Direct(0)")
        {
            LinkButtondirect.Visible = false;
            tbdirect.Text = "---Select---";
        }
        int p = PlaceHolderdirect.Controls.IndexOf(b);
        PlaceHolderdirect.Controls.RemoveAt(p - 1);
        PlaceHolderdirect.Controls.Remove(b);
        //if (cbldirect.SelectedItem.Text=="Select All")
        //{
        //    for (int j = 0; j < cbldirect.Items.Count; j++)
        //    {
        //        cbldirect.Items[j].Selected = true;
        //    }
        //}
    }
    protected void cblstaffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        int staffcount = 0;
        string value = "";
        string code = "";
        LinkButtonstaff.Visible = true;
        for (int i = 0; i < cblstaffname.Items.Count; i++)
        {
            if (cblstaffname.Items[i].Selected == true)
            {
                value = cblstaffname.Items[i].Text;
                code = cblstaffname.Items[i].Value.ToString();
                staffcount = staffcount + 1;
                tbstaffname.Text = "Staffname(" + staffcount.ToString() + ")";
            }
        }
        if (staffcount == 0)
        {
            tbstaffname.Text = "---Select---";
        }
        else
        {
            Label lbl = stafflabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl15-" + code.ToString();
            ImageButton ib = staffimage();
            ib.ID = "imgbut15_" + code.ToString();
            ib.Click += new ImageClickEventHandler(staffimg_Click);
        }
        staffcnt = staffcount;
    }
    public ImageButton staffimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHolderstaff.Controls.Add(imc);
        ViewState["istaffcontrol"] = true;
        return (imc);
    }
    public Label stafflabel()
    {
        Label lbc = new Label();
        PlaceHolderstaff.Controls.Add(lbc);
        ViewState["lstaffcontrol"] = true;
        return (lbc);
    }
    public void staffimg_Click(object sender, ImageClickEventArgs e)
    {
        staffcnt = staffcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblstaffname.Items[r].Selected = false;
        tbstaffname.Text = "Staffname(" + staffcnt.ToString() + ")";
        if (tbstaffname.Text == "Staffname(0)")
        {
            LinkButtonstaff.Visible = false;
            tbstaffname.Text = "---Select---";
        }
        int p = PlaceHolderstaff.Controls.IndexOf(b);
        PlaceHolderstaff.Controls.RemoveAt(p - 1);
        PlaceHolderstaff.Controls.Remove(b);
        //if (cblstaffname.SelectedItem.Text == "Select All")
        //{
        //    for (int j = 0; j < cblstaffname.Items.Count; j++)
        //    {
        //        cblstaffname.Items[j].Selected = true;
        //    }
        //}
    }
    //protected void cblothers_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int otherscount = 0;
    //    string value = "";
    //    string code = "";
    //    LinkButtonothers.Visible = true;
    //    for (int i = 0; i < cblothers.Items.Count; i++)
    //    {
    //        if (cblothers.Items[i].Selected == true)
    //        {
    //            value = cblothers.Items[i].Text;
    //            code = cblothers.Items[i].Value.ToString();
    //            otherscount = otherscount + 1;
    //            tbothers.Text = "others(" + otherscount.ToString() + ")";
    //        }
    //    }
    //    if (otherscount == 0)
    //    {
    //        tbothers.Text = "---Select---";
    //    }
    //    else
    //    {
    //        Label lbl = otherslabel();
    //        lbl.Text = " " + value + " ";
    //        lbl.ID = "lbl16-" + code.ToString();
    //        ImageButton ib = othersimage();
    //        ib.ID = "imgbut16_" + code.ToString();
    //        ib.Click += new ImageClickEventHandler(othersimg_Click);
    //    }
    //    otherscnt = otherscount;
    //}
    //public ImageButton othersimage()
    //{
    //    ImageButton imc = new ImageButton();
    //    imc.ImageUrl = "xb.jpeg";
    //    imc.Height = 9;
    //    imc.Width = 9;
    //    PlaceHolderothers.Controls.Add(imc);
    //    ViewState["iotherscontrol"] = true;
    //    return (imc);
    //}
    //public Label otherslabel()
    //{
    //    Label lbc = new Label();
    //    PlaceHolderothers.Controls.Add(lbc);
    //    ViewState["lotherscontrol"] = true;
    //    return (lbc);
    //}
    //public void othersimg_Click(object sender, ImageClickEventArgs e)
    //{
    //    otherscnt = otherscnt - 1;
    //    ImageButton b = sender as ImageButton;
    //    int r = Convert.ToInt32(b.CommandArgument);
    //    cblothers.Items[r].Selected = false;
    //    tbothers.Text = "Others(" + otherscnt.ToString() + ")";
    //    if (tbothers.Text == "Others(0)")
    //    {
    //        LinkButtonothers.Visible = false;
    //        tbothers.Text = "---Select---";
    //    }
    //    int p = PlaceHolderothers.Controls.IndexOf(b);
    //    PlaceHolderothers.Controls.RemoveAt(p - 1);
    //    PlaceHolderothers.Controls.Remove(b);
    //if (cblothers.SelectedItem.Text == "Select All")
    //{
    //    for (int j = 0; j < cblothers.Items.Count; j++)
    //    {
    //        cblothers.Items[j].Selected = true;
    //    }
    //}
    //}
    protected void LinkButtondirect_Click(object sender, EventArgs e)
    {
        cbldirect.ClearSelection();
        PlaceHolderdirect.Controls.Clear();
        directcnt = 0;
        tbdirect.Text = "---Select---";
        LinkButtondirect.Visible = false;
    }
    protected void LinkButtonstaff_Click(object sender, EventArgs e)
    {
        cblstaffname.ClearSelection();
        PlaceHolderstaff.Controls.Clear();
        staffcnt = 0;
        tbstaffname.Text = "---Select---";
        LinkButtonstaff.Visible = false;
    }
    //protected void LinkButtonothers_Click(object sender, EventArgs e)
    //{
    //    cblothers.ClearSelection();
    //    PlaceHolderothers.Controls.Clear();
    //    otherscnt = 0;
    //    tbothers.Text = "---Select---";
    //    LinkButtonothers.Visible = false;
    //}
    protected void clear_Click(object sender, EventArgs e)
    {
        //ddlappno.SelectedIndex = 0;
        //tbappno.Text = "";
        //ddladmno.SelectedIndex = 0;
        //tbadmno.Text = "";
        //ddlstudname.SelectedIndex = 0;
        //tbfromadmdt.Text = "";
        //tbfromappdt.Text = "";
        //tbtoadmdt.Text = "";
        //tbtoappdt.Text = "";
        //tbstudname.Text = "";
        //ddlregno.SelectedIndex = 0;
        //ddlrollno.SelectedIndex = 0;
        //tbregno.Text = "";
        //tbrollno.Text = "";
        //// Button1.Visible = false;
        //Labeldatead.Visible = false;
        //Labeldateap.Visible = false;
        //tbfromdob.Text = "";
        //tbtodob.Text = "";
        //ddlfname.SelectedIndex = 0;
        //ddlmname.Text = "---Select---";
        //ddlgname.Text = "---Select---";
        //tbfname.Text = "";
        //tbmname.Text = "";
        //tbgname.Text = "";
        //tbseattype.Text = "---Select---";
        //tbblood.Text = "---Select---";
        //tbcaste.Text = "---Select---";
        //tbregion.Text = "---Select---";
        //tbreligion.Text = "---Select---";
        //tbcomm.Text = "---Select---";
        //tbfmobno.Text = "---Select---";
        //tbmmobno.Text = "---Select---";
        //tbfoccu.Text = "---Select---";
        //tbfoffno.Text = "---Select---";
        //tbfqual.Text = "---Select---";
        //tbmmobno.Text = "---Select---";
        //tbmoccu.Text = "---Select---";
        //tbmoffno.Text = "---Select---";
        //tbmqual.Text = "---Select---";
        //tbmtongue.Text = "---Select---";
        //tbstudmobno.Text = "---Select---";
        //PlaceHolderblood.Controls.Clear();
        //PlaceHoldercaste.Controls.Clear();
        //PlaceHoldercomm.Controls.Clear();
        //PlaceHolderfoccu.Controls.Clear();
        //PlaceHolderfqual.Controls.Clear();
        //PlaceHoldermoccu.Controls.Clear();
        //PlaceHoldermqual.Controls.Clear();
        //PlaceHoldermtongue.Controls.Clear();
        //PlaceHolderregion.Controls.Clear();
        //PlaceHolderreligion.Controls.Clear();
        //PlaceHolderseattype.Controls.Clear();
        //cblseattype.ClearSelection();
        //cblblood.ClearSelection();
        //cblcaste.ClearSelection();
        //cblregion.ClearSelection();
        //cblregion.ClearSelection();
        //cblcomm.ClearSelection();
        //cblmtongue.ClearSelection();
        //cblfoccu.ClearSelection();
        //cblfqual.ClearSelection();
        //cblmoccu.ClearSelection();
        //cblmqual.ClearSelection();
        //LinkButtonseattype.Visible = false;
        //LinkButtonblood.Visible = false;
        //LinkButtonreligion.Visible = false;
        //LinkButtonmtongue.Visible = false;
        //LinkButtoncaste.Visible = false;
        //LinkButtoncomm.Visible = false;
        //LinkButtonfoccu.Visible = false;
        //LinkButtonfqual.Visible = false;
        //LinkButtonmoccu.Visible = false;
        //LinkButtonmqual.Visible = false;
        //LinkButtonregion.Visible = false;
        //Labeldatedob.Visible = false;
        //// Button2.Visible = false;
        //lblsmobno.Visible = false;
        //lblmoffno.Visible = false;
        //lblmmobno.Visible = false;
        //lblfoffno.Visible = false;
        //lblfmobno.Visible = false;
        //ddlpemailid1.Text = "---Select---";
        //ddlpemailid1_SelectedIndexChanged(sender, e);
        //ddlpcity1.Text = "---Select---";
        //ddlpcity1_SelectedIndexChanged(sender, e);
        //ddlpcountry1.Text = "---Select---";
        //ddlpcountry1_SelectedIndexChanged(sender, e);
        //ddlpdistrict1.Text = "---Select---";
        //ddlpdistrict1_SelectedIndexChanged(sender, e);
        //ddlpstreet1.Text = "---Select---";
        //ddlpstreet1_SelectedIndexChanged(sender, e);
        //ddlccity1.Text = "---Select---";
        //ddlccity1_SelectedIndexChanged(sender, e);
        //ddlccountry.Text = "---Select---";
        //ddlccountry1_SelectedIndexChanged(sender, e);
        //ddlcdistrict1.Text = "---Select---";
        //ddlcdistrict1_SelectedIndexChanged(sender, e);
        //ddlgcity1.Text = "---Select---";
        //ddlgcity1_SelectedIndexChanged(sender, e);
        //ddlgcountry1.Text = "---Select---";
        //ddlgcountry1_SelectedIndexChanged(sender, e);
        //ddlgdistrict1.Text = "---Select---";
        //ddlgdistrict1_SelectedIndexChanged(sender, e);
        //ddlcstreet1.Text = "---Select---";
        //ddlcstreet1_SelectedIndexChanged(sender, e);
        //ddlgstreet1.Text = "---Select---";
        //ddlgstreet1_SelectedIndexChanged(sender, e);
        //ddlpstate1.Text = "---Select---";
        //ddlpstate1_SelectedIndexChanged(sender, e);
        //ddlcstate1.Text = "---Select---";
        //ddlcstate1_SelectedIndexChanged(sender, e);
        //ddlgstate1.Text = "---Select---";
        //ddlgstate1_SelectedIndexChanged(sender, e);
        //tbpcity.Visible = false;
        //// Button3.Visible = false;
        //ddlbatchyrfrm.Text = "---Select---";
        //ddlbatchyrto.Text = "---Select---";
        //tbdegree.Text = "";
        //tbbranch.Text = "";
        //tbsection.Text = "";
        //tbsem.Text = "";
        //PlaceHolderdegree.Controls.Clear();
        //PlaceHolderbranch.Controls.Clear();
        //PlaceHoldersection.Controls.Clear();
        //PlaceHoldersemester.Controls.Clear();
        //LinkButtonsection.Visible = false;
        //LinkButtondegree.Visible = false;
        //LinkButtonbranch.Visible = false;
        //LinkButtonsemester.Visible = false;
        //cbldegree.ClearSelection();
        //cblbranch.ClearSelection();
        //cblsection.ClearSelection();
        //cblsem.ClearSelection();
        //// Button4.Visible = false;
        Response.Redirect("About.aspx");
    }
    protected void ddlconsultant_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbothers.Visible = true;
        tbothers.Enabled = true;
        cmd.CommandText = "select distinct agent_name from stud_consultant where consul_name='" + ddlconsultant.SelectedItem.Text + "'";
        cmd.Connection = con;
        con.Open();
        tbothers.DataSource = cmd.ExecuteReader();
        tbothers.DataTextField = "agent_name";
        tbothers.DataBind();
        tbothers.Items.Add("All");
        tbothers.SelectedIndex = tbothers.Items.Count - 1;
        con.Close();
    }
    protected void tbothers_TextChanged(object sender, EventArgs e)
    {
    }
    protected void tbstaffname_TextChanged(object sender, EventArgs e)
    {
    }
    protected void tbothers_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void clearlist()
    {
        //ddlbatchyrfrm.Items.Clear();
        ddlbatchyrto.Items.Clear();
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        // clear_Click(sender, e);
        int colcount = 0;
        string value = "";
        string code = "";
        LinkButtoncol.Visible = true;
        for (int i = 0; i < ddlcollege.Items.Count; i++)
        {
            if (ddlcollege.Items[i].Selected == true)
            {
                value = ddlcollege.Items[i].Text;
                code = ddlcollege.Items[i].Value.ToString();
                colcount = colcount + 1;
                tbcollege.Text = lblcollege.Text + "(" + colcount.ToString() + ")";
            }
        }
        if (colcount == 0)
        {
            tbcollege.Text = "---Select---";
        }
        else
        {
            Label lbl = collabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1c4-" + code.ToString();
            ImageButton ib = colimage();
            ib.ID = "imgbut1c4_" + code.ToString();
            ib.Click += new ImageClickEventHandler(colimg_Click);
        }
        colcnt = colcount;
        loaddetails();
    }
    public ImageButton colimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        PlaceHoldercollege.Controls.Add(imc);
        ViewState["icolcontrol"] = true;
        return (imc);
    }
    public Label collabel()
    {
        Label lbc = new Label();
        PlaceHoldercollege.Controls.Add(lbc);
        ViewState["lcolcontrol"] = true;
        return (lbc);
    }
    public void colimg_Click(object sender, ImageClickEventArgs e)
    {
        colcnt = colcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        ddlcollege.Items[r].Selected = false;
        tbcollege.Text = lblcollege.Text + "(" + colcnt.ToString() + ")";
        if (tbcollege.Text == lblcollege.Text + "(0)")
        {
            LinkButtoncol.Visible = false;
            tbcollege.Text = "---Select---";
        }
        int p = PlaceHoldercollege.Controls.IndexOf(b);
        PlaceHoldercollege.Controls.RemoveAt(p - 1);
        PlaceHoldercollege.Controls.Remove(b);
    }
    protected void LinkButtoncol_Click(object sender, EventArgs e)
    {
        ddlcollege.ClearSelection();
        PlaceHoldercollege.Controls.Clear();
        colcnt = 0;
        tbcollege.Text = "---Select---";
        LinkButtoncol.Visible = false;
        tbcollege.Text = "---Select---";
        Accordion1.Visible = false;
        lblcol.Visible = true;
        lblcol.Text = "Select College and then proceed";
        btnsearch.Visible = false;
        clear.Visible = false;
        FpSpread1.Visible = false;
        Panelpage.Visible = false;
        cbcollege.Checked = false;
        //Added By Srinath 7/5/2013
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnexcel.Visible = false;
        Printcontrol.Visible = false;
        return;
    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con1.Close();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con1);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = con1;
        con1.Open();
        drnew = cmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }
    public void loaddetails()
    {
        int i = 0; string college = ""; int count = 0;
        for (i = 0; i < ddlcollege.Items.Count; i++)
        {
            if (ddlcollege.Items[i].Selected == true)
            {
                count = count + 1;
                if (college == "")
                {
                    college = ddlcollege.Items[i].Value.ToString();
                }
                else
                {
                    college = college + "," + ddlcollege.Items[i].Value.ToString();
                }
            }
        }
        college_code = "";
        if (college.Trim().ToString() != "")
        {
            college_code = " and college_code in(" + college + ") ";
        }
        if (count > 0)
        {
            tbcollege.Text = lblcollege.Text + "(" + count + ")";
            lblnorec.Visible = false;
            lblcol.Visible = false;
            Accordion1.Visible = true;
            btnsearch.Visible = true;
            clear.Visible = true;
            // FpSpread1.Visible = true;
            if (count == ddlcollege.Items.Count)
                cbcollege.Checked = true;
            else
                cbcollege.Checked = false;
        }
        else
        {
            tbcollege.Text = "---Select---";
            Accordion1.Visible = false;
            lblnorec.Visible = true;
            lblcol.Visible = false;
            lblcol.Text = "Select College and then proceed";
            btnsearch.Visible = false;
            clear.Visible = false;
            FpSpread1.Visible = false;
            //Added By Srinath 7/5/2013
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            Panelpage.Visible = false;
            btnexcel.Visible = false;
            Printcontrol.Visible = false;
            return;
        }
        //ddlconsultant.Items.Insert(0,new ListItem("---Select---","-1"));
        ddlpstate1.Items.Clear();
        ddlcstate1.Items.Clear();
        ddlgstate1.Items.Clear();
        ddlpstate1.Items.Add("---Select---");
        ddlcstate1.Items.Add("---Select---");
        ddlgstate1.Items.Add("---Select---");
        //Modified By Srinath 18/5/2013
        //cmd.CommandText = "select distinct TextVal,Textcode  from textvaltable where TextCriteria='state' and college_code in(" + college+")";
        cmd.CommandText = "select distinct TextVal,Textcode  from textvaltable where TextCriteria='state' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader drr = cmd.ExecuteReader();
        while (drr.Read())
        {
            //ddlpstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drr.GetValue(0).ToString()));//, drr.GetValue(0).ToString()));
            ddlcstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drr.GetValue(0).ToString(), drr.GetValue(1).ToString()));//, drr.GetValue(0).ToString()));
            //ddlgstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drr.GetValue(0).ToString()));//, drr.GetValue(0).ToString()));
            ddlpstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drr.GetValue(0).ToString(), drr.GetValue(1).ToString()));
            ddlgstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drr.GetValue(0).ToString(), drr.GetValue(1).ToString()));
        }
        drr.Close();
        ddlpstate1.Items.Add("Others");
        ddlgstate1.Items.Add("Others");
        ddlcstate1.Items.Add("Others");
        con.Close();
        ddlconsultant.Items.Clear();
        cmd.CommandText = "select distinct consul_name from stud_consultant where coll_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        ddlconsultant.DataSource = cmd.ExecuteReader();
        //ddlconsultant.Items.Insert(0, new ListItem("---Select---","-1"));
        ddlconsultant.DataTextField = "consul_name";
        ddlconsultant.DataBind();
        con.Close();
        ddlconsultant.Items.Add("All");
        ddlconsultant.SelectedIndex = ddlconsultant.Items.Count - 1;
        ddllaststudied.Items.Clear();
        cmd.CommandText = "select distinct case institute_name when '-1' then ' ' else institute_name end as ins from stud_prev_details where institute_name!='NULL'";
        cmd.Connection = con;
        con.Open();
        ddllaststudied.DataSource = cmd.ExecuteReader();
        ddllaststudied.DataTextField = "ins";
        ddllaststudied.DataBind();
        con.Close();
        ddllaststudied.Items.Add("All");
        ddllaststudied.SelectedIndex = ddllaststudied.Items.Count - 1;
        //pothers.Visible = false;
        tbstaffname.Enabled = false;
        tbothers.Enabled = false;
        tbothers.Visible = false;
        ddlconsultant.Enabled = false;
        cblseattype.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='seat' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblseattype.DataSource = cmd.ExecuteReader();
        cblseattype.DataTextField = "TextVal";
        //cblseattype.DataValueField = "TextCode";
        cblseattype.DataBind();
        con.Close();
        cblreligion.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='relig' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblreligion.DataSource = cmd.ExecuteReader();
        cblreligion.DataTextField = "TextVal";
        //cblregion.DataValueField = "TextCode";
        cblreligion.DataBind();
        con.Close();
        cblcomm.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='comm' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblcomm.DataSource = cmd.ExecuteReader();
        cblcomm.DataTextField = "TextVal";
        // cblcomm.DataValueField = "TextCode";
        cblcomm.DataBind();
        con.Close();
        cblcaste.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='caste' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblcaste.DataSource = cmd.ExecuteReader();
        cblcaste.DataTextField = "TextVal";
        // cblcaste.DataValueField = "TextCode";
        cblcaste.DataBind();
        con.Close();
        cblblood.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='bgrou' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblblood.DataSource = cmd.ExecuteReader();
        cblblood.DataTextField = "TextVal";
        // cblblood.DataValueField = "TextCode";
        cblblood.DataBind();
        con.Close();
        cblmtongue.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='mton' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblmtongue.DataSource = cmd.ExecuteReader();
        cblmtongue.DataTextField = "TextVal";
        // cblmtongue.DataValueField = "TextCode";
        cblmtongue.DataBind();
        con.Close();
        cblregion.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='Reg' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblregion.DataSource = cmd.ExecuteReader();
        cblregion.DataTextField = "TextVal";
        // cblregion.DataValueField = "TextCode";
        cblregion.DataBind();
        con.Close();
        cblfqual.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='fqual' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblfqual.DataSource = cmd.ExecuteReader();
        cblfqual.DataTextField = "TextVal";
        // cblfqual.DataValueField = "TextCode";
        cblfqual.DataBind();
        con.Close();
        ddlbatchyrfrm.Items.Clear();
        ddlbatchyrfrm.Items.Add("---Select---");
        cmd.CommandText = "select distinct Batch_Year from registration where college_code in(" + college + ") order by Batch_Year";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr23 = cmd.ExecuteReader();
        while (dr23.Read())
        {
            ddlbatchyrfrm.Items.Add(dr23.GetValue(0).ToString());
        }
        dr23.Close();
        con.Close();
        ddlbatchyrto.Items.Clear();
        ddlbatchyrto.Items.Add("---Select---");
        cmd.CommandText = "select distinct Batch_Year from registration where college_code in(" + college + ") order by Batch_Year";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr22 = cmd.ExecuteReader();
        while (dr22.Read())
        {
            ddlbatchyrto.Items.Add(dr22.GetValue(0).ToString());
        }
        dr22.Close();
        con.Close();
        cblfoccu.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='foccu' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblfoccu.DataSource = cmd.ExecuteReader();
        cblfoccu.DataTextField = "TextVal";
        // cblfoccu.DataValueField = "TextCode";
        cblfoccu.DataBind();
        con.Close();
        cblmqual.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='mqual' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblmqual.DataSource = cmd.ExecuteReader();
        cblmqual.DataTextField = "TextVal";
        // cblmqual.DataValueField = "TextCode";
        cblmqual.DataBind();
        con.Close();
        cblmoccu.Items.Clear();
        cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='moccu' and textval<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblmoccu.DataSource = cmd.ExecuteReader();
        cblmoccu.DataTextField = "TextVal";
        // cblmoccu.DataValueField = "TextCode";
        cblmoccu.DataBind();
        con.Close();
        cbldegree.Items.Clear();
        cmd.CommandText = "select distinct course.Course_Name from course where college_code in(" + college + ") order by Course_Name";
        cmd.Connection = con;
        con.Open();
        cbldegree.DataSource = cmd.ExecuteReader();
        cbldegree.DataTextField = "Course_Name";
        // cbldegree.DataValueField = "course_id";
        cbldegree.DataBind();
        con.Close();
        cblbranch.Items.Clear();
        cmd.CommandText = "select distinct department.Dept_Acronym from department where college_code in(" + college + ") order by dept_acronym";
        cmd.Connection = con;
        con.Open();
        cblbranch.DataSource = cmd.ExecuteReader();
        cblbranch.DataTextField = "Dept_Acronym";
        // cblbranch.DataValueField = "Dept_code";
        cblbranch.DataBind();
        con.Close();
        //ddlothers.Items.Add("---Select---");
        //ddlstaffname.Items.Add("---Select---");
        //sem
        cblsem.Items.Clear();
        cmd.CommandText = " select distinct current_semester from registration where college_code in(" + college + ") order by current_semester asc";
        cmd.Connection = con;
        con.Open();
        cblsem.DataSource = cmd.ExecuteReader();
        cblsem.DataTextField = "current_semester";
        cblsem.DataBind();
        con.Close();
        //sec
        cblsection.Items.Clear();
        cmd.CommandText = " select distinct sections from registration where college_code in(" + college + ") and sections<>'-1' and sections<>'' order by sections asc";
        cmd.Connection = con;
        con.Open();
        cblsection.DataSource = cmd.ExecuteReader();
        cblsection.DataTextField = "sections";
        cblsection.DataBind();
        con.Close();
        if (cblsection.Items.Count > 5)
        {
            psection.Height = 300;
        }
        ddlpemailid1.Items.Clear();
        ddlpemailid1.Items.Add("---Select---");
        cmd.CommandText = "select distinct StuPer_Id from applyn where StuPer_Id!='' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            ddlpemailid1.Items.Add(dr.GetValue(0).ToString());
        }
        dr.Close();
        //ddlpemailid1.Items.Add("Others");
        con.Close();
        ddlpdistrict1.Items.Clear();
        //cmd.CommandText = "select distinct textcode,textval from textvaltable where textcriteria='Dis' and textval<>'' and college_code in(" + college + ")";
        //cmd.Connection = con;
        //con.Open();
        //SqlDataReader dr1 = cmd.ExecuteReader();
        //if (dr1.HasRows ==true )
        //{
        //        ddlpdistrict1.DataSource =dr1;
        //        ddlpdistrict1.DataTextField = "textval";
        //        ddlpdistrict1.DataValueField = "textcode";
        //        ddlpdistrict1.DataBind();
        //}
        //dr1.Close();
        ddlpdistrict1.Items.Clear();
        string strdisquery = "select distinct textcode,textval from textvaltable where textcriteria='Dis' and college_code in(" + college + ")";
        DataSet dsdis = new DataSet();
        dsdis.Reset(); dsdis.Dispose();
        dsdis = da.select_method_wo_parameter(strdisquery, "Text");
        if (dsdis.Tables[0].Rows.Count > 0)
        {
            for (int dis = 0; dis < dsdis.Tables[0].Rows.Count; dis++)
            {
                ddlpdistrict1.Items.Insert(dis, new System.Web.UI.WebControls.ListItem(dsdis.Tables[0].Rows[dis]["textval"].ToString(), dsdis.Tables[0].Rows[dis]["textcode"].ToString()));
            }
            //ddlpdistrict1.DataSource = dsdis;
            //ddlpdistrict1.DataTextField = "textval";
            //ddlpdistrict1.DataValueField = "textcode";
            //ddlpdistrict1.DataBind();
        }
        ddlpdistrict1.Items.Add("Others");
        ddlpdistrict1.Items.Add("---Select---");
        ddlpdistrict1.SelectedIndex = ddlpdistrict1.Items.Count - 1;
        // con.Close();
        ddlpcity1.Items.Clear();
        ddlpcity1.Items.Add("---Select---");
        //cmd.CommandText = "select distinct Cityp from applyn where Cityp!='' and college_code in(" + college + ")";//17.01.18 barath
        cmd.CommandText = "select distinct CASE WHEN ISNUMERIC(Cityp) = 1 THEN (SELECT TextVal FROM TextValTable M WHERE convert(varchar(200),M.TextCode)  = convert(varchar(200),A.Cityp)) ELSE Cityp end City,a.Cityp from applyn a where isnull(Cityp,'')<>'' and a.college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr2 = cmd.ExecuteReader();
        while (dr2.Read())
        {
            //ddlpcity1.Items.Add(dr2.GetValue(0).ToString());
            ddlpcity1.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(dr2.GetValue(0)), Convert.ToString(dr2.GetValue(1))));
        }
        dr2.Close();
        ddlpcity1.Items.Add("Others");
        con.Close();
        ddlpcountry1.Items.Clear();
        ddlpcountry1.Items.Add("---Select---");
        //cmd.CommandText = "select distinct Countryp from applyn where Countryp!='' and college_code in(" + college + ")";
        cmd.CommandText = "select distinct CASE WHEN ISNUMERIC(Countryc) = 1 THEN (SELECT TextVal FROM TextValTable M WHERE convert(varchar(200),M.TextCode)  = convert(varchar(200),A.Countryc)) ELSE Countryc end Country,a.Countryc from applyn a where (isnull(Countryc,'')<>'' and Countryc<>'0') and a.college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr3 = cmd.ExecuteReader();
        while (dr3.Read())
        {
            ddlpcountry1.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(dr3.GetValue(0)), Convert.ToString(dr3.GetValue(1))));
            //ddlpcountry1.Items.Add(dr3.GetValue(0).ToString());
        }
        dr3.Close();
        ddlpcountry1.Items.Add("Others");
        con.Close();
        ddlpstreet1.Items.Clear();
        ddlpstreet1.Items.Add("---Select---");
        cmd.CommandText = "select distinct Streetp from applyn where Streetp!='' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr4 = cmd.ExecuteReader();
        while (dr4.Read())
        {
            ddlpstreet1.Items.Add(dr4.GetValue(0).ToString());
        }
        dr4.Close();
        ddlpstreet1.Items.Add("Others");
        con.Close();
        ddlcdistrict1.Items.Clear();
        ddlcdistrict1.Items.Add("---Select---");
        strdisquery = "select distinct textcode,textval from textvaltable where textcriteria='Dis' and college_code in(" + college + ")";
        dsdis.Dispose();
        dsdis.Reset();
        dsdis = da.select_method_wo_parameter(strdisquery, "Text");
        if (dsdis.Tables[0].Rows.Count > 0)
        {
            for (int dis = 0; dis < dsdis.Tables[0].Rows.Count; dis++)
            {
                ddlcdistrict1.Items.Insert(dis, new System.Web.UI.WebControls.ListItem(dsdis.Tables[0].Rows[dis]["textval"].ToString(), dsdis.Tables[0].Rows[dis]["textcode"].ToString()));
            }
            //ddlpdistrict1.DataSource = dsdis;
            //ddlpdistrict1.DataTextField = "textval";
            //ddlpdistrict1.DataValueField = "textcode";
            //ddlpdistrict1.DataBind();
        }
        //cmd.CommandText = "select distinct textcode,textval from textvaltable where textcriteria='Dis' and college_code in(" + college + ")";
        //cmd.Connection = con;
        //con.Open();
        //SqlDataReader dr5 = cmd.ExecuteReader();
        //if (dr5.HasRows == true)
        //{
        //    ddlcdistrict1.DataSource = dr5;
        //    ddlcdistrict1.DataTextField = "textval";
        //    ddlcdistrict1.DataValueField = "textcode";
        //    ddlcdistrict1.DataBind();
        //}
        //dr5.Close();
        ddlcdistrict1.Items.Add("Others");
        ddlcdistrict1.Items.Add("---Select---");
        ddlcdistrict1.SelectedIndex = ddlcdistrict1.Items.Count - 1;
        con.Close();
        ddlccity1.Items.Clear();
        ddlccity1.Items.Add("---Select---");
        //cmd.CommandText = "select distinct Cityc from applyn where Cityc!='' and college_code in(" + college + ")";//17.01.18 barath
        cmd.CommandText = "select distinct CASE WHEN ISNUMERIC(Cityc) = 1 THEN (SELECT TextVal FROM TextValTable M WHERE convert(varchar(200),M.TextCode)  = convert(varchar(200),A.Cityc)) ELSE Cityc end City,a.Cityc from applyn a where isnull(Cityc,'')<>'' and a.college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr6 = cmd.ExecuteReader();
        while (dr6.Read())
        {
            ddlccity1.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(dr6.GetValue(0)), Convert.ToString(dr6.GetValue(1))));
            //ddlccity1.Items.Add(dr6.GetValue(0).ToString());
        }
        dr6.Close();
        ddlccity1.Items.Add("Others");
        con.Close();
        ddlccountry1.Items.Clear();
        ddlccountry1.Items.Add("---Select---");
        // cmd.CommandText = "select distinct Countryc from applyn where Countryc!='' and college_code in(" + college + ")";//17.01.18 barath
        cmd.CommandText = "select distinct CASE WHEN ISNUMERIC(Countryc) = 1 THEN (SELECT TextVal FROM TextValTable M WHERE convert(varchar(200),M.TextCode)  = convert(varchar(200),A.Countryc)) ELSE Countryc end Country,a.Countryc from applyn a where (isnull(Countryc,'')<>'' and Countryc<>'0') and a.college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr7 = cmd.ExecuteReader();
        while (dr7.Read())
        {
            ddlccountry1.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(dr7.GetValue(0)), Convert.ToString(dr7.GetValue(1))));
            //ddlccountry1.Items.Add(dr7.GetValue(0).ToString());
        }
        dr7.Close();
        ddlccountry1.Items.Add("Others");
        con.Close();
        ddlcstreet1.Items.Clear();
        ddlcstreet1.Items.Add("---Select---");
        cmd.CommandText = "select distinct Streetc from applyn where Streetc!='' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr8 = cmd.ExecuteReader();
        while (dr8.Read())
        {
            ddlcstreet1.Items.Add(dr8.GetValue(0).ToString());
        }
        dr8.Close();
        ddlcstreet1.Items.Add("Others");
        con.Close();
        ddlgdistrict1.Items.Clear();
        ddlgdistrict1.Items.Add("---Select---");
        strdisquery = "select distinct textcode,textval from textvaltable where textcriteria='Dis' and college_code in(" + college + ")";
        dsdis.Dispose();
        dsdis.Reset();
        dsdis = da.select_method_wo_parameter(strdisquery, "Text");
        if (dsdis.Tables[0].Rows.Count > 0)
        {
            for (int dis = 0; dis < dsdis.Tables[0].Rows.Count; dis++)
            {
                ddlgdistrict1.Items.Insert(dis, new System.Web.UI.WebControls.ListItem(dsdis.Tables[0].Rows[dis]["textval"].ToString(), dsdis.Tables[0].Rows[dis]["textcode"].ToString()));
            }
        }
        //cmd.CommandText = "select distinct textcode,textval from textvaltable where textcriteria='Dis' and college_code in(" + college + ")";
        //cmd.Connection = con;
        //con.Open();
        //SqlDataReader dr9 = cmd.ExecuteReader();
        //if (dr9.HasRows == true)
        //{
        //    ddlgdistrict1.DataSource = dr9;
        //    ddlgdistrict1.DataTextField = "textval";
        //    ddlgdistrict1.DataValueField = "textcode";
        //    ddlgdistrict1.DataBind();
        //}
        //dr9.Close();
        ddlgdistrict1.Items.Add("Others");
        ddlgdistrict1.Items.Add("---Select---");
        ddlgdistrict1.SelectedIndex = ddlgdistrict1.Items.Count - 1;
        con.Close();
        ddlgcity1.Items.Clear();
        ddlgcity1.Items.Add("---Select---");
        cmd.CommandText = "select distinct Cityg from applyn where Cityg!='' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr10 = cmd.ExecuteReader();
        while (dr10.Read())
        {
            ddlgcity1.Items.Add(dr10.GetValue(0).ToString());
        }
        dr10.Close();
        ddlgcity1.Items.Add("Others");
        con.Close();
        ddlgcountry1.Items.Clear();
        ddlgcountry1.Items.Add("---Select---");
        cmd.CommandText = "select distinct Countryg from applyn where Countryg!='' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr11 = cmd.ExecuteReader();
        while (dr11.Read())
        {
            ddlgcountry1.Items.Add(dr11.GetValue(0).ToString());
        }
        dr11.Close();
        ddlgcountry1.Items.Add("Others");
        con.Close();
        ddlgstreet1.Items.Clear();
        ddlgstreet1.Items.Add("---Select---");
        cmd.CommandText = "select distinct Streetg from applyn where Streetg!='' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr12 = cmd.ExecuteReader();
        while (dr12.Read())
        {
            ddlgstreet1.Items.Add(dr12.GetValue(0).ToString());
        }
        dr12.Close();
        ddlgstreet1.Items.Add("Others");
        con.Close();
        //start aruna 19july2013=======================================================
        drp_padress.Items.Clear();
        drp_padress.Items.Add("---Select---");
        cmd.CommandText = "select distinct parent_addressp from applyn where (parent_addressp!='' and parent_addressp is not null) and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr_1 = cmd.ExecuteReader();
        while (dr_1.Read())
        {
            drp_padress.Items.Add(dr_1.GetValue(0).ToString());
        }
        dr_1.Close();
        drp_padress.Items.Add("Others");
        con.Close();
        drp_cadress.Items.Clear();
        drp_cadress.Items.Add("---Select---");
        cmd.CommandText = "select distinct parent_addressc from applyn where (parent_addressc!='' and parent_addressc is not null) and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr_2 = cmd.ExecuteReader();
        while (dr_2.Read())
        {
            drp_cadress.Items.Add(dr_2.GetValue(0).ToString());
        }
        dr_2.Close();
        drp_cadress.Items.Add("Others");
        con.Close();
        drp_gadress.Items.Clear();
        drp_gadress.Items.Add("---Select---");
        cmd.CommandText = "select distinct addressg from applyn where (addressg!='' and addressg is not null) and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr_3 = cmd.ExecuteReader();
        while (dr_3.Read())
        {
            drp_gadress.Items.Add(dr_3.GetValue(0).ToString());
        }
        dr_2.Close();
        drp_gadress.Items.Add("Others");
        con.Close();
        drp_ppincode.Items.Clear();
        drp_ppincode.Items.Add("---Select---");
        cmd.CommandText = "select distinct parent_pincodep from applyn where (parent_pincodep!='' and parent_pincodep is not null) and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr_4 = cmd.ExecuteReader();
        while (dr_4.Read())
        {
            drp_ppincode.Items.Add(dr_4.GetValue(0).ToString());
        }
        dr_4.Close();
        drp_ppincode.Items.Add("Others");
        con.Close();
        drp_cpincode.Items.Clear();
        drp_cpincode.Items.Add("---Select---");
        cmd.CommandText = "select distinct parent_pincodec from applyn where (parent_pincodec!='' and parent_pincodec is not null) and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr_5 = cmd.ExecuteReader();
        while (dr_5.Read())
        {
            drp_cpincode.Items.Add(dr_5.GetValue(0).ToString());
        }
        dr_5.Close();
        drp_cpincode.Items.Add("Others");
        con.Close();
        drp_gpincode.Items.Clear();
        drp_gpincode.Items.Add("---Select---");
        cmd.CommandText = "select distinct ping from applyn where (ping is not null) and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr_6 = cmd.ExecuteReader();
        while (dr_6.Read())
        {
            drp_gpincode.Items.Add(dr_6.GetValue(0).ToString());
        }
        dr_6.Close();
        drp_gpincode.Items.Add("Others");
        con.Close();
        //=============================================================================
    }
    protected void ddlcstate1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcstate1.SelectedValue == "Others")
        {
            ddlcstate.Visible = true;
            tbstatec.Visible = true;
        }
        else
        {
            ddlcstate.Visible = false;
            tbstatec.Visible = false;
        }
    }
    protected void ddlpstate1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpstate1.SelectedValue == "Others")
        {
            ddlpstate.Visible = true;
            tbstatep.Visible = true;
        }
        else
        {
            ddlpstate.Visible = false;
            tbstatep.Visible = false;
        }
    }
    protected void ddlgstate1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgstate1.SelectedValue == "Others")
        {
            ddlgstate.Visible = true;
            tbstateg.Visible = true;
        }
        else
        {
            ddlgstate.Visible = false;
            tbstateg.Visible = false;
        }
    }
    protected void ddlcstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcstate.SelectedValue == "---Select---")
        {
            tbstatec.Text = "";
            tbstatec.Enabled = false;
        }
        else
        {
            tbstatec.Enabled = true;
        }
    }
    protected void ddlpstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpstate.SelectedValue == "---Select---")
        {
            tbstatep.Text = "";
            tbstatep.Enabled = false;
        }
        else
        {
            tbstatep.Enabled = true;
        }
    }
    protected void ddlgstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgstate.SelectedValue == "---Select---")
        {
            tbstateg.Text = "";
            tbstateg.Enabled = false;
        }
        else
        {
            tbstateg.Enabled = true;
        }
    }
    protected void cbcollege_CheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        if (cbcollege.Checked == true)
        {
            for (int i = 0; i < ddlcollege.Items.Count; i++)
            {
                string si = Convert.ToString(i);
                ddlcollege.Items[i].Selected = true;
                count = count + 1;
            }
            tbcollege.Text = lblcollege.Text + "(" + count.ToString() + ")";
            Accordion1.Visible = true;
            btnsearch.Visible = true;
            clear.Visible = true;
            lblcol.Visible = false;
        }
        else
        {
            ddlcollege.ClearSelection();
            tbcollege.Text = "---Select---";
            lblcol.Visible = true;
            lblcol.Text = "Select College and then proceed";
            Accordion1.Visible = false;
            btnsearch.Visible = false;
            clear.Visible = false;
            FpSpread1.Visible = false;
            Panelpage.Visible = false;
            //Added By Srinath 7/5/2013
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnexcel.Visible = false;
            Printcontrol.Visible = false;
            return;
        }
    }
    public string Encrypt(string message)
    {
        UTF8Encoding textConverter = new UTF8Encoding();
        RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
        //Convert the data to a byte array.
        byte[] toEncrypt = textConverter.GetBytes(message);
        //Get an encryptor.
        ICryptoTransform encryptor = rc2CSP.CreateEncryptor(ScrambleKey, ScrambleIV);
        //Encrypt the data.
        MemoryStream msEncrypt = new MemoryStream();
        CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        //Write all data to the crypto stream and flush it.
        // Encode length as first 4 bytes
        byte[] length = new byte[4];
        length[0] = (byte)(message.Length & 0xFF);
        length[1] = (byte)((message.Length >> 8) & 0xFF);
        length[2] = (byte)((message.Length >> 16) & 0xFF);
        length[3] = (byte)((message.Length >> 24) & 0xFF);
        csEncrypt.Write(length, 0, 4);
        csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
        csEncrypt.FlushFinalBlock();
        //Get encrypted array of bytes.
        byte[] encrypted = msEncrypt.ToArray();
        // Convert to Base64 string
        string b64 = Convert.ToBase64String(encrypted);
        // Protect against URLEncode/Decode problem
        string b64mod = b64.Replace('+', '@');
        // Return a URL encoded string
        return HttpUtility.UrlEncode(b64mod);
    }
    public string Decrypt(string scrambledMessage)
    {
        UTF8Encoding textConverter = new UTF8Encoding();
        RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
        // URL decode , replace and convert from Base64
        string b64mod = HttpUtility.UrlDecode(scrambledMessage);
        // Replace '@' back to '+' (avoid URLDecode problem)
        string b64 = b64mod.Replace('@', '+');
        // Base64 decode
        byte[] encrypted = Convert.FromBase64String(b64);
        //Get a decryptor that uses the same key and IV as the encryptor.
        ICryptoTransform decryptor = rc2CSP.CreateDecryptor(ScrambleKey, ScrambleIV);
        //Now decrypt the previously encrypted message using the decryptor
        // obtained in the above step.
        MemoryStream msDecrypt = new MemoryStream(encrypted);
        CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        byte[] fromEncrypt = new byte[encrypted.Length - 4];
        //Read the data out of the crypto stream.
        byte[] length = new byte[4];
        csDecrypt.Read(length, 0, 4);
        csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
        int len = (int)length[0] | (length[1] << 8) | (length[2] << 16) | (length[3] << 24);
        //Convert the byte array back into a string.
        return textConverter.GetString(fromEncrypt).Substring(0, len);
    }
    public byte[] ScrambleKey
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                // Use existing key if non provided
                key = ScrambleKey;
            }
            Session["ScrambleKey"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleKey"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateKey();
                key = rc2.Key;
                Session["ScrambleKey"] = key;
            }
            return key;
        }
    }
    // Initialization vector management for scrambling support
    public byte[] ScrambleIV
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                key = ScrambleIV;
            }
            Session["ScrambleIV"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleIV"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateIV();
                key = rc2.IV;
                Session["ScrambleIV"] = key;
            }
            return key;
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        string degreedetails = "Universal Report";
        string pagename = "About.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        string reportname = txtexcelname.Text.ToString().Trim();
        if (reportname != "")
        {
            da.printexcelreport(FpSpread1, reportname);
        }
    }
    protected void drp_padress_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_padress.SelectedValue == "Others")
        {
            drp_padress1.Visible = true;
            txt_padress.Visible = true;
        }
        else
        {
            drp_padress1.Visible = false;
            txt_padress.Visible = false;
        }
    }
    protected void drp_cadress_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_cadress.SelectedValue == "Others")
        {
            drp_cadress1.Visible = true;
            txt_cadress.Visible = true;
        }
        else
        {
            drp_cadress1.Visible = false;
            txt_cadress.Visible = false;
        }
    }
    protected void drp_gadress_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_gadress.SelectedValue == "Others")
        {
            drp_gadress1.Visible = true;
            txt_gadress.Visible = true;
        }
        else
        {
            drp_gadress1.Visible = false;
            txt_gadress.Visible = false;
        }
    }
    protected void drp_padress1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_padress1.SelectedValue == "---Select---")
        {
            txt_padress.Text = "";
            txt_padress.Enabled = false;
        }
        else
        {
            txt_padress.Enabled = true;
        }
    }
    protected void drp_cadress1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_cadress1.SelectedValue == "---Select---")
        {
            txt_cadress.Text = "";
            txt_cadress.Enabled = false;
        }
        else
        {
            txt_cadress.Enabled = true;
        }
    }
    protected void drp_gadress1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_gadress1.SelectedValue == "---Select---")
        {
            txt_gadress.Text = "";
            txt_gadress.Enabled = false;
        }
        else
        {
            txt_gadress.Enabled = true;
        }
    }
    protected void drp_ppincode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_ppincode.SelectedValue == "Others")
        {
            drp_ppincode1.Visible = true;
            txt_ppincode.Visible = true;
        }
        else
        {
            drp_ppincode1.Visible = false;
            txt_ppincode.Visible = false;
        }
    }
    protected void drp_cpincode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_cpincode.SelectedValue == "Others")
        {
            drp_cpincode1.Visible = true;
            txt_cpincode.Visible = true;
        }
        else
        {
            drp_cpincode1.Visible = false;
            txt_cpincode.Visible = false;
        }
    }
    protected void drp_gpincode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_gpincode.SelectedValue == "Others")
        {
            drp_gpincode1.Visible = true;
            txt_gpincode.Visible = true;
        }
        else
        {
            drp_gpincode1.Visible = false;
            txt_gpincode.Visible = false;
        }
    }
    protected void drp_ppincode1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_ppincode1.SelectedValue == "---Select---")
        {
            txt_ppincode.Text = "";
            txt_ppincode.Enabled = false;
        }
        else
        {
            txt_ppincode.Enabled = true;
        }
    }
    protected void drp_cpincode1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_cpincode1.SelectedValue == "---Select---")
        {
            txt_cpincode.Text = "";
            txt_cpincode.Enabled = false;
        }
        else
        {
            txt_cpincode.Enabled = true;
        }
    }
    protected void drp_gpincode1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_gpincode1.SelectedValue == "---Select---")
        {
            txt_gpincode.Text = "";
            txt_gpincode.Enabled = false;
        }
        else
        {
            txt_gpincode.Enabled = true;
        }
    }
    #region roll,reg,adm no settings
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = da.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
    }
    #endregion


    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lblcollege);
        lbl.Add(lbl_stuDegree);
        //lbl.Add(lbl_branchT);
        lbl.Add(lbl_stuSemOrT);

        fields.Add(0);
        fields.Add(2);
        // fields.Add(3);
        fields.Add(4);
        //  lbl.Add(lbl_semT);
        // fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}