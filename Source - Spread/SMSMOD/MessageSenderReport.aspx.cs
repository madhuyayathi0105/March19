using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Web;
public partial class MessageSenderReport : System.Web.UI.Page
{
    #region "Variable Declaration"
    static Boolean forschoolsetting = false;// Added by sridharan
    int countperdate = 0;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
    string deptvalue = "";
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
    //added bu srinath 2/2/2013
    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;
    string send_mail = string.Empty;
    string send_pw = string.Empty;
    string to_mail = string.Empty;
    string strstuname = string.Empty;
    DAccess2 da = new DAccess2();
    bool flagstudent;
    Boolean spreadgo = false;
    public SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    private WebProxy objProxy1 = null;
    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    static int sectioncnt = 0;
    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    int sno = 0;
    Boolean Cellclick;
    Boolean flag_true = false;
    Institution SchoolCollege;
    byte schoolOrCollege = 0;
    string mobilenos = "";
    string PageLogOut = "";
    #endregion

    #region "Hash Table Declaration"
    Hashtable hat = new Hashtable();
    #endregion

    #region "Dataset Declaration"
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet1();
    DataSet ds2 = new DataSet1();
    DataSet dsbind = new DataSet();
    DataTable dt = new DataTable();
    DataSet ds3 = new DataSet();
    #endregion

    #region "Page Load Event"
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            setLabelText();
            if (!IsPostBack)
            {
                clearnotification();
                rdbtnstudent.Checked = true;
                rdbtnstaff.Checked = false;
                chkvoicecall.Visible = true;
                tblmail.Visible = false;
                FpSpread2.Sheets[0].AutoPostBack = true;
                FpSpread1.Width = 1000;
                //FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = false;
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
                txtsub.Text = "";
                txtbody.Text = "";

                // smscreditcountperdate();
                // Fpspreadvoice.Visible = false;
                // Fpspreadvoice.Sheets[0].Visible = false;
                // Fpspreadvoice.Sheets[0].RowCount = 1;
                // Fpspreadvoice.Sheets[0].ColumnCount = 1;
                //// bindspreadvoice();
                //// panelvoice.Visible = true;
                // Fpspreadvoice.Sheets[0].Visible = false;
                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
                FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
                FpSpread1.Pager.PageCount = 5;
                FpSpread1.Visible = false;
                FpSpread2.Visible = false;
                fpMsg.Visible = false;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                btnxl.Visible = false;
                // btnsms.Visible = false;
                btnsend.Visible = false;
                Div7.Visible = false;
                Divv1.Visible = false;
                Divv2.Visible = false;
                Div5.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                txtmessage.Visible = false;
                lblsendmail.Visible = false;
                BindCollege(sender, e);
                rdbtnstudent_CheckedChanged(sender, e);
                Txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");//Added by saranya on 3/9/2018
                if (rdbtnstudent.Checked == true)
                {
                    fve.Visible = true;
                    fvehicletype.Visible = true;
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
                    chkstudent.Checked = true;
                    lbldeg.Text = SchoolCollege.InsDegree;
                    lblbranch.Text = SchoolCollege.InsBranch;
                    batch();
                    BindDegree();
                    if (Chkdeg.Items.Count > 0)
                    {
                        //  BindDegree(singleuser, group_user, collegecode, usercode);
                        BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);
                        BindSectionDetail(strbatch, strbranch);
                    }
                    else
                    {
                        chklstbranch.Items.Clear();
                        chklstsection.Items.Clear();
                    }
                    loadreligion();
                }
                else if (rdbtnstaff.Checked == true)
                {
                    BindDesignation();
                    bindept();
                    bindstafftype();

                }
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
                bindroute();
                bindvechileid();
                loadvechilestage();
                getVehicleType();
            }
            setLabelText();
            errnote.Visible = false;
        }
        catch
        {
        }
    }
    
    #endregion

    #region "Page Load Function"
    //public void PageLoad(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        usercode = Session["usercode"].ToString();
    //        collegecode = Session["InternalCollegeCode"].ToString();
    //        singleuser = Session["single_user"].ToString();
    //        group_user = Session["group_code"].ToString();
    //        rdbtnstudent.Checked = true;
    //        rdbtnstaff.Checked = false;
    //        FpSpread2.Sheets[0].AutoPostBack = true;
    //        FpSpread1.Width = 1000;
    //        // FpSpread1.Sheets[0].AutoPostBack = true;
    //        FpSpread1.CommandBar.Visible = true;
    //        FpSpread1.Sheets[0].SheetName = " ";
    //        FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
    //        FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
    //        FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
    //        FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
    //        style1.Font.Size = 12;
    //        style1.Font.Bold = true;
    //        style1.HorizontalAlign = HorizontalAlign.Center;
    //        style1.ForeColor = System.Drawing.Color.Black;
    //        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].AllowTableCorner = true;
    //        FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
    //        FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
    //        FpSpread1.Pager.Align = HorizontalAlign.Right;
    //        FpSpread1.Pager.Font.Bold = true;
    //        FpSpread1.Pager.Font.Name = "Book Antiqua";
    //        FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
    //        FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
    //        FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
    //        FpSpread1.Pager.PageCount = 5;
    //        FpSpread1.Visible = false;
    //        FpSpread2.Visible = false;
    //        lblpurpose1.Visible = false;
    //        ddlpurpose.Visible = false;
    //        btnxl.Visible = false;
    //        btnsms.Visible = false;
    //        btnaddtemplate.Visible = false;
    //        btndeletetemplate.Visible = false;
    //        txtmessage.Visible = false;
    //        lblsendmail.Visible = false;
    //        rdbtnstudent_CheckedChanged(sender, e);
    //        if (rdbtnstudent.Checked == true)
    //        {
    //            staffpanel.Visible = false;
    //            lblbatch.Visible = true;
    //            tbbat.Visible = true;
    //            pbat.Visible = true;
    //            Chkbatsel.Visible = true;
    //            Chkbat.Visible = true;
    //            lblsection.Visible = true;
    //            txtsection.Visible = true;
    //            psection.Visible = true;
    //            chklstsection.Visible = true;
    //            chksection.Visible = true;
    //            lbldeg.Text = "Degree";
    //            lblbranch.Text = "Branch";
    //            batch();
    //            BindDegree();
    //            if (Chkdeg.Items.Count > 0)
    //            {
    //                BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);
    //                BindSectionDetail(strbatch, strbranch);
    //            }
    //            else
    //            {
    //            }
    //        }
    //        else if (rdbtnstaff.Checked == true)
    //        {
    //            BindDesignation();
    //            bindept();
    //            bindstafftype();
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
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
                //PageLoad(sender, e);
            }
        }
        catch
        {
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
        try
        {
            Chkbatsel.Checked = false;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
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
                Chkbatsel.Checked = true;
            }
        }
        catch
        {
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
        try
        {
            Chkdeg.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, ddlcollege.SelectedValue.ToString(), usercode);
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
                Chkdegsel.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void loadreligion()
    {
        try
        {
            txtregion.Text = "---Select---";
            chkregion.Checked = false;
            chklsregion.Items.Clear();
            string strquery = "select textval,TextCode from TextValTable where TextCriteria='relig' and college_code='" + ddlcollege.SelectedValue.ToString() + "' order by TextVal";
            DataSet dsreligion = d2.select_method_wo_parameter(strquery, "Text");
            if (dsreligion.Tables[0].Rows.Count > 0)
            {
                chklsregion.DataSource = dsreligion;
                chklsregion.DataTextField = "textval";
                chklsregion.DataValueField = "TextCode";
                chklsregion.DataBind();
                int icou = 0;
                for (int c = 0; c < chklsregion.Items.Count; c++)
                {
                    chklsregion.Items[c].Selected = true;
                    icou++;
                }
                if (icou > 0)
                {
                    txtregion.Text = "Religion (" + icou + ")";
                    if (icou == chklsregion.Items.Count)
                    {
                        chkregion.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void Chkbatsel_CheckedChanged(object sender, EventArgs e)
    {
        try
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
            BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);
            BindSectionDetail(strbatch, strbatch);
        }
        catch
        {
        }
    }
    protected void Chkdegsel_CheckedChanged(object sender, EventArgs e)
    {
        try
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
            BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);
            BindSectionDetail(strbatch, strbranch);
        }
        catch
        {
        }
    }
    protected void chkregion_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkregion.Checked == true)
            {
                for (int c = 0; c < chklsregion.Items.Count; c++)
                {
                    chklsregion.Items[c].Selected = true;
                }
                txtregion.Text = "Religion (" + chklsregion.Items.Count + ")";
                chkregion.Checked = true;
            }
            else
            {
                for (int c = 0; c < chklsregion.Items.Count; c++)
                {
                    chklsregion.Items[c].Selected = false;
                }
                txtregion.Text = "---Select---";
                chkregion.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void chklsregion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtregion.Text = "---Select---";
            chkregion.Checked = false;
            int icou = 0;
            for (int c = 0; c < chklsregion.Items.Count; c++)
            {
                if (chklsregion.Items[c].Selected == true)
                {
                    icou++;
                }
            }
            if (icou > 0)
            {
                txtregion.Text = "Religion (" + icou + ")";
                if (icou == chklsregion.Items.Count)
                {
                    chkregion.Checked = true;
                }
            }
        }
        catch
        {
        }
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
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);
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
                chkbranch.Checked = true;
            }
            BindSectionDetail(strbatch, strbranch);
        }
        catch
        {
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
                ds2 = d2.BindSectionDetail(strbatch, strbranch);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstsection.Items.Insert(0, " ");
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
        catch
        {
        }
    }
    #endregion

    protected void Chkdeg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
            BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);
        }
        catch
        {
        }
    }

    #region "Load Function for Designation Details"

    public void BindDesignation()
    {
        try
        {
            count = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.binddesi(ddlcollege.SelectedValue.ToString());
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
        catch
        {
        }
    }

    #endregion

    #region "Load Function for Department Details"

    public void BindDepartment()
    {
        try
        {
            count = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.loaddepartment(ddlcollege.SelectedValue.ToString());
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
        catch
        {
        }
    }

    #endregion

    public void bindept()
    {
        try
        {
            count = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.loaddepartment(ddlcollege.SelectedValue.ToString());
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
        catch
        {
        }
    }

    #region "College Dropdown Selected Index Changed Event"

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblmsgcredit.Text = "SMS Available Credits :0";
            smscreditcountperdate();
            FpSpread1.Visible = false;
            FpSpread2.Visible = false;
            //chkpurpose.Visible = false;
            lblpurpose1.Visible = false;
            ddlpurpose.Visible = false;
            btnxl.Visible = false;
            //btnsms.Visible = false;
            btnaddtemplate.Visible = false;
            btndeletetemplate.Visible = false;
            // added by sridhar 08 sep 2014 start
            lblsubject.Visible = false;
            lblnotification.Visible = false;
            txtsubject.Visible = false;
            lblnote.Visible = false;
            txtnotification.Visible = false;
            lblfile.Visible = false;
            lblattachements.Visible = false;
            fudfile.Visible = false;
            fudattachemnts.Visible = false;
            //btnnotfsave.Visible = false;
            batch();
            BindDegree();// added by sridhar 08 sep2014
            BindDepartment();
            bindept();
            loadreligion();
            bindroute();
            bindvechileid();
            loadvechilestage();
            getVehicleType();
            // added by sridhar 08 sep 2014 end
            txtmessage.Visible = false;
            if (Convert.ToString(Session["QueryString"]) != "")
            {
                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                // make collection editable
                isreadonly.SetValue(this.Request.QueryString, false, null);
                // remove
                this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
                Request.QueryString.Clear();
            }
            Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
            // PageLoad(sender, e);
            string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + ddlcollege.SelectedValue.ToString() + "'";
            ds1.Dispose();
            ds1.Reset();
            ds1 = d2.select_method(strsenderquery, hat, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
            }
            //modified by srinath 1/8/2014
            //GetUserapi(user_id);
            string getval = d2.GetUserapi(user_id);
            string[] spret = getval.Split('-');
            if (spret.GetUpperBound(0) == 1)
            {
                SenderID = spret[0].ToString();
                Password = spret[1].ToString();
                Session["api"] = user_id;
                Session["senderid"] = SenderID;
            }
            if (SenderID != "" && Password != "")
            {
                lblmsgcredit.Visible = true;
                //Modified By Srinath 8/2/2014
                //WebRequest request = WebRequest.Create("http://inter.onlinespeedsms.in/api/balance.php?user=" + SenderID.ToLower() + "&password=" + Password + "&type=4");
                //WebRequest request = WebRequest.Create("http://pr.airsmsmarketing.info/api/checkbalance.php?user=" + SenderID + "&pass=" + Password + "");
                WebRequest request = WebRequest.Create("http://hp.dial4sms.com/balalert/main.php?uname=" + SenderID + "&pass=" + Password + "");
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

    #region "Radio Button Checked Change Event"

    protected void rdbtnstudent_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Tablenote.Visible = false;
            tbdeg.Text = "--Select--";
            txtbranch.Text = "--Select--";
            studentpanel.Visible = true;
            Panel1244.Visible = true;
            chksmsgroup.Visible = false;
            Button2.Visible = false;
            chkboxsms.Checked = false;
            chkboxmail.Checked = false;
            chknotification.Checked = false;
            chkvoicecall.Checked = false;
            Div5.Visible = false;
            Divv2.Visible = false;
            Divv1.Visible = false;
            Div7.Visible = false;
            btnsend.Visible = false;
            //Added by saranya
            lbl_Date.Visible = false;
            Txtdate.Visible = false;
            PnlPorAColor.Visible = false;
            //===============//
            if (rdbtnstudent.Checked == true)
            {
                FpSpread3.Visible = false;
                fve.Visible = true;
                fvehicletype.Visible = true;
                chkvoicecall.Visible = true;
                staffpanel.Visible = false;
                FpSpread1.Visible = false;
                FpSpread2.Visible = false;
                fpMsg.Visible = false;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                txtmessage.Visible = false;
                //btnsms.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
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
                lblregion.Visible = true;
                txtregion.Visible = true;
                chkstudent.Visible = true;
                chkfather.Visible = true;
                chkmother.Visible = true;
                chkstudent.Checked = true;
                chkfather.Checked = false;
                chkmother.Checked = false;
                txtbody.Text = "";
                txtsub.Text = "";
                txtmessage.Text = "";
                txtnotification.Text = "";
                //lbldeg.Text = "Degree";
                //lblbranch.Text = "Branch";
                batch();
                BindDegree();
                if (Chkdeg.Items.Count > 0)
                {
                    BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);
                    BindSectionDetail(strbatch, strbranch);
                }
                else
                {
                    chklstbranch.Items.Clear();
                    chklstsection.Items.Clear();
                }
            }
        }
        catch
        {
        }
    }

    protected void rdbtnstaff_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread3.Visible = false;
            Tablenote.Visible = false;
            studentpanel.Visible = false;
            staffpanel.Visible = true;
            rdbtnstudent.Checked = false;
            Panel1244.Visible = false;
            pndes1.Width = 246;
            chksmsgroup.Visible = true;
            chksmsgroup.Checked = false;
            Button2.Visible = false;
            fve.Visible = true;
            fvehicletype.Visible = true;
            chkboxsms.Checked = false;
            chkboxmail.Checked = false;
            chknotification.Checked = false;
            chkvoicecall.Checked = false;
            Div5.Visible = false;
            Divv2.Visible = false;
            Divv1.Visible = false;
            Div7.Visible = false;
            btnsend.Visible = false;
            //tbdeg.Text = "--Select--";width: 246px;
            //txtbranch.Text = "--Select--";

            //Added by saranya
            lbl_Date.Visible = true;
            Txtdate.Visible = true;
            PnlPorAColor.Visible = true;
            //===============//
            if (rdbtnstaff.Checked == true)
            {
                FpSpread1.Visible = false;
                FpSpread2.Visible = false;
                //chkpurpose.Visible = false;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                txtmessage.Visible = false;
                //btnsms.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                rdbtnstudent.Checked = false;
                chkstudent.Visible = false;
                chkfather.Visible = false;
                chkmother.Visible = false;
                chkstudent.Checked = false;
                chkfather.Checked = false;
                chkmother.Checked = false;
                lblregion.Visible = false;
                txtregion.Visible = false;
                bindept();
                bindstafftype();
                BindDesignation();
                txtbody.Text = "";
                txtsub.Text = "";
                txtmessage.Text = "";
                txtnotification.Text = "";
            }
        }
        catch
        {
        }
    }

    protected void rbnotification_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            clearnotification();
        }
        catch
        {
        }
    }

    #endregion

    //added by annyutha//
    protected void smscreditcountperdate()
    {
        string todaydate = DateTime.Now.ToString("yyyy-MM-dd");
        string datecreate = "select groupmessageid from smsdeliverytrackmaster where date='" + todaydate + "' and groupmessageid!='No Sufficient Credits'";
        DataSet creiddate = new DataSet();
        DAccess2 credite = new DAccess2();
        creiddate = credite.select_method_wo_parameter(datecreate, "text");
        if (creiddate.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < creiddate.Tables[0].Rows.Count; i++)
            {
                string date1 = creiddate.Tables[0].Rows[i]["groupmessageid"].ToString();
                string[] split = date1.Split(new Char[] { ' ' });
                for (int k = 0; k <= split.GetUpperBound(0); k++)
                {
                    if (split[k].ToString().Trim() != "")
                    {
                        countperdate++;
                    }
                }
            }
        }
        lblmsgused.Text = "Credits User Today:" + countperdate + "";
    }

    public void bindstafftype()
    {
        try
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
                //ddl_stftype.DataSource = ds;
                //ddl_stftype.DataTextField = "StfType";
                //ddl_stftype.DataValueField = "StfType";
                //ddl_stftype.DataBind();
            }
            mysql.Close();
        }
        catch
        {
        }
    }

    void bind_design()
    {
        try
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
        catch
        {
        }
    }

    #region "Batch Dropdown Extender"
    protected void Chkbat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
            BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);
        }
        catch
        {
        }
    }
    protected void LinkButtonbatch_Click(object sender, EventArgs e)
    {
        try
        {
            Chkbat.ClearSelection();
            batchcnt = 0;
            tbbat.Text = "---Select---";
        }
        catch
        {
        }
    }
    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        try
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
        catch
        {
        }
    }
    public Label batchlabel()
    {
        try
        {
            Label lbc = new Label();
            ViewState["lseatcontrol"] = true;
            return (lbc);
        }
        catch
        {
            return null;
        }
    }
    public ImageButton batchimage()
    {
        try
        {
            ImageButton imc = new ImageButton();
            imc.ImageUrl = "xb.jpeg";
            imc.Height = 9;
            imc.Width = 9;
            ViewState["iseatcontrol"] = true;
            return (imc);
        }
        catch
        {
            return null;
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
                chklstbranch.Items[i].Selected = true;//((schoolOrCollege == 0) ? SchoolCollege.InsBranch : "") 
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
        //  BindSectionDetail(strbatch, strbranch);
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

    #region "GO Button Function"

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            smscreditcountperdate();
            FpSpread1.CommandBar.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            int checkcount_batch = 0;
            int checkcount_deg = 0;
            int checkcount_bran = 0;
            // int checkcount_sec = 0;
            ds3.Clear();
            DataView dv = new DataView();
            string sql = "select staff_code from usermaster where User_code='" + usercode + "'";
            ds3 = d2.select_method_wo_parameter(sql, "Text");
            string staff_code = ds3.Tables[0].Rows[0][0].ToString();
            for (int i = 0; i < Chkbat.Items.Count; i++)
            {
                if (Chkbat.Items[i].Selected == true)
                {
                    checkcount_batch++;
                    if (strbatch == "")
                    {
                        if (staff_code == "")
                            strbatch = "'" + Chkbat.Items[i].Value.ToString() + "'";
                        else
                            strbatch = "'" + Chkbat.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        if (staff_code == "")
                            strbatch = strbatch + "," + "'" + Chkbat.Items[i].Value.ToString() + "'";
                        else
                            strbatch = strbatch + "," + "'" + Chkbat.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (checkcount_batch == 0)
            {
                lblerrsri.Text = "Please Select Any " + ((schoolOrCollege == 0) ? "Batch" : "Year");
                return;
            }
            if (strbatch == "")
            {
                strbatch = "''";
            }
            for (int i = 0; i < Chkdeg.Items.Count; i++)
            {
                if (Chkdeg.Items[i].Selected == true)
                {
                    checkcount_deg++;
                    if (strdegree == "")
                    {
                        if (staff_code == "")
                        {
                            strdegree = "'" + Chkdeg.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strdegree = "'" + Chkdeg.Items[i].Value.ToString() + "'";
                        }
                    }
                    else
                    {
                        if (staff_code == "")
                        {
                            strdegree = strdegree + "," + "'" + Chkdeg.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strdegree = strdegree + "," + "'" + Chkdeg.Items[i].Value.ToString() + "'";
                        }
                    }
                }
            }
            if (checkcount_deg == 0)
            {
                lblerrsri.Text = "Please Select Any " + SchoolCollege.InsDegree;
                return;
            }
            if (strdegree == "")
            {
                strdegree = "''";
            }
            string regicode = "";
            for (int r = 0; r < chklsregion.Items.Count; r++)
            {
                if (chklsregion.Items[r].Selected == true)
                {
                    if (regicode.Trim() == "")
                    {
                        regicode = "'" + chklsregion.Items[r].Value.ToString() + "'";
                    }
                    else
                    {
                        regicode = regicode + ",'" + chklsregion.Items[r].Value.ToString() + "'";
                    }
                }
            }
            if (regicode.Trim() != "")
            {
                regicode = " and applyn.religion in(" + regicode + ")";
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    checkcount_bran++;
                    if (strbranch == "")
                    {
                        if (staff_code == "")
                        {
                            strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                        }
                    }
                    else
                    {
                        if (staff_code == "")
                        {
                            strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                        }
                    }
                }
            }
            if (checkcount_bran == 0)
            {
                lblerrsri.Text = "Please Select Any " + SchoolCollege.InsBranch;
                return;
            }
            if (strbranch == "")
            {
                strbranch = "0";
            }
            #region Transport
            string routeId = string.Empty;
            string vehiID = string.Empty;
            string stageId = string.Empty;
            if (cbTrans.Checked)
            {
                routeId = Convert.ToString(getCblSelectedValue(cblroute));
                vehiID = Convert.ToString(getCblSelectedValue(cblvechile));
                stageId = Convert.ToString(getCblSelectedValue(cblstage));
            }
            #endregion
            #region vehicle type
            string vehicleType = string.Empty;
            if (cbvehicleType.Checked && ddlvehType.Items.Count > 0)
            {
                if (rdbtnstudent.Checked)
                {
                    if (ddlvehType.SelectedIndex == 0)//own vehicle                 
                        vehicleType = " and registration.stud_type in('Day Scholar') and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='')) ";
                    else if (ddlvehType.SelectedIndex == 1)//college vehicle                
                        vehicleType = " and registration.stud_type in('Day Scholar') and ((isnull(Bus_RouteID,'')<>'' and isnull(Boarding,'')<>'' and isnull(VehID,'')<>'')) ";
                    else //both vehicle                
                        vehicleType = " and registration.stud_type in('Day Scholar')";
                }
                else
                {
                    if (ddlvehType.SelectedIndex == 0)//own vehicle                 
                        vehicleType = "  and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='')) ";
                    else if (ddlvehType.SelectedIndex == 1)//college vehicle                
                        vehicleType = "  and ((isnull(Bus_RouteID,'')<>'' and isnull(Boarding,'')<>'' and isnull(VehID,'')<>'')) ";
                    else //both vehicle                
                        vehicleType = " ";
                }
            }
            #endregion
            #region Order by 28.08.17
            string orderStr = d2.GetFunction("select value from Master_Settings where settings='order_by'");
            if (orderStr != "0")
            {
                if (orderStr == "0")
                    orderStr = "ORDER BY registration.Roll_No";
                else if (orderStr == "1")
                    orderStr = "ORDER BY registration.Reg_No";
                else if (orderStr == "2")
                    orderStr = "ORDER BY registration.Stud_Name";
                else if (orderStr == "0,1,2")
                    orderStr = "ORDER BY registration.Roll_No,registration.Reg_No,registration.Stud_Name";
                else if (orderStr == "0,1")
                    orderStr = "ORDER BY registration.Roll_No,registration.Reg_No";
                else if (orderStr == "1,2")
                    orderStr = "ORDER BY registration.Reg_No,registration.Stud_Name";
                else if (orderStr == "0,2")
                    orderStr = "ORDER BY registration.Roll_No,registration.Stud_Name";
            }
            orderStr = (orderStr == "0") ? "" : orderStr;
            #endregion
            if (staff_code == "")
            {
            }
            else
            {
                string sec_now = "";
                if (chklstsection.Items.Count == 0)
                {
                }
                else
                {
                    for (int i = 0; i < chklstsection.Items.Count; i++)
                    {
                        if (chklstsection.Items[i].Selected == true)
                        {
                            if (sec_now == "")
                            {
                                sec_now = "'" + chklstsection.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                sec_now = sec_now + "," + "'" + chklstsection.Items[i].Value.ToString() + "'";
                            }
                        }
                    }
                }
                string SqlFinal = "";
                SqlFinal = " select distinct cc.Course_Name,cc.Course_Id, de.Acronym, r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si,Degree de,COURSE cc where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and r.sections=ss.sections and ss.batch_year=r.Batch_Year";
                SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "' and de.Degree_Code=si.degree_code and de.Course_Id=cc.Course_Id and cc.Course_Id in (" + strdegree + ") and r.degree_code in(" + strbranch + ") and r.batch_year in (" + strbatch + ")  ";
                if (sec_now != "")
                {
                    sec_now = "and r.Sections in (" + sec_now + ") ";
                    SqlFinal = SqlFinal + "" + sec_now + "";
                }
                ds3.Clear();
                ds3 = d2.select_method_wo_parameter(SqlFinal, "Text");
                if (rdbtnstudent.Checked == true)
                {
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        string strstudentdetail = "select ROW_NUMBER() OVER (ORDER BY  Roll_no) As SrNo,roll_no,reg_no,registration.stud_name,registration.stud_type,applyn.Student_Mobile,applyn.parentF_Mobile,applyn.parentM_Mobile,applyn.ParentIdP,applyn.emailM,applyn.stuper_id,sio.start_date,registration.Adm_Date,registration.mode as Mode,registration.roll_admit as rolladmit,registration.degree_code as degcode,Registration.App_No  from seminfo sio,registration inner join applyn on applyn.app_no = registration.app_no where registration.degree_code=sio.degree_code and registration.batch_year=sio.batch_year and registration.current_semester=sio.semester  and registration.degree_code ='" + ds3.Tables[0].Rows[i]["degree_code"].ToString() + "' and Registration.Current_Semester = '" + ds3.Tables[0].Rows[i]["semester"].ToString() + "'  and registration.sections = '" + ds3.Tables[0].Rows[i]["Sections"].ToString() + "'  and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and registration.batch_year ='" + ds3.Tables[0].Rows[i]["Batch_Year"].ToString() + "' " + regicode + " ";
                        if (!string.IsNullOrEmpty(routeId))
                            strstudentdetail += " and registration.bus_routeid in('" + routeId + "')";
                        if (!string.IsNullOrEmpty(vehiID))
                            strstudentdetail += " and registration.vehid in('" + vehiID + "')";
                        if (!string.IsNullOrEmpty(stageId))
                            strstudentdetail += " and registration.boarding in('" + stageId + "')";
                        if (!string.IsNullOrEmpty(vehicleType))
                            strstudentdetail += vehicleType;
                        strstudentdetail += " " + orderStr;
                        //strstudentdetail += " order by Len(roll_no),roll_no";
                        MethodStudentGo(strstudentdetail);
                        Dropdownload();
                        Spread2Go();
                    }
                    lblerrsri.Text = "";
                    goto srilabel;
                }
            }
            if (chklstsection.Items.Count == 0)
            {
                strsec = "";
                strsec1 = "";
                strsecmark = "";
            }
            else
            {
                for (int i = 0; i < chklstsection.Items.Count; i++)
                {
                    if (chklstsection.Items[i].Selected == true)
                    {
                        if (strsection == "")
                        {
                            if (staff_code == "")
                            {
                                strsection = "'" + chklstsection.Items[i].Value.ToString() + "'";
                            }
                        }
                        else
                        {
                            if (staff_code == "")
                            {
                                strsection = strsection + "," + "'" + chklstsection.Items[i].Value.ToString() + "'";
                            }
                        }
                        if (strsection == "")
                        {
                            strsection = "''";
                        }
                    }
                }
                strsec = " and  ltrim(rtrim(isnull(registration.sections,''))) in ( " + strsection + ")";
                strsec1 = " and sections in (" + strsection + ")";
                strsecmark = "and re.sections in (" + strsection + ")";
            }
            if (rdbtnstudent.Checked == true)
            {
                string strstudentdetail = "select ROW_NUMBER() OVER (ORDER BY  Roll_no) As SrNo,roll_no,reg_no,registration.stud_name,registration.stud_type,applyn.Student_Mobile,applyn.parentF_Mobile,applyn.parentM_Mobile,applyn.ParentIdP,applyn.emailM,applyn.stuper_id,sio.start_date,registration.Adm_Date,registration.mode as Mode,registration.roll_admit as rolladmit,registration.degree_code as degcode,Registration.App_No   from seminfo sio,registration inner join applyn on applyn.app_no = registration.app_no where registration.degree_code=sio.degree_code and registration.batch_year=sio.batch_year and registration.current_semester=sio.semester  and registration.degree_code in (" + strbranch + ")   " + strsec + "  and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and registration.batch_year in (" + strbatch + ")  " + regicode + "  ";
                if (!string.IsNullOrEmpty(routeId))
                    strstudentdetail += " and registration.bus_routeid in('" + routeId + "')";
                if (!string.IsNullOrEmpty(vehiID))
                    strstudentdetail += " and registration.vehid in('" + vehiID + "')";
                if (!string.IsNullOrEmpty(stageId))
                    strstudentdetail += " and registration.boarding in('" + stageId + "')";
                if (!string.IsNullOrEmpty(vehicleType))
                    strstudentdetail += vehicleType;
                //strstudentdetail += " order by Len(roll_no),roll_no";
                strstudentdetail += " " + orderStr;
                MethodStudentGo(strstudentdetail);
                Dropdownload();
                Spread2Go();
            }
        srilabel: ;
            lblerrsri.Text = "";
            if (ds3.Tables[0].Rows.Count == 0)
            {
                lblerrsri.Text = "No Records Found";
                FpSpread1.Visible = false;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                txtmessage.Visible = false;
                // btnsms.Visible = false;
            }
            if (rdbtnstaff.Checked == true)
            {
                // string strstaffdetail = "select ROW_NUMBER() OVER (ORDER BY  staff_name) As SrNo,sm.staff_code,sm.staff_name,st.stftype,sam.per_mobileno,sam.email from staffmaster sm,stafftrans st,staff_appl_master sam ,hrdept_master h where st.staff_code=sm.staff_code and h.dept_code=sam.dept_code and st.dept_code =h.dept_code and sm.appl_no = sam.appl_no and latestrec = 1 and sm.college_code = " + ddlcollege.SelectedValue.ToString() + " and st.dept_code in ( " + strbranch + " ) and st.desig_code in ( " + strdegree + ") and resign = 0 and settled = 0 order by h.dept_name , sm.staff_code";//modified by srinath 4/9/2014
                string strstaffdetail = "select ROW_NUMBER() OVER (ORDER BY  staff_name) As SrNo,sm.staff_code,sm.staff_name,st.stftype,sam.per_mobileno,sam.email,sam.appl_id as App_No from staffmaster sm,stafftrans st,staff_appl_master sam ,hrdept_master h where st.staff_code=sm.staff_code  and st.dept_code =h.dept_code and sm.appl_no = sam.appl_no and latestrec = 1 and sm.college_code = " + ddlcollege.SelectedValue.ToString() + " and st.dept_code in ( " + strbranch + " ) and st.desig_code in ( " + strdegree + ") and resign = 0 and settled = 0 order by h.dept_name , sm.staff_code";
                if (!string.IsNullOrEmpty(routeId))
                    strstaffdetail += " and sm.bus_routeid in('" + routeId + "')";
                if (!string.IsNullOrEmpty(vehiID))
                    strstaffdetail += " and sm.vehid in('" + vehiID + "')";
                if (!string.IsNullOrEmpty(stageId))
                    strstaffdetail += " and sm.boarding in('" + stageId + "')";
                if (!string.IsNullOrEmpty(vehicleType))
                    strstaffdetail += vehicleType;
                MethodStaffGo(strstaffdetail);
                Dropdownload();
                Spread2Go();
            }
            Spread2Go();
            //  chkvoicecall_CheckedChange(sender, e);
            if (FpSpread1.Visible == true)
            {
                if (chknotification.Checked == true)
                {
                    Tablenote.Visible = true;
                    lblsubject.Visible = true;
                    lblnotification.Visible = true;
                    txtsubject.Visible = true;
                    lblnote.Visible = true;
                    txtnotification.Visible = true;
                    lblfile.Visible = true;
                    lblattachements.Visible = true;
                    fudfile.Visible = true;
                    fudattachemnts.Visible = true;
                    btnsend.Visible = true;
                    Divv2.Visible = true;
                }
                else
                {
                    Tablenote.Visible = false;
                    lblsubject.Visible = false;
                    lblnotification.Visible = false;
                    txtsubject.Visible = false;
                    lblnote.Visible = false;
                    txtnotification.Visible = false;
                    lblfile.Visible = false;
                    lblattachements.Visible = false;
                    fudfile.Visible = false;
                    fudattachemnts.Visible = false;
                    Divv2.Visible = false;
                    //btnnotfsave.Visible = false;
                }
                if (chkboxmail.Checked == true && chkboxsms.Checked == true)
                {
                    tblmail.Visible = true;
                    txtmessage.Visible = true;
                    //btnsms.Visible = true;
                    lblpurpose1.Visible = true;
                    ddlpurpose.Visible = true;
                    FpSpread2.Visible = true;
                    btnaddtemplate.Visible = true;
                    btndeletetemplate.Visible = true;
                    Label7.Visible = true;
                    Div7.Visible = true;
                    btnsend.Visible = true;
                    Divv1.Visible = true;
                }
                else
                {
                    if (chkboxmail.Checked == false && chkboxsms.Checked == true)
                    {
                        tblmail.Visible = false;
                        txtmessage.Visible = true;
                        //btnsms.Visible = true;
                        lblpurpose1.Visible = true;
                        ddlpurpose.Visible = true;
                        FpSpread2.Visible = true;
                        btnaddtemplate.Visible = true;
                        btndeletetemplate.Visible = true;
                        Label7.Visible = true;
                        Div7.Visible = true;
                        btnsend.Visible = true;
                        Divv1.Visible = false;
                    }
                    else
                    {
                        txtmessage.Visible = false;
                        //btnsms.Visible = false;
                        lblpurpose1.Visible = false;
                        ddlpurpose.Visible = false;
                        FpSpread2.Visible = false;
                        btnaddtemplate.Visible = false;
                        btndeletetemplate.Visible = false;
                        txtmessage.Visible = false;
                        Label7.Visible = false;
                        Div7.Visible = false;
                    }

                    if (chkboxmail.Checked == true && chkboxsms.Checked == false)
                    {
                        tblmail.Visible = true;
                        txtmessage.Visible = false;
                        //btnsms.Visible = false;
                        lblpurpose1.Visible = false;
                        ddlpurpose.Visible = false;
                        FpSpread2.Visible = false;
                        btnaddtemplate.Visible = false;
                        btndeletetemplate.Visible = false;
                        Label7.Visible = false;
                        Div7.Visible = false;
                        btnsend.Visible = true;
                        Divv1.Visible = true;
                    }
                    else
                    {
                        tblmail.Visible = false;

                    }
                }
                if (chkvoicecall.Checked == true)
                {
                    Fpspreadvoice.CommandBar.Visible = false;
                    Div5.Visible = true;
                    panelvoice.Visible = true;
                    bindspreadvoice();
                    btnsend.Visible = true;
                }
                else
                {
                    Fpspreadvoice.CommandBar.Visible = false;
                    panelvoice.Visible = false;
                    Div5.Visible = false;

                }
            }
            else
            {
                Tablenote.Visible = false;
                lblsubject.Visible = false;
                lblnotification.Visible = false;
                txtsubject.Visible = false;
                lblnote.Visible = false;
                txtnotification.Visible = false;
                lblfile.Visible = false;
                lblattachements.Visible = false;
                fudfile.Visible = false;
                fudattachemnts.Visible = false;
                //btnnotfsave.Visible = false;
                btnsend.Visible = true;
            }
        }
        catch
        {
        }
    }

    public void clearnotification()
    {
        try
        {
            norecordlbl.Visible = false;
            FpSpread1.Visible = false;
            errmsg.Visible = false;
            FpSpread2.Visible = false;
            btnaddtemplate.Visible = false;
            btndeletetemplate.Visible = false;
            btnexit.Visible = false;
            lblpurpose.Visible = false;
            txtpurposemsg.Visible = false;
            txtpurposecaption.Visible = false;
            templatepanel.Visible = false;
            purposepanel.Visible = false;
            btnsave.Visible = false;
            btnxl.Visible = false;
            lblpurpose1.Visible = false;
            ddlpurpose.Visible = false;
            //btnsms.Visible = false;
            txtmessage.Visible = false;
            Tablenote.Visible = false;
        }
        catch
        {
        }
    }

    public void Spread2Go()
    {
        try
        {
            FpSpread2.Sheets[0].ColumnHeaderVisible = false;
            FpSpread2.Sheets[0].SheetCorner.Columns[0].Visible = false;
            //FpSpread2.Visible = true;
            //lblpurpose1.Visible = true;
            //ddlpurpose.Visible = true;
            FpSpread2.Sheets[0].RowCount = 1;
            FpSpread2.Sheets[0].ColumnCount = 2;
            FpSpread2.Columns[1].Width = 900;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = "Template";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
            string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template";
            ds = d2.select_method(spread2query1, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                }
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.SaveChanges();
        }
        catch
        {
        }
    }

    public void MethodStudentGo(string strcmd)
    {
        try
        {
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            errmsg.Visible = false;
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(strcmd, hat, "Text");
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                if (spreadgo == false)
                {
                    norecordlbl.Visible = false;
                    FpSpread1.Visible = true;
                    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                    style.Font.Size = 12;
                    // style.Font.Bold = true;
                    FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                    FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                    FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                    FpSpread1.Pager.Align = HorizontalAlign.Left;
                    FpSpread1.Pager.Font.Bold = true;
                    FpSpread1.Pager.Font.Name = "Book Antiqua";
                    FpSpread1.Pager.ForeColor = Color.DarkGreen;
                    // FpSpread1.Pager.BackColor = Color.Beige;
                    // FpSpread1.Pager.BackColor = Color.AliceBlue;
                    FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                    FpSpread1.SheetCorner.Columns[0].Visible = false;
                    FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                    style1.Font.Size = 12;
                    style1.Font.Bold = true;
                    style1.HorizontalAlign = HorizontalAlign.Center;
                    style1.ForeColor = System.Drawing.Color.Black;
                    style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                    FpSpread1.Sheets[0].SheetCorner.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                    FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].ColumnCount = 12;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnHeader.Visible = true;
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    FpSpread1.Sheets[0].Columns[2].CellType = new FarPoint.Web.Spread.TextCellType();
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Mobile";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Email_ID";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Father Mobile";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Father Email_ID";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Mother Mobile";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Mother Email_ID";
                    FpSpread1.Sheets[0].Columns[0].CellType = txt;
                    FpSpread1.Sheets[0].Columns[1].CellType = txt;
                    FpSpread1.Sheets[0].Columns[2].CellType = txt;
                    FpSpread1.Sheets[0].Columns[3].CellType = txt;
                    FpSpread1.Sheets[0].Columns[4].CellType = txt;
                    FpSpread1.Sheets[0].Columns[5].CellType = txt;
                    FpSpread1.Sheets[0].Columns[6].CellType = txt;
                    FpSpread1.Sheets[0].Columns[7].CellType = txt;
                    FpSpread1.Sheets[0].Columns[8].CellType = txt;
                    FpSpread1.Sheets[0].Columns[9].CellType = txt;
                    FpSpread1.Sheets[0].Columns[10].CellType = txt;
                    FpSpread1.Sheets[0].Columns[0].Locked = true;
                    FpSpread1.Sheets[0].Columns[1].Locked = true;
                    FpSpread1.Sheets[0].Columns[2].Locked = true;
                    FpSpread1.Sheets[0].Columns[3].Locked = true;
                    FpSpread1.Sheets[0].Columns[4].Locked = true;
                    FpSpread1.Sheets[0].Columns[5].Locked = true;
                    FpSpread1.Sheets[0].Columns[6].Locked = true;
                    FpSpread1.Sheets[0].Columns[7].Locked = true;
                    FpSpread1.Sheets[0].Columns[8].Locked = true;
                    FpSpread1.Sheets[0].Columns[9].Locked = true;
                    FpSpread1.Sheets[0].Columns[10].Locked = true;
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].SpanModel.Add(0, 0, 1, 11);
                    FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].CellType = chkcell1;
                    chkcell1.AutoPostBack = true;
                    spreadgo = true;
                }
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    sno++;
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["roll_no"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[dscnt]["app_no"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["rolladmit"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["reg_no"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["degCode"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["stud_name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["stud_name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["stud_type"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Student_Mobile"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["Student_Mobile"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["stuper_id"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["stuper_id"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["parentF_Mobile"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["parentF_Mobile"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["ParentIdP"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["ParentIdP"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["parentM_Mobile"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["parentM_Mobile"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["emailM"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["emailM"]);
                }
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Select";
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                FpSpread1.Sheets[0].Columns[11].CellType = chkcell;
                FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
                chkcell.AutoPostBack = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
                //btnsms.Visible = false;
                btnsend.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                txtmessage.Text = "";
                txtmessage.Visible = false;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;
            }
            else
            {
                FpSpread1.Visible = false;
                norecordlbl.Text = "No Record Found";
                norecordlbl.Visible = true;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;
                fpMsg.Visible = false;
            }
        }
        catch
        {
        }
    }

    public void MethodStaffGo(string strcmd)
    {
        try
        {
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            errmsg.Visible = false;
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(strcmd, hat, "Text");
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                norecordlbl.Visible = false;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 12;
                style.Font.Bold = true;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Left;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = Color.DarkGreen;
                FpSpread1.Pager.BackColor = Color.Beige;
                FpSpread1.Pager.BackColor = Color.AliceBlue;
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.SheetCorner.Columns[0].Visible = false;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].SheetCorner.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 7;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Mobile";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Staff Email_ID";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                FpSpread1.Sheets[0].Columns[6].CellType = chkcell;
                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                chkcell.AutoPostBack = true;
                FpSpread1.Sheets[0].Columns[0].CellType = txt;
                FpSpread1.Sheets[0].Columns[1].CellType = txt;
                FpSpread1.Sheets[0].Columns[2].CellType = txt;
                FpSpread1.Sheets[0].Columns[3].CellType = txt;
                FpSpread1.Sheets[0].Columns[4].CellType = txt;
                FpSpread1.Sheets[0].Columns[5].CellType = txt;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].SpanModel.Add(0, 0, 1, 6);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;

                FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();

                int sno = 0;
                //=========Added by saranya on 3/9/2018=======//
                string StaffCode = "";
                string Date = Convert.ToString(Txtdate.Text);
                string[] strDate = Date.Split('/');

                string CkDate = strDate[0];
                //For Date
                CkDate = CkDate.StartsWith("0") ? CkDate.Substring(1) : CkDate;

                CkDate = "[" + CkDate + "]";

                //For Month
                string Mnt = strDate[1];
                Mnt = Mnt.StartsWith("0") ? Mnt.Substring(1) : Mnt;
                string Year = strDate[2];
                string MonthYear = Mnt + "/" + Year;

                //============================================//


                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    sno++;
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["staff_code"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[dscnt]["app_no"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["staff_name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["staff_name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["stftype"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    strstaffmobile = Convert.ToString(ds.Tables[0].Rows[dscnt]["per_mobileno"]);
                    StaffCode = Convert.ToString(ds.Tables[0].Rows[dscnt]["staff_code"]);
                    if (strstaffmobile != "")
                    {
                        // string[] strstfmbl = strstaffmobile.Split('-');
                        //if (strstfmbl.GetUpperBound(0) >= 1)
                        //{
                        //    strstaffmobile = strstfmbl[1].ToString();
                        //}
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = strstaffmobile;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Note = strstaffmobile;
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["email"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(ds.Tables[0].Rows[dscnt]["email"]);

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = chkcell1;
                    chkcell1.AutoPostBack = true;

                    //=========Added by saranya on 3/9/2018=======//
                    string Attendance = d2.GetFunction(" select " + CkDate + " from staff_attnd where mon_year in('" + MonthYear + "') and staff_code='" + StaffCode + "' ");
                    if (!string.IsNullOrEmpty(Attendance) && Attendance != "0")
                    {
                        string[] attnValue = Attendance.Split('-');
                        //string TreatAsPorAMrg = d2.GetFunction("select status from leave_category where shortname='" + attnValue[0] + "' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'");
                        //string TreatAsPorAEvg = d2.GetFunction("select status from leave_category where shortname='" + attnValue[1] + "' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'");
                        if ((attnValue[0] == "P" && attnValue[1] == "P") || (attnValue[0] == "P" && attnValue[1] == "PER") || (attnValue[0] == "PER" && attnValue[1] == "P"))//(TreatAsPorAMrg == "0" && TreatAsPorAEvg == "0") || (TreatAsPorAMrg == "2" && TreatAsPorAEvg == "2") || 
                        {
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Green;
                        }
                        else if ((attnValue[0] != "" && attnValue[1] != ""))//|| (TreatAsPorAMrg == "1" && TreatAsPorAEvg == "1")
                        {
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Red;
                        }
                    }
                    //============================================//
                }


                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
                //btnsms.Visible = true;
                btnaddtemplate.Visible = true;
                btndeletetemplate.Visible = true;
                txtmessage.Text = "";
                txtmessage.Visible = true;
                lblpurpose1.Visible = true;
                ddlpurpose.Visible = true;
                FpSpread2.Visible = true;
            }
            else
            {
                FpSpread1.Visible = false;
                norecordlbl.Text = "No Record Found";
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                FpSpread2.Visible = false;
                norecordlbl.Visible = true;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;
                fpMsg.Visible = false;
            }
        }
        catch
        {
        }
    }

    public void MethodStaffGoSms(string strstaffdetail)
    {
        try
        {
            // FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            errmsg.Visible = false;

            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strstaffdetail, "Text");
            // ds = d2.select_method(strstaffdetail, hat, "Text");
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                fpMsg.Sheets[0].ColumnCount = 0;
                fpMsg.Sheets[0].RowCount = 0;
                //FpSpread1.Visible = false;
                norecordlbl.Visible = false;
                //fpMsg.Visible = true;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 12;
                style.Font.Bold = true;
                // fpMsg.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                // fpMsg.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                // fpMsg.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                // fpMsg.Pager.Align = HorizontalAlign.Left;
                //  fpMsg.Pager.Font.Bold = true;
                //  fpMsg.Pager.Font.Name = "Book Antiqua";
                //  fpMsg.Pager.ForeColor = Color.DarkGreen;
                //  fpMsg.Pager.BackColor = Color.Beige;
                //  fpMsg.Pager.BackColor = Color.AliceBlue;
                //  fpMsg.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                // fpMsg.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                //  fpMsg.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                fpMsg.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpMsg.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpMsg.Sheets[0].DefaultStyle.Font.Bold = false;
                // fpMsg.SheetCorner.Columns[0].Visible = false;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                // fpMsg.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                // fpMsg.Sheets[0].SheetCorner.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                // fpMsg.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                // fpMsg.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                // fpMsg.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                // fpMsg.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                // fpMsg.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fpMsg.Sheets[0].DefaultStyle.Font.Bold = false;
                fpMsg.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;

                fpMsg.Sheets[0].ColumnHeader.RowCount = 1;
                fpMsg.Sheets[0].ColumnCount = 4;
                fpMsg.Sheets[0].RowCount = 0;
                // fpMsg.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                // fpMsg.Sheets[0].Columns[0].Width = 10;


                FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell1.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell.AutoPostBack = false;
                FarPoint.Web.Spread.ButtonCellType clickcell1 = new FarPoint.Web.Spread.ButtonCellType();
                clickcell1.Text = "View";

                fpMsg.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Group Name";
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Select";
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 3].Text = "View";
                fpMsg.Width = 300;
                //   fpMsg.Sheets[0].Columns[1].Width = 30;             
                //  fpMsg.Sheets[0].Columns[2].Width = 5;
                //  fpMsg.Sheets[0].Columns[3].Width = 5;                
                //  fpMsg.Sheets[0].Columns[0].Locked = true;
                //   fpMsg.Sheets[0].Columns[1].Locked = true;               

                // fpMsg.Sheets[0].RowCount++;                       
                //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 2].CellType = chkcell1;
                //fpMsg.Sheets[0].SpanModel.Add(0, 0, 1, 2);



                int sno = 0;
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    sno++;
                    fpMsg.Sheets[0].RowCount++;
                    //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["MasterValue"]);
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[dscnt]["MasterCode"]);
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 2].CellType = chkcell;
                    //  fpMsg.Sheets[0].Columns[2].CellType = chkcell;
                    //fpMsg.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 3].CellType = clickcell1;
                }


                // FarPoint.Web.Spread.ButtonCellType clickcell = new FarPoint.Web.Spread.ButtonCellType();


                fpMsg.Sheets[0].PageSize = fpMsg.Sheets[0].RowCount;
                fpMsg.SaveChanges();
                fpMsg.Visible = true;
                //btnsms.Visible = true;
                btnaddtemplate.Visible = true;
                btndeletetemplate.Visible = true;
                txtmessage.Text = "";
                txtmessage.Visible = true;
                lblpurpose1.Visible = true;
                ddlpurpose.Visible = true;
                FpSpread2.Visible = false;
            }
            else
            {
                fpMsg.Visible = false;
                norecordlbl.Text = "No Record Found";
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                FpSpread2.Visible = false;
                norecordlbl.Visible = true;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;

            }
        }
        catch
        {
        }
    }

    public void MethodStaffGoSms1(string strstaffdetail)
    {
        try
        {
            // FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            errmsg.Visible = false;

            ds.Dispose();
            ds.Reset();


            ds = d2.select_method_wo_parameter(strstaffdetail, "Text");
            // ds = d2.select_method(strstaffdetail, hat, "Text");
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {


                fpMsg.Sheets[0].RowCount = 0;
                fpMsg.Sheets[0].ColumnCount = 0;
                fpMsg.CommandBar.Visible = false;
                fpMsg.Sheets[0].AutoPostBack = false;
                fpMsg.Sheets[0].ColumnHeader.RowCount = 1;
                fpMsg.Sheets[0].RowHeader.Visible = false;
                fpMsg.Sheets[0].ColumnCount = 4;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //darkstyle.ForeColor = Color.White;
                darkstyle.ForeColor = Color.Black;
                fpMsg.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                int check = 0;
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

                // fpMsg.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Group Name";
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Select";
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 3].Text = "View";
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpMsg.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpMsg.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;


                fpMsg.Columns[0].Width = 50;
                fpMsg.Columns[1].Width = 100;
                fpMsg.Columns[2].Width = 100;
                fpMsg.Columns[3].Width = 100;



                FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell1.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell.AutoPostBack = false;
                FarPoint.Web.Spread.ButtonCellType clickcell1 = new FarPoint.Web.Spread.ButtonCellType();
                clickcell1.Text = "View";




                //  fpMsg.Sheets[0].Columns[3].Width = 5;                
                //  fpMsg.Sheets[0].Columns[0].Locked = true;
                //   fpMsg.Sheets[0].Columns[1].Locked = true;               




                fpMsg.Sheets[0].RowCount++;
                fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 2].CellType = chkcell1;
                fpMsg.Sheets[0].SpanModel.Add(0, 0, 1, 2);

                fpMsg.Width = 400;

                int sno = 0;
                int height = 0;
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    sno++;
                    fpMsg.Sheets[0].RowCount++;
                    height += 10;
                    //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    //fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["MasterValue"]);
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[dscnt]["MasterCode"]);
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 2].CellType = chkcell;
                    //fpMsg.Sheets[0].Columns[2].CellType = chkcell;
                    //fpMsg.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    fpMsg.Sheets[0].Cells[fpMsg.Sheets[0].RowCount - 1, 3].CellType = clickcell1;
                }


                // FarPoint.Web.Spread.ButtonCellType clickcell = new FarPoint.Web.Spread.ButtonCellType();

                height = fpMsg.Sheets[0].RowCount * 18 + 50;
                fpMsg.Height = (height < 400) ? height + 80 : height;
                fpMsg.Sheets[0].PageSize = fpMsg.Sheets[0].RowCount;
                fpMsg.SaveChanges();
                fpMsg.Visible = true;
                //btnsms.Visible = true;
                btnaddtemplate.Visible = true;
                btndeletetemplate.Visible = true;
                txtmessage.Text = "";
                txtmessage.Visible = true;
                lblpurpose1.Visible = true;
                ddlpurpose.Visible = true;
                FpSpread2.Visible = false;
                FpSpread1.Visible = false;
            }
            else
            {
                fpMsg.Visible = false;
                norecordlbl.Text = "No Record Found";
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                FpSpread2.Visible = false;
                norecordlbl.Visible = true;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;

            }
        }
        catch
        {
        }
    }

    #endregion

    protected void btnaddtemplate_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.Visible = true;
            // UpdatePanel1.Visible = true;
            // UpdatePanel2.Visible = true;
            templatepanel.Visible = true;
            lblpurpose.Visible = true;
            btnplus.Visible = true;
            btnminus.Visible = true;
            ddlpurpose.Visible = true;
            txtpurposemsg.Visible = true;
            btnsave.Visible = true;
            btnexit.Visible = true;
            lblerror.Visible = false;
            Dropdownload();
        }
        catch
        {
        }
    }

    protected void btnplus_Click(object sender, EventArgs e)
    {
        try
        {
            templatepanel.Enabled = false;
            purposepanel.Visible = true;
            lblpurposecaption.Visible = true;
            txtpurposecaption.Visible = true;
            btnpurposeadd.Visible = true;
            btnpurposeexit.Visible = true;
        }
        catch
        {
        }
    }

    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string strdelpurpose = "Delete from sms_purpose where temp_code = '" + ddlpurposemsg.SelectedValue + "'";
            i = d2.insert_method(strdelpurpose, hat, "Text");
            if (i == 1)
            {
                lblerror.Text = "Purpose deleted Successfully";
                lblerror.Visible = true;
                Dropdownload();
            }
            else
            {
                lblerror.Text = "Purpose deleted Failed";
                lblerror.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void btndeletetemplate_Click(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
            if (Cellclick == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1)
                {
                    string msg = FpSpread2.Sheets[0].GetText(ar, 1);
                    string strdeletequery = "delete   sms_template where Template='" + msg + "'";
                    int vvv = d2.insert_method(strdeletequery, hat, "");
                    if (vvv == 1)
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Delete Template Succefully";
                    }
                    else
                    {
                        lblerror.Text = "Delete Template  failed";
                    }
                }
                Spread2Go();
                Cellclick = false;
            }
        }
        catch
        {
        }
    }

    #region "Send SMS Function "
    protected void sendsms()
    // protected void btnsms_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + ddlcollege.SelectedValue.ToString() + "'";
            #region Copy Of SMS ADDED BY MALANG RAJA
            string copysmsmobno = "";
            copysmsmobno = d2.GetFunctionv("select value from Master_Settings where settings='Copy of SMS'");
            #endregion Copy Of SMS ADDED BY MALANG RAJA
            //ds1.Dispose();
            //ds1.Reset();
            //ds1 = d2.select_method(strsenderquery, hat, "Text");
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
            //    user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
            //}
            user_id = d2.GetFunction(strsenderquery);

            int flg = 0;
            //start====== added by Manikandan 27/07/2013
            if (chkboxsms.Checked == false && chkboxmail.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any One Option SMS or MAIL ')", true);
                return;
            }
            if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
            {
                if (chkstudent.Checked == false && chkfather.Checked == false && chkmother.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Atleased one person to send Sms or Mail ')", true);
                    return;
                }
            }
            //End========
            #region For SMS
            SMSSettings smsObject = new SMSSettings();
            //smsObject.User_degreecode = Convert.ToInt32(degcode);
            smsObject.User_collegecode = Convert.ToInt32(ddlcollege.SelectedValue);
            smsObject.User_usercode = usercode;
            smsObject.IsStaff = 0;
            byte sms_settings = smsObject.getSMSSettings(smsObject.User_collegecode);
            if (sms_settings == 0)
            {
                #region Common SMS
                if (chkboxsms.Checked == true)
                {
                    string strsmsuserid = string.Empty;

                    string studentAppNo = string.Empty;
                    try
                    {
                        if (rdbtnstudent.Checked == true)
                        {
                            strmsg = txtmessage.Text;
                            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 11].Value);
                                if (isval == 1)
                                {
                                    flg = 1;
                                    flagstudent = true;
                                    strmobileno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Note);
                                    strfmobile = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Note);
                                    strmmobile = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 9].Note);
                                    string AppNo = Convert.ToString(FpSpread1.Sheets[0].GetTag(i, 1));
                                    if (!string.IsNullOrEmpty(AppNo))
                                    {
                                        if (studentAppNo == "")
                                            studentAppNo = AppNo;
                                        else
                                            studentAppNo = studentAppNo + "," + AppNo;
                                    }
                                    if (chkstudent.Checked == true)
                                    {
                                        if (strmobileno != "Nil" && strmobileno != "")
                                        {
                                            if (mobilenos == "")
                                            {
                                                mobilenos = strmobileno;

                                            }
                                            else
                                            {
                                                mobilenos = mobilenos + "," + strmobileno;
                                            }
                                        }
                                    }
                                    if (chkfather.Checked == true)
                                    {
                                        if (strfmobile != "Nil" && strfmobile != "")
                                        {
                                            if (mobilenos == "")
                                            {
                                                mobilenos = strfmobile;
                                            }
                                            else
                                            {
                                                mobilenos = mobilenos + "," + strfmobile;
                                            }
                                        }
                                    }
                                    if (chkmother.Checked == true)
                                    {
                                        if (strmmobile != "Nil" && strmmobile != "")
                                        {
                                            if (mobilenos == "")
                                            {
                                                mobilenos = strmmobile;
                                            }
                                            else
                                            {
                                                mobilenos = mobilenos + "," + strmmobile;
                                            }
                                        }
                                    }
                                }
                            }
                            //modified by srinath 8/2/2014
                            if (flg == 1)//Modify By M.SakthiPriya 11-12-2014
                            {
                                //string strpath = "  http://unicel.in/SendSMS/sendmsg.php?uname=" + user_id + "&pass=" + Password + "&send=" + SenderID + "&dest=" + mobilenos + "&msg=" + strmsg + "";
                                ////string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                                //// string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobilenos + "&message=" + strmsg + "&sender=" + SenderID;
                                //string isst = "0";
                                ///smsreport(strpath, isst);
                                ///
                                if (mobilenos != "" && copysmsmobno.Trim().Trim(',') != "")
                                {
                                    mobilenos += "," + copysmsmobno.Trim().Trim(',');
                                }
                                if (RbEnglish.Checked == true)//Modified by saranya on 17/9/2018
                                {
                                    int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, mobilenos, strmsg, "0", "", studentAppNo);
                                }
                                if (RbTamil.Checked == true)
                                {
                                    int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, mobilenos, strmsg, "0", "", studentAppNo, "1");
                                }
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('SMS Sended Successfully')", true);
                            }
                            else//Modify By M.SakthiPriya 11-12-2014
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Atleast One Student')", true);
                            }
                        }
                        else if (rdbtnstaff.Checked == true)
                        {
                            strmsg = txtmessage.Text;
                            if (!chksmsgroup.Checked)
                            {
                                for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                {
                                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 6].Value);
                                    if (isval == 1)
                                    {
                                        strmobileno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Note);
                                        if (strmobileno != "Nil" && strmobileno != "")
                                        {
                                            if (mobilenos == "")
                                            {
                                                mobilenos = strmobileno;

                                            }
                                            else
                                            {
                                                mobilenos = mobilenos + "," + strmobileno;
                                            }
                                        }
                                        string AppNo = Convert.ToString(FpSpread1.Sheets[0].GetTag(i, 1));
                                        if (!string.IsNullOrEmpty(AppNo))
                                        {
                                            if (studentAppNo == "")
                                            {
                                                studentAppNo = AppNo;
                                            }
                                            else
                                            {
                                                studentAppNo = studentAppNo + "," + AppNo;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                fpMsg.SaveChanges();
                                for (int i = 1; i < fpMsg.Sheets[0].RowCount; i++)
                                {
                                    int isval = Convert.ToInt32(fpMsg.Sheets[0].Cells[i, 2].Value);
                                    if (isval == 1)
                                    {
                                        strstuname = Convert.ToString(fpMsg.Sheets[0].Cells[i, 1].Tag);
                                        string selQ = " select distinct staff_code,sam.per_mobileno,sam.email from staffmaster sm,staff_appl_master sam where  sm.appl_no = sam.appl_no and sms_groupCode in('" + strstuname + "')";
                                        DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                                        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                                        {
                                            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                                            {
                                                strmobileno = Convert.ToString(dsVal.Tables[0].Rows[row]["per_mobileno"]);
                                                if (strmobileno != "Nil" && strmobileno != "")
                                                {
                                                    if (mobilenos == "")
                                                    {
                                                        mobilenos = strmobileno;

                                                    }
                                                    else
                                                    {

                                                        mobilenos = mobilenos + "," + strmobileno;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            //modified by srinath 8/2/2014
                            //string strpath1 = "http://alerts.sinfini.com/api/web2sms.php?workingkey=" + strsenderid + " &sender=" + struserapi + "&to=" + mobilenos + "  &message=" + strmsg;
                            // string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobilenos + "&message=" + strmsg + "&sender=" + SenderID;
                            //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                            //// System.Diagnostics.Process.Start(strpath1);
                            //string isstf = "1";
                            //smsreport(strpath1, isstf);
                            //added by sasikumar
                            //  lblsendmail.Text = "";
                            //  lblsendmail.Visible = true;
                            //---------end---
                            if (mobilenos != "" && copysmsmobno.Trim().Trim(',') != "")
                            {
                                mobilenos += "," + copysmsmobno.Trim().Trim(',');
                            }
                            if (RbEnglish.Checked == true)//Modified by saranya on 17/9/2018
                            {
                                int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, mobilenos, strmsg, "1");
                            }
                            if (RbTamil.Checked == true)
                            {
                                int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, mobilenos, strmsg, "1", "", "", "1");
                            }

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('SMS Sended Successfully')", true);
                        }
                        if (chkboxmail.Checked == false)
                        {
                            txtmessage.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                #endregion
            }
            else if (sms_settings == 1)
            {
                #region Individual SMS
                {
                    string admissionNo = string.Empty;
                    string strsmsuserid = string.Empty;
                    try
                    {
                        smsObject.Text_message = strmsg = txtmessage.Text;
                        if (rdbtnstudent.Checked == true)
                        {
                            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 11].Value);
                                if (isval == 1)
                                {
                                    flg = 1;
                                    flagstudent = true;
                                    admissionNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Note);
                                    smsObject.User_degreecode = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 2].Note);
                                    strmobileno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Note);
                                    strfmobile = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Note);
                                    strmmobile = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 9].Note);
                                    mobilenos = "";
                                    if (chkstudent.Checked == true)
                                    {
                                        if (strmobileno != "Nil" && strmobileno != "")
                                        {
                                            if (mobilenos == "")
                                            {
                                                mobilenos = strmobileno;
                                            }
                                            smsObject.MobileNos = mobilenos;
                                            smsObject.AdmissionNos = admissionNo;
                                            int nofosmssend = smsObject.sendTextMessage(sms_settings);
                                        }
                                    }
                                    if (chkfather.Checked == true)
                                    {
                                        if (strfmobile != "Nil" && strfmobile != "")
                                        {
                                            if (mobilenos == "")
                                            {
                                                mobilenos = strfmobile;
                                            }
                                            smsObject.MobileNos = mobilenos;
                                            smsObject.AdmissionNos = admissionNo;
                                            int nofosmssend = smsObject.sendTextMessage(sms_settings);
                                        }
                                    }
                                    if (chkmother.Checked == true)
                                    {
                                        if (strmmobile != "Nil" && strmmobile != "")
                                        {
                                            if (mobilenos == "")
                                            {
                                                mobilenos = strmmobile;
                                            }
                                            smsObject.MobileNos = mobilenos;
                                            smsObject.AdmissionNos = admissionNo;
                                            int nofosmssend = smsObject.sendTextMessage(sms_settings);
                                        }
                                    }
                                }
                            }
                            //modified by srinath 8/2/2014
                            if (flg == 1)//Modify By M.SakthiPriya 11-12-2014
                            {
                                //string strpath = "  http://unicel.in/SendSMS/sendmsg.php?uname=" + user_id + "&pass=" + Password + "&send=" + SenderID + "&dest=" + mobilenos + "&msg=" + strmsg + "";
                                ////string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                                //// string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobilenos + "&message=" + strmsg + "&sender=" + SenderID;
                                //string isst = "0";
                                ///smsreport(strpath, isst);
                                ///
                                //if (mobilenos != "" && copysmsmobno.Trim().Trim(',') != "")
                                //{
                                //    mobilenos += "," + copysmsmobno.Trim().Trim(',');
                                //}
                                //smsObject.MobileNos = mobilenos;
                                //smsObject.AdmissionNos = admissionNo;
                                //int nofosmssend = smsObject.sendTextMessage(sms_settings);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('SMS Sended Successfully')", true);
                            }
                            else//Modify By M.SakthiPriya 11-12-2014
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Atleast One Student')", true);
                            }
                        }
                        else if (rdbtnstaff.Checked == true)
                        {
                            int nofosmssend = 0;
                            if (!chksmsgroup.Checked)
                            {
                                for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                {
                                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 6].Value);
                                    if (isval == 1)
                                    {
                                        admissionNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Note);
                                        smsObject.User_degreecode = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 2].Note);
                                        strmobileno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Note);
                                        if (strmobileno != "Nil" && strmobileno != "")
                                        {
                                            smsObject.MobileNos = strmobileno;
                                            smsObject.AdmissionNos = admissionNo;
                                            nofosmssend += smsObject.sendTextMessage(sms_settings);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                {
                                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 2].Value);
                                    if (isval == 1)
                                    {
                                        strstuname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                        string selQ = " select distinct staff_code,sam.per_mobileno,sam.email from staffmaster sm,staff_appl_master sam where  sm.appl_no = sam.appl_no and sms_groupCode in('" + strstuname + "')";
                                        DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                                        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                                        {
                                            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                                            {
                                                strmobileno = Convert.ToString(dsVal.Tables[0].Rows[row]["per_mobileno"]);
                                                if (strmobileno != "Nil" && strmobileno != "")
                                                {
                                                    if (mobilenos == "")
                                                    {
                                                        mobilenos = strmobileno;
                                                    }
                                                    else
                                                    {
                                                        mobilenos = mobilenos + "," + strmobileno;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //modified by srinath 8/2/2014
                            //string strpath1 = "http://alerts.sinfini.com/api/web2sms.php?workingkey=" + strsenderid + " &sender=" + struserapi + "&to=" + mobilenos + "  &message=" + strmsg;
                            // string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mobilenos + "&message=" + strmsg + "&sender=" + SenderID;
                            //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                            //// System.Diagnostics.Process.Start(strpath1);
                            //string isstf = "1";
                            //smsreport(strpath1, isstf);
                            //added by sasikumar
                            //  lblsendmail.Text = "";
                            //  lblsendmail.Visible = true;
                            //---------end---
                            if (mobilenos != "" && copysmsmobno.Trim().Trim(',') != "")
                            {
                                mobilenos += "," + copysmsmobno.Trim().Trim(',');
                            }
                            //int nofosmssend = d2.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, mobilenos, strmsg, "1");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('SMS Sended Successfully')", true);
                        }
                        if (chkboxmail.Checked == false)
                        {
                            txtmessage.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                #endregion
            }
            #endregion
            if (chkboxmail.Checked == true)
            {
                try
                {
                    #region Copy Of EMAIL ADDED BY MALANG RAJA
                    bool isSendCopyEmail = false;
                    string[] copyEmailList = new string[1];
                    string copyeamilid = "";
                    copyeamilid = d2.GetFunctionv("select value from Master_Settings where settings='Copy of Email'");
                    if (copyeamilid.Trim().Trim(',') != "")
                    {
                        copyEmailList = copyeamilid.Split(',');
                        isSendCopyEmail = true;
                    }
                    #endregion Copy Of EMAIL ADDED BY MALANG RAJA
                    if (rdbtnstudent.Checked == true)
                    {
                        #region student
                        strmsg = txtmessage.Text;
                        string strquery = "select massemail,masspwd from collinfo where college_code = " + ddlcollege.SelectedValue.ToString() + " ";
                        ds1.Dispose();
                        ds1.Reset();
                        ds1 = d2.select_method(strquery, hat, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                            send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                        }
                        #region Added By Malang Raja on Oct 18 2016
                        else
                        {
                            lblsendmail.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                            lblsendmail.Visible = true;
                            return;
                        }
                        #endregion Added By Malang Raja on Oct 18 2016
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 11].Value);
                            if (isval == 1)
                            {
                                for (int stuorpart = 0; stuorpart < 3; stuorpart++)
                                {
                                    //Modified by srinath 18/12/2013
                                    to_mail = "";
                                    if (stuorpart == 0)
                                    {
                                        to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Note);
                                    }
                                    if (stuorpart == 1)
                                    {
                                        to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 8].Note);
                                    }
                                    if (stuorpart == 2)
                                    {
                                        to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Note);
                                    }
                                    if (to_mail.Trim() != "" && to_mail != null)
                                    {
                                        strstuname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Note);
                                        SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                        MailMessage mailmsg = new MailMessage();
                                        MailAddress mfrom = new MailAddress(send_mail);
                                        mailmsg.From = mfrom;
                                        mailmsg.To.Add(to_mail);
                                        mailmsg.Subject = "Report";
                                        mailmsg.IsBodyHtml = false;
                                        //mailmsg.Body = "Dear";
                                        //mailmsg.Body = mailmsg.Body + strstuname;
                                        mailmsg.Body = strstuname;
                                        mailmsg.Body = mailmsg.Body + "\n" + strmsg;
                                        mailmsg.Body = mailmsg.Body + "\n" + "Thank You..";
                                        //mailmsg.Body = "Hi ";
                                        //mailmsg.Body = mailmsg.Body + strstuname + "<br/>";
                                        //mailmsg.Body = mailmsg.Body + strmsg;
                                        //mailmsg.Body = mailmsg.Body + "<br/><br/><br/>Thank You...<br/><br/>";
                                        Mail.EnableSsl = true;
                                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                        Mail.UseDefaultCredentials = false;
                                        Mail.Credentials = credentials;
                                        Mail.Send(mailmsg);
                                        flagstudent = true;
                                    }
                                }
                                // lblsendmail.Text = "The Selected Students mail has been sent";
                                // lblsendmail.Visible = true;
                            }
                        }
                        if (chkboxmail.Checked == true && chkboxsms.Checked == false)
                        {
                            if (flagstudent == true)
                            {
                                #region Send EmailCopy
                                if (isSendCopyEmail)
                                {
                                    SendCopyEmail(copyEmailList, send_mail, send_pw, strmsg);
                                }
                                #endregion Send EmailCopy
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Students Mail has been sent')", true);
                            }
                        }
                        #endregion
                    }
                    else if (rdbtnstaff.Checked == true)
                    {
                        if (!chksmsgroup.Checked)
                        {
                            #region staff
                            strmsg = txtmessage.Text;
                            string strquery = "select massemail,masspwd from collinfo where college_code = " + ddlcollege.SelectedValue.ToString() + " ";
                            ds1.Dispose();
                            ds1.Reset();
                            ds1 = d2.select_method(strquery, hat, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                                send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                            }
                            #region Added By Malang Raja on Oct 18 2016
                            else
                            {
                                lblsendmail.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                                lblsendmail.Visible = true;
                                return;
                            }
                            #endregion Added By Malang Raja on Oct 18 2016
                            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 6].Value);
                                if (isval == 1)
                                {
                                    strstuname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Note);
                                    to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Note);
                                    // to_mail = "karthikeyanmurugesan08@gmail.com";
                                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                    MailMessage mailmsg = new MailMessage();
                                    MailAddress mfrom = new MailAddress(send_mail);
                                    mailmsg.From = mfrom;
                                    mailmsg.To.Add(to_mail);
                                    mailmsg.Subject = "Report";
                                    mailmsg.IsBodyHtml = false;
                                    // mailmsg.Body = "Dear";
                                    // mailmsg.Body = mailmsg.Body + strstuname;
                                    mailmsg.Body = strstuname;
                                    mailmsg.Body = mailmsg.Body + "\n" + strmsg;
                                    mailmsg.Body = mailmsg.Body + "\n" + "Thank You..";
                                    //mailmsg.IsBodyHtml = true;
                                    //mailmsg.Body = "Hi  ";
                                    //mailmsg.Body = mailmsg.Body + strstuname;
                                    //mailmsg.Body = mailmsg.Body + strmsg;
                                    //mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                                    Mail.EnableSsl = true;
                                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                    Mail.UseDefaultCredentials = false;
                                    Mail.Credentials = credentials;
                                    Mail.Send(mailmsg);
                                    flagstudent = true;
                                    //  lblsendmail.Text = "The Selected Staff mail has been sent";
                                    //  lblsendmail.Visible = true;
                                }
                            }
                            if (flagstudent == true)
                            {
                                #region Send EmailCopy
                                if (isSendCopyEmail)
                                {
                                    SendCopyEmail(copyEmailList, send_mail, send_pw, strmsg);
                                }
                                #endregion Send EmailCopy
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('The Selected Staff mail has been sent')", true);
                            }
                            #endregion
                        }
                        else //sms group 
                        {
                            #region staff
                            strmsg = txtmessage.Text;
                            string strquery = "select massemail,masspwd from collinfo where college_code = " + ddlcollege.SelectedValue.ToString() + " ";
                            ds1.Dispose();
                            ds1.Reset();
                            ds1 = d2.select_method(strquery, hat, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                                send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                            }
                            #region Added By Malang Raja on Oct 18 2016
                            else
                            {
                                lblsendmail.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                                lblsendmail.Visible = true;
                                return;
                            }
                            #endregion Added By Malang Raja on Oct 18 2016
                            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 2].Value);
                                if (isval == 1)
                                {
                                    strstuname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                    string selQ = " select distinct staff_code,sam.per_mobileno,sam.email from staffmaster sm,staff_appl_master sam where  sm.appl_no = sam.appl_no and sms_groupCode in('" + strstuname + "')";
                                    DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                                    if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                                    {
                                        for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                                        {
                                            to_mail = Convert.ToString(dsVal.Tables[0].Rows[row]["email"]);
                                            // to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Note);
                                            // to_mail = "karthikeyanmurugesan08@gmail.com";
                                            //SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                            //MailMessage mailmsg = new MailMessage();
                                            //MailAddress mfrom = new MailAddress(send_mail);
                                            //mailmsg.From = mfrom;
                                            //mailmsg.To.Add(to_mail);
                                            //mailmsg.Subject = "Report";
                                            //mailmsg.IsBodyHtml = true;
                                            //mailmsg.Body = "Hi  ";
                                            //mailmsg.Body = mailmsg.Body + strstuname;
                                            //mailmsg.Body = mailmsg.Body + strmsg;
                                            //mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                                            //Mail.EnableSsl = true;
                                            //NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                            //Mail.UseDefaultCredentials = false;
                                            //Mail.Credentials = credentials;
                                            //Mail.Send(mailmsg);
                                            flagstudent = true;
                                            //  lblsendmail.Text = "The Selected Staff mail has been sent";
                                            //  lblsendmail.Visible = true;
                                        }
                                    }
                                }
                            }
                            if (flagstudent == true)
                            {
                                #region Send EmailCopy
                                if (isSendCopyEmail)
                                {
                                    SendCopyEmail(copyEmailList, send_mail, send_pw, strmsg);
                                }
                                #endregion Send EmailCopy
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('The Selected Staff mail has been sent')", true);
                            }
                            #endregion
                        }
                    }
                    txtmessage.Text = "";
                }
                catch
                {
                    lblsendmail.Text = "Send Email Failed.";
                }
            }
        }
        catch
        {
        }
    }
    public void smsreport(string uril, string isstaff)
    {
        try
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = "";
            groupmsgid = strvel.Trim().ToString(); //aruna 02oct2013 strvel;       
            int sms = 0;
            string smsreportinsert = "";
            string[] split_id = groupmsgid.Split(' ');
            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                string group_id = split_id[icount].ToString();
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date )values( '" + split_mobileno[icount] + "','" + group_id + "','" + strmsg + "','" + ddlcollege.SelectedValue.ToString() + "','" + isstaff + "','" + date + "' )"; //Modify By M.SakthiPriya 11-12-2014
                sms = d2.insert_method(smsreportinsert, hat, "Text");
            }
            if (sms == 1)
            {
                lblerror.Visible = true;
                lblerror.Text = "Detail's added Succefully";
                flagstudent = true;
            }
            else
            {
                lblerror.Text = "Detail's added failed";
            }
            if (rdbtnstudent.Checked == true && rdbtnstaff.Checked == false)
            {
                if (chkboxsms.Checked == true && chkboxmail.Checked == false)
                {
                    if (flagstudent == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Students Message has been sent')", true);
                    }
                }
                if (chkboxmail.Checked == true && chkboxsms.Checked == false)
                {
                    if (flagstudent == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Students Mail has been sent')", true);
                    }
                }
                if (chkboxmail.Checked == true && chkboxsms.Checked == true)
                {
                    if (flagstudent == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Students Message/Mail has been sent')", true);
                    }
                }
            }
            if (rdbtnstaff.Checked == true && rdbtnstudent.Checked == false)
            {
                if (chkboxsms.Checked == true && chkboxmail.Checked == false)
                {
                    if (flagstudent == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Staff Message has been sent')", true);
                    }
                }
                if (chkboxmail.Checked == true && chkboxsms.Checked == false)
                {
                    if (flagstudent == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Staff Mail has been sent')", true);
                    }
                }
                if (chkboxmail.Checked == true && chkboxsms.Checked == true)
                {
                    if (flagstudent == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Staff Message/Mail has been sent')", true);
                    }
                }
            }
        }
        catch
        {
        }
    }
    //modified by srinath
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "DEANSEC")
    //        {
    //            SenderID = "DEANSE";
    //            Password = "DEANSEC";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "SASTHA")
    //        {
    //            SenderID = "SASTHA";
    //            Password = "SASTHA";
    //        }
    //        else if (user_id == "SSMCE")
    //        {
    //            SenderID = "SSMCE";
    //            Password = "SSMCE";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "DHIRA")
    //        {
    //            SenderID = "DHIRAJ";
    //            Password = "DHIRA";
    //        }
    //        else if (user_id == "ANGEL123")
    //        {
    //            SenderID = "ANGELS";
    //            Password = "ANGEL123";
    //        }
    //        else if (user_id == "BALAJI12")
    //        {
    //            SenderID = "BALAJI";
    //            Password = "BALAJI12";
    //        }
    //        else if (user_id == "AKSHYA123")
    //        {
    //            SenderID = "AKSHYA";
    //            Password = "AKSHYA";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "JJCET")
    //        {
    //            SenderID = "JJCET";
    //            Password = "JJCET";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "AMSECE")
    //        {
    //            SenderID = "AMSECE";
    //            Password = "AMSECE";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSEC")
    //        {
    //            SenderID = "DCTSEC";
    //            Password = "DCTSEC";
    //        }
    //        else if (user_id == "DCTSBS")
    //        {
    //            SenderID = "DCTSBS";
    //            Password = "DCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "AIHTCH")
    //        {
    //            SenderID = "AIHTCH";
    //            Password = "AIHTCH";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SCHCLG")
    //        {
    //            SenderID = "SCHCLG";
    //            Password = "SCHCLG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "SRECTD")
    //        {
    //            SenderID = "SRECTD";
    //            Password = "SRECTD";
    //        }
    //        else if (user_id == "EICTPC")
    //        {
    //            SenderID = "EICTPC";
    //            Password = "EICTPC";
    //        }
    //        else if (user_id == "SHACLG")
    //        {
    //            SenderID = "SHACLG";
    //            Password = "SHACLG";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "TECAAA")
    //        {
    //            SenderID = "TECAAA";
    //            Password = "TECAAA";
    //        }
    //        else if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "SVISTE")
    //        {
    //            SenderID = "SVISTE";
    //            Password = "SVISTE";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //        else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //        else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }
    //        else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }
    //        else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        }
    //        else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        }
    //        else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        }
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //Modified by srinath 8/2/2014
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "AMSPTC")
    //        {
    //            SenderID = "AMSPTC";
    //            Password = "AMSPTC";
    //        }
    //        else if (user_id == "ANGE")
    //        {
    //            SenderID = "ANGE";
    //            Password = "ANGE";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }
    //        else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        }
    //        else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "JJAAMC")
    //        {
    //            SenderID = "JJAAMC";
    //            Password = "JJAAMC";
    //        }
    //        else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        }
    //        else if (user_id == "KNMHSS")
    //        {
    //            SenderID = "KNMHSS";
    //            Password = "KNMHSS";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "SASTH")
    //        {
    //            SenderID = "SASTH";
    //            Password = "SASTH";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SKCETC")
    //        {
    //            SenderID = "SKCETC";
    //            Password = "SKCETC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "SSMCEE")
    //        {
    //            SenderID = "SSMCEE";
    //            Password = "SSMCEE";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //        else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        else if (user_id == "AUDIIT")
    //        {
    //            SenderID = "AUDIIT";
    //            Password = "AUDIIT";
    //        }
    //        else if (user_id == "SAENGG")
    //        {
    //            SenderID = "SAENGG";
    //            Password = "SAENGG";
    //        }
    //        else if (user_id == "STANE")
    //        {
    //            SenderID = "STANES";
    //            Password = "STANES";
    //        }
    //        else if (user_id == "MBCBSE")
    //        {
    //            SenderID = "MBCBSE";
    //            Password = "MBCBSE";
    //        }
    //        else if (user_id == "HIETPT")
    //        {
    //            SenderID = "HIETPT";
    //            Password = "HIETPT";
    //        }
    //        else if (user_id == "SVPITM")
    //        {
    //            SenderID = "SVPITM";
    //            Password = "SVPITM";
    //        }
    //        else if (user_id == "AUDCET")
    //        {
    //            SenderID = "AUDCET";
    //            Password = "AUDCET";
    //        }
    //        else if (user_id == "AUDWOM")
    //        {
    //            SenderID = "AUDWOM";
    //            Password = "AUDWOM";
    //        }
    //        else if (user_id == "AUDIPG")
    //        {
    //            SenderID = "AUDIPG";
    //            Password = "AUDIPG";
    //        }
    //        else if (user_id == "MCCDAY")
    //        {
    //            SenderID = "MCCDAY";
    //            Password = "MCCDAY";
    //        }
    //        else if (user_id == "MCCSFS")
    //        {
    //            SenderID = "MCCSFS";
    //            Password = "MCCSFS";
    //        }
    //        else if (user_id == "JMHRSS")
    //        {
    //            SenderID = "JMHRSS";
    //            Password = "JMHRSS";
    //        }
    //        else if (user_id == "JHSSCB")
    //        {
    //            SenderID = "JHSSCB";
    //            Password = "JHSSCB";
    //        } 
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch
    //    {
    //    }
    //}
    #endregion

    #region "Convert to Excel Function"
    protected void btnxl_Click(object sender, EventArgs e)
    {
    }
    #endregion

    protected void FpSpread2_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch
        {
        }
    }

    protected void FpSpread2_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Cellclick = true;
            if (Cellclick == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1)
                {
                    txtmessage.Text = FpSpread2.Sheets[0].GetText(ar, 1);
                }
                Cellclick = false;
            }
        }
        catch
        {
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string strsavequery = "insert into sms_template (temp_code,Template,college_code)values( '" + ddlpurposemsg.SelectedValue.ToString() + "','" + txtpurposemsg.Text.ToString() + "','" + ddlcollege.SelectedValue.ToString() + "')";
            i = d2.insert_method(strsavequery, hat, "Text");
            if (i == 1)
            {
                lblerror.Visible = true;
                lblerror.Text = "Template added Succefully";
                Dropdownload();
            }
            else
            {
                lblerror.Text = "Template added failed";
            }
        }
        //Spread2Go();
        catch
        {
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            templatepanel.Visible = false;
            purposepanel.Visible = false;
            Dropdownload();
        }
        catch
        {
        }
    }

    protected void btnpurposeadd_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string strtxtpurpose = string.Empty;
            strtxtpurpose = txtpurposecaption.Text;
            if (strtxtpurpose != "")
            {
                string strinsertpurpose = "insert into sms_purpose (Purpose,college_code) values ( '" + strtxtpurpose + "','" + ddlcollege.SelectedValue.ToString() + "')";
                i = d2.insert_method(strinsertpurpose, hat, "Text");
                //  txtpurposecaption.Text = "";
                if (i == 1)
                {
                    lblerror.Text = "Purpose added Successfully";
                    lblerror.Visible = true;
                    Dropdownload();
                    ddlpurposemsg.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
                    ddlpurpose.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
                    // purposepanel.Visible = false;
                }
                else
                {
                    lblerror.Text = "Purpose added failed";
                    lblerror.Visible = true;
                }
            }
            else
            {
                lblsendmail.Text = "Please Enter the Purpose";
                lblsendmail.Visible = true;
            }
            txtpurposecaption.Text = "";
            Spread2Go();
        }
        catch
        {
        }
    }

    protected void btnpurposeexit_Click(object sender, EventArgs e)
    {
        try
        {
            templatepanel.Enabled = true;
            purposepanel.Visible = false;
        }
        catch
        {
        }
    }

    //protected void chkpurpose_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkpurpose.Checked == true)
    //    {
    //        FpSpread2.Visible = false;
    //        ddlpurpose.Visible = true;
    //        btndeletetemplate.Visible = false;
    //        Dropdownload();
    //        Spread2Go();
    //    }
    //    else
    //    {
    //        Spread2Go();
    //        ddlpurpose.Items.Clear();
    //        btndeletetemplate.Visible = true;
    //    }
    //}

    public void Dropdownload()
    {
        try
        {
            ds1.Dispose();
            ds1.Reset();
            string strpurposename = "select purpose,temp_code from sms_purpose where college_code = '" + ddlcollege.SelectedValue.ToString() + "'";
            ds1 = d2.select_method(strpurposename, hat, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddlpurpose.DataSource = ds1;
                ddlpurpose.DataTextField = "Purpose";
                ddlpurpose.DataValueField = "temp_code";
                ddlpurpose.DataBind();
                ddlpurpose.Items.Add(" ");
                ddlpurpose.Text = " ";
                ddlpurposemsg.DataSource = ds1;
                ddlpurposemsg.DataTextField = "Purpose";
                ddlpurposemsg.DataValueField = "temp_code";
                ddlpurposemsg.DataBind();
                ddlpurposemsg.Items.Add(" ");
                ddlpurposemsg.Text = " ";
            }
        }
        catch
        {
        }
    }

    protected void ddlpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Spread2Go();
        FpSpread2.Visible = true;
        try
        {
            FpSpread2.Sheets[0].ColumnHeaderVisible = false;
            FpSpread2.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FpSpread2.Visible = true;
            //lblpurpose1.Visible = true;
            ddlpurpose.Visible = true;
            FpSpread2.Sheets[0].RowCount = 1;
            FpSpread2.Sheets[0].ColumnCount = 2;
            FpSpread2.Columns[1].Width = 900;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = "Template";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
            string gfg = ddlpurpose.SelectedValue.ToString();
            string gfvgj = ddlpurposemsg.Text;
            if (gfg == " ")
            {
                ds.Dispose();
                ds.Reset();
                string spread2query = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template";
                ds = d2.select_method(spread2query, hat, "Text");
            }
            else
            {
                string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template where temp_code = " + ddlpurpose.SelectedValue + "";
                ds = d2.select_method(spread2query1, hat, "Text");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                }
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.SaveChanges();
        }
        catch
        {
        }
    }

    protected void tbdeg_TextChanged(object sender, EventArgs e)
    {
    }

    protected void txtpurposemsg_TextChanged(object sender, EventArgs e)
    {
    }

    protected void btnstaffgo_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread3.Visible = false;
            Tablenote.Visible = false;
            lblsubject.Visible = false;
            lblnotification.Visible = false;
            txtsubject.Visible = false;
            lblnote.Visible = false;
            txtnotification.Visible = false;
            lblfile.Visible = false;
            lblattachements.Visible = false;
            fudfile.Visible = false;
            fudattachemnts.Visible = false;
            //btnnotfsave.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            string value = "";
            string code = "";
            string staffvalue = "";
            string staffcode = "";
            string designvalue = "";
            string stafftyp = "";
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
            if (staffvalue.Trim() != "")
            {
                stafftyp = " and st.stftype in (" + staffvalue + ")";
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
            if (deptvalue.Trim() != "")
            {
                deptvalue = " and st.dept_code in ( " + deptvalue + " )";
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
            if (designvalue.Trim() != "")
            {
                designvalue = "and st.desig_code in ( " + designvalue + ")";
            }

            #region Transport
            string routeId = string.Empty;
            string vehiID = string.Empty;
            string stageId = string.Empty;
            if (cbTrans.Checked)
            {
                routeId = Convert.ToString(getCblSelectedValue(cblroute));
                vehiID = Convert.ToString(getCblSelectedValue(cblvechile));
                stageId = Convert.ToString(getCblSelectedValue(cblstage));
            }
            #endregion

            #region vehicle type
            string vehicleType = string.Empty;
            if (cbvehicleType.Checked && ddlvehType.Items.Count > 0)
            {
                if (ddlvehType.SelectedIndex == 0)//own vehicle                 
                    vehicleType = "  and ((isnull(sm.Bus_RouteID,'')='' and isnull(sm.Boarding,'')='' and isnull(sm.VehID,'')='')) ";
                else if (ddlvehType.SelectedIndex == 1)//college vehicle                
                    vehicleType = "  and ((isnull(sm.Bus_RouteID,'')<>'' and isnull(sm.Boarding,'')<>'' and isnull(sm.VehID,'')<>'')) ";
                else //both vehicle                
                    vehicleType = " ";
            }
            #endregion

            if (rdbtnstaff.Checked == true)
            {
                // string strstaffdetail = "select distinct sm.staff_code,sm.staff_name,st.stftype,sam.per_mobileno,sam.email from staffmaster sm,stafftrans st,staff_appl_master sam where st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no and latestrec = 1 and sm.college_code = " + ddlcollege.SelectedValue.ToString() + " and st.dept_code in ( " + deptvalue + " ) and st.desig_code in ( " + designvalue + ") "+stafftyp+" and resign = 0 and settled = 0 order by staff_name";
                //string strstaffdetail = "select distinct sm.staff_code,sm.staff_name,st.stftype,sam.per_mobileno,sam.email,h.dept_name  from staffmaster sm,stafftrans st,staff_appl_master sam, hrdept_master h where st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no and h.dept_code=sam.dept_code and st.dept_code =h.dept_code and latestrec = 1 and sm.college_code = " + ddlcollege.SelectedValue.ToString() + " " + deptvalue + " " + designvalue + " " + stafftyp + " and resign = 0 and settled = 0 order by h.dept_name , sm.staff_code";//modified by srinath 4/9/2014
                if (!chksmsgroup.Checked)
                {
                    string strstaffdetail = "select distinct sm.staff_code,sm.staff_name,st.stftype,sam.per_mobileno,sam.email,h.dept_name ,sam.appl_id as App_No from staffmaster sm,stafftrans st,staff_appl_master sam, hrdept_master h where st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no  and st.dept_code =h.dept_code and latestrec = 1 and sm.college_code = " + ddlcollege.SelectedValue.ToString() + " " + deptvalue + " " + designvalue + " " + stafftyp + " and resign = 0 and settled = 0 ";
                    if (!string.IsNullOrEmpty(routeId))
                        strstaffdetail += " and sm.bus_routeid in('" + routeId + "')";
                    if (!string.IsNullOrEmpty(vehiID))
                        strstaffdetail += " and sm.vehid in('" + vehiID + "')";
                    if (!string.IsNullOrEmpty(stageId))
                        strstaffdetail += " and sm.boarding in('" + stageId + "')";
                    if (!string.IsNullOrEmpty(vehicleType))
                        strstaffdetail += vehicleType;
                    strstaffdetail += " order by h.dept_name , sm.staff_code";
                    MethodStaffGo(strstaffdetail);
                }
                else
                {
                    string strstaffdetail = "select MasterValue,MasterCode from CO_MasterValues where collegeCode='" + collegecode + "' and MasterCriteria='smsGroup'";
                    MethodStaffGoSms1(strstaffdetail);
                }
                Dropdownload();
                Spread2Go();
            }

            if (chknotification.Checked == true)
            {
                Tablenote.Visible = true;
                lblsubject.Visible = true;
                lblnotification.Visible = true;
                txtsubject.Visible = true;
                lblnote.Visible = true;
                txtnotification.Visible = true;
                lblfile.Visible = true;
                lblattachements.Visible = true;
                fudfile.Visible = true;
                fudattachemnts.Visible = true;
                //btnnotfsave.Visible = true;
                btnsend.Visible = true;
                Divv2.Visible = true;

            }
            //magesh
            if (chkboxmail.Checked == true && chkboxsms.Checked == true)
            {
                tblmail.Visible = true;
                txtmessage.Visible = true;
                //btnsms.Visible = true;
                txtmessage.Visible = true;
                //btnsms.Visible = true;
                lblpurpose1.Visible = true;
                ddlpurpose.Visible = true;
                FpSpread2.Visible = true;
                btnaddtemplate.Visible = true;
                btndeletetemplate.Visible = true;
                Label7.Visible = true;
                Div7.Visible = true;
                btnsend.Visible = true;
                Divv1.Visible = true;
            }
            else
            {
                if (chkboxmail.Checked == false && chkboxsms.Checked == true)
                {
                    tblmail.Visible = false;
                    txtmessage.Visible = true;
                    //btnsms.Visible = true;
                    lblpurpose1.Visible = true;
                    ddlpurpose.Visible = true;
                    FpSpread2.Visible = true;
                    btnaddtemplate.Visible = true;
                    btndeletetemplate.Visible = true;
                    Label7.Visible = true;
                    Div7.Visible = true;
                    btnsend.Visible = true;
                    Divv1.Visible = false;

                }
                else
                {
                    txtmessage.Visible = false;
                    //btnsms.Visible = false;
                    lblpurpose1.Visible = false;
                    ddlpurpose.Visible = false;
                    FpSpread2.Visible = false;
                    btnaddtemplate.Visible = false;
                    btndeletetemplate.Visible = false;
                    txtmessage.Visible = false;
                    Label7.Visible = false;
                    Div7.Visible = false;
                }

                if (chkboxmail.Checked == true && chkboxsms.Checked == false)
                {
                    tblmail.Visible = true;
                    txtmessage.Visible = false;
                    // btnsms.Visible = false;
                    lblpurpose1.Visible = false;
                    ddlpurpose.Visible = false;
                    FpSpread2.Visible = false;
                    btnaddtemplate.Visible = false;
                    btndeletetemplate.Visible = false;
                    Label7.Visible = false;
                    Div7.Visible = false;
                    btnsend.Visible = true;
                    Divv1.Visible = true;

                }
                else
                {
                    tblmail.Visible = false;

                }
            }//magesh
            if (chkvoicecall.Checked == true)
            {
                Fpspreadvoice.CommandBar.Visible = false;
                panelvoice.Visible = true;
                bindspreadvoice();
                Div5.Visible = true;
            }
            else
            {
                Fpspreadvoice.CommandBar.Visible = false;
                panelvoice.Visible = false;
                Div5.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void chkdesignation_CheckedChanged(object sender, EventArgs e)
    {
        try
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
        catch
        {
        }
    }

    protected void chklstdesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
        catch
        {
        }
    }

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
        catch
        {
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        try
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
        catch
        {
        }
    }

    protected void Chkboxstafftype_CheckedChanged(object sender, EventArgs e)
    {
        try
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
        catch
        {
        }
    }

    protected void Chhliststafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
            // bind_design();
        }
        catch
        {
        }
    }

    protected void notificationsend()
    {
        try
        {
            string viewer = "", notificationdate = "", subject = "", notifiaction = "", filetype = "", isstaff = "";
            string file_extension = "", file_type = "";
            int fileSize = 0;
            byte[] documentBinary = new byte[0];
            byte[] attchementfile = new byte[0];
            int attachfile = 0;
            string attchefileexten = "", attachfiletype = "";
            Boolean atchflag = false;
            string filename = "";
            if (fudattachemnts.HasFile)
            {
                if (fudattachemnts.FileName.EndsWith(".txt") || fudattachemnts.FileName.EndsWith(".pdf") || fudattachemnts.FileName.EndsWith(".doc") || fudattachemnts.FileName.EndsWith(".xls") || fudattachemnts.FileName.EndsWith(".xlsx") || fudattachemnts.FileName.EndsWith(".docx"))
                {
                    atchflag = true;
                    attachfile = fudattachemnts.PostedFile.ContentLength;
                    attchementfile = new byte[attachfile];
                    fudattachemnts.PostedFile.InputStream.Read(attchementfile, 0, attachfile);
                    filename = fudattachemnts.PostedFile.FileName;
                    attchefileexten = Path.GetExtension(fudattachemnts.PostedFile.FileName);
                    attachfiletype = Get_file_format(attchefileexten);
                }
                else
                {
                    errnote.Visible = true;
                    errnote.Text = "Please Select File Formate Like (.txt,.pdf,.doc,.xls,.xlsx,.docx)";
                    return;
                }
            }
            Boolean fle = false;
            if (fudfile.HasFile)
            {
                if (fudfile.FileName.EndsWith(".jpg") || fudfile.FileName.EndsWith(".jpeg") || fudfile.FileName.EndsWith(".JPG") || fudfile.FileName.EndsWith(".gif") || fudfile.FileName.EndsWith(".png"))
                {
                    fle = true;
                    fileSize = fudfile.PostedFile.ContentLength;
                    documentBinary = new byte[fileSize];
                    fudfile.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    file_extension = Path.GetExtension(fudfile.PostedFile.FileName);
                    file_type = Get_file_format(file_extension);
                }
                else
                {
                    errnote.Visible = true;
                    errnote.Text = "Please Select Image Formate Like (.jpg,.peg,.JPG,.gif,.png)";
                    return;
                }
            }
            Boolean saveflag = false;
            if (!chksmsgroup.Checked)
            {
                #region
                string senderid = "", senderstaff = "0", descrip = "";
                string staffcode = Session["Staff_Code"].ToString();//saranya
                ds.Reset();
                ds.Dispose();
                string strquery = "";
                if (staffcode != "" && staffcode != null)
                {
                    senderstaff = "1";
                    strquery = "select Staff_name,dm.desig_name,hm.dept_name from staffmaster sm,stafftrans st,Desig_Master dm,HRDept_Master hm where sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and sm.staff_code='" + staffcode + "'";
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        senderid = ds.Tables[0].Rows[0]["Staff_name"].ToString();
                        descrip = ds.Tables[0].Rows[0]["desig_name"].ToString() + " - " + ds.Tables[0].Rows[0]["dept_name"].ToString();
                    }
                }
                else
                {
                    if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                    {
                        group_user = Session["group_code"].ToString();
                        if (group_user.Contains(';'))
                        {
                            string[] group_semi = group_user.Split(';');
                            group_user = group_semi[0].ToString();
                        }
                        strquery = "select full_name,description from usermaster where group_code='" + group_user + "'";
                    }
                    else
                    {
                        strquery = "select full_name,description from usermaster where user_code='" + Session["UserCode"].ToString() + "' ";
                    }
                    usercode = Session["usercode"].ToString();
                    group_user = Session["group_code"].ToString();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        senderid = ds.Tables[0].Rows[0]["full_name"].ToString();
                        descrip = ds.Tables[0].Rows[0]["description"].ToString();
                    }
                }
                saveflag = false;
                for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    int isval = 0;
                    if (rdbtnstudent.Checked == true)
                    {
                        isstaff = "0";
                        isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 11].Value);
                    }
                    else if (rdbtnstaff.Checked == true)
                    {
                        isstaff = "1";
                        isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 6].Value);
                    }
                    if (isval == 1)
                    {
                        string query = "";
                        if (fle == false && atchflag == false)
                        {
                            query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff)";
                            query = query + "  values(@viewrs,@notification_date,@notification_time,@subject,@notification,@isstaff,@College_code,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff)";
                        }
                        else if (fle == true && atchflag == false)
                        {
                            query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,filetype,fileupload,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff)";
                            query = query + "  values(@viewrs,@notification_date,@notification_time,@subject,@notification,@filetype,@fileupload,@isstaff,@College_code,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff)";
                        }
                        else if (fle == false && atchflag == true)
                        {
                            query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filename)";
                            query = query + "  values(@viewrs,@notification_date,@notification_time,@subject,@notification,@isstaff,@College_code,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff,@attche_filetype,@attache_file,@filename)";
                        }
                        else if (fle == true && atchflag == true)
                        {
                            query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filetype,fileupload,filename)";
                            query = query + "  values(@viewrs,@notification_date,@notification_time,@subject,@notification,@isstaff,@College_code,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff,@attche_filetype,@attache_file,@filetype,@fileupload,@filename)";
                        }
                        SqlCommand cmd = new SqlCommand(query, mysql);
                        SqlParameter uploadedsubject_name = new SqlParameter("@isstaff", SqlDbType.Int, 50);
                        uploadedsubject_name.Value = isstaff;
                        cmd.Parameters.Add(uploadedsubject_name);
                        if (atchflag == true)
                        {
                            uploadedsubject_name = new SqlParameter("@attche_filetype", SqlDbType.VarChar, 50);
                            uploadedsubject_name.Value = attachfiletype;
                            cmd.Parameters.Add(uploadedsubject_name);
                            uploadedsubject_name = new SqlParameter("@attache_file", SqlDbType.Binary, attachfile);
                            uploadedsubject_name.Value = attchementfile;
                            cmd.Parameters.Add(uploadedsubject_name);
                            uploadedsubject_name = new SqlParameter("@filename", SqlDbType.VarChar, 200);
                            uploadedsubject_name.Value = filename;
                            cmd.Parameters.Add(uploadedsubject_name);
                        }
                        uploadedsubject_name = new SqlParameter("@staff_code", SqlDbType.VarChar, 50);
                        uploadedsubject_name.Value = staffcode;
                        cmd.Parameters.Add(uploadedsubject_name);
                        uploadedsubject_name = new SqlParameter("@sender_id", SqlDbType.VarChar, 50);
                        uploadedsubject_name.Value = senderid;
                        cmd.Parameters.Add(uploadedsubject_name);
                        uploadedsubject_name = new SqlParameter("@Sender_Description", SqlDbType.VarChar, 50);
                        uploadedsubject_name.Value = descrip;
                        cmd.Parameters.Add(uploadedsubject_name);
                        uploadedsubject_name = new SqlParameter("@sender_staff", SqlDbType.VarChar, 50);
                        uploadedsubject_name.Value = senderstaff;
                        cmd.Parameters.Add(uploadedsubject_name);
                        viewer = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
                        uploadedsubject_name = new SqlParameter("@viewrs", SqlDbType.NVarChar, 100);
                        uploadedsubject_name.Value = viewer;
                        cmd.Parameters.Add(uploadedsubject_name);
                        string dtdate = DateTime.Now.ToString("MM/dd/yyyy");
                        uploadedsubject_name = new SqlParameter("@notification_date", SqlDbType.VarChar, 50);
                        uploadedsubject_name.Value = dtdate;
                        cmd.Parameters.Add(uploadedsubject_name);
                        string dttime = DateTime.Now.ToLongTimeString();
                        uploadedsubject_name = new SqlParameter("@notification_time", SqlDbType.VarChar, 50);
                        uploadedsubject_name.Value = dttime;
                        cmd.Parameters.Add(uploadedsubject_name);
                        subject = txtsubject.Text.ToString().Trim();
                        if (subject == "" || subject == null)
                        {
                            errnote.Visible = true;
                            errnote.Text = "Please Enter Subject";
                            return;
                        }
                        uploadedsubject_name = new SqlParameter("@subject", SqlDbType.NVarChar, 200);
                        uploadedsubject_name.Value = subject;
                        cmd.Parameters.Add(uploadedsubject_name);
                        notifiaction = txtnotification.Text.ToString().Trim();
                        if (notifiaction == null || notifiaction == "")
                        {
                            errnote.Visible = true;
                            errnote.Text = "Please Enter Notification";
                            return;
                        }
                        if (notifiaction.Length > 8999)
                        {
                            errnote.Visible = true;
                            errnote.Text = "Please Enter Notification Less than 9000 Character";
                            return;
                        }
                        uploadedsubject_name = new SqlParameter("@notification", SqlDbType.NVarChar, 1000);
                        uploadedsubject_name.Value = notifiaction;
                        cmd.Parameters.Add(uploadedsubject_name);
                        collegecode = ddlcollege.SelectedValue.ToString();
                        uploadedsubject_name = new SqlParameter("@College_code", SqlDbType.Int);
                        uploadedsubject_name.Value = collegecode;
                        cmd.Parameters.Add(uploadedsubject_name);
                        string staus = "0";
                        uploadedsubject_name = new SqlParameter("@status", SqlDbType.Int);
                        uploadedsubject_name.Value = staus;
                        cmd.Parameters.Add(uploadedsubject_name);
                        if (fle == true)
                        {
                            uploadedsubject_name = new SqlParameter("@filetype", SqlDbType.VarChar, 50);
                            uploadedsubject_name.Value = file_type;
                            cmd.Parameters.Add(uploadedsubject_name);
                            uploadedsubject_name = new SqlParameter("@fileupload", SqlDbType.Binary, fileSize);
                            uploadedsubject_name.Value = documentBinary;
                            cmd.Parameters.Add(uploadedsubject_name);
                        }
                        mysql.Close();
                        mysql.Open();
                        cmd.ExecuteNonQuery();
                        mysql.Close();
                        saveflag = true;
                    }
                }
            }
                #endregion
            else//saranya
            {
                #region
                for (int i = 1; i < fpMsg.Sheets[0].RowCount; i++)
                {
                    //magesh 12.2.18
                    isstaff = "1";
                    int isval = Convert.ToInt32(fpMsg.Sheets[0].Cells[i, 2].Value);
                    if (isval == 1)
                    {
                        strstuname = Convert.ToString(fpMsg.Sheets[0].Cells[i, 1].Tag);
                        string selQ = " select distinct staff_code,sam.per_mobileno,sam.email from staffmaster sm,staff_appl_master sam where  sm.appl_no = sam.appl_no and sms_groupCode in('" + strstuname + "')";
                        DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                        {
                            string senderid = string.Empty;
                            string descrip = string.Empty;
                            string senderstaff = "1";
                            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                            {
                                string staffcode = Convert.ToString(dsVal.Tables[0].Rows[row]["staff_code"]);
                                string strquery = "select Staff_name,dm.desig_name,hm.dept_name from staffmaster sm,stafftrans st,Desig_Master dm,HRDept_Master hm where sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and sm.staff_code='" + staffcode + "'";
                                ds = d2.select_method_wo_parameter(strquery, "Text");
                                ds = d2.select_method_wo_parameter(strquery, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    senderid = ds.Tables[0].Rows[0]["Staff_name"].ToString();
                                    descrip = ds.Tables[0].Rows[0]["desig_name"].ToString() + " - " + ds.Tables[0].Rows[0]["dept_name"].ToString();
                                    senderstaff = "1";
                                }
                                //  arStaff.Add(to_mail);
                                if (isval == 1)
                                {
                                    string query = "";
                                    if (fle == false && atchflag == false)
                                    {
                                        query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff)";
                                        query = query + "  values(@viewrs,@notification_date,@notification_time,@subject,@notification,@isstaff,@College_code,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff)";
                                    }
                                    else if (fle == true && atchflag == false)
                                    {
                                        query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,filetype,fileupload,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff)";
                                        query = query + "  values(@viewrs,@notification_date,@notification_time,@subject,@notification,@filetype,@fileupload,@isstaff,@College_code,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff)";
                                    }
                                    else if (fle == false && atchflag == true)
                                    {
                                        query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filename)";
                                        query = query + "  values(@viewrs,@notification_date,@notification_time,@subject,@notification,@isstaff,@College_code,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff,@attche_filetype,@attache_file,@filename)";
                                    }
                                    else if (fle == true && atchflag == true)
                                    {
                                        query = "insert into tbl_notification(viewrs,notification_date,notification_time,subject,notification,isstaff,College_code,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filetype,fileupload,filename)";
                                        query = query + "  values(@viewrs,@notification_date,@notification_time,@subject,@notification,@isstaff,@College_code,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff,@attche_filetype,@attache_file,@filetype,@fileupload,@filename)";
                                    }
                                    SqlCommand cmd = new SqlCommand(query, mysql);
                                    SqlParameter uploadedsubject_name = new SqlParameter("@isstaff", SqlDbType.Int, 50);
                                    uploadedsubject_name.Value = isstaff;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    if (atchflag == true)
                                    {
                                        uploadedsubject_name = new SqlParameter("@attche_filetype", SqlDbType.VarChar, 50);
                                        uploadedsubject_name.Value = attachfiletype;
                                        cmd.Parameters.Add(uploadedsubject_name);
                                        uploadedsubject_name = new SqlParameter("@attache_file", SqlDbType.Binary, attachfile);
                                        uploadedsubject_name.Value = attchementfile;
                                        cmd.Parameters.Add(uploadedsubject_name);
                                        uploadedsubject_name = new SqlParameter("@filename", SqlDbType.VarChar, 200);
                                        uploadedsubject_name.Value = filename;
                                        cmd.Parameters.Add(uploadedsubject_name);
                                    }
                                    uploadedsubject_name = new SqlParameter("@staff_code", SqlDbType.VarChar, 50);
                                    uploadedsubject_name.Value = staffcode;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    uploadedsubject_name = new SqlParameter("@sender_id", SqlDbType.VarChar, 50);
                                    uploadedsubject_name.Value = senderid;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    uploadedsubject_name = new SqlParameter("@Sender_Description", SqlDbType.VarChar, 50);
                                    uploadedsubject_name.Value = descrip;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    uploadedsubject_name = new SqlParameter("@sender_staff", SqlDbType.VarChar, 50);
                                    uploadedsubject_name.Value = senderstaff;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    //magesh 12.2.18
                                    viewer = Convert.ToString(dsVal.Tables[0].Rows[row]["staff_code"]).Trim();
                                    uploadedsubject_name = new SqlParameter("@viewrs", SqlDbType.NVarChar, 100);
                                    uploadedsubject_name.Value = viewer;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    string dtdate = DateTime.Now.ToString("MM/dd/yyyy");
                                    uploadedsubject_name = new SqlParameter("@notification_date", SqlDbType.VarChar, 50);
                                    uploadedsubject_name.Value = dtdate;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    string dttime = DateTime.Now.ToLongTimeString();
                                    uploadedsubject_name = new SqlParameter("@notification_time", SqlDbType.VarChar, 50);
                                    uploadedsubject_name.Value = dttime;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    subject = txtsubject.Text.ToString().Trim();
                                    if (subject == "" || subject == null)
                                    {
                                        errnote.Visible = true;
                                        errnote.Text = "Please Enter Subject";
                                        return;
                                    }
                                    uploadedsubject_name = new SqlParameter("@subject", SqlDbType.NVarChar, 200);
                                    uploadedsubject_name.Value = subject;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    notifiaction = txtnotification.Text.ToString().Trim();
                                    if (notifiaction == null || notifiaction == "")
                                    {
                                        errnote.Visible = true;
                                        errnote.Text = "Please Enter Notification";
                                        return;
                                    }
                                    if (notifiaction.Length > 8999)
                                    {
                                        errnote.Visible = true;
                                        errnote.Text = "Please Enter Notification Less than 9000 Character";
                                        return;
                                    }
                                    uploadedsubject_name = new SqlParameter("@notification", SqlDbType.NVarChar, 1000);
                                    uploadedsubject_name.Value = notifiaction;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    collegecode = ddlcollege.SelectedValue.ToString();
                                    uploadedsubject_name = new SqlParameter("@College_code", SqlDbType.Int);
                                    uploadedsubject_name.Value = collegecode;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    string staus = "0";
                                    uploadedsubject_name = new SqlParameter("@status", SqlDbType.Int);
                                    uploadedsubject_name.Value = staus;
                                    cmd.Parameters.Add(uploadedsubject_name);
                                    if (fle == true)
                                    {
                                        uploadedsubject_name = new SqlParameter("@filetype", SqlDbType.VarChar, 50);
                                        uploadedsubject_name.Value = file_type;
                                        cmd.Parameters.Add(uploadedsubject_name);
                                        uploadedsubject_name = new SqlParameter("@fileupload", SqlDbType.Binary, fileSize);
                                        uploadedsubject_name.Value = documentBinary;
                                        cmd.Parameters.Add(uploadedsubject_name);
                                    }
                                    mysql.Close();
                                    mysql.Open();
                                    cmd.ExecuteNonQuery();
                                    mysql.Close();
                                    saveflag = true;
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            txtnotification.Text = "";
            txtsubject.Text = "";
            if (saveflag == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Send Successfully')", true);
                // FpSpread3.Visible = false;
            }
            else
            {
                if (rdbtnstudent.Checked == true)
                {
                    errnote.Text = "Please Select Student's and proceed";
                    errnote.Visible = true;
                }
                else if (rdbtnstaff.Checked == true)
                {
                    errnote.Text = "Please Select Staff's and proceed";
                    errnote.Visible = true;
                }
            }
        }
        catch
        {
        }
    }

    protected void chknotification_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            Tablenote.Visible = false;
            lblsubject.Visible = false;
            lblnotification.Visible = false;
            txtsubject.Visible = false;
            lblnote.Visible = false;
            txtnotification.Visible = false;
            lblfile.Visible = false;
            lblattachements.Visible = false;
            fudfile.Visible = false;
            fudattachemnts.Visible = false;
            Divv2.Visible = false;

            // btnnotfsave.Visible = false;
            if (FpSpread1.Visible == true)
            {
                if (chknotification.Checked == true)
                {
                    txtnotification.Text = "";
                    txtsubject.Text = "";
                    Tablenote.Visible = true;
                    lblsubject.Visible = true;
                    lblnotification.Visible = true;
                    txtsubject.Visible = true;
                    lblnote.Visible = true;
                    txtnotification.Visible = true;
                    lblfile.Visible = true;
                    lblattachements.Visible = true;
                    fudfile.Visible = true;
                    fudattachemnts.Visible = true;
                    btnsend.Visible = true;
                    Divv2.Visible = true;
                    if (btnstaffgo.Visible == true)
                    {
                        btnstaffgo_Click(sender, e);
                    }
                    if (btngo.Visible == true)
                    {
                        btngo_Click(sender, e);
                    }
                    //btnnotfsave.Visible = true;

                }
            }
        }
        catch
        {
        }
    }

    protected void chkboxsms_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkboxsms.Checked == true)
            {
                if (btnstaffgo.Visible == true)
                {
                    btnstaffgo_Click(sender, e);
                }
                if (btngo.Visible == true)
                {
                    btngo_Click(sender, e);
                }
                RbEnglish.Checked = true;
                Div7.Visible = true;
                btnsend.Visible = true;
            }
            else
            {
                Div7.Visible = false;
                btnsend.Visible = false;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                Tablenote.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void chkboxmail_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkboxmail.Checked == true)
            {
                if (btnstaffgo.Visible == true)
                {
                    btnstaffgo_Click(sender, e);
                }
                if (btngo.Visible == true)
                {
                    btngo_Click(sender, e);

                }
                Divv1.Visible = true;
                btnsend.Visible = true;
            }
            else
            {
                Divv1.Visible = false;
                btnsend.Visible = false;
            }
        }
        catch
        {
        }
    }

    public string Get_file_format(string file_extension)
    {
        try
        {
            string file_type = "";
            switch (file_extension)
            {
                case ".pdf":
                    file_type = "application/pdf";
                    break;
                case ".txt":
                    file_type = "application/notepad";
                    break;
                case ".xls":
                    file_type = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    file_type = "application/vnd.ms-excel";
                    break;
                case ".doc":
                    file_type = "application/vnd.ms-word";
                    break;
                case ".docx":
                    file_type = "application/vnd.ms-word";
                    break;
                case ".gif":
                    file_type = "image/gif";
                    break;
                case ".png":
                    file_type = "image/png";
                    break;
                case ".jpg":
                    file_type = "image/jpg";
                    break;
                case ".jpeg":
                    file_type = "image/jpeg";
                    break;
            }
            return file_type;
        }
        catch
        {
            return null;
        }
    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Divv1.Visible = false;
        Divv2.Visible = false;
        btnsend.Visible = false;

        try
        {
            string actrow = e.CommandArgument.ToString();

            if (flag_true == false && actrow == "0")
            {
                string seltext = "";
                string actcol = "";
                if (rdbtnstudent.Checked == true)
                {
                    actcol = "11";
                }
                else if (rdbtnstaff.Checked == true)
                {
                    if (!chksmsgroup.Checked)
                    {
                        actcol = "6";
                    }
                    else
                    {
                        actcol = "2";
                    }
                }
                seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                for (int j = 1; j < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); j++)
                {
                    if (seltext != "System.Object")
                        FpSpread1.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
                }
                flag_true = true;
                popstud.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void fpMsg_OnButtonCommand(object sender, EventArgs e)
    {

        try
        {
            Divv1.Visible = false;
            Divv2.Visible = false;
            btnsend.Visible = false;
            fpMsg.SaveChanges();
            int Arow = fpMsg.Sheets[0].ActiveRow;
            int Acol = fpMsg.Sheets[0].ActiveColumn;

            //string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            //string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (Arow > -1 && Acol > -1)
            {
                //Arow = Convert.ToInt32(actrow);
                //Acol = Convert.ToInt32(actcol);
                if (Arow == 0 && Acol == 2)
                {
                    FpSpread3.Visible = false;
                    Panel4.Visible = false;
                    string value = Convert.ToString(fpMsg.Sheets[0].Cells[0, 2].Value);
                    if (value == "1")
                    {
                        for (int i = 0; i < fpMsg.Sheets[0].Rows.Count; i++)
                        {
                            fpMsg.Sheets[0].Cells[i, 2].Value = 1;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < fpMsg.Sheets[0].Rows.Count; i++)
                        {
                            fpMsg.Sheets[0].Cells[i, 2].Value = 0;
                        }
                    }

                }
                else if (Acol == 3)
                {
                    FpSpread3.Visible = false;
                    Panel4.Visible = false;
                    string groupcode = Convert.ToString(fpMsg.Sheets[0].Cells[Arow, 1].Tag);
                    string selQ = " select distinct sm.staff_code,staff_name,sam.per_mobileno,st.stftype,sam.email from staffmaster sm,staff_appl_master sam,stafftrans st where  sm.appl_no = sam.appl_no and st.staff_code=sm.staff_code and sms_groupCode in('" + groupcode + "')";
                    DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                    if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                    {
                        FpSpread3.Sheets[0].RowCount = 0;

                        FpSpread3.Sheets[0].ColumnCount = 0;
                        FpSpread3.CommandBar.Visible = false;
                        FpSpread3.Sheets[0].AutoPostBack = false;
                        FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread3.Sheets[0].RowHeader.Visible = false;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FpSpread3.Sheets[0].ColumnCount = 6;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[0].Width = 50;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[1].Width = 150;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[2].Width = 200;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Email";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[3].Width = 200;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Mobile No";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[4].Width = 200;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Mobile No";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[5].Width = 200;

                        //=========Added by saranya on 4/9/2018=======//
                        string StaffCode = "";
                        string Date = Convert.ToString(Txtdate.Text);
                        string[] strDate = Date.Split('/');

                        string CkDate = strDate[0];
                        //For Date
                        CkDate = CkDate.StartsWith("0") ? CkDate.Substring(1) : CkDate;

                        CkDate = "[" + CkDate + "]";

                        //For Month
                        string Mnt = strDate[1];
                        Mnt = Mnt.StartsWith("0") ? Mnt.Substring(1) : Mnt;
                        string Year = strDate[2];
                        string MonthYear = Mnt + "/" + Year;

                        //============================================//

                        for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                        {
                            // to_mail = Convert.ToString(dsVal.Tables[0].Rows[row]["email"]);
                            FpSpread3.Sheets[0].RowCount++;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread3.Sheets[0].RowCount);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["staff_code"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["staff_name"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["stftype"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["email"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["per_mobileno"]);


                            //=========Added by saranya on 3/9/2018=======//

                            StaffCode = Convert.ToString(dsVal.Tables[0].Rows[row]["staff_code"]);
                            string Attendance = d2.GetFunction(" select " + CkDate + " from staff_attnd where mon_year in('" + MonthYear + "') and staff_code='" + StaffCode + "' ");
                            if (!string.IsNullOrEmpty(Attendance) && Attendance != "0")
                            {
                                string[] attnValue = Attendance.Split('-');
                                //string TreatAsPorAMrg = d2.GetFunction("select status from leave_category where shortname='" + attnValue[0] + "' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'");
                                //string TreatAsPorAEvg = d2.GetFunction("select status from leave_category where shortname='" + attnValue[1] + "' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'");
                                //if ((TreatAsPorAMrg == "0" && TreatAsPorAEvg == "0") || (TreatAsPorAMrg == "2" && TreatAsPorAEvg == "2") || (attnValue[0] == "P" && attnValue[1] == "P") || (attnValue[0] == "P" && attnValue[1] == "PER") || (attnValue[0] == "PER" && attnValue[1] == "P"))
                                if ((attnValue[0] == "P" && attnValue[1] == "P") || (attnValue[0] == "P" && attnValue[1] == "PER") || (attnValue[0] == "PER" && attnValue[1] == "P"))
                                {
                                    FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].BackColor = Color.Green;
                                }
                                else if ((attnValue[0] != "" && attnValue[1] != ""))// || (TreatAsPorAMrg == "1" && TreatAsPorAEvg == "1")
                                {
                                    FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].BackColor = Color.Red;
                                }
                            }
                            //============================================//
                        }
                        FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                        int height = FpSpread3.Sheets[0].RowCount * 28 + 50;
                        FpSpread3.Height = (height < 400) ? height + 80 : height;
                        FpSpread3.Width = 900;
                        FpSpread3.SaveChanges();
                        FpSpread3.Visible = true;
                        Panel4.Visible = true;
                        //  popstud.Visible = true;
                    }
                }
            }
        }
        catch { }
    }

    protected void btnupload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.FileName != "")
            {
                String filePath = Server.MapPath(FileUpload1.FileName);
                // FileUpload1.SaveAs(filePath);
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                if (fileExtension == ".wav")
                {
                    int fileSize = FileUpload1.PostedFile.ContentLength;
                    byte[] documentBinary = new byte[0];
                    //  FileUpload1.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    DateTime date = Convert.ToDateTime(DateTime.Today);
                    string newdate = date.ToString("MM/dd/yyyy");
                    TimeSpan time = DateTime.Now.TimeOfDay;
                    DateTime datetime = Convert.ToDateTime(newdate) + time;
                    string insertquery = "insert into uploadvoices values('" + fileName + "','" + fileExtension + "','" + datetime + "'," + Session["collegecode"].ToString() + ")";
                    //SqlParameter DocName = new SqlParameter("@DocName1", SqlDbType.VarChar, 50);
                    //DocName.Value = fileName.ToString();
                    //DocName = new SqlParameter("@DocData1", SqlDbType.Binary, fileSize);
                    //DocName.Value = documentBinary;
                    //string name = "Uploadvoice_file";
                    //hat.Add("@filename", fileName);
                    //hat.Add("@filetype", fileExtension);
                    //hat.Add("@filedate", newdate);
                    //hat.Add("@filevalue", documentBinary);
                    //hat.Add("@collegecode", Session["collegecode"].ToString());
                    int a = d2.insert_method(insertquery, hat, "Text");
                    bindspreadvoice();
                    // insertmethod();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Your Voice File Saved Successfully')", true);
                    lblerrorvoice.Visible = false;
                    Button1.Focus();
                }
                else
                {
                    lblerrorvoice.Visible = true;
                    lblerrorvoice.Text = "Please Select Any One File";
                    Button1.Focus();
                }
            }
            else
            {
                lblerrorvoice.Visible = true;
                lblerrorvoice.Text = "Please Browse Any One File";
                Button1.Focus();
            }
        }
        catch
        {
        }
    }

    //protected void btnvoicesave_Click(object sender, EventArgs e) // modify by jairam 09-09-2014

    protected void sendvoicemsg()
    {
        try
        {
            //string collegequery = "";
            //string collegename = "";
            //string collacronym = "";
            //collegequery = "select Coll_acronymn,collname from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            //ds = d2.select_method_wo_parameter(collegequery, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    collegename = ds.Tables[0].Rows[0]["collname"].ToString();
            //    collacronym = ds.Tables[0].Rows[0]["Coll_acronymn"].ToString();
            //}
            bool flag = false;
            bool sendflag = false;
            string filename = "";
            for (int k = 0; k < Fpspreadvoice.Sheets[0].RowCount; k++)
            {
                int value = Convert.ToInt32(Fpspreadvoice.Sheets[0].Cells[k, 1].Value);
                if (value == 1)
                {
                    filename = Convert.ToString(Fpspreadvoice.Sheets[0].GetText(k, 2));
                    flag = true;
                    lblerrorvoice.Visible = false;
                }
            }
            //string[] split = filename.Split('.');
            //biz.lbinfotech.www.common_msg h1 = new biz.lbinfotech.www.common_msg();
            if (flag == true)
            {
                if (rdbtnstudent.Checked == true)
                {
                    for (int j = 1; j < FpSpread1.Sheets[0].RowCount; j++)
                    {
                        bool value1 = Convert.ToBoolean(FpSpread1.Sheets[0].Cells[j, FpSpread1.Sheets[0].ColumnCount - 1].Value);
                        if (value1 == true)
                        {
                            string rollno = FpSpread1.Sheets[0].Cells[j, 1].Text;
                            string name = FpSpread1.Sheets[0].Cells[j, 3].Text;
                            string orginalname = "";
                            string student_name = name;
                            if (student_name.Contains(".") == true)
                            {
                                string[] splitname = student_name.Split('.');
                                for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                                {
                                    string lengthname = splitname[i].ToString();
                                    if (lengthname.Trim().Length > 2)
                                    {
                                        orginalname = splitname[i].ToString();
                                    }
                                }
                            }
                            else
                            {
                                string[] split2ndname = student_name.Split(' ');
                                if (split2ndname.Length > 0)
                                {
                                    for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                                    {
                                        string firstname = split2ndname[k].ToString();
                                        if (firstname.Trim().Length > 2)
                                        {
                                            if (orginalname == "")
                                            {
                                                orginalname = firstname.ToString();
                                            }
                                            else
                                            {
                                                orginalname = orginalname + " " + firstname.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            string mobileno = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 5].Text);
                            string fathermobile = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 7].Text);
                            string mothermobile = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 9].Text);
                            string gender = "";
                            string query = "Select sex from applyn where app_formno ='" + rollno + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(query, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0]["sex"].ToString() == "0")
                                {
                                    gender = "Male";
                                }
                                if (ds.Tables[0].Rows[0]["Sex"].ToString() == "1")
                                {
                                    gender = "Female";
                                }
                            }
                            if (chkstudent.Checked == true)
                            {
                                string result = voicecall(mobileno, filename);
                                if (result.Trim().ToUpper() != "ERROR")
                                {
                                    //string NEW1 = h1.get_common("" + mobileno + "", "Common", "" + split[0].ToString() + "", "" + collegename + "", "" + name + "", "" + gender + "", "2014", "BE-Computerscience", "" + rollno + "", "2014-07-21", "Holiday", "English");
                                    sendflag = true;
                                }
                            }
                            if (chkfather.Checked == true)
                            {
                                string result = voicecall(fathermobile, filename);
                                if (result.Trim().ToUpper() != "ERROR")
                                {
                                    //string NEW1 = h1.get_common("" + mobileno + "", "Common", "" + split[0].ToString() + "", "" + collegename + "", "" + name + "", "" + gender + "", "2014", "BE-Computerscience", "" + rollno + "", "2014-07-21", "Holiday", "English");
                                    sendflag = true;
                                }
                            }
                            if (chkmother.Checked == true)
                            {
                                string result = voicecall(mothermobile, filename);
                                if (result.Trim().ToUpper() != "ERROR")
                                {
                                    //string NEW1 = h1.get_common("" + mobileno + "", "Common", "" + split[0].ToString() + "", "" + collegename + "", "" + name + "", "" + gender + "", "2014", "BE-Computerscience", "" + rollno + "", "2014-07-21", "Holiday", "English");
                                    sendflag = true;
                                }
                            }
                            lblerrorvoice.Visible = false;
                        }
                    }
                }
                else if (rdbtnstaff.Checked == true)
                {
                    if (!chksmsgroup.Checked)
                    {
                        for (int j = 1; j < FpSpread1.Sheets[0].RowCount; j++)
                        {
                            bool value1 = Convert.ToBoolean(FpSpread1.Sheets[0].Cells[j, FpSpread1.Sheets[0].ColumnCount - 1].Value);
                            if (value1 == true)
                            {
                                string staff_code = FpSpread1.Sheets[0].Cells[j, 1].Text;
                                string name = FpSpread1.Sheets[0].Cells[j, 2].Text;
                                string mobileno = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 4].Text);
                                string result = voicecall(mobileno, filename);
                                //string NEW1 = h1.get_common("" + mobileno + "", "Common", "" + split[0].ToString() + "", "" + collegename + "", "" + name + "", "MALE", "2014", "BE-Computerscience", "" + staff_code + "", "2014-07-21", "Staff", "English");
                                if (result.Trim().ToUpper() != "ERROR")
                                {
                                    //string NEW1 = h1.get_common("" + mobileno + "", "Common", "" + split[0].ToString() + "", "" + collegename + "", "" + name + "", "" + gender + "", "2014", "BE-Computerscience", "" + rollno + "", "2014-07-21", "Holiday", "English");
                                    sendflag = true;
                                }
                            }
                        }
                    }
                    else//added by abarna 27.09.2017
                    {
                        for (int i = 1; i < fpMsg.Sheets[0].RowCount; i++)
                        {
                            int isval = Convert.ToInt32(fpMsg.Sheets[0].Cells[i, 2].Value);
                            if (isval == 1)
                            {
                                strstuname = Convert.ToString(fpMsg.Sheets[0].Cells[i, 1].Tag);
                                string selQ = " select distinct staff_code,sam.per_mobileno,sam.email from staffmaster sm,staff_appl_master sam where  sm.appl_no = sam.appl_no and sms_groupCode in('" + strstuname + "')";
                                DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                                if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                                {
                                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                                    {
                                        to_mail = Convert.ToString(dsVal.Tables[0].Rows[row]["email"]);
                                        string mobileno = Convert.ToString(dsVal.Tables[0].Rows[row]["per_mobileno"]);
                                        string result = "";
                                        //  string result = voicecall(mobileno, filename);
                                        if (result.Trim().ToUpper() != "ERROR")
                                        {
                                            //string NEW1 = h1.get_common("" + mobileno + "", "Common", "" + split[0].ToString() + "", "" + collegename + "", "" + name + "", "" + gender + "", "2014", "BE-Computerscience", "" + rollno + "", "2014-07-21", "Holiday", "English");
                                            sendflag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //else
            //{
            //    lblerrorvoice.Visible = true;
            //    lblerrorvoice.Text = "Please Select Any One Record";
            //}
            if (sendflag == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Voice Call Sent Successfully')", true);
            }
            //Button1.Focus();
        }
        catch
        {
        }
    }

    //magesh

    protected void btnsend_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkboxsms.Checked == true)
            {
                if (txtmessage.Text != "")
                {
                    sendsms();
                }
                else
                {
                    lblsendmail.Text = "txtmessage is empty";
                }
            }
            else
            {
                txtmessage.Text = "";
            }
            if (chkboxmail.Checked == true)
            {
                if (txtbody.Text != "")
                {
                    emailsend();
                }
                else
                {
                    lblsendmail.Text = "txtbody is empty";
                }
            }
            else
            {
                txtbody.Text = "";
            }
            if (chknotification.Checked == true)
            {
                if (txtnotification.Text != "")
                {
                    notificationsend();
                }
                else
                {
                    lblsendmail.Text = "txtnotification is empty";
                }
            }
            else
            {
                txtnotification.Text = "";
            }
            if (chkvoicecall.Checked == true)
            {
                if (FileUpload1.FileName != "")
                {
                    sendvoicemsg();
                }
                else
                {
                    lblerrorvoice.Text = "upload voice file ";
                }
            }

        }
        catch
        {
        }
    }

    public string voicecall(string mobile, string filename)
    {
        string api_key = "OJwvc0CgvvVGE0i3w0Aw";
        string access_key = "jZEl8Dmc35XxcQL8N8l3uz9f3kZiyTolCQYQHqOi";
        string Mobile_Number = "91" + mobile + "";
        string request = "";
        string error = "";
        string stringpost = null;
        // request = "<request action="http://smscountry.com/testdr.aspx"method="GET"><to>" + Mobile_Number + "</to><play>http://smscountry.com/voice_clips/4031001209_130806161411.wav</play></request>";
        // request = "<request action= http://smscountry.com/testdr.aspx method=GET><to>" + Mobile_Number + "</to><play> http://smscountry.com/voice_clips/4031001209_130806161411.wav </play></request>";
        //request ="<request action=
        string value1 = "http://www.palpap.com/voice/";
        string value2 = filename;
        string value3 = value1 + "" + value2;
        request = "<request action='http://smscountry.com/testdr.aspx' method='GET'>";
        request = request + "<to>" + Mobile_Number + "</to>";
        //request = request + "<play>http://www.palpap.com/voice/Attendance.wav</play>";
        request = request + "<play>" + value3 + "</play>";
        //request = request + "<play>https://www.sendspace.com/file/ynlrbj</play>";
        request = request + "</request>";
        stringpost = "api_key=" + api_key + "&access_key=" + access_key + "&xml=" + request;
        HttpWebRequest objWebRequest = null;
        HttpWebResponse objWebResponse = null;
        StreamWriter objStreamWriter = null;
        StreamReader objStreamReader = null;
        try
        {
            string stringResult = null;
            objWebRequest = (HttpWebRequest)WebRequest.Create("http://voiceapi.smscountry.com/api ");
            objWebRequest.Method = "POST";
            objWebRequest.ContentType = "application/x-www-form-urlencoded";
            objStreamWriter = new StreamWriter(objWebRequest.GetRequestStream());
            objStreamWriter.Write(stringpost);
            objStreamWriter.Flush();
            objStreamWriter.Close();
            objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();
            objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
            stringResult = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            // Response.Write(stringResult);
            JObject JObj = new JObject();
            string ErrorReason = "";
            string CallUUId = "";
            JObj = JObject.Parse(stringResult);
            string result = Convert.ToString(stringResult);
            string[] firstsplit = result.Split(':');
            if (firstsplit.Length > 0)
            {
                string[] secondsplit = firstsplit[2].ToString().Split('"');
                if (secondsplit.Length > 0)
                {
                    error = secondsplit[1].ToString();
                    if (error.Trim().ToUpper() == "ERROR")
                    {
                        secondsplit = firstsplit[3].ToString().Split('"');
                        if (secondsplit.Length > 0)
                        {
                            error = secondsplit[1].ToString();
                        }
                    }
                }
            }
            //if (JObj.SelectToken("event").ToString().ToUpper() == "ERROR")
            //{
            //    ErrorReason = JObj.SelectToken("error_reason").ToString();
            //}
            //else
            //{
            //    CallUUId = JObj.SelectToken("calluid").ToString();
            //}
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        finally
        {
            if ((objStreamWriter != null))
            {
                objStreamWriter.Close();
            }
            if ((objStreamReader != null))
            {
                objStreamReader.Close();
            }
            objWebRequest = null;
            objWebResponse = null;
            objProxy1 = null;
        }
        return error;
    }

    public void bindspreadvoice()
    {
        try
        {
            // FpSpread1.Sheets[0].SheetCorner.Rows[0].Visible = false;
            FarPoint.Web.Spread.StyleInfo mystyle = new FarPoint.Web.Spread.StyleInfo();
            mystyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            mystyle.ForeColor = Color.Black;
            mystyle.Font.Size = FontUnit.Medium;
            mystyle.Font.Bold = true;
            mystyle.Font.Name = "Book Antiqua";
            Fpspreadvoice.Sheets[0].ColumnHeader.DefaultStyle = mystyle;
            Fpspreadvoice.Sheets[0].AutoPostBack = false;
            Fpspreadvoice.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspreadvoice.Sheets[0].Columns.Count = 4;
            Fpspreadvoice.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspreadvoice.Sheets[0].Columns[0].Locked = true;
            Fpspreadvoice.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspreadvoice.Sheets[0].ColumnHeader.Cells[0, 2].Text = "File Name";
            Fpspreadvoice.Sheets[0].Columns[2].Locked = true;
            Fpspreadvoice.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Upload Date";
            Fpspreadvoice.Sheets[0].Columns[3].Locked = true;
            //FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            //cb.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
            cb1.AutoPostBack = true;
            Fpspreadvoice.Sheets[0].RowCount = 1;
            int height = 100;
            string voicequery = "";
            voicequery = "Select voicefilename, filedate from uploadvoices where collegecode=" + Session["collegecode"].ToString() + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(voicequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                //Fpspreadvoice.Sheets[0].RowCount++;
                //Fpspreadvoice.Sheets[0].SpanModel.Add(0, 2, 1, 2);
                //Fpspreadvoice.Sheets[0].Cells[0, 1].CellType = cb;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 1].CellType = cb1;
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["voicefilename"].ToString();
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    // Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    DateTime date = Convert.ToDateTime(ds.Tables[0].Rows[i]["filedate"].ToString());
                    string newdate = date.ToString();
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 3].Text = newdate.ToString();
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspreadvoice.Sheets[0].RowCount++;
                    height = height + 50;
                    // Fpspreadvoice.Sheets[0].Cells[Fpspreadvoice.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                }
                Fpspreadvoice.Sheets[0].PageSize = Fpspreadvoice.Sheets[0].RowCount;
                Fpspreadvoice.Visible = true;
                Fpspreadvoice.Sheets[0].Visible = true;
                Fpspreadvoice.Height = height;
                Fpspreadvoice.Width = 500;
                Fpspreadvoice.Sheets[0].RowHeader.Visible = false;
                lblerrorvoice.Visible = false;
            }
            else
            {
                Fpspreadvoice.Height = height;
                Fpspreadvoice.Width = 500;
                Fpspreadvoice.Sheets[0].Cells[0, 0].Text = Convert.ToString("1");
                Fpspreadvoice.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadvoice.Sheets[0].Cells[0, 1].CellType = cb1;
                Fpspreadvoice.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadvoice.Sheets[0].Cells[0, 2].Text = "";
                Fpspreadvoice.Sheets[0].Cells[0, 3].Text = "";
            }
        }
        catch
        {
        }
    }

    //public void insertmethod()
    //{
    //    try
    //    {
    //        bool upload = false;
    //        String filePath = Server.MapPath("~/UploadFiles/" + FileUpload1.FileName);
    //        FileUpload1.SaveAs(filePath);
    //        FileInfo fileInf = new FileInfo(filePath);
    //        //  string uri = "ftp://" + "203.109.109.29" + "/" + fileInf.Name; //ftp://192.168.1.99/New Stories (Highway Blues).wma
    //        FtpWebRequest reqFTP;
    //        // Create FtpWebRequest object from the Uri provided
    //        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + "203.109.109.29" + "/" + fileInf.Name));
    //        // Provide the WebPermission Credintials
    //        reqFTP.Credentials = new NetworkCredential("LBITIND", "vodafone");
    //        // By default KeepAlive is true, where the control connection is not closed
    //        // after a command is executed.
    //        reqFTP.KeepAlive = false;
    //        // Specify the command to be executed.
    //        reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
    //        // Specify the data transfer type.
    //        reqFTP.UseBinary = true;
    //        // Notify the server about the size of the uploaded file
    //        reqFTP.ContentLength = fileInf.Length; //size
    //        // The buffer size is set to 2kb
    //        int buffLength = 2048;
    //        byte[] buff = new byte[buffLength];
    //        int contentLen;
    //        // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
    //        FileStream fs = fileInf.OpenRead();
    //        try
    //        {
    //            // Stream to which the file to be upload is written
    //            Stream strm = reqFTP.GetRequestStream();
    //            // Read from the file stream 2kb at a time
    //            contentLen = fs.Read(buff, 0, buffLength); //ftp://192.168.1.99/New%20Stories%20(Highway%20Blues).wma
    //            // Till Stream content ends
    //            while (contentLen != 0)
    //            {
    //                // Write Content from the file stream to the FTP Upload Stream
    //                strm.Write(buff, 0, contentLen);
    //                contentLen = fs.Read(buff, 0, buffLength);
    //                upload = true;
    //                lblerrorvoice.Visible = false;
    //            }
    //            // Close the file stream and the Request Stream
    //            strm.Close();
    //            fs.Close();
    //            if (upload == true)
    //            {
    //                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Uploaded Successfully')", true);
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            int a = 0;
            bool value = false;
            for (int j = 0; j < Fpspreadvoice.Sheets[0].RowCount; j++)
            {
                int val = Convert.ToInt32(Fpspreadvoice.Sheets[0].Cells[j, 1].Value);
                if (val == 1)
                {
                    string filename = Convert.ToString(Fpspreadvoice.Sheets[0].Cells[j, 2].Text);
                    string deletequery = "";
                    deletequery = "delete uploadvoices where voicefilename ='" + filename.ToString() + "' and collegecode=" + Session["collegecode"] + "";
                    a = d2.update_method_wo_parameter(deletequery, "Text");
                    value = true;
                }
            }
            if (a != 0)
            {
                bindspreadvoice();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            }
            if (value == false)
            {
                lblerrorvoice.Visible = true;
                lblerrorvoice.Text = "Please Select Any One Record";
            }
            Button1.Focus();
        }
        catch
        {
        }
    }

    protected void chkvoicecall_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkvoicecall.Checked == true)
            {
                if (btnstaffgo.Visible == true)
                {
                    btnstaffgo_Click(sender, e);
                }
                if (btngo.Visible == true)
                {
                    btngo_Click(sender, e);

                }
                Div5.Visible = true;
                btnsend.Visible = true;
            }
            else
            {
                Div5.Visible = false;
                btnsend.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void Fpspreadvoice_Updatecommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Button1.Focus();
        }
        catch
        {
        }
    }

    protected void emailsend()
    {
        if (chkboxmail.Checked == true)
        {
            try
            {
                #region Copy Of EMAIL ADDED BY MALANG RAJA
                bool isSendCopyEmail = false;
                string[] copyEmailList = new string[1];
                string copyeamilid = "";
                copyeamilid = d2.GetFunctionv("select value from Master_Settings where settings='Copy of Email'");
                if (copyeamilid.Trim().Trim(',') != "")
                {
                    copyEmailList = copyeamilid.Split(',');
                    isSendCopyEmail = true;
                }
                #endregion Copy Of EMAIL ADDED BY MALANG RAJA
                if (rdbtnstudent.Checked == true)
                {
                    strmsg = txtbody.Text;
                    string strquery = "select massemail,masspwd from collinfo where college_code ='" + ddlcollege.SelectedValue.ToString() + "' ";
                    ds1.Dispose();
                    ds1.Reset();
                    ds1 = d2.select_method(strquery, hat, "Text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                        send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                        //send_mail = "palpaporange@gmail.com";
                        //send_pw = "palpap1234";
                    }
                    #region Added By Malang Raja on Oct 18 2016
                    else
                    {
                        //Modified by saranya on 20/9/2018
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                        //lblsendmail.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                        //lblsendmail.Visible = true;
                        return;
                    }
                    #endregion Added By Malang Raja on Oct 18 2016

                    //Changed by saranya on 17/10/2018
                    string attachfile;
                    string attchefileexten = "", attachfiletype = "";
                    Boolean atchflag = false;
                    string filename = "";
                    string File1 = "";
                    if (FileUpload2.HasFile)
                    {
                        if (FileUpload2.FileName.EndsWith(".txt") || FileUpload2.FileName.EndsWith(".pdf") || FileUpload2.FileName.EndsWith(".doc") || FileUpload2.FileName.EndsWith(".xls") || FileUpload2.FileName.EndsWith(".xlsx") || FileUpload2.FileName.EndsWith(".docx") || FileUpload2.FileName.EndsWith(".jpg"))
                        {
                            atchflag = true;
                            attachfile = Path.GetFileName(FileUpload2.PostedFile.FileName);
                            if (attachfile.Trim() != "")
                            {
                                string path = Server.MapPath("~\\Report\\" + System.IO.Path.GetFileName(FileUpload2.FileName));
                                FileUpload2.SaveAs(path);
                            }
                        }
                    }

                    for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 11].Value);
                        if (isval == 1)
                        {
                            for (int stuorpart = 0; stuorpart < 3; stuorpart++)
                            {
                                //Modified by srinath 18/12/2013
                                to_mail = "";
                                if (stuorpart == 0)
                                {
                                    to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Note);
                                }
                                if (stuorpart == 1)
                                {
                                    to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 8].Note);
                                }
                                if (stuorpart == 2)
                                {
                                    to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Note);
                                }
                                if (to_mail.Trim() != "" && to_mail != null)
                                {
                                    strstuname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Note);
                                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                    Mail.EnableSsl = true;
                                    MailMessage mailmsg = new MailMessage();
                                    MailAddress mfrom = new MailAddress(send_mail);

                                    mailmsg.From = mfrom;
                                    mailmsg.To.Add(to_mail);
                                    mailmsg.Subject = txtsub.Text.ToString();
                                    //magesh
                                    mailmsg.IsBodyHtml = false;
                                    //mailmsg.Body = "First line" + Environment.NewLine + "Second line";
                                    //mailmsg.Body = "First line <br /> Second line";
                                    // mailmsg.Body = "Dear";
                                    // mailmsg.Body = mailmsg.Body + strstuname ;
                                    mailmsg.Body = strstuname;
                                    mailmsg.Body = mailmsg.Body + "\n\n" + strmsg;
                                    mailmsg.Body = mailmsg.Body + "\n\n" + "Thank You..";//magesh
                                    // mailmsg.Body = mailmsg.Body + "<br/><br/>Thank You...<br/><br/>";
                                    byte[] documentBinary = new byte[0];
                                    byte[] attchementfile = new byte[0];
                                    string filenameMail = "";

                                    //Modified on 17/10/2018
                                    if (FileUpload2.HasFile)
                                    {
                                        string appPath = HttpContext.Current.Server.MapPath("~");
                                        if (appPath != "")
                                        {
                                            string szPath = appPath + "\\Report\\";
                                            File1 = szPath + Path.GetFileName(FileUpload2.PostedFile.FileName);
                                            filenameMail = File1;
                                            string[] path = File1.Split('\\');
                                            File1 = path[path.Length - 1].ToString();
                                        }
                                    }
                                    System.Net.Mail.Attachment attachment;
                                    attachment = new System.Net.Mail.Attachment(filenameMail);
                                    mailmsg.Attachments.Add(attachment);
                                    //====================//
                                    Mail.EnableSsl = true;
                                    Mail.UseDefaultCredentials = false;
                                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                    Mail.Credentials = credentials;
                                    Mail.Send(mailmsg);
                                    flagstudent = true;
                                }
                            }
                            // lblsendmail.Text = "The Selected Students mail has been sent";
                            // lblsendmail.Visible = true;
                        }
                    }
                    if (flagstudent == true)
                    {
                        filedeleted(File1);
                    }
                    if (chkboxmail.Checked == true)
                    {
                        if (flagstudent == true)
                        {
                            #region Send EmailCopy
                            if (isSendCopyEmail)
                            {
                                SendEmailCopywithAttachment(copyEmailList, send_mail, send_pw, Convert.ToString(txtsub.Text), strmsg, FileUpload2);
                            }
                            #endregion Send EmailCopy
                            //Modified by saranya on 20/9/2018
                            imgAlert.Visible = true;
                            lbl_alert.Text = "The Selected Students Mail has been sent";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Students Mail has been sent')", true);
                        }
                        else
                        {
                            //Modified by saranya on 20/9/2018
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Please Select Any One Detail";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any One Detail')", true);
                        }
                    }
                }
                else if (rdbtnstaff.Checked == true)
                {
                    strmsg = txtbody.Text;
                    string strquery = "select massemail,masspwd from collinfo where college_code = " + ddlcollege.SelectedValue.ToString() + " ";
                    ds1.Dispose();
                    ds1.Reset();
                    ds1 = d2.select_method(strquery, hat, "Text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                        send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                        //send_mail = "palpaporange@gmail.com";
                        //send_pw = "palpap1234";

                    }
                    #region Added By Malang Raja on Oct 18 2016
                    else
                    {
                        //Modified by saranya on 20/9/2018
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                        //lblsendmail.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                        //lblsendmail.Visible = true;
                        return;
                    }
                    #endregion Added By Malang Raja on Oct 18 2016
                    // magesh 12/2/18
                    if (chksmsgroup.Checked == false)
                    {
                        string attachfile;
                        string attchefileexten = "", attachfiletype = "";
                        Boolean atchflag = false;
                        string filename = "";
                        string File1 = "";
                        //Changed by saranya on 17/10/2018
                        if (FileUpload2.HasFile)
                        {
                            if (FileUpload2.FileName.EndsWith(".txt") || FileUpload2.FileName.EndsWith(".pdf") || FileUpload2.FileName.EndsWith(".doc") || FileUpload2.FileName.EndsWith(".xls") || FileUpload2.FileName.EndsWith(".xlsx") || FileUpload2.FileName.EndsWith(".docx") || FileUpload2.FileName.EndsWith(".jpg"))
                            {
                                atchflag = true;
                                attachfile = Path.GetFileName(FileUpload2.PostedFile.FileName);
                                if (attachfile.Trim() != "")
                                {
                                    string path = Server.MapPath("~\\Report\\" + System.IO.Path.GetFileName(FileUpload2.FileName));
                                    FileUpload2.SaveAs(path);
                                }
                            }
                        }
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 6].Value);
                            if (isval == 1)
                            {
                                strstuname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Note);
                                to_mail = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Note);
                                // to_mail = "karthikeyanmurugesan08@gmail.com";
                                MailMessage mailmsg = new MailMessage();
                                MailAddress mfrom = new MailAddress(send_mail);
                                mailmsg.From = mfrom;
                                mailmsg.To.Add(to_mail);
                                mailmsg.Subject = txtsub.Text.ToString();
                                //magesh
                                mailmsg.IsBodyHtml = false;
                                // mailmsg.Body = "Dear";
                                //mailmsg.Body = mailmsg.Body + strstuname;
                                mailmsg.Body = strstuname;
                                mailmsg.Body = mailmsg.Body + "\n\n" + strmsg;
                                mailmsg.Body = mailmsg.Body + "\n\n" + "Thank You...";//magesh
                                byte[] documentBinary = new byte[0];
                                byte[] attchementfile = new byte[0];
                                //Modified by saranya on 17/10/2018
                                string filenameMail = "";
                                if (FileUpload2.HasFile)
                                {
                                    string appPath = HttpContext.Current.Server.MapPath("~");
                                    if (appPath != "")
                                    {
                                        string szPath = appPath + "\\Report\\";
                                        File1 = szPath + Path.GetFileName(FileUpload2.PostedFile.FileName);
                                        filenameMail = File1;
                                        string[] path = File1.Split('\\');
                                        File1 = path[path.Length - 1].ToString();
                                    }
                                }
                                System.Net.Mail.Attachment attachment;
                                attachment = new System.Net.Mail.Attachment(filenameMail);
                                mailmsg.Attachments.Add(attachment);
                                //=============================//
                                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                Mail.EnableSsl = true;
                                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                Mail.UseDefaultCredentials = false;
                                Mail.Credentials = credentials;
                                Mail.Send(mailmsg);
                                flagstudent = true;
                                //  lblsendmail.Text = "The Selected Staff mail has been sent";
                                //  lblsendmail.Visible = true;
                            }
                        }
                        if (flagstudent == true)
                        {
                            filedeleted(File1);
                        }

                    }
                    //magesh 12/2/18
                    else
                    {
                        string attachfile;
                        string attchefileexten = "", attachfiletype = "";
                        Boolean atchflag = false;
                        string filename = "";
                        string File1 = "";
                        //Changed by saranya on 17/10/2018
                        if (FileUpload2.HasFile)
                        {
                            if (FileUpload2.FileName.EndsWith(".txt") || FileUpload2.FileName.EndsWith(".pdf") || FileUpload2.FileName.EndsWith(".doc") || FileUpload2.FileName.EndsWith(".xls") || FileUpload2.FileName.EndsWith(".xlsx") || FileUpload2.FileName.EndsWith(".docx") || FileUpload2.FileName.EndsWith(".jpg"))
                            {
                                atchflag = true;
                                attachfile = Path.GetFileName(FileUpload2.PostedFile.FileName);
                                if (attachfile.Trim() != "")
                                {
                                    string path = Server.MapPath("~\\Report\\" + System.IO.Path.GetFileName(FileUpload2.FileName));
                                    FileUpload2.SaveAs(path);
                                }
                            }
                        }
                        for (int i = 1; i < fpMsg.Sheets[0].RowCount; i++)
                        {
                            int isval = Convert.ToInt32(fpMsg.Sheets[0].Cells[i, 2].Value);
                            if (isval == 1)
                            {
                                strstuname = Convert.ToString(fpMsg.Sheets[0].Cells[i, 1].Tag);
                                //magesh 2.3.18
                                //string selQ = " select distinct staff_code,sam.per_mobileno,sam.email from staffmaster sm,staff_appl_master sam where  sm.appl_no = sam.appl_no and sms_groupCode in('" + strstuname + "')";
                                string selQ = " select distinct staff_code,staff_name,sam.per_mobileno,sam.email from staffmaster sm,staff_appl_master sam where  sm.appl_no = sam.appl_no and sms_groupCode in('" + strstuname + "')";//magesh 2.3.18
                                DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                                if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                                {
                                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                                    {
                                        //for (i = 0; i < FpSpread3.Sheets[0].RowCount; i++)
                                        //{
                                        //magesh 2.3.18
                                        strstuname = Convert.ToString(dsVal.Tables[0].Rows[row]["staff_name"]).Trim();//magesh 2.3.18
                                        to_mail = Convert.ToString(dsVal.Tables[0].Rows[row]["email"]).Trim();
                                        //strstuname = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 8].Text);
                                        //to_mail = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 4].Text);
                                        // to_mail = "karthikeyanmurugesan08@gmail.com";
                                        MailMessage mailmsg = new MailMessage();
                                        MailAddress mfrom = new MailAddress(send_mail);
                                        mailmsg.From = mfrom;
                                        mailmsg.To.Add(to_mail);
                                        mailmsg.Subject = txtsub.Text.ToString();
                                        //magesh
                                        mailmsg.IsBodyHtml = false;
                                        // mailmsg.Body = "Dear";
                                        //mailmsg.Body = mailmsg.Body + strstuname;
                                        mailmsg.Body = strstuname;
                                        mailmsg.Body = mailmsg.Body + "\n\n" + strmsg;
                                        mailmsg.Body = mailmsg.Body + "\n\n" + "Thank You...";//magesh
                                        byte[] documentBinary = new byte[0];
                                        byte[] attchementfile = new byte[0];
                                        //Modified by saranya on 17/10/2018
                                        string filenameMail = "";
                                        if (FileUpload2.HasFile)
                                        {
                                            string appPath = HttpContext.Current.Server.MapPath("~");
                                            if (appPath != "")
                                            {
                                                string szPath = appPath + "\\Report\\";
                                                File1 = szPath + Path.GetFileName(FileUpload2.PostedFile.FileName);
                                                filenameMail = File1;
                                                string[] path = File1.Split('\\');
                                                File1 = path[path.Length - 1].ToString();

                                            }
                                        }
                                        System.Net.Mail.Attachment attachment;
                                        attachment = new System.Net.Mail.Attachment(filenameMail);
                                        mailmsg.Attachments.Add(attachment);
                                        //==============================//

                                        SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                        Mail.EnableSsl = true;
                                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                        Mail.UseDefaultCredentials = false;
                                        Mail.Credentials = credentials;
                                        Mail.Send(mailmsg);
                                        flagstudent = true;
                                        //  lblsendmail.Text = "The Selected Staff mail has been sent";
                                        //  lblsendmail.Visible = true;
                                    }
                                }
                            }
                        }
                        if (flagstudent == true)
                        {
                            filedeleted(File1);
                        }
                    }
                    if (flagstudent == true)
                    {
                        #region Send EmailCopy
                        if (isSendCopyEmail)
                        {
                            SendEmailCopywithAttachment(copyEmailList, send_mail, send_pw, Convert.ToString(txtsub.Text), strmsg, FileUpload2);
                        }
                        #endregion Send EmailCopy
                        //Modified by saranya on 20/9/2018
                        imgAlert.Visible = true;
                        lbl_alert.Text = "The Selected Staff mail has been sent";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Selected Staff mail has been sent')", true);
                    }
                    else
                    {
                        //Modified by saranya on 20/9/2018
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Select Any One Detail";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any One Detail')", true);
                    }
                }
                txtmessage.Text = "";
            }
            catch
            {
                //Modified by saranya on 20/9/2018
                imgAlert.Visible = true;
                lbl_alert.Text = "Send Email Failed.";
                //lblsendmail.Text = "Send Email Failed.";
            }
        }
    }

    //protected void lnk_upload_OnClick(object sender, EventArgs e)
    //{
    //    //try
    //    //{
    //    //    Response.Redirect("http://www.lbinfotech.biz/PALPAP/");
    //    //}
    //    //catch
    //    //{
    //    //}
    //}

    #region Added By T Malang Raja
    public void SendCopyEmail(string[] copyemailid, string send_mail, string send_pw, string strmsg)
    {
        try
        {
            for (int i = 0; i < copyemailid.Length; i++)
            {
                to_mail = copyemailid[i];
                if (to_mail.Trim() != "" && to_mail != null)
                {
                    strstuname = "Copy of Email";
                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                    MailMessage mailmsg = new MailMessage();
                    MailAddress mfrom = new MailAddress(send_mail);
                    mailmsg.From = mfrom;
                    mailmsg.To.Add(to_mail);
                    mailmsg.Subject = "Report";
                    mailmsg.IsBodyHtml = true;
                    mailmsg.Body = "Respected Sir,<br/><br/>";
                    mailmsg.Body = mailmsg.Body;
                    mailmsg.Body = mailmsg.Body + strmsg;
                    mailmsg.Body = mailmsg.Body + "<br><br>Thank You...<br/><br/>";
                    Mail.EnableSsl = true;
                    Mail.UseDefaultCredentials = false;
                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                    Mail.Credentials = credentials;
                    Mail.Send(mailmsg);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void SendEmailCopywithAttachment(string[] copyemailid, string send_mail, string send_pw, string subject, string strmsg, FileUpload FileUpload2)
    {
        try
        {
            for (int i = 0; i < copyemailid.Length; i++)
            {
                to_mail = copyemailid[i];
                if (to_mail.Trim() != "" && to_mail != null)
                {
                    MailMessage mailmsg = new MailMessage();
                    MailAddress mfrom = new MailAddress(send_mail);
                    mailmsg.From = mfrom;
                    mailmsg.To.Add(to_mail);
                    mailmsg.Subject = subject;
                    mailmsg.IsBodyHtml = true;
                    mailmsg.Body = "Respected Sir, <br/><br/> ";
                    mailmsg.Body = mailmsg.Body;
                    mailmsg.Body = mailmsg.Body + strmsg;
                    mailmsg.Body = mailmsg.Body + "<br><br>Thank You...<br/><br/>";
                    byte[] documentBinary = new byte[0];
                    byte[] attchementfile = new byte[0];
                    string attachfile;
                    string attchefileexten = "", attachfiletype = "";
                    Boolean atchflag = false;
                    string filename = "";
                    if (FileUpload2.HasFile)
                    {
                        if (FileUpload2.FileName.EndsWith(".txt") || FileUpload2.FileName.EndsWith(".pdf") || FileUpload2.FileName.EndsWith(".doc") || FileUpload2.FileName.EndsWith(".xls") || FileUpload2.FileName.EndsWith(".xlsx") || FileUpload2.FileName.EndsWith(".docx"))
                        {
                            atchflag = true;
                            attachfile = Path.GetFileName(FileUpload2.PostedFile.FileName);
                            mailmsg.Attachments.Add(new Attachment(FileUpload2.PostedFile.InputStream, attachfile));
                        }
                    }
                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                    Mail.EnableSsl = true;
                    Mail.UseDefaultCredentials = false;
                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                    Mail.Credentials = credentials;
                    Mail.Send(mailmsg);
                }
            }
        }
        catch (Exception ex)
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
    #endregion Added By T Malang Raja

    //added by sudhagar 17.07.2017
    //route and stage

    #region Route
    public void bindroute()
    {
        try
        {
            cblroute.Items.Clear();
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct Route_ID from routemaster order by Route_ID";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblroute.DataSource = ds;
                cblroute.DataTextField = "Route_ID";
                cblroute.DataValueField = "Route_ID";
                cblroute.DataBind();
                if (cblroute.Items.Count > 0)
                {
                    for (int i = 0; i < cblroute.Items.Count; i++)
                    {
                        cblroute.Items[i].Selected = true;
                    }
                    txtroute.Text = "Route ID(" + cblroute.Items.Count + ")";
                    cbroute.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cbroute_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbroute, cblroute, txtroute, "Route", "--Select--");
            //  binddept();
            bindvechileid();
            loadvechilestage();
        }
        catch { }
    }
    protected void cblroute_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbroute, cblroute, txtroute, "Route", "--Select--");
            // binddept();
            loadvechilestage();
            bindvechileid();
        }
        catch { }
    }
    #endregion

    #region vechile id
    public void bindvechileid()
    {
        try
        {
            cblvechile.Items.Clear();
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            string route = "";
            for (int i = 0; i < cblroute.Items.Count; i++)
            {
                if (cblroute.Items[i].Selected == true)
                {
                    if (route == "")
                    {
                        route = Convert.ToString(cblroute.Items[i].Value);
                    }
                    else
                    {
                        route += "','" + Convert.ToString(cblroute.Items[i].Value);
                    }
                }
            }
            ds.Clear();
            string selqry = "select distinct Veh_ID from vehicle_master where route in('" + route + "')  order by Veh_ID";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblvechile.DataSource = ds;
                cblvechile.DataTextField = "Veh_ID";
                cblvechile.DataValueField = "Veh_ID";
                cblvechile.DataBind();
                if (cblvechile.Items.Count > 0)
                {
                    for (int i = 0; i < cblvechile.Items.Count; i++)
                    {
                        cblvechile.Items[i].Selected = true;
                    }
                    txtvechile.Text = "Vechile ID(" + cblvechile.Items.Count + ")";
                    cbvechile.Checked = true;
                }
                loadvechilestage();
            }
            else
            {
                txtvechile.Text = "Select";
                cbvechile.Checked = false;
            }
        }
        catch { }
    }
    protected void cbvechile_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbvechile, cblvechile, txtvechile, "Vechile ID", "--Select--");
            //  binddept();
            loadvechilestage();
        }
        catch { }
    }
    protected void cblvechile_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbvechile, cblvechile, txtvechile, "Vechile ID", "--Select--");
            // binddept();
            loadvechilestage();
        }
        catch { }
    }
    #endregion

    #region Stage
    //public void bindstage()
    //{
    //    try
    //    {
    //        cblstage.Items.Clear();
    //        string clgvalue = ddlcollege.SelectedItem.Value.ToString();
    //        ds.Clear();
    //        string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
    //        //if (stream != "")
    //        //{
    //        //    selqry = selqry + " and type  in('" + stream + "')";
    //        //}
    //        ds = d2.select_method_wo_parameter(selqry, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cblstage.DataSource = ds;
    //            cblstage.DataTextField = "course_name";
    //            cblstage.DataValueField = "course_id";
    //            cblstage.DataBind();
    //            if (cblstage.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cblstage.Items.Count; i++)
    //                {
    //                    cblstage.Items[i].Selected = true;
    //                }
    //                txtstage.Text = "Stage(" + cblstage.Items.Count + ")";
    //                cbstage.Checked = true;
    //            }
    //        }
    //    }
    //    catch { }
    //}
    public void loadvechilestage()
    {
        string sqlquery = string.Empty;
        string filter = "";
        cblstage.Items.Clear();
        //   cblstage.Items.Insert(0, new ListItem("All", "-1"));
        string route = "";
        for (int i = 0; i < cblroute.Items.Count; i++)
        {
            if (cblroute.Items[i].Selected == true)
            {
                if (route == "")
                {
                    route = Convert.ToString(cblroute.Items[i].Value);
                }
                else
                {
                    route += "','" + Convert.ToString(cblroute.Items[i].Value);
                }
            }
        }
        string vechile = "";
        for (int i = 0; i < cblvechile.Items.Count; i++)
        {
            if (cblvechile.Items[i].Selected == true)
            {
                if (vechile == "")
                {
                    vechile = Convert.ToString(cblvechile.Items[i].Value);
                }
                else
                {
                    vechile += "','" + Convert.ToString(cblvechile.Items[i].Value);
                }
            }
        }
        if (route != "-1")
        {
            filter = " and v.Route in('" + route + "')";
        }
        if (vechile != "-1")
        {
            filter = filter + ' ' + "and r.Veh_ID in('" + vechile + "')";
        }
        sqlquery = "select distinct Stage_Name from routemaster r,vehicle_master v where Stage_Name is not null and Stage_Name<>'' and v.Veh_ID=r.Veh_ID " + filter + "";
        ds = d2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Boolean e1 = isNumeric(ds.Tables[0].Rows[i]["Stage_Name"].ToString(), System.Globalization.NumberStyles.Integer);
                if (e1)
                {
                    string Get_Stage = d2.GetFunction("select distinct Stage_Name from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    string Get_Stage_id = d2.GetFunction("select distinct Stage_id from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    cblstage.Items.Add(new ListItem(Get_Stage, Get_Stage_id));//Added By SRinath 8/10/2013
                }
                else
                {
                    cblstage.Items.Add(ds.Tables[0].Rows[i]["Stage_Name"].ToString());
                }
            }
            if (cblstage.Items.Count > 0)
            {
                for (int i = 0; i < cblstage.Items.Count; i++)
                {
                    cblstage.Items[i].Selected = true;
                }
                txtstage.Text = "Stage(" + cblstage.Items.Count + ")";
                cbstage.Checked = true;
            }
        }
        else
        {
            txtstage.Text = "Select";
            cbstage.Checked = false;
        }
        // cblstage.SelectedIndex = 0;
    }
    protected void cbstage_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
            //  binddept();
        }
        catch { }
    }
    protected void cblstage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
            // binddept();
        }
        catch { }
    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    #endregion

    #region Common Checkbox and Checkboxlist Event
    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }
    #endregion

    //added by sudhagar 19.07.2017 for vehicle type

    protected void getVehicleType()
    {
        ddlvehType.Items.Clear();
        ddlvehType.Items.Add(new ListItem("Own Vehicle", "1"));
        ddlvehType.Items.Add(new ListItem("College Vehicle", "2"));
        ddlvehType.Items.Add(new ListItem("Both", "3"));
    }

    protected void rdbtnsmsGroup_CheckedChanged(object sender, EventArgs e)
    {
        chkboxsms.Checked = false;
        chkboxmail.Checked = false;
        chknotification.Checked = false;
        chkvoicecall.Checked = false;
        Div5.Visible = false;
        Divv2.Visible = false;
        Divv1.Visible = false;
        Div7.Visible = false;
        btnsend.Visible = false;
        if (chksmsgroup.Checked == true)
        {
            staffpanel.Visible = false;
            fve.Visible = false;
            fvehicletype.Visible = false;
            Button2.Visible = true;
            FpSpread1.Visible = false;
            txtbody.Text = "";
            txtsub.Text = "";
            txtmessage.Text = "";
            txtnotification.Text = "";
        }
        else
        {
            staffpanel.Visible = true;
            fve.Visible = true;
            fvehicletype.Visible = true;
            Button2.Visible = false;
        }
    }

    //Added by saranya on 17/9/2018

    protected void RbEnglish_OnCheckedChanged(object sender, EventArgs e)
    {
        RbTamil.Checked = false;
    }

    protected void RbTamil_OnCheckedChanged(object sender, EventArgs e)
    {
        RbEnglish.Checked = false;
    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }

    //saran

    public void filedeleted(string file)
    {
        try
        {
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/" + file;
                string File1 = szPath;

                string[] filePaths = Directory.GetFiles(File1);
                foreach (string filePath in filePaths)
                    File.Delete(filePath);
            }
        }

        catch
        {

        }
    }
}