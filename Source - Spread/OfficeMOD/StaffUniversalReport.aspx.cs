using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

public partial class StaffUniversalReport : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection tcon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DAccess2 d2 = new DAccess2();
    int j = 0;
    int bloodcount;
    Boolean Cellclick = false;
    int bc;
    string search1;
    string appno;
    int roll = 0;
    string[] castevalue = new string[200];
    string[] castecode = new string[200];
    int[] casteindex = new int[200];
    string[] seatvalue = new string[200];
    string[] seatcode = new string[200];
    int[] seatindex = new int[200];
    string[] directvalue = new string[200];
    string[] directcode = new string[200];
    int[] directindex = new int[200];
    string[] bloodvalue = new string[200];
    string[] bloodcode = new string[200];
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
    DAccess2 da = new DAccess2();


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
        //string PageLogOut = "";
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
        if (IsPostBack)
        {

            college_code = "";
        }

        if (!IsPostBack)
        {


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
                        //string st = splitorder[temp].ToString();
                        //int s = Convert.ToInt32(st);
                        //cblsearch.Items[s].Selected = true;
                        //Itemindex.Add(st);
                        //ItemList.Add(cblsearch.Items[s].Text);
                    }
                }
            }
            string colleges;
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

                    LinkButtoncol.Visible = false;

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
            //lblhostel.Font.Bold = true;
            //lblhostel.Font.Name = "Book Antiqua";
            //lblhostel.Font.Size = FontUnit.Medium;
            //ddlhosday.Font.Bold = true;
            //ddlhosday.Font.Name = "Book Antiqua";
            //ddlhosday.Font.Size = FontUnit.Medium;
            //ddlconsultant.Font.Bold = true;
            //ddlconsultant.Font.Name = "Book Antiqua";
            //ddlconsultant.Font.Size = FontUnit.Medium;
            //tbothers.Font.Bold = true;
            //tbothers.Font.Name = "Book Antiqua";
            //tbothers.Font.Size = FontUnit.Medium;
            //lbllaststudied.Font.Bold = true;
            //lbllaststudied.Font.Name = "Book Antiqua";
            //lbllaststudied.Font.Size = FontUnit.Medium;
            //ddllaststudied.Font.Bold = true;
            //ddllaststudied.Font.Name = "Book Antiqua";
            //ddllaststudied.Font.Size = FontUnit.Medium;

            if (cook == 0)
            {
                // cblsearch.ClearSelection();
                Itemindex.Clear();
                ItemList.Clear();
                if (Request.Cookies["order"] != null)
                {
                    string temporder = (Request.Cookies["order"].Value);
                    string[] splitorder = temporder.Split(new char[] { ',' });
                    //for (int temp = 0; temp < splitorder.Length; temp++)
                    //{
                    //    string st = splitorder[temp].ToString();
                    //    int s = Convert.ToInt32(st);
                    //    cblsearch.Items[s].Selected = true;
                    //    Itemindex.Add(st);
                    //    ItemList.Add(cblsearch.Items[s].Text);
                    //}
                }
            }
            cook = 1;

            //ddladmno.Items.Add("---Select---");
            //ddladmno.Items.Add("Like");
            //ddladmno.Items.Add("Starts with");
            //ddladmno.Items.Add("Ends with");
            //ddladmno.Items.Add("Equal");
            //ddladmno.Items.Add("Not Equal");
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
            ddlstaffname.Items.Add("---Select---");
            ddlstaffname.Items.Add("Like");
            ddlstaffname.Items.Add("Starts with");
            ddlstaffname.Items.Add("Ends with");

            ddldeptname.Items.Add("---Select---");
            ddldeptname.Items.Add("Like");
            ddldeptname.Items.Add("Starts with");
            ddldeptname.Items.Add("Ends with");
            ddl_desig.Items.Add("---Select---");
            ddl_desig.Items.Add("Like");
            ddl_desig.Items.Add("Starts with");
            ddl_desig.Items.Add("Ends with");
            ddlfname.Items.Add("---Select---");
            ddlfname.Items.Add("Like");
            ddlfname.Items.Add("Starts with");
            ddlfname.Items.Add("Ends with");

            ddl_maritalstatus.Items.Add("---Select---");
            ddl_maritalstatus.Items.Add("Like");
            ddl_maritalstatus.Items.Add("Starts with");
            ddl_maritalstatus.Items.Add("Ends with");
            ddlpdistrict.Items.Add("---Select---");
            ddlpdistrict.Items.Add("Like");
            ddlpdistrict.Items.Add("Starts with");
            ddlpdistrict.Items.Add("Ends with");
            ddlpcity.Items.Add("---Select---");
            ddlpcity.Items.Add("Like");
            ddlpcity.Items.Add("Starts with");
            ddlpcity.Items.Add("Ends with");

            ddlpstate.Items.Add("---Select---");
            ddlpstate.Items.Add("Like");
            ddlpstate.Items.Add("Starts with");
            ddlpstate.Items.Add("Ends with");
            ddlcdistrict.Items.Add("---Select---");
            ddlcdistrict.Items.Add("Like");
            ddlcdistrict.Items.Add("Starts with");
            ddlcdistrict.Items.Add("Ends with");
            ddlcstate.Items.Add("---Select---");
            ddlcstate.Items.Add("Like");
            ddlcstate.Items.Add("Starts with");
            ddlcstate.Items.Add("Ends with");
            drp_cpincode1.Items.Add("---Select---");
            drp_cpincode1.Items.Add("Like");
            drp_cpincode1.Items.Add("Starts with");
            drp_cpincode1.Items.Add("Ends with");

            ddlyearofexp.Items.Add("---Select---");
            ddlyearofexp.Items.Add("Like");
            ddlyearofexp.Items.Add("Starts with");
            ddlyearofexp.Items.Add("Ends with");
            ddlyearofexp.Items.Add("Equal");
            ddlyearofexp.Items.Add("Not Equal");
            ddlyearofexp.Items.Add("Greater than");
            ddlyearofexp.Items.Add("Greater than or equal to");
            ddlyearofexp.Items.Add("Lesser than");
            ddlyearofexp.Items.Add("Lesser than or equal to");




            //End==============================================================
            //ddlcollege_SelectedIndexChanged(sender, e);
            loaddetails();
            FpSpread1.Visible = false;

            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnexcel.Visible = false;
            Printcontrol.Visible = false;


            //  btnprintmaster.Visible = false;
            // txtexcelname.Visible = false;
            // lblrptname.Visible = false;
            // btnexcel.Visible = false;
            // Printcontrol.Visible = false;
            //RollAndRegSettings();
        }
    }
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
        //lbl.Add(lbl_stuDegree);
        //lbl.Add(lbl_branchT);
        //   lbl.Add(lbl_stuSemOrT);

        fields.Add(0);
        fields.Add(2);

        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
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

        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnexcel.Visible = false;
        Printcontrol.Visible = false;

        return;
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
            //  FpSpread1.Visible = false;
            //  Panelpage.Visible = false;

            //  btnprintmaster.Visible = false;
            //  txtexcelname.Visible = false;
            //  lblrptname.Visible = false;
            //  btnexcel.Visible = false;
            //  Printcontrol.Visible = false;
            return;
        }
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
    public Label collabel()
    {
        Label lbc = new Label();
        PlaceHoldercollege.Controls.Add(lbc);
        ViewState["lcolcontrol"] = true;
        return (lbc);
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
            FpSpread1.Visible = true;
            if (count == ddlcollege.Items.Count)
                cbcollege.Checked = true;
            else
                cbcollege.Checked = false;
        }
        else
        {
            tbcollege.Text = "---Select---";
            Accordion1.Visible = false;
            //lblnorec.Visible = true;
            lblcol.Visible = false;
            lblcol.Text = "Select College and then proceed";
            btnsearch.Visible = false;
            clear.Visible = false;
            FpSpread1.Visible = false;

            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            Panelpage.Visible = false;
            btnexcel.Visible = false;
            Printcontrol.Visible = false;
            return;
        }

        cblcomm.Items.Clear();
        // cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='comm' and textval<>'' and college_code in(" + college + ")";
        cmd.CommandText = "select distinct community from staff_appl_master where  community<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblcomm.DataSource = cmd.ExecuteReader();
        cblcomm.DataTextField = "community";

        cblcomm.DataBind();
        con.Close();

        cblreligion.Items.Clear();
        //  cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='relig' and textval<>'' and college_code in(" + college + ")";
        cmd.CommandText = " select distinct religion from staff_appl_master where religion<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblreligion.DataSource = cmd.ExecuteReader();
        cblreligion.DataTextField = "religion";

        cblreligion.DataBind();
        con.Close();
        cblcaste.Items.Clear();
        // cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='caste' and textval<>'' and college_code in(" + college + ")";
        cmd.CommandText = "select distinct Caste from staff_appl_master where Caste<>'' and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblcaste.DataSource = cmd.ExecuteReader();
        cblcaste.DataTextField = "Caste";

        cblcaste.DataBind();
        con.Close();
        cblblood.Items.Clear();
        //  cmd.CommandText = "select distinct TextVal  from textvaltable where TextCriteria='bgrou' and textval<>'' and college_code in(" + college + ")";
        cmd.CommandText = " select distinct bldgrp from staff_appl_master where bldgrp<>''and college_code in(" + college + ")";
        cmd.Connection = con;
        con.Open();
        cblblood.DataSource = cmd.ExecuteReader();
        cblblood.DataTextField = "bldgrp";

        cblblood.DataBind();
        con.Close();

        ddlpemailid1.Items.Clear();
        ddlpemailid1.Items.Add("---Select---");
        cmd.CommandText = "select distinct email from staff_appl_master where college_code in(" + college + ")";
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

        ddlpcity1.Items.Clear();
        ddlpcity1.Items.Add("---Select---");

        //   cmd.CommandText = "select distinct CASE WHEN ISNUMERIC(pcity) = 1 THEN (SELECT TextVal FROM TextValTable M WHERE convert(varchar(200),M.TextCode)  = convert(varchar(200),A.pcity)) ELSE pcity end City,a.pcity from staff_appl_master a where isnull(pcity,'')<>'' and a.college_code in(" + college + ")";

        cmd.CommandText = "select distinct pcity from staff_appl_master where college_code in('" + college + "') and pcity<>''";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr2 = cmd.ExecuteReader();
        while (dr2.Read())
        {
            //  ddlpemailid1.Items.Add(dr.GetValue(0).ToString());

            ddlpcity1.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(dr2.GetValue(0))));
        }
        dr2.Close();
        ddlpcity1.Items.Add("Others");

        ddlpdistrict1.Items.Clear();
        //  string strdisquery = "select distinct textcode,textval from textvaltable where textcriteria='Dis' and college_code in(" + college + ")";
        string strdisquery = "select distinct pdistrict from staff_appl_master where college_code in(" + college + ") and pdistrict<>''";

        DataSet dsdis = new DataSet();
        dsdis.Reset(); dsdis.Dispose();
        dsdis = da.select_method_wo_parameter(strdisquery, "Text");
        if (dsdis.Tables[0].Rows.Count > 0)
        {
            for (int dis = 0; dis < dsdis.Tables[0].Rows.Count; dis++)
            {
                ddlpdistrict1.Items.Insert(dis, new System.Web.UI.WebControls.ListItem(dsdis.Tables[0].Rows[dis]["pdistrict"].ToString()));
            }

        }
        ddlpdistrict1.Items.Add("Others");
        ddlpdistrict1.Items.Add("---Select---");
        ddlpdistrict1.SelectedIndex = ddlpdistrict1.Items.Count - 1;
        con.Close();

        ddlccity1.Items.Clear();
        ddlccity1.Items.Add("---Select---");

        //  cmd.CommandText = "select distinct CASE WHEN ISNUMERIC(ccity) = 1 THEN (SELECT TextVal FROM TextValTable M WHERE convert(varchar(200),M.TextCode)  = convert(varchar(200),A.ccity)) ELSE ccity end City,a.ccity from staff_appl_master a where isnull(ccity,'')<>'' and a.college_code in(" + college + ")";
        cmd.CommandText = "select distinct ccity from staff_appl_master where college_code in('" + college + "') and ccity<>''";
        cmd.Connection = con;
        con.Open();
        SqlDataReader dr6 = cmd.ExecuteReader();
        while (dr6.Read())
        {
            ddlccity1.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(dr6.GetValue(0))));
            //ddlccity1.Items.Add(dr6.GetValue(0).ToString());
        }
        dr6.Close();
        ddlccity1.Items.Add("Others");
        con.Close();


        ddlpstate1.Items.Clear();
        ddlcstate1.Items.Clear();

        ddlpstate1.Items.Add("---Select---");
        ddlcstate1.Items.Add("---Select---");


        //  cmd.CommandText = "select distinct TextVal,Textcode  from textvaltable where TextCriteria='state' and college_code in(" + college + ")";
        cmd.CommandText = "select distinct pstate from staff_appl_master where college_code in(" + college + ") and pstate<>''";
        cmd.Connection = con;
        con.Open();
        SqlDataReader drp = cmd.ExecuteReader();
        while (drp.Read())
        {

            //ddlcstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drr.GetValue(1).ToString()));

            ddlpstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drp.GetValue(0).ToString()));
            // ddlgstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drr.GetValue(0).ToString(), drr.GetValue(1).ToString()));
        }
        drp.Close();
        ddlpstate1.Items.Add("Others");
        con.Close();



        cmd.CommandText = "select distinct cstate from staff_appl_master where college_code in(" + college + ") and cstate<>''";
        cmd.Connection = con;
        con.Open();
        SqlDataReader drr = cmd.ExecuteReader();
        while (drr.Read())
        {

            ddlcstate1.Items.Add(new System.Web.UI.WebControls.ListItem(drr.GetValue(0).ToString()));


        }
        drr.Close();
        // ddlgstate1.Items.Add("Others");
        ddlcstate1.Items.Add("Others");
        con.Close();

        ddlcdistrict1.Items.Clear();
        ddlcdistrict1.Items.Add("---Select---");
        // strdisquery = "select distinct textcode,textval from textvaltable where textcriteria='Dis' and college_code in(" + college + ")";
        strdisquery = "select distinct cdistrict from staff_appl_master where college_code in(" + college + ") and cdistrict<>''";
        dsdis.Dispose();
        dsdis.Reset();
        dsdis = da.select_method_wo_parameter(strdisquery, "Text");
        if (dsdis.Tables[0].Rows.Count > 0)
        {
            for (int dis = 0; dis < dsdis.Tables[0].Rows.Count; dis++)
            {
                ddlcdistrict1.Items.Insert(dis, new System.Web.UI.WebControls.ListItem(dsdis.Tables[0].Rows[dis]["cdistrict"].ToString()));
            }

        }

        //con.Open();

        ddlcdistrict1.Items.Add("Others");
        ddlcdistrict1.Items.Add("---Select---");
        ddlcdistrict1.SelectedIndex = ddlcdistrict1.Items.Count - 1;
        con.Close();

        drp_cpincode.Items.Clear();
        drp_cpincode.Items.Add("---Select---");
        cmd.CommandText = "select distinct com_pincode from staff_appl_master where (com_pincode!='' and com_pincode is not null) and college_code in(" + college + ")";
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
    protected void ddlyearofexp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlyearofexp.SelectedValue == "---Select---")
        {
            txtyearofexp.Text = "";
            txtyearofexp.Enabled = false;
        }
        else
        {
            txtyearofexp.Enabled = true;
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

    protected void LinkButtonblood_Click(object sender, EventArgs e)
    {
        cblblood.ClearSelection();
        PlaceHolderblood.Controls.Clear();
        bloodcnt = 0;
        tbblood.Text = "---Select---";
        LinkButtonblood.Visible = false;
    }
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
    public Label bloodlabel()
    {
        Label lbc = new Label();
        PlaceHolderblood.Controls.Add(lbc);
        ViewState["lbloodcontrol"] = true;
        return (lbc);
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

    protected void LinkButtoncaste_Click(object sender, EventArgs e)
    {
        cblcaste.ClearSelection();
        PlaceHoldercaste.Controls.Clear();
        castecnt = 0;
        tbcaste.Text = "---Select---";
        LinkButtoncaste.Visible = false;
    }

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
    public Label castelabel()
    {
        Label lbc = new Label();
        PlaceHoldercaste.Controls.Add(lbc);
        ViewState["lcastecontrol"] = true;
        return (lbc);
    }
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
    protected void ddlstudname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlstaffname.SelectedValue == "---Select---")
        {
            tbstaffname.Text = "";
            tbstaffname.Enabled = false;
        }
        else
        {
            tbstaffname.Enabled = true;
        }
    }

    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldeptname.SelectedValue == "---Select---")
        {
            txtdept.Text = "";
            txtdept.Enabled = false;
        }
        else
        {
            txtdept.Enabled = true;
        }
    }


    protected void ddl_desig_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_desig.SelectedValue == "---Select---")
        {
            txt_desig.Text = "";
            txt_desig.Enabled = false;
        }
        else
        {
            txt_desig.Enabled = true;
        }
    }

    protected void tbfromappdt_TextChanged(object sender, EventArgs e)
    {

        if (tbtoappdt.Text == "")
        {
            Labeldateap.Text = "Enter to date";
            Labeldateap.Visible = true;
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
    protected void LinkButtoncomm_Click(object sender, EventArgs e)
    {
        cblcomm.ClearSelection();
        PlaceHoldercomm.Controls.Clear();
        commcnt = 0;
        tbcomm.Text = "---Select---";
        LinkButtoncomm.Visible = false;
    }
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
    protected void ddl_maritalstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_maritalstatus.SelectedValue == "---Select---")
        {
            txt_maritalstatus.Text = "";
            txt_maritalstatus.Enabled = false;
        }
        else
        {
            txt_maritalstatus.Enabled = true;
        }
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

    public Label commlabel()
    {
        Label lbc = new Label();
        PlaceHoldercomm.Controls.Add(lbc);
        ViewState["lcommcontrol"] = true;
        return (lbc);
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

    protected void tbfromdob_TextChanged(object sender, EventArgs e)
    {
        if (tbfromdob.Text != "")
        {
            Labeldatedob.Text = "Enter to date";
            Labeldatedob.Visible = true;
        }
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

    protected void LinkButtonreligion_Click(object sender, EventArgs e)
    {
        cblreligion.ClearSelection();
        PlaceHolderreligion.Controls.Clear();
        religcnt = 0;
        tbreligion.Text = "---Select---";
        LinkButtonreligion.Visible = false;
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
    protected void ddlpemailid1_SelectedIndexChanged(object sender, EventArgs e)
    {

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

    protected void FpSpread1_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
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
                    Response.Redirect("../IndStaffUniversalReport.aspx?app=" + Encrypt(appno) + "&Type=Admin");
                    //Response.Write(appno);
                }
                Cellclick = false;
            }
        }
        catch (Exception ex)
        {

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
        string degreedetails = "Staff Universal Report";
        string pagename = "StaffUniversalReport.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread1.Sheets[0].AutoPostBack = true;
        Cellclick = true;
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
    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "   Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }
    protected void btnsearch_Click(object sender, EventArgs e)//delsiref17
    {
        try
        {
            string orderStr = string.Empty;
            orderStr = " Order by a.appl_no";

            FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
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

            selitem5 = ddlappno.SelectedItem.Text;
            selitem3 = ddlstaffname.SelectedItem.Text;
            selitem4 = ddldeptname.SelectedItem.Text;
            selitem1 = ddl_desig.SelectedItem.Text;
            selitem6 = ddlfname.SelectedItem.Text;
            selitem2 = ddl_maritalstatus.SelectedItem.Text;
            selitem7 = ddlyearofexp.SelectedItem.Text;


            if (txtyearofexp.Text != "" && selitem7 != "---Select---")
            {
                if (selitem7 == "Like")
                {
                    selection7 = "like '%" + txtyearofexp.Text + "%'";
                }
                else if (selitem7 == "Starts with")
                {
                    selection7 = "like '" + txtyearofexp.Text + "%'";
                }
                else if (selitem7 == "Ends with")
                {
                    selection7 = "like '%" + txtyearofexp.Text + "'";
                }
                else if (selitem7 == "Equal")
                {
                    selection7 = "='" + txtyearofexp.Text + "'";
                }
                else if (selitem7 == "Not Equal")
                {
                    selection7 = "!='" + txtyearofexp.Text + "'";
                }
                else if (selitem7 == "Greater than")
                {
                    selection7 = ">'" + txtyearofexp.Text + "'";
                }
                else if (selitem7 == "Greater than or equal to")
                {
                    selection7 = ">='" + txtyearofexp.Text + "'";
                }
                else if (selitem7 == "Lesser than")
                {
                    selection7 = "<'" + txtyearofexp.Text + "'";
                }
                else if (selitem7 == "Lesser than or equal to")
                {
                    selection7 = "<='" + txtyearofexp.Text + "'";
                }
                sel7 = " and a.yofexp " + selection7 + "";
                if (!Itemindex.Contains("21"))
                {
                    ItemList.Add("year of Experience");
                    Itemindex.Add("21");
                }
                cblsearch.Items[21].Selected = true;

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
                sel5 = " and a.appl_no " + selection5 + "";
                if (!Itemindex.Contains("0"))
                {
                    ItemList.Add("Application No");
                    Itemindex.Add("0");
                }
                cblsearch.Items[0].Selected = true;

            }
            if (tbstaffname.Text != "" && selitem3 != "---Select---")
            {
                if (selitem3 == "Like")
                {
                    selection3 = " like '%" + tbstaffname.Text + "%'";
                }
                else if (selitem3 == "Starts with")
                {
                    selection3 = " like '" + tbstaffname.Text + "%'";
                }
                else if (selitem3 == "Ends with")
                {
                    selection3 = " like '%" + tbstaffname.Text + "'";
                }
                sel3 = " and a.appl_name" + selection3 + "";
                if (!Itemindex.Contains("2"))
                {
                    ItemList.Add("Staff Name");
                    Itemindex.Add("1");
                }
                //selection = selection + ",r.Stud_Name";
                //columncount = columncount + 1;
                //ItemList.Add("Student Name");
                cblsearch.Items[1].Selected = true;
            }

            //dept
            if (txtdept.Text != "" && selitem4 != "---Select---")
            {
                if (selitem4 == "Like")
                {
                    selection4 = " like '%" + txtdept.Text + "%'";
                }
                else if (selitem4 == "Starts with")
                {
                    selection4 = " like '" + txtdept.Text + "%'";
                }
                else if (selitem4 == "Ends with")
                {
                    selection4 = " like '%" + txtdept.Text + "'";
                }
                sel4 = " and a.dept_name" + selection4 + "";
                if (!Itemindex.Contains("2"))
                {
                    ItemList.Add("Department Name");
                    Itemindex.Add("2");
                }
                //selection = selection + ",r.Stud_Name";
                //columncount = columncount + 1;
                //ItemList.Add("Student Name");
                cblsearch.Items[2].Selected = true;
            }
            //desig

            if (txt_desig.Text != "" && selitem1 != "---Select---")
            {
                if (selitem1 == "Like")
                {
                    selection1 = " like '%" + txt_desig.Text + "%'";
                }
                else if (selitem1 == "Starts with")
                {
                    selection1 = " like '" + txt_desig.Text + "%'";
                }
                else if (selitem1 == "Ends with")
                {
                    selection1 = " like '%" + txt_desig.Text + "'";
                }
                sel1 = " and a.desig_name" + selection1 + "";
                if (!Itemindex.Contains("3"))
                {
                    ItemList.Add("Designation Name");
                    Itemindex.Add("3");
                }
                //selection = selection + ",r.Stud_Name";
                //columncount = columncount + 1;
                //ItemList.Add("Student Name");
                cblsearch.Items[3].Selected = true;
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
                sel9 = " and a.father_name " + selection6 + "";
                selection = selection + ",a.father_name";
                columncount = columncount + 1;
                if (!Itemindex.Contains("7"))
                {
                    ItemList.Add("Father Name");
                    Itemindex.Add("7");
                }
                cblsearch.Items[7].Selected = true;
                //cblsearch.Items[9].Selected = true;
            }

            //Maritalstatus
            if (txt_maritalstatus.Text != "" && selitem2 != "---Select---")
            {
                if (selitem2 == "Like")
                {
                    selection2 = "like '%" + txt_maritalstatus.Text + "%'";
                }
                else if (selitem2 == "Starts with")
                {
                    selection2 = "like '" + txt_maritalstatus.Text + "%'";
                }
                else if (selitem2 == "Ends with")
                {
                    selection2 = "like '%" + txt_maritalstatus.Text + "'";
                }
                sel2 = " and a.martial_status " + selection2 + "";
                selection = selection + ",a.martial_status";
                columncount = columncount + 1;
                if (!Itemindex.Contains("6"))
                {
                    ItemList.Add("Marital Status");
                    Itemindex.Add("6");
                }
                cblsearch.Items[6].Selected = true;
                //cblsearch.Items[9].Selected = true;
            }


            if (tbfmobno.Text != "")
            {
                sel36 = " and a.per_mobileno ='" + tbfmobno.Text + "' ";
                selection = selection + ",a.per_mobileno";
                columncount = columncount + 1;
                if (!Itemindex.Contains("9"))
                {
                    ItemList.Add("Staff Mobile No");
                    Itemindex.Add("9");
                }
                cblsearch.Items[9].Selected = true;
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
                sel13 = " and a.Caste in(" + caste + ")";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.Caste and textcriteria='caste'  " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("10"))
                {
                    ItemList.Add("Caste");
                    Itemindex.Add("10");
                }
                cblsearch.Items[10].Selected = true;
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
                sel14 = " and a.religion in(" + religion + ")";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.religion and textcriteria='relig' " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("11"))
                {
                    ItemList.Add("Religion");
                    Itemindex.Add("11");
                }
                cblsearch.Items[11].Selected = true;
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
                sel16 = " and a.Community  in(" + community + ") ";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.Community and textcriteria='comm' " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("12"))
                {
                    ItemList.Add("Community");
                    Itemindex.Add("12");
                }
                cblsearch.Items[12].Selected = true;
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
                        sel6 = " and a.date_of_birth between '" + dtfromdob + "' and '" + datetodob + "' ";
                        if (!Itemindex.Contains("5"))
                        {
                            ItemList.Add("Date of birth");
                            Itemindex.Add("5");
                        }
                        cblsearch.Items[5].Selected = true;
                        //selection = selection + ",CONVERT(VARCHAR(10),a.dob,103)";
                        //columncount = columncount + 1;
                        //ItemList.Add("Date of Birth");
                    }
                }
            }

            if (ddlpemailid1.SelectedValue == "---Select---")
            {
                sel39 = "";
            }
            else
            {
                sel39 = " and a.email='" + ddlpemailid1.SelectedItem.Text.ToString() + "'";
                selection = selection + ",a.email";
                columncount = columncount + 1;
                if (!Itemindex.Contains("13"))
                {
                    ItemList.Add("Email Id");
                    Itemindex.Add("13");
                }
                cblsearch.Items[13].Selected = true;
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
                    sel25 = " and a.pcity " + selection11 + "";
                    selection = selection + ",a.pcity";
                    columncount = columncount + 1;
                    if (!Itemindex.Contains("14"))
                    {
                        ItemList.Add("Permanent City");
                        Itemindex.Add("14");
                    }
                    cblsearch.Items[14].Selected = true;
                }
            }
            else
            {
                sel25 = " and a.pcity='" + ddlpcity1.SelectedValue + "' ";
                selection = selection + ",a.pcity";
                columncount = columncount + 1;
                if (!Itemindex.Contains("14"))
                {
                    ItemList.Add("Permanent City");
                    Itemindex.Add("14");
                }
                cblsearch.Items[14].Selected = true;
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
                    sel24 = " and a.pdistrict " + selection10 + "";
                    selection = selection + ",a.pdistrict";
                    columncount = columncount + 1;
                    if (!Itemindex.Contains("15"))
                    {
                        ItemList.Add("Permanent District");
                        Itemindex.Add("15");
                    }
                    cblsearch.Items[15].Selected = true;
                }
            }
            else
            {
                sel24 = " and a.pdistrict in('" + ddlpdistrict1.SelectedItem.ToString() + "')";
                selection = selection + ",a.pdistrict";
                columncount = columncount + 1;
                if (!Itemindex.Contains("15"))
                {
                    ItemList.Add("Permanent District");
                    Itemindex.Add("15");
                }
                cblsearch.Items[15].Selected = true;
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
                        sel_p = " and a.pstate in(" + state + ")";

                    selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.pstate and textcriteria='state'  " + college_code + ")";
                    columncount = columncount + 1;
                    if (!Itemindex.Contains("16"))
                    {
                        ItemList.Add("Permanent state");
                        Itemindex.Add("16");
                    }
                    cblsearch.Items[16].Selected = true;
                }
            }
            else
            {
                sel_p = " and a.pstate in ('" + ddlpstate1.SelectedItem.ToString() + "')";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.pstate and textcriteria='state'  " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("16"))
                {
                    ItemList.Add("Permanent State");
                    Itemindex.Add("16");
                }
                cblsearch.Items[16].Selected = true;
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
                    sel29 = " and a.ccity " + selection15 + "";
                    selection = selection + ",a.ccity";
                    columncount = columncount + 1;
                    if (!Itemindex.Contains("17"))
                    {
                        ItemList.Add("Contact City");
                        Itemindex.Add("17");
                    }
                    cblsearch.Items[17].Selected = true;
                }
            }
            else
            {
                sel29 = " and a.ccity ='" + ddlccity1.SelectedValue + "' ";
                selection = selection + ",a.ccity";
                columncount = columncount + 1;
                if (!Itemindex.Contains("17"))
                {
                    ItemList.Add("Contact City");
                    Itemindex.Add("17");
                }
                cblsearch.Items[17].Selected = true;
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
                sel41 = " and a.bldgrp in(" + blood + ")";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.bldgrp and textcriteria='bgrou' " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("8"))
                {
                    ItemList.Add("Blood Group");
                    Itemindex.Add("8");
                }
                cblsearch.Items[8].Selected = true;
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
                    sel28 = " and a.cdistrict " + selection14 + "";
                    selection = selection + ",a.cdistrict";
                    columncount = columncount + 1;
                    if (!Itemindex.Contains("18"))
                    {
                        ItemList.Add("Contact District");
                        Itemindex.Add("18");
                    }
                    cblsearch.Items[18].Selected = true;
                }
            }
            else
            {
                //sel28 = " and a.Districtc ='" + ddlcdistrict1.SelectedValue + "' ";
                sel28 = " and a.cdistrict in ('" + ddlcdistrict1.SelectedItem.ToString() + "')";
                selection = selection + ",a.cdistrict";
                columncount = columncount + 1;
                if (!Itemindex.Contains("18"))
                {
                    ItemList.Add("Contact District");
                    Itemindex.Add("18");
                }
                cblsearch.Items[18].Selected = true;
            }

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
                        sel_c = " and a.cstate in(" + state + ")";
                    //(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.bldgrp and textcriteria='bgrou')";
                    selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.cstate and textcriteria='state'  " + college_code + ")";
                    columncount = columncount + 1;
                    if (!Itemindex.Contains("19"))
                    {
                        ItemList.Add("Contact state");
                        Itemindex.Add("19");
                    }
                    cblsearch.Items[19].Selected = true;
                }
            }
            else
            {

                sel_c = " and a.cstate in ('" + ddlcstate1.SelectedItem.ToString() + "')";
                selection = selection + ",(select case textval when '-1' then ' ' else textval end from textvaltable  where textcode=a.cstate and textcriteria='state'  " + college_code + ")";
                columncount = columncount + 1;
                if (!Itemindex.Contains("19"))
                {
                    ItemList.Add("Contact State");
                    Itemindex.Add("19");
                }
                cblsearch.Items[19].Selected = true;
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
                    sel52 = " and a.com_pincode " + selection26 + "";
                    selection = selection + ",a.com_pincode";
                    columncount = columncount + 1;
                    if (!Itemindex.Contains("20"))
                    {
                        ItemList.Add("Contact Pincode");
                        Itemindex.Add("20");
                    }
                    cblsearch.Items[20].Selected = true;
                }
            }
            else
            {
                sel52 = " and a.com_pincode ='" + drp_cpincode.SelectedValue + "' ";
                selection = selection + ",a.com_pincode";
                columncount = columncount + 1;
                if (!Itemindex.Contains("20"))
                {
                    ItemList.Add("Contact Pincode");
                    Itemindex.Add("20");
                }
                cblsearch.Items[20].Selected = true;
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
                        sel7 = " and a.dateofapply between '" + dtfromap + "' and '" + datetoap + "' ";
                        if (!Itemindex.Contains("4"))
                        {
                            ItemList.Add("Applied Date");
                            Itemindex.Add("4");
                        }
                        cblsearch.Items[4].Selected = true;

                    }
                }
            }

            cblsearch.ClearSelection();
            for (int it = 0; it < Itemindex.Count; it++)
            {
                string t = Itemindex[it].ToString();
                int te = Convert.ToInt32(t);
                cblsearch.Items[te].Selected = true;
            }
            string[] search = new string[100];//delsiref
            if (cblsearch.Items[0].Selected == true)
            {
                search[0] = "a.appl_no";
            }
            if (cblsearch.Items[1].Selected == true)
            {
                search[1] = "appl_name";
            }
            if (cblsearch.Items[2].Selected == true)
            {
                search[2] = "a.dept_name";
            }
            if (cblsearch.Items[3].Selected == true)
            {
                search[3] = "a.desig_name";
            }
            if (cblsearch.Items[4].Selected == true)
            {
                search[4] = "a.dateofapply";
            }

            if (cblsearch.Items[5].Selected == true)
            {
                search[5] = "a.date_of_birth";
            }
            if (cblsearch.Items[6].Selected == true)
            {
                search[6] = "a.martial_status";
            }
            if (cblsearch.Items[7].Selected == true)
            {
                search[7] = "a.father_name";
            }
            if (cblsearch.Items[8].Selected == true)
            {
                search[8] = "a.bldgrp";
            }
            if (cblsearch.Items[9].Selected == true)
            {
                search[9] = "a.per_mobileno";
            }
            if (cblsearch.Items[10].Selected == true)
            {
                search[10] = "a.Caste";
            }

            if (cblsearch.Items[11].Selected == true)
            {
                search[11] = "a.religion";
            }

            if (cblsearch.Items[12].Selected == true)
            {
                search[12] = "a.community";
            }
            if (cblsearch.Items[13].Selected == true)
            {
                search[13] = "a.email";
            }
            if (cblsearch.Items[14].Selected == true)
            {
                search[14] = "a.pcity";
            }
            if (cblsearch.Items[15].Selected == true)
            {
                search[15] = "a.pdistrict";
            }
            if (cblsearch.Items[16].Selected == true)
            {
                search[16] = "a.pstate";
            }
            if (cblsearch.Items[17].Selected == true)
            {
                search[17] = "a.ccity";
            }
            if (cblsearch.Items[18].Selected == true)
            {
                search[18] = "a.cdistrict";
            }
            if (cblsearch.Items[19].Selected == true)
            {
                search[19] = "a.cstate";
            }
            if (cblsearch.Items[20].Selected == true)
            {
                search[20] = "a.com_pincode";
            }
            if (cblsearch.Items[21].Selected == true)
            {
                search[21] = "a.yofexp";
            }
            string wsearch = "";
            int count = 0;
            count = 1;
            FpSpread1.Visible = true;
            //btnprintmaster.Visible = true;

            //txtexcelname.Visible = true;
            //lblrptname.Visible = true;
            //btnexcel.Visible = true;
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
            if (count == 0)
            {

                FpSpread1.Sheets[0].ColumnCount = columncount + 3;
                string cmdquery = "select  " + selection + " from staff_appl_master a";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "App No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "staff Name";

                for (int icount = 0; icount < ItemList.Count; icount++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, icount + 2].Text = ItemList[icount].ToString();
                }
                FpSpread1.ColumnHeader.Height = 50;
                FarPoint.Web.Spread.NamedStyle instyle = new FarPoint.Web.Spread.NamedStyle();
                FarPoint.Web.Spread.NamedStyle outstyle = new FarPoint.Web.Spread.NamedStyle();
                instyle.BackColor = Color.Yellow;
                outstyle.BackColor = Color.Gray;

                FarPoint.Web.Spread.HideRowFilter sf = new FarPoint.Web.Spread.HideRowFilter(FpSpread1.Sheets[0]);

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

                        for (j = 1; j < columncount + 9; j++)
                        {
                            if (i == 0)
                            {
                                FpSpread1.Sheets[0].RowFilter.AddColumn(j);
                            }

                            if (dr.GetName(j).ToString() == "pdistrict")
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
                            else if (dr.GetName(j).ToString() == "pdistrict")
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
                wsearch = " a.appl_no";
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

                FarPoint.Web.Spread.NamedStyle instyle = new FarPoint.Web.Spread.NamedStyle();
                FarPoint.Web.Spread.NamedStyle outstyle = new FarPoint.Web.Spread.NamedStyle();
                instyle.BackColor = Color.Yellow;
                outstyle.BackColor = Color.Gray;
                FpSpread1.CommandBar.Visible = false;
                FarPoint.Web.Spread.HideRowFilter sf = new FarPoint.Web.Spread.HideRowFilter(FpSpread1.Sheets[0]);
                FpSpread1.Sheets[0].RowFilter = sf;
                FpSpread1.Sheets[0].ColumnCount = Itemindex.Count + 1;
                FpSpread1.ColumnHeader.Height = 50;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;

                FpSpread1.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
                FpSpread1.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
                FpSpread1.ActiveSheetView.Columns[0].Font.Bold = true;
                string con_sec_value = "";
                //if (cblsection.Items.Count > 0)
                //{
                //    for (int sec = 0; sec < cblsection.Items.Count; sec++)
                //    {
                //        if (cblsection.Items[sec].Selected == true)
                //        {
                //            string sec_value = cblsection.Items[sec].Value.ToString();
                //            if (con_sec_value == "")
                //                con_sec_value = sec_value;
                //            else
                //                con_sec_value = con_sec_value + "','" + sec_value;
                //        }
                //    }
                //}
                //if (laststudiedsel != "")
                //    selection = "select " + wsearch + " from staff_appl_master a, staffmaster s  where a.appl_no=s.appl_no";
                //else
                //    selection = "select " + wsearch + " from staff_appl_master a,staffmaster s  where a.appl_no=s.appl_no ";

                string isstaff = string.Empty;//delsi1306
                isstaff = d2.GetFunction("select is_staff from usermaster where User_code='" + Session["usercode"] + "'");
                if (isstaff.ToUpper() == "TRUE" || isstaff.ToUpper() == "1")
                {

                    if (laststudiedsel != "")
                        selection = "select " + wsearch + " from staff_appl_master a, staffmaster s  where a.appl_no=s.appl_no";
                    else
                        selection = "select " + wsearch + " from staff_appl_master a,staffmaster s  where a.appl_no=s.appl_no ";
                }
                else
                {

                    if (laststudiedsel != "")
                        selection = "select " + wsearch + " from staff_appl_master a, staffmaster s  where a.appl_no=s.appl_no";
                    else
                        selection = "select " + wsearch + " from staff_appl_master a,staffmaster s,hr_privilege hp  where a.appl_no=s.appl_no ";
                
                }



                tborder.Text = "";
                for (i = 0; i < Itemindex.Count; i++)
                {

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
               // collegecode_selection = " and a.college_code in(" + college + ") ";

              
                if (isstaff.ToUpper() == "TRUE" || isstaff.ToUpper() == "1")
                {
                    string stfcode = d2.GetFunction("select staff_code from usermaster where User_code='" + Session["usercode"] + "'");

                    collegecode_selection = "and a.college_code in(" + college + ") and s.staff_code ='" + stfcode + "'";
                }
                else
                {

                    collegecode_selection = "and a.college_code in(" + college + ")and hp.college_code=a.college_code and user_code='" + Session["usercode"] + "' and a.dept_code=hp.dept_code";
                }
                query = selection + sel1 + sel2 + sel3 + sel4 + sel5 + sel6 + sel7 + sel8 + sel9 + sel10 + sel11 + sel12 + sel13 + sel14 + sel15 + sel16 + sel17 + sel18 + sel19 + sel20 + sel21 + sel22 + sel23 + sel24 + sel25 + sel26 + sel27 + sel28 + sel29 + sel30 + sel31 + sel32 + sel33 + sel34 + sel35 + sel36 + sel37 + sel38 + sel39 + sel40 + sel41 + sel42 + sel43 + sel44 + sel45 + sel46 + sel47 + sel48 + sel49 + sel50 + sel51 + sel52 + sel53 + hosdaysel + laststudiedsel + sel_c + sel_p + collegecode_selection + " " + orderStr;
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

                        for (j = 1; j <= ItemList.Count; j++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Tag = dr.GetValue(0).ToString();

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
                            else if (dr.GetName(j).ToString().ToLower() == "cityc" || dr.GetName(j).ToString().ToLower() == "cityp" || dr.GetName(j).ToString().ToLower() == "cityg")
                            {

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

                            FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Left;
                            //FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.;
                        }
                        i = i + 1;
                        j = 0;
                    }
                btnprintmaster.Visible = true;

                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                btnexcel.Visible = true;
                dr.Close();
                con.Close();
                CalculateTotalPages();
            }
        }
        catch (ExcelException ex)
        { 
        
        }

    }
    protected void clear_Click(object sender, EventArgs e)
    {
        Response.Redirect("StaffUniversalReport.aspx");
    }

}