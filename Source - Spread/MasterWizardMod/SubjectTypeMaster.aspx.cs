using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class MasterWizardMod_SubjectTypeMaster : System.Web.UI.Page
{

    #region FieldDeclaration
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collcode = string.Empty;
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    DataTable dtCommon = new DataTable();
    DataSet dsprint = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable has = new Hashtable();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataTable data = new DataTable();
    DataRow drow;
    ArrayList arrColHdrNames1 = new ArrayList();
    ArrayList arrColHdrNames2 = new ArrayList();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("MasterWizardMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MasterWizardMod/MasterWizardMenuIndex.aspx");
                    return;
                }
            }

            userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

            //****************************************************//
            if (!IsPostBack)
            {
                bindcollegename();
                bindpop2batchyear();
                bindpop2degree();
                loadbranch();
                bindsem();

                bindbatch();
                binddegree1();
                bindbranch1();
                bindsem1();
            }
        }
        catch
        {

        }
    }

    #region Load_Function

    protected void bindcollegename()
    {
        try
        {
            string clgname = "select college_code,collname from collinfo ";
            if (clgname != "")
            {
                ds = d2.select_method(clgname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ddl_pop2collgname.DataSource = ds;
                    ddl_pop2collgname.DataTextField = "collname";
                    ddl_pop2collgname.DataValueField = "college_code";
                    ddl_pop2collgname.DataBind();

                    ddl_college.DataSource = ds;
                    ddl_college.DataTextField = "collname";
                    ddl_college.DataValueField = "college_code";
                    ddl_college.DataBind();

                }
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    protected void bindpop2degree()
    {
        try
        {

            ddl_pop2degre.Items.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddl_pop2collgname.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleUser);
            has.Add("group_code", groupUserCode);
            has.Add("college_code", userCollegeCode);
            has.Add("user_code", userCode);
            ds = d2.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddl_pop2degre.DataSource = ds;
                ddl_pop2degre.DataTextField = "course_name";
                ddl_pop2degre.DataValueField = "course_id";
                ddl_pop2degre.DataBind();
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    public void loadbranch()
    {
        try
        {

            ddl_pop2branch.Items.Clear();
            has.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddl_pop2collgname.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }
            has.Add("single_user", singleUser);
            has.Add("group_code", groupUserCode);
            has.Add("course_id", ddl_pop2degre.SelectedValue);
            has.Add("college_code", ddl_pop2collgname.SelectedValue);
            has.Add("user_code", userCode);
            ds = d2.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddl_pop2branch.DataSource = ds;
                ddl_pop2branch.DataTextField = "dept_name";
                ddl_pop2branch.DataValueField = "degree_code";
                ddl_pop2branch.DataBind();
            }



        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
    }

    protected void bindpop2batchyear()
    {
        try
        {

            ddl_pop2batchyear.Items.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddl_pop2batchyear.DataSource = ds;
                ddl_pop2batchyear.DataTextField = "batch_year";
                ddl_pop2batchyear.DataValueField = "batch_year";
                ddl_pop2batchyear.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddl_pop2batchyear.SelectedValue = max_bat.ToString();
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    public void bindsem()
    {
        try
        {
            ddlsemester.Items.Clear();
            string duration = string.Empty;
            Boolean first_year = false;
            has.Clear();
            userCollegeCode = ddl_pop2collgname.SelectedItem.Value;
            has.Add("degree_code", ddl_pop2branch.SelectedValue.ToString());
            has.Add("batch_year", ddl_pop2batchyear.SelectedValue.ToString());
            has.Add("college_code", userCollegeCode);
            ds = d2.select_method("bind_sem", has, "sp");
            int count3 = ds.Tables[0].Rows.Count;
            if (count3 > 0)
            {
                ddlsemester.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlsemester.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                count3 = ds.Tables[1].Rows.Count;
                if (count3 > 0)
                {
                    ddlsemester.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlsemester.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlsemester.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    ddlsemester.Enabled = false;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    #endregion


    #region Events
    protected void ddl_pop2collgname_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            divMainContents.Visible = false;
            grdsubject.Visible = false;
            rptprint.Visible = false;
            bindpop2batchyear();
            bindpop2degree();
            loadbranch();
            bindsem();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    protected void ddl_pop2batchyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divMainContents.Visible = false;
            grdsubject.Visible = false;
            rptprint.Visible = false;
            bindpop2degree();
            loadbranch();
            bindsem();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    protected void ddl_pop2degre_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdsubject.Visible = false;
            rptprint.Visible = false;
            divMainContents.Visible = false;
            loadbranch();
            bindsem();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdsubject.Visible = false;
        divMainContents.Visible = false;
        rptprint.Visible = false;
        bindsem();
    }

    protected void ddlsemester_SelectedIndexChanged(object sener, EventArgs e)
    {

        grdsubject.Visible = false;
        divMainContents.Visible = false;
        rptprint.Visible = false;
    }
    #endregion

    #region select_Subject

    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            bindbatch();
            binddegree1();
            bindbranch1();
            bindsem1();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }



    public void bindbatch()
    {
        try
        {

            ddlbatch.Items.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch1();
            binddegree1();
            bindsem1();


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    public void binddegree1()
    {
        try
        {

            ddldegree.Items.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddl_college.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleUser);
            has.Add("group_code", groupUserCode);
            has.Add("college_code", userCollegeCode);
            has.Add("user_code", userCode);
            ds = d2.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch1();
            bindsem1();


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    public void bindbranch1()
    {
        try
        {

            ddlbranch.Items.Clear();
            has.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddl_college.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }
            has.Add("single_user", singleUser);
            has.Add("group_code", groupUserCode);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", ddl_college.SelectedValue);
            has.Add("user_code", userCode);
            ds = d2.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    protected void ddlbranch_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            bindsem1();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }

    public void bindsem1()
    {
        try
        {

            ddlsem.Items.Clear();
            string duration = string.Empty;
            Boolean first_year = false;
            has.Clear();
            userCollegeCode = ddl_college.SelectedItem.Value;
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("batch_year", ddlbatch.SelectedValue.ToString());
            has.Add("college_code", userCollegeCode);
            ds = d2.select_method("bind_sem", has, "sp");
            int count3 = ds.Tables[0].Rows.Count;
            if (count3 > 0)
            {
                ddlsem.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                count3 = ds.Tables[1].Rows.Count;
                if (count3 > 0)
                {
                    ddlsem.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    ddlsem.Enabled = false;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }

    }



    #endregion



    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> PaperType(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = " select distinct subject_type from sub_sem where subject_type like '" + prefixText + "%'";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["subject_type"].ToString());
            }
        }
        return name;
    }



    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            string collegecode1 = string.Empty;
            string batch1 = string.Empty;
            string courseid1 = string.Empty;
            string dept1 = string.Empty;
            string sem1 = string.Empty;

            arrColHdrNames1.Add("S.No");
            arrColHdrNames1.Add("Type Of Paper");
            arrColHdrNames1.Add("No.Of Papers");
            arrColHdrNames1.Add("Fee/Paper In RS.");
            arrColHdrNames1.Add("Fee/Paper In RS.");
            arrColHdrNames1.Add("Fee/Paper In RS."); ;
            arrColHdrNames1.Add("Fee/Paper In RS.");
            arrColHdrNames1.Add("Fee/Paper In RS.");
            arrColHdrNames1.Add("Compulsary");
            arrColHdrNames1.Add("LAB Oriented");
            arrColHdrNames1.Add("Project/Thesis");
            arrColHdrNames1.Add("Seminar");
            arrColHdrNames1.Add("Elective");
            arrColHdrNames1.Add("Pre-Schedulable");
            arrColHdrNames1.Add("Online Elective");
            arrColHdrNames1.Add("Subject Type No");

            arrColHdrNames2.Add("S.No");
            arrColHdrNames2.Add("Type Of Paper");
            arrColHdrNames2.Add("No.Of Papers");
            arrColHdrNames2.Add("Current");
            arrColHdrNames2.Add("Arrear");
            arrColHdrNames2.Add("Improve");
            arrColHdrNames2.Add("Re-Total");
            arrColHdrNames2.Add("Re-Valuation");
            arrColHdrNames2.Add("Compulsary");
            arrColHdrNames2.Add("LAB Oriented");
            arrColHdrNames2.Add("Project/Thesis");
            arrColHdrNames2.Add("Seminar");
            arrColHdrNames2.Add("Elective");
            arrColHdrNames2.Add("Pre-Schedulable");
            arrColHdrNames2.Add("P");
            arrColHdrNames2.Add("Subject Type No");

            data.Columns.Add("S.No");
            data.Columns.Add("Type Of Paper");
            data.Columns.Add("No.Of Papers");
            data.Columns.Add("Current");
            data.Columns.Add("Arrear");
            data.Columns.Add("Improve");
            data.Columns.Add("Re-Total");
            data.Columns.Add("Re-Valuation");
            data.Columns.Add("Compulsary");
            data.Columns.Add("LAB Oriented");
            data.Columns.Add("Project/Thesis");
            data.Columns.Add("Seminar");
            data.Columns.Add("Elective");
            data.Columns.Add("Pre-Schedulable");
            data.Columns.Add("P");
            data.Columns.Add("Subject Type No");
            DataRow drHdr1 = data.NewRow();
            DataRow drHdr2 = data.NewRow();
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames1[grCol];
                drHdr2[grCol] = arrColHdrNames2[grCol];
            }
            data.Rows.Add(drHdr1);
            data.Rows.Add(drHdr2);
            DataSet dssub = new DataSet();
            if (ddl_college.Items.Count > 0)
                collegecode1 = Convert.ToString(ddl_pop2collgname.SelectedValue);
            if (ddlbatch.Items.Count > 0)
                batch1 = Convert.ToString(ddl_pop2batchyear.SelectedValue);
            if (ddldegree.Items.Count > 0)
                courseid1 = Convert.ToString(ddl_pop2degre.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                dept1 = Convert.ToString(ddl_pop2branch.SelectedValue);
            if (ddlsem.Items.Count > 0)
                sem1 = Convert.ToString(ddlsemester.SelectedValue);
            if (!string.IsNullOrEmpty(collegecode1) && !string.IsNullOrEmpty(batch1) && !string.IsNullOrEmpty(courseid1) && !string.IsNullOrEmpty(dept1) && !string.IsNullOrEmpty(sem1))
            {
                string syllcode = d2.GetFunction("select syll_code from syllabus_master where degree_code='" + dept1 + "' and semester='" + sem1 + "' and Batch_Year='" + batch1 + "'");
                if (syllcode != "0")
                {
                    string sqlqry = "select * from sub_sem where syll_code='" + syllcode + "'";
                    dssub.Clear();
                    dssub = d2.select_method_wo_parameter(sqlqry, "Text");
                    if (dssub.Tables.Count > 0 && dssub.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dssub.Tables[0].Rows.Count; i++)
                        {
                            double curr = Convert.ToDouble(dssub.Tables[0].Rows[i]["fee_per_paper"]);
                            double arr = Convert.ToDouble(dssub.Tables[0].Rows[i]["arr_fee"]);
                            double Improve = Convert.ToDouble(dssub.Tables[0].Rows[i]["improvement_fee"]);
                            double Retot = Convert.ToDouble(dssub.Tables[0].Rows[i]["re_tot"]);
                            double Reval = Convert.ToDouble(dssub.Tables[0].Rows[i]["re_val"]);

                            string comp = Convert.ToString(dssub.Tables[0].Rows[i]["promote_count"]);
                            string LAB = Convert.ToString(dssub.Tables[0].Rows[i]["Lab"]);
                            string Project = Convert.ToString(dssub.Tables[0].Rows[i]["projThe"]);
                            string Seminar = Convert.ToString(dssub.Tables[0].Rows[i]["seminar"]);
                            string Elective = Convert.ToString(dssub.Tables[0].Rows[i]["ELectivePap"]);
                            string Pre = Convert.ToString(dssub.Tables[0].Rows[i]["pre_schedule"]);
                            string onEle = Convert.ToString(dssub.Tables[0].Rows[i]["onlineelective"]);
                            if (comp.ToUpper() == "TRUE")
                                comp = "Yes";
                            else
                                comp = "No";
                            if (LAB.ToUpper() == "TRUE")
                                LAB = "Yes";
                            else
                                LAB = "No";
                            if (Project.ToUpper() == "TRUE")
                                Project = "Yes";
                            else
                                Project = "No";
                            if (Seminar.ToUpper() == "TRUE")
                                Seminar = "Yes";
                            else
                                Seminar = "No";
                            if (Elective.ToUpper() == "TRUE")
                                Elective = "Yes";
                            else
                                Elective = "No";
                            if (Pre.ToUpper() == "TRUE")
                                Pre = "Yes";
                            else
                                Pre = "No";
                            if (onEle.ToUpper() == "TRUE")
                                onEle = "Yes";
                            else
                                onEle = "No";


                            drow = data.NewRow();
                            drow["S.No"] = Convert.ToString(i + 1);
                            drow["Type Of Paper"] = Convert.ToString(dssub.Tables[0].Rows[i]["subject_type"]);
                            drow["No.Of Papers"] = Convert.ToString(dssub.Tables[0].Rows[i]["no_of_papers"]);
                            drow["Current"] = Convert.ToString(curr);
                            drow["Arrear"] = Convert.ToString(arr);
                            drow["Improve"] = Convert.ToString(Improve);
                            drow["Re-Total"] = Convert.ToString(Retot);
                            drow["Re-Valuation"] = Convert.ToString(Reval);
                            drow["Compulsary"] = Convert.ToString(comp);
                            drow["LAB Oriented"] = Convert.ToString(LAB);
                            drow["Project/Thesis"] = Convert.ToString(Project);
                            drow["Seminar"] = Convert.ToString(Seminar);
                            drow["Elective"] = Convert.ToString(Elective);
                            drow["Pre-Schedulable"] = Convert.ToString(Pre);
                            drow["P"] = Convert.ToString(onEle);
                            drow["Subject Type No"] = Convert.ToString(dssub.Tables[0].Rows[i]["subType_no"]);
                            data.Rows.Add(drow);


                        }
                        if (data.Rows.Count > 0 && data.Columns.Count > 0)
                        {


                            grdsubject.DataSource = data;
                            grdsubject.DataBind();
                            grdsubject.Visible = true;
                            divMainContents.Visible = true;
                            rptprint.Visible = true;
                            int rowcnt = grdsubject.Rows.Count - 2;
                            //Rowspan
                            for (int rowIndex = grdsubject.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                            {
                                GridViewRow row = grdsubject.Rows[rowIndex];
                                GridViewRow previousRow = grdsubject.Rows[rowIndex + 1];
                                grdsubject.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                grdsubject.Rows[rowIndex].Font.Bold = true;
                                grdsubject.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;
                                grdsubject.Rows[rowIndex].Enabled = false;

                                for (int i = 0; i < row.Cells.Count; i++)
                                {
                                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                    {
                                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                               previousRow.Cells[i].RowSpan + 1;
                                        previousRow.Cells[i].Visible = false;
                                    }

                                }

                            }

                            //ColumnSpan

                            for (int cell = grdsubject.Rows[0].Cells.Count - 1; cell > 0; cell--)
                            {
                                TableCell colum = grdsubject.Rows[0].Cells[cell];
                                TableCell previouscol = grdsubject.Rows[0].Cells[cell - 1];
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
                            for (int rowIndex1 = 0; rowIndex1 <= grdsubject.Rows.Count - 1; rowIndex1++)
                            {
                                grdsubject.Rows[rowIndex1].Cells[15].Visible = false;
                            }
                        }

                    }
                    else
                    {
                        alertimg.Visible = true;
                        lbl_alert.Text = "No Record's Found!";
                        rptprint.Visible = false;
                    }
                }
                else
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "No Record's Found!";
                    rptprint.Visible = false;

                }
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Please Select All The Field!";
                rptprint.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {


        }

    }

    protected void btn_Add_Click(object sender, EventArgs e)
    {
        try
        {
            popupsubject.Visible = true;
            btn_Delete.Visible = false;
            btn_save.Text = "Save";
            ddl_college.Enabled = true;
            ddlbatch.Enabled = true;
            ddldegree.Enabled = true;
            ddlbranch.Enabled = true;
            ddlsem.Enabled = true;
            bindcollegename();
            bindbatch();
            binddegree1();
            bindbranch1();
            bindsem1();

            txtPaperType.Text = "";
            Textpaperno.Text = "";
            TextCurrent.Text = "";
            TextArrear.Text = "";
            TextImprovement.Text = "";
            TextRetot.Text = "";
            TextReval.Text = "";
            chkCompulsary.Checked = false;
            chkLAB.Checked = false;
            chkProject.Checked = false;
            chkSeminar.Checked = false;
            chkElective.Checked = false;
            chkOnElective.Checked = false;
            Session["subtypeno"] = null;


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {

        }

    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Center;
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {


        }

    }


    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex > 1)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
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

    protected void SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex1 = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        grdsubject.Rows[rowIndex1].Cells[selectedCellIndex].BackColor = Color.LightCoral;
        grdsubject.Rows[rowIndex1].Cells[selectedCellIndex].BorderColor = Color.Black;
        Session["Gridcellrow"] = Convert.ToString(rowIndex1);

        if (rowIndex1 > 1)
        {
            popupsubject.Visible = true;
            btn_save.Visible = true;
            btn_save.Text = "Update";
            btn_exit.Visible = true;
            btn_Delete.Visible = true;
            ddl_college.Enabled = false;
            ddlbatch.Enabled = false;
            ddldegree.Enabled = false;
            ddlbranch.Enabled = false;
            ddlsem.Enabled = false;


            ddl_college.SelectedIndex = ddl_pop2collgname.Items.IndexOf(ddl_pop2collgname.Items.FindByText(Convert.ToString(ddl_pop2collgname.SelectedValue)));
            ddlbatch.SelectedIndex = ddl_pop2batchyear.Items.IndexOf(ddl_pop2batchyear.Items.FindByText(Convert.ToString(ddl_pop2batchyear.SelectedValue)));
            ddldegree.SelectedIndex = ddl_pop2degre.Items.IndexOf(ddl_pop2degre.Items.FindByText(Convert.ToString(ddl_pop2degre.SelectedValue)));
            ddlbranch.SelectedIndex = ddl_pop2branch.Items.IndexOf(ddl_pop2branch.Items.FindByText(Convert.ToString(ddl_pop2branch.SelectedValue)));
            ddlsem.SelectedIndex = ddlsemester.Items.IndexOf(ddlsemester.Items.FindByText(Convert.ToString(ddlsemester.SelectedValue)));


            txtPaperType.Text = grdsubject.Rows[rowIndex1].Cells[1].Text;
            Textpaperno.Text = grdsubject.Rows[rowIndex1].Cells[2].Text;
            TextCurrent.Text = grdsubject.Rows[rowIndex1].Cells[3].Text;
            TextArrear.Text = grdsubject.Rows[rowIndex1].Cells[4].Text;
            TextImprovement.Text = grdsubject.Rows[rowIndex1].Cells[5].Text;
            TextRetot.Text = grdsubject.Rows[rowIndex1].Cells[6].Text;
            TextReval.Text = grdsubject.Rows[rowIndex1].Cells[7].Text;

            string compl = grdsubject.Rows[rowIndex1].Cells[8].Text;
            if (compl.ToUpper() == "YES")
                chkCompulsary.Checked = true;
            else
                chkCompulsary.Checked = false;

            string labs = grdsubject.Rows[rowIndex1].Cells[9].Text;
            if (labs.ToUpper() == "YES")
                chkLAB.Checked = true;
            else
                chkLAB.Checked = false;

            string prot = grdsubject.Rows[rowIndex1].Cells[10].Text;
            if (prot.ToUpper() == "YES")
                chkProject.Checked = true;
            else
                chkProject.Checked = false;

            string semin = grdsubject.Rows[rowIndex1].Cells[11].Text;
            if (semin.ToUpper() == "YES")
                chkSeminar.Checked = true;
            else
                chkSeminar.Checked = false;

            string elec = grdsubject.Rows[rowIndex1].Cells[12].Text;
            if (elec.ToUpper() == "YES")
                chkElective.Checked = true;
            else
                chkElective.Checked = false;

            string onelec = grdsubject.Rows[rowIndex1].Cells[14].Text;
            if (onelec.ToUpper() == "YES")
                chkOnElective.Checked = true;
            else
                chkOnElective.Checked = false;

            string subtypeno = grdsubject.Rows[rowIndex1].Cells[15].Text;
            Session["subtypeno"] = subtypeno;

        }
    }


    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupsubject.Visible = false;
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {


            string collegecode = string.Empty;
            string batch = string.Empty;
            string courseid = string.Empty;
            string dept = string.Empty;
            string sem = string.Empty;
            DataSet dssys = new DataSet();
            string com = string.Empty;
            string Lab = string.Empty;
            string proj = string.Empty;
            string semi = string.Empty;
            string Ele = string.Empty;
            string onele = string.Empty;
            double cuu = 0;
            double arr = 0;
            double improve = 0;
            double retotal = 0;
            double revaluation = 0;


            if (ddl_college.Items.Count > 0)
                collegecode = Convert.ToString(ddl_college.SelectedValue);
            if (ddlbatch.Items.Count > 0)
                batch = Convert.ToString(ddlbatch.SelectedValue);
            if (ddldegree.Items.Count > 0)
                courseid = Convert.ToString(ddldegree.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                dept = Convert.ToString(ddlbranch.SelectedValue);
            if (ddlsem.Items.Count > 0)
                sem = Convert.ToString(ddlsem.SelectedValue);


            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(courseid) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(sem))
            {
                if (txtPaperType.Text != "" && Textpaperno.Text != "" && Textpaperno.Text != "0")
                {
                    string paperty = txtPaperType.Text.Trim();
                    int noofpaper = 0;
                    int.TryParse(Convert.ToString(Textpaperno.Text), out noofpaper);
                    if (chkCompulsary.Checked)
                        com = "1";
                    else
                        com = "0";
                    if (chkLAB.Checked)
                        Lab = "1";
                    else
                        Lab = "0";
                    if (chkProject.Checked)
                        proj = "1";
                    else
                        proj = "0";
                    if (chkSeminar.Checked)
                        semi = "1";
                    else
                        semi = "0";
                    if (chkElective.Checked)
                        Ele = "1";
                    else
                        Ele = "0";
                    if (chkOnElective.Checked)
                        onele = "1";
                    else
                        onele = "0";

                    if (TextCurrent.Text != "")
                        double.TryParse(Convert.ToString(TextCurrent.Text), out cuu);
                    if (TextArrear.Text != "")
                        double.TryParse(Convert.ToString(TextArrear.Text), out arr);
                    if (TextImprovement.Text != "")
                        double.TryParse(Convert.ToString(TextImprovement.Text), out improve);
                    if (TextRetot.Text != "")
                        double.TryParse(Convert.ToString(TextRetot.Text), out retotal);
                    if (TextReval.Text != "")
                        double.TryParse(Convert.ToString(TextReval.Text), out revaluation);

                    string q1 = "if not exists (select syll_code from syllabus_master where Batch_Year='" + batch + "' and degree_code='" + dept + "' and semester='" + sem + "') insert into syllabus_master(Batch_Year,degree_code,semester,syllabus_year) values('" + batch + "','" + dept + "','" + sem + "','" + batch + "')";
                    int insupdval = d2.update_method_wo_parameter(q1, "Text");
                    string syllcode = d2.GetFunction("select syll_code from syllabus_master where degree_code='" + dept + "' and semester='" + sem + "' and Batch_Year='" + batch + "'");
                    if (syllcode != "0")
                    {
                        string qq = "if not exists(select * from syll_batchYear where syll_code='" + syllcode + "')insert into syll_batchYear (syll_code,batch_year) values('" + syllcode + "','" + batch + "')";
                        int exeqq = d2.update_method_wo_parameter(qq, "Text");

                        int insup = 0;
                        if (btn_save.Text.ToUpper() == "SAVE")
                        {
                            string q2 = "if exists(select subtype_no from sub_sem where subject_type='" + paperty + "' and syll_code='" + syllcode + "')   update sub_sem set subject_type='" + paperty + "',no_of_papers='" + noofpaper + "',promote_count='" + com + "',Lab='" + Lab + "',projThe='" + proj + "',ELectivePap='" + Ele + "',fee_per_paper='" + cuu + "',arr_fee='" + arr + "',improvement_fee='" + improve + "',onlineelective='" + onele + "',seminar='" + semi + "',re_tot='" + retotal + "',re_val='" + revaluation + "',markOrGrade='0',pre_schedule='0' where subject_type='" + paperty + "' and syll_code='" + syllcode + "' else insert into sub_sem (syll_code, subject_type, no_of_papers, promote_count, Lab, projThe, ELectivePap,onlineelective,fee_per_paper, arr_fee, improvement_fee,re_tot,re_val,seminar,markOrGrade,pre_schedule) values ('" + syllcode + "','" + paperty + "','" + noofpaper + "','" + com + "','" + Lab + "','" + proj + "','" + Ele + "','" + onele + "','" + cuu + "','" + arr + "','" + improve + "','" + retotal + "','" + revaluation + "','" + semi + "','0','0')";
                            insup = d2.update_method_wo_parameter(q2, "text");
                        }
                        if (btn_save.Text.ToUpper() == "UPDATE")
                        {
                            if (Session["subtypeno"] != "" && Session["subtypeno"] != null)
                            {
                                string q2 = " update sub_sem set subject_type='" + paperty + "',no_of_papers='" + noofpaper + "',promote_count='" + com + "',Lab='" + Lab + "',projThe='" + proj + "',ELectivePap='" + Ele + "',fee_per_paper='" + cuu + "',arr_fee='" + arr + "',improvement_fee='" + improve + "',onlineelective='" + onele + "',seminar='" + semi + "',re_tot='" + retotal + "',re_val='" + revaluation + "',markOrGrade='0',pre_schedule='0' where subType_no='" + Session["subtypeno"].ToString() + "' and syll_code='" + syllcode + "'";
                                insup = d2.update_method_wo_parameter(q2, "text");
                            }
                        }

                        if (insup > 0)
                        {
                            alertimg.Visible = true;
                            if (btn_save.Text.ToUpper() == "SAVE")
                                lbl_alert.Text = "Saved Successfully";
                            if (btn_save.Text.ToUpper() == "UPDATE")
                                lbl_alert.Text = "Updated Successfully";
                            btn_go_Click(sender, e);
                        }

                    }

                }
                else
                {
                    if (txtPaperType.Text == "")
                        lbl_alert.Text = "Please Enter The Paper Type!";
                    if (Textpaperno.Text == "" || Textpaperno.Text == "0")
                        lbl_alert.Text = "Please Enter The No.Of Paper!";
                    alertimg.Visible = true;
                }
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Please Select All The Field!";

            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {

        }

    }

    protected void btn_Delete_Click(object sender, EventArgs e)
    {
        try
        {

            string syll_code = d2.GetFunction("select syll_code from subject  where  subtype_no='" + Session["subtypeno"].ToString() + "'");
            if (syll_code != "" && syll_code != "0")
            {
                string Sub_Type = d2.GetFunction("select subject_type from sub_sem  where  subtype_no='" + Session["subtypeno"].ToString() + "' and syll_code='" + syll_code + "'");
                alertimg.Visible = true;
                lbl_alert.Text = "You can't delete,Since subjects already alloted for " + Sub_Type + " paper.Please delete subjects to carry on this process";

            }
            else
            {
                divPopAlertNEW.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsgNEW.Text = "Are you sure to delete the records ?";


            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {


        }
    }

    protected void btn_yes_Click(object sender, EventArgs e)
    {
        try
        {
            string delqry = "delete from sub_sem where  subtype_no='" + Session["subtypeno"].ToString() + "'";
            int del = d2.update_method_wo_parameter(delqry, "Text");
            if (del > 0)
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Deleted SuccessFully";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {

        }

    }

    protected void btn_No_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlertNEW.Visible = false;
            divPopAlertContent.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {

        }

    }


    protected void btn_exit_Click(object sender, EventArgs e)
    {
        try
        {
            popupsubject.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {

        }
    }


    protected void btn_errorclose_Click1(object sender, EventArgs e)
    {
        try
        {
            alertimg.Visible = false;
            lbl_alert.Text = "";

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectTypeMaster"); }
        {

        }

    }


    #region Print

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Attendance Shortage Details - Regulation Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }

    #region Print PDF

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {

            string ss = null;
            string degreedetails = "Subject Type Master";
            string pagename = "SubjectTypeMaster.aspx";
            Printcontrol.loadspreaddetails(grdsubject, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;

        }
        catch (Exception ex)
        {

        }
    }

    #endregion Print PDF

    #region Print Excel

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            Printcontrol.Visible = false;
            string reportname = txtexcelname1.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                d2.printexcelreportgrid(grdsubject, reportname);

                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }

        }
        catch (Exception ex)
        {

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }


    #endregion Print PDF
    #endregion



}