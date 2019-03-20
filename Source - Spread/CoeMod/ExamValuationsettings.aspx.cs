using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;

public partial class ExamValuationSettion : System.Web.UI.Page
{
    DAccess2 dt = new DAccess2();
    DataSet ds = new DataSet();
    string college_code = "";
    string user_code = string.Empty;
    Boolean flag_true = false;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
            lblmsg.Visible = false;
            college_code = Session["collegecode"].ToString();
            user_code = Session["usercode"].ToString();
            lblserror.Visible = false;
            lblserror.ForeColor = Color.Red;
            if (!IsPostBack)
            {
                btnvaluationletter.Visible = false;
                Panelsubject.Visible = false;
                //loadYear();
                //loadmonth();

                //ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                int year1;
                year1 = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {
                    ddlYear.Items.Add(Convert.ToString(year1 - l));
                }
                // ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));


                mode();
                department();
                txtvaldate.Attributes.Add("readonly", "readonly");
                txtvaltodate.Attributes.Add("readonly", "readonly");
                txtvaldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtvaltodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblvaldate.Visible = false;
                lblvaltodate.Visible = false;
                txtvaltodate.Visible = false;
                txtvaldate.Visible = false;

                chkvalsummry.Visible = false;
                btnsubdelete.Visible = false;
                rdbValuation.Visible = true;
                rdbQsetter.Visible = true;
                NewDiv.Visible = true;
                chkSendEMail.Checked = false;
            }
        }

        catch (Exception ex)
        {
        }
    }

    public void loadYear()
    {
        try
        {
            ds = dt.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void loadmonth()
    {
        try
        {
            ds.Clear();
            string year = ddlYear.SelectedValue;
            ds = dt.Exammonth(year);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthname";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void mode()
    {
        try
        {
            string mode = "select distinct type from course where college_code='" + college_code + "' and type is not null and type<>''";
            ds = dt.select_method_wo_parameter(mode, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataValueField = "type";
                ddltype.DataBind();
            }
            else
            {
                ddltype.Items.Insert(0, "");
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void department()
    {
        try
        {
            ddldepat.Items.Clear();
            if (ddlstaream.SelectedItem.ToString() == "External") //&& rdbValuation.Checked == true
            {
                ds = dt.select_method_wo_parameter("select textval,TextCode from TextValTable where TextCriteria='exdep' order by textval", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldepat.DataSource = ds;
                    ddldepat.DataTextField = "textval";
                    ddldepat.DataValueField = "TextCode";
                    ddldepat.DataBind();
                    ddldepat.Items.Insert(0, "All");
                }
            }
            else if (rdbQsetter.Checked == true)
            {
                string type = ddltype.SelectedItem.Text;
                ds = dt.select_method_wo_parameter("select distinct Degree_Code,Dept_Name from Degree d,Department dt,Course c where d.Dept_Code=dt.Dept_Code and c.Course_Id =D.Course_Id and C.type='" + type + "' order  by Degree_Code ", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldepat.DataSource = ds;
                    ddldepat.DataTextField = "Dept_Name";
                    ddldepat.DataValueField = "Degree_Code";
                    ddldepat.DataBind();
                    ddldepat.Items.Insert(0, "All");
                }
            }
            else
            {
                ds = dt.loaddepartment(college_code);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldepat.DataSource = ds;
                    ddldepat.DataTextField = "dept_name";
                    ddldepat.DataValueField = "dept_code";
                    ddldepat.DataBind();
                    ddldepat.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void clear()
    {
        Printcontrol.Visible = false;
        lblmsg.Visible = false;
        FpValuation.Visible = false;
        Print_seating.Visible = false;
        Excel_seating.Visible = false;
        lblexcsea.Visible = false;
        txtexseat.Visible = false;
        Panelsubject.Visible = false;
        btnvaluationletter.Visible = false;
        lblvaldate.Visible = false;
        lblvaltodate.Visible = false;
        txtvaltodate.Visible = false;
        txtvaldate.Visible = false;
        chkvalsummry.Visible = false;
        btnsubdelete.Visible = false;
    }

    protected void Logout_btn_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        // clear();
        // loadmonth();
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        department();
    }

    protected void ddlstaream_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();

        if (ddlstaream.SelectedIndex == 0)
        {
            NewDiv.Visible = true;
            chkSendEMail.Checked = false;
            rdbValuation.Checked = true;
        }
        if (ddlstaream.SelectedIndex == 1)
        {
            NewDiv.Visible = true;
            chkSendEMail.Checked = false;
            rdbValuation.Checked = true;
        }
        department();
    }

    protected void ddldepat_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        clear();
        loadvaluationstaff();
    }

    public void loadvaluationstaff()
    {
        try
        {
            FpValuation.ShowHeaderSelection = false;
            FpValuation.Visible = false;
            FpValuation.Sheets[0].RowCount = 0;
            FpValuation.Sheets[0].ColumnCount = 0;
            FpValuation.Sheets[0].ColumnCount = 7;
            FpValuation.Sheets[0].RowHeader.Visible = false;
            FpValuation.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpValuation.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpValuation.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpValuation.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpValuation.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpValuation.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpValuation.Sheets[0].DefaultStyle.Font.Bold = false;
            FpValuation.Sheets[0].Columns[0].Width = 50;
            FpValuation.Sheets[0].Columns[1].Width = 250;
            FpValuation.Sheets[0].Columns[2].Width = 100;
            FpValuation.Sheets[0].Columns[3].Width = 150;
            FpValuation.Sheets[0].Columns[4].Width = 100;
            FpValuation.Sheets[0].Columns[5].Width = 250;
            FpValuation.Sheets[0].Columns[6].Width = 50;

            FpValuation.Sheets[0].Columns[0].Locked = true;
            FpValuation.Sheets[0].Columns[1].Locked = true;
            FpValuation.Sheets[0].Columns[2].Locked = true;
            FpValuation.Sheets[0].Columns[3].Locked = true;
            FpValuation.Sheets[0].Columns[4].Locked = true;
            FpValuation.Sheets[0].Columns[5].Locked = true;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FpValuation.Sheets[0].Columns[0].CellType = txt;
            FpValuation.Sheets[0].Columns[1].CellType = txt;
            FpValuation.Sheets[0].Columns[2].CellType = txt;
            FpValuation.Sheets[0].Columns[3].CellType = txt;
            FpValuation.Sheets[0].Columns[4].CellType = txt;
            FpValuation.Sheets[0].Columns[5].CellType = txt;

            FpValuation.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpValuation.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpValuation.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpValuation.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpValuation.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpValuation.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            FpValuation.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

            FpValuation.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpValuation.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpValuation.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpValuation.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpValuation.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            FpValuation.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            FpValuation.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;

            FpValuation.Sheets[0].Columns[2].Visible = false;

            FpValuation.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpValuation.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            FpValuation.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
            FpValuation.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
            FpValuation.Sheets[0].ColumnHeader.Cells[0, 4].Text = "TYPE";
            FpValuation.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code - Name";
            FpValuation.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";

            FpValuation.Sheets[0].AutoPostBack = false;
            FpValuation.CommandBar.Visible = false;

            string collgr = Session["collegecode"].ToString();
            string strtypeval = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
            }

            string deotcode = "";
            if (ddldepat.Items.Count > 0)
            {
                if (ddldepat.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    deotcode = " and h.dept_code ='" + ddldepat.SelectedValue.ToString() + "'";
                    if (ddlstaream.SelectedItem.ToString() == "External")
                    {
                        deotcode = " and sm.dept_code ='" + ddldepat.SelectedValue.ToString() + "'";
                    }
                }
            }

            string isexternal = "0";
            if (ddlstaream.SelectedItem.ToString() == "External")
            {
                if (rdbValuation.Checked == true)
                {
                    isexternal = "1";
                    ValidationDiv.Visible = true;
                    QuestionDiv.Visible = false;
                }
                if (rdbQsetter.Checked == true)
                {
                    isexternal = "2";
                    ValidationDiv.Visible = false;
                    QuestionDiv.Visible = true;
                }
            }
            if (ddlstaream.SelectedItem.ToString() == "Internal")
            {
                if (rdbValuation.Checked == true)
                {
                    ValidationDiv.Visible = true;
                    QuestionDiv.Visible = false;
                }
                if (rdbQsetter.Checked == true)
                {
                    ValidationDiv.Visible = false;
                    QuestionDiv.Visible = true;
                }
            }
            string spreadbind1 = "select distinct sm.staff_name,sm.staff_code,h.dept_name,Convert(nvarchar(15),sm.join_date,103) as joindas,ev.isexternal from tbl_exam_valuatiuon_staff ev,staffmaster sm,stafftrans st,hrdept_master h,examstaffmaster c where ev.staff_code=sm.staff_code and sm.staff_code=st.staff_code and c.staff_code=ev.staff_code and st.dept_code=h.dept_code and st.latestrec=1 and ev.year='" + ddlYear.SelectedValue.ToString() + "' and ev.MONTH='" + ddlMonth.SelectedValue.ToString() + "' and ev.isexternal='" + isexternal + "'  " + strtypeval + " " + deotcode + " order by  h.dept_name,sm.staff_name";
            if (ddlstaream.SelectedItem.ToString() == "External")
            {
                if (rdbValuation.Checked == true)
                {
                    spreadbind1 = "select distinct sm.staff_name,sm.staff_code,sm.dept_name,ev.isexternal from tbl_exam_valuatiuon_staff ev,external_staff sm,examstaffmaster c where ev.staff_code=Convert(nvarchar(50),sm.staff_code ) and c.staff_code=ev.staff_code and ev.year='" + ddlYear.SelectedValue.ToString() + "' and ev.MONTH='" + ddlMonth.SelectedValue.ToString() + "' and ev.isexternal='" + isexternal + "'  " + strtypeval + " " + deotcode + "  order by  sm.dept_name,sm.staff_name";
                }
                if (rdbQsetter.Checked == true)
                {
                    spreadbind1 = "select distinct sm.staff_name,sm.staff_code,sm.dept_name,ev.isexternal from tbl_exam_valuatiuon_staff ev,external_staff sm,examstaffmaster c where ev.staff_code=Convert(nvarchar(50),sm.staff_code ) and c.staff_code=ev.staff_code and ev.year='" + ddlYear.SelectedValue.ToString() + "' and ev.MONTH='" + ddlMonth.SelectedValue.ToString() + "' and ev.isexternal='" + isexternal + "'  " + strtypeval + " " + deotcode + " order by  sm.dept_name,sm.staff_name";
                }
            }

            spreadbind1 = spreadbind1 + " select distinct t.*,s.subject_name from tbl_exam_valuatiuon_staff t,subject s where t.subject_code=s.subject_code and t.year='" + ddlYear.SelectedValue.ToString() + "' and t.MONTH='" + ddlMonth.SelectedValue.ToString() + "' and t.isexternal='" + isexternal + "' ";
            DataSet ds2 = dt.select_method_wo_parameter(spreadbind1, "Text");

            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;

            FpValuation.Sheets[0].RowCount = FpValuation.Sheets[0].RowCount + 1;
            FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 6].CellType = chkall;
            FpValuation.Sheets[0].SpanModel.Add(FpValuation.Sheets[0].RowCount - 1, 0, 1, 6);

            FpValuation.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpValuation.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpValuation.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpValuation.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpValuation.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

            DataView dvfilterinvi = new DataView();
            int sno = 0;
            int height = 45;
            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtQpBefore.Text = DateTime.Now.ToString("dd/MM/yyyy");
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    FpValuation.Sheets[0].RowCount++;

                    if ((sno % 2) == 0)
                    {
                        FpValuation.Sheets[0].Rows[FpValuation.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                    }
                    string staffname = ds2.Tables[0].Rows[i]["staff_name"].ToString();
                    string staffcode = ds2.Tables[0].Rows[i]["staff_code"].ToString();
                    string department = ds2.Tables[0].Rows[i]["dept_name"].ToString();
                    //string joinda = ds2.Tables[0].Rows[i]["joindas"].ToString();
                    string isexternalval = ds2.Tables[0].Rows[i]["isexternal"].ToString();
                    FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 1].Text = staffname.ToString();
                    FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 2].Text = staffcode.ToString();
                    FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 3].Text = department.ToString();
                    if (isexternalval.Trim() == "1" || isexternalval.Trim().ToLower() == "true")
                    {
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 4].Text = "External";
                    }
                    else if (isexternalval.Trim() == "False")
                    {
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 4].Text = "Internal";
                    }
                    else if (isexternalval.Trim() == "2")
                    {
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 4].Text = "External";
                    }

                    string subname = "";

                    ds2.Tables[1].DefaultView.RowFilter = "staff_code='" + staffcode + "' and isexternal='" + isexternalval + "'";
                    DataView dvvalsub = ds2.Tables[1].DefaultView;



                    for (int s = 0; s < dvvalsub.Count; s++)
                    {
                        string edu_level = dt.GetFunctionv("select distinct c.Edu_Level   from subject S,syllabus_master sy ,Degree d,Course C where s.syll_code =sy.syll_code and D.Degree_Code =sy.degree_code and d.Course_Id =c.Course_Id and s.subject_code ='" + Convert.ToString(dvvalsub[s]["subject_code"]) + "'");
                        if (subname.Trim() == "")
                        {
                            subname = dvvalsub[s]["subject_code"].ToString() + "-" + dvvalsub[s]["subject_name"].ToString();
                        }
                        else
                        {
                            subname = subname + ",\n " + dvvalsub[s]["subject_code"].ToString() + "-" + dvvalsub[s]["subject_name"].ToString();
                        }
                        if (s > 0)
                        {
                            FpValuation.Sheets[0].RowCount++;
                        }
                        if ((sno % 2) == 0)
                        {
                            FpValuation.Sheets[0].Rows[FpValuation.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                        }
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 1].Text = staffname.ToString();
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 2].Text = staffcode.ToString();
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 3].Text = department.ToString();
                        if (isexternalval.Trim() == "1" || isexternalval.Trim().ToLower() == "true")
                        {
                            FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 4].Text = "External";
                        }
                        else if (isexternalval.Trim() == "False")
                        {
                            FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 4].Text = "Internal";
                        }
                        else if (isexternalval.Trim() == "2")
                        {
                            FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 4].Text = "External";
                        }
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 5].Text = subname;
                        FpValuation.Sheets[0].Cells[FpValuation.Sheets[0].RowCount - 1, 6].CellType = chk;
                        subname = "";
                    }
                    height = height + FpValuation.Sheets[0].Rows[i].Height + 25;
                }

                FpValuation.Sheets[0].Columns[5].Visible = true;
                FpValuation.SaveChanges();
                FpValuation.Visible = true;
                lblexcsea.Visible = true;
                txtexseat.Visible = true;
                Excel_seating.Visible = true;
                Print_seating.Visible = true;
                btnaddsubject.Visible = true;
                btnvaluationletter.Visible = true;
                lblvaldate.Visible = true;
                lblvaltodate.Visible = true;
                if (ddlstaream.SelectedItem.ToString() == "External")
                {
                    lblvaldate.Text = "Valuation From Date";
                    txtvaltodate.Visible = true;
                }
                else
                {
                    lblvaltodate.Visible = false;
                    txtvaltodate.Visible = false;
                }
                txtvaldate.Visible = true;
                chkvalsummry.Visible = true;
                btnsubdelete.Visible = true;
                btnDeleteSubjectValue.Visible = true;
                btnGeneratePdf.Visible = true;
                lblQPBefore.Visible = true;
                txtQpBefore.Visible = true;
                cbQValSummary.Visible = true;
                lblmsg.Visible = false;
                FpValuation.Sheets[0].PageSize = FpValuation.Sheets[0].RowCount;
            }
            else
            {
                Printcontrol.Visible = false;
                lblmsg.Visible = false;
                FpValuation.Visible = false;
                Print_seating.Visible = false;
                Excel_seating.Visible = false;
                lblexcsea.Visible = false;
                txtexseat.Visible = false;
                btnvaluationletter.Visible = false;
                lblvaldate.Visible = false;
                lblvaltodate.Visible = false;
                txtvaltodate.Visible = false;
                chkvalsummry.Visible = false;
                btnDeleteSubjectValue.Visible = false;
                btnGeneratePdf.Visible = false;
                lblQPBefore.Visible = false;
                txtQpBefore.Visible = false;
                cbQValSummary.Visible = false;
                lblmsg.Visible = true;
                lblmsg.Text = "No Records Found";
            }


            FpValuation.Sheets[0].PageSize = FpValuation.Sheets[0].RowCount;
            FpValuation.Width = 950;

            Double heighva = 20;
            if (FpValuation.Sheets[0].RowCount > 500)
            {
                heighva = 950;
            }
            else
            {
                heighva = 400;
            }
            heighva = Math.Round(heighva, 0, MidpointRounding.AwayFromZero);
            heighva = heighva + 20;
            FpValuation.Height = Convert.ToInt32(heighva);
            FpValuation.Width = 950;
            FpValuation.SaveChanges();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void FpValuation_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = e.SheetView.ActiveRow.ToString();
            if (flag_true == false && actrow == "0")
            {
                int s = Convert.ToInt16(FpValuation.Sheets[0].Cells[0, 6].Value);

                for (int j = 1; j < Convert.ToInt16(FpValuation.Sheets[0].RowCount); j++)
                {
                    FpValuation.Sheets[0].Cells[j, 6].Value = s;
                }
                flag_true = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void printseating_click(object sender, EventArgs e)
    {
        try
        {
            string pagename = "hallwisestudentcount.aspx";
            string degreedetails = "";
            Printcontrol.loadspreaddetails(FpValuation, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void Excelseating_click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexseat.Text;
            if (report.ToString().Trim() != "")
            {
                dt.printexcelreport(FpValuation, report);
                lblmsg.Visible = false;
            }
            else
            {
                lblmsg.Text = "Please Enter Your Report Name";
                lblmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void btnaddsubject_click(object sender, EventArgs e)
    {
        try
        {
            Panelsubject.Visible = true;
            if (rdbQsetter.Checked == true)
            {
                loadsubcategory1();
                loadsubjectvalueation1();
                loadstaffvalpane();
            }
            else
            {
                loadsubcategory();
                loadsubjectvalueation();
                loadstaffvalpane();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            Panelsubject.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    public void loadsubcategory()
    {
        try
        {
            ddlsubcatrory.Items.Clear();
            string strtypeval = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtypeval = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtypeval = "and c.type in('Day','MCA')";
                }
            }
            string strsubcategoryquery = "select distinct ss.subject_type from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,degree d,Course c";
            strsubcategoryquery = strsubcategoryquery + " where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ed.degree_code=d.Degree_Code";
            strsubcategoryquery = strsubcategoryquery + " and d.Course_Id=c.Course_Id  and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' " + strtypeval + " order by ss.subject_type ";
            DataSet dscate = dt.select_method_wo_parameter(strsubcategoryquery, "Text");
            if (dscate.Tables[0].Rows.Count > 0)
            {
                ddlsubcatrory.DataSource = dscate;
                ddlsubcatrory.DataTextField = "subject_type";
                ddlsubcatrory.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblserror.Text = ex.ToString();
            lblserror.Visible = true;
        }
    }

    public void loadsubcategory1()
    {
        try
        {
            ddlsubcatrory.Items.Clear();
            string strtypeval = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtypeval = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtypeval = "and c.type in('Day','MCA')";
                }
            }
            string Value = "";
            if (ddldepat.SelectedItem.Text.Trim() != "All")
            {
                Value = " and d.Degree_Code in ('" + ddldepat.SelectedItem.Value + "')";
            }
            string strsubcategoryquery = " select distinct subject_type  from subject s,syllabus_master y ,sub_sem sm ,Degree d,Course c where s.syll_code  = y.syll_code and s.subType_no =sm.subType_no and d.Degree_Code =y.degree_code and c.Course_Id =d.Course_Id  " + Value + "  " + strtypeval + "";
            DataSet dscate = dt.select_method_wo_parameter(strsubcategoryquery, "Text");
            if (dscate.Tables[0].Rows.Count > 0)
            {
                ddlsubcatrory.DataSource = dscate;
                ddlsubcatrory.DataTextField = "subject_type";
                ddlsubcatrory.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblserror.Text = ex.ToString();
            lblserror.Visible = true;
        }
    }

    public void loadsubjectvalueation()
    {
        try
        {
            ddlsubject.Items.Clear();
            string strtypeval = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtypeval = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtypeval = "and c.type in('Day','MCA')";
                }
            }
            if (ddlsubcatrory.Items.Count > 0)
            {
                string strsubcategoryquery = "select distinct s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code subcodename from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,degree d,Course c";
                strsubcategoryquery = strsubcategoryquery + " where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ed.degree_code=d.Degree_Code";
                strsubcategoryquery = strsubcategoryquery + " and d.Course_Id=c.Course_Id  and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubcatrory.SelectedItem.ToString() + "' " + strtypeval + " order by s.subject_name,s.subject_code desc ";
                DataSet dscate = dt.select_method_wo_parameter(strsubcategoryquery, "Text");
                if (dscate.Tables[0].Rows.Count > 0)
                {
                    ddlsubject.DataSource = dscate;
                    ddlsubject.DataTextField = "subcodename";
                    ddlsubject.DataValueField = "subject_code";
                    ddlsubject.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblserror.Text = ex.ToString();
            lblserror.Visible = true;
        }
    }

    public void loadsubjectvalueation1()
    {
        try
        {
            ddlsubject.Items.Clear();
            string strtypeval = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtypeval = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtypeval = "and c.type in('Day','MCA')";
                }
            }
            string Value = "";
            if (ddldepat.SelectedItem.Text.Trim() != "All")
            {
                Value = " and d.Degree_Code in ('" + ddldepat.SelectedItem.Value + "')";
            }
            if (ddlsubcatrory.Items.Count > 0)
            {
                string strsubcategoryquery = " select distinct subject_code,(subject_name+' - '+subject_code)as subject_name  from subject s,syllabus_master y ,sub_sem sm ,Degree d,Course c where s.syll_code  = y.syll_code and s.subType_no =sm.subType_no and d.Degree_Code =y.degree_code and c.Course_Id =d.Course_Id and sm.subject_type='" + ddlsubcatrory.SelectedItem.Text + "' " + strtypeval + " " + Value + " order by subject_name,s.subject_code desc ";
                DataSet dscate = dt.select_method_wo_parameter(strsubcategoryquery, "Text");
                if (dscate.Tables[0].Rows.Count > 0)
                {
                    ddlsubject.DataSource = dscate;
                    ddlsubject.DataTextField = "subject_name";
                    ddlsubject.DataValueField = "subject_code";
                    ddlsubject.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblserror.Text = ex.ToString();
            lblserror.Visible = true;
        }
    }

    public void loadstaffvalpane()
    {
        try
        {
            ddlvalstaff.Items.Clear();
            string strtypeval = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
            }

            string deotcode = "";
            if (ddldepat.Items.Count > 0)
            {
                if (ddldepat.SelectedItem.ToString().Trim().ToLower() != "all")
                {
                    deotcode = " and h.dept_code ='" + ddldepat.SelectedValue.ToString() + "'";
                }
            }
            string strsubcategoryquery = "select sm.staff_name,sm.staff_code from examstaffmaster c,staffmaster sm,stafftrans st,hrdept_master h,staffcategorizer sc where c.staff_code=sm.staff_code and sm.staff_code=st.staff_code  and st.dept_code=h.dept_code and sc.category_code=st.category_code and st.latestrec=1 and c.Valuation=1 " + strtypeval + " " + deotcode + " and sc.category_name like 'Teaching%'  order by  sm.staff_name";
            if (ddlstaream.SelectedItem.ToString() == "External")
            {
                if (rdbValuation.Checked == true)
                {
                    strsubcategoryquery = "select distinct c.staff_code,h.staff_name from external_staff h,examstaffmaster c where h.staff_code=c.staff_code and c.isexternal=1 " + strtypeval + " " + deotcode + "  order by h.staff_name";
                }
                if (rdbQsetter.Checked == true)
                {
                    strsubcategoryquery = "select distinct c.staff_code,(h.staff_name+' - '+ designation +' - '+ dept_name) as staff_name from external_staff h,examstaffmaster c where h.staff_code=c.staff_code and c.isexternal=1 " + strtypeval + " order by staff_name";
                }
            }

            DataSet dscate = dt.select_method_wo_parameter(strsubcategoryquery, "Text");
            if (dscate.Tables[0].Rows.Count > 0)
            {
                ddlvalstaff.DataSource = dscate;
                ddlvalstaff.DataTextField = "staff_name";
                ddlvalstaff.DataValueField = "staff_code";
                ddlvalstaff.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblserror.Text = ex.ToString();
            lblserror.Visible = true;
        }
    }

    protected void ddlsubcatrory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbQsetter.Checked == true)
            {
                loadsubjectvalueation1();
            }
            else
            {
                loadsubjectvalueation();
            }
        }
        catch (Exception ex)
        {
            lblserror.Text = ex.ToString();
            lblserror.Visible = true;
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbValuation.Checked == true)
            {
                string isexternal = "0";
                if (ddlstaream.SelectedItem.ToString() == "External")
                {
                    isexternal = "1";
                }
                if (ddlvalstaff.Items.Count == 0)
                {
                    lblserror.Visible = true;
                    lblserror.Text = "Please Allot The Staff And Then Proceed";
                    return;
                }
                string staffcode = ddlvalstaff.SelectedValue.ToString();
                string subcode = ddlsubject.SelectedValue.ToString();

                string staffchecquery = "select * from tbl_exam_valuatiuon_staff where year='" + ddlYear.SelectedItem.ToString() + "' and isexternal='" + isexternal + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and staff_code='" + staffcode + "' and subject_code='" + subcode + "'";
                DataSet dsstaffsub = dt.select_method_wo_parameter(staffchecquery, "Text");
                if (dsstaffsub.Tables[0].Rows.Count > 0)
                {
                    lblserror.Visible = true;
                    lblserror.Text = "The Staff Already Exists That Subject";
                    return;
                }

                string edate = null;
                string esession = null;
                string strexamdteaqyer = "select distinct et.exam_date,et.exam_session from exmtt e,exmtt_det et,subject s where et.exam_code=e.exam_code and et.subject_no=s.subject_no and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_code='" + subcode + "'";
                DataSet dsexamdatequery = dt.select_method_wo_parameter(strexamdteaqyer, "Text");
                if (dsexamdatequery.Tables[0].Rows.Count > 0)
                {
                    edate = dsexamdatequery.Tables[0].Rows[0]["exam_date"].ToString();
                    esession = dsexamdatequery.Tables[0].Rows[0]["exam_session"].ToString();
                }

                string strinsupdatequery = "if not exists(select * from tbl_exam_valuatiuon_staff where year='" + ddlYear.SelectedItem.ToString() + "' and isexternal='" + isexternal + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and staff_code='" + staffcode + "' and subject_code='" + subcode + "')";
                strinsupdatequery = strinsupdatequery + " insert into tbl_exam_valuatiuon_staff(year,month,isexternal,staff_code,subject_code,edate,session)";
                strinsupdatequery = strinsupdatequery + "values('" + ddlYear.SelectedItem.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + isexternal + "','" + staffcode + "','" + subcode + "','" + edate + "','" + esession + "')";
                int a = dt.update_method_wo_parameter(strinsupdatequery, "Text");

                loadvaluationstaff();
                lblserror.Visible = true;
                lblserror.ForeColor = Color.Green;
                lblserror.Text = "Saved Successfully";
            }
            if (rdbQsetter.Checked == true)
            {
                string isexternal = "2";
                //if (ddlstaream.SelectedItem.ToString() == "External")
                //{
                //    isexternal = "1";
                //}
                if (ddlvalstaff.Items.Count == 0)
                {
                    lblserror.Visible = true;
                    lblserror.Text = "Please Allot The Staff And Then Proceed";
                    return;
                }
                string staffcode = ddlvalstaff.SelectedValue.ToString();
                string subcode = ddlsubject.SelectedValue.ToString();

                string staffchecquery = "select * from tbl_exam_valuatiuon_staff where year='" + ddlYear.SelectedItem.ToString() + "' and isexternal='" + isexternal + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and staff_code='" + staffcode + "' and subject_code='" + subcode + "'";
                DataSet dsstaffsub = dt.select_method_wo_parameter(staffchecquery, "Text");
                if (dsstaffsub.Tables[0].Rows.Count > 0)
                {
                    lblserror.Visible = true;
                    lblserror.Text = "The Staff Already Exists That Subject";
                    return;
                }

                string edate = "null";
                string esession = "null";
                string strexamdteaqyer = "select distinct et.exam_date,et.exam_session from exmtt e,exmtt_det et,subject s where et.exam_code=e.exam_code and et.subject_no=s.subject_no and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_code='" + subcode + "'";
                DataSet dsexamdatequery = dt.select_method_wo_parameter(strexamdteaqyer, "Text");
                if (dsexamdatequery.Tables[0].Rows.Count > 0)
                {
                    edate = dsexamdatequery.Tables[0].Rows[0]["exam_date"].ToString();
                    esession = dsexamdatequery.Tables[0].Rows[0]["exam_session"].ToString();
                }

                string strinsupdatequery = "if not exists(select * from tbl_exam_valuatiuon_staff where year='" + ddlYear.SelectedItem.ToString() + "' and isexternal='" + isexternal + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and staff_code='" + staffcode + "' and subject_code='" + subcode + "')";
                strinsupdatequery = strinsupdatequery + " insert into tbl_exam_valuatiuon_staff(year,month,isexternal,staff_code,subject_code)";
                strinsupdatequery = strinsupdatequery + "values('" + ddlYear.SelectedItem.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + isexternal + "','" + staffcode + "','" + subcode + "')";
                int a = dt.update_method_wo_parameter(strinsupdatequery, "Text");

                loadvaluationstaff();
                lblserror.Visible = true;
                lblserror.ForeColor = Color.Green;
                lblserror.Text = "Saved Successfully";

            }
        }
        catch (Exception ex)
        {
            lblserror.Text = ex.ToString();
            lblserror.Visible = true;
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            string isexternal = "0";
            if (ddlstaream.SelectedItem.ToString() == "External")
            {
                isexternal = "1";
            }
            string staffcode = ddlvalstaff.SelectedValue.ToString();
            string subcode = ddlsubject.SelectedValue.ToString();

            string staffchecquery = "select * from tbl_exam_valuatiuon_staff where year='" + ddlYear.SelectedItem.ToString() + "' and isexternal='" + isexternal + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and staff_code='" + staffcode + "' and subject_code='" + subcode + "'";
            DataSet dsstaffsub = dt.select_method_wo_parameter(staffchecquery, "Text");
            if (dsstaffsub.Tables[0].Rows.Count == 0)
            {
                lblserror.Visible = true;
                lblserror.Text = "Subject Valuation Can't Delete.Because Staff not allotted for this subject!!!!!!!!!!!!!!!!!!!";
                return;
            }


            string strinsupdatequery = "Delete from tbl_exam_valuatiuon_staff where year='" + ddlYear.SelectedItem.ToString() + "' and isexternal='" + isexternal + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and staff_code='" + staffcode + "' and subject_code='" + subcode + "'";
            int a = dt.update_method_wo_parameter(strinsupdatequery, "Text");

            loadvaluationstaff();

            lblserror.Visible = true;
            lblserror.ForeColor = Color.Green;
            lblserror.Text = "Delete Successfully";
        }
        catch (Exception ex)
        {
            lblserror.Text = ex.ToString();
            lblserror.Visible = true;
        }
    }

    protected void btnvaluationletter_click(object sender, EventArgs e)
    {
        bindValuationLetter();
    }

    protected void btnsubdelete_click(object sender, EventArgs e)
    {
        try
        {
            FpValuation.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpValuation.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpValuation.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    string staffnbame = FpValuation.Sheets[0].Cells[res, 1].Text.ToString().Trim();
                    string staffcode = FpValuation.Sheets[0].Cells[res, 2].Text.ToString().Trim();
                    string department = FpValuation.Sheets[0].Cells[res, 3].Text.ToString().Trim();
                    string valsubdetails = FpValuation.Sheets[0].Cells[res, 5].Text.ToString().Trim();
                    string type = FpValuation.Sheets[0].Cells[res, 4].Text.ToString().Trim();
                    string[] spsub = valsubdetails.Split('-');
                    string subcode = spsub[0].Trim();
                    string isexternal = "0";
                    if (ddlstaream.SelectedItem.ToString() == "External")
                    {
                        isexternal = "1";
                    }
                    string strinsupdatequery = "Delete from tbl_exam_valuatiuon_staff where year='" + ddlYear.SelectedItem.ToString() + "' and isexternal='" + isexternal + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and staff_code='" + staffcode + "' and subject_code='" + subcode + "'";
                    int a = dt.update_method_wo_parameter(strinsupdatequery, "Text");
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Please Select the Staff and then Proceed";
            }
            else
            {
                loadvaluationstaff();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    public void bindValuationLetter()
    {
        try
        {
            FpValuation.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res < Convert.ToInt32(FpValuation.Sheets[0].RowCount); res++)
            {
                int isval = Convert.ToInt32(FpValuation.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {

                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Please Select the Staff and then Proceed";
                return;
            }

            string valdate = txtvaldate.Text.ToString();

            Font Fontboldbig = new Font("Book Antiqua", 21, FontStyle.Bold);
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontboldd = new Font("Book Antiqua", 17, FontStyle.Regular);
            Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Bold);
            Font Fontsmall1 = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Bold);
            Font Fontsmall12 = new Font("Book Antiqua", 11, FontStyle.Bold);
            Font Fontsmall122 = new Font("Book Antiqua", 11, FontStyle.Regular);

            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();


            string examyear = ddlYear.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            Boolean halfflag = false;

            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                string strtypeval = "";
                if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
                {
                    strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                string isexternal = "0";
                if (ddlstaream.SelectedItem.ToString() == "External")
                {
                    isexternal = "1";
                }
                string deotcode = "";
                if (ddldepat.Items.Count > 0)
                {
                    if (ddldepat.SelectedItem.ToString().Trim().ToLower() != "all")
                    {
                        deotcode = " and h.dept_code ='" + ddldepat.SelectedValue.ToString() + "'";
                        if (ddlstaream.SelectedItem.ToString() == "External")
                        {
                            deotcode = " and sm.dept_code ='" + ddldepat.SelectedValue.ToString() + "'";
                        }
                    }
                }

                string spreadbind1 = "select distinct c.staff_code,c.isexternal,h.dept_name,d.desig_name,s.subject_name,s.subject_code,t.edate,t.session,convert(nvarchar(15),t.edate,103) examdate from examstaffmaster c,tbl_exam_valuatiuon_staff t,subject s,stafftrans st,hrdept_master h,desig_master d";
                spreadbind1 = spreadbind1 + " where c.staff_code=t.staff_code and t.subject_code=s.subject_code and st.staff_code=t.staff_code and st.staff_code=c.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and t.isexternal=c.isexternal and st.latestrec=1 and c.Valuation=1 and t.isexternal=0 and t.year='" + ddlYear.SelectedValue.ToString() + "' and t.month='" + ddlMonth.SelectedValue.ToString() + "' " + strtypeval + " " + deotcode + "  order by c.staff_code,t.edate,t.session desc";
                if (ddlstaream.SelectedItem.ToString() == "External")
                {
                    spreadbind1 = "select distinct sm.staff_name,sm.title,sm.staff_code,sm.dept_name,ev.isexternal,sm.designation desig_name,s.subject_name,s.subject_code,ev.edate,ev.session,convert(nvarchar(15),ev.edate,103) examdate from tbl_exam_valuatiuon_staff ev,external_staff sm,examstaffmaster c,subject s where ev.staff_code=Convert(nvarchar(50),sm.staff_code ) and c.staff_code=ev.staff_code and s.subject_code=ev.subject_code and ev.year='" + ddlYear.SelectedValue.ToString() + "' and ev.MONTH='" + ddlMonth.SelectedValue.ToString() + "' and ev.isexternal='" + isexternal + "'  " + strtypeval + " " + deotcode + "  order by  sm.dept_name,sm.staff_name";
                }
                DataSet ds2 = dt.select_method_wo_parameter(spreadbind1, "Text");


                string strsubstustrenth = "select s.subject_code,count(ea.roll_no) stucount from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' group by s.subject_code";
                DataSet dssubstu = dt.select_method_wo_parameter(strsubstustrenth, "Text");

                string strcolldetails = " select * from collinfo";
                DataSet dshall = dt.select_method_wo_parameter(strcolldetails, "Text");
                string collname = "";
                string address = "";
                string pincode = "";
                string university = "";
                string category = "";
                string coename = "";
                string addval = "";
                if (dshall.Tables[0].Rows.Count > 0)
                {
                    collname = dshall.Tables[0].Rows[0]["collname"].ToString();
                    string ad1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                    string ad2 = dshall.Tables[0].Rows[0]["address2"].ToString();
                    string ad3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                    university = dshall.Tables[0].Rows[0]["university"].ToString();
                    category = dshall.Tables[0].Rows[0]["category"].ToString();
                    pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                    coename = dshall.Tables[0].Rows[0]["coe"].ToString();
                    if (ad1 != "" && ad1 != null)
                    {
                        address = ad1.Trim(',');
                    }
                    //if (ad2 != "" && ad2 != null)
                    //{
                    //    if (address == "")
                    //    {
                    //        address = ad2;
                    //    }
                    //    else
                    //    {
                    //        address = address + ", " + ad2;
                    //    }
                    //}
                    if (ad3 != "" && ad3 != null)
                    {
                        if (address == "")
                        {
                            address = ad3;
                        }
                        else
                        {
                            address = address + "," + ad3;
                        }
                    }
                    if (pincode != "" && pincode != null)
                    {
                        if (address == "")
                        {
                            address = pincode;
                        }
                        else
                        {
                            address = address + " - " + pincode;
                        }
                    }
                }

                DataSet supplymsubds = new DataSet();
                Hashtable hatstaff = new Hashtable();
                string strsupplymsub = "";
                for (int res = 1; res < FpValuation.Sheets[0].RowCount; res++)
                {
                    Double coltop = 0;
                    int isval = Convert.ToInt32(FpValuation.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {

                        string staffnbame = FpValuation.Sheets[0].Cells[res, 1].Text.ToString();
                        string staffcode = FpValuation.Sheets[0].Cells[res, 2].Text.ToString();
                        string department = FpValuation.Sheets[0].Cells[res, 3].Text.ToString();
                        string valsubdetails = FpValuation.Sheets[0].Cells[res, 5].Text.ToString();
                        string type = FpValuation.Sheets[0].Cells[res, 4].Text.ToString();
                        if (!hatstaff.Contains(staffcode))
                        {
                            hatstaff.Add(staffcode, staffcode);
                            ds2.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                            DataView dvstaffde = ds2.Tables[0].DefaultView;
                            if (dvstaffde.Count > 0)
                            {
                                string designation = dvstaffde[0]["desig_name"].ToString();
                                if (type.Trim().ToLower() == "internal")
                                {
                                }
                                else
                                {
                                    staffnbame = dvstaffde[0]["title"].ToString() + ' ' + staffnbame;
                                }
                                mypdfpage = mydocument.NewPage();
                                halfflag = true;
                                if (chkvalsummry.Checked == false)
                                {

                                    coltop = coltop + 10;
                                    PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                    mypdfpage.Add(ptc);


                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                    mypdfpage.Add(ptc);


                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 30, 10, 500);
                                    }

                                    //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                    //{
                                    //    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    //    mypdfpage.Add(leftimage, 740, 10, 500);
                                    //}


                                    coltop = coltop + 40;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, coename);
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 680, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date :" + DateTime.Now.ToString("dd/MM/yyyy"));
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 40;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "To");
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 15;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, staffnbame);
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 15;
                                    if (type.Trim().ToLower() == "internal")
                                    {
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, designation + " Dept. of " + department);
                                        mypdfpage.Add(ptc);
                                    }

                                    coltop = coltop + 15;
                                    if (type.Trim().ToLower() == "internal")
                                    {
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, collname);
                                        mypdfpage.Add(ptc);
                                    }
                                    else
                                    {
                                        coltop = coltop + 40;
                                    }

                                    coltop = coltop + 30;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Dear Sir/Madam,");
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 30;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Sub : End of Semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString());
                                    mypdfpage.Add(ptc);



                                    coltop = coltop + 15;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 56, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, " Valuation of Answer paper-Appointment of " + ddlstaream.SelectedItem.ToString().ToUpper() + " Examiner - Reg.");
                                    mypdfpage.Add(ptc);


                                    coltop = coltop + 40;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "We are happy to inform you that you are appointed as an Examiner to value the following paper(s) of the semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString());
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 30;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "S.No ");
                                    mypdfpage.Add(ptc);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 100, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Paper Code");
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 200, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Paper");
                                    mypdfpage.Add(ptc);
                                    if (type.Trim().ToLower() == "internal")
                                    {

                                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mydocument, 550, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Exam Date & Session");
                                        mypdfpage.Add(ptc);
                                    }

                                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                         new PdfArea(mydocument, 700, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Max.No.of Scripts");
                                    //mypdfpage.Add(ptc);

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);


                                    string[] spvalsu = valsubdetails.Split(',');

                                    int sbbno = 0;
                                    for (int es = 0; es < dvstaffde.Count; es++)
                                    {
                                        sbbno++;
                                        coltop = coltop + 20;

                                        string scode = dvstaffde[es]["subject_code"].ToString();
                                        string sname = dvstaffde[es]["subject_name"].ToString();
                                        string exmdate = dvstaffde[es]["examdate"].ToString();
                                        string esess = dvstaffde[es]["session"].ToString();

                                        string stucount = "0";
                                        dssubstu.Tables[0].DefaultView.RowFilter = "subject_code='" + scode + "'";
                                        DataView dvcount = dssubstu.Tables[0].DefaultView;
                                        if (dvcount.Count > 0)
                                        {
                                            stucount = dvcount[0]["stucount"].ToString();
                                        }

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 60, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sbbno.ToString());
                                        mypdfpage.Add(ptc);

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 95, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, scode);
                                        mypdfpage.Add(ptc);

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 200, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sname);
                                        mypdfpage.Add(ptc);
                                        if (type.Trim().ToLower() == "internal")
                                        {
                                            ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 580, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, exmdate + " - " + esess);
                                            mypdfpage.Add(ptc);

                                            //ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                            //                                     new PdfArea(mydocument, 720, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, stucount);
                                            //mypdfpage.Add(ptc);
                                        }

                                    }

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);


                                    if (type.Trim().ToLower() == "internal")
                                    {
                                        coltop = coltop + 30;
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "The valuation is to be done in the Examinations Office. Answer scripts will be made available from the fifth day  after the examination is held in that particular paper.");
                                        mypdfpage.Add(ptc);

                                        coltop = coltop + 30;
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Tea will be served at 10.30 am and at 3 pm during the central valuation period.kindly complete the valuation by " + valdate + ".");
                                        mypdfpage.Add(ptc);

                                        coltop = coltop + 30;
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Looking forward to your kind co-operation .");
                                        mypdfpage.Add(ptc);
                                    }
                                    else
                                    {
                                        coltop = coltop + 30;
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "The Valuation is to be done in the Examinations Office.You will be required to value 40 UG papers or 30 PG papers per day.");
                                        mypdfpage.Add(ptc);

                                        coltop = coltop + 30;
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "The valuation timings are 9.30 am to 12.30 pm and 1.30 pm to 4.30 pm.");
                                        mypdfpage.Add(ptc);

                                        coltop = coltop + 30;
                                        //ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                            new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Tea will be provided everyday at 10.30 am and 3.00 pm. Lunch will be provided everyday at 12.45 pm. T.A and D.A. will be paid basically according to the university norms to all the examiners first class suburban train fare only.");
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Tea will be provided everyday at 10.30 am and 3.00 pm. Lunch will be provided everyday at 12.45 pm. T.A and D.A. will be paid basically according to the University Of Madras norms.");
                                        mypdfpage.Add(ptc);

                                        coltop = coltop + 40;
                                        //ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                            new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Please let us know urgently by return post/through the messenger your acceptance of this appointment I the enclosed form and attend the valuation work on any suitable day from " + valdate + " to " + txtvaltodate.Text.ToString() + ".");
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Please let us know  your acceptance of this appointment by post or through messanger. I have enclosed acceptance form and kindly attend to valuation work on any suitable day(s) from " + valdate + " to " + txtvaltodate.Text.ToString() + ".");
                                        mypdfpage.Add(ptc);

                                        coltop = coltop + 40;
                                        //ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                            new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "If you are unable to accept this appointment. We shall appreciate the return of the enclosures accompanying this letter immediately through post/messenger.");
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "If you are unable to accept this appointment, kindly intimate the same immediately.");
                                        mypdfpage.Add(ptc);

                                        coltop = coltop + 30;
                                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "With regards");
                                        mypdfpage.Add(ptc);


                                    }

                                    coltop = coltop + 30;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Yours sincerely,");
                                    mypdfpage.Add(ptc);

                                    MemoryStream memoryStream1 = new MemoryStream();
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg")))
                                    {
                                        if (dshall.Tables[0].Rows[0]["coe_signature"] != null && dshall.Tables[0].Rows[0]["coe_signature"].ToString().Trim() != "")
                                        {
                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg")))
                                            {
                                                byte[] file = (byte[])dshall.Tables[0].Rows[0]["coe_signature"];
                                                memoryStream1.Write(file, 0, file.Length);
                                                if (file.Length > 0)
                                                {
                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream1, true, true);
                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(1000, 400, null, IntPtr.Zero);
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                                memoryStream1.Dispose();
                                                memoryStream1.Close();
                                            }
                                        }
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg"));
                                        mypdfpage.Add(LogoImage, 60, coltop + 35, 500);
                                    }
                                    coltop = coltop + 80;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");
                                    mypdfpage.Add(ptc);


                                }
                                else
                                {
                                    coltop = coltop + 10;
                                    PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                    mypdfpage.Add(ptc);


                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                    mypdfpage.Add(ptc);


                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 30, 10, 500);
                                    }

                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                    {
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                        mypdfpage.Add(leftimage, 740, 10, 500);
                                    }

                                    coltop = coltop + 40;
                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "LIST OF " + ddlstaream.SelectedItem.ToString().ToUpper() + " EXAMINARS FOR THE E.S.E " + ddlMonth.SelectedItem.ToString() + "-" + ddlYear.SelectedItem.ToString() + "  [" + ddltype.SelectedItem.ToString().ToUpper() + "]");
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 20;
                                    if (type.Trim().ToLower() == "internal")
                                    {
                                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Department : " + department + "");
                                        mypdfpage.Add(ptc);
                                    }


                                    coltop = coltop + 15;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "S.No ");
                                    mypdfpage.Add(ptc);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 100, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Examiner");
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 400, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Paper Code");
                                    mypdfpage.Add(ptc);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 500, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Paper");
                                    mypdfpage.Add(ptc);

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    int sbbno = 0;
                                    for (int es = 0; es < dvstaffde.Count; es++)
                                    {
                                        sbbno++;
                                        coltop = coltop + 20;

                                        string scode = dvstaffde[es]["subject_code"].ToString();
                                        string sname = dvstaffde[es]["subject_name"].ToString();
                                        string exmdate = dvstaffde[es]["examdate"].ToString();
                                        string esess = dvstaffde[es]["session"].ToString();

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 60, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sbbno.ToString());
                                        mypdfpage.Add(ptc);
                                        if (es == 0)
                                        {
                                            ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, 90, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, staffnbame);
                                            mypdfpage.Add(ptc);
                                        }

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 400, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, scode);
                                        mypdfpage.Add(ptc);

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 500, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sname);
                                        mypdfpage.Add(ptc);

                                    }

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);
                                }
                                mypdfpage.SaveToDocument();
                                lblmsg.Visible = false;
                            }
                        }
                    }
                }
                if (halfflag == true)
                {
                    lblmsg.Visible = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "ExamHallTicket.pdf";
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
                else
                {
                    lblmsg.Text = "Please Select the Student and then Proceed";
                    lblmsg.Visible = true;
                }
            }
            else
            {
                lblmsg.Text = "Please Select Exam Month And Year";
                lblmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void btnGeneratePdf_click(object sender, EventArgs e)
    {
        printPDf();
    }

    protected void btnDeleteSubjectValue_click(object sender, EventArgs e)
    {
        try
        {
            FpValuation.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpValuation.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpValuation.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    string staffnbame = FpValuation.Sheets[0].Cells[res, 1].Text.ToString().Trim();
                    string staffcode = FpValuation.Sheets[0].Cells[res, 2].Text.ToString().Trim();
                    string department = FpValuation.Sheets[0].Cells[res, 3].Text.ToString().Trim();
                    string valsubdetails = FpValuation.Sheets[0].Cells[res, 5].Text.ToString().Trim();
                    string type = FpValuation.Sheets[0].Cells[res, 4].Text.ToString().Trim();
                    string[] spsub = valsubdetails.Split('-');
                    string subcode = spsub[0].Trim();
                    string isexternal = "0";
                    if (ddlstaream.SelectedItem.ToString() == "External")
                    {
                        isexternal = "2";
                    }
                    string strinsupdatequery = "Delete from tbl_exam_valuatiuon_staff where year='" + ddlYear.SelectedItem.ToString() + "' and isexternal='" + isexternal + "' and month='" + ddlMonth.SelectedValue.ToString() + "' and staff_code='" + staffcode + "' and subject_code='" + subcode + "'";
                    int a = dt.update_method_wo_parameter(strinsupdatequery, "Text");
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Please Select the Staff and then Proceed";
            }
            else
            {
                loadvaluationstaff();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    public void printPDf()
    {

        try
        {
            FpValuation.SaveChanges();
            int selectedcount = 0;
            lblmsg.Visible = false;
            lblmsg.Text = "";
            string QpBefore = "";
            for (int res = 1; res <= Convert.ToInt32(FpValuation.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpValuation.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Please Select the Staff and then Proceed";
                return;
            }

            if (txtQpBefore.Text != "")
            {
                QpBefore = txtQpBefore.Text;
            }
            else
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Please Enter Question Paper Send Before Date";
                return;
            }
            string valdate = txtvaldate.Text.ToString();

            Font Fontboldbig = new Font("Book Antiqua", 21, FontStyle.Bold);
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontboldd = new Font("Book Antiqua", 17, FontStyle.Regular);
            Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Bold);
            Font Fontsmall1 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Bold);
            Font Fontsmall12 = new Font("Book Antiqua", 11, FontStyle.Bold);
            Font Fontsmall122 = new Font("Book Antiqua", 12, FontStyle.Regular);

            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();


            string examyear = ddlYear.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            Boolean halfflag = false;

            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                string strtypeval = "";
                if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
                {
                    strtypeval = " and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                string isexternal = "0";
                if (ddlstaream.SelectedItem.ToString() == "External")
                {
                    isexternal = "2";
                }
                string deotcode = "";
                if (ddldepat.Items.Count > 0)
                {
                    if (ddldepat.SelectedItem.ToString().Trim().ToLower() != "all")
                    {
                        deotcode = " and h.dept_code ='" + ddldepat.SelectedValue.ToString() + "'";
                        if (ddlstaream.SelectedItem.ToString() == "External")
                        {
                            deotcode = " and sm.dept_code ='" + ddldepat.SelectedValue.ToString() + "'";
                        }
                    }
                }

                string spreadbind1 = "select distinct c.staff_code,c.isexternal,h.dept_name,d.desig_name,s.subject_name,s.subject_code,t.edate,t.session,convert(nvarchar(15),t.edate,103) examdate from examstaffmaster c,tbl_exam_valuatiuon_staff t,subject s,stafftrans st,hrdept_master h,desig_master d";
                spreadbind1 = spreadbind1 + " where c.staff_code=t.staff_code and t.subject_code=s.subject_code and st.staff_code=t.staff_code and st.staff_code=c.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec=1 and c.Valuation=1 and c.isexternal=0 and t.year='" + ddlYear.SelectedValue.ToString() + "' and t.month='" + ddlMonth.SelectedValue.ToString() + "' " + strtypeval + " " + deotcode + "  order by c.staff_code,t.edate,t.session desc";
                if (ddlstaream.SelectedItem.ToString() == "External")
                {
                    spreadbind1 = "select distinct sm.staff_name,sm.title,sm.staff_code,sm.dept_name,ev.isexternal,sm.designation desig_name,s.subject_name,s.subject_code,ev.edate,ev.session,convert(nvarchar(15),ev.edate,103) examdate from tbl_exam_valuatiuon_staff ev,external_staff sm,examstaffmaster c,subject s where ev.staff_code=Convert(nvarchar(50),sm.staff_code ) and c.staff_code=ev.staff_code and s.subject_code=ev.subject_code and ev.year='" + ddlYear.SelectedValue.ToString() + "' and ev.MONTH='" + ddlMonth.SelectedValue.ToString() + "' and ev.isexternal='" + isexternal + "'  " + strtypeval + " " + deotcode + "  order by  sm.dept_name,sm.staff_name";
                }
                DataSet ds2 = dt.select_method_wo_parameter(spreadbind1, "Text");


                string strsubstustrenth = "select s.subject_code,count(ea.roll_no) stucount from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' group by s.subject_code";
                DataSet dssubstu = dt.select_method_wo_parameter(strsubstustrenth, "Text");

                string strcolldetails = " select * from collinfo";
                DataSet dshall = dt.select_method_wo_parameter(strcolldetails, "Text");
                string collname = "";
                string address = "";
                string pincode = "";
                string university = "";
                string category = "";
                string coename = "";
                string phno = "";
                string addval = "";
                if (dshall.Tables[0].Rows.Count > 0)
                {
                    collname = Convert.ToString(dshall.Tables[0].Rows[0]["collname"]);
                    string ad1 = Convert.ToString(dshall.Tables[0].Rows[0]["address1"]);
                    string ad2 = Convert.ToString(dshall.Tables[0].Rows[0]["address2"]);
                    string ad3 = Convert.ToString(dshall.Tables[0].Rows[0]["address3"]);
                    university = Convert.ToString(dshall.Tables[0].Rows[0]["university"]);
                    category = Convert.ToString(dshall.Tables[0].Rows[0]["category"]);
                    pincode = Convert.ToString(dshall.Tables[0].Rows[0]["pincode"]);
                    coename = Convert.ToString(dshall.Tables[0].Rows[0]["coe"]);
                    phno = Convert.ToString(dshall.Tables[0].Rows[0]["phoneno"]);
                    if (ad1.Trim() != "" && ad1.Trim() != null)
                    {
                        address = ad1.Trim();
                    }
                    //if (ad2 != "" && ad2 != null)
                    //{
                    //    if (address == "")
                    //    {
                    //        address = ad2;
                    //    }
                    //    else
                    //    {
                    //        address = address + ", " + ad2;
                    //    }
                    //}
                    if (pincode.Trim() != "" && pincode.Trim() != null)
                    {
                        if (address == "")
                        {
                            address = pincode.Trim();
                        }
                        else
                        {
                            address = address + ' ' + ad3 + " - " + pincode.Trim();
                        }
                    }
                }
                string user_id = string.Empty;
                string ssr = "select * from Track_Value where college_code='" + Convert.ToString(college_code) + "'";
                ds.Clear();
                ds = dt.select_method_wo_parameter(ssr, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                }
                DataSet supplymsubds = new DataSet();
                Hashtable hatstaff = new Hashtable();
                string strsupplymsub = "";
                string SubjectDetails = string.Empty;
                for (int res = 1; res < FpValuation.Sheets[0].RowCount; res++)
                {
                    Double coltop = 0;
                    int isval = Convert.ToInt32(FpValuation.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {
                        string staffnbame = FpValuation.Sheets[0].Cells[res, 1].Text.ToString();
                        string staffcode = FpValuation.Sheets[0].Cells[res, 2].Text.ToString();
                        string department = FpValuation.Sheets[0].Cells[res, 3].Text.ToString();
                        string valsubdetails = FpValuation.Sheets[0].Cells[res, 5].Text.ToString();
                        string type = FpValuation.Sheets[0].Cells[res, 4].Text.ToString();
                        string staffEmail = Convert.ToString(dt.GetFunction("select email from external_staff where staff_code='" + staffcode + "'")).Trim();
                        string StaffMobile = Convert.ToString(dt.GetFunction("select Per_Mobileno from external_staff where staff_code='" + staffcode + "'")).Trim();


                        if (!hatstaff.Contains(staffcode))
                        {
                            hatstaff.Add(staffcode, staffcode);
                            ds2.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                            DataView dvstaffde = ds2.Tables[0].DefaultView;
                            if (dvstaffde.Count > 0)
                            {
                                string designation = dvstaffde[0]["desig_name"].ToString();
                                if (type.Trim().ToLower() == "internal")
                                {
                                }
                                else
                                {
                                    staffnbame = dvstaffde[0]["title"].ToString() + ' ' + staffnbame;
                                }
                                mypdfpage = mydocument.NewPage();

                                //Added by Idhris
                                //Start
                                Gios.Pdf.PdfDocument mySingledocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
                                Gios.Pdf.PdfPage mypdfSinglepage = mySingledocument.NewPage();
                                //end

                                halfflag = true;
                                if (cbQValSummary.Checked == false)
                                {

                                    coltop = coltop + 10;
                                    PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                    mypdfpage.Add(ptc);

                                    PdfTextArea ptcS = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                    mypdfSinglepage.Add(ptcS);


                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                    mypdfSinglepage.Add(ptcS);


                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 30, 10, 500);

                                        PdfImage LogoImageS = mySingledocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfSinglepage.Add(LogoImageS, 30, 10, 500);
                                    }

                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                    {
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                        mypdfpage.Add(leftimage, 740, 10, 500);

                                        PdfImage leftimageS = mySingledocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfSinglepage.Add(leftimage, 740, 10, 500);
                                    }


                                    coltop = coltop + 35;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, coename);
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, coename);
                                    mypdfSinglepage.Add(ptcS);

                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, 680, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Ph : " + phno);
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, 680, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Ph : " + phno);
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 680, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date :" + DateTime.Now.ToString("dd/MM/yyyy"));
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 680, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date :" + DateTime.Now.ToString("dd/MM/yyyy"));
                                    mypdfSinglepage.Add(ptcS);

                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "To");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "To");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, staffnbame);
                                    mypdfpage.Add(ptc);
                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, staffnbame);
                                    mypdfSinglepage.Add(ptcS);


                                    if (type.Trim().ToLower() == "internal")
                                    {
                                        coltop = coltop + 20;
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, designation + " Dept. of " + department);
                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, designation + " Dept. of " + department);
                                        mypdfSinglepage.Add(ptcS);
                                    }

                                    if (type.Trim().ToLower() == "internal")
                                    {
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, collname);
                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, collname);
                                        mypdfSinglepage.Add(ptcS);
                                    }
                                    else
                                    {
                                        coltop = coltop + 20;
                                    }

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Dear Sir/Madam,");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Dear Sir/Madam,");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 38), System.Drawing.ContentAlignment.MiddleLeft, "Sub : End of Semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString() + " Question paper setting - Reg.");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 38), System.Drawing.ContentAlignment.MiddleLeft, "Sub : End of Semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString() + " Question paper setting - Reg.");
                                    mypdfSinglepage.Add(ptcS);



                                    //coltop = coltop + 15;
                                    //ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                    //                                            new PdfArea(mydocument, 56, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, " Valuation of Answer paper-Appointment of " + ddlstaream.SelectedItem.ToString().ToUpper() + " Examiner - Reg.");

                                    //ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                    //                                           new PdfArea(mydocument, 56, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, " Question paper setting - Reg.");
                                    //mypdfpage.Add(ptc);


                                    coltop = coltop + 28;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 80), System.Drawing.ContentAlignment.MiddleLeft, "We are happy to inform you that you are appointed as Question Paper Setter for the following paper(s) for our End Semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString());
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 80), System.Drawing.ContentAlignment.MiddleLeft, "We are happy to inform you that you are appointed as Question Paper Setter for the following paper(s) for our End Semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString());
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 50;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 35), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 35), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "S.No ");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 50, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "S.No ");
                                    mypdfSinglepage.Add(ptcS);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 100, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Subject Code");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 100, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Subject Code");
                                    mypdfSinglepage.Add(ptcS);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 200, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Title of the Paper");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 200, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Title of the Paper");
                                    mypdfSinglepage.Add(ptcS);


                                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                         new PdfArea(mydocument, 550, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Exam Date & Session");
                                    //mypdfpage.Add(ptc);

                                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                         new PdfArea(mydocument, 700, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Max.No.of Scripts");
                                    //mypdfpage.Add(ptc);

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfSinglepage.Add(ptcS);


                                    string[] spvalsu = valsubdetails.Split(',');

                                    int sbbno = 0;

                                    for (int es = 0; es < dvstaffde.Count; es++)
                                    {
                                        sbbno++;
                                        coltop = coltop + 20;

                                        string scode = dvstaffde[es]["subject_code"].ToString();
                                        string sname = dvstaffde[es]["subject_name"].ToString();
                                        string exmdate = dvstaffde[es]["examdate"].ToString();
                                        string esess = dvstaffde[es]["session"].ToString();
                                        string edu_level = dt.GetFunctionv("select distinct c.Edu_Level   from subject S,syllabus_master sy ,Degree d,Course C where s.syll_code =sy.syll_code and D.Degree_Code =sy.degree_code and d.Course_Id =c.Course_Id and s.subject_code ='" + scode + "'");
                                        string stucount = "0";
                                        dssubstu.Tables[0].DefaultView.RowFilter = "subject_code='" + scode + "'";
                                        DataView dvcount = dssubstu.Tables[0].DefaultView;
                                        if (dvcount.Count > 0)
                                        {
                                            stucount = dvcount[0]["stucount"].ToString();
                                        }

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 60, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sbbno.ToString());

                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 60, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sbbno.ToString());

                                        mypdfSinglepage.Add(ptcS);

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 95, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, edu_level.ToUpper() + " - " + scode);
                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 95, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, edu_level.ToUpper() + " - " + scode);
                                        mypdfSinglepage.Add(ptcS);

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 200, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sname);
                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 200, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sname);
                                        mypdfSinglepage.Add(ptcS);

                                        //ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                        //                                        new PdfArea(mydocument, 580, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, exmdate + " - " + esess);
                                        //mypdfpage.Add(ptc);

                                        //ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                        //                                     new PdfArea(mydocument, 720, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, stucount);
                                        //mypdfpage.Add(ptc);
                                        if (SubjectDetails.Trim() == "")
                                        {
                                            SubjectDetails = sbbno + ") " + edu_level.ToUpper() + " - " + scode + "  " + sname;
                                        }
                                        else
                                        {
                                            SubjectDetails += " " + sbbno + ") " + edu_level.ToUpper() + " - " + scode + "  " + sname;
                                        }

                                    }

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop += 30;

                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 800, 120), System.Drawing.ContentAlignment.MiddleLeft, "In anticipation of your acceptance of the appointment, the relevant syllabus and model question papers are enclosed. The Question paper should be set in the same pattern of the model paper according to the syllabus.Please set one Question paper for the " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString() + " Examination. Kindly send both soft copy and Hard Copy of the Question Paper(s) to the Examination Office.The Subject Code of the Question Paper should be the file name of your soft Copy.Please let us know by return post / through the messenger, your acceptance of this appointment in the enclosed form.");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 800, 120), System.Drawing.ContentAlignment.MiddleLeft, "In anticipation of your acceptance of the appointment, the relevant syllabus and model question papers are enclosed. The Question paper should be set in the same pattern of the model paper according to the syllabus.Please set one Question paper for the " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString() + " Examination. Kindly send both soft copy and Hard Copy of the Question Paper(s) to the Examination Office.The Subject Code of the Question Paper should be the file name of your soft Copy.Please let us know by return post / through the messenger, your acceptance of this appointment in the enclosed form.");
                                    mypdfSinglepage.Add(ptcS);

                                    //coltop += 60;
                                    //ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "");
                                    //mypdfpage.Add(ptc);
                                    //coltop += 30;
                                    //ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "");
                                    //mypdfpage.Add(ptc);
                                    coltop += 110;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Kindly send us the question paper in the self addressed envelope by Speed Post / Courier so as to reach us on or before ");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Kindly send us the question paper in the self addressed envelope by Speed Post / Courier so as to reach us on or before ");
                                    mypdfSinglepage.Add(ptcS);

                                    //coltop += 15;
                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 80, coltop + 10, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, QpBefore);
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 80, coltop + 10, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, QpBefore);
                                    mypdfSinglepage.Add(ptcS);

                                    coltop += 30;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 800, 100), System.Drawing.ContentAlignment.MiddleLeft, "If you are unable to accept the offer of appointment. Kindly return the enclosures accompanying this letter immediately by post / messenger, in order to make alternate arrangements. Your co-operation in this regard would be highly appreciated.");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 800, 100), System.Drawing.ContentAlignment.MiddleLeft, "If you are unable to accept the offer of appointment. Kindly return the enclosures accompanying this letter immediately by post / messenger, in order to make alternate arrangements. Your co-operation in this regard would be highly appreciated.");
                                    mypdfSinglepage.Add(ptcS);
                                    coltop += 40;

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Thanking you.");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Thanking you.");
                                    mypdfSinglepage.Add(ptcS);


                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Yours sincerely,");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Yours sincerely,");
                                    mypdfSinglepage.Add(ptcS);

                                    MemoryStream memoryStream1 = new MemoryStream();
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg")))
                                    {
                                        if (dshall.Tables[0].Rows[0]["coe_signature"] != null && dshall.Tables[0].Rows[0]["coe_signature"].ToString().Trim() != "")
                                        {
                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg")))
                                            {
                                                byte[] file = (byte[])dshall.Tables[0].Rows[0]["coe_signature"];
                                                memoryStream1.Write(file, 0, file.Length);
                                                if (file.Length > 0)
                                                {
                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream1, true, true);
                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(1000, 400, null, IntPtr.Zero);
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                                memoryStream1.Dispose();
                                                memoryStream1.Close();
                                            }
                                        }
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg"));
                                        mypdfpage.Add(LogoImage, 60, coltop + 35, 500);

                                        PdfImage LogoImageS = mySingledocument.NewImage(HttpContext.Current.Server.MapPath("~/college/coesign.jpeg"));
                                        mypdfSinglepage.Add(LogoImageS, 60, coltop + 35, 500);
                                    }
                                    coltop = coltop + 70;
                                    ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 36;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Encl:   1. Form for acceptance with envelopes for reply");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 20, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "Encl:   1. Form for acceptance with envelopes for reply");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. Copy of the syllabus");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. Copy of the syllabus");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "3. Model Question Paper(s)");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "3. Model Question Paper(s)");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "4. Claim form with rates of remuneration ");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "4. Claim form with rates of remuneration ");
                                    mypdfSinglepage.Add(ptcS);


                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "5. Envelope for dispatching   1. Question Paper(s)");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "5. Envelope for dispatching   1. Question Paper(s)");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 260, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. Solution & Scheme");
                                    mypdfpage.Add(ptc);
                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 260, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. Solution & Scheme");
                                    mypdfSinglepage.Add(ptcS);
                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "6. CD. (Soft Copy)");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "6. CD. (Soft Copy)");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "7. Self addressed envelope for all items");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 65, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, "7. Self addressed envelope for all items");
                                    mypdfSinglepage.Add(ptcS);

                                }
                                else
                                {
                                    coltop = coltop + 10;
                                    PdfTextArea ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                    mypdfpage.Add(ptc);

                                    PdfTextArea ptcS = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                    mypdfSinglepage.Add(ptcS);


                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                    mypdfSinglepage.Add(ptcS);


                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 30, 10, 500);

                                        PdfImage LogoImageS = mySingledocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfSinglepage.Add(LogoImageS, 30, 10, 500);
                                    }

                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                    {
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                        mypdfpage.Add(leftimage, 740, 10, 500);

                                        PdfImage leftimageS = mySingledocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                        mypdfSinglepage.Add(leftimageS, 740, 10, 500);
                                    }

                                    coltop = coltop + 40;
                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "LIST OF " + ddlstaream.SelectedItem.ToString().ToUpper() + " EXAMINARS FOR THE E.S.E " + ddlMonth.SelectedItem.ToString() + "-" + ddlYear.SelectedItem.ToString() + "  [" + ddltype.SelectedItem.ToString().ToUpper() + "]");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "LIST OF " + ddlstaream.SelectedItem.ToString().ToUpper() + " EXAMINARS FOR THE E.S.E " + ddlMonth.SelectedItem.ToString() + "-" + ddlYear.SelectedItem.ToString() + "  [" + ddltype.SelectedItem.ToString().ToUpper() + "]");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 20;
                                    if (type.Trim().ToLower() == "internal")
                                    {
                                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Department : " + department + "");
                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Department : " + department + "");
                                        mypdfSinglepage.Add(ptcS);
                                    }


                                    coltop = coltop + 15;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "S.No ");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 50, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "S.No ");
                                    mypdfSinglepage.Add(ptcS);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 100, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Examiner");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 100, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Examiner");
                                    mypdfSinglepage.Add(ptcS);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 400, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Paper Code");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 400, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Paper Code");
                                    mypdfSinglepage.Add(ptcS);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 500, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Paper");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 500, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Paper");
                                    mypdfSinglepage.Add(ptcS);

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfSinglepage.Add(ptcS);

                                    int sbbno = 0;
                                    for (int es = 0; es < dvstaffde.Count; es++)
                                    {
                                        sbbno++;
                                        coltop = coltop + 20;

                                        string scode = dvstaffde[es]["subject_code"].ToString();
                                        string sname = dvstaffde[es]["subject_name"].ToString();
                                        string exmdate = dvstaffde[es]["examdate"].ToString();
                                        string esess = dvstaffde[es]["session"].ToString();

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 60, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sbbno.ToString());
                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 60, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sbbno.ToString());
                                        mypdfSinglepage.Add(ptcS);

                                        if (es == 0)
                                        {
                                            ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, 90, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, staffnbame);
                                            mypdfpage.Add(ptc);

                                            ptcS = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 90, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, staffnbame);
                                            mypdfSinglepage.Add(ptcS);
                                        }

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 400, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, scode);
                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 400, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, scode);
                                        mypdfSinglepage.Add(ptcS);

                                        ptc = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 500, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sname);
                                        mypdfpage.Add(ptc);

                                        ptcS = new PdfTextArea(Fontsmall122, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 500, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, sname);
                                        mypdfSinglepage.Add(ptcS);

                                    }

                                    coltop = coltop + 10;
                                    ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(ptc);

                                    ptcS = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 20, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfSinglepage.Add(ptcS);
                                }

                                mypdfpage.SaveToDocument();
                                mypdfSinglepage.SaveToDocument();
                                lblmsg.Visible = false;

                                //Added by Idhris For sending EMail  15-02-2017
                                //== Code started
                                if (chkSendEMail.Checked && Regex.IsMatch(staffEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                {


                                    //mypdfSinglepage = mypdfpage.CreateCopy();
                                    //mySingledocument.NewPage();

                                    string appPath = HttpContext.Current.Server.MapPath("~");
                                    if (appPath != "")
                                    {
                                        string szPath = appPath + "/Report/";
                                        string szFile = "ExamEvalEmail" + DateTime.Now.Ticks.ToString() + ".pdf";
                                        string emailFileName = szPath + szFile;
                                        try
                                        {
                                            mySingledocument.SaveToFile(emailFileName);

                                            if (!string.IsNullOrEmpty(emailFileName))
                                            {
                                                string tomail = staffEmail;
                                                string msg = "";
                                                string studname = string.Empty;
                                                string subject = "Sub : End of Semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString() + " Question paper setting - Reg.";
                                                sendEMail(emailFileName, tomail, msg, studname, college_code, subject);
                                                //try
                                                //{
                                                //    if (File.Exists(emailFileName))
                                                //    {
                                                //        File.Delete(emailFileName);
                                                //    }
                                                //}
                                                //catch { }
                                            }
                                        }
                                        catch { }
                                    }
                                }
                                if (cbsendSMS.Checked && StaffMobile.Trim() != "" && StaffMobile.Trim() != "0")
                                {

                                    //Dear Sir/Madam, Kindly Collect theInternal Valuation letter for E.S.E.Nov.2017 from the Examinations Office. Thank&Regards, COE i/c
                                    //string Msg = " Sub : End of Semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString() + " Question paper setting - Reg. We are happy to inform you that you are appointed as Question Paper Setter for the following paper(s) for our End Semester Examinations " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString();
                                    string Msg = " Dear Sir/Madam, Kindly Collect the Internal Valuation letter for E.S.E." + ddlMonth.SelectedItem.ToString() + ". " + ddlYear.SelectedItem.ToString() + " from the Examinations Office. Thank&Regards, COE i/c";
                                    Msg += " " + SubjectDetails;
                                    int d = dt.send_sms(user_id, college_code, user_code, StaffMobile, Msg, "1");

                                    //SendSMSM(StaffMobile,user_id,user_code,college_code,Msg)


                                    SMSSettings smsObject = new SMSSettings();
                                    //smsObject.User_degreecode = Convert.ToInt32(degcode);
                                    smsObject.User_collegecode = Convert.ToInt32(college_code);
                                    smsObject.User_usercode = user_id;
                                    smsObject.Text_message = Msg;
                                    smsObject.IsStaff = 0;
                                    //smsObject.MobileNos = StaffMobile;
                                    smsObject.MobileNos = "8015304571";//
                                    //smsObject.AdmissionNos = dsMobile.Tables[0].Rows[0]["roll_admit"].ToString();
                                    smsObject.sendTextMessage();
                                }
                                //== Code Ended by Idhris


                            }
                        }
                    }
                }
                if (halfflag == true)
                {
                    lblmsg.Visible = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "ExamValuationLetter.pdf";
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);

                    }
                }
                else
                {
                    lblmsg.Text = "Please Select the Staff and then Proceed";
                    lblmsg.Visible = true;
                }
            }
            else
            {
                lblmsg.Text = "Please Select Exam Month And Year";
                lblmsg.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void rdbQsetter_Change(object sender, EventArgs e)
    {
        department();
    }

    protected void rdbValuation_Change(object sender, EventArgs e)
    {
        department();
    }

    //Added by Idhris 15-02-2017
    private void sendEMail(string fileName, string tomail, string msg, string studname, string collegecode, string subject)
    {
        try
        {
            string send_mail = "";
            string send_pw = "";
            string to_mail = tomail;

            string subtext = subject;

            string strquery = "select massemail,masspwd from collinfo where college_code = " + collegecode + " ";
            ds.Clear();
            ds = dt.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                send_mail = Convert.ToString(ds.Tables[0].Rows[0]["massemail"]);
                send_pw = Convert.ToString(ds.Tables[0].Rows[0]["masspwd"]);
            }
            //send_mail = "palpaporange@gmail.com";
            //send_pw = "abcd1234abcd";
            if (send_mail.Trim() != "" && send_pw.Trim() != "" && to_mail.Trim() != "")
            {
                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mailmsg = new MailMessage();
                MailAddress mfrom = new MailAddress(send_mail);
                mailmsg.From = mfrom;
                mailmsg.To.Add(to_mail);
                mailmsg.Subject = subtext;
                mailmsg.IsBodyHtml = true;

                mailmsg.Body = msg;
                mailmsg.Attachments.Add(new Attachment(fileName));

                Mail.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                Mail.UseDefaultCredentials = false;
                Mail.Credentials = credentials;
                Mail.Send(mailmsg);
            }
        }
        catch { }
    }

    protected void btnsendsms_click(object sender, EventArgs e)
    {
        string user_id = string.Empty;
        string ssr = "select * from Track_Value where college_code='" + Convert.ToString(college_code) + "'";
        ds.Clear();
        ds = dt.select_method_wo_parameter(ssr, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
        }
        FpValuation.SaveChanges();
        for (int res = 1; res < FpValuation.Sheets[0].RowCount; res++)
        {
            int isval = Convert.ToInt32(FpValuation.Sheets[0].Cells[res, 6].Value);
            if (isval == 1)
            {
                string staffnbame = FpValuation.Sheets[0].Cells[res, 1].Text.ToString();
                string staffcode = FpValuation.Sheets[0].Cells[res, 2].Text.ToString();
                string department = FpValuation.Sheets[0].Cells[res, 3].Text.ToString();
                string valsubdetails = FpValuation.Sheets[0].Cells[res, 5].Text.ToString();
                string type = FpValuation.Sheets[0].Cells[res, 4].Text.ToString();
                string staffEmail = Convert.ToString(dt.GetFunction("select email from external_staff where staff_code='" + staffcode + "'")).Trim();
                string StaffMobile = Convert.ToString(dt.GetFunction("select Per_Mobileno from external_staff where staff_code='" + staffcode + "'")).Trim();
                string Msg = " Dear Sir/Madam, Kindly Collect the Internal Valuation letter for E.S.E." + ddlMonth.SelectedItem.ToString() + ". " + ddlYear.SelectedItem.ToString() + " from the Examinations Office. Thank&Regards, COE i/c";
                //Msg += " " + SubjectDetails;
                int d = dt.send_sms(user_id, college_code, user_code, StaffMobile, Msg, "1");

                //SendSMSM(StaffMobile,user_id,user_code,college_code,Msg)


                SMSSettings smsObject = new SMSSettings();
                //smsObject.User_degreecode = Convert.ToInt32(degcode);
                smsObject.User_collegecode = Convert.ToInt32(college_code);
                smsObject.User_usercode = user_id;
                smsObject.Text_message = Msg;
                smsObject.IsStaff = 0;
                smsObject.MobileNos = StaffMobile;
                //smsObject.MobileNos = "8883364145";//
                //smsObject.AdmissionNos = dsMobile.Tables[0].Rows[0]["roll_admit"].ToString();
                smsObject.sendTextMessage();


            }
        }
    }

    protected void cbsendSMS_CheckedChanged(object sender, EventArgs e)
    {
        if (cbsendSMS.Checked == true)
        {
            btnsendsms.Visible = true;
        }
        else
        {
            btnsendsms.Visible = false;
        }
    }
}