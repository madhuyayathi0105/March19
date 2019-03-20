using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using wc = System.Web.UI.WebControls;
using System.Configuration;
public partial class StudentMod_Student_FingerPrint_Reg : System.Web.UI.Page
{
    #region Field_declaration
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    DataTable dtCommon = new DataTable();
    ReuasableMethods rs = new ReuasableMethods();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collcode = string.Empty;
    string libcode = string.Empty;
    string libname = string.Empty;
    string activerow = "";
    string activecol = "";
    int selectedcount = 0;
    Boolean Cellclick = false;
    static string collegecode = string.Empty;
    string batch = "";
    string courseid = "";
    string bran = "";
    string sem = "";
    string sec = "";
    string Section = "";
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }//****************************************************//

            userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

            if (!IsPostBack)
            {
                Bindcollege();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                stdbindbatch();
                stdbinddegree();
                stdbindbranch();
                stdbindsem();
                stdbindsec();
                studentlist();
                if (ddlcoll.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcoll.SelectedValue);
                }
            }
            if (ddlcoll.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcoll.SelectedValue);
            }
        }
        catch
        {

        }
    }

    #region College

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;


                ddlcoll.DataSource = dtCommon;
                ddlcoll.DataTextField = "collname";
                ddlcoll.DataValueField = "college_code";
                ddlcoll.DataBind();
                ddlcoll.SelectedIndex = 0;
                ddlcoll.Enabled = true;

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;

    }


    protected void ddlcoll_Change(object sender, EventArgs e)
    {
        stdbindbatch();
        stdbinddegree();
        stdbindbranch();
        stdbindsem();
        stdbindsec();
        studentlist();
    }

    #endregion

    #region Batch
    public void bindbatch()
    {
        try
        {

            cbl_batchyear.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                cbl_batchyear.DataSource = ds;
                cbl_batchyear.DataTextField = "batch_year";
                cbl_batchyear.DataValueField = "batch_year";
                cbl_batchyear.DataBind();
                if (cbl_batchyear.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batchyear.Items.Count; i++)
                    {
                        cbl_batchyear.Items[i].Selected = true;
                    }
                    txt_batchyr.Text = Lblbatch.Text + "(" + cbl_batchyear.Items.Count + ")";
                }


            }

            //if (count > 0)
            //{
            //    ddlbatch.DataSource = ds;
            //    ddlbatch.DataTextField = "batch_year";
            //    ddlbatch.DataValueField = "batch_year";
            //    ddlbatch.DataBind();

            //}
            //int count1 = ds.Tables[1].Rows.Count;
            //if (count > 0)
            //{
            //    int max_bat = 0;
            //    max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            //    ddlbatch.SelectedValue = max_bat.ToString();
            //}
        }
        catch (Exception ex) { }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            binddegree();
            bindsem();
            bindsec();
            Fpspreadpop.Visible = false;
            rptprint.Visible = false;
            btndelete.Visible = false;

        }
        catch (Exception ex) { }

    }
    #endregion

    #region Degree
    public void binddegree()
    {
        try
        {

            cbl_degree.Items.Clear();

            userCode = Session["usercode"].ToString();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }

            ds.Clear();
            string query = "";
            if (userCode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(userCollegeCode) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + userCode + "";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(userCollegeCode) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + groupUserCode + "";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            cbl_degree.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = Lbldegree.Text + "(" + cbl_degree.Items.Count + ")";
                }
            }
            else
            {
                txt_degree.Text = "--Select--";
            }

            //has.Clear();
            //has.Add("single_user", singleUser);
            //has.Add("group_code", groupUserCode);
            //has.Add("college_code", userCollegeCode);
            //has.Add("user_code", userCode);
            //ds = da.select_method("bind_degree", has, "sp");
            //int count1 = ds.Tables[0].Rows.Count;
            //if (count1 > 0)
            //{
            //    ddldegree.DataSource = ds;
            //    ddldegree.DataTextField = "course_name";
            //    ddldegree.DataValueField = "course_id";
            //    ddldegree.DataBind();

            //}
        }
        catch (Exception ex) { }

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            bindsem();
            bindsec();

            Fpspreadpop.Visible = false;
            rptprint.Visible = false;
            btndelete.Visible = false;
        }
        catch (Exception ex) { }

    }
    #endregion


    #region Branch
    public void bindbranch()
    {
        try
        {

            //ddlsem.Items.Clear();
            has.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }

            ds.Clear();

            string query1 = "";
            string buildvalue1 = "";
            if (cbl_degree.Items.Count > 0)
            {
                buildvalue1 = returnwithsinglecodevalue(cbl_degree);
                if (userCode != "")
                {
                    query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddlCollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + userCode + "'";
                }
                else
                {
                    query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddlCollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + groupUserCode + "'";

                }
                ds = d2.select_method_wo_parameter(query1, "Text");
                cbl_branch.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txtbranch.Text = LblBranch.Text + "(" + cbl_branch.Items.Count + ")";
                    }
                }
                else
                {
                    txtbranch.Text = "--Select--";
                }
            }
            //has.Add("single_user", singleUser);
            //has.Add("group_code", groupUserCode);
            ////has.Add("course_id", ddldegree.SelectedValue);
            //has.Add("college_code", userCollegeCode);
            //has.Add("user_code", userCode);
            //ds = da.select_method("bind_branch", has, "sp");
            //int count2 = ds.Tables[0].Rows.Count;
            //if (count2 > 0)
            //{
            //    ddlbranch.DataSource = ds;
            //    ddlbranch.DataTextField = "dept_name";
            //    ddlbranch.DataValueField = "degree_code";
            //    ddlbranch.DataBind();

            //}
        }
        catch (Exception ex) { }

    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsec();
            Fpspreadpop.Visible = false;
            rptprint.Visible = false;
            btndelete.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }
    #endregion

    #region Sem
    public void bindsem()
    {
        try
        {
            ds.Clear();
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string duration = string.Empty;
            Boolean first_year = false;
            userCollegeCode = ddlCollege.SelectedItem.Value;
            if (cbl_batchyear.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cbl_batchyear);
            if (cbl_branch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cbl_branch);
            string SelSem = string.Empty;
            if (!string.IsNullOrEmpty(userCollegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                SelSem = "select distinct current_semester from Registration where  isnull(cc,0)=0 and isnull(delflag,0)=0  order by Current_Semester";// Batch_Year in('" + valBatch + "') and and degree_code in('" + valDegree + "')
                ds = da.select_method_wo_parameter(SelSem, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sem.DataSource = ds;
                    cbl_sem.DataTextField = "current_semester";
                    cbl_sem.DataValueField = "current_semester";
                    cbl_sem.DataBind();
                    if (cbl_sem.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sem.Items.Count; i++)
                        {
                            cbl_sem.Items[i].Selected = true;
                        }
                        txtsem.Text = LblSem.Text + "(" + cbl_sem.Items.Count + ")";
                    }


                }
                else
                {
                    txtsem.Text = "--Select--";

                }
            }

            //    ds = da.select_method("bind_sem", has, "sp");
            //    int count3 = ds.Tables[0].Rows.Count;
            //    if (count3 > 0)
            //    {
            //        ddlsem.Enabled = true;
            //        duration = ds.Tables[0].Rows[0][0].ToString();
            //        first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            //        for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
            //        {
            //            if (first_year == false)
            //            {
            //                ddlsem.Items.Add(loop_val.ToString());
            //            }
            //            else if (first_year == true && loop_val != 2)
            //            {
            //                ddlsem.Items.Add(loop_val.ToString());
            //            }
            //        }
            //    }
            //    else
            //    {
            //        count3 = ds.Tables[1].Rows.Count;
            //        if (count3 > 0)
            //        {
            //            ddlsem.Enabled = true;
            //            duration = ds.Tables[1].Rows[0][0].ToString();
            //            first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
            //            for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
            //            {
            //                if (first_year == false)
            //                {
            //                    ddlsem.Items.Add(loop_val.ToString());
            //                }
            //                else if (first_year == true && loop_val != 2)
            //                {
            //                    ddlsem.Items.Add(loop_val.ToString());
            //                }
            //            }
            //        }
            //        else
            //        {
            //            ddlsem.Enabled = false;
            //        }

            //    }
        }
        catch (Exception ex) { }

    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            Fpspreadpop.Visible = false;
            rptprint.Visible = false;
            btndelete.Visible = false;
        }
        catch (Exception ex) { }

    }
    #endregion

    #region Sec
    public void bindsec()
    {
        try
        {
            ds.Clear();
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            cblsec.Items.Clear();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            if (cbl_batchyear.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cbl_batchyear);
            if (cbl_branch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cbl_branch);

            string qry = "select distinct sections from registration where batch_year in('" + valBatch + "') and degree_code in('" + valDegree + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections ";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblsec.DataSource = ds;
                cblsec.DataTextField = "section";
                cblsec.DataTextField = "sections";
                cblsec.DataBind();
                if (cblsec.Items.Count > 0)
                {
                    for (int i = 0; i < cblsec.Items.Count; i++)
                    {
                        cblsec.Items[i].Selected = true;
                    }
                    txtsec.Text = lblSec.Text + "(" + cblsec.Items.Count + ")";
                }

            }
            else
            {
                txtsec.Text = "--Select--";

            }
            //ddlSec.Items.Clear();
            //  hat.Clear();
            // // hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
            ////  hat.Add("degree_code", ddlbranch.SelectedValue);
            //  ds = da.select_method("bind_sec", hat, "sp");
            //  int count5 = ds.Tables[0].Rows.Count;
            //  if (count5 > 0)
            //  {
            //      ddlSec.DataSource = ds;
            //      ddlSec.DataTextField = "sections";
            //      ddlSec.DataValueField = "sections";
            //      ddlSec.DataBind();
            //      ddlSec.Enabled = true;
            //  }
            //  else
            //  {
            //      ddlSec.Enabled = false;
            //  }
            //  ddlSec.Items.Add("All");
        }
        catch (Exception ex) { }

    }
    #endregion

    #region Go
    protected void btngo_click(object sender, EventArgs e)
    {

        try
        {
            string selq = "";
            string valDegree = string.Empty;
            string valBatch = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_batchyear.Items.Count > 0)
                batch = rs.GetSelectedItemsValueAsString(cbl_batchyear);
            if (cbl_degree.Items.Count > 0)
                courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
            if (cbl_branch.Items.Count > 0)
                bran = rs.GetSelectedItemsValueAsString(cbl_branch);
            if (cbl_sem.Items.Count > 0)
                sem = rs.GetSelectedItemsValueAsString(cbl_sem);
            if (cblsec.Items.Count > 0)
                sec = rs.GetSelectedItemsValueAsString(cblsec);
            // if (dlbatch.Items.Count > 0)
            //     //batch = Convert.ToString(ddlbatch.SelectedValue);
            // if (dldegree.Items.Count > 0)
            //     //courseid = Convert.ToString(ddldegree.SelectedValue);
            // if (dlbranch.Items.Count > 0)
            ////     bran = Convert.ToString(ddlbranch.SelectedValue);
            // if (dlsem.Items.Count > 0)
            //     //sem = Convert.ToString(ddlsem.SelectedValue);
            // if (dlsec.Items.Count > 0)
            //     //sec = Convert.ToString(ddlSec.SelectedValue).Trim();

            if (sec == "" || sec == "All")
                Section = "";
            else
                Section = "and R.sections in('" + sec + "')";

            if (collcode != "")
            {
                selq = "SELECT distinct R.Roll_No,R.Stud_Name,C.Course_Name + '-' + D.Dept_Name as Degree, R.Current_Semester,R.finger_id FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'  and R.batch_year in('" + batch + "') and G.Degree_Code in('" + bran + "') AND C.Course_Id in('" + courseid + "')  and C.college_code='" + collcode + "' and R.Current_Semester in('" + sem + "') " + Section + " ";
                if (rbfingerid.Checked == true)
                    selq = selq + " and ((R.finger_id is not null) and (cast(R.finger_id as varchar)<>''))";
                else
                    selq = selq + " and ((R.finger_id is null) or (cast(R.finger_id as varchar)=''))";
                selq = selq + "order by Roll_No";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadrepsprcolumns();
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = false;
                FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                cball.AutoPostBack = true;


                Fpspreadpop.Sheets[0].RowCount++;
                Fpspreadpop.Sheets[0].Cells[0, 1].CellType = cball;
                Fpspreadpop.Sheets[0].Cells[0, 1].Value = 0;
                Fpspreadpop.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadpop.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";


                for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                {
                    Fpspreadpop.Sheets[0].RowCount++;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ik + 1);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].CellType = cb;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[ik]["Roll_No"]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].CellType = txtcell;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[ik]["Stud_Name"]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[ik]["finger_id"]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;


                }

                Fpspreadpop.Sheets[0].PageSize = Fpspreadpop.Sheets[0].RowCount;
                Fpspreadpop.Visible = true;
                rptprint.Visible = true;
                btndelete.Visible = true;
            }
            else
            {
                Fpspreadpop.Visible = false;
                rptprint.Visible = false;
                btndelete.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";

            }
        }
        catch { }
    }


    private void loadrepsprcolumns()
    {
        try
        {
            Fpspreadpop.Sheets[0].RowCount = 0;
            Fpspreadpop.Sheets[0].ColumnCount = 5;
            Fpspreadpop.CommandBar.Visible = false;
            Fpspreadpop.RowHeader.Visible = false;
            Fpspreadpop.Sheets[0].AutoPostBack = false;
            Fpspreadpop.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspreadpop.Sheets[0].FrozenRowCount = 1;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpspreadpop.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[0].Locked = true;
            Fpspreadpop.Columns[0].Width = 50;




            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[1].Width = 75;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[2].Width = 165;
            Fpspreadpop.Columns[2].Locked = true;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[3].Locked = true;
            Fpspreadpop.Columns[3].Width = 220;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Finger ID";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[4].Locked = true;
            Fpspreadpop.Columns[4].Width = 120;

            if (rbnofingerid.Checked == true)
            {

                Fpspreadpop.Columns[4].Visible = false;

            }
            else
            {

                Fpspreadpop.Columns[4].Visible = true;

            }
        }
        catch { }
    }

    #endregion

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student_FingerPrint_Reg_Report";
            string pagename = "Student_FingerPrint_Reg.aspx";
            Printcontrol.loadspreaddetails(Fpspreadpop, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student_FingerPrint_Reg_Report"); }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspreadpop, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Student_FingerPrint_Reg_Report"); }

    }
    #endregion

    #region Add
    protected void btnAdd_click(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = true;
            stdbindbatch();
            stdbinddegree();
            stdbindbranch();
            stdbindsem();
            stdbindsec();
            studentlist();
            FpSpread.Visible = false;
            btnsave.Visible = false;
            txt_macid.Text = "";
            txt_macid_Change(sender, e);

        }
        catch
        {

        }

    }
    #endregion

    #region Delete
    protected void Fpspreadpop_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Fpspreadpop.SaveChanges();
        try
        {
            int ik = 0;
            byte check = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[0, 1].Value);
            if (check == 1)
            {
                for (ik = 1; ik < Fpspreadpop.Sheets[0].RowCount; ik++)
                {
                    Fpspreadpop.Sheets[0].Cells[ik, 1].Value = 1;
                }
            }
            else
            {
                for (ik = 1; ik < Fpspreadpop.Sheets[0].RowCount; ik++)
                {
                    Fpspreadpop.Sheets[0].Cells[ik, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    private bool checkedspr()
    {
        bool ok = false;
        Fpspreadpop.SaveChanges();
        try
        {
            for (int ik = 0; ik < Fpspreadpop.Sheets[0].RowCount; ik++)
            {
                byte check = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[ik, 1].Value);
                if (check == 1)
                    ok = true;
            }
        }
        catch { }
        return ok;
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkedspr())
            {
                //lblpoperr.Visible = false;
                Fpspreadpop.SaveChanges();
                string delq = "";
                int delcount = 0;
                for (int ik = 0; ik < Fpspreadpop.Sheets[0].RowCount; ik++)
                {
                    byte check = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[ik, 1].Value);
                    if (check == 1)
                    {
                        string rollno = Convert.ToString(Fpspreadpop.Sheets[0].Cells[ik, 2].Text);
                        delq = "update Registration set finger_id='' where Roll_No='" + rollno + "'";
                        int upcount = d2.update_method_wo_parameter(delq, "Text");
                        if (upcount > 0)
                            delcount++;
                    }
                }
                if (delcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully!";
                    btngo_click(sender, e);
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please Select Any Student!";
                //lblpoperr.Visible = true;
                //lblpoperr.Text = "Please Select Any Staff!";
            }
        }
        catch { }
    }
    #endregion

    #region FingerID_Match_Popup

    #region Batch
    public void stdbindbatch()
    {
        try
        {
            cbl_batchYear1.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                cbl_batchYear1.DataSource = ds;
                cbl_batchYear1.DataTextField = "batch_year";
                cbl_batchYear1.DataValueField = "batch_year";
                cbl_batchYear1.DataBind();
                if (cbl_batchYear1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batchYear1.Items.Count; i++)
                    {
                        cbl_batchYear1.Items[i].Selected = true;
                    }
                    txtbatchyear1.Text = lblbatchyear1.Text + "(" + cbl_batchYear1.Items.Count + ")";
                }


            }
            //dlbatch.Items.Clear();
            //ds = da.select_method_wo_parameter("bind_batch", "sp");
            //int count = ds.Tables[0].Rows.Count;
            //if (count > 0)
            //{
            //    dlbatch.DataSource = ds;
            //    dlbatch.DataTextField = "batch_year";
            //    dlbatch.DataValueField = "batch_year";
            //    dlbatch.DataBind();
            //}
            //int count1 = ds.Tables[1].Rows.Count;
            //if (count > 0)
            //{
            //    int max_bat = 0;
            //    max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            //    dlbatch.SelectedValue = max_bat.ToString();
            //}
        }
        catch (Exception ex) { }
    }
    protected void dlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            stdbindbranch();
            stdbinddegree();
            stdbindsem();
            stdbindsec();
            studentlist();

        }
        catch (Exception ex) { }

    }
    #endregion

    #region Degree
    public void stdbinddegree()
    {
        try
        {

            cbl_degree1.Items.Clear();

            userCode = Session["usercode"].ToString();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }

            ds.Clear();
            string query = "";
            if (userCode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(userCollegeCode) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + userCode + "";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(userCollegeCode) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + groupUserCode + "";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            cbl_degree1.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree1.DataSource = ds;
                cbl_degree1.DataTextField = "course_name";
                cbl_degree1.DataValueField = "course_id";
                cbl_degree1.DataBind();
                if (cbl_degree1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree1.Items.Count; i++)
                    {
                        cbl_degree1.Items[i].Selected = true;
                    }
                    txtdegree1.Text = lbldegree1.Text + "(" + cbl_degree1.Items.Count + ")";
                }
            }
            else
            {
                txt_degree.Text = "--Select--";
            }

            //dldegree.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //userCollegeCode = ddlcoll.SelectedItem.Value;
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            //if (groupUserCode.Contains(';'))
            //{
            //    string[] group_semi = groupUserCode.Split(';');
            //    groupUserCode = group_semi[0].ToString();
            //}
            //has.Clear();
            //has.Add("single_user", singleUser);
            //has.Add("group_code", groupUserCode);
            //has.Add("college_code", userCollegeCode);
            //has.Add("user_code", userCode);
            //ds = da.select_method("bind_degree", has, "sp");
            //int count1 = ds.Tables[0].Rows.Count;
            //if (count1 > 0)
            //{
            //    dldegree.DataSource = ds;
            //    dldegree.DataTextField = "course_name";
            //    dldegree.DataValueField = "course_id";
            //    dldegree.DataBind();
            //}
        }
        catch (Exception ex) { }

    }
    protected void dldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            stdbindbranch();
            stdbindsem();
            stdbindsec();
            studentlist();

        }
        catch (Exception ex) { }

    }
    #endregion


    #region Branch
    public void stdbindbranch()
    {
        try
        {
            cbl_branch1.Items.Clear();
            has.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }

            ds.Clear();

            string query1 = "";
            string buildvalue1 = "";
            if (cbl_degree1.Items.Count > 0)
            {
                buildvalue1 = returnwithsinglecodevalue(cbl_degree1);
                if (userCode != "")
                {
                    query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddlCollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + userCode + "'";
                }
                else
                {
                    query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddlCollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + groupUserCode + "'";

                }
                ds = d2.select_method_wo_parameter(query1, "Text");
                cbl_branch1.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();
                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txtbranch1.Text = lblbranch1.Text + "(" + cbl_branch1.Items.Count + ")";
                    }
                }
                else
                {
                    txtbranch1.Text = "--Select--";
                }
            }

            //dlsem.Items.Clear();
            //has.Clear();
            //userCode = Session["usercode"].ToString();
            //userCollegeCode = ddlcoll.SelectedItem.Value;
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            //if (groupUserCode.Contains(';'))
            //{
            //    string[] group_semi = groupUserCode.Split(';');
            //    groupUserCode = group_semi[0].ToString();
            //}
            //has.Add("single_user", singleUser);
            //has.Add("group_code", groupUserCode);
            ////has.Add("course_id", dldegree.SelectedValue);
            //has.Add("college_code", userCollegeCode);
            //has.Add("user_code", userCode);
            //ds = da.select_method("bind_branch", has, "sp");
            //int count2 = ds.Tables[0].Rows.Count;
            //if (count2 > 0)
            //{
            //    dlbranch.DataSource = ds;
            //    dlbranch.DataTextField = "dept_name";
            //    dlbranch.DataValueField = "degree_code";
            //    dlbranch.DataBind();
            //}
        }
        catch (Exception ex) { }

    }
    protected void dlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            stdbindsem();
            stdbindsec();
            studentlist();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }
    #endregion

    #region Sem
    public void stdbindsem()
    {
        try
        {

            cbl_sem1.Items.Clear();
            ds.Clear();
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string duration = string.Empty;
            Boolean first_year = false;
            userCollegeCode = ddlCollege.SelectedItem.Value;
            if (cbl_batchYear1.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cbl_batchYear1);
            if (cbl_branch1.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cbl_branch1);
            string SelSem = string.Empty;
            if (!string.IsNullOrEmpty(userCollegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                SelSem = "select distinct current_semester from Registration where  isnull(cc,0)=0 and isnull(delflag,0)=0  order by Current_Semester";// Batch_Year in('" + valBatch + "') and and degree_code in('" + valDegree + "')
                ds = da.select_method_wo_parameter(SelSem, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sem1.DataSource = ds;
                    cbl_sem1.DataTextField = "current_semester";
                    cbl_sem1.DataValueField = "current_semester";
                    cbl_sem1.DataBind();
                    if (cbl_sem1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sem1.Items.Count; i++)
                        {
                            cbl_sem1.Items[i].Selected = true;
                        }
                        txtsem1.Text = lbl_sem.Text + "(" + cbl_sem1.Items.Count + ")";
                    }


                }
                else
                {
                    txtsem1.Text = "--Select--";

                }
            }

            //string duration = string.Empty;
            //Boolean first_year = false;
            //has.Clear();
            //userCollegeCode = ddlcoll.SelectedItem.Value;
            ////has.Add("degree_code", dlbranch.SelectedValue.ToString());
            ////  has.Add("batch_year", dlbatch.SelectedValue.ToString());
            //has.Add("college_code", userCollegeCode);
            //ds = da.select_method("bind_sem", has, "sp");
            //int count3 = ds.Tables[0].Rows.Count;
            //if (count3 > 0)
            //{
            //    dlsem.Enabled = true;
            //    duration = ds.Tables[0].Rows[0][0].ToString();
            //    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            //    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
            //    {
            //        if (first_year == false)
            //        {
            //            dlsem.Items.Add(loop_val.ToString());
            //        }
            //        else if (first_year == true && loop_val != 2)
            //        {
            //            dlsem.Items.Add(loop_val.ToString());
            //        }
            //    }
            //}
            //else
            //{
            //    count3 = ds.Tables[1].Rows.Count;
            //    if (count3 > 0)
            //    {
            //        dlsem.Enabled = true;
            //        duration = ds.Tables[1].Rows[0][0].ToString();
            //        first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
            //        for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
            //        {
            //            if (first_year == false)
            //            {
            //                dlsem.Items.Add(loop_val.ToString());
            //            }
            //            else if (first_year == true && loop_val != 2)
            //            {
            //                dlsem.Items.Add(loop_val.ToString());
            //            }
            //        }
            //    }
            //    else
            //    {
            //        dlsem.Enabled = false;
            //    }
            //}
        }
        catch (Exception ex) { }

    }
    protected void dlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            stdbindsec();
            studentlist();
        }
        catch (Exception ex) { }

    }
    #endregion

    #region Sec
    public void stdbindsec()
    {
        try
        {
            ds.Clear();
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            cbl_sec1.Items.Clear();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            if (cbl_batchYear1.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cbl_batchYear1);
            if (cbl_branch1.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cbl_branch1);

            string qry = "select distinct sections from registration where batch_year in('" + valBatch + "') and degree_code in('" + valDegree + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections ";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sec1.DataSource = ds;
                cbl_sec1.DataTextField = "section";
                cbl_sec1.DataTextField = "sections";
                cbl_sec1.DataBind();
                if (cbl_sec1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sec1.Items.Count; i++)
                    {
                        cbl_sec1.Items[i].Selected = true;
                    }
                    txtsec1.Text = lblSec.Text + "(" + cbl_sec1.Items.Count + ")";
                }

            }
            else
            {
                txtsec1.Text = "--Select--";

            }
            // dlsec.Items.Clear();
            // hat.Clear();
            // //hat.Add("batch_year", dlbatch.SelectedValue.ToString());
            //// hat.Add("degree_code", dlbranch.SelectedValue);
            // ds = da.select_method("bind_sec", hat, "sp");
            // int count5 = ds.Tables[0].Rows.Count;
            // if (count5 > 0)
            // {
            //     dlsec.DataSource = ds;
            //     dlsec.DataTextField = "sections";
            //     dlsec.DataValueField = "sections";
            //     dlsec.DataBind();
            //     dlsec.Enabled = true;
            // }
            // else
            // {
            //     dlsec.Enabled = false;
            // }
            // dlsec.Items.Add("All");
        }
        catch (Exception ex) { }

    }


    protected void dlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        studentlist();

    }
    #endregion

    private void studentlist()
    {
        try
        {


            //ddlstdlst.Items.Clear();
            //cbl_staffname.Items.Clear();
            //if (ddlcoll.Items.Count > 0)
            //    collcode = Convert.ToString(ddlcoll.SelectedValue);
            //  if (dlbatch.Items.Count > 0)
            //   batch = Convert.ToString(dlbatch.SelectedValue);
            //  if (dldegree.Items.Count > 0)
            //    courseid = Convert.ToString(dldegree.SelectedValue);
            //if (dlbranch.Items.Count > 0)
            //    bran = Convert.ToString(dlbranch.SelectedValue);
            //if (dlsem.Items.Count > 0)
            //    sem = Convert.ToString(dlsem.SelectedValue);
            //if (dlsec.Items.Count > 0)
            //    sec = Convert.ToString(dlsec.SelectedValue).Trim();
            string selq = "";
            string valDegree = string.Empty;
            string valBatch = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_batchYear1.Items.Count > 0)
                batch = rs.GetSelectedItemsValueAsString(cbl_batchYear1);
            if (cbl_degree1.Items.Count > 0)
                courseid = rs.GetSelectedItemsValueAsString(cbl_degree1);
            if (cbl_branch1.Items.Count > 0)
                bran = rs.GetSelectedItemsValueAsString(cbl_branch1);
            if (cbl_sem1.Items.Count > 0)
                sem = rs.GetSelectedItemsValueAsString(cbl_sem1);
            if (cbl_sec1.Items.Count > 0)
                sec = rs.GetSelectedItemsValueAsString(cbl_sec1);

            if (sec == "" || sec == "All")
                Section = "";
            else
                Section = "and R.sections in('" + sec + "')";

            string sqlgetstddetails = "SELECT distinct R.Stud_Name+ '$' +R.Roll_No Stud_Name,R.Roll_No, C.Course_Name + '-' + D.Dept_Name as Degree, R.Current_Semester FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and R.batch_year in('" + batch + "') and G.Degree_Code in('" + bran + "') AND C.Course_Id in('" + courseid + "')  and C.college_code in('" + collcode + "') and R.Current_Semester in('" + sem + "') " + Section + " order by Stud_Name";

            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlgetstddetails, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstdlst.DataSource = ds;
                ddlstdlst.DataTextField = "Stud_Name";
                ddlstdlst.DataValueField = "Roll_No";
                ddlstdlst.DataBind();
                ddlstdlst.Items.Insert(0, "Select");
            }
            else
            {
                ddlstdlst.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    protected void ddlstdlst_change(object sender, EventArgs e)
    {
        txt_macid.Text = "";
        txt_macid_Change(sender, e);
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetMacID(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct MachineNo from DeviceInfo where College_Code='" + collegecode + "' and MachineNo like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    protected void txt_macid_Change(object sender, EventArgs e)
    {
        try
        {
            ddlfingerid.Items.Clear();
            int txtval = 0;
            Int32.TryParse(txt_macid.Text.Trim(), out txtval);
            if (txt_macid.Text.Trim() != "" || txtval != 0)
            {
                //Cmd By SaranyaDevi 16.4.2018

                //string selq = "select distinct cast(Enrollno as bigint) as Enrollno from bio..enrollments where Branchid='" + txt_macid.Text.Trim() + "' order by cast(Enrollno as bigint) asc";

                //Added By Saranyadevi 16.4.2018
                string selq = "select distinct cast(Enrollno as varchar) as Enrollno from bio..enrollments where Branchid='" + txt_macid.Text.Trim() + "' order by cast(Enrollno as varchar) asc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlfingerid.DataSource = ds;
                    ddlfingerid.DataTextField = "Enrollno";
                    ddlfingerid.DataValueField = "Enrollno";
                    ddlfingerid.DataBind();
                    ddlfingerid.Items.Insert(0, "Select");
                }
                else
                {
                    ddlfingerid.Items.Insert(0, "Select");
                }
            }
            else
            {
                ddlfingerid.Items.Insert(0, "Select");
            }
        }
        catch { }
    }


    protected bool checkstdcode()
    {
        bool chkspr = true;
        string staffcode = Convert.ToString(ddlstdlst.SelectedValue);
        string sprstaffcode = "";
        FpSpread.SaveChanges();
        try
        {
            for (int ik = 0; ik < FpSpread.Sheets[0].RowCount; ik++)
            {
                sprstaffcode = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 1].Text);
                if (staffcode == sprstaffcode)
                    chkspr = false;
            }
        }
        catch { }
        return chkspr;
    }

    protected bool checkvalue()
    {
        bool chkspr = true;
        string sprstaffcode = "";
        FpSpread.SaveChanges();
        try
        {
            for (int ik = 0; ik < FpSpread.Sheets[0].RowCount; ik++)
            {
                sprstaffcode = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 0].Text);
                if (sprstaffcode.Trim() == "")
                    chkspr = false;
            }
        }
        catch { }
        return chkspr;
    }

    private void loadsprcolumns()
    {
        try
        {
            FpSpread.Sheets[0].RowCount = 0;
            FpSpread.Sheets[0].ColumnCount = 5;
            FpSpread.CommandBar.Visible = false;
            FpSpread.RowHeader.Visible = false;
            FpSpread.Sheets[0].AutoPostBack = false;
            FpSpread.Sheets[0].ColumnHeader.RowCount = 1;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread.Columns[0].Locked = true;
            FpSpread.Columns[0].Width = 50;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread.Columns[1].Locked = true;
            FpSpread.Columns[1].Width = 150;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread.Columns[2].Locked = true;
            FpSpread.Columns[2].Width = 300;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Device ID";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread.Columns[3].Locked = true;
            FpSpread.Columns[3].Width = 150;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Finger ID";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread.Columns[4].Locked = true;
            FpSpread.Columns[4].Width = 150;
        }
        catch { }
    }

    protected void btnmatch_click(object sender, EventArgs e)
    {
        try
        {
            string getnamecode = Convert.ToString(ddlstdlst.SelectedItem.Text);
            string staffname = "";
            if (getnamecode.Trim() != "Select")
                staffname = getnamecode.Split('$')[0];
            if ((FpSpread.Sheets[0].RowCount == 3 && checkvalue() == false) || FpSpread.Sheets[0].RowCount == 0)
                loadsprcolumns();
            FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
            if (checkstdcode() == false)
            {
                lblerr.Visible = true;
                lblerr.Text = "Student Already Exists!";
                return;
            }
            else if (ddlstdlst.SelectedIndex == 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select Student!";
                return;
            }
            else if (txt_macid.Text.Trim() == "")
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Enter MachineID!";
                return;
            }
            else if (ddlfingerid.SelectedIndex == 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select FingerID!";
                return;
            }
            else
            {
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread.Sheets[0].RowCount);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].CellType = txtcell;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ddlstdlst.SelectedValue);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = txtcell;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(staffname);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].CellType = txtcell;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(txt_macid.Text);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].CellType = txtcell;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ddlfingerid.SelectedValue);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].CellType = txtcell;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.Visible = true;
                lblerr.Visible = false;
                btnsave.Visible = true;
            }
        }
        catch { }
    }


    protected void btnsave_click(object sender, EventArgs e)
    {
        try
        {
            if (checkvalue() == true && FpSpread.Sheets[0].RowCount > 0)
            {
                string rollno = "";
                string fingerid = "";
                string deviceid = "";
                string collcode = Convert.ToString(ddlcoll.SelectedItem.Value);
                string updq = "";
                int upcount = 0;
                for (int ik = 0; ik < FpSpread.Sheets[0].RowCount; ik++)
                {
                    rollno = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 1].Text);
                    fingerid = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 4].Text);
                    deviceid = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 3].Text);
                    updq = "update Registration set finger_id='" + fingerid + "',DeviceID='" + deviceid + "' where Roll_No='" + rollno + "'";
                    int inscount = d2.update_method_wo_parameter(updq, "Text");
                    if (inscount > 0)
                        upcount++;
                }
                if (upcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved Successfully!";
                }
            }
        }
        catch { }
    }

    protected void btnexit_click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    #endregion

    protected void imagebtnpopcloseadd_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void cb_batchyear_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batchyear, cbl_batchyear, txt_batchyr, Lblbatch.Text, "--Select--");
        bindbranch();
        binddegree();
        bindsem();
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;

    }
    protected void cbl_batchyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batchyear, cbl_batchyear, txt_batchyr, Lblbatch.Text);
        bindbranch();
        binddegree();
        bindsem();
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;

    }
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, Lbldegree.Text, "--Select--");
        bindbranch();
        bindsem();
        bindsec();

        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;

    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, Lbldegree.Text);
        bindbranch();
        bindsem();
        bindsec();

        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;

    }
    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch, cbl_branch, txtbranch, LblBranch.Text, "--Select--");
        bindsem();
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;


    }


    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch, cbl_branch, txtbranch, LblBranch.Text);
        bindsem();
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;

    }
    protected void cb_sem_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txtsem, LblSem.Text, "--Select--");
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;
    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txtsem, LblSem.Text);
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;
    }
    protected void cbsec_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cbsec, cblsec, txtsec, lblSec.Text, "--Select--");
    }
    protected void cblsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbsec, cblsec, txtsec, lblSec.Text);

    }
    protected void cb_batchyear1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batchyear1, cbl_batchYear1, txtbatchyear1, lblbatchyear1.Text, "--Select--");
        stdbindbranch();
        stdbinddegree();
        stdbindsem();
        stdbindsec();
        studentlist();
    }
    protected void cbl_batchYear1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batchyear1, cbl_batchYear1, txtbatchyear1, lblbatchyear1.Text);
        stdbindbranch();
        stdbinddegree();
        stdbindsem();
        stdbindsec();
        studentlist();
    }
    protected void cb_degree1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree1, cbl_degree1, txtdegree1, lbldegree1.Text, "--Select--");
        stdbindbranch();
        stdbindsem();
        stdbindsec();
        studentlist();


    }

    protected void cbl_degree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree1, cbl_degree1, txtdegree1, lbldegree1.Text);
        stdbindbranch();
        stdbindsem();
        stdbindsec();
        studentlist();


    }
    protected void cb_branch1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch1, cbl_branch1, txtbranch1, lblbranch1.Text, "--Select--");
        stdbindsem();
        stdbindsec();
        studentlist();
    }
    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch1, cbl_branch1, txtbranch1, lblbranch1.Text);
        stdbindsem();
        stdbindsec();
        studentlist();
    }
    protected void cb_sem1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem1, cbl_sem1, txtsem1, lbl_sem.Text, "--Select--");
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;

    }
    protected void cbl_sem1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem1, cbl_sem1, txtsem1, lbl_sem.Text);
        bindsec();
        Fpspreadpop.Visible = false;
        rptprint.Visible = false;
        btndelete.Visible = false;

    }

    protected void cb_sec1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sec1, cbl_sec1, txtsec1, lblsec1.Text, "--Select--");
        studentlist();

    }
    protected void cbl_sec1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sec1, cbl_sec1, txtsec1, lblsec1.Text);
        studentlist();
    }
    protected void cb_staffname_checkedchange(object sender, EventArgs e)
    {
        //CallCheckboxChange(cb_staffname, cbl_staffname, txtstaffname, lblstudeName.Text, "--Select--");
    }
    protected void cbl_staffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        // CallCheckboxListChange(cb_staffname, cbl_staffname, txtstaffname, lblstudeName.Text);

    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }
    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
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


}