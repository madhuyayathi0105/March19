using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using Gios.Pdf;
using System.IO;
using System.Configuration;
public partial class Hosteladmissionprocess : System.Web.UI.Page
{
    ReuasableMethods rs = new ReuasableMethods();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string college_code = string.Empty;
    string FinYearFK = string.Empty;
    string q1 = "";
    static string eqltohsc = "";
    static string colval = "";
    static string headertext = "";
    static string sqlheadertext = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        if (Session["usercode"] != null)
        {
            usercode = Session["usercode"].ToString();
        }
        if (!IsPostBack)
        {
            bindCollege();
            bindbatch();
            bindedu();
            binddegree();
            bindbranch();
            bindcommunity();
            bindreligion();
            rdbtype.Items[0].Selected = true;
            bindhostel();
            bindbuilding();
            bindfloor();
            bindroom();
            bindroomtype();
            columnordertype();
            bindheader();
            bindledgershortlist();
        }
        if (FinYearFK.Trim() == "")
            FinYearFK = d2.getCurrentFinanceYear(Session["usercode"].ToString(), Convert.ToString(ddl_collegename.SelectedValue));
    }
    void bindCollege()
    {
        try
        {
            ds.Clear();
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
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
            Hashtable hat = new Hashtable();
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddl_collegename.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        {
        }
    }
    public void ddl_collegename_SelectedIndexchange(object sender, EventArgs e)
    {
        if (ddl_collegename.Items.Count > 0)
        {
            college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
        }
        bindedu();
        binddegree();
        bindbranch();
        bindcommunity();
        bindreligion();
        columnordertype();
    }
    public void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void ddledu_SelectedIndexchange(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
    }
    public void rdbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        btn_go_Click(sender, e);
    }
    public void bindbatch()
    {
        ddl_batch.Items.Clear();
        q1 = " SELECT distinct batch_year FROM tbl_attendance_rights where user_id='" + usercode + "' ORDER BY batch_year DESC";
        ds = d2.select_method_wo_parameter(q1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_batch.DataSource = ds;
            ddl_batch.DataTextField = "batch_year";
            ddl_batch.DataValueField = "batch_year";
            ddl_batch.DataBind();
        }
        else
        {
            ddl_batch.Items.Insert(0, "--Select--");
        }
    }
    public void bindedu()
    {
        try
        {
            ddledu.Items.Clear();
            if (ddl_collegename.Items.Count > 0)
            {
                ds = d2.select_method_wo_parameter("select distinct Edu_Level from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + ddl_collegename.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' order by Edu_Level desc", "Text");
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddledu.DataSource = ds;
                    ddledu.DataTextField = "Edu_Level";
                    ddledu.DataValueField = "Edu_Level";
                    ddledu.DataBind();
                }
                else
                {
                    ddledu.Items.Insert(0, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void binddegree()
    {
        try
        {
            string query = "";
            string edulvl = "";
            cbl_degree.Items.Clear();
            txt_degree.Text = "--Select--";
            if (ddl_collegename.Items.Count > 0)
            {
                if (ddledu.SelectedItem.Text == "--Select--")
                {
                    query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddl_collegename.SelectedItem.Value + "'";
                }
                else
                {
                    edulvl = Convert.ToString(ddledu.SelectedItem.Value);
                    query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddl_collegename.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "')";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = lbl_degree.Text + "(" + (cbl_degree.Items.Count) + ")";
                        cb_degree.Checked = true;
                    }
                }
                else
                {
                    txt_degree.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch()
    {
        try
        {
            cbl_dept.Items.Clear(); txt_dept.Text = lbl_department.Text;
            string deg = rs.GetSelectedItemsValueAsString(cbl_degree);
            if (ddl_collegename.Items.Count > 0)
            {
                if (deg != "--Select--" && deg != null && deg != "")
                {
                    ds = d2.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + deg + "') and degree.college_code='" + ddl_collegename.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "'", "Text");
                    //}
                    //else
                    //{
                    //    ds = d2.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddl_collegename.SelectedItem.Value + "' and user_code='" + usercode + "'", "Text");
                }
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    for (int i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                        txt_dept.Text = lbl_department.Text + "(" + (cbl_dept.Items.Count) + ")";
                        cb_dept.Checked = true;
                    }
                }
                else
                {
                    cbl_dept.Items.Insert(0, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindreligion()
    {
        try
        {
            string religion = "";
            cbl_religion.Items.Clear();
            string reliquery = "select distinct Textval,textcode from Textvaltable t,applyn a where a.religion=t.TextCode and TextCriteria ='relig' and batch_year='" + Convert.ToString(ddl_batch.SelectedItem.Value) + "' and a.college_code ='" + ddl_collegename.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(reliquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_religion.DataSource = ds;
                    cbl_religion.DataTextField = "TextVal";
                    cbl_religion.DataValueField = "textcode";
                    cbl_religion.DataBind();
                    if (cbl_religion.Items.Count > 0)
                    {
                        //for (int i = 0; i < cbl_religion.Items.Count; i++)
                        //{
                        //    cbl_religion.Items[i].Selected = true;
                        //    religion = Convert.ToString(cbl_religion.Items[i].Text);
                        //}
                        //if (cbl_religion.Items.Count == 1)
                        //{
                        //    txt_religion.Text = "" + religion + "";
                        //}
                        //else
                        //{
                        //    txt_religion.Text = "Religion(" + cbl_religion.Items.Count + ")";
                        //}
                        //cb_religion.Checked = true;
                    }
                }
            }
            else
            {
                txt_religion.Text = "--Select--";
                cb_religion.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void bindcommunity()
    {
        try
        {
            string comm = "";
            string selq = "SELECT Distinct community,T.TextVal  FROM applyn A,TextValTable T WHERE  T.TextCode =A.community  AND TextVal<>'' AND a.college_code ='" + ddl_collegename.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_comm.DataSource = ds;
                    cbl_comm.DataTextField = "TextVal";
                    cbl_comm.DataValueField = "community";
                    cbl_comm.DataBind();
                    //if (cbl_comm.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < cbl_comm.Items.Count; i++)
                    //    {
                    //        cbl_comm.Items[i].Selected = true;
                    //        comm = Convert.ToString(cbl_comm.Items[i].Text);
                    //    }
                    //    if (cbl_comm.Items.Count == 1)
                    //    {
                    //        txt_comm.Text = "" + comm + "";
                    //    }
                    //    else
                    //    {
                    //        txt_comm.Text = "Community(" + cbl_comm.Items.Count + ")";
                    //    }
                    //    cb_comm.Checked = true;
                    //}
                }
            }
            else
            {
                txt_comm.Text = "--Select--";
                cb_comm.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void cb_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();
    }
    protected void cb_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_dept, cb_dept, txt_dept, lbl_department.Text);
    }
    protected void cbl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_dept, cb_dept, txt_dept, lbl_department.Text);
    }
    protected void cb_religion_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_religion, cb_religion, txt_religion, "Religion");
    }
    protected void cbl_religion_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_religion, cb_religion, txt_religion, "Religion");
    }
    protected void cb_comm_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_comm, cb_comm, txt_comm, "Community");
    }
    protected void cbl_comm_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_comm, cb_comm, txt_comm, "Community");
    }
    public void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_dept);
            string religioncode = rs.GetSelectedItemsValueAsString(cbl_religion);
            string communitycode = rs.GetSelectedItemsValueAsString(cbl_comm);
            btn_movetoshort.Visible = false;
            btn_movetoadmit.Visible = false;
            if (ddl_collegename.Items.Count > 0 && ddl_batch.Items.Count > 0)
            {
                applied();
                //string typefor = d2.GetFunction(" select setcolumn from admitcolumnset where textcriteria='Section process Format' and user_code='" + usercode + "' and college_code='" + Convert.ToString(ddl_collegename.SelectedValue) + "'");
                //if (typefor == "0")
                //{
                //    applied();
                //}
                //if (typefor == "1")
                //{
                //    applied2();
                //}
            }
        }
        catch (Exception Ex)
        { }
    }
    public void applied()
    {
        try
        {
            string batchyear = Convert.ToString(Convert.ToString(ddl_batch.SelectedItem.Value));
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_dept);
            string religioncode = rs.GetSelectedItemsValueAsString(cbl_religion);
            string communitycode = rs.GetSelectedItemsValueAsString(cbl_comm);
            string orderval = "";
            lbl_toapplied.Text = "";
            string percentagevalue = "";
            if (degreecode.Trim() != "" && batchyear.Trim() != "" && ddl_colord.Items.Count > 0)
            {
                q1 = "select value from Master_Settings where settings='OrderBy Marks Setting' and usercode='" + usercode + "'";
                q1 = q1 + "  select value from Master_Settings where settings='orderbymarks'";
                //q1 = q1 + " select column_name from admitcolumnset where textcriteria='percent'";//26
                q1 = q1 + "  select linkvalue from New_InsSettings n,CO_MasterValues c where n.linkname=c.mastervalue and c.MasterCriteria ='Hosteladmissioncolumnsettings' and n.user_code='" + usercode + "' and c.collegecode=n.college_code and c.collegecode ='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    orderval = Convert.ToString(ds.Tables[0].Rows[0]["value"]);
                }
                if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
                {
                    string columnvalue = Convert.ToString(ds.Tables[2].Rows[0][0]);
                    if (columnvalue.Contains("26"))
                    {
                        percentagevalue = "percentage";
                    }
                    else
                    {
                        percentagevalue = "percentage";
                    }
                }
                string marksaddquery = "";
                EquivalentToHSC();
                if (ddledu.SelectedItem.Text == "UG")
                {
                    marksaddquery = ",((ISNULL((securedmark / NULLIF( totalmark, 0 )),0))*1200) as securedmark";
                    marksaddquery = eqltohsc;
                }
                else
                {
                    marksaddquery = "," + percentagevalue + "";
                }
                FpSpread3.Sheets[0].ColumnCount = 0;
                FpSpread3.Sheets[0].RowCount = 0;
                FpSpread3.SaveChanges();
                int count = 0;
                int i = 0;
                int cc = 0;
                string addcomreli = "";
                if (religioncode != "")
                {
                    addcomreli = " and religion in('" + religioncode + "')";
                }
                if (communitycode != "")
                {
                    addcomreli = addcomreli + " and community in('" + communitycode + "')";
                }
                //FpSpread3.Sheets[0].PageSize = 5;
                //FpSpread3.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                //FpSpread3.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
                //FpSpread3.Pager.Align = HorizontalAlign.Right;
                //FpSpread3.Pager.Font.Bold = true;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.White;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                //FpSpread3.Pager.PageCount = 5;
                FpSpread3.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = false;
                //FpSpread3.ActiveSheetView.DefaultRowHeight = 25;
                FpSpread3.ActiveSheetView.Rows.Default.Font.Name = "Book Antiqua";
                FpSpread3.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
                FpSpread3.ActiveSheetView.Rows.Default.Font.Bold = false;
                FpSpread3.ActiveSheetView.Columns.Default.Font.Bold = false;
                FpSpread3.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread3.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
                FpSpread3.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpSpread3.ShowHeaderSelection = false;
                FpSpread3.Sheets[0].ColumnCount = 3;
                FpSpread3.Sheets[0].RowCount = 0;
                string admittedtype = "";
                btn_movetoreject.Visible = false;
                //btnprintpdf.Visible = false;
                string admithostelq = "";
                if (rdbtype.Items[0].Selected == true)
                {
                    admittedtype = " and isnull(hostel_admission_status,0)=0";
                    admithostelq = " and r.App_No not in(select APP_No from HT_HostelRegistration where (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0)) ";
                }
                else if (rdbtype.Items[1].Selected == true)
                {
                    admittedtype = " and hostel_admission_status=1";
                    admithostelq = " and r.App_No not in(select APP_No from HT_HostelRegistration where (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0)) ";
                }
                else if (rdbtype.Items[2].Selected == true)
                {
                    admittedtype = " and hostel_admission_status=2";
                    admithostelq = "  and r.App_No in(select APP_No from HT_HostelRegistration where (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0)) ";
                }
                string query = "";
                query = "select distinct a.cityp,case when isnull(CampusReq,0)=0 then 'No'  when CampusReq=1 then 'Yes' end as CampusReq,a.stud_type,a.parent_addressP,CASE WHEN co_curricular=0 then 'No' when co_curricular=1 then 'Yes' end as co_curricular,CASE WHEN DistinctSport=0 then 'No'  else 'Yes'  end as DistinctSport,CASE WHEN first_graduate=0 then 'No' when first_graduate=1 then 'Yes' end as first_graduate,CASE WHEN isdisable=0 then 'No' when isdisable=1 then 'Yes' end as isdisable,CASE WHEN IsExService=0 then 'No' when IsExService=1 then 'Yes' end as IsExService,CASE WHEN TamilOrginFromAndaman=0 then 'No' when TamilOrginFromAndaman=1 then 'Yes' end as TamilOrginFromAndaman,p.tancet_mark,(Select TextVal FROM TextValTable T WHERE mother_tongue = T.TextCode) mother_tongue,(Select TextVal FROM TextValTable T WHERE parent_statep = T.TextCode) parent_statep,p.percentage,p.major_percent, p.majorallied_percent,(Select TextVal FROM TextValTable T WHERE parent_occu = T.TextCode) parent_occu,(Select TextVal FROM TextValTable T WHERE citizen = T.TextCode) citizen,(Select TextVal FROM TextValTable T WHERE caste = T.TextCode) caste,a.parent_name,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(VARCHAR(11),date_applied,103) as date_applied,a.StuPer_Id, a.remarks,CASE WHEN sex=1 then 'Female' when sex=0 then 'Male' end as sex ,(Select TextVal FROM TextValTable T WHERE community = T.TextCode) community,(Select TextVal FROM TextValTable T WHERE religion = T.TextCode) religion,securedmark, a.app_no,a.Student_Mobile, a.Alternativedegree_code,a.stud_name,a.app_formno, r.degree_code,r.Batch_Year,r.Current_Semester, C.Course_Name,c.Course_Id ,Dt.Dept_Name,p.totalmark,religion,community,ISNULL (tt.priority2,0) as priority2 ,isnull(ts.priority1,0) as priority1,noofattempts,p.course_entno,P.instaddress, a.App_No, uni_state,type_semester,university_code,ISNULL(pt.TExtVal,'') Part2Language,ISNULL(Cc.TExtVal,'') Part1Language,ISNULL(Cc.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear,a.ApplBankRefNumber,CONVERT(varchar(10), ApplBankRefDate,103) as ApplBankRefDate,case when  isnull(p.vocational_stream,'0')='0' then 'No' when isnull(p.vocational_stream,'0')='1' then 'Yes' end as vocational_stream,(select TextVal  from TextValTable  where TextCode =p.course_code) as  course_code from degree d,Department dt,Course C,registration r  ,applyn A left join Stud_prev_details P ON P.app_no = A.app_no left join perv_marks_history ph on ph.course_entno =p.course_entno LEFT JOIN TextValTable Cc ON Cc.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language left join TextValTable tt on tt.TextCode =a.religion left join TextValTable ts on ts.TextCode =a.community  Where  r.app_no=a.app_no " + admithostelq + " and r.app_no=p.app_no and r.degree_code=d.degree_code and  p.app_no = a.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and ISNULL(markPriority,1)=1  and a.Batch_Year in('" + batchyear + "')  and a.college_code='" + ddl_collegename.SelectedItem.Value + "'  and a.college_code=d.college_code and CampusReq=1 " + admittedtype + " ";
                query += " " + "and r.degree_code in('" + degreecode + "')" + " " + addcomreli + "";
                if (ddl_orderby.SelectedItem.Text.ToLower() == "communitity")
                    query += Convert.ToString(ddl_orderby.SelectedItem.Value);
                else if (ddl_orderby.SelectedItem.Text.ToLower() == "mark")
                    query += Convert.ToString(ddl_orderby.SelectedItem.Value);
                else if (ddl_orderby.SelectedItem.Text.ToLower() == "attempts")
                    query += Convert.ToString(ddl_orderby.SelectedItem.Value);
                else if (ddl_orderby.SelectedItem.Text.ToLower() == "religion")
                    query += Convert.ToString(ddl_orderby.SelectedItem.Value);
                else if (ddl_orderby.SelectedItem.Text.ToLower() == "state")
                    query += Convert.ToString(ddl_orderby.SelectedItem.Value);
                //query = query + " select * from admitcolumnset  where user_code='" + usercode + "' and  textcriteria='column' union select '" + usercode + "' user_code,'Hostel Request' setcolumn ,'CampusReq' column_name ,46 priority,'" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' college_code ,'column' textcriteria ,null allot ,null allot_Confirm  union select '" + usercode + "' user_code,'City' setcolumn ,'cityp' column_name ,47 priority,'" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' college_code ,'column' textcriteria ,null allot ,null allot_Confirm order by Convert(int,priority) asc";
                query = query + "  select linkvalue from New_InsSettings n,CO_MasterValues c where n.linkname=c.mastervalue and c.MasterCriteria ='Hosteladmissioncolumnsettings' and n.user_code='" + usercode + "' and c.collegecode=n.college_code and c.collegecode ='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' and c.mastercode='" + ddl_colord.SelectedItem.Value + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Dictionary<int, double> dicsubcol = new Dictionary<int, double>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string Getmark = "";
                            string Totalmark = "";
                            string percentage = "";
                            if (ddledu.SelectedItem.Text == "UG")
                            {
                                Getmark = Convert.ToString(ds.Tables[0].Rows[i]["securedmark"]);
                                Totalmark = Convert.ToString(ds.Tables[0].Rows[i]["totalmark"]);
                            }
                            percentage = Convert.ToString(ds.Tables[0].Rows[i]["percentage"]);// + percentagevalue + ""]);
                            double totlmark = 0;
                            DataView dv = new DataView();
                            if (ddledu.SelectedItem.Text == "UG")
                            {
                                if (Getmark.Trim() == "")
                                {
                                    Getmark = "0";
                                }
                                totlmark = Convert.ToDouble(Getmark);
                                if (!dicsubcol.ContainsKey(Convert.ToInt32(ds.Tables[0].Rows[i]["app_no"])))
                                {
                                    dicsubcol.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["app_no"]), Convert.ToDouble(Math.Round(totlmark)));
                                }
                            }
                            else
                            {
                                if (percentage.Trim() == "")
                                {
                                    percentage = "0";
                                }
                                totlmark = Convert.ToDouble(percentage);
                                if (!dicsubcol.ContainsKey(Convert.ToInt32(ds.Tables[0].Rows[i]["app_no"])))
                                {
                                    dicsubcol.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["app_no"]), Convert.ToDouble(Math.Round(totlmark, 2)));
                                }
                            }
                        }
                    }
                    if (dicsubcol.Count > 0)
                    {
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "View";
                        cc = 2;
                        if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            string headernamevalue = Convert.ToString(ds.Tables[1].Rows[0][0]);
                            string[] headername = headernamevalue.Split(',');
                            for (int u = 0; u < headername.Length; u++)
                            {
                                colval = Convert.ToString(headername[u]);
                                loadtext();
                                string percentage = headertext;
                                //string percentage = Convert.ToString(ds.Tables[1].Rows[u]["setcolumn"]);
                                if (percentage.Trim() == "Hostel Name" || percentage.Trim() == "Room Type" || percentage.Trim() == "Boarding")
                                {
                                }
                                else
                                {
                                    cc++;
                                    FpSpread3.Sheets[0].ColumnCount = cc + 1;
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].Text = percentage;// Convert.ToString(ds.Tables[1].Rows[u]["setcolumn"]);
                                    if (ddledu.SelectedItem.Text == "PG")
                                    {
                                        if (percentage == "Marks")
                                        {
                                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].Text = "Percentage";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            btn_movetoshort.Visible = false;
                            btn_movetoadmit.Visible = false;
                            FpSpread3.Visible = false;
                            lblalerterr.Text = "Please Set The Column Order Setting";
                            alertpopwindow.Visible = true;
                            return;
                        }
                        //Hostel admission form fee 30.03.17
                        string settings = "";
                        if (rdbtype.Items[1].Selected == true)
                        {
                            settings = admissionformfeesetting();
                            if (settings == "1")
                            {
                                cc++;
                                FpSpread3.Sheets[0].ColumnCount = cc + 1;
                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].Text = "Allot Amount";//Hostel Admission Form Fee
                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].Font.Bold = true;
                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Columns[cc].Locked = true;
                                cc++;
                                FpSpread3.Sheets[0].ColumnCount = cc + 1;
                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].Text = "Paid Amount";//Hostel Admission Form Fee 
                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].Font.Bold = true;
                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Columns[cc].Locked = true;
                            }
                        }
                        FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                        cball.AutoPostBack = true;
                        FarPoint.Web.Spread.TextCellType txtCt = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb.AutoPostBack = false;
                        FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType("MyCommand", FarPoint.Web.Spread.ButtonType.ImageButton, "../dashbd/view.png");
                        DataView dv = new DataView();
                        FpSpread3.Sheets[0].RowCount++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = cball;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].Locked = true;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Locked = false;
                        FpSpread3.Sheets[0].SpanModel.Add(FpSpread3.Sheets[0].RowCount - 1, 2, 1, ds.Tables[1].Rows.Count + 2 - 1);

                        q1 = " select FeeAmount,PaidAmount,app_no,FeeCategory,HeaderFK,LedgerFK from FT_FeeAllot where FinYearFK='" + FinYearFK + "'";
                        DataSet studentbalancefee = new DataSet();
                        studentbalancefee = d2.select_method_wo_parameter(q1, "text");
                        foreach (var kvp in dicsubcol)
                        {
                            string app_no = kvp.Key.ToString();
                            string percentage = kvp.Value.ToString();
                            FpSpread3.Sheets[0].RowCount++;
                            count++;
                            ds.Tables[0].DefaultView.RowFilter = "app_no='" + app_no + "'";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["app_no"]);
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dv[0]["stud_name"]);
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Locked = true;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = cb;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].CellType = btn;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Column.Width = 50;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                cc = 2;
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    string headernamevalue = Convert.ToString(ds.Tables[1].Rows[0][0]);
                                    string[] headername = headernamevalue.Split(',');
                                    for (int u = 0; u < headername.Length; u++)
                                    {
                                        colval = Convert.ToString(headername[u]);
                                        loadtext();
                                        string columname = sqlheadertext;
                                        //string columname = Convert.ToString(ds.Tables[1].Rows[u]["column_name"]);
                                        if (columname == "HostelRegistrationPK" || columname == "RoomFK" || columname == "Boarding")
                                        {
                                        }
                                        else
                                        {
                                            cc++;
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].CellType = txtCt;
                                            if (columname == "DistinctSport")
                                            {
                                                string value = Convert.ToString(dv[0][columname]);
                                                if (value == "Yes")
                                                {
                                                    string val = d2.GetFunction("select textval from applyn a,textvaltable t where app_no='" + app_no + "' and textcode=DistinctSport");
                                                    if (val != "0")
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = value + "-" + val;
                                                    }
                                                    else
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = value;
                                                    }
                                                }
                                                else
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = value;
                                                }
                                            }
                                            else if (columname == "totalfees")
                                            {
                                                FpSpread3.Sheets[0].Columns[cc].Visible = false;
                                            }
                                            else if (columname == "remarks")
                                            {
                                                string VocationStream = Convert.ToString(dv[0]["vocational_stream"]);
                                                string Nationality = Convert.ToString(dv[0]["citizen"]);
                                                string CourseCode = Convert.ToString(dv[0]["course_code"]);
                                                string Concatvalue = "";
                                                if (VocationStream.Trim() != "No")
                                                {
                                                    Concatvalue = "Vocational";
                                                }
                                                if (Nationality.Trim().ToUpper() != "INDIAN")
                                                {
                                                    if (Concatvalue.Trim() == "")
                                                    {
                                                        Concatvalue = Nationality;
                                                    }
                                                    else
                                                    {
                                                        Concatvalue = Concatvalue + " - " + Nationality;
                                                    }
                                                }
                                                if (CourseCode.Trim().ToUpper() == "CBSE")
                                                {
                                                    if (Concatvalue.Trim() == "")
                                                    {
                                                        Concatvalue = CourseCode;
                                                    }
                                                    else
                                                    {
                                                        Concatvalue = Concatvalue + " - " + CourseCode;
                                                    }
                                                }
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(Concatvalue);
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                            }
                                            else if (columname == "securedmark")
                                            {
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(percentage);
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                            }
                                            else if (columname != "PaidAmount")
                                            {
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(dv[0][columname]);
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                            }
                                        }
                                    }
                                }
                                #region Hostel admission form fee
                                if (rdbtype.Items[1].Selected == true)
                                {
                                    if (settings == "1")
                                    {
                                        string headerledger = admissionformfeesheaderledger();
                                        if (headerledger.Trim() != "")
                                        {
                                            string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Fee Yearwise' AND college_code ='" + ddl_collegename.SelectedItem.Value + "' and user_code='" + Session["usercode"].ToString() + "'");
                                            string type = "";
                                            if (linkvalue.Trim() == "1")
                                                type = "Year";
                                            if (linkvalue.Trim() == "0")
                                                type = "Semester";
                                            string feecatval = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + dv[0]["Current_Semester"] + " " + type + "'");
                                            string[] headerled = headerledger.Split(',');
                                            if (headerled.Length > 1 && feecatval.Trim() != "")
                                            {
                                                if (Convert.ToString(headerled[0].Trim()) != "" && Convert.ToString(headerled[1].Trim()) != "")
                                                {
                                                    if (studentbalancefee.Tables[0].Rows.Count > 0)
                                                    {
                                                        studentbalancefee.Tables[0].DefaultView.RowFilter = " App_No='" + Convert.ToString(dv[0]["app_no"]) + "' and HeaderFK='" + Convert.ToString(headerled[0].Trim()) + "' and LedgerFK='" + Convert.ToString(headerled[1].Trim()) + "' and FeeCategory='" + feecatval + "'";
                                                        DataView balfee_dv = new DataView();
                                                        balfee_dv = studentbalancefee.Tables[0].DefaultView;
                                                        if (balfee_dv.Count > 0)
                                                        {
                                                            double feeamt = 0; cc++;
                                                            double.TryParse(Convert.ToString(balfee_dv[0]["FeeAmount"]), out feeamt);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(feeamt);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Bold = true;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                            cc++;
                                                            double balamt = 0;
                                                            double.TryParse(Convert.ToString(balfee_dv[0]["PaidAmount"]), out balamt);

                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(balamt);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Bold = true;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].ForeColor = Color.Brown;

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        FpSpread3.Visible = true;
                        btn_movetoshort.Visible = true;
                        btn_movetoadmit.Visible = true;
                        FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                        FpSpread3.TitleInfo.Height = 30;
                        FpSpread3.CommandBar.Visible = false;
                        FpSpread3.Sheets[0].RowHeader.Visible = false;
                        FpSpread3.Width = 930;
                        FpSpread3.Height = 390;
                        FpSpread3.SaveChanges();
                        linkapplication.Visible = false;
                        linkundertaking.Visible = false;
                        //linkwithdrawal.Visible = false;
                        if (rdbtype.Items[1].Selected == true)
                        {
                            btn_movetoreject.Visible = true;
                            btn_movetoshort.Visible = false;
                        }
                        if (rdbtype.Items[2].Selected == true)
                        {
                            //btnprintpdf.Visible = true;
                            btn_movetoshort.Visible = false;
                            btn_movetoadmit.Visible = false;
                            linkapplication.Visible = true;
                            linkundertaking.Visible = true;
                            //linkwithdrawal.Visible = true;
                        }
                        lbl_toapplied.Text = "Total No Of Applied : " + dicsubcol.Count;
                    }
                    else
                    {
                        lbl_toapplied.Text = "";
                        FpSpread3.Visible = false;
                        lblalerterr.Text = "No Records Found";
                        alertpopwindow.Visible = true;
                        btn_movetoshort.Visible = false;
                        btn_movetoadmit.Visible = false;
                        btn_movetoreject.Visible = false;
                    }
                }
                else
                {
                    lbl_toapplied.Text = "";
                    FpSpread3.Visible = false;
                    lblalerterr.Text = "No Records Found";
                    alertpopwindow.Visible = true;
                    btn_movetoshort.Visible = false;
                    btn_movetoadmit.Visible = false;
                    btn_movetoreject.Visible = false;
                    return;
                }
            }
            else
            {
                if (ddl_colord.Items.Count == 0)
                {
                    lblalerterr.Text = "Please Set The Column Order Setting";
                    alertpopwindow.Visible = true;
                }
                else
                {
                    lblalerterr.Text = "Please Select All Fields";
                    alertpopwindow.Visible = true;
                }
                lbl_toapplied.Text = "";
                FpSpread3.Visible = false;
                btn_movetoshort.Visible = false;
                btn_movetoadmit.Visible = false;
                btn_movetoreject.Visible = false;
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, college_code, "Hostel admission Process");
        }
    }
    public string admissionformfeesetting()
    {
        string set = "";
        try
        {
            q1 = "  select LinkValue from New_InsSettings where LinkName='Hostel_Admission_Form_Fee' and user_code ='" + usercode + "' and college_code ='" + ddl_collegename.SelectedItem.Value + "'";
            string hosteladmissionfeesetting = Convert.ToString(d2.GetFunction(q1));
            if (hosteladmissionfeesetting.Trim() != "0")
            {
                string[] admissionformfee = hosteladmissionfeesetting.Split('$');//1$9,10$500
                if (admissionformfee.Length > 1)
                {
                    if (Convert.ToString(admissionformfee[0]) == "1")
                    {
                        set = "1";
                    }
                }
            }
        }
        catch { }
        return set;
    }
    public string admissionformfeesheaderledger()
    {
        string set = "";
        try
        {
            q1 = "  select LinkValue from New_InsSettings where LinkName='Hostel_Admission_Form_Fee' and user_code ='" + usercode + "' and college_code ='" + ddl_collegename.SelectedItem.Value + "'";
            string hosteladmissionfeesetting = Convert.ToString(d2.GetFunction(q1));
            if (hosteladmissionfeesetting.Trim() != "0")
            {
                string[] admissionformfee = hosteladmissionfeesetting.Split('$');//1$9,10$500
                if (admissionformfee.Length > 1)
                {
                    if (Convert.ToString(admissionformfee[0]) == "1")
                    {
                        if (Convert.ToString(admissionformfee[1]).Trim() != "")
                        {
                            set = Convert.ToString(admissionformfee[1]);
                        }
                    }
                }
            }
        }
        catch { }
        return set;
    }
    public void applied2()
    {
        try
        {
            string batchyear = Convert.ToString(Convert.ToString(ddl_batch.SelectedItem.Value));
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_dept);
            string religioncode = rs.GetSelectedItemsValueAsString(cbl_religion);
            string communitycode = rs.GetSelectedItemsValueAsString(cbl_comm);
            string orderval = "";
            string orderby = "";
            string ordervalue = ""; lbl_toapplied.Text = "";
            string percentagevalue = ""; string or = "";
            if (degreecode.Trim() != "" && batchyear.Trim() != "")
            {
                q1 = "select value from Master_Settings where settings='OrderBy Marks Setting' and usercode='" + usercode + "'";
                q1 = q1 + "  select value from Master_Settings where settings='orderbymarks'";
                q1 = q1 + "  select linkvalue from New_InsSettings n,CO_MasterValues c where n.linkname=c.mastervalue and c.MasterCriteria ='Hosteladmissioncolumnsettings' and n.user_code='" + usercode + "' and c.collegecode=n.college_code and c.collegecode ='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'";
                // q1 = q1 + " select column_name from admitcolumnset where textcriteria='percent'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    orderval = Convert.ToString(ds.Tables[0].Rows[0]["value"]);
                }
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    orderby = Convert.ToString(ds.Tables[1].Rows[0]["value"]);
                }
                if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
                {
                    string columnvalue = Convert.ToString(ds.Tables[2].Rows[0][0]);
                    if (columnvalue.Contains("26"))
                    {
                        percentagevalue = "percentage";
                    }
                    else
                    {
                        percentagevalue = "percentage";
                    }
                    //percentagevalue = Convert.ToString(ds.Tables[2].Rows[0][0]);
                    //if (percentagevalue == "")
                    //{
                    //    percentagevalue = "percentage";
                    //}
                }
                string marksaddquery = "";
                EquivalentToHSC();
                if (ddledu.SelectedItem.Text == "UG")
                {
                    marksaddquery = ",((ISNULL((securedmark / NULLIF( totalmark, 0 )),0))*1200) as securedmark";
                    marksaddquery = eqltohsc;
                }
                else
                {
                    marksaddquery = "," + percentagevalue + "";
                }
                FpSpread3.Sheets[0].ColumnCount = 0;
                FpSpread3.Sheets[0].RowCount = 0;
                FpSpread3.SaveChanges();
                int count = 0;
                int i = 0;
                int cc = 0;
                string addcomreli = "";
                if (religioncode != "")
                {
                    addcomreli = " and religion in('" + religioncode + "')";
                }
                if (communitycode != "")
                {
                    addcomreli = addcomreli + " and community in('" + communitycode + "')";
                }
                FpSpread3.Sheets[0].PageSize = 5;
                //FpSpread3.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                //FpSpread3.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
                //FpSpread3.Pager.Align = HorizontalAlign.Right;
                //FpSpread3.Pager.Font.Bold = true;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.White;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                //FpSpread3.Pager.PageCount = 5;
                FpSpread3.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = false;
                FpSpread3.ActiveSheetView.DefaultRowHeight = 25;
                FpSpread3.ActiveSheetView.Rows.Default.Font.Name = "Book Antiqua";
                FpSpread3.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
                FpSpread3.ActiveSheetView.Rows.Default.Font.Bold = false;
                FpSpread3.ActiveSheetView.Columns.Default.Font.Bold = false;
                FpSpread3.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread3.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
                FpSpread3.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpSpread3.ShowHeaderSelection = false;
                FpSpread3.Sheets[0].ColumnCount = 3;
                FpSpread3.Sheets[0].RowCount = 0;
                string admittedtype = "";
                btn_movetoreject.Visible = false; //btnprintpdf.Visible = false;
                string admithostelq = "";
                if (rdbtype.Items[0].Selected == true)
                {
                    admittedtype = " and isnull(hostel_admission_status,0)=0";
                    admithostelq = " and r.App_No not in(select APP_No from HT_HostelRegistration) ";
                }
                else if (rdbtype.Items[1].Selected == true)
                {
                    //btn_movetoreject.Visible = true;
                    admittedtype = " and hostel_admission_status=1";
                    admithostelq = " and r.App_No not in(select APP_No from HT_HostelRegistration) ";
                }
                else if (rdbtype.Items[2].Selected == true)
                {
                    admittedtype = " and hostel_admission_status=2";
                    admithostelq = " ";
                }
                string query = "";
                query = "select distinct case when isnull(CampusReq,0)=0 then 'No'  when CampusReq=1 then 'Yes' end as CampusReq,a.stud_type,a.parent_addressP,CASE WHEN co_curricular=0 then 'No' when co_curricular=1 then 'Yes' end as co_curricular,CASE WHEN DistinctSport=0 then 'No'  else 'Yes'  end as DistinctSport,CASE WHEN first_graduate=0 then 'No' when first_graduate=1 then 'Yes' end as first_graduate,CASE WHEN isdisable=0 then 'No' when isdisable=1 then 'Yes' end as isdisable,CASE WHEN IsExService=0 then 'No' when IsExService=1 then 'Yes' end as IsExService,CASE WHEN TamilOrginFromAndaman=0 then 'No' when TamilOrginFromAndaman=1 then 'Yes' end as TamilOrginFromAndaman,p.tancet_mark,(Select TextVal FROM TextValTable T WHERE mother_tongue = T.TextCode) mother_tongue,(Select TextVal FROM TextValTable T WHERE parent_statep = T.TextCode) parent_statep,p.percentage,p.major_percent,p.majorallied_percent,(Select TextVal FROM TextValTable T WHERE parent_occu = T.TextCode) parent_occu,(Select TextVal FROM TextValTable T WHERE citizen = T.TextCode) citizen,(Select TextVal FROM TextValTable T WHERE caste = T.TextCode) caste,a.parent_name,CONVERT(VARCHAR(11),dob,103) as dob,CONVERT(VARCHAR(11),date_applied,103) as date_applied,a.StuPer_Id,a.remarks,CASE WHEN sex=1 then 'Female' when sex=0 then 'Male' end as sex ,(Select TextVal FROM TextValTable T WHERE community = T.TextCode) community,(Select TextVal FROM TextValTable T WHERE religion = T.TextCode) religion,securedmark, a.app_no,a.Student_Mobile,a.Alternativedegree_code,a.stud_name,a.app_formno, a.degree_code,a.Batch_Year,a.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name,p.totalmark,religion,community,ISNULL (tt.priority2,0) as priority2 ,isnull(ts.priority1,0) as priority1,noofattempts,p.course_entno,P.instaddress,a.App_No,uni_state,type_semester,university_code,ISNULL(pt.TExtVal,'') Part2Language,ISNULL(Cc.TExtVal,'') Part1Language,ISNULL(Cc.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear,a.ApplBankRefNumber,CONVERT(varchar(10), ApplBankRefDate,103) as ApplBankRefDate,case when  isnull(p.vocational_stream,'0')='0' then 'No' when isnull(p.vocational_stream,'0')='1' then 'Yes' end as vocational_stream,(select TextVal  from TextValTable  where TextCode =p.course_code) as  course_code from degree d,Department dt,Course C,registration r  ,applyn A inner join Stud_prev_details P ON P.app_no = A.app_no left join perv_marks_history ph on ph.course_entno =p.course_entno LEFT JOIN TextValTable Cc ON Cc.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language left join TextValTable tt on tt.TextCode =a.religion left join TextValTable ts on ts.TextCode =a.community  Where  r.app_no=a.app_no " + admithostelq + " and r.app_no=p.app_no and r.degree_code=d.degree_code and  p.app_no = a.app_no  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and ISNULL(markPriority,1)=1  and a.Batch_Year in('" + batchyear + "')  and a.college_code='" + ddl_collegename.SelectedItem.Value + "'  and a.college_code=d.college_code and CampusReq=1 " + admittedtype + " ";
                query += " " + "and r.degree_code in('" + degreecode + "')" + " " + addcomreli + "";
                query = query + "  select linkvalue from New_InsSettings n,CO_MasterValues c where n.linkname=c.mastervalue and c.MasterCriteria ='Hosteladmissioncolumnsettings' and n.user_code='" + usercode + "' and c.collegecode=n.college_code and c.collegecode ='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'";
                // query = query + " select * from admitcolumnset  where user_code='" + usercode + "' and  textcriteria='column' union select '" + usercode + "' user_code,'Hostel Request' setcolumn ,'CampusReq' column_name ,46 priority,'" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' college_code ,'column' textcriteria ,null allot ,null allot_Confirm order by Convert(int,priority) asc";
                query = query + "  select value from Master_Settings  where settings='orderbymarks' and value<>''";
                query = query + "   select mastercriteriavalue1,mastervalue from CO_MasterValues where MasterCriteria ='Region header' and mastercriteriavalue1<>''  order by mastercriteria1 asc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                string headernamevalue = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Dictionary<int, double> dicsubcol = new Dictionary<int, double>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string Getmark = "";
                            string Totalmark = "";
                            string percentage = "";
                            if (ddledu.SelectedItem.Text == "UG")
                            {
                                Getmark = Convert.ToString(ds.Tables[0].Rows[i]["securedmark"]);
                                Totalmark = Convert.ToString(ds.Tables[0].Rows[i]["totalmark"]);
                            }
                            percentage = Convert.ToString(ds.Tables[0].Rows[i]["" + percentagevalue + ""]);
                            double totlmark = 0;
                            DataView dv = new DataView();
                            if (ddledu.SelectedItem.Text == "UG")
                            {
                                if (Getmark.Trim() == "")
                                {
                                    Getmark = "0";
                                }
                                totlmark = Convert.ToDouble(Getmark);
                                if (!dicsubcol.ContainsKey(Convert.ToInt32(ds.Tables[0].Rows[i]["app_no"])))
                                {
                                    dicsubcol.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["app_no"]), Convert.ToDouble(Math.Round(totlmark)));
                                }
                            }
                            else
                            {
                                if (percentage.Trim() == "")
                                {
                                    percentage = "0";
                                }
                                totlmark = Convert.ToDouble(percentage);
                                if (!dicsubcol.ContainsKey(Convert.ToInt32(ds.Tables[0].Rows[i]["app_no"])))
                                {
                                    dicsubcol.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["app_no"]), Convert.ToDouble(Math.Round(totlmark, 2)));
                                }
                            }
                        }
                    }
                    if (dicsubcol.Count > 0)
                    {
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "View";
                        cc = 2;
                        if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            headernamevalue = Convert.ToString(ds.Tables[1].Rows[0][0]);
                            string[] headername = headernamevalue.Split(',');
                            for (int u = 0; u < headername.Length; u++)
                            {
                                colval = Convert.ToString(headername[u]);
                                loadtext();
                                string percentage = headertext;
                                //for (int u = 0; u < ds.Tables[1].Rows.Count; u++)
                                //{
                                //    string percentage = Convert.ToString(ds.Tables[1].Rows[u]["setcolumn"]);
                                if (percentage.Trim() == "Hostel Name" || percentage.Trim() == "Room Type" || percentage.Trim() == "Boarding")
                                {
                                }
                                else
                                {
                                    cc++;
                                    FpSpread3.Sheets[0].ColumnCount = cc + 1;
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].Text = headertext;// Convert.ToString(ds.Tables[1].Rows[u]["setcolumn"]);
                                    if (ddledu.SelectedItem.Text == "PG")
                                    {
                                        if (percentage == "Marks")
                                        {
                                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, cc].Text = "Percentage";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            btn_movetoshort.Visible = false;
                            btn_movetoadmit.Visible = false;
                            FpSpread3.Visible = false;
                            lblalerterr.Text = "Please Set The Column Order Setting";
                            alertpopwindow.Visible = true;
                            return;
                        }
                        FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                        cball.AutoPostBack = true;
                        FarPoint.Web.Spread.TextCellType txtCt = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb.AutoPostBack = false;
                        FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType("MyCommand", FarPoint.Web.Spread.ButtonType.ImageButton, "../dashbd/view.png");
                        DataView dv = new DataView();
                        FpSpread3.Sheets[0].RowCount++;
                        FpSpread3.Sheets[0].SpanModel.Add(FpSpread3.Sheets[0].RowCount - 1, 2, 1, ds.Tables[1].Rows.Count + 2 - 1);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = cball;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].Locked = true;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Locked = false;
                        string oldregion = ""; string attempts = "";
                        Dictionary<string, string> prioritydic = new Dictionary<string, string>();
                        bool notregionsetting = false; bool notregioncount = false; bool notsetreli = false;
                    bb:
                        if (notregionsetting == false)
                        {
                            string valuereligion = "";
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds.Tables[3].Rows.Count; k++)
                                {
                                    if (valuereligion == "")
                                    {
                                        valuereligion = Convert.ToString(ds.Tables[3].Rows[k][0]);
                                    }
                                    else
                                    {
                                        valuereligion = valuereligion + "," + Convert.ToString(ds.Tables[3].Rows[k][0]);
                                    }
                                }
                            }
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds.Tables[3].Rows.Count; k++)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " religion1 in(" + Convert.ToString(ds.Tables[3].Rows[k][0]) + ")  ";
                                    dv = ds.Tables[0].DefaultView;
                                    if (notregioncount == true)
                                    {
                                        notregionsetting = true;
                                        ds.Tables[0].DefaultView.RowFilter = " religion1 not in(" + valuereligion + ") ";
                                        dv = ds.Tables[0].DefaultView;
                                        dv.Sort = "noofattempts ASC " + or;
                                        if (notsetreli == true)
                                        {
                                            break;
                                        }
                                    }
                                    if (dv.Count > 0)
                                    {
                                        for (int p = 0; p < dv.Count; p++)
                                        {
                                            FpSpread3.Sheets[0].RowCount++;
                                            count++;
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                string headernameval = Convert.ToString(ds.Tables[1].Rows[0][0]);
                                                string[] headername = headernameval.Split(',');
                                                if (notregioncount == false)
                                                {
                                                    if (oldregion.Trim() != Convert.ToString(ds.Tables[3].Rows[k][1]))//Convert.ToString(dv[p]["religion"]))
                                                    {
                                                        oldregion = Convert.ToString(ds.Tables[3].Rows[k][1]);
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[3].Rows[k][1]);//) Convert.ToString(dv[p]["religion"]);
                                                        FpSpread3.Sheets[0].SpanModel.Add(FpSpread3.Sheets[0].RowCount - 1, 0, 1, headername.Length + 4 - 1);
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
                                                        FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].Locked = true;
                                                        FpSpread3.Sheets[0].RowCount++;
                                                        if (attempts.Trim() != Convert.ToString(dv[p]["noofattempts"]))
                                                        {
                                                            attempts = Convert.ToString(dv[p]["noofattempts"]);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[p]["noofattempts"]) + " Attempts";
                                                            FpSpread3.Sheets[0].SpanModel.Add(FpSpread3.Sheets[0].RowCount - 1, 0, 1, headername.Length + 4 - 1);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue; FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].Locked = true;
                                                            FpSpread3.Sheets[0].RowCount++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (attempts.Trim() != Convert.ToString(dv[p]["noofattempts"]))
                                                        {
                                                            attempts = Convert.ToString(dv[p]["noofattempts"]);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[p]["noofattempts"]) + " Attempts";
                                                            FpSpread3.Sheets[0].SpanModel.Add(FpSpread3.Sheets[0].RowCount - 1, 0, 1, headername.Length + 4 - 1);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue; FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].Locked = true;
                                                            FpSpread3.Sheets[0].RowCount++;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (oldregion.Trim() != "Others")
                                                    {
                                                        oldregion = "Others";
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = "Others";
                                                        FpSpread3.Sheets[0].SpanModel.Add(FpSpread3.Sheets[0].RowCount - 1, 0, 1, headername.Length + 4 - 1);
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue; FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].Locked = true;
                                                        FpSpread3.Sheets[0].RowCount++;
                                                    }
                                                    if (attempts.Trim() != Convert.ToString(dv[p]["noofattempts"]))
                                                    {
                                                        attempts = Convert.ToString(dv[p]["noofattempts"]);
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[p]["noofattempts"]) + " Attempts";
                                                        FpSpread3.Sheets[0].SpanModel.Add(FpSpread3.Sheets[0].RowCount - 1, 0, 1, headername.Length + 4 - 1);
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue; FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].Locked = true;
                                                        FpSpread3.Sheets[0].RowCount++;
                                                        notsetreli = true;
                                                    }
                                                }
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[p]["app_no"]);
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Locked = true;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = cb;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].CellType = btn;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Column.Width = 50;
                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                cc = 2;
                                                //headernamevalue = Convert.ToString(ds.Tables[1].Rows[0][0]);
                                                //string[] headername = headernamevalue.Split(',');
                                                for (int u = 0; u < headername.Length; u++)
                                                {
                                                    colval = Convert.ToString(headername[u]);
                                                    loadtext();
                                                    string columname = sqlheadertext;
                                                    //for (int u = 0; u < ds.Tables[1].Rows.Count; u++)
                                                    //{
                                                    //    string columname = Convert.ToString(ds.Tables[1].Rows[u]["column_name"]);
                                                    if (columname == "HostelRegistrationPK" || columname == "RoomFK" || columname == "Boarding")
                                                    {
                                                    }
                                                    else
                                                    {
                                                        cc++;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].CellType = txtCt;
                                                        if (columname == "Alternativedegree_code")
                                                        {
                                                            string altercourse = d2.GetFunction("select distinct dt.Dept_Name+'-'+c.Course_Name as name,dt.Dept_Code,d.Degree_Code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + ddl_collegename.SelectedItem.Value + "' and d.Degree_Code='" + Convert.ToString(dv[0]["Alternativedegree_code"]) + "' ");
                                                            if (altercourse == "0")
                                                            {
                                                                altercourse = "";
                                                            }
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = altercourse;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                        }
                                                        else if (columname == "DistinctSport")
                                                        {
                                                            string value = Convert.ToString(dv[p][columname]);
                                                            if (value == "Yes")
                                                            {
                                                                string val = d2.GetFunction("select textval from applyn a,textvaltable t where app_no='" + Convert.ToString(dv[p]["app_no"]) + "' and textcode=DistinctSport");
                                                                if (val != "0")
                                                                {
                                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = value + "-" + val;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = value;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = value;
                                                            }
                                                        }
                                                        else if (columname == "totalfees")
                                                        {
                                                            FpSpread3.Sheets[0].Columns[cc].Visible = false;
                                                        }
                                                        else if (columname == "remarks")
                                                        {
                                                            string VocationStream = Convert.ToString(dv[p]["vocational_stream"]);
                                                            string Nationality = Convert.ToString(dv[p]["citizen"]);
                                                            string CourseCode = Convert.ToString(dv[p]["course_code"]);
                                                            string Concatvalue = "";
                                                            if (VocationStream.Trim() != "No")
                                                            {
                                                                Concatvalue = "Vocational";
                                                            }
                                                            if (Nationality.Trim().ToUpper() != "INDIAN")
                                                            {
                                                                if (Concatvalue.Trim() == "")
                                                                {
                                                                    Concatvalue = Nationality;
                                                                }
                                                                else
                                                                {
                                                                    Concatvalue = Concatvalue + " - " + Nationality;
                                                                }
                                                            }
                                                            if (CourseCode.Trim().ToUpper() == "CBSE")
                                                            {
                                                                if (Concatvalue.Trim() == "")
                                                                {
                                                                    Concatvalue = CourseCode;
                                                                }
                                                                else
                                                                {
                                                                    Concatvalue = Concatvalue + " - " + CourseCode;
                                                                }
                                                            }
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(Concatvalue);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                        }
                                                        else if (columname == "securedmark")
                                                        {
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(dicsubcol[Convert.ToInt32(dv[p]["app_no"])]);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                        }
                                                        else if (columname != "PaidAmount")
                                                        {
                                                            //if (columname == "religion")
                                                            //{
                                                            //oldregion = Convert.ToString(ds.Tables[3].Rows[k][1]);
                                                            //}
                                                            if (columname == "noofattempts")
                                                            {
                                                                attempts = Convert.ToString(dv[p][columname]);
                                                            }
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(dv[p][columname]);
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        attempts = "";
                                    }
                                }
                            }
                            if (notregionsetting == false)
                            {
                                notregioncount = true;
                                goto bb;
                            }
                        }
                        linkapplication.Visible = false;
                        linkundertaking.Visible = false;
                        //linkwithdrawal.Visible = false;
                        if (rdbtype.Items[1].Selected == true)
                        {
                            btn_movetoreject.Visible = true;
                        }
                        if (rdbtype.Items[2].Selected == true)
                        {
                            //btnprintpdf.Visible = true;
                            linkapplication.Visible = true;
                            linkundertaking.Visible = true;
                            //linkwithdrawal.Visible = true;
                        }
                        btn_movetoshort.Visible = true;
                        btn_movetoadmit.Visible = true;
                        FpSpread3.Visible = true;
                        FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                        FpSpread3.TitleInfo.Height = 30;
                        FpSpread3.CommandBar.Visible = false;
                        FpSpread3.Sheets[0].RowHeader.Visible = false;
                        FpSpread3.Width = 930;
                        FpSpread3.Height = 390;
                    }
                    else
                    {
                        FpSpread3.Visible = false; //Fpspread1_div.Visible = false;
                        lblalerterr.Text = "No Records Found";
                        alertpopwindow.Visible = true;
                        btn_movetoshort.Visible = false;
                        btn_movetoadmit.Visible = false;
                        btn_movetoreject.Visible = false;
                    }
                }
                else
                {
                    FpSpread3.Visible = false;
                    lblalerterr.Text = "No Records Found";
                    alertpopwindow.Visible = true;
                    btn_movetoshort.Visible = false;
                    btn_movetoadmit.Visible = false;
                    btn_movetoreject.Visible = false;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, college_code, "Hostel admission Process");
        }
    }
    public void EquivalentToHSC()
    {
        try
        {
            string value = d2.GetFunction("select value from Master_Settings where settings ='Equivalent To HSC' and usercode ='" + usercode + "'");
            if (value == "0")
            {
                if (ddledu.SelectedItem.Text == "UG")
                {
                    eqltohsc = ",securedmark";
                }
                else
                {
                    eqltohsc = ",percentage";
                }
            }
            else
            {
                if (ddledu.SelectedItem.Text == "UG")
                {
                    eqltohsc = ",((ISNULL((securedmark / NULLIF( totalmark, 0 )),0))*1200) as securedmark";
                }
                else
                {
                    eqltohsc = ",percentage";
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_popadmissionfeemovetoshort_Click(object sender, EventArgs e)
    {
        try
        {
            q1 = "  select LinkValue from New_InsSettings where LinkName='Hostel_Admission_Form_Fee' and user_code ='" + usercode + "' and college_code ='" + ddl_collegename.SelectedItem.Value + "'";
            string hosteladmissionfeesetting = Convert.ToString(d2.GetFunction(q1));
            if (hosteladmissionfeesetting.Trim() != "0")
            {
                string[] admissionformfee = hosteladmissionfeesetting.Split('$');//1$9,10$500
                if (admissionformfee.Length > 1)
                {
                    if (Convert.ToString(admissionformfee[0]) == "1")
                    {
                        applicationfees_div.Visible = true;
                        //ViewState["Hostel_Admission_Form_Fee"] = "1";
                    }
                    else
                    {
                        applicationfees_div.Visible = false;
                        btn_movetoshort_Click(sender, e);
                        return;
                    }

                    if (Convert.ToString(admissionformfee[1]).Trim() != "")
                    {
                        string[] headled = Convert.ToString(admissionformfee[1]).Split(',');
                        if (headled.Length > 1)
                        {
                            ddl_hosteladmissionH.SelectedIndex = ddl_hosteladmissionH.Items.IndexOf(ddl_hosteladmissionH.Items.FindByValue(headled[0]));
                            ddl_hosteladmissionL.SelectedIndex = ddl_hosteladmissionL.Items.IndexOf(ddl_hosteladmissionL.Items.FindByValue(headled[1]));
                            ddl_hosteladmissionH.Enabled = false;
                            ddl_hosteladmissionL.Enabled = false;
                        }
                    }
                    if (Convert.ToString(admissionformfee[2]).Trim() != "")
                    {
                        txt_hosteladmission.Text = Convert.ToString(admissionformfee[2]);
                    }
                }
            }
            else
            {
                btn_movetoshort_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = ex.ToString();
            applicationfees_div.Visible = false;
        }
    }
    protected void btn_movetoshort_Click(object sender, EventArgs e)
    {
        try
        {
            bool shortlist = false; int regup = 0;
            if (FpSpread3.Sheets[0].RowCount > 0)
            {
                FpSpread3.SaveChanges();
                ds.Clear();
                ds = d2.select_method_wo_parameter("select current_semester,app_no from Registration where college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' ", "text");
                for (int i = 1; i < FpSpread3.Sheets[0].Rows.Count; i++)
                {
                    int checkcol = Convert.ToInt32(FpSpread3.Sheets[0].Cells[i, 1].Value);
                    if (checkcol == 1)
                    {
                        string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Tag);
                        //barath 30.03.17
                        if (applicationfees_div.Visible == true)
                        {
                            #region Admission Form Fee allot
                            ds.Tables[0].DefaultView.RowFilter = " app_no='" + app_no + "'";
                            DataView dv = ds.Tables[0].DefaultView;
                            string currentsem = "";
                            if (dv.Count > 0)
                            {
                                currentsem = Convert.ToString(dv[0]["current_semester"]).Trim();
                            }
                            if (ddl_hosteladmissionH.Items.Count > 0 && ddl_hosteladmissionL.Items.Count > 0)
                            {
                                if (ddl_hosteladmissionH.SelectedItem.Value != "0" && ddl_hosteladmissionL.SelectedItem.Value != "0")
                                {
                                    string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Fee Yearwise' AND college_code ='" + ddl_collegename.SelectedItem.Value + "' and user_code='" + Session["usercode"].ToString() + "'");
                                    string type = "";
                                    if (linkvalue.Trim() == "1")
                                        type = "Year";
                                    if (linkvalue.Trim() == "0")
                                        type = "Semester";

                                    string feecatval = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + currentsem.Trim() + " " + type + "'");
                                    if (FinYearFK.Trim() != "" && feecatval.Trim() != "" && feecatval.Trim() != "0" && currentsem.Trim() != "")
                                    {
                                        string headerledger = admissionformfeesheaderledger();
                                        if (headerledger.Trim() != "")
                                        {
                                            string[] headerled = headerledger.Split(',');
                                            if (headerled.Length > 1)
                                            {
                                                if (Convert.ToString(headerled[0].Trim()) != "" && Convert.ToString(headerled[1].Trim()) != "")
                                                {
                                                    double amount = 0;
                                                    double.TryParse(txt_hosteladmission.Text, out amount);
                                                    q1 = " if exists (select * from FT_FeeAllot where App_No='" + app_no + "' and HeaderFK='" + Convert.ToString(headerled[0].Trim()) + "' and LedgerFK='" + Convert.ToString(headerled[1].Trim()) + "' and FeeCategory='" + feecatval + "' and FinYearFK='" + FinYearFK + "')update FT_FeeAllot set FeeAmount='" + amount + "',TotalAmount='" + amount + "',BalAmount='" + amount + "',AllotDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' ,DeductAmout='0',FromGovtAmt ='0' where App_No='" + app_no + "' and HeaderFK='" + Convert.ToString(headerled[0].Trim()) + "' and LedgerFK='" + Convert.ToString(headerled[1].Trim()) + "' and FeeCategory='" + feecatval + "' and FinYearFK='" + FinYearFK + "' else insert into FT_FeeAllot (App_No,HeaderFK,LedgerFK,FeeAmount,FeeCategory,MemType,PayMode,TotalAmount,BalAmount,FinYearFK,AllotDate) values('" + app_no + "','" + Convert.ToString(headerled[0].Trim()) + "','" + Convert.ToString(headerled[1].Trim()) + "','" + amount + "','" + feecatval + "','1','1','" + amount + "','" + amount + "','" + FinYearFK + "','" + System.DateTime.Now.ToString("MM/dd/yyyy") + "')";//ddl_hosteladmissionH.SelectedItem.Value.ToString() ddl_hosteladmissionL.SelectedItem.Value.ToString()
                                                    q1 += " update Registration set hostel_admission_status='1' where App_No='" + app_no + "'";
                                                    regup = d2.update_method_wo_parameter(q1, "text");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (FinYearFK.Trim() == "")
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Text = "Please Set Financial Year Settings";
                                            return;
                                        }
                                        if (feecatval.Trim() == "")
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Text = "Please Set FeeCatagory Settings";
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Text = "Please Select Room Header and ledger";
                                    return;
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please Set Header and Ledger Settings";
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            q1 += " update Registration set hostel_admission_status='1' where App_No='" + app_no + "'";
                            regup = d2.update_method_wo_parameter(q1, "text");
                        }
                        if (regup != 0)
                        {
                            shortlist = true;
                        }
                    }
                }
                if (shortlist == true)
                {
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "ShortListed Successfully";
                    alertpopwindow.Visible = true;
                    applicationfees_div.Visible = false;
                    btn_go_Click(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, college_code, "Hostel admission Process");
        }
    }
    protected void btn_movetoreject_Click(object sender, EventArgs e)
    {
        try
        {
            bool shortlist = false; bool chk = false;
            if (FpSpread3.Sheets[0].RowCount > 0)
            {
                FpSpread3.SaveChanges();
                for (int i = 1; i < FpSpread3.Sheets[0].Rows.Count; i++)
                {
                    int checkcol = Convert.ToInt32(FpSpread3.Sheets[0].Cells[i, 1].Value);
                    if (checkcol == 1)
                    {
                        string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Tag);
                        string regupdate = " update Registration set hostel_admission_status='3' where App_No='" + app_no + "'";
                        int regup = d2.update_method_wo_parameter(regupdate, "Text");
                        if (regup != 0)
                        {
                            shortlist = true;
                        }
                        chk = true;
                    }
                }
                if (shortlist == true)
                {
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Rejected Successfully";
                    alertpopwindow.Visible = true;
                    btn_go_Click(sender, e);
                }
                if (chk == false)
                {
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please Select Student";
                    alertpopwindow.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, college_code, "Hostel admission Process");
        }
    }
    protected void btn_movetoadmit_Click(object sender, EventArgs e)
    {
        bool notselect = false;
        if (FpSpread3.Sheets[0].RowCount > 0)
        {
            if (ddl_collegename.Items.Count > 0 && ddl_hostelname.Items.Count > 0 && ddl_floorname.Items.Count > 0 && ddl_roomtype.Items.Count > 0 && ddl_building.Items.Count > 0)
            {
                if (ddl_collegename.SelectedItem.Value != "0" && ddl_hostelname.SelectedItem.Value != "0" && ddl_floorname.SelectedItem.Value != "0" && ddl_building.SelectedItem.Value != "0" && ddl_roomname.SelectedItem.Value != "0" && ddl_roomtype.SelectedItem.Value != "0")
                {
                    FpSpread3.SaveChanges();
                    if (rdbtype.SelectedIndex == 2)
                    {
                        return;
                    }
                    for (int i = 1; i < FpSpread3.Sheets[0].Rows.Count; i++)
                    {
                        int checkcol = Convert.ToInt32(FpSpread3.Sheets[0].Cells[i, 1].Value);
                        if (checkcol == 1)
                        {
                            Radiobtnstype.Items[0].Selected = true;
                            pop_roomselection.Visible = true;
                            btn_save.Visible = true;
                            notselect = true;
                        }
                    }
                    if (notselect == false)
                    {
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please select student ";
                        alertpopwindow.Visible = true;
                    }
                }
            }
        }
    }
    protected void FpSpread3_command(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
            string value = "";
            if (Convert.ToInt32(activecol) == 1 && Convert.ToInt32(activerow) == 0)
            {
                value = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Value);
                if (value == "1")
                {
                    for (int i = 0; i < FpSpread3.Sheets[0].Rows.Count; i++)
                    {
                        FpSpread3.Sheets[0].Cells[i, 1].Value = 1;
                    }
                }
                else
                {
                    for (int i = 0; i < FpSpread3.Sheets[0].Rows.Count; i++)
                    {
                        FpSpread3.Sheets[0].Cells[i, 1].Value = 0;
                    }
                }
            }
            if (activecol == "2")
            {
                string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                ViewState["pdfapp_no"] = app_no;
                pdf();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_printapp_Click(object sender, EventArgs e)
    {
        pdf();
    }
    public void pdf()
    {
        try
        {
            Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypage = mydoc.NewPage();
            Gios.Pdf.PdfPage mypage1 = mydoc.NewPage();
            Gios.Pdf.PdfPage mypage2 = mydoc.NewPage();
            bool dummyflage = false;
            if (File.Exists(HttpContext.Current.Server.MapPath("~/image/logo.jpg")))//Aruna
            {
                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/image/logo.jpg"));
                mypage.Add(LogoImage, 20, 20, 200);
            }
            if (File.Exists(HttpContext.Current.Server.MapPath("~/image/logo1.jpg")))//Aruna
            {
                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/image/logo1.jpg"));
                mypage.Add(LogoImage, 500, 20, 200);
            }
            string collquery = "";
            collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(collquery, "Text");
            string collegename = "";
            string collegeaddress = "";
            string collegedistrict = "";
            string phonenumber = "";
            string fax = "";
            string email = "";
            string website = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address2"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
                email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
            }
            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 10, 10, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 25, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 35, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 45, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 55, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 65, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, website);
            mypage.Add(ptc);
            int y = 60;
            int line1 = 50;
            int line2 = 400;
            string query = "  select dt.Dept_Name,c.Course_Name,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,r.stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,r.degree_code,r.batch_year,r.college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,r.current_semester,ncccadet from applyn a,Registration r,degree d,Department dt,Course C where a.app_no=r.App_No and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no='" + Convert.ToString(ViewState["pdfapp_no"]) + "'";
            query = query + " select course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + Convert.ToString(ViewState["pdfapp_no"]) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Course Details");
                mypage.Add(ptc);
                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                                 new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Stream");
                //mypage.Add(ptc);
                //string stream = "";
                //if (ddltype.SelectedItem.Text != "--Select--")
                //{
                //    stream = Convert.ToString(ddltype.SelectedItem.Text);
                //}
                //else
                //{
                //    stream = "";
                //}
                //Convert.ToString(ddldegree.SelectedItem.Text);
                //    Convert.ToString(ddldept.SelectedItem.Text);
                string degreename = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
                string coursename = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);
                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                                 new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + stream);
                //mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Graduation");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ddledu.SelectedItem.Text));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + degreename);
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Course");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + coursename);
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, line1, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Application No");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Applicant Name");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, line2, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                mypage.Add(ptc);
                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                                new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Applicant Last  Name");
                //mypage.Add(ptc);
                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                              new PdfArea(mydoc, line2, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Session["lastname"]));
                //mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date of Birth");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line2, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["dob"]));
                mypage.Add(ptc);
                string gender = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                if (gender == "0")
                {
                    gender = "Male";
                }
                else if (gender == "1")
                {
                    gender = "Female";
                }
                else if (gender == "2")
                {
                    gender = "Transgender";
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Gender");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(gender));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Parent's Name/Guardian Name");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Relationship");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Relationship"]));
                mypage.Add(ptc);
                string occupation = Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]);
                if (occupation.Trim() != "")
                {
                    occupation = subjectcode(occupation);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Occupation");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Session["occupation"]));
                mypage.Add(ptc);
                string mothertounge = Convert.ToString(ds.Tables[0].Rows[0]["mother_tongue"]);
                if (mothertounge.Trim() != "")
                {
                    mothertounge = subjectcode(mothertounge);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mother Tounge ");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mothertounge));
                mypage.Add(ptc);
                string Religion = Convert.ToString(ds.Tables[0].Rows[0]["religion"]);
                if (Religion.Trim() != "")
                {
                    Religion = subjectcode(Religion);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Religion");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Religion));
                mypage.Add(ptc);
                string Nationality = Convert.ToString(ds.Tables[0].Rows[0]["citizen"]);
                if (Nationality.Trim() != "")
                {
                    Nationality = subjectcode(Nationality);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Nationality");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Nationality));
                mypage.Add(ptc);
                string coummunity = Convert.ToString(ds.Tables[0].Rows[0]["community"]);
                if (coummunity.Trim() != "")
                {
                    coummunity = subjectcode(coummunity);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Coummunity(Foriegn Students Select OC)");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(coummunity));
                mypage.Add(ptc);
                string caste = Convert.ToString(ds.Tables[0].Rows[0]["caste"]);
                if (caste.Trim() != "")
                {
                    caste = subjectcode(caste);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Caste");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(caste));
                mypage.Add(ptc);
                string subreligion = Convert.ToString(ds.Tables[0].Rows[0]["caste"]);
                if (subreligion.Trim() != "")
                {
                    subreligion = subjectcode(subreligion);
                }
                int col = y + 370;
                if (Convert.ToString(subreligion).ToUpper() == "PROTESTANT")
                {
                    string missionarychild = Convert.ToString(ds.Tables[0].Rows[0]["MissionaryChild"]);
                    if (missionarychild == "0" || missionarychild == "False")
                    {
                        missionarychild = "No";
                    }
                    else
                    {
                        missionarychild = "Yes";
                    }
                    col += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You a missionary child ?");
                    mypage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(missionarychild));
                    mypage.Add(ptc);
                }
                string tamilorgion = Convert.ToString(ds.Tables[0].Rows[0]["TamilOrginFromAndaman"]);
                if (tamilorgion.Trim() == "0" || tamilorgion.Trim() == "False")
                {
                    tamilorgion = "No";
                }
                else
                {
                    tamilorgion = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You of Tamil Origin From Andaman and Nicobar Islands ? ");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(tamilorgion));
                mypage.Add(ptc);
                string xserviceman = Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]);
                if (xserviceman.Trim() == "0" || xserviceman.Trim() == "False")
                {
                    xserviceman = "No";
                }
                else
                {
                    xserviceman = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You a Child of an Ex-serviceman of Tamil Nadu origin ?");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(xserviceman));
                mypage.Add(ptc);
                string differentlyabled = Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]);
                if (differentlyabled.Trim() == "0" || differentlyabled.Trim() == "False")
                {
                    differentlyabled = "No";
                }
                else
                {
                    differentlyabled = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you a Differently abled");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(differentlyabled));
                mypage.Add(ptc);
                string firstgeneration = Convert.ToString(ds.Tables[0].Rows[0]["first_graduate"]);
                if (firstgeneration.Trim() == "0" || firstgeneration.Trim() == "False")
                {
                    firstgeneration = "No";
                }
                else
                {
                    firstgeneration = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you a first genaration learner ?");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(firstgeneration));
                mypage.Add(ptc);
                string oncampus = Convert.ToString(ds.Tables[0].Rows[0]["CampusReq"]);
                if (oncampus.Trim() == "0" || oncampus.Trim() == "False")
                {
                    oncampus = "No";
                }
                else
                {
                    oncampus = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Is Residence on Campus Required ?");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(oncampus));
                mypage.Add(ptc);
                string sports = Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]);
                if (sports.Trim() != "")
                {
                    sports = subjectcode(sports);
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Distinction in Sports");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(sports));
                mypage.Add(ptc);
                string cocucuricular = Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]);
                if (cocucuricular.Trim() != "")
                {
                    cocucuricular = subjectcode(cocucuricular);
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Extra Curricular Activites/Co-Curricular Activites");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(cocucuricular));
                mypage.Add(ptc);
                col += 20;
                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Communication Address");
                mypage.Add(ptc);
                string addressline1 = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressC"]);
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line1");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline1));
                mypage.Add(ptc);
                string addressline2 = Convert.ToString(ds.Tables[0].Rows[0]["Streetc"]);
                string addressline3 = "";
                if (addressline2.Contains('/') == true)
                {
                    string[] splitaddress = addressline2.Split('/');
                    if (splitaddress.Length > 1)
                    {
                        addressline2 = Convert.ToString(splitaddress[0]);
                        addressline3 = Convert.ToString(splitaddress[1]);
                    }
                    else
                    {
                        addressline2 = Convert.ToString(splitaddress[0]);
                    }
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line2");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline2));
                mypage.Add(ptc);
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line3");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline3));
                mypage.Add(ptc);
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "City");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Cityc"]));
                mypage.Add(ptc);
                string pstate = Convert.ToString(ds.Tables[0].Rows[0]["parent_statec"]);
                if (pstate.Trim() != "")
                {
                    pstate = subjectcode(pstate);
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "State");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(pstate));
                mypage.Add(ptc);
                col += 20;
                string country = Convert.ToString(ds.Tables[0].Rows[0]["Countryc"]);
                if (country.Trim() != "")
                {
                    country = subjectcode(country);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Country");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(country));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col + 20, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "PIN code");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col + 20, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodec"]));
                mypage.Add(ptc);
                y = 40;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, line1, y + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, line1, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Alternate Number");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["alter_mobileno"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Email ID");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Phone Number With STD Code");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnoc"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Permanent Address");
                mypage1.Add(ptc);
                string addresslinec1 = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line1");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec1));
                mypage1.Add(ptc);
                string addresslinec2 = Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]);
                string addresslinec3 = "";
                if (addressline2.Contains('/') == true)
                {
                    string[] splitaddress = addressline2.Split('/');
                    if (splitaddress.Length > 1)
                    {
                        addresslinec2 = Convert.ToString(splitaddress[0]);
                        addresslinec3 = Convert.ToString(splitaddress[1]);
                    }
                    else
                    {
                        addresslinec2 = Convert.ToString(splitaddress[0]);
                    }
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line2");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec2));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line3");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec3));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "City");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["cityp"]));
                mypage1.Add(ptc);
                string cstate = Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]);
                if (cstate.Trim() != "")
                {
                    cstate = subjectcode(cstate);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "State");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(cstate));
                mypage1.Add(ptc);
                string ccournty = Convert.ToString(ds.Tables[0].Rows[0]["Countryp"]);
                if (ccournty.Trim() != "")
                {
                    ccournty = subjectcode(ccournty);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Country");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ccournty));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "PIN code");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Phone Number With STD Code");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Academic Details");
                mypage1.Add(ptc);
                if (ddledu.SelectedItem.Text.ToUpper() == "UG")
                {
                    string qualifyingexam = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                    if (qualifyingexam.Trim() != "")
                    {
                        qualifyingexam = subjectcode(qualifyingexam);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Examination Passed");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qualifyingexam));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name of School");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Location of School");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]));
                    mypage1.Add(ptc);
                    string mediumofstudy = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);
                    if (mediumofstudy.Trim() != "")
                    {
                        mediumofstudy = subjectcode(mediumofstudy);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Medium of Study of Qualifying Examination");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mediumofstudy));
                    mypage1.Add(ptc);
                    string qulifyboard = Convert.ToString(ds.Tables[1].Rows[0]["university_code"]);
                    if (qulifyboard.Trim() != "")
                    {
                        qulifyboard = subjectcode(qulifyboard);
                    }
                    string qulifystate = Convert.ToString(ds.Tables[1].Rows[0]["uni_state"]);
                    if (qulifystate.Trim() != "")
                    {
                        qulifystate = subjectcode(qulifystate);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Board & State");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qulifyboard) + " " + Convert.ToString(qulifystate));
                    mypage1.Add(ptc);
                    string vocationalstream = Convert.ToString(ds.Tables[1].Rows[0]["Vocational_stream"]);
                    if (vocationalstream.Trim() == "0" || vocationalstream.Trim() == "False")
                    {
                        vocationalstream = "No";
                    }
                    else
                    {
                        vocationalstream = "Yes";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you Vocational stream");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(vocationalstream));
                    mypage1.Add(ptc);
                    string markgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
                    if (markgrade.Trim() == "False")
                    {
                        markgrade = "Mark";
                    }
                    else
                    {
                        markgrade = "Grade";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Marks/Grade");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(markgrade));
                    mypage1.Add(ptc);
                    string percentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
                    int totalmark = 0;
                    int maxtotal = 0;
                    DataTable data = new DataTable();
                    DataRow dr = null;
                    Hashtable hash = new Hashtable();
                    string markquery = "select psubjectno,registerno,acual_marks,grade,max_marks,noofattempt,pass_month,pass_year from perv_marks_history  where course_entno ='" + Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]) + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(markquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        data.Columns.Add("Language", typeof(string));
                        data.Columns.Add("Subject", typeof(string));
                        data.Columns.Add("Marks Obtained", typeof(string));
                        data.Columns.Add("Month", typeof(string));
                        data.Columns.Add("Year", typeof(string));
                        data.Columns.Add("Register No / Roll No", typeof(string));
                        data.Columns.Add("No of Attempts", typeof(string));
                        data.Columns.Add("Maximum Marks", typeof(string));
                        hash.Add(0, "Language1");
                        hash.Add(1, "Language2");
                        hash.Add(2, " Subject1");
                        hash.Add(3, " Subject2");
                        hash.Add(4, " Subject3");
                        hash.Add(5, " Subject4");
                        hash.Add(6, " Subject5");
                        hash.Add(7, " Subject6");
                        hash.Add(8, " Subject7");
                        hash.Add(9, " Subject8");
                        hash.Add(10, " Subject9");
                        hash.Add(11, " Subject10");
                        hash.Add(12, " Subject11");
                        for (int mark = 0; mark < ds.Tables[0].Rows.Count; mark++)
                        {
                            string subjectno = Convert.ToString(ds.Tables[0].Rows[mark]["psubjectno"]);
                            string actualmark = "";
                            if (markgrade.Trim() == "Mark")
                            {
                                actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["acual_marks"]);
                            }
                            if (markgrade.Trim() == "Grade")
                            {
                                actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["grade"]);
                            }
                            string month = Convert.ToString(ds.Tables[0].Rows[mark]["pass_month"]);
                            string year = Convert.ToString(ds.Tables[0].Rows[mark]["pass_year"]);
                            string regno = Convert.ToString(ds.Tables[0].Rows[mark]["registerno"]);
                            string noofattenm = Convert.ToString(ds.Tables[0].Rows[mark]["noofattempt"]);
                            string maxmark = Convert.ToString(ds.Tables[0].Rows[mark]["max_marks"]);
                            dr = data.NewRow();
                            string lang = Convert.ToString(hash[mark]);
                            dr[0] = Convert.ToString(lang);
                            string sub = subjectcode(subjectno);
                            dr[1] = Convert.ToString(sub);
                            dr[2] = Convert.ToString(actualmark);
                            dr[3] = Convert.ToString(month);
                            dr[4] = Convert.ToString(year);
                            dr[5] = Convert.ToString(regno);
                            dr[6] = Convert.ToString(noofattenm);
                            dr[7] = Convert.ToString(maxmark);
                            data.Rows.Add(dr);
                            if (markgrade.Trim() != "Grade")
                            {
                                totalmark = totalmark + Convert.ToInt32(actualmark);
                                maxtotal = maxtotal + Convert.ToInt32(maxmark);
                            }
                        }
                        //////////////// zzz
                        int count = 0;
                        count = data.Rows.Count;
                        Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Columns[4].SetWidth(100);
                        table2.Columns[5].SetWidth(100);
                        table2.Columns[6].SetWidth(100);
                        table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Subjects");
                        if (markgrade.Trim() == "Mark")
                        {
                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 1).SetContent("Mark");
                        }
                        if (markgrade.Trim() == "Grade")
                        {
                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 1).SetContent("Grade");
                        }
                        table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 2).SetContent("Month");
                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 3).SetContent("Year");
                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 4).SetContent("Register No");
                        table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 5).SetContent("No.of Attempts");
                        table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 6).SetContent("Maximun Marks");
                        for (int add = 0; add < data.Rows.Count; add++)
                        {
                            table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Subject"]));
                            table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 1).SetContent(Convert.ToString(data.Rows[add]["Marks Obtained"]));
                            table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Month"]));
                            // Month.First().ToString().ToUpper() + Month.Substring(1)
                            table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Year"]));
                            table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Register No / Roll No"]));
                            table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["No of Attempts"]));
                            table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 550, 550, 550));
                        mypage1.Add(myprov_pdfpage1);
                        if (Convert.ToString(markgrade).Trim() == "Mark")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 40, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total Marks Obtained :  " + Convert.ToString(totalmark));
                            mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 250, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Maximum Marks :  " + Convert.ToString(maxtotal));
                            mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 480, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Percentage :  " + Convert.ToString(percentage));
                            mypage1.Add(ptc);
                        }
                    }
                }
                if (ddledu.SelectedItem.Text.ToUpper() == "PG")
                {
                    string qualifyingexam = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                    if (qualifyingexam.Trim() != "")
                    {
                        qualifyingexam = subjectcode(qualifyingexam);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Examination Passed");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qualifyingexam));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name of the College");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Location of the College");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]));
                    mypage1.Add(ptc);
                    string branchcode = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                    if (branchcode.Trim() != "")
                    {
                        branchcode = subjectcode(branchcode);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mention Major");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(branchcode));
                    mypage1.Add(ptc);
                    string typeofmajor = Convert.ToString(ds.Tables[1].Rows[0]["type_major"]);
                    if (typeofmajor.Trim() == "1")
                    {
                        typeofmajor = "Single";
                    }
                    else if (typeofmajor.Trim() == "2")
                    {
                        typeofmajor = "Double";
                    }
                    else if (typeofmajor.Trim() == "3")
                    {
                        typeofmajor = "Triple";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Type of Major");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(typeofmajor));
                    mypage1.Add(ptc);
                    string typeofsemester = Convert.ToString(ds.Tables[1].Rows[0]["type_semester"]);
                    if (typeofsemester.Trim() == "True")
                    {
                        typeofsemester = "Semester";
                    }
                    else
                    {
                        typeofsemester = "Non Semester";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Type of Semester");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(typeofsemester));
                    mypage1.Add(ptc);
                    string mediumofstudy = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);
                    if (mediumofstudy.Trim() != "")
                    {
                        mediumofstudy = subjectcode(mediumofstudy);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, line1, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Medium of Study at UG level");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mediumofstudy));
                    mypage1.Add(ptc);
                    string markgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
                    if (markgrade.Trim() == "False")
                    {
                        markgrade = "Mark";
                    }
                    else
                    {
                        markgrade = "Grade";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 450, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Marks/Grade");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 450, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(markgrade));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 470, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Registration No as Mentioned on your Mark Sheet");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 470, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["registration_no"]));
                    mypage1.Add(ptc);
                    string majorpercentage = Convert.ToString(ds.Tables[1].Rows[0]["major_percent"]);
                    string majoralliedpercentage = Convert.ToString(ds.Tables[1].Rows[0]["majorallied_percent"]);
                    string majoralliedpracticalspercentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
                    DataTable data = new DataTable();
                    DataRow dr = null;
                    Hashtable hash = new Hashtable();
                    int count = 0;
                    string pgquery = "select psubjectno,subject_typeno,acual_marks,max_marks,pass_month,pass_year,semyear ,grade  from perv_marks_history where course_entno ='" + Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]) + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(pgquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        data.Columns.Add("Sem", typeof(string));
                        //  data.Columns.Add("Sem/Year", typeof(string));
                        data.Columns.Add("Subject", typeof(string));
                        data.Columns.Add("Subject type", typeof(string));
                        data.Columns.Add("Marks", typeof(string));
                        data.Columns.Add("Month", typeof(string));
                        data.Columns.Add("Year", typeof(string));
                        data.Columns.Add("Maximum Marks", typeof(string));
                        int sno = 0;
                        for (int pg = 0; pg < ds.Tables[0].Rows.Count; pg++)
                        {
                            sno++;
                            string semyear = Convert.ToString(ds.Tables[0].Rows[pg]["semyear"]);
                            string subjectno = Convert.ToString(ds.Tables[0].Rows[pg]["psubjectno"]);
                            string subjecttypeno = Convert.ToString(ds.Tables[0].Rows[pg]["subject_typeno"]);
                            string actualmark = "";
                            if (markgrade.Trim() == "Mark")
                            {
                                actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["acual_marks"]);
                            }
                            else if (markgrade.Trim() == "Grade")
                            {
                                actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["grade"]);
                            }
                            string month = Convert.ToString(ds.Tables[0].Rows[pg]["pass_month"]);
                            string year = Convert.ToString(ds.Tables[0].Rows[pg]["pass_year"]);
                            // string noofattenm = Convert.ToString(ds.Tables[0].Rows[pg]["noofattempt"]);
                            string maxmark = Convert.ToString(ds.Tables[0].Rows[pg]["max_marks"]);
                            dr = data.NewRow();
                            dr[0] = Convert.ToString(semyear);
                            string subject = subjectcode(subjectno);
                            dr[1] = Convert.ToString(subject);
                            string typesub = subjectcode(subjecttypeno);
                            dr[2] = Convert.ToString(typesub);
                            dr[3] = Convert.ToString(actualmark);
                            dr[4] = Convert.ToString(month);
                            dr[5] = Convert.ToString(year);
                            dr[6] = Convert.ToString(maxmark);
                            data.Rows.Add(dr);
                        }
                    }
                    count = data.Rows.Count;
                    if (count < 8)
                    {
                        Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Columns[4].SetWidth(100);
                        table2.Columns[5].SetWidth(100);
                        table2.Columns[6].SetWidth(100);
                        table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Sem/Year");
                        table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 1).SetContent("Subject");
                        table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 2).SetContent("Type of Subject");
                        if (markgrade.Trim() == "Mark")
                        {
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Mark");
                        }
                        if (markgrade.Trim() == "Grade")
                        {
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Grade");
                        }
                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 4).SetContent("Month");
                        table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 5).SetContent("Year");
                        table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 6).SetContent("Maximun Marks");
                        for (int add = 0; add < data.Rows.Count; add++)
                        {
                            table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Sem"]));
                            table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 1).SetContent(Convert.ToString(Convert.ToString(data.Rows[add]["Subject"])));
                            table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Subject type"]));
                            // Month.First().ToString().ToUpper() + Month.Substring(1)
                            table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Marks"]));
                            table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Month"]));
                            table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["Year"]));
                            table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 600, 550, 550));
                        mypage1.Add(myprov_pdfpage1);
                        if (markgrade.Trim() == "Mark")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, line1, 750, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective inclusive of Theory and Practical  : " + Convert.ToString(majoralliedpracticalspercentage) + "");
                            mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, line1, 770, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total % of Marks in Major subjects alone (Including theory & Practicals)  : " + Convert.ToString(majorpercentage) + "");
                            mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, 790, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals  : " + Convert.ToString(majoralliedpercentage) + "");
                            mypage1.Add(ptc);
                        }
                    }
                    else
                    {
                        dummyflage = true;
                        Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Columns[4].SetWidth(100);
                        table2.Columns[5].SetWidth(100);
                        table2.Columns[6].SetWidth(100);
                        table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Sem/Year");
                        table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 1).SetContent("Subject");
                        table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 2).SetContent("Type of Subject");
                        if (markgrade.Trim() == "Mark")
                        {
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Mark");
                        }
                        if (markgrade.Trim() == "Grade")
                        {
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Grade");
                        }
                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 4).SetContent("Month");
                        table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 5).SetContent("Year");
                        table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 6).SetContent("Maximun Marks");
                        for (int add = 0; add < data.Rows.Count; add++)
                        {
                            table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Sem"]));
                            table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 1).SetContent(Convert.ToString(Convert.ToString(data.Rows[add]["Subject"])));
                            table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Subject type"]));
                            // Month.First().ToString().ToUpper() + Month.Substring(1)
                            table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Marks"]));
                            table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Month"]));
                            table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["Year"]));
                            table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 40, 550, 700));
                        mypage2.Add(myprov_pdfpage1);
                        if (markgrade.Trim() == "Mark")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, line1, 750, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective inclusive of Theory and Practical  : " + Convert.ToString(majoralliedpracticalspercentage) + "");
                            mypage2.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, line1, 770, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total % of Marks in Major subjects alone (Including theory & Practicals)  : " + Convert.ToString(majorpercentage) + "");
                            mypage2.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, 790, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals  : " + Convert.ToString(majoralliedpercentage) + "");
                            mypage2.Add(ptc);
                        }
                    }
                }
                mypage.SaveToDocument();
                mypage1.SaveToDocument();
                if (dummyflage == true)
                {
                    mypage2.SaveToDocument();
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Application.pdf";
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
    public void bindheader()
    {
        try
        {
            ddl_rrh.Items.Clear();
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + Session["usercode"].ToString() + " AND H.CollegeCode = " + Convert.ToString(ddl_collegename.SelectedItem.Value) + "   ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_rrh.DataSource = ds;
                ddl_rrh.DataTextField = "HeaderName";
                ddl_rrh.DataValueField = "HeaderPK";
                ddl_rrh.DataBind();

                ddl_hosteladmissionH.DataSource = ds;
                ddl_hosteladmissionH.DataTextField = "HeaderName";
                ddl_hosteladmissionH.DataValueField = "HeaderPK";
                ddl_hosteladmissionH.DataBind();
            }
            else
            {
                ddl_rrh.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex) { }
    }
    public void bindledger()
    {
        try
        {
            if (ddl_rrh.Items.Count > 0)
            {
                ddl_rrl.Items.Clear();
                string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and l.LedgerMode=0   AND P. UserCode = " + Session["usercode"].ToString() + " AND L.CollegeCode = " + Convert.ToString(ddl_collegename.SelectedItem.Value) + " and L.HeaderFK in (" + ddl_rrh.SelectedItem.Value.ToString() + ")";
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_rrl.DataSource = ds;
                    ddl_rrl.DataTextField = "LedgerName";
                    ddl_rrl.DataValueField = "LedgerPK";
                    ddl_rrl.DataBind();
                }
                else
                {
                    ddl_rrl.Items.Insert(0, "--Select--");
                }
            }
        }
        catch (Exception ex) { }
    }
    public void bindledgershortlist()
    {
        try
        {
            if (ddl_rrh.Items.Count > 0)
            {
                ddl_rrl.Items.Clear();
                string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and l.LedgerMode=0   AND P. UserCode = " + Session["usercode"].ToString() + " AND L.CollegeCode = " + Convert.ToString(ddl_collegename.SelectedItem.Value) + " and L.HeaderFK in (" + ddl_hosteladmissionH.SelectedItem.Value.ToString() + ")";
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_hosteladmissionL.DataSource = ds;
                    ddl_hosteladmissionL.DataTextField = "LedgerName";
                    ddl_hosteladmissionL.DataValueField = "LedgerPK";
                    ddl_hosteladmissionL.DataBind();
                }
                else
                {
                    ddl_hosteladmissionL.Items.Insert(0, "--Select--");
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void ddl_roorentH_Selectedindex_Changed(object sender, EventArgs e)
    {
        bindledger();
    }
    public string subjectcode(string textcri)
    {
        string subjec_no = "";
        try
        {
            DataSet ds23 = new DataSet();
            string select_subno = "select TextVal from textvaltable where TextCode ='" + textcri + "'";// and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' ";
            ds23.Clear();
            ds23 = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds23.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds23.Tables[0].Rows[0]["TextVal"]);
            }
        }
        catch
        {
        }
        return subjec_no;
    }
    public void clear()
    {
        try
        {
            ddl_hostelname.SelectedIndex = 0;
            ddl_building.SelectedIndex = 0;
            ddl_floorname.SelectedIndex = 0;
            ddl_roomname.SelectedIndex = 0;
            ddl_roomtype.SelectedIndex = 0;
        }
        catch { }
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            bool notselect = false; string smsappno_List = ""; List<string> smsstudentdetails = new List<string>();
            if (FpSpread3.Sheets[0].RowCount > 0)
            {
                if (ddl_collegename.Items.Count > 0 && ddl_hostelname.Items.Count > 0 && ddl_floorname.Items.Count > 0 && ddl_roomtype.Items.Count > 0 && ddl_building.Items.Count > 0)
                {
                    if (ddl_collegename.SelectedItem.Value != "0" && ddl_hostelname.SelectedItem.Value != "0" && ddl_floorname.SelectedItem.Value != "0" && ddl_building.SelectedItem.Value != "0" && ddl_roomname.SelectedItem.Value != "0" && ddl_roomtype.SelectedItem.Value != "0")
                    {
                        FpSpread3.SaveChanges(); int regup = 0;
                        for (int i = 1; i < FpSpread3.Sheets[0].Rows.Count; i++)
                        {
                            int checkcol = Convert.ToInt32(FpSpread3.Sheets[0].Cells[i, 1].Value);
                            if (checkcol == 1)
                            {
                                string SerialNo = Hostelserialno();
                                Radiobtnstype.Items[0].Selected = true;
                                pop_roomselection.Visible = true;
                                notselect = true;
                                string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Tag);
                                #region roomupdation
                                string hostelgender = d2.GetFunction("select HostelType from HM_HostelMaster where HostelMasterPK ='" + ddl_hostelname.SelectedValue + "'");
                                string studgender = d2.GetFunction("select sex from applyn where app_no='" + app_no + "'");
                                string studentgen = "";
                                if (studgender.Trim() == "0")
                                {
                                    studentgen = "1";
                                }
                                else if (studgender.Trim() == "1")
                                {
                                    studentgen = "2";
                                }
                                else if (studgender.Trim() == "2")
                                {
                                    studentgen = "0";
                                }
                                string buildingFk = Convert.ToString(ddl_building.SelectedItem.Value);
                                string floorFK = Convert.ToString(ddl_floorname.SelectedItem.Value);
                                string roomFk = Convert.ToString(ddl_roomname.SelectedItem.Value);
                                string roomtype = Convert.ToString(ddl_roomtype.SelectedItem.Value);
                                string building = Convert.ToString(ddl_building.SelectedItem.Text);
                                string floor = Convert.ToString(ddl_floorname.SelectedItem.Text);
                                string room = Convert.ToString(ddl_roomname.SelectedItem.Text);
                                string studmesstype = "";
                                if (Radiobtnstype.Items[0].Selected == true)
                                {
                                    studmesstype = "0";
                                }
                                else if (Radiobtnstype.Items[1].Selected == true)
                                {
                                    studmesstype = "1";
                                }
                                else
                                {
                                }
                                if (hostelgender.Trim() == studentgen.Trim() || hostelgender.Trim() == "0")
                                {
                                    if (roomtype.Trim() != "")
                                    {
                                        string q = "select students_allowed,Avl_Student from Room_Detail where Room_Type='" + roomtype + "' and Floor_Name='" + floor + "' and Room_Name='" + room + "' and Building_Name='" + building + "'";
                                        q += " select current_semester from Registration where App_No='" + app_no + "'";
                                        ds2.Clear();
                                        ds2 = d2.select_method_wo_parameter(q, "text");
                                        if (ds2.Tables.Count > 0 && ds2.Tables != null && ds2.Tables[0].Rows.Count > 0)
                                        {
                                            double comp1 = 0; double comp2 = 0;
                                            double.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["students_allowed"].ToString()), out comp1);
                                            double.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["Avl_Student"]), out comp2);
                                            if (comp1 >= comp2 && comp1 != comp2)
                                            {
                                                string query = "if not exists(select app_no from HT_HostelRegistration where APP_No='" + app_no + "' and (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0)) insert into HT_HostelRegistration(MemType,APP_No,HostelAdmDate,BuildingFK,FloorFK,RoomFK,StudMessType,HostelMasterFK, collegecode,Serial_No) values(1,'" + app_no + "','" + System.DateTime.Now.ToString("MM/dd/yyyy") + "','" + buildingFk + "','" + floorFK + "','" + roomFk + "','" + studmesstype + "','" + ddl_hostelname.SelectedValue + "','" + ddl_collegename.SelectedItem.Value + "','" + SerialNo + "')";
                                                int h = d2.update_method_wo_parameter(query, "Text");
                                                string regupdate = " update Registration set Stud_Type='Hostler',hostel_admission_status='2' where App_No='" + app_no + "'";
                                                regupdate += " update Room_Detail set Avl_Student= isnull(Avl_Student,0) + 1 where Room_Type='" + roomtype + "' and Floor_Name='" + floor + "' and Room_Name='" + room + "' and Building_Name='" + building + "'";
                                                regup = d2.update_method_wo_parameter(regupdate, "Text");
                                                //sms mobile no
                                                string name = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note);
                                                smsstudentdetails.Add(app_no + "-" + name + "-" + building + "-" + floor + "-" + room + "-" + roomtype);
                                                if (cb_feesallot.Checked == true)
                                                {
                                                    string currentsem = "";
                                                    if (ds2.Tables[1].Rows.Count > 0)
                                                    {
                                                        currentsem = Convert.ToString(ds2.Tables[1].Rows[0][0]);
                                                    }
                                                    if (ddl_rrh.Items.Count > 0 && ddl_rrl.Items.Count > 0)
                                                    {
                                                        if (ddl_rrh.SelectedItem.Value != "0" && ddl_rrl.SelectedItem.Value != "0")
                                                        {
                                                            string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Fee Yearwise' AND college_code ='" + ddl_collegename.SelectedItem.Value + "' and user_code='" + Session["usercode"].ToString() + "'");
                                                            string type = "";
                                                            if (linkvalue.Trim() == "1")
                                                                type = "Year";
                                                            if (linkvalue.Trim() == "0")
                                                                type = "Semester";
                                                            string FinYearFK = d2.getCurrentFinanceYear(Session["usercode"].ToString(), Convert.ToString(ddl_collegename.SelectedValue));
                                                            string feecatval = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + currentsem + " " + type + "'");
                                                            if (FinYearFK.Trim() != "" && feecatval.Trim() != "")
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(txt_roomrent.Text, out amount);
                                                                if (amount != 0)
                                                                {
                                                                    q1 = " if exists (select * from FT_FeeAllot where App_No='" + app_no + "' and HeaderFK='" + ddl_rrh.SelectedItem.Value.ToString() + "' and LedgerFK='" + ddl_rrl.SelectedItem.Value.ToString() + "' and FeeCategory='" + feecatval + "' and FinYearFK='" + FinYearFK + "')update FT_FeeAllot set FeeAmount='" + amount + "',TotalAmount='" + amount + "',BalAmount='" + amount + "',AllotDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' ,DeductAmout='0',FromGovtAmt ='0' where App_No='" + app_no + "' and HeaderFK='" + ddl_rrh.SelectedItem.Value.ToString() + "' and LedgerFK='" + ddl_rrl.SelectedItem.Value.ToString() + "' and FeeCategory='" + feecatval + "' and FinYearFK='" + FinYearFK + "' else insert into FT_FeeAllot (App_No,HeaderFK,LedgerFK,FeeAmount,FeeCategory,MemType,PayMode,TotalAmount,BalAmount,FinYearFK,AllotDate) values('" + app_no + "','" + ddl_rrh.SelectedItem.Value.ToString() + "','" + ddl_rrl.SelectedItem.Value.ToString() + "','" + amount + "','" + feecatval + "','1','1','" + amount + "','" + amount + "','" + FinYearFK + "','" + System.DateTime.Now.ToString("MM/dd/yyyy") + "')";
                                                                    int finsave = d2.update_method_wo_parameter(q1, "text");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (FinYearFK.Trim() == "")
                                                                {
                                                                    alertpopwindow.Visible = true;
                                                                    lblalerterr.Text = "Please Set Financial Year Settings";
                                                                    return;
                                                                }
                                                                if (feecatval.Trim() == "")
                                                                {
                                                                    alertpopwindow.Visible = true;
                                                                    lblalerterr.Text = "Please Set FeeCatagory Settings";
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            alertpopwindow.Visible = true;
                                                            lblalerterr.Text = "Please Select Room Header and ledger";
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        alertpopwindow.Visible = true;
                                                        lblalerterr.Text = "Please Set Header and Ledger Settings";
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                alertpopwindow.Visible = true;
                                                lblalerterr.Text = "Room fill please select another room";
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Text = "Please select room Room details";
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Text = "Please Update room Type";
                                        return;
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Text = "Please select valid hostel in this student";
                                }
                                #endregion
                            }
                        }
                        if (notselect == false)
                        {
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please select student ";
                            alertpopwindow.Visible = true;
                        }
                        if (regup != 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Admitted Successfully";
                            //send sms
                            ds.Clear();
                            q1 = " select value from Master_Settings where settings='Hostel Admission sms' and usercode='" + usercode + "'";
                            q1 += " select sms_user_id from Track_Value where college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'";
                            q1 += " select value from Master_Settings where settings='SMS Mobile Rights' and usercode='" + usercode + "'";
                            ds = d2.select_method_wo_parameter(q1, "text");
                            string smsrights = ""; string userid = ""; string sendsmsfor = ""; string sendno = "";
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                smsrights = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                if (smsrights.Trim() == "")
                                    smsrights = "0";
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                userid = Convert.ToString(ds.Tables[1].Rows[0][0]);
                                if (userid.Trim() == "")
                                    userid = "0";
                            }
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sendsmsfor = Convert.ToString(ds.Tables[2].Rows[0][0]);
                                if (sendsmsfor.Trim() == "")
                                    sendsmsfor = "0";
                            }
                            //string smsrights = d2.GetFunction("select value from Master_Settings where settings='Hostel Admission sms' and usercode='" + usercode + "'");
                            if (smsrights.Trim() == "1")
                            {
                                string messagetext = "";
                                //string userid = d2.GetFunction(" select sms_user_id from Track_Value where college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
                                foreach (string det in smsstudentdetails)
                                {
                                    string[] studentdet = det.Split('-');
                                    messagetext = " Hi " + Convert.ToString(studentdet[1]) + " your admitted in " + Convert.ToString(ddl_hostelname.SelectedItem.Text) + ", Hostel Details : Building Name:" + Convert.ToString(studentdet[2]) + ", Floor Name:" + Convert.ToString(studentdet[3]) + ", RoomName:" + Convert.ToString(studentdet[4]) + ", Room Type=" + Convert.ToString(studentdet[5]) + "  \n Thanks.";
                                    string mobile = d2.GetFunction("select ParentF_Mobile+'-'+ParentM_Mobile+'-'+Student_Mobile from applyn where app_no=" + studentdet[0] + "").Trim();
                                    if (mobile.Trim() != "--")
                                    {
                                        sendno = "";
                                        string[] sendmobilenumber = mobile.Split('-');
                                        string[] numbers = sendsmsfor.Split(',');
                                        if (numbers.Length > 0)
                                        {
                                            foreach (string no in numbers)
                                            {
                                                if (no == "1")
                                                    sendno = "," + Convert.ToString(sendmobilenumber[0]);
                                                if (no == "2")
                                                    sendno = "," + Convert.ToString(sendmobilenumber[1]);
                                                if (no == "3")
                                                    sendno = "," + Convert.ToString(sendmobilenumber[2]);
                                            }
                                            if (sendno != "0")
                                            {
                                                d2.send_sms(userid, Convert.ToString(ddl_collegename.SelectedItem.Value), usercode, sendno.TrimStart(','), messagetext, "0");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //btn_go_Click(sender, e);
                        clear();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, college_code, "Hostel admission Process");
        }
    }
    protected void imagebtnpop1close_Click(object sender, EventArgs e)
    {
        pop_roomselection.Visible = false;
    }
    protected void ddl_hostelname_SelectedIndexchange(object sender, EventArgs e)
    {
        try
        {
            bindbuilding();
            bindfloor();
            bindroom();
            bindroomtype();
            if (ddl_hostelname.Items.Count > 0)
            {
                txt_studentledger.Text = d2.GetFunction("select hosteladmfeeamount from hm_hostelmaster where hostelmasterpk='" + Convert.ToString(ddl_hostelname.SelectedItem.Value) + "'");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, college_code, "Hostel admission Process");
        }
    }
    protected void ddl_building_SelectedIndexchange(object sender, EventArgs e)
    {
        bindfloor();
        bindroom();
        bindroomtype();
    }
    protected void ddl_floorname_SelectedIndexchange(object sender, EventArgs e)
    {
        bindroom();
        bindroomtype();
    }
    protected void ddl_roomname_SelectedIndexchange(object sender, EventArgs e)
    {
        bindroomtype();
    }
    protected void cbfeeallot_click(object sender, EventArgs e)
    {
        if (cb_feesallot.Checked == true)
        {
            fintable.Visible = true;
            if (ddl_hostelname.Items.Count > 0)
            {
                txt_studentledger.Text = d2.GetFunction("select hosteladmfeeamount from hm_hostelmaster where hostelmasterpk='" + Convert.ToString(ddl_hostelname.SelectedItem.Value) + "'");
            }
            bindheader();
            bindledger();
        }
        else
        {
            fintable.Visible = false;
        }
    }
    protected void bindhostel()
    {
        try
        {
            ddl_hostelname.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_hostelname.DataSource = ds;
                ddl_hostelname.DataTextField = "HostelName";
                ddl_hostelname.DataValueField = "HostelMasterPK";
                ddl_hostelname.DataBind();
            }
            else
            {
                ddl_hostelname.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, college_code, "Hostel admission Process");
        }
    }
    protected void bindbuilding()
    {
        try
        {
            if (ddl_hostelname.Items.Count > 0)
            {
                if (ddl_hostelname.SelectedItem.Text.Trim() != "--Select--")
                {
                    ddl_building.Items.Clear();
                    string bul = ""; ds.Clear();
                    bul = d2.GetBuildingCode_inv(ddl_hostelname.SelectedItem.Value);
                    ds = d2.BindBuilding(bul);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_building.DataSource = ds;
                        ddl_building.DataTextField = "Building_Name";
                        ddl_building.DataValueField = "code";
                        ddl_building.DataBind();
                    }
                    else
                    {
                        ddl_building.Items.Insert(0, "--Select--");
                    }
                }
            }
        }
        catch { }
    }
    protected void bindfloor()
    {
        try
        {
            if (ddl_building.Items.Count > 0)
            {
                ddl_floorname.Items.Clear(); ds.Clear();
                ds = d2.BindFloor(ddl_building.SelectedItem.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_floorname.DataSource = ds;
                    ddl_floorname.DataTextField = "Floor_Name";
                    ddl_floorname.DataValueField = "Floorpk";
                    ddl_floorname.DataBind();
                }
                else
                {
                    ddl_floorname.Items.Insert(0, "--Select--");
                }
            }
        }
        catch { }
    }
    protected void bindroom()
    {
        try
        {
            if (ddl_building.Items.Count > 0 && ddl_floorname.Items.Count > 0)
            {
                ddl_roomname.Items.Clear();
                string itemname = "select distinct Room_Name,Roompk from Room_Detail where Building_Name in('" + ddl_building.SelectedItem.Text + "') and floor_name in('" + ddl_floorname.SelectedItem.Text + "') order by Room_Name";
                ds.Clear();
                ds = d2.select_method_wo_parameter(itemname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_roomname.DataSource = ds;
                    ddl_roomname.DataTextField = "Room_Name";
                    ddl_roomname.DataValueField = "Roompk";
                    ddl_roomname.DataBind();
                    //ddl_roomname.SelectedItem.Text
                }
                else
                {
                    ddl_roomname.Items.Insert(0, "--Select--");
                }
            }
        }
        catch
        {
        }
    }
    protected void bindroomtype()
    {
        try
        {
            if (ddl_building.Items.Count > 0 && ddl_floorname.Items.Count > 0)
            {
                ds.Clear(); ddl_roomtype.Items.Clear();
                ds = d2.BindRoomtype(ddl_floorname.SelectedItem.Text, ddl_building.SelectedItem.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_roomtype.DataSource = ds;
                    ddl_roomtype.DataTextField = "Room_Type";
                    ddl_roomtype.DataValueField = "Room_Type";
                    ddl_roomtype.DataBind();
                }
                else
                {
                    ddl_roomtype.Items.Insert(0, "--Select--");
                }
            }
        }
        catch { }
    }
    protected void linkapplication_click(object sender, EventArgs e)
    {
        ViewState["HostelForm"] = "admission";
        studentdetailspdf();
    }
    protected void linkundertaking_click(object sender, EventArgs e)
    {
        ViewState["HostelForm"] = "undertaking";
        studentdetailspdf();
    }
    protected void linkwithdrawal_click(object sender, EventArgs e)
    {
        ViewState["HostelForm"] = "withdrawal";
        studentdetailspdf();
    }
    protected void hosteladmissionform_empty()
    {
        try
        {
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Font header = new Font("Arial", 15, FontStyle.Bold);
            Font header1 = new Font("Arial", 14, FontStyle.Bold);
            Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
            Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            int left1 = 1;
            int left2 = 225;
            int left4 = 470;
            string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
            string university = "";
            string collname = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string affliated = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                collname = ds.Tables[0].Rows[0]["collname"].ToString();
                address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
            }
            string[] split = collname.Split('(');
            //**************right photo**************
            PdfArea pa4 = new PdfArea(mydocument, 459, 40, 120, 120);
            PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
            mypdfpage.Add(pr4);
            //**************left logo**************
            PdfArea collogoA = new PdfArea(mydocument, 14, 40, 120, 120);
            PdfRectangle collogoR = new PdfRectangle(mydocument, collogoA, Color.Black);
            mypdfpage.Add(collogoR);
            //**************1st header**************
            PdfArea pa5 = new PdfArea(mydocument, 140, 100, 310, 60);
            PdfRectangle pr5 = new PdfRectangle(mydocument, pa5, Color.Black);
            mypdfpage.Add(pr5);
            //*************page**************//
            PdfArea pa1 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
            PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
            mypdfpage.Add(pr3);
            //
            int coltop = 23;
            PdfTextArea ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Serial No:  ");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 460, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No: ");
            mypdfpage.Add(ptc);
            coltop = coltop + 10;
            ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, -40, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]));
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("(HOSTEL)"));
            mypdfpage.Add(ptc);
            coltop = coltop + 10;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, -22, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA");
            mypdfpage.Add(ptc);
            coltop = coltop + 35;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, -5, coltop - 20, 600, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "APPLICATION FOR ADMISSION");
            mypdfpage.Add(ptc);
            coltop = coltop + 15;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "2017 - 2018");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left2, 65, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "RECENT");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left2, 75, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "PASSPORT SIZE");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left2, 85, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "PHOTOGRAPH");
            mypdfpage.Add(ptc);
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
            {
                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                mypdfpage.Add(LogoImage, 27, 50, 250);
            }
            left1 = 40;
            coltop += 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "Note : (i)   Read the Hostel Prospectus carefully before filling up the application");
            coltop += 20;
            left1 = 65;
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " (ii)  Students are advised to satisfy themselves about the feacilities avaliable in the Hostel");
            coltop += 20; mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " (iii) Change of Address, if any, should be immediately intimated to this office.");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "To");
            mypdfpage.Add(ptc);
            coltop += 30;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "THE DEPUTY WARDEN,");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(split[0]).ToUpper());
            mypdfpage.Add(ptc);
            left1 += 90;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(" HOSTEL ," + address3.ToUpper() + " - " + pincode + ""));
            mypdfpage.Add(ptc);
            coltop += 30; left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "Sir,");
            mypdfpage.Add(ptc);
            left1 = 65; coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " I request you to provide accommodation in the Hostel for academic year 201    - 201    . I have gone through the Rules and ");
            mypdfpage.Add(ptc);
            left1 = 40; coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 500, 50), System.Drawing.ContentAlignment.MiddleLeft, " Regulations of the hostel and assure you that I will abide by them.");
            mypdfpage.Add(ptc);
            left1 = 40; coltop += 20;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Name of Applicant ( In CAPITAL)");
            mypdfpage.Add(ptc);
            PdfArea nameA = new PdfArea(mydocument, left1 + 170, coltop + 13, 360, 20);
            PdfRectangle nameR = new PdfRectangle(mydocument, nameA, Color.Black);
            mypdfpage.Add(nameR);
            left1 = 60; coltop += 20;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Year / Class ");
            mypdfpage.Add(ptc);
            coltop += 35;
            left1 = 40;
            PdfArea yearclassA = new PdfArea(mydocument, left1, coltop, 100, 20);
            PdfRectangle yearclassR = new PdfRectangle(mydocument, yearclassA, Color.Black);
            mypdfpage.Add(yearclassR);
            left1 = 180; coltop -= 35;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Major ");
            mypdfpage.Add(ptc);
            coltop += 35;
            left1 = 150;
            PdfArea majorA = new PdfArea(mydocument, left1, coltop, 100, 20);
            PdfRectangle majorR = new PdfRectangle(mydocument, majorA, Color.Black);
            mypdfpage.Add(majorR);
            left1 = 290; coltop -= 35;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Roll No ");
            mypdfpage.Add(ptc);
            coltop += 35;
            left1 = 260;
            PdfArea rollnoA = new PdfArea(mydocument, left1, coltop, 100, 20);
            PdfRectangle rollnoR = new PdfRectangle(mydocument, rollnoA, Color.Black);
            mypdfpage.Add(rollnoR);
            //DOB
            left1 = 400; coltop -= 35;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Date of Birth ");
            mypdfpage.Add(ptc);
            coltop += 35;
            left1 -= 30;
            PdfArea dobd = new PdfArea(mydocument, left1, coltop, 20, 20);
            PdfRectangle dobrd = new PdfRectangle(mydocument, dobd, Color.Black);
            mypdfpage.Add(dobrd);
            left1 += 20;
            PdfArea dobd1 = new PdfArea(mydocument, left1, coltop, 20, 20);
            PdfRectangle dobRd1 = new PdfRectangle(mydocument, dobd1, Color.Black);
            mypdfpage.Add(dobRd1);
            //Month
            left1 += 25;
            PdfArea dobm = new PdfArea(mydocument, left1, coltop, 20, 20);
            PdfRectangle dobrm = new PdfRectangle(mydocument, dobm, Color.Black);
            mypdfpage.Add(dobrm);
            left1 += 20;
            PdfArea dobmm = new PdfArea(mydocument, left1, coltop, 20, 20);
            PdfRectangle dobRdmm = new PdfRectangle(mydocument, dobmm, Color.Black);
            mypdfpage.Add(dobRdmm);
            //year
            left1 += 25;
            PdfArea doby = new PdfArea(mydocument, left1, coltop, 20, 20);
            PdfRectangle dobry = new PdfRectangle(mydocument, doby, Color.Black);
            mypdfpage.Add(dobry);
            left1 += 20;
            PdfArea dobmy = new PdfArea(mydocument, left1, coltop, 20, 20);
            PdfRectangle dobRdmmy = new PdfRectangle(mydocument, dobmy, Color.Black);
            mypdfpage.Add(dobRdmmy);
            left1 += 20;
            PdfArea doby1 = new PdfArea(mydocument, left1, coltop, 20, 20);
            PdfRectangle dobry1 = new PdfRectangle(mydocument, doby1, Color.Black);
            mypdfpage.Add(dobry1);
            left1 += 20;
            PdfArea dobmy1 = new PdfArea(mydocument, left1, coltop, 20, 20);
            PdfRectangle dobRdmmy1 = new PdfRectangle(mydocument, dobmy1, Color.Black);
            mypdfpage.Add(dobRdmmy1);
            //age
            left1 += 30; coltop -= 35;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Age ");
            mypdfpage.Add(ptc);
            left1 += 0;
            coltop += 35;
            PdfArea age = new PdfArea(mydocument, left1, coltop, 27, 20);
            PdfRectangle ager = new PdfRectangle(mydocument, age, Color.Black);
            mypdfpage.Add(ager);
            //second row
            left1 = 60; coltop += 20;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Nationality ");
            mypdfpage.Add(ptc);
            coltop += 40; left1 = 40;
            PdfArea nationa = new PdfArea(mydocument, left1, coltop, 100, 20);
            PdfRectangle nationr = new PdfRectangle(mydocument, nationa, Color.Black);
            mypdfpage.Add(nationr);
            left1 = 180; coltop += -40;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Religion ");
            mypdfpage.Add(ptc);
            coltop += 40;
            left1 = 150;
            PdfArea religiona = new PdfArea(mydocument, left1, coltop, 100, 20);
            PdfRectangle religionr = new PdfRectangle(mydocument, religiona, Color.Black);
            mypdfpage.Add(religionr);
            left1 = 290; coltop += -45;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Community ");
            mypdfpage.Add(ptc);
            left1 = 270; coltop += 10;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " FC / BC / MBC / ST / SC ");
            mypdfpage.Add(ptc);
            coltop += 35;
            PdfArea com = new PdfArea(mydocument, left1, coltop, 100, 20);
            PdfRectangle comr = new PdfRectangle(mydocument, com, Color.Black);
            mypdfpage.Add(comr);
            left1 = 440; coltop += -45;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Shift ");
            mypdfpage.Add(ptc);
            left1 = 420; coltop += 10;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " I / II / Fulltime ");
            mypdfpage.Add(ptc);
            coltop += 35; left1 = 410;
            PdfArea shift = new PdfArea(mydocument, left1, coltop, 100, 20);
            PdfRectangle shiftr = new PdfRectangle(mydocument, shift, Color.Black);
            mypdfpage.Add(shiftr);
            coltop += 30; left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " whether stayed in this hostel or any other hostel before ?");
            mypdfpage.Add(ptc);
            left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "If yes, state the name of the hostel, place & Room No");
            mypdfpage.Add(ptc);
            left1 = 410;
            PdfArea prehostel = new PdfArea(mydocument, left1, coltop, 160, 18);
            PdfRectangle prehostelr = new PdfRectangle(mydocument, prehostel, Color.Black);
            mypdfpage.Add(prehostelr);
            coltop += 18;
            PdfArea preroom = new PdfArea(mydocument, left1, coltop, 160, 18);
            PdfRectangle preroomr = new PdfRectangle(mydocument, preroom, Color.Black);
            mypdfpage.Add(preroomr);
            coltop += 10; left1 = 40;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " Particulars of Parent / Guardian ");
            mypdfpage.Add(ptc);
            coltop += 30;
            //parent table
            PdfArea parent = new PdfArea(mydocument, left1, coltop, 520, 195);
            PdfRectangle parentr = new PdfRectangle(mydocument, parent, Color.Black);
            mypdfpage.Add(parentr);
            left1 = 290;
            PdfLine liner3 = new PdfLine(mydocument, new Point(left1, coltop), new Point(left1, coltop + 195), Color.Black, 1);
            mypdfpage.Add(liner3);
            coltop -= 10; left1 = 140;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Parent ");
            mypdfpage.Add(ptc);
            left1 = 380;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Local Guardian ");
            mypdfpage.Add(ptc);
            coltop += 15; left1 = 40;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " NAME: ");
            mypdfpage.Add(ptc);
            left1 = 300;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " NAME: ");
            mypdfpage.Add(ptc);
            coltop += 10; left1 = 40;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 520, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Door No          : ");
            mypdfpage.Add(ptc);
            left1 = 140;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Old  : ");
            mypdfpage.Add(ptc);
            left1 = 220;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " New  : ");
            mypdfpage.Add(ptc);
            left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Door No          : ");
            mypdfpage.Add(ptc);
            left1 = 400;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Old  : ");
            mypdfpage.Add(ptc);
            left1 = 480;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " New  : ");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Road / Street  : ");
            mypdfpage.Add(ptc); left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Road / Street  : ");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Town / City     : ");
            mypdfpage.Add(ptc); left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Town / City     : ");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " District/ State  : ");
            mypdfpage.Add(ptc); left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " District/ State  : ");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Pincode          : ");
            mypdfpage.Add(ptc); left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Pincode          : ");
            mypdfpage.Add(ptc);
            coltop += 15;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Code / No       : ");
            mypdfpage.Add(ptc); left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Code / No       : ");
            mypdfpage.Add(ptc);
            coltop += 15;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Phone             : ");
            mypdfpage.Add(ptc); left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Phone             : ");
            mypdfpage.Add(ptc);
            coltop += 15;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mobile             : ");
            mypdfpage.Add(ptc); left1 = 300;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mobile             : ");
            mypdfpage.Add(ptc);
            //sign
            left1 = 40; coltop += 60;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the parent");
            mypdfpage.Add(ptc);
            left1 = 250;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Applicant");
            mypdfpage.Add(ptc);
            left1 = 440;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the guardian");
            mypdfpage.Add(ptc);
            left1 = 40;
            coltop += 15;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " NOTE : SALAH (NAMAZ) shall be compulsory for all muslim Boarders");
            mypdfpage.Add(ptc);
            mypdfpage.SaveToDocument();
            //second page
            mypdfpage = mydocument.NewPage();
            PdfArea P2 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
            PdfRectangle P2R = new PdfRectangle(mydocument, P2, Color.Black);
            mypdfpage.Add(P2R);
            coltop = 20;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " DECLARATION ");
            mypdfpage.Add(ptc);
            left1 = 80;
            coltop += 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " I ............................................................................................. S/o...............................................................................................");
            mypdfpage.Add(ptc);
            left1 = 40;
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " of ............................................................................................. Class if admited in the New College Hostel, hereby undertake to");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " observe the following rules and regulations : ");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 80;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " I shall abide by any disciplinary action which may be taken by the Hostel authority. The decision of the hostel authority in ");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "this respect will be  final.");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 1.  I will not entertain guest in my room.");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 2.  I will pay Mess advance along with Residential Fee promptly.");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 3.  I will not stay out during nights without permission of the Deputy Warden / Resident  Superintendents.");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 4.  I will strictly observe the Study hours and Mess timings.");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 5.  I will behave will in the Mess hall and not damage the hostel properties.");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 6.  I will not participate in any strike or demostration under any circumstances.");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 7.  I will not demand for election in the hostel.");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 8.  I will abide by all rules and regulations of the hostel, introduced / modified from time to time.");
            mypdfpage.Add(ptc);
            coltop += 40; left1 = 40;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Place :");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 40;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   :");
            mypdfpage.Add(ptc);
            left1 = 420;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the applicant");
            mypdfpage.Add(ptc);
            coltop += 60; left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "I Mr/Mrs........................................................................................................................................................do hereby  guarantee that");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "my son / ward Mr ........................................................................................ if given admission in the New College Hostel  will not take ");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "part in any activities prejudical to the interest of the Hostel. I assure for his good behaviour and conduct  during his stay in the Hostel. ");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "I agree  to pay all the dues of my son / ward. If he contravences the guarantee, my son / ward shall abide by any disciplinary action");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "which may be taken by Hostel authority. The desicion of the Hostel authority in this respect will be final. ");
            mypdfpage.Add(ptc);
            coltop += 40; left1 = 40;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Place :");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 40;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   :");
            mypdfpage.Add(ptc);
            left1 = 220;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent");
            mypdfpage.Add(ptc);
            left1 = 420;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Guardian");
            mypdfpage.Add(ptc);
            left1 = 220; coltop += 20;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "FOR OFFICIAL USE ONLY ");
            mypdfpage.Add(ptc);
            left1 = 40; coltop += 40;
            PdfArea officeuse = new PdfArea(mydocument, left1, coltop, 500, 150);
            PdfRectangle officeuser = new PdfRectangle(mydocument, officeuse, Color.Black);
            mypdfpage.Add(officeuser);
            //line
            left1 = 140; coltop += 0;
            //PdfArea line = new PdfArea(mydocument, 140, coltop, 500, 150);
            PdfLine liner = new PdfLine(mydocument, new Point(left1, coltop), new Point(left1, coltop + 150), Color.Black, 1);
            mypdfpage.Add(liner);
            left1 = 210;
            // PdfArea line1 = new PdfArea(mydocument, 140, coltop, 500, 150);
            PdfLine liner1 = new PdfLine(mydocument, new Point(left1, coltop), new Point(left1, coltop + 150), Color.Black, 1);
            mypdfpage.Add(liner1);
            left1 = 310;
            PdfLine liner2 = new PdfLine(mydocument, new Point(left1, coltop), new Point(left1, coltop + 150), Color.Black, 1);
            mypdfpage.Add(liner2);
            //coltop -= 40;
            coltop -= 10; left1 = 80;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Details ");
            mypdfpage.Add(ptc);
            left1 = 150;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Amount ");
            mypdfpage.Add(ptc);
            left1 = 220;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, " Date of Payment ");
            mypdfpage.Add(ptc);
            coltop += 10; left1 = 40;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 520, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________________________________________________________________");
            mypdfpage.Add(ptc);
            left1 = 420; coltop += 20;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 150, 50), System.Drawing.ContentAlignment.MiddleLeft, " Date : ______________");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Caution Deposit              Rs : ");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Hostel Fees                    Rs : ");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mess Advance                Rs : ");
            mypdfpage.Add(ptc);
            coltop += 20;
            left1 = 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Initial of Clerk");
            mypdfpage.Add(ptc);
            coltop -= 40;
            left1 = 360;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Admitted in Room No");
            mypdfpage.Add(ptc);
            coltop += 60;
            left1 = 400;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Deputy Warden");
            mypdfpage.Add(ptc);
            mypdfpage.SaveToDocument();
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
                Response.End();
            }
            else
            { }
        }
        catch { }
    }
    public void studentdetailspdf()
    {
        try
        {
            string value = ""; FpSpread3.SaveChanges(); bool nothingselect = false;
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            for (int i = 1; i < FpSpread3.Sheets[0].RowCount; i++)
            {
                value = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 1].Value);
                if (value == "1")
                {
                    mypdfpage = mydocument.NewPage();
                    Font header = new Font("Arial", 15, FontStyle.Bold);
                    Font header1 = new Font("Arial", 14, FontStyle.Bold);
                    Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
                    Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
                    Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
                    Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
                    Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
                    Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
                    Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
                    Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
                    Font Fontsmall1 = new Font("Arial", 11, FontStyle.Regular);
                    int left1 = 1;
                    int left2 = 225;
                    int left4 = 470;
                    #region college details
                    string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
                    DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
                    string university = "";
                    string collname = "";
                    string address1 = "";
                    string address2 = "";
                    string address3 = "";
                    string pincode = "";
                    string affliated = ""; string category = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        collname = ds.Tables[0].Rows[0]["collname"].ToString();
                        address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                        address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                        address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                        pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                        affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        category = ds.Tables[0].Rows[0]["category"].ToString();
                    }
                    #endregion
                    string[] split = collname.Split('(');
                    #region Header Logo
                    //**************right photo**************
                    PdfArea pa4 = new PdfArea(mydocument, 459, 40, 120, 120);
                    PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                    mypdfpage.Add(pr4);
                    //**************left logo**************
                    PdfArea collogoA = new PdfArea(mydocument, 14, 40, 120, 120);
                    PdfRectangle collogoR = new PdfRectangle(mydocument, collogoA, Color.Black);
                    mypdfpage.Add(collogoR);
                    //**************1st header**************
                    PdfArea pa5 = new PdfArea(mydocument, 140, 100, 310, 60);
                    PdfRectangle pr5 = new PdfRectangle(mydocument, pa5, Color.Black);
                    mypdfpage.Add(pr5);
                    //*************page**************//
                    PdfArea pa1 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                    PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                    mypdfpage.Add(pr3);
                    #endregion
                    string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(i), 0].Tag);
                    //CONVERT(varchar(10), dob,103) as 
                    string query = "   select h.Serial_No, r.reg_no,a.parentF_Mobile,a.guardian_name,r.roll_no, rd.Room_Name,dt.Dept_Name, c.Course_Name, app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,r.stud_name,sex,Relationship,parent_name,dob , parent_occu,mother_tongue, religion, citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy, first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,r.degree_code,r.batch_year,r.college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,r.current_semester,ncccadet from applyn a,Registration r,degree d,Department dt,Course C,ht_hostelregistration h ,Room_Detail rd where h.RoomFK=rd.Roompk and h.app_no=a.app_no and h.app_no=r.app_no and a.app_no=r.App_No and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no='" + app_no + "' and (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0) ";
                    query = query + " select photo from StdPhoto where  app_no='" + app_no + "' ";
                    query = query + " select course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + app_no + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(query, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //page 1
                        int coltop = 23;
                        string padd2 = Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]);
                        string padd3 = Convert.ToString(ds.Tables[0].Rows[0]["cityp"]);
                        string padd4 = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]));
                        string pin = Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]);
                        string ppho = Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]);
                        string address = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]);
                        string pmoobile = Convert.ToString(ds.Tables[0].Rows[0]["parentF_Mobile"]);
                        string[] add = address.Split(',');
                        if (Convert.ToString(ViewState["HostelForm"]) == "admission")
                        {
                            #region hostel application from
                            coltop = 23;
                            PdfTextArea ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Serial No: " + Convert.ToString(ds.Tables[0].Rows[0]["Serial_No"]) + "");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 460, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No: " + Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]) + "");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]) + " (HOSTEL)");
                            mypdfpage.Add(ptc);
                            //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                            //                                                 new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("(HOSTEL)"));
                            //mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, -22, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address3 + " - " + pincode + ".  INDIA");//address1 + " , " + address2 + " , " +
                            mypdfpage.Add(ptc);
                            coltop = coltop + 35;
                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                        new PdfArea(mydocument, -5, coltop - 20, 600, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                            //mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "APPLICATION FOR ADMISSION");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(System.DateTime.Now.ToString("yyyy")) + " - " + (Convert.ToInt32(System.DateTime.Now.ToString("yyyy")) + 1));
                            mypdfpage.Add(ptc);
                            #region  photo
                            string imgPhoto = string.Empty;
                            byte[] photoid = new byte[0];
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (ds.Tables[1].Rows[0][0] != null && Convert.ToString(ds.Tables[1].Rows[0][0]) != "")
                                {
                                    photoid = (byte[])(ds.Tables[1].Rows[0][0]);
                                }
                            }
                            string appformno = Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]);
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg")))
                            {
                                imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg");
                            }
                            else
                            {
                                try
                                {
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg")))
                                    {
                                        MemoryStream memoryStream = new MemoryStream();
                                        memoryStream.Write(photoid, 0, photoid.Length);
                                        if (photoid.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg");
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                                catch { }
                            }
                            if (imgPhoto.Trim() == string.Empty)
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, 65, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "RECENT");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left2, 75, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "PASSPORT SIZE");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left2, 85, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "PHOTOGRAPH");
                                mypdfpage.Add(ptc);
                            }
                            else
                            {
                                try
                                {
                                    PdfImage studimg = mydocument.NewImage(imgPhoto);
                                    mypdfpage.Add(studimg, 460, 44, 250);
                                }
                                catch { }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                                mypdfpage.Add(LogoImage, 17, 44, 250);
                            }
                            #endregion
                            left1 = 40;
                            coltop += 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "Note : (i)   Read the Hostel Prospectus carefully before filling up the application .");
                            coltop += 20;
                            left1 = 65;
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " (ii)  Students are advised to satisfy themselves about the facilities avaliable in the Hostel .");
                            coltop += 20; mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " (iii) Change of Address, if any, should be immediately intimated to this office .");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "To");
                            mypdfpage.Add(ptc);
                            coltop += 30;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "THE DEPUTY WARDEN,");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(split[0]).ToUpper() + " HOSTEL ," + address3.ToUpper() + " - " + pincode);
                            mypdfpage.Add(ptc);
                            left1 += 90;
                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                                 new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(" HOSTEL ," + address3.ToUpper() + " - " + pincode + ""));
                            //mypdfpage.Add(ptc);
                            coltop += 30; left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "Sir,");
                            mypdfpage.Add(ptc);
                            left1 = 65; coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " I request you to provide accommodation in the Hostel for the academic year " + Convert.ToString(System.DateTime.Now.ToString("yyyy")) + " - " + (Convert.ToInt32(System.DateTime.Now.ToString("yyyy")) + 1) + " . I have gone through the Rules and ");
                            mypdfpage.Add(ptc);
                            left1 = 40; coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 500, 50), System.Drawing.ContentAlignment.MiddleLeft, " Regulations of the hostel and assure you that I will abide by them.");
                            mypdfpage.Add(ptc);
                            left1 = 40; coltop += 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Name of Applicant ( In CAPITAL)          " + Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]) + "");
                            mypdfpage.Add(ptc);
                            PdfArea nameA = new PdfArea(mydocument, left1 + 170, coltop + 13, 360, 20);
                            PdfRectangle nameR = new PdfRectangle(mydocument, nameA, Color.Black);
                            mypdfpage.Add(nameR);
                            left1 = 60; coltop += 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Year / Class ");
                            mypdfpage.Add(ptc);
                            left1 = 43;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop + 20, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]) + "/" + Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
                            mypdfpage.Add(ptc);
                            coltop += 35;
                            left1 = 40;
                            PdfArea yearclassA = new PdfArea(mydocument, left1, coltop, 100, 20);
                            PdfRectangle yearclassR = new PdfRectangle(mydocument, yearclassA, Color.Black);
                            mypdfpage.Add(yearclassR);
                            left1 = 200; coltop -= 35;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Major ");
                            mypdfpage.Add(ptc);
                            coltop += 35;
                            left1 = 150;
                            PdfArea majorA = new PdfArea(mydocument, left1, coltop, 130, 20);
                            PdfRectangle majorR = new PdfRectangle(mydocument, majorA, Color.Black);
                            mypdfpage.Add(majorR);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 16, 130, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]));
                            mypdfpage.Add(ptc);
                            //ADVANCED ZOOLOGY & BIOTECHNOLOGY"
                            left1 = 310; coltop -= 35;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Roll No ");
                            mypdfpage.Add(ptc);
                            left1 = 300;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop + 20, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
                            mypdfpage.Add(ptc);
                            coltop += 35;
                            left1 = 283;
                            PdfArea rollnoA = new PdfArea(mydocument, left1, coltop, 100, 20);
                            PdfRectangle rollnoR = new PdfRectangle(mydocument, rollnoA, Color.Black);
                            mypdfpage.Add(rollnoR);
                            //DOB
                            left1 = 400; coltop -= 35;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Date of Birth ");
                            mypdfpage.Add(ptc);
                            coltop += 35;
                            left1 -= 0;
                            PdfArea dobd = new PdfArea(mydocument, left1, coltop, 60, 20);
                            PdfRectangle dobrd = new PdfRectangle(mydocument, dobd, Color.Black);
                            mypdfpage.Add(dobrd);
                            //left1 += 20;
                            //PdfArea dobd1 = new PdfArea(mydocument, left1, coltop, 40, 20);
                            //PdfRectangle dobRd1 = new PdfRectangle(mydocument, dobd1, Color.Black);
                            //mypdfpage.Add(dobRd1);
                            string dob = Convert.ToString(ds.Tables[0].Rows[0]["dob"]);
                            DateTime dobdt = new DateTime();
                            try
                            {
                                DateTime.TryParse(dob, out dobdt);
                            }
                            catch { }
                            left1 += 5;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 15, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, dobdt.ToString("dd/MM/yyyy"));
                            mypdfpage.Add(ptc);
                            //Month
                            //left1 += 25;
                            //PdfArea dobm = new PdfArea(mydocument, left1, coltop, 20, 20);
                            //PdfRectangle dobrm = new PdfRectangle(mydocument, dobm, Color.Black);
                            //mypdfpage.Add(dobrm);
                            //left1 += 20;
                            //PdfArea dobmm = new PdfArea(mydocument, left1, coltop, 20, 20);
                            //PdfRectangle dobRdmm = new PdfRectangle(mydocument, dobmm, Color.Black);
                            //mypdfpage.Add(dobRdmm);
                            //year
                            //left1 += 25;
                            //PdfArea doby = new PdfArea(mydocument, left1, coltop, 20, 20);
                            //PdfRectangle dobry = new PdfRectangle(mydocument, doby, Color.Black);
                            //mypdfpage.Add(dobry);
                            //left1 += 20;
                            //PdfArea dobmy = new PdfArea(mydocument, left1, coltop, 20, 20);
                            //PdfRectangle dobRdmmy = new PdfRectangle(mydocument, dobmy, Color.Black);
                            //mypdfpage.Add(dobRdmmy);
                            //left1 += 20;
                            //PdfArea doby1 = new PdfArea(mydocument, left1, coltop, 20, 20);
                            //PdfRectangle dobry1 = new PdfRectangle(mydocument, doby1, Color.Black);
                            //mypdfpage.Add(dobry1);
                            //left1 += 20;
                            //PdfArea dobmy1 = new PdfArea(mydocument, left1, coltop, 20, 20);
                            //PdfRectangle dobRdmmy1 = new PdfRectangle(mydocument, dobmy1, Color.Black);
                            //mypdfpage.Add(dobRdmmy1);
                            //age
                            left1 += 100; coltop -= 35;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Age ");
                            mypdfpage.Add(ptc);
                            left1 += 0;
                            coltop += 35;
                            PdfArea age = new PdfArea(mydocument, left1, coltop, 27, 20);
                            PdfRectangle ager = new PdfRectangle(mydocument, age, Color.Black);
                            mypdfpage.Add(ager);
                            int cur = 0; int org = 0;
                            int.TryParse(System.DateTime.Now.ToString("yyyy"), out cur);
                            int.TryParse(dobdt.ToString("yyyy"), out org);
                            string curage = Convert.ToString(cur - org);
                            left1 += 5;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 15, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, curage);
                            mypdfpage.Add(ptc);
                            //second row
                            left1 = 60; coltop += 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Nationality ");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 40;
                            PdfArea nationa = new PdfArea(mydocument, left1, coltop, 100, 20);
                            PdfRectangle nationr = new PdfRectangle(mydocument, nationa, Color.Black);
                            mypdfpage.Add(nationr);
                            string nationality = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["citizen"]));
                            left1 += 5;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 15, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, nationality);
                            mypdfpage.Add(ptc);
                            left1 = 180; coltop += -40;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Religion ");
                            mypdfpage.Add(ptc);
                            string relig = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["religion"]));
                            left1 -= 30;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop + 25, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, relig);
                            mypdfpage.Add(ptc);
                            coltop += 40;
                            left1 = 150;
                            PdfArea religiona = new PdfArea(mydocument, left1, coltop, 100, 20);
                            PdfRectangle religionr = new PdfRectangle(mydocument, religiona, Color.Black);
                            mypdfpage.Add(religionr);
                            left1 = 290; coltop += -45;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Community ");
                            mypdfpage.Add(ptc);
                            left1 = 270; coltop += 10;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " FC / BC / MBC / ST / SC ");
                            mypdfpage.Add(ptc);
                            coltop += 35;
                            PdfArea com = new PdfArea(mydocument, left1, coltop, 100, 20);
                            PdfRectangle comr = new PdfRectangle(mydocument, com, Color.Black);
                            mypdfpage.Add(comr);
                            string communitity = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["community"]));
                            //left1 -= 30;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 16, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, communitity);
                            mypdfpage.Add(ptc);
                            left1 = 440; coltop += -45;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Shift ");
                            mypdfpage.Add(ptc);
                            left1 = 420; coltop += 10;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " I / II / Fulltime ");
                            mypdfpage.Add(ptc);
                            coltop += 35; left1 = 410;
                            PdfArea shift = new PdfArea(mydocument, left1, coltop, 100, 20);
                            PdfRectangle shiftr = new PdfRectangle(mydocument, shift, Color.Black);
                            mypdfpage.Add(shiftr);
                            string SHIFT = "";
                            if (Convert.ToString(ddl_collegename.SelectedItem.Value) == "13")
                            {
                                SHIFT = "SHIFT - I";
                            }
                            if (Convert.ToString(ddl_collegename.SelectedItem.Value) == "14")
                            {
                                SHIFT = "SHIFT - II";
                            }
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 16, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, SHIFT);
                            mypdfpage.Add(ptc);
                            coltop += 30; left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " Whether stayed in this hostel or any other hostel before ?");
                            mypdfpage.Add(ptc);
                            left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "If yes, state the name of the hostel, place & Room No");
                            mypdfpage.Add(ptc);
                            left1 = 410;
                            PdfArea prehostel = new PdfArea(mydocument, left1, coltop, 160, 18);
                            PdfRectangle prehostelr = new PdfRectangle(mydocument, prehostel, Color.Black);
                            mypdfpage.Add(prehostelr);
                            coltop += 18;
                            PdfArea preroom = new PdfArea(mydocument, left1, coltop, 160, 18);
                            PdfRectangle preroomr = new PdfRectangle(mydocument, preroom, Color.Black);
                            mypdfpage.Add(preroomr);
                            coltop += 10; left1 = 40;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " Particulars of Parent / Guardian ");
                            mypdfpage.Add(ptc);
                            coltop += 30;
                            //parent table
                            PdfArea parent = new PdfArea(mydocument, left1, coltop, 520, 195);
                            PdfRectangle parentr = new PdfRectangle(mydocument, parent, Color.Black);
                            mypdfpage.Add(parentr);
                            left1 = 290;
                            PdfLine liner3 = new PdfLine(mydocument, new Point(left1, coltop), new Point(left1, coltop + 195), Color.Black, 1);
                            mypdfpage.Add(liner3);
                            coltop -= 10; left1 = 140;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Parent ");
                            mypdfpage.Add(ptc);
                            left1 = 380;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Local Guardian ");
                            mypdfpage.Add(ptc);
                            coltop += 15; left1 = 40;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, " NAME: " + Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]));
                            mypdfpage.Add(ptc);
                            left1 = 300;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, " NAME: " + Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]));
                            mypdfpage.Add(ptc);
                            coltop += 10; left1 = 40;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 520, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            left1 = 40;
                            try
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Door No          : " + add[0].ToString());
                                mypdfpage.Add(ptc);
                                //left1 = 140;
                                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Old  : ");
                                //mypdfpage.Add(ptc);
                                //left1 = 220;
                                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " New  : ");
                                //mypdfpage.Add(ptc);
                                left1 = 300;
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Door No          : ");
                                mypdfpage.Add(ptc);
                                //left1 = 400;
                                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Old  : ");
                                //mypdfpage.Add(ptc);
                                //left1 = 480;
                                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " New  : ");
                                //mypdfpage.Add(ptc);
                            }
                            catch { }
                            coltop += 20;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 250, 50), System.Drawing.ContentAlignment.MiddleLeft, " Road / Street  : " + padd2);
                            mypdfpage.Add(ptc); left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 250, 50), System.Drawing.ContentAlignment.MiddleLeft, " Road / Street  : ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Town / City     : " + padd3);
                            mypdfpage.Add(ptc); left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Town / City     : ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " District/ State  : " + padd4);
                            mypdfpage.Add(ptc); left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " District/ State  : ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Pincode          : " + pin);
                            mypdfpage.Add(ptc); left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Pincode          : ");
                            mypdfpage.Add(ptc);
                            coltop += 15;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Code / No       : ");
                            mypdfpage.Add(ptc); left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Code / No       : ");
                            mypdfpage.Add(ptc);
                            coltop += 15;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Phone             : " + ppho);
                            mypdfpage.Add(ptc); left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Phone             : ");
                            mypdfpage.Add(ptc);
                            coltop += 15;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mobile             : " + pmoobile);
                            mypdfpage.Add(ptc); left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mobile             : ");
                            mypdfpage.Add(ptc);
                            //sign
                            left1 = 40; coltop += 60;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Parent");
                            mypdfpage.Add(ptc);
                            left1 = 250;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Applicant");
                            mypdfpage.Add(ptc);
                            left1 = 440;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Guardian");
                            mypdfpage.Add(ptc);
                            left1 = 40;
                            coltop += 15;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " NOTE : SALAH (NAMAZ) shall be compulsory for all Muslim Boarders");
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            #endregion
                            //page 2
                            #region declaration form1
                            mypdfpage = mydocument.NewPage();
                            PdfArea P2 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                            PdfRectangle P2R = new PdfRectangle(mydocument, P2, Color.Black);
                            mypdfpage.Add(P2R);
                            coltop = 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " DECLARATION ");
                            mypdfpage.Add(ptc);
                            left1 = 80;
                            coltop += 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " I ............................................................................................. S/o...............................................................................................");
                            mypdfpage.Add(ptc);
                            left1 = 90;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 5, 210, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]) + Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]));
                            mypdfpage.Add(ptc);
                            left1 = 340;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 5, 210, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypdfpage.Add(ptc);
                            left1 = 40;
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " of ............................................................................................. Class if admited in the New College Hostel, hereby undertake to");
                            mypdfpage.Add(ptc);
                            left1 = 70;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 5, 210, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]) + " / " + Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
                            mypdfpage.Add(ptc);
                            //+ " nsd sdf sdf sdf sdf sdfs dfsd f sdf sdf dsf dsf sdf sd fsd f sdf sd fs df sd fsdf "
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " observe the following rules and regulations : ");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 80;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " I shall abide by any disciplinary action which may be taken by the Hostel authority. The decision of the hostel authority in ");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "this respect will be  final.");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 1.  I will not entertain guest in my room.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 2.  I will pay Mess advance along with Residential Fee promptly.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 3.  I will not stay out during nights without permission of the Deputy Warden / Resident  Superintendents.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 4.  I will strictly observe the Study hours and Mess timings.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 5.  I will behave well in the Mess hall and will not damage the hostel properties.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 6.  I will not participate in any strike or demostration under any circumstances.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 7.  I will not demand for election in the hostel.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " 8.  I will abide by all rules and regulations of the hostel, introduced / modified from time to time.");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 40;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Place :");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   :");
                            mypdfpage.Add(ptc);
                            left1 = 420;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the applicant");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "I Mr/Mrs........................................................................................................................................................do hereby  guarantee that");
                            mypdfpage.Add(ptc);
                            left1 = 80;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 5, 350, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]) + Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]));
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "my son / ward Mr ........................................................................................ if given admission in the New College Hostel  will not take ");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 80, coltop - 5, 200, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "part in any activities prejudical to the interest of the Hostel. I assure for his good behaviour and conduct  during his stay in the Hostel. ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "I agree  to pay all the dues of my son / ward. If he contravences the guarantee, my son / ward shall abide by any disciplinary action");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "which may be taken by Hostel authority. The desicion of the Hostel authority in this respect will be final. ");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 40;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Place :");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   :");
                            mypdfpage.Add(ptc);
                            left1 = 220;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent");
                            mypdfpage.Add(ptc);
                            left1 = 420;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Guardian");
                            mypdfpage.Add(ptc);
                            left1 = 220; coltop += 20;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "FOR OFFICIAL USE ONLY ");
                            mypdfpage.Add(ptc);
                            left1 = 40; coltop += 40;
                            PdfArea officeuse = new PdfArea(mydocument, left1, coltop, 500, 150);
                            PdfRectangle officeuser = new PdfRectangle(mydocument, officeuse, Color.Black);
                            mypdfpage.Add(officeuser);
                            //line
                            left1 = 140; coltop += 0;
                            //PdfArea line = new PdfArea(mydocument, 140, coltop, 500, 150);
                            PdfLine liner = new PdfLine(mydocument, new Point(left1, coltop), new Point(left1, coltop + 150), Color.Black, 1);
                            mypdfpage.Add(liner);
                            left1 = 210;
                            // PdfArea line1 = new PdfArea(mydocument, 140, coltop, 500, 150);
                            PdfLine liner1 = new PdfLine(mydocument, new Point(left1, coltop), new Point(left1, coltop + 150), Color.Black, 1);
                            mypdfpage.Add(liner1);
                            left1 = 310;
                            PdfLine liner2 = new PdfLine(mydocument, new Point(left1, coltop), new Point(left1, coltop + 150), Color.Black, 1);
                            mypdfpage.Add(liner2);
                            //coltop -= 40;
                            coltop -= 10; left1 = 80;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Details ");
                            mypdfpage.Add(ptc);
                            left1 = 150;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Amount ");
                            mypdfpage.Add(ptc);
                            left1 = 220;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, " Date of Payment ");
                            mypdfpage.Add(ptc);
                            coltop += 10; left1 = 40;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 520, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________________________________________________________________");
                            mypdfpage.Add(ptc);
                            left1 = 420; coltop += 20;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 150, 50), System.Drawing.ContentAlignment.MiddleLeft, " Date : ______________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Caution Deposit              Rs : ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Hostel Fees                    Rs : ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mess Advance                Rs : ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Initial of Clerk");
                            mypdfpage.Add(ptc);
                            coltop -= 40;
                            left1 = 360;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Admitted in Room No:" + Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]));
                            mypdfpage.Add(ptc);
                            coltop += 60;
                            left1 = 400;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Deputy Warden");
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            #endregion
                        }
                        if (Convert.ToString(ViewState["HostelForm"]) == "undertaking")
                        {
                            //page 4
                            #region undertaking form1
                            mypdfpage = mydocument.NewPage();
                            PdfArea P3 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                            PdfRectangle P3R = new PdfRectangle(mydocument, P3, Color.Black);
                            mypdfpage.Add(P3R);
                            coltop = 10;
                            coltop = coltop + 10;
                            PdfTextArea ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]) + "HOSTEL");
                            mypdfpage.Add(ptc);
                            //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                            //                                                 new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("(HOSTEL)"));
                            //mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, -22, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address3 + " - " + pincode + ".  INDIA");//address1 + " , " + address2 + " , " + 
                            mypdfpage.Add(ptc);
                            coltop = coltop + 35;
                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                        new PdfArea(mydocument, -5, coltop - 20, 600, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                            //mypdfpage.Add(ptc);
                            left1 = 65;
                            coltop += 20;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, "NAME                       :" + Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, "ROOM NO                :" + Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]));
                            coltop += 20; mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "REG NO / ROLLNO :" + Convert.ToString(ds.Tables[0].Rows[0]["reg_no"]) + " / " + Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
                            mypdfpage.Add(ptc);
                            coltop += 20; mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "COURSE                  :" + Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " UNDERTAKING BY THE BOARDER ");
                            mypdfpage.Add(ptc);
                            coltop += 30; mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "Abide by the following regulations:");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "1. I will not use cell phone during the study hours.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "2. I will use computer / laptop after obtaining  written permission from the Deputy Warden and will pay the prescribed fees. ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "    I will use Laptop / Tablet only for study purpose.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "3. I will Maintain utmost dicipline and decorum in the hostel");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "4. I will not indulge in any ragging activities, in any form.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "5. I assure you that I will obey the hostel rules and regulations strictly.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "6. If I misbehave or violate hostel rules or indulge in ragging of any sort. I may be expelled from the hostel without any enquiry.");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "7. I, the undersigned abide by the above rules.");
                            mypdfpage.Add(ptc);
                            coltop += 60;
                            left1 = 400;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the boarder");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 40;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " UNDERTAKING BY THE PARENT / GUARDIAN ");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 80;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 565, 50), System.Drawing.ContentAlignment.MiddleLeft, " I am thankful to the hostel authorities for considering my son / ward ..........................................................................................");
                            mypdfpage.Add(ptc);
                            left1 = 360;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop - 5, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 565, 50), System.Drawing.ContentAlignment.MiddleLeft, " ........................................................ Reg No / Roll No....................................... for the admission to The New College Hostel.");
                            mypdfpage.Add(ptc);
                            left1 = 300;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop - 5, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " I assure you that my son / Ward will obey the Hostel rules and regulations strickly. If  he misbehaves or violates hostel rules or ");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, " indulges in ragging he may be expelled from the hostel without any enquriry.");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, "' I, the undersigned abide by the above Rules");
                            mypdfpage.Add(ptc);
                            coltop += 60;
                            left1 = 400;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the parent / guardian");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 360;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 220, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name        : " + Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]) + Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]));
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 220, 50), System.Drawing.ContentAlignment.MiddleLeft, "Address     : " + address);
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 220, 50), System.Drawing.ContentAlignment.MiddleLeft, "                    " + padd3 + ", " + padd4 + ", " + pin);
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 220, 50), System.Drawing.ContentAlignment.MiddleLeft, "Ph/ Cell No: " + ppho + "/ " + pmoobile);
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            #endregion
                            #region undertaking form2
                            //Page 6 By Jeyaprakash
                            mypdfpage = mydocument.NewPage();
                            PdfArea pa8 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                            PdfRectangle pr8 = new PdfRectangle(mydocument, pa8, Color.Black);
                            mypdfpage.Add(pr8);
                            coltop = 0;
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_logo(" + ddl_collegename.SelectedItem.Value + ").jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_logo(" + ddl_collegename.SelectedItem.Value + ").jpeg"));
                                mypdfpage.Add(LogoImage, 27, 50, 400);
                            }
                            coltop = coltop + 35;
                            ptc = new PdfTextArea(header, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, collname.Split('(')[0] + " HOSTEL");
                            mypdfpage.Add(ptc);
                            //coltop = coltop + 20;
                            //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, collname.Split('(')[0] + " (" + Convert.ToString(category) + ")");
                            //mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, address2 + "," + address3 + " - " + pincode);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 60;
                            PdfLine pdfHorizandalLine = new PdfLine(mydocument, new Point(27, coltop), new Point(Convert.ToInt32(mydocument.PageWidth - 30), coltop), Color.Black, 2);
                            mypdfpage.Add(pdfHorizandalLine);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(header, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleCenter, "UNDERTAKING");
                            mypdfpage.Add(ptc);
                            int lineno = 27;
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "1.     I will fully abide by the rules and regulations of the hostel administration.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "2.     I will not involve in any form of ragging and in disciplinary activities.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "3.     I will maintain discipline and healthy atmosphere in the campus.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "4.     I will not entertain day scholars inside the hostel premises.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "5.     I will be inside the premises before 9.30 pm.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "6.     I will keep the hostel premises neat and hygienic.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "7.     I won't write anything on the walls and damage any hostel property.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "8.     I won't play in the hostel rooms and corridors.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "9.     If I am found using banned substances (Narcotic drugs,tobacco and Alcoholic drinks).");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "        I may be expelled from the hostel without any enquiry.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "10.   In case of going out of my room , I will switch off the electrical appliances and");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "        Lock the door.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "11.   I will be responsible for all my belonging; hostel authorities are not responsible if I lose");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "        any of my belongings.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "12.   I will be in my room during the study hours and not use any communication Devices.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "13.   When I am going on leave I will get Prior Permission from the hostel authorities.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "14.   I will Perform 5 times prayer daily with jamath and I will attend the Quran Classes");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "        regularly organized by hostel authorities.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "15.   I Will not use smart phone (Use only basic model) in the hostel premises.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "        I will abide by the above rules, If I violate the above mentioned, I may be expelled from the hostel");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "        immediately.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 100;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent / Guardian");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno + 400, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Student");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "(With name in capital letters)");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black, new PdfArea(mydocument, lineno + 400, coltop, mydocument.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, "(With name in capital letters)");
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            #endregion
                        }
                        if (Convert.ToString(ViewState["HostelForm"]) == "withdrawal")
                        {
                            //page 5
                            #region With draw from 1
                            mypdfpage = mydocument.NewPage();
                            PdfArea P4 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                            PdfRectangle P4R = new PdfRectangle(mydocument, P4, Color.Black);
                            mypdfpage.Add(P4R);
                            coltop = 20;
                            PdfTextArea ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, -40, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]));
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("(HOSTEL)"));
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, -22, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 35;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, -5, coltop - 20, 600, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " Withdrawal Form ");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                            mypdfpage.Add(ptc);
                            left1 = 65; coltop += 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Boarder _________________________________________________________________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 100, coltop - 5, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class ________________________________ Room No _________________ Roll No _____________________ ");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 40, coltop - 5, 150, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]) + " / " + Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 240, coltop - 5, 70, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["room_name"]));
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 5, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Purpose of vacating Room _____________________________________________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date & Time of vacating   ______________________________________________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
                            mypdfpage.Add(ptc);
                            left1 = 50; coltop += 40;
                            PdfArea br = new PdfArea(mydocument, left1, coltop, 500, 180);// 14, 12, 560, 825);
                            PdfRectangle brr = new PdfRectangle(mydocument, br, Color.Black);
                            mypdfpage.Add(brr);
                            coltop -= 15; left1 = 60;
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Clearance must be obtained for the following particulars Breakage Report");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " a. Cot ");
                            mypdfpage.Add(ptc);
                            PdfArea a = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                            PdfRectangle ar = new PdfRectangle(mydocument, a, Color.Black);
                            mypdfpage.Add(ar);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 300, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " g. miscellaneous ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " b. Table ");
                            mypdfpage.Add(ptc);
                            PdfArea b = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                            PdfRectangle bbr = new PdfRectangle(mydocument, b, Color.Black);
                            mypdfpage.Add(bbr);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " c. chair ");
                            mypdfpage.Add(ptc);
                            PdfArea c = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                            PdfRectangle cr = new PdfRectangle(mydocument, c, Color.Black);
                            mypdfpage.Add(cr);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " d. Cupboard ");
                            mypdfpage.Add(ptc);
                            PdfArea d = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                            PdfRectangle dr = new PdfRectangle(mydocument, d, Color.Black);
                            mypdfpage.Add(dr);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " e. Fans ");
                            mypdfpage.Add(ptc);
                            PdfArea ee = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                            PdfRectangle er = new PdfRectangle(mydocument, ee, Color.Black);
                            mypdfpage.Add(er);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " f. Lights ");
                            mypdfpage.Add(ptc);
                            PdfArea f = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                            PdfRectangle fr = new PdfRectangle(mydocument, f, Color.Black);
                            mypdfpage.Add(fr);
                            coltop += 13; left1 = 50;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________________________________________________________________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 10;
                            left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " h. Sports Items ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " i. Identity Card ");
                            mypdfpage.Add(ptc);
                            coltop -= 50; left1 = 250;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Room Boy");
                            mypdfpage.Add(ptc);
                            left1 = 450;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "___________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Supervisor");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "___________________");
                            mypdfpage.Add(ptc);
                            coltop += 50; left1 = 400;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Store keeper");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 40;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " For Office use Only ");
                            mypdfpage.Add(ptc);
                            coltop += 30; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Deposit                       Rs _________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Less: Dues                 Rs _________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Balance Amount to be");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Refunded / Collected  Rs _________________");
                            mypdfpage.Add(ptc);
                            left1 = 400;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Office Clerk with Office Seal");
                            mypdfpage.Add(ptc);
                            coltop += 50; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Fees Dues if Any .............................................................................");
                            mypdfpage.Add(ptc);
                            left1 = 400;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Office Clerk with Office Seal");
                            mypdfpage.Add(ptc);
                            coltop += 50; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
                            mypdfpage.Add(ptc); left1 = 400;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 5; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Deputy Warden");
                            mypdfpage.Add(ptc);
                            coltop += 5; left1 = 400;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Principal & Warden");
                            mypdfpage.Add(ptc);
                            coltop += 25; left1 = 40;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 565, 50), System.Drawing.ContentAlignment.MiddleCenter, " Submitted to the Hony.Secretary & Correspondent for approval ");
                            mypdfpage.Add(ptc);
                            coltop += 15; left1 = 50;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________________________________________________________________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 10; left1 = 40;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " Voucher ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Received Cheque / Cash Rs ................................... Repuees .........................................................................................................");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " ...................................................................................................................................................................................................................");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Cheque No................................................................................ Date ...........................");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop + 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            #endregion
                            //page 6
                            #region withdraw form2
                            mypdfpage = mydocument.NewPage();
                            PdfArea P5 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                            PdfRectangle P5R = new PdfRectangle(mydocument, P5, Color.Black);
                            mypdfpage.Add(P5R);
                            coltop = 20;
                            ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, -40, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]));
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("(HOSTEL)"));
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, -22, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 35;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, -5, coltop - 20, 600, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 40;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " Withdrawal Form ");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                            mypdfpage.Add(ptc);
                            left1 = 65; coltop += 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Boarder _________________________________________________________________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 100, coltop - 5, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class ________________________________ Room No _________________ Roll No _____________________ ");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 40, coltop - 5, 150, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]) + " / " + Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 240, coltop - 5, 70, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["room_name"]));
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 5, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Purpose of vacating Room _____________________________________________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date & Time of vacating   ______________________________________________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 40;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
                            mypdfpage.Add(ptc);
                            left1 = 50; coltop += 40;
                            PdfArea m = new PdfArea(mydocument, left1, coltop, 500, 120);// 14, 12, 560, 825);
                            PdfRectangle mr = new PdfRectangle(mydocument, m, Color.Black);
                            mypdfpage.Add(mr);
                            coltop -= 0; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " No. of days Mess Bill : Rs ");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                            mypdfpage.Add(ptc);
                            coltop += 30; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Extra                           : Rs ");
                            mypdfpage.Add(ptc);
                            coltop += 10; left1 = 170;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                            mypdfpage.Add(ptc);
                            coltop += 15; left1 = 120;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Total Rs");
                            mypdfpage.Add(ptc);
                            coltop += 10; left1 = 170;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 170, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 170, coltop + 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Mess Contractor with seal");
                            mypdfpage.Add(ptc);
                            coltop += 50; left1 = 40;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " For Office use Only ");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mess Advance Amount  Rs ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Less Total Mess Bill       Rs ");
                            mypdfpage.Add(ptc);
                            coltop += 15;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Less Others                   Rs ");
                            mypdfpage.Add(ptc);
                            coltop += 15;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Due Refundable             Rs ");
                            mypdfpage.Add(ptc);
                            coltop += 15;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                            mypdfpage.Add(ptc);
                            coltop += 50;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "______________________________");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 340, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "___________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 5; left1 = 60;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Deputy Warden");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 340, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of Office Clerk with Office Seal");
                            mypdfpage.Add(ptc);
                            coltop += 40; left1 = 40;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 565, 50), System.Drawing.ContentAlignment.MiddleCenter, " Submitted to the Hony.Secretary & Correspondent for approval ");
                            mypdfpage.Add(ptc);
                            coltop += 20; left1 = 50;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________________________________________________________________________________________");
                            mypdfpage.Add(ptc);
                            coltop += 10; left1 = 40;
                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " Voucher ");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Received Cheque / Cash Rs ................................... Repuees .........................................................................................................");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " ...................................................................................................................................................................................................................");
                            mypdfpage.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Cheque No................................................................................ Date ...........................");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop + 60, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            #endregion
                        }
                        nothingselect = true;
                    }
                }
            }
            ViewState["HostelForm"] = null;
            if (nothingselect == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                    Response.End();
                }
                else
                { }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = " Please select the student";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void imagebtnpop3close_Click(object sender, EventArgs e)
    {
        popwindow3.Visible = false;
    }
    public void roomchecklist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (roomchecklist.Items[0].Selected == false)
            {
                chck1.Checked = false;
            }
            if (roomchecklist.Items[1].Selected == false)
            {
                chck1.Checked = false;
            }
            if (roomchecklist.Items[2].Selected == false)
            {
                chck1.Checked = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnroomdetails_Click(object sender, EventArgs e)
    {
        popwindow3.Visible = true;
    }
    public void btn_gopop3_Click(object sender, EventArgs e)
    {
        try
        {
            search();
        }
        catch { }
    }
    public void chck1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chck1.Checked == true)
            {
                for (int i = 0; i < roomchecklist.Items.Count; i++)
                {
                    roomchecklist.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < roomchecklist.Items.Count; i++)
                {
                    roomchecklist.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void search()
    {
        try
        {
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            toalrooms.Visible = true;
            totalvaccants.Visible = true;
            fill.Visible = true;
            partialfill.Visible = true;
            unfill.Visible = true;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            if (ddl_hostelname.Items.Count > 0 && ddl_building.Items.Count > 0 && ddl_floorname.Items.Count > 0 && ddl_roomtype.Items.Count > 0)
            {
                string hostelcode = Convert.ToString(ddl_hostelname.SelectedItem.Value);
                string building = Convert.ToString(ddl_building.SelectedItem.Text);
                string floor = Convert.ToString(ddl_floorname.SelectedItem.Text);
                string roomtype0 = Convert.ToString(ddl_roomtype.SelectedItem.Value);
                string vaccanttype = Convert.ToString(ddl_pop3vaccant.SelectedItem.Text);
                if (floor.Trim() != "" && roomtype0.Trim() != "")
                {
                    string bcode = d2.GetFunction(" select HostelBuildingFK  from HM_HostelMaster where HostelMasterPK ='" + hostelcode + "'");
                    string selectquery = " select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student,r.Building_Name,b.College_Code from Building_Master B,Room_Detail R where b.Building_Name =r.Building_Name and b.College_Code =r.College_Code and b.Code in (" + bcode + ")";
                    if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Filled")
                    {
                        selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
                    }
                    else if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Un Filled")
                    {
                        selectquery = selectquery + " AND R.Avl_Student = 0";
                    }
                    else if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Partially Filled")
                    {
                        selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
                    }
                    selectquery = selectquery + " Select Distinct F.Floor_Name+' - '+Room_Type RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name in ('" + building + "') AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY F.Floor_Name+' - '+Room_Type";
                    selectquery = selectquery + " select ISNULL(Room_Cost,0)as Room_Cost,Hostel_Code,Room_Type  from RoomCost_Master";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    int IntRoomLen = 0;
                    int totalunfill = 0;
                    int totalfill = 0;
                    int totalpartialfill = 0;
                    int totalvaccant = 0;
                    string strRoomDetail = "";
                    int colcnt = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowHeader.Visible = false;
                                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                colcnt = 0;
                                if (FpSpread1.Sheets[0].ColumnCount - 1 < colcnt)
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                }
                                string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
                                string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
                                string alldetails = floorname + "-" + roomtype;
                                FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                FpSpread1.Sheets[0].Columns[colcnt].CellType = textcel_type;
                                FpSpread1.Sheets[0].Cells[i, colcnt].Text = alldetails;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[i, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
                                DataView dv = new DataView();
                                ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    int columncount = dv.Count;
                                    for (int cnt = 0; cnt < dv.Count; cnt++)
                                    {
                                        colcnt++;
                                        FpSpread1.Sheets[0].Cells[i, cnt].Tag = Convert.ToString(dv[cnt]["Building_Name"]);
                                        string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]);
                                        DataView cost = new DataView(); string rmcost = "";
                                        if (ds.Tables[2].Rows.Count > 0)
                                        {
                                            for (int rmc = 0; rmc < ds.Tables[2].Rows.Count; rmc++)
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = " Hostel_Code='" + hostelcode + "' and Room_Type='" + roomtype + "'";
                                                cost = ds.Tables[2].DefaultView;
                                                if (cost.Count > 0)
                                                {
                                                    rmcost = Convert.ToString(cost[rmc]["Room_Cost"]);
                                                }
                                            }
                                        }
                                        if (rmcost.Trim() == "")
                                        {
                                            rmcost = "0";
                                        }
                                        s = s + rmcost;
                                        if (FpSpread1.Sheets[0].ColumnCount - 1 < colcnt)
                                        {
                                            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                            FpSpread1.Sheets[0].Columns[0].Locked = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Room Details";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        }
                                        if (chck1.Checked == true)
                                        {
                                            FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                            FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                totalfill = totalfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + rmcost.Length;
                                            totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                if (chck1.Checked == false)
                                                {
                                                    if (roomchecklist.Items[0].Selected == false && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
                                                    {
                                                        FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                            totalunfill = totalunfill + 1;
                                                        }
                                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                            totalpartialfill = totalpartialfill + 1;
                                                        }
                                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                            totalfill = totalfill + 1;
                                                        }
                                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                            totalpartialfill = totalpartialfill + 1;
                                                        }
                                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                            totalunfill = totalunfill + 1;
                                                        }
                                                        strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);
                                                        if (IntRoomLen < strRoomDetail.Length)
                                                        {
                                                            IntRoomLen = strRoomDetail.Length;
                                                        }
                                                        FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
                                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                    }
                                                }
                                                if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
                                                {
                                                    FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                    {
                                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                }
                                                else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == false)
                                                {
                                                    FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                    {
                                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                }
                                                else if (roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[0].Selected == false)
                                                {
                                                    FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                    {
                                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                }
                                                else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == false)
                                                {
                                                    FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                    {
                                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + rmcost;
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                }
                                                else if (roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == false && roomchecklist.Items[0].Selected == false)
                                                {
                                                    FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                    {
                                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                }
                                                else if (roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == false && roomchecklist.Items[0].Selected == false)
                                                {
                                                    FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                    {
                                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                }
                                                else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == true)
                                                {
                                                    chck1.Checked = true;
                                                    FpSpread1.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                    {
                                                        IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                }
                                                totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
                                        {
                                            totalvaccants.Text = " ";
                                            toalrooms.Text = " ";
                                            int totalroom = totalunfill + totalfill + totalpartialfill;
                                            toalrooms.Text = "Total No.of Rooms :" + totalroom;
                                            totalvaccants.Text = "Total No.of Vacant :" + totalvaccant;
                                            fill.Text = ("Filled(" + totalfill + ")");
                                            unfill.Text = ("UnFilled(" + totalunfill + ")");
                                            partialfill.Text = ("Partially Filled(" + totalpartialfill + ")");
                                        }
                                    }
                                    FpSpread1.SaveChanges();
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].ColumnCount;
                                    FpSpread1.Sheets[0].FrozenColumnCount = 1;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                }
                            }
                            FpSpread1.Visible = true;
                            tblStatus.Visible = true;
                            lblpop3err.Visible = false;
                            lblpop3err.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        tblStatus.Visible = false;
                        lblpop3err.Visible = true;
                        lblpop3err.Text = "No Records Found";
                    }
                }
                else
                {
                    tblStatus.Visible = false;
                    FpSpread1.Visible = false;
                    lblpop3err.Visible = true;
                    lblpop3err.Text = "No Records Found";
                }
            }
        }
        catch
        {
        }
    }
    public void loadtext()
    {
        try
        {
            Hashtable columnheadertxt = new Hashtable();
            columnheadertxt.Add("1", "Student Name-stud_name");
            columnheadertxt.Add("2", "DOB-dob");
            columnheadertxt.Add("3", "Application Date-date_applied");
            columnheadertxt.Add("4", "Address-parent_addressP");
            columnheadertxt.Add("5", "Mobile No-Student_Mobile");
            columnheadertxt.Add("6", "Email_Id-StuPer_Id");
            columnheadertxt.Add("7", "Alternative Course-Alternativedegree_code");
            columnheadertxt.Add("8", "Gender-sex");
            columnheadertxt.Add("9", "Parent Name-parent_name");
            columnheadertxt.Add("10", "Religion-religion");
            columnheadertxt.Add("11", "Community-community");
            columnheadertxt.Add("12", "Caste-caste");
            columnheadertxt.Add("13", "Nationality-citizen");
            columnheadertxt.Add("14", "Occupation-parent_occu");
            columnheadertxt.Add("15", "Remarks-remarks");
            columnheadertxt.Add("16", "Application ID-app_formno");
            columnheadertxt.Add("17", "Batch Year-Batch_Year");
            columnheadertxt.Add("18", "Course-Course_Name");
            columnheadertxt.Add("19", "Department-Dept_Name");
            columnheadertxt.Add("20", "Semester-Current_Semester");
            columnheadertxt.Add("21", "Institute Name-Institute_Name");
            columnheadertxt.Add("22", "Institute Address-instaddress");
            columnheadertxt.Add("23", "Pass Month-PassMonth");
            columnheadertxt.Add("24", "Pass Year-PassYear");
            columnheadertxt.Add("25", "Marks-securedmark");
            columnheadertxt.Add("26", "Total Percentage-percentage");
            columnheadertxt.Add("27", "State-parent_statep");
            columnheadertxt.Add("28", "Mother Tongue-mother_tongue");
            columnheadertxt.Add("29", "TANCET Mark-tancet_mark");
            columnheadertxt.Add("30", "Island-TamilOrginFromAndaman");
            columnheadertxt.Add("31", "Ex serviceman-IsExService");
            columnheadertxt.Add("32", "Differently abled-isdisable");
            columnheadertxt.Add("33", "First generation-first_graduate");
            columnheadertxt.Add("34", "Sports-DistinctSport");
            columnheadertxt.Add("35", "Co Curricular Activites-co_curricular");
            columnheadertxt.Add("36", "BankReferenceNo-ApplBankRefNumber");
            columnheadertxt.Add("37", "BankReferenceDate-applbankrefdate");
            columnheadertxt.Add("38", "Vocational-vocational_stream");
            columnheadertxt.Add("39", "TotalFess-totalfees");
            columnheadertxt.Add("40", "Paid-PaidAmount");
            columnheadertxt.Add("41", "NoOfAttempt-noofattempts");
            columnheadertxt.Add("42", "Hostel Request-CampusReq");
            columnheadertxt.Add("43", "City-cityp");
            string header = Convert.ToString(columnheadertxt[colval]);
            string[] headername = header.Split('-');
            headertext = Convert.ToString(headername[0]);
            sqlheadertext = Convert.ToString(headername[1]);
        }
        catch { }
    }
    public void columnordertype()
    {
        ddl_colord.Items.Clear();
        if (ddl_collegename.Items.Count > 0)
        {
            //string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='Hosteladmissioncolumnsettings' and CollegeCode='" + ddl_collegename.SelectedItem.Value + "' 
            string query = " select MasterCode,MasterValue from New_InsSettings n,CO_MasterValues c where n.linkname=c.mastervalue and c.MasterCriteria ='Hosteladmissioncolumnsettings' and n.user_code='" + Session["usercode"].ToString() + "' and c.collegecode=n.college_code and c.collegecode ='" + ddl_collegename.SelectedItem.Value + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_colord.DataSource = ds;
                ddl_colord.DataTextField = "MasterValue";
                ddl_colord.DataValueField = "MasterCode";
                ddl_colord.DataBind();
                ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }
    protected void imgbtn_all_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HostelMod/Hosteladmissionsettings.aspx");
    }
    protected void imagebtnpop1close1_Click(object sender, EventArgs e)
    {
        applicationfees_div.Visible = false;
    }
    protected void ddl_hosteladmissionH_Selectedindex_Changed(object sender, EventArgs e)
    {
        bindledgershortlist();
    }
    public string Hostelserialno()
    {
        string newitemcode = "";
        try
        {
            string selectquery = "select HostelserialAcr,HostelserialStNo,Hostelserialsize  from HM_Codesettings order by startdate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["HostelserialAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["HostelserialStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["Hostelserialsize"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                {
                    selectquery = " select top (1) Serial_No  from HT_HostelRegistration where Serial_No like '" + Convert.ToString(itemacronym) + "%' and isnull(Serial_No,'')<>'' order by HostelRegistrationPK desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["Serial_No"]);
                        string itemacr = Convert.ToString(itemacronym);
                        int len = itemacr.Length;
                        itemcode = itemcode.Remove(0, len);
                        int len1 = Convert.ToString(itemcode).Length;
                        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                        len = Convert.ToString(newnumber).Length;
                        len1 = len1 - len;
                        if (len1 == 2)
                        {
                            newitemcode = "00" + newnumber;
                        }
                        else if (len1 == 1)
                        {
                            newitemcode = "0" + newnumber;
                        }
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
                        }
                        else if (len1 == 5)
                        {
                            newitemcode = "00000" + newnumber;
                        }
                        else if (len1 == 6)
                        {
                            newitemcode = "000000" + newnumber;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(newnumber);
                        }
                        if (newitemcode.Trim() != "")
                        {
                            newitemcode = itemacr + "" + newitemcode;
                        }
                    }
                    else
                    {
                        string itemacr = Convert.ToString(itemstarno);
                        int len = itemacr.Length;
                        string items = Convert.ToString(itemsize);
                        int len1 = Convert.ToInt32(items);
                        int size = len1 - len;
                        if (size == 2)
                        {
                            newitemcode = "00" + itemstarno;
                        }
                        else if (size == 1)
                        {
                            newitemcode = "0" + itemstarno;
                        }
                        else if (size == 3)
                        {
                            newitemcode = "000" + itemstarno;
                        }
                        else if (size == 4)
                        {
                            newitemcode = "0000" + itemstarno;
                        }
                        else if (size == 5)
                        {
                            newitemcode = "00000" + itemstarno;
                        }
                        else if (size == 6)
                        {
                            newitemcode = "000000" + itemstarno;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(itemstarno);
                        }
                        newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Set Hostel Admission Serial No";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Set Hostel Admission Serial No";
            }
        }
        catch (Exception ex)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = ex.ToString();
        }
        return newitemcode;
    }
}