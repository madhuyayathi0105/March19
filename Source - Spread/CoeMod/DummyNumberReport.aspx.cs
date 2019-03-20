using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.Configuration;

public partial class DummyNumberReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    int count = 0;
    DataSet ds2 = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
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
                loaddate();
                rdbserial.Checked = true;
                Fpspread1.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.CommandBar.Visible = false;


                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                ds = d2.select_method_wo_parameter(Master1, "text");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                    }
                }
            }
            errorlable.Visible = false;
            lblvalidation1.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    public void loaddate()
    {
        try
        {
            string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date,exdt.Exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedItem.Value.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " order by exdt.Exam_date";
            ds.Clear();
            ds = d2.select_method_wo_parameter(getexamdate, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldate.DataSource = ds;
                ddldate.DataValueField = "Exam_date";
                ddldate.DataBind();
            }
        }
        catch
        {

        }
    }
    public void loadsubject()
    {
        try
        {
            if (ddldate.Items.Count > 0)
            {
                if (ddldate.SelectedItem.Text.Trim() != "")
                {
                    ddlsubject.Items.Clear();
                    string subnoquery = "select distinct s.subject_Name as SubjectName ,s.Subject_code as subjectcode from subject s,exmtt e,exmtt_det ex,sub_sem where sub_sem.subtype_no=s.subtype_no  and s.subject_no=ex.subject_no and ex.coll_code=" + Session["collegecode"].ToString() + " and ex.exam_Date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',103)and ex.exam_code=e.exam_code and e.Exam_Month=" + ddlMonth.SelectedValue.ToString() + " and e.Exam_Year=" + ddlYear.SelectedValue.ToString() + " and e.exam_type='Univ' and exam_session ='" + ddlsession.SelectedItem.Text + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(subnoquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ddlsubject.Items.Insert(i, new System.Web.UI.WebControls.ListItem("" + ds.Tables[0].Rows[i]["SubjectName"].ToString() + "", "" + ds.Tables[0].Rows[i]["subjectcode"].ToString() + ""));

                        }
                    }
                }
            }

        }
        catch
        {

        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loaddate();
            loadsubject();
            Fpspread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loaddate();
            loadsubject();
            Fpspread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }
    protected void ddldate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadsubject();
            Fpspread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }
    protected void ddlsession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadsubject();
            Fpspread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }

    protected void rdbserial_Change(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }

    protected void rdbRandam_Change(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }


    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string mainvalue = "";
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Printcontrol.Visible = false;
            if (chklstbranch.Items.Count > 0)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        if (mainvalue == "")
                        {
                            mainvalue = chklstbranch.Items[i].Value;
                        }
                        else
                        {
                            mainvalue = mainvalue + "," + chklstbranch.Items[i].Value;
                        }
                    }
                }
            }
            if (ddldate.Items.Count != 0 && ddlsubject.Items.Count != 0 && mainvalue.Trim() != "")
            {
                string month = Convert.ToString(ddlMonth.SelectedItem.Text);
                string year = Convert.ToString(ddlYear.SelectedItem.Text);
                string date = Convert.ToString(ddldate.SelectedItem.Text);
                string session = Convert.ToString(ddlsession.SelectedItem.Text);
                string subject = Convert.ToString(ddlsubject.SelectedItem.Value);
                if (month.Trim() != "" && year.Trim() != "" && date.Trim() != "" && session.Trim() != "" && subject.Trim() != "")
                {

                    string dummytype = "";
                    if (rdbserial.Checked == true)
                    {
                        dummytype = "1";
                    }
                    if (rdbRandam.Checked == true)
                    {
                        dummytype = "0";
                    }
                    //Modified by srinath 7/11/2015
                    // string selectquery = "select roll_no,regno,Course_Name,Dept_Name,dummy_no   from  dummynumber du,Degree d,Department dt,Course c where du.degreecode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and exam_month ='" + ddlMonth.SelectedItem.Value + "' and exam_year ='" + ddlYear.SelectedItem.Text + "' and exam_date ='" + ddldate.SelectedItem.Text + "' and subject ='" + ddlsubject.SelectedItem.Value + "' and dummy_type ='" + dummytype + "' and du.degreecode in (" + mainvalue + ")";
                    string selectquery = "select roll_no,regno,Course_Name,Dept_Name,dummy_no,du.subject_no,ss.Lab,du.sheet_no,semester from  dummynumber du,Degree d,Department dt,Course c,sub_sem ss,subject s where du.degreecode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and s.subType_no=ss.subType_no and s.subject_no=du.subject_no and exam_month ='" + ddlMonth.SelectedItem.Value + "' and exam_year ='" + ddlYear.SelectedItem.Text + "' and exam_date ='" + ddldate.SelectedItem.Text + "' and subject ='" + ddlsubject.SelectedItem.Value + "' and dummy_type ='" + dummytype + "' and du.degreecode in (" + mainvalue + ") order by dummy_no,Dept_Name,regno";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].ColumnCount = 6;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Dummy No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Branch";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;

                        Fpspread1.Sheets[0].Columns[0].Width = 50;

                        Fpspread1.Sheets[0].Columns[1].Width = 100;
                        Fpspread1.Sheets[0].Columns[2].Width = 100;
                        Fpspread1.Sheets[0].Columns[3].Width = 100;
                        Fpspread1.Sheets[0].Columns[4].Width = 50;
                        Fpspread1.Sheets[0].Columns[5].Width = 200;
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["regno"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["dummy_no"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["sheet_no"]);
                            string labs = ds.Tables[0].Rows[i]["lab"].ToString().Trim().ToLower();
                            int labsu = 0;
                            if (labs == "true" || labs == "1")
                            {
                                labsu = 1;
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = labsu.ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["semester"]);
                        }
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        Fpspread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        Fpspread1.Visible = true;
                        errorlable.Visible = false;
                        rptprint.Visible = true;
                        if (Session["Rollflag"].ToString() == "1")
                        {
                            Fpspread1.Sheets[0].Columns[1].Visible = true;
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Columns[1].Visible = false;
                        }
                        if (Session["Regflag"].ToString() == "1")
                        {
                            Fpspread1.Sheets[0].Columns[2].Visible = true;
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Columns[2].Visible = false;
                        }
                    }
                    else
                    {
                        errorlable.Visible = true;
                        errorlable.Text = "No Records Found";
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                    }
                }
                else
                {
                    errorlable.Visible = true;
                    errorlable.Text = "Please Select All Fields";
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                }
            }
            else
            {
                errorlable.Visible = true;
                errorlable.Text = "Please Select All Fields";
                Fpspread1.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Dummy Number Report";
            string pagename = "DummyNumberReport.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
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
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }

    }
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {


            count = 0;
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                if (chkdegree.Checked == true)
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = true;
                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = false;
                        txtdegree.Text = "---Select---";
                    }
                }
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }

    }
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            if (course_id.Trim() != "")
            {
                ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds2;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
                    chklstbranch.DataBind();
                    chklstbranch.Items[0].Selected = true;
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
                    if (chkbranch.Checked == true)
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chklstbranch.Items[i].Selected = true;
                            txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chkbranch.Checked = false;
                            chklstbranch.Items[i].Selected = false;
                            txtbranch.Text = "---Select---";
                        }
                    }
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
                chklstbranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {


            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "--Select--";
                txtbranch.Text = "--Select--";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "--Select--";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

        }
        catch (Exception ex)
        {

        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                chkbranch.Checked = false;
                txtbranch.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void btndummynoprint_Click(object sender, EventArgs e)
    {
        try
        {
            ArrayList adddummyarray = new ArrayList();
            Gios.Pdf.PdfDocument mydoc;
            Font Fontbold = new Font("Book Antiqua", 18, FontStyle.Regular);
            Font fbold = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font fontname = new Font("Book Antiqua", 11, FontStyle.Bold);
            Font fontmedium = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font fontmediumb = new Font("Book Antiqua", 8, FontStyle.Bold);
            mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(24, 30));
            Gios.Pdf.PdfPage mypdfpage;
            //  Gios.Pdf.PdfTable table1 = myprovdoc.NewTable(Fontsmall, prov_cnt + 8, 6, 1);
            Gios.Pdf.PdfTable table;
            Gios.Pdf.PdfTable table1;
            Gios.Pdf.PdfTablePage myprov_pdfpage1;
            string deptvalue = "";
            Fpspread1.SaveChanges();

            int prinsheetno = 0;
            string getmaxsheetno = d2.GetFunction("select isnull(max(sheet_no),'10000') from dummynumber where exam_year='" + ddlYear.SelectedValue.ToString() + "' and exam_month='" + ddlMonth.SelectedValue.ToString() + "' ");
            int strshetno = Convert.ToInt32(getmaxsheetno);
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                for (int row = 0; row < Fpspread1.Sheets[0].RowCount; row++)
                {
                    string getval = Fpspread1.Sheets[0].Cells[row, 2].Tag.ToString();
                    if (getval == "1")
                    {
                        adddummyarray.Add(Convert.ToString(Fpspread1.Sheets[0].Cells[row, 2].Text));
                    }
                    else
                    {
                        adddummyarray.Add(Convert.ToString(Fpspread1.Sheets[0].Cells[row, 3].Text));
                    }
                    string dummyno = Fpspread1.Sheets[0].Cells[row, 3].Tag.ToString();
                    if (dummyno == "")
                    {
                        if ((row % 25) == 0)
                        {
                            strshetno++;
                        }
                        dummyno = strshetno.ToString();
                        string insertval = "update dummynumber set sheet_no='" + dummyno + "' where dummy_no='" + Fpspread1.Sheets[0].Cells[row, 3].Text.ToString() + "'";
                        int val = d2.update_method_wo_parameter(insertval, "Text");
                    }
                    if (prinsheetno == 0)
                    {
                        prinsheetno = Convert.ToInt32(dummyno);
                    }
                    Fpspread1.Sheets[0].Cells[row, 3].Tag = dummyno;
                }
            }
            Fpspread1.SaveChanges();
            string month = Convert.ToString(ddlMonth.SelectedItem.Text);
            string year = Convert.ToString(ddlYear.SelectedItem.Text);
            string dept = "";
            if (chklstbranch.Items.Count > 0)
            {
                for (int ros = 0; ros < chklstbranch.Items.Count; ros++)
                {
                    if (chklstbranch.Items[ros].Selected == true)
                    {
                        dept = Convert.ToString(chklstbranch.Items[ros].Text);
                    }
                }
            }
            string course = Convert.ToString(Fpspread1.Sheets[0].Cells[0, 4].Text);
            deptvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[0, 5].Text);
            string semval = Convert.ToString(Fpspread1.Sheets[0].Cells[0, 4].Tag);
            string subjectname = ddlsubject.SelectedItem.Text;
            string subjectcode = ddlsubject.SelectedItem.Value;


            PdfTextArea ptc4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 515, 0, 113, 30), System.Drawing.ContentAlignment.MiddleCenter, prinsheetno.ToString());

            PdfTextArea ptc5 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 515, 30, 113, 20), System.Drawing.ContentAlignment.MiddleCenter, month + " " + year);

            int y = 48;

            PdfTextArea ptde = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 120, y, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, "" + course + "");

            PdfTextArea ptc = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 303, y, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "" + deptvalue + "");

            PdfTextArea psem = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 544, y, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "" + semval + "");

            y = y + 20;
            PdfTextArea ptc1 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 120, y, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "" + subjectname + "");

            PdfTextArea ptc2 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 544, y, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "" + subjectcode + "");
            if (adddummyarray.Count > 0)
            {
                int rowcount = 0;
                int totalcount = adddummyarray.Count;
                while (totalcount > 25)
                {
                    if (rowcount > 24)
                    {
                        prinsheetno++;
                        ptc4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 515, 2, 113, 30), System.Drawing.ContentAlignment.MiddleCenter, prinsheetno.ToString());
                    }
                    totalcount = totalcount - 25;
                    table = mydoc.NewTable(Fontsmall, 13, 1, 10);
                    for (int row = 0; row < 13; row++)
                    {
                        string getvalue = Convert.ToString(adddummyarray[rowcount]);
                        table.Cell(row, 0).SetContent(getvalue);
                        table.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table.Cell(row, 0).SetCellPadding(11);
                        rowcount++;
                    }
                    table1 = mydoc.NewTable(Fontsmall, 12, 1, 10);
                    for (int row = 0; row < 12; row++)
                    {
                        string getvalue = Convert.ToString(adddummyarray[rowcount]);
                        table1.Cell(row, 0).SetContent(getvalue);
                        table1.Cell(row, 0).SetCellPadding(11);
                        rowcount++;
                    }
                    mypdfpage = mydoc.NewPage();
                    myprov_pdfpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 150, 100, 500));
                    mypdfpage.Add(myprov_pdfpage1);

                    myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 340, 150, 100, 500));
                    mypdfpage.Add(myprov_pdfpage1);
                    mypdfpage.Add(ptc);
                    mypdfpage.Add(ptc1);
                    mypdfpage.Add(ptc2);
                    mypdfpage.Add(ptc4);
                    mypdfpage.Add(ptc5);
                    mypdfpage.Add(ptde);
                    mypdfpage.Add(psem);
                    mypdfpage.SaveToDocument();
                }
                bool check = false;
                int subtotalcount = totalcount;
                int value = 0;
                mypdfpage = mydoc.NewPage();
                if (subtotalcount >= 13)
                {
                    value = subtotalcount - 13;
                    check = true;
                }
                if (check == true)
                {
                    prinsheetno++;
                    ptc4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 515, 2, 113, 30), System.Drawing.ContentAlignment.MiddleCenter, prinsheetno.ToString());

                    table = mydoc.NewTable(Fontsmall, 13, 1, 10);
                    for (int row = 0; row < 13; row++)
                    {
                        string getvalue = Convert.ToString(adddummyarray[rowcount]);
                        table.Cell(row, 0).SetContent(getvalue);
                        rowcount++;
                    }
                    myprov_pdfpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 150, 100, 500));
                    mypdfpage.Add(myprov_pdfpage1);
                }
                else
                {
                    prinsheetno++;
                    ptc4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 515, 2, 113, 30), System.Drawing.ContentAlignment.MiddleCenter, prinsheetno.ToString());
                    table = mydoc.NewTable(Fontsmall, subtotalcount, 1, 10);
                    for (int row = 0; row < subtotalcount; row++)
                    {
                        string getvalue = Convert.ToString(adddummyarray[rowcount]);
                        table.Cell(row, 0).SetContent(getvalue);
                        rowcount++;
                    }
                    myprov_pdfpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 150, 100, 500));
                    mypdfpage.Add(myprov_pdfpage1);
                }
                if (value != 0)
                {
                    table1 = mydoc.NewTable(Fontsmall, value, 1, 10);
                    for (int row = 0; row < value; row++)
                    {
                        string getvalue = Convert.ToString(adddummyarray[rowcount]);
                        table1.Cell(row, 0).SetContent(getvalue);
                        rowcount++;
                    }
                    myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 340, 170, 100, 500));
                    mypdfpage.Add(myprov_pdfpage1);
                }
                mypdfpage.Add(ptc);
                mypdfpage.Add(ptc1);
                mypdfpage.Add(ptc2);
                mypdfpage.Add(ptc4);
                mypdfpage.Add(ptc5);
                mypdfpage.Add(ptde);
                mypdfpage.Add(psem);
                mypdfpage.SaveToDocument();
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Dummy.pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);

                }
            }
        }
        catch
        {

        }
    }
}