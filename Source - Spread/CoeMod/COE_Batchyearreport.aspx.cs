using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Windows.Forms;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Globalization;
using System.Configuration;

public partial class COE_Batchyearreport : System.Web.UI.Page
{
    static string[] ss;
    static string p = "";
    static string[] ss1;
    string ss2 = "";
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet hds = new DataSet();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();

    DataSet dsstudinfo = new DataSet();
    DataView dv = new DataView();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", grouporusercode = "";
    Boolean flag_true = false;
    ArrayList alv = new ArrayList();
    Hashtable hashmark = new Hashtable();
    ArrayList rights = new ArrayList();
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
            collegecode = Session["collegecode"].ToString();

            if (!IsPostBack)
            {

                ss2 = "";
                p = "";
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Trebuchet MS";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.White;
                style2.BackColor = Color.Teal;

                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                style2 = new FarPoint.Web.Spread.StyleInfo();


                style2.VerticalAlign = VerticalAlign.Middle;

                FpSpread2.Sheets[0].DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);


                // ddlsubtype.Items.Add(new ListItem(""));


                bindbatch();
                binddegree();
                bindbranch();
                bindsem();

                clear();
                clear();

            }
        }
        catch (Exception ex)
        {
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
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
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
            has.Add("college_code", collegecode);
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
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void bindsem()
    {
        try
        {

            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                        //ddlSemYr.Enabled = false;
                    }
                    else if (first_year == true && i == 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");
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
            ddlsem.Items.Insert(0, "All");
            bindsubjtype();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void bindsubjtype()
    {
        try
        {
            txtsubtype.Text = "---Select---";
            chksubtype.Checked = false;
            chklssubtype.Items.Clear();
            string sqlnew = "select distinct subject_type from syllabus_master sm,sub_sem s where sm.syll_code=s.syll_code  and batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            if (ddlsem.SelectedItem.ToString() != "All")
            {
                sqlnew = "select distinct subject_type from syllabus_master sm,sub_sem s where sm.syll_code=s.syll_code  and batch_year='" + ddlbatch.Text.ToString() + "' and semester='" + ddlsem.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            }
            DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklssubtype.DataSource = ds;
                chklssubtype.DataTextField = "subject_type";
                chklssubtype.DataBind();

                for (int i = 0; i < chklssubtype.Items.Count; i++)
                {
                    chklssubtype.Items[i].Selected = true;
                }
                txtsubtype.Text = "Subject Type (" + chklssubtype.Items.Count.ToString() + ")";
                chksubtype.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void bindbranch()
    {
        try
        {
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
            has.Add("college_code", collegecode);
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
            bindsem();

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void clear()
    {
        FpSpread2.Visible = false; btnreset.Visible = false;
        btnsave.Visible = false;
        lblvalidation1.Text = "";
        rptprint.Visible = false;
        btnPrint.Visible = false;
    }
    protected void chksubtype_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chksubtype.Checked == true)
            {
                for (int i = 0; i < chklssubtype.Items.Count; i++)
                {
                    chklssubtype.Items[i].Selected = true;
                }
                txtsubtype.Text = "Subject Type(" + (chklssubtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklssubtype.Items.Count; i++)
                {
                    chklssubtype.Items[i].Selected = false;
                }
                txtsubtype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void chklssubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtsubtype.Text = "--Select--";
            chksubtype.Checked = false;
            int count = 0;
            for (int i = 0; i < chklssubtype.Items.Count; i++)
            {
                if (chklssubtype.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtsubtype.Text = "Subject Type(" + count.ToString() + ")";
                if (count == chklssubtype.Items.Count)
                {
                    chksubtype.Checked = true;
                }
            }
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
            clear();
            binddegree();
            bindbranch();
            bindsem();
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
            clear();
            bindbranch();
            bindsem();
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
            clear();
            bindsem();
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
            bindsubjtype();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void loadheader()
    {
        DataSet printds = new DataSet();
        string depart_code = ddlbranch.SelectedValue.ToString();

        string strsubtype = "";
        for (int i = 0; i < chklssubtype.Items.Count; i++)
        {
            if (chklssubtype.Items[i].Selected == true)
            {
                if (strsubtype.Trim() != "")
                {
                    strsubtype = strsubtype + ",'" + chklssubtype.Items[i].Text.ToString() + "'";
                }
                else
                {
                    strsubtype = "'" + chklssubtype.Items[i].Text.ToString() + "'";
                }
            }
        }
        if (strsubtype.Trim() != "")
        {
            strsubtype = " And ss.subject_type in(" + strsubtype + ")";
        }

        string sql = "select (subject_code+'  ' + subject_name + ' Sem - '+ CONVERT (nvarchar(10), semester)) as subject_name ,subject_no from subject s,syllabus_master sm,sub_sem ss where s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and ss.subType_no=s.subType_no and Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and degree_code='" + depart_code + "' " + strsubtype + " order by sm.semester, s.subjectpriority";
        if (ddlsem.SelectedItem.ToString() != "All")
        {
            sql = "select (subject_code+'  ' + subject_name + ' Sem - '+ CONVERT (nvarchar(10), semester)) as subject_name ,subject_no from subject s,syllabus_master sm,sub_sem ss where s.syll_code=sm.syll_code and sm.syll_code=ss.syll_code and ss.subType_no=s.subType_no and Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and degree_code='" + depart_code + "' and semester='" + ddlsem.SelectedValue.ToString() + "' " + strsubtype + " order by sm.semester, s.subjectpriority";
        }
        printds.Clear();
        printds = da.select_method_wo_parameter(sql, "Text");
        if (printds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < printds.Tables[0].Rows.Count; i++)
            {
                FpSpread2.Sheets[0].ColumnCount++;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = printds.Tables[0].Rows[i][0].ToString();
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = printds.Tables[0].Rows[i][1].ToString();
                FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
            }
        }
    }

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            ss2 = "";
            p = "";
            FpSpread2.Visible = false; btnreset.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 5;
            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[1].Width = 113;
            FpSpread2.Sheets[0].Columns[2].Width = 342;
            FpSpread2.Sheets[0].Columns[3].Width = 83;
            FpSpread2.Sheets[0].Columns[4].Width = 90;

            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg.No.";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Papers Selected";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Papers Appeared";

            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[2].Locked = true;
            FpSpread2.Sheets[0].Columns[4].Locked = true;

            FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            loadheader();

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread2.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();

            chkcell1.AutoPostBack = true;
            chkcell.AutoPostBack = true;
            FpSpread2.Sheets[0].AutoPostBack = true;

            DataSet printds = new DataSet();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string year = ddlbatch.SelectedValue.ToString();
            string degree = ddldegree.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string depart_code = ddlbranch.SelectedValue.ToString();
            string batchyearatt = ddlbatch.SelectedValue.ToString();
            string studinfo = "";
            studinfo = "select Stud_Name,Roll_No,Reg_No,APP_No from Registration  where Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and degree_code='" + depart_code + "' and DelFlag=0 and Exam_Flag<>'DEBAR' order by Reg_No  ; select m.roll_no,ed.Exam_year,ed.Exam_Month,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result from mark_entry m,Exam_Details ed where ed.exam_code=m.exam_code and ed.batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and ed.degree_code='" + depart_code + "' order by m.roll_no,m.subject_no,ed.Exam_year,ed.Exam_Month";
            studinfo = studinfo + " select r.Roll_No,Count(distinct sc.subject_no) subcount from Registration r,subjectChooser sc where r.Roll_No=sc.roll_no and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.degree_code='" + depart_code + "' and DelFlag=0 and Exam_Flag<>'DEBAR' group by r.Roll_No";
            studinfo = studinfo + " select r.Roll_No,Count(distinct m.subject_no) subcount from Registration r,mark_entry m,subjectChooser sc where r.Roll_No=m.roll_no and r.Roll_No=sc.roll_no and sc.subject_no=m.subject_no and sc.roll_no=m.roll_no and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.degree_code='" + depart_code + "' and DelFlag=0 and Exam_Flag<>'DEBAR' group by r.Roll_No";

            dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            dv = new DataView();
            if (dsstudinfo.Tables[0].Rows.Count > 0)
            {
                btnsave.Visible = false;
                FpSpread2.Visible = true;
                btnPrint.Visible = true;
                int sno = 0;
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {
                    sno++;
                    string roll_no = dsstudinfo.Tables[0].Rows[studcount]["Roll_No"].ToString();

                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dsstudinfo.Tables[0].Rows[studcount]["Reg_No"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = dsstudinfo.Tables[0].Rows[studcount]["Roll_No"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsstudinfo.Tables[0].Rows[studcount]["Stud_Name"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = dsstudinfo.Tables[0].Rows[studcount]["APP_No"].ToString();

                    int noofsubject = 0;
                    dsstudinfo.Tables[2].DefaultView.RowFilter = "roll_no='" + roll_no + "'";
                    dv = dsstudinfo.Tables[2].DefaultView;
                    if (dv.Count > 0)
                    {
                        noofsubject = Convert.ToInt32(dv[0]["subcount"].ToString());
                    }
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = noofsubject.ToString();

                    noofsubject = 0;
                    dsstudinfo.Tables[3].DefaultView.RowFilter = "roll_no='" + roll_no + "'";
                    dv = dsstudinfo.Tables[3].DefaultView;
                    if (dv.Count > 0)
                    {
                        noofsubject = Convert.ToInt32(dv[0]["subcount"].ToString());
                    }
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = noofsubject.ToString();

                    int startrow = FpSpread2.Sheets[0].RowCount;
                    for (int i = 5; i < FpSpread2.Sheets[0].Columns.Count; i++)
                    {
                        string strm_y = "";
                        int noofrow = startrow;

                        string subject_no = FpSpread2.Sheets[0].ColumnHeader.Cells[0, i].Tag.ToString();
                        dsstudinfo.Tables[1].DefaultView.RowFilter = "roll_no='" + roll_no + "' and subject_no='" + subject_no + "'";
                        dv = dsstudinfo.Tables[1].DefaultView;

                        for (int k = 0; k < dv.Count; k++)
                        {
                            int e_month = Convert.ToInt32(dv[k]["Exam_Month"].ToString());
                            string e_year = dv[k]["Exam_year"].ToString();
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames[e_month - 1];
                            strMonthName = e_year + "-" + strMonthName;
                            string strintmark = dv[k]["internal_mark"].ToString();
                            if (strintmark.Trim() == "-1")
                            {
                                strintmark = "AB";
                            }
                            else if (strintmark.Trim() == "-2")
                            {
                                strintmark = "NE";
                            }
                            else if (strintmark.Trim() == "-3")
                            {
                                strintmark = "NR";
                            }

                            string strextmark = dv[k]["external_mark"].ToString();
                            if (strextmark.Trim() == "-1")
                            {
                                strextmark = "AB";
                            }
                            else if (strextmark.Trim() == "-2")
                            {
                                strextmark = "NE";
                            }
                            else if (strextmark.Trim() == "-3")
                            {
                                strextmark = "NR";
                            }

                            if (strm_y.Trim() == "")
                            {
                                strm_y = strMonthName + "-" + strintmark + "," + strextmark;
                            }
                            else
                            {
                                strm_y = strm_y + " " + strMonthName + "-" + strintmark + "," + strextmark;
                                noofrow++;
                                if (FpSpread2.Sheets[0].RowCount < noofrow)
                                {
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Border.BorderColorBottom = Color.White;
                                }
                            }
                            FpSpread2.Sheets[0].Cells[noofrow - 1, i].Text = strMonthName + "-" + strintmark + "," + strextmark;
                        }
                    }
                }
                dsstudinfo.Clear();
                dsstudinfo.Dispose();
                rptprint.Visible = true;
            }
            else
            {
                clear();
                lblerror.Text = "No Records Found";
                lblerror.Visible = true;
            }
            string totalrows = FpSpread2.Sheets[0].RowCount.ToString();
            FpSpread2.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread2.Height = (Convert.ToInt32(totalrows) * 50) + 40;

            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[1].Width = 113;
            FpSpread2.Sheets[0].Columns[2].Width = 342;
            FpSpread2.Sheets[0].Columns[3].Width = 83;
            FpSpread2.Sheets[0].Columns[4].Width = 90;
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.SaveChanges();
            FpSpread2.Visible = true;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int a = 0;
            FpSpread2.SaveChanges();
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 3].Value);
                if (isval == 1)
                {
                    string sql = " update subject set subjectpriority='" + FpSpread2.Sheets[0].Cells[res, 4].Text.ToString() + "' where subject_no='" + FpSpread2.Sheets[0].Cells[res, 1].Note.ToString() + "'";
                    a = da.update_method_wo_parameter(sql, "Text");

                }
            }

            if (a == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved successfully')", true);
            }


        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void btnresetclick(object sender, EventArgs e)
    {
        try
        {
            ss2 = "";
            p = "";
            int a = 0;
            FpSpread2.SaveChanges();
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 3].Value);
                if (isval == 1)
                {
                    FpSpread2.Sheets[0].Cells[res, 4].Text = "";
                    FpSpread2.Sheets[0].Cells[res, 3].Value = 0;
                    FpSpread2.Sheets[0].Cells[res, 3].Locked = false;
                }
            }
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
            string actrow1;
            actrow1 = e.SheetView.ActiveRow.ToString();
            if (flag_true == false && actrow1 == "0")
            {
                for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); j++)
                {
                    string actcol1 = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[Convert.ToInt16(actcol1)].ToString();
                    if (seltext != "System.Object")
                        FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol1)].Text = seltext.ToString();
                }
                flag_true = true;
            }
            else if (actrow1 != "0")
            {
                string number = "True";
                int actcol = Convert.ToInt16(e.SheetView.ActiveColumn.ToString());
                int actrow = Convert.ToInt16(e.SheetView.ActiveRow.ToString());
                string st1;
                string st;
                st = FpSpread2.GetEditValue(actrow, actcol).ToString();
                //  string sssshhs = sprdHallMaster.GetEditValue(1, 7).ToString();
                st1 = e.EditValues[actcol].ToString();
                if (st == number)
                {
                    if (p == "")
                    {
                        p = actrow.ToString();
                    }
                    else
                    {
                        p = p + "-" + actrow.ToString();
                    }
                    ss = p.Split(new char[] { '-' });
                    int cnt12 = 0;
                    for (int i = 0; i < ss.Length; i++)
                    {
                        if (ss[i] != "")
                        {
                            cnt12 = cnt12 + 1;
                            FpSpread2.Sheets[0].Cells[Convert.ToInt16(ss[i]), 4].Text = cnt12.ToString();

                        }
                    }

                }
                else
                {

                    for (int j = 0; j < ss.Length; j++)
                    {
                        int n;
                        if (ss[j] == "")
                        {
                            n = 0;
                        }
                        else
                        {
                            n = Convert.ToInt16(ss[j]);

                        }

                        if (n == actrow)
                        {
                            FpSpread2.Sheets[0].Cells[n, 4].Text = "";
                            ss[j] = "";

                        }
                        else
                        {


                            if (ss2 == "")
                            {
                                ss2 = ss[j].ToString();
                            }
                            else
                            {
                                ss2 = ss2 + "-" + ss[j].ToString();
                            }
                        }
                    }
                    int ccnt = 0;
                    ss1 = ss2.Split(new char[] { '-' });
                    for (int s = 0; s < ss1.Length; s++)
                    {
                        if (ss1[s] != "")
                        {
                            ccnt = ccnt + 1;
                            FpSpread2.Sheets[0].Cells[Convert.ToInt16(ss1[s]), 4].Text = ccnt.ToString();

                        }
                    }

                    p = ss2;
                }

            }
        }
        catch (Exception ex)
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
                d2.printexcelreport(FpSpread2, reportname);
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Papers Selected by the Students of " + ddldegree.SelectedItem.Text.ToString() + " " + ddlbranch.SelectedItem.Text.ToString() + " Batch " + ddlbatch.SelectedItem.Text.ToString() + " ";
            string pagename = "COE_Batchyearreport.aspx";
            Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printcontrol.Visible = true;
            // 
        }
        catch
        {
        }
    }
    protected void btn_commonprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            string coename = "";
            string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = "";
            string aff = "";
            string collacr = "";
            string dispin = "";
            string category = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                string[] strpa = aff.Split(',');
                aff = strpa[0];
                coename = ds.Tables[0].Rows[0]["coe"].ToString();
                collacr = ds.Tables[0].Rows[0]["acr"].ToString();
                dispin = ds.Tables[0].Rows[0]["districtpin"].ToString();
                category = ds.Tables[0].Rows[0]["category"].ToString();
            }
            string degreecode = ddlbranch.SelectedValue.ToString();
            //string 
            //string eve = d2.GetFunction(" select c.type from Degree d,Course c where d.Course_Id=c.Course_Id and d.Degree_Code='" + degreecode + "'");
            string eve = "";
            string course = "";
            string deptacr = "";

            string headingquery = "select c.type,c.Course_Name,de.dept_name from Degree d,Course c,Department de where d.Course_Id=c.Course_Id and de.Dept_Code=d.Dept_Code and d.Degree_Code='" + degreecode + "'";
            hds = d2.select_method_wo_parameter(headingquery, "Text");
            if (hds.Tables[0].Rows.Count > 0)
            {
                eve = hds.Tables[0].Rows[0]["type"].ToString();
                course = hds.Tables[0].Rows[0]["Course_Name"].ToString();
                deptacr = hds.Tables[0].Rows[0]["dept_name"].ToString();
            }
            string batch = ddlbatch.SelectedItem.Text;
            string title = "PAPERS SELECTED BY THE STUDENTS OF " + course + " - " + deptacr + " - BATCH " + batch + " (" + eve + ")";

            Font Fontbold1 = new Font("Times New Roman", 15, FontStyle.Bold);
            Font font2bold = new Font("Times New Roman", 12, FontStyle.Bold);
            Font font2small = new Font("Times New Roman", 12, FontStyle.Regular);
            Font font3bold = new Font("Times New Roman", 9, FontStyle.Bold);
            Font font3small = new Font("Times New Roman", 10, FontStyle.Regular);
            Font font4bold = new Font("Times New Roman", 7, FontStyle.Bold);
            Font font4small = new Font("Times New Roman", 7, FontStyle.Regular);

            Gios.Pdf.PdfDocument mydoc;
            Gios.Pdf.PdfPage mypdfpage;
            Gios.Pdf.PdfTable table1forpage2;
            Gios.Pdf.PdfTablePage newpdftabpage2;

            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                int nofocolun = 0;
                Hashtable hatrowset = new Hashtable();
                int haskpage = 0;
                int checkrow = 0;
                for (int r = 0; r < FpSpread2.Sheets[0].RowCount; r++)
                {
                    string nameval = FpSpread2.Sheets[0].Cells[r, 1].Text.ToString();
                    if (nameval.Trim() != "")
                    {
                        checkrow++;
                        if (checkrow == 11 || checkrow == 1)
                        {
                            haskpage++;
                            checkrow = 1;
                        }
                    }
                    if (hatrowset.Contains(haskpage))
                    {
                        hatrowset[haskpage] = Convert.ToInt32(hatrowset[haskpage]) + 1;
                    }
                    else
                    {
                        hatrowset.Add(haskpage, 1);
                    }
                }
                int pagcount = hatrowset.Count;
                haskpage = 0;
                int totcol = FpSpread2.Sheets[0].ColumnCount - 3;
                int noofcolumn = totcol / 10;
                if (((FpSpread2.Sheets[0].ColumnCount - 3) % 10) > 0)
                {
                    noofcolumn++;
                }
                mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(60, 40));
                int startcolun = 0;
                int endcolumn;
                int stratrow = 0;
                int endrow = 0;
                for (int pc = 1; pc <= pagcount; pc++)
                {
                    int noofrows = Convert.ToInt32(hatrowset[pc]);
                    startcolun = 3;
                    stratrow = endrow;
                    endrow = stratrow + noofrows;
                    startcolun = 3;
                    int colcou = 3;
                    for (int col = 0; col < noofcolumn; col++)
                    {
                        if (col > 0)
                        {
                            startcolun = startcolun + 10;
                        }
                        endcolumn = startcolun + 10;
                        if (endcolumn > FpSpread2.Sheets[0].ColumnCount)
                        {
                            endcolumn = FpSpread2.Sheets[0].ColumnCount;
                        }
                        colcou = colcou + 10;
                        nofocolun = 13;
                        if (colcou > FpSpread2.Sheets[0].ColumnCount)
                        {
                            colcou = (FpSpread2.Sheets[0].ColumnCount + 10) - colcou;
                            nofocolun = colcou + 3;
                        }

                        mypdfpage = mydoc.NewPage();
                        int coltop = 20;

                        #region Left Logo

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 35, 20, 320);
                        }

                        #endregion

                        #region TOP DETAILS

                        coltop = coltop + 10;

                        PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 0, coltop, 1700, 30), System.Drawing.ContentAlignment.TopCenter, Collegename + "(" + category + ")");
                        mypdfpage.Add(ptc);

                        coltop = coltop + 20;

                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 0, coltop, 1700, 30), System.Drawing.ContentAlignment.TopCenter, title);
                        mypdfpage.Add(ptc);

                        coltop = coltop + 20;

                        //ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                        //                                            new PdfArea(mydoc, 560, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
                        //mypdfpage.Add(ptc);
                        //coltop = coltop + 15;

                        //ptc = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                        //                                            new PdfArea(mydoc, 560, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, dispin);
                        //mypdfpage.Add(ptc);

                        #endregion

                        # region Table Binding

                        table1forpage2 = mydoc.NewTable(font3small, noofrows + 1, nofocolun, 4);
                        table1forpage2.VisibleHeaders = false;
                        table1forpage2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 0).SetContent("S.No");
                        table1forpage2.Columns[0].SetWidth(30);
                        table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 1).SetContent("Reg.No");
                        table1forpage2.Columns[1].SetWidth(60);
                        table1forpage2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2.Cell(0, 2).SetContent("Student Name");
                        table1forpage2.Columns[2].SetWidth(150);
                        //table1forpage2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //table1forpage2.Cell(0, 3).SetContent("(Total Papers Selected)");
                        //table1forpage2.Columns[3].SetWidth(80);
                        //table1forpage2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //table1forpage2.Cell(0, 4).SetContent("Total Papers Appeared");
                        //table1forpage2.Columns[4].SetWidth(80);

                        table1forpage2.Cell(0, 0).SetFont(font2bold);
                        table1forpage2.Cell(0, 1).SetFont(font2bold);
                        table1forpage2.Cell(0, 2).SetFont(font2bold);
                        //table1forpage2.Cell(0, 3).SetFont(font2bold);
                        //table1forpage2.Cell(0, 4).SetFont(font2bold);

                        int totalrow = endrow - stratrow;
                        int tr = 0;
                        int tc = 3;
                        for (int r = stratrow; r < endrow; r++)
                        {
                            tr++;
                            tc = 2;
                            table1forpage2.Cell(tr, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(tr, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(tr, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //table1forpage2.Cell(tr, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            //table1forpage2.Cell(tr, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(tr, 0).SetContent(FpSpread2.Sheets[0].Cells[r, 0].Text.ToString());
                            table1forpage2.Cell(tr, 1).SetContent(FpSpread2.Sheets[0].Cells[r, 1].Text.ToString());
                            table1forpage2.Cell(tr, 1).SetFont(font2bold);
                            table1forpage2.Cell(tr, 2).SetContent(FpSpread2.Sheets[0].Cells[r, 2].Text.ToString());
                            table1forpage2.Cell(tr, 2).SetFont(font2bold);
                            //table1forpage2.Cell(tr, 3).SetContent(FpSpread2.Sheets[0].Cells[r, 3].Text.ToString());
                            //table1forpage2.Cell(tr, 3).SetFont(font3small);
                            //table1forpage2.Cell(tr, 4).SetContent(FpSpread2.Sheets[0].Cells[r, 4].Text.ToString());
                            //table1forpage2.Cell(tr, 4).SetFont(font3small);
                            for (int c = startcolun; c < endcolumn; c++)
                            {
                                tc++;
                                if (tc < FpSpread2.Sheets[0].ColumnCount)
                                {
                                    if (r == stratrow)
                                    {
                                        table1forpage2.Cell(0, tc).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(0, tc).SetContent(FpSpread2.Sheets[0].ColumnHeader.Cells[0, c].Text.ToString());
                                        table1forpage2.Cell(0, tc).SetFont(font2bold);
                                        table1forpage2.Columns[tc].SetWidth(100);
                                        table1forpage2.Cell(tr, tc).SetCellPadding(20);
                                    }
                                    table1forpage2.Cell(tr, tc).SetContent(FpSpread2.Sheets[0].Cells[r, c].Text.ToString());
                                    table1forpage2.Cell(tr, tc).SetFont(font2bold);
                                    string value = FpSpread2.Sheets[0].Cells[r, c].Text.ToString().ToString();
                                    if (value.Trim() == "")
                                    {
                                        table1forpage2.Cell(tr, tc).SetContent(".");
                                        table1forpage2.Cell(tr, tc).SetForegroundColor(Color.White);
                                    }
                                    if (totalrow > 40)
                                    {
                                        table1forpage2.Cell(tr, tc).SetCellPadding(1);
                                    }
                                    else if (totalrow > 30)
                                    {
                                        table1forpage2.Cell(tr, tc).SetCellPadding(5);
                                    }
                                    else if (totalrow > 18)
                                    {
                                        table1forpage2.Cell(tr, tc).SetCellPadding(10);
                                    }
                                    else
                                    {
                                        table1forpage2.Cell(tr, tc).SetCellPadding(20);
                                    }
                                }
                            }
                        }
                        newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 130, 1670, 5000));
                        mypdfpage.Add(newpdftabpage2);
                        mypdfpage.SaveToDocument();

                        #endregion
                    }
                }

                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "SubjectAllotment" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
}