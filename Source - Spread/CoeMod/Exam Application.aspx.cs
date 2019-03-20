using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Collections.Generic;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using System.Configuration;

public partial class Exam_Application : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string qry = string.Empty;
    string section = string.Empty;
    string qrysec = string.Empty;
    Boolean flag_true = false;
    ArrayList alv = new ArrayList();
    Hashtable hashmark = new Hashtable();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    static string grpusercd = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Convert.ToString(Session["usercode"]);
            lblvalidation1.Visible = false;
            lblerror.Visible = false;
            if (!IsPostBack)
            {
                chkIsSupplementaryExam.Checked = false;
                loadcollege();
                collegecode = Convert.ToString(ddlcollege.SelectedValue);
                bindbatch();
                binddegree();
                bindbranch();
                bindsemester();
                bindSection();
                clear();
                CheckFiance();
                BindSessionHourMinites();
                txtappldate.Attributes.Add("readonly", "readonly");
                txtappllastdate.Attributes.Add("readonly", "readonly");
                txtappldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtappllastdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                int year1;
                year1 = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {
                    ddlYear.Items.Add(Convert.ToString(year1 + 1 - l));
                }
                ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                clear();
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string grouporusercode = string.Empty;

                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim() + "'";
                    grpusercd = " and usercode=='" + Convert.ToString(Session["group_code"]).Trim() + "'";
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                    grpusercd = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                }
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
                string extrafeename1 = d2.GetFunction("select value from COE_Master_Settings where settings='Exter Fee Name 1'");
                string extrafeename2 = d2.GetFunction("select value from COE_Master_Settings where settings='Exter Fee Name 2'");
                if (extrafeename1.Trim() != "" && extrafeename1.Trim() != "0")
                {
                    txtextrafeename.Text = extrafeename1;
                }
                if (extrafeename2.Trim() != "" && extrafeename2.Trim() != "0")
                {
                    txtextrafeename2.Text = extrafeename2;
                }

            }
        }
        catch (Exception ex)
        {
        }

    }

    public void loadcollege()
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
        ds = da.select_method("bind_college", hat, "sp");
        ddlcollege.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.Enabled = true;
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = da.BindBatch();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            //lblerror.Text = ex.ToString();
            //lblerror.Visible = true;
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", ddlcollege.SelectedValue.ToString());
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            //lblerror.Text = ex.ToString();
            //lblerror.Visible = true;
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    public void bindbranch()
    {
        try
        {
            ddlbranch.Items.Clear();
            has.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", ddlcollege.SelectedValue.ToString());
            has.Add("user_code", usercode);
            ds = da.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindsemester()
    {
        try
        {
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddlcollege.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and degree_code='" + ddlbranch.SelectedValue.ToString() + "' order by NDurations desc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
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
            else
            {
                strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddlcollege.SelectedValue.ToString() + " and degree_code='" + ddlbranch.SelectedValue.ToString() + "' order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
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
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "Exam Application");
        }
    }

    public void bindSection()
    {
        try
        {
            ddlSec.Enabled = false;
            ddlSec.Items.Clear();
            hat.Clear();
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            if (ddlbranch.Items.Count > 0 && ddlbatch.Items.Count > 0)
            {
                ds = d2.BindSectionDetail(ddlbatch.SelectedValue, ddlbranch.SelectedValue);
            }
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
                ddlSec.Items.Insert(0, "All");
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void clear()
    {
        FpSpread1.Visible = false;
        printbtn.Visible = false;
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("default.aspx");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlYear.Items.Count > 0)
            {
                ddlYear.SelectedIndex = 0;
            }
            if (ddlMonth.Items.Count > 0)
            {
                ddlMonth.SelectedIndex = 0;
            }
            bindbatch();
            binddegree();
            bindbranch();
            bindsemester();
            bindSection();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlYear.Items.Count > 0)
            {
                ddlYear.SelectedIndex = 0;
            }
            if (ddlMonth.Items.Count > 0)
            {
                ddlMonth.SelectedIndex = 0;
            }
            bindsemester();
            bindSection();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlYear.Items.Count > 0)
            {
                ddlYear.SelectedIndex = 0;
            }
            if (ddlMonth.Items.Count > 0)
            {
                ddlMonth.SelectedIndex = 0;
            }
            bindbranch();
            bindsemester();
            bindSection();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlYear.Items.Count > 0)
            {
                ddlYear.SelectedIndex = 0;
            }
            if (ddlMonth.Items.Count > 0)
            {
                ddlMonth.SelectedIndex = 0;
            }
            bindsemester();
            bindSection();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlYear.Items.Count > 0)
            {
                ddlYear.SelectedIndex = 0;
            }
            if (ddlMonth.Items.Count > 0)
            {
                ddlMonth.SelectedIndex = 0;
            }
            bindSection();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlYear.Items.Count > 0)
            {
                ddlYear.SelectedIndex = 0;
            }
            if (ddlMonth.Items.Count > 0)
            {
                ddlMonth.SelectedIndex = 0;
            }
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        loadexamdetails();
    }

    public void loadexamdetails()
    {
        try
        {
            bool isIncludeRedo = true;
            FpSpread1.Visible = false;
            printbtn.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 8;
            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[1].Width = 80;
            FpSpread1.Sheets[0].Columns[2].Width = 130;
            FpSpread1.Sheets[0].Columns[3].Width = 180;
            FpSpread1.Sheets[0].Columns[4].Width = 90;
            FpSpread1.Width = 810;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;
            FpSpread1.Sheets[0].Columns[5].Locked = true;
            FpSpread1.Sheets[0].Columns[6].Locked = true;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].SheetCorner.RowCount = 1;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem / Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Status";
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Fee Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
            if (Convert.ToString(Session["Rollflag"]).Trim() == "1")
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }
            if (Convert.ToString(Session["Regflag"]).Trim() == "1")
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = chkcell1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].FrozenRowCount = 1;
            chkcell1.AutoPostBack = true;
            FpSpread1.Sheets[0].AutoPostBack = false;
            string examyear = Convert.ToString(ddlYear.SelectedValue).Trim();
            string exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            string batchyear = Convert.ToString(ddlbatch.SelectedValue).Trim();
            string degreecode = Convert.ToString(ddlbranch.SelectedValue).Trim();
            string year = Convert.ToString(ddlbatch.SelectedValue).Trim();
            string degree = Convert.ToString(ddldegree.SelectedItem).Trim();
            string course = Convert.ToString(ddldegree.SelectedItem).Trim();
            string depart_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            string batchyearatt = Convert.ToString(ddlbatch.SelectedValue).Trim();
            section = string.Empty;
            qrysec = string.Empty;
            string IspassedoutSec = string.Empty;
            if (ddlSec.Items.Count > 0)
            {
                if (ddlSec.Enabled)
                {
                    if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "-1" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all")
                    {
                        section = Convert.ToString(ddlSec.SelectedItem.Text).Trim();
                        qrysec = " and Ltrim(rtrim(isnull(r.sections,'')))='" + section.Trim() + "'";
                        IspassedoutSec = "  and Ltrim(rtrim(isnull(Sections,'')))='" + section.Trim() + "'";
                    }
                }
                else
                {
                    section = string.Empty;
                }
            }
            string strdisstu = " and r.delflag=0 ";
            if (chkdiscontine.Checked == true)
            {
                strdisstu = string.Empty;
            }
            string sem = Convert.ToString(ddlsem.SelectedValue).Trim();
            string qryPassedOut = string.Empty;
            if (chkpassout.Checked == true)
            {
                int semval = ddlsem.Items.Count;
                semval++;
                sem = Convert.ToString(semval).Trim();
                //qryPassedOut = "  and Roll_No not in (select m.roll_no from mark_entry m where m.result='Pass')";
            }
            string qryExternalCheck = string.Empty;
            if (chkExternalMark.Checked)
            {
                //m.total>=s.mintotal m.external_mark>=s.min_ext_marks
                qryExternalCheck = " and (m1.result='Pass' or m1.external_mark>=s1.min_ext_marks)";
            }
            else
            {
                qryExternalCheck = " and m1.result='Pass'";
            }
            qry = "select sr.BatchYear,sr.DegreeCode,r.Current_Semester,sr.Semester,r.App_No,r.Roll_No from Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and sr.RedoType='1' ";
            DataSet dsRedoStudents = new DataSet();
            dsRedoStudents = da.select_method_wo_parameter(qry, "text");

            //string studinfo = "select distinct len(r.reg_no),r.App_No,r.reg_no,r.stud_name,r.roll_no,r.batch_year,r.current_semester,ISNULL(isRedo,'0') as isRedo from registration r where  r.degree_code='" + depart_code + "' and r.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' " + strdisstu + qrysec + " and exam_flag<>'debar' order by len(r.reg_no),r.reg_no,r.stud_name";
            DataSet dsstudinfo = new DataSet();
            string studinfo = string.Empty;
            if (!chkpassout.Checked)
            {
                studinfo = "select distinct len(r.reg_no)  as reglen,r.App_No,r.reg_no,r.stud_name,r.roll_no,r.batch_year,r.degree_code as degree_code,r.current_semester,ISNULL(r.isRedo,'0') as isRedo from registration r where  r.degree_code='" + depart_code + "' and r.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' " + strdisstu + qrysec + " and exam_flag<>'debar' and ISNULL(r.isRedo,'0')='0' ";
                if (isIncludeRedo)
                {
                    studinfo += " union select distinct len(r.reg_no) as reglen,r.App_No,r.reg_no,r.stud_name,r.roll_no,sr.BatchYear as batch_year,sr.DegreeCode as degree_code,r.current_semester,ISNULL(r.isRedo,'0') as isRedo from Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and sr.RedoType='1' and sr.BatchYear='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and sr.DegreeCode='" + depart_code + "' " + strdisstu + qrysec + " and exam_flag<>'debar' and ISNULL(r.isRedo,'0')='1' ";
                }
                studinfo += "order by reglen,r.reg_no,r.stud_name";

                dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            }

            else if (chkpassout.Checked)
            {
                //studinfo = "select distinct len(r.reg_no),r.App_No,r.reg_no,r.stud_name,r.roll_no,r.batch_year,r.current_semester,ISNULL(isRedo,'0') as isRedo from Registration r,subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and r.delflag=0 and exam_flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1  and sc.semester<='" + sem + "' and s.subject_no not in (select m.subject_no from mark_entry m where  r.Roll_No=m.roll_no " + qryExternalCheck + " and r.degree_code='" + degreecode + "' and r.batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "'  and exam_flag<>'debar' " + strdisstu + qrysec + ") and r.degree_code='" + degreecode + "' and r.batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "' " + strdisstu + qrysec + "  and exam_flag<>'debar' order by len(r.reg_no),r.reg_no,r.stud_name";
                //studinfo = "select distinct len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year,r.current_semester from subject s,registration r,subjectChooser sc,sub_sem ss,syllabus_master sy  where r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and r.delflag=0 and exam_flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no in(select r1.roll_no from registration r1 where batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "' and degree_code='" + degreecode + "') " + qryExternalCheck + ")  and sc.roll_no in(select r1.roll_no from registration r1 where batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "' and degree_code='" + degreecode + "')";


                //--------------------------command by Rajkumar 
                //studinfo = "select distinct len(r.reg_no) as reglen,r.App_No,r.reg_no,r.stud_name,r.roll_no,r.batch_year as batch_year,r.degree_code as degree_code,r.current_semester,ISNULL(r.isRedo,'0') as isRedo from Registration r,subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and r.delflag=0 and exam_flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1  and sc.semester<='" + sem + "' and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and r.Roll_No=m1.roll_no  " + qryExternalCheck + " and r.degree_code='" + degreecode + "' and r.batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "'  and exam_flag<>'debar' " + strdisstu + qrysec + ") and ISNULL(r.isRedo,'0')='0'  and r.degree_code='" + degreecode + "' and r.batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "' " + strdisstu + qrysec + "  and exam_flag<>'debar'";// order by reglen,r.reg_no,r.stud_name
                //if (isIncludeRedo)
                //{
                //studinfo += " union select distinct len(r.reg_no) as reglen,r.App_No,r.reg_no,r.stud_name,r.roll_no,sr.BatchYear as batch_year,sr.DegreeCode as degree_code,r.current_semester,ISNULL(isRedo,'0') as isRedo from Registration r,subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No  and r.Roll_No=sc.roll_no and sr.BatchYear=sy.Batch_Year and sr.DegreeCode=sy.degree_code and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and r.delflag=0 and exam_flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1  and sc.semester<='" + sem + "' and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and r.Roll_No=m1.roll_no " + qryExternalCheck + " and sr.BatchYear='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "' and sr.DegreeCode='" + degreecode + "' and exam_flag<>'debar' " + strdisstu + qrysec + ") and sr.RedoType='1'  and ISNULL(r.isRedo,'0')='1' and sr.DegreeCode='" + degreecode + "' and sr.BatchYear='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "' " + strdisstu + qrysec + "  and exam_flag<>'debar'";// order by reglen,r.reg_no,r.stud_name";
                //}
                //studinfo += "order by reglen,r.reg_no,r.stud_name";
                //----------------------------------

                studinfo = "select * from PreExamPassedOutStudents where batch_year='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "' and degree_code='" + degreecode + "' and current_semester='" + sem + "'" + IspassedoutSec;

                dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");

            }

            //and (m.result='Pass' or m.external_mark>=s.min_ext_marks)

            //if (chkpassout.Checked)
            //{
            //    studinfo = "select distinct len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year,r.current_semester from registration r where  r.degree_code=" + depart_code + " and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + strdisstu + " and exam_flag<>'debar' order by len(r.reg_no),r.reg_no,r.stud_name";
            //}
            //DataSet dsstudinfo = new DataSet();and sr.RedoType='1'  and ISNULL(r.isRedo,'0')='1'
            string qryExamMonthYear = string.Empty;
            bool isSupplementaryExam = false;
            string SupplementaryExam = string.Empty;
            bool.TryParse(SupplementaryExam.Trim(), out isSupplementaryExam);
            chkIsSupplementaryExam.Checked = isSupplementaryExam;
            string examMonthNew = string.Empty;
            string examYearNew = string.Empty;
            if (ddlMonth.Items.Count > 0)
            {
                if (Convert.ToString(ddlMonth.SelectedValue).Trim() != "0")
                {
                    examMonthNew = Convert.ToString(ddlMonth.SelectedValue).Trim();
                    qryExamMonthYear = " and ed.exam_month='" + examMonthNew + "'";
                }
            }
            if (ddlYear.Items.Count > 0)
            {
                if (Convert.ToString(ddlYear.SelectedValue).Trim() != "0")
                {
                    examYearNew = Convert.ToString(ddlYear.SelectedValue).Trim();
                    qryExamMonthYear += " and exam_year='" + examYearNew + "'";
                }
            }
            if (!string.IsNullOrEmpty(examYearNew) && !string.IsNullOrEmpty(examMonthNew))
            {
                string qrynew = "select * from Exam_details where batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and degree_code='" + depart_code + "' and current_semester='" + sem + "' and Exam_Month='" + examMonthNew + "' and Exam_year='" + examYearNew + "'";
                DataSet dsExamDetailNew = new DataSet();
                dsExamDetailNew = d2.select_method_wo_parameter(qrynew, "Text");
                if (dsExamDetailNew.Tables.Count > 0 && dsExamDetailNew.Tables[0].Rows.Count > 0)
                {
                    SupplementaryExam = Convert.ToString(dsExamDetailNew.Tables[0].Rows[0]["isSupplementaryExam"]).Trim();
                    bool.TryParse(SupplementaryExam.Trim(), out isSupplementaryExam);
                    chkIsSupplementaryExam.Checked = isSupplementaryExam;
                }
            }
            if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
            {
                //string examdetai = "select distinct roll_no,ea.total_fee,ea.cost_appl,ea.cost_mark,ea.extra_fee,ea.extra_fee2,ea.fine,ed.exam_code,ed.Exam_Month,ed.Exam_year,CONVERT(nvarchar(15),ea.applied_date,103) appldate,CONVERT(nvarchar(15),ea.LastDate,103) lastdate,isnull(isSupplementaryExam,'0') as  isSupplementaryExam from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and ed.degree_code='" + depart_code + "' and ed.current_semester='" + sem + "' " + qryExamMonthYear;
                string examdetai = "select distinct r.App_No,ea.roll_no,ea.total_fee,ea.cost_appl,ea.cost_mark,ea.extra_fee,ea.extra_fee2,ea.fine,ed.exam_code,ed.Exam_Month,ed.Exam_year,CONVERT(nvarchar(15),ea.applied_date,103) appldate,CONVERT(nvarchar(15),ea.LastDate,103) lastdate,isnull(isSupplementaryExam,'0') as  isSupplementaryExam from Exam_Details ed,exam_application ea,exam_appl_details ead,Registration r where r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and ea.roll_no=r.Roll_No  and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and ed.degree_code='" + depart_code + "' " + qryExamMonthYear;
                if (isIncludeRedo)
                {
                    examdetai += " union select distinct r.App_No,ea.roll_no,ea.total_fee,ea.cost_appl,ea.cost_mark,ea.extra_fee,ea.extra_fee2,ea.fine,ed.exam_code,ed.Exam_Month,ed.Exam_year,CONVERT(nvarchar(15),ea.applied_date,103) appldate,CONVERT(nvarchar(15),ea.LastDate,103) lastdate,isnull(isSupplementaryExam,'0') as  isSupplementaryExam from Exam_Details ed,exam_application ea,exam_appl_details ead,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and sr.DegreeCode=ed.degree_code and sr.BatchYear=ed.batch_year and ea.roll_no=r.Roll_No and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and ed.degree_code='" + depart_code + "' " + qryExamMonthYear;

                }
                examdetai += " order by r.App_No";
                if (chkpassout.Checked == true)
                {
                    //examdetai = "select distinct roll_no,ea.total_fee,ea.cost_appl,ea.cost_mark,ea.extra_fee,ea.extra_fee2,ea.fine,ed.exam_code,ed.Exam_Month,ed.Exam_year,CONVERT(nvarchar(15),ea.applied_date,103) appldate,CONVERT(nvarchar(15),ea.LastDate,103) lastdate,isnull(isSupplementaryExam,'0') as isSupplementaryExam from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and ed.degree_code='" + depart_code + "' and ed.current_semester='" + sem + "' " + qryExamMonthYear;
                    examdetai = "select distinct r.App_No,ea.roll_no,ea.total_fee,ea.cost_appl,ea.cost_mark,ea.extra_fee,ea.extra_fee2,ea.fine,ed.exam_code,ed.Exam_Month,ed.Exam_year,CONVERT(nvarchar(15),ea.applied_date,103) appldate,CONVERT(nvarchar(15),ea.LastDate,103) lastdate,isnull(isSupplementaryExam,'0') as isSupplementaryExam from Exam_Details ed,exam_application ea,exam_appl_details ead,Registration r where r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and ea.roll_no=r.Roll_No and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and ed.degree_code='" + depart_code + "' " + qryExamMonthYear;
                    if (isIncludeRedo)
                    {
                        examdetai += " union select distinct r.App_No,ea.roll_no,ea.total_fee,ea.cost_appl,ea.cost_mark,ea.extra_fee,ea.extra_fee2,ea.fine,ed.exam_code,ed.Exam_Month,ed.Exam_year,CONVERT(nvarchar(15),ea.applied_date,103) appldate,CONVERT(nvarchar(15),ea.LastDate,103) lastdate,isnull(isSupplementaryExam,'0') as isSupplementaryExam from Exam_Details ed,exam_application ea,exam_appl_details ead,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and sr.DegreeCode=ed.degree_code and sr.BatchYear=ed.batch_year and ea.roll_no=r.Roll_No and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' and ed.degree_code='" + depart_code + "' " + qryExamMonthYear;
                    }
                    examdetai += " order by r.App_No";
                }
                DataSet dsexam = da.select_method_wo_parameter(examdetai, "Text");
                int sno = 0;
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {
                    string regno = string.Empty;
                    string studname = string.Empty;
                    string rollno = string.Empty;
                    FpSpread1.Visible = true;
                    printbtn.Visible = true;
                    sno++;

                    batchyear = Convert.ToString(dsstudinfo.Tables[0].Rows[studcount]["batch_year"]).Trim();
                    string degreeCode = Convert.ToString(dsstudinfo.Tables[0].Rows[studcount]["degree_code"]).Trim();
                    string appNo = Convert.ToString(dsstudinfo.Tables[0].Rows[studcount]["app_no"]).Trim();
                    regno = Convert.ToString(dsstudinfo.Tables[0].Rows[studcount]["reg_no"]).Trim();
                    studname = Convert.ToString(dsstudinfo.Tables[0].Rows[studcount]["stud_name"]).Trim();
                    rollno = Convert.ToString(dsstudinfo.Tables[0].Rows[studcount]["roll_no"]).Trim();
                    string redoStudent = Convert.ToString(dsstudinfo.Tables[0].Rows[studcount]["isRedo"]).Trim();

                    bool isRedo = false;
                    bool.TryParse(redoStudent.Trim(), out isRedo);
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    if (isRedo)
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Red;
                    }

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(redoStudent.Trim()).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(appNo.Trim()).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(degreeCode.Trim()).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(batchyear.Trim()).Trim();


                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = studname;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsstudinfo.Tables[0].Rows[studcount]["current_semester"]).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                    dsexam.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    DataView dvexam = dsexam.Tables[0].DefaultView;
                    dvexam.Sort = "exam_code asc";
                    if (dvexam.Count > 0)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Applied";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.Blue;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dvexam[0]["total_fee"]).Trim();
                        ddlYear.Text = Convert.ToString(dvexam[0]["Exam_year"]).Trim();
                        ddlMonth.Text = Convert.ToString(dvexam[0]["Exam_Month"]).Trim();
                        //SupplementaryExam = Convert.ToString(dvexam[0]["isSupplementaryExam"]).Trim();
                        //bool.TryParse(SupplementaryExam.Trim(), out isSupplementaryExam);
                        //chkIsSupplementaryExam.Checked = isSupplementaryExam;
                        if (!string.IsNullOrEmpty(Convert.ToString(dvexam[0]["appldate"]).Trim()))
                        {
                            txtappldate.Text = Convert.ToString(dvexam[0]["appldate"]).Trim();
                        }
                        else
                        {
                            txtappldate.Text = DateTime.Now.ToString("dd/MM/yyyy").Trim();
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dvexam[0]["lastdate"]).Trim()))
                        {
                            txtappllastdate.Text = Convert.ToString(dvexam[0]["lastdate"]).Trim();
                        }
                        else
                        {
                            txtappllastdate.Text = DateTime.Now.ToString("dd/MM/yyyy").Trim();
                        }
                        string applicationfee = Convert.ToString(dvexam[0]["cost_appl"]).Trim();
                        string stmtfee = Convert.ToString(dvexam[0]["cost_mark"]).Trim();
                        string extrfee = Convert.ToString(dvexam[0]["extra_fee"]).Trim();
                        string fineamount = Convert.ToString(dvexam[0]["fine"]).Trim();
                        string extrfee2 = Convert.ToString(dvexam[0]["extra_fee2"]).Trim();
                        if (applicationfee.Trim() != "")
                        {
                            double costfee = 0;// Convert.ToDouble(applicationfee);
                            double.TryParse(Convert.ToString(applicationfee).Trim(), out costfee);
                            costfee = Math.Round(costfee, 0, MidpointRounding.AwayFromZero);
                            if (costfee == 0)
                            {
                                applicationfee = string.Empty;
                            }
                            else
                            {
                                applicationfee = costfee.ToString();
                            }
                        }
                        if (stmtfee.Trim() != "")
                        {
                            double costfee = 0;// Convert.ToDouble(stmtfee);
                            double.TryParse(Convert.ToString(stmtfee).Trim(), out costfee);
                            costfee = Math.Round(costfee, 0, MidpointRounding.AwayFromZero);
                            if (costfee == 0)
                            {
                                stmtfee = string.Empty;
                            }
                            else
                            {
                                stmtfee = costfee.ToString();
                            }
                        }
                        if (extrfee.Trim() != "")
                        {
                            double costfee = 0;// Convert.ToDouble(extrfee);
                            double.TryParse(Convert.ToString(extrfee).Trim(), out costfee);
                            costfee = Math.Round(costfee, 0, MidpointRounding.AwayFromZero);
                            if (costfee == 0)
                            {
                                extrfee = string.Empty;
                            }
                            else
                            {
                                extrfee = costfee.ToString();
                            }
                        }
                        if (extrfee2.Trim() != "")
                        {
                            double costfee = 0; // Convert.ToDouble(extrfee2);
                            double.TryParse(Convert.ToString(extrfee2).Trim(), out costfee);
                            costfee = Math.Round(costfee, 0, MidpointRounding.AwayFromZero);
                            if (costfee == 0)
                            {
                                extrfee2 = string.Empty;
                            }
                            else
                            {
                                extrfee2 = costfee.ToString();
                            }
                        }
                        if (fineamount.Trim() != "")
                        {
                            double costfee = 0;// Convert.ToDouble(fineamount);
                            double.TryParse(Convert.ToString(fineamount).Trim(), out costfee);
                            costfee = Math.Round(costfee, 0, MidpointRounding.AwayFromZero);
                            if (costfee == 0)
                            {
                                fineamount = string.Empty;
                            }
                            else
                            {
                                fineamount = costfee.ToString();
                            }
                        }
                        //magesh 10.3.18
                        //txtapplfee.Text = applicationfee;
                        //txtstmtfee.Text = stmtfee;
                        txtextrafee.Text = extrfee;
                        txtfine.Text = fineamount;
                        txtextrafee2.Text = extrfee2;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Not Applied";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.Black;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = string.Empty;
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = chkcell;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            else
            {
                clear();
                lblerror.Text = "No Records Found";
                lblerror.Visible = true;
            }
            string totalrows = FpSpread1.Sheets[0].RowCount.ToString();
            FpSpread1.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread1.Height = (Convert.ToInt32(totalrows) * 20) + 40;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                string[] spiltspreadname = ctrlname.Split('$');
                if (spiltspreadname.GetUpperBound(0) > 1)
                {
                    string getrowxol = spiltspreadname[3].ToString().Trim();
                    string[] spr = getrowxol.Split(',');
                    if (spr.GetUpperBound(0) == 1)
                    {
                        int arow = Convert.ToInt32(spr[0]);
                        int acol = Convert.ToInt32(spr[1]);
                        if (arow == 0 && acol > 4)
                        {
                            string setval = e.EditValues[acol].ToString();
                            int setvalcel = 0;
                            if (setval.Trim().ToLower() == "true" || setval.Trim() == "1")
                            {
                                setvalcel = 1;
                            }
                            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                            {
                                FpSpread1.Sheets[0].Cells[r, acol].Value = setvalcel;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnapply_Click(object sender, EventArgs e)
    {
        try
        {
            string appldate = txtappldate.Text.ToString();
            string lastappladet = txtappllastdate.Text.ToString();
            DateTime dtappl = new DateTime();
            DateTime dtlastappl = new DateTime();
            string[] spd;
            if (!string.IsNullOrEmpty(appldate))
            {
                spd = appldate.Split('/');
                dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Applied Date And Then Proceed";
                return;
            }
            if (!string.IsNullOrEmpty(lastappladet))
            {
                //spd = appldate.Split('/');
                spd = lastappladet.Split('/');
                dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Last Date For Application And Then Proceed";
                return;
            }
            if (dtappl > dtlastappl)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            if (chkInclFinMand.Checked)
            {
                ArrayList arrMandFees = MandatoryFees();
                if (arrMandFees.Count == 0)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Set Mandatory Fees";
                }
                else
                {
                    getFinanceAppliedAmount();
                }
            }
            else
            {
                getFinanceNotAppliedAmount();
            }
            string examfeenameset = "if exists(select * from COE_Master_Settings where settings='Exter Fee Name 1')";
            examfeenameset = examfeenameset + " update COE_Master_Settings set value='" + txtextrafeename.Text + "' where settings='Exter Fee Name 1' else";
            examfeenameset = examfeenameset + " insert into COE_Master_Settings(settings,value) values('Exter Fee Name 1','" + txtextrafeename.Text + "')";
            int valu = d2.update_method_wo_parameter(examfeenameset, "Text");
            examfeenameset = "if exists(select * from COE_Master_Settings where settings='Exter Fee Name 2')";
            examfeenameset = examfeenameset + " update COE_Master_Settings set value='" + txtextrafeename2.Text + "' where settings='Exter Fee Name 2' else";
            examfeenameset = examfeenameset + " insert into COE_Master_Settings(settings,value) values('Exter Fee Name 2','" + txtextrafeename2.Text + "')";
            valu = d2.update_method_wo_parameter(examfeenameset, "Text");
        }

        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Examination Apllication"); 
        }

    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            string collcode = ddlcollege.SelectedValue.ToString();
            if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Convert.ToString(ddlcollege.SelectedValue).Trim() + ".jpeg")))
            {
                DataSet dsstuphoto = da.select_method_wo_parameter("select fileupload from tbl_notification where viewrs='Printmaster' and College_Code='" + ddlcollege.SelectedValue.ToString() + "'", "Text");
                if (dsstuphoto.Tables[0].Rows.Count > 0)
                {
                    if (dsstuphoto.Tables[0].Rows[0]["fileupload"] != null && Convert.ToString(dsstuphoto.Tables[0].Rows[0]["fileupload"]).Trim() != "")
                    {
                        byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["fileupload"];
                        MemoryStream memoryStream = new MemoryStream();
                        memoryStream.Write(file, 0, file.Length);
                        if (file.Length > 0)
                        {
                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                            System.Drawing.Image thumb = imgx.GetThumbnailImage(2630, 440, null, IntPtr.Zero);
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Convert.ToString(ddlcollege.SelectedValue).Trim() + ".jpeg")))
                            {
                                thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Convert.ToString(ddlcollege.SelectedValue).Trim() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                //NetworkShare.SaveOnNetworkShare(thumb, HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg"), ImageFormat.Jpeg);
                            }
                        }
                        memoryStream.Dispose();
                        memoryStream.Close();
                    }
                }
            }
            if (rbFormat1.Checked == true)
            {
                #region Format1
                FpSpread1.SaveChanges();
                if (Convert.ToString(ddlYear.SelectedValue).Trim() == "0")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select The Exam Year And Then Proceed";
                    return;
                }
                if ((Convert.ToString(ddlMonth.SelectedValue).Trim() == "0"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select The Exam Month And Then Proceed";
                    return;
                }
                int insupdateval = 0;
                string degreecode = Convert.ToString(ddlbranch.SelectedValue).Trim();
                string batchyear = Convert.ToString(ddlbatch.SelectedValue).Trim();
                string sem = Convert.ToString(ddlsem.SelectedValue).Trim();
                if (chkpassout.Checked == true)
                {
                    int semval = ddlsem.Items.Count;
                    semval++;
                    sem = semval.ToString();
                }
                string exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
                string examyear = Convert.ToString(ddlYear.SelectedValue).Trim();
                collegecode = Convert.ToString(ddlcollege.SelectedValue).Trim();
                string appldate = Convert.ToString(txtappldate.Text).Trim();
                string[] spd = appldate.Split('/');
                DateTime dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                string lastappladet = txtappllastdate.Text.ToString(); spd = appldate.Split('/');
                spd = lastappladet.Split('/');
                DateTime dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                qry = "select value from COE_Master_Settings where settings='Fore Noon'";
                string foreNoon = d2.GetFunctionv(qry);
                qry = "select value from COE_Master_Settings where settings='After Noon'";
                string afterNoon = d2.GetFunctionv(qry);
                if (dtappl > dtlastappl)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                    return;
                }
                string applfee = Convert.ToString(txtapplfee.Text).Trim().Trim();
                string smtfee = Convert.ToString(txtstmtfee.Text).Trim();
                string extrafee = Convert.ToString(txtextrafee.Text).Trim();
                string fineamount = Convert.ToString(txtfine.Text).Trim();
                double appltotalcost = 0;
                double applcost = 0;
                if (applfee.Trim() != "")
                {
                    applcost = Convert.ToDouble(applfee);
                }
                double stmtcost = 0;
                if (smtfee.Trim() != "")
                {
                    stmtcost = Convert.ToDouble(smtfee);
                }
                double extracost = 0;
                if (extrafee.Trim() != "")
                {
                    extracost = Convert.ToDouble(extrafee);
                }
                double fineamo = 0;
                if (fineamount.Trim() != "")
                {
                    fineamo = Convert.ToDouble(fineamount);
                }
                appltotalcost = applcost + stmtcost + extracost + fineamo;
                bool setflag = false;
                for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                {
                    int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                    if (stva == 1)
                    {
                        setflag = true;
                    }
                }
                if (setflag == false)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select The Student And Then Proceed";
                    return;
                }
                Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
                Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
                Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
                Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
                Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
                Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
                Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
                Font font31small = new Font("Palatino Linotype", 7, FontStyle.Regular);
                string coename = string.Empty;
                string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                string Collegename = string.Empty;
                string aff = string.Empty;
                string collacr = string.Empty;
                string dispin = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                    aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                    string[] strpa = aff.Split(',');
                    aff = strpa[0];
                    coename = ds.Tables[0].Rows[0]["coe"].ToString();
                    collacr = ds.Tables[0].Rows[0]["acr"].ToString();
                    dispin = ds.Tables[0].Rows[0]["districtpin"].ToString();
                }
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                DataSet printds = new DataSet();
                string studinfo = "select sy.semester,r.app_no, r.Current_Semester, r.batch_year,exam_month,exam_year,r.stud_name,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp)  end as countryp,r.degree_code,subject_code,subject_name,isnull(total_fee,0) as total_fee,convert(decimal(5,0),ROUND(fee,0)) as fee,isnull(ea.extra_fee,0) as extra_fee,isnull(ea.extra_fee2,0) as extra_fee2,ead.attempts,ea.roll_no,isnull(ea.fee_amount,0) fee_amount,isnull(ea.fine,0) fine,isnull(ea.cost_appl,0) cost_appl,isnull(cost_mark,0) as cost_mark,case when r.current_semester=sy.semester then '0' else '1' end as Paper,ss.subject_type from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy,registration r,applyn a where a.app_no=r.App_No and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ss.syll_code=sy.syll_code and r.roll_no=ea.roll_no  and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "' order by r.app_no,sy.semester desc,s.subjectpriority";
                studinfo = studinfo + " select dd.dept_name,c.course_name,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id ";
                printds.Clear();
                printds = da.select_method_wo_parameter(studinfo, "Text");

                DataSet ds1 = new DataSet();
                string selectqry = "select template from Master_Settings where usercode='" + Convert.ToString(Session["usercode"]) + "' and settings='Exam Application Format Settings'";
                DataView dv = new DataView();
                string semroman = string.Empty;
                Gios.Pdf.PdfPage mypdfpage;
                for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                {
                    string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                    string regnnono = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                    int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                    if (stva == 1)
                    {
                        //printds.Tables[1].DefaultView.RowFilter = "degree_code='" + ddlbranch.SelectedItem.Value.ToString() + "'";
                        int currentpaper = 0;
                        int arearcount = 0;
                        int theoryCount = 0;
                        int Practicalcount = 0;
                        int project = 0;
                        int others = 0;
                        printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and  subject_type like '%Theory%'";
                        theoryCount = printds.Tables[0].DefaultView.Count;

                        printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and subject_type like '%Practical%'";
                        Practicalcount = printds.Tables[0].DefaultView.Count;

                        printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and subject_type like '%Project%'";
                        project = printds.Tables[0].DefaultView.Count;

                        printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and subject_type like '%Others%'";
                        others = printds.Tables[0].DefaultView.Count;

                        printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and paper=0";
                        currentpaper = printds.Tables[0].DefaultView.Count;
                        printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and paper=1";
                        arearcount = printds.Tables[0].DefaultView.Count;
                        printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                        dv = printds.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            mypdfpage = mydoc.NewPage();
                            string studnmae = dv[0]["stud_name"].ToString();
                            string seminfo = dv[0]["Current_Semester"].ToString();
                            string stdappno = dv[0]["app_no"].ToString();
                            string degreecodee = dv[0]["degree_code"].ToString();
                            string father_name = Convert.ToString(dv[0]["parent_name"]);
                            string studaddr = Convert.ToString(dv[0]["parent_addressP"]);
                            string studstreetname = Convert.ToString(dv[0]["Streetp"]);
                            string studcity = Convert.ToString(dv[0]["Cityp"]);
                            string studdist = Convert.ToString(dv[0]["Districtp"]);
                            string studsate = Convert.ToString(dv[0]["parent_statep"]);
                            string studcountry = Convert.ToString(dv[0]["countryp"]);
                            string studpincode = Convert.ToString(dv[0]["parent_pincodep"]);
                            string studmob_no = Convert.ToString(dv[0]["student_mobile"]);
                            string studFathermob_no = Convert.ToString(dv[0]["parentF_Mobile"]);
                            string studresidentialaddress = "", studresidentialaddress1 = string.Empty;
                            if (studaddr.Trim().Trim(',') != "")
                            {
                                studresidentialaddress = studaddr.Trim().Trim(',');
                            }
                            //if (studstreetname.Trim().Trim(',') != "")
                            //{
                            //    if (studresidentialaddress != "")
                            //    {
                            //        studresidentialaddress += ", " + studstreetname.Trim().Trim(',');
                            //    }
                            //    else
                            //    {
                            //        studresidentialaddress = studstreetname.Trim().Trim(',');
                            //    }
                            //}
                            //if (studcity.Trim().Trim(',') != "")
                            //{
                            //    if (studresidentialaddress != "")
                            //    {
                            //        studresidentialaddress += ", " + studcity.Trim().Trim(',');
                            //    }
                            //    else
                            //    {
                            //        studresidentialaddress = studcity.Trim().Trim(',');
                            //    }
                            //}
                            //if (studdist.Trim().Trim(',') != "")
                            //{
                            //    if (studresidentialaddress != "")
                            //    {
                            //        studresidentialaddress += ", " + studdist.Trim().Trim(',');
                            //    }
                            //    else
                            //    {
                            //        studresidentialaddress = studdist.Trim().Trim(',');
                            //    }
                            //}
                            //if (studsate.Trim().Trim(',') != "")
                            //{
                            //    if (studresidentialaddress != "")
                            //    {
                            //        studresidentialaddress += ", " + studsate.Trim().Trim(',');
                            //    }
                            //    else
                            //    {
                            //        studresidentialaddress = studsate.Trim().Trim(',');
                            //    }
                            //}
                            //if (studcountry.Trim().Trim(',') != "")
                            //{
                            //    if (studresidentialaddress != "")
                            //    {
                            //        studresidentialaddress += ", " + studcountry.Trim().Trim(',');
                            //    }
                            //    else
                            //    {
                            //        studresidentialaddress = studcountry.Trim().Trim(',');
                            //    }
                            //}
                            //if (studpincode.Trim().Trim(',') != "")
                            //{
                            //    if (studresidentialaddress != "")
                            //    {
                            //        studresidentialaddress += ", Pincode : " + studpincode.Trim().Trim(',');
                            //    }
                            //    else
                            //    {
                            //        studresidentialaddress = "Pincode : " + studpincode.Trim().Trim(',');
                            //    }
                            //}
                            PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
                            PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                            mypdfpage.Add(pr1);
                            int coltop = 25;
                            PdfTextArea ptc;
                            #region STudent Photo
                            string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                            MemoryStream memoryStream = new MemoryStream();
                            DataSet dsstdpho = new DataSet();
                            dsstdpho.Clear();
                            dsstdpho.Dispose();
                            dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                            if (dsstdpho.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                                    {
                                    }
                                    else
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        //NetworkShare.SaveOnNetworkShare(thumb, HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), ImageFormat.Jpeg);
                                    }
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"));
                                mypdfpage.Add(LogoImage2, 480, 30, 300);
                            }
                            else
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpeg"));
                                mypdfpage.Add(LogoImage2, 480, 30, 300);
                            }
                            #endregion
                            #region TOP DETAILS
                            if (chkheadimage.Checked == false)
                            {
                                #region Left Logo
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 20, 400);
                                }
                                #endregion
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
                                mypdfpage.Add(ptc);
                                //coltop = coltop + 20;
                                //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, collacr);
                                //mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                                ptc = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
                                mypdfpage.Add(ptc);
                                coltop = coltop + 15;
                                ptc = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, dispin);
                                mypdfpage.Add(ptc);
                                coltop = coltop + 15;
                                ptc = new PdfTextArea(font2small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
                                mypdfpage.Add(ptc);
                                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                                ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 30), System.Drawing.ContentAlignment.TopCenter, "APPLICATION FOR SEMESTER EXAMINATIONS " + strMonthName.ToUpper() + " - " + examyear + "");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                            }
                            else
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 25, 410);
                                }
                                coltop = coltop + 70;
                                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                                ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 105, 595, 30), System.Drawing.ContentAlignment.TopCenter, "APPLICATION FOR SEMESTER EXAMINATIONS " + strMonthName.ToUpper() + " - " + examyear + "");
                                mypdfpage.Add(ptc);
                            }
                            PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 28, 565, 0.01);
                            PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            #endregion
                            #region studinfo
                            coltop = coltop + 15;
                            if (seminfo == "1")
                            {
                                semroman = "I";
                            }
                            else if (seminfo == "2")
                            {
                                semroman = "II";
                            }
                            else if (seminfo == "3")
                            {
                                semroman = "III";
                            }
                            else if (seminfo == "4")
                            {
                                semroman = "IV";
                            }
                            else if (seminfo == "5")
                            {
                                semroman = "V";
                            }
                            else if (seminfo == "6")
                            {
                                semroman = "VI";
                            }
                            else if (seminfo == "7")
                            {
                                semroman = "VII";
                            }
                            else if (seminfo == "8")
                            {
                                semroman = "VIII";
                            }
                            Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(font3small, 5, 6, 3);
                            table1forpage2.Columns[0].SetWidth(120);
                            table1forpage2.Columns[1].SetWidth(3);
                            table1forpage2.Columns[2].SetWidth(140);
                            table1forpage2.Columns[3].SetWidth(100);
                            table1forpage2.Columns[4].SetWidth(3);
                            table1forpage2.Columns[5].SetWidth(135);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(0, 0).SetContent("Register Number & Semester ");
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 1).SetContent(":");
                            table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(0, 2).SetContent(regnnono.ToUpper() + " & " + semroman);
                            table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(0, 3).SetContent("Father's Name(English)");
                            table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 4).SetContent(":");
                            table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(0, 5).SetContent(father_name);
                            table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(1, 0).SetContent("Student's Name(English)");
                            table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(1, 1).SetContent(":");
                            table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(1, 2).SetContent(studnmae);
                            table1forpage2.Cell(1, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(1, 3).SetContent("Student's Name(Tamil)");
                            table1forpage2.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(1, 4).SetContent(":");
                            //foreach (PdfCell pr in table1forpage2.CellRange(1, 5, 1, 5).Cells)
                            //{
                            //    pr.RowSpan = 2;
                            //}
                            table1forpage2.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(2, 3).SetContent("");
                            table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(2, 0).SetContent("Address");
                            table1forpage2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(2, 1).SetContent(":");
                            table1forpage2.Cell(2, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(2, 2).SetContent(studresidentialaddress);
                            int addrlength = studresidentialaddress.Length;
                            if (addrlength > 40)
                            {
                                table1forpage2.Cell(2, 2).SetFont(font31small);
                            }
                            foreach (PdfCell pr in table1forpage2.CellRange(2, 2, 2, 2).Cells)
                            {
                                pr.ColSpan = 4;
                            }
                            //table1forpage2.Cell(2, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            //table1forpage2.Cell(2, 3).SetContent("Father's Mobile No.");
                            //table1forpage2.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(2, 4).SetContent(":");
                            //table1forpage2.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(2, 5).SetContent(studFathermob_no);
                            table1forpage2.Cell(3, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(3, 0).SetContent("Date of Birth");
                            table1forpage2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(3, 1).SetContent(":");
                            string dob = da.GetFunction("select convert(varchar,dob,103) dob from applyn  where app_no='" + stdappno + "'");
                            table1forpage2.Cell(3, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(3, 2).SetContent(dob);
                            table1forpage2.Cell(3, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(3, 3).SetContent("Father's Mobile No.");
                            table1forpage2.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(3, 4).SetContent(":");
                            table1forpage2.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(3, 5).SetContent(studFathermob_no);
                            table1forpage2.Cell(4, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(4, 0).SetContent("Degree & Branch");
                            table1forpage2.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(4, 1).SetContent(":");
                            string degreebranch = da.GetFunction("select (c.course_name + ' - '+dd.dept_name) as degree,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id  and degree_code='" + degreecodee + "'");
                            table1forpage2.Cell(4, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(4, 2).SetContent(degreebranch);
                            table1forpage2.Cell(4, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(4, 3).SetContent("Mobile Number");
                            table1forpage2.Cell(4, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(4, 4).SetContent(":");
                            table1forpage2.Cell(4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(4, 5).SetContent(studmob_no);
                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 122, 565, 200));//sr
                            mypdfpage.Add(newpdftabpage2);
                            coltop = coltop + 85;
                            tlinerect = new PdfArea(mydoc, 15, coltop + 33, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            #endregion
                            table1forpage2 = mydoc.NewTable(font4small, dv.Count + 1, 10, 4);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2.Columns[0].SetWidth(20);
                            table1forpage2.Columns[1].SetWidth(50);
                            table1forpage2.Columns[2].SetWidth(130);
                            table1forpage2.Columns[3].SetWidth(30);
                            table1forpage2.Columns[4].SetWidth(15);
                            table1forpage2.Columns[5].SetWidth(20);
                            table1forpage2.Columns[6].SetWidth(50);
                            table1forpage2.Columns[7].SetWidth(130);
                            table1forpage2.Columns[8].SetWidth(30);
                            table1forpage2.Columns[9].SetWidth(15);
                            mypdfpage.Add(newpdftabpage2);
                            coltop = coltop + 85;
                            tlinerect = new PdfArea(mydoc, 15, 245, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 38, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 95, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 246, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 275, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 293, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 323, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 375, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 532, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 557, 228, 0.01, 387);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 15, 615, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 0).SetContent("Sem");
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 1).SetContent("SubCode");
                            table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 2).SetContent("Subject Title");
                            table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 3).SetContent("(Rs)");
                            table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 4).SetContent("");
                            table1forpage2.Cell(0, 0).SetCellPadding(6);
                            table1forpage2.Cell(0, 1).SetCellPadding(6);
                            table1forpage2.Cell(0, 2).SetCellPadding(6);
                            table1forpage2.Cell(0, 3).SetCellPadding(6);
                            table1forpage2.Cell(0, 4).SetCellPadding(6);
                            table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 5).SetContent("Sem");
                            table1forpage2.Cell(0, 6).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 6).SetContent("SubCode");
                            table1forpage2.Cell(0, 7).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 7).SetContent("Subject Title");
                            table1forpage2.Cell(0, 8).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 8).SetContent("(Rs)");
                            table1forpage2.Cell(0, 9).SetContentAlignment(ContentAlignment.TopCenter);
                            table1forpage2.Cell(0, 9).SetContent("");
                            table1forpage2.Cell(0, 5).SetCellPadding(6);
                            table1forpage2.Cell(0, 6).SetCellPadding(6);
                            table1forpage2.Cell(0, 7).SetCellPadding(6);
                            table1forpage2.Cell(0, 8).SetCellPadding(6);
                            table1forpage2.Cell(0, 9).SetCellPadding(6);
                            double currentPaperCost = 0;
                            double arrearPaperCost = 0;
                            int j = 0;
                            for (int i = 0; i < dv.Count; i++)
                            {
                                if (i < 20)
                                {
                                    table1forpage2.Cell(i + 1, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1forpage2.Cell(i + 1, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1forpage2.Cell(i + 1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                    table1forpage2.Cell(i + 1, 3).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1forpage2.Cell(i + 1, 0).SetCellPadding(3);
                                    table1forpage2.Cell(i + 1, 1).SetCellPadding(3);
                                    table1forpage2.Cell(i + 1, 2).SetCellPadding(3);
                                    table1forpage2.Cell(i + 1, 3).SetCellPadding(3);
                                    table1forpage2.Cell(i + 1, 4).SetCellPadding(3);
                                    if (i == 1)
                                    {
                                        table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                        table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                        table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                        table1forpage2.Cell(i + 1, 3).SetContent(dv[i]["fee"].ToString() + " ");
                                    }
                                    else
                                    {
                                        table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                        table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                        table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                        table1forpage2.Cell(i + 1, 3).SetContent(dv[i]["fee"].ToString() + " ");
                                    }
                                    double value = 0;
                                    if (Convert.ToString(dv[i]["paper"]).Trim() == "1")
                                    {
                                        //table1forpage2.Cell(i + 1, 4).SetBackgroundColor(Color.Red);
                                        double.TryParse(Convert.ToString(dv[i]["fee"]).Trim(), out value);
                                        arrearPaperCost += value;
                                    }
                                    else
                                    {
                                        double.TryParse(Convert.ToString(dv[i]["fee"]).Trim(), out value);
                                        currentPaperCost += value;
                                    }
                                }
                                else
                                {
                                    table1forpage2.Cell(j + 1, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1forpage2.Cell(j + 1, 6).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1forpage2.Cell(j + 1, 7).SetContentAlignment(ContentAlignment.TopLeft);
                                    table1forpage2.Cell(j + 1, 8).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1forpage2.Cell(j + 1, 5).SetCellPadding(3);
                                    table1forpage2.Cell(j + 1, 6).SetCellPadding(3);
                                    table1forpage2.Cell(j + 1, 7).SetCellPadding(3);
                                    table1forpage2.Cell(j + 1, 8).SetCellPadding(3);
                                    table1forpage2.Cell(j + 1, 9).SetCellPadding(3);
                                    if (j == 1)
                                    {
                                        table1forpage2.Cell(j + 1, 5).SetContent(dv[i]["semester"].ToString());
                                        table1forpage2.Cell(j + 1, 6).SetContent(dv[i]["subject_code"].ToString());
                                        table1forpage2.Cell(j + 1, 7).SetContent(dv[i]["subject_name"].ToString());
                                        table1forpage2.Cell(j + 1, 8).SetContent(dv[i]["fee"].ToString());
                                    }
                                    else
                                    {
                                        table1forpage2.Cell(j + 1, 5).SetContent(dv[i]["semester"].ToString());
                                        table1forpage2.Cell(j + 1, 6).SetContent(dv[i]["subject_code"].ToString());
                                        table1forpage2.Cell(j + 1, 7).SetContent(dv[i]["subject_name"].ToString());
                                        table1forpage2.Cell(j + 1, 8).SetContent(dv[i]["fee"].ToString());
                                    }
                                    double value = 0;
                                    if (Convert.ToString(dv[i]["paper"]).Trim() == "1")
                                    {
                                        double.TryParse(Convert.ToString(dv[i]["fee"]).Trim(), out value);
                                        arrearPaperCost += value;
                                        //table1forpage2.Cell(i + 1, 9).SetBackgroundColor(Color.Red);
                                    }
                                    else
                                    {
                                        double.TryParse(Convert.ToString(dv[i]["fee"]).Trim(), out value);
                                        currentPaperCost += value;
                                    }
                                    j++;
                                }
                            }
                            newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 228, 565, 500));//sr
                            mypdfpage.Add(newpdftabpage2);
                            j = 0;
                            for (int row = 0; row < dv.Count; row++)
                            {
                                if (row < 20)
                                {
                                    if (Convert.ToString(dv[row]["paper"]).Trim() == "1")
                                    {
                                        PdfRectangle prnew = newpdftabpage2.CellArea(row + 1, 4).ToRectangle(Color.Black, 1, Color.White);
                                        PdfArea pdfdfjdf = prnew.RectangleArea.InnerArea(newpdftabpage2.CellArea(row + 1, 4).CenterX, newpdftabpage2.CellArea(row + 1, 4).CenterY);
                                        prnew.RectangleArea.Height = 7;
                                        prnew.RectangleArea.Width = 7;
                                        mypdfpage.Add(prnew);
                                    }
                                }
                                else
                                {
                                    if (Convert.ToString(dv[row]["paper"]).Trim() == "1")
                                    {
                                        PdfRectangle prnew = newpdftabpage2.CellArea(j + 1, 9).ToRectangle(Color.Black, 1, Color.White);
                                        prnew.RectangleArea.InnerArea(20, 20);
                                        prnew.RectangleArea.Height = 7;
                                        prnew.RectangleArea.Width = 7;
                                        mypdfpage.Add(prnew);
                                    }
                                    j++;
                                }
                            }
                            tlinerect = new PdfArea(mydoc, 15, 722, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            table1forpage2 = mydoc.NewTable(font4bold, 3, 5, 4);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.Columns);
                            table1forpage2.Columns[0].SetWidth(100);
                            table1forpage2.Columns[1].SetWidth(100);
                            table1forpage2.Columns[2].SetWidth(100);
                            table1forpage2.Columns[3].SetWidth(100);
                            table1forpage2.Columns[4].SetWidth(100);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 0).SetContent("Papers Appearing");
                            foreach (PdfCell pr in table1forpage2.CellRange(0, 0, 0, 0).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            //int theoryCount = 0;
                            //int Practicalcount = 0;
                            //int project = 0;
                            //int others = 0;
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 1).SetContent("Theory");
                            table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(1, 1).SetContent((theoryCount == 0) ? "-" : Convert.ToString(theoryCount));

                            table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 2).SetContent("Practical");
                            table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(1, 2).SetContent((Practicalcount == 0) ? "-" : Convert.ToString(Practicalcount));

                            table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 3).SetContent("Project");

                            table1forpage2.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(1, 3).SetContent((project == 0) ? "-" : Convert.ToString(project));

                            table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 4).SetContent("Others");
                            table1forpage2.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(1, 4).SetContent((others == 0) ? "-" : Convert.ToString(others));

                            newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 620, 200, 500));
                            mypdfpage.Add(newpdftabpage2);
                            PdfRectangle prt = newpdftabpage2.Area.ToRectangle(Color.Black);
                            mypdfpage.Add(prt);
                            for (int cel = 1; cel < 5; cel++)
                            {
                                PdfLine plt = newpdftabpage2.CellArea(0, cel).LowerBound(Color.Black, 1);
                                mypdfpage.Add(plt);
                            }
                            string subjextsregister = "No. of Subject(s) Registered : " + dv.Count + "";
                            table1forpage2 = mydoc.NewTable(font3small, 4 + ((arearcount > 0) ? 2 : 1), 3, 3);
                            table1forpage2.Columns[0].SetWidth(120);
                            table1forpage2.Columns[1].SetWidth(10);
                            table1forpage2.Columns[2].SetWidth(130);
                            int count = 0;
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            bool hasarrear = false;
                            bool hascurrent = true;
                            table1forpage2.Cell(count, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(count, 0).SetContent("No.of Current Subject(s) Registered : " + currentpaper);
                            foreach (PdfCell pr in table1forpage2.CellRange(count, 0, count, 0).Cells)
                            {
                                pr.ColSpan = 3;
                            }
                            hascurrent = true;
                            count++;
                            if (arearcount > 0)
                            {
                                hasarrear = true;
                                table1forpage2.Cell(count, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                table1forpage2.Cell(count, 0).SetContent("No.of Arrear Subject(s) Registered : " + arearcount);
                                foreach (PdfCell pr in table1forpage2.CellRange(count, 0, count, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                count++;
                            }
                            DataTable dtAmt = new DataTable();
                            string markSheetAmt = string.Empty;

                            #region // for Adhiyaman magesh
                            //string MarkSheetAmount = "Select fa.FeeAmount from FM_LedgerMaster lm,FT_FeeAllot fa where lm.LedgerName='Statement Of Marks' and fa.LedgerFK=lm.LedgerPK and fa.App_no='" + stdappno + "'";
                            //dtAmt = dirAcc.selectDataTable(MarkSheetAmount);
                            //if (chkInclFinMand.Checked == true)
                            //{
                            //    if (dtAmt.Rows.Count > 0)
                            //    {
                            //        markSheetAmt = dtAmt.Rows[0]["FeeAmount"].ToString();
                            //    }
                            //    else
                            //    {
                            //        markSheetAmt = "0";
                            //    }
                            //} 
                            #endregion // for Adhiyaman
                            ////Rajkumar 10/1/2018-----------
                            //else
                            //{
                            markSheetAmt = dv[0]["cost_mark"].ToString();
                            // }
                            //====================================
                            //count++;
                            double ttol = Convert.ToDouble(dv[0]["total_fee"].ToString());

                            table1forpage2.Cell(count, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(count, 0).SetContent("Exam Fees (Rs)");
                            table1forpage2.Cell(count, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(count, 1).SetContent(":");
                            table1forpage2.Cell(count, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                            string data = String.Format("{0:0.00}", currentPaperCost + arrearPaperCost);//arrearPaperCost
                            //table1forpage2.Cell(count, 2).SetContent(((hascurrent) ? data : "") + ((hasarrear) ? ((hascurrent) ? " + " : "") : ""));
                            table1forpage2.Cell(count, 2).SetContent(data);

                            table1forpage2.Cell(count + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(count + 1, 0).SetContent("Statement of Mark Sheet Fee (Rs)");
                            table1forpage2.Cell(count + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(count + 1, 1).SetContent(":");
                            double markSheet = Convert.ToDouble(markSheetAmt);
                            data = String.Format("{0:0.00}", markSheet);
                            table1forpage2.Cell(count + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(count + 1, 2).SetContent(data);

                            table1forpage2.Cell(count + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(count + 2, 0).SetContent("Others Fees (Rs)");
                            table1forpage2.Cell(count + 2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(count + 2, 1).SetContent(":");
                            double otherfee = Convert.ToDouble(dv[0]["extra_fee"].ToString()) + Convert.ToDouble(dv[0]["fine"].ToString()) + Convert.ToDouble(dv[0]["cost_appl"].ToString()) + Convert.ToDouble(dv[0]["extra_fee2"].ToString());//Convert.ToDouble(dv[0]["cost_mark"].ToString()) +
                            data = String.Format("{0:0.00}", otherfee);
                            table1forpage2.Cell(count + 2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(count + 2, 2).SetContent(data);


                            table1forpage2.Cell(count + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(count + 3, 0).SetContent("Total Fees (Rs)");
                            data = String.Format("{0:0.00}", currentPaperCost + arrearPaperCost + otherfee + markSheet);
                            table1forpage2.Cell(count + 3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(count + 3, 1).SetContent(":");
                            table1forpage2.Cell(count + 3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(count + 3, 2).SetContent(Convert.ToString(data));
                            // table1forpage2.Cell(count + 3, 2).SetContent(((hasarrear) ? "" : ((hascurrent) ? Convert.ToString(data):"")));
                            newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 230, 620, 200, 500));
                            mypdfpage.Add(newpdftabpage2);
                            coltop = 720;
                            if (!string.IsNullOrEmpty(foreNoon.Trim()))
                            {
                                PdfTextArea ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 210, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "FN - FORENOON " + foreNoon.Trim());
                                mypdfpage.Add(ptcfn);
                            }
                            else
                            {
                                PdfTextArea ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 210, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "FN - FORENOON 10.00 AM - 1.00 PM");
                                mypdfpage.Add(ptcfn);
                            }
                            coltop = coltop + 15;
                            if (!string.IsNullOrEmpty(foreNoon.Trim()))
                            {
                                PdfTextArea ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 210, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "AN - AFTERNOON " + afterNoon.Trim());
                                mypdfpage.Add(ptcan);
                            }
                            else
                            {
                                PdfTextArea ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 210, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "AN - AFTERNOON 2.00 PM - 5.00 PM");
                                mypdfpage.Add(ptcan);
                            }
                            coltop = 740;
                            //added by Mullai


                            ds1 = d2.select_method_wo_parameter(selectqry, "text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                string templates = Convert.ToString(ds1.Tables[0].Rows[0]["template"]).Trim();
                                string[] sign = templates.Split(';');
                                string sig1 = sign[0];
                                string sig2 = sign[1];
                                string sig3 = sign[2];

                                string note = sign[3];
                                //string note1 = note.Trim().Replace("$", " ").Trim();
                                string[] notesplit = note.Split('$');
                                PdfTextArea ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop - 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(sig1));
                                mypdfpage.Add(ptcstisign);
                                ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop - 35, 400, 20), System.Drawing.ContentAlignment.MiddleLeft, "Date : ");
                                mypdfpage.Add(ptcstisign);
                                ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 430, coltop - 40, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(sig2));
                                mypdfpage.Add(ptcstisign);
                                PdfTextArea ptccontroller = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 380, coltop + 30, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(sig3));
                                mypdfpage.Add(ptccontroller);
                                coltop = coltop + 15;
                                PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                                mypdfpage.Add(ptcsnote);
                                for (int s = 0; s < notesplit.Length; s++)
                                {
                                    string notesp = Convert.ToString(notesplit[s]);

                                    coltop = coltop + 15;
                                    PdfTextArea ptcsnote1 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(notesp));
                                    mypdfpage.Add(ptcsnote1);
                                }
                            }
                            else
                            {
                                PdfTextArea ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop - 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Tutor");
                                mypdfpage.Add(ptcstisign);
                                ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop - 35, 400, 20), System.Drawing.ContentAlignment.MiddleLeft, "Date : ");
                                mypdfpage.Add(ptcstisign);
                                ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 430, coltop - 40, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                                mypdfpage.Add(ptcstisign);
                                PdfTextArea ptccontroller = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 380, coltop + 30, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Controller of Examinations");
                                mypdfpage.Add(ptccontroller);
                                coltop = coltop + 15;
                                PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                                mypdfpage.Add(ptcsnote);

                                coltop = coltop + 15;
                                PdfTextArea ptcsnote1 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1. Detained students are NOT ELIGIBLE to write their current papers");
                                mypdfpage.Add(ptcsnote1);
                                coltop = coltop + 15;
                                PdfTextArea ptcsnote2 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. If any discrepancy is found in the form, report to office immediately.");
                                mypdfpage.Add(ptcsnote2);
                                coltop = coltop + 15;
                                ptcsnote2 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "3. Student Should bring his/her Identity Card Hall Ticket to The Exam Hall. Otherwise they will not be permitted to write the exams.");
                                mypdfpage.Add(ptcsnote2);

                                PdfTextArea ptccoename = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 380, coltop + 15, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, coename);
                                mypdfpage.Add(ptccoename);
                            }

                            //


                            mypdfpage.SaveToDocument();
                        }
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "ExamApplication" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
                #endregion Format1
            }
            else if (rbFormat2.Checked == true)
            {
                printExamAppn();
            }
            else if (rbFormat3.Checked == true)
            {
                # region Format3 Nec Exam Application
                FpSpread1.SaveChanges();
                if (ddlYear.SelectedValue.ToString() == "0")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select The Exam Year And Then Proceed";
                    return;
                }
                if ((ddlMonth.SelectedValue.ToString() == "0"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select The Exam Month And Then Proceed";
                    return;
                }
                int insupdateval = 0;
                string degreecode = ddlbranch.SelectedValue.ToString();
                string batchyear = ddlbatch.SelectedValue.ToString();
                string sem = ddlsem.SelectedValue.ToString();
                if (chkpassout.Checked == true)
                {
                    int semval = ddlsem.Items.Count;
                    semval++;
                    sem = semval.ToString();
                }
                string exammonth = ddlMonth.SelectedValue.ToString();
                string examyear = ddlYear.SelectedValue.ToString();
                collegecode = ddlcollege.SelectedValue.ToString();
                string appldate = txtappldate.Text.ToString();
                string[] spd = appldate.Split('/');
                DateTime dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                string lastappladet = txtappllastdate.Text.ToString(); spd = appldate.Split('/');
                spd = lastappladet.Split('/');
                DateTime dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                if (dtappl > dtlastappl)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                    return;
                }
                string applfee = txtapplfee.Text.ToString();
                string smtfee = txtstmtfee.Text.ToString();
                string extrafee = txtextrafee.Text.ToString();
                string fineamount = txtfine.Text.ToString();
                double appltotalcost = 0;
                double applcost = 0;
                if (applfee.Trim() != "")
                {
                    applcost = Convert.ToDouble(applfee);
                }
                double stmtcost = 0;
                if (smtfee.Trim() != "")
                {
                    stmtcost = Convert.ToDouble(smtfee);
                }
                double extracost = 0;
                if (extrafee.Trim() != "")
                {
                    extracost = Convert.ToDouble(extrafee);
                }
                double fineamo = 0;
                if (fineamount.Trim() != "")
                {
                    fineamo = Convert.ToDouble(fineamount);
                }
                appltotalcost = applcost + stmtcost + extracost + fineamo;
                bool setflag = false;
                for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                {
                    int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                    if (stva == 1)
                    {
                        setflag = true;
                    }
                }
                if (setflag == false)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select The Student And Then Proceed";
                    return;
                }
                Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
                Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
                Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
                Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
                Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
                Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
                Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
                string coename = string.Empty;
                string strquery = "select address1+', '+address3+'- '+pincode districtpin,Collname, affliatedby,coe,acr,category  from collinfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                string Collegename = string.Empty;
                string aff = string.Empty;
                string collacr = string.Empty;
                string dispin = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                    aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                    string[] strpa = aff.Split(',');
                    aff = strpa[0];
                    aff = "An " + ds.Tables[0].Rows[0]["category"].ToString() + " Institution Affiliated to " + aff + " , Chennai";
                    coename = ds.Tables[0].Rows[0]["coe"].ToString();
                    collacr = ds.Tables[0].Rows[0]["acr"].ToString();
                    dispin = ds.Tables[0].Rows[0]["districtpin"].ToString();
                }
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                DataSet printds = new DataSet();
                string studinfo = "select sy.semester,r.app_no, r.Current_Semester, r.batch_year,exam_month,exam_year,stud_name,r.degree_code,subject_code,subject_name,isnull(total_fee,0) as total_fee,convert(decimal(5,0),ROUND(fee,0)) as fee,isnull(ea.extra_fee,0) as extra_fee,ead.attempts,ea.roll_no,isnull(ea.fee_amount,0) fee_amount,isnull(ea.fine,0) fine,isnull(ea.cost_appl,0) cost_appl,isnull(cost_mark,0) as cost_mark from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy,registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ss.syll_code=sy.syll_code and r.roll_no=ea.roll_no  and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "'";
                studinfo = studinfo + " select dd.dept_name,c.course_name,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id ";
                printds.Clear();
                printds = da.select_method_wo_parameter(studinfo, "Text");
                DataView dv = new DataView();
                string semroman = string.Empty;
                Gios.Pdf.PdfPage mypdfpage;
                for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                {
                    string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                    string regnnono = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                    int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                    if (stva == 1)
                    {
                        //printds.Tables[1].DefaultView.RowFilter = "degree_code='" + ddlbranch.SelectedItem.Value.ToString() + "'";
                        printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                        dv = printds.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            mypdfpage = mydoc.NewPage();
                            string studnmae = dv[0]["stud_name"].ToString();
                            string seminfo = dv[0]["Current_Semester"].ToString();
                            string stdappno = dv[0]["app_no"].ToString();
                            string degreecodee = dv[0]["degree_code"].ToString();
                            PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
                            //PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                            //mypdfpage.Add(pr1);
                            int coltop = 25;
                            PdfTextArea ptc;
                            #region STudent Photo
                            string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                            MemoryStream memoryStream = new MemoryStream();
                            DataSet dsstdpho = new DataSet();
                            dsstdpho.Clear();
                            dsstdpho.Dispose();
                            dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                            if (dsstdpho.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                                    {
                                    }
                                    else
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        //NetworkShare.SaveOnNetworkShare(thumb, HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), ImageFormat.Jpeg);
                                    }
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"));
                                mypdfpage.Add(LogoImage2, 485, 30, 300);
                            }
                            else
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage2, 485, 30, 300);
                            }
                            #endregion
                            if (chkheadimage.Checked == false)
                            {
                                #region Left Logo
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 20, 400);
                                }
                                #endregion
                                #region TOP DETAILS
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                                ptc = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, dispin);
                                mypdfpage.Add(ptc);
                                coltop = coltop + 15;
                                ptc = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "(" + aff + ")");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 10;
                                ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 0, coltop + 10, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                                ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 0, coltop + 10, 595, 30), System.Drawing.ContentAlignment.TopCenter, "APPLICATION FOR SEMESTER EXAMINATIONS " + strMonthName.ToUpper() + " - " + examyear + "");
                                mypdfpage.Add(ptc);
                                #endregion
                            }
                            else
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 25, 410);
                                }
                                coltop = 90;
                                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                                ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 0, coltop + 20, 595, 30), System.Drawing.ContentAlignment.TopCenter, "APPLICATION FOR SEMESTER EXAMINATIONS " + strMonthName.ToUpper() + " - " + examyear + "");
                                mypdfpage.Add(ptc);
                            }
                            PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
                            PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            #region studinfo
                            coltop = coltop + 15;
                            if (seminfo == "1")
                            {
                                semroman = "I";
                            }
                            else if (seminfo == "2")
                            {
                                semroman = "II";
                            }
                            else if (seminfo == "3")
                            {
                                semroman = "III";
                            }
                            else if (seminfo == "4")
                            {
                                semroman = "IV";
                            }
                            else if (seminfo == "5")
                            {
                                semroman = "V";
                            }
                            else if (seminfo == "6")
                            {
                                semroman = "VI";
                            }
                            else if (seminfo == "7")
                            {
                                semroman = "VII";
                            }
                            else if (seminfo == "8")
                            {
                                semroman = "VIII";
                            }
                            Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(font3small, 5, 6, 5);
                            table1forpage2.Columns[0].SetWidth(120);
                            table1forpage2.Columns[1].SetWidth(10);
                            table1forpage2.Columns[2].SetWidth(130);
                            table1forpage2.Columns[3].SetWidth(100);
                            table1forpage2.Columns[4].SetWidth(10);
                            table1forpage2.Columns[5].SetWidth(100);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(0, 0).SetContent("Student Name ");
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 1).SetContent(":");
                            table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(0, 2).SetContent(studnmae);
                            table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(0, 3).SetContent("Register No ");
                            table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 4).SetContent(":");
                            table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 5).SetContent(regnnono.ToUpper());
                            table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(1, 0).SetContent("Degree & Branch ");
                            table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(1, 1).SetContent(":");
                            string degreebranch = da.GetFunction("select (c.course_name + ' - '+dd.dept_name) as degree,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id  and degree_code='" + degreecodee + "'");
                            table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(1, 2).SetContent(degreebranch);
                            table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(2, 0).SetContent("Date of Birth");
                            table1forpage2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(2, 1).SetContent(":");
                            string dob = da.GetFunction("select convert(varchar,dob,103) dob from applyn  where app_no='" + stdappno + "'");
                            table1forpage2.Cell(2, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(2, 2).SetContent(dob);
                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 121, 553, 200));//sr
                            mypdfpage.Add(newpdftabpage2);
                            coltop = coltop + 85;
                            tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            #endregion
                            table1forpage2 = mydoc.NewTable(font4small, dv.Count + 1, 8, 4);
                            table1forpage2.Columns[0].SetWidth(20);
                            table1forpage2.Columns[1].SetWidth(50);
                            table1forpage2.Columns[2].SetWidth(130);
                            table1forpage2.Columns[3].SetWidth(30);
                            table1forpage2.Columns[4].SetWidth(20);
                            table1forpage2.Columns[5].SetWidth(50);
                            table1forpage2.Columns[6].SetWidth(130);
                            table1forpage2.Columns[7].SetWidth(30);
                            mypdfpage.Add(newpdftabpage2);
                            coltop = coltop + 85;
                            tlinerect = new PdfArea(mydoc, 15, 240, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 40, 220, 0.01, 400);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            //mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 100, 220, 0.01, 400);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            //mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 255, 220, 0.01, 400);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            //mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 290, 220, 0.01, 400);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 315, 220, 0.01, 400);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            //mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 375, 220, 0.01, 400);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            //mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 532, 220, 0.01, 400);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            //mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 532, 220, 0.01, 400);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            //mypdfpage.Add(plimerecyt);
                            tlinerect = new PdfArea(mydoc, 15, 620, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 0).SetContent("Sem");
                            table1forpage2.Cell(0, 0).SetFont(font4bold);
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 1).SetContent("SubCode");
                            table1forpage2.Cell(0, 1).SetFont(font4bold);
                            table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 2).SetContent("Subject Title");
                            table1forpage2.Cell(0, 2).SetFont(font4bold);
                            table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 3).SetContent("(Rs)");
                            table1forpage2.Cell(0, 3).SetFont(font4bold);
                            table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 4).SetContent("Sem");
                            table1forpage2.Cell(0, 4).SetFont(font4bold);
                            table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 5).SetContent("SubCode");
                            table1forpage2.Cell(0, 5).SetFont(font4bold);
                            table1forpage2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 6).SetContent("Subject Title");
                            table1forpage2.Cell(0, 6).SetFont(font4bold);
                            table1forpage2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 7).SetContent("(Rs)");
                            table1forpage2.Cell(0, 7).SetFont(font4bold);
                            int j = 0;
                            for (int i = 0; i < dv.Count; i++)
                            {
                                if (i < 20)
                                {
                                    table1forpage2.Cell(i + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(i + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(i + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpage2.Cell(i + 1, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                    if (i == 1)
                                    {
                                        table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                        table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                        table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                        table1forpage2.Cell(i + 1, 3).SetContent(dv[i]["fee"].ToString() + " ");
                                    }
                                    else
                                    {
                                        table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                        table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                        table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                        table1forpage2.Cell(i + 1, 3).SetContent(dv[i]["fee"].ToString() + " ");
                                    }
                                }
                                else
                                {
                                    j++;
                                    table1forpage2.Cell(j + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(j + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(j + 1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpage2.Cell(j + 1, 7).SetContentAlignment(ContentAlignment.MiddleRight);
                                    if (j == 1)
                                    {
                                        table1forpage2.Cell(j + 1, 4).SetContent(dv[i]["semester"].ToString());
                                        table1forpage2.Cell(j + 1, 5).SetContent(dv[i]["subject_code"].ToString());
                                        table1forpage2.Cell(j + 1, 6).SetContent(dv[i]["subject_name"].ToString());
                                        table1forpage2.Cell(j + 1, 7).SetContent(dv[i]["fee"].ToString());
                                    }
                                    else
                                    {
                                        table1forpage2.Cell(j + 1, 4).SetContent(dv[i]["semester"].ToString());
                                        table1forpage2.Cell(j + 1, 5).SetContent(dv[i]["subject_code"].ToString());
                                        table1forpage2.Cell(j + 1, 6).SetContent(dv[i]["subject_name"].ToString());
                                        table1forpage2.Cell(j + 1, 7).SetContent(dv[i]["fee"].ToString());
                                    }
                                }
                            }
                            newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 223, 553, 500));//sr
                            mypdfpage.Add(newpdftabpage2);
                            tlinerect = new PdfArea(mydoc, 15, 710, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                            //string subjextsregister = "No. of Subject(s) Registered : " + dv.Count + "";
                            table1forpage2 = mydoc.NewTable(font3bold, 4, 3, 3);
                            table1forpage2.Columns[0].SetWidth(130);
                            table1forpage2.Columns[1].SetWidth(10);
                            table1forpage2.Columns[2].SetWidth(130);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                            //table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            //table1forpage2.Cell(0, 0).SetContent(subjextsregister);
                            double ttol = Convert.ToDouble(dv[0]["total_fee"].ToString());
                            table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(1, 0).SetContent("Exam Fees (Rs)");
                            table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(1, 1).SetContent(":");
                            table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            string data = String.Format("{0:0.00}", Convert.ToDouble(dv[0]["fee_amount"].ToString()));
                            table1forpage2.Cell(1, 2).SetContent(data);
                            table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //table1forpage2.Cell(2, 0).SetContent("Others Fees (Rs)");
                            table1forpage2.Cell(2, 0).SetContent("Marksheet Fees (Rs)");
                            table1forpage2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(2, 1).SetContent(":");
                            //double otherfee = Convert.ToDouble(dv[0]["extra_fee"].ToString()) + Convert.ToDouble(dv[0]["fine"].ToString()) + Convert.ToDouble(dv[0]["cost_mark"].ToString()) + Convert.ToDouble(dv[0]["cost_appl"].ToString());
                            double otherfee = Convert.ToDouble(dv[0]["cost_mark"].ToString());
                            data = String.Format("{0:0.00}", otherfee);
                            table1forpage2.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(2, 2).SetContent(data);
                            table1forpage2.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(3, 0).SetContent("Total Fees (Rs)");
                            data = String.Format("{0:0.00}", ttol);
                            table1forpage2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(3, 1).SetContent(":");
                            table1forpage2.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(3, 2).SetContent(data);
                            foreach (PdfCell pr in table1forpage2.CellRange(0, 0, 0, 0).Cells)
                            {
                                pr.ColSpan = 3;
                            }
                            newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 230, 625, 200, 500));//sr
                            mypdfpage.Add(newpdftabpage2);
                            coltop = 715;
                            //PdfTextArea ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                            //                                                  new PdfArea(mydoc, 200, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "FN - FORENOON 10.00 AM - 1.00 PM");
                            //mypdfpage.Add(ptcfn);
                            PdfTextArea ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                             new PdfArea(mydoc, 200, coltop, 250, 30), System.Drawing.ContentAlignment.MiddleCenter, "These Particulars are verified and found correct");
                            mypdfpage.Add(ptcfn);
                            coltop = coltop + 25;
                            //PdfTextArea ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                            //                                                  new PdfArea(mydoc, 200, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "AN - AFTERNOON 2.00 PM - 5.00 PM");
                            //mypdfpage.Add(ptcan);
                            PdfTextArea ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydoc, 200, 770, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Signature of the HOD");
                            mypdfpage.Add(ptcan);
                            coltop = 740;
                            //PdfTextArea ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                            //                                                    new PdfArea(mydoc, 30, coltop - 55, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Clerk with date");
                            //mypdfpage.Add(ptcstisign);
                            PdfTextArea subjectsregister = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 30, coltop - 120, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "No. of Subject(s) Registered : " + dv.Count + "");
                            mypdfpage.Add(subjectsregister);
                            PdfTextArea ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 440, coltop - 55, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                            mypdfpage.Add(ptcstisign);
                            PdfTextArea ptccoename = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 380, coltop + 15, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, coename);
                            mypdfpage.Add(ptccoename);
                            //PdfTextArea ptccontroller = new PdfTextArea(font3small, System.Drawing.Color.Black,
                            //                                                   new PdfArea(mydoc, 380, coltop + 30, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Controller of Examinations");
                            PdfTextArea ptccontroller = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 400, 770, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Signature of the Principal");
                            mypdfpage.Add(ptccontroller);
                            coltop = coltop + 15;
                            //PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                            //                                                    new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                            PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date :");
                            mypdfpage.Add(ptcsnote);
                            coltop = coltop + 15;
                            //PdfTextArea ptcsnote1 = new PdfTextArea(font4small, System.Drawing.Color.Black,
                            //                                                   new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1. Detained students are NOT ELIGIBLE to write their current papers");
                            //mypdfpage.Add(ptcsnote1);
                            //coltop = coltop + 15;
                            //PdfTextArea ptcsnote2 = new PdfTextArea(font4small, System.Drawing.Color.Black,
                            //                                                   new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. If anydiscrepancies are found in the form, report to the C.O.E office immediately");
                            //mypdfpage.Add(ptcsnote2);
                            mypdfpage.SaveToDocument();
                        }
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "CertificateCourseMarkSheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
                #endregion Format3
            }
            else if (rbFormat4.Checked == true)
            {
                PrintFormat4Mcc();
            }
            else if (rbFormat5.Checked)
            {
                ApplicationFormat5();
            }
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            // d2.sendErrorMail(ex, collegecode1, "Exam Application");
        }
    }

    protected void btnapplpaper_Click(object sender, EventArgs e)
    {
        txtrollpop.Text = string.Empty;
        txtregnopop.Text = string.Empty;
        showdetails.Visible = false;
        FpSpread2.Sheets[0].RowCount = 0;
        string exammonth = ddlMonth.SelectedItem.Value.ToString();
        string examyear = ddlYear.SelectedItem.Text.ToString().Trim();
        if (examyear.Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Exam Year')", true);
            showdetails.Visible = false;
            return;
        }
        if (exammonth.Trim() == "0")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Exam Month')", true);
            showdetails.Visible = false;
            return;
        }
        if (examyear.Trim() != "" && exammonth.Trim() != "")
        {
            showappicable.Visible = true;
        }
        else
        {
            showappicable.Visible = false;
        }
    }

    protected void btnsearch_click(object sender, EventArgs e)
    {
        try
        {
            string rollnopop = txtrollpop.Text.Trim();
            string regnopop = txtregnopop.Text.Trim();
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Trebuchet MS";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = Color.White;
            style2.BackColor = Color.Teal;

            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread2.CommandBar.Visible = false;

            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 7;
            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[1].Width = 40;
            FpSpread2.Sheets[0].Columns[3].Width = 300;
            FpSpread2.Sheets[0].Columns[2].Width = 120;
            FpSpread2.Sheets[0].Columns[4].Width = 80;
            FpSpread2.Sheets[0].Columns[5].Width = 80;
            FpSpread2.Sheets[0].Columns[6].Width = 75;
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[2].Locked = true;
            FpSpread2.Sheets[0].Columns[3].Locked = true;
            FpSpread2.Sheets[0].Columns[4].Locked = true;
            FpSpread2.Sheets[0].Columns[5].Locked = true;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].SheetCorner.RowCount = 1;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Sem";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Code";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Paper";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Fees";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Attempts";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            string exammonth = ddlMonth.SelectedItem.Value.ToString();
            string examyear = ddlYear.SelectedItem.Text.ToString();
            if (examyear.Trim() == "")
            {
                lbl_alert.Text = "Please Select Exam Year";
                imgAlert.Visible = true;
                showdetails.Visible = false;
                return;
            }
            if (exammonth.Trim() == "")
            {
                lbl_alert.Text = "Please Select Exam Month";
                imgAlert.Visible = true;
                showdetails.Visible = false;
                return;
            }
            if (rollnopop == string.Empty)
            {
                rollnopop = d2.GetFunction("select roll_no from registration where reg_no='" + regnopop + "'").Trim();
                if (rollnopop == "0" || rollnopop == string.Empty || rollnopop == null)
                {
                    rollnopop = string.Empty;
                }
            }
            if (rollnopop != string.Empty)
            {
                string qry = "select Ed.exam_code,ea.appl_no,ead.subject_no,ea.roll_no from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.exam_month='" + exammonth.Trim() + "' and ed.exam_year='" + examyear.Trim() + "' and ea.roll_no='" + rollnopop + "' ";
                DataSet dsExamApplied = da.select_method_wo_parameter(qry, "Text");
                if (dsExamApplied.Tables.Count > 0 && dsExamApplied.Tables[0].Rows.Count > 0)
                {
                    lbl_alert.Text = "You Are Already Applied. Please Delete Your Exam Application And Then Proceed.";
                    imgAlert.Visible = true;
                    showdetails.Visible = false;
                    return;
                }
            }
            else
            {
                lbl_alert.Text = "Please Provide RollNo or RegNo";
                imgAlert.Visible = true;
                showdetails.Visible = false;
                return;
            }
            string qryExternalCheck = string.Empty;
            if (chkExternalMark.Checked)
            {
                //m.total>=s.mintotal m.external_mark>=s.min_ext_marks
                qryExternalCheck = " and (m1.result='Pass' or m1.external_mark>=s1.min_ext_marks)";
            }
            else
            {
                qryExternalCheck = " and m1.result='Pass'";
            }
            string studinfo = "select batch_year,roll_no,reg_no,current_semester,degree_code,stud_name,ISNULL(isredo,'0') isredo,app_no from  registration  where roll_no='" + rollnopop + "' or reg_no='" + regnopop + "';";
            studinfo = studinfo + " select dd.dept_name,c.course_name,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id ";
            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            DataView dv = new DataView();
            if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
            {
                string rollnonew = dsstudinfo.Tables[0].Rows[0]["roll_no"].ToString();
                string batchyyearnew = dsstudinfo.Tables[0].Rows[0]["batch_year"].ToString();
                string appNo = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["app_no"]).Trim();
                string redoStudent = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["isredo"]).Trim();
                string degCode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["degree_code"]);
                bool isStudentRedo = false;
                bool.TryParse(redoStudent.Trim(), out isStudentRedo);
                string currsemeter = ddlsem.SelectedValue.ToString();
                if (chkpassout.Checked == true)
                {
                    int semval = ddlsem.Items.Count;
                    semval++;
                    currsemeter = semval.ToString();
                }
                string currentSemester = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["current_semester"]).Trim();
                dsstudinfo.Tables[1].DefaultView.RowFilter = "degree_code='" + dsstudinfo.Tables[0].Rows[0]["degree_code"].ToString() + "'";
                dv = dsstudinfo.Tables[1].DefaultView;
                if (dv.Count > 0)
                {
                    lblbranchpop.Text = dv[0][0].ToString();
                    lbldegreepop.Text = dv[0][1].ToString();
                    lblstudnamepop.Text = dsstudinfo.Tables[0].Rows[0]["stud_name"].ToString();
                    lblbatchyearpop.Text = dsstudinfo.Tables[0].Rows[0]["batch_year"].ToString();
                    string exam_m = ddlMonth.SelectedItem.Value.ToString();
                    string exam_y = ddlYear.SelectedItem.Text.ToString();
                    string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m));
                    strMonthName = strMonthName.ToUpper() + " / " + exam_y + "   ";
                    lblexam_m_ypop.Text = strMonthName;
                }

                Dictionary<string, List<string>> dicAppNoRedoSemester = new Dictionary<string, List<string>>();
                Dictionary<string, List<string>> dicRollNoRedoSemester = new Dictionary<string, List<string>>();
                getAllRedoStudentsSemester(out dicAppNoRedoSemester, out dicRollNoRedoSemester);

                string query = string.Empty;
                string qryNotEligibleSemesters = string.Empty;
                if (dicRollNoRedoSemester.ContainsKey(rollnonew.Trim()))
                {
                    List<string> lstRedoSem = new List<string>();
                    lstRedoSem = dicRollNoRedoSemester[rollnonew.Trim()];
                    string semester = string.Empty;
                    semester = string.Join("','", lstRedoSem.ToArray());
                    if (isStudentRedo)
                    {
                        lstRedoSem.Remove(currentSemester);
                        semester = string.Join("','", lstRedoSem.ToArray());
                    }
                    if (!string.IsNullOrEmpty(semester))
                    {
                        qryNotEligibleSemesters = " and sc.semester not in('" + semester + "')";
                    }
                }
                if (dicAppNoRedoSemester.ContainsKey(appNo.Trim()))
                {
                    List<string> lstRedoSem = new List<string>();
                    lstRedoSem = dicAppNoRedoSemester[appNo.Trim()];
                    string semester = string.Empty;
                    semester = string.Join("','", lstRedoSem.ToArray());
                    if (isStudentRedo)
                    {
                        lstRedoSem.Remove(currentSemester);
                        semester = string.Join("','", lstRedoSem.ToArray());
                    }
                    if (!string.IsNullOrEmpty(semester))
                    {
                        qryNotEligibleSemesters = " and sc.semester not in('" + semester + "')";
                    }
                }
                string valSemcur = string.Empty;
                string valsemarr = string.Empty;
                if (cblsem.Items.Count > 0)
                {
                    for (int s = 0; s < cblsem.Items.Count; s++)
                    {
                        if (cblsem.Items[s].Selected)
                        {
                            int ss = Convert.ToInt32(cblsem.Items[s].Value);
                            if (ss > Convert.ToInt32(currsemeter))
                            {
                                if (string.IsNullOrEmpty(valSemcur))
                                    valSemcur = "'" + ss + "'";
                                else
                                    valSemcur = valSemcur + "," + "'" + ss + "'";
                            }
                            else if (ss < Convert.ToInt32(currsemeter))
                            {
                                if (string.IsNullOrEmpty(valsemarr))
                                    valsemarr = "'" + ss + "'";
                                else
                                    valsemarr = valsemarr + "," + "'" + ss + "'";
                            }
                        }
                    }
                }
                DataTable dtfuturesub = new DataTable();
                string futSubject = string.Empty;
                if (chksemwise.Checked && !chkpassout.Checked)
                {
                    string strfuturesub = "select subject_no from Futuresub_Exam_app where ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "' and Batch_year='" + batchyyearnew + "' and DegreeCode='" + degCode + "' and Semester='" + currsemeter + "'";
                    dtfuturesub = dirAcc.selectDataTable(strfuturesub);
                    if (dtfuturesub.Rows.Count > 0)
                    {
                        foreach (DataRow dt1 in dtfuturesub.Rows)
                        {
                            string subjNo = Convert.ToString(dt1["subject_no"]);
                            if (string.IsNullOrEmpty(futSubject))
                                futSubject = "'" + subjNo + "'";
                            else
                                futSubject = futSubject + "," + "'" + subjNo + "'";
                        }
                    }
                }

                if (chksemwise.Checked && !chkpassout.Checked)
                {
                    //query = "select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                    //query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                    //query = query + " and r.degree_code=sy.degree_code and sy.semester='" + currsemeter + "' and sc.roll_no='" + rollnonew + "' " + qryNotEligibleSemesters;

                    ////if (!string.IsNullOrEmpty(futSubject))
                    ////{
                    //query = query + " union ";
                    //query = query + "select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                    //query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                    //query = query + " and r.degree_code=sy.degree_code and sy.semester in(" + valSemcur + ")  and sc.roll_no='" + rollnonew + "' " + qryNotEligibleSemesters;//and s.subject_no in(" + futSubject + ")
                    ////}
                    //query = query + " union ";


                    studinfo = "select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r where sy.syll_code=ss.syll_code  and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year and ss.promote_count=1 and r.degree_code=sy.degree_code and sy.semester='" + currsemeter + "' and sc.roll_no='" + rollnonew + "' " + qryNotEligibleSemesters;

                    studinfo = studinfo + "union select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r where sy.syll_code=ss.syll_code  and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year and ss.promote_count=1 and r.degree_code=sy.degree_code and sy.semester in(" + valSemcur + ") and sc.roll_no='" + rollnonew + "' " + qryNotEligibleSemesters;

                    studinfo = studinfo + "union select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'N' as arrst,s.arrfee as total_fee from Registration r,subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                    studinfo = studinfo + " where r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and sy.Batch_Year=r.Batch_Year and  sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollnonew + "' and sc.semester in(" + valsemarr + ") " + qryNotEligibleSemesters;
                    studinfo = studinfo + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollnonew + "' " + qryExternalCheck + ") ";
                    studinfo = studinfo + "  order by sy.semester desc,s.subject_code,ss.subject_type";

                    //studinfo = studinfo + "  union  select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'N' as arrst,s.arrfee as total_fee  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollnonew + "' and sc.semester in(" + valsemarr + ") and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollnonew + "' " + qryExternalCheck + ") " + qryNotEligibleSemesters + " order by sy.semester desc,s.subject_code,ss.subject_type";
                }

                else if (isStudentRedo)
                {
                    //studinfo = "select  sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r where sy.syll_code=ss.syll_code  and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year and ss.promote_count=1 and r.degree_code=sy.degree_code and sy.semester='" + currsemeter + "' and sc.roll_no='" + rollnonew + "'";
                    //studinfo = studinfo + "  union  select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'N' as arrst,s.arrfee as total_fee  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollnonew + "' and sc.semester<" + currsemeter + " and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='" + rollnonew + "' " + qryExternalCheck + ") order by sy.semester desc,s.subject_code,ss.subject_type";

                    studinfo = " select  sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and sy.syll_code=ss.syll_code  and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and sr.BatchYear=sy.Batch_Year and ss.promote_count=1 and sr.DegreeCode=sy.degree_code and sy.semester='" + currsemeter + "' and sc.roll_no='" + rollnonew + "' " + qryNotEligibleSemesters;

                    studinfo += " union  select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'N' as arrst,s.arrfee as total_fee  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollnonew + "' and sc.semester<'" + currsemeter + "' and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollnonew + "' " + qryExternalCheck + ") " + qryNotEligibleSemesters + " order by sy.semester desc,s.subject_code,ss.subject_type";

                    //and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollno + "' " + qryExternalCheck + ")
                }
                else
                {
                    studinfo = "select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r where sy.syll_code=ss.syll_code  and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year and ss.promote_count=1 and r.degree_code=sy.degree_code and sy.semester='" + currsemeter + "' and sc.roll_no='" + rollnonew + "' " + qryNotEligibleSemesters;
                    studinfo = studinfo + "  union  select sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'N' as arrst,s.arrfee as total_fee  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollnonew + "' and sc.semester<" + currsemeter + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollnonew + "' " + qryExternalCheck + ") " + qryNotEligibleSemesters + " order by sy.semester desc,s.subject_code,ss.subject_type";
                }
                //and (m.result='Pass' or m.external_mark>=s.min_ext_marks)
                dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
                int sno = 0;
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {
                    sno++;
                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dsstudinfo.Tables[0].Rows[studcount]["semester"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsstudinfo.Tables[0].Rows[studcount]["subject_code"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = dsstudinfo.Tables[0].Rows[studcount]["subject_no"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = dsstudinfo.Tables[0].Rows[studcount]["subject_name"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = dsstudinfo.Tables[0].Rows[studcount]["total_fee"].ToString();
                    int attempts = Convert.ToInt32(currsemeter) - Convert.ToInt32(dsstudinfo.Tables[0].Rows[studcount]["semester"].ToString());
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(attempts + 1);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = chk;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    showdetails.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('No Records Found')", true);
            }
            string totalrows = FpSpread2.Sheets[0].RowCount.ToString();
            FpSpread2.Sheets[0].PageSize = Convert.ToInt32(totalrows);
            FpSpread2.Height = 400;
            FpSpread2.SaveChanges();
        }
        catch (Exception ex)
        {
            //string collegecode1 = Session["collegecode"].ToString();
            //d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Exam Application ";
            string pagename = "Exam Application.aspx";
            Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            d2.printexcelreport(FpSpread2, reportname);
        }
        catch
        {
        }
    }

    protected void linkcloseclick(object sender, EventArgs e)
    {
        showappicable.Visible = false;
    }

    protected void printbtn_Click(object sender, EventArgs e)
    {
        FpSpread2.SaveChanges();
        bindppdf();
        FpSpread2.Visible = false;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.SaveChanges();
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            string rollnopop = txtrollpop.Text.Trim();
            string regnopop = txtregnopop.Text.Trim();
            string studinfo = "select batch_year,roll_no,reg_no,current_semester,degree_code,stud_name from  registration  where roll_no='" + rollnopop + "' or reg_no='" + regnopop + "';";
            studinfo = studinfo + " select dd.dept_name,c.course_name,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id ";
            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            DataView dv = new DataView();
            if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
            {
                int insupdateval = 0;
                string degreecode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["degree_code"]).Trim();
                string batchyear = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["batch_year"]).Trim();
                string sem = Convert.ToString(ddlsem.SelectedValue).Trim();
                string rollno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["roll_no"]).Trim();
                if (chkpassout.Checked == true)
                {
                    int semval = ddlsem.Items.Count;
                    semval++;
                    sem = semval.ToString();
                }
                string exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
                string examyear = Convert.ToString(ddlYear.SelectedValue).Trim();
                collegecode = Convert.ToString(ddlcollege.SelectedValue).Trim();
                string appldate = Convert.ToString(txtappldate.Text).Trim();
                string[] spd = appldate.Split('/');
                DateTime dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                string lastappladet = Convert.ToString(txtappllastdate.Text).Trim();
                spd = appldate.Split('/');
                spd = lastappladet.Split('/');
                DateTime dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
                if (dtappl > dtlastappl)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                    return;
                }
                string applfee = Convert.ToString(txtapplfee.Text).Trim();
                string smtfee = Convert.ToString(txtstmtfee.Text).Trim();
                string extrafee = Convert.ToString(txtextrafee.Text).Trim();
                string fineamount = Convert.ToString(txtfine.Text).Trim();
                double appltotalcost = 0;
                double applcost = 0;
                if (applfee.Trim() != "")
                {
                    // applcost = Convert.ToDouble(applfee);
                    double.TryParse(applfee, out applcost);
                }
                double stmtcost = 0;
                if (smtfee.Trim() != "")
                {
                    //stmtcost = Convert.ToDouble(smtfee);
                    double.TryParse(smtfee, out stmtcost);
                }
                double extracost = 0;
                if (extrafee.Trim() != "")
                {
                    double.TryParse(extrafee, out extracost);
                    //extracost = Convert.ToDouble(extrafee);
                }
                double fineamo = 0;
                if (fineamount.Trim() != "")
                {
                    double.TryParse(fineamount, out fineamo);
                    //fineamo = Convert.ToDouble(fineamount);
                }
                appltotalcost = applcost + stmtcost + extracost + fineamo;
                string getexamcode = d2.GetFunction("select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
                //if (getexamcode.Trim() == "" || getexamcode == null || getexamcode.Trim() == "0")
                //{
                //    string setdegreevalues = "if not exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and current_semester='" + sem + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                //    setdegreevalues = setdegreevalues + " insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code)";
                //    setdegreevalues = setdegreevalues + " values ('" + degreecode + "','" + exammonth + "','" + examyear + "','" + batchyear + "','" + sem + "','" + collegecode + "')";
                //    insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
                //    getexamcode = d2.GetFunction("select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
                //}
                if (getexamcode == null || getexamcode.Trim() == "" || getexamcode.Trim() == "0")
                {
                    string setdegreevalues = "if not exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                    setdegreevalues += " insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code,isSupplementaryExam) values ('" + degreecode + "','" + exammonth + "','" + examyear + "','" + batchyear + "','" + sem + "','" + collegecode + "','" + chkIsSupplementaryExam.Checked + "')";
                    setdegreevalues += " else update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                    insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
                    getexamcode = d2.GetFunction("select exam_code from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
                }
                else
                {
                    string setdegreevalues = "if exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                    setdegreevalues += " update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                    insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
                }
                string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                if (applno.Trim() == "" || applno == null || applno.Trim() == "0")
                {
                    string setexamapplication = "if exists(select * from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "')";
                    setexamapplication = setexamapplication + " update exam_application set applied_date='" + dtappl.ToString("MM/dd/yyyy") + "',total_fee='0',extra_fee='" + extracost.ToString() + "',fine='" + fineamo.ToString() + "',cost_appl='" + applcost.ToString() + "',cost_mark='" + stmtcost.ToString() + "',lastdate='" + dtlastappl.ToString("MM/dd/yyyy") + "',Exam_type='0',fee_amount='0'";
                    setexamapplication = setexamapplication + " where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                    setexamapplication = setexamapplication + " else ";
                    setexamapplication = setexamapplication + " insert into exam_application(roll_no,applied_date,total_fee,exam_code,extra_fee,fine,cost_appl,cost_mark,lastdate,Exam_type,fee_amount)";
                    setexamapplication = setexamapplication + " values('" + rollno + "','" + dtappl.ToString("MM/dd/yyyy") + "','0','" + getexamcode + "','" + extracost.ToString() + "','" + fineamo.ToString() + "','" + applcost + "','" + stmtcost + "','" + dtlastappl.ToString("MM/dd/yyyy") + "','0','0')";
                    insupdateval = d2.update_method_wo_parameter(setexamapplication, "text");
                    applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                }
                string insupdquery = "delete from exam_appl_details where appl_no='" + applno + "'";
                insupdateval = d2.update_method_wo_parameter(insupdquery, "text");
                double exmfee = appltotalcost;
                double totalpaperfee = 0;
                bool saveflag = false;
                for (int s = 0; s < FpSpread2.Sheets[0].RowCount; s++)
                {
                    int stva = Convert.ToInt32(FpSpread2.Sheets[0].Cells[s, 6].Value);
                    if (stva == 1)
                    {
                        saveflag = true;
                        s = FpSpread2.Sheets[0].RowCount;
                    }
                }
                if (saveflag == true)
                {
                    for (int s = 0; s < FpSpread2.Sheets[0].RowCount; s++)
                    {
                        string subject = FpSpread2.Sheets[0].Cells[s, 3].Tag.ToString();
                        int subsem = Convert.ToInt32(FpSpread2.Sheets[0].Cells[s, 1].Text.ToString());
                        string fees = FpSpread2.Sheets[0].Cells[s, 4].Text.ToString();
                        int stva = Convert.ToInt32(FpSpread2.Sheets[0].Cells[s, 6].Value);
                        if (stva == 1)
                        {
                            if (fees.Trim() != "")
                            {
                                exmfee = exmfee + Convert.ToDouble(fees);
                                totalpaperfee = totalpaperfee + Convert.ToDouble(fees);
                            }
                            string attempts = "0";
                            string types = string.Empty;
                            int attval = Convert.ToInt32(sem) - Convert.ToInt32(subsem);
                            if (attval > 0)
                            {
                                attempts = attval.ToString();
                                types = "*";
                            }
                            insupdquery = "if exists(select * from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subject + "')";
                            insupdquery = insupdquery + "  update exam_appl_details set attempts='" + attempts + "',fee='" + fees + "',type='" + types + "',attend='1' where appl_no='" + applno + "' and subject_no='" + subject + "'";
                            insupdquery = insupdquery + " else insert into exam_appl_details(subject_no,attempts,fee,type,appl_no,attend) values('" + subject + "','" + attempts + "','" + fees + "','" + types + "','" + applno + "','1')";
                            insupdateval = d2.update_method_wo_parameter(insupdquery, "text");
                        }
                        else
                        {
                            insupdquery = "delete from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subject + "'";
                            insupdateval = d2.update_method_wo_parameter(insupdquery, "text");
                        }
                    }
                    insupdquery = "update exam_application set total_fee='" + exmfee + "',fee_amount='" + totalpaperfee + "' where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                    insupdateval = d2.update_method_wo_parameter(insupdquery, "text");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Save Sucessfully.')", true);
                }
                else
                {
                    insupdquery = "delete from exam_application where appl_no='" + applno + "'";
                    insupdateval = d2.update_method_wo_parameter(insupdquery, "text");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Deleted Sucessfully.')", true);
                }
                loadexamdetails();
            }
            else
            {
                lblvalidation1.Visible = true;
                lblvalidation1.Text = "Student Doesn't Exists";
            }
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    public void printExamAppn()
    {
        try
        {
            FpSpread1.SaveChanges();
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            int insupdateval = 0;
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string sem = ddlsem.SelectedValue.ToString();
            if (chkpassout.Checked == true)
            {
                int semval = ddlsem.Items.Count;
                semval++;
                sem = semval.ToString();
            }
            switch (sem)
            {
                case "1":
                    sem = "I";
                    break;
                case "2":
                    sem = "II";
                    break;
                case "3":
                    sem = "III";
                    break;
                case "4":
                    sem = "IV";
                    break;
                case "5":
                    sem = "V";
                    break;
                case "6":
                    sem = "VI";
                    break;
                case "7":
                    sem = "VII";
                    break;
                case "8":
                    sem = "VIII";
                    break;
                case "9":
                    sem = "IX";
                    break;
                case "10":
                    sem = "X";
                    break;
                case "11":
                    sem = "XI";
                    break;
                case "12":
                    sem = "XII";
                    break;
                default:
                    sem = sem;
                    break;
            }
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            string appldate = txtappldate.Text.ToString();
            string[] spd = appldate.Split('/');
            DateTime dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            string lastappladet = txtappllastdate.Text.ToString(); spd = appldate.Split('/');
            spd = lastappladet.Split('/');
            DateTime dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            if (dtappl > dtlastappl)
            {
                lblerror.Visible = true;
                lblerror.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            string applfee = txtapplfee.Text.ToString();
            string smtfee = txtstmtfee.Text.ToString();
            string extrafee = txtextrafee.Text.ToString();
            string fineamount = txtfine.Text.ToString();
            double appltotalcost = 0;
            double applcost = 0;
            if (applfee.Trim() != "")
            {
                applcost = Convert.ToDouble(applfee);
            }
            double stmtcost = 0;
            if (smtfee.Trim() != "")
            {
                stmtcost = Convert.ToDouble(smtfee);
            }
            double extracost = 0;
            if (extrafee.Trim() != "")
            {
                extracost = Convert.ToDouble(extrafee);
            }
            double fineamo = 0;
            if (fineamount.Trim() != "")
            {
                fineamo = Convert.ToDouble(fineamount);
            }
            appltotalcost = applcost + stmtcost + extracost + fineamo;
            bool setflag = false;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    setflag = true;
                }
            }
            if (setflag == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Student And Then Proceed";
                return;
            }
            Font fontcolgname = new Font("Algerian", 11, FontStyle.Bold);
            Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
            Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
            Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
            Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font8small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
            Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
            string coename = string.Empty;
            string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = string.Empty;
            string aff = string.Empty;
            string collacr = string.Empty;
            string dispin = string.Empty;
            string clgaddress = string.Empty;
            string univ = string.Empty;
            string pincode = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                string[] strpa = aff.Split(',');
                aff = "( " + univ + " " + strpa[0] + " )";
                coename = ds.Tables[0].Rows[0]["coe"].ToString();
                collacr = ds.Tables[0].Rows[0]["acr"].ToString();
                pincode = Convert.ToString(ds.Tables[0].Rows[0]["pincode"]).Trim();
                pincode = pincode.Substring(pincode.Length - 3);
                int pin = 0;
                int.TryParse(pincode, out pin);
                clgaddress = Convert.ToString(ds.Tables[0].Rows[0]["address3"]) + " , " + Convert.ToString(ds.Tables[0].Rows[0]["district"]) + ((pin != 0) ? (" - " + pin.ToString()) : " - " + pincode);
                dispin = ds.Tables[0].Rows[0]["districtpin"].ToString();
            }
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            DataSet printds = new DataSet();
            string studinfo = "select sy.semester,r.app_no, r.Current_Semester, r.batch_year,exam_month,exam_year,stud_name,r.degree_code,subject_code,subject_name,isnull(total_fee,0) as total_fee,convert(decimal(5,0),ROUND(fee,0)) as fee,isnull(ea.extra_fee,0) as extra_fee,ead.attempts,ea.roll_no,isnull(ea.fee_amount,0) fee_amount,isnull(ea.fine,0) fine,isnull(ea.cost_appl,0) cost_appl,isnull(cost_mark,0) as cost_mark from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy,registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ss.syll_code=sy.syll_code and r.roll_no=ea.roll_no  and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "' order by sy.semester desc,r.degree_code,r.roll_no,subject_code";
            studinfo = studinfo + " select dd.dept_name,c.course_name,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id ";
            printds.Clear();
            printds = da.select_method_wo_parameter(studinfo, "Text");
            DataView dv = new DataView();
            string semroman = string.Empty;
            Gios.Pdf.PdfPage mypdfpage;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                string regnnono = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    dv = printds.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        mypdfpage = mydoc.NewPage();
                        string studnmae = dv[0]["stud_name"].ToString();
                        string seminfo = dv[0]["Current_Semester"].ToString();
                        string stdappno = dv[0]["app_no"].ToString();
                        string degreecodee = dv[0]["degree_code"].ToString();
                        PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
                        PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        mypdfpage.Add(pr1);
                        int coltop = 25;
                        if (chkheadimage.Checked == true)
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 18, 18, 340);
                            }
                            coltop = coltop + 70;
                        }
                        else
                        {
                            #region Left Logo
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 400);
                            }
                            #endregion
                            #region TOP DETAILS
                            PdfTextArea ptc = new PdfTextArea(fontcolgname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename + " , " + clgaddress);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(font4small, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                            ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Convert.ToString(ddldegree.SelectedItem) + " DEGREE EXAMINATIONS " + strMonthName.ToUpper() + " - " + examyear + "");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "SEMESTER - " + sem);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "EXAMINATION APPLICATION FORM");
                            mypdfpage.Add(ptc);
                            #endregion
                        }
                        PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
                        PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        #region studinfo
                        coltop = coltop + 15;
                        if (seminfo == "1")
                        {
                            semroman = "I";
                        }
                        else if (seminfo == "2")
                        {
                            semroman = "II";
                        }
                        else if (seminfo == "3")
                        {
                            semroman = "III";
                        }
                        else if (seminfo == "4")
                        {
                            semroman = "IV";
                        }
                        else if (seminfo == "5")
                        {
                            semroman = "V";
                        }
                        else if (seminfo == "6")
                        {
                            semroman = "VI";
                        }
                        else if (seminfo == "7")
                        {
                            semroman = "VII";
                        }
                        else if (seminfo == "8")
                        {
                            semroman = "VIII";
                        }
                        Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(font3small, 6, 3, 5);
                        table1forpage2.Columns[0].SetWidth(120);
                        table1forpage2.Columns[1].SetWidth(10);
                        table1forpage2.Columns[2].SetWidth(130);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 0).SetContent("Branch & Programme");
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 1).SetContent(":");
                        string degreebranch = da.GetFunction("select (c.course_name) as degree,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id  and degree_code='" + degreecodee + "'");
                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 2).SetContent(degreebranch);
                        table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(1, 0).SetContent("Reg.No.");
                        table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 1).SetContent(":");
                        table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(1, 2).SetContent(regnnono.ToUpper());
                        table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 0).SetContent("Student's Name");
                        table1forpage2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(2, 1).SetContent(":");
                        table1forpage2.Cell(2, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 2).SetContent(studnmae);
                        table1forpage2.Cell(3, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(3, 0).SetContent("Semester");
                        table1forpage2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(3, 1).SetContent(":");
                        table1forpage2.Cell(3, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(3, 2).SetContent(sem);
                        table1forpage2.Cell(4, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(4, 0).SetContent("Date Of Birth");
                        table1forpage2.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(4, 1).SetContent(":");
                        string dob = da.GetFunction("select convert(varchar,dob,103) dob from applyn  where app_no='" + stdappno + "'");
                        table1forpage2.Cell(4, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(4, 2).SetContent(dob);
                        table1forpage2.Cell(5, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(5, 0).SetContent("Medium Selected");
                        table1forpage2.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(5, 1).SetContent(":");
                        table1forpage2.Cell(5, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(5, 2).SetContent("Core Course - " + " \n\nSpecial Course - ");
                        #region STudent Photo
                        string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                        MemoryStream memoryStream = new MemoryStream();
                        DataSet dsstdpho = new DataSet();
                        dsstdpho.Clear();
                        dsstdpho.Dispose();
                        dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                        if (dsstdpho.Tables[0].Rows.Count > 0)
                        {
                            byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                            memoryStream.Write(file, 0, file.Length);
                            if (file.Length > 0)
                            {
                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                                {
                                }
                                else
                                {
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    //NetworkShare.SaveOnNetworkShare(thumb, HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), ImageFormat.Jpeg);
                                }
                            }
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                        {
                            Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"));
                            mypdfpage.Add(LogoImage2, 485, coltop + 30, 300);
                        }
                        else
                        {
                            Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                            mypdfpage.Add(LogoImage2, 485, coltop + 30, 300);
                        }
                        #endregion
                        Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 121, 280, 200));
                        mypdfpage.Add(newpdftabpage2);
                        double height = newpdftabpage2.Area.Height;
                        coltop = coltop + 85;
                        tlinerect = new PdfArea(mydoc, 15, 260, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        #endregion
                        table1forpage2 = mydoc.NewTable(font8small, dv.Count + 1, 3, 4);
                        table1forpage2.Columns[0].SetWidth(30);
                        table1forpage2.Columns[1].SetWidth(55);
                        table1forpage2.Columns[2].SetWidth(240);
                        mypdfpage.Add(newpdftabpage2);
                        coltop = coltop + 85;
                        tlinerect = new PdfArea(mydoc, 15, 280, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 55, 260, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 130, 260, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 15, 660, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 0).SetContent("Sem");
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 1).SetContent("Course Code");
                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 2).SetContent("Courses");
                        int j = 0;
                        for (int i = 0; i < dv.Count; i++)
                        {
                            if (i < 20)
                            {
                                table1forpage2.Cell(i + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(i + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(i + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                if (i == 1)
                                {
                                    table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                }
                                else
                                {
                                    table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                }
                            }
                        }
                        newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 13, 263, 480, 500));
                        mypdfpage.Add(newpdftabpage2);
                        tlinerect = new PdfArea(mydoc, 15, 710, 565, 0.01);//03.05.17 barath
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        string subjextsregister = "No. of Subject(s) Registered : " + dv.Count + "";
                        coltop = 740;
                        PdfTextArea ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 17, coltop - 75, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, subjextsregister);
                        mypdfpage.Add(ptcstisign);
                        ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 430, coltop - 55, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                        mypdfpage.Add(ptcstisign);
                        coltop = coltop + 30;
                        PdfTextArea ptccontroller = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 380, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Controller of Examinations");
                        mypdfpage.Add(ptccontroller);
                        PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 30, coltop + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note : Corrections if any is to be reported to the Controller of Examinations immediatly");
                        mypdfpage.Add(ptcsnote);
                        mypdfpage.SaveToDocument();
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Exam_Application_Format-2_" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    public void bindppdf()
    {
        Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
        Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
        Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
        //Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
        string coename = string.Empty;
        string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
        ds.Dispose();
        ds.Reset();
        ds = d2.select_method_wo_parameter(strquery, "Text");
        string Collegename = string.Empty;
        string aff = string.Empty;
        string collacr = string.Empty;
        string dispin = string.Empty;
        if (ds.Tables[0].Rows.Count > 0)
        {
            Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
            aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
            string[] strpa = aff.Split(',');
            aff = strpa[0];
            coename = ds.Tables[0].Rows[0]["coe"].ToString();
            collacr = ds.Tables[0].Rows[0]["acr"].ToString();
            dispin = ds.Tables[0].Rows[0]["districtpin"].ToString();
        }
        Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
        #region TOP DETAILS
        double coltop = 15;
        PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
        mypdfpage.Add(ptc);
        coltop = coltop + 20;
        ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 0, coltop + 10, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
        mypdfpage.Add(ptc);
        coltop = coltop + 15;
        ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 0, coltop + 10, 595, 30), System.Drawing.ContentAlignment.TopCenter, "LIST OF EXAMINATION FEES(TO BE PAID) ");
        mypdfpage.Add(ptc);
        coltop = coltop + 15;
        string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(ddlMonth.SelectedValue.ToString()));
        ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 0, coltop + 10, 595, 30), System.Drawing.ContentAlignment.TopCenter, "EXAM MONTH & YEAR " + strMonthName.ToUpper() + " - " + ddlYear.SelectedValue.ToString() + "");
        mypdfpage.Add(ptc);
        ptc = new PdfTextArea(font2small, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 20, coltop + 30, 595, 30), System.Drawing.ContentAlignment.MiddleLeft, "BATCH :  " + ddlbatch.SelectedValue.ToString() + " - " + ddldegree.SelectedItem.Text.ToString() + "  [ " + ddlbranch.SelectedItem.Text.ToString() + " ]");
        mypdfpage.Add(ptc);
        #endregion
        PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);
        PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);
        mypdfpage.Add(border);
        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
        {
            Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
            mypdfpage.Add(LogoImage, 20, 20, 450);
        }
        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
        {
            Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
            mypdfpage.Add(LogoImage1, 500, 20, 450);
        }
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        printbtn.Visible = false;
        DataSet dsstudinfo = new DataSet();
        // string studinfo = "select len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year from registration r where r.current_semester=" + sem + "and r.degree_code=" + depart_code + "  and cc=0 and delflag=0 and exam_flag<>'Debar' order by len(r.reg_no),r.reg_no,r.stud_name";
        string studinfo = "select distinct r.Reg_No,r.Stud_Name,r.Roll_No, convert(decimal(10,0),ROUND((isnull(ea.total_fee,0) + isnull(EA.extra_fee,0)),0)) as total_fee  from Exam_Details ed,exam_application ea,exam_appl_details ead,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no  and r.Roll_No=ea.roll_no and r.degree_code='" + ddlbranch.SelectedItem.Value.ToString() + "'  and r.college_code='13'  and r.batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedItem.Value.ToString() + "' and ed.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'     ";
        int sno = 0;
        dsstudinfo.Clear();
        dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
        Gios.Pdf.PdfTable studinfoss;
        int rowscc = 0;
        if (dsstudinfo.Tables[0].Rows.Count < 30)
        {
            studinfoss = mydoc.NewTable(Fontsmall1, dsstudinfo.Tables[0].Rows.Count + 1, 6, 5);
        }
        else
        {
            rowscc = dsstudinfo.Tables[0].Rows.Count;
            rowscc = rowscc - 30;
            studinfoss = mydoc.NewTable(Fontsmall1, 31, 6, 5);
        }
        studinfoss.VisibleHeaders = false;
        studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
        studinfoss.Cell(0, 0).SetContent("S.No");
        studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
        studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
        studinfoss.Cell(0, 1).SetContent("Reg.No");
        studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        studinfoss.Cell(0, 2).SetContent("Name");
        studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
        studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
        studinfoss.Cell(0, 3).SetContent("AMOUNT");
        studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
        studinfoss.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
        studinfoss.Cell(0, 4).SetContent("DATE OF PAYMENT");
        studinfoss.Cell(0, 4).SetFont(Fontsmall1bold);
        studinfoss.Cell(0, 5).SetContent("STUDENT SIGNATURE");
        studinfoss.Cell(0, 5).SetFont(Fontsmall1bold);
        studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
        studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
        studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
        studinfoss.Columns[0].SetWidth(8);
        studinfoss.Columns[1].SetWidth(15);
        studinfoss.Columns[2].SetWidth(40);
        studinfoss.Columns[3].SetWidth(15);
        studinfoss.Columns[4].SetWidth(15);
        studinfoss.Columns[5].SetWidth(30);
        int newtablerow = 0;
        Boolean finish = false;
        if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
        {
            for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
            {
                string regno = string.Empty;
                string studname = string.Empty;
                string rollno = string.Empty;
                printbtn.Visible = true;
                FpSpread2.Visible = true;
                //batchyear = dsstudinfo.Tables[0].Rows[studcount]["batch_year"].ToString();
                regno = dsstudinfo.Tables[0].Rows[studcount]["reg_no"].ToString();
                studname = dsstudinfo.Tables[0].Rows[studcount]["stud_name"].ToString();
                rollno = dsstudinfo.Tables[0].Rows[studcount]["roll_no"].ToString();
                sno++;
                studinfoss.Cell(newtablerow + 1, 0).SetContent(sno);
                studinfoss.Cell(newtablerow + 1, 2).SetContent(studname);
                studinfoss.Cell(newtablerow + 1, 1).SetContent(regno);
                studinfoss.Cell(newtablerow + 1, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                studinfoss.Cell(newtablerow + 1, 3).SetContent(dsstudinfo.Tables[0].Rows[studcount]["total_fee"].ToString());
                if (newtablerow == 29)
                {
                    int rowsccheck = rowscc - 30;
                    Gios.Pdf.PdfTablePage addtabletopage = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                    mypdfpage.Add(addtabletopage);
                    mypdfpage.SaveToDocument();
                    mypdfpage = mydoc.NewPage();
                    #region TOP DETAILS
                    coltop = 15;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
                    mypdfpage.Add(ptc);
                    coltop = coltop + 20;
                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 0, coltop + 10, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
                    mypdfpage.Add(ptc);
                    coltop = coltop + 15;
                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 0, coltop + 10, 595, 30), System.Drawing.ContentAlignment.TopCenter, "LIST OF EXAMINATION FEES(TO BE PAID) ");
                    mypdfpage.Add(ptc);
                    coltop = coltop + 15;
                    strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(ddlMonth.SelectedValue.ToString()));
                    ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 0, coltop + 10, 595, 30), System.Drawing.ContentAlignment.TopCenter, "EXAM MONTH & YEAR " + strMonthName.ToUpper() + " - " + ddlYear.SelectedValue.ToString() + "");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(font2small, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 20, coltop + 30, 595, 30), System.Drawing.ContentAlignment.MiddleLeft, "BATCH :  " + ddlbatch.SelectedValue.ToString() + " - " + ddldegree.SelectedItem.Text.ToString() + "  [ " + ddlbranch.SelectedItem.Text.ToString() + " ]");
                    mypdfpage.Add(ptc);
                    #endregion
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 20, 20, 450);
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage1, 500, 20, 450);
                    }
                    mypdfpage.Add(border);
                    if (rowsccheck > 0)
                    {
                        studinfoss = mydoc.NewTable(Fontsmall1, 31, 6, 5);
                        studinfoss.VisibleHeaders = false;
                        studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        studinfoss.Cell(0, 0).SetContent("S.No");
                        studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(0, 1).SetContent("Reg.No");
                        studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(0, 2).SetContent("Name");
                        studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(0, 3).SetContent("AMOUNT");
                        studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 4).SetContent("DATE OF PAYMENT");
                        studinfoss.Cell(0, 4).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 5).SetContent("STUDENT SIGNATURE");
                        studinfoss.Cell(0, 5).SetFont(Fontsmall1bold);
                        studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Columns[0].SetWidth(8);
                        studinfoss.Columns[1].SetWidth(15);
                        studinfoss.Columns[2].SetWidth(30);
                        rowscc = rowscc - 30;
                    }
                    else if (rowsccheck != -30)
                    {
                        studinfoss = mydoc.NewTable(Fontsmall1, rowscc + 1, 6, 5);
                        studinfoss.VisibleHeaders = false;
                        studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        studinfoss.Cell(0, 0).SetContent("S.No");
                        studinfoss.Cell(0, 0).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(0, 1).SetContent("Reg.No");
                        studinfoss.Cell(0, 1).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(0, 2).SetContent("Name");
                        studinfoss.Cell(0, 2).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(0, 3).SetContent("AMOUNT");
                        studinfoss.Cell(0, 3).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(0, 4).SetContent("DATE OF PAYMENT");
                        studinfoss.Cell(0, 4).SetFont(Fontsmall1bold);
                        studinfoss.Cell(0, 5).SetContent("STUDENT SIGNATURE");
                        studinfoss.Cell(0, 5).SetFont(Fontsmall1bold);
                        studinfoss.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Columns[0].SetWidth(8);
                        studinfoss.Columns[1].SetWidth(15);
                        studinfoss.Columns[2].SetWidth(30);
                    }
                    else
                    {
                        studinfoss = mydoc.NewTable(Fontsmall1, rowscc + 1, 11, 5);
                        studinfoss.VisibleHeaders = false;
                        studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        finish = true;
                    }
                    newtablerow = -1;
                }
                newtablerow++;
            }
            if (finish == false)
            {
                Gios.Pdf.PdfTablePage addtabletopage001 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                mypdfpage.Add(addtabletopage001);
                double getheigh = addtabletopage001.Area.Height;
                getheigh = Math.Round(getheigh, 0);
                coltop = getheigh + 200;
                ptc = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 170, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "CONTROLLER OF EXAMINIATION");
                mypdfpage.Add(ptc);
                ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 170, coltop - 15, 595, 30), System.Drawing.ContentAlignment.TopCenter, coename);
                mypdfpage.Add(ptc);
                mypdfpage.SaveToDocument();
            }
            else
            {
                Gios.Pdf.PdfTablePage addtabletopage001 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 120, 553, 800));
                mypdfpage.Add(addtabletopage001);
                double getheigh = addtabletopage001.Area.Height;
                getheigh = Math.Round(getheigh, 0);
                coltop = getheigh + 200;
                ptc = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 170, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "CONTROLLER OF EXAMINIATION");
                mypdfpage.Add(ptc);
                ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 170, coltop - 15, 595, 30), System.Drawing.ContentAlignment.TopCenter, coename);
                mypdfpage.Add(ptc);
                mypdfpage.SaveToDocument();
            }
        }
        string appPath = HttpContext.Current.Server.MapPath("~");
        if (appPath != "")
        {
            string szPath = appPath + "/Report/";
            string szFile = "Marksheets" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
            Response.Buffer = true;
            Response.Clear();
            mydoc.SaveToFile(szPath + szFile);
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
            Response.ContentType = "application/pdf";
            Response.WriteFile(szPath + szFile);
        }
    }

    private void PrintFormat4Mcc()
    {
        try
        {
            FpSpread1.SaveChanges();
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                int vall = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 7].Value);
                if (vall == 1)
                {
                    string courseamt = string.Empty;

                    string courseamount = "select value from Master_Settings where settings='Course Completaion'  " + grpusercd + "";
                    DataSet dsw = da.select_method_wo_parameter(courseamount, "text");
                    if (dsw.Tables.Count > 0 && dsw.Tables[0].Rows.Count > 0)
                    {
                        string cramt = Convert.ToString(dsw.Tables[0].Rows[0]["value"]);
                        string[] cramt1 = cramt.Split(';');
                        courseamt = Convert.ToString(cramt1[2]);
                    }
                    //if (courseamount > 0)
                    //{
                    //}

                    DataSet ds = new DataSet();
                    Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                    string examyear = ddlYear.SelectedItem.Text.ToString();
                    string exammonth = ddlMonth.SelectedItem.Value.ToString();
                    string examno = da.GetFunctionv("select exam_code from Exam_Details where Exam_Month='" + ddlMonth.SelectedItem.Value.ToString() + "' and Exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedItem.Value.ToString() + "' and batch_year=" + ddlbatch.SelectedItem.Text.ToString() + "");
                    string edulevel = da.GetFunctionv("	select Edu_Level from Course where Course_Id in (select Course_Id from Degree where Degree_Code='" + ddlbranch.SelectedItem.Value.ToString() + "')");
                    string getsemmarkfee = da.GetFunction("select value from coe_feemaster where settings='Cost_of_StMt_Marks' and edulevel='" + edulevel + "'");
                    //string getsemmarkfee = d2.GetFunction("select value from Master_Settings where settings='Semester Mark Sheet'");
                    string getappfee = da.GetFunction("select value from coe_feemaster where settings='Cost_of_Appliaction' and edulevel='" + edulevel + "'");
                    string rollno = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
                    string regno = FpSpread1.Sheets[0].Cells[i, 2].Text.ToString();
                    string strquery = "select * from collinfo where college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'";
                    ds.Dispose();
                    ds.Reset();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    string strexamdetailsquery = " select ed.Exam_year,sm.semester,ed.Exam_Month,ea.exam_code,ed.batch_year,ed.degree_code,ea.roll_no,ead.subject_no,s.subject_name,s.subject_code,ead.attempts,s.curfee,s.arrfee,ISNULL(s.subjectpriority,'0' ) subjectpriority   ";

                    //Modified on 25/9/2017 by prabha
                    strexamdetailsquery = strexamdetailsquery + " from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,syllabus_master sm where ed.exam_code=ea.exam_code and sm.syll_code=s.syll_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedValue + "' and ea.roll_no='" + rollno + "' order by sm.semester desc,subjectpriority ";

                    DataSet dsexamdetails = d2.select_method_wo_parameter(strexamdetailsquery, "Text");
                    string strregquery = "select r.batch_year,r.degree_code,r.roll_no,r.stud_name,r.Reg_No,r.Current_Semester,c.Edu_Level Edulevel,c.Course_Name,de.Dept_Name from Registration r,Degree d,course c,Department de  where r.degree_code=d.degree_code and d.course_id=c.course_id and d.Dept_Code=de.Dept_Code and r.roll_no='" + rollno + "' order by r.batch_year,r.degree_code,r.reg_no";
                    DataSet dsreg = d2.select_method_wo_parameter(strregquery, "Text");
                    for (int srp = 0; srp < dsreg.Tables[0].Rows.Count; srp++)
                    {
                        rollno = dsreg.Tables[0].Rows[srp]["roll_no"].ToString();
                        string batchyear = dsreg.Tables[0].Rows[srp]["batch_year"].ToString();
                        string degree_code = dsreg.Tables[0].Rows[srp]["degree_code"].ToString();
                        string sem = dsreg.Tables[0].Rows[srp]["Current_Semester"].ToString();
                        //   string edulevel = dsreg.Tables[0].Rows[srp]["Edulevel"].ToString();
                        string GetDuration = d2.GetFunction(" select duration from degree where degree_code ='" + degree_code + "'");
                        if (GetDuration.Trim() != "0" && GetDuration.Trim() != "")
                        {
                            if (Convert.ToInt32(GetDuration) < Convert.ToInt32(sem))
                            {
                                sem = "Private";
                            }
                        }
                        string collegecode = "13";
                        string sname = dsreg.Tables[0].Rows[srp]["stud_name"].ToString();
                        string sdegree = dsreg.Tables[0].Rows[srp]["Course_Name"].ToString();
                        string sdept = dsreg.Tables[0].Rows[srp]["Dept_Name"].ToString();
                        dsexamdetails.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                        DataView dvexamdetails = dsexamdetails.Tables[0].DefaultView;
                        if (dvexamdetails.Count > 0)
                        {
                            Font Fontbold = new Font("Book Antiqua", 15, FontStyle.Regular);
                            Font fbold = new Font("Book Antiqua", 15, FontStyle.Bold);
                            Font Fontsmall = new Font("Book Antiqua", 11, FontStyle.Regular);
                            Font fontmedium = new Font("Book Antiqua", 11, FontStyle.Regular);
                            Font fontmediumb = new Font("Book Antiqua", 11, FontStyle.Bold);
                            Font fontname = new Font("Book Antiqua", 11, FontStyle.Bold);
                            string Collegename = string.Empty;
                            string aff = string.Empty;
                            string address = string.Empty;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString() + " ( " + ds.Tables[0].Rows[0]["category"].ToString() + " )";
                                aff = "(Affiliated to the " + ds.Tables[0].Rows[0]["university"].ToString() + ")";
                                address = ds.Tables[0].Rows[0]["address1"].ToString() + " , " + ds.Tables[0].Rows[0]["address2"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                            }
                            PdfArea tete = new PdfArea(mydoc, 5, 5, 585, 830);
                            PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                            mypdfpage.Add(pr1);
                            int coltop = 5;
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 25, 25, 400);
                            }
                            PdfTextArea ptc = new PdfTextArea(fbold, System.Drawing.Color.Black, new PdfArea(mydoc, 100, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Madras Christian College (Autonomous)");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            PdfTextArea ptc02 = new PdfTextArea(fbold, System.Drawing.Color.Black, new PdfArea(mydoc, 100, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Tambaram,Chennai - 600 059");
                            mypdfpage.Add(ptc02);
                            double thefee = 0;
                            double thearrfee = 0;
                            double prafee = 0;
                            double praarrfee = 0;
                            //examno = "November";
                            //examyear = "2015";
                            coltop = coltop + 20;
                            PdfTextArea ptc03 = new PdfTextArea(fbold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 100, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "End of Semester Examinations - " + ddlMonth.SelectedItem.Text.ToString().ToUpper() + " - " + examyear + "");
                            mypdfpage.Add(ptc03);
                            coltop = coltop + 20;
                            PdfTextArea ptc031a = new PdfTextArea(fbold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 100, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "APPLICATION FORM ");
                            mypdfpage.Add(ptc031a);
                            coltop = coltop + 20;
                            string lastDate = string.Empty;
                            lastDate = txtappllastdate.Text.Trim();
                            if (lastDate == string.Empty)
                            {
                                lastDate = "          ";
                            }
                            PdfTextArea ptclats = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 100, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "(Last date for Online Fee Payment : " + lastDate + " )");
                            //Last date for Submission of  Exam Application Form 
                            mypdfpage.Add(ptclats);
                            coltop = coltop + 15;
                            PdfTextArea ptcodt = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 15, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(ptcodt);
                            coltop = coltop + 15;
                            PdfTextArea ptc07 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name of Student");
                            mypdfpage.Add(ptc07);
                            PdfTextArea ptc08na = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 130, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + sname.ToString() + "");
                            mypdfpage.Add(ptc08na);
                            // string[] spdeg = ddlbranch.SelectedItem.Text.ToString().Split(';');
                            PdfTextArea ptc071 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Reg.No");
                            mypdfpage.Add(ptc071);
                            PdfTextArea ptc071a = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                              new PdfArea(mydoc, 400, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + regno + "");
                            mypdfpage.Add(ptc071a);
                            coltop = coltop + 15;
                            PdfTextArea ptc08 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree");
                            mypdfpage.Add(ptc08);
                            PdfTextArea ptc08na1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 130, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + sdegree.ToString() + "");
                            mypdfpage.Add(ptc08na1);
                            PdfTextArea ptc081 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Group");
                            mypdfpage.Add(ptc081);
                            PdfTextArea ptc081a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydoc, 400, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + sdept.ToString() + "");
                            mypdfpage.Add(ptc081a);
                            coltop = coltop + 15;
                            PdfTextArea ptcsem = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Semester");
                            mypdfpage.Add(ptcsem);
                            PdfTextArea ptcsem1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 130, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + sem + "");
                            mypdfpage.Add(ptcsem1);
                            coltop = coltop + 10;
                            PdfTextArea ptcodt1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 15, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(ptcodt1);
                            double examfee = 0;
                            coltop = coltop + 15;
                            PdfTextArea ptsrnor = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "S.No");
                            mypdfpage.Add(ptsrnor);
                            PdfTextArea ptcsubcoder = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 60, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Subject Code");
                            mypdfpage.Add(ptcsubcoder);
                            PdfTextArea ptcsubnamer = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 150, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Subject Name");
                            mypdfpage.Add(ptcsubnamer);
                            PdfTextArea ptcarrearr = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 500, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "(Regular/Arrear)");
                            mypdfpage.Add(ptcarrearr);
                            int srno = 0;
                            double currentFees = 0;
                            double arrearFees = 0;
                            for (int sn = 0; sn < dvexamdetails.Count; sn++)
                            {
                                coltop = coltop + 15;
                                srno++;
                                string subcode = dvexamdetails[sn]["subject_code"].ToString();
                                string subname = dvexamdetails[sn]["subject_name"].ToString();
                                string arrr = dvexamdetails[sn]["attempts"].ToString();
                                string fees = dvexamdetails[sn]["curfee"].ToString();
                                double feesAmt = 0;
                                if (arrr.Trim() != "0")
                                {
                                    arrr = "arrear";
                                    fees = dvexamdetails[sn]["arrfee"].ToString();
                                    double.TryParse(fees, out feesAmt);
                                    arrearFees += feesAmt;
                                }
                                else
                                {
                                    arrr = "Current";
                                    fees = dvexamdetails[sn]["curfee"].ToString();
                                    double.TryParse(fees, out feesAmt);
                                    currentFees += feesAmt;
                                }
                                string chckname = subname.Trim().ToLower();
                                PdfTextArea ptsrno = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, srno.ToString());
                                mypdfpage.Add(ptsrno);
                                PdfTextArea ptcsubcode = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 60, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, subcode.ToString());
                                mypdfpage.Add(ptcsubcode);
                                PdfTextArea ptcsubname = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 150, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, subname.ToString());
                                mypdfpage.Add(ptcsubname);
                                string isarr = arrr;
                                double subfee = 0;
                                double.TryParse(fees, out subfee);
                                if (isarr.Trim().ToLower() == "arrear")
                                {
                                    PdfTextArea ptcarrear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                             new PdfArea(mydoc, 500, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "(Arrear)");
                                    mypdfpage.Add(ptcarrear);
                                    examfee = examfee + Convert.ToDouble(subfee);
                                }
                                else
                                {
                                    PdfTextArea ptcarrear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 500, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "(Regular)");
                                    mypdfpage.Add(ptcarrear);
                                    examfee = examfee + Convert.ToDouble(subfee);
                                }
                            }
                            double totfee = examfee;
                            coltop = coltop + 10;
                            PdfTextArea ptcodt3 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 15, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(ptcodt3);
                            coltop = coltop + 10;
                            PdfTextArea ptcapp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Application Fee");
                            mypdfpage.Add(ptcapp);
                            double applicfee = 0;
                            if (getappfee.Trim() != "" && getappfee.Trim() != "0" && getappfee != null)
                            {
                                applicfee = Convert.ToDouble(getappfee);
                            }
                            totfee = totfee + applicfee;
                            PdfTextArea ptcappfee = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 170, coltop, 80, 30), System.Drawing.ContentAlignment.MiddleRight, applicfee.ToString());
                            mypdfpage.Add(ptcappfee);
                            coltop = coltop + 15;
                            bool courseflag = false;
                            BindMandFeeGrid();
                            foreach (GridViewRow gv1 in gridMandFee.Rows)
                            {
                                CheckBox chk = (gv1.FindControl("cb_SelFee") as CheckBox);
                                if (chk.Checked == true)
                                {
                                    Label lblfeenam = (Label)gv1.FindControl("lblFeeName");
                                    string feenam = lblfeenam.Text;
                                    if (feenam.ToLower() == "course completaion")
                                        courseflag = true;
                                }
                            }
                            double semmarkfee = 0;
                            if (courseflag == true)
                            {
                                PdfTextArea ptcsemf = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Semester Mark Sheet Fee / PPRF");
                                mypdfpage.Add(ptcsemf);
                                double couramt = 0;
                                double.TryParse(courseamt, out couramt);
                                if (getsemmarkfee.Trim() != "" && getsemmarkfee.Trim() != "0" && getsemmarkfee != null)
                                {
                                    semmarkfee = Convert.ToDouble(getsemmarkfee);
                                }
                                semmarkfee = semmarkfee + couramt;
                            }
                            else
                            {
                                PdfTextArea ptcsemf = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Semester Mark Sheet Fee");
                                mypdfpage.Add(ptcsemf);
                                if (getsemmarkfee.Trim() != "" && getsemmarkfee.Trim() != "0" && getsemmarkfee != null)
                                {
                                    semmarkfee = Convert.ToDouble(getsemmarkfee);
                                }
                            }



                            totfee = totfee + semmarkfee;
                            PdfTextArea ptcsemffee = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 170, coltop, 80, 30), System.Drawing.ContentAlignment.MiddleRight, semmarkfee.ToString());
                            mypdfpage.Add(ptcsemffee);
                            //coltop = coltop + 15;
                            //PdfTextArea ptcex = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                                    new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Examination Fee");
                            //mypdfpage.Add(ptcex);
                            //PdfTextArea ptcexfee = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                                    new PdfArea(mydoc, 170, coltop, 80, 30), System.Drawing.ContentAlignment.MiddleRight, examfee.ToString());
                            //mypdfpage.Add(ptcexfee);

                            coltop = coltop + 15;
                            PdfTextArea ptcCurrent = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Regular Fee");
                            mypdfpage.Add(ptcCurrent);
                            PdfTextArea ptcCurrentVal = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 170, coltop, 80, 30), System.Drawing.ContentAlignment.MiddleRight, currentFees.ToString());
                            mypdfpage.Add(ptcCurrentVal);

                            coltop = coltop + 15;
                            PdfTextArea ptcArrear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Arrear Fee");
                            mypdfpage.Add(ptcArrear);
                            PdfTextArea ptcArrearVal = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 170, coltop, 80, 30), System.Drawing.ContentAlignment.MiddleRight, arrearFees.ToString());
                            mypdfpage.Add(ptcArrearVal);

                            coltop = coltop + 15;
                            PdfTextArea ptctot = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total Fee");
                            mypdfpage.Add(ptctot);
                            PdfTextArea ptctotfee = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 170, coltop, 80, 30), System.Drawing.ContentAlignment.MiddleRight, totfee.ToString());
                            mypdfpage.Add(ptctotfee);
                            coltop = coltop + 10;
                            PdfTextArea ptcodt4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 15, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(ptcodt4);
                            coltop = coltop + 10;
                            PdfTextArea ptcde = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 700, 30), System.Drawing.ContentAlignment.MiddleLeft, "I hereby declare that I have selected the given list of papers and request the COE");
                            mypdfpage.Add(ptcde);
                            coltop = coltop + 15;
                            PdfTextArea ptcde1 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 700, 30), System.Drawing.ContentAlignment.MiddleLeft, " to register these papers for the End of Semester Examinations," + ddlMonth.SelectedItem.Text.ToString().ToUpper() + " - " + examyear + "");
                            mypdfpage.Add(ptcde1);
                            coltop = coltop + 15;
                            PdfTextArea ptcodt5 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 15, coltop, 700, 30), System.Drawing.ContentAlignment.MiddleLeft, "------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            //
                            mypdfpage.Add(ptcodt5);
                            coltop = coltop + 15;
                            PdfTextArea note = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 700, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note:Students who appear for arrear exams are asked to pay their arrear fees");
                            mypdfpage.Add(note);
                            coltop = coltop + 15;
                            PdfTextArea note1 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 30, coltop, 700, 30), System.Drawing.ContentAlignment.MiddleLeft, "through Online (visit: www.mcc.edu.in). No payment is required for regular papers.");
                            mypdfpage.Add(note1);
                            coltop = coltop + 15;

                            PdfTextArea ptcodt6 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 15, coltop, 700, 30), System.Drawing.ContentAlignment.MiddleLeft, "------------------------------------------------------------------------------------------------------------------------------------------------------------");

                            //
                            mypdfpage.Add(ptcodt6);
                            coltop = coltop + 30;
                            coltop = coltop + 13;
                            PdfTextArea ptcdate = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date    : " + DateTime.Now.ToString("dd-MM-yyyy") + "");
                            mypdfpage.Add(ptcdate);
                            PdfTextArea ptcstisign = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 400, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "SIGNATURE OF THE STUDENT");
                            mypdfpage.Add(ptcstisign);
                            coltop = 590;
                            PdfTextArea ptcodt9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 15, mydoc.PageHeight - 60, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(ptcodt9);
                            coltop = coltop + 10;
                            PdfTextArea ptccon = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 30, mydoc.PageHeight - 35, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Contact examinations office for clarifications,if any.");
                            mypdfpage.Add(ptccon);
                            mypdfpage.SaveToDocument();
                        }
                    }
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        Response.Buffer = true;
                        Response.Clear();
                        string szPath = appPath + "/Report/";
                        string szFile = regno + DateTime.Now.ToString("ddMMyyyyHHmmsstt") + ".pdf";
                        mydoc.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (chkInclFinMand.Checked)
        {
            WithFinanceDelete();
        }
        else
        {
            WithOutFinanceDelete();
        }
    }

    public void WithFinanceDelete()
    {
        try
        {
            FpSpread1.SaveChanges();
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            string examcodequery = "Select * from exam_details where exam_year='" + examyear + "' and exam_month='" + exammonth + "' and batch_year='" + batchyear + "' and degree_code='" + degreecode + "'";
            DataSet dsexam = d2.select_method_wo_parameter(examcodequery, "text");
            string feecat = da.GetFunctionv("select LinkValue from New_InsSettings where LinkName='Fee Yearwise' and user_code='" + Session["usercode"].ToString() + "' and college_code='" + ddlcollege.SelectedValue + "'");
            string examcode = string.Empty;
            for (int i = 0; i < dsexam.Tables[0].Rows.Count; i++)
            {
                if (examcode.Trim() == "")
                {
                    examcode = dsexam.Tables[0].Rows[i]["exam_code"].ToString();
                }
                else
                {
                    examcode = examcode + "," + dsexam.Tables[0].Rows[i]["exam_code"].ToString();
                }
            }
            if (examcode.Trim() != "")
            {
                examcode = " and ed.exam_code in(" + examcode + ")";
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Exam Conducted";
                return;
            }
            #region Mandatory Fees Values
            ArrayList arrHeaderFk = new ArrayList();
            ArrayList arrLedgerFk = new ArrayList();
            ArrayList arrfeeValue = new ArrayList();
            ArrayList arrMandFees = MandatoryFees(out arrHeaderFk, out arrLedgerFk, out arrfeeValue);
            StringBuilder headerCodes = new StringBuilder();
            StringBuilder ledgerCodes = new StringBuilder();
            foreach (string hdr in arrHeaderFk)
            {
                headerCodes.Append(hdr + "','");
            }
            if (headerCodes.Length > 2)
            {
                headerCodes.Remove(headerCodes.Length - 3, 3);
            }
            foreach (string lgr in arrLedgerFk)
            {
                ledgerCodes.Append(lgr + "','");
            }
            if (ledgerCodes.Length > 2)
            {
                ledgerCodes.Remove(ledgerCodes.Length - 3, 3);
            }
            #endregion
            string ChallGenerateRegNo = string.Empty;
            Boolean saveflag = false;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                    string GetSem = d2.GetFunction("select current_semester from registration where roll_no ='" + rollno + "'");
                    string GetAppNo = d2.GetFunction("select app_no from registration where roll_no ='" + rollno + "'");
                    string addtype = string.Empty;
                    string currentsem = string.Empty;
                    string currentfinyear = da.getCurrentFinanceYear(usercode, collegecode);
                    bool isPassedOut = false;
                    int currentSemester = 0;
                    int.TryParse(GetSem, out currentSemester);
                    string maxDuration = d2.GetFunction("select distinct NDurations from ndegree where Degree_code='" + degreecode + " and batch_year='" + batchyear + "");
                    int maxDurations = 0;
                    if (maxDuration.Trim() == "" || maxDuration == null || maxDuration == "0")
                    {
                        maxDuration = d2.GetFunction("select distinct duration,first_year_nonsemester  from degree where degree_code='" + degreecode + "'");
                    }
                    int.TryParse(maxDuration, out maxDurations);
                    if (currentSemester == maxDurations + 1)
                    {
                        isPassedOut = true;
                    }
                    string qryPassedOutFinYear = string.Empty;
                    if (isPassedOut)
                        qryPassedOutFinYear = " and FinYearFK='" + currentfinyear + "' ";
                    if (GetSem.Trim() != "0")
                    {
                        if (feecat == "0")
                        {
                            addtype = " Semester";
                            currentsem = GetSem + addtype;
                        }
                        else
                        {
                            addtype = " Year";
                            if (currentsem.Trim() == "1" || currentsem.Trim() == "2")
                            {
                                currentsem = "1" + addtype;
                            }
                            else if (currentsem.Trim() == "3" || currentsem.Trim() == "4")
                            {
                                currentsem = "2" + addtype;
                            }
                            else if (currentsem.Trim() == "5" || currentsem.Trim() == "6")
                            {
                                currentsem = "3" + addtype;
                            }
                            else if (currentsem.Trim() == "7" || currentsem.Trim() == "8")
                            {
                                currentsem = "4" + addtype;
                            }
                            else if (currentsem.Trim() == "9" || currentsem.Trim() == "10")
                            {
                                currentsem = "5" + addtype;
                            }
                        }
                        currentsem = da.GetFunctionv("select TextCode from textvaltable where TextCriteria='Feeca' and college_code='" + ddlcollege.SelectedValue + "' and TextVal='" + currentsem + "'");
                        if (currentsem.Trim() != "0")
                        {
                            DataSet GetChallanno = d2.select_method_wo_parameter("select distinct challanno from ft_challandet where headerfk in ('" + headerCodes + "') and ledgerfk in ('" + ledgerCodes + "') and feecategory='" + currentsem + "' and app_no ='" + GetAppNo + "' " + qryPassedOutFinYear, "Text");
                            if (GetChallanno.Tables.Count > 0 && GetChallanno.Tables[0].Rows.Count == 0)
                            {
                                int DelteFee = d2.update_method_wo_parameter("delete from ft_feeallot where app_no ='" + GetAppNo + "' and headerfk in ('" + headerCodes + "') and ledgerfk in ('" + ledgerCodes + "') and feecategory='" + currentsem + "' " + qryPassedOutFinYear, "Text");
                                string deletequery = "delete ead from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no='" + rollno + "' and ed.exam_year='" + examyear + "' and ed.exam_month='" + exammonth + "'";
                                int delva = d2.update_method_wo_parameter(deletequery, "text");
                                deletequery = "delete ea from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ea.roll_no='" + rollno + "' and ed.exam_year='" + examyear + "' and ed.exam_month='" + exammonth + "'";
                                delva = d2.update_method_wo_parameter(deletequery, "text");
                                saveflag = true;
                            }
                            else
                            {
                                if (ChallGenerateRegNo.Trim() == "")
                                {
                                    ChallGenerateRegNo = rollno;
                                }
                                else
                                {
                                    ChallGenerateRegNo = ChallGenerateRegNo + "," + rollno;
                                }
                            }
                        }
                    }
                }
            }
            if (saveflag == true)
            {
                if (ChallGenerateRegNo.Trim() != "")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Not Deleted Roll No : " + ChallGenerateRegNo + "";
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Deleted Sucessfully.')", true);
                loadexamdetails();
            }
            else
            {
                if (ChallGenerateRegNo.Trim() != "")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Not Deleted Roll No : " + ChallGenerateRegNo + "";
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select The Student And Then Proceed";
                }
            }
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            //d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    public void WithOutFinanceDelete()
    {
        try
        {
            FpSpread1.SaveChanges();
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            string examcodequery = "Select * from exam_details where exam_year='" + examyear + "' and exam_month='" + exammonth + "' and batch_year='" + batchyear + "' and degree_code='" + degreecode + "'";
            DataSet dsexam = d2.select_method_wo_parameter(examcodequery, "text");
            string examcode = string.Empty;
            for (int i = 0; i < dsexam.Tables[0].Rows.Count; i++)
            {
                if (examcode.Trim() == "")
                {
                    examcode = dsexam.Tables[0].Rows[i]["exam_code"].ToString();
                }
                else
                {
                    examcode = examcode + "," + dsexam.Tables[0].Rows[i]["exam_code"].ToString();
                }
            }
            if (examcode.Trim() != "")
            {
                examcode = " and ed.exam_code in(" + examcode + ")";
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Exam Conducted";
                return;
            }
            Boolean saveflag = false;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                    saveflag = true;
                    string deletequery = "delete ead from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no='" + rollno + "' and ed.exam_year='" + examyear + "' and ed.exam_month='" + exammonth + "'";
                    int delva = d2.update_method_wo_parameter(deletequery, "text");
                    deletequery = "delete ea from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ea.roll_no='" + rollno + "' and ed.exam_year='" + examyear + "' and ed.exam_month='" + exammonth + "'";
                    delva = d2.update_method_wo_parameter(deletequery, "text");
                }
            }
            if (saveflag == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Deleted Sucessfully.')", true);
                loadexamdetails();
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Student And Then Proceed";
            }
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    protected void btndeleteAll_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            string examcodequery = "Select * from exam_details where exam_year='" + examyear + "' and exam_month='" + exammonth + "' and batch_year='" + batchyear + "' and degree_code='" + degreecode + "'";
            DataSet dsexam = d2.select_method_wo_parameter(examcodequery, "text");
            string feecat = da.GetFunctionv("select LinkValue from New_InsSettings where LinkName='Fee Yearwise' and user_code='" + Session["usercode"].ToString() + "' and college_code='" + ddlcollege.SelectedValue + "'");
            string examcode = string.Empty;
            string examCode1 = string.Empty;
            for (int i = 0; i < dsexam.Tables[0].Rows.Count; i++)
            {
                if (examcode.Trim() == "")
                {
                    examcode = dsexam.Tables[0].Rows[i]["exam_code"].ToString();
                }
                else
                {
                    examcode = examcode + "," + dsexam.Tables[0].Rows[i]["exam_code"].ToString();
                }
            }
            if (examcode.Trim() != "")
            {
                examCode1 = "exam_code in(" + examcode + ")";
                examcode = " and ed.exam_code in(" + examcode + ")";

            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Exam Conducted";
                return;
            }

            bool ismark = false;
            string checkmark = "select * from mark_entry where " + examCode1;
            DataTable dtmark = dirAcc.selectDataTable(checkmark);
            if (dtmark.Rows.Count > 0)
                ismark = true;
            if (!ismark)
            {
                #region Mandatory Fees Values
                ArrayList arrHeaderFk = new ArrayList();
                ArrayList arrLedgerFk = new ArrayList();
                ArrayList arrfeeValue = new ArrayList();
                ArrayList arrMandFees = MandatoryFees(out arrHeaderFk, out arrLedgerFk, out arrfeeValue);
                StringBuilder headerCodes = new StringBuilder();
                StringBuilder ledgerCodes = new StringBuilder();
                foreach (string hdr in arrHeaderFk)
                {
                    headerCodes.Append(hdr + "','");
                }
                if (headerCodes.Length > 2)
                {
                    headerCodes.Remove(headerCodes.Length - 3, 3);
                }
                foreach (string lgr in arrLedgerFk)
                {
                    ledgerCodes.Append(lgr + "','");
                }
                if (ledgerCodes.Length > 2)
                {
                    ledgerCodes.Remove(ledgerCodes.Length - 3, 3);
                }
                #endregion
                string ChallGenerateRegNo = string.Empty;
                Boolean saveflag = false;
                for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                {
                    saveflag = false;
                    string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                    string GetSem = d2.GetFunction("select current_semester from registration where roll_no ='" + rollno + "'");
                    string GetAppNo = d2.GetFunction("select app_no from registration where roll_no ='" + rollno + "'");
                    string addtype = string.Empty;
                    string currentsem = string.Empty;
                    string currentfinyear = da.getCurrentFinanceYear(usercode, collegecode);
                    bool isPassedOut = false;
                    int currentSemester = 0;
                    int.TryParse(GetSem, out currentSemester);
                    string maxDuration = d2.GetFunction("select distinct NDurations from ndegree where Degree_code='" + degreecode + " and batch_year='" + batchyear + "");
                    int maxDurations = 0;
                    if (maxDuration.Trim() == "" || maxDuration == null || maxDuration == "0")
                    {
                        maxDuration = d2.GetFunction("select distinct duration,first_year_nonsemester  from degree where degree_code='" + degreecode + "'");
                    }
                    int.TryParse(maxDuration, out maxDurations);
                    if (currentSemester == maxDurations + 1)
                    {
                        isPassedOut = true;
                    }
                    string qryPassedOutFinYear = string.Empty;
                    if (isPassedOut)
                        qryPassedOutFinYear = " and FinYearFK='" + currentfinyear + "' ";
                    if (GetSem.Trim() != "0")
                    {
                        if (feecat == "0")
                        {
                            addtype = " Semester";
                            currentsem = GetSem + addtype;
                        }
                        else
                        {
                            addtype = " Year";
                            if (currentsem.Trim() == "1" || currentsem.Trim() == "2")
                            {
                                currentsem = "1" + addtype;
                            }
                            else if (currentsem.Trim() == "3" || currentsem.Trim() == "4")
                            {
                                currentsem = "2" + addtype;
                            }
                            else if (currentsem.Trim() == "5" || currentsem.Trim() == "6")
                            {
                                currentsem = "3" + addtype;
                            }
                            else if (currentsem.Trim() == "7" || currentsem.Trim() == "8")
                            {
                                currentsem = "4" + addtype;
                            }
                            else if (currentsem.Trim() == "9" || currentsem.Trim() == "10")
                            {
                                currentsem = "5" + addtype;
                            }
                        }
                        currentsem = da.GetFunctionv("select TextCode from textvaltable where TextCriteria='Feeca' and college_code='" + ddlcollege.SelectedValue + "' and TextVal='" + currentsem + "'");
                        if (currentsem.Trim() != "0")
                        {
                            DataSet GetChallanno = d2.select_method_wo_parameter("select distinct challanno from ft_challandet where headerfk in ('" + headerCodes + "') and ledgerfk in ('" + ledgerCodes + "') and feecategory='" + currentsem + "' and app_no ='" + GetAppNo + "' " + qryPassedOutFinYear, "Text");
                            if (GetChallanno.Tables.Count > 0 && GetChallanno.Tables[0].Rows.Count == 0)
                            {
                                int DelteFee = d2.update_method_wo_parameter("delete from ft_feeallot where app_no ='" + GetAppNo + "' and headerfk in ('" + headerCodes + "') and ledgerfk in ('" + ledgerCodes + "') and feecategory='" + currentsem + "' " + qryPassedOutFinYear, "Text");
                                string deletequery = "delete ead from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no='" + rollno + "' and ed.exam_year='" + examyear + "' and ed.exam_month='" + exammonth + "'";
                                int delva = d2.update_method_wo_parameter(deletequery, "text");
                                deletequery = "delete ea from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ea.roll_no='" + rollno + "' and ed.exam_year='" + examyear + "' and ed.exam_month='" + exammonth + "'";
                                delva = d2.update_method_wo_parameter(deletequery, "text");
                                saveflag = true;
                            }
                            else
                            {
                                if (ChallGenerateRegNo.Trim() == "")
                                {
                                    ChallGenerateRegNo = rollno;
                                }
                                else
                                {
                                    ChallGenerateRegNo = ChallGenerateRegNo + "," + rollno;
                                }
                            }
                        }
                    }
                }
                if (saveflag == true)
                {
                    if (ChallGenerateRegNo.Trim() != "")
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Not Deleted Roll No : " + ChallGenerateRegNo + "";
                    }
                    else
                    {
                        string delExamDetails = "delete Exam_Details where " + examCode1;
                        int delflag = da.update_method_wo_parameter(delExamDetails, "text");
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Deleted Sucessfully.')", true);
                    loadexamdetails();
                }
                else
                {
                    if (ChallGenerateRegNo.Trim() != "")
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Not Deleted Roll No : " + ChallGenerateRegNo + "";
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Select The Student And Then Proceed";
                    }
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Delete Mark Entry";
            }
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            //d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    //added by Mohamed Idhris -- Sep 20-2016
    protected void btnSetFees_Click(object sender, EventArgs e)
    {
        BindMandFeeGrid();
        divMandFee.Visible = true;
    }

    private void BindMandFeeGrid()
    {
        DataTable dtMandFee = new DataTable();
        dtMandFee.Columns.Add("FeeName");
        dtMandFee.Columns.Add("MandOrNot");
        dtMandFee.Rows.Add("Application Form", "0");
        dtMandFee.Rows.Add("Semester Mark Sheet", "0");
        dtMandFee.Rows.Add("Theory", "0");
        dtMandFee.Rows.Add("Practical", "0");
        dtMandFee.Rows.Add("Project", "0");
        dtMandFee.Rows.Add("Field Work", "0");
        dtMandFee.Rows.Add("Viva Voice", "0");
        dtMandFee.Rows.Add("Disseration", "0");
        dtMandFee.Rows.Add("Consolidate Mark Sheet", "0");
        dtMandFee.Rows.Add("Course Completaion", "0");
        dtMandFee.Rows.Add("Online Application Fee", "0");
        dtMandFee.Rows.Add("Arrear Theory", "0");
        dtMandFee.Rows.Add("Arrear Practical", "0");
        dtMandFee.Rows.Add("Central Valuation", "0");
        dtMandFee.Rows.Add("Syllabi & Curricular", "0");
        gridMandFee.DataSource = dtMandFee;
        gridMandFee.DataBind();
    }

    protected void gridMandFee_OnDataBound(object sender, EventArgs e)
    {
        setPrevVal();
    }

    private void setPrevVal()
    {
        try
        {
            foreach (GridViewRow gRow in gridMandFee.Rows)
            {
                Label lblFee = (Label)gRow.FindControl("lblFeeName");
                string linkVal = lblFee.Text.Trim() + "@#MandatoryFee";
                CheckBox chkSel = (CheckBox)gRow.FindControl("cb_SelFee");
                byte prevVal = Convert.ToByte(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'").Trim());
                if (prevVal == 1)
                {
                    chkSel.Checked = true;
                }
                else
                {
                    chkSel.Checked = false;
                }
            }
        }
        catch { }
    }

    protected void btn_ResetMandFee_Click(object sender, EventArgs e)
    {
        setPrevVal();
    }

    protected void chkSelAllMand_OnCheckChange(object sender, EventArgs e)
    {
        foreach (GridViewRow gRow in gridMandFee.Rows)
        {
            CheckBox chkSel = (CheckBox)gRow.FindControl("cb_SelFee");
            if (chkSelAllMand.Checked)
            {
                chkSel.Checked = true;
            }
            else
            {
                chkSel.Checked = false;
            }
        }
    }

    protected void btn_SaveMandfee_Click(object sender, EventArgs e)
    {
        try
        {
            string insqry = string.Empty;
            foreach (GridViewRow gRow in gridMandFee.Rows)
            {
                Label lblFee = (Label)gRow.FindControl("lblFeeName");
                string linkVal = lblFee.Text.Trim() + "@#MandatoryFee";
                CheckBox chkSel = (CheckBox)gRow.FindControl("cb_SelFee");
                byte saveVal = 0;
                if (chkSel.Checked)
                {
                    saveVal = 1;
                }
                insqry = "if exists (select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "' ) update New_InsSettings set LinkValue ='" + saveVal + "' where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('" + linkVal + "','" + saveVal + "','" + usercode + "','" + ddlcollege.SelectedValue + "')";
                d2.update_method_wo_parameter(insqry, "Text");
            }
            byte Incfinance = 0;
            if (chkInclFinMand.Checked)
            {
                Incfinance = 1;
            }
            insqry = "if exists (select LinkValue from New_InsSettings where LinkName='IncludeFinance' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "' ) update New_InsSettings set LinkValue ='" + Incfinance + "' where LinkName='IncludeFinance' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('IncludeFinance','" + Incfinance + "','" + usercode + "','" + ddlcollege.SelectedValue + "')";
            d2.update_method_wo_parameter(insqry, "Text");
            imgAlert.Visible = true;
            lbl_alert.Text = "Saved Successfully";
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    protected void btn_CloseMandFee_Click(object sender, EventArgs e)
    {
        divMandFee.Visible = false;
    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
        if (lbl_alert.Text == "Receipt No Not Set For All Headers")
        {
            Response.Redirect("~/Finance.aspx");
        }
    }

    private void getFinanceNotAppliedAmount()
    {
        try
        {
            FpSpread1.SaveChanges();
            if (Convert.ToString(ddlYear.SelectedValue).Trim() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((Convert.ToString(ddlMonth.SelectedValue).Trim() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            int insupdateval = 0;
            string degreecode = Convert.ToString(ddlbranch.SelectedValue).Trim();
            string batchyear = Convert.ToString(ddlbatch.SelectedValue).Trim();
            string sem = Convert.ToString(ddlsem.SelectedValue).Trim();
            if (chkpassout.Checked == true)
            {
                int semval = ddlsem.Items.Count;
                semval++;
                sem = semval.ToString();
            }
            string exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            string strnoteligilbesubject = "select r.Roll_No,s.subject_no from Registration r,studentsemestersubjectdebar s where r.Roll_No=s.roll_no and r.cc=0 and r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "'";
            DataSet dsnoteligiblesubject = d2.select_method_wo_parameter(strnoteligilbesubject, "Text");
            string examyear = Convert.ToString(ddlYear.SelectedValue).Trim();
            collegecode = Convert.ToString(ddlcollege.SelectedValue).Trim();
            string appldate = Convert.ToString(txtappldate.Text).Trim();
            if (appldate.Trim() == "")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter The Applied Date";
                return;
            }
            string[] spd = appldate.Split('/');
            DateTime dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            string lastappladet = Convert.ToString(txtappllastdate.Text).Trim();
            if (lastappladet.Trim() == "")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter The Last Applied Date";
                return;
            }
            spd = lastappladet.Split('/');
            DateTime dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            if (dtappl > dtlastappl)
            {
                lblerror.Visible = true;
                lblerror.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            string qryExternalCheck = string.Empty;
            if (chkExternalMark.Checked)
            {
                //or m.total>=s.mintotal m.external_mark>=s.min_ext_marks
                qryExternalCheck = " and (m1.result='Pass' or m1.external_mark>=s1.min_ext_marks)";
            }
            else
            {
                qryExternalCheck = " and m1.result='Pass'";
            }
            string applfee = Convert.ToString(txtapplfee.Text).Trim();
            string smtfee = Convert.ToString(txtstmtfee.Text).Trim();
            string extrafee = Convert.ToString(txtextrafee.Text).Trim();
            string fineamount = Convert.ToString(txtfine.Text).Trim();
            string extrafee2 = Convert.ToString(txtextrafee2.Text).Trim();
            double appltotalcost = 0;
            double applcost = 0;
            if (applfee.Trim() != "")
            {
                //applcost = Convert.ToDouble(applfee);
                double.TryParse(applfee.Trim(), out applcost);
            }
            double stmtcost = 0;
            if (smtfee.Trim() != "")
            {
                //stmtcost = Convert.ToDouble(smtfee);
                double.TryParse(smtfee.Trim(), out stmtcost);
            }
            double extracost = 0;
            if (extrafee.Trim() != "")
            {
                //extracost = Convert.ToDouble(extrafee);
                double.TryParse(extrafee.Trim(), out extracost);
            }
            double extracost2 = 0;
            if (extrafee2.Trim() != "")
            {
                //extracost2 = Convert.ToDouble(extrafee2);
                double.TryParse(extrafee2.Trim(), out extracost2);
            }
            double fineamo = 0;
            if (fineamount.Trim() != "")
            {
                //fineamo = Convert.ToDouble(fineamount);
                double.TryParse(fineamount.Trim(), out fineamo);
            }
            appltotalcost = applcost + stmtcost + extracost + fineamo + extracost2;
            bool setflag = false;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    setflag = true;
                }
            }
            if (setflag == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Student And Then Proceed";
                return;
            }
            Dictionary<string, List<string>> dicAppNoRedoSemester = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dicRollNoRedoSemester = new Dictionary<string, List<string>>();
            getAllRedoStudentsSemester(out dicAppNoRedoSemester, out dicRollNoRedoSemester, batchYear: batchyear, degreeCode: degreecode);

            //string getexamcode = d2.GetFunction("select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and current_semester='" + sem + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");

            //if (getexamcode.Trim() == "" || getexamcode == null || getexamcode.Trim() == "0")
            //{
            //    string setdegreevalues = "if not exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and current_semester='" + sem + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
            //    setdegreevalues = setdegreevalues + " insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code)";
            //    setdegreevalues = setdegreevalues + " values ('" + degreecode + "','" + exammonth + "','" + examyear + "','" + batchyear + "','" + sem + "','" + collegecode + "')";
            //    insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
            //    getexamcode = d2.GetFunction("select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            //}
            string getexamcode = d2.GetFunction("select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            if (getexamcode == null || getexamcode.Trim() == "" || getexamcode.Trim() == "0")
            {
                //string setdegreevalues = "if not exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and current_semester='" + sem + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                //setdegreevalues += " insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code,isSupplementaryExam) values ('" + degreecode + "','" + exammonth + "','" + examyear + "','" + batchyear + "','" + sem + "','" + collegecode + "','" + chkIsSupplementaryExam.Checked + "')";
                //setdegreevalues += " else update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and current_semester='" + sem + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                //insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
                //getexamcode = d2.GetFunction("select exam_code from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and current_semester='" + sem + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
                string setdegreevalues = "if not exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                setdegreevalues += " insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code,isSupplementaryExam) values ('" + degreecode + "','" + exammonth + "','" + examyear + "','" + batchyear + "','" + sem + "','" + collegecode + "','" + chkIsSupplementaryExam.Checked + "')";
                setdegreevalues += " else update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "' where degree_code='" + degreecode + "' and batch_year='" + batchyear + "'  and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
                getexamcode = d2.GetFunction("select exam_code from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "'  and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            }
            else
            {
                string setdegreevalues = "if exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                setdegreevalues += " update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "' where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
            }
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                string rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Text).Trim();
                string appNo = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Note).Trim();
                string redoStudent = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Tag).Trim();
                string studentBatchYear = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Note).Trim();
                string studentDegreeCode = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Tag).Trim();
                string studentCurrentSem = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 4].Text).Trim();
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                bool isStudentRedo = false;
                bool.TryParse(redoStudent.Trim(), out isStudentRedo);
                if (stva == 1)
                {
                    string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                    string insupexamsubject = "delete ead from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no='" + rollno + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "'";
                    insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                    string strnotelisub = string.Empty;
                    dsnoteligiblesubject.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    DataView dvnoteliglebsubject = dsnoteligiblesubject.Tables[0].DefaultView;
                    for (int ne = 0; ne < dvnoteliglebsubject.Count; ne++)
                    {
                        if (strnotelisub.Trim() == "")
                        {
                            strnotelisub = Convert.ToString(dvnoteliglebsubject[ne]["subject_no"]).Trim();
                        }
                        else
                        {
                            strnotelisub = Convert.ToString(dvnoteliglebsubject[ne]["subject_no"]).Trim();
                        }
                    }
                    if (strnotelisub.Trim() != "")
                    {
                        strnotelisub = " and s.subject_no not in(" + strnotelisub + ")";
                    }
                    double exmfee = appltotalcost;
                    double totalpaperfee = 0;
                    string query = string.Empty;
                    string qryNotEligibleSemesters = string.Empty;
                    if (dicRollNoRedoSemester.ContainsKey(rollno.Trim()))
                    {
                        List<string> lstRedoSem = new List<string>();
                        lstRedoSem = dicRollNoRedoSemester[rollno.Trim()];
                        string semester = string.Empty;
                        semester = string.Join("','", lstRedoSem.ToArray());
                        if (isStudentRedo)
                        {
                            lstRedoSem.Remove(studentCurrentSem);
                            semester = string.Join("','", lstRedoSem.ToArray());
                        }
                        if (!string.IsNullOrEmpty(semester))
                        {
                            qryNotEligibleSemesters = " and sc.semester not in('" + semester + "')";
                        }
                    }
                    if (dicAppNoRedoSemester.ContainsKey(appNo.Trim()))
                    {
                        List<string> lstRedoSem = new List<string>();
                        lstRedoSem = dicAppNoRedoSemester[appNo.Trim()];
                        string semester = string.Empty;
                        semester = string.Join("','", lstRedoSem.ToArray());
                        if (isStudentRedo)
                        {
                            lstRedoSem.Remove(studentCurrentSem);
                            semester = string.Join("','", lstRedoSem.ToArray());
                        }
                        if (!string.IsNullOrEmpty(semester))
                        {
                            qryNotEligibleSemesters = " and sc.semester not in('" + semester + "')";
                        }
                    }


                    string valSemcur = string.Empty;
                    string valsemarr = string.Empty;
                    if (cblsem.Items.Count > 0)
                    {
                        for (int s = 0; s < cblsem.Items.Count; s++)
                        {
                            if (cblsem.Items[s].Selected)
                            {
                                int ss = Convert.ToInt32(cblsem.Items[s].Value);
                                if (ss > Convert.ToInt32(sem))
                                {
                                    if (string.IsNullOrEmpty(valSemcur))
                                        valSemcur = "'" + ss + "'";
                                    else
                                        valSemcur = valSemcur + "," + "'" + ss + "'";
                                }
                                //else if (ss < Convert.ToInt32(sem))
                                //{
                                if (string.IsNullOrEmpty(valsemarr))
                                    valsemarr = "'" + ss + "'";
                                else
                                    valsemarr = valsemarr + "," + "'" + ss + "'";
                                // }
                            }
                        }
                    }
                    DataTable dtfuturesub = new DataTable();
                    string futSubject = string.Empty;
                    if (chksemwise.Checked && !chkpassout.Checked)
                    {
                        string strfuturesub = "select subject_no from Futuresub_Exam_app where ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "' and Batch_year='" + batchyear + "' and DegreeCode='" + degreecode + "' and Semester='" + sem + "'";
                        dtfuturesub = dirAcc.selectDataTable(strfuturesub);
                        if (dtfuturesub.Rows.Count > 0)
                        {
                            foreach (DataRow dt1 in dtfuturesub.Rows)
                            {
                                string subjNo = Convert.ToString(dt1["subject_no"]);
                                if (string.IsNullOrEmpty(futSubject))
                                    futSubject = "'" + subjNo + "'";
                                else
                                    futSubject = futSubject + "," + "'" + subjNo + "'";
                            }
                        }
                    }
                    string Curfeetype = string.Empty;
                    string Arrfeetype = string.Empty;
                    string subFeetype = da.GetFunction("select value from Master_Settings where settings='Fee obtained from Subject Type'");
                    if (subFeetype == "1")
                    {
                        Curfeetype = ",ss.fee_per_paper as feesval ";
                        Arrfeetype = ",ss.arr_fee as feesval ";
                    }
                    else
                    {
                        Curfeetype = ",s.curfee as feesval ";
                        Arrfeetype = ",s.arrfee as feesval ";
                    }

                    if (chksemwise.Checked && !chkpassout.Checked)
                    {
                        query = "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst " + Curfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                        query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                        query = query + " and r.degree_code=sy.degree_code and ss.promote_count=1 and sy.semester='" + sem + "' and sc.roll_no='" + rollno + "' " + qryNotEligibleSemesters;
                        if (!string.IsNullOrEmpty(futSubject))
                        {
                            query = query + " union ";
                            query = query + "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst " + Curfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r";
                            query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                            query = query + " and r.degree_code=sy.degree_code and ss.promote_count=1 and sy.semester in(" + valSemcur + ") and sc.roll_no='" + rollno + "' and s.subject_no in(" + futSubject + ") " + qryNotEligibleSemesters;
                        }
                        query = query + " union ";
                        query = query + " select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst" + Arrfeetype + " from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                        query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollno + "' and sc.semester in(" + valsemarr + ") and sc.semester not in('" + sem + "')  " + qryNotEligibleSemesters;
                        query = query + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollno + "' " + qryExternalCheck + ")  ";
                        query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";
                    }
                    else if (isStudentRedo)
                    {
                        query = "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst " + Curfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r,StudentRedoDetails sr ";
                        query = query + " where sr.Stud_AppNo=r.App_No  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and sr.BatchYear=sy.Batch_Year ";
                        query = query + " and sr.DegreeCode=sy.degree_code and ss.promote_count=1 and sy.semester='" + sem + "' and sc.roll_no='" + rollno + "' " + strnotelisub + " " + qryNotEligibleSemesters;
                        query = query + " union ";
                        query = query + " select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst " + Arrfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                        query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollno + "' " + qryNotEligibleSemesters;//and sc.semester<" + sem + "
                        //query = query + " and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='" + rollno + "' " + qryExternalCheck + ") " + strnotelisub + "";
                        query = query + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollno + "' " + qryExternalCheck + ")" + strnotelisub + "";
                        query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";

                        //select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and sr.BatchYear=sy.Batch_Year and sr.DegreeCode=sy.degree_code and ss.promote_count=1 and sy.semester='7' and sc.roll_no='13PPH001' " + strnotelisub + " " + qryNotEligibleSemesters

                        //union select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst,s.arrfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='13PPH001' and sc.semester<7 and sc.semester not in(3) " + sem + " " + qryNotEligibleSemesters and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='' and result='pass')  order by sy.semester desc,s.subject_code,ss.subject_type
                    }
                    else
                    {
                        query = "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst " + Curfeetype + "   from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                        query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                        query = query + " and r.degree_code=sy.degree_code and ss.promote_count=1 and sy.semester='" + sem + "' and sc.roll_no='" + rollno + "' " + strnotelisub + " " + qryNotEligibleSemesters;
                        query = query + " union ";
                        query = query + " select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst " + Arrfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                        query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollno + "' and sc.semester<" + sem + " " + qryNotEligibleSemesters;
                        //and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='0114208041'  and m1.result='Pass')
                        query = query + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollno + "' " + qryExternalCheck + ") " + strnotelisub + "";
                        query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";
                    }
                    // and (m.result='Pass' or m.external_mark>=s.min_ext_marks)
                    DataSet dssubappl = da.select_method_wo_parameter(query, "Text");
                    if (dssubappl.Tables.Count > 0 && dssubappl.Tables[0].Rows.Count > 0)
                    {
                        if (applno.Trim() == "" || applno.Trim() == "0")
                        {
                            string setexamapplication = "if exists(select * from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "')";
                            setexamapplication = setexamapplication + " update exam_application set applied_date='" + dtappl.ToString("MM/dd/yyyy") + "',total_fee='0',extra_fee='" + extracost.ToString() + "',fine='" + fineamo.ToString() + "',cost_appl='" + applcost.ToString() + "',cost_mark='" + stmtcost.ToString() + "',lastdate='" + dtlastappl.ToString("MM/dd/yyyy") + "',Exam_type='0',fee_amount='0',extra_fee2='" + extracost2.ToString() + "'";
                            setexamapplication = setexamapplication + " where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                            setexamapplication = setexamapplication + " else ";
                            setexamapplication = setexamapplication + " insert into exam_application(roll_no,applied_date,total_fee,exam_code,extra_fee,fine,cost_appl,cost_mark,lastdate,Exam_type,fee_amount,extra_fee2)";
                            setexamapplication = setexamapplication + " values('" + rollno + "','" + dtappl.ToString("MM/dd/yyyy") + "','0','" + getexamcode + "','" + extracost.ToString() + "','" + fineamo.ToString() + "','" + applcost + "','" + stmtcost + "','" + dtlastappl.ToString("MM/dd/yyyy") + "','0','0','" + extracost2.ToString() + "')";
                            insupdateval = d2.update_method_wo_parameter(setexamapplication, "text");
                        }
                        applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                        for (int s = 0; s < dssubappl.Tables[0].Rows.Count; s++)
                        {
                            string subject = dssubappl.Tables[0].Rows[s]["subject_no"].ToString();
                            int subsem = Convert.ToInt32(dssubappl.Tables[0].Rows[s]["semester"].ToString());
                            string fees = dssubappl.Tables[0].Rows[s]["feesval"].ToString();
                            if (fees.Trim() != "")
                            {
                                exmfee = exmfee + Convert.ToDouble(fees);
                                totalpaperfee = totalpaperfee + Convert.ToDouble(fees);
                            }
                            string attempts = "0";
                            string types = string.Empty;
                            int attval = Convert.ToInt32(sem) - Convert.ToInt32(subsem);
                            if (attval > 0)
                            {
                                attempts = attval.ToString();
                                types = "*";
                            }
                            insupexamsubject = "if exists(select * from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subject + "')";
                            insupexamsubject = insupexamsubject + "  update exam_appl_details set attempts='" + attempts + "',fee='" + fees + "',type='" + types + "',attend='1' where appl_no='" + applno + "' and subject_no='" + subject + "'";
                            insupexamsubject = insupexamsubject + " else insert into exam_appl_details(subject_no,attempts,fee,type,appl_no,attend) values('" + subject + "','" + attempts + "','" + fees + "','" + types + "','" + applno + "','1')";
                            insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                        }
                        string updateexamapplication = "update exam_application set total_fee='" + exmfee + "',fee_amount='" + totalpaperfee + "' where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                        insupdateval = d2.update_method_wo_parameter(updateexamapplication, "text");

                        ////divViewSubjects.Visible = false;
                        //lblerror.Visible = true;
                        //lblerror.Text = "Selected Student Cannot Apply for Examination";
                        //return;
                    }
                    else
                    {
                        //divViewSubjects.Visible = false;
                        //lblerror.Visible = true;
                        //lblerror.Text = "Selected Student Cannot Apply for Examination";
                        //return;
                    }
                }
                else
                {
                    //string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                    //string insupexamsubject = "delete from exam_appl_details where appl_no='" + applno + "'";
                    //insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                    //insupexamsubject = "delete from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                    //insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                }
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Sucessfully.')", true);
            loadexamdetails();
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    private void getFinanceAppliedAmount()
    {
        try
        {
            ArrayList arrMandFees = MandatoryFees();
            FpSpread1.SaveChanges();
            double st_mark = 0;
            double costappl = 0;
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            string appnofin = string.Empty;
            string currentsem = string.Empty;
            int insupdateval = 0;
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string sem = ddlsem.SelectedValue.ToString();
            //magesh 9/2/18
            string rrollno = string.Empty;
            if (chkpassout.Checked == true)
            {
                int semval = ddlsem.Items.Count;
                semval++;
                sem = semval.ToString();
            }
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            string appldate = txtappldate.Text.ToString();
            if (appldate.Trim() == "")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter The Applied Date";
                return;
            }
            string[] spd = appldate.Split('/');
            DateTime dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            string lastappladet = txtappllastdate.Text.ToString();
            if (lastappladet.Trim() == "")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter The Last Applied Date";
                return;
            }
            spd = lastappladet.Split('/');
            DateTime dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            if (dtappl > dtlastappl)
            {
                lblerror.Visible = true;
                lblerror.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            string qryExternalCheck = string.Empty;
            if (chkExternalMark.Checked)
            {
                //m.external_mark>=s.min_ext_marks
                qryExternalCheck = " and (m1.result='Pass' or m1.external_mark>=s1.min_ext_marks)";
            }
            else
            {
                qryExternalCheck = " and m1.result='Pass'";
            }
            string applfee = txtapplfee.Text.ToString();
            string smtfee = txtstmtfee.Text.ToString();
            string extrafee = txtextrafee.Text.ToString();
            string fineamount = txtfine.Text.ToString();
            string extrafee2 = txtextrafee2.Text.ToString();
            double appltotalcost = 0;
            double applcost = 0;
            if (applfee.Trim() != "")
            {
                applcost = Convert.ToDouble(applfee);
            }
            double stmtcost = 0;
            if (smtfee.Trim() != "")
            {
                stmtcost = Convert.ToDouble(smtfee);
            }
            double extracost = 0;
            if (extrafee.Trim() != "")
            {
                extracost = Convert.ToDouble(extrafee);
            }
            double extracost2 = 0;
            if (extrafee2.Trim() != "")
            {
                extracost2 = Convert.ToDouble(extrafee2);
            }
            double fineamo = 0;
            if (fineamount.Trim() != "")
            {
                fineamo = Convert.ToDouble(fineamount);
            }
            appltotalcost = applcost + stmtcost + extracost + fineamo + extracost2;
            Boolean setflag = false;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    setflag = true;
                }
            }
            if (setflag == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Student And Then Proceed";
                return;
            }
            Dictionary<string, List<string>> dicAppNoRedoSemester = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dicRollNoRedoSemester = new Dictionary<string, List<string>>();
            getAllRedoStudentsSemester(out dicAppNoRedoSemester, out dicRollNoRedoSemester, batchYear: batchyear, degreeCode: degreecode);
            string getexamcode = d2.GetFunction("select exam_code from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            if (getexamcode == null || getexamcode.Trim() == "" || getexamcode.Trim() == "0")
            {
                string setdegreevalues = "if not exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                setdegreevalues += " insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code,isSupplementaryExam) values ('" + degreecode + "','" + exammonth + "','" + examyear + "','" + batchyear + "','" + sem + "','" + collegecode + "','" + chkIsSupplementaryExam.Checked + "')";
                setdegreevalues += " else update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "'  and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
                getexamcode = d2.GetFunction("select exam_code from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            }
            else
            {
                string setdegreevalues = "if exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                setdegreevalues += " update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
            }
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                string appNo = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Note).Trim();
                string redoStudent = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Tag).Trim();
                string studentBatchYear = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Note).Trim();
                string studentDegreeCode = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Tag).Trim();
                string studentCurrentSem = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 4].Text).Trim();
                bool isStudentRedo = false;
                bool.TryParse(redoStudent.Trim(), out isStudentRedo);
                if (stva == 1)
                {
                    //magesh 9/2/18
                    if (FpSpread1.Sheets[0].Cells[r, 5].Text == "Applied")
                    {
                        rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                        if (rrollno == "")
                        {
                            rrollno = "" + rollno + "";
                        }
                        else
                        {
                            rrollno = rrollno + "," + rollno + "";
                        }
                    }
                    else
                    {
                        double exmfee = appltotalcost;
                        double totalpaperfee = 0;
                        string query = string.Empty;
                        string qryNotEligibleSemesters = string.Empty;
                        if (dicRollNoRedoSemester.ContainsKey(rollno.Trim()))
                        {
                            List<string> lstRedoSem = new List<string>();
                            lstRedoSem = dicRollNoRedoSemester[rollno.Trim()];
                            string semester = string.Empty;
                            semester = string.Join("','", lstRedoSem.ToArray());
                            if (isStudentRedo)
                            {
                                lstRedoSem.Remove(studentCurrentSem);
                                semester = string.Join("','", lstRedoSem.ToArray());
                            }
                            if (!string.IsNullOrEmpty(semester))
                            {
                                qryNotEligibleSemesters = " and sc.semester not in('" + semester + "')";
                            }
                        }
                        if (dicAppNoRedoSemester.ContainsKey(appNo.Trim()))
                        {
                            List<string> lstRedoSem = new List<string>();
                            lstRedoSem = dicAppNoRedoSemester[appNo.Trim()];
                            string semester = string.Empty;
                            semester = string.Join("','", lstRedoSem.ToArray());
                            if (isStudentRedo)
                            {
                                lstRedoSem.Remove(studentCurrentSem);
                                semester = string.Join("','", lstRedoSem.ToArray());
                            }
                            if (!string.IsNullOrEmpty(semester))
                            {
                                qryNotEligibleSemesters = " and sc.semester not in('" + semester + "')";
                            }
                        }

                        //string query = "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                        //query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                        //query = query + " and r.degree_code=sy.degree_code and sy.semester='" + sem + "' and sc.roll_no='" + rollno + "' " + qryNotEligibleSemesters;
                        //query = query + " union ";
                        //query = query + " select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst,s.arrfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                        //query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollno + "' and sc.semester<" + sem + " " + qryNotEligibleSemesters;
                        //query = query + " and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='" + rollno + "' " + qryExternalCheck + " )";
                        //query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";
                        //and (m.result='Pass' or or m.external_mark>=s.min_ext_marks)
                        //fastrack Exam application
                        string valSemcur = string.Empty;
                        string valsemarr = string.Empty;
                        if (cblsem.Items.Count > 0)
                        {
                            for (int s = 0; s < cblsem.Items.Count; s++)
                            {
                                if (cblsem.Items[s].Selected)
                                {
                                    int ss = Convert.ToInt32(cblsem.Items[s].Value);
                                    if (ss > Convert.ToInt32(sem))
                                    {
                                        if (string.IsNullOrEmpty(valSemcur))
                                            valSemcur = "'" + ss + "'";
                                        else
                                            valSemcur = valSemcur + "," + "'" + ss + "'";
                                    }
                                    else if (ss < Convert.ToInt32(sem))
                                    {
                                        if (string.IsNullOrEmpty(valsemarr))
                                            valsemarr = "'" + ss + "'";
                                        else
                                            valsemarr = valsemarr + "," + "'" + ss + "'";
                                    }
                                }
                            }
                        }
                        DataTable dtfuturesub = new DataTable();
                        string futSubject = string.Empty;
                        if (chksemwise.Checked && !chkpassout.Checked)
                        {
                            string strfuturesub = "select subject_no from Futuresub_Exam_app where ExamMonth='" + exammonth + "' and ExamYear='" + examyear + "' and Batch_year='" + batchyear + "' and DegreeCode='" + degreecode + "' and Semester='" + sem + "'";
                            dtfuturesub = dirAcc.selectDataTable(strfuturesub);
                            if (dtfuturesub.Rows.Count > 0)
                            {
                                foreach (DataRow dt1 in dtfuturesub.Rows)
                                {
                                    string subjNo = Convert.ToString(dt1["subject_no"]);
                                    if (string.IsNullOrEmpty(futSubject))
                                        futSubject = "'" + subjNo + "'";
                                    else
                                        futSubject = futSubject + "," + "'" + subjNo + "'";
                                }
                            }
                        }

                        string Curfeetype = string.Empty;
                        string Arrfeetype = string.Empty;
                        string subFeetype = da.GetFunction("select value from Master_Settings where settings='Fee obtained from Subject Type'");
                        if (subFeetype == "1")
                        {
                            Curfeetype = ",ss.fee_per_paper as feesval ";
                            Arrfeetype = ",ss.arr_fee as feesval ";
                        }
                        else
                        {
                            Curfeetype = ",s.curfee as feesval ";
                            Arrfeetype = ",s.arrfee as feesval ";
                        }

                        if (chksemwise.Checked && !chkpassout.Checked)
                        {
                            query = "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst " + Curfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                            query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                            query = query + " and r.degree_code=sy.degree_code and ss.promote_count=1 and sy.semester='" + sem + "' and sc.roll_no='" + rollno + "' " + qryNotEligibleSemesters;
                            if (!string.IsNullOrEmpty(futSubject))
                            {
                                query = query + " union ";
                                query = query + "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst " + Curfeetype + " from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r";
                                query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                                query = query + " and r.degree_code=sy.degree_code and ss.promote_count=1 and sy.semester in(" + valSemcur + ") and sc.roll_no='" + rollno + "' and s.subject_no in(" + futSubject + ") " + qryNotEligibleSemesters;
                            }

                            query = query + " union ";
                            query = query + " select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst " + Arrfeetype + " from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                            query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollno + "' and sc.semester in(" + valsemarr + ") and sc.semester not in('" + sem + "')  " + qryNotEligibleSemesters;
                            query = query + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollno + "' " + qryExternalCheck + ")  ";
                            query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";
                        }
                        else if (isStudentRedo)
                        {
                            query = "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst " + Curfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r,StudentRedoDetails sr ";
                            query = query + " where sr.Stud_AppNo=r.App_No  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and sr.BatchYear=sy.Batch_Year ";
                            query = query + " and sr.DegreeCode=sy.degree_code and ss.promote_count=1 and sy.semester='" + sem + "' and sc.roll_no='" + rollno + "' " + qryNotEligibleSemesters;
                            query = query + " union ";
                            query = query + " select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst " + Arrfeetype + "   from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                            query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollno + "' " + qryNotEligibleSemesters;//and sc.semester<" + sem + "
                            query = query + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollno + "' " + qryExternalCheck + ")  ";
                            query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";

                            //select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and sr.BatchYear=sy.Batch_Year and sr.DegreeCode=sy.degree_code and ss.promote_count=1 and sy.semester='7' and sc.roll_no='13PPH001' " + strnotelisub + " " + qryNotEligibleSemesters

                            //union select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst,s.arrfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='13PPH001' and sc.semester<7 and sc.semester not in(3) " + sem + " " + qryNotEligibleSemesters and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='' and result='pass')  order by sy.semester desc,s.subject_code,ss.subject_type
                        }
                        else
                        {
                            query = "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst " + Curfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                            query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                            query = query + " and r.degree_code=sy.degree_code and ss.promote_count=1 and sy.semester='" + sem + "' and sc.roll_no='" + rollno + "' " + qryNotEligibleSemesters;
                            query = query + " union ";
                            query = query + " select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst " + Arrfeetype + "  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                            query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollno + "' and sc.semester<" + sem + " " + qryNotEligibleSemesters;
                            query = query + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollno + "' " + qryExternalCheck + ")  ";
                            query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";
                        }
                        DataSet dssubappl = da.select_method_wo_parameter(query, "Text");
                        if (dssubappl.Tables.Count > 0 && dssubappl.Tables[0].Rows.Count > 0)
                        {
                            string setexamapplication = "if exists(select * from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "')";
                            setexamapplication = setexamapplication + " update exam_application set applied_date='" + dtappl.ToString("MM/dd/yyyy") + "',total_fee='0',extra_fee='" + extracost.ToString() + "',fine='" + fineamo.ToString() + "',cost_appl='" + applcost.ToString() + "',cost_mark='" + stmtcost.ToString() + "',lastdate='" + dtlastappl.ToString("MM/dd/yyyy") + "',Exam_type='0',fee_amount='0',extra_fee2='" + extracost2.ToString() + "'";
                            setexamapplication = setexamapplication + " where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                            setexamapplication = setexamapplication + " else ";
                            setexamapplication = setexamapplication + " insert into exam_application(roll_no,applied_date,total_fee,exam_code,extra_fee,fine,cost_appl,cost_mark,lastdate,Exam_type,fee_amount,extra_fee2)";
                            setexamapplication = setexamapplication + " values('" + rollno + "','" + dtappl.ToString("MM/dd/yyyy") + "','0','" + getexamcode + "','" + extracost.ToString() + "','" + fineamo.ToString() + "','" + applcost + "','" + stmtcost + "','" + dtlastappl.ToString("MM/dd/yyyy") + "','0','0','" + extracost2.ToString() + "')";
                            insupdateval = d2.update_method_wo_parameter(setexamapplication, "text");
                            appnofin = da.GetFunctionv("select app_no from registration where roll_no='" + rollno + "'");
                            string curSem = da.GetFunctionv("select current_semester from registration where roll_no='" + rollno + "'");

                            #region Hidden/Commented

                            //if (chkinclu_fin.Checked == true)
                            //{
                            //    string currentfinyear = da.getCurrentFinanceYear(usercode, collegecode); 
                            //    for (int i = 2; i < cbl_header.Items.Count; i++)
                            //    {
                            //        if (cbl_header.Items[i].Selected == true)
                            //        {
                            //            string edulevel = da.GetFunctionv("	select Edu_Level from Course where Course_Id in (select Course_Id from Degree where Degree_Code='" + ddlbranch.SelectedItem.Value.ToString() + "')");
                            //            string ledgercode = da.GetFunctionv("select Ledger from coe_feemaster where settings='" + cbl_header.Items[i].Value.ToString() + "' and edulevel='" + edulevel + "'");
                            //            string headecode = da.GetFunctionv("select HeaderFK from FM_LedgerMaster where LedgerPK='" + ledgercode + "'");
                            //            string finfkyear = d2.getCurrentFinanceYear(usercode, Session["collegecode"].ToString()); 
                            //            string feecat = da.GetFunctionv("select LinkValue from New_InsSettings where LinkName='Fee Yearwise' and user_code='" + Session["usercode"].ToString() + "' and college_code='" + Session["collegecode"].ToString() + "'");
                            //            string Amount = da.GetFunctionv("select value from coe_feemaster where settings='" + cbl_header.Items[i].Value.ToString() + "' and edulevel='" + edulevel + "'");
                            //            if (Amount.Trim() != "")
                            //            {
                            //                Amount = " '" + Amount + "'";
                            //            }
                            //            else
                            //            {
                            //                Amount = "null";
                            //            }
                            //            string addtype =string.Empty;
                            //            if (feecat == "0")
                            //            {
                            //                addtype = " Semester";
                            //                currentsem = currentsem + addtype;
                            //            }
                            //            else
                            //            {
                            //                addtype = " Year";
                            //                if (currentsem.Trim() == "1" || currentsem.Trim() == "2")
                            //                {
                            //                    currentsem = "1" + addtype;
                            //                }
                            //                else if (currentsem.Trim() == "3" || currentsem.Trim() == "4")
                            //                {
                            //                    currentsem = "2" + addtype;
                            //                }
                            //                else if (currentsem.Trim() == "5" || currentsem.Trim() == "6")
                            //                {
                            //                    currentsem = "3" + addtype;
                            //                }
                            //                else if (currentsem.Trim() == "7" || currentsem.Trim() == "8")
                            //                {
                            //                    currentsem = "4" + addtype;
                            //                }
                            //            }
                            //            currentsem = da.GetFunctionv("select TextCode from textvaltable where TextCriteria='Feeca' and college_code='" + Session["collegecode"].ToString() + "' and TextVal='" + currentsem + "'");
                            //            setexamapplication = "if exists ( select * from FT_FeeAllot where App_No ='" + appnofin + "' and FeeCategory ='" + currentsem + "' and LedgerFK ='" + ledgercode + "' and HeaderFK ='" + headecode + "')";
                            //            setexamapplication = setexamapplication + " update FT_FeeAllot set FeeAmount =FeeAmount+" + Amount + ",TotalAmount =TotalAmount+" + Amount + ",BalAmount=BalAmount+" + Amount + ",FinYearFK='" + finfkyear + "'  where App_No ='" + appnofin + "' and FeeCategory ='" + currentsem + "' and LedgerFK ='" + ledgercode + "' and HeaderFK ='" + headecode + "' else ";
                            //            setexamapplication = setexamapplication + " insert into FT_FeeAllot (App_No,FeeCategory,LedgerFK,HeaderFK,FeeAmount,TotalAmount,PaidAmount,BalAmount,FinYearFK) values ('" + appnofin + "','" + currentsem + "','" + ledgercode + "','" + headecode + "'," + Amount + "," + Amount + ",'0'," + Amount + ",'" + finfkyear + "')";
                            //            insupdateval = d2.update_method_wo_parameter(setexamapplication, "text");
                            //        }
                            //    }
                            //} 

                            #endregion

                            int currentSemester = 0;
                            int.TryParse(curSem, out currentSemester);
                            string maxDuration = d2.GetFunction("select distinct NDurations from ndegree where Degree_code='" + degreecode + " and batch_year='" + batchyear + "");
                            int maxDurations = 0;
                            if (maxDuration.Trim() == "" || maxDuration == null || maxDuration == "0")
                            {
                                maxDuration = d2.GetFunction("select distinct duration,first_year_nonsemester  from degree where degree_code='" + degreecode + "'");
                            }
                            int.TryParse(maxDuration, out maxDurations);

                            Dictionary<string, string[]> dicManFeesAmt = new Dictionary<string, string[]>();
                            MandatoryFeesValues(out dicManFeesAmt);

                            string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                            string currentfinyear = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);
                            string finfkyear = currentfinyear;
                            string colllnmae = string.Empty;
                            string feecat = da.GetFunctionv("select LinkValue from New_InsSettings where LinkName='Fee Yearwise' and user_code='" + Session["usercode"].ToString() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                            //magesh 9/2/18
                            //DataSet settingValue = d2.select_method_wo_parameter(" select settings,value from Master_Settings where settings='Arrear Theory' --and usercode='" + Session["usercode"].ToString() + "'", "Text");
                            string feeValue = string.Empty;
                            string headerCode = string.Empty;
                            string ledgerCode = string.Empty;

                            string addtype = string.Empty;
                            string feeCatagory = string.Empty;
                            string semPO = string.Empty;
                            string semPOcat = string.Empty;
                            if (feecat == "0")
                            {
                                addtype = " Semester";
                                feeCatagory = curSem + addtype;
                                semPO = curSem + addtype;
                            }
                            else
                            {
                                addtype = " Year";
                                if (curSem.Trim() == "1" || curSem.Trim() == "2")
                                {
                                    feeCatagory = "1" + addtype;
                                    semPO = "1" + addtype;
                                }
                                else if (curSem.Trim() == "3" || curSem.Trim() == "4")
                                {
                                    feeCatagory = "2" + addtype;
                                    semPO = "2" + addtype;

                                }
                                else if (curSem.Trim() == "5" || curSem.Trim() == "6")
                                {
                                    feeCatagory = "3" + addtype;
                                    semPO = "3" + addtype;
                                }
                                else if (curSem.Trim() == "7" || curSem.Trim() == "8")
                                {
                                    feeCatagory = "4" + addtype;
                                    semPO = "4" + addtype;
                                }
                                else if (curSem.Trim() == "9" || curSem.Trim() == "10")
                                {
                                    feeCatagory = "5" + addtype;
                                    semPO = "5" + addtype;
                                }
                            }
                            bool isPassedOut = false;
                            feeCatagory = da.GetFunctionv("select TextCode from textvaltable where TextCriteria='Feeca' and college_code='" + ddlcollege.SelectedValue + "' and TextVal='" + feeCatagory + "'");
                            double applFees = 0;
                            double totfee = 0;
                            if (dicManFeesAmt.Count > 0)
                            {
                                if (currentSemester == maxDurations + 1)
                                {
                                    isPassedOut = true;
                                }

                                foreach (KeyValuePair<string, string[]> dicMandFees in dicManFeesAmt)
                                {
                                    double costFee = 0;
                                    applFees = 0;
                                    string[] valuePair = new string[3];
                                    string key = dicMandFees.Key.Trim().ToLower();
                                    valuePair = dicMandFees.Value;
                                    string headerFK = (valuePair.Length >= 1) ? valuePair[0] : "";
                                    string ledgerFK = (valuePair.Length >= 2) ? valuePair[1] : "";
                                    string feeAmt = (valuePair.Length >= 3) ? valuePair[2] : "";
                                    double.TryParse(feeAmt.Trim(), out costFee);
                                    if (feeAmt.Trim() != "")
                                    {
                                        feeAmt = " '" + feeAmt + "'";

                                    }
                                    else
                                    {
                                        feeAmt = "null";
                                    }
                                    switch (key)
                                    {
                                        case "application form":
                                            applFees = costFee;
                                            costappl = costFee;
                                            break;
                                        case "semester mark sheet":
                                            applFees = costFee;// for Adhiyaman magesh
                                            st_mark = costFee;// for Adhiyaman
                                            break;
                                        case "theory":
                                            break;
                                        case "practical":
                                            break;
                                        case "project":
                                            break;
                                        case "field work":
                                            break;
                                        case "viva voice":
                                            break;
                                        case "disseration":
                                            break;
                                        case "consolidate mark sheet":
                                            break;
                                        case "course completaion":
                                            break;
                                        case "online application fee":
                                            break;
                                        case "arrear theory":
                                            break;
                                        case "arrear practical":
                                            break;
                                        case "central valuation":
                                            break;
                                        case "syllabi & curricular":
                                            break;
                                        default:
                                            break;
                                    }

                                    if (!string.IsNullOrEmpty(key) && valuePair != null && !string.IsNullOrEmpty(applno) && !string.IsNullOrEmpty(headerFK) && !string.IsNullOrEmpty(ledgerFK) && !string.IsNullOrEmpty(feeCatagory) && !string.IsNullOrEmpty(finfkyear) && ledgerFK.ToLower().Trim() != "0" && headerFK.ToLower().Trim() != "0")
                                    {
                                        string insertQuery = " INSERT INTO FT_FeeAllot(AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK,FromGovtAmt, DeductReason) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1,1," + appnofin + "," + ledgerFK + "," + headerFK + "," + feeAmt + "," + feeAmt + "," + feeCatagory + "," + feeAmt + "," + finfkyear + ",0,0) ";

                                        string selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + headerFK + "') and FeeCategory in('" + feeCatagory + "') and FinYearFK='" + finfkyear + "' and App_No in('" + appnofin + "') ";
                                        string updateQuery = string.Empty;

                                        if (!isPassedOut)
                                        {
                                            updateQuery = "  update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=1,FeeAmount=" + feeAmt + ",BalAmount=" + feeAmt + ",TotalAmount=" + feeAmt + " where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + headerFK + "') and FeeCategory in('" + feeCatagory + "') and  FinYearFK='" + finfkyear + "' and App_No in('" + appnofin + "') ";
                                        }
                                        else
                                        {
                                            if (isPassedOut)
                                            {
                                                semPOcat = da.GetFunctionv("select TextCode from textvaltable where TextCriteria='Feeca' and college_code='" + ddlcollege.SelectedValue + "' and TextVal='" + semPO + "'");
                                                updateQuery = "update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=1,FeeAmount =FeeAmount+" + feeAmt + ",TotalAmount =TotalAmount+" + feeAmt + ",BalAmount=BalAmount+" + feeAmt + "  where App_No ='" + appnofin + "' and FeeCategory ='" + semPOcat + "' and LedgerFK ='" + ledgerFK + "' and HeaderFK ='" + headerFK + "'";
                                            }
                                        }

                                        string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                                        int insok = d2.update_method_wo_parameter(finalQuery, "Text");

                                        if (insok > 0)
                                        {
                                            double appfee = applFees;
                                            totfee += appfee;
                                            string upquery = " update exam_application set total_fee='" + totfee.ToString() + "' , cost_appl='" + costappl.ToString() + "' ,  cost_mark='" + st_mark.ToString() + "' where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'"; // for Adhiyaman add cost_mark='" + st_mark.ToString() + "' magesh
                                            int ect = d2.update_method_wo_parameter(upquery, "Text");
                                        }
                                    }
                                }
                                //}
                            }
                            ArrayList arrHeaderFk = new ArrayList();
                            ArrayList arrLedgerFk = new ArrayList();
                            ArrayList arrfeeValue = new ArrayList();
                            MandatoryFees(out arrHeaderFk, out arrLedgerFk, out arrfeeValue);
                            string FeeAmt = string.Empty;
                            int arrcon = arrfeeValue.Count;
                            for (int i = 0; i < arrcon; i++)
                            {
                                DataSet settingValue = d2.select_method_wo_parameter(" select settings,value from Master_Settings where settings='" + arrfeeValue[i].ToString() + "' --and usercode='" + Session["usercode"].ToString() + "'", "Text");

                                if (settingValue.Tables.Count > 0 && settingValue.Tables[0].Rows.Count > 0)
                                {
                                    try
                                    {
                                        exmfee = 0.0;
                                        totalpaperfee = 0.0;
                                        feeValue = Convert.ToString(settingValue.Tables[0].Rows[0][0]).Trim();
                                        headerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[0];
                                        ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1];
                                        string[] mafess = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';');
                                        int sefee = mafess.Count();
                                        if (sefee == 3)
                                        {
                                            FeeAmt = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[2];
                                        }
                                        else
                                        {
                                            FeeAmt = "";
                                        }
                                    }
                                    catch { }
                                }

                                // if (feeValue != string.Empty && headerCode != string.Empty && ledgerCode != string.Empty)
                                if (feeValue != string.Empty && headerCode != string.Empty && ledgerCode != string.Empty && FeeAmt == "")
                                {
                                    int tblCount = dssubappl.Tables[0].Rows.Count;
                                    //if (tblCount > 18)
                                    //{
                                    //    tblCount = 18;
                                    //}
                                    exmfee += totfee;
                                    //magesh 9/2/18
                                    ledgerCode = string.Empty;
                                    for (int s = 0; s < dssubappl.Tables[0].Rows.Count; s++)
                                    {
                                        string subject = dssubappl.Tables[0].Rows[s]["subject_no"].ToString();
                                        int subsem = Convert.ToInt32(dssubappl.Tables[0].Rows[s]["semester"].ToString());
                                        string fees = dssubappl.Tables[0].Rows[s]["feesval"].ToString();
                                        if (fees.Trim() != "")
                                        {
                                            exmfee = exmfee + Convert.ToDouble(fees);
                                            totalpaperfee = totalpaperfee + Convert.ToDouble(fees);
                                        }
                                        string attempts = "0";
                                        string types = string.Empty;
                                        ledgerCode = string.Empty;
                                        int attval = Convert.ToInt32(sem) - Convert.ToInt32(subsem);
                                        if (attval > 0)
                                        {
                                            attempts = attval.ToString();
                                            types = "*";
                                        }

                                        string insupexamsubject = "if exists(select * from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subject + "')";
                                        insupexamsubject = insupexamsubject + "  update exam_appl_details set attempts='" + attempts + "',fee='" + fees + "',type='" + types + "',attend='1' where appl_no='" + applno + "' and subject_no='" + subject + "'";
                                        insupexamsubject = insupexamsubject + " else insert into exam_appl_details(subject_no,attempts,fee,type,appl_no,attend) values('" + subject + "','" + attempts + "','" + fees + "','" + types + "','" + applno + "','1')";
                                        insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                                        string isArrear = string.Empty;
                                        isArrear = Convert.ToString(dssubappl.Tables[0].Rows[s]["arrst"]).Trim().ToUpper();
                                        if (isArrear == "N" || isArrear == "Y")
                                        {
                                            #region Finance Include

                                            string Amount = Convert.ToString(dssubappl.Tables[0].Rows[s]["feesval"]).Trim();
                                            //string Amount = da.GetFunctionv("select " + colllnmae + " from subject  where subject_no='" + subject + "'");
                                            if (Amount.Trim() != "")
                                            {
                                                Amount = " " + Amount + "";
                                            }
                                            else
                                            {
                                                Amount = "0";
                                            }
                                            addtype = string.Empty;
                                            if (feecat == "0")
                                            {
                                                addtype = " Semester";
                                                currentsem = curSem + addtype;
                                            }
                                            else
                                            {
                                                addtype = " Year";
                                                if (curSem.Trim() == "1" || curSem.Trim() == "2")
                                                {
                                                    currentsem = "1" + addtype;
                                                }
                                                else if (curSem.Trim() == "3" || curSem.Trim() == "4")
                                                {
                                                    currentsem = "2" + addtype;
                                                }
                                                else if (curSem.Trim() == "5" || curSem.Trim() == "6")
                                                {
                                                    currentsem = "3" + addtype;
                                                }
                                                else if (curSem.Trim() == "7" || curSem.Trim() == "8")
                                                {
                                                    currentsem = "4" + addtype;
                                                }
                                                else if (curSem.Trim() == "9" || curSem.Trim() == "10")
                                                {
                                                    currentsem = "5" + addtype;
                                                }
                                            }
                                            string qryPassedOutFinYear = string.Empty;
                                            string ExamFees = string.Empty;
                                            DataTable dtExamFee = new DataTable();
                                            if (isArrear == "Y")
                                            {
                                                if (arrfeeValue[i].ToString() == "Arrear Theory" || arrfeeValue[i] == "Arrear Practical")
                                                {
                                                    //magesh 12.3.18
                                                    //  String ArrerFee = "Select LedgerPK from FM_LedgerMaster where LedgerName='Exam Fee Arrear'";
                                                    //dtExamFee = dirAcc.selectDataTable(ArrerFee);
                                                    //if (dtExamFee.Rows.Count > 0)
                                                    //{
                                                    //    ledgerCode = Convert.ToString(dtExamFee.Rows[0]["LedgerPK"]);
                                                    //}
                                                    ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1];//magesh 12.3.18
                                                }
                                            }
                                            if (isArrear == "N")
                                            {
                                                if (arrfeeValue[i].ToString() == "Theory" || arrfeeValue[i] == "Practical")
                                                {
                                                    //magesh 12.3.18
                                                    //String RegularFee = "Select LedgerPK from FM_LedgerMaster where LedgerName='Regular Fee'";
                                                    //dtExamFee = dirAcc.selectDataTable(RegularFee);
                                                    //if (dtExamFee.Rows.Count > 0)
                                                    //{
                                                    //    ledgerCode = Convert.ToString(dtExamFee.Rows[0]["LedgerPK"]);
                                                    //}
                                                    ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1];//magesh 12.3.18

                                                }
                                            }
                                            if (curSem == "1")
                                            {
                                                //magesh 12.3.18
                                                String RegularFee = "Select LedgerPK from FM_LedgerMaster where LedgerName='Regular Fee'";
                                                dtExamFee = dirAcc.selectDataTable(RegularFee);
                                                if (dtExamFee.Rows.Count > 0)
                                                {
                                                    ledgerCode = Convert.ToString(dtExamFee.Rows[0]["LedgerPK"]); //mcc 19sep2018
                                                }
                                                // ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1];//magesh 12.3.18
                                            }
                                            if (ledgerCode != "")
                                            {
                                                if (isPassedOut)
                                                    qryPassedOutFinYear = " and FinYearFK='" + finfkyear + "' ";

                                                currentsem = da.GetFunctionv("select TextCode from textvaltable where TextCriteria='Feeca' and college_code='" + ddlcollege.SelectedValue + "' and TextVal='" + currentsem + "'");

                                                insupexamsubject = "if exists ( select * from FT_FeeAllot where App_No ='" + appnofin + "' and FeeCategory ='" + currentsem + "' and LedgerFK ='" + ledgerCode + "' and HeaderFK ='" + headerCode + "' " + qryPassedOutFinYear + ")";
                                                //10/10/2017 aruna insupexamsubject = insupexamsubject + " update FT_FeeAllot set FeeAmount =FeeAmount+" + Amount + ",TotalAmount =TotalAmount+" + Amount + ",BalAmount=BalAmount+" + Amount + ",FinYearFK='" + finfkyear + "'  where App_No ='" + appnofin + "' and FeeCategory ='" + currentsem + "' and LedgerFK ='" + ledgerCode + "' and HeaderFK ='" + headerCode + "' else ";
                                                insupexamsubject = insupexamsubject + " update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=1,FeeAmount =FeeAmount+" + Amount + ",TotalAmount =TotalAmount+" + Amount + ",BalAmount=BalAmount+" + Amount + "  where App_No ='" + appnofin + "' and FeeCategory ='" + currentsem + "' and LedgerFK ='" + ledgerCode + "' and HeaderFK ='" + headerCode + "' " + qryPassedOutFinYear + " else ";//10/10/2017 aruna
                                                //  insupexamsubject = insupexamsubject + " update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=1,FeeAmount =FeeAmount+" + Amount + ",TotalAmount =TotalAmount+" + Amount + ",BalAmount=BalAmount+" + Amount + "  where App_No ='" + appnofin + "' and FeeCategory ='" + currentsem + "' and LedgerFK ='" + ledgerCode + "' and HeaderFK ='" + headerCode + "' " + qryPassedOutFinYear + " else ";//10/10/2017 aruna


                                                //10/10/2017 aruna insupexamsubject = insupexamsubject + " insert into FT_FeeAllot (App_No,FeeCategory,LedgerFK,HeaderFK,FeeAmount,TotalAmount,PaidAmount,BalAmount,FinYearFK)values ('" + appnofin + "','" + currentsem + "','" + ledgerCode + "','" + headerCode + "'," + Amount + "," + Amount + ",'0'," + Amount + ",'" + finfkyear + "')";
                                                insupexamsubject = insupexamsubject + " insert into FT_FeeAllot (App_No,PayMode,FeeCategory,LedgerFK,HeaderFK,FeeAmount,TotalAmount,PaidAmount,BalAmount,FinYearFK,AllotDate,MemType)values ('" + appnofin + "','1','" + currentsem + "','" + ledgerCode + "','" + headerCode + "'," + Amount + "," + Amount + ",'0'," + Amount + ",'" + finfkyear + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','1')";  //10/10/2017 aruna  //modified on 18/12/2017 due to run time error occurred
                                                insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");

                                            #endregion
                                            }
                                        }
                                    }
                                }
                            }
                            string updateexamapplication = "update exam_application set total_fee='" + exmfee + "',fee_amount='" + totalpaperfee + "' where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                            insupdateval = d2.update_method_wo_parameter(updateexamapplication, "text");
                        }
                        else
                        {
                            string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                            string insupexamsubject = "delete from exam_appl_details where appl_no='" + applno + "'";
                            insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                            insupexamsubject = "delete from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                            insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                        }
                    }
                }
                else
                {
                    //string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                    //string insupexamsubject = "delete from exam_appl_details where appl_no='" + applno + "'";
                    //insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                    //insupexamsubject = "delete from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                    //insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                }
            }
            //magesh 9/2/18
            if (rrollno != "")
            {
                lblAlertMsg.Text = rrollno + "-" + "already Applied please delete the exam application";
                divPopAlert.Visible = true;
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Save Sucessfully.')", true);
            loadexamdetails();
        }
        catch (Exception ex)
        {
            //da.sendErrorMail(ex, collegecode, "Exam Application");
            //lblerror.Visible = true;
            //lblerror.Text = ex.ToString();
        }
    }

    private ArrayList MandatoryFees()
    {
        DataTable dtMandFee = new DataTable();
        dtMandFee.Columns.Add("Application Form");
        dtMandFee.Columns.Add("Semester Mark Sheet");
        dtMandFee.Columns.Add("Theory");
        dtMandFee.Columns.Add("Practical");
        dtMandFee.Columns.Add("Project");
        dtMandFee.Columns.Add("Field Work");
        dtMandFee.Columns.Add("Viva Voice");
        dtMandFee.Columns.Add("Disseration");
        dtMandFee.Columns.Add("Consolidate Mark Sheet");
        dtMandFee.Columns.Add("Course Completaion");
        dtMandFee.Columns.Add("Online Application Fee");
        dtMandFee.Columns.Add("Arrear Theory");
        dtMandFee.Columns.Add("Arrear Practical");
        dtMandFee.Columns.Add("Central Valuation");
        dtMandFee.Columns.Add("Syllabi & Curricular");
        dtMandFee.Rows.Add("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        for (int dCol = 0; dCol < dtMandFee.Columns.Count; dCol++)
        {
            string linkVal = Convert.ToString(dtMandFee.Columns[dCol].ColumnName.Trim()) + "@#MandatoryFee";
            byte prevVal = Convert.ToByte(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'").Trim());
            if (prevVal == 1)
            {
                dtMandFee.Rows[0][dCol] = prevVal;
            }
        }
        ArrayList arrMandFees = new ArrayList();
        for (int dRow = 0; dRow < dtMandFee.Columns.Count; dRow++)
        {
            string colName = Convert.ToString(dtMandFee.Columns[dRow].ColumnName).Trim();
            if (Convert.ToString(dtMandFee.Rows[0][colName]) == "1")
                arrMandFees.Add(colName);
        }
        return arrMandFees;
    }

    public void CheckFiance()
    {
        string CheckFiannce = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeFinance' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "' ");
        if (CheckFiannce.Trim() != "0")
        {
            chkInclFinMand.Checked = true;
        }
    }

    private ArrayList MandatoryFees1(out ArrayList arrHeaderFk, out ArrayList arrLedgerFk)
    {
        DataTable dtMandFee = new DataTable();
        dtMandFee.Columns.Add("Application Form");
        dtMandFee.Columns.Add("Semester Mark Sheet");
        dtMandFee.Columns.Add("Theory");
        dtMandFee.Columns.Add("Practical");
        dtMandFee.Columns.Add("Project");
        dtMandFee.Columns.Add("Field Work");
        dtMandFee.Columns.Add("Viva Voice");
        dtMandFee.Columns.Add("Disseration");
        dtMandFee.Columns.Add("Consolidate Mark Sheet");
        dtMandFee.Columns.Add("Course Completaion");
        dtMandFee.Columns.Add("Online Application Fee");
        dtMandFee.Columns.Add("Arrear Theory");
        dtMandFee.Columns.Add("Arrear Practical");
        dtMandFee.Columns.Add("Central Valuation");
        dtMandFee.Columns.Add("Syllabi & Curricular");
        dtMandFee.Rows.Add("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        ArrayList arrMandFees = new ArrayList();
        arrHeaderFk = new ArrayList();
        arrLedgerFk = new ArrayList();
        for (int dCol = 0; dCol < dtMandFee.Columns.Count; dCol++)
        {
            string linkVal = Convert.ToString(dtMandFee.Columns[dCol].ColumnName.Trim()) + "@#MandatoryFee";
            byte prevVal = Convert.ToByte(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'").Trim());
            if (prevVal == 1)
            {
                dtMandFee.Rows[0][dCol] = prevVal;
            }

        }
        for (int dRow = 0; dRow < dtMandFee.Columns.Count; dRow++)
        {
            string colName = Convert.ToString(dtMandFee.Columns[dRow].ColumnName).Trim();
            if (Convert.ToString(dtMandFee.Rows[0][colName]) == "1")
            {
                arrMandFees.Add(colName);
                DataSet settingValue = d2.select_method_wo_parameter(" select settings,value from Master_Settings where settings='" + colName + "' --and usercode='" + Session["usercode"].ToString() + "'", "Text");
                string feeValue = string.Empty;
                string headerCode = string.Empty;
                string ledgerCode = string.Empty;
                if (settingValue.Tables.Count > 0 && settingValue.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        feeValue = Convert.ToString(settingValue.Tables[0].Rows[0][0]);
                        headerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[0];
                        ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1];
                        if (!arrHeaderFk.Contains(headerCode))
                        {
                            arrHeaderFk.Add(headerCode);
                        }
                        if (!arrLedgerFk.Contains(ledgerCode))
                        {
                            arrLedgerFk.Add(ledgerCode);
                        }
                    }
                    catch { }
                }

                else
                {
                    string malinkvalue = Convert.ToString(dtMandFee.Columns[dRow].ColumnName.Trim());
                    DataSet settingValue1 = d2.select_method_wo_parameter(" Select LedgerPK,HeaderFK from FM_LedgerMaster where LedgerName='" + malinkvalue + "'", "Text");
                    try
                    {
                        feeValue = Convert.ToString(settingValue.Tables[0].Rows[settingValue.Tables[0].Rows.Count][0]);
                        headerCode = Convert.ToString(settingValue.Tables[0].Rows[settingValue.Tables[0].Rows.Count][1]).Split(';')[0];
                        ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[settingValue.Tables[0].Rows.Count][1]).Split(';')[1];
                        if (!arrHeaderFk.Contains(headerCode))
                        {
                            arrHeaderFk.Add(headerCode);
                        }
                        if (!arrLedgerFk.Contains(ledgerCode))
                        {
                            arrLedgerFk.Add(ledgerCode);
                        }
                    }
                    catch { }


                }

            }
        }
        return arrMandFees;
    }
    //magesh 10/2/18
    private ArrayList MandatoryFees(out ArrayList arrHeaderFk, out ArrayList arrLedgerFk, out ArrayList arrfeeValue)
    {
        DataTable dtMandFee = new DataTable();
        dtMandFee.Columns.Add("Application Form");
        dtMandFee.Columns.Add("Semester Mark Sheet");
        dtMandFee.Columns.Add("Theory");
        dtMandFee.Columns.Add("Practical");
        dtMandFee.Columns.Add("Project");
        dtMandFee.Columns.Add("Field Work");
        dtMandFee.Columns.Add("Viva Voice");
        dtMandFee.Columns.Add("Disseration");
        dtMandFee.Columns.Add("Consolidate Mark Sheet");
        dtMandFee.Columns.Add("Course Completaion");
        dtMandFee.Columns.Add("Online Application Fee");
        dtMandFee.Columns.Add("Arrear Theory");
        dtMandFee.Columns.Add("Arrear Practical");
        dtMandFee.Columns.Add("Central Valuation");
        dtMandFee.Columns.Add("Syllabi & Curricular");
        dtMandFee.Rows.Add("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        ArrayList arrMandFees = new ArrayList();
        arrHeaderFk = new ArrayList();
        arrLedgerFk = new ArrayList();
        arrfeeValue = new ArrayList();
        string[] FeeCodeHFkLFkFees = new string[3];
        for (int dCol = 0; dCol < dtMandFee.Columns.Count; dCol++)
        {
            string linkVal = Convert.ToString(dtMandFee.Columns[dCol].ColumnName.Trim()) + "@#MandatoryFee";
            byte prevVal = Convert.ToByte(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'").Trim());
            if (prevVal == 1)
            {
                dtMandFee.Rows[0][dCol] = prevVal;
            }

        }
        for (int dRow = 0; dRow < dtMandFee.Columns.Count; dRow++)
        {
            string colName = Convert.ToString(dtMandFee.Columns[dRow].ColumnName).Trim();
            if (Convert.ToString(dtMandFee.Rows[0][colName]) == "1")
            {
                arrMandFees.Add(colName);
                DataSet settingValue = d2.select_method_wo_parameter(" select settings,value from Master_Settings where settings='" + colName + "' --and usercode='" + Session["usercode"].ToString() + "'", "Text");
                string feeValue = string.Empty;
                string headerCode = string.Empty;
                string ledgerCode = string.Empty;
                string FeeAmt = string.Empty;
                if (settingValue.Tables.Count > 0 && settingValue.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        string[] ValuesList = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';');
                        feeValue = Convert.ToString(settingValue.Tables[0].Rows[0][0]);
                        string[] mafess = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';');
                        int sefee = mafess.Count();
                        headerCode = (ValuesList.Length >= 1) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[0] : "0";
                        ledgerCode = (ValuesList.Length >= 2) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1] : "0";
                        if (sefee == 3)
                        {
                            FeeAmt = (ValuesList.Length >= 3) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[2] : "0";
                        }
                        else
                        {
                            FeeAmt = "";
                        }
                        FeeCodeHFkLFkFees = new string[3];
                        FeeCodeHFkLFkFees[0] = headerCode;
                        FeeCodeHFkLFkFees[1] = ledgerCode;
                        FeeCodeHFkLFkFees[2] = FeeAmt;

                        if (!arrfeeValue.Contains(feeValue))
                        {
                            arrfeeValue.Add(feeValue);
                        }
                        if (!arrHeaderFk.Contains(headerCode))
                        {
                            arrHeaderFk.Add(headerCode);
                        }
                        if (!arrLedgerFk.Contains(ledgerCode))
                        {
                            arrLedgerFk.Add(ledgerCode);
                        }
                    }
                    catch { }
                }

                else
                {
                    string malinkvalue = Convert.ToString(dtMandFee.Columns[dRow].ColumnName.Trim());
                    DataSet settingValue1 = d2.select_method_wo_parameter(" Select LedgerPK,HeaderFK from FM_LedgerMaster where LedgerName='" + malinkvalue + "'", "Text");
                    try
                    {
                        feeValue = Convert.ToString(settingValue.Tables[0].Rows[settingValue.Tables[0].Rows.Count][0]);
                        headerCode = Convert.ToString(settingValue.Tables[0].Rows[settingValue.Tables[0].Rows.Count][1]).Split(';')[0];
                        ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[settingValue.Tables[0].Rows.Count][1]).Split(';')[1];
                        if (!arrHeaderFk.Contains(headerCode))
                        {
                            arrHeaderFk.Add(headerCode);
                        }
                        if (!arrLedgerFk.Contains(ledgerCode))
                        {
                            arrLedgerFk.Add(ledgerCode);
                        }
                    }
                    catch { }


                }

            }
        }
        return arrMandFees;
    }

    //last modified by Idhris -- 16-12-2016
    protected void chkSelAllSub_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelAllSub.Checked)
        {
            for (int r = 0; r < FpSpreadViewSubjects.Sheets[0].RowCount; r++)
            {
                FpSpreadViewSubjects.Sheets[0].Cells[r, 7].Value = 1;
            }
        }
        else
        {
            for (int r = 0; r < FpSpreadViewSubjects.Sheets[0].RowCount; r++)
            {
                FpSpreadViewSubjects.Sheets[0].Cells[r, 7].Value = 0;
            }
        }
        FpSpreadViewSubjects.SaveChanges();
    }

    protected void btnViewSubject_Click(object sender, EventArgs e)
    {
        try
        {
            chkSelAllSub.Checked = false;
            lblViewSubjectError.Text = string.Empty;
            lblViewSubjectError.Visible = false;
            divViewSubjects.Visible = false;
            string rollNo = string.Empty;
            string appNo = string.Empty;
            string redoStudent = string.Empty;
            string studentBatchYear = string.Empty;
            string studentDegreeCode = string.Empty;
            string studentCurrentSem = string.Empty;
            bool isStudentRedo = false;
            string qry = string.Empty;
            string examMonth = string.Empty;
            string examYear = string.Empty;
            int selStudents = 0;
            FpSpread1.SaveChanges();
            if (Convert.ToString(ddlYear.SelectedValue).Trim() == "0")
            {
                divViewSubjects.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            else
            {
                examYear = Convert.ToString(ddlYear.SelectedValue).Trim();
            }
            if ((Convert.ToString(ddlMonth.SelectedValue).Trim() == "0"))
            {
                divViewSubjects.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            else
            {
                examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            }
            string appldate = Convert.ToString(txtappldate.Text).Trim();
            string lastappladet = Convert.ToString(txtappllastdate.Text).Trim();
            DateTime dtappl = new DateTime();
            DateTime dtlastappl = new DateTime();
            string[] spd;
            if (!string.IsNullOrEmpty(appldate))
            {
                spd = appldate.Split('/');
                dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Applied Date And Then Proceed";
                return;
            }
            if (!string.IsNullOrEmpty(lastappladet))
            {
                spd = lastappladet.Split('/');
                dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Last Date For Application And Then Proceed";
                return;
            }
            if (dtappl > dtlastappl)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            if (FpSpread1.Sheets[0].RowCount <= 1)
            {
                divViewSubjects.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "No Record(s) Were Found";
                return;
            }
            for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    selStudents++;
                    rollNo = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Text);
                    appNo = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Note).Trim();
                    redoStudent = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Tag).Trim();
                    studentBatchYear = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Note).Trim();
                    studentDegreeCode = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Tag).Trim();
                    studentCurrentSem = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 4].Text).Trim();
                    isStudentRedo = false;
                    bool.TryParse(redoStudent.Trim(), out isStudentRedo);
                }
            }
            if (selStudents == 0)
            {
                divViewSubjects.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Student And Then Proceed";
                return;
            }
            else if (selStudents > 1)
            {
                divViewSubjects.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Select Only One Student And Then Proceed";
                return;
            }
            string qryExternalCheck = string.Empty;
            if (chkExternalMark.Checked)
            {
                qryExternalCheck = " and (m1.result='Pass' or m1.external_mark>=s1.min_ext_marks)";
            }
            else
            {
                qryExternalCheck = " and m1.result='Pass'";
            }
            if (rollNo.Trim() != "")
            {
                //string query = " select distinct r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,sy.semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sc.roll_no,'N' as arrst,s.arrfee as total_fee  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r where r.Roll_No =sc.roll_no and  sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and sc.roll_no='" + rollNo + "' and sc.semester<7 and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='" + rollNo + "' and m.result='Pass') order by sy.semester desc,s.subject_code,ss.subject_type asc";
                qry = "select Ed.exam_code,ea.appl_no,ead.subject_no,ea.roll_no from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ed.exam_month='" + examMonth + "' and ed.exam_year='" + examYear + "' and ea.roll_no='" + rollNo + "' ";
                DataSet dsExamApplied = da.select_method_wo_parameter(qry, "Text");
                if (dsExamApplied.Tables.Count > 0 && dsExamApplied.Tables[0].Rows.Count > 0)
                {
                    divViewSubjects.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "You Are Already Applied.So, Please Delete Your Exam Application And Then Proceed.";
                    return;
                }
                string degreecode = ddlbranch.SelectedValue.ToString();
                string batchyear = ddlbatch.SelectedValue.ToString();
                string sem = ddlsem.SelectedValue.ToString();
                if (chkpassout.Checked == true)
                {
                    int semval = ddlsem.Items.Count;
                    semval++;
                    sem = semval.ToString();
                }
                Dictionary<string, List<string>> dicAppNoRedoSemester = new Dictionary<string, List<string>>();
                Dictionary<string, List<string>> dicRollNoRedoSemester = new Dictionary<string, List<string>>();
                getAllRedoStudentsSemester(out dicAppNoRedoSemester, out dicRollNoRedoSemester, batchYear: batchyear, degreeCode: degreecode);
                string query = string.Empty;
                string qryNotEligibleSemesters = string.Empty;
                if (dicRollNoRedoSemester.ContainsKey(rollNo.Trim()))
                {
                    List<string> lstRedoSem = new List<string>();
                    lstRedoSem = dicRollNoRedoSemester[rollNo.Trim()];
                    string semester = string.Empty;
                    semester = string.Join("','", lstRedoSem.ToArray());
                    if (isStudentRedo)
                    {
                        lstRedoSem.Remove(studentCurrentSem);
                        semester = string.Join("','", lstRedoSem.ToArray());
                    }
                    if (!string.IsNullOrEmpty(semester))
                    {
                        qryNotEligibleSemesters = " and sc.semester not in('" + semester + "')";
                    }
                }
                if (dicAppNoRedoSemester.ContainsKey(appNo.Trim()))
                {
                    List<string> lstRedoSem = new List<string>();
                    lstRedoSem = dicAppNoRedoSemester[appNo.Trim()];
                    string semester = string.Empty;
                    semester = string.Join("','", lstRedoSem.ToArray());
                    if (isStudentRedo)
                    {
                        lstRedoSem.Remove(studentCurrentSem);
                        semester = string.Join("','", lstRedoSem.ToArray());
                    }
                    if (!string.IsNullOrEmpty(semester))
                    {
                        qryNotEligibleSemesters = " and sc.semester not in('" + semester + "')";
                    }
                }
                string valSemcur = string.Empty;
                string valsemarr = string.Empty;
                if (cblsem.Items.Count > 0)
                {
                    for (int s = 0; s < cblsem.Items.Count; s++)
                    {
                        if (cblsem.Items[s].Selected)
                        {
                            int ss = Convert.ToInt32(cblsem.Items[s].Value);
                            if (ss >= Convert.ToInt32(sem))
                            {
                                if (string.IsNullOrEmpty(valSemcur))
                                    valSemcur = "'" + ss + "'";
                                else
                                    valSemcur = valSemcur + "," + "'" + ss + "'";
                            }
                            if (ss < Convert.ToInt32(sem))
                            {
                                if (string.IsNullOrEmpty(valsemarr))
                                    valsemarr = "'" + ss + "'";
                                else
                                    valsemarr = valsemarr + "," + "'" + ss + "'";
                            }
                        }
                    }

                }
                if (valsemarr == "")
                {
                    valsemarr = "''";
                }
                DataTable dtfuturesub = new DataTable();
                string futSubject = string.Empty;
                if (chksemwise.Checked && !chkpassout.Checked)
                {
                    string strfuturesub = "select subject_no from Futuresub_Exam_app where ExamMonth='" + examMonth + "' and ExamYear='" + examYear + "' and Batch_year='" + batchyear + "' and DegreeCode='" + degreecode + "' and Semester='" + sem + "'";
                    dtfuturesub = dirAcc.selectDataTable(strfuturesub);
                    if (dtfuturesub.Rows.Count > 0)
                    {
                        foreach (DataRow dt1 in dtfuturesub.Rows)
                        {
                            string subjNo = Convert.ToString(dt1["subject_no"]);
                            if (string.IsNullOrEmpty(futSubject))
                                futSubject = "'" + subjNo + "'";
                            else
                                futSubject = futSubject + "," + "'" + subjNo + "'";
                        }
                    }
                }

                if (chksemwise.Checked && !chkpassout.Checked)
                {
                    query = "select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                    query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                    query = query + " and r.degree_code=sy.degree_code and sy.semester='" + sem + "' and sc.roll_no='" + rollNo + "' " + qryNotEligibleSemesters;

                    //if (!string.IsNullOrEmpty(futSubject))
                    //{
                    query = query + " union ";
                    query = query + "select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                    query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                    query = query + " and r.degree_code=sy.degree_code and sy.semester in(" + valSemcur + ")  and sc.roll_no='" + rollNo + "' " + qryNotEligibleSemesters;//and s.subject_no in(" + futSubject + ")
                    //}
                    query = query + " union ";
                    query = query + " select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst,s.arrfee as total_fee from Registration r,subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                    query = query + " where r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and sy.Batch_Year=r.Batch_Year and  sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollNo + "' and sc.semester in(" + valsemarr + ") " + qryNotEligibleSemesters; ;
                    query = query + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollNo + "' " + qryExternalCheck + ") ";
                    query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";
                }
                else if (isStudentRedo)
                {
                    //query = "select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r,StudentRedoDetails sr ";
                    //query = query + " where sr.Stud_AppNo=r.App_No  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and sr.BatchYear=sy.Batch_Year ";
                    //query = query + " and sr.DegreeCode=sy.degree_code and ss.promote_count=1 and sy.semester='" + sem + "' and sc.roll_no='" + rollno + "' " + strnotelisub + " " + qryNotEligibleSemesters;
                    //query = query + " union ";
                    //query = query + " select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst,s.arrfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                    //query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollno + "' " + qryNotEligibleSemesters;//and sc.semester<" + sem + "
                    //query = query + " and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='" + rollno + "' " + qryExternalCheck + ") " + strnotelisub + "";
                    //query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";

                    query = "  select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r,StudentRedoDetails sr ";
                    query += " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and r.Roll_No=sc.roll_no and r.App_No=sr.Stud_AppNo and sr.BatchYear=sy.Batch_Year ";
                    query += " and sr.DegreeCode=sy.degree_code and sy.semester='" + sem + "' and sc.roll_no='" + rollNo + "' " + qryNotEligibleSemesters;
                    query += " union ";
                    query += "  select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst,s.arrfee as total_fee from Registration r,subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,StudentRedoDetails sr  where sr.Stud_AppNo=r.App_No and r.Roll_No=sc.roll_no and sr.DegreeCode=sy.degree_code and sy.Batch_Year=sr.BatchYear and  sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollNo + "' and sc.semester<'" + sem + "' " + qryNotEligibleSemesters;
                    query += " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollNo + "' " + qryExternalCheck + ") order by sy.semester desc,s.subject_code,ss.subject_type";

                    //select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and r.Roll_No=sc.roll_no and sr.BatchYear=sy.Batch_Year and sr.DegreeCode=sy.degree_code and ss.promote_count=1 and sy.semester='7' and sc.roll_no='13PPH001' " + strnotelisub + " " + qryNotEligibleSemesters

                    //union select s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst,s.arrfee as feesval  from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='13PPH001' and sc.semester<7 and sc.semester not in(3) " + sem + " " + qryNotEligibleSemesters and s.subject_no not in (select m.subject_no from mark_entry m where m.roll_no='' and result='pass')  order by sy.semester desc,s.subject_code,ss.subject_type
                }
                else
                {
                    query = "select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'Y' as arrst,s.curfee as total_fee from subject s,subjectChooser sc,sub_sem ss,syllabus_master sy,Registration r ";
                    query = query + " where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and r.Roll_No=sc.roll_no and r.Batch_Year=sy.Batch_Year ";
                    query = query + " and r.degree_code=sy.degree_code and sy.semester='" + sem + "' and sc.roll_no='" + rollNo + "' " + qryNotEligibleSemesters;
                    query = query + " union ";
                    query = query + " select r.Stud_Name,r.Reg_No,sy.Batch_Year,sy.degree_code,r.current_semester,s.subject_name,s.subject_code,ss.subject_type,ss.subType_no,s.subject_no,sy.semester,sc.roll_no,'N' as arrst,s.arrfee as total_fee from Registration r,subject s,subjectChooser sc,sub_sem ss,syllabus_master sy ";
                    query = query + " where r.Roll_No=sc.roll_no and r.degree_code=sy.degree_code and sy.Batch_Year=r.Batch_Year and  sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.subject_no=sc.subject_no and ss.promote_count=1 and sc.roll_no='" + rollNo + "' and sc.semester<" + sem + " " + qryNotEligibleSemesters; ;
                    query = query + " and s.subject_code not in (select distinct s1.subject_code from mark_entry m1,subject s1 where s1.subject_no=m1.subject_no and m1.roll_no='" + rollNo + "' " + qryExternalCheck + ") ";
                    query = query + " order by sy.semester desc,s.subject_code,ss.subject_type";
                }

                //and (m.result='Pass' or m.external_mark>=s.min_ext_marks)
                DataSet dssubappl = da.select_method_wo_parameter(query, "Text");
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
            }
            FpSpreadViewSubjects.Sheets[0].RowCount = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                FpSpreadViewSubjects.Visible = false;
                FpSpreadViewSubjects.CommandBar.Visible = false;
                FpSpreadViewSubjects.Sheets[0].RowCount = 0;
                FpSpreadViewSubjects.Sheets[0].ColumnCount = 8;
                FpSpreadViewSubjects.Sheets[0].Columns[0].Width = 40;
                FpSpreadViewSubjects.Sheets[0].Columns[1].Width = 80;
                FpSpreadViewSubjects.Sheets[0].Columns[2].Width = 130;
                FpSpreadViewSubjects.Sheets[0].Columns[3].Width = 180;
                FpSpreadViewSubjects.Sheets[0].Columns[4].Width = 90;
                FpSpreadViewSubjects.Sheets[0].AutoPostBack = false;
                FpSpreadViewSubjects.Sheets[0].Columns[0].Locked = true;
                FpSpreadViewSubjects.Sheets[0].Columns[1].Locked = true;
                FpSpreadViewSubjects.Sheets[0].Columns[1].Visible = false;
                FpSpreadViewSubjects.Sheets[0].Columns[2].Locked = true;
                FpSpreadViewSubjects.Sheets[0].Columns[2].Visible = false;
                FpSpreadViewSubjects.Sheets[0].Columns[3].Locked = true;
                FpSpreadViewSubjects.Sheets[0].Columns[3].Visible = false;
                FpSpreadViewSubjects.Sheets[0].Columns[4].Locked = true;
                FpSpreadViewSubjects.Sheets[0].Columns[5].Locked = true;
                FpSpreadViewSubjects.Sheets[0].Columns[6].Locked = true;
                FpSpreadViewSubjects.Sheets[0].RowHeader.Visible = false;
                FpSpreadViewSubjects.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpreadViewSubjects.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpreadViewSubjects.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpreadViewSubjects.Sheets[0].SheetCorner.RowCount = 1;
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem / Year";
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code";
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Name";
                FpSpreadViewSubjects.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
                FpSpreadViewSubjects.Sheets[0].Columns[0].Width = 60;
                FpSpreadViewSubjects.Sheets[0].Columns[1].Width = 100;
                FpSpreadViewSubjects.Sheets[0].Columns[2].Width = 100;
                FpSpreadViewSubjects.Sheets[0].Columns[3].Width = 300;
                FpSpreadViewSubjects.Sheets[0].Columns[4].Width = 150;
                FpSpreadViewSubjects.Sheets[0].Columns[5].Width = 150;
                FpSpreadViewSubjects.Sheets[0].Columns[6].Width = 350;
                FpSpreadViewSubjects.Sheets[0].Columns[7].Width = 80;
                int sno = 0;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell.AutoPostBack = false;
                for (int studcount = 0; studcount < ds.Tables[0].Rows.Count; studcount++)
                {
                    string regno = string.Empty;
                    string studname = string.Empty;
                    FpSpreadViewSubjects.Visible = true;
                    string batchyear = ds.Tables[0].Rows[studcount]["batch_year"].ToString();
                    regno = ds.Tables[0].Rows[studcount]["reg_no"].ToString();
                    studname = ds.Tables[0].Rows[studcount]["stud_name"].ToString();
                    rollNo = ds.Tables[0].Rows[studcount]["roll_no"].ToString();
                    string currentSemester = Convert.ToString(ds.Tables[0].Rows[studcount]["current_semester"]).Trim();
                    sno++;
                    FpSpreadViewSubjects.Sheets[0].RowCount++;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].Text = rollNo;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(currentSemester);
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].Text = regno;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].Text = studname;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[studcount]["degree_code"]);
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[studcount]["semester"].ToString();
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 4].Tag = ds.Tables[0].Rows[studcount]["arrst"].ToString().Trim();
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[studcount]["subject_code"]);
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[studcount]["subject_no"]);
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[studcount]["subject_name"]);
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[studcount]["total_fee"]);
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 7].CellType = chkcell;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    FpSpreadViewSubjects.Sheets[0].Cells[FpSpreadViewSubjects.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                }
                FpSpreadViewSubjects.Sheets[0].PageSize = FpSpreadViewSubjects.Sheets[0].RowCount;
                FpSpreadViewSubjects.Width = 810;
                FpSpreadViewSubjects.Height = 350;
                FpSpreadViewSubjects.Visible = true;
                divViewSubjects.Visible = true;
                lblViewSubjectError.Visible = false;
                FpSpreadViewSubjects.SaveChanges();
            }
            else
            {
                //lblViewSubjectError.Visible = true;
                //lblViewSubjectError.Text = "No Records Found";
                //FpSpreadViewSubjects.Visible = false;
                imgAlert.Visible = true;
                lbl_alert.Text = "No Subjects Were Found";
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnViewSaveApplication_Click(object sender, EventArgs e)
    {
        try
        {
            lblViewSubjectError.Text = string.Empty;
            lblViewSubjectError.Visible = false;
            bool saveflag = false;
            FpSpreadViewSubjects.SaveChanges();
            if (FpSpreadViewSubjects.Sheets[0].RowCount == 0)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "No Subjects Were Found";
                return;
            }
            string rollNo = string.Empty;
            for (int s = 0; s < FpSpreadViewSubjects.Sheets[0].RowCount; s++)
            {
                int stva = Convert.ToInt32(FpSpreadViewSubjects.Sheets[0].Cells[s, 7].Value);
                int totalfee_new = 0;
                if (stva == 1)
                {
                    saveflag = true;
                    string fees = Convert.ToString(FpSpreadViewSubjects.Sheets[0].Cells[s, 6].Tag);
                    rollNo = Convert.ToString(FpSpreadViewSubjects.Sheets[0].Cells[s, 1].Text);
                    if (fees.Trim() != "")
                    {
                        totalfee_new = totalfee_new + Convert.ToInt32(fees);
                    }
                }
            }
            if (saveflag == true)
            {
                if (!chkInclFinMand.Checked)
                {
                    getFinanceNotAppliedAmountForView(FpSpreadViewSubjects, rollNo);
                }
                else
                {
                    ArrayList arrMandFees = MandatoryFees();
                    if (arrMandFees.Count == 0)
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Set Mandatory Fees";
                        return;
                    }
                    else
                    {
                        getFinanceAppliedAmountForView(FpSpreadViewSubjects, rollNo);
                    }
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Atleast One Subject";
                return;
            }
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    protected void btnViewSubjects_exit_Clcik(object sender, EventArgs e)
    {
        try
        {
            lblViewSubjectError.Text = string.Empty;
            lblViewSubjectError.Visible = false;
            divViewSubjects.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    private void getFinanceNotAppliedAmountForView(FarPoint.Web.Spread.FpSpread FpSpreadView, string rollNo = null)
    {
        try
        {
            FpSpreadView.SaveChanges();
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            int insupdateval = 0;
            string subjectNo = string.Empty;
            string qrysubjectnoinclude = string.Empty;
            ArrayList arrNotEligibleSubjects = new ArrayList();
            string degreecode = Convert.ToString(ddlbranch.SelectedValue).Trim();
            string batchyear = Convert.ToString(ddlbatch.SelectedValue).Trim();
            string sem = Convert.ToString(ddlsem.SelectedValue).Trim();
            if (chkpassout.Checked == true)
            {
                int semval = ddlsem.Items.Count;
                semval++;
                sem = semval.ToString();
            }
            string exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            Boolean setflag = false;
            bool appliedSuccess = false;
            for (int r = 0; r < FpSpreadView.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpreadView.Sheets[0].Cells[r, 7].Value);
                if (rollNo == null)
                {
                    rollNo = Convert.ToString(FpSpreadView.Sheets[0].Cells[r, 1].Text).Trim();
                }
                if (stva == 1)
                {
                    setflag = true;
                    string subno = Convert.ToString(FpSpreadView.Sheets[0].Cells[r, 5].Tag).Trim();
                    if (!string.IsNullOrEmpty(subno.Trim()))
                    {
                        if (!string.IsNullOrEmpty(subjectNo.Trim()))
                        {
                            subjectNo += ",'" + subno.Trim() + "'";
                        }
                        else
                        {
                            subjectNo = "'" + subno.Trim() + "'";
                        }
                    }
                }
            }
            if (setflag == false)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select The Subjects And Then Proceed";
                return;
            }
            if (!string.IsNullOrEmpty(subjectNo.Trim()))
            {
                qrysubjectnoinclude = " and subject_no in(" + subjectNo + ")";
            }
            string strnoteligilbesubject = "select r.Roll_No,s.subject_no from Registration r,studentsemestersubjectdebar s where r.Roll_No=s.roll_no and r.cc=0 and r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.Roll_No in('" + rollNo + "') ";
            DataSet dsnoteligiblesubject = d2.select_method_wo_parameter(strnoteligilbesubject, "Text");
            string examyear = ddlYear.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            string appldate = Convert.ToString(txtappldate.Text).Trim();
            string lastappladet = Convert.ToString(txtappllastdate.Text).Trim();
            DateTime dtappl = new DateTime();
            DateTime dtlastappl = new DateTime();
            string[] spd;
            if (!string.IsNullOrEmpty(appldate))
            {
                spd = appldate.Split('/');
                dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Applied Date And Then Proceed";
                return;
            }
            if (!string.IsNullOrEmpty(lastappladet))
            {
                //spd = appldate.Split('/');
                spd = lastappladet.Split('/');
                dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Last Date For Application And Then Proceed";
                return;
            }
            if (dtappl > dtlastappl)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            string applfee = Convert.ToString(txtapplfee.Text).Trim();
            string smtfee = Convert.ToString(txtstmtfee.Text).Trim();
            string extrafee = Convert.ToString(txtextrafee.Text).Trim();
            string fineamount = Convert.ToString(txtfine.Text).Trim();
            double appltotalcost = 0;
            double applcost = 0;
            if (applfee.Trim() != "")
            {
                applcost = Convert.ToDouble(applfee);
            }
            double stmtcost = 0;
            if (smtfee.Trim() != "")
            {
                stmtcost = Convert.ToDouble(smtfee);
            }
            double extracost = 0;
            if (extrafee.Trim() != "")
            {
                extracost = Convert.ToDouble(extrafee);
            }
            double fineamo = 0;
            if (fineamount.Trim() != "")
            {
                fineamo = Convert.ToDouble(fineamount);
            }
            appltotalcost = applcost + stmtcost + extracost + fineamo;
            string getexamcode = d2.GetFunction("select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            if (getexamcode == null || getexamcode.Trim() == "" || getexamcode.Trim() == "0")
            {
                string setdegreevalues = "if not exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                setdegreevalues += " insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code,isSupplementaryExam) values ('" + degreecode + "','" + exammonth + "','" + examyear + "','" + batchyear + "','" + sem + "','" + collegecode + "','" + chkIsSupplementaryExam.Checked + "')";
                setdegreevalues += " else update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
                getexamcode = d2.GetFunction("select exam_code from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            }
            else
            {
                string setdegreevalues = "if exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                setdegreevalues += " update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
            }
            if (setflag)
            {
                string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'");
                string insupexamsubject = "delete ead from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no='" + rollNo + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                string strnotelisub = string.Empty;
                dsnoteligiblesubject.Tables[0].DefaultView.RowFilter = "roll_no='" + rollNo + "'";
                DataView dvnoteliglebsubject = dsnoteligiblesubject.Tables[0].DefaultView;
                arrNotEligibleSubjects.Clear();
                for (int ne = 0; ne < dvnoteliglebsubject.Count; ne++)
                {
                    strnotelisub = Convert.ToString(dvnoteliglebsubject[ne]["subject_no"]).Trim();
                    if (!arrNotEligibleSubjects.Contains(strnotelisub))
                    {
                        arrNotEligibleSubjects.Add(strnotelisub);
                    }
                }
                double exmfee = appltotalcost;
                double totalpaperfee = 0;
                if (FpSpreadView.Sheets[0].RowCount > 0)
                {
                    if (applno.Trim() == "" || applno.Trim() == "0")
                    {
                        string setexamapplication = "if exists(select * from exam_application where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "')";
                        setexamapplication = setexamapplication + " update exam_application set applied_date='" + dtappl.ToString("MM/dd/yyyy") + "',total_fee='0',extra_fee='" + extracost.ToString() + "',fine='" + fineamo.ToString() + "',cost_appl='" + applcost.ToString() + "',cost_mark='" + stmtcost.ToString() + "',lastdate='" + dtlastappl.ToString("MM/dd/yyyy") + "',Exam_type='0',fee_amount='0'";
                        setexamapplication = setexamapplication + " where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'";
                        setexamapplication = setexamapplication + " else ";
                        setexamapplication = setexamapplication + " insert into exam_application(roll_no,applied_date,total_fee,exam_code,extra_fee,fine,cost_appl,cost_mark,lastdate,Exam_type,fee_amount)";
                        setexamapplication = setexamapplication + " values('" + rollNo + "','" + dtappl.ToString("MM/dd/yyyy") + "','0','" + getexamcode + "','" + extracost.ToString() + "','" + fineamo.ToString() + "','" + applcost + "','" + stmtcost + "','" + dtlastappl.ToString("MM/dd/yyyy") + "','0','0')";
                        insupdateval = d2.update_method_wo_parameter(setexamapplication, "text");
                    }
                    applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'");
                    for (int s = 0; s < FpSpreadView.Sheets[0].RowCount; s++)
                    {
                        int stva = Convert.ToInt32(FpSpreadView.Sheets[0].Cells[s, 7].Value);
                        if (stva == 1)
                        {
                            string subject = Convert.ToString(FpSpreadView.Sheets[0].Cells[s, 5].Tag).Trim();
                            int subsem = Convert.ToInt16(Convert.ToString(FpSpreadView.Sheets[0].Cells[s, 4].Text).Trim());
                            string fees = Convert.ToString(FpSpreadView.Sheets[0].Cells[s, 6].Tag).Trim();
                            if (!arrNotEligibleSubjects.Contains(subject))
                            {
                                if (fees.Trim() != "")
                                {
                                    exmfee = exmfee + Convert.ToDouble(fees);
                                    totalpaperfee = totalpaperfee + Convert.ToDouble(fees);
                                }
                                string attempts = "0";
                                string types = string.Empty;
                                int attval = Convert.ToInt32(sem) - Convert.ToInt32(subsem);
                                if (attval > 0)
                                {
                                    attempts = attval.ToString();
                                    types = "*";
                                }
                                insupexamsubject = "if exists(select * from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subject + "')";
                                insupexamsubject = insupexamsubject + "  update exam_appl_details set attempts='" + attempts + "',fee='" + fees + "',type='" + types + "',attend='1' where appl_no='" + applno + "' and subject_no='" + subject + "'";
                                insupexamsubject = insupexamsubject + " else insert into exam_appl_details(subject_no,attempts,fee,type,appl_no,attend) values('" + subject + "','" + attempts + "','" + fees + "','" + types + "','" + applno + "','1')";
                                insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                                if (insupdateval > 0)
                                {
                                    appliedSuccess = true;
                                }
                            }
                        }
                        string updateexamapplication = "update exam_application set total_fee='" + exmfee + "',fee_amount='" + totalpaperfee + "' where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'";
                        insupdateval = d2.update_method_wo_parameter(updateexamapplication, "text");
                        if (insupdateval > 0)
                        {
                            appliedSuccess = true;
                        }
                    }
                }
                else
                {
                }
            }
            else
            {
                //string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                //string insupexamsubject = "delete from exam_appl_details where appl_no='" + applno + "'";
                //insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                //insupexamsubject = "delete from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                //insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
            }
            //}
            divViewSubjects.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Sucessfully.')", true);
            loadexamdetails();
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    private void getFinanceAppliedAmountForView(FarPoint.Web.Spread.FpSpread FpSpreadView, string rollNo = null)
    {
        try
        {
            ArrayList arrMandFees = MandatoryFees();
            bool setflag = false;
            string subjectNo = string.Empty;
            string qrysubjectnoinclude = string.Empty;
            FpSpreadView.SaveChanges();
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            string appnofin = string.Empty;
            string currentsem = string.Empty;
            int insupdateval = 0;
            string degreecode = Convert.ToString(ddlbranch.SelectedValue).Trim();
            string batchyear = Convert.ToString(ddlbatch.SelectedValue).Trim();
            string sem = Convert.ToString(ddlsem.SelectedValue).Trim();
            if (chkpassout.Checked == true)
            {
                int semval = ddlsem.Items.Count;
                semval++;
                sem = semval.ToString();
            }
            string exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            string examyear = Convert.ToString(ddlYear.SelectedValue).Trim();
            collegecode = Convert.ToString(ddlcollege.SelectedValue).Trim();
            string appldate = Convert.ToString(txtappldate.Text).Trim();
            string lastappladet = Convert.ToString(txtappllastdate.Text).Trim();
            DateTime dtappl = new DateTime();
            DateTime dtlastappl = new DateTime();
            string[] spd;
            if (!string.IsNullOrEmpty(appldate))
            {
                spd = appldate.Split('/');
                dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Applied Date And Then Proceed";
                return;
            }
            if (!string.IsNullOrEmpty(lastappladet))
            {
                spd = lastappladet.Split('/');
                dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Last Date For Application And Then Proceed";
                return;
            }
            if (dtappl > dtlastappl)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            if (dtappl > dtlastappl)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            string applfee = Convert.ToString(txtapplfee.Text).Trim();
            string smtfee = Convert.ToString(txtstmtfee.Text).Trim();
            string extrafee = Convert.ToString(txtextrafee.Text).Trim();
            string fineamount = Convert.ToString(txtfine.Text).Trim();
            for (int r = 0; r < FpSpreadView.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpreadView.Sheets[0].Cells[r, 7].Value);
                if (rollNo == null)
                {
                    rollNo = Convert.ToString(FpSpreadView.Sheets[0].Cells[r, 1].Text);
                }
                if (stva == 1)
                {
                    setflag = true;
                    string subno = Convert.ToString(FpSpreadView.Sheets[0].Cells[r, 5].Tag).Trim();
                    if (!string.IsNullOrEmpty(subno.Trim()))
                    {
                        if (!string.IsNullOrEmpty(subjectNo.Trim()))
                        {
                            subjectNo += ",'" + subno.Trim() + "'";
                        }
                        else
                        {
                            subjectNo = "'" + subno.Trim() + "'";
                        }
                    }
                }
            }
            if (setflag == false)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select The Subjects And Then Proceed";
                return;
            }
            if (!string.IsNullOrEmpty(subjectNo.Trim()))
            {
                qrysubjectnoinclude = " and subject_no in(" + subjectNo + ")";
            }
            double appltotalcost = 0;
            double applcost = 0;
            if (applfee.Trim() != "")
            {
                applcost = Convert.ToDouble(applfee.Trim());
            }
            double stmtcost = 0;
            if (smtfee.Trim() != "")
            {
                stmtcost = Convert.ToDouble(smtfee);
            }
            double extracost = 0;
            if (extrafee.Trim() != "")
            {
                extracost = Convert.ToDouble(extrafee);
            }
            double fineamo = 0;
            if (fineamount.Trim() != "")
            {
                fineamo = Convert.ToDouble(fineamount);
            }
            appltotalcost = applcost + stmtcost + extracost + fineamo;
            string getexamcode = d2.GetFunction("select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            if (getexamcode == null || getexamcode.Trim() == "" || getexamcode.Trim() == "0")
            {
                string setdegreevalues = "if not exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                setdegreevalues += " insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code,isSupplementaryExam) values ('" + degreecode + "','" + exammonth + "','" + examyear + "','" + batchyear + "','" + sem + "','" + collegecode + "','" + chkIsSupplementaryExam.Checked + "')";
                setdegreevalues += " else update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
                getexamcode = d2.GetFunction("select exam_code from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'");
            }
            else
            {
                string setdegreevalues = "if exists(select * from exam_details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "')";
                setdegreevalues += " update exam_details set isSupplementaryExam='" + chkIsSupplementaryExam.Checked + "'  where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
                insupdateval = d2.update_method_wo_parameter(setdegreevalues, "text");
            }
            if (setflag)
            {
                double exmfee = appltotalcost;
                double totalpaperfee = 0;
                if (FpSpreadView.Sheets[0].RowCount > 0)
                {
                    string setexamapplication = "if exists(select * from exam_application where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "')";
                    setexamapplication = setexamapplication + " update exam_application set applied_date='" + dtappl.ToString("MM/dd/yyyy") + "',total_fee='0',extra_fee='" + Convert.ToString(extracost).Trim() + "',fine='" + fineamo.ToString() + "',cost_appl='" + applcost.ToString() + "',cost_mark='" + stmtcost.ToString() + "',lastdate='" + dtlastappl.ToString("MM/dd/yyyy") + "',Exam_type='0',fee_amount='0'";
                    setexamapplication = setexamapplication + " where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'";
                    setexamapplication = setexamapplication + " else ";
                    setexamapplication = setexamapplication + " insert into exam_application(roll_no,applied_date,total_fee,exam_code,extra_fee,fine,cost_appl,cost_mark,lastdate,Exam_type,fee_amount)";
                    setexamapplication = setexamapplication + " values('" + rollNo + "','" + dtappl.ToString("MM/dd/yyyy") + "','0','" + getexamcode + "','" + extracost.ToString() + "','" + fineamo.ToString() + "','" + applcost + "','" + stmtcost + "','" + dtlastappl.ToString("MM/dd/yyyy") + "','0','0')";
                    insupdateval = d2.update_method_wo_parameter(setexamapplication, "text");
                    appnofin = da.GetFunctionv("select app_no from registration where roll_no='" + rollNo + "'");
                    string curSem = da.GetFunctionv("select current_semester from registration where roll_no='" + rollNo + "'");
                    string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'");
                    string currentfinyear = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);
                    string finfkyear = currentfinyear;
                    string colllnmae = string.Empty;
                    string feecat = da.GetFunctionv("select LinkValue from New_InsSettings where LinkName='Fee Yearwise' and user_code='" + Convert.ToString(Session["usercode"]) + "' and college_code='" + ddlcollege.SelectedValue + "'");
                    DataSet settingValue = d2.select_method_wo_parameter(" select settings,value from Master_Settings where settings='Arrear Theory' --and usercode='" + Convert.ToString(Session["usercode"]) + "'", "Text");
                    string feeValue = string.Empty;
                    string headerCode = string.Empty;
                    string ledgerCode = string.Empty;

                    int currentSemester = 0;
                    int.TryParse(curSem, out currentSemester);
                    string maxDuration = d2.GetFunction("select distinct NDurations from ndegree where Degree_code='" + degreecode + " and batch_year='" + batchyear + "");
                    int maxDurations = 0;
                    if (maxDuration.Trim() == "" || maxDuration == null || maxDuration == "0")
                    {
                        maxDuration = d2.GetFunction("select distinct duration,first_year_nonsemester  from degree where degree_code='" + degreecode + "'");
                    }
                    int.TryParse(maxDuration, out maxDurations);

                    Dictionary<string, string[]> dicManFeesAmt = new Dictionary<string, string[]>();
                    MandatoryFeesValues(out dicManFeesAmt);
                    bool isPassedOut = false;
                    string addtype = string.Empty;
                    string feeCatagory = string.Empty;
                    if (feecat == "0")
                    {
                        addtype = " Semester";
                        feeCatagory = curSem + addtype;
                    }
                    else
                    {
                        addtype = " Year";
                        if (curSem.Trim() == "1" || curSem.Trim() == "2")
                        {
                            feeCatagory = "1" + addtype;
                        }
                        else if (curSem.Trim() == "3" || curSem.Trim() == "4")
                        {
                            feeCatagory = "2" + addtype;
                        }
                        else if (curSem.Trim() == "5" || curSem.Trim() == "6")
                        {
                            feeCatagory = "3" + addtype;
                        }
                        else if (curSem.Trim() == "7" || curSem.Trim() == "8")
                        {
                            feeCatagory = "4" + addtype;
                        }
                        else if (curSem.Trim() == "9" || curSem.Trim() == "10")
                        {
                            feeCatagory = "5" + addtype;
                        }
                    }
                    feeCatagory = da.GetFunctionv("select TextCode from textvaltable where TextCriteria='Feeca' and college_code='" + ddlcollege.SelectedValue + "' and TextVal='" + feeCatagory + "'");
                    double applFees = 0;
                    double totfee = 0;
                    if (dicManFeesAmt.Count > 0)
                    {
                        if (currentSemester > maxDurations)//change by Rajkumar For Mcc On 13.03.2019
                        {
                            isPassedOut = true;
                        }
                        foreach (KeyValuePair<string, string[]> dicMandFees in dicManFeesAmt)
                        {
                            double costFee = 0;
                            string[] valuePair = new string[3];
                            string key = dicMandFees.Key.Trim().ToLower();
                            valuePair = dicMandFees.Value;
                            string headerFK = (valuePair.Length >= 1) ? valuePair[0] : "";
                            string ledgerFK = (valuePair.Length >= 2) ? valuePair[1] : "";
                            string feeAmt = (valuePair.Length >= 3) ? valuePair[2] : "";
                            double.TryParse(feeAmt.Trim(), out costFee);
                            if (feeAmt.Trim() != "")
                            {
                                feeAmt = " '" + feeAmt + "'";
                            }
                            else
                            {
                                feeAmt = "null";
                            }
                            switch (key)
                            {
                                case "application form":
                                    applFees = costFee;
                                    break;
                                case "semester mark sheet":
                                    break;
                                case "theory":
                                    break;
                                case "practical":
                                    break;
                                case "project":
                                    break;
                                case "field work":
                                    break;
                                case "viva voice":
                                    break;
                                case "disseration":
                                    break;
                                case "consolidate mark sheet":
                                    break;
                                case "course completaion":
                                    break;
                                case "online application fee":
                                    break;
                                case "arrear theory":
                                    break;
                                case "arrear practical":
                                    break;
                                case "central valuation":
                                    break;
                                default:
                                    break;
                            }

                            if (!string.IsNullOrEmpty(key) && valuePair != null && !string.IsNullOrEmpty(applno) && !string.IsNullOrEmpty(headerFK) && !string.IsNullOrEmpty(ledgerFK) && !string.IsNullOrEmpty(feeCatagory) && !string.IsNullOrEmpty(finfkyear) && ledgerFK.ToLower().Trim() != "0" && headerFK.ToLower().Trim() != "0")
                            {
                                if (isPassedOut)
                                {
                                    string insertQuery = " INSERT INTO FT_FeeAllot(AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK,FromGovtAmt, DeductReason) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1,1," + appnofin + "," + ledgerFK + "," + headerFK + "," + feeAmt + "," + feeAmt + "," + feeCatagory + "," + feeAmt + "," + finfkyear + ",0,0) ";

                                    string selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + headerFK + "') and FeeCategory in('" + feeCatagory + "') and  FinYearFK='" + finfkyear + "' and App_No in('" + appnofin + "') ";

                                    string updateQuery = "  update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=1,FeeAmount=FeeAmount+" + feeAmt + ",BalAmount=BalAmount+" + feeAmt + ",TotalAmount=TotalAmount+" + feeAmt + " where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + headerFK + "') and FeeCategory in('" + feeCatagory + "') and  FinYearFK='" + finfkyear + "' and App_No in('" + appnofin + "') ";

                                    string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                                    int insok = d2.update_method_wo_parameter(finalQuery, "Text");

                                    if (insok > 0)
                                    {
                                        double appfee = applFees;
                                        totfee += appfee;
                                        string upquery = " update exam_application set total_fee='" + totfee.ToString() + "' , cost_appl='" + appfee.ToString() + "' where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'";
                                        int ect = d2.update_method_wo_parameter(upquery, "Text");

                                    }
                                }
                                else
                                {
                                    string insertQuery = " INSERT INTO FT_FeeAllot(AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK,FromGovtAmt, DeductReason) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1,1," + appnofin + "," + ledgerFK + "," + headerFK + "," + feeAmt + "," + feeAmt + "," + feeCatagory + "," + feeAmt + "," + finfkyear + ",0,0) ";

                                    string selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + headerFK + "') and FeeCategory in('" + feeCatagory + "') and  FinYearFK='" + finfkyear + "' and App_No in('" + appnofin + "') ";

                                    string updateQuery = "  update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=1,FeeAmount=" + feeAmt + ",BalAmount=" + feeAmt + ",TotalAmount=" + feeAmt + " where LedgerFK in('" + ledgerFK + "') and HeaderFK in('" + headerFK + "') and FeeCategory in('" + feeCatagory + "') and  FinYearFK='" + finfkyear + "' and App_No in('" + appnofin + "') ";

                                    string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                                    int insok = d2.update_method_wo_parameter(finalQuery, "Text");

                                    if (insok > 0)
                                    {
                                        double appfee = applFees;
                                        totfee += appfee;
                                        string upquery = " update exam_application set total_fee='" + totfee.ToString() + "' , cost_appl='" + appfee.ToString() + "' where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'";
                                        int ect = d2.update_method_wo_parameter(upquery, "Text");

                                    }
                                }
                            }
                        }
                    }
                    // }

                    if (settingValue.Tables.Count > 0 && settingValue.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            feeValue = Convert.ToString(settingValue.Tables[0].Rows[0][0]);
                            headerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[0];
                            ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1];
                        }
                        catch { }
                    }
                    if (feeValue != string.Empty && headerCode != string.Empty && ledgerCode != string.Empty)
                    {
                        int tblCount = FpSpreadView.Sheets[0].RowCount;
                        //if (tblCount > 18)
                        //{
                        //    tblCount = 18;
                        //}
                        for (int s = 0; s < FpSpreadView.Sheets[0].RowCount; s++)
                        {
                            int stva = Convert.ToInt32(FpSpreadView.Sheets[0].Cells[s, 7].Value);
                            if (stva == 1)
                            {
                                string subject = Convert.ToString(FpSpreadView.Sheets[0].Cells[s, 5].Tag).Trim();
                                int subsem = Convert.ToInt16(Convert.ToString(FpSpreadView.Sheets[0].Cells[s, 4].Text).Trim());
                                string fees = Convert.ToString(FpSpreadView.Sheets[0].Cells[s, 6].Tag).Trim();
                                if (fees.Trim() != "")
                                {
                                    exmfee = exmfee + Convert.ToDouble(fees);
                                    totalpaperfee = totalpaperfee + Convert.ToDouble(fees);
                                }
                                string attempts = "0";
                                string types = string.Empty;
                                int attval = Convert.ToInt32(sem) - Convert.ToInt32(subsem);
                                if (attval > 0)
                                {
                                    attempts = attval.ToString();
                                    types = "*";
                                }
                                string insupexamsubject = "if exists(select * from exam_appl_details where appl_no='" + applno + "' and subject_no='" + subject + "')";
                                insupexamsubject = insupexamsubject + "  update exam_appl_details set attempts='" + attempts + "',fee='" + fees + "',type='" + types + "',attend='1' where appl_no='" + applno + "' and subject_no='" + subject + "'";
                                insupexamsubject = insupexamsubject + " else insert into exam_appl_details(subject_no,attempts,fee,type,appl_no,attend) values('" + subject + "','" + attempts + "','" + fees + "','" + types + "','" + applno + "','1')";
                                insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                                string isArrear = string.Empty;
                                isArrear = Convert.ToString(FpSpreadView.Sheets[0].Cells[s, 4].Tag).Trim().ToUpper();
                                if (isArrear == "N")
                                {
                                    #region Finance Include

                                    string Amount = Convert.ToString(FpSpreadView.Sheets[0].Cells[s, 6].Tag).Trim();
                                    if (Amount.Trim() != "")
                                    {
                                        Amount = " '" + Amount + "'";
                                    }
                                    else
                                    {
                                        Amount = "null";
                                    }
                                    addtype = string.Empty;
                                    if (feecat == "0")
                                    {
                                        addtype = " Semester";
                                        currentsem = curSem + addtype;
                                    }
                                    else
                                    {
                                        addtype = " Year";
                                        if (curSem.Trim() == "1" || curSem.Trim() == "2")
                                        {
                                            currentsem = "1" + addtype;
                                        }
                                        else if (curSem.Trim() == "3" || curSem.Trim() == "4")
                                        {
                                            currentsem = "2" + addtype;
                                        }
                                        else if (curSem.Trim() == "5" || curSem.Trim() == "6")
                                        {
                                            currentsem = "3" + addtype;
                                        }
                                        else if (curSem.Trim() == "7" || curSem.Trim() == "8")
                                        {
                                            currentsem = "4" + addtype;
                                        }
                                        else if (curSem.Trim() == "9" || curSem.Trim() == "10")
                                        {
                                            currentsem = "5" + addtype;
                                        }
                                    }
                                    string qryPassedOutFinYear = string.Empty;
                                    if (isPassedOut)
                                        qryPassedOutFinYear = " and FinYearFK='" + finfkyear + "' ";
                                    currentsem = da.GetFunctionv("select TextCode from textvaltable where TextCriteria='Feeca' and college_code='" + ddlcollege.SelectedValue + "' and TextVal='" + currentsem + "'");
                                    insupexamsubject = "if exists ( select * from FT_FeeAllot where App_No ='" + appnofin + "' and FeeCategory ='" + currentsem + "' and LedgerFK ='" + ledgerCode + "' and HeaderFK ='" + headerCode + "' " + qryPassedOutFinYear + ")";
                                    insupexamsubject = insupexamsubject + " update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=1,FeeAmount =FeeAmount+" + Amount + ",TotalAmount =TotalAmount+" + Amount + ",BalAmount=BalAmount+" + Amount + " where App_No ='" + appnofin + "' and FeeCategory ='" + currentsem + "' and LedgerFK ='" + ledgerCode + "' and HeaderFK ='" + headerCode + "'  " + qryPassedOutFinYear + " else ";
                                    insupexamsubject = insupexamsubject + " insert into FT_FeeAllot (App_No,FeeCategory,LedgerFK,HeaderFK,FeeAmount,TotalAmount,PaidAmount,BalAmount,FinYearFK,AllotDate,MemType)values ('" + appnofin + "','" + currentsem + "','" + ledgerCode + "','" + headerCode + "'," + Amount + "," + Amount + ",'0'," + Amount + ",'" + finfkyear + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','1')";
                                    insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");

                                    #endregion
                                }
                            }
                        }
                    }
                    exmfee += totfee;
                    string updateexamapplication = "update exam_application set total_fee='" + exmfee + "',fee_amount='" + totalpaperfee + "' where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'";
                    insupdateval = d2.update_method_wo_parameter(updateexamapplication, "text");
                }
                else
                {
                    string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'");
                    string insupexamsubject = "delete from exam_appl_details where appl_no='" + applno + "'";
                    insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                    insupexamsubject = "delete from exam_application where roll_no='" + rollNo + "' and exam_code='" + getexamcode + "'";
                    insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                }
            }
            else
            {
                //string applno = d2.GetFunction("select appl_no from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'");
                //string insupexamsubject = "delete from exam_appl_details where appl_no='" + applno + "'";
                //insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
                //insupexamsubject = "delete from exam_application where roll_no='" + rollno + "' and exam_code='" + getexamcode + "'";
                //insupdateval = d2.update_method_wo_parameter(insupexamsubject, "text");
            }
            //}
            divViewSubjects.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Save Sucessfully.')", true);
            loadexamdetails();
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");

        }
    }

    public void BindSessionHourMinites()
    {
        try
        {
            ddlFHr.Items.Clear();
            ddlTHr.Items.Clear();
            ddlFMin.Items.Clear();
            ddlTMin.Items.Clear();
            for (int hr = 0; hr <= 12; hr++)
            {
                ddlFHr.Items.Insert(hr, new ListItem(Convert.ToString(hr).PadLeft(2, '0'), Convert.ToString(hr).PadLeft(2, '0')));
                ddlTHr.Items.Insert(hr, new ListItem(Convert.ToString(hr).PadLeft(2, '0'), Convert.ToString(hr).PadLeft(2, '0')));
            }
            for (int min = 0; min <= 60; min++)
            {
                ddlFMin.Items.Insert(min, new ListItem(Convert.ToString(min).PadLeft(2, '0'), Convert.ToString(min).PadLeft(2, '0')));
                ddlTMin.Items.Insert(min, new ListItem(Convert.ToString(min).PadLeft(2, '0'), Convert.ToString(min).PadLeft(2, '0')));
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void rblSessions_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string session = string.Empty;
            string qry = string.Empty;
            if (rblSessions.Items.Count > 0)
            {
                if (rblSessions.SelectedValue.Trim() == "0")
                {
                    qry = "select value from COE_Master_Settings where settings='Fore Noon'";
                    session = d2.GetFunctionv(qry);
                }
                else if (rblSessions.SelectedValue.Trim() == "1")
                {
                    qry = "select value from COE_Master_Settings where settings='After Noon'";
                    session = d2.GetFunctionv(qry);
                }
                if (!string.IsNullOrEmpty(session.Trim()))
                {
                    string[] sessionValue = session.Split(new char[] { '-' });
                    if (sessionValue.Length > 0)
                    {
                        string FromValue = (sessionValue[0].Split(' ').Length > 0) ? sessionValue[0].Split(' ')[0] : "";
                        string FromSess = (sessionValue[0].Split(' ').Length > 1) ? sessionValue[0].Split(' ')[1] : "";
                        string ToValue = (sessionValue[1].Trim().Split(' ').Length > 0) ? sessionValue[1].Trim().Split(' ')[0] : "";
                        string ToSess = (sessionValue[1].Trim().Split(' ').Length > 1) ? sessionValue[1].Trim().Split(' ')[1] : "";
                        string FromHr = (FromValue.Split('.').Length > 0) ? FromValue.Split('.')[0] : "";
                        string FromMin = (FromValue.Split('.').Length > 1) ? FromValue.Split('.')[1] : "";
                        string ToHr = (ToValue.Split('.').Length > 0) ? ToValue.Split('.')[0] : "";
                        string ToMin = (ToValue.Split('.').Length > 1) ? ToValue.Split('.')[1] : "";
                        int value = 0;
                        if (ddlFHr.Items.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(FromHr.Trim()))
                            {
                                int.TryParse(FromHr.Trim(), out value);
                                ddlFHr.SelectedValue = Convert.ToString(value).PadLeft(2, '0');
                            }
                        }
                        if (ddlTHr.Items.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(ToHr.Trim()))
                            {
                                int.TryParse(ToHr.Trim(), out value);
                                ddlTHr.SelectedValue = Convert.ToString(value).PadLeft(2, '0');
                            }
                        }
                        if (ddlFMin.Items.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(FromMin.Trim()))
                            {
                                int.TryParse(FromMin.Trim(), out value);
                                ddlFMin.SelectedValue = Convert.ToString(value).PadLeft(2, '0');
                            }
                        }
                        if (ddlTMin.Items.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(ToMin.Trim()))
                            {
                                int.TryParse(ToMin.Trim(), out value);
                                ddlTMin.SelectedValue = Convert.ToString(value).PadLeft(2, '0');
                            }
                        }
                        if (FromSess.Trim().ToLower() == "am")
                        {
                            ddlFSess.SelectedValue = "0";
                        }
                        else if (FromSess.Trim().ToLower() == "pm")
                        {
                            ddlFSess.SelectedValue = "1";
                        }
                        if (ToSess.Trim().ToLower() == "am")
                        {
                            ddlTSession.SelectedValue = "0";
                        }
                        else if (ToSess.Trim().ToLower() == "pm")
                        {
                            ddlTSession.SelectedValue = "1";
                        }
                    }
                }
                else
                {
                    if (ddlFHr.Items.Count > 0)
                    {
                        ddlFHr.SelectedIndex = 0;
                    }
                    if (ddlFMin.Items.Count > 0)
                    {
                        ddlFMin.SelectedIndex = 0;
                    }
                    if (ddlFSess.Items.Count > 0)
                    {
                        ddlFSess.SelectedIndex = 0;
                    }
                    if (ddlTHr.Items.Count > 0)
                    {
                        ddlTHr.SelectedIndex = 0;
                    }
                    if (ddlTMin.Items.Count > 0)
                    {
                        ddlTMin.SelectedIndex = 0;
                    }
                    if (ddlTSession.Items.Count > 0)
                    {
                        ddlTSession.SelectedIndex = 0;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnExamSessionSettings_Click(object sender, EventArgs e)
    {
        try
        {
            rblSessions.SelectedIndex = 0;
            rblSessions_SelectedIndexChanged(sender, e);
            divSessionSettings.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnSesstionSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (rblSessions.Items.Count > 0)
            {
                qry = string.Empty;
                string fromHr = string.Empty;
                string fromMin = string.Empty;
                string fromSession = string.Empty;
                string fromValue = string.Empty;
                int res = 0;
                string toHr = string.Empty;
                string tomin = string.Empty;
                string toSession = string.Empty;
                string toValue = string.Empty;
                string sessionValue = string.Empty;
                string searchvalue = string.Empty;
                if (ddlFHr.Items.Count > 0)
                {
                    fromHr = Convert.ToString(ddlFHr.SelectedItem.Text).Trim();
                }
                if (ddlFMin.Items.Count > 0)
                {
                    fromMin = Convert.ToString(ddlFMin.SelectedItem.Text).Trim();
                }
                if (ddlFSess.Items.Count > 0)
                {
                    fromSession = Convert.ToString(ddlFSess.SelectedItem.Text).Trim();
                }
                if (!string.IsNullOrEmpty(fromHr.Trim()) && !string.IsNullOrEmpty(fromMin.Trim()) && !string.IsNullOrEmpty(fromSession.Trim()))
                {
                    fromValue = fromHr.Trim() + "." + fromMin.Trim() + " " + fromSession.Trim().ToUpper();
                }
                if (ddlTHr.Items.Count > 0)
                {
                    toHr = Convert.ToString(ddlTHr.SelectedItem.Text).Trim();
                }
                if (ddlTMin.Items.Count > 0)
                {
                    tomin = Convert.ToString(ddlTMin.SelectedItem.Text).Trim();
                }
                if (ddlTSession.Items.Count > 0)
                {
                    toSession = Convert.ToString(ddlTSession.SelectedItem.Text).Trim();
                }
                if (!string.IsNullOrEmpty(toHr.Trim()) && !string.IsNullOrEmpty(tomin.Trim()) && !string.IsNullOrEmpty(toSession.Trim()))
                {
                    toValue = toHr.Trim() + "." + tomin.Trim() + " " + toSession.Trim().ToUpper();
                }
                if (!string.IsNullOrEmpty(fromSession.Trim()) && !string.IsNullOrEmpty(toSession.Trim()))
                {
                    sessionValue = fromValue.Trim() + " - " + toValue.Trim();
                }
                if (Convert.ToString(rblSessions.SelectedValue.Trim()) == "0")
                {
                    searchvalue = "Fore Noon";
                }
                else if (Convert.ToString(rblSessions.SelectedValue).Trim() == "1")
                {
                    searchvalue = "After Noon";
                }
                if (!string.IsNullOrEmpty(sessionValue.Trim()))
                {
                    qry = "if exists (select * from COE_Master_Settings where settings='" + searchvalue.Trim() + "') update COE_Master_Settings set value='" + sessionValue.Trim() + "' where settings='" + searchvalue.Trim() + "' else insert into COE_Master_Settings (settings,value) values ('" + searchvalue.Trim() + "','" + sessionValue.Trim() + "') ";
                    res = d2.update_method_wo_parameter(qry, "Text");
                }
                if (res > 0)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved Successfully!!!";
                    return;
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Not Saved!!!";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnSessionExit_Click(object sender, EventArgs e)
    {
        try
        {
            rblSessions.SelectedIndex = 0;
            rblSessions_SelectedIndexChanged(sender, e);
            divSessionSettings.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void ExamApplicationFormat1()
    {
        try
        {
            #region Format1
            FpSpread1.SaveChanges();
            if (ddlYear.SelectedValue.ToString() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            int insupdateval = 0;
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string sem = ddlsem.SelectedValue.ToString();
            if (chkpassout.Checked == true)
            {
                int semval = ddlsem.Items.Count;
                semval++;
                sem = semval.ToString();
            }
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            string appldate = txtappldate.Text.ToString();
            string[] spd = appldate.Split('/');
            DateTime dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            string lastappladet = txtappllastdate.Text.ToString(); spd = appldate.Split('/');
            spd = lastappladet.Split('/');
            DateTime dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            if (dtappl > dtlastappl)
            {
                lblerror.Visible = true;
                lblerror.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            string applfee = txtapplfee.Text.ToString();
            string smtfee = txtstmtfee.Text.ToString();
            string extrafee = txtextrafee.Text.ToString();
            string fineamount = txtfine.Text.ToString();
            double appltotalcost = 0;
            double applcost = 0;
            if (applfee.Trim() != "")
            {
                applcost = Convert.ToDouble(applfee);
            }
            double stmtcost = 0;
            if (smtfee.Trim() != "")
            {
                stmtcost = Convert.ToDouble(smtfee);
            }
            double extracost = 0;
            if (extrafee.Trim() != "")
            {
                extracost = Convert.ToDouble(extrafee);
            }
            double fineamo = 0;
            if (fineamount.Trim() != "")
            {
                fineamo = Convert.ToDouble(fineamount);
            }
            appltotalcost = applcost + stmtcost + extracost + fineamo;
            Boolean setflag = false;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    setflag = true;
                }
            }
            if (setflag == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Student And Then Proceed";
                return;
            }
            Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
            Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
            Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
            Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
            Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
            string coename = string.Empty;
            string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = string.Empty;
            string aff = string.Empty;
            string collacr = string.Empty;
            string dispin = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                string[] strpa = aff.Split(',');
                aff = strpa[0];
                coename = ds.Tables[0].Rows[0]["coe"].ToString();
                collacr = ds.Tables[0].Rows[0]["acr"].ToString();
                dispin = ds.Tables[0].Rows[0]["districtpin"].ToString();
            }
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            DataSet printds = new DataSet();
            string studinfo = "select sy.semester,r.app_no, r.Current_Semester, r.batch_year,exam_month,exam_year,stud_name,r.degree_code,subject_code,subject_name,isnull(total_fee,0) as total_fee,convert(decimal(5,0),ROUND(fee,0)) as fee,isnull(ea.extra_fee,0) as extra_fee,ead.attempts,ea.roll_no,isnull(ea.fee_amount,0) fee_amount,isnull(ea.fine,0) fine,isnull(ea.cost_appl,0) cost_appl,isnull(cost_mark,0) as cost_mark,case when r.current_semester=sy.semester then '0' else '1' end as Paper from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy,registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ss.syll_code=sy.syll_code and r.roll_no=ea.roll_no  and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "' order by sy.semester desc";
            studinfo = studinfo + " select dd.dept_name,c.course_name,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id ";
            printds.Clear();
            printds = da.select_method_wo_parameter(studinfo, "Text");
            DataView dv = new DataView();
            string semroman = string.Empty;
            Gios.Pdf.PdfPage mypdfpage;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString().Trim();
                string regnnono = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString().Trim();
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    //printds.Tables[1].DefaultView.RowFilter = "degree_code='" + ddlbranch.SelectedItem.Value.ToString() + "'";
                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    dv = printds.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        mypdfpage = mydoc.NewPage();
                        string studnmae = dv[0]["stud_name"].ToString();
                        string seminfo = dv[0]["Current_Semester"].ToString();
                        string stdappno = dv[0]["app_no"].ToString();
                        string degreecodee = dv[0]["degree_code"].ToString();
                        PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
                        PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        mypdfpage.Add(pr1);
                        int coltop = 25;
                        PdfTextArea ptc;
                        #region Student Photo
                        string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                        MemoryStream memoryStream = new MemoryStream();
                        DataSet dsstdpho = new DataSet();
                        dsstdpho.Clear();
                        dsstdpho.Dispose();
                        dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                        if (dsstdpho.Tables[0].Rows.Count > 0)
                        {
                            byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                            memoryStream.Write(file, 0, file.Length);
                            if (file.Length > 0)
                            {
                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                                {
                                }
                                else
                                {
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    //NetworkShare.SaveOnNetworkShare(thumb, HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), ImageFormat.Jpeg);
                                }
                            }
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                        {
                            Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"));
                            mypdfpage.Add(LogoImage2, 485, 30, 300);
                        }
                        else
                        {
                            Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                            mypdfpage.Add(LogoImage2, 485, 30, 300);
                        }
                        #endregion
                        #region TOP DETAILS
                        if (chkheadimage.Checked == false)
                        {
                            #region Left Logo
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 400);
                            }
                            #endregion
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
                            mypdfpage.Add(ptc);
                            //coltop = coltop + 20;
                            //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, collacr);
                            //mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, dispin);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(font2small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
                            mypdfpage.Add(ptc);
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                            ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 30), System.Drawing.ContentAlignment.TopCenter, "APPLICATION FOR SEMESTER EXAMINATIONS " + strMonthName.ToUpper() + " - " + examyear + "");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                        }
                        else
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 20, 25, 410);
                            }
                            coltop = coltop + 70;
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                            ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 105, 595, 30), System.Drawing.ContentAlignment.TopCenter, "APPLICATION FOR SEMESTER EXAMINATIONS " + strMonthName.ToUpper() + " - " + examyear + "");
                            mypdfpage.Add(ptc);
                        }
                        PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
                        PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        #endregion
                        #region studinfo
                        coltop = coltop + 15;
                        if (seminfo == "1")
                        {
                            semroman = "I";
                        }
                        else if (seminfo == "2")
                        {
                            semroman = "II";
                        }
                        else if (seminfo == "3")
                        {
                            semroman = "III";
                        }
                        else if (seminfo == "4")
                        {
                            semroman = "IV";
                        }
                        else if (seminfo == "5")
                        {
                            semroman = "V";
                        }
                        else if (seminfo == "6")
                        {
                            semroman = "VI";
                        }
                        else if (seminfo == "7")
                        {
                            semroman = "VII";
                        }
                        else if (seminfo == "8")
                        {
                            semroman = "VIII";
                        }
                        Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(font3small, 5, 6, 5);
                        table1forpage2.Columns[0].SetWidth(120);
                        table1forpage2.Columns[1].SetWidth(10);
                        table1forpage2.Columns[2].SetWidth(130);
                        table1forpage2.Columns[3].SetWidth(100);
                        table1forpage2.Columns[4].SetWidth(10);
                        table1forpage2.Columns[5].SetWidth(100);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 0).SetContent("Register Number & Semester ");
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 1).SetContent(":");
                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 2).SetContent(regnnono.ToUpper() + " & " + semroman);
                        table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 3).SetContent("Father's Name(Tamil)");
                        table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 4).SetContent(":");
                        table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 5).SetContent(" ");
                        table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(1, 0).SetContent("Student's Name(English)");
                        table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 1).SetContent(":");
                        table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(1, 2).SetContent(studnmae);
                        table1forpage2.Cell(1, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(1, 3).SetContent("Address");
                        table1forpage2.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 4).SetContent(":");
                        table1forpage2.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 5).SetContent(" ");
                        table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 0).SetContent("Student's Name(Tamil)");
                        table1forpage2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(2, 1).SetContent(":");
                        table1forpage2.Cell(2, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 2).SetContent("");
                        table1forpage2.Cell(2, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 3).SetContent("");
                        table1forpage2.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(2, 4).SetContent("");
                        table1forpage2.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(2, 5).SetContent(" ");
                        table1forpage2.Cell(3, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(3, 0).SetContent("Date of Birth");
                        table1forpage2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(3, 1).SetContent(":");
                        string dob = da.GetFunction("select convert(varchar,dob,103) dob from applyn  where app_no='" + stdappno + "'");
                        table1forpage2.Cell(3, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(3, 2).SetContent(dob);
                        table1forpage2.Cell(3, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(3, 3).SetContent("Mobile Number");
                        table1forpage2.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(3, 4).SetContent(":");
                        table1forpage2.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(3, 5).SetContent(" ");
                        table1forpage2.Cell(4, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(4, 0).SetContent("Degree & Branch");
                        table1forpage2.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(4, 1).SetContent(":");
                        string degreebranch = da.GetFunction("select (c.course_name + ' - '+dd.dept_name) as degree,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id  and degree_code='" + degreecodee + "'");
                        table1forpage2.Cell(4, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(4, 2).SetContent(degreebranch);
                        table1forpage2.Cell(4, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(4, 3).SetContent("Email ID");
                        table1forpage2.Cell(4, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(4, 4).SetContent(":");
                        table1forpage2.Cell(4, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(4, 5).SetContent(" ");
                        Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 121, 553, 200));//sr
                        mypdfpage.Add(newpdftabpage2);
                        coltop = coltop + 85;
                        tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        #endregion
                        table1forpage2 = mydoc.NewTable(font4small, dv.Count + 1, 8, 4);
                        table1forpage2.Columns[0].SetWidth(20);
                        table1forpage2.Columns[1].SetWidth(50);
                        table1forpage2.Columns[2].SetWidth(130);
                        table1forpage2.Columns[3].SetWidth(30);
                        table1forpage2.Columns[4].SetWidth(20);
                        table1forpage2.Columns[5].SetWidth(50);
                        table1forpage2.Columns[6].SetWidth(130);
                        table1forpage2.Columns[7].SetWidth(30);
                        mypdfpage.Add(newpdftabpage2);
                        coltop = coltop + 85;
                        tlinerect = new PdfArea(mydoc, 15, 240, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 40, 220, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 100, 220, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 255, 220, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 290, 220, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 315, 220, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 375, 220, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 532, 220, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 532, 220, 0.01, 400);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 15, 620, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 0).SetContent("Sem");
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 1).SetContent("SubCode");
                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 2).SetContent("Subject Title");
                        table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 3).SetContent("(Rs)");
                        table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 4).SetContent("Sem");
                        table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 5).SetContent("SubCode");
                        table1forpage2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 6).SetContent("Subject Title");
                        table1forpage2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 7).SetContent("(Rs)");
                        int j = 0;
                        for (int i = 0; i < dv.Count; i++)
                        {
                            if (i < 20)
                            {
                                table1forpage2.Cell(i + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(i + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(i + 1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage2.Cell(i + 1, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                if (i == 1)
                                {
                                    table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                    table1forpage2.Cell(i + 1, 3).SetContent(dv[i]["fee"].ToString() + " ");
                                }
                                else
                                {
                                    table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                    table1forpage2.Cell(i + 1, 3).SetContent(dv[i]["fee"].ToString() + " ");
                                }
                            }
                            else
                            {
                                j++;
                                table1forpage2.Cell(j + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(j + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(j + 1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage2.Cell(j + 1, 7).SetContentAlignment(ContentAlignment.MiddleRight);
                                if (j == 1)
                                {
                                    table1forpage2.Cell(j + 1, 4).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(j + 1, 5).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(j + 1, 6).SetContent(dv[i]["subject_name"].ToString());
                                    table1forpage2.Cell(j + 1, 7).SetContent(dv[i]["fee"].ToString());
                                }
                                else
                                {
                                    table1forpage2.Cell(j + 1, 4).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(j + 1, 5).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(j + 1, 6).SetContent(dv[i]["subject_name"].ToString());
                                    table1forpage2.Cell(j + 1, 7).SetContent(dv[i]["fee"].ToString());
                                }
                            }
                        }
                        newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 223, 553, 500));
                        mypdfpage.Add(newpdftabpage2);
                        tlinerect = new PdfArea(mydoc, 15, 710, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        string subjextsregister = "No. of Subject(s) Registered : " + dv.Count + "";
                        table1forpage2 = mydoc.NewTable(font3small, 4, 3, 3);
                        table1forpage2.Columns[0].SetWidth(120);
                        table1forpage2.Columns[1].SetWidth(10);
                        table1forpage2.Columns[2].SetWidth(130);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 0).SetContent(subjextsregister);
                        double ttol = Convert.ToDouble(dv[0]["total_fee"].ToString());
                        table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(1, 0).SetContent("Exam Fees (Rs)");
                        table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 1).SetContent(":");
                        table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        string data = String.Format("{0:0.00}", Convert.ToDouble(dv[0]["fee_amount"].ToString()));
                        table1forpage2.Cell(1, 2).SetContent(data);
                        table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(2, 0).SetContent("Others Fees (Rs)");
                        table1forpage2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(2, 1).SetContent(":");
                        double otherfee = Convert.ToDouble(dv[0]["extra_fee"].ToString()) + Convert.ToDouble(dv[0]["fine"].ToString()) + Convert.ToDouble(dv[0]["cost_mark"].ToString()) + Convert.ToDouble(dv[0]["cost_appl"].ToString());
                        data = String.Format("{0:0.00}", otherfee);
                        table1forpage2.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(2, 2).SetContent(data);
                        table1forpage2.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(3, 0).SetContent("Total Fees (Rs)");
                        data = String.Format("{0:0.00}", ttol);
                        table1forpage2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(3, 1).SetContent(":");
                        table1forpage2.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(3, 2).SetContent(data);
                        foreach (PdfCell pr in table1forpage2.CellRange(0, 0, 0, 0).Cells)
                        {
                            pr.ColSpan = 3;
                        }
                        newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 230, 625, 200, 500));
                        mypdfpage.Add(newpdftabpage2);
                        coltop = 715;
                        PdfTextArea ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 200, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "FN - FORENOON 10.00 AM - 1.00 PM");
                        mypdfpage.Add(ptcfn);
                        coltop = coltop + 15;
                        PdfTextArea ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 200, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "AN - AFTERNOON 2.00 PM - 5.00 PM");
                        mypdfpage.Add(ptcan);
                        coltop = 740;
                        PdfTextArea ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop - 55, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Clerk with date");
                        mypdfpage.Add(ptcstisign);
                        ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 430, coltop - 55, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                        mypdfpage.Add(ptcstisign);
                        PdfTextArea ptccoename = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 380, coltop + 15, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, coename);
                        mypdfpage.Add(ptccoename);
                        PdfTextArea ptccontroller = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 380, coltop + 30, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Controller of Examinations");
                        mypdfpage.Add(ptccontroller);
                        coltop = coltop + 15;
                        PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                        mypdfpage.Add(ptcsnote);
                        coltop = coltop + 15;
                        PdfTextArea ptcsnote1 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1. Detained students are NOT ELIGIBLE to write their current papers");
                        mypdfpage.Add(ptcsnote1);
                        coltop = coltop + 15;
                        PdfTextArea ptcsnote2 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. If anydiscrepancies are found in the form, report to the C.O.E office immediately");
                        mypdfpage.Add(ptcsnote2);
                        mypdfpage.SaveToDocument();
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ExamApplication" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
            #endregion Format1
        }
        catch (Exception ex)
        {
        }
    }

    private void getAllRedoStudentsSemester(out Dictionary<string, List<string>> dicAppNoRedoSemester, out Dictionary<string, List<string>> dicRollNoRedoSemester, string batchYear = null, string degreeCode = null)
    {
        dicAppNoRedoSemester = new Dictionary<string, List<string>>();
        dicRollNoRedoSemester = new Dictionary<string, List<string>>();
        DataSet dsRedoSemester = new DataSet();
        try
        {
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode))
            {
                qry = "select el.batch_year,el.degree_code,el.Semester,el.is_eligible,el.app_no,el.Roll_no,ISNULL(el.fine_amt,'0') as fine_amt,ISNULL(el.isCondonationFee,'0') isCondonationFee from Eligibility_list el where is_eligible='3' and batch_year='" + batchYear + "' and degree_code='" + degreeCode + "' union select el.batch_year,el.degree_code,el.Semester,el.is_eligible,el.app_no,el.Roll_no,ISNULL(el.fine_amt,'0') as fine_amt,ISNULL(el.isCondonationFee,'0') isCondonationFee from Eligibility_list el,Registration r where r.Batch_Year=el.batch_year and el.degree_code=r.degree_code and r.Current_Semester=el.Semester and r.App_No=el.app_no and is_eligible='4' and el.batch_year='" + batchYear + "' and el.degree_code='" + degreeCode + "'";
                dsRedoSemester = da.select_method_wo_parameter(qry, "text");
            }
            else
            {
                qry = "select el.batch_year,el.degree_code,el.Semester,el.is_eligible,el.app_no,el.Roll_no,ISNULL(el.fine_amt,'0') as fine_amt,ISNULL(el.isCondonationFee,'0') isCondonationFee from Eligibility_list el where is_eligible='3' union select el.batch_year,el.degree_code,el.Semester,el.is_eligible,el.app_no,el.Roll_no,ISNULL(el.fine_amt,'0') as fine_amt,ISNULL(el.isCondonationFee,'0') isCondonationFee from Eligibility_list el,Registration r where r.Batch_Year=el.batch_year and el.degree_code=r.degree_code and r.Current_Semester=el.Semester and r.App_No=el.app_no and is_eligible='4'";
                dsRedoSemester = da.select_method_wo_parameter(qry, "text");
            }
            if (dsRedoSemester.Tables.Count > 0 && dsRedoSemester.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drRedoSem in dsRedoSemester.Tables[0].Rows)
                {
                    string appNo = Convert.ToString(drRedoSem["app_no"]).Trim();
                    string rollNo = Convert.ToString(drRedoSem["Roll_no"]).Trim();
                    string semester = Convert.ToString(drRedoSem["Semester"]).Trim();
                    List<string> lstAppNo = new List<string>();
                    List<string> lstRollNo = new List<string>();
                    if (!string.IsNullOrEmpty(appNo.Trim()))
                    {
                        if (!dicAppNoRedoSemester.ContainsKey(appNo.Trim()))
                        {
                            lstAppNo.Add(semester);
                            dicAppNoRedoSemester.Add(appNo.Trim(), lstAppNo);
                        }
                        else
                        {
                            lstAppNo = dicAppNoRedoSemester[appNo.Trim()];
                            lstAppNo.Add(semester);
                            dicAppNoRedoSemester[appNo.Trim()] = lstAppNo;
                        }
                    }
                    if (!string.IsNullOrEmpty(rollNo.Trim()))
                    {
                        if (!dicRollNoRedoSemester.ContainsKey(rollNo.Trim()))
                        {
                            lstRollNo.Add(semester);
                            dicRollNoRedoSemester.Add(rollNo.Trim(), lstRollNo);
                        }
                        else
                        {
                            lstRollNo = dicRollNoRedoSemester[rollNo.Trim()];
                            lstRollNo.Add(semester);
                            dicRollNoRedoSemester[rollNo.Trim()] = lstRollNo;
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
        }
    }


    //private void MandatoryFees(out Dictionary<string,double> dicMandFees)
    //{
    //    dicMandFees = new Dictionary<string, double>();
    //    dicMandFees.Clear();
    //    DataTable dtMandFee = new DataTable();
    //    dtMandFee.Columns.Add("Application Form");
    //    dtMandFee.Columns.Add("Semester Mark Sheet");
    //    dtMandFee.Columns.Add("Theory");
    //    dtMandFee.Columns.Add("Practical");
    //    dtMandFee.Columns.Add("Project");
    //    dtMandFee.Columns.Add("Field Work");
    //    dtMandFee.Columns.Add("Viva Voice");
    //    dtMandFee.Columns.Add("Disseration");
    //    dtMandFee.Columns.Add("Consolidate Mark Sheet");
    //    dtMandFee.Columns.Add("Course Completaion");
    //    dtMandFee.Columns.Add("Online Application Fee");
    //    dtMandFee.Columns.Add("Arrear Theory");
    //    dtMandFee.Columns.Add("Arrear Practical");
    //    dtMandFee.Columns.Add("Central Valuation");
    //    dtMandFee.Rows.Add("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
    //    for (int dCol = 0; dCol < dtMandFee.Columns.Count; dCol++)
    //    {
    //        string linkVal = Convert.ToString(dtMandFee.Columns[dCol].ColumnName.Trim()) + "@#MandatoryFee";
    //        byte prevVal = Convert.ToByte(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'").Trim());
    //        if (prevVal == 1)
    //        {
    //            dtMandFee.Rows[0][dCol] = prevVal;
    //        }
    //    }
    //    ArrayList arrMandFees = new ArrayList();
    //    for (int dRow = 0; dRow < dtMandFee.Columns.Count; dRow++)
    //    {
    //        string colName = Convert.ToString(dtMandFee.Columns[dRow].ColumnName).Trim();
    //        if (Convert.ToString(dtMandFee.Rows[0][colName]) == "1")
    //            arrMandFees.Add(colName);
    //    }
    //    //return arrMandFees;
    //}

    /// <summary>
    /// Developed By Malang Raja T On Mar 27 2017
    /// </summary>
    /// <param name="dicMandFees"></param>
    private void MandatoryFeesValues(out Dictionary<string, string[]> dicMandFees)
    {
        ArrayList arrHeaderFk;
        ArrayList arrLedgerFk;
        dicMandFees = new Dictionary<string, string[]>();
        dicMandFees.Clear();
        DataTable dtMandFee = new DataTable();
        dtMandFee.Columns.Add("Application Form");
        dtMandFee.Columns.Add("Semester Mark Sheet");
        dtMandFee.Columns.Add("Theory");
        dtMandFee.Columns.Add("Practical");
        dtMandFee.Columns.Add("Project");
        dtMandFee.Columns.Add("Field Work");
        dtMandFee.Columns.Add("Viva Voice");
        dtMandFee.Columns.Add("Disseration");
        dtMandFee.Columns.Add("Consolidate Mark Sheet");
        dtMandFee.Columns.Add("Course Completaion");
        dtMandFee.Columns.Add("Online Application Fee");
        dtMandFee.Columns.Add("Arrear Theory");
        dtMandFee.Columns.Add("Arrear Practical");
        dtMandFee.Columns.Add("Central Valuation");
        dtMandFee.Columns.Add("Syllabi & Curricular");
        dtMandFee.Rows.Add("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        ArrayList arrMandFees = new ArrayList();
        arrHeaderFk = new ArrayList();
        arrLedgerFk = new ArrayList();
        string[] FeeCodeHFkLFkFees = new string[3];
        for (int dCol = 0; dCol < dtMandFee.Columns.Count; dCol++)
        {
            string linkVal = Convert.ToString(dtMandFee.Columns[dCol].ColumnName.Trim()) + "@#MandatoryFee";
            byte prevVal = 0;
            string linkValNew = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'").Trim();
            byte.TryParse(linkValNew.Trim(), out prevVal);
            if (prevVal == 1)
            {
                dtMandFee.Rows[0][dCol] = prevVal;
            }
        }
        for (int dRow = 0; dRow < dtMandFee.Columns.Count; dRow++)
        {
            string colName = Convert.ToString(dtMandFee.Columns[dRow].ColumnName).Trim();
            if (Convert.ToString(dtMandFee.Rows[0][colName]).Trim() == "1")
            {
                arrMandFees.Add(colName);
                DataSet settingValue = d2.select_method_wo_parameter(" select settings,value from Master_Settings where settings='" + colName + "' --and usercode='" + Session["usercode"].ToString() + "'", "Text");
                string feeValue = string.Empty;
                string headerCode = string.Empty;
                string ledgerCode = string.Empty;
                string FeeAmt = string.Empty;
                if (settingValue.Tables.Count > 0 && settingValue.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        string[] ValuesList = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';');
                        feeValue = Convert.ToString(settingValue.Tables[0].Rows[0][0]);
                        headerCode = (ValuesList.Length >= 1) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[0] : "0";
                        ledgerCode = (ValuesList.Length >= 2) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1] : "0";
                        FeeAmt = (ValuesList.Length >= 3) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[2] : "0";
                        FeeCodeHFkLFkFees = new string[3];
                        FeeCodeHFkLFkFees[0] = headerCode;
                        FeeCodeHFkLFkFees[1] = ledgerCode;
                        FeeCodeHFkLFkFees[2] = FeeAmt;
                        if (!dicMandFees.ContainsKey(colName.ToLower().Trim()))
                        {
                            dicMandFees.Add(colName.ToLower().Trim(), FeeCodeHFkLFkFees);
                        }
                        if (!arrHeaderFk.Contains(headerCode))
                        {
                            arrHeaderFk.Add(headerCode);
                        }
                        if (!arrLedgerFk.Contains(ledgerCode))
                        {
                            arrLedgerFk.Add(ledgerCode);
                        }
                    }
                    catch { }
                }
            }
        }
        //return arrMandFees;
    }


    #region Format 5 for SNS

    public void ApplicationFormat5()
    {
        try
        {
            #region Format5 for SNS College

            FpSpread1.SaveChanges();
            if (Convert.ToString(ddlYear.SelectedValue).Trim() == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((Convert.ToString(ddlMonth.SelectedValue).Trim() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            string degreecode = Convert.ToString(ddlbranch.SelectedValue).Trim();
            string batchyear = Convert.ToString(ddlbatch.SelectedValue).Trim();
            string sem = Convert.ToString(ddlsem.SelectedValue).Trim();
            if (chkpassout.Checked == true)
            {
                int semval = ddlsem.Items.Count;
                semval++;
                sem = semval.ToString();
            }
            string exammonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            string examyear = Convert.ToString(ddlYear.SelectedValue).Trim();
            collegecode = Convert.ToString(ddlcollege.SelectedValue).Trim();
            string appldate = Convert.ToString(txtappldate.Text).Trim();
            string[] spd = appldate.Split('/');
            DateTime dtappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            string lastappladet = txtappllastdate.Text.ToString(); spd = appldate.Split('/');
            spd = lastappladet.Split('/');
            DateTime dtlastappl = Convert.ToDateTime(spd[1] + '/' + spd[0] + '/' + spd[2]);
            qry = "select value from COE_Master_Settings where settings='Fore Noon'";
            string foreNoon = d2.GetFunctionv(qry);
            qry = "select value from COE_Master_Settings where settings='After Noon'";
            string afterNoon = d2.GetFunctionv(qry);
            if (dtappl > dtlastappl)
            {
                lblerror.Visible = true;
                lblerror.Text = "Exam Application Date Must Be Less Than Last Exam Application Date";
                return;
            }
            string applfee = Convert.ToString(txtapplfee.Text).Trim().Trim();
            string smtfee = Convert.ToString(txtstmtfee.Text).Trim();
            string extrafee = Convert.ToString(txtextrafee.Text).Trim();
            string fineamount = Convert.ToString(txtfine.Text).Trim();
            int theoryCount = 0;
            int Practicalcount = 0;
            double appltotalcost = 0;
            double applcost = 0;
            if (applfee.Trim() != "")
            {
                applcost = Convert.ToDouble(applfee);
            }
            double stmtcost = 0;
            if (smtfee.Trim() != "")
            {
                stmtcost = Convert.ToDouble(smtfee);
            }
            double extracost = 0;
            if (extrafee.Trim() != "")
            {
                extracost = Convert.ToDouble(extrafee);
            }
            double fineamo = 0;
            if (fineamount.Trim() != "")
            {
                fineamo = Convert.ToDouble(fineamount);
            }
            appltotalcost = applcost + stmtcost + extracost + fineamo;
            bool setflag = false;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    setflag = true;
                }
            }
            if (setflag == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Student And Then Proceed";
                return;
            }
            Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
            Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
            Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
            Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
            Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
            Font font5bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font5small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font6small = new Font("Palatino Linotype", 6, FontStyle.Regular);
            string coename = string.Empty;
            string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = string.Empty;
            string aff = string.Empty;
            string collacr = string.Empty;
            string dispin = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                string[] strpa = aff.Split(',');
                aff = strpa[0];
                coename = ds.Tables[0].Rows[0]["coe"].ToString();
                collacr = ds.Tables[0].Rows[0]["acr"].ToString();
                dispin = ds.Tables[0].Rows[0]["districtpin"].ToString();
            }
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            DataSet printds = new DataSet();
            string studinfo = "select sy.semester,r.app_no, r.Current_Semester, r.batch_year,exam_month,exam_year,r.stud_name,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp)  end as countryp,r.degree_code,subject_code,subject_name,isnull(total_fee,0) as total_fee,convert(decimal(5,0),ROUND(fee,0)) as fee,isnull(ea.extra_fee,0) as extra_fee,ead.attempts,ea.roll_no,isnull(ea.fee_amount,0) fee_amount,isnull(ea.fine,0) fine,isnull(ea.cost_appl,0) cost_appl,isnull(cost_mark,0) as cost_mark,case when r.current_semester=sy.semester then '0' else '1' end as Paper,ss.Lab,ss.subType_no,ss.subject_type,ss.promote_count from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy,registration r,applyn a where a.app_no=r.App_No and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ss.syll_code=sy.syll_code and r.roll_no=ea.roll_no  and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "' order by r.app_no,sy.semester desc";
            studinfo = studinfo + " select dd.dept_name,c.course_name,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id ";
            printds.Clear();
            printds = da.select_method_wo_parameter(studinfo, "Text");
            DataView dv = new DataView();
            string semroman = string.Empty;
            Gios.Pdf.PdfPage mypdfpage;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                string regnnono = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 7].Value);
                if (stva == 1)
                {
                    //printds.Tables[1].DefaultView.RowFilter = "degree_code='" + ddlbranch.SelectedItem.Value.ToString() + "'";
                    int currentpaper = 0;
                    int arearcount = 0;
                    int project = 0;
                    int others = 0;

                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and  subject_type like '%core%' and promote_count=1 ";
                    theoryCount = printds.Tables[0].DefaultView.Count;

                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and subject_type like '%Practical%' and promote_count=1 ";
                    Practicalcount = printds.Tables[0].DefaultView.Count;

                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and subject_type like '%Project%' and promote_count=1 ";
                    project = printds.Tables[0].DefaultView.Count;

                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and subject_type like '%Others%' and promote_count=1 ";
                    others = printds.Tables[0].DefaultView.Count;

                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and paper=0";
                    currentpaper = printds.Tables[0].DefaultView.Count;
                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and paper=1";
                    arearcount = printds.Tables[0].DefaultView.Count;
                    printds.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    dv = printds.Tables[0].DefaultView;



                    if (dv.Count > 0)
                    {
                        mypdfpage = mydoc.NewPage();
                        string studnmae = dv[0]["stud_name"].ToString();
                        string seminfo = dv[0]["Current_Semester"].ToString();
                        string stdappno = dv[0]["app_no"].ToString();
                        string degreecodee = dv[0]["degree_code"].ToString();
                        string father_name = Convert.ToString(dv[0]["parent_name"]);
                        string studaddr = Convert.ToString(dv[0]["parent_addressP"]);
                        string studstreetname = Convert.ToString(dv[0]["Streetp"]);
                        string studcity = Convert.ToString(dv[0]["Cityp"]);
                        string studdist = Convert.ToString(dv[0]["Districtp"]);
                        string studsate = Convert.ToString(dv[0]["parent_statep"]);
                        string studcountry = Convert.ToString(dv[0]["countryp"]);
                        string studpincode = Convert.ToString(dv[0]["parent_pincodep"]);
                        string studmob_no = Convert.ToString(dv[0]["student_mobile"]);
                        string studFathermob_no = Convert.ToString(dv[0]["parentF_Mobile"]);
                        string studresidentialaddress = "", studresidentialaddress1 = string.Empty;
                        if (studaddr.Trim().Trim(',') != "")
                        {
                            studresidentialaddress = studaddr.Trim().Trim(',');
                        }
                        if (studstreetname.Trim().Trim(',') != "")
                        {
                            if (studresidentialaddress != "")
                            {
                                studresidentialaddress += ", " + studstreetname.Trim().Trim(',');
                            }
                            else
                            {
                                studresidentialaddress = studstreetname.Trim().Trim(',');
                            }
                        }
                        if (studcity.Trim().Trim(',') != "")
                        {
                            if (studresidentialaddress != "")
                            {
                                studresidentialaddress += ", " + studcity.Trim().Trim(',');
                            }
                            else
                            {
                                studresidentialaddress = studcity.Trim().Trim(',');
                            }
                        }
                        if (studdist.Trim().Trim(',') != "")
                        {
                            if (studresidentialaddress != "")
                            {
                                studresidentialaddress += ", " + studdist.Trim().Trim(',');
                            }
                            else
                            {
                                studresidentialaddress = studdist.Trim().Trim(',');
                            }
                        }
                        if (studsate.Trim().Trim(',') != "")
                        {
                            if (studresidentialaddress != "")
                            {
                                studresidentialaddress += ", " + studsate.Trim().Trim(',');
                            }
                            else
                            {
                                studresidentialaddress = studsate.Trim().Trim(',');
                            }
                        }
                        if (studcountry.Trim().Trim(',') != "")
                        {
                            if (studresidentialaddress != "")
                            {
                                studresidentialaddress += ", " + studcountry.Trim().Trim(',');
                            }
                            else
                            {
                                studresidentialaddress = studcountry.Trim().Trim(',');
                            }
                        }
                        if (studpincode.Trim().Trim(',') != "")
                        {
                            if (studresidentialaddress != "")
                            {
                                studresidentialaddress += ", Pincode : " + studpincode.Trim().Trim(',');
                            }
                            else
                            {
                                studresidentialaddress = "Pincode : " + studpincode.Trim().Trim(',');
                            }
                        }
                        PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
                        PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        mypdfpage.Add(pr1);
                        int coltop = 25;
                        PdfTextArea ptc;

                        #region STudent Photo

                        //string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                        //MemoryStream memoryStream = new MemoryStream();
                        //DataSet dsstdpho = new DataSet();
                        //dsstdpho.Clear();
                        //dsstdpho.Dispose();
                        //dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                        //if (dsstdpho.Tables[0].Rows.Count > 0)
                        //{
                        //    byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                        //    memoryStream.Write(file, 0, file.Length);
                        //    if (file.Length > 0)
                        //    {
                        //        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                        //        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                        //        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                        //        {
                        //        }
                        //        else
                        //        {
                        //            thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                        //            //NetworkShare.SaveOnNetworkShare(thumb, HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), ImageFormat.Jpeg);
                        //        }
                        //    }
                        //}
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                        {

                            Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            mypdfpage.Add(LogoImage2, 470, 20, 400);

                        }
                        #endregion

                        #region TOP DETAILS
                        string collCode = Convert.ToString(ddlcollege.SelectedValue);
                        if (chkheadimage.Checked == false)
                        {
                            #region Left Logo

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collCode.Trim() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collCode.Trim() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 400);
                            }
                            else if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 400);
                            }
                            #endregion
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
                            mypdfpage.Add(ptc);
                            //coltop = coltop + 20;
                            //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, collacr);
                            //mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, dispin);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(font2small, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
                            mypdfpage.Add(ptc);
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                            ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 30), System.Drawing.ContentAlignment.TopCenter, "Application For Course Registration and End Semester Examination Fees -" + strMonthName.ToUpper() + " - " + examyear + ""); // Examination Fees -" + strMonthName.ToUpper() + " - " + examyear + "
                            mypdfpage.Add(ptc);

                            //ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 100, 250, 20), System.Drawing.ContentAlignment.TopCenter, " Examination Fees -" + strMonthName.ToUpper() + " - " + examyear + "");
                            //mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                        }
                        else
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + ddlcollege.SelectedValue.ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 20, 25, 410);
                            }
                            coltop = coltop + 70;
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exammonth));
                            ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 105, 595, 30), System.Drawing.ContentAlignment.TopCenter, "Application For Course Registration and End Semester ");//Examination Fees -" + strMonthName.ToUpper() + " - " + examyear + ""
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 150, 250, 20), System.Drawing.ContentAlignment.TopCenter, "Examination Fees -" + strMonthName.ToUpper() + " - " + examyear + "");
                            mypdfpage.Add(ptc);
                        }
                        PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 28, 565, 0.01);
                        PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        #endregion

                        #region studinfo

                        coltop = coltop + 15;
                        if (seminfo == "1")
                        {
                            semroman = "I";
                        }
                        else if (seminfo == "2")
                        {
                            semroman = "II";
                        }
                        else if (seminfo == "3")
                        {
                            semroman = "III";
                        }
                        else if (seminfo == "4")
                        {
                            semroman = "IV";
                        }
                        else if (seminfo == "5")
                        {
                            semroman = "V";
                        }
                        else if (seminfo == "6")
                        {
                            semroman = "VI";
                        }
                        else if (seminfo == "7")
                        {
                            semroman = "VII";
                        }
                        else if (seminfo == "8")
                        {
                            semroman = "VIII";
                        }
                        Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(font3small, 5, 6, 3);
                        table1forpage2.Columns[0].SetWidth(120);
                        table1forpage2.Columns[1].SetWidth(3);
                        table1forpage2.Columns[2].SetWidth(150);
                        table1forpage2.Columns[3].SetWidth(120);
                        table1forpage2.Columns[4].SetWidth(3);
                        table1forpage2.Columns[5].SetWidth(135);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);

                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 0).SetContent("Register Number  ");//& Semester   // + " & " + semroman
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 1).SetContent(":");
                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 2).SetContent(regnnono.ToUpper());

                        table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 3).SetContent("Student's Name");
                        table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 4).SetContent(":");
                        table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(0, 5).SetContent(studnmae);

                        table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(1, 0).SetContent("Semester");
                        table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 1).SetContent(":");
                        table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(1, 2).SetContent(semroman);

                        table1forpage2.Cell(1, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(1, 3).SetContent("Student's Mobile No.");
                        table1forpage2.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 4).SetContent(":");
                        table1forpage2.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(1, 5).SetContent(studmob_no);

                        table1forpage2.Cell(2, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 0).SetContent("Degree & Branch");
                        table1forpage2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(2, 1).SetContent(":");
                        string degreebranch = da.GetFunction("select (c.course_name + ' - '+dd.dept_name) as degree,d.degree_code FROM degree d, department dd,course c where d.dept_code=dd.dept_code and c.course_id=d.course_id  and degree_code='" + degreecodee + "'");
                        table1forpage2.Cell(2, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 2).SetContent(degreebranch);

                        table1forpage2.Cell(2, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 3).SetContent("Date of Birth(DD/MM/YYYY)");
                        table1forpage2.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(2, 4).SetContent(":");
                        string dob = da.GetFunction("select convert(varchar,dob,103) dob from applyn  where app_no='" + stdappno + "'");
                        table1forpage2.Cell(2, 5).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(2, 5).SetContent(dob);


                        table1forpage2.Cell(3, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(3, 0).SetContent("Father's Name");
                        table1forpage2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(3, 1).SetContent(":");
                        table1forpage2.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(3, 2).SetContent(father_name);

                        table1forpage2.Cell(3, 3).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(3, 3).SetContent("Father's Mobile No.");
                        table1forpage2.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(3, 4).SetContent(":");
                        table1forpage2.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(3, 5).SetContent(studFathermob_no);


                        table1forpage2.Cell(4, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(4, 0).SetContent("Present Address");
                        table1forpage2.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(4, 1).SetContent(":");
                        table1forpage2.Cell(4, 2).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(4, 2).SetContent(studresidentialaddress);
                        table1forpage2.Cell(4, 2).ColSpan = 2;


                        Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 122, 565, 200));
                        mypdfpage.Add(newpdftabpage2);
                        coltop = coltop + 85;
                        tlinerect = new PdfArea(mydoc, 15, coltop + 33, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        #endregion

                        table1forpage2 = mydoc.NewTable(font4small, dv.Count + 1, 9, 4);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                        table1forpage2.Columns[0].SetWidth(20);
                        table1forpage2.Columns[1].SetWidth(50);
                        table1forpage2.Columns[2].SetWidth(130);
                        table1forpage2.Columns[3].SetWidth(20);

                        table1forpage2.Columns[4].SetWidth(20);
                        table1forpage2.Columns[5].SetWidth(50);
                        table1forpage2.Columns[6].SetWidth(130);
                        table1forpage2.Columns[7].SetWidth(20);
                        mypdfpage.Add(newpdftabpage2);
                        coltop = coltop + 85;

                        tlinerect = new PdfArea(mydoc, 15, 245, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        tlinerect = new PdfArea(mydoc, 38, 228, 0.01, 387);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        tlinerect = new PdfArea(mydoc, 95, 228, 0.01, 387);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        tlinerect = new PdfArea(mydoc, 255, 228, 0.01, 387);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        tlinerect = new PdfArea(mydoc, 288, 228, 0.01, 387);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        //tlinerect = new PdfArea(mydoc, 293, 228, 0.01, 387);
                        //plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        //mypdfpage.Add(plimerecyt);

                        tlinerect = new PdfArea(mydoc, 320, 228, 0.01, 387);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        tlinerect = new PdfArea(mydoc, 390, 228, 0.01, 387);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        tlinerect = new PdfArea(mydoc, 532, 228, 0.01, 387);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        //tlinerect = new PdfArea(mydoc, 557, 228, 0.01, 387);
                        //plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        //mypdfpage.Add(plimerecyt);

                        tlinerect = new PdfArea(mydoc, 15, 615, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);

                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopCenter);
                        table1forpage2.Cell(0, 0).SetFont(font4bold);
                        table1forpage2.Cell(0, 0).SetContent("Sem");
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.TopCenter);
                        table1forpage2.Cell(0, 1).SetFont(font4bold);
                        table1forpage2.Cell(0, 1).SetContent("Course Code");
                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopCenter);
                        table1forpage2.Cell(0, 2).SetFont(font4bold);
                        table1forpage2.Cell(0, 2).SetContent("Course Name");
                        table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.TopCenter);
                        table1forpage2.Cell(0, 3).SetFont(font4bold);
                        table1forpage2.Cell(0, 3).SetContent("(Rs)");
                        table1forpage2.Cell(0, 0).SetCellPadding(6);
                        table1forpage2.Cell(0, 1).SetCellPadding(6);
                        table1forpage2.Cell(0, 2).SetCellPadding(6);
                        table1forpage2.Cell(0, 3).SetCellPadding(6);
                        table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.TopCenter);
                        table1forpage2.Cell(0, 4).SetFont(font4bold);
                        table1forpage2.Cell(0, 4).SetContent("Sem");
                        table1forpage2.Cell(0, 5).SetContentAlignment(ContentAlignment.TopCenter);
                        table1forpage2.Cell(0, 5).SetFont(font4bold);
                        table1forpage2.Cell(0, 5).SetContent("Course Code");
                        table1forpage2.Cell(0, 6).SetContentAlignment(ContentAlignment.TopCenter);
                        table1forpage2.Cell(0, 6).SetFont(font4bold);
                        table1forpage2.Cell(0, 6).SetContent("Course Name");
                        table1forpage2.Cell(0, 7).SetContentAlignment(ContentAlignment.TopCenter);
                        table1forpage2.Cell(0, 7).SetFont(font4bold);
                        table1forpage2.Cell(0, 7).SetContent("(Rs)");
                        table1forpage2.Cell(0, 5).SetCellPadding(6);
                        table1forpage2.Cell(0, 6).SetCellPadding(6);
                        table1forpage2.Cell(0, 7).SetCellPadding(6);
                        table1forpage2.Cell(0, 8).SetCellPadding(6);
                        double currentPaperCost = 0;
                        double arrearPaperCost = 0;
                        int j = 0;
                        for (int i = 0; i < dv.Count; i++)
                        {
                            if (i < 20)
                            {
                                string subject_Code = Convert.ToString(dv[i]["subject_code"]);

                                table1forpage2.Cell(i + 1, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                table1forpage2.Cell(i + 1, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                table1forpage2.Cell(i + 1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                table1forpage2.Cell(i + 1, 3).SetContentAlignment(ContentAlignment.TopCenter);
                                table1forpage2.Cell(i + 1, 0).SetCellPadding(3);
                                table1forpage2.Cell(i + 1, 1).SetCellPadding(3);
                                table1forpage2.Cell(i + 1, 2).SetCellPadding(3);
                                table1forpage2.Cell(i + 1, 3).SetCellPadding(3);
                                table1forpage2.Cell(i + 1, 4).SetCellPadding(3);
                                if (i == 1)
                                {
                                    table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                    table1forpage2.Cell(i + 1, 3).SetContent(dv[i]["fee"].ToString() + " ");
                                }
                                else
                                {
                                    table1forpage2.Cell(i + 1, 0).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(i + 1, 1).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(i + 1, 2).SetContent(dv[i]["subject_name"].ToString());
                                    table1forpage2.Cell(i + 1, 3).SetContent(dv[i]["fee"].ToString() + " ");
                                }
                                double value = 0;
                                if (Convert.ToString(dv[i]["paper"]).Trim() == "1")
                                {
                                    //table1forpage2.Cell(i + 1, 4).SetBackgroundColor(Color.Red);
                                    double.TryParse(Convert.ToString(dv[i]["fee"]).Trim(), out value);
                                    arrearPaperCost += value;
                                }
                                else
                                {
                                    double.TryParse(Convert.ToString(dv[i]["fee"]).Trim(), out value);
                                    currentPaperCost += value;
                                }
                            }
                            else
                            {
                                table1forpage2.Cell(j + 1, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                table1forpage2.Cell(j + 1, 6).SetContentAlignment(ContentAlignment.TopCenter);
                                table1forpage2.Cell(j + 1, 7).SetContentAlignment(ContentAlignment.TopLeft);
                                table1forpage2.Cell(j + 1, 8).SetContentAlignment(ContentAlignment.TopCenter);
                                table1forpage2.Cell(j + 1, 5).SetCellPadding(3);
                                table1forpage2.Cell(j + 1, 6).SetCellPadding(3);
                                table1forpage2.Cell(j + 1, 7).SetCellPadding(3);
                                table1forpage2.Cell(j + 1, 8).SetCellPadding(3);
                                table1forpage2.Cell(j + 1, 9).SetCellPadding(3);
                                if (j == 1)
                                {
                                    table1forpage2.Cell(j + 1, 5).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(j + 1, 6).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(j + 1, 7).SetContent(dv[i]["subject_name"].ToString());
                                    table1forpage2.Cell(j + 1, 8).SetContent(dv[i]["fee"].ToString());
                                }
                                else
                                {
                                    table1forpage2.Cell(j + 1, 5).SetContent(dv[i]["semester"].ToString());
                                    table1forpage2.Cell(j + 1, 6).SetContent(dv[i]["subject_code"].ToString());
                                    table1forpage2.Cell(j + 1, 7).SetContent(dv[i]["subject_name"].ToString());
                                    table1forpage2.Cell(j + 1, 8).SetContent(dv[i]["fee"].ToString());
                                }
                                double value = 0;
                                if (Convert.ToString(dv[i]["paper"]).Trim() == "1")
                                {
                                    double.TryParse(Convert.ToString(dv[i]["fee"]).Trim(), out value);
                                    arrearPaperCost += value;
                                    //table1forpage2.Cell(i + 1, 9).SetBackgroundColor(Color.Red);
                                }
                                else
                                {
                                    double.TryParse(Convert.ToString(dv[i]["fee"]).Trim(), out value);
                                    currentPaperCost += value;
                                }
                                j++;
                            }
                        }
                        newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 228, 565, 500));//sr
                        mypdfpage.Add(newpdftabpage2);
                        j = 0;
                        for (int row = 0; row < dv.Count; row++)
                        {
                            if (row < 20)
                            {
                                if (Convert.ToString(dv[row]["paper"]).Trim() == "1")
                                {
                                    PdfRectangle prnew = newpdftabpage2.CellArea(row + 1, 4).ToRectangle(Color.Black, 1, Color.White);
                                    PdfArea pdfdfjdf = prnew.RectangleArea.InnerArea(newpdftabpage2.CellArea(row + 1, 4).CenterX, newpdftabpage2.CellArea(row + 1, 4).CenterY);
                                    prnew.RectangleArea.Height = 7;
                                    prnew.RectangleArea.Width = 7;
                                    mypdfpage.Add(prnew);
                                }
                            }
                            else
                            {
                                if (Convert.ToString(dv[row]["paper"]).Trim() == "1")
                                {
                                    PdfRectangle prnew = newpdftabpage2.CellArea(j + 1, 9).ToRectangle(Color.Black, 1, Color.White);
                                    prnew.RectangleArea.InnerArea(20, 20);
                                    prnew.RectangleArea.Height = 7;
                                    prnew.RectangleArea.Width = 7;
                                    mypdfpage.Add(prnew);
                                }
                                j++;
                            }
                        }
                        tlinerect = new PdfArea(mydoc, 15, 780, 565, 0.01);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        table1forpage2 = mydoc.NewTable(font4bold, 3, 5, 4);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.Columns);
                        table1forpage2.Columns[0].SetWidth(100);
                        table1forpage2.Columns[1].SetWidth(100);
                        table1forpage2.Columns[2].SetWidth(100);
                        table1forpage2.Columns[3].SetWidth(100);
                        table1forpage2.Columns[4].SetWidth(100);
                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 0).SetContent("Courses Appearing");
                        foreach (PdfCell pr in table1forpage2.CellRange(0, 0, 0, 0).Cells)
                        {
                            pr.RowSpan = 2;
                        }
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 1).SetContent("Core");
                        table1forpage2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 1).SetContent((theoryCount == 0) ? "-" : Convert.ToString(theoryCount));

                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 2).SetContent("Practical");
                        table1forpage2.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 2).SetContent((Practicalcount == 0) ? "-" : Convert.ToString(Practicalcount));


                        table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 3).SetContent("Project");
                        table1forpage2.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 3).SetContent((project == 0) ? "-" : Convert.ToString(project));

                        table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 4).SetContent("Others");
                        table1forpage2.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(1, 4).SetContent((others == 0) ? "-" : Convert.ToString(others));

                        newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 620, 200, 500));
                        mypdfpage.Add(newpdftabpage2);
                        PdfRectangle prt = newpdftabpage2.Area.ToRectangle(Color.Black);
                        mypdfpage.Add(prt);
                        for (int cel = 1; cel < 5; cel++)
                        {
                            PdfLine plt = newpdftabpage2.CellArea(0, cel).LowerBound(Color.Black, 1);
                            mypdfpage.Add(plt);
                        }
                        string subjextsregister = "No. of Course(s) Registered : " + dv.Count + "";
                        table1forpage2 = mydoc.NewTable(font3small, 5 + ((arearcount > 0) ? 2 : 1), 3, 3);
                        table1forpage2.Columns[0].SetWidth(120);
                        table1forpage2.Columns[1].SetWidth(10);
                        table1forpage2.Columns[2].SetWidth(130);
                        int count = 0;
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.None);
                        bool hasarrear = false;
                        bool hascurrent = true;
                        table1forpage2.Cell(count, 0).SetContentAlignment(ContentAlignment.TopLeft);
                        table1forpage2.Cell(count, 0).SetContent("No.of Current Course(s) Registered : " + currentpaper);
                        foreach (PdfCell pr in table1forpage2.CellRange(count, 0, count, 0).Cells)
                        {
                            pr.ColSpan = 3;
                        }
                        hascurrent = true;
                        count++;
                        if (arearcount > 0)
                        {
                            hasarrear = true;
                            table1forpage2.Cell(count, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1forpage2.Cell(count, 0).SetContent("No.of Arrear Course(s) Registered : ");
                            foreach (PdfCell pr in table1forpage2.CellRange(count, 0, count, 0).Cells)
                            {
                                pr.ColSpan = 3;
                            }
                            count++;
                        }
                        DataTable dtAmt = new DataTable();
                        DataTable syllamnt = new DataTable();
                        string markSheetAmt = string.Empty;
                        string MarkSheetAmount = "Select fa.FeeAmount from FM_LedgerMaster lm,FT_FeeAllot fa where lm.LedgerName='Statement Of Marks' and fa.LedgerFK=lm.LedgerPK and fa.App_no='" + stdappno + "'";

                        string SyllFee = "0";
                        Dictionary<string, string[]> dicManFeesAmt = new Dictionary<string, string[]>();
                        MandatoryFeesValuesforsyllabi(out dicManFeesAmt);

                        if (dicManFeesAmt.Count > 0)
                        {
                            foreach (KeyValuePair<string, string[]> dicMandFees in dicManFeesAmt)
                            {
                                double costFee = 0;
                                string[] valuePair = new string[3];
                                string key = dicMandFees.Key.Trim().ToLower();
                                valuePair = dicMandFees.Value;
                                string headerFK = (valuePair.Length >= 1) ? valuePair[0] : "";
                                string ledgerFK = (valuePair.Length >= 2) ? valuePair[1] : "";
                                string feeAmt = (valuePair.Length >= 3) ? valuePair[2] : "";
                                double.TryParse(feeAmt.Trim(), out costFee);

                                if (feeAmt.Trim() != "")
                                {
                                    SyllFee = feeAmt;

                                }
                                else
                                {
                                    SyllFee = "0";
                                }
                            }
                        }



                        dtAmt = dirAcc.selectDataTable(MarkSheetAmount);

                        if (dtAmt.Rows.Count > 0)
                        {
                            markSheetAmt = dtAmt.Rows[0]["FeeAmount"].ToString();
                        }
                        else
                        {
                            markSheetAmt = "0";
                        }

                        //count++;

                        double ttol = 0;
                        ttol = Convert.ToDouble(dv[0]["total_fee"].ToString()) + Convert.ToDouble(SyllFee);

                        table1forpage2.Cell(count, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(count, 0).SetContent("Exam Fees (Rs)");
                        table1forpage2.Cell(count, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(count, 1).SetContent(":");
                        table1forpage2.Cell(count, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        string data = String.Format("{0:0.00}", currentPaperCost + arrearPaperCost);
                        table1forpage2.Cell(count, 2).SetContent(((hascurrent) ? data : "") + ((hasarrear) ? ((hascurrent) ? "  " : "") : ""));
                        //table1forpage2.Cell(count, 2).SetContent(((hascurrent) ? Convert.ToString(currentPaperCost) : "") + ((hasarrear) ? ((hascurrent) ? " + " : "") : Convert.ToString(arrearPaperCost)));

                        table1forpage2.Cell(count + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(count + 1, 0).SetContent("Statement of Mark Sheet Fee (Rs)");
                        table1forpage2.Cell(count + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(count + 1, 1).SetContent(":");
                        double markSheet = Convert.ToDouble(markSheetAmt);
                        data = String.Format("{0:0.00}", markSheet);
                        table1forpage2.Cell(count + 1, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage2.Cell(count + 1, 2).SetContent(data);

                        table1forpage2.Cell(count + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(count + 2, 0).SetContent("Others (Rs)");
                        table1forpage2.Cell(count + 2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(count + 2, 1).SetContent(":");
                        double otherfee = Convert.ToDouble(dv[0]["extra_fee"].ToString()) + Convert.ToDouble(dv[0]["fine"].ToString()) + Convert.ToDouble(dv[0]["cost_mark"].ToString()) + Convert.ToDouble(dv[0]["cost_appl"].ToString());
                        data = String.Format("{0:0.00}", otherfee);
                        table1forpage2.Cell(count + 2, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage2.Cell(count + 2, 2).SetContent(data);


                        int var = 4;
                        if (SyllFee != "0")
                        {
                            table1forpage2.Cell(count + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(count + 3, 0).SetContent("Syllabi & Curriculam (Rs)");
                            table1forpage2.Cell(count + 3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(count + 3, 1).SetContent(":");
                            double Syllamnt = Convert.ToDouble(SyllFee);
                            data = string.Format("{0:0.00}", Syllamnt);
                            table1forpage2.Cell(count + 3, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                            table1forpage2.Cell(count + 3, 2).SetContent(data);
                        }
                        else
                        {
                            var -= 1;
                        }

                        table1forpage2.Cell(count + var, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage2.Cell(count + var, 0).SetContent("Total Fees (Rs)");
                        double tot = currentPaperCost + arrearPaperCost + otherfee + markSheet;
                        data = String.Format("{0:0.00}", tot); //+ Syllamnt
                        table1forpage2.Cell(count + var, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(count + var, 1).SetContent(":");
                        table1forpage2.Cell(count + var, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        table1forpage2.Cell(count + var, 2).SetContent(data);
                        //table1forpage2.Cell(count + var, 2).SetContent(((hasarrear) ?((hascurrent) ? Convert.ToString(data) : "")));
                        //table1forpage2.Cell(count + var, 2).SetContent(data);
                        newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 230, 620, 200, 500));
                        mypdfpage.Add(newpdftabpage2);
                        coltop = 750;
                        if (!string.IsNullOrEmpty(foreNoon.Trim()))
                        {
                            //PdfTextAr ea ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 210, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "FN - FORENOON " + foreNoon.Trim());
                            //mypdfpage.Add(ptcfn);
                        }
                        else
                        {
                            //PdfTextArea ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 210, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "FN - FORENOON 10.00 AM - 1.00 PM");
                            //mypdfpage.Add(ptcfn);
                        }
                        coltop = coltop + 15;
                        if (!string.IsNullOrEmpty(foreNoon.Trim()))
                        {
                            //PdfTextArea ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 210, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "AN - AFTERNOON " + afterNoon.Trim());
                            //mypdfpage.Add(ptcan);
                        }
                        else
                        {
                            //PdfTextArea ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 210, coltop, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "AN - AFTERNOON 2.00 PM - 5.00 PM");
                            //mypdfpage.Add(ptcan);
                        }
                        coltop = 740;
                        PdfTextArea ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop - 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                        mypdfpage.Add(ptcstisign);
                        ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop - 35, 400, 20), System.Drawing.ContentAlignment.MiddleLeft, "Date : ");
                        mypdfpage.Add(ptcstisign);

                        ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 445, coltop - 90, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Tutor/");
                        mypdfpage.Add(ptcstisign);

                        ptcstisign = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 455, coltop - 80, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class Advisor");
                        mypdfpage.Add(ptcstisign);

                        PdfTextArea ptccontroller = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 380, coltop - 30, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Dean/HOD");
                        mypdfpage.Add(ptccontroller);

                        coltop = coltop + 35;
                        PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                        mypdfpage.Add(ptcsnote);
                        coltop = coltop + 15;
                        PdfTextArea ptcsnote1 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1. If any discrepancy is found in the form then report to the COE office immediately.");
                        mypdfpage.Add(ptcsnote1);
                        coltop = coltop + 15;
                        PdfTextArea ptcsnote2 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "");
                        mypdfpage.Add(ptcsnote2);
                        coltop = coltop + 15;
                        ptcsnote2 = new PdfTextArea(font4small, System.Drawing.Color.Black, new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "");
                        mypdfpage.Add(ptcsnote2);
                        mypdfpage.SaveToDocument();
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ExamApplication" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
            #endregion Format1
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"] ) , "Student Special Hour Entry");
        }
    }


    /// <summary>
    /// Developed By Prabha KARAN On Nov 29 2017
    /// for getting Syllabi fee value
    /// </summary>
    /// <param name="dicMandFees"></param>
    private void MandatoryFeesValuesforsyllabi(out Dictionary<string, string[]> dicMandFees)
    {
        ArrayList arrHeaderFk;
        ArrayList arrLedgerFk;
        dicMandFees = new Dictionary<string, string[]>();
        dicMandFees.Clear();
        DataTable dtMandFee = new DataTable();
        dtMandFee.Columns.Add("Application Form");
        dtMandFee.Columns.Add("Semester Mark Sheet");
        dtMandFee.Columns.Add("Theory");
        dtMandFee.Columns.Add("Practical");
        dtMandFee.Columns.Add("Project");
        dtMandFee.Columns.Add("Field Work");
        dtMandFee.Columns.Add("Viva Voice");
        dtMandFee.Columns.Add("Disseration");
        dtMandFee.Columns.Add("Consolidate Mark Sheet");
        dtMandFee.Columns.Add("Course Completaion");
        dtMandFee.Columns.Add("Online Application Fee");
        dtMandFee.Columns.Add("Arrear Theory");
        dtMandFee.Columns.Add("Arrear Practical");
        dtMandFee.Columns.Add("Central Valuation");
        dtMandFee.Columns.Add("Syllabi & Curricular");
        dtMandFee.Rows.Add("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        ArrayList arrMandFees = new ArrayList();
        arrHeaderFk = new ArrayList();
        arrLedgerFk = new ArrayList();
        string[] FeeCodeHFkLFkFees = new string[3];
        for (int dCol = 0; dCol < dtMandFee.Columns.Count; dCol++)
        {
            string linkVal = Convert.ToString(dtMandFee.Columns[dCol].ColumnName.Trim()) + "@#MandatoryFee";
            byte prevVal = 0;
            string linkValNew = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'").Trim();
            byte.TryParse(linkValNew.Trim(), out prevVal);
            if (prevVal == 1)
            {
                dtMandFee.Rows[0][dCol] = prevVal;
            }
        }
        for (int dRow = 0; dRow < dtMandFee.Columns.Count; dRow++)
        {
            string colName = Convert.ToString(dtMandFee.Columns[dRow].ColumnName).Trim();
            if (Convert.ToString(dtMandFee.Rows[0][colName]).Trim() == "1")
            {
                arrMandFees.Add(colName);
                DataSet settingValue = d2.select_method_wo_parameter(" select settings,value from Master_Settings where settings='" + colName + "' --and usercode='" + Session["usercode"].ToString() + "'", "Text");
                string feeValue = string.Empty;
                string headerCode = string.Empty;
                string ledgerCode = string.Empty;
                string FeeAmt = string.Empty;
                if (settingValue.Tables.Count > 0 && settingValue.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        if (Convert.ToString(settingValue.Tables[0].Rows[0]["settings"]).ToUpper() == "SYLLABI & CURRICULAR")
                        {
                            string[] ValuesList = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';');
                            feeValue = Convert.ToString(settingValue.Tables[0].Rows[0][0]);
                            headerCode = (ValuesList.Length >= 1) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[0] : "0";
                            ledgerCode = (ValuesList.Length >= 2) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1] : "0";
                            FeeAmt = (ValuesList.Length >= 3) ? Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[2] : "0";
                            FeeCodeHFkLFkFees = new string[3];
                            FeeCodeHFkLFkFees[0] = headerCode;
                            FeeCodeHFkLFkFees[1] = ledgerCode;
                            FeeCodeHFkLFkFees[2] = FeeAmt;
                            if (!dicMandFees.ContainsKey(colName.ToLower().Trim()))
                            {
                                dicMandFees.Add(colName.ToLower().Trim(), FeeCodeHFkLFkFees);
                            }
                            if (!arrHeaderFk.Contains(headerCode))
                            {
                                arrHeaderFk.Add(headerCode);
                            }
                            if (!arrLedgerFk.Contains(ledgerCode))
                            {
                                arrLedgerFk.Add(ledgerCode);
                            }
                        }
                    }
                    catch { }
                }
            }
        }
        //return arrMandFees;
    }

    #endregion

    //magesh 9/2/18
    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegecode, "Exam Application");
        }
    }

    public void Loadsemester()
    {
        try
        {
            cblsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddlcollege.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and degree_code='" + ddlbranch.SelectedValue.ToString() + "' order by NDurations desc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        cblsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        cblsem.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddlcollege.SelectedValue.ToString() + " and degree_code='" + ddlbranch.SelectedValue.ToString() + "' order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            cblsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            cblsem.Items.Add(i.ToString());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "Exam Application");
        }
    }

    protected void chksrmwise_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chksemwise.Checked)
        {
            upnlsem.Visible = true;
            lblSem1.Visible = true;
            Loadsemester();
        }
        else
        {
            upnlsem.Visible = false;
            lblSem1.Visible = false;
        }
    }

    protected void chkchksem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chksem, cblsem, txtSem, lblSem1.Text, "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    protected void cblsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chksem, cblsem, txtSem, lblSem1.Text, "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    protected void btnfasttrackSub_Click(object sender, EventArgs e)
    {
        try
        {
            div3.Visible = false;
            Label2.Visible = false;
            Label2.Text = "";
            FpSpread3.Visible = false;
            string examMonth = string.Empty;
            FpSpread3.CommandBar.Visible = false;
            string examYear = string.Empty;
            //FpSpread1.SaveChanges();
            string sem = ddlsem.SelectedValue.ToString();
            string degree = ddlbranch.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            if (Convert.ToString(ddlYear.SelectedValue).Trim() == "0")
            {
                divViewSubjects.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            else
            {
                examYear = Convert.ToString(ddlYear.SelectedValue).Trim();
            }
            if ((Convert.ToString(ddlMonth.SelectedValue).Trim() == "0"))
            {
                divViewSubjects.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }
            else
            {
                examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
            }
            Hashtable hatprv = new Hashtable();
            string prvSel = "select * from Futuresub_Exam_app where ExamMonth='" + examMonth + "' and ExamYear='" + examYear + "' and Batch_year='" + batch + "' and DegreeCode='" + degree + "' and Semester='" + sem + "'";
            DataTable dtprv = dirAcc.selectDataTable(prvSel);
            if (dtprv.Rows.Count > 0)
            {
                foreach (DataRow ds in dtprv.Rows)
                {
                    string subno = Convert.ToString(ds["subject_no"]);
                    if (!hatprv.ContainsKey(subno))
                    {
                        hatprv.Add(subno, subno);
                    }
                }
            }
            string valSemcur = string.Empty;
            string valsemarr = string.Empty;
            if (cblsem.Items.Count > 0)
            {
                for (int s = 0; s < cblsem.Items.Count; s++)
                {
                    if (cblsem.Items[s].Selected)
                    {
                        int ss = Convert.ToInt32(cblsem.Items[s].Value);
                        if (ss > Convert.ToInt32(sem))
                        {
                            if (string.IsNullOrEmpty(valSemcur))
                                valSemcur = "'" + ss + "'";
                            else
                                valSemcur = valSemcur + "," + "'" + ss + "'";
                        }
                        else if (ss < Convert.ToInt32(sem))
                        {
                            if (string.IsNullOrEmpty(valsemarr))
                                valsemarr = "'" + ss + "'";
                            else
                                valsemarr = valsemarr + "," + "'" + ss + "'";
                        }
                    }
                }
            }

            //FpSpread3.Sheets[0].RowCount = 0;
            string SelectQ = "select s.subject_code,s.subject_name,sy.semester,s.subject_no from subject s,syllabus_master sy,sub_sem  ss where ss.syll_code=sy.syll_code and s.subType_no=ss.subType_no and s.syll_code=sy.syll_code and sy.Batch_Year='" + batch + "' and sy.degree_code='" + degree + "' and sy.semester in(" + valSemcur + ") and ss.promote_count=1";
            DataTable dtFusubject = dirAcc.selectDataTable(SelectQ);
            bool reportfalg = false;
            if (dtFusubject.Rows.Count > 0)
            {
                FpSpread3.Visible = true;
                FpSpread3.Sheets[0].RowCount = 0;
                //FpSpread3.Sheets[0].ColumnCount = 0;
                FpSpread3.Sheets[0].SheetCorner.ColumnCount = 0;
                FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                FpSpread3.Sheets[0].ColumnCount = 5;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Sem/Year";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
                FpSpread3.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread3.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                FpSpread3.Sheets[0].Columns[0].Width = 70;
                FpSpread3.Sheets[0].Columns[1].Width = 100;
                FpSpread3.Sheets[0].Columns[2].Width = 150;
                FpSpread3.Sheets[0].Columns[3].Width = 220;
                FpSpread3.Sheets[0].Columns[4].Width = 70;
                int sno = 0;
                foreach (DataRow dt in dtFusubject.Rows)
                {
                    int val = 0;
                    string subno = Convert.ToString(dt["subject_no"]);
                    string subCode = Convert.ToString(dt["subject_code"]);
                    string subName = Convert.ToString(dt["subject_name"]);
                    string sems = Convert.ToString(dt["semester"]);
                    if (hatprv.ContainsKey(subno))
                        val = 1;
                    reportfalg = true;
                    sno++;
                    FpSpread3.Sheets[0].RowCount = FpSpread3.Sheets[0].RowCount + 1;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = sems;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = subCode;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Note = subno;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = subName;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].CellType = chkcell;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Value = val;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                }
                if (reportfalg == true)
                {
                    div3.Visible = true;
                    FpSpread3.Visible = true;
                    FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                    FpSpread3.SaveChanges();
                    FpSpread3.Width = 630;
                    FpSpread3.Height = 450;
                    //lblexportxl.Visible = true;
                }
            }
            else
            {
                div3.Visible = false;
                FpSpread3.Visible = false;
                Label2.Visible = true;
                Label2.Text = "No subject were Found";
            }
            //FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
            //FpSpread3.SaveChanges();
            //no subject were found



        }
        catch (Exception ex)
        {

        }
    }

    protected void btnFutSave_Clcik(object sender, EventArgs e)
    {
        Label2.Visible = false;
        Label2.Text = "";
        string examMonth = string.Empty;
        string examYear = string.Empty;
        FpSpread3.SaveChanges();
        string sem = ddlsem.SelectedValue.ToString();
        string degree = ddlbranch.SelectedValue.ToString();
        string batch = ddlbatch.SelectedValue.ToString();
        if (Convert.ToString(ddlYear.SelectedValue).Trim() == "0")
        {
            divViewSubjects.Visible = false;
            lblerror.Visible = true;
            lblerror.Text = "Please Select The Exam Year And Then Proceed";
            return;
        }
        else
        {
            examYear = Convert.ToString(ddlYear.SelectedValue).Trim();
        }
        if ((Convert.ToString(ddlMonth.SelectedValue).Trim() == "0"))
        {
            divViewSubjects.Visible = false;
            lblerror.Visible = true;
            lblerror.Text = "Please Select The Exam Month And Then Proceed";
            return;
        }
        else
        {
            examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
        }

        int count = 0;
        string delQ = "delete from Futuresub_Exam_app where ExamMonth='" + examMonth + "' and ExamYear='" + examYear + "' and Batch_year='" + batch + "' and DegreeCode='" + degree + "' and Semester='" + sem + "'";
        int del = da.update_method_wo_parameter(delQ, "Text");
        for (int fp = 0; fp < FpSpread3.Rows.Count; fp++)
        {
            int stva = Convert.ToInt32(FpSpread3.Sheets[0].Cells[fp, 4].Value);
            if (stva == 1)
            {
                string subNo = Convert.ToString(FpSpread3.Sheets[0].Cells[fp, 2].Note);

                if (!string.IsNullOrEmpty(subNo))
                {
                    string InsertQ = "if not exists(select * from Futuresub_Exam_app where ExamMonth='" + examMonth + "' and ExamYear='" + examYear + "' and Batch_year='" + batch + "' and DegreeCode='" + degree + "' and Semester='" + sem + "' and subject_no='" + subNo + "') insert into Futuresub_Exam_app(ExamMonth,ExamYear,Batch_year,DegreeCode,Semester,subject_no) values('" + examMonth + "','" + examYear + "','" + batch + "','" + degree + "','" + sem + "','" + subNo + "')";
                    count = da.update_method_wo_parameter(InsertQ, "Text");

                }
            }
            if (count != 0)
            {
                Label2.Visible = true;
                Label2.Text = "Saved Sucessfully";
            }

        }

    }

    protected void btnExit_exit_Clcik(object sender, EventArgs e)
    {
        div3.Visible = false;
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
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
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
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

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    //***added by Mullai
    protected void btnformatsetting_Click(object sender, EventArgs e)
    {
        divformatsettings.Visible = true;
        divformatset.Visible = true;
    }
    protected void btnsaveformsetting_Click(object sender, EventArgs e)
    {
        try
        {
            string sign1 = string.Empty;
            string sign2 = string.Empty;
            string sign3 = string.Empty;
            string note = string.Empty;

            sign1 = txtsignature1.Text;
            sign2 = txtsignature2.Text;
            sign3 = txtsignature3.Text;
            note = txtnote.Text;

            string signatures = sign1 + ";" + sign2 + ";" + sign3 + ";" + note;

            string updateqry = "if exists(select template from Master_Settings where settings='Exam Application Format Settings' and usercode='" + Convert.ToString(Session["usercode"]) + "') update Master_Settings set template='" + Convert.ToString(signatures) + "' where usercode='" + Convert.ToString(Session["usercode"]) + "' and settings='Exam Application Format Settings' else insert into Master_Settings (usercode,settings,template) values('" + Convert.ToString(Session["usercode"]) + "','Exam Application Format Settings','" + Convert.ToString(signatures) + "')";
            int updqry = da.update_method_wo_parameter(updateqry, "text");
            if (updqry > 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";
                divformatset.Visible = false;
                divformatsettings.Visible = false;
                txtsignature1.Text = string.Empty;
                txtsignature2.Text = string.Empty;
                txtsignature3.Text = string.Empty;
                txtnote.Text = string.Empty;

            }


        }
        catch
        {
        }

    }
    protected void btnexit1_Click(object sender, EventArgs e)
    {

        divformatsettings.Visible = false;
        divformatset.Visible = false;

    }
    //***

}
