using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;


public partial class Equalpaperselection : System.Web.UI.Page
{
    string CollegeCode;
    DAccess2 da = new DAccess2();
    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.IntegerCellType intgrcel = new FarPoint.Web.Spread.IntegerCellType();
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    FarPoint.Web.Spread.CheckBoxCellType cbct = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType cbct1 = new FarPoint.Web.Spread.CheckBoxCellType();
    string exam_month = "", year = "";
    string commansubjectno = "";

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
            //****************************************************//msg.Visible = false;
            CollegeCode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                Fpsaved.Visible = false;
                cbct.AutoPostBack = true;

                loadtype();
                loadyear();
                loadmonth();

                fpselected.Sheets[0].RowCount = 0;
                fpselected.Sheets[0].RowHeader.Visible = false;
                fpselected.CommandBar.Visible = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                fpselected.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                fpselected.Sheets[0].ColumnCount = 4;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpselected.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpselected.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                fpselected.Sheets[0].Columns[0].Width = 60;
                fpselected.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                fpselected.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                fpselected.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                fpselected.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                fpselected.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

                fpselected.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                fpselected.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                fpselected.Sheets[0].Columns[1].Width = 65;

                fpselected.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
                fpselected.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                fpselected.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                fpselected.Sheets[0].Columns[2].Width = 160;
                fpselected.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

                fpselected.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject_Name";
                fpselected.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                fpselected.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpselected.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                fpselected.Sheets[0].Columns[3].Width = 145;


                cbGeneral.Checked = false;
                loadspread();
                loadsavedetails();
            }
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
        Response.Redirect("default.aspx", false);
    }
    public void loadyear()
    {
        try
        {
            ddlYear.Items.Clear();
            DataSet ds = da.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
                ddlYear.SelectedIndex = ddlYear.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void loadmonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            DataSet ds = new DataSet();
            string year1 = ddlYear.SelectedValue;
            ds = da.Exammonth(year1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
                ddlMonth.SelectedIndex = ddlMonth.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void loadspread()
    {
        try
        {
            fpshowsubject.RowHeader.Visible = false;
            fpselected.RowHeader.Visible = false;
            fpshowsubject.Sheets[0].RowCount = 1;
            fpshowsubject.Sheets[0].RowHeader.Visible = false;
            fpshowsubject.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            fpshowsubject.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            fpshowsubject.Sheets[0].ColumnCount = 4;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpshowsubject.Sheets[0].Columns[0].Width = 60;
            fpshowsubject.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            fpshowsubject.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            fpshowsubject.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpshowsubject.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpshowsubject.Sheets[0].Columns[1].Width = 65;
            fpshowsubject.Sheets[0].Cells[0, 1].CellType = cbct;

            fpshowsubject.Sheets[0].SpanModel.Add(0, 2, 1, 2);
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            fpshowsubject.Sheets[0].Columns[2].Width = 160;
            fpshowsubject.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject_Name";
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpshowsubject.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            fpshowsubject.Sheets[0].Columns[3].Width = 145;

            string type = ddltype.SelectedItem.Text;
            string degrr = ddldegree.SelectedItem.Text;
            string strsql = "";
            if (!cbGeneral.Checked)
            {
                strsql = "select distinct s.subject_name,s.subject_code from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,Degree d,course c where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and ead.subject_no=s.subject_no and ss.subType_no=s.subType_no and ed.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and c.Edu_Level='" + degrr + "' and c.type='" + type + "' order by s.subject_name,s.subject_code";
            }
            else
            {
                strsql = "select distinct s.subject_code ,s.subject_name from Registration r,syllabus_master sm,sub_sem ss,subject s,course c ,Degree d where r.Batch_Year=sm.Batch_Year and r.Current_Semester=sm.semester and r.degree_code=sm.degree_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.syll_code=sm.syll_code and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and ss.promote_count=1 and c.Edu_Level='" + degrr + "' and c.type='" + type + "' order by s.subject_name,s.subject_code";
            }

            DataSet dss = new DataSet();
            dss.Clear();
            dss = da.select_method_wo_parameter(strsql, "Text");
            fpshowsubject.Sheets[0].RowCount = 1;
            if (dss.Tables[0].Rows.Count > 0)
            {
                fpshowsubject.Sheets[0].RowCount = fpshowsubject.Sheets[0].RowCount + dss.Tables[0].Rows.Count;
                int sno = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    fpshowsubject.Sheets[0].Cells[i + 1, 0].Text = Convert.ToString(sno);
                    fpshowsubject.Sheets[0].Cells[i + 1, 1].CellType = cbct1;
                    fpshowsubject.Sheets[0].Cells[i + 1, 2].Text = dss.Tables[0].Rows[i]["subject_code"].ToString();
                    fpshowsubject.Sheets[0].Cells[i + 1, 3].Text = dss.Tables[0].Rows[i]["subject_name"].ToString();
                }
            }
            fpshowsubject.Sheets[0].PageSize = fpshowsubject.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void loadtype()
    {
        try
        {
            string collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''; select distinct Edu_Level from course where college_code='" + collegecode + "' and Edu_Level is not null and Edu_Level<>''";
            DataSet dstype = da.select_method_wo_parameter(strquery, "Text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = dstype;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
            }
            else
            {
                ddltype.Enabled = false;
            }
            if (dstype.Tables[1].Rows.Count > 0)
            {
                ddldegree.DataSource = dstype.Tables[1];
                ddldegree.DataTextField = "Edu_Level";
                ddldegree.DataBind();
                ddldegree.SelectedIndex = ddldegree.Items.Count - 1;
                ddldegree.Enabled = true;
            }
            else
            {
                ddldegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadspread();
        loadsavedetails();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadspread();
        loadsavedetails();
    }

    protected void fpshowsubject_OnButtonCommand(object sender, EventArgs e)
    {
        if (Convert.ToInt32(fpshowsubject.Sheets[0].Cells[0, 1].Value) == 1)
        {
            for (int i = 0; i < fpshowsubject.Sheets[0].Rows.Count; i++)
            {
                fpshowsubject.Sheets[0].Cells[i, 1].Value = 1;

            }
        }
        else
        {
            for (int i = 0; i < fpshowsubject.Sheets[0].Rows.Count; i++)
            {
                fpshowsubject.Sheets[0].Cells[i, 1].Value = 0;

            }
        }
        fpshowsubject.SaveChanges();


    }
    //protected void fpselected_OnButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    if (Convert.ToInt32(fpselected.Sheets[0].Cells[0, 1].Value) == 1)
    //    {
    //        for (int i = 0; i < fpselected.Sheets[0].Rows.Count; i++)
    //        {
    //            fpselected.Sheets[0].Cells[i, 1].Value = 1;

    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < fpselected.Sheets[0].Rows.Count; i++)
    //        {
    //            fpselected.Sheets[0].Cells[i, 1].Value = 0;

    //        }
    //    }
    //    fpselected.SaveChanges();
    //}
    protected void btnpassadd_Click(object sender, EventArgs e)
    {
        try
        {
            fpselected.Sheets[0].RowCount = 0;
            fpselected.Sheets[0].RowHeader.Visible = false;
            fpselected.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            fpselected.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            fpselected.Sheets[0].ColumnCount = 4;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpselected.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpselected.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpselected.Sheets[0].Columns[0].Width = 60;
            fpselected.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            fpselected.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            fpselected.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            fpselected.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpselected.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

            fpselected.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpselected.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpselected.Sheets[0].Columns[1].Width = 65;

            fpselected.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
            fpselected.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpselected.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            fpselected.Sheets[0].Columns[2].Width = 160;
            fpselected.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

            fpselected.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject_Name";
            fpselected.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpselected.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpselected.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            fpselected.Sheets[0].Columns[3].Width = 145;

            fpshowsubject.SaveChanges();

            int sno = 0;
            int addcount = 0;

            year = ddlYear.SelectedItem.Text.Trim();
            exam_month = ddlMonth.SelectedItem.Value;

            string getsubject = "select * from tbl_equal_paper_Matching ";
            DataSet dscopapers = da.select_method_wo_parameter(getsubject, "text");

            for (int i = 1; i < fpshowsubject.Sheets[0].Rows.Count; i++)
            {
                if (Convert.ToInt32(fpshowsubject.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    string strsubject = fpshowsubject.Sheets[0].Cells[i, 2].Text;
                    dscopapers.Tables[0].DefaultView.RowFilter = "Equal_Subject_Code='" + strsubject + "'";
                    DataView dvcopapers = dscopapers.Tables[0].DefaultView;
                    if (dvcopapers.Count > 0)
                    {
                        fpshowsubject.Sheets[0].Cells[i, 1].Value = 0;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + strsubject + " Already Contanis')", true);
                        return;
                    }

                    sno++;
                    addcount++;
                    fpselected.Sheets[0].Rows.Count++;
                    fpselected.Sheets[0].Cells[fpselected.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(sno);
                    fpselected.Sheets[0].Cells[fpselected.Sheets[0].Rows.Count - 1, 2].Text = fpshowsubject.Sheets[0].Cells[i, 2].Text;
                    fpselected.Sheets[0].Cells[fpselected.Sheets[0].Rows.Count - 1, 3].Text = fpshowsubject.Sheets[0].Cells[i, 3].Text;
                    fpselected.Sheets[0].Cells[fpselected.Sheets[0].Rows.Count - 1, 1].CellType = cbct1;
                    fpselected.Sheets[0].Cells[fpselected.Sheets[0].Rows.Count - 1, 1].Value = false;
                }
            }
            if (addcount == 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Any One Record')", true);
            }
            fpselected.Sheets[0].PageSize = fpselected.Sheets[0].Rows.Count;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            fpselected.SaveChanges();

            int count = 0;
            int a_count = 0;

            string cosubjectno = "";

            year = ddlYear.SelectedItem.Text.Trim();
            exam_month = ddlMonth.SelectedItem.Value;

            string getsubject = "select * from tbl_equal_paper_Matching ";
            DataSet dscopapers = da.select_method_wo_parameter(getsubject, "text");

            for (int i = 0; i < fpselected.Sheets[0].Rows.Count; i++)
            {
                string strsubject = fpselected.Sheets[0].Cells[i, 2].Text.ToString();
                dscopapers.Tables[0].DefaultView.RowFilter = "Equal_Subject_Code='" + strsubject + "'";
                DataView dvcopapers = dscopapers.Tables[0].DefaultView;
                if (dvcopapers.Count > 0)
                {
                    fpselected.Sheets[0].Cells[i, 1].Value = 0;

                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + strsubject + " Already Contanis')", true);
                    return;
                }
            }

            for (int i = 0; i < fpselected.Sheets[0].Rows.Count; i++)
            {
                if (Convert.ToInt32(fpselected.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    count++;
                    commansubjectno = Convert.ToString(fpselected.Sheets[0].Cells[i, 2].Text.ToString());
                }
            }
            if (count > 1 || count == 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select Any Only One Subject')", true);
                return;
            }

            string strdelete = "delete from tbl_equal_paper_Matching where Com_Subject_Code='" + commansubjectno + "'";
            int delete = da.update_method_wo_parameter(strdelete, "Text");

            for (int i = 0; i < fpselected.Sheets[0].Rows.Count; i++)
            {
                cosubjectno = Convert.ToString(fpselected.Sheets[0].Cells[i, 2].Text);
                string strsql = string.Empty;
                if (cbGeneral.Checked)
                {
                    strsql = "insert into tbl_Subject_paper_Matching(Equal_Subject_Code,Com_Subject_Code) values('" + cosubjectno + "','" + commansubjectno + "')";
                }
                else
                {
                     strsql = "insert into tbl_equal_paper_Matching(Exam_Year,Exam_month,Equal_Subject_Code,Com_Subject_Code) values ('" + year + "','" + exam_month + "','" + cosubjectno + "','" + commansubjectno + "')";
                }
                int a = da.update_method_wo_parameter(strsql, "Text");
                if (a == 1)
                {
                    a_count++;
                }
            }
            if (a_count > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                for (int i = 0; i < fpshowsubject.Sheets[0].Rows.Count; i++)
                {
                    fpshowsubject.Sheets[0].Cells[i, 1].Value = 0;
                }

            }
            fpselected.Sheets[0].RowCount = 0;
            loadsavedetails();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }

    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        loadspread();
        loadsavedetails();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadmonth();
        loadspread();
        loadsavedetails();
    }
    public void loadsavedetails()
    {
        try
        {

            Fpsaved.Sheets[0].RowCount = 0;
            Fpsaved.Sheets[0].RowHeader.Visible = false;
            Fpsaved.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpsaved.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            Fpsaved.Sheets[0].ColumnCount = 4;
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpsaved.Sheets[0].Columns[0].Width = 80;
            Fpsaved.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            Fpsaved.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subjectcode_Year";
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpsaved.Sheets[0].Columns[1].Width = 200;
            Fpsaved.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject_Name";
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpsaved.Sheets[0].Columns[2].Width = 400;

            Fpsaved.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            Fpsaved.Sheets[0].Columns[3].Width = 50;

            Fpsaved.Width = 750;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

            year = ddlYear.SelectedItem.Text.Trim();
            exam_month = ddlMonth.SelectedItem.Value;
            //string strsql = " select distinct t.Equal_Subject_Code,s.subject_name from tbl_equal_paper_Matching t,Exam_Details ed,exam_application ea,exam_appl_details ead,Degree d,course c,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and ed.Exam_Month=t.exam_month and ed.Exam_year=t.exam_year and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and s.subject_code=t.Equal_Subject_Code and ed.exam_month='" + exam_month + "' and ed.exam_year='" + year + "' and c.Edu_Level='" +ddldegree.SelectedItem.ToString() + "' and c.type='" +ddltype.SelectedItem.ToString() + "' order by t.Equal_Subject_Code desc";
            string strsql = string.Empty;
            if (!cbGeneral.Checked)
            {
                strsql = " select distinct t.Equal_Subject_Code,s.subject_name,t.Com_Subject_Code from tbl_equal_paper_Matching t,Degree d,course c,subject s,syllabus_master sy where sy.syll_code=s.syll_code and d.Degree_Code=sy.degree_code and d.Course_Id=c.Course_Id and s.subject_code=t.Equal_Subject_Code and c.Edu_Level='" + ddldegree.SelectedItem.ToString() + "' and c.type='" + ddltype.SelectedItem.ToString() + "' order by t.Com_Subject_Code,s.subject_name,t.Equal_Subject_Code desc";
            }
            else
            {
                strsql = " select distinct t.Equal_Subject_Code,s.subject_name,t.Com_Subject_Code from tbl_Subject_paper_Matching t,Degree d,course c,subject s,syllabus_master sy where sy.syll_code=s.syll_code and d.Degree_Code=sy.degree_code and d.Course_Id=c.Course_Id and s.subject_code=t.Equal_Subject_Code and c.Edu_Level='" + ddldegree.SelectedItem.ToString() + "' and c.type='" + ddltype.SelectedItem.ToString() + "' order by t.Com_Subject_Code,s.subject_name,t.Equal_Subject_Code desc";
            }
            DataSet dsequalsubject = new DataSet();
            dsequalsubject.Clear();
            dsequalsubject = da.select_method_wo_parameter(strsql, "Text");
            if (dsequalsubject.Tables[0].Rows.Count > 0)
            {
                Fpsaved.Visible = true;
                btnremove.Visible = true;
                Fpsaved.Sheets[0].RowCount = 0;
                int sno = 0;
                for (int i = 0; i < dsequalsubject.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    Fpsaved.Sheets[0].RowCount++;
                    Fpsaved.Sheets[0].Cells[i, 0].Text = Convert.ToString(sno);
                    Fpsaved.Sheets[0].Cells[i, 1].Text = Convert.ToString(dsequalsubject.Tables[0].Rows[i]["Equal_Subject_Code"]);
                    Fpsaved.Sheets[0].Cells[i, 2].Text = Convert.ToString(dsequalsubject.Tables[0].Rows[i]["subject_name"]);
                    Fpsaved.Sheets[0].Cells[i, 3].CellType = chk;
                }
            }
            else
            {
                Fpsaved.Visible = false;
                btnremove.Visible = false;
            }
            Fpsaved.Sheets[0].PageSize = Fpsaved.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnremove_Click(object sender, EventArgs e)
    {
        try
        {

            Fpsaved.SaveChanges();
            Boolean flag = false;
            for (int i = 0; i < Fpsaved.Sheets[0].Rows.Count; i++)
            {
                if (Convert.ToInt32(Fpsaved.Sheets[0].Cells[i, 3].Value) == 1)
                {
                    string subcode = Fpsaved.Sheets[0].Cells[i, 1].Text.ToString();
                     string delquery=string.Empty;
                    if(!cbGeneral.Checked)
                         delquery = "delete from tbl_equal_paper_Matching Where Equal_Subject_Code='" + subcode + "' ";
                    else
                        delquery = "delete from tbl_Subject_paper_Matching Where (Equal_Subject_Code='" + subcode + "' or Com_Subject_Code='" + subcode + "')";

                    int a = da.update_method_wo_parameter(delquery, "Text");
                    if (a > 0)
                    {
                        flag = true;
                    }
                }
            }
            if (flag == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select The Subject's And Then Proceed')", true);
            }
            loadsavedetails();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    //Deepali 18.5.18==============================
    protected void cbGeneral_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbGeneral.Checked)
            {
                loadspread();
                ddlMonth.Enabled = false;
                ddlYear.Enabled = false;
            }
            else
            {
                loadspread();
                ddlMonth.Enabled = true;
                ddlYear.Enabled = true;
            }
        }
        catch { }
    }


    //=============================================
}