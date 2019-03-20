using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Net;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Text;

public partial class smmreport : System.Web.UI.Page
{
    static string collegecode = "";
    static Boolean forschoolsetting = false;// Added by sridharan
    string usercode = "";
    //  string collegecode = string.Empty;
    string singleuser = "";
    string group_user = "";
    string strworkingkey = "", strsenderid = "";

    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet dssmsrpt = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet Dsrv = new DataSet();
    string degree = "";
    int sentco = 0;
    int ival = 0;
    string deptvalue = "";
    string strcmdretrivesmsreport = "";
    string secvv = "";
    string PageLogOut = "";

    #region "Variable Declaration"

    int cnt = 0;

    string columnfield = string.Empty;
    string examcodeval = string.Empty;
    string strgrade = string.Empty;
    string strsec = string.Empty;
    string strsec1 = string.Empty;
    string strsecmark = string.Empty;
    string sturollno = string.Empty;
    string strsubcrd = string.Empty;
    string graders = string.Empty;
    string sqlstr = string.Empty;
    string strsection = string.Empty;

    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strdegree = string.Empty;
    string strbranch = string.Empty;
    string strsem = string.Empty;

    string strmobileno = string.Empty;
    string strfmobile = string.Empty;
    string strmmobile = string.Empty;
    string strstaffmobile = string.Empty;
    string strmsg = string.Empty;
    // string struserapi = string.Empty;
    //  string strsenderid = string.Empty;
    public SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    //added bu srinath 2/2/2013
    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;

    string send_mail = string.Empty;
    string send_pw = string.Empty;
    string to_mail = string.Empty;
    string strstuname = string.Empty;

    string sentcount = "";
    int rno = 0;

    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    static int sectioncnt = 0;

    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;

    string strsmsuserid = string.Empty;

    Boolean Cellclick = false;
    string mobilenos = "";

    Institution SchoolCollege;
    byte schoolOrCollege = 0;

    #endregion

    #region "Hash Table Declaration"

    //  Hashtable hat = new Hashtable();

    #endregion

    #region "Dataset Declaration"

    //DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    // DataSet ds1 = new DataSet1();
    DataSet ds2 = new DataSet1();
    DataSet dsbind = new DataSet();
    DataTable dt = new DataTable();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            panelnotification.Visible = false;
            ////****************************************************//
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("smsM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["SMS"].Trim();
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
            ////****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("sms"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/SMSMOD/sms.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            norecordlbl.Visible = false;
            errmsg.Visible = false;
            lblnorec.Visible = false;// added by sridhar 13 sep 2014

            setLabelText();
            if (!IsPostBack)
            {
                string getrights = "";
                txtstartdate.Attributes.Add("Readonly", "Readonly");
                txtenddate.Attributes.Add("Readonly", "Readonly");// added by sridhar 13 sep 2014
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    group_user = Session["group_code"].ToString();
                    if (group_user.Contains(';'))
                    {
                        string[] group_semi = group_user.Split(';');
                        group_user = group_semi[0].ToString();
                    }
                    getrights = d2.GetFunction("select rights_code from security_user_right where college_code=" + Session["collegecode"] + " and group_code='" + group_user + "' and rights_code='90002'");
                }
                else
                {
                    getrights = d2.GetFunction("select rights_code from security_user_right where college_code=" + Session["collegecode"] + " and user_code='" + Session["UserCode"] + "' and rights_code='90002'");
                }

                lblfrom.Visible = false;
                txtfrom.Visible = false;
                txtto.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
                btnselect.Visible = false;
                btndelete.Visible = false;
                FpSpread1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                FpSpread1.Width = 1000;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = true;
                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].AllowTableCorner = true;

                //---------------page number

                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
                FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
                FpSpread1.Pager.PageCount = 100;

                FpSpread1.CommandBar.Visible = false;
                //if (getrights.Trim() == "90002")
                if (getrights.Trim() == "90002" && Convert.ToString(Session["Staff_Code"]) == "")//rajasekar 12/07/2018
                {
                    staffpanel.Visible = true;
                    rdbtnstaff.Checked = true;
                    panelnotification.Visible = true;
                    FpSpread1.Visible = true;
                    txtbranch.Enabled = true;
                    txtdesignation.Enabled = true;
                    txtenddate.Enabled = true;
                    txtexcelname.Enabled = true;
                    txtsection.Enabled = true;
                    txtstafftype.Enabled = true;
                    txtstartdate.Enabled = true;
                    ddlcollege.Enabled = true;
                    rbnnotification.Enabled = true;
                    rbnsms.Enabled = true;
                    rdnbtncount.Enabled = true;
                    rdnbtndetails.Enabled = true;
                    btngo.Enabled = true;
                    btnnok.Enabled = true;
                    btnstaffgo.Enabled = true;
                    btnxl.Enabled = true;
                    tbbat.Enabled = true;
                    tbdeg.Enabled = true;
                    rdbtnstudent.Enabled = true;
                    rdbtnstaff.Enabled = true;

                    FpSpread1.Visible = false;
                    panelnotification.Visible = false;
                    txtstartdate.Text = DateTime.Today.ToString("d/MM/yyyy");
                    txtenddate.Text = DateTime.Today.ToString("d/MM/yyyy");

                    rdbtnstudent.Checked = true;
                    rdnbtndetails.Checked = true;
                    rdbtnstaff.Checked = false;
                    BindCollege(sender, e);
                    if (rdbtnstudent.Checked == true)
                    {
                        staffpanel.Visible = false;
                        lblbatch.Visible = true;
                        tbbat.Visible = true;
                        pbat.Visible = true;
                        Chkbatsel.Visible = true;
                        Chkbat.Visible = true;
                        lblsection.Visible = true;
                        txtsection.Visible = true;
                        psection.Visible = true;
                        chklstsection.Visible = true;
                        chksection.Visible = true;
                        //lbldeg.Text = "Degree";
                        //lblbranch.Text = "Branch";
                        batch();
                        BindDegree();
                        if (Chkdeg.Items.Count > 0)
                        {
                            //  BindBatch();
                            // BindDegree(singleuser, group_user, collegecode, usercode);
                            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                            BindSectionDetail(strbatch, strbranch);
                        }
                        else
                        {
                            chklstbranch.Items.Clear();
                            chklstsection.Items.Clear();
                        }

                    }
                    else if (rdbtnstaff.Checked == true)
                    {
                        lblsection.Visible = false;
                        txtsection.Visible = false;
                        psection.Visible = false;
                        chklstsection.Visible = false;
                        chksection.Visible = false;
                        lblst.Visible = true;
                        txtstartdate.Visible = true;

                        BindDesignation();
                        bindept();
                        bindstafftype();
                    }
                    // BindBatch();
                    //  BindDegree(singleuser, group_user, collegecode, usercode);
                    //  batch();
                    //  BindDegree();
                    // BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                    //  BindSectionDetail(strbatch, strbranch);
                    rbnsms.Checked = true;
                }
                else
                {
                    staffpanel.Visible = false;
                    rdbtnstaff.Checked = false;
                    panelnotification.Visible = false;
                    FpSpread1.Visible = false;
                    txtbranch.Enabled = false;
                    txtdesignation.Enabled = false;
                    txtenddate.Enabled = false;
                    txtexcelname.Enabled = false;
                    txtsection.Enabled = false;
                    txtstafftype.Enabled = false;
                    txtstartdate.Enabled = false;
                    ddlcollege.Enabled = false;
                    rbnnotification.Enabled = false;
                    rbnsms.Enabled = false;
                    rdnbtncount.Enabled = false;
                    rdnbtndetails.Enabled = false;
                    btngo.Enabled = false;
                    btnstaffgo.Enabled = false;
                    btnxl.Enabled = false;
                    tbbat.Enabled = false;
                    tbdeg.Enabled = false;
                    rdbtnstudent.Enabled = false;
                    rdbtnstaff.Enabled = false;
                    rbnnotification.Checked = false;
                    rbnsms.Checked = false;
                    loadindividualnote();
                }
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                string grouporusercodeschool = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercodeschool = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercodeschool + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        forschoolsetting = true;
                        //lblcollege.Text = "School";
                        lblbatch.Text = "Year";
                        //lbldeg.Text = "School Type";
                        //lblbranch.Text = "Standard";
                        //lblDuration.Text = "Term";
                        //Label1.Text = "Test Mark R11-Continuous Assessment Report";
                        //lbldeg.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 229px;    position: absolute;    top: 210px;");
                        //tbdeg.Attributes.Add("Style", "   font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    left: 328px;    margin-right: 15px;    position: absolute;    top: 210px;    width: 100px;");
                        //lblbranch.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 439px;    position: absolute;    top: 212px;    width: 90px;");
                        //txtbranch.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    left: 509px;    position: absolute;    top: 210px;    width: 180px;");
                        //lblsection.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 702px;    position: absolute;    top: 211px;    width: 100px;");


                    }
                    else
                    {
                        forschoolsetting = false;
                    }
                }
                else
                {
                    forschoolsetting = false;
                }
            }
        }
        catch (Exception ex) { }
    }

    #region "Page Load Function"

    public void PageLoad(object sender, EventArgs e)
    {

        usercode = Session["usercode"].ToString();
        collegecode = Session["InternalCollegeCode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (rdbtnstudent.Checked == true)
        {
            staffpanel.Visible = false;
            lblbatch.Visible = true;
            tbbat.Visible = true;
            pbat.Visible = true;
            Chkbatsel.Visible = true;
            Chkbat.Visible = true;
            lblsection.Visible = true;
            txtsection.Visible = true;
            psection.Visible = true;
            chklstsection.Visible = true;
            chksection.Visible = true;
            //lbldeg.Text = "Degree";
            //lblbranch.Text = "Branch";
            // lblst.Visible = false;
            //  BindBatch();
            batch();
            BindDegree();
            if (Chkdeg.Items.Count > 0)
            {

                //  BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                BindSectionDetail(strbatch, strbranch);
            }
            else
            {
                chklstbranch.Items.Clear();
                chklstsection.Items.Clear();
            }
        }
        else if (rdbtnstaff.Checked == true)
        {
            // staffpanel.Visible = true;
            //studentpanel.Visible = false;
            lblsection.Visible = false;
            txtsection.Visible = false;
            psection.Visible = false;
            chklstsection.Visible = false;
            chksection.Visible = false;
            lblst.Visible = true;
            txtstartdate.Visible = true;
            BindDesignation();
            bindept();
            bindstafftype();
        }
    }

    #endregion

    #region "Load Function for College Details"

    public void BindCollege(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["QueryString"] = "";
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                {
                    columnfield = " and group_code='" + group_user + "'";
                }
                else
                {
                    columnfield = " and user_code='" + Session["usercode"] + "'";
                }
                hat.Clear();
                hat.Add("column_field", columnfield.ToString());
                ds2.Dispose();
                ds2.Reset();
                ds2 = d2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = ds2;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);
                }
                PageLoad(sender, e);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    #endregion

    #region "Load Function for Batch Details"

    public void batch()
    {
        //SqlDataAdapter cmdbat = new SqlDataAdapter("select  distinct batch_year from registration where cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year", mysql);
        //mysql.Close();
        //mysql.Open();
        //DataSet dr = new DataSet();
        //cmdbat.Fill(dr);
        //if (dr.Tables[0].Rows.Count > 0)
        //{
        //    Chkbat.DataSource = dr;
        //    Chkbat.DataValueField = "batch_year";
        //    Chkbat.DataTextField = "batch_year";
        //    Chkbat.DataBind();

        //    for (int i = 0; i < Chkbat.Items.Count; i++)
        //    {
        //        Chkbat.Items[i].Selected = true;
        //        if (Chkbat.Items[i].Selected == true)
        //        {
        //            count2 += 1;
        //        }
        //        if (Chkbat.Items.Count == count2)
        //        {

        //            Chkbatsel.Checked = true;
        //        }
        //    }

        //    // Chkbat.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- -Select- -", "0"));
        //}
        //mysql.Close();

        ds.Dispose();
        ds.Reset();
        //ds = d2.BindBatch();
        string Master1 = "";
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {

            string group = Session["group_code"].ToString();
            if (group.Contains(';'))
            {
                string[] group_semi = group.Split(';');
                Master1 = group_semi[0].ToString();
            }
        }
        else
        {
            Master1 = Session["usercode"].ToString();
        }
        string collegecode = Session["collegecode"].ToString();
        string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "' order by batch_year desc";
        ds = d2.select_method_wo_parameter(strbinddegree, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            Chkbat.DataSource = ds;
            Chkbat.DataTextField = "Batch_year";
            Chkbat.DataValueField = "Batch_year";
            Chkbat.DataBind();
            Chkbat.SelectedIndex = Chkbat.Items.Count - 1;

            for (int i = 0; i < Chkbat.Items.Count; i++)
            {
                Chkbat.Items[i].Selected = true;
                if (Chkbat.Items[i].Selected == true)
                {
                    count2 += 1;
                }
                if (Chkbat.Items.Count == count2)
                {

                    Chkbatsel.Checked = true;
                }
            }
        }
    }

    public void BindDegree()
    {
        ////string collegecode = "13";// Session["collegecode"].ToString();
        //SqlDataAdapter cmdbat = new SqlDataAdapter("select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode.ToString() + " order by course.course_name ", mysql);
        //mysql.Open();
        //DataSet dr = new DataSet();
        //cmdbat.Fill(dr);
        //if (dr.Tables[0].Rows.Count > 0)
        //{
        //    Chkdeg.DataSource = dr;
        //    Chkdeg.DataValueField = "Course_Id";
        //    Chkdeg.DataTextField = "Course_Name";
        //    Chkdeg.DataBind();

        //for (int i = 0; i < Chkdeg.Items.Count; i++)
        //{
        //    Chkdeg.Items[i].Selected = true;
        //    if (Chkdeg.Items[i].Selected == true)
        //    {
        //        count2 += 1;
        //    }
        //    if (Chkdeg.Items.Count == count2)
        //    {
        //        Chkdegsel.Checked = true;
        //    }
        //}
        //    // Chkdeg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- -Select- -", "0"));
        //}
        //mysql.Close();

        Chkdeg.Items.Clear();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        ds.Dispose();
        ds.Reset();
        ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
        //ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Chkdeg.DataSource = ds;
            Chkdeg.DataTextField = "course_name";
            Chkdeg.DataValueField = "course_id";
            Chkdeg.DataBind();
            Chkdeg.Items[0].Selected = true;
            for (int i = 0; i < Chkdeg.Items.Count; i++)
            {
                Chkdeg.Items[i].Selected = true;
                if (Chkdeg.Items[i].Selected == true)
                {
                    count2 += 1;
                }
                if (Chkdeg.Items.Count == count2)
                {
                    Chkdegsel.Checked = true;
                }
            }

        }
    }

    protected void Chkbatsel_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkbatsel.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkbat.Items)
            {
                li.Selected = true;
                tbbat.Text = ((schoolOrCollege == 0) ? "Batch" : "Year") + "(" + (Chkbat.Items.Count) + ")";

            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkbat.Items)
            {
                li.Selected = false;
                tbbat.Text = "- - Select - -";
            }
        }
        BindDegree();
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetail(strbatch, strbatch);
    }

    protected void Chkdegsel_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkdegsel.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkdeg.Items)
            {
                li.Selected = true;
                tbdeg.Text = SchoolCollege.InsDegree + "(" + (Chkdeg.Items.Count) + ")";
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkdeg.Items)
            {
                li.Selected = false;
                tbdeg.Text = "- - Select - -";

            }
        }
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetail(strbatch, strbranch);
    }

    #endregion

    #region "Load Function for Branch Details"

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            for (int i = 0; i < Chkdeg.Items.Count; i++)
            {
                if (Chkdeg.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + Chkdeg.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + Chkdeg.Items[i].Value.ToString() + "";
                    }
                }
            }
            //course_id = chklstdegree.SelectedValue.ToString();
            // chklstbranch.Items.Clear();
            //if (group_user.Contains(';'))
            //{
            //    string[] group_semi = group_user.Split(';');
            //    group_user = group_semi[0].ToString();
            //}
            //ds2.Dispose();
            //ds2.Reset();
            //ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            //if (ds2.Tables[0].Rows.Count > 0)
            //{
            //    chklstbranch.DataSource = ds2;
            //    chklstbranch.DataTextField = "dept_name";
            //    chklstbranch.DataValueField = "degree_code";
            //    chklstbranch.DataBind();
            //for (int i = 0; i < chklstbranch.Items.Count; i++)
            //{
            //    chklstbranch.Items[i].Selected = true;
            //    if (chklstbranch.Items[i].Selected == true)
            //    {
            //        count2 += 1;
            //    }
            //    if (chklstbranch.Items.Count == count2)
            //    {
            //        chkbranch.Checked = true;
            //    }
            //}
            //}
            chklstbranch.Items.Clear();
            if (course_id.ToString() != "")
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                ds.Dispose();
                ds.Reset();
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
                    chklstbranch.DataBind();
                    chklstbranch.Items[0].Selected = true;
                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        chklstbranch.Items[i].Selected = true;
                        if (chklstbranch.Items[i].Selected == true)
                        {
                            count2 += 1;
                        }
                        if (chklstbranch.Items.Count == count2)
                        {
                            chkbranch.Checked = true;
                        }
                    }
                }
                BindSectionDetail(strbatch, strbranch);
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    #endregion

    #region "Load Function for Section Details"

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            for (int i = 0; i < Chkbat.Items.Count; i++)
            {
                if (Chkbat.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + Chkbat.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + Chkbat.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            //strbranch = chklstbranch.SelectedValue.ToString();


            chklstsection.Items.Clear();
            if (strbranch.ToString() != "" && strbatch.ToString() != "")
            {
                ds2.Dispose();
                ds2.Reset();
                chklstsection.Items.Insert(0, " ");
                ds2 = d2.BindSectionDetail(strbatch, strbranch);
                if (ds2.Tables[0].Rows.Count > 0)
                {

                    chklstsection.DataSource = ds2;
                    chklstsection.DataTextField = "sections";
                    chklstsection.DataBind();
                    chklstsection.Items.Insert(0, " ");
                    if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                    {
                        chklstsection.Enabled = false;
                    }
                    else
                    {
                        chklstsection.Enabled = true;
                        chklstsection.SelectedIndex = chklstsection.Items.Count - 2;
                        chklstsection.Items[0].Selected = true;
                        for (int i = 0; i < chklstsection.Items.Count; i++)
                        {
                            chklstsection.Items[i].Selected = true;
                            if (chklstsection.Items[i].Selected == true)
                            {
                                count3 += 1;
                            }
                            if (chklstsection.Items.Count == count3)
                            {
                                chksection.Checked = true;
                            }
                        }
                    }
                }

                else
                {

                    chklstsection.Items[0].Selected = true;

                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    #endregion

    protected void Chkdeg_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        string value = "";
        string code = "";
        // LinkButtondeg.Visible = true;
        for (int i = 0; i < Chkdeg.Items.Count; i++)
        {
            if (Chkdeg.Items[i].Selected == true)
            {
                value = Chkdeg.Items[i].Text;
                code = Chkdeg.Items[i].Value.ToString();
                commcount = commcount + 1;
                tbdeg.Text = SchoolCollege.InsDegree + "(" + commcount.ToString() + ")";
            }
        }
        if (commcount == 0)
            tbdeg.Text = "- - All - -";
        else
        {
            //Label lbl2 = deglabel();
            //lbl2.Text = " " + value + " ";
            //lbl2.ID = "lbl9-" + code.ToString();
            //ImageButton ib2 = batimage();
            //ib2.ID = "imgbut9_" + code.ToString();
            //ib2.Click += new ImageClickEventHandler(degimg_Click);
        }
        int commcnt = commcount;

        for (int ival = 0; ival < Chkdeg.Items.Count; ival++)
        {
            if (Chkdeg.Items[ival].Selected == true)
            {
                if ((course_id == ""))
                {
                    course_id = Chkdeg.Items[ival].Value;
                }
                else
                {
                    course_id = course_id + "," + Chkdeg.Items[ival].Value;
                }
            }
        }
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        //  BindSectionDetail(strbatch, strbranch);
    }

    #region "Load Function for Designation Details"

    public void bindstafftype()
    {
        SqlDataAdapter cmstafftype = new SqlDataAdapter("SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1", mysql);
        mysql.Close();
        mysql.Open();
        DataSet ds = new DataSet();
        cmstafftype.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Chhliststafftype.DataSource = ds;
            Chhliststafftype.DataValueField = "StfType";
            Chhliststafftype.DataTextField = "StfType";
            Chhliststafftype.DataBind();
            for (int i = 0; i < Chhliststafftype.Items.Count; i++)
            {
                Chhliststafftype.Items[i].Selected = true;
                if (Chhliststafftype.Items[i].Selected == true)
                {
                    count2 += 1;
                }
                if (Chhliststafftype.Items.Count == count2)
                {
                    Chkboxstafftype.Checked = true;
                }
            }
        }
        mysql.Close();


    }

    public void bindept()
    {
        count = 0;
        ds2.Dispose();
        ds2.Reset();
        ds2 = d2.loaddepartment(collegecode);
        CheckBoxList1.DataSource = ds2;
        CheckBoxList1.DataTextField = "dept_name";
        CheckBoxList1.DataValueField = "Dept_Code";
        CheckBoxList1.DataBind();

        for (int i = 0; i < CheckBoxList1.Items.Count; i++)
        {
            CheckBoxList1.Items[i].Selected = true;
            if (CheckBoxList1.Items[i].Selected == true)
            {
                count += 1;
            }
            if (CheckBoxList1.Items.Count == count)
            {
                CheckBox1.Checked = true;
            }
        }
    }

    public void BindDesignation()
    {
        count = 0;
        ds2.Dispose();
        ds2.Reset();
        ds2 = d2.binddesi(collegecode);
        chklstdesignation.DataSource = ds2;
        chklstdesignation.DataValueField = "desig_code";
        chklstdesignation.DataTextField = "desig_name";
        chklstdesignation.DataBind();
        chklstdesignation.SelectedIndex = chklstdesignation.Items.Count - 1;
        for (int i = 0; i < chklstdesignation.Items.Count; i++)
        {
            chklstdesignation.Items[i].Selected = true;
            if (chklstdesignation.Items[i].Selected == true)
            {
                count += 1;
            }
            if (chklstdesignation.Items.Count == count)
            {
                chkdesignation.Checked = true;
            }
        }
    }

    #endregion

    void bind_design()
    {
        int branchcount = 0;
        string value = "";
        string code = "";

        string staffvalue = "";
        string staffcode = "";

        for (int i = 0; i < Chhliststafftype.Items.Count; i++)
        {
            if (Chhliststafftype.Items[i].Selected == true)
            {
                value = Chhliststafftype.Items[i].Text;
                staffcode = Chhliststafftype.Items[i].Value.ToString();
                // branchcount = branchcount + 1;
                // TextBox1.Text = "Department(" + branchcount.ToString() + ")";
                if (staffvalue == "")
                {
                    staffvalue = "'" + value + "'";
                }
                else
                {
                    staffvalue = staffvalue + "," + "'" + value + "'";
                }

            }
        }

        for (int i = 0; i < CheckBoxList1.Items.Count; i++)
        {
            if (CheckBoxList1.Items[i].Selected == true)
            {
                value = CheckBoxList1.Items[i].Text;
                code = CheckBoxList1.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                TextBox1.Text = "Department(" + branchcount.ToString() + ")";

                if (deptvalue == "")
                {
                    deptvalue = "'" + code + "'";
                }
                else
                {
                    deptvalue = deptvalue + "," + "'" + code + "'";
                }

            }
        }

        SqlDataAdapter cmddesiggn = new SqlDataAdapter(" SELECT  Distinct Desig_Name FROM StaffTrans T,Desig_Master G WHERE T.Desig_Code = G.Desig_Code AND Latestrec = 1 and stftype in(" + staffvalue + ") and G.dept_code in(" + deptvalue + ") and G.staffcategory=T.stftype  and G.dept_code=T.dept_code", mysql);
        mysql.Open();
        DataSet dr = new DataSet();
        cmddesiggn.Fill(dr);
        chklstdesignation.DataSource = dr;
        chklstdesignation.DataValueField = "Desig_Name";
        chklstdesignation.DataTextField = "desig_name";
        chklstdesignation.DataBind();
        // chklstdesignation.SelectedIndex = Chkdeg.Items.Count - 1;

        for (int i = 0; i < chklstdesignation.Items.Count; i++)
        {
            chklstdesignation.Items[i].Selected = true;
            if (chklstdesignation.Items[i].Selected == true)
            {
                count += 1;
            }
            if (chklstdesignation.Items.Count == count)
            {
                chkdesignation.Checked = true;
            }
        }
    }

    #region "Load Function for Department Details"

    public void BindDepartment()
    {
        count = 0;
        ds2.Dispose();
        ds2.Reset();
        ds2 = d2.loaddepartment(collegecode);
        chklstbranch.DataSource = ds2;
        chklstbranch.DataTextField = "dept_name";
        chklstbranch.DataValueField = "Dept_Code";
        chklstbranch.DataBind();

        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            chklstbranch.Items[i].Selected = true;
            if (chklstbranch.Items[i].Selected == true)
            {
                count += 1;
            }
            if (chklstbranch.Items.Count == count)
            {
                chkbranch.Checked = true;
            }
        }
    }

    #endregion

    #region "College Dropdown Selected Index Changed Event"

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblmsgcredit.Text = "SMS Available Credits :0";
        if (Convert.ToString(Session["QueryString"]) != "")
        {

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
            Request.QueryString.Clear();

        }
        collegecode = ddlcollege.SelectedValue.ToString();
        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        PageLoad(sender, e);

        try
        {
            string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + ddlcollege.SelectedValue.ToString() + "'";
            ds1.Dispose();
            ds1.Reset();
            ds1 = d2.select_method(strsenderquery, hat, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
            }
            //GetUserapi(user_id);
            //modified by srinath 1/8/2014
            //GetUserapi(user_id);
            string getval = d2.GetUserapi(user_id);
            string[] spret = getval.Split('-');
            if (spret.GetUpperBound(0) == 1)
            {
                strsenderid = spret[0].ToString();
                strworkingkey = spret[1].ToString();
                Session["api"] = user_id;
                Session["senderid"] = strsenderid;
            }
            if (strsenderid != "" && strworkingkey != "")
            {
                lblmsgcredit.Visible = true;
                //modified by srinath 14/2/2014
                //WebRequest request = WebRequest.Create("http://inter.onlinespeedsms.in/api/balance.php?user=" + strsenderid.ToLower() + "&password=" + strworkingkey + "&type=4");
                //WebRequest request = WebRequest.Create("http://pr.airsmsmarketing.info/api/checkbalance.php?user=" + strsenderid + "&pass=" + strworkingkey + "");
                WebRequest request = WebRequest.Create("http://hp.dial4sms.com/balalert/main.php?uname=" + strsenderid + "&pass=" + strworkingkey + "");
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                StreamReader sr = new StreamReader(data);
                string strvel = sr.ReadToEnd();

                lblmsgcredit.Text = strvel.ToString();
                string[] strrrvel = strvel.Split(' ');
                int getuprbnd = strrrvel.GetUpperBound(0);
                lblmsgcredit.Text = "SMS Available Credits :" + strrrvel[getuprbnd];
            }
        }
        catch
        {
        }
    }

    #endregion

    #region "Branch Dropdown Extender"

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbranch.Checked == true)
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = true;
                if (rdbtnstudent.Checked == true)
                {
                    txtbranch.Text = SchoolCollege.InsBranch + "(" + (chklstbranch.Items.Count) + ")";
                }
                else if (rdbtnstaff.Checked == true)
                {
                    txtbranch.Text = "Department(" + (chklstbranch.Items.Count) + ")";
                }
            }
        }
        else
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = false;
                txtbranch.Text = "---Select---";
            }
        }

        // BindDegree(singleuser, group_user, collegecode, usercode);
        BindSectionDetail(strbatch, strbranch);
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbranch.Focus();

        int branchcount = 0;
        string value = "";
        string code = "";

        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {

                value = chklstbranch.Items[i].Text;
                code = chklstbranch.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                if (rdbtnstudent.Checked == true)
                {
                    txtbranch.Text = SchoolCollege.InsBranch + "(" + branchcount.ToString() + ")";
                }
                else if (rdbtnstaff.Checked == true)
                {
                    txtbranch.Text = "Department(" + branchcount.ToString() + ")";
                }
            }
        }

        if (branchcount == 0)
            txtbranch.Text = "---Select---";
        else
        {
            Label lbl = branchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = branchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(branchimg_Click);
        }
        branchcnt = branchcount;

        if (rdbtnstudent.Checked == true)
        {
            BindSectionDetail(strbatch, strbranch);
        }

    }

    protected void LinkButtonbranch_Click(object sender, EventArgs e)
    {

        chklstbranch.ClearSelection();
        branchcnt = 0;
        txtbranch.Text = "---Select---";
    }

    public void branchimg_Click(object sender, ImageClickEventArgs e)
    {
        branchcnt = branchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbranch.Items[r].Selected = false;
        if (rdbtnstudent.Checked == true)
        {
            tbdeg.Text = SchoolCollege.InsBranch + "(" + branchcnt.ToString() + ")";
            if (tbdeg.Text == SchoolCollege.InsBranch + "(0)")
            {
                tbdeg.Text = "---Select---";

            }
        }
        else if (rdbtnstaff.Checked == true)
        {
            tbdeg.Text = "Department(" + branchcnt.ToString() + ")";
            if (tbdeg.Text == "Department(0)")
            {
                tbdeg.Text = "---Select---";

            }
        }
    }

    public Label branchlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton branchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    #endregion

    #region "Section Dropdown Extender"

    protected void chksection_CheckedChanged(object sender, EventArgs e)
    {
        if (chksection.Checked == true)
        {
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                chklstsection.Items[i].Selected = true;
                txtsection.Text = "Section(" + (chklstsection.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                chklstsection.Items[i].Selected = false;
                txtsection.Text = "---Select---";
            }
        }
    }

    protected void chklstsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        psection.Focus();

        int sectioncount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstsection.Items.Count; i++)
        {
            if (chklstsection.Items[i].Selected == true)
            {

                value = chklstsection.Items[i].Text;
                code = chklstsection.Items[i].Value.ToString();
                sectioncount = sectioncount + 1;
                txtsection.Text = "Section(" + sectioncount.ToString() + ")";
            }

        }

        if (sectioncount == 0)
            txtsection.Text = "---Select---";
        else
        {
            Label lbl = sectionlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = sectionimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(sectionimg_Click);
        }
        sectioncnt = sectioncount;

    }

    protected void LinkButtonsection_Click(object sender, EventArgs e)
    {

        chklstsection.ClearSelection();
        sectioncnt = 0;
        txtsection.Text = "---Select---";
    }

    public void sectionimg_Click(object sender, ImageClickEventArgs e)
    {
        sectioncnt = sectioncnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstsection.Items[r].Selected = false;

        txtsection.Text = "Section(" + sectioncnt.ToString() + ")";
        if (txtsection.Text == "Section(0)")
        {
            txtsection.Text = "---Select---";

        }

    }

    public Label sectionlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton sectionimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    #endregion

    #region "Radio Button Checked Change Event"

    protected void rdbtnstudent_CheckedChanged(object sender, EventArgs e)
    {
        rdbtnstaff.Checked = false;
        studentpanel.Visible = true;
        staffpanel.Visible = false;
        rdbtnstaff.Checked = false;
        tbdeg.Text = "--Select--";
        txtbranch.Text = "--Select--";
        panelnotification.Visible = false;
        FpSpread1.Visible = false;
        if (rdbtnstudent.Checked == true)
        {
            lblst.Visible = true;
            errmsg.Text = "";
            FpSpread1.Visible = false;
            rdbtnstaff.Checked = false;
            lblbatch.Visible = true;
            tbbat.Visible = true;
            pbat.Visible = true;
            Chkbatsel.Visible = true;
            Chkbat.Visible = true;
            lblsection.Visible = true;
            txtsection.Visible = true;
            psection.Visible = true;
            chklstsection.Visible = true;
            chksection.Visible = true;
            //lbldeg.Text = "Degree";
            //lblbranch.Text = "Branch";
            lblst.Visible = true;
            Page_Load(sender, e);
            collegecode = ddlcollege.SelectedValue.ToString();
            batch();
            BindDegree();
            if (Chkdeg.Items.Count > 0)
            {
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                BindSectionDetail(strbatch, strbranch);
            }
            else
            {
                chklstbranch.Items.Clear();
                chklstsection.Items.Clear();
            }
        }
    }

    protected void rdbtnstaff_CheckedChanged(object sender, EventArgs e)
    {
        rdbtnstudent.Checked = false;
        studentpanel.Visible = false;
        staffpanel.Visible = true;
        tbdeg.Text = "--Select--";
        txtbranch.Text = "--Select--";
        rdbtnstudent.Checked = false;
        lblst.Visible = true;
        panelnotification.Visible = false;
        FpSpread1.Visible = false;
        if (rdbtnstaff.Checked == true)
        {
            errmsg.Text = "";
            rdbtnstudent.Checked = false;
            bindept();
            bindstafftype();
            BindDesignation();

        }
    }

    protected void txtdegree_TextChanged(object sender, EventArgs e)
    {

    }

    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        int checknetwork = 0;
        try
        {
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            lblfrom.Visible = false;
            txtfrom.Visible = false;
            txtto.Visible = false;
            lblto.Visible = false;
            txtto.Visible = false;
            btnselect.Visible = false;
            btndelete.Visible = false;
            if (rbnsms.Checked == true)
            {
                string startdate = "";
                string startdate1 = "";

                string enddate = "";
                string enddate1 = "";

                string date = txtstartdate.Text;
                string[] splitdate = date.Split(new char[] { '/' });
                startdate = splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString();
                startdate1 = splitdate[2].ToString() + "-" + splitdate[1].ToString() + "-" + splitdate[0].ToString();

                DateTime stdate = Convert.ToDateTime(startdate1);

                string date1 = txtenddate.Text;
                string[] splitdate1 = date1.Split(new char[] { '/' });
                enddate = splitdate1[1].ToString() + "/" + splitdate1[0].ToString() + "/" + splitdate1[2].ToString();
                enddate1 = splitdate1[2].ToString() + "/" + splitdate1[1].ToString() + "/" + splitdate1[0].ToString();
                DateTime eddate = Convert.ToDateTime(enddate1);


                errmsg.Text = "";
                if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
                {
                    for (int i = 0; i < Chkbat.Items.Count; i++)
                    {
                        if (Chkbat.Items[i].Selected == true)
                        {
                            if (strbatch == "")
                            {
                                strbatch = "'" + Chkbat.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                strbatch = strbatch + "," + "'" + Chkbat.Items[i].Value.ToString() + "'";
                            }
                        }
                    }

                    for (int i = 0; i < Chkdeg.Items.Count; i++)
                    {
                        if (Chkdeg.Items[i].Selected == true)
                        {
                            if (strdegree == "")
                            {
                                strdegree = "'" + Chkdeg.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                strdegree = strdegree + "," + "'" + Chkdeg.Items[i].Value.ToString() + "'";
                            }
                        }
                    }

                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        if (chklstbranch.Items[i].Selected == true)
                        {
                            if (strbranch == "")
                            {
                                strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                            }
                        }
                    }

                    int sccou = 0;

                    if (chklstsection.Items.Count == 0)
                    {
                        secvv = "";
                        strsec = "";
                        strsec1 = "";
                        strsecmark = "";
                    }
                    else
                    {
                        for (int i = 1; i < chklstsection.Items.Count; i++)
                        {
                            if (chklstsection.Items[i].Selected == true)
                            {
                                sccou++;
                                if (secvv == "")
                                {
                                    secvv = "and ( sections='' or sections is null or sections='" + chklstsection.Items[i].Value.ToString() + "'";
                                }
                                else
                                {
                                    secvv = secvv + "or sections='" + chklstsection.Items[i].Value.ToString() + "'";
                                }


                            }
                        }

                        if (sccou == 0)
                        {

                        }

                        else
                        {
                            secvv = secvv + ')';
                        }

                        strsec = " and registration.sections in ( " + strsection + ")";


                        strsec1 = " and sections in (" + strsection + ")";
                        strsecmark = "and re.sections in (" + strsection + ")";
                    }

                }
                string value = "";
                string code = "";

                string staffvalue = "";
                string staffcode = "";
                string designvalue = "";

                if (rdbtnstudent.Checked == false && rdbtnstaff.Checked == true)
                {
                    for (int i = 0; i < Chhliststafftype.Items.Count; i++)
                    {
                        if (Chhliststafftype.Items[i].Selected == true)
                        {
                            value = Chhliststafftype.Items[i].Text;
                            staffcode = Chhliststafftype.Items[i].Value.ToString();


                            if (staffvalue == "")
                            {
                                staffvalue = "'" + value + "'";
                            }
                            else
                            {
                                staffvalue = staffvalue + "," + "'" + value + "'";
                            }

                        }
                    }
                    if (staffvalue != "")
                    {
                        staffvalue = " and st.stftype in (" + staffvalue + ")";
                    }


                    for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                    {
                        if (CheckBoxList1.Items[i].Selected == true)
                        {
                            value = CheckBoxList1.Items[i].Text;
                            code = CheckBoxList1.Items[i].Value.ToString();
                            if (deptvalue == "")
                            {
                                deptvalue = "'" + code + "'";
                            }
                            else
                            {
                                deptvalue = deptvalue + "," + "'" + code + "'";
                            }

                        }
                    }

                    if (deptvalue != "")
                    {
                        deptvalue = "and  st.dept_code in (" + deptvalue + ")";
                    }

                    for (int i = 0; i < chklstdesignation.Items.Count; i++)
                    {
                        if (chklstdesignation.Items[i].Selected == true)
                        {
                            value = chklstdesignation.Items[i].Text;
                            code = chklstdesignation.Items[i].Value.ToString();
                            if (designvalue == "")
                            {
                                designvalue = "'" + code + "'";
                            }
                            else
                            {
                                designvalue = designvalue + "," + "'" + code + "'";
                            }

                        }
                    }
                }
                if (designvalue != "")
                {
                    designvalue = "and st.desig_code in (" + designvalue + ")";
                }

                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Visible = false;

                if (rdnbtndetails.Checked == true && rdnbtncount.Checked == false)
                {
                    FpSpread1.Sheets[0].ColumnCount = 10;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Phone No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Message";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Date";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Type";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Status";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Sender ID";
                    FpSpread1.Sheets[0].Columns[0].CellType = txt;
                    FpSpread1.Sheets[0].Columns[1].CellType = txt;
                    FpSpread1.Sheets[0].Columns[2].CellType = txt;
                    FpSpread1.Sheets[0].Columns[3].CellType = txt;
                    FpSpread1.Sheets[0].Columns[4].CellType = txt;
                    FpSpread1.Sheets[0].Columns[5].CellType = txt;
                    FpSpread1.Sheets[0].Columns[6].CellType = txt;
                    FpSpread1.Sheets[0].Columns[7].CellType = txt;
                    FpSpread1.Sheets[0].Columns[8].CellType = txt;
                    if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "RollNo";
                    }
                    else if (rdbtnstaff.Checked == true && rdbtnstudent.Checked == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff code";
                    }
                }
                else if (rdnbtndetails.Checked == false && rdnbtncount.Checked == true)
                {
                    FpSpread1.Sheets[0].ColumnCount = 5;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
                    }
                    else if (rdbtnstaff.Checked == true && rdbtnstudent.Checked == false)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                    }
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Date";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Sent Count";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sender ID";
                }
                //Modified by Srinath 18/12/2013
                strcmdretrivesmsreport = "";
                if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
                {
                    if (rdnbtndetails.Checked == true && rdnbtncount.Checked == false)
                    {
                        strcmdretrivesmsreport = "select distinct date,len(r.roll_no),r.roll_no,a.student_mobile,a.stud_name,a.degree_code,r.current_semester,dt.dept_acronym,c.course_name,r.batch_year,r.sections ,mobilenos,groupmessageid,message,case isstaff when 0 then 'Student' when 1 then 'Staff' end as isstaff,uu.User_id  from UserMaster uu, smsdeliverytrackmaster s, registration r,applyn a,course c,department dt,degree d where isstaff in('0') and a.app_no = r.app_no and  d.dept_code=dt.dept_code and c.course_id=d.course_id and r.degree_code=d.degree_code and uu.college_code =d.college_code and uu.college_code =s.college_code  and r.degree_code in (" + strbranch + ")  " + secvv + "  and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  and  mobilenos <> '' and r.batch_year in (" + strbatch + ") and  convert(varchar(10),date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) and groupmessageid not like '%code%' and groupmessageid not like '504%' and (a.Student_Mobile =s.mobilenos or a.parentF_Mobile=s.mobilenos or a.parentM_Mobile=s.mobilenos)  and r.App_No=s.studentAppNo and a.app_no=s.studentAppNo and uu.User_code=s.sender_id order by  date,len(r.roll_no),r.roll_no ";
                        //and uu.User_code =" + Session["Usercode"].ToString() + "
                    }
                    else if (rdnbtndetails.Checked == false && rdnbtncount.Checked == true)
                    {
                        strcmdretrivesmsreport = "select distinct convert(varchar(10),date,101) date ,r.batch_year,r.degree_code,c.course_name,d.acronym,r.current_semester,r.sections,count(r.roll_no) as count,uu.User_id from UserMaster uu, smsdeliverytrackmaster s, registration r,applyn a,course c,department dt,degree d where isstaff in('0') and a.app_no = r.app_no and  d.dept_code=dt.dept_code and c.course_id=d.course_id and r.degree_code=d.degree_code and uu.college_code =d.college_code and uu.college_code =s.college_code  and r.degree_code in (" + strbranch + ")  " + secvv + "  and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and mobilenos <> '' and r.batch_year in (" + strbatch + ") and  convert(varchar(10),date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) and groupmessageid not like '%code%' and groupmessageid not like '504%'  and (a.Student_Mobile =s.mobilenos or a.parentF_Mobile=s.mobilenos or a.parentM_Mobile=s.mobilenos)  and r.App_No=s.studentAppNo and a.app_no=s.studentAppNo and uu.User_code=s.sender_id group by convert(varchar(10),date,101),r.batch_year,r.degree_code,c.course_name,d.acronym,r.current_semester,r.sections,uu.User_id  order by convert(varchar(10),date,101),r.batch_year,r.degree_code ,c.course_name,d.acronym,r.current_semester,r.sections";//and uu.User_code =" + Session["Usercode"].ToString() + " 
                    }
                }
                else if (rdbtnstaff.Checked == true && rdbtnstudent.Checked == false)
                {
                    if (rdnbtndetails.Checked == true && rdnbtncount.Checked == false)
                    {
                        //Modified by srinath
                        //  strcmdretrivesmsreport = " select distinct date,len(sm.staff_code),sm.staff_code,sm.staff_name,h.dept_name ,mobilenos,groupmessageid,message,smsdeliverytrackmaster.college_code,case isstaff when 0 then 'Student' when 1 then 'Staff' end as isstaff from smsdeliverytrackmaster,staffmaster sm,stafftrans st,staff_appl_master sam,hrdept_master h where st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no and st.dept_code=h.dept_code and isstaff in('1') and sam.per_mobileno=mobilenos and mobilenos <> '' and  st.dept_code in ( " + deptvalue + " ) and st.desig_code in ( " + designvalue + ") and convert(varchar(10),date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) and resign = 0 and settled = 0 and latestrec = 1 order by date,len(sm.staff_code),sm.staff_code";
                        strcmdretrivesmsreport = " select distinct date,len(sm.staff_code),sm.staff_code,sm.staff_name,h.dept_name ,mobilenos,groupmessageid,message,s.college_code,case isstaff when 0 then 'Student' when 1 then 'Staff' end as isstaff,uu.User_id from UserMaster uu, smsdeliverytrackmaster s,staffmaster sm,stafftrans st,staff_appl_master sam,hrdept_master h where st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no and st.dept_code=h.dept_code and isstaff in('1') and uu.college_code =sm.college_code and uu.college_code =s.college_code  and sam.per_mobileno=mobilenos and mobilenos <> '' " + deptvalue + " " + designvalue + " " + staffvalue + " and convert(varchar(10),date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) and resign = 0 and settled = 0 and latestrec = 1  and uu.User_code=s.sender_id and (sam.per_mobileno=s.mobilenos or sam.com_mobileno =s.mobilenos) order by date,len(sm.staff_code),sm.staff_code";//and uu.User_code =" + Session["Usercode"].ToString() + "
                    }
                    else if (rdnbtndetails.Checked == false && rdnbtncount.Checked == true)
                    {
                        //strcmdretrivesmsreport = " select distinct convert(varchar(10),date,101) date ,st.dept_code,count(sm.appl_no) as count,h.dept_name as degree from smsdeliverytrackmaster,staffmaster sm,staff_appl_master sam,stafftrans st,hrdept_master h where st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no and st.dept_code=h.dept_code and isstaff in('1') and sam.per_mobileno=mobilenos and mobilenos <> '' and  st.dept_code in ( " + deptvalue + " ) and st.desig_code in ( " + designvalue + ") and convert(varchar(10),date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) and resign = 0 and settled = 0  and latestrec = 1 group by convert(varchar(10),date,101),st.dept_code,h.dept_name order by convert(varchar(10),date,101),st.dept_code,h.dept_name";
                        strcmdretrivesmsreport = " select distinct convert(varchar(10),date,101) date ,st.dept_code,count(sm.appl_no) as count,h.dept_name as degree ,uu.User_id  from UserMaster uu, smsdeliverytrackmaster s,staffmaster sm,staff_appl_master sam,stafftrans st,hrdept_master h where st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no and st.dept_code=h.dept_code and isstaff in('1') and uu.college_code =sm.college_code and uu.college_code =s.college_code  and sam.per_mobileno=mobilenos and mobilenos <> '' " + deptvalue + " " + designvalue + " " + staffvalue + " and convert(varchar(10),date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) and resign = 0 and settled = 0  and latestrec = 1 and uu.User_code=s.sender_id and (sam.per_mobileno=s.mobilenos or sam.com_mobileno =s.mobilenos) group by convert(varchar(10),date,101),st.dept_code,h.dept_name,uu.User_id  order by convert(varchar(10),date,101),st.dept_code,h.dept_name";//and uu.User_code =" + Session["Usercode"].ToString() + "
                    }
                }

                dssmsrpt.Clear();
                dssmsrpt.Dispose();
                dssmsrpt.Reset();

                if (strcmdretrivesmsreport.Trim().ToString() != "")
                {
                    dssmsrpt = d2.select_method(strcmdretrivesmsreport, hat, "");
                }
                else
                {
                    FpSpread1.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    Printcontrol.Visible = false;
                    return;
                }

                if (dssmsrpt.Tables[0].Rows.Count > 0)
                {
                    if (rdnbtndetails.Checked == false && rdnbtncount.Checked == true)
                    {
                        //Count Details-----------------------------
                        if (rdnbtndetails.Checked == false && rdnbtncount.Checked == true)
                        {
                            int totalsmscount = 0;
                            for (int smscount = 0; smscount < dssmsrpt.Tables[0].Rows.Count; smscount++)
                            {
                                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(smscount + Convert.ToInt16(1));
                                if (rdbtnstudent.Checked == false && rdbtnstaff.Checked == true)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dssmsrpt.Tables[0].Rows[smscount]["degree"]);
                                }
                                else if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
                                {
                                    string secs = dssmsrpt.Tables[0].Rows[smscount]["sections"].ToString();

                                    if (secs == "" || secs == null || secs == "-1")
                                    {
                                        secs = "";
                                    }
                                    else
                                    {
                                        secs = "-" + dssmsrpt.Tables[0].Rows[smscount]["sections"].ToString();
                                    }

                                    string degree = dssmsrpt.Tables[0].Rows[smscount]["batch_year"].ToString() + "[" + dssmsrpt.Tables[0].Rows[smscount]["course_name"].ToString() + "-" + dssmsrpt.Tables[0].Rows[smscount]["acronym"].ToString() + "-" + dssmsrpt.Tables[0].Rows[smscount]["current_semester"].ToString() + secs + "]";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(degree);
                                }


                              

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dssmsrpt.Tables[0].Rows[smscount]["date"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dssmsrpt.Tables[0].Rows[smscount]["count"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dssmsrpt.Tables[0].Rows[smscount]["User_id"]);
                             

                                if (Convert.ToString(dssmsrpt.Tables[0].Rows[smscount]["count"]) != "")
                                {
                                    totalsmscount = totalsmscount + Convert.ToInt32(Convert.ToString(dssmsrpt.Tables[0].Rows[smscount]["count"]));
                                }

                                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                            }

                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Total";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = totalsmscount.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                        }

                        FpSpread1.Visible = true;
                        btnxl.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        btnprintmaster.Visible = true;
                        Printcontrol.Visible = false;
                        return;

                        //------------------------------------------
                    }
                    //else
                    //{
                    //    FpSpread1.Sheets[0].RowCount = 0;

                    //    for (int i = 0; i < dssmsrpt.Tables[0].Rows.Count; i++)
                    //    {
                    //        DateTime sms_date = Convert.ToDateTime(dssmsrpt.Tables[0].Rows[i]["date"].ToString());

                    //        FpSpread1.Sheets[0].RowCount++;

                    //        FpSpread1.Sheets[0].Cells[i, 0].Text = FpSpread1.Sheets[0].RowCount.ToString();
                    //        FpSpread1.Sheets[0].Cells[i, 1].Text = dssmsrpt.Tables[0].Rows[i]["dept_name"].ToString();
                    //        FpSpread1.Sheets[0].Cells[i, 2].Text = dssmsrpt.Tables[0].Rows[i]["staff_code"].ToString();
                    //        FpSpread1.Sheets[0].Cells[i, 3].Text = dssmsrpt.Tables[0].Rows[i]["staff_name"].ToString();
                    //        FpSpread1.Sheets[0].Cells[i, 4].Text = dssmsrpt.Tables[0].Rows[i]["mobilenos"].ToString();
                    //        FpSpread1.Sheets[0].Cells[i, 5].Text = dssmsrpt.Tables[0].Rows[i]["message"].ToString();
                    //        FpSpread1.Sheets[0].Cells[i, 6].Text = sms_date.ToString("dd/MM/yyyy");
                    //        FpSpread1.Sheets[0].Cells[i, 7].Text = "";
                    //        FpSpread1.Sheets[0].Cells[i, 8].Text = "";

                    //        FpSpread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                    //        FpSpread1.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;
                    //        FpSpread1.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Center;
                    //        FpSpread1.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
                    //        FpSpread1.Sheets[0].Cells[i, 6].HorizontalAlign = HorizontalAlign.Center;

                    //    }

                    //    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    //    FpSpread1.Visible = true;
                    //}
                }
                else
                {
                    FpSpread1.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    norecordlbl.Text = "No Records Found";
                    norecordlbl.Visible = true;
                    btnprintmaster.Visible = false;
                    Printcontrol.Visible = false;
                    return;
                }

                string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + ddlcollege.SelectedValue.ToString() + "'";
                ds1.Dispose();
                ds1.Reset();
                ds1 = d2.select_method(strsenderquery, hat, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strsmsuserid = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
                }


                //Aruna 7/72017==========================================================

                SMSSettings smsObject = new SMSSettings();
                smsObject.User_collegecode = Convert.ToInt32(ddlcollege.SelectedValue);
                smsObject.User_usercode = usercode;
                smsObject.IsStaff = 0;
                byte sms_settings = smsObject.getSMSSettings(smsObject.User_collegecode);

                if (sms_settings == 0)
                {

                }
                else if (sms_settings == 1)
                {
                    #region Individual SMS
                    {
                    }
                    #endregion
                }

                //=======================================================================

                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {

                    strsenderid = spret[0].ToString();
                    strworkingkey = spret[1].ToString();
                    Session["api"] = user_id;
                    Session["senderid"] = strsenderid;
                }
                if (dssmsrpt != null && dssmsrpt.Tables[0] != null && dssmsrpt.Tables[0].Rows.Count > 0)
                {
                    string groupmsgid = "";
                    string message = "", isstaff = "";

                    FpSpread1.Visible = true;
                    btnxl.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnprintmaster.Visible = true;
                    Printcontrol.Visible = false;

                    FarPoint.Web.Spread.TextCellType chkcell = new FarPoint.Web.Spread.TextCellType();
                    FpSpread1.Sheets[0].Columns[3].CellType = chkcell;
                    int sno = 0;

                    for (int groupid = 0; groupid < dssmsrpt.Tables[0].Rows.Count; groupid++)
                    {
                        groupmsgid = dssmsrpt.Tables[0].Rows[groupid]["groupmessageid"].ToString().Trim();
                        //aruna 02oct2013 dssmsrpt.Tables[0].Rows[groupid]
                        message = dssmsrpt.Tables[0].Rows[groupid]["message"].ToString();
                        if(!message.Contains(' '))
                          message=   Decryptdata(message.ToString().Trim());
                        isstaff = dssmsrpt.Tables[0].Rows[groupid]["isstaff"].ToString();
                        string mobilenos = dssmsrpt.Tables[0].Rows[groupid]["mobilenos"].ToString();
                        string datetime1 = dssmsrpt.Tables[0].Rows[groupid]["date"].ToString();
                        string sender_name = dssmsrpt.Tables[0].Rows[groupid]["User_id"].ToString();
                        checknetwork = 0;
                        if (sms_settings == 0) //Common SMS
                        {
                            //  string requestweb = "http://inter.onlinespeedsms.in/api/check_delivery.php?user=" + strsmsuserid.ToLower() + "&password=" + strworkingkey + "&msgid=" + groupmsgid;
                            //string requestweb = "http://pr.airsmsmarketing.info/api/recdlr.php?user=" + strsmsuserid.ToLower() + "&password=" + strworkingkey + "&msgid=" + groupmsgid;
                            //WebRequest request;
                            //WebResponse response;
                            //Stream data;
                            //StreamReader sr;
                            //checknetwork = 1;
                            //string strvel = "";
                            int uprbound = 0;
                            int uprboun = 0;
                            //try
                            //{
                            //    request = WebRequest.Create(requestweb);
                            //    response = request.GetResponse();
                            //    data = response.GetResponseStream();
                            //    sr = new StreamReader(data);
                            //    checknetwork = 1;
                            //    strvel = sr.ReadToEnd();

                            //}
                            //catch
                            //{
                            //}
                            string strvel = "24234|23423|234223";
                            string[] strrrvel = strvel.Split('|');
                            uprbound = strrrvel.GetUpperBound(0);
                            uprboun = uprbound / 3;
                            if (uprbound > 0)
                            {
                                for (int iter = 0; iter < 1; iter++)
                                {
                                    string splitone = strrrvel[0].ToString();
                                    string[] strarrtwo = mobilenos.Split(',');

                                    for (int intv = 0; intv < strarrtwo.Length; intv++)
                                    {
                                        string phoneno = "";
                                        string datetime = "";//, time = "";
                                        phoneno = Convert.ToString(strrrvel[0]);
                                        if (mobilenos.Contains(","))
                                        {
                                            datetime = Convert.ToString(strrrvel[3]);
                                        }
                                        string[] datetimespilt = datetime.Split('<');
                                        datetime = datetimespilt[0].ToString();
                                        string status = Convert.ToString(strrrvel[2]);
                                        string valueToFind = Between_String(status, "stat:", " err:");
                                        status = Convert.ToString(valueToFind);
                                        phoneno = Convert.ToString(strarrtwo[intv]);

                                        sno = sno + 1;
                                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = datetime1;



                                        //DateTime dtnew = Convert.ToDateTime(datetime1);//Added by Manikandan 07/08/2013
                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = dtnew.ToString("dd/MM/yyyy hh:mm:ss");//Modified by Manikandan 07/08/2013


                                        //Added By Saranya devi 17.11.2018
                                        DateTime dtnew = Convert.ToDateTime(datetime1);
                                        string Datetime = Convert.ToString(dtnew);
                                        if (Datetime != "")
                                        {
                                            string[] split = Datetime.Split(' ');
                                            string time = split[2].Contains("AM") ? "AM" : "PM";
                                            if (split[1] == "12:00:00" && time == "AM")
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = dtnew.ToString("dd/MM/yyyy");
                                            else
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = dtnew.ToString("dd/MM/yyyy hh:mm tt");

                                        }
                                        //End

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(message);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = chkcell;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(phoneno);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(isstaff);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(status);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = sender_name;

                                        if (rdbtnstudent.Checked == true)
                                        {
                                            string secs = dssmsrpt.Tables[0].Rows[groupid]["sections"].ToString();
                                            if (secs == "" || secs == null || secs == "-1")
                                            {
                                                secs = "";
                                            }
                                            else
                                            {
                                                secs = "-" + dssmsrpt.Tables[0].Rows[groupid]["sections"].ToString();
                                            }
                                            string degree = dssmsrpt.Tables[0].Rows[groupid]["batch_year"].ToString() + "[" + dssmsrpt.Tables[0].Rows[groupid]["course_name"].ToString() + "-" + dssmsrpt.Tables[0].Rows[groupid]["dept_acronym"].ToString() + "-" + dssmsrpt.Tables[0].Rows[groupid]["current_semester"].ToString() + secs + "]";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(degree);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dssmsrpt.Tables[0].Rows[groupid]["roll_no"].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dssmsrpt.Tables[0].Rows[groupid]["stud_name"].ToString();

                                        }
                                        else if (rdbtnstaff.Checked == true)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dssmsrpt.Tables[0].Rows[groupid]["dept_name"].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dssmsrpt.Tables[0].Rows[groupid]["staff_code"].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dssmsrpt.Tables[0].Rows[groupid]["staff_name"].ToString();

                                        }

                                    }
                                }

                            }
                            else
                            {
                                string phoneno = "";
                                string datetime = "";//, time = "";
                                DateTime dtv = new DateTime();
                                phoneno = Convert.ToString(strrrvel[0]);

                                sno = sno + 1;
                                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = datetime1;
                                DateTime dtnew = Convert.ToDateTime(datetime1);//Added by Manikandan 07/08/2013
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = dtnew.ToString("dd/MM/yyyy hh:mm:ss");//Modified by Manikandan 07/08/2013
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(message);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = chkcell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(mobilenos);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(isstaff);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = strvel.ToString();//modify by M.SakthiPriya 15/12/2014
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = sender_name;

                                if (rdbtnstudent.Checked == true)
                                {
                                    string secs = dssmsrpt.Tables[0].Rows[groupid]["sections"].ToString();
                                    if (secs == "" || secs == null || secs == "-1")
                                    {
                                        secs = "";
                                    }
                                    else
                                    {
                                        secs = "-" + dssmsrpt.Tables[0].Rows[groupid]["sections"].ToString();
                                    }
                                    string degree = dssmsrpt.Tables[0].Rows[groupid]["batch_year"].ToString() + "[" + dssmsrpt.Tables[0].Rows[groupid]["course_name"].ToString() + "-" + dssmsrpt.Tables[0].Rows[groupid]["dept_acronym"].ToString() + "-" + dssmsrpt.Tables[0].Rows[groupid]["current_semester"].ToString() + secs + "]";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(degree);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dssmsrpt.Tables[0].Rows[groupid]["roll_no"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dssmsrpt.Tables[0].Rows[groupid]["stud_name"].ToString();

                                }
                                else if (rdbtnstaff.Checked == true)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dssmsrpt.Tables[0].Rows[groupid]["dept_name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dssmsrpt.Tables[0].Rows[groupid]["staff_code"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dssmsrpt.Tables[0].Rows[groupid]["staff_name"].ToString();

                                }
                            }
                            FpSpread1.Columns[8].Visible = false;
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        }
                        else if (sms_settings == 1) //Individual SMS
                        {

                        }
                    }
                }
                else
                {

                    FpSpread1.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    norecordlbl.Visible = true;
                    norecordlbl.Text = "No Records Found";
                    btnprintmaster.Visible = false;
                    Printcontrol.Visible = false;
                }
            }
            else if (rbnnotification.Checked == true)
            {
                loadnotification();
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            if (checknetwork == 0)
            {
                norecordlbl.Visible = true;
                norecordlbl.Text = "Server Error.Kindly try again after sometime.";
                FpSpread1.Visible = false;
            }
            else
            {
                norecordlbl.Visible = true;
                norecordlbl.Text = ex.ToString();

            }

            //throw ex;
        }
    }

    protected void Chkbat_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbat.Focus();

        int batchcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < Chkbat.Items.Count; i++)
        {
            if (Chkbat.Items[i].Selected == true)
            {

                value = Chkbat.Items[i].Text;
                code = Chkbat.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                tbbat.Text = ((schoolOrCollege == 0) ? "Batch" : "Year") + "(" + batchcount.ToString() + ")";
            }

        }

        if (batchcount == 0)
            tbbat.Text = "---Select---";
        else
        {
            Label lbl = batchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = batchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(batchimg_Click);
        }
        batchcnt = batchcount;
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetail(strbatch, strbranch);

    }

    protected void LinkButtonbatch_Click(object sender, EventArgs e)
    {

        Chkbat.ClearSelection();
        batchcnt = 0;
        tbbat.Text = "---Select---";
    }

    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        batchcnt = batchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        Chkbat.Items[r].Selected = false;

        tbbat.Text = ((schoolOrCollege == 0) ? "Batch" : "Year") + "(" + batchcnt.ToString() + ")";
        if (tbbat.Text == ((schoolOrCollege == 0) ? "Batch" : "Year") + "(0)")
        {
            tbbat.Text = "---Select---";
        }

    }

    public Label batchlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton batchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    //Modified By Srinath 14/2/2014
    //public void GetUserapi(string struserid)
    //{
    //    try
    //    {
    //        if (struserid == "DEANSEC")
    //        {
    //            strsenderid = "DEANSEC";
    //            strworkingkey = "DEANSEC";
    //        }
    //        else if (struserid == "ProfClg")
    //        {
    //            strsenderid = "ProfClg";
    //            strworkingkey = "ProfClg";
    //        }

    //        else if (struserid == "SASTHA")
    //        {
    //            strsenderid = "SASTHA";
    //            strworkingkey = "SASTHA";
    //        }

    //        else if (struserid == "SSMCE")
    //        {
    //            strsenderid = "SSMCEE";
    //            strworkingkey = "SSMCE";
    //        }

    //        else if (struserid == "NECARE")
    //        {
    //            strsenderid = "NECARE";
    //            strworkingkey = "NECARE";
    //        }

    //        else if (struserid == "SVCTCG")
    //        {
    //            strsenderid = "SVCTCG";
    //            strworkingkey = "SVCTCG";
    //        }
    //        else if (struserid == "AGNICT")
    //        {
    //            strsenderid = "AGNICT";
    //            strworkingkey = "AGNICT";
    //        }
    //        else if (struserid == "NANDHA")
    //        {
    //            strsenderid = "NANDHA";
    //            strworkingkey = "NANDHA";
    //        }
    //        else if (struserid == "DHIRA")
    //        {
    //            strsenderid = "DHIRAJ";
    //            strworkingkey = "DHIRA";
    //        }
    //        else if (struserid == "ANGEL123")
    //        {
    //            strsenderid = "ANGELS";
    //            strworkingkey = "ANGEL123";
    //        }
    //        else if (struserid == "BALAJI12")
    //        {
    //            strsenderid = "BALAJI";
    //            strworkingkey = "BALAJI12";
    //        }
    //        else if (struserid == "AKSHYA123")
    //        {
    //            strsenderid = "AKSHYA";
    //            strworkingkey = "AKSHYA";
    //        }
    //        else if (struserid == "PPGITS")
    //        {
    //            strsenderid = "PPGITS";
    //            strworkingkey = "PPGITS";
    //        }
    //        else if (struserid == "PETENG")
    //        {
    //            strsenderid = "PETENG";
    //            strworkingkey = "PETENG";
    //        }
    //        else if (struserid == "JJCET")
    //        {
    //            strsenderid = "JJCET";
    //            strworkingkey = "JJCET";
    //        }
    //        else if (struserid == "PSVCET")
    //        {
    //            strsenderid = "PSVCET";
    //            strworkingkey = "PSVCET";
    //        }
    //        else if (struserid == "AMSECE")
    //        {
    //            SenderID = "AMSECE";
    //            strworkingkey = "AMSECE";
    //        }

    //        else if (struserid == "GKMCET")
    //        {
    //            strsenderid = "GKMCET";
    //            strworkingkey = "GKMCET";
    //        }
    //        else if (struserid == "SLAECT")
    //        {
    //            strsenderid = "SLAECT";
    //            strworkingkey = "SLAECT";
    //        }
    //        else if (struserid == "DCTSCE")
    //        {
    //            strsenderid = "DCTSCE";
    //            strworkingkey = "DCTSCE";
    //        }
    //        else if (struserid == "DCTSCE")
    //        {
    //            strsenderid = "DCTSCE";
    //            strworkingkey = "DCTSCE";
    //        }
    //        else if (struserid == "DCTSEC")
    //        {
    //            strsenderid = "DCTSEC";
    //            strworkingkey = "DCTSEC";
    //        }
    //        else if (struserid == "DCTSBS")
    //        {
    //            strsenderid = "DCTSBS";
    //            strworkingkey = "DCTSBS";
    //        }
    //        else if (struserid == "SCTSCE")
    //        {
    //            strsenderid = "SCTSCE";
    //            strworkingkey = "SCTSCE";
    //        }

    //        else if (struserid == "SCTSEC")
    //        {
    //            strsenderid = "SCTSEC";
    //            strworkingkey = "SCTSEC";
    //        }
    //        else if (struserid == "SCTSBS")
    //        {
    //            strsenderid = "SCTSBS";
    //            strworkingkey = "SCTSBS";
    //        }

    //        else if (struserid == "ESECED")
    //        {
    //            strsenderid = "ESECED";
    //            strworkingkey = "ESECED";
    //        }

    //        else if (struserid == "IJAYAM")
    //        {
    //            strsenderid = "IJAYAM";
    //            strworkingkey = "IJAYAM";
    //        }
    //        else if (struserid == "MPNMJS")
    //        {
    //            strsenderid = "MPNMJS";
    //            strworkingkey = "MPNMJS";
    //        }

    //        else if (struserid == "EASACG")
    //        {
    //            strsenderid = "EASACG";
    //            strworkingkey = "EASACG";
    //        }
    //        else if (struserid == "KTVRKP")
    //        {
    //            strsenderid = "KTVRKP";
    //            strworkingkey = "KTVRKP";
    //        }
    //        else if (struserid == "SVSCBE")
    //        {
    //            strsenderid = "SVSCBE";
    //            strworkingkey = "SVSCBE";
    //        }
    //        else if (struserid == "AIHTCH")
    //        {
    //            strsenderid = "AIHTCH";
    //            strworkingkey = "AIHTCH";
    //        }
    //        else if (struserid == "NSNCET")
    //        {
    //            strsenderid = "NSNCET";
    //            strworkingkey = "NSNCET";
    //        }
    //        else if (struserid == "SVICET")
    //        {
    //            strsenderid = "SVICET";
    //            strworkingkey = "SVICET";
    //        }
    //        else if (struserid == "SSCENG")
    //        {
    //            strsenderid = "SSCENG";
    //            strworkingkey = "SSCENG";
    //        }
    //        else if (struserid == "ECESMS")
    //        {
    //            strsenderid = "ECESMS";
    //            strworkingkey = "ECESMS";
    //        }
    //        else if (struserid == "NGPTEC")
    //        {
    //            strsenderid = "NGPTEC";
    //            strworkingkey = "NGPTEC";
    //        }
    //        else if (struserid == "NGPTEC")
    //        {
    //            strsenderid = "NGPTEC";
    //            strworkingkey = "NGPTEC";
    //        }

    //        else if (struserid == "KSRIET")
    //        {
    //            strsenderid = "KSRIET";
    //            strworkingkey = "KSRIET";
    //        }

    //        else if (struserid == "VCWSMS")
    //        {
    //            strsenderid = "VCWSMS";
    //            strworkingkey = "VCWSMS";
    //        }

    //        else if (struserid == "PMCTEC")
    //        {
    //            strsenderid = "PMCTEC";
    //            strworkingkey = "PMCTEC";
    //        }
    //        else if (struserid == "SRECCG")
    //        {
    //            strsenderid = "SRECCG";
    //            strworkingkey = "SRECCG";
    //        }

    //        else if (struserid == "SCHCLG")
    //        {
    //            strsenderid = "SCHCLG";
    //            strworkingkey = "SCHCLG";
    //        }
    //        else if (struserid == "TSMJCT")
    //        {
    //            strsenderid = "TSMJCT";
    //            strworkingkey = "TSMJCT";
    //        }
    //        else if (struserid == "SRECTD")
    //        {
    //            strsenderid = "SRECTD";
    //            strworkingkey = "SRECTD";
    //        }
    //        else if (struserid == "EICTPC")
    //        {
    //            strsenderid = "EICTPC";
    //            strworkingkey = "EICTPC";
    //        }
    //        else if (struserid == "SHACLG")
    //        {
    //            strsenderid = "SHACLG";
    //            strworkingkey = "SHACLG";
    //        }
    //        else if (struserid == "ARASUU")
    //        {
    //            strsenderid = "ARASUU";
    //            strworkingkey = "ARASUU";
    //        }
    //        else if (struserid == "TECAAA")
    //        {
    //            strsenderid = "TECAAA";
    //            strworkingkey = "TECAAA";
    //        }
    //        else if (struserid == "AAACET")
    //        {
    //            strsenderid = "AAACET";
    //            strworkingkey = "AAACET";
    //        }
    //        else if (struserid == "SVISTE")
    //        {
    //            strsenderid = "SVISTE";
    //            strworkingkey = "SVISTE";
    //        }
    //        else if (struserid == "AALIME")
    //        {
    //            strsenderid = "AALIME";
    //            strworkingkey = "AALIME";
    //        }
    //        else if (struserid == "VRSCET")
    //        {
    //            strsenderid = "VRSCET";
    //            strworkingkey = "VRSCET";
    //        }
    //        else if (struserid == "ACETVM")
    //        {
    //            strsenderid = "ACETVM";
    //            strworkingkey = "ACETVM";
    //        }
    //        else if (struserid == "TECENG")
    //        {
    //            strsenderid = "TECENG";
    //            strworkingkey = "TECENG";
    //        }
    //        else if (struserid == "TJENGG")
    //        {
    //            strsenderid = "TJENGG";
    //            strworkingkey = "TJENGG";
    //        }
    //        else if (struserid == "DAVINC")
    //        {
    //            strsenderid = "DAVINC";
    //            strworkingkey = "DAVINC";
    //        }
    //        else if (struserid == "ESENGG")
    //        {
    //            strsenderid = "ESENGG";
    //            strworkingkey = "ESENGG";
    //        }
    //        else if (struserid == "ESMSCH")
    //        {
    //            strsenderid = "ESMSCH";
    //            strworkingkey = "ESMSCH";
    //        }
    //        else if (struserid == "ESEPTC")
    //        {
    //            strsenderid = "ESEPTC";
    //            strworkingkey = "ESEPTC";
    //        }
    //        else if (struserid == "KINGSE")
    //        {
    //            strsenderid = "KINGSE";
    //            strworkingkey = "KINGSE";
    //        }


    //        //--------end-----------
    //        Session["senderid"] = strsenderid;
    //        Session["workingkey"] = strworkingkey;

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "AAACET")
    //        {
    //            strsenderid = "AAACET";
    //            strworkingkey = "AAACET";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            strsenderid = "AALIME";
    //            strworkingkey = "AALIME";
    //        }
    //        else if (user_id == "SVschl")
    //        {
    //            SenderID = "SVschl";
    //            Password = "SVschl";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            strsenderid = "ACETVM";
    //            strworkingkey = "ACETVM";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            strsenderid = "AGNICT";
    //            strworkingkey = "AGNICT";
    //        }
    //        else if (user_id == "AMSPTC")
    //        {
    //            strsenderid = "AMSPTC";
    //            strworkingkey = "AMSPTC";
    //        }
    //        else if (user_id == "ANGE")
    //        {
    //            strsenderid = "ANGE";
    //            strworkingkey = "ANGE";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            strsenderid = "ARASUU";
    //            strworkingkey = "ARASUU";
    //        }
    //        else if (user_id == "DAVINC")
    //        {
    //            strsenderid = "DAVINC";
    //            strworkingkey = "DAVINC";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            strsenderid = "EASACG";
    //            strworkingkey = "EASACG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            strsenderid = "ECESMS";
    //            strworkingkey = "ECESMS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            strsenderid = "ESECED";
    //            strworkingkey = "ESECED";
    //        }
    //        else if (user_id == "ESENGG")
    //        {
    //            strsenderid = "ESENGG";
    //            strworkingkey = "ESENGG";
    //        }
    //        else if (user_id == "ESEPTC")
    //        {
    //            strsenderid = "ESEPTC";
    //            strworkingkey = "ESEPTC";
    //        }
    //        else if (user_id == "ESMSCH")
    //        {
    //            strsenderid = "ESMSCH";
    //            strworkingkey = "ESMSCH";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            strsenderid = "GKMCET";
    //            strworkingkey = "GKMCET";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            strsenderid = "IJAYAM";
    //            strworkingkey = "IJAYAM";
    //        }
    //        else if (user_id == "JJAAMC")
    //        {
    //            strsenderid = "JJAAMC";
    //            strworkingkey = "JJAAMC";
    //        }

    //        else if (user_id == "KINGSE")
    //        {
    //            strsenderid = "KINGSE";
    //            strworkingkey = "KINGSE";
    //        }
    //        else if (user_id == "KNMHSS")
    //        {
    //            strsenderid = "KNMHSS";
    //            strworkingkey = "KNMHSS";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            strsenderid = "KSRIET";
    //            strworkingkey = "KSRIET";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            strsenderid = "KTVRKP";
    //            strworkingkey = "KTVRKP";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            strsenderid = "MPNMJS";
    //            strworkingkey = "MPNMJS";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            strsenderid = "NANDHA";
    //            strworkingkey = "NANDHA";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            strsenderid = "NECARE";
    //            strworkingkey = "NECARE";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            strsenderid = "NSNCET";
    //            strworkingkey = "NSNCET";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            strsenderid = "PETENG";
    //            strworkingkey = "PETENG";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            strsenderid = "PMCTEC";
    //            strworkingkey = "PMCTEC";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            strsenderid = "PPGITS";
    //            strworkingkey = "PPGITS";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            strsenderid = "PROFCL";
    //            strworkingkey = "PROFCL";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            strsenderid = "PSVCET";
    //            strworkingkey = "PSVCET";
    //        }
    //        else if (user_id == "SASTH")
    //        {
    //            strsenderid = "SASTH";
    //            strworkingkey = "SASTH";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            strsenderid = "SCTSBS";
    //            strworkingkey = "SCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            strsenderid = "SCTSCE";
    //            strworkingkey = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            strsenderid = "SCTSEC";
    //            strworkingkey = "SCTSEC";
    //        }
    //        else if (user_id == "SKCETC")
    //        {
    //            strsenderid = "SKCETC";
    //            strworkingkey = "SKCETC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            strsenderid = "SRECCG";
    //            strworkingkey = "SRECCG";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            strsenderid = "SLAECT";
    //            strworkingkey = "SLAECT";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            strsenderid = "SSCENG";
    //            strworkingkey = "SSCENG";
    //        }
    //        else if (user_id == "SSMCEE")
    //        {
    //            strsenderid = "SSMCEE";
    //            strworkingkey = "SSMCEE";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            strsenderid = "SVICET";
    //            strworkingkey = "SVICET";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            strsenderid = "SVCTCG";
    //            strworkingkey = "SVCTCG";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            strsenderid = "SVSCBE";
    //            strworkingkey = "SVSCBE";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            strsenderid = "TECENG";
    //            strworkingkey = "TECENG";
    //        }
    //        else if (user_id == "TJENGG")
    //        {
    //            strsenderid = "TJENGG";
    //            strworkingkey = "TJENGG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            strsenderid = "TSMJCT";
    //            strworkingkey = "TSMJCT";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            strsenderid = "VCWSMS";
    //            strworkingkey = "VCWSMS";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            strsenderid = "VRSCET";
    //            strworkingkey = "VRSCET";
    //        }
    //        else if (user_id == "AUDIIT")
    //        {
    //            strsenderid = "AUDIIT";
    //            strworkingkey = "AUDIIT";
    //        }
    //        else if (user_id == "SAENGG")
    //        {
    //            strsenderid = "SAENGG";
    //            strworkingkey = "SAENGG";
    //        }

    //        else if (user_id == "STANE")
    //        {
    //            strsenderid = "STANES";
    //            Password = "STANES";
    //        }

    //        else if (user_id == "MBCBSE")
    //        {
    //            strsenderid = "MBCBSE";
    //            strworkingkey = "MBCBSE";
    //        }

    //        else if (user_id == "HIETPT")
    //        {
    //            strsenderid = "HIETPT";
    //            strworkingkey = "HIETPT";
    //        }

    //        else if (user_id == "SVPITM")
    //        {
    //            strsenderid = "SVPITM";
    //            strworkingkey = "SVPITM";
    //        }

    //        else if (user_id == "AUDCET")
    //        {
    //            strsenderid = "AUDCET";
    //            strworkingkey = "AUDCET";
    //        }
    //        else if (user_id == "AUDWOM")
    //        {
    //            strsenderid = "AUDWOM";
    //            strworkingkey = "AUDWOM";
    //        }

    //        else if (user_id == "AUDIPG")
    //        {
    //            strsenderid = "AUDIPG";
    //            strworkingkey = "AUDIPG";
    //        }

    //        else if (user_id == "MCCDAY")
    //        {
    //            strsenderid = "MCCDAY";
    //            strworkingkey = "MCCDAY";
    //        }

    //        else if (user_id == "MCCSFS")
    //        {
    //            strsenderid = "MCCSFS";
    //            strworkingkey = "MCCSFS";
    //        }
    //        else if (user_id == "JMHRSS")
    //        {
    //            strsenderid = "JMHRSS";
    //            strworkingkey = "JMHRSS";
    //        }
    //        else if (user_id == "JHSSCB")
    //        {
    //            strsenderid = "JHSSCB";
    //            strworkingkey = "JHSSCB";
    //        } 
    //        Session["api"] = user_id;
    //        Session["senderid"] = strsenderid;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            //string print = "";Hided by Manikandan 07/08/2013
            //string appPath = HttpContext.Current.Server.MapPath("~");Hided by Manikandan 07/08/2013
            string strexcelname = "";
            //if (appPath != "")
            //{
            strexcelname = txtexcelname.Text;
            //appPath = appPath.Replace("\\", "/");Hided by Manikandan 07/08/2013
            if (strexcelname != "")
            {
                lblnorec.Visible = false;
                //start==========Hided by Manikandan 07/08/2013
                //print = strexcelname;
                //FpSpread1.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet                    
                //End=============
                d2.printexcelreport(FpSpread1, strexcelname);//Added by Manikandan 07/08/2013
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);//Modified by Manikandan 07/8/2013
            }
            else
            {
                lblnorec.Text = "Please enter your Report Name";
                lblnorec.Visible = true;
            }
            //}
            txtexcelname.Text = "";
            txtexcelname.Focus();// added by sridhar 13 sep 2014
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }
    }

    protected void rdnbtndetails_CheckedChanged(object sender, EventArgs e)
    {
        rdnbtncount.Checked = false;
        rdnbtndetails.Checked = true;

        btngo_Click(sender, e);

    }

    protected void rbnsms_CheckedChanged(object sender, EventArgs e)
    {
        lblfrom.Visible = false;
        txtfrom.Visible = false;
        txtto.Visible = false;
        lblto.Visible = false;
        txtto.Visible = false;
        btnselect.Visible = false;
        btndelete.Visible = false;
        txtfrom.Text = "";
        txtto.Text = "";
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        panelnotification.Visible = false;
        FpSpread1.Visible = false;
        if (rbnsms.Checked == true)
        {
            rdnbtndetails.Visible = true;
            rdnbtncount.Visible = true;
        }
        else if (rbnnotification.Checked == true)
        {
            rdnbtndetails.Visible = false;
            rdnbtncount.Visible = false;
        }

    }

    protected void rbnnotification_CheckedChanged(object sender, EventArgs e)
    {
        lblfrom.Visible = false;
        txtfrom.Visible = false;
        txtto.Visible = false;
        lblto.Visible = false;
        txtto.Visible = false;
        btnselect.Visible = false;
        btndelete.Visible = false;
        txtfrom.Text = "";
        txtto.Text = "";
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        panelnotification.Visible = false;
        FpSpread1.Visible = false;
        if (rbnsms.Checked == true)
        {
            rdnbtndetails.Visible = true;
            rdnbtncount.Visible = true;
        }
        else if (rbnnotification.Checked == true)
        {
            rdnbtndetails.Visible = false;
            rdnbtncount.Visible = false;
        }

    }

    protected void chkdesignation_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdesignation.Checked == true)
        {
            for (int i = 0; i < chklstdesignation.Items.Count; i++)
            {
                chklstdesignation.Items[i].Selected = true;

                txtdesignation.Text = "Designation(" + (chklstdesignation.Items.Count) + ")";

            }
        }
        else
        {
            for (int i = 0; i < chklstdesignation.Items.Count; i++)
            {
                chklstdesignation.Items[i].Selected = false;
                txtdesignation.Text = "---Select---";
            }
        }

    }

    protected void chklstdesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        int branchcount = 0;
        string value = "";
        string code = "";

        for (int i = 0; i < chklstdesignation.Items.Count; i++)
        {
            if (chklstdesignation.Items[i].Selected == true)
            {
                value = chklstdesignation.Items[i].Text;
                code = chklstdesignation.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                txtdesignation.Text = "Designation(" + branchcount.ToString() + ")";
            }

        }
    }

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {

        int branchcount = 0;
        string value = "";
        string code = "";

        for (int i = 0; i < CheckBoxList1.Items.Count; i++)
        {
            if (CheckBoxList1.Items[i].Selected == true)
            {
                value = CheckBoxList1.Items[i].Text;
                code = CheckBoxList1.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                TextBox1.Text = "Department(" + branchcount.ToString() + ")";

            }
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox1.Checked == true)
        {
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                CheckBoxList1.Items[i].Selected = true;

                TextBox1.Text = "Department(" + (CheckBoxList1.Items.Count) + ")";

            }
        }
        else
        {
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                CheckBoxList1.Items[i].Selected = false;
                TextBox1.Text = "---Select---";
            }
        }
    }

    protected void Chkboxstafftype_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkboxstafftype.Checked == true)
        {
            for (int i = 0; i < Chhliststafftype.Items.Count; i++)
            {
                Chhliststafftype.Items[i].Selected = true;

                txtstafftype.Text = "Stafftype(" + (Chhliststafftype.Items.Count) + ")";

            }
        }
        else
        {
            for (int i = 0; i < Chhliststafftype.Items.Count; i++)
            {
                Chhliststafftype.Items[i].Selected = false;
                txtstafftype.Text = "---Select---";
            }
        }
        // bind_design();
    }

    protected void Chhliststafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        int branchcount = 0;
        string value = "";
        string code = "";

        for (int i = 0; i < Chhliststafftype.Items.Count; i++)
        {
            if (Chhliststafftype.Items[i].Selected == true)
            {
                value = Chhliststafftype.Items[i].Text;
                code = Chhliststafftype.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                txtstafftype.Text = "Stafftype(" + branchcount.ToString() + ")";
            }
        }
        //  bind_design();
    }

    protected void rdnbtncount_CheckedChanged(object sender, EventArgs e)
    {
        rdnbtndetails.Checked = false;
        rdnbtncount.Checked = true;
        btngo_Click(sender, e);
    }

    public string Between_String(string src, string findfrom, string findto)
    {
        int start = src.IndexOf(findfrom);
        int to = src.IndexOf(findto, start + findfrom.Length);
        if (start < 0 || to < 0) return "";
        string s = src.Substring(
                       start + findfrom.Length,
                       to - start - findfrom.Length);
        return s;
    }

    public void loadnotification()
    {
        try
        {
            lblfrom.Visible = false;
            txtfrom.Visible = false;
            txtto.Visible = false;
            lblto.Visible = false;
            txtto.Visible = false;
            btnselect.Visible = false;
            btndelete.Visible = false;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            strbatch = ""; strbranch = "";
            //------------Modify By M.SakthiPriya 16/12/2014-----------------------
            FpSpread1.Sheets[0].ColumnCount = 8;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Sender Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Time";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Type";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
            //-----------------End-------------------
            collegecode = ddlcollege.SelectedValue.ToString();
            FpSpread1.Sheets[0].Columns[0].Width = 25;
            FpSpread1.Sheets[0].RowCount = 0;
            string startdate = "";
            string enddate = "";
            FpSpread1.Sheets[0].AutoPostBack = true;
            string date = txtstartdate.Text;
            string[] splitdate = date.Split(new char[] { '/' });
            startdate = splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString();
            DateTime stdate = Convert.ToDateTime(startdate);

            string date1 = txtenddate.Text;
            string[] splitdate1 = date1.Split(new char[] { '/' });
            enddate = splitdate1[1].ToString() + "/" + splitdate1[0].ToString() + "/" + splitdate1[2].ToString();
            DateTime eddate = Convert.ToDateTime(enddate);

            string viewers = "Student";


            if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
            {
                errmsg.Text = "";
                for (int i = 0; i < Chkbat.Items.Count; i++)
                {
                    if (Chkbat.Items[i].Selected == true)
                    {
                        if (strbatch == "")
                        {
                            strbatch = "'" + Chkbat.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strbatch = strbatch + "," + "'" + Chkbat.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                if (strbatch != "")
                {
                    strbatch = " and r.batch_year in(" + strbatch + ")";
                }
                for (int i = 0; i < Chkdeg.Items.Count; i++)
                {
                    if (Chkdeg.Items[i].Selected == true)
                    {
                        if (strdegree == "")
                        {
                            strdegree = "'" + Chkdeg.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strdegree = strdegree + "," + "'" + Chkdeg.Items[i].Value.ToString() + "'";
                        }
                    }
                }


                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        if (strbranch == "")
                        {
                            strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                if (strbranch != "")
                {
                    strbranch = " and r.degree_code in(" + strbranch + ")";
                }
                int sccou = 0;
                if (chklstsection.Items.Count == 0)
                {
                    secvv = "";
                    strsec = "";
                }
                else
                {
                    for (int i = 1; i < chklstsection.Items.Count; i++)
                    {
                        if (chklstsection.Items[i].Selected == true)
                        {
                            sccou++;
                            if (secvv == "")
                            {
                                secvv = "and ( sections='' or sections is null or sections='" + chklstsection.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                secvv = secvv + "or sections='" + chklstsection.Items[i].Value.ToString() + "'";
                            }
                        }
                    }
                    if (sccou > 0)
                    {
                        secvv = secvv + ')';
                    }
                    strsec = " and r.sections in ( " + strsection + ")";
                }
                strcmdretrivesmsreport = "select r.stud_name,n.sender_id,viewrs,convert(varchar(10),notification_date,103) date,RIGHT(CONVERT(VARCHAR, notification_time, 100),7) as Time,CONVERT(VARCHAR, notification_time, 108) as Time1,subject,n.status,notification_date,notification_time from tbl_notification n,Registration r where n.isstaff=0 and n.viewrs=r.Roll_No and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strbatch + " " + strbranch + " " + secvv + " and convert(varchar(10),notification_date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) and n.college_code='" + collegecode + "' order by notification_date desc,notification_time desc";//------------Modify By M.SakthiPriya 16/12/2014-----------------------

            }
            else
            {
                string value = "";
                string code = "";
                viewers = "Staff";
                string staffvalue = "";
                string staffcode = "";
                string designvalue = "";

                if (rdbtnstudent.Checked == false && rdbtnstaff.Checked == true)
                {
                    for (int i = 0; i < Chhliststafftype.Items.Count; i++)
                    {
                        if (Chhliststafftype.Items[i].Selected == true)
                        {
                            value = Chhliststafftype.Items[i].Text;
                            staffcode = Chhliststafftype.Items[i].Value.ToString();


                            if (staffvalue == "")
                            {
                                staffvalue = "'" + value + "'";
                            }
                            else
                            {
                                staffvalue = staffvalue + "," + "'" + value + "'";
                            }

                        }
                    }

                    for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                    {
                        if (CheckBoxList1.Items[i].Selected == true)
                        {
                            value = CheckBoxList1.Items[i].Text;
                            code = CheckBoxList1.Items[i].Value.ToString();
                            if (deptvalue == "")
                            {
                                deptvalue = "'" + code + "'";
                            }
                            else
                            {
                                deptvalue = deptvalue + "," + "'" + code + "'";
                            }

                        }
                    }
                    for (int i = 0; i < chklstdesignation.Items.Count; i++)
                    {
                        if (chklstdesignation.Items[i].Selected == true)
                        {
                            value = chklstdesignation.Items[i].Text;
                            code = chklstdesignation.Items[i].Value.ToString();
                            if (designvalue == "")
                            {
                                designvalue = "'" + code + "'";
                            }
                            else
                            {
                                designvalue = designvalue + "," + "'" + code + "'";
                            }

                        }
                    }

                    strcmdretrivesmsreport = "select distinct sm.staff_name,n.sender_id,viewrs,convert(varchar(10),notification_date,103) date, RIGHT(CONVERT(VARCHAR, notification_time, 100),7) as Time,CONVERT(VARCHAR, notification_time, 108) as Time1, subject,n.status,notification_date,notification_time from staffmaster sm,stafftrans st,staff_appl_master sam,hrdept_master h, tbl_notification n where  n.viewrs=sm.staff_code and isstaff=1 and st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no and st.dept_code=h.dept_code  and  st.dept_code in ( " + deptvalue + " ) and st.desig_code in  ( " + designvalue + ") and resign = 0 and settled = 0 and latestrec = 1 and sm.college_code=h.college_code and convert(varchar(10),notification_date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) and n.college_code='" + collegecode + "' order by notification_date desc,notification_time desc";//------------Modify By M.SakthiPriya 16/12/2014-----------------------
                }
            }
            dssmsrpt = d2.select_method_wo_parameter(strcmdretrivesmsreport, "text");
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType text = new FarPoint.Web.Spread.TextCellType();//------------Added By M.SakthiPriya 16/12/2014--------
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].Columns[6].CellType = text;//------------Added By M.SakthiPriya 16/12/2014--------
            int sr = 0;
            if (dssmsrpt.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                lblfrom.Visible = true;
                txtfrom.Visible = true;
                txtto.Visible = true;
                lblto.Visible = true;
                txtto.Visible = true;
                btnselect.Visible = true;
                btndelete.Visible = false;
                txtfrom.Text = "";
                txtto.Text = "";
                for (int i = 0; i < dssmsrpt.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;

                    string sender = dssmsrpt.Tables[0].Rows[i]["sender_id"].ToString();//------------Added By M.SakthiPriya 16/12/2014-----------------------
                    string ndate = dssmsrpt.Tables[0].Rows[i]["date"].ToString();
                    string roll = dssmsrpt.Tables[0].Rows[i]["viewrs"].ToString();
                    string subject = dssmsrpt.Tables[0].Rows[i]["subject"].ToString();
                    string time = dssmsrpt.Tables[0].Rows[i]["Time"].ToString();
                    string notetime = dssmsrpt.Tables[0].Rows[i]["Time1"].ToString();
                    string name = "";
                    if (viewers == "Student")
                    {
                        name = dssmsrpt.Tables[0].Rows[i]["stud_name"].ToString();
                    }
                    if (viewers == "Staff")
                        name = dssmsrpt.Tables[0].Rows[i]["Staff_name"].ToString();
                    sr++;
                    string status = dssmsrpt.Tables[0].Rows[i]["status"].ToString();
                    if (status == "1")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Green;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Bold = false;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Size = FontUnit.Small;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                    }

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sr.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = name + '-' + roll.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = roll;
                    //------------Modify By M.SakthiPriya 16/12/2014-----------------------
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = sender.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ndate.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = time.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = notetime;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = subject.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = viewers;
                }
                FpSpread1.Sheets[0].Columns[7].CellType = cb;
                //-------------End------------
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
            }
            else
            {
                FpSpread1.Visible = false;
                norecordlbl.Visible = true;
                norecordlbl.Text = "No Records Found";
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
        }
    }

    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Cellclick = true;
    }

    protected void FpSpread1_PreRender(Object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                if (rbnnotification.Checked == true && rbnsms.Checked == false || Session["Staff_Code"] != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    int ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
                    int ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
                    //------------Modify By M.SakthiPriya 16/12/2014-----------------------
                    string roll = FpSpread1.Sheets[0].Cells[ar, 1].Tag.ToString();
                    string date = FpSpread1.Sheets[0].Cells[ar, 3].Text.ToString();
                    string subject = FpSpread1.Sheets[0].Cells[ar, 5].Text.ToString();
                    //------------------End-----------------------
                    string isstaff = "0";
                    FpSpread1.Sheets[0].Rows[ar].BackColor = Color.Green;
                    if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
                    {
                        isstaff = "0";
                    }
                    else if (rdbtnstudent.Checked == false && rdbtnstaff.Checked == true)
                    {
                        isstaff = "1";
                    }
                    if (rdbtnstudent.Enabled = true && rdbtnstaff.Enabled == true)
                    {
                        collegecode = ddlcollege.SelectedValue.ToString();
                    }
                    else
                    {
                        collegecode = Session["collegecode"].ToString();
                        isstaff = "1";
                    }
                    string[] dt = date.Split('/');
                    date = dt[1] + '/' + dt[0] + '/' + dt[2];
                    string time = FpSpread1.Sheets[0].Cells[ar, 4].Tag.ToString();//------------Modify By M.SakthiPriya 16/12/2014-----------------------

                    string strquery = "select viewrs,subject,fileupload,convert(varchar(10),notification_date,103) date,RIGHT(CONVERT(VARCHAR, notification_time, 100),7) as Time,notification,status,sender_id,Sender_Description,attache_file,attche_filetype,filename from tbl_notification where viewrs='" + roll + "' and College_Code='" + collegecode + "' and notification_date='" + date + "'  and notification_time='" + time + "'";
                    ds.Dispose();
                    ds.Reset();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        panelnotification.Visible = true;
                        roll = ds.Tables[0].Rows[0]["viewrs"].ToString();
                        subject = ds.Tables[0].Rows[0]["Subject"].ToString();
                        txtnotification.Text = ds.Tables[0].Rows[0]["notification"].ToString();

                        if (ds.Tables[0].Rows[0]["status"].ToString().Trim() != "1")
                        {
                            strquery = "update tbl_notification set status=1 where viewrs='" + roll + "' and isstaff=" + isstaff + " and notification_date='" + date + "' and notification_time='" + time + "'";
                            int update = d2.update_method_wo_parameter(strquery, "text");
                        }

                        date = ds.Tables[0].Rows[0]["date"].ToString();
                        string senderdetails = ds.Tables[0].Rows[0]["sender_id"].ToString() + " - " + ds.Tables[0].Rows[0]["Sender_Description"].ToString();
                        lblsender.Text = senderdetails;
                        lblsubject.Text = subject;
                        string[] spdt = date.Split('/');
                        time = ds.Tables[0].Rows[0]["Time"].ToString();
                        string[] spti = FpSpread1.Sheets[0].Cells[ar, 4].Tag.ToString().Split(':');//------------Modify By M.SakthiPriya 16/12/2014-----------------------

                        string notificationimage = roll + spdt[0] + spdt[1] + spdt[2] + spti[0] + spti[1] + spti[2];
                        lblndate.Text = spdt[0] + '/' + spdt[1] + '/' + spdt[2] + ' ' + time;
                        if (ds.Tables[0].Rows[0]["attache_file"].ToString().Length > 0 && ds.Tables[0].Rows[0]["attache_file"] != null)
                        {
                            btnattachement.Visible = true;
                        }
                        else
                        {
                            btnattachement.Visible = false;
                        }

                        try
                        {
                            if (ds.Tables[0].Rows[0]["fileupload"].ToString().Length > 0 && ds.Tables[0].Rows[0]["fileupload"] != null)
                            {
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + notificationimage + ".jpeg")))
                                {

                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["fileupload"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + notificationimage + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                                imgnotification.ImageUrl = "~/college/" + notificationimage + ".jpeg";
                            }
                            else
                            {
                                imgnotification.ImageUrl = "~/college/NoImage.jpg";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //try
                        //{
                        //if (ds.Tables[0].Rows[0]["attache_file"].ToString().Length > 0 && ds.Tables[0].Rows[0]["attache_file"] != null)
                        //{
                        //    Response.ContentType = ds.Tables[0].Rows[0]["attche_filetype"].ToString();
                        //    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ds.Tables[0].Rows[0]["filename"].ToString() + "\"");
                        //    Response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["attache_file"]);
                        //    Response.End();
                        //}

                        //}
                        //catch
                        //{
                        //}
                    }

                    Cellclick = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnnok_Click(Object sender, EventArgs e)
    {
        panelnotification.Visible = false;
    }

    public void loadindividualnote()
    {

        txtfrom.Text = "";
        txtto.Text = "";
        btndelete.Visible = false;

        FpSpread1.Visible = false;
        lblfrom.Visible = false;
        txtfrom.Visible = false;
        txtto.Visible = false;
        lblto.Visible = false;
        txtto.Visible = false;
        btnselect.Visible = false;

        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
        strbatch = ""; strbranch = "";
        FpSpread1.Sheets[0].ColumnCount = 6;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 8;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Sender Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Time";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Type";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
        strcmdretrivesmsreport = "select s.staff_name,n.idno,n.viewrs,convert(varchar(10),n.notification_date,103) date, RIGHT(CONVERT(VARCHAR, n.notification_time, 100),7) as Time,CONVERT(VARCHAR, n.notification_time, 108) as Time1, subject,status,n.sender_id,notification_date,notification_time from tbl_notification n,staffmaster s where s.staff_code=n.viewrs and  viewrs='" + Session["Staff_Code"].ToString() + "' and n.College_Code=" + Session["collegecode"].ToString() + "   order by n.notification_date desc,n.notification_time desc";// change by sridhar 08 sep 2014
        dssmsrpt = d2.select_method_wo_parameter(strcmdretrivesmsreport, "text");
        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].RowCount = 0;
        int sr = 0;
        if (dssmsrpt.Tables[0].Rows.Count > 0)
        {
            FpSpread1.Visible = true;
            lblfrom.Visible = true;
            txtfrom.Visible = true;
            txtto.Visible = true;
            lblto.Visible = true;
            txtto.Visible = true;
            btnselect.Visible = true;

            string viewers = "Staff";

            for (int i = 0; i < dssmsrpt.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].RowCount++;

                string sender = dssmsrpt.Tables[0].Rows[i]["sender_id"].ToString();//------------Added By M.SakthiPriya 16/12/2014-----------------------
                string ndate = dssmsrpt.Tables[0].Rows[i]["date"].ToString();
                string roll = dssmsrpt.Tables[0].Rows[i]["viewrs"].ToString();
                string subject = dssmsrpt.Tables[0].Rows[i]["subject"].ToString();
                string time = dssmsrpt.Tables[0].Rows[i]["Time"].ToString();
                string notetime = dssmsrpt.Tables[0].Rows[i]["Time1"].ToString();
                string name = dssmsrpt.Tables[0].Rows[i]["staff_name"].ToString();
                sr++;
                string status = dssmsrpt.Tables[0].Rows[i]["status"].ToString();
                if (status == "1")
                {
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Green;
                }

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sr.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = name + '-' + roll.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = roll;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                //------------Modify By M.SakthiPriya 16/12/2014-----------------------
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = sender.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ndate.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = time.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = notetime;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = subject.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = viewers;
            }
            FpSpread1.Sheets[0].Columns[7].CellType = cb;
        }
        else
        {
            FpSpread1.Visible = false;
            norecordlbl.Visible = true;
            norecordlbl.Text = "No Records Found";
        }
    }

    protected void btnprintmaster_Click(Object sender, EventArgs e)
    {

        string degreedetails = "smsreport" + '@' + "Date :" + txtstartdate.Text.ToString() + " To " + txtenddate.Text.ToString();
        string pagename = "smsreport.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnattachement_Click(Object sender, EventArgs e)
    {
        try
        {
            int ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            int ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            //------------Modify By M.SakthiPriya 16/12/2014-----------------------
            string roll = FpSpread1.Sheets[0].Cells[ar, 1].Tag.ToString();
            string date = FpSpread1.Sheets[0].Cells[ar, 3].Text.ToString();
            string subject = FpSpread1.Sheets[0].Cells[ar, 5].Text.ToString();
            //------------------------------------------------------
            string isstaff = "0";
            FpSpread1.Sheets[0].Rows[ar].BackColor = Color.Green;
            if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
            {
                isstaff = "0";
            }
            else if (rdbtnstudent.Checked == false && rdbtnstaff.Checked == true)
            {
                isstaff = "1";
            }
            if (rdbtnstudent.Enabled = true && rdbtnstaff.Enabled == true)
            {
                collegecode = ddlcollege.SelectedValue.ToString();
            }
            else
            {
                collegecode = Session["collegecode"].ToString();
                isstaff = "1";
            }
            string[] dt = date.Split('/');
            date = dt[1] + '/' + dt[0] + '/' + dt[2];
            string time = FpSpread1.Sheets[0].Cells[ar, 4].Tag.ToString();  //------------Modify By M.SakthiPriya 16/12/2014-----------------------




            string strquery = "select attache_file,attche_filetype,filename from tbl_notification where viewrs='" + roll + "' and College_Code='" + collegecode + "' and notification_date='" + date + "'  and notification_time='" + time + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["attache_file"].ToString().Length > 0 && ds.Tables[0].Rows[0]["attache_file"] != null)
                {
                    Response.ContentType = ds.Tables[0].Rows[0]["attche_filetype"].ToString();
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ds.Tables[0].Rows[0]["filename"].ToString() + "\"");
                    Response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["attache_file"]);
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Attachements')", true);
                }
            }
        }
        catch
        {
        }
    }

    protected void btnselect_Click(Object sender, EventArgs e)
    {
        try
        {
            btndelete.Visible = false;
            int frange = 0;
            int trange = 0;
            int totrow = FpSpread1.Sheets[0].RowCount;
            if (FpSpread1.Visible == true)
            {
                if (txtfrom.Text.Trim() != "" && txtfrom.Text != null && txtfrom.Text.Trim() != "0")
                {
                    frange = Convert.ToInt32(txtfrom.Text);
                }
                else
                {
                    norecordlbl.Visible = true;
                    norecordlbl.Text = "Please Enter From Value";
                    return;
                }
                if (txtto.Text.Trim() != "" && txtto.Text != null && txtto.Text.Trim() != "0")
                {
                    trange = Convert.ToInt32(txtto.Text);
                }
                else
                {
                    norecordlbl.Visible = true;
                    norecordlbl.Text = "Please Enter To Value";
                    return;
                }
                if (frange > trange)
                {
                    norecordlbl.Visible = true;
                    norecordlbl.Text = "To Value Must Be Greater Than or Equal To From Value";
                    return;
                }
                if (frange > FpSpread1.Sheets[0].RowCount || trange > FpSpread1.Sheets[0].RowCount)
                {
                    norecordlbl.Visible = true;
                    norecordlbl.Text = "From Value and To Value Must Be Lesser Than or Equal To " + FpSpread1.Sheets[0].RowCount + "";
                    return;
                }
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    btndelete.Visible = true;
                    frange = frange - 1;
                    trange = trange - 1;
                    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        if (frange <= i && trange >= i)
                        {
                            FpSpread1.Sheets[0].Cells[i, 7].Value = 1;//------------Modify By M.SakthiPriya 16/12/2014-----------------------
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[i, 7].Value = 0;//------------Modify By M.SakthiPriya 16/12/2014-----------------------
                        }
                    }
                }
            }
            FpSpread1.SaveChanges();
        }
        catch
        {
        }

    }

    protected void btndelete_Click(Object sender, EventArgs e)
    {
        try
        {
            string getrights = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                getrights = d2.GetFunction("select rights_code from security_user_right where college_code=" + Session["collegecode"] + " and group_code='" + group_user + "' and rights_code='90002'");
            }
            else
            {
                getrights = d2.GetFunction("select rights_code from security_user_right where college_code=" + Session["collegecode"] + " and user_code='" + Session["UserCode"] + "' and rights_code='90002'");
            }
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                Boolean delflag = false;
                for (int jk = 0; jk < FpSpread1.Sheets[0].RowCount; jk++)
                {
                    //------------Modify By M.SakthiPriya 16/12/2014-----------------------
                    string value = Convert.ToString(FpSpread1.Sheets[0].Cells[jk, 7].Value);
                    if (value == "1")
                    {
                        delflag = true;
                        string staff_code = Convert.ToString(FpSpread1.Sheets[0].Cells[jk, 1].Tag);
                        string ndate = Convert.ToString(FpSpread1.Sheets[0].Cells[jk, 3].Text);
                        string ntime = Convert.ToString(FpSpread1.Sheets[0].Cells[jk, 4].Tag);
                        //-------------------------End----------------------------
                        string[] dt = ndate.Split('/');
                        ndate = dt[1] + '/' + dt[0] + '/' + dt[2];

                        string deletequery = "delete tbl_notification where viewrs ='" + staff_code + "' and notification_date='" + ndate + "' and notification_time='" + ntime + "' ";
                        if (getrights.Trim() == "90002")
                        {
                            deletequery = deletequery + " and College_Code=" + ddlcollege.SelectedItem.Value + "";
                        }
                        int a = d2.update_method_wo_parameter(deletequery, "Text");
                    }
                }
                if (delflag == true)
                {
                    if (getrights.Trim() == "90002")
                    {
                        loadnotification();
                    }
                    else
                    {
                        loadindividualnote();
                    }
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                }
            }

        }
        catch
        {

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
        lbl.Add(lbldeg);
        lbl.Add(lblbranch);
        //lbl.Add(lblSem1);
        //lbl.Add(lblSem);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

        SchoolCollege = new Institution(grouporusercode);
        schoolOrCollege = SchoolCollege.TypeInstitute;
    }

    private string Decryptdata(string encryptpwd)
    {
      
        string decryptpwd = string.Empty;
       // encryptpwd = "QyQxMyZVJDIyMSZHJDAmUyRUcnVlJlAkMSZWJElucy0yMDE4LjEmU0MkJlVOJGFkbWluJkUkMzc4ODE3JkwkMSZDdSQxMyZJQyQxMyZQYSQmUFYkZmFsc2UmRCQxJk0kd3Jvbmc=";

       
        UTF8Encoding encodepwd = new UTF8Encoding();

        Decoder Decode = encodepwd.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
        int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        decryptpwd = new String(decoded_char);
        return decryptpwd;
    }


}

