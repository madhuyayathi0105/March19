using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using System.Text;
using System.Globalization;


public partial class StudentTimeTable : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    SqlConnection con = new SqlConnection(Convert.ToString(ConfigurationManager.AppSettings["con"]));
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    Boolean cellfalsg = false;
    Boolean celldetails = false;
    string tablevalue = string.Empty;
    string userCollegeCode = string.Empty;
    Boolean allowcom = false;
    Boolean allowmuliallot = false;
    DataSet srids = new DataSet();
    DAccess2 srida = new DAccess2();
    Hashtable allotrow = new Hashtable();
    Hashtable hatHr = new Hashtable();
    Boolean cnfrm = false;


    DataTable dtable = new DataTable();
    DataTable dtablevalue = new DataTable();
    DataTable dtablestaff = new DataTable();
    DataTable dtstaffdet = new DataTable();
    DataTable dtadvisor = new DataTable();
    DataRow dradvisor = null;
    DataRow drstaffdet = null;
    DataRow dtrow = null;
    DataRow dtrowvalue = null;
    DataRow dtrowstaff = null;
    DataTable dtable1 = new DataTable();
    DataRow dtrow1 = null;
    Hashtable hperiod = new Hashtable();
    Hashtable hnum = new Hashtable();
    Hashtable hatcol = new Hashtable();
    ArrayList rowperiod = new ArrayList();
    ArrayList rowhead = new ArrayList();
    DataTable dtgview1 = new DataTable();
    DataRow drgview1 = null;

    int rowIndex = -1;
    int selectedCellIndex = -1;

    #region Load Details

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            string PageLogOut = "";
            string sess = Convert.ToString(Session["IsLogin"]);
            PageLogOut = Convert.ToString(Session["PageLogout"]);
            if (sess == "")
            {
            }
            else
            {
                if (!Request.FilePath.Contains("AttM"))
                {
                    string strPreviousPage = "";
                    if (Request.UrlReferrer != null)
                    {
                        strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                    }
                    if (strPreviousPage == "")
                    {
                        string redrURI = ConfigurationManager.AppSettings["Attendance"].Trim();
                        Response.Redirect(redrURI, false);
                        return;
                    }
                }
            }
            if (Session["collegecode"] == null)
            {
                string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
                Response.Redirect(redrURI, false);
                return;
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            userCollegeCode = Convert.ToString(Session["collegecode"]);
            if (!IsPostBack)
            {
                lblErrMsg.Text = string.Empty;
                gview.Visible = false;
                gviewclassadv.Visible = false;
                gviewstaff.Visible = false;
                gview1.Visible = false;
                gridSelTT.Visible = false;

                txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                errmsg.Visible = false;
                txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Bindcolg();
                ddlcollege.SelectedIndex = 0;
                BindBatch();
                BindDegree();
                if (ddldegree.Items.Count > 0)
                {
                    ddldegree.Enabled = true;
                    ddlbranch.Enabled = true;
                    ddlsec.Enabled = true;
                    ddlsem.Enabled = true;
                    btngo.Enabled = true;
                    ddltimetable.Enabled = true;
                    BindBranch();
                    BindSem();
                    BindSectionDetail(strbatch, strbranch);
                    BindTimetable();
                    txtdate.Enabled = true;
                }
                else
                {
                    ddldegree.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlsec.Enabled = false;
                    ddlsem.Enabled = false;
                    btngo.Enabled = false;
                    ddltimetable.Enabled = false;
                    txtdate.Enabled = false;
                }
                lblday.Visible = false;
                lblday1.Visible = false;
                lbltimings.Visible = false;
                lblfromtime.Visible = false;
                lbltotime.Visible = false;
                treepanel.Visible = false;
                btnsave.Enabled = false;
                btnsave.Visible = false;
                txtmultisubj.Enabled = false;
                txttimetable.Visible = false;
                panelstaff.Visible = true;

                panelstaff.Visible = false;
                btnclassadvisor.Visible = false;
                btndelete.Visible = false;
                btnexcel.Visible = false;
                //btnprint.Visible = false;
                btnPDF.Visible = false;//Added by Manikandan on 10/12/2013
                Printcontrol.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                string grouporusercode = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                // Added By Sridharan 12 Mar 2015
                //{
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        //forschoolsetting = true;
                        lblcolg.Text = "School";
                        lblbatch.Text = "Year";
                        lbldegree.Text = "School Type";
                        lblbranch.Text = "Standard";
                        lblsem.Text = "Term";
                        //lblDegree.Attributes.Add("style", " width: 95px;");
                        //lblBranch.Attributes.Add("style", " width: 67px;");
                        //ddlBranch.Attributes.Add("style", " width: 241px;");
                    }
                    else
                    {
                        // forschoolsetting = false;
                    }
                }
                //} Sridharan
            }
            errmsg.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    public void BindBatch()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    //========Sangeetha On 29 Aug2014==========
    public void Bindcolg()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcolg.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcolg.Enabled = true;
                ddlcolg.DataSource = ds;
                ddlcolg.DataTextField = "collname";
                ddlcolg.DataValueField = "college_code";
                ddlcolg.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
            collegecode = ddlcolg.SelectedValue.ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    public void BindBranch()
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            collegecode = ddlcolg.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    public void BindSem()
    {
        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            collegecode = ddlcolg.SelectedValue.ToString();
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            errmsg.Visible = false;
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                //  ddlsec.Items.Insert(0, "All");
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsec.Enabled = false;
                }
                else
                {
                    ddlsec.Enabled = true;
                }
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void collook_load(object sender, EventArgs e)
    {
        try
        {
            BindBatch();
            BindDegree();
            if (ddldegree.Items.Count > 0)
            {
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsec.Enabled = true;
                ddlsem.Enabled = true;
                btngo.Enabled = true;
                ddltimetable.Enabled = true;
                BindBranch();
                BindSem();
                BindSectionDetail(strbatch, strbranch);
                BindTimetable();
                txtdate.Enabled = true;
            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsec.Enabled = false;
                ddlsem.Enabled = false;
                btngo.Enabled = false;
                ddltimetable.Enabled = false;
                txtdate.Enabled = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    public void BindTimetable()
    {
        try
        {
            lblday.Visible = false;
            lblday1.Visible = false;
            lbltimings.Visible = false;
            lblfromtime.Visible = false;
            lbltotime.Visible = false;
            treepanel.Visible = false;
            btnsave.Enabled = false;
            btnsave.Visible = false;
            txtmultisubj.Enabled = false;
            txttimetable.Visible = false;
            errmsg.Visible = false;
            btnexcel.Visible = false;
            //btnprint.Visible = false;
            btnPDF.Visible = false;//Added by Manikandan on 10/12/2013
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            ddltimetable.Items.Clear();
            btndelete.Visible = false;
            gviewclassadv.Visible = false;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string section = string.Empty;
            if (strsec != "" && strsec != "-1" && strsec.Trim().ToLower() != "all")
            {
                section = "and sections='" + strsec + "'";
            }
            ds.Dispose();
            ds.Reset();
            string strtimetable = "Select DISTINCT TTname from semester_schedule where batch_year=" + strbatchyear + " and degree_code=" + strbranch + " and semester=" + strsem + " " + section + "";
            ds = d2.select_method(strtimetable, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddltimetable.DataSource = ds;
                ddltimetable.DataTextField = "TTname";
                ddltimetable.DataBind();
                binddate();
            }
            ddltimetable.Items.Insert(0, "");
            ddltimetable.Items.Insert(1, "New");
            if (ddltimetable.Items.Count >= 3)
            {
                ddltimetable.SelectedIndex = ddltimetable.Items.Count - 1;
                loaddetail();
            }
            else
            {
                ddltimetable.SelectedIndex = 0;
            }
            txttimetable.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    public void binddate()
    {
        try
        {
            btnexcel.Visible = false;
            //btnprint.Visible = false;
            btnPDF.Visible = false;//Added by Manikandan on 10/12/2013
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errmsg.Visible = false;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string section = string.Empty;
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                section = "and sections='" + strsec + "'";
            }
            string date = d2.GetFunction("Select convert(nvarchar(15),Fromdate,103) as date from semester_schedule where batch_year=" + strbatchyear + " and degree_code=" + strbranch + " and semester=" + strsem + " " + section + " and ttname='" + ddltimetable.SelectedItem.ToString() + "'");
            if (ddltimetable.SelectedItem.ToString() == "New" || ddltimetable.SelectedItem.ToString() == "")
            {
                if (date == "" || date == null || date == "0")
                {
                    date = d2.GetFunction("Select convert(nvarchar(15),start_date,103) as date from seminfo where batch_year=" + strbatchyear + " and degree_code=" + strbranch + " and semester=" + strsem + " ");
                }
            }
            if (date != "" && date != null && date != "0" && ddltimetable.Enabled == true)
            {
                txtdate.Text = date;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldegree.Items.Count > 0)
        {
            errmsg.Visible = false;
            BindBranch();
            BindSem();
            BindSectionDetail(strbatch, strbranch);
            gview.Visible = false;
            gviewstaff.Visible = false;
            BindTimetable();
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        BindBranch();
        BindSem();
        BindSectionDetail(strbatch, strbranch);
        gview.Visible = false;
        gviewstaff.Visible = false;
        BindTimetable();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        BindSem();
        BindSectionDetail(strbatch, strbranch);
        gview.Visible = false;
        gviewstaff.Visible = false;
        BindTimetable();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        BindSectionDetail(strbatch, strbranch);
        gview.Visible = false;
        gviewstaff.Visible = false;
        BindTimetable();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        gview.Visible = false;
        gviewstaff.Visible = false;
        BindTimetable();
    }

    protected void ddltimetable_SelectedIndexChanged(object sender, EventArgs e)
    {
        loaddetail();
    }

    protected void loaddetail()
    {
        try
        {
            if (ddltimetable.SelectedItem.ToString() == "New")
            {
                txttimetable.Visible = true;
                txttimetable.Text = string.Empty;
                tdTime.Attributes.Add("style", "display:block;");
                tdbranch.Attributes.Add("colspan", "3");
                binddate();
            }
            else
            {
                tdbranch.Attributes.Add("colspan", "2");
                tdTime.Attributes.Add("style", "display:none;");
                txttimetable.Visible = false;
                btnclassadvisor.Visible = false;
                if (ddltimetable.SelectedItem.ToString() != "")
                {
                    gviewstaff.Visible = false;
                    binddate();
                    //loadfunction();//Hidden By Srinath 16/8/2013
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        loadfunction();
    }

    protected void loadfunction()
    {
        try
        {
            int cont = 0;
            rowhead.Clear();
            rowperiod.Clear();
            errmsg.Visible = false;
            Hashtable hatHr = new Hashtable();
            lblErrMsg.Text = string.Empty;
            btndelete.Visible = true;
            lblday.Visible = false;
            lblday1.Visible = false;
            lbltimings.Visible = false;
            lblfromtime.Visible = false;
            lbltotime.Visible = false;
            btnsave.Enabled = false;
            btnsave.Visible = true;
            gviewstaff.Visible = false;
            treepanel.Visible = false;
            btnexcel.Visible = false;
            //btnprint.Visible = false;
            btnPDF.Visible = false;//Added by Manikandan on 10/12/2013
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            gviewclassadv.Visible = false;
            int noofhours = 0;
            int dayorder = 0;
            int noofdays = 0;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] Daymon = new string[7] { "Monday", "Tuesday", "wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            errmsg.Visible = false;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = "and sections='" + strsec + "'";
            }
            else
            {
                strsec = string.Empty;
            }
            if (ddltimetable.SelectedItem.Text == "New")
            {
                string[] sp = txtdate.Text.Split('/');
                string dat = sp[2] + "/" + sp[1] + "/" + sp[0];
                string qry = "select TTName from Semester_Schedule where batch_year='" + ddlbatch.SelectedValue + "' and degree_code='" + ddlbranch.SelectedValue + "' and semester='" + ddlsem.SelectedItem.Text + "' and FromDate='" + dat + "' " + strsec + "";
                DataSet ds_set = d2.select_method_wo_parameter(qry, "Type");
                DataView dView = new DataView();
                ds_set.Tables[0].DefaultView.RowFilter = "TTName='" + txttimetable.Text + "'";
                dView = ds_set.Tables[0].DefaultView;
                if (ds_set.Tables[0].Rows.Count > 0)
                {
                    errmsg.Text = "Already Timetable Exist For " + txtdate.Text + "";
                    errmsg.Visible = true;
                    gview.Visible = false;
                    btnsave.Visible = false;
                    btndelete.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnexcel.Visible = false;
                    btnclassadvisor.Visible = false;
                    btnPDF.Visible = false;
                    return;
                }
                if (dView.Count > 0)
                {
                    errmsg.Text = "Time Table Name Already Exsist";
                    errmsg.Visible = true;
                    gview.Visible = false;
                    btnsave.Visible = false;
                    btndelete.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnexcel.Visible = false;
                    btnclassadvisor.Visible = false;
                    btnPDF.Visible = false;
                    return;
                }
            }
            string[] datespilt = txtdate.Text.Split('/');
            string temp_date = datespilt[1] + '/' + datespilt[0] + '/' + datespilt[2];
            ds.Dispose();
            ds.Reset();
            string holiday = string.Empty;
            string strpriodquery = "Select No_of_hrs_per_day,schorder,nodays,holiday from PeriodAttndSchedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + "";
            ds = d2.select_method(strpriodquery, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dayorder = Convert.ToInt32(ds.Tables[0].Rows[0]["schorder"]);
                noofhours = Convert.ToInt32(ds.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                noofdays = Convert.ToInt32(ds.Tables[0].Rows[0]["nodays"]);
                holiday = Convert.ToString(ds.Tables[0].Rows[0]["holiday"]);
                Session["totalhrs"] = Convert.ToString(noofhours);
                Session["totnoofdays"] = Convert.ToString(noofdays);
                Session["dayorder"] = Convert.ToString(dayorder);
            }
            if (noofhours > 0)
            {

                btnexcel.Visible = true;
                //btnprint.Visible = true;
                btnPDF.Visible = true;//Added by Manikandan on 10/12/2013
                Printcontrol.Visible = false;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;

                dtable.Columns.Add("Day/Timings");

                dtablevalue.Columns.Add("Day/Timings");
                rowperiod.Add("Day");


                rowhead.Add("Timings");
                for (int i = 1; i <= noofhours; i++)
                {
                    string belltime = string.Empty;
                    //Modified by srinath 10/6/2014
                    // string strtimequery = "Select start_time,end_time from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i + "'";
                    string strtimequery = "Select RIGHT(CONVERT(VARCHAR, start_time, 100),7) as sTime,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as endtime from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i + "'";
                    ds = d2.select_method(strtimequery, hat, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string starttime = ds.Tables[0].Rows[0]["sTime"].ToString();
                        string endtime = ds.Tables[0].Rows[0]["endtime"].ToString();
                        if (starttime != null && starttime.Trim() != "" && endtime != null && endtime.Trim() != "")
                        {
                            belltime = starttime + " To " + endtime;
                        }
                        //if (ds.Tables[0].Rows[0]["sTime"].ToString() != "" && ds.Tables[0].Rows[0]["endtime"].ToString() != null && ds.Tables[0].Rows[0]["end_time"].ToString() != "" && ds.Tables[0].Rows[0]["end_time"].ToString() != null)
                        //{
                        //string[] spiltstarttime = ds.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                        //string[] spiltendtime = ds.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                        //belltime=spiltstarttime[1].ToString() + ' ' + spiltstarttime[2].ToString() + ' ' + " To "+spiltendtime[1].ToString() + ' ' + spiltendtime[2].ToString();
                        // }
                    }
                    hperiod.Add(i, i);
                    rowperiod.Add(i);
                    string settext = i + " " + belltime;
                    hnum.Add(i, i);
                    dtable.Columns.Add(belltime);
                    dtablevalue.Columns.Add(Convert.ToString(i));
                    rowhead.Add(belltime);

                }
                string dayvalue = string.Empty;
                dtrow = dtable.NewRow();
                dtable.Rows.Add(dtrow);
                dtrow = dtable.NewRow();
                dtable.Rows.Add(dtrow);
                dtrowvalue = dtablevalue.NewRow();
                dtablevalue.Rows.Add(dtrowvalue);
                dtrowvalue = dtablevalue.NewRow();
                dtablevalue.Rows.Add(dtrowvalue);
                Dictionary<int, string> hasnote = new Dictionary<int, string>();
                Dictionary<int, string> hasnote1 = new Dictionary<int, string>();
                for (int day = 0; day <= 6; day++) //added by Mullai
                //for (int day = 0; day <= noofdays; day++)//Deepali on 16.4.18
                {
                    string dayofweek = Days[day];
                    string dayofweek1 = Daymon[day];
                    int daysetweek = day + 2;
                    // if (day == 6)
                    if (day == 6)//Deepali on 16.4.18
                    {
                        daysetweek = 1;
                    }
                    if (!holiday.Contains(daysetweek.ToString()))
                    {
                        if (dayorder == 1)
                        {
                            //  dtrow[1]  = dayofweek1;
                        }
                        else
                        {
                            int date = day + 1;

                            // dtrow[1] = "Day " + date;
                        }
                        hasnote.Add(cont, dayofweek);
                        hasnote1.Add(cont, dayofweek1);
                        cont++;
                        for (int i = 1; i <= noofhours; i++)
                        {
                            if (dayvalue == "")
                            {
                                dayvalue = dayofweek + i;
                            }
                            else
                            {
                                dayvalue = dayvalue + ',' + dayofweek + i;
                            }
                        }
                    }
                }
                if (ddltimetable.SelectedItem.ToString() != "New")
                {
                    ds.Clear();
                    string schedukle = "select  top 1 " + dayvalue + " from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + " and batch_year = " + strbatchyear + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc";
                    ds = d2.select_method(schedukle, hat, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < noofdays; i++)
                        {
                            dtrow = dtable.NewRow();
                            dtrowvalue = dtablevalue.NewRow();
                            string dayofweek = Days[i];
                            //string dayofweek1 = Daymon[i];
                            string dayofweek1 = Convert.ToString(hasnote1[i]);
                            string value = Convert.ToString(hasnote[i]);
                            for (int j = 1; j <= noofhours; j++)
                            {

                                string dsvalue = value + j;
                                string classhour = ds.Tables[0].Rows[0]["" + dsvalue + ""].ToString();
                                string setclasshour = string.Empty;
                                if (classhour.Trim() != "" && classhour.Trim() != "0" && classhour != null)
                                {
                                    string[] spiltmulpl = classhour.Split(';');
                                    for (int mul = 0; mul <= spiltmulpl.GetUpperBound(0); mul++)
                                    {
                                        string[] spiltclasshour = spiltmulpl[mul].Split('-');
                                        for (int sp = 0; sp <= spiltclasshour.GetUpperBound(0); sp++)
                                        {
                                            if (sp == 0)
                                            {
                                                if (setclasshour == "")
                                                {
                                                    try
                                                    {
                                                        setclasshour = d2.GetFunction("select case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no='" + spiltclasshour[sp].ToString() + "'");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
                                                    }
                                                }
                                                else
                                                {
                                                    setclasshour = setclasshour + ';' + d2.GetFunction("select case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no='" + spiltclasshour[sp].ToString() + "'"); ;
                                                }
                                            }
                                            else
                                            {
                                                setclasshour = setclasshour + '-' + spiltclasshour[sp].ToString();
                                            }
                                        }
                                    }
                                }
                                dtrow[j] = setclasshour;
                                dtrowvalue[j] = classhour;
                                dtrowvalue[0] = value;

                            }


                            int daysetweek = i + 2;
                            // if (day == 6)
                            //if (i == 6)//Deepali on 16.4.18
                            //{
                            //    daysetweek = 1;
                            //}
                            //if (!holiday.Contains(daysetweek.ToString()))
                            //{
                            if (dayorder == 1)
                            {
                                dtrow[0] = dayofweek1;

                            }
                            else
                            {
                                int date = i + 1;

                                dtrow[0] = "Day " + date;
                            }
                            dtable.Rows.Add(dtrow);
                            dtablevalue.Rows.Add(dtrowvalue);
                            // }

                        }

                        for (int day = 0; day <= noofdays; day++)//Deepali on 16.4.18
                        {

                        }
                        btnsave.Visible = true;
                        btndelete.Visible = false;
                        gviewclassadv.Visible = false;
                        schedukle = d2.GetFunction("select  top 1 class_advisor from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + " and batch_year = " + strbatchyear + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc");
                        if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                        {

                            dtadvisor.Columns.Add("sno");
                            dtadvisor.Columns.Add("stfnme");
                            dtadvisor.Columns.Add("stfcode");
                            int sno = 0;
                            string[] spiltadvisor = schedukle.Split(',');
                            for (int i = 0; i <= spiltadvisor.GetUpperBound(0); i++)
                            {
                                dradvisor = dtadvisor.NewRow();
                                sno++;
                                string staffname = d2.GetFunction("select staff_name from staffmaster where staff_code='" + spiltadvisor[i].ToString() + "'");

                                dradvisor["sno"] = Convert.ToString(sno);
                                dradvisor["stfnme"] = staffname;
                                dradvisor["stfcode"] = spiltadvisor[i].ToString();
                                dtadvisor.Rows.Add(dradvisor);
                            }
                        }
                        ViewState["dtadvisor"] = dtadvisor;
                        gviewclassadv.DataSource = dtadvisor;
                        gviewclassadv.DataBind();
                        gviewclassadv.Visible = true;
                    }
                    else
                    {
                        btndelete.Visible = false;
                        errmsg.Visible = true;
                        gview.Visible = false;
                        errmsg.Text = "No Records Found";
                        btnsave.Visible = false;
                        btnclassadvisor.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnexcel.Visible = false;
                        btnPDF.Visible = false;
                    }
                }
                else
                {
                    if (txttimetable.Text == "" || txttimetable.Text == null || txttimetable.Text.ToLower() == "new")
                    {
                        btndelete.Visible = false;
                        gview.Visible = false;
                        errmsg.Visible = true;
                        gview.Visible = false;
                        errmsg.Text = "Please Enter Time Table Name";
                        btnsave.Visible = false;
                        btnclassadvisor.Visible = false;
                        gviewclassadv.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnexcel.Visible = false;
                        btnPDF.Visible = false;
                    }
                    else
                    {
                        for (int day = 0; day <= 6; day++)//Deepali on 16.4.18
                        {
                            string dayofweek = Days[day];
                            string dayofweek1 = Daymon[day];
                            int daysetweek = day + 2;
                            // if (day == 6)
                            if (day == 6)//Deepali on 16.4.18
                            {
                                daysetweek = 1;
                            }
                            if (!holiday.Contains(daysetweek.ToString()))
                            {
                                dtrow = dtable.NewRow();
                                dtrowvalue = dtablevalue.NewRow();

                                if (dayorder == 1)
                                {
                                    dtrow[0] = dayofweek1;
                                }
                                else
                                {
                                    int date = day + 1;
                                    dtrow[0] = "Day " + date;
                                }
                                dtrowvalue[0] = dayofweek;
                                for (int i = 1; i <= noofhours; i++)
                                {
                                    if (dayvalue == "")
                                    {
                                        dayvalue = dayofweek + i;
                                    }
                                    else
                                    {
                                        dayvalue = dayvalue + ',' + dayofweek + i;
                                    }
                                }
                                dtable.Rows.Add(dtrow);
                                dtablevalue.Rows.Add(dtrowvalue);
                            }
                        }
                    }
                }
            }
            else
            {
                btndelete.Visible = false;
                btnsave.Visible = false;
                errmsg.Visible = true;
                gview.Visible = false;
                errmsg.Text = "Please Update Semester Information";
            }
            ViewState["table"] = dtablevalue;
            ViewState["table1"] = dtable;
            ViewState["rowhead"] = rowhead;
            ViewState["rowperiod"] = rowperiod;
            gview.DataSource = dtable;
            gview.DataBind();
            if (gview.Rows.Count > 2)
            {
                gview.Visible = true;
                btnsave.Visible = true;
                btndelete.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnexcel.Visible = true;
                btnPDF.Visible = true;
            }


            RowHead(gview);
            RowHeadSpan(gview);
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 2; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }

    protected void RowHeadstaff(GridView gviewstaff)
    {
        for (int head = 0; head < 1; head++)
        {
            gviewstaff.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gviewstaff.Rows[head].Font.Bold = true;
            gviewstaff.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gviewstaff.Rows[head].Font.Name = "Book Antique";
        }
    }

    protected void RowHeadSpan(GridView gview)
    {
        for (int row = 2; row > 0; row--)
        {
            GridViewRow roww = gview.Rows[row];
            GridViewRow previousRow = gview.Rows[row - 1];
            for (int cell = 0; cell < gview.Rows[row].Cells.Count; cell++)
            {
                if (gview.HeaderRow.Cells[cell].Text.Trim() == "Semester")
                {
                    if (roww.Cells[cell].Text == previousRow.Cells[cell].Text)
                    {
                        if (previousRow.Cells[cell].RowSpan == 0)
                        {
                            if (roww.Cells[cell].RowSpan == 0)
                            {
                                previousRow.Cells[cell].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[cell].RowSpan = roww.Cells[cell].RowSpan + 1;
                            }
                            roww.Cells[cell].Visible = false;
                        }
                    }
                }
            }
        }
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex != 0 && e.Row.RowIndex != 1)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    TableCell cell = e.Row.Cells[i];
                    cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, i
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                }
            }
        }
    }

    protected void gview_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int c = rowhead.Count;
            int c1 = rowperiod.Count;
            ArrayList headlist = (ArrayList)ViewState["rowhead"];
            ArrayList periodlist = (ArrayList)ViewState["rowperiod"];

            if (e.Row.RowIndex == 0)
            {
                e.Row.Width = 50;
                for (int cell = 0; cell < periodlist.Count; cell++)
                {
                    e.Row.Cells[cell].Text = Convert.ToString(periodlist[cell]);
                }
                e.Row.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                e.Row.Font.Bold = true;
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Font.Name = "Book Antique";
            }
            if (e.Row.RowIndex == 1)
            {
                e.Row.Width = 50;
                for (int cell = 0; cell < headlist.Count; cell++)
                {
                    e.Row.Cells[cell].Text = Convert.ToString(headlist[cell]);
                }
                e.Row.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                e.Row.Font.Bold = true;
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Font.Name = "Book Antique";
            }
            e.Row.Width = 50;
        }
    }

    protected void gview_onselectedindexchanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            rowIndex = grid.SelectedIndex;
            selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string activerow = Convert.ToString(rowIndex);
            string activecol = Convert.ToString(selectedCellIndex);
            Session["roww"] = activerow;
            Session["coll"] = activecol;

            string gviewVal = gview.Rows[Convert.ToInt32(activerow)].Cells[Convert.ToInt32(activecol)].Text;
            if (gviewVal != string.Empty && gviewVal != "&nbsp;")
            {
                div2.Visible = true;
                Div9.Visible = true;
            }
            else
            {
                addPopup();
            }
            if (1 == 1)
            {

            }
            else
            {
                txtmulstaff.Visible = false;
                lblmulstaff.Visible = false;
                pmulstaff.Visible = false;
                btnmulstaff.Visible = false;
                btndelete.Visible = false;
                btndelete.Enabled = false;
                lbltimings.Visible = false;
                lblfromtime.Visible = false;
                lbltotime.Visible = false;
                btnOk.Visible = false;
                txtmultisubj.Visible = false;
                chkappend.Checked = false;
                chkappend.Visible = false;
                dtablevalue = (DataTable)ViewState["table"];
                //var grid = (GridView)sender;
                //GridViewRow selectedRow = grid.SelectedRow;
                //rowIndex = grid.SelectedIndex;
                //selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
                //string activerow = Convert.ToString(rowIndex);
                //string activecol = Convert.ToString(selectedCellIndex);
                Session["actrow"] = rowIndex;
                Session["actcol"] = selectedCellIndex;
                if (activecol != "0" && activerow != "-1")
                {
                    int row = Convert.ToInt32(activerow);
                    int col = Convert.ToInt32(activecol);
                    lblday.Visible = true;
                    lblday1.Visible = true;
                    lblday1.Text = gview.Rows[row].Cells[col].Text;
                    ds.Dispose();
                    ds.Reset();
                    string strtimequery = "Select RIGHT(CONVERT(VARCHAR, start_time, 100),7) as start_time,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as end_time from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + col + "'";
                    ds = d2.select_method(strtimequery, hat, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["start_time"].ToString() != "" && ds.Tables[0].Rows[0]["start_time"].ToString() != null && ds.Tables[0].Rows[0]["end_time"].ToString() != "" && ds.Tables[0].Rows[0]["end_time"].ToString() != null)
                        {
                            //string[] spiltstarttime = ds.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                            //string[] spiltendtime = ds.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                            //lblfromtime.Text = spiltstarttime[1].ToString() + ' ' + spiltstarttime[2].ToString() + ' ' + "To";
                            //lbltotime.Text = spiltendtime[1].ToString() + ' ' + spiltendtime[2].ToString();
                            lblfromtime.Text = ds.Tables[0].Rows[0]["start_time"].ToString();
                            lbltotime.Text = ds.Tables[0].Rows[0]["end_time"].ToString();
                            lbltimings.Visible = true;
                            lblfromtime.Visible = true;
                            lbltotime.Visible = true;
                        }
                    }
                    if (gview.Rows[row].Cells[col].Text != null && gview.Rows[row].Cells[col].Text != "")
                    {
                        btndelete.Visible = true;
                        btndelete.Enabled = true;
                        string columnvalue = Convert.ToString(dtablevalue.Rows[row][col]);
                        dtablestaff.Columns.Add("Subject");
                        dtablestaff.Columns.Add("Staff Name");
                        dtablestaff.Columns.Add("Class Type");

                        dtrowstaff = dtablestaff.NewRow();
                        dtrowstaff["Subject"] = "Subject";
                        dtrowstaff["Staff Name"] = "Staff Name";
                        dtrowstaff["Class Type"] = "Class Type";

                        dtablestaff.Rows.Add(dtrowstaff);

                        string[] spitcolumnvalue = columnvalue.Split(';');
                        ArrayList arrStaffSubjects = new ArrayList();
                        for (int i = 0; i <= spitcolumnvalue.GetUpperBound(0); i++)
                        {
                            string[] spitsubject = spitcolumnvalue[i].Split('-');
                            int t = spitsubject.GetUpperBound(0);
                            for (int j = 1; j < t; j++)
                            {
                                for (int next = j; next <= j; next++)
                                {
                                    string subjectno = d2.GetFunction("Select subject_name from subject where subject_no='" + spitsubject[0] + "'");
                                    string type = spitsubject[spitsubject.GetUpperBound(0)].Trim();
                                    if (type == "S")
                                    {
                                        type = "Single";
                                    }
                                    else if (type == "C")   //modifed by Prabha 25/11/2017
                                    {
                                        type = "Combined";
                                    }
                                    else
                                    {
                                        type = "Lab";
                                    }
                                    string staff = d2.GetFunction("Select Staff_name from staffmaster where staff_code='" + spitsubject[next].ToString() + "'");
                                    if (staff.Trim() == "" || staff.Trim() == "0")
                                    {
                                        staff = "-";
                                    }
                                    string keyList = spitsubject[0].Trim() + "@" + Convert.ToString(spitsubject[next]).Trim();
                                    if (!arrStaffSubjects.Contains(keyList.Trim().ToLower()))
                                    {
                                        dtrowstaff = dtablestaff.NewRow();
                                        arrStaffSubjects.Add(keyList.Trim().ToLower());
                                        dtrowstaff["Subject"] = subjectno;
                                        dtrowstaff["Staff Name"] = staff;
                                        dtrowstaff["Class Type"] = type;
                                        dtablestaff.Rows.Add(dtrowstaff);
                                        gviewstaff.DataSource = dtablestaff;
                                        gviewstaff.DataBind();
                                        gviewstaff.Visible = true;
                                        RowHeadstaff(gviewstaff);
                                    }
                                }
                            }
                        }
                    }
                    treeload();
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    private void treeload()
    {
        try
        {
            spcellClickPopup.Visible = true;
            chkappend.Checked = false;
            txtmultisubj.Visible = false;
            pnlmultisubj.Visible = false;
            chk_multisubj.Visible = false;
            gview1.Visible = false;
            gridSelTT.Visible = false;
            subjtree.Visible = true;
            treepanel.Visible = true;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            subjtree.Nodes.Clear();
            //sankar added july'01
            //---------alternate subj shouldnt be same as subject
            int actrow = 0;
            int actcol = 0;
            string subjname_staffcode = string.Empty;
            string subjname = string.Empty;


            actrow = Convert.ToInt32(Session["actrow"]);//rowIndex;
            actcol = Convert.ToInt32(Session["actcol"]);//selectedCellIndex;

            if (actrow != -1)
            {
                subjname_staffcode = gview.Rows[actrow].Cells[actcol].Text;
                string[] splitsubj = subjname_staffcode.Split(new Char[] { '-' });
                subjname = splitsubj[0].ToString();
                //-------------------
                string Syllabus_year = string.Empty;
                Syllabus_year = GetSyllabusYear(strbranch, strbatchyear, strsem);
                if (Syllabus_year != "-1")
                {
                    //--------------get subject type and subjects
                    string query = "select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + strbranch + " and semester=" + strsem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + strbatchyear + ") order by subject.subtype_no";
                    DataSet dssubTypeRs = d2.select_method(query, hat, "Text");
                    TreeNode node;
                    int rec_count = 0;
                    if (dssubTypeRs.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dssubTypeRs.Tables[0].Rows.Count; i++)
                        {
                            if ((dssubTypeRs.Tables[0].Rows[i]["subject_type"].ToString()) != "0")
                            {
                                query = "select subject.subtype_no,subject_type,subject_no,subject_name,subject_code from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + strbranch + " and semester=" + strsem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + strbatchyear + ") and subject.subtype_no=" + dssubTypeRs.Tables[0].Rows[i]["subtype_no"].ToString() + " order by subject.subtype_no,subject.subject_no";
                                DataSet subTypeRs1 = d2.select_method(query, hat, "Text");
                                node = new TreeNode(dssubTypeRs.Tables[0].Rows[i]["subject_type"].ToString(), rec_count.ToString());
                                for (int j = 0; j < subTypeRs1.Tables[0].Rows.Count; j++)
                                {
                                    if (subTypeRs1.Tables[0].Rows[j]["subject_name"].ToString() != "0" && subTypeRs1.Tables[0].Rows[j]["subject_name"] != subjname)
                                    {
                                        node.ChildNodes.Add(new TreeNode(subTypeRs1.Tables[0].Rows[j]["subject_code"].ToString() + "-" + subTypeRs1.Tables[0].Rows[j]["subject_name"].ToString(), subTypeRs1.Tables[0].Rows[j]["subject_no"].ToString())); //modified by Mullai
                                        rec_count = rec_count + 1;
                                    }
                                }
                                subjtree.Nodes.Add(node);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    private string GetSyllabusYear(string degree_code, string batch_year, string sem)
    {
        string syl_year = string.Empty;
        try
        {
            DataSet dssyl_year = d2.select_method("select syllabus_year from syllabus_master where degree_code=" + degree_code + " and semester =" + sem + " and batch_year=" + batch_year + " ", hat, "Text");
            if (dssyl_year.Tables[0].Rows.Count > 0)
            {
                if (dssyl_year.Tables[0].Rows[0]["syllabus_year"].ToString() == "\0")
                {
                    syl_year = "-1";
                }
                else
                {
                    syl_year = dssyl_year.Tables[0].Rows[0]["syllabus_year"].ToString();
                }
            }
            else
            {
                syl_year = "-1";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
        return syl_year;
    }

    protected void advRemoveOnClick(object sender, EventArgs e)
    {
        Button grids = (Button)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;

        DataTable dtadv = (DataTable)ViewState["dtadvisor"];
        dtadv.Rows[rowIndx].Delete();

        ViewState["dtadvisor"] = dtadv;
        gviewclassadv.DataSource = dtadv;
        gviewclassadv.DataBind();
        gviewclassadv.Visible = true;

        classadvisorsave();
    }

    public void classadvisorsave()
    {
        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = "and sections='" + strsec + "'";
            }
            else
            {
                strsec = string.Empty;
            }
            string[] datespilt = txtdate.Text.Split('/');
            string temp_date = datespilt[1] + '/' + datespilt[0] + '/' + datespilt[2];
            string classadvisor = string.Empty;
            for (int i = 0; i < gviewclassadv.Rows.Count; i++)
            {
                string[] spiltcheck = classadvisor.Split(',');
                Boolean chevalflag = false;
                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                {
                    string str = (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;
                    if (str == spiltcheck[ch].ToString())
                    {
                        chevalflag = true;
                    }
                }
                if (chevalflag == false)
                {
                    if (classadvisor == "")
                    {
                        classadvisor = (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;
                    }
                    else
                    {
                        classadvisor = classadvisor + ',' + (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;
                    }
                }
            }
            if (gviewclassadv.Rows.Count > 0)
            {
                gviewclassadv.Visible = true;
            }
            else
            {
                gviewclassadv.Visible = false;
            }
            string schedukle = "update semester_schedule set class_advisor='" + classadvisor + "' where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedItem.ToString() + " and batch_year = " + strbatchyear + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + "";
            int insert = d2.update_method_wo_parameter(schedukle, "Text");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void subjtree_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt_node = new DataTable();
            DataRow dr;
            dtgview1.Rows.Clear();
            dtgview1.Columns.Clear();
            dtgview1.Columns.Add("subnme");
            dtgview1.Columns.Add("subtag");
            dtgview1.Columns.Add("chkvalue");
            dtgview1.Columns.Add("ddltag");
            dtgview1.Columns.Add("ddlvalue");
            gridSelTT.Enabled = true;
            btnOk.Enabled = true;
            lblErrMsg.Visible = false;

            if (chkappend.Checked)
            {
                dt_node.Columns.Add("subnme");
                dt_node.Columns.Add("subtag");
                dt_node.Columns.Add("chkvalue");
                dt_node.Columns.Add("ddltag");
                dt_node.Columns.Add("ddlvalue");

                for (int i = 0; i < gview1.Rows.Count; i++)
                {
                    string strVal = string.Empty;
                    dr = dt_node.NewRow();

                    dr["subnme"] = (gview1.Rows[i].FindControl("lblsubnme") as Label).Text;
                    dr["subtag"] = (gview1.Rows[i].FindControl("lblsubtag") as Label).Text;
                    CheckBoxList chklst = (gview1.Rows[i].FindControl("cbl_staffname") as CheckBoxList);
                    for (int cnt = 0; cnt < chklst.Items.Count; cnt++)
                    {
                        if (chklst.Items[cnt].Selected)
                        {
                            if (strVal == string.Empty)
                            {
                                strVal = chklst.Items[cnt].Text;
                            }
                            else
                            {
                                strVal = strVal + "," + chklst.Items[cnt].Text;
                            }
                        }
                    }
                    dr["chkvalue"] = strVal;
                    dr["ddltag"] = (gview1.Rows[i].FindControl("lblddltag") as Label).Text;
                    dr["ddlvalue"] = (gview1.Rows[i].FindControl("lblddlvalue") as Label).Text;
                    dt_node.Rows.Add(dr);
                }
                ViewState["dtgview1"] = dt_node;
            }

            Boolean check = false;
            Hashtable ddlhas = new Hashtable();
            txtmulstaff.Visible = false;
            lblmulstaff.Visible = false;
            pmulstaff.Visible = false;
            btnmulstaff.Visible = false;
            chkmullsstaff.Items.Clear();
            chkmulstaff.Checked = false;
            txtmulstaff.Text = "---Select---";
            chk_multisubj.Visible = false;
            gview1.Visible = false;
            gridSelTT.Visible = false;
            subjtree.Visible = true;
            chkappend.Visible = false;
            btnOk.Visible = false;
            treepanel.Visible = true;
            spcellClickPopup.Visible = true;
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec1 = string.Empty;
            int staf_cnt = 0;
            string staff_code = "", staff_name_code = string.Empty;
            string[] staff_list = new string[0];
            int rowval = 0;
            int rowa = 0;
            string strsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString().Trim().ToLower() != "all" && ddlsec.SelectedItem.ToString().Trim().ToLower() != "" && ddlsec.SelectedItem.ToString().Trim().ToLower() != "-1" && ddlsec.SelectedItem.ToString().Trim().ToLower() != null)
                {
                    strsec = " and sections='" + ddlsec.SelectedItem.ToString().Trim() + "'";
                    strsec1 = ddlsec.SelectedItem.ToString();
                }
            }
            int parent_count = subjtree.Nodes.Count;//----------count parent node value
            for (int i = 0; i < parent_count; i++)
            {
                for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                {
                    if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                    {
                        if (ddltimetable.SelectedItem.Text.Trim() == "New")
                        {
                            gridSelTT.Visible = true;
                            bindGridTTSelect(gview.HeaderRow.Cells.Count - 1);
                        }
                        else
                        {
                            gridSelTT.Visible = true;
                            bindGridTTSelect(gview.HeaderRow.Cells.Count - 1);
                        }
                        gview1.Visible = true;
                        subjtree.Visible = true;
                        chkappend.Visible = true;
                        btnOk.Visible = true;
                        treepanel.Visible = true;
                        string temp_sec = string.Empty;
                        if (strsec == "")
                        {
                            temp_sec = string.Empty;
                        }
                        else
                        {
                            temp_sec = " and Sections='" + strsec1 + "'";
                        }
                        if (chkappend.Checked == true)
                        {
                            check = true;
                            bool subj = false;
                            string subno = subjtree.Nodes[i].ChildNodes[node_count].Value;

                            if (gview1.Rows.Count > 0)
                            {
                                rowa = gview1.Rows.Count - 1;
                                while (rowa >= 0)
                                {
                                    string rows = (gview1.Rows[rowa].FindControl("lblsubtag") as Label).Text;

                                    if (subno == rows)
                                    {

                                        subj = true;

                                    }
                                    rowa--;
                                }
                            }
                            if (subj == false)
                            {
                                if (gview1.Rows.Count >= 1)
                                {
                                }
                                dtgview1 = (DataTable)ViewState["dtgview1"];

                                drgview1 = dtgview1.NewRow();
                                //tring ss = dtgview1.Rows[2]["ddlvalue"].ToString();
                                drgview1["subnme"] = subjtree.Nodes[i].ChildNodes[node_count].Text;
                                drgview1["subtag"] = subjtree.Nodes[i].ChildNodes[node_count].Value;

                                string chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;
                                //--------------bind staff name into the spread
                                DataSet staf_set = d2.select_method("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + strbatchyear + "  " + temp_sec + ")", hat, "Text");


                                ddlhas.Add(dtgview1.Rows.Count, chile_index);

                                if (staf_set.Tables.Count > 0 && staf_set.Tables[0].Rows.Count > 0)
                                {
                                    txtmulstaff.Visible = false;
                                    lblmulstaff.Visible = false;
                                    pmulstaff.Visible = false;
                                    btnmulstaff.Visible = false;
                                    staff_list = new string[staf_set.Tables[0].Rows.Count + 1];//string[] 
                                    for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                                    {
                                        staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        chkmullsstaff.Items.Add(staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString());
                                        if (staff_code == "")
                                        {
                                            staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        }
                                        else
                                        {
                                            staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        }
                                    }
                                    if (staff_list.GetUpperBound(0) > 0)
                                    {
                                        staff_list[staf_cnt] = "All";
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Staff Found')", true);
                                }

                                drgview1["ddltag"] = staff_code;
                                drgview1["ddlvalue"] = staff_name_code;


                                treepanel.Visible = true;
                                if (staff_name_code != string.Empty && staff_code != string.Empty)
                                {
                                    dtgview1.Rows.Add(drgview1);
                                }
                                ViewState["dtgview1"] = dtgview1;
                            }
                            dtgview1 = (DataTable)ViewState["dtgview1"];
                        }
                        else
                        {
                            check = true;
                            drgview1 = dtgview1.NewRow();

                            rowval = 0;
                            //-------set selected subject name into the sprad

                            string chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;


                            drgview1["subnme"] = subjtree.Nodes[i].ChildNodes[node_count].Text;
                            drgview1["subtag"] = subjtree.Nodes[i].ChildNodes[node_count].Value;
                            //--------------bind staff name into the spread
                            DataSet staf_set = d2.select_method("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + strbatchyear.ToString() + " " + temp_sec + ")", hat, "Text");
                            if (staf_set.Tables[0].Rows.Count > 0)
                            {
                                txtmulstaff.Visible = false;
                                lblmulstaff.Visible = false;
                                pmulstaff.Visible = false;
                                btnmulstaff.Visible = false;
                                staff_list = new string[staf_set.Tables[0].Rows.Count + 1];//string[] 
                                for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                                {
                                    staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    chkmullsstaff.Items.Add(staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString());
                                    if (staff_code == "")
                                    {
                                        staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    }
                                    else
                                    {
                                        staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                    }
                                }
                                if (staff_list.GetUpperBound(0) > 0)
                                {
                                    staff_list[staf_cnt] = "All";
                                }

                                drgview1["ddltag"] = staff_code;
                                drgview1["ddlvalue"] = staff_name_code;
                                treepanel.Visible = true;
                                dtgview1.Rows.Add(drgview1);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Staff Found')", true);
                                gridSelTT.Enabled = false;
                                btnOk.Enabled = false;
                            }
                        }
                        if (ddltimetable.SelectedItem.Text.Trim() == "New")
                        {
                            if (ddltimetable.SelectedItem.Text.Trim() == "New")
                            {
                                gridSelTT.Visible = true;
                                bindGridTTSelect();
                            }
                            else
                            {
                                gridSelTT.Visible = true;
                                bindGridTTSelect();
                            }
                        }
                        ViewState["dtgview1"] = dtgview1;
                        gview1.DataSource = dtgview1;
                        gview1.DataBind();
                        gview1.Visible = true;

                        if (gview1.Rows.Count > 0)
                        {
                            foreach (GridViewRow gr in gview1.Rows)
                            {
                                string chile_index = (gr.FindControl("lblsubtag") as Label).Text;
                                DataSet staf_set = d2.select_method("select staff_name+'-'+staff_code staffname,staff_code from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year='" + strbatchyear.ToString() + "' " + temp_sec + ")", hat, "Text");
                                if (staf_set.Tables.Count > 0 && staf_set.Tables[0].Rows.Count > 0)
                                {
                                    CheckBoxList ddlstaff = (gr.FindControl("cbl_staffname") as CheckBoxList);
                                    ddlstaff.DataSource = staf_set.Tables[0];
                                    ddlstaff.DataTextField = "staffname";
                                    ddlstaff.DataValueField = "staff_code";
                                    ddlstaff.DataBind();
                                }
                            }

                            for (int row = 0; row < dtgview1.Rows.Count; row++)
                            {
                                string chkval = dtgview1.Rows[row]["chkvalue"].ToString();
                                CheckBoxList ddlstaff = (gview1.Rows[row].FindControl("cbl_staffname") as CheckBoxList);
                                if (chkval != string.Empty)
                                {
                                    string[] chkspl = chkval.Split(',');
                                    for (int len = 0; len < chkspl.Length; len++)
                                    {
                                        ddlstaff.Items.FindByText(chkspl[len]).Selected = true;
                                    }
                                    TextBox txt = (gview1.Rows[row].FindControl("txt_staffname") as TextBox);
                                    txt.Text = chkval;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    protected void gview1_OnDataBound(object sender, EventArgs e)
    {
        string strsec = string.Empty;
        string temp_sec = string.Empty;
        string strsec1 = string.Empty;
        //string staff_code = "", staff_name_code = string.Empty;
        strbatchyear = ddlbatch.Text.ToString();

        if (ddlsec.Enabled == true)
        {
            if (ddlsec.SelectedItem.ToString().Trim().ToLower() != "all" && ddlsec.SelectedItem.ToString().Trim().ToLower() != "" && ddlsec.SelectedItem.ToString().Trim().ToLower() != "-1" && ddlsec.SelectedItem.ToString().Trim().ToLower() != null)
            {
                strsec = " and sections='" + ddlsec.SelectedItem.ToString().Trim() + "'";
                strsec1 = ddlsec.SelectedItem.ToString();
            }
        }

        if (strsec == "")
        {
            temp_sec = string.Empty;
        }
        else
        {
            temp_sec = " and Sections='" + strsec1 + "'";
        }

        foreach (GridViewRow gr in gview1.Rows)
        {
            string chile_index = (gr.FindControl("lblsubtag") as Label).Text;
            DataSet staf_set = d2.select_method("select staff_name+'-'+staff_code staffname from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year='" + strbatchyear.ToString() + "' " + temp_sec + ")", hat, "Text");
            if (staf_set.Tables.Count > 0 && staf_set.Tables[0].Rows.Count > 0)
            {
                DropDownList ddlstaff = (gr.FindControl("ddlstaffnme") as DropDownList);
                ddlstaff.DataSource = staf_set.Tables[0];
                ddlstaff.DataTextField = "staffname";
                ddlstaff.DataBind();
                int cnt = staf_set.Tables[0].Rows.Count;
                ddlstaff.Items.Insert(cnt, "All");
            }
        }
    }

    protected void RemoveOnClick(object sender, EventArgs e)
    {
        Button grids = (Button)sender;
        string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxSs) - 2;

        DataTable Vgview1 = (DataTable)ViewState["dtgview1"];
        Vgview1.Rows[rowIndx].Delete();
        ViewState["dtgview1"] = Vgview1;
        gview1.DataSource = Vgview1;
        gview1.DataBind();
        gview1.Visible = true;

        string strsec1 = string.Empty;
        string strsec = string.Empty;
        if (ddlsec.Enabled == true)
        {
            if (ddlsec.SelectedItem.ToString().Trim().ToLower() != "all" && ddlsec.SelectedItem.ToString().Trim().ToLower() != "" && ddlsec.SelectedItem.ToString().Trim().ToLower() != "-1" && ddlsec.SelectedItem.ToString().Trim().ToLower() != null)
            {
                strsec = " and sections='" + ddlsec.SelectedItem.ToString().Trim() + "'";
                strsec1 = ddlsec.SelectedItem.ToString();
            }
        }
        string temp_sec = string.Empty;
        if (strsec == "")
        {
            temp_sec = string.Empty;
        }
        else
        {
            temp_sec = " and Sections='" + strsec1 + "'";
        }
        if (gview1.Rows.Count > 0)
        {
            foreach (GridViewRow gr in gview1.Rows)
            {
                string chile_index = (gr.FindControl("lblsubtag") as Label).Text;
                DataSet staf_set = d2.select_method("select staff_name+'-'+staff_code staffname,staff_code from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year='" + strbatchyear.ToString() + "' " + temp_sec + ")", hat, "Text");
                if (staf_set.Tables.Count > 0 && staf_set.Tables[0].Rows.Count > 0)
                {
                    CheckBoxList ddlstaff = (gr.FindControl("cbl_staffname") as CheckBoxList);
                    ddlstaff.DataSource = staf_set.Tables[0];
                    ddlstaff.DataTextField = "staffname";
                    ddlstaff.DataValueField = "staff_code";
                    ddlstaff.DataBind();
                }
            }

            for (int row = 0; row < Vgview1.Rows.Count; row++)
            {
                string chkval = Vgview1.Rows[row]["chkvalue"].ToString();
                CheckBoxList ddlstaff = (gview1.Rows[row].FindControl("cbl_staffname") as CheckBoxList);
                if (chkval != string.Empty)
                {
                    string[] chkspl = chkval.Split(',');
                    for (int len = 0; len < chkspl.Length; len++)
                    {
                        ddlstaff.Items.FindByText(chkspl[len]).Selected = true;
                    }
                    TextBox txt = (gview1.Rows[row].FindControl("txt_staffname") as TextBox);
                    txt.Text = chkval;
                }
            }
        }

        subjtree.Visible = true;
        gview1.Visible = true;
        chkappend.Visible = true;
        btnOk.Visible = true;
        treepanel.Visible = true;

        txtmulstaff.Visible = false;
        lblmulstaff.Visible = false;
        pmulstaff.Visible = false;
        btnmulstaff.Visible = false;
    }

    protected void chk_multisubj_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtmultisubj.Visible = true;
            pnlmultisubj.Visible = true;
            if (chk_multisubj.Checked == true)// && chklistmultisubj .Items.Count>0)
            {
                txtmultisubj.Enabled = true;
            }
            else
            {
                txtmultisubj.Visible = false;
                pnlmultisubj.Visible = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void chklistmultisubj_selectedindetxchange(object sender, EventArgs e)
    {
        int cnt = 0;
        for (int chk_cnt = 0; chk_cnt < chklistmultisubj.Items.Count; chk_cnt++)
        {
            if (chklistmultisubj.Items[chk_cnt].Selected == true)
            {
                cnt++;
            }
        }
        txtmultisubj.Text = cnt + " Staff(s)";
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            int allotcount = 0;
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;
            string subj_number = string.Empty;
            //   Hashtable allotrow = new Hashtable();
            int hasrow = 0;
            int hascol = 0;
            Hashtable allotcol = new Hashtable();
            string splval = string.Empty; string splval_temp = string.Empty;
            string subno_staff = string.Empty; string subno_staffnote = string.Empty;
            string staffname = string.Empty; string staff_name_code = string.Empty; string staffcode = string.Empty;
            btnsave.Visible = true;
            btnsave.Enabled = true;
            string SqlFinal = string.Empty;
            string[] date = txtdate.Text.Split('/');
            string fromdate = date[1] + '/' + date[0] + '/' + date[2];

            int activerow = Convert.ToInt32(Session["actrow"]); //rowIndex;
            int activecol = Convert.ToInt32(Session["actcol"]); //selectedCellIndex;
            string coursename = string.Empty;
            string acrnym = string.Empty;
            string ster = string.Empty;
            string btch = string.Empty;
            string sctn = string.Empty;
            string history_data = string.Empty;
            string strsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.Items.Count > 0)
                {
                    if (ddlsec.SelectedItem.ToString().Trim() != "" && ddlsec.SelectedItem.ToString().Trim() != "-1" && ddlsec.SelectedItem.ToString() != null && ddlsec.SelectedItem.ToString().Trim().ToLower() != "all")
                    {
                        strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                    }
                }
            }
            for (int stafnme = 0; stafnme < gview1.Rows.Count; stafnme++)
            {
                TextBox txtnme = (gview1.Rows[stafnme].FindControl("txt_staffname") as TextBox);
                string naem = txtnme.Text;
                if (naem == "---Select---")
                {
                    lblErrMsg.Text = "Please choose staff";
                    lblErrMsg.Visible = true;
                    return;
                }
            }

            bool invisi = false;
            if (chk_multisubj.Checked == false)
            {
                for (int rowcnt = 0; rowcnt <= gview1.Rows.Count - 1; rowcnt++)
                {
                    string subcode = string.Empty;
                    string txt_staff_name = (gview1.Rows[rowcnt].FindControl("txt_staffname") as TextBox).Text;
                    staff_name_code = txt_staff_name;

                    if (staff_name_code == "" || staff_name_code == " ")//-----------check wether the staff name selected or not
                    {
                        subjtree.Visible = true;

                        gview1.Visible = true;
                        if (ddltimetable.SelectedItem.Text.Trim() == "New")
                        {
                            gridSelTT.Visible = true;
                            bindGridTTSelect();
                        }
                        chkappend.Visible = true;
                        btnOk.Visible = true;
                        treepanel.Visible = true;
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "Select Staff name";
                        return;
                    }
                    else
                    {
                        invisi = true;
                    }
                }
                //------------message for select the subject from the tree
                if (gview1.Rows.Count == 0)
                {
                    subjtree.Visible = true;

                    gview1.Visible = true;
                    chkappend.Visible = true;
                    if (ddltimetable.SelectedItem.Text.Trim() == "New")
                    {
                        gridSelTT.Visible = true;
                        bindGridTTSelect();
                    }
                    chkappend.Visible = true;
                    btnOk.Visible = true;
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Select Subject name for alternate schedule from tree view";
                    return;
                }

                for (int row_cnt = 0; row_cnt <= gview1.Rows.Count - 1; row_cnt++)
                {
                    string txt_staff_name = (gview1.Rows[row_cnt].FindControl("txt_staffname") as TextBox).Text;
                    staff_name_code = txt_staff_name;
                    if (staff_name_code == null)
                    {
                        staff_name_code = string.Empty;
                    }
                    if (staff_name_code.Trim().ToLower() != string.Empty && staff_name_code.Trim().ToLower() != " ")
                    {
                        string str = string.Empty;
                        string[] staff_name_code_spt = staff_name_code.Split('-');
                        if (staff_name_code_spt.GetUpperBound(0) > 0)
                        {
                            staffname = staff_name_code_spt[0].ToString();
                            staffcode = staff_name_code_spt[1].ToString();
                            if (staff_name_code_spt.GetUpperBound(0) > 1)
                            {
                                CheckBoxList chklist = (gview1.Rows[row_cnt].FindControl("cbl_staffname") as CheckBoxList);
                                if (chklist != null)
                                {
                                    for (int ji = 0; ji < chklist.Items.Count; ji++)
                                    {
                                        if (chklist.Items[ji].Selected)
                                        {
                                            if (str == string.Empty)
                                            {
                                                str = chklist.Items[ji].Value;
                                            }
                                            else
                                            {
                                                str = str + "-" + chklist.Items[ji].Value;
                                            }
                                        }
                                    }
                                }
                                staffcode = str;
                            }
                        }
                    }
                    // Added by sridhar 11.07.2014
                    if (allowcom == false)
                    {
                        string[] staffcode_check = staffcode.Split('-');
                        if (activecol != 0 || activerow != -1)
                        {
                            #region magesh 15.8.18

                            int noofHrs = dirAcc.selectScalarInt("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue + "' and semester='" + ddlsem.SelectedValue + "'");
                            if (gridSelTT.Visible)
                            {
                                for (int rowI = 0; rowI < gridSelTT.Rows.Count; rowI++)
                                {
                                    for (int colI = 1; colI <= noofHrs; colI++)
                                    {
                                        DropDownList ddlVal = (DropDownList)gridSelTT.Rows[rowI].FindControl("ddlH" + colI);
                                        if (ddlVal.SelectedIndex == 1)
                                        {
                                            allotcount++;
                                            //int row = Convert.ToInt32(activerow);
                                            //int col = Convert.ToInt32(activecol);
                                            int row = rowI;
                                            int col = colI;
                            #endregion magesh 15.8.18
                                            if (row == 0)
                                            {
                                                tablevalue = "mon" + col + "";
                                            }
                                            else if (row == 1)
                                            {
                                                tablevalue = "tue" + col + "";
                                            }
                                            else if (row == 2)
                                            {
                                                tablevalue = "wed" + col + "";
                                            }
                                            else if (row == 3)
                                            {
                                                tablevalue = "thu" + col + "";
                                            }
                                            else if (row == 4)
                                            {
                                                tablevalue = "fri" + col + "";
                                            }
                                            else if (row == 5)
                                            {
                                                tablevalue = "sat" + col + "";
                                            }
                                            else if (row == 6)//Deepali on 16.4.18
                                            {
                                                tablevalue = "sun" + col + "";
                                            }
                                            //string getRights = d2.GetFunction("select value from Master_Settings where  settings='Time Table Alert Rights'");
                                            //if (getRights.Trim() == "0" || String.IsNullOrEmpty(getRights))
                                            //{
                                            for (int i = 0; i <= staffcode_check.GetUpperBound(0); i++)
                                            {
                                                string staff_code = staffcode_check[i].ToString();
                                                Hashtable hatdegree = new Hashtable();
                                                SqlFinal = " select cc.Course_Name, de.Acronym, r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
                                                SqlFinal = SqlFinal + " subject s,sub_sem sm,syllabus_master sy,seminfo si,Degree de,COURSE cc where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
                                                SqlFinal = SqlFinal + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
                                                SqlFinal = SqlFinal + " and s.subject_no=ss.subject_no and ltrim(rtrim(isnull(r.sections,''))) =ltrim(rtrim(isnull(ss.sections,''))) and ss.batch_year=r.Batch_Year";
                                                SqlFinal = SqlFinal + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
                                                SqlFinal = SqlFinal + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
                                                SqlFinal = SqlFinal + " and r.DelFlag=0 and ss.staff_code='" + staff_code + "' and de.Degree_Code=si.degree_code and de.Course_Id=cc.Course_Id and '" + fromdate + "' between si.start_date and si.end_date";
                                                srids.Clear();
                                                srids = srida.select_method_wo_parameter(SqlFinal, "Text");

                                                if (srids.Tables.Count > 0 && srids.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int j = 0; j < srids.Tables[0].Rows.Count; j++)
                                                    {
                                                        btch = srids.Tables[0].Rows[j]["batch_year"].ToString();
                                                        string dgre = srids.Tables[0].Rows[j]["degree_code"].ToString();
                                                        ster = srids.Tables[0].Rows[j]["semester"].ToString();
                                                        sctn = srids.Tables[0].Rows[j]["Sections"].ToString();
                                                        acrnym = srids.Tables[0].Rows[j]["Acronym"].ToString();
                                                        coursename = srids.Tables[0].Rows[j]["Course_Name"].ToString();
                                                        if (!hatdegree.ContainsKey(btch + '-' + dgre + '-' + ster + '-' + sctn))
                                                        {
                                                            hatdegree.Add(btch + '-' + dgre + '-' + ster + '-' + sctn, btch + '-' + dgre + '-' + ster + '-' + sctn);
                                                            // loadstaffinfo();
                                                            // string slq = "select * from Semester_Schedule where batch_year='" + btch + "' and semester ='" + ster + "' and degree_code='" + dgre + "' and Sections='" + sctn + "' and " + tablevalue + " like '%" + staff_code + "%' and FromDate = '" + fromdate + "' ";
                                                            string slq = "select top 1 * from Semester_Schedule where batch_year='" + btch + "' and semester ='" + ster + "' and degree_code='" + dgre + "' and Sections='" + sctn + "' and FromDate <= '" + fromdate + "' ORDER BY FromDate desc";
                                                            string rept = string.Empty;
                                                            ds.Clear();
                                                            ds = srida.select_method_wo_parameter(slq, "Text");
                                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                string strsetval = "" + tablevalue + " like '%" + staff_code + "%'";
                                                                ds.Tables[0].DefaultView.RowFilter = strsetval;
                                                                DataView dvfils = ds.Tables[0].DefaultView;
                                                                if (dvfils.Count > 0)
                                                                {
                                                                    #region magesh 15.8.18
                                                                    hasrow++;
                                                                    hascol++;
                                                                    string sess = Convert.ToString(Session["rows"]);
                                                                    string colcc = Convert.ToString(Session["col"]);
                                                                    if (!allotrow.ContainsValue(row))
                                                                    {
                                                                        allotrow.Add(hasrow, row);
                                                                        if (Convert.ToString(Session["rows"]) == "")
                                                                        {
                                                                            Session["rows"] = row;
                                                                            Session["col"] = col;
                                                                        }
                                                                        else
                                                                        {
                                                                            Session["rows"] = Convert.ToString(Session["rows"]) + ',' + row;
                                                                            Session["col"] = Convert.ToString(Session["col"]) + ',' + col;
                                                                        }
                                                                        allotcol.Add(hascol, col);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (allotrow.ContainsValue(row) && !allotcol.ContainsValue(col))
                                                                        {
                                                                            if (Convert.ToString(Session["rows"]) == "")
                                                                            {
                                                                                Session["rows"] = row;
                                                                                Session["col"] = col;
                                                                            }
                                                                            else
                                                                            {
                                                                                Session["rows"] = Convert.ToString(Session["rows"]) + ',' + row;
                                                                                Session["col"] = Convert.ToString(Session["col"]) + ',' + col;
                                                                            }
                                                                            allotrow.Add(hasrow, row);
                                                                            allotcol.Add(hascol, col);
                                                                        }
                                                                    }
                                                                    #endregion magesh 15.8.18
                                                                    if (history_data == "")
                                                                    {
                                                                        if (ster == "1")
                                                                        {
                                                                            history_data = tablevalue + "-" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "st Sem";
                                                                            if (sctn != null && sctn != "")
                                                                            {
                                                                                history_data = history_data + "-" + sctn + "  Sec";
                                                                            }
                                                                        }
                                                                        if (ster == "2")
                                                                        {
                                                                            history_data = tablevalue + "-" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "nd Sem";
                                                                            if (sctn != null && sctn != "")
                                                                            {
                                                                                history_data = history_data + "-" + sctn + "  Sec";
                                                                            }
                                                                        }
                                                                        if (ster == "3")
                                                                        {
                                                                            history_data = tablevalue + "-" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "rd Sem";
                                                                            if (sctn != null && sctn != "")
                                                                            {
                                                                                history_data = history_data + "-" + sctn + "  Sec";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            history_data = tablevalue + "-" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "th Sem";
                                                                            if (sctn != null && sctn != "")
                                                                            {
                                                                                history_data = history_data + "-" + sctn + "  Sec";
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ster == "1")
                                                                        {
                                                                            history_data = history_data + " ; " + tablevalue + "-" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "st Sem";
                                                                            if (sctn != null && sctn != "")
                                                                            {
                                                                                history_data = history_data + "-" + sctn + "  Sec";
                                                                            }
                                                                        }
                                                                        else if (ster == "2")
                                                                        {
                                                                            history_data = history_data + " ; " + tablevalue + "-" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "nd Sem";
                                                                            if (sctn != null && sctn != "")
                                                                            {
                                                                                history_data = history_data + "-" + sctn + "-Sec";
                                                                            }
                                                                        }
                                                                        else if (ster == "3")
                                                                        {
                                                                            history_data = history_data + " ; " + tablevalue + "-" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "rd Sem";
                                                                            if (sctn != null && sctn != "")
                                                                            {
                                                                                history_data = history_data + "-" + sctn + "  Sec";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            history_data = history_data + " ; " + tablevalue + "-" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "th Sem";
                                                                            if (sctn != null && sctn != "")
                                                                            {
                                                                                history_data = "-" + history_data + "-" + sctn + "  Sec";
                                                                            }
                                                                        }
                                                                        //history_data = history_data + ";" + btch + "-" + coursename + "-" + acrnym + "-" + ster + "th Sem -" + sctn + "Sec";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (history_data != "")
                    {
                        cnfrm = true;
                        string getRights = d2.GetFunction("select value from Master_Settings where  settings='Time Table Alert Rights'");
                        if (getRights.Trim() == "0" || String.IsNullOrEmpty(getRights))
                        {

                            allowmuliallot = true;
                            //btnupOk_Click(sender, e);
                            mpemsgboxupdate.Show();
                            Label1.Text = "The Staff " + staffname + " is BUSY in " + history_data + " - Do you want to Schedule the Class Anyway?";
                            Label1.Visible = true;
                            spcellClickPopup.Visible = false;
                        }
                        else
                        {
                            mpemsgboxupdate.Hide();
                            btnupOk_Click(sender, e);
                            //allowcom = true;
                            //btnOk_Click(sender, e);
                        }
                        //  goto golabel;//magesh 15.8.18
                    }
                    //............................................................................................................................

                    subj_number = (gview1.Rows[row_cnt].FindControl("lblsubtag") as Label).Text;
                    if (splval == "")
                    {
                        string val = "S";
                        string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number.ToString() + "'");
                        if (subj_type == "1" || subj_type.ToLower().Trim() == "true")
                        {
                            val = "L";
                        }
                        if (allowcom == true)
                        {
                            val = "C";
                        }
                        splval = (d2.GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        subno_staff = subj_number + "-" + staffcode + "-" + val + "";
                        subno_staffnote = (d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                    }
                    else
                    {
                        string val = "S";
                        string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number.ToString() + "'");
                        if (subj_type == "1" || subj_type.Trim().ToLower() == "true")
                        {
                            val = "L";
                        }
                        if (allowcom == true)
                        {
                            val = "C";
                        }
                        splval = splval + ";" + (d2.GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        subno_staff = subno_staff + ";" + subj_number + "-" + staffcode + "-" + val + "";
                        subno_staffnote = subno_staffnote + ";" + (d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                    }

                }
            }
            else
            {
                for (int row_cnt = 0; row_cnt <= gview1.Rows.Count - 1; row_cnt++)
                {
                    for (int chk_cnt = 0; chk_cnt < chklistmultisubj.Items.Count; chk_cnt++)
                    {
                        if (chklistmultisubj.Items[chk_cnt].Selected == true)
                        {
                            staff_name_code = chklistmultisubj.Items[chk_cnt].Text;
                            string[] staff_name_code_spt = staff_name_code.Split('-');
                            staffname = staff_name_code_spt[0].ToString();
                            staffcode = staff_name_code_spt[1].ToString();

                            subj_number = (gview1.Rows[row_cnt].FindControl("lblsubtag") as Label).Text;

                            if (splval_temp == "")
                            {
                                splval_temp = staffcode;// subj_number + "-" + staffcode + "-S";
                            }
                            else
                            {
                                splval_temp = splval_temp + "-" + staffcode;// subj_number + "-" + staffcode + "-S";
                            }
                        }
                    }
                    if (splval == "")
                    {
                        string val = "S";
                        string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number.ToString() + "'");
                        if (subj_type == "1" || subj_type.Trim().ToLower() == "true")
                        {
                            val = "L";
                        }
                        if (allowcom == true)
                        {
                            val = "C";
                        }
                        splval = (d2.GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        subno_staff = subj_number + "-" + splval_temp + "-" + val + "";
                        subno_staffnote = d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "";
                    }
                    else
                    {
                        string val = "S";
                        string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subj_number.ToString() + "'");
                        if (subj_type == "1" || subj_type.Trim().ToLower() == "true")
                        {
                            val = "L";
                        }
                        if (allowcom == true)
                        {
                            val = "C";
                        }
                        splval = splval + ";" + (d2.GetFunction("select subject_name from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                        subno_staff = subno_staff + ";" + subj_number + "-" + splval_temp + "-" + val + "";
                        subno_staffnote = subno_staffnote + ";" + (d2.GetFunction("select  case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no=" + subj_number.ToString() + " ") + "-" + staffcode + "-" + "" + val + "");
                    }
                }
            }


            int x = Convert.ToInt32(Session["actrow"]); //rowIndex;
            int y = Convert.ToInt32(Session["actcol"]); //selectedCellIndex;

            DataTable dttablente = (DataTable)ViewState["table"];
            DataTable dttabletxt = (DataTable)ViewState["table1"];

            if (y > 0)
            {

                int noofHrs = dirAcc.selectScalarInt("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue + "' and semester='" + ddlsem.SelectedValue + "'");
                if (gridSelTT.Visible)
                {
                    for (int rowI = 0; rowI < gridSelTT.Rows.Count; rowI++)
                    {
                        for (int colI = 1; colI <= noofHrs; colI++)
                        {
                            DropDownList ddlVal = (DropDownList)gridSelTT.Rows[rowI].FindControl("ddlH" + colI);
                            if (ddlVal.SelectedIndex == 1)
                            {
                                #region magesh 15.8.18
                                if (!allotrow.ContainsValue(rowI))
                                {
                                    if (gview.Rows.Count > rowI)
                                    {

                                        dttablente.Rows[rowI + 2][colI] = subno_staff.ToString();
                                        dttabletxt.Rows[rowI + 2][colI] = subno_staffnote.ToString();
                                    }
                                }
                                else
                                {
                                    if (allotrow.ContainsValue(rowI) && allotcol.ContainsValue(colI))
                                    {

                                        if (gview.Rows.Count > rowI)
                                        {

                                            dttablente.Rows[rowI + 2][colI] = subno_staff.ToString();
                                            dttabletxt.Rows[rowI + 2][colI] = subno_staffnote.ToString();

                                        }
                                    }
                                }
                                #endregion magesh 15.8.18
                            }
                        }
                    }
                }
                else
                {
                    #region magesh 15.8.18
                    string rowsession = string.Empty;
                    string colsession = string.Empty;
                    if (Convert.ToString(Session["rows"]) != "")
                    {
                        rowsession = Convert.ToString(Session["rows"]);
                        colsession = Convert.ToString(Session["col"]);

                        if (rowsession != "")
                        {
                            string[] sesshour = rowsession.Split(',');
                            string[] sesshour1 = colsession.Split(',');

                            if (sesshour.Length > 0)
                            {
                                for (int sess = 0; sess < sesshour.Count(); sess++)
                                {

                                    dttablente.Rows[Convert.ToInt32(sesshour[sess]) + 2][Convert.ToInt32(sesshour1[sess])] = subno_staff.ToString();
                                    dttabletxt.Rows[Convert.ToInt32(sesshour[sess]) + 2][Convert.ToInt32(sesshour1[sess])] = subno_staffnote.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        dttabletxt.Rows[x][y] = subno_staffnote.ToString();
                        dttablente.Rows[x][y] = subno_staff.ToString();
                    }
                    #endregion magesh 15.8.18
                }
                ViewState["table"] = dttablente;
                ViewState["table1"] = dttabletxt;
                gview.DataSource = dttabletxt;
                gview.DataBind();
                gview.Visible = true;
            }
            if (invisi)
            {
                //else
                //    {
                btnsave.Enabled = true;
                // }
            }
        golabel: ;
            //Modified By srinath 25/8/2015 For KCG======
            if (ddltimetable.SelectedItem.ToString().Trim() != "New" && allotcount != 0)
            {
                btnsave_Click(sender, e);
            }
            if (allotcount != 0)//ddltimetable.SelectedItem.ToString().Trim() != "New" &&
            {
                subjtree.Visible = false;
                gview1.Visible = false;
                gridSelTT.Visible = false;
                chkappend.Visible = false;
                btnOk.Visible = false;
                treepanel.Visible = false;
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;


            }
            else
            {
                spcellClickPopup.Visible = true;
                lblErrMsg.Text = "Please Select Any Hour";
                lblErrMsg.Visible = true;
            }
            //===================
            //magesh 5.9.18
            if (allowmuliallot == false)
            {
                Session["rows"] = "";
                Session["col"] = "";

            } //magesh 5.9.18
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    //protected void btnuupdetails_Click(object sender, EventArgs e)
    //{
    //    fp1.Visible = true;
    //    pnlsp1.Visible = true;
    //   // loadstaffinfo();
    //}

    protected void btnupOk_Click(object sender, EventArgs e)
    {
        mpemsgboxupdate.Hide();
        try
        {
            allowcom = true;
            btnOk_Click(sender, e);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    //protected void exitpop_Click1(object sender, EventArgs e)
    //{
    //    pnlsp1.Visible = false;
    //}

    protected void btnupCancel_Click(object sender, EventArgs e)
    {
        try
        {
            mpemsgboxupdate.Hide();
            return;
        }
        catch
        {
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string[] date = txtdate.Text.Split('/');
            string fromdate = date[1] + '/' + date[0] + '/' + date[2];
            errmsg.Visible = false;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string strsec = string.Empty;
            string section = string.Empty;
            string ttname = string.Empty;
            btndelete.Enabled = false;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() != "" && ddlsec.SelectedItem.ToString() != "-1" && ddlsec.SelectedItem.ToString().ToLower().Trim() != "all" && ddlsec.SelectedItem.ToString() != null)
                {
                    section = ddlsec.SelectedItem.ToString();
                    strsec = "and sections='" + ddlsec.SelectedItem.ToString() + "'";
                }
            }
            string batch = ddlbatch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedValue.ToString();
            string sem = ddlsem.SelectedValue.ToString();
            if (ddltimetable.SelectedItem.ToString() == "New")
            {
                ttname = txttimetable.Text;
            }
            else
            {
                ttname = ddltimetable.SelectedItem.ToString();
            }
            string Classadvisor = string.Empty;
            if (ttname == "" || ttname.ToLower() == "new")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter Time Table Name";
                return;
            }
            string strtimetable = "Select DISTINCT TTname from semester_schedule where batch_year=" + ddlbatch.Text.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " " + strsec + " and ttname='" + ttname + "'";
            ds = d2.select_method(strtimetable, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataSet dstrigger = new DataSet();
                string trigger = "select * from sysobjects where name='TR_Semester_Schudule' and Type='TR'";
                dstrigger = d2.select_method(trigger, hat, "Text");
                if (dstrigger.Tables[0].Rows.Count > 0)
                {
                    con.Close();
                    con.Open();
                    SqlCommand sqlcmd = new SqlCommand("drop trigger TR_Semester_Schudule", con);
                    SqlDataReader dr = sqlcmd.ExecuteReader();
                }
                con.Close();
                con.Open();
                SqlCommand sqlcmd1 = new SqlCommand("Delete from semester_schedule where batch_year=" + ddlbatch.Text.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " " + strsec + " and ttname='" + ttname + "'", con);
                SqlDataReader dr1 = sqlcmd1.ExecuteReader();
                trigger = "select * from sysobjects where name='TR_Semester_Schudule' and Type='TR'";
                dstrigger = d2.select_method(trigger, hat, "Text");
                if (dstrigger.Tables[0].Rows.Count == 0)
                {
                    con.Close();
                    con.Open();
                    SqlCommand sqlcmd = new SqlCommand("Create TRIGGER TR_Semester_Schudule On Semester_Schedule For Delete AS BEGIN Insert Into Semester_Schedule select * FROM deleted ins End", con);
                    SqlDataReader dr = sqlcmd.ExecuteReader();
                }
            }
            string holiday = d2.GetFunction("Select holiday from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + "");
            string Daycoulmn = string.Empty;
            string Daycoulmnvalue = string.Empty;
            int noofdays = Convert.ToInt32(d2.GetFunction("Select nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + ""));
            int day = 0;
            //for (int day1 = 0; day1 < 6; day1++)
            DataTable dtnote = (DataTable)ViewState["table"];
            for (int day1 = 0; day1 < noofdays; day1++) //Deepali 16.4.18
            {
                int row = day + 2;
                string dayofweek = Days[day1];
                int daysetweek = day1 + 2;
                //if (day1 == 6)
                if (day1 == noofdays)//Deepali 16.4.18
                {
                    daysetweek = 1;
                }
                if (!holiday.Contains(daysetweek.ToString()))
                {

                    for (int j = 1; j < dtnote.Columns.Count; j++)
                    {
                        if (Daycoulmn == "")
                        {
                            Daycoulmn = dayofweek + j;

                            string value = dtnote.Rows[row][j].ToString();
                            Daycoulmnvalue = "'" + value + "'";
                        }
                        else
                        {
                            Daycoulmn = Daycoulmn + ',' + dayofweek + j;

                            string value = dtnote.Rows[row][j].ToString();
                            Daycoulmnvalue = Daycoulmnvalue + ',' + "'" + value + "'";
                        }
                    }
                    Classadvisor = string.Empty;

                    for (int i = 0; i < gviewclassadv.Rows.Count; i++)
                    {
                        string[] spiltcheck = Classadvisor.Split(',');
                        Boolean chevalflag = false;
                        for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                        {
                            string str = (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;

                            if (str == spiltcheck[ch].ToString())
                            {
                                chevalflag = true;
                            }
                        }
                        if (chevalflag == false)
                        {
                            if (Classadvisor == "")
                            {

                                Classadvisor = (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;
                            }
                            else
                            {

                                Classadvisor = Classadvisor + ',' + (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;
                            }
                        }
                    }
                    day++;
                }
            }
            string inserquery = "insert into Semester_Schedule (class_advisor,degree_code,batch_year,semester,sections,TTName,FromDate," + Daycoulmn + ",lastrec) values('" + Classadvisor + "'," + degree + "," + batch + "," + sem + ",'" + section + "','" + ttname + "','" + fromdate + "'," + Daycoulmnvalue + ",1)";
            int a = d2.insert_method(inserquery, hat, "Text");
            txttimetable.Visible = false;
            btngo_Click(sender, e);

            if (gviewclassadv.Rows.Count > 0)
            {
                gviewclassadv.Visible = true;
            }
            btnclassadvisor.Visible = true;
            if (!cnfrm)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            }
            btnsave.Enabled = false;
            btnsave.Visible = true;
            btndelete.Enabled = false;
            if (ddltimetable.SelectedItem.ToString() == "New")
            {
                ddlsem_SelectedIndexChanged(sender, e);
                gview.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    public string findday(string curday, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = curday.Split(new Char[] { '/' });
            string cur_date = sp_date[1].ToString() + "-" + sp_date[0].ToString() + "-" + sp_date[2].ToString();
            DateTime dt1 = Convert.ToDateTime(sdate);
            DateTime dt2 = Convert.ToDateTime(cur_date);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";
            string holday = d2.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            if (stastdayorder.ToString().Trim() != "")
            {
                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                {
                    order = order + (Convert.ToInt32(stastdayorder) - 1);
                    if (order == (nodays + 1))
                        order = 1;
                    else if (order > nodays)
                        order = order % nodays;
                }
            }
            string findday = string.Empty;
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            return findday;
        }
        else
            return "";
    }

    public void BindCollege()
    {
        string collquery = "select collname,college_code from collinfo";
        ds = d2.select_method(collquery, hat, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
    }

    public void loadstaffdep()
    {
        try
        {
            string staffquery = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            ds = d2.select_method(staffquery, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldepratstaff.DataSource = ds;
                ddldepratstaff.DataTextField = "dept_name";
                ddldepratstaff.DataValueField = "dept_code";
                ddldepratstaff.DataBind();
                ddldepratstaff.Items.Insert(0, "All");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    protected void btnclassadvisor_Click(object sender, EventArgs e)
    {
        panelstaff.Visible = true;

        gviewstaffdetail.Visible = true;
        BindCollege();
        loadstaffdep();
        loadfsstaff();
    }

    protected void loadfsstaff()
    {
        try
        {
            string sql = string.Empty;
            if (ddldepratstaff.SelectedItem.ToString() == "All")
            {
                if (txt_search.Text != "")
                {
                    if (ddlstaff.SelectedIndex == 0)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                    else if (ddlstaff.SelectedIndex == 1)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                }
                else
                {
                    sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "')";
                }
            }
            else
            {
                if (txt_search.Text != "")
                {
                    if (ddlstaff.SelectedIndex == 0)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                    else if (ddlstaff.SelectedIndex == 1)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                }
                else
                {
                    sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
            }


            dtstaffdet.Columns.Add("sno");
            dtstaffdet.Columns.Add("stfnme");
            dtstaffdet.Columns.Add("stfcode");

            ds = d2.select_method(sql, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    drstaffdet = dtstaffdet.NewRow();
                    sno++;
                    string name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    string code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();


                    drstaffdet["sno"] = Convert.ToString(sno);
                    drstaffdet["stfnme"] = name;
                    drstaffdet["stfcode"] = code;

                    dtstaffdet.Rows.Add(drstaffdet);
                }


                gviewstaffdetail.DataSource = dtstaffdet;
                gviewstaffdetail.DataBind();
                gviewstaffdetail.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {

            string classadvisor = string.Empty;


            for (int i = 0; i < gviewclassadv.Rows.Count; i++)
            {
                string[] spiltcheck = classadvisor.Split(',');
                Boolean chevalflag = false;
                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                {

                    string st = (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;
                    if (st == spiltcheck[ch].ToString())
                    {
                        chevalflag = true;
                    }
                }
                if (chevalflag == false)
                {
                    if (classadvisor == "")
                    {

                        classadvisor = (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;
                    }
                    else
                    {

                        classadvisor = classadvisor + ',' + (gviewclassadv.Rows[i].FindControl("lblclsstfcode") as Label).Text;
                    }
                }
            }
            string Staffcode = string.Empty;

            DataTable dtadv = (DataTable)ViewState["dtadvisor"];

            if (dtadv.Columns.Count > 0)
            { }
            else
            {
                dtadv.Columns.Add("stfnme");
                dtadv.Columns.Add("stfcode");
            }


            for (int rolcount = 0; rolcount < gviewstaffdetail.Rows.Count; rolcount++)
            {
                string[] spiltcheck = classadvisor.Split(',');
                Boolean chevalflag = false;
                for (int ch = 0; ch <= spiltcheck.GetUpperBound(0); ch++)
                {

                    string str = (gviewstaffdetail.Rows[rolcount].FindControl("lblstfcode") as Label).Text;
                    if (str == spiltcheck[ch].ToString())
                    {
                        chevalflag = true;
                    }
                }
                if (chevalflag == false)
                {
                    CheckBox chk = (CheckBox)gviewstaffdetail.Rows[rolcount].FindControl("chk");

                    if (chk.Checked)
                    {
                        dradvisor = dtadv.NewRow();



                        //dradvisor["sno"] = "1";
                        dradvisor["stfnme"] = (gviewstaffdetail.Rows[rolcount].FindControl("lblstafnme") as Label).Text;
                        dradvisor["stfcode"] = (gviewstaffdetail.Rows[rolcount].FindControl("lblstfcode") as Label).Text;

                        dtadv.Rows.Add(dradvisor);
                    }
                }
            }
            ViewState["dtadvisor"] = dtadv;
            gviewclassadv.DataSource = dtadv;
            gviewclassadv.DataBind();
            gviewclassadv.Visible = true;
            classadvisorsave();
            Session["ClassAdvisor"] = Staffcode;
            panelstaff.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void exitpop_Click(object sender, EventArgs e)
    {
        panelstaff.Visible = false;
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadstaffdep();
        gview.Visible = false;
        btnsave.Visible = false;
        btnclassadvisor.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnexcel.Visible = false;
        btnPDF.Visible = false;
    }

    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
        gview.Visible = false;
        btnsave.Visible = false;
        btnclassadvisor.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnexcel.Visible = false;
        btnPDF.Visible = false;
    }

    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
        gview.Visible = false;
        btnsave.Visible = false;
        btnclassadvisor.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnexcel.Visible = false;
        btnPDF.Visible = false;
    }

    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        //string Batch = ddlbatch.SelectedItem.ToString();
        //string Degree = ddlbranch.SelectedValue.ToString();
        //string Sem = ddlsem.SelectedItem.ToString();
        //string ttname = ddltimetable.SelectedItem.ToString();
        //string sec =string.Empty;
        //if (ddlsec.Enabled == true )
        //{
        //    if (ddlsec.SelectedItem.ToString() != "All")
        //    {
        //        sec = " and Sections='" + ddlsec.SelectedItem.ToString() + "'";
        //    }
        //}
        //if (ttname.ToLower() != "new" && Fptimetable.Visible == true)
        //{
        //    string deletquery = "Delete from Semester_Schedule where batch_year=" + Batch + " and degree_code=" + Degree + " and semester=" + Sem + " and ttname='" + ttname + "' " + sec + "";
        //    int a = d2.insert_method(deletquery, hat, "Text");
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Deleted successfully')", true);
        //}
        //string activerow = Fptimetable.ActiveSheetView.ActiveRow.ToString();
        //string activecol = Fptimetable.ActiveSheetView.ActiveColumn.ToString();

        rowIndex = (int)Session["actrow"];
        selectedCellIndex = (int)Session["actcol"];
        if (selectedCellIndex != 0 && rowIndex != -1)
        {
            btnsave.Enabled = true;
            errmsg.Visible = false;
            lblday.Visible = false;
            lblday1.Visible = false;
            lbltimings.Visible = false;
            lblfromtime.Visible = false;
            lbltotime.Visible = false;
            treepanel.Visible = false;
            btnsave.Visible = true;
            btnsave.Enabled = true;
            txtmultisubj.Enabled = false;
            treepanel.Visible = false;
            txttimetable.Visible = false;
            panelstaff.Visible = false;

            gviewstaff.Visible = false;
            btndelete.Enabled = false;
        }
    }

    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        btnsave.Enabled = true;
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {

                d2.printexcelreportgrid(gview, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        GeneratePDF();
    }

    List<string> str_arr = new List<string>();
    Font table_cell = new Font("Book Antiqua", 11, FontStyle.Regular);
    Font table_col = new Font("Book Antiqua", 12, FontStyle.Bold);
    Font FontboldNew = new Font("Book Antiqua", 15, FontStyle.Bold);

    public void GeneratePDF()
    {
        try
        {
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string[] datespilt = txtdate.Text.Split('/');
            string temp_date = datespilt[1] + '/' + datespilt[0] + '/' + datespilt[2];
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = "and sections='" + strsec + "'";
            }
            else
            {
                strsec = string.Empty;
            }
            Font Fontbold = new Font("Book Antiqua", 14, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font Fontbold1 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            string[] shortday = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] days = new string[7] { "Monday", "Tuesday", "wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            int noofhours = Convert.ToInt32(Session["totalhrs"]);
            int noofdays = Convert.ToInt32(Session["totnoofdays"]);
            int dayorder = Convert.ToInt32(Session["dayorder"]);
            DataTable dt = new DataTable();
            DataColumn dc;
            DataRow dr;
            DataTable dt1 = new DataTable();
            DataColumn dc1;
            DataRow dr1;
            dc = new DataColumn("Day");
            // dc1 = new DataColumn("Day");
            dt.Columns.Add(dc);
            //  dt1.Columns.Add(dc1);
            for (int i = 1; i <= noofhours; i++)
            {
                dc = new DataColumn(i.ToString());
                //  dc1 = new DataColumn(i.ToString());

                dt.Columns.Add(dc);
                //  dt1.Columns.Add(dc1);
            }
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dr["Day"] = "Timings";

            // dr1 = dt1.NewRow();
            // dt1.Rows.Add(dr1);
            // dr1["Day"] = "Timings";
            string dayvalue = string.Empty;
            int date = 0;
            string holiday = d2.GetFunction("Select holiday from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + "");
            for (int day1 = 0; day1 < 6; day1++)
            {
                string dayofweek = shortday[day1];
                string dayofweek1 = days[day1];
                int daysetweek = day1 + 2;
                if (day1 == 6)
                {
                    daysetweek = 1;
                }
                if (!holiday.Contains(daysetweek.ToString()))
                {
                    dr = dt.NewRow();
                    //  dr1 = dt1.NewRow();
                    dt.Rows.Add(dr);
                    //  dt1.Rows.Add(dr1);
                    if (dayorder == 1)
                    {
                        dr["Day"] = dayofweek1;
                        // dr1["Day"] = dayofweek1;
                    }
                    else
                    {
                        date = date + 1;
                        dr["Day"] = "Day " + date;
                        // dr1["Day"] = "Day " + date;
                    }
                    for (int i = 1; i <= noofhours; i++)
                    {
                        if (dayvalue == "")
                        {
                            dayvalue = dayofweek + i;
                        }
                        else
                        {
                            dayvalue = dayvalue + ',' + dayofweek + i;
                        }
                    }
                }
            }
            //added by srinath 8/1/2014
            //added by srinath 10/6/2014
            for (int i = 1; i <= noofhours; i++)
            {
                string belltime = string.Empty;
                // string strtimequery = "Select start_time,end_time from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i + "'";
                //ds = d2.select_method(strtimequery, hat, "Text");
                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    if (ds.Tables[0].Rows[0]["start_time"].ToString() != "" && ds.Tables[0].Rows[0]["start_time"].ToString() != null && ds.Tables[0].Rows[0]["end_time"].ToString() != "" && ds.Tables[0].Rows[0]["end_time"].ToString() != null)
                //    {
                //        string[] spiltstarttime = ds.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                //        string[] spiltendtime = ds.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                //        belltime = spiltstarttime[1].ToString() + ' ' + spiltstarttime[2].ToString() + ' ' + " To " + spiltendtime[1].ToString() + ' ' + spiltendtime[2].ToString();
                //    }
                string strtimequery = "Select RIGHT(CONVERT(VARCHAR, start_time, 100),7) as sTime,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as endtime from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i + "'";
                ds = d2.select_method(strtimequery, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string starttime = ds.Tables[0].Rows[0]["sTime"].ToString();
                    string endtime = ds.Tables[0].Rows[0]["endtime"].ToString();
                    if (starttime != null && starttime.Trim() != "" && endtime != null && endtime.Trim() != "")
                    {
                        belltime = starttime + " To " + endtime;
                    }
                }
                dt.Rows[0][i] = belltime;
                // dt1.Rows[0][i] = belltime;
            }
            string schedukle = "select  top 1 " + dayvalue + " from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + " and batch_year = " + ddlbatch.SelectedItem.ToString() + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc";
            ds = d2.select_method(schedukle, hat, "Text");
            int cout = 0;
            int cout1 = 0;
            int cout2 = 0;
            int countdt = 0;
            int dtr = 0;
            int d = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    cout2 = cout2 + cout1;
                    cout1 = 0;
                    if (cout2 >= 45)
                    {
                        dtr++;
                    }
                    DataTable notetable = (DataTable)ViewState["table"];

                    string value = notetable.Rows[i + 2][0].ToString();
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        cout = 0;
                        string dsvalue = value + j;
                        string classhour = ds.Tables[0].Rows[0]["" + dsvalue + ""].ToString();
                        string setclasshour = string.Empty;
                        if (classhour.Trim() != "" && classhour.Trim() != "0" && classhour != null)
                        {
                            string[] spiltmulpl = classhour.Split(';');
                            for (int mul = 0; mul <= spiltmulpl.GetUpperBound(0); mul++)
                            {
                                string[] spiltclasshour = spiltmulpl[mul].Split('-');
                                for (int sp = 0; sp <= spiltclasshour.GetUpperBound(0); sp++)
                                {
                                    if (sp == 0)
                                    {
                                        if (setclasshour == "")
                                        {
                                            try
                                            {
                                                cout++;
                                                setclasshour = d2.GetFunction("select case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no='" + spiltclasshour[sp].ToString() + "'"); // Modify by jairam 24-07-2017
                                                if (!str_arr.Contains(spiltclasshour[sp].ToString()))
                                                {
                                                    str_arr.Add(spiltclasshour[sp].ToString());
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
                                            }
                                        }
                                        else
                                        {
                                            cout++;
                                            setclasshour = setclasshour + "; " + d2.GetFunction("select case when isnull(acronym,'')='' then subject_code else acronym end from subject where subject_no='" + spiltclasshour[sp].ToString() + "'"); // Modify by jairam 24-07-2017
                                            if (!str_arr.Contains(spiltclasshour[sp].ToString()))
                                            {
                                                str_arr.Add(spiltclasshour[sp].ToString());
                                            }
                                        }
                                        //dt.Rows[i + 1][j] = setclasshour;
                                        break;
                                    }

                                }
                                for (int cc = 0; cc < spiltclasshour.Length; cc++)
                                {
                                    string va = Convert.ToString(spiltclasshour[cc]);
                                    if (!hatHr.ContainsKey(spiltclasshour[0].ToString() + "-" + va))
                                    {
                                        hatHr.Add(spiltclasshour[0].ToString() + "-" + va, 1);
                                    }
                                    else
                                    {
                                        int mark = Convert.ToInt32(hatHr[spiltclasshour[0].ToString() + "-" + va]);
                                        mark = mark + 1;
                                        hatHr[spiltclasshour[0].ToString() + "-" + va] = mark;
                                    }
                                }

                            }
                        }
                        if (cout >= 15)
                        {
                            cout1 = cout;
                        }
                        if (cout2 >= 45)
                        {
                            if (countdt == 0)
                            {
                                countdt++;


                                dc1 = new DataColumn("Day");
                                dt1.Columns.Add(dc1);
                                for (int i1 = 1; i1 <= noofhours; i1++)
                                {
                                    dc1 = new DataColumn(i1.ToString());
                                    dt1.Columns.Add(dc1);
                                }


                                dr1 = dt1.NewRow();
                                dt1.Rows.Add(dr1);
                                dr1["Day"] = "Timings";
                                string dayvalue1 = string.Empty;
                                int date1 = i;
                                //  dtr = i + 1;
                                string holiday1 = d2.GetFunction("Select holiday from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedValue.ToString() + "");
                                for (int day1 = i; day1 < 6; day1++)
                                {
                                    string dayofweek = shortday[day1];
                                    string dayofweek1 = days[day1];
                                    int daysetweek = day1 + 2;
                                    if (day1 == 6)
                                    {
                                        daysetweek = 1;
                                    }
                                    if (!holiday1.Contains(daysetweek.ToString()))
                                    {
                                        dr1 = dt1.NewRow();
                                        dt1.Rows.Add(dr1);
                                        if (dayorder == 1)
                                        {
                                            dr1["Day"] = dayofweek1;
                                        }
                                        else
                                        {
                                            date1 = date1 + 1;
                                            dr1["Day"] = "Day " + date1;
                                        }
                                        for (int i2 = 1; i2 <= noofhours; i2++)
                                        {
                                            if (dayvalue1 == "")
                                            {
                                                dayvalue1 = dayofweek + i2;
                                            }
                                            else
                                            {
                                                dayvalue1 = dayvalue1 + ',' + dayofweek + i2;
                                            }
                                        }
                                    }
                                }
                                for (int i3 = 1; i3 <= noofhours; i3++)
                                {
                                    string belltime = string.Empty;

                                    string strtimequery = "Select RIGHT(CONVERT(VARCHAR, start_time, 100),7) as sTime,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as endtime from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + i3 + "'";
                                    DataSet dstim = new DataSet();
                                    dstim = d2.select_method(strtimequery, hat, "Text");
                                    if (dstim.Tables.Count > 0 && dstim.Tables[0].Rows.Count > 0)
                                    {
                                        string starttime = dstim.Tables[0].Rows[0]["sTime"].ToString();
                                        string endtime = dstim.Tables[0].Rows[0]["endtime"].ToString();
                                        if (starttime != null && starttime.Trim() != "" && endtime != null && endtime.Trim() != "")
                                        {
                                            belltime = starttime + " To " + endtime;
                                        }
                                    }
                                    dt1.Rows[0][i3] = belltime;
                                }
                                d = i + 1;

                            }


                            dt1.Rows[dtr][j] = setclasshour;
                            // dtr++;
                        }
                        else
                        {

                            dt.Rows[i + 1][j] = setclasshour;
                        }
                    }
                }
                if (cout2 >= 45)
                {
                    int rw = dt.Rows.Count;
                    for (int m = d; m < rw; m++)
                    {
                        dt.Rows.RemoveAt(m);
                        rw--;
                        m--;
                    }
                }
                btnsave.Visible = true;
                btndelete.Visible = false;
                gviewclassadv.Visible = false;

            }
            bindall(mydoc, Fontsmall, Fontbold, Fontbold1, dt, dt1, str_arr);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    public void bindall(Gios.Pdf.PdfDocument mydoc, Font Fontsmall, Font Fontbold, Font Fontbold1, DataTable dt_dummyno, DataTable dt_dummyno1, List<string> str_arr)
    {
        try
        {

            Hashtable hat_select_method = new Hashtable();
            //MyImg mi = new MyImg();
            //mi.ImageUrl = "~/images/10BIT001.jpeg";
            //mi.ImageUrl = "Handler/Handler2.ashx?";
            //MyImg1 mi2 = new MyImg1();
            //mi2.ImageUrl = "~/images/10BIT001.jpeg";
            //mi2.ImageUrl = "Handler/Handler5.ashx?";
            string str = "select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(district, ' ') as district,isnull(pincode,' ') as pincode,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            con.Close();
            con.Open();
            SqlCommand comm = new SqlCommand(str, con);
            SqlDataReader drr = comm.ExecuteReader();
            drr.Read();
            string coll_name = Convert.ToString(drr["collname"]);
            string coll_address1 = Convert.ToString(drr["address1"]);
            string coll_address2 = Convert.ToString(drr["address2"]);
            string coll_address3 = Convert.ToString(drr["address3"]);
            string district = Convert.ToString(drr["district"]);
            string pin_code = Convert.ToString(drr["pincode"]);
            string catgory = drr["category"].ToString();
            catgory = "(An " + catgory + " Institution" + " " + "-" + "";
            string affliatedby = drr["affliated"].ToString();
            string affliatedbynew = Regex.Replace(affliatedby, ",", " ");
            string affiliated = catgory + " " + "Affiliated to" + " " + affliatedbynew + ")";
            string address = coll_address1 + "," + " " + coll_address2 + "," + " " + district + "-" + " " + pin_code + ".";
            string phoneno = drr["phoneno"].ToString();
            string faxno = drr["faxno"].ToString();
            string email = drr["email"].ToString();
            string website = drr["website"].ToString();
            con.Close();
            con.Open();
            SqlCommand cmd_timetabledate = new SqlCommand("select convert(varchar(10),fromdate,104) from semester_schedule where batch_year=" + ddlbatch.SelectedItem.ToString() + " and semester=" + ddlsem.SelectedItem.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString(), con);
            string date_affect = Convert.ToString(cmd_timetabledate.ExecuteScalar());
            int total_row_count = dt_dummyno.Rows.Count - 1;
            int tot_rw_ct = 0;
            if (dt_dummyno1.Rows.Count > 0)
            {
                tot_rw_ct = dt_dummyno1.Rows.Count - 1;
            }
            double pdfpage = total_row_count / 25;
            int ppcount = (int)Math.Round(pdfpage + 1);
            int recordcount = 0;
            //if (ppcount != 0)
            //{
            double recordcount_1 = 25;
            //}
            int prowcount = (int)Math.Round(recordcount_1);
            int recordcount_2 = 0;
            int rowincree = 0;
            string strsem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string[] datespilt = txtdate.Text.Split('/');
            string temp_date = datespilt[1] + '/' + datespilt[0] + '/' + datespilt[2];
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = "and sections='" + strsec + "'";
            }
            else
            {
                strsec = string.Empty;
            }
            string getyear = string.Empty;
            if (ddlsem.SelectedItem.ToString() == "1" || ddlsem.SelectedItem.ToString() == "2")
            {
                getyear = "I";
            }
            else if (ddlsem.SelectedItem.ToString() == "3" || ddlsem.SelectedItem.ToString() == "4")
            {
                getyear = "II";
            }
            else if (ddlsem.SelectedItem.ToString() == "5" || ddlsem.SelectedItem.ToString() == "6")
            {
                getyear = "III";
            }
            else if (ddlsem.SelectedItem.ToString() == "7" || ddlsem.SelectedItem.ToString() == "8")
            {
                getyear = "IV";
            }
            else if (ddlsem.SelectedItem.ToString() == "9" || ddlsem.SelectedItem.ToString() == "10")
            {
                getyear = "V";
            }

            for (int pagecount = 0; pagecount < 1; pagecount++)
            {
                Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    PdfImage Left_Logo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(Left_Logo, 25, 20, 500);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))//Aruna
                {
                    PdfImage Right_Logo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(Right_Logo, 700, 20, 500);
                }
                PdfTextArea ptc9 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 160, 20, 480, 30), System.Drawing.ContentAlignment.MiddleCenter, coll_name);
                PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 180, 40, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, address);
                PdfTextArea ptc1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, 200, 60, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, affiliated);

                PdfTextArea ptc4;
                if (lblcolg.Text.Trim().ToLower() == "school")
                {
                    ptc4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, 150, 70, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "" + lblbranch.Text + " of " + ddlbranch.SelectedItem.ToString());
                }
                else
                {
                    ptc4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(mydoc, 150, 70, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "Department of " + ddlbranch.SelectedItem.ToString());
                }

                PdfTextArea ptc5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, 150, 90, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "Time Table with effect from " + date_affect);


                PdfTextArea ptc7 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 15, 125, 50, 30), System.Drawing.ContentAlignment.MiddleCenter, "" + lblsem.Text.Trim() + " : " + ddlsem.SelectedItem.ToString());//modify by rajasekar 26/10/2018
                strsec = ddlsec.SelectedValue.ToString();
                if (strsec != "" && strsec != "-1" && strsec != "All")
                {
                    strsec = "and sections='" + strsec + "'";
                    PdfTextArea ptcsec = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 100, 125, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Sec :" + ddlsec.SelectedValue.ToString() + "");//modify by rajasekar 26/10/2018
                    mypdfpage.Add(ptcsec);
                    PdfTextArea ptc8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 18, 110, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class : " + ddldegree.SelectedItem.ToString() + "[" + ddlbranch.SelectedItem.ToString() + "]");//modify by rajasekar 26/10/2018
                    mypdfpage.Add(ptc8);
                }
                else
                {
                    PdfTextArea ptc8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 15, 110, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class : " + ddldegree.SelectedItem.ToString() + "[" + ddlbranch.SelectedItem.ToString() + "]");
                    mypdfpage.Add(ptc8);
                }
                //PdfTextArea ptc10 = new PdfTextArea(FontboldNew, System.Drawing.Color.Black,
                //                                    new PdfArea(mydoc, 25, 130, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Schedule:");
                //PdfTextArea ptc11 = new PdfTextArea(FontboldNew, System.Drawing.Color.Black,
                //                                   new PdfArea(mydoc, 25, 940, 500, 30), System.Drawing.ContentAlignment.TopLeft, "Class Advisor(s):");
                mypdfpage.Add(ptc9);
                mypdfpage.Add(ptc);
                //mypdfpage.Add(ptc1);
                // mypdfpage.Add(ptc2);
                // mypdfpage.Add(ptc3);
                mypdfpage.Add(ptc4);
                mypdfpage.Add(ptc5);
                // mypdfpage.Add(ptc6);
                mypdfpage.Add(ptc7);
                //  mypdfpage.Add(ptc8);
                //mypdfpage.Add(ptc10);
                //mypdfpage.Add(ptc11);            
                if (pagecount == 0)
                {
                    recordcount_2 = 0;
                    recordcount = recordcount + prowcount;
                }
                else
                {
                    recordcount = recordcount + prowcount;
                    recordcount_2 = recordcount_2 + prowcount;
                }
                int bindrow_2 = 0;
                if (pagecount == 0)
                {
                    rowincree = recordcount / 2;
                    bindrow_2 = rowincree;
                }
                else
                {
                    bindrow_2 = (recordcount - 25 + rowincree);
                }
                DataTable dt_temp_1 = new DataTable();
                DataRow dr_temp_1;
                DataColumn dc_temp_1;
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "S.No";
                dt_temp_1.Columns.Add(dc_temp_1);
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "Subject Type";
                dt_temp_1.Columns.Add(dc_temp_1);
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "Subject Code";
                dt_temp_1.Columns.Add(dc_temp_1);
                //added by rajasekar 26/10/2018
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "Subject Acronym";
                dt_temp_1.Columns.Add(dc_temp_1);
                //===================//
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "Subject";
                dt_temp_1.Columns.Add(dc_temp_1);
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "Staff";
                dt_temp_1.Columns.Add(dc_temp_1);
                dc_temp_1 = new DataColumn();
                dc_temp_1.ColumnName = "No Of Hours/Week";
                dt_temp_1.Columns.Add(dc_temp_1);


                DataTable dt_temp_2 = new DataTable();
                DataRow dr_temp_2;
                DataColumn dc_temp_2;
                dc_temp_2 = new DataColumn();
                dc_temp_2.ColumnName = "S.No";
                dt_temp_2.Columns.Add(dc_temp_2);
                dc_temp_2 = new DataColumn();
                dc_temp_2.ColumnName = "Subject Type";
                dt_temp_2.Columns.Add(dc_temp_2);
                dc_temp_2 = new DataColumn();
                dc_temp_2.ColumnName = "Subject Code";
                dt_temp_2.Columns.Add(dc_temp_2);
                //added by rajasekar 26/10/2018
                dc_temp_2 = new DataColumn();
                dc_temp_2.ColumnName = "Subject Acronym";
                dt_temp_2.Columns.Add(dc_temp_2);
                //===================//
                dc_temp_2 = new DataColumn();
                dc_temp_2.ColumnName = "Subject";
                dt_temp_2.Columns.Add(dc_temp_2);
                dc_temp_2 = new DataColumn();
                dc_temp_2.ColumnName = "Staff";
                dt_temp_2.Columns.Add(dc_temp_2);
                dc_temp_2 = new DataColumn();
                dc_temp_2.ColumnName = "No Of Hours/Week";
                dt_temp_2.Columns.Add(dc_temp_2);


                int sno = 0;
                //Rajkumar on 15-10-2018=====================================
                DataTable dtSubjOrder = dirAcc.selectDataTable("select s.subject_no from subject s,sub_sem ss where s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and s.subject_no in(" + string.Join(",", str_arr.ToArray()) + ") order by s.subType_no,s.subjectpriority");
                int tothrs = 0;
                for (int i = 0; i < dtSubjOrder.Rows.Count; i++)
                {
                    if (i <= 20)
                    {
                        string subjectNO = Convert.ToString(dtSubjOrder.Rows[i]["subject_no"]);
                        string str_staffdetails = "select s.subject_code,s.subject_name,s.acronym,ss.subject_type,s.noofhrsperweek,s.subject_no from subject s,sub_sem ss where subject_no='" + subjectNO + "' and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no;select staff_name,staff_code from staffmaster where staff_code in(select staff_code from staff_selector where subject_no='" + subjectNO + "' and batch_year=" + ddlbatch.SelectedItem.ToString() + " " + strsec + ")";
                        DataSet ds_staffdetails = new DataSet();
                        ds_staffdetails = d2.select_method(str_staffdetails, hat_select_method, "Text");
                        dr_temp_1 = dt_temp_1.NewRow();
                        sno++;
                        if (ds_staffdetails.Tables[0].Rows.Count > 0)
                        {
                            dr_temp_1["S.No"] = sno.ToString();
                            dr_temp_1["Subject Type"] = ds_staffdetails.Tables[0].Rows[0]["subject_type"].ToString();
                            dr_temp_1["Subject Code"] = ds_staffdetails.Tables[0].Rows[0]["subject_code"].ToString();
                            dr_temp_1["Subject Acronym"] = ds_staffdetails.Tables[0].Rows[0]["acronym"].ToString();//modify by rajasekar 26/10/2018
                            dr_temp_1["Subject"] = ds_staffdetails.Tables[0].Rows[0]["subject_name"].ToString();//modify by rajasekar 26/10/2018
                            //dr_temp_1["Subject"] = ds_staffdetails.Tables[0].Rows[0]["acronym"].ToString() + "-" + ds_staffdetails.Tables[0].Rows[0]["subject_name"].ToString();

                        }
                        if (ds_staffdetails.Tables[1].Rows.Count > 0)
                        {
                            int totHr = 0;
                            //Modified by srinath 19/12/2013
                            string staffname = string.Empty;
                            for (int st = 0; st < ds_staffdetails.Tables[1].Rows.Count; st++)
                            {
                                if (staffname == "")
                                {
                                    staffname = ds_staffdetails.Tables[1].Rows[st]["staff_name"].ToString();
                                }
                                else
                                {
                                    staffname = staffname + ", " + ds_staffdetails.Tables[1].Rows[st]["staff_name"].ToString();
                                }
                                string SubjectNo = ds_staffdetails.Tables[0].Rows[0]["subject_no"].ToString();
                                string staffC = ds_staffdetails.Tables[1].Rows[st]["staff_code"].ToString();
                                totHr = Convert.ToInt32(hatHr[SubjectNo + "-" + staffC]);

                            }
                            //  dr_temp_1["Staff"] = ds_staffdetails.Tables[1].Rows[0]["staff_name"].ToString();
                            dr_temp_1["Staff"] = staffname;
                            dr_temp_1["No Of Hours/Week"] = Convert.ToString(totHr);
                            tothrs += totHr;
                        }
                        dt_temp_1.Rows.Add(dr_temp_1);
                    }
                    else
                    {
                        string subjectNO = Convert.ToString(dtSubjOrder.Rows[i]["subject_no"]);
                        string str_staffdetails = "select s.subject_code,s.subject_name,s.acronym,ss.subject_type,s.noofhrsperweek,s.subject_no from subject s,sub_sem ss where subject_no='" + subjectNO + "' and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no;select staff_name,staff_code from staffmaster where staff_code in(select staff_code from staff_selector where subject_no='" + subjectNO + "' and batch_year=" + ddlbatch.SelectedItem.ToString() + " " + strsec + ")";
                        DataSet ds_staffdetails = new DataSet();
                        ds_staffdetails = d2.select_method(str_staffdetails, hat_select_method, "Text");
                        dr_temp_2 = dt_temp_2.NewRow();
                        sno++;
                        if (ds_staffdetails.Tables[0].Rows.Count > 0)
                        {
                            dr_temp_2["S.No"] = sno.ToString();
                            dr_temp_2["Subject Type"] = ds_staffdetails.Tables[0].Rows[0]["subject_type"].ToString();
                            dr_temp_2["Subject Code"] = ds_staffdetails.Tables[0].Rows[0]["subject_code"].ToString();
                            dr_temp_2["Subject Acronym"] = ds_staffdetails.Tables[0].Rows[0]["acronym"].ToString();//modify by rajasekar 26/10/2018
                            dr_temp_2["Subject"] = ds_staffdetails.Tables[0].Rows[0]["subject_name"].ToString();//modify by rajasekar 26/10/2018
                            //dr_temp_1["Subject"] = ds_staffdetails.Tables[0].Rows[0]["acronym"].ToString() + "-" + ds_staffdetails.Tables[0].Rows[0]["subject_name"].ToString();

                        }
                        if (ds_staffdetails.Tables[1].Rows.Count > 0)
                        {
                            int totHr = 0;
                            //Modified by srinath 19/12/2013
                            string staffname = string.Empty;
                            for (int st = 0; st < ds_staffdetails.Tables[1].Rows.Count; st++)
                            {
                                if (staffname == "")
                                {
                                    staffname = ds_staffdetails.Tables[1].Rows[st]["staff_name"].ToString();
                                }
                                else
                                {
                                    staffname = staffname + ", " + ds_staffdetails.Tables[1].Rows[st]["staff_name"].ToString();
                                }
                                string SubjectNo = ds_staffdetails.Tables[0].Rows[0]["subject_no"].ToString();
                                string staffC = ds_staffdetails.Tables[1].Rows[st]["staff_code"].ToString();
                                totHr = Convert.ToInt32(hatHr[SubjectNo + "-" + staffC]);

                            }
                            //  dr_temp_1["Staff"] = ds_staffdetails.Tables[1].Rows[0]["staff_name"].ToString();
                            dr_temp_2["Staff"] = staffname;
                            dr_temp_2["No Of Hours/Week"] = Convert.ToString(totHr);
                            tothrs += totHr;
                        }
                        dt_temp_2.Rows.Add(dr_temp_2);
                    }

                }
                if (dt_temp_2.Rows.Count > 0)
                {
                    dr_temp_2 = dt_temp_2.NewRow();
                    dr_temp_2["Staff"] = "TOTAL NO OF HOURS/WEEK";
                    dr_temp_2["No Of Hours/Week"] = Convert.ToString(tothrs);
                    dt_temp_2.Rows.Add(dr_temp_2);
                }
                else
                {
                    dr_temp_1 = dt_temp_1.NewRow();
                    dr_temp_1["Staff"] = "TOTAL NO OF HOURS/WEEK";
                    dr_temp_1["No Of Hours/Week"] = Convert.ToString(tothrs);
                    dt_temp_1.Rows.Add(dr_temp_1);
                }
                //***************added by srinath**********************
                if (str_arr.Count == 1)
                {
                    dr_temp_1 = dt_temp_1.NewRow();
                    sno++;
                    dr_temp_1["S.No"] = sno.ToString();
                    dr_temp_1["Subject Type"] = " ";
                    dr_temp_1["Subject Code"] = " ";
                    dr_temp_1["Subject Acronym"] = " ";//modify by rajasekar 26/10/2018
                    dr_temp_1["Subject"] = " ";
                    dr_temp_1["Staff"] = " ";
                    dr_temp_1["No Of Hours/Week"] = " ";
                    dt_temp_1.Rows.Add(dr_temp_1);
                }
                //***********************************************************
                DataTable dt_temp_4 = new DataTable();
                DataColumn dc_temp_4;
                DataRow dr_temp_4;
                string schedukle = d2.GetFunction("select  top 1 class_advisor from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + " and batch_year = " + ddlbatch.SelectedItem.ToString() + " and ttname='" + ddltimetable.SelectedItem.ToString() + "' and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc");
                if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                {
                    dc_temp_4 = new DataColumn("S.No");
                    dt_temp_4.Columns.Add(dc_temp_4);
                    dc_temp_4 = new DataColumn("Staff code");
                    dt_temp_4.Columns.Add(dc_temp_4);
                    dc_temp_4 = new DataColumn("Staff Name");
                    dt_temp_4.Columns.Add(dc_temp_4);
                    string[] spiltadvisor = schedukle.Split(',');
                    int serial = 0;
                    for (int i = 0; i <= spiltadvisor.GetUpperBound(0); i++)
                    {
                        serial++;
                        string staffname = d2.GetFunction("select staff_name from staffmaster where staff_code='" + spiltadvisor[i].ToString() + "'");
                        dr_temp_4 = dt_temp_4.NewRow();
                        dr_temp_4["S.No"] = serial.ToString();
                        dr_temp_4["Staff Code"] = spiltadvisor[i].ToString();
                        dr_temp_4["Staff Name"] = staffname;
                        dt_temp_4.Rows.Add(dr_temp_4);
                    }
                }
                DataTable dt_staffdetails = new DataTable();
                dt_staffdetails.Columns.Add("Stafflabel", typeof(String));
                dt_staffdetails.Rows.Add("Staff Details:");
                DataTable dt_classadvisor = new DataTable();
                dt_classadvisor.Columns.Add("classadvisor", typeof(String));
                dt_classadvisor.Rows.Add("Class Advisor:");
                PdfDocument myPdfDocument = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                PdfTable myPdfTable = myPdfDocument.NewTable(new Font("Verdana", 13), dt_dummyno.Rows.Count, dt_dummyno.Columns.Count, 5);
                myPdfTable.ImportDataTable(dt_dummyno);
                //myPdfTable.SetColumnsWidth(new int[] { 100 });
                //myPdfTable.VisibleHeaders = false;
                myPdfTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                PdfDocument mypdfstafflabel = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                PdfTable myPdfTablestafflabel = mypdfstafflabel.NewTable(new Font("Book Antiqua", 15, FontStyle.Bold), dt_staffdetails.Rows.Count, dt_staffdetails.Columns.Count, 5);
                myPdfTablestafflabel.ImportDataTable(dt_staffdetails);
                //myPdfTable.SetColumnsWidth(new int[] { 100 });
                myPdfTablestafflabel.VisibleHeaders = false;
                myPdfTablestafflabel.SetBorders(Color.White, 1, BorderType.CompleteGrid);
                myPdfTablestafflabel.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                int norowva = dt_temp_1.Rows.Count + 1;
                PdfDocument myPdfDocument_3 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));

                PdfTable myPdfTable_3 = myPdfDocument_3.NewTable(new Font("Verdana", 13), norowva, 7, 8);//modify by rajasekar 26/10/2018

                myPdfTable_3.ImportDataTable(dt_temp_1);
                myPdfTable_3.SetColumnsWidth(new int[] { 20, 50, 50, 50, 120, 120, 40 });//modify by rajasekar 26/10/2018

                //myPdfTable_3.VisibleHeaders = false;
                myPdfTable_3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                int norowva1 = 0;


                PdfDocument mypdfclassadvisor = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                PdfTable myPdfTableclassadvisor = mypdfclassadvisor.NewTable(new Font("Book Antiqua", 15, FontStyle.Bold), dt_classadvisor.Rows.Count, dt_classadvisor.Columns.Count, 6);
                myPdfTableclassadvisor.ImportDataTable(dt_classadvisor);
                //myPdfTable.SetColumnsWidth(new int[] { 100 });
                myPdfTableclassadvisor.VisibleHeaders = false;
                myPdfTableclassadvisor.SetBorders(Color.White, 1, BorderType.CompleteGrid);
                myPdfTableclassadvisor.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);



                PdfDocument myPdfDocument_1 = null;
                PdfTable myPdfTable_1 = null;
                int findrow = 0;
                if (dt_temp_4.Rows.Count > 0)
                {
                    findrow = 1;
                    myPdfDocument_1 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                    myPdfTable_1 = myPdfDocument_1.NewTable(new Font("Verdana", 13), dt_temp_4.Rows.Count, 3, 8);
                    myPdfTable_1.ImportDataTable(dt_temp_4);
                    myPdfTable_1.SetColumnsWidth(new int[] { 30, 150, 200 });
                    myPdfTable_1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                }
                //myPdfTable.HeadersRow.SetColors(Color.Black, Color.LightGray);
                //myPdfTable_3.HeadersRow.SetColors(Color.Black, Color.LightGray);
                //myPdfTable_1.HeadersRow.SetColors(Color.Black, Color.LightGray);
                myPdfTable.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                myPdfTable_3.Columns[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                myPdfTable_3.Columns[4].SetContentAlignment(ContentAlignment.MiddleLeft);
                if (findrow == 1)
                {
                    myPdfTable_1.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                    myPdfTable_1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                }
                Gios.Pdf.PdfTable table = mydoc.NewTable(table_cell, dt_dummyno.Rows.Count + 1, dt_dummyno.Columns.Count, 1);
                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                Gios.Pdf.PdfTable table_1 = mydoc.NewTable(table_cell, dt_temp_1.Rows.Count + 5, dt_temp_1.Columns.Count, 1);
                table_1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                if (findrow == 1)
                {
                    Gios.Pdf.PdfTable table_4 = mydoc.NewTable(table_cell, dt_temp_4.Rows.Count + 1, dt_temp_4.Columns.Count, 1);
                    table_4.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                }


                PdfTablePage newPdfTablePage = myPdfTable.CreateTablePage(new PdfArea(myPdfDocument, 20, -135, 800, 1300));
                mypdfpage.Add(newPdfTablePage);
                double lastrow = newPdfTablePage.Area.BottomRightCornerY;
                PdfTablePage newPdfTablePagestafflabel = myPdfTablestafflabel.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, lastrow + 5, 800, 500));
                //   mypdfpage.Add(newPdfTablePagestafflabel);
                PdfTextArea ptcf1;
                PdfTextArea ptcf2;
                PdfTextArea ptcf3;
                PdfTextArea ptcf4;

                if (dt_temp_2.Rows.Count > 1)
                {
                    norowva1 = dt_temp_2.Rows.Count + 1;
                    PdfDocument myPdfDocument_4 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));

                    PdfTable myPdfTable_4 = myPdfDocument_4.NewTable(new Font("Verdana", 13), norowva1 + 1, 7, 8);//modify by rajasekar 26/10/2018

                    myPdfTable_4.ImportDataTable(dt_temp_2);
                    myPdfTable_4.SetColumnsWidth(new int[] { 20, 50, 50, 50, 120, 120, 40 });//modify by rajasekar 26/10/2018
                    myPdfTable_4.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                    myPdfTable_4.Columns[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                    myPdfTable_4.Columns[4].SetContentAlignment(ContentAlignment.MiddleLeft);
                    Gios.Pdf.PdfTable table_4 = mydoc.NewTable(table_cell, dt_temp_2.Rows.Count + 2, dt_temp_2.Columns.Count, 1);
                    table_4.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                }

                PdfDocument myPdfDocument_2 = null;
                PdfTable myPdfTable_2 = null;
                // int findrow1 = 0;
                if (dt_dummyno1.Rows.Count > 0)
                {
                    mypdfpage.SaveToDocument();
                    myPdfDocument_2 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                    myPdfTable_2 = myPdfDocument_2.NewTable(new Font("Verdana", 13), dt_dummyno1.Rows.Count, dt_dummyno1.Columns.Count, 5);
                    myPdfTable_2.ImportDataTable(dt_dummyno1);
                    // myPdfTable_2.SetColumnsWidth(new int[] { 30, 150, 200 });
                    myPdfTable_2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                    Gios.Pdf.PdfTable table1 = mydoc.NewTable(table_cell, dt_dummyno1.Rows.Count + 1, dt_dummyno1.Columns.Count, 1);
                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                    mypdfpage = mydoc.NewPage();
                    PdfTablePage newPdfTablePage1 = myPdfTable_2.CreateTablePage(new PdfArea(myPdfDocument_2, 20, -135, 800, 1300));
                    mypdfpage.Add(newPdfTablePage1);
                }
                if (lblcolg.Text.Trim().ToLower() != "school")
                {
                    ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, 0, 1115, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Advisor");
                    ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 80, 1115, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                    ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 360, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Coordinator "); //AEC
                    ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 550, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                }
                else
                {
                    ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, 0, 1115, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Teacher");
                    ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 80, 1115, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "");
                    ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 360, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, " "); //AEC
                    ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 550, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                }
                mypdfpage.Add(ptcf1);
                mypdfpage.Add(ptcf2);
                mypdfpage.Add(ptcf3);
                mypdfpage.Add(ptcf4);
                double lastrowofstafflabel = newPdfTablePagestafflabel.Area.BottomRightCornerY;
                PdfTablePage newPdfTablePage_3 = myPdfTable_3.CreateTablePage(new PdfArea(myPdfDocument_3, 20, lastrowofstafflabel, 800, (dt_temp_1.Rows.Count + 1) * 45));


                double totalh = newPdfTablePage_3.Area.Height;
                totalh = totalh + lastrowofstafflabel + 10;
                if (totalh < 830)
                {
                    mypdfpage.Add(newPdfTablePagestafflabel);
                    mypdfpage.Add(newPdfTablePage_3);
                    mypdfpage.SaveToDocument();
                    double lastrowofstaffdetails = newPdfTablePage_3.Area.BottomRightCornerY;
                    if (lastrowofstaffdetails <= 620)
                    {
                        if (findrow == 1)
                        {
                            PdfTablePage newPdfTablePageclassadvisor = myPdfTableclassadvisor.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, lastrowofstaffdetails + 20, 200, 100));
                            mypdfpage.Add(newPdfTablePageclassadvisor);
                            double lastrowofclassadvisor = newPdfTablePageclassadvisor.Area.BottomRightCornerY;
                            if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                            {
                                PdfTablePage newPdfTablePage_1 = myPdfTable_1.CreateTablePage(new PdfArea(myPdfDocument_1, 20, lastrowofclassadvisor, 800, (dt_temp_4.Rows.Count + 1) * 34));
                                mypdfpage.Add(newPdfTablePage_1);
                            }
                        }
                    }
                    else
                    {
                        if (findrow == 1)
                        {
                            mypdfpage = mydoc.NewPage();
                            PdfTablePage newPdfTablePageclassadvisor = myPdfTableclassadvisor.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, -150, 200, 100));
                            mypdfpage.Add(newPdfTablePageclassadvisor);
                            if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                            {
                                PdfTablePage newPdfTablePage_1 = myPdfTable_1.CreateTablePage(new PdfArea(myPdfDocument_1, 20, -120, 800, (dt_temp_4.Rows.Count + 1) * 34));
                                mypdfpage.Add(newPdfTablePage_1);
                            }
                            ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 0, 1115, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Advisor");
                            ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 80, 1115, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                            ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 360, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "AEC Coordinator ");
                            ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 550, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                            mypdfpage.Add(ptcf1);
                            mypdfpage.Add(ptcf2);
                            mypdfpage.Add(ptcf3);
                            mypdfpage.Add(ptcf4);
                        }
                        //mypdfpage.SaveToDocument();
                    }
                }
                else
                {

                    mypdfpage.SaveToDocument();
                    mypdfpage = mydoc.NewPage();
                    PdfTextArea ptcfst = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, 20, 50, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, "Staff Details:");
                    mypdfpage.Add(ptcfst);
                    PdfDocument myPdfDocument_5 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                    PdfTable myPdfTable_5 = myPdfDocument_5.NewTable(new Font("Verdana", 13), dt_temp_1.Rows.Count, 7, 9);//modify by rajasekar 26/10/2018
                    myPdfTable_5.ImportDataTable(dt_temp_1);
                    myPdfTable_5.SetColumnsWidth(new int[] { 20, 50, 50, 50, 120, 120, 40 });//modify by rajasekar 26/10/2018
                    myPdfTable_5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    myPdfTable_5.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                    myPdfTable_5.Columns[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                    myPdfTable_5.Columns[4].SetContentAlignment(ContentAlignment.MiddleLeft);

                    //  PdfTablePage newPdfTablePage41 = myPdfTable_5.CreateTablePage(new PdfArea(myPdfDocument_5
                    PdfTablePage newPdfTablePage4 = myPdfTable_5.CreateTablePage(new PdfArea(myPdfDocument_5, 20, -200, 800, (dt_temp_1.Rows.Count + 5) * 38));
                    mypdfpage.Add(newPdfTablePage4);
                    double lastrowofstaffdetails = newPdfTablePage4.Area.BottomRightCornerY;




                    if (lastrowofstaffdetails <= 620)
                    {
                        if (dt_temp_2.Rows.Count > 0)
                        {
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydoc.NewPage();
                            PdfTextArea ptcfst1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, 20, 50, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, "Staff Details:");
                            mypdfpage.Add(ptcfst1);
                            PdfDocument myPdfDocument_51 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                            PdfTable myPdfTable_51 = myPdfDocument_51.NewTable(new Font("Verdana", 13), dt_temp_2.Rows.Count + 2, 7, 9);//modify by rajasekar 26/10/2018
                            myPdfTable_51.ImportDataTable(dt_temp_2);
                            myPdfTable_51.SetColumnsWidth(new int[] { 20, 50, 50, 50, 120, 120, 40 });//modify by rajasekar 26/10/2018
                            myPdfTable_51.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            myPdfTable_51.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfTable_51.Columns[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfTable_51.Columns[4].SetContentAlignment(ContentAlignment.MiddleLeft);
                            //PdfTablePage newPdfTablePagestafflabe2 = myPdfTablestafflabel.CreateTablePage(new PdfArea(mypdfstafflabel, 20, 10, 800, 500));
                            //mypdfpage.Add(newPdfTablePagestafflabe2);
                            PdfTablePage newPdfTablePage41 = myPdfTable_51.CreateTablePage(new PdfArea(myPdfDocument_51, 20, -200, 800, (dt_temp_2.Rows.Count + 1) * 48));
                            mypdfpage.Add(newPdfTablePage41);

                        }

                        PdfTablePage newPdfTablePageclassadvisor = myPdfTableclassadvisor.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, lastrowofstaffdetails + 70, 200, 100));
                        mypdfpage.Add(newPdfTablePageclassadvisor);
                        if (schedukle != null && schedukle.Trim() != "" && schedukle.Trim() != "0")
                        {
                            PdfTablePage newPdfTablePage_1 = myPdfTable_1.CreateTablePage(new PdfArea(myPdfDocument_1, 20, lastrowofstaffdetails + 80, 800, (dt_temp_4.Rows.Count + 1) * 34));
                            mypdfpage.Add(newPdfTablePage_1);
                        }
                        ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, 0, 1115, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Advisor");
                        ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 80, 1115, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                        ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 360, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "AEC Coordinator ");
                        ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 550, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                        mypdfpage.Add(ptcf1);
                        mypdfpage.Add(ptcf2);
                        mypdfpage.Add(ptcf3);
                        mypdfpage.Add(ptcf4);
                    }
                    else
                    {
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        if (dt_temp_2.Rows.Count > 0)
                        {
                            //mypdfpage.SaveToDocument();
                            //mypdfpage = mydoc.NewPage();
                            PdfTextArea ptcfst1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, 20, 50, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, "Staff Details:");
                            mypdfpage.Add(ptcfst1);
                            PdfDocument myPdfDocument_51 = new PdfDocument(PdfDocumentFormat.InCentimeters(21, 30));
                            PdfTable myPdfTable_51 = myPdfDocument_51.NewTable(new Font("Verdana", 13), dt_temp_2.Rows.Count + 2, 7, 9);//modify by rajasekar 26/10/2018
                            myPdfTable_51.ImportDataTable(dt_temp_2);
                            myPdfTable_51.SetColumnsWidth(new int[] { 20, 50, 50, 50, 120, 120, 40 });//modify by rajasekar 26/10/2018
                            myPdfTable_51.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            myPdfTable_51.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfTable_51.Columns[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfTable_51.Columns[4].SetContentAlignment(ContentAlignment.MiddleLeft);
                            //PdfTablePage newPdfTablePagestafflabe2 = myPdfTablestafflabel.CreateTablePage(new PdfArea(mypdfstafflabel, 20, 10, 800, 500));
                            //mypdfpage.Add(newPdfTablePagestafflabe2);
                            PdfTablePage newPdfTablePage41 = myPdfTable_51.CreateTablePage(new PdfArea(myPdfDocument_51, 20, -200, 800, (dt_temp_2.Rows.Count + 1) * 48));
                            mypdfpage.Add(newPdfTablePage41);

                        }

                        PdfTablePage newPdfTablePageclassadvisor = myPdfTableclassadvisor.CreateTablePage(new PdfArea(mypdfclassadvisor, 20, 550, 200, 100));
                        mypdfpage.Add(newPdfTablePageclassadvisor);
                        if (myPdfTable_1 != null)
                        {
                            PdfTablePage newPdfTablePage_1 = myPdfTable_1.CreateTablePage(new PdfArea(myPdfDocument_1, 20, -120, 800, (dt_temp_4.Rows.Count + 1) * 34));
                            mypdfpage.Add(newPdfTablePage_1);
                        }
                        ptcf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, 0, 1115, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "Class Advisor");
                        ptcf2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 80, 1115, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                        ptcf3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 360, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "AEC Coordinator ");
                        ptcf4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 550, 1115, 300, 30), System.Drawing.ContentAlignment.MiddleCenter, "Principal");
                        mypdfpage.Add(ptcf1);
                        mypdfpage.Add(ptcf2);
                        mypdfpage.Add(ptcf3);
                        mypdfpage.Add(ptcf4);
                    }
                }

                mypdfpage.SaveToDocument();
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "TimeTable.pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    protected void chkmulstaff_ChekedChange(object sender, EventArgs e)
    {
        txtmulstaff.Text = "---Select---";
        if (chkmulstaff.Checked == true)
        {
            if (chkmullsstaff.Items.Count > 0)
            {
                for (int i = 0; i < chkmullsstaff.Items.Count; i++)
                {
                    chkmullsstaff.Items[i].Selected = true;
                }
                txtmulstaff.Text = "Staff (" + chkmullsstaff.Items.Count + ")";
            }
        }
        else
        {
            for (int i = 0; i < chkmullsstaff.Items.Count; i++)
            {
                chkmullsstaff.Items[i].Selected = false;
            }
        }
    }

    protected void chkmullsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtmulstaff.Text = "---Select---";
        chkmulstaff.Checked = false;
        int cou = 0;
        for (int i = 0; i < chkmullsstaff.Items.Count; i++)
        {
            if (chkmullsstaff.Items[i].Selected == true)
            {
                cou++;
            }
        }
        if (cou > 0)
        {
            txtmulstaff.Text = "Staff (" + cou + ")";
            if (chkmullsstaff.Items.Count == cou)
            {
                chkmulstaff.Checked = true;
            }
        }
    }

    protected void btnmulstaff_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtview = (DataTable)ViewState["dtgview1"]; string ddlvalue = string.Empty;
            DataTable staffselet = new DataTable();
            DataRow drstaff = null;
            staffselet.Columns.Add("staff_name");
            staffselet.Columns.Add("staff_value");
            string strsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() != "" && ddlsec.SelectedItem.ToString() != "-1" && ddlsec.SelectedItem.ToString() != null)
                {
                    strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                }
            }
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            string strsem = ddlsem.SelectedValue.ToString();
            int activerow = gview1.Rows.Count - 1;
            if (activerow != -1)
            {
                if (chkmullsstaff.Items.Count > 0)
                {
                    string stafftext = string.Empty;
                    string stafftag = string.Empty;
                    for (int i = 0; i < chkmullsstaff.Items.Count; i++)
                    {
                        if (chkmullsstaff.Items[i].Selected == true)
                        {
                            string stte = chkmullsstaff.Items[i].Text.ToString();
                            string[] stcode = stte.Split('-');
                            if (stafftext == "")
                            {
                                stafftext = chkmullsstaff.Items[i].Text.ToString();
                                stafftag = stcode[stcode.GetUpperBound(0)].ToString();
                            }
                            else
                            {
                                stafftext = stafftext + "-" + chkmullsstaff.Items[i].Text.ToString();
                                stafftag = stafftag + '-' + stcode[stcode.GetUpperBound(0)].ToString();
                            }
                        }
                    }
                    int staf_cnt = 0;
                    string staff_code = string.Empty;
                    string staff_name_code = string.Empty;
                    int parent_count = subjtree.Nodes.Count;
                    for (int i = 0; i < parent_count; i++)
                    {
                        for (int node_count = 0; node_count < subjtree.Nodes[i].ChildNodes.Count; node_count++)//-------count child node
                        {
                            if (subjtree.Nodes[i].ChildNodes[node_count].Selected == true)//-------check checked condition
                            {

                                subjtree.Visible = true;
                                chkappend.Visible = true;
                                btnOk.Visible = true;
                                treepanel.Visible = true;

                                string chile_index = subjtree.Nodes[i].ChildNodes[node_count].Value;

                                DataSet staf_set = d2.select_method("select staff_code,staff_name from staffmaster where staff_code in (select staff_code from staff_selector where subject_no = " + Convert.ToInt32(chile_index) + " and batch_year=" + strbatchyear.ToString() + " " + strsec + ")", hat, "Text");
                                if (staf_set.Tables[0].Rows.Count > 1)
                                {
                                    txtmulstaff.Visible = false;
                                    lblmulstaff.Visible = false;
                                    string[] staff_list = new string[staf_set.Tables[0].Rows.Count + 2];
                                    for (staf_cnt = 0; staf_cnt < staf_set.Tables[0].Rows.Count; staf_cnt++)
                                    {
                                        staff_list[staf_cnt] = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        if (staff_code == "")
                                        {
                                            ddlvalue = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_code = staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_name_code = staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        }
                                        else
                                        {
                                            ddlvalue = ddlvalue + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_code = staff_code + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                            staff_name_code = staff_name_code + ";" + staf_set.Tables[0].Rows[staf_cnt][1].ToString() + "-" + staf_set.Tables[0].Rows[staf_cnt][0].ToString();
                                        }
                                    }
                                    if (staff_list.GetUpperBound(0) > 0)
                                    {
                                        ddlvalue = ddlvalue + ";" + stafftext;
                                        staff_list[staf_cnt] = stafftext;
                                        staff_list[staf_cnt + 1] = "All";
                                        ddlvalue = ddlvalue + ";" + "All";
                                    }

                                    for (int row = 0; row < staff_list.Length; row++)
                                    {
                                        drstaff = staffselet.NewRow();
                                        drstaff["staff_name"] = staff_list[row];
                                        staffselet.Rows.Add(drstaff);
                                    }
                                    dtview.Rows[dtview.Rows.Count - 1]["ddlvalue"] = ddlvalue;
                                    ViewState["dtgview1"] = dtview;
                                    Label lbl = (gview1.Rows[activerow].FindControl("lblddltag") as Label);
                                    lbl.Text = stafftag;
                                    DropDownList ddl = (gview1.Rows[activerow].FindControl("ddlstaffnme") as DropDownList);
                                    ddl.DataSource = staffselet;
                                    ddl.DataTextField = "staff_name";
                                    ddl.DataValueField = "staff_name";
                                    ddl.DataBind();
                                    //ViewState["gview1"]=
                                }
                                treepanel.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Time Table");
        }
    }

    private void bindGridTTSelect(int totalHour = 8)
    {
        string dayvalue = string.Empty;
        int dayorder = 0;
        string holiday = string.Empty;
        int noofhours = 0;
        int noofdays = 0; //Deepali on 16.4.18
        DataTable dtTTSel = new DataTable();
        //DataRow dr = dtTTSel.NewRow();
        string day = string.Empty;
        DataSet dsDay = new DataSet();
        int date = 0;
        string strsem = ddlsem.SelectedValue.ToString();
        string strpriodquery = "Select No_of_hrs_per_day,schorder,nodays,holiday from PeriodAttndSchedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + strsem + "";
        dsDay = d2.select_method(strpriodquery, hat, "Text");
        if (dsDay.Tables.Count > 0 && dsDay.Tables[0].Rows.Count > 0)
        {
            dayorder = Convert.ToInt32(dsDay.Tables[0].Rows[0]["schorder"]);
            noofhours = Convert.ToInt32(dsDay.Tables[0].Rows[0]["No_of_hrs_per_day"]);
            totalHour = noofhours; //aruna 07/dec/2017
            noofdays = Convert.ToInt32(dsDay.Tables[0].Rows[0]["nodays"]);//Deepali on 16.4.18
            holiday = Convert.ToString(dsDay.Tables[0].Rows[0]["holiday"]);
            //Session["totalhrs"] = Convert.ToString(noofhours);
            //Session["totnoofdays"] = Convert.ToString(noofdays);
            Session["dayorder"] = Convert.ToString(dayorder);
        }
        if (dayorder == 1)
        {
            dtTTSel.Columns.Add("Day");
            dtTTSel.Columns.Add("DayVal");
            dtTTSel.Columns.Add("H1");
            dtTTSel.Columns.Add("H2");
            dtTTSel.Columns.Add("H3");
            dtTTSel.Columns.Add("H4");
            dtTTSel.Columns.Add("H5");
            dtTTSel.Columns.Add("H6");
            dtTTSel.Columns.Add("H7");
            dtTTSel.Columns.Add("H8");
            dtTTSel.Columns.Add("H9");
            dtTTSel.Columns.Add("H10");
            //for (int i = 1; i < 7; i++)
            for (int i = 1; i <= noofdays; i++)//Deepali on 16.4.18
            {
                switch (i)
                {
                    case 1:
                        day = "Monday";
                        break;
                    case 2:
                        day = "Tuesday";
                        break;
                    case 3:
                        day = "Wednesday";
                        break;
                    case 4:
                        day = "Thursday";
                        break;
                    case 5:
                        day = "Friday";
                        break;
                    case 6:
                        day = "Saturday";
                        break;
                    case 7:
                        day = "Sunday";
                        break;
                }
                DataRow dr = dtTTSel.NewRow();
                dr["Day"] = day;
                dr["DayVal"] = i;
                dtTTSel.Rows.Add(dr);
            }

            gridSelTT.DataSource = dtTTSel;
            gridSelTT.DataBind();
        }

        else
        {
            dtTTSel.Columns.Add("Day");
            dtTTSel.Columns.Add("DayVal");
            //for (int day1 = 0; day1 < 6; day1++)
            for (int day1 = 0; day1 < noofdays; day1++) //Deepali on 16.4.18
            {
                DataRow dr = dtTTSel.NewRow();
                //string dayofweek = Days[day];
                //string dayofweek1 = Daymon[day];
                int daysetweek = day1 + 2;
                //if (day1 == 6)
                if (day1 == noofdays)//Deepali on 16.4.18
                {
                    daysetweek = 1;
                }
                if (!holiday.Contains(daysetweek.ToString()))
                {

                    if (dayorder == 1)
                    {

                    }
                    else
                    {
                        date = day1 + 1;
                        dr["Day"] = "Day" + " " + date;
                        dr["DayVal"] = date;
                        dtTTSel.Rows.Add(dr);
                    }
                }
            }
            gridSelTT.DataSource = dtTTSel;
            gridSelTT.DataBind();
        }
        //if (totalHour <=10)
        //{
        //foreach (GridViewRow gRow in gridSelTT.Rows)
        //{
        //for (int cell = 1; cell < gRow.Cells.Count; cell++)
        for (int cell = 1; cell <= 10; cell++)
        {
            //gRow.Cells[cell].Visible = (cell <= totalHour) ? true : false;
            //gridSelTT.HeaderRow.Cells[cell].Visible = (cell <= totalHour) ? true : false;
            if (cell > totalHour)
            {
                gridSelTT.Columns[cell].Visible = false;
            }
            else
            {
                gridSelTT.Columns[cell].Visible = true;
            }
        }
        //}
        //}
    }

    protected void spcellClickPopupclose_Click(object sender, EventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        lblErrMsg.Visible = false;
        spcellClickPopup.Visible = false;
        treepanel.Visible = false;
    }

    protected void cb_Category_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ddlLabTest = (CheckBox)sender;
            var row = ddlLabTest.NamingContainer;
            string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            CheckBox ddlAddLabTestShortName = (CheckBox)row.FindControl("cb_staffname");
            CheckBoxList cbl = (CheckBoxList)row.FindControl("cbl_staffname");
            TextBox txtB = (TextBox)row.FindControl("txt_staffname");
            CallCheckboxChange(ddlAddLabTestShortName, cbl, txtB, "Staff", "--Select--");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    protected void cbl_staffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBoxList ddlLabTest = (CheckBoxList)sender;
        var row = ddlLabTest.NamingContainer;
        string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 2;
        CheckBox ddlAddLabTestShortName = (CheckBox)row.FindControl("cb_staffname");
        CheckBoxList cbl = (CheckBoxList)row.FindControl("cbl_staffname");
        TextBox txtB = (TextBox)row.FindControl("txt_staffname");
        CallCheckboxListChange(ddlAddLabTestShortName, cbl, txtB, "Staff", "--Select--");
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;

                    if (name == string.Empty)
                    {
                        name = Convert.ToString(cbl.Items[sel].Text);
                    }
                    else
                    {
                        name = name + "-" + Convert.ToString(cbl.Items[sel].Text);
                    }
                }
                txt.Text = name;
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    if (name == string.Empty)
                    {
                        name = Convert.ToString(cbl.Items[sel].Text);
                    }
                    else
                    {
                        name = name + "-" + Convert.ToString(cbl.Items[sel].Text);
                    }
                }
            }
            txt.Text = name;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student Time Table"); }
    }

    public void Btnnoremove_Click(object sender, EventArgs e)
    {
        div2.Visible = false;
        Div9.Visible = false;
        //addPopup();
    }

    public void Btnalter_Click(object sender, EventArgs e)
    {
        div2.Visible = false;
        Div9.Visible = false;
        addPopup();
    }

    public void btnyes_Click(object sender, EventArgs e)
    {
        int actrow = Convert.ToInt32(Session["roww"]);
        int actcol = Convert.ToInt32(Session["coll"]);
        gview.Rows[actrow].Cells[actcol].Text = "";
        DataTable dttab = (DataTable)ViewState["table"];
        dttab.Rows[actrow][actcol] = "";
        ViewState["table"] = dttab;
        btnsave.Enabled = true;
        btnsave.Visible = true;
        div2.Visible = false;
        Div9.Visible = false;
    }

    public void addPopup()
    {
        try
        {
            string actrow = Convert.ToString(Session["roww"]);
            string actcol = Convert.ToString(Session["coll"]);

            txtmulstaff.Visible = false;
            lblmulstaff.Visible = false;
            pmulstaff.Visible = false;
            btnmulstaff.Visible = false;
            btndelete.Visible = false;
            btndelete.Enabled = false;
            lbltimings.Visible = false;
            lblfromtime.Visible = false;
            lbltotime.Visible = false;
            btnOk.Visible = false;
            txtmultisubj.Visible = false;
            chkappend.Checked = false;
            chkappend.Visible = false;
            dtablevalue = (DataTable)ViewState["table"];
            //var grid = (GridView)sender;
            //GridViewRow selectedRow = grid.SelectedRow;
            //rowIndex = grid.SelectedIndex;
            //selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            //string activerow = Convert.ToString(rowIndex);
            //string activecol = Convert.ToString(selectedCellIndex);
            Session["actrow"] = actrow;
            Session["actcol"] = actcol;
            if (actcol != "0" && actrow != "-1")
            {
                int row = Convert.ToInt32(actrow);
                int col = Convert.ToInt32(actcol);
                lblday.Visible = true;
                lblday1.Visible = true;
                lblday1.Text = gview.Rows[row].Cells[col].Text;
                ds.Dispose();
                ds.Reset();
                string strtimequery = "Select RIGHT(CONVERT(VARCHAR, start_time, 100),7) as start_time,RIGHT(CONVERT(VARCHAR, end_time, 100),7) as end_time from BellSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and period1='" + col + "'";
                ds = d2.select_method(strtimequery, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["start_time"].ToString() != "" && ds.Tables[0].Rows[0]["start_time"].ToString() != null && ds.Tables[0].Rows[0]["end_time"].ToString() != "" && ds.Tables[0].Rows[0]["end_time"].ToString() != null)
                    {
                        //string[] spiltstarttime = ds.Tables[0].Rows[0]["start_time"].ToString().Split(' ');
                        //string[] spiltendtime = ds.Tables[0].Rows[0]["end_time"].ToString().Split(' ');
                        //lblfromtime.Text = spiltstarttime[1].ToString() + ' ' + spiltstarttime[2].ToString() + ' ' + "To";
                        //lbltotime.Text = spiltendtime[1].ToString() + ' ' + spiltendtime[2].ToString();
                        lblfromtime.Text = ds.Tables[0].Rows[0]["start_time"].ToString();
                        lbltotime.Text = ds.Tables[0].Rows[0]["end_time"].ToString();
                        lbltimings.Visible = true;
                        lblfromtime.Visible = true;
                        lbltotime.Visible = true;
                    }
                }
                if (gview.Rows[row].Cells[col].Text != null && gview.Rows[row].Cells[col].Text != "")
                {
                    btndelete.Visible = true;
                    btndelete.Enabled = true;
                    string columnvalue = Convert.ToString(dtablevalue.Rows[row][col]);
                    dtablestaff.Columns.Add("Subject");
                    dtablestaff.Columns.Add("Staff Name");
                    dtablestaff.Columns.Add("Class Type");

                    dtrowstaff = dtablestaff.NewRow();
                    dtrowstaff["Subject"] = "Subject";
                    dtrowstaff["Staff Name"] = "Staff Name";
                    dtrowstaff["Class Type"] = "Class Type";

                    dtablestaff.Rows.Add(dtrowstaff);

                    string[] spitcolumnvalue = columnvalue.Split(';');
                    ArrayList arrStaffSubjects = new ArrayList();
                    for (int i = 0; i <= spitcolumnvalue.GetUpperBound(0); i++)
                    {
                        string[] spitsubject = spitcolumnvalue[i].Split('-');
                        int t = spitsubject.GetUpperBound(0);
                        for (int j = 1; j < t; j++)
                        {
                            for (int next = j; next <= j; next++)
                            {
                                string subjectno = d2.GetFunction("Select subject_name from subject where subject_no='" + spitsubject[0] + "'");
                                string type = spitsubject[spitsubject.GetUpperBound(0)].Trim();
                                if (type == "S")
                                {
                                    type = "Single";
                                }
                                else if (type == "C")   //modifed by Prabha 25/11/2017
                                {
                                    type = "Combined";
                                }
                                else
                                {
                                    type = "Lab";
                                }
                                string staff = d2.GetFunction("Select Staff_name from staffmaster where staff_code='" + spitsubject[next].ToString() + "'");
                                if (staff.Trim() == "" || staff.Trim() == "0")
                                {
                                    staff = "-";
                                }
                                string keyList = spitsubject[0].Trim() + "@" + Convert.ToString(spitsubject[next]).Trim();
                                if (!arrStaffSubjects.Contains(keyList.Trim().ToLower()))
                                {
                                    dtrowstaff = dtablestaff.NewRow();
                                    arrStaffSubjects.Add(keyList.Trim().ToLower());
                                    dtrowstaff["Subject"] = subjectno;
                                    dtrowstaff["Staff Name"] = staff;
                                    dtrowstaff["Class Type"] = type;
                                    dtablestaff.Rows.Add(dtrowstaff);
                                    gviewstaff.DataSource = dtablestaff;
                                    gviewstaff.DataBind();
                                    gviewstaff.Visible = true;
                                    RowHeadstaff(gviewstaff);
                                }
                            }
                        }
                    }
                }
                treeload();
            }
        }
        catch
        {
        }
    }
}