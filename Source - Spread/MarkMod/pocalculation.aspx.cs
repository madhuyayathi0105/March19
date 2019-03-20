using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using InsproDataAccess;
using System.Configuration;
using System.Data.SqlClient;

public partial class MarkMod_pocalculation : System.Web.UI.Page
{

    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;
    string columnfield = string.Empty;

    ReuasableMethods rs = new ReuasableMethods();
    InsproDirectAccess dir = new InsproDirectAccess();
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();

    int tblcount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }
            collegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            usercode = (Session["userco,de"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            group_user = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

            if (!IsPostBack)
            {
                bindclg();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();

            }

        }
        catch
        {
        }
    }


    #region bindheaders

    public void bindclg()
    {
        try
        {

            group_code = Session["group_code"].ToString();
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
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

            }
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnmasterprint.Visible = false;
            GridView1.Visible = false;

        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        try
        {

            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnmasterprint.Visible = false;
            GridView1.Visible = false;
        }
        catch
        {
        }
    }

    public void binddegree()
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnmasterprint.Visible = false;
            GridView1.Visible = false;
        }
        catch
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            string typ = ddldegree.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", typ);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnmasterprint.Visible = false;
            GridView1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }

    public void bindsem()
    {
        try
        {
            ddlsemester.Items.Clear();

            string degreecode = ddlbranch.SelectedValue.ToString();
            string strgetfuncuti = string.Empty;
            if (degreecode.Trim() != "")
            {
                strgetfuncuti = d2.GetFunction("select max(Duration) from Degree where Degree_Code in(" + degreecode + ")");
            }
            if (Convert.ToInt32(strgetfuncuti) > 0)
            {
                for (int loop_val = 1; loop_val <= Convert.ToInt32(strgetfuncuti); loop_val++)
                {
                    ddlsemester.Items.Add(loop_val.ToString());
                }
            }
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnmasterprint.Visible = false;
            GridView1.Visible = false;

        }
        catch
        {
        }
    }

    #endregion


    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        bindsem();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
    }
    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public string filterfunction()
    {
        string strorder = "ORDER BY Registration.Roll_No";
        string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Convert.ToString(ddlcollege.SelectedValue) + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "   order by registration.serialno";
        }
        else
        {
            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            if (orderby_Setting == "0")
            {
                strorder = "  ORDER BY Registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "   ORDER BY Registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "  ORDER BY Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "  ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "   ORDER BY Registration.Roll_No,Registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "  ORDER BY Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "  ORDER BY Registration.Roll_No,Registration.Stud_Name";
            }
        }
        return strorder;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string qry = string.Empty;
            string clgcode = Convert.ToString(ddlcollege.SelectedValue);
            string degreecode = Convert.ToString(ddlbranch.SelectedValue);
            string batch = Convert.ToString(ddlbatch.SelectedValue);
            string sem = Convert.ToString(ddlsemester.SelectedItem.Text);

            string strstaffselecotr = string.Empty;
            string staffCode = Convert.ToString(Session["staff_code"]);
            bool staffSelector = false;
            string minimumabsentsms = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(ddlcollege.SelectedValue).Trim() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(ddlbatch.SelectedItem.Text.ToString()) >= batchyearsetting)
                    {
                        staffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            if (staffSelector == true && !string.IsNullOrEmpty(staffCode))
            {
                strstaffselecotr = " and sc.staffcode like '%" + Convert.ToString(staffCode) + "%'";
            }
            string strorder = filterfunction();
            DataTable dtPoMaster = new DataTable();
            dtPoMaster = dir.selectDataTable("select MasterValue,MasterCode from CO_MasterValues where MasterCriteria='posettings' and collegecode='" + clgcode + "'");

            qry = " select  r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.App_no,r.Sections   from Registration r where r.batch_year='" + batch + "' and r.degree_code='" + degreecode + "' and r.Current_Semester='" + sem + "'  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' " + strorder + "";
            qry = qry + " select distinct subject.subject_name,subject.subject_code,subject.subject_no from subject,sub_sem,syllabus_master where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code   and  syllabus_master.degree_code in('" + degreecode + "') and syllabus_master.batch_year in('" + batch + "') and syllabus_master.semester='" + sem + "'  order by subject.subject_name";
            DataSet ds1 = d2.select_method_wo_parameter(qry, "text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                string subjectNo = string.Empty;


                #region datatable column

                dt.Columns.Add("SNo");
                dt.Columns.Add("Roll No");
                dt.Columns.Add("Reg No");
                dt.Columns.Add("Student Name");
                if (dtPoMaster.Rows.Count > 0)
                {
                    foreach (DataRow drCo in dtPoMaster.Rows)
                    {
                        string coName = Convert.ToString(drCo["MasterValue"]);
                        dt.Columns.Add(coName.Trim().ToUpper() + "(S)");
                        dt.Columns.Add(coName.Trim().ToUpper() + "(M)");
                        dt.Columns.Add(coName.Trim().ToUpper() + "(W)");
                    }
                }

                #endregion

                #region extra row

                dr = dt.NewRow();
                dr["SNo"] = "S.No";
                dr["Roll No"] = "Roll No";
                dr["Reg No"] = "Reg No";
                dr["Student Name"] = "Student Name";

                if (dtPoMaster.Rows.Count > 0)
                {
                    foreach (DataRow drCo in dtPoMaster.Rows)
                    {
                        string coName = Convert.ToString(drCo["MasterValue"]);
                        dr[coName.Trim().ToUpper() + "(S)"] = coName.Trim().ToUpper();
                        dr[coName.Trim().ToUpper() + "(M)"] = coName.Trim().ToUpper();
                        dr[coName.Trim().ToUpper() + "(W)"] = coName.Trim().ToUpper();
                    }
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["SNo"] = "S.No";
                dr["Roll No"] = "Roll No";
                dr["Reg No"] = "Reg No";
                dr["Student Name"] = "Student Name";

                if (dtPoMaster.Rows.Count > 0)
                {
                    foreach (DataRow drCo in dtPoMaster.Rows)
                    {
                        string coName = Convert.ToString(drCo["MasterValue"]);

                        dr[coName.Trim().ToUpper() + "(S)"] = "S";
                        dr[coName.Trim().ToUpper() + "(M)"] = "M";
                        dr[coName.Trim().ToUpper() + "(W)"] = "W";
                    }
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Student Name"] = "CO Sum";
                dt.Rows.Add(dr);
                #endregion

                if (ds1.Tables[1].Rows.Count > 0)
                {

                    for (int k = 0; k < ds1.Tables[1].Rows.Count; k++)
                    {
                        if (string.IsNullOrEmpty(subjectNo))
                            subjectNo = "'" + Convert.ToString(ds1.Tables[1].Rows[k]["subject_no"]) + "'";
                        else
                            subjectNo = subjectNo + ",'" + Convert.ToString(ds1.Tables[1].Rows[k]["subject_no"]) + "'";
                    }
                }
                if (!string.IsNullOrEmpty(subjectNo))
                {
                    string posettings = "select c.*,m.template from COThresholdSettings c,Master_Settings m where m.settings='COSettings' and c.CourseID=m.masterno and c.Subject_NO in(" + subjectNo + ")";
                    DataTable dtPoSettings = dir.selectDataTable(posettings);
                    string comarks = "select app_no,cono,subject_no,mark from coattainmentmarks where subject_no in (" + subjectNo + ")";
                    DataTable dtcomarks = dir.selectDataTable(comarks);
                    if (dt.Columns.Count > 0)
                    {
                        for (int col = 4; col < dt.Columns.Count; col++)
                        {


                            string colName = Convert.ToString(dt.Columns[col]);
                            string[] colspt = colName.Split('(');
                            int val = 0;
                            string val1 = Convert.ToString(colspt[0]);
                            string val2 = Convert.ToString(colspt[1]);
                            if (val2.Contains('S'))
                                val = 3;
                            else if (val2.Contains('M'))
                                val = 2;
                            else if (val2.Contains('W'))
                                val = 1;

                            dtPoSettings.DefaultView.RowFilter = "   ISNULL(" + val1 + ",0)='" + val + "'";
                            DataView dv = dtPoSettings.DefaultView;
                            int count = dv.Count;

                            dt.Rows[2][colName] = Convert.ToString(count);

                        }
                    }
                    else
                    {
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        btnmasterprint.Visible = false;
                        div4.Visible = true;
                        div5.Visible = true;
                        Label4.Visible = true;
                        Label4.Text = "Please Generate CO Attainment";
                        return;
                    }

                    int sno = 0;
                    double totmark = 0;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        totmark = 0;
                        string rollno = Convert.ToString(ds1.Tables[0].Rows[i]["roll_no"]);
                        string appno = Convert.ToString(ds1.Tables[0].Rows[i]["App_No"]);
                        string regno = Convert.ToString(ds1.Tables[0].Rows[i]["reg_no"]);
                        string studname = Convert.ToString(ds1.Tables[0].Rows[i]["stud_name"]);

                        dr = dt.NewRow();
                        sno++;
                        dr["SNo"] = Convert.ToString(sno);
                        dr["Roll No"] = Convert.ToString(rollno);
                        dr["Reg No"] = Convert.ToString(regno);
                        dr["Student Name"] = Convert.ToString(studname);
                        if (dt.Columns.Count > 0)
                        {
                            for (int col1 = 4; col1 < dt.Columns.Count; col1++)
                            {
                                totmark = 0;
                                string colName = Convert.ToString(dt.Columns[col1]);
                                string[] colspt = colName.Split('(');
                                int val = 0;
                                string val1 = Convert.ToString(colspt[0]);
                                string val2 = Convert.ToString(colspt[1]);
                                if (val2.Contains('S'))
                                    val = 3;
                                else if (val2.Contains('M'))
                                    val = 2;
                                else if (val2.Contains('W'))
                                    val = 1;
                                dtPoSettings.DefaultView.RowFilter = "   ISNULL(" + val1 + ",0)='" + val + "'";
                                DataTable dtmark = dtPoSettings.DefaultView.ToTable();
                                foreach (DataRow dr1 in dtmark.Rows)
                                {
                                    string conum = Convert.ToString(dr1["CourseID"]);
                                    string subjno = Convert.ToString(dr1["Subject_NO"]);
                                    dtcomarks.DefaultView.RowFilter = " app_no='" + appno + "' and cono='" + conum + "' and subject_no='" + subjno + "'";
                                    DataView dv2 = dtcomarks.DefaultView;
                                    if (dv2.Count > 0)
                                    {
                                        double mark = 0;
                                        string mark1 = Convert.ToString(dv2[0]["mark"]);
                                        double.TryParse(mark1, out mark);
                                        totmark += mark;
                                    }
                                }
                                dr[colName] = Convert.ToString(totmark);
                            }
                        }

                        dt.Rows.Add(dr);

                    }
                    GridView1.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    btnmasterprint.Visible = true;
                    tblcount = dt.Columns.Count;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    GridView1.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    GridView1.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    GridView1.Rows[1].Font.Bold = true;
                    GridView1.Rows[0].Font.Bold = true;
                    GridView1.Rows[2].Font.Bold = true;
                    GridView1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    GridView1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                    GridView1.Rows[2].HorizontalAlign = HorizontalAlign.Center;
                    GridView1.Rows[2].BackColor = ColorTranslator.FromHtml("#FF9999");
                    GridViewRow row = GridView1.Rows[0];
                    GridViewRow previousRow = GridView1.Rows[1];
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Text == previousRow.Cells[i].Text)
                        {
                            row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                   previousRow.Cells[i].RowSpan + 1;
                            previousRow.Cells[i].Visible = false;
                        }

                    }

                    for (int cell = GridView1.Rows[0].Cells.Count - 1; cell > 0; cell--)
                    {
                        TableCell colum = GridView1.Rows[0].Cells[cell];
                        TableCell previouscol = GridView1.Rows[0].Cells[cell - 1];
                        if (colum.Text == previouscol.Text)
                        {
                            if (previouscol.ColumnSpan == 0)
                            {
                                if (colum.ColumnSpan == 0)
                                {
                                    previouscol.ColumnSpan += 2;

                                }
                                else
                                {
                                    previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                }
                                colum.Visible = false;

                            }
                        }
                    }


                }
                else
                {
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnmasterprint.Visible = false;
                    div4.Visible = true;
                    div5.Visible = true;
                    Label4.Visible = true;
                    Label4.Text = "No SUbjects Found";
                }

            }
            else
            {
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnmasterprint.Visible = false;
                div4.Visible = true;
                div5.Visible = true;
                Label4.Visible = true;
                Label4.Text = "No Students Found";
            }

        }
        catch
        {
        }
    }

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex != 0 && e.Row.RowIndex != 1 && e.Row.RowIndex != 2)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[1].Font.Bold = true;
                e.Row.Cells[2].Font.Bold = true;
                e.Row.Cells[3].Font.Bold = true;
                e.Row.Cells[0].Width = 50;
                e.Row.Cells[1].Width = 100;
                e.Row.Cells[2].Width = 100;
                e.Row.Cells[3].Width = 200;
                int j = 0;
                for (int i = 4; i < tblcount; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[i].Width = 100;
                    e.Row.Cells[i].Font.Bold = true;                  
                }
                for (int i1 = 4; i1 < tblcount; i1++)
                {
                    j++;
                    e.Row.Cells[i1].BackColor = ColorTranslator.FromHtml("#66CCFF");
                    if (j == 3)
                    {                      
                        i1 += 3;
                        j = 0;
                    }                    
                }
                for (int i1 = 7; i1 < tblcount; i1++)
                {
                    j++;
                    e.Row.Cells[i1].BackColor = ColorTranslator.FromHtml("#66CCCC");
                    if (j == 3)
                    {
                        i1 += 3;
                        j = 0;
                    }
                }
            }
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                d2.printexcelreportgrid(GridView1, reportname);
                lbl_err.Visible = false;
            }
            else
            {
                lbl_err.Text = "Please Enter Your Report Name";
                lbl_err.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string ss = Convert.ToString(Session["usercode"]);
        GridView1.Visible = true;

        string degreedetails = string.Empty;
        degreedetails ="Student PO Analysis @" + ddlbatch.SelectedItem.Text+ " " + ddldegree.SelectedItem.Text + " " + ddlbranch.SelectedItem.Text + "     Semester-"+ddlsemester.SelectedItem.Text.ToString() ;
        Printcontrol.loadspreaddetails(GridView1, "pocalculation.aspx", degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }

    protected void btnaltok_Click(object sender, EventArgs e)
    {
        div4.Visible = true;
        div5.Visible = true;
    }
}