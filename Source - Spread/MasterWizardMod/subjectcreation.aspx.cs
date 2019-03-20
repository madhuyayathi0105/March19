using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using InsproDataAccess;
using System.Drawing;
using System.Configuration;

public partial class MasterWizardMod_subjectcreation : System.Web.UI.Page
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            divMainContents.Visible = false;
            grdsubject.Visible = false;
            rptprint.Visible = false;
            btnPrint11();
            string collegecode1 = string.Empty;
            string batch1 = string.Empty;
            string courseid1 = string.Empty;
            string dept1 = string.Empty;
            string sem1 = string.Empty;

            arrColHdrNames1.Add("S.No");
            arrColHdrNames1.Add("Subject Type");
            arrColHdrNames1.Add("Subject Code");
            arrColHdrNames1.Add("Subject Name");
            arrColHdrNames1.Add("Tamil Subject Name");
            arrColHdrNames1.Add("Acronym");
            arrColHdrNames1.Add("External Marks");
            arrColHdrNames1.Add("External Marks");
            arrColHdrNames1.Add("Internal Marks");
            arrColHdrNames1.Add("Internal Marks");
            arrColHdrNames1.Add("Total Marks");
            arrColHdrNames1.Add("Total Marks");
            arrColHdrNames1.Add("No.Of Hrs/Week");
            arrColHdrNames1.Add("Credit Points");
            arrColHdrNames1.Add("Elective Paper");
            arrColHdrNames1.Add("Language1");
            arrColHdrNames1.Add("Language2");
            arrColHdrNames1.Add("Course Code");
            arrColHdrNames1.Add("Max Students");
            arrColHdrNames1.Add("Elective");
            arrColHdrNames1.Add("IS Common Paper");
            arrColHdrNames1.Add("Current");
            arrColHdrNames1.Add("Arrear");
            arrColHdrNames1.Add("Written");
            arrColHdrNames1.Add("Internal");
            arrColHdrNames1.Add("Subno");
            arrColHdrNames1.Add("Subtypeno");

            arrColHdrNames2.Add("S.No");
            arrColHdrNames2.Add("Subject Type");
            arrColHdrNames2.Add("Subject Code");
            arrColHdrNames2.Add("Subject Name");
            arrColHdrNames2.Add("Tamil Subject Name");
            arrColHdrNames2.Add("Acronym");
            arrColHdrNames2.Add("Min");
            arrColHdrNames2.Add("Max");
            arrColHdrNames2.Add("Min ");
            arrColHdrNames2.Add("Max ");
            arrColHdrNames2.Add("Min  ");
            arrColHdrNames2.Add("Max  ");
            arrColHdrNames2.Add("No.Of Hrs/Week");
            arrColHdrNames2.Add("Credit Points");
            arrColHdrNames2.Add("Elective Paper");
            arrColHdrNames2.Add("Language1");
            arrColHdrNames2.Add("Language2");
            arrColHdrNames2.Add("Course Code");
            arrColHdrNames2.Add("Max Students");
            arrColHdrNames2.Add("Elective");
            arrColHdrNames2.Add("IS Common Paper");
            arrColHdrNames2.Add("Current");
            arrColHdrNames2.Add("Arrear");
            arrColHdrNames2.Add("Written");
            arrColHdrNames2.Add("Internal");
            arrColHdrNames2.Add("Subno");
            arrColHdrNames2.Add("Subtypeno");


            data.Columns.Add("S.No");
            data.Columns.Add("Subject Type");
            data.Columns.Add("Subject Code");
            data.Columns.Add("Subject Name");
            data.Columns.Add("Tamil Subject Name");
            data.Columns.Add("Acronym");
            data.Columns.Add("Min");
            data.Columns.Add("Max");
            data.Columns.Add("Min ");
            data.Columns.Add("Max ");
            data.Columns.Add("Min  ");
            data.Columns.Add("Max  ");
            data.Columns.Add("No.Of Hrs/Week");
            data.Columns.Add("Credit Points");
            data.Columns.Add("Elective Paper");
            data.Columns.Add("Language1");
            data.Columns.Add("Language2");
            data.Columns.Add("Course Code");
            data.Columns.Add("Max Students");
            data.Columns.Add("Elective");
            data.Columns.Add("IS Common Paper");
            data.Columns.Add("Current");
            data.Columns.Add("Arrear");
            data.Columns.Add("Written");
            data.Columns.Add("Internal");
            data.Columns.Add("Subno");
            data.Columns.Add("Subtypeno");
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

                string strqry = "select subject_no,subject_type ,subject_code,subject_name,isnull(TamilSubjectName ,'') TamilSubjectName,acronym,min_ext_marks,max_ext_marks,min_int_marks,max_int_marks,mintotal,maxtotal,noofhrsperweek,subject.subType_no,0,credit_points,comp_pap,language1,language2,isnull(maxstud,0 ) max_stud,isnull(elective,0) elective,isnull(CommonSub,0) CommonSub,isnull(CurFee,0) CurFee,isnull(ArrFee,0) ArrFee,isnull(WrittenMaxMark,0) WrittenMaxMark,isnull(IsInternalOnly,0) IsInternalOnly from subject,sub_sem where sub_sem.syll_code = subject.syll_code and subject.subType_no=sub_sem.subType_no and subject.syll_code=(select syll_code from syllabus_master where degree_code='" + dept1 + "' and semester='" + sem1 + "' and batch_year='" + batch1 + "') order by subject_no";
                dssub.Clear();
                dssub = d2.select_method_wo_parameter(strqry, "Text");
                if (dssub.Tables.Count > 0 && dssub.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dssub.Tables[0].Rows.Count; i++)
                    {

                        double minint = Convert.ToDouble(dssub.Tables[0].Rows[i]["min_int_marks"]);
                        double maxint = Convert.ToDouble(dssub.Tables[0].Rows[i]["max_int_marks"]);
                        double minext = Convert.ToDouble(dssub.Tables[0].Rows[i]["min_ext_marks"]);
                        double maxext = Convert.ToDouble(dssub.Tables[0].Rows[i]["max_ext_marks"]);
                        double mintot = Convert.ToDouble(dssub.Tables[0].Rows[i]["mintotal"]);
                        double maxtot = Convert.ToDouble(dssub.Tables[0].Rows[i]["maxtotal"]);
                        double cret = Convert.ToDouble(dssub.Tables[0].Rows[i]["credit_points"]);
                        double CurFee = Convert.ToDouble(dssub.Tables[0].Rows[i]["CurFee"]);
                        double ArrFee = Convert.ToDouble(dssub.Tables[0].Rows[i]["ArrFee"]);
                        double WrittenMaxMark = Convert.ToDouble(dssub.Tables[0].Rows[i]["WrittenMaxMark"]);

                        string comp_pap = Convert.ToString(dssub.Tables[0].Rows[i]["comp_pap"]);
                        string elective = Convert.ToString(dssub.Tables[0].Rows[i]["elective"]);
                        string language1 = Convert.ToString(dssub.Tables[0].Rows[i]["language1"]);
                        string language2 = Convert.ToString(dssub.Tables[0].Rows[i]["language2"]);
                        string IsInternalOnly = Convert.ToString(dssub.Tables[0].Rows[i]["IsInternalOnly"]);

                        if (comp_pap.ToUpper() == "TRUE")
                            comp_pap = "Yes";
                        else
                            comp_pap = "No";
                        if (elective.ToUpper() == "TRUE")
                            elective = "Yes";
                        else
                            elective = "No";
                        if (language1.ToUpper() == "TRUE")
                            language1 = "Yes";
                        else
                            language1 = "No";
                        if (language2.ToUpper() == "TRUE")
                            language2 = "Yes";
                        else
                            language2 = "No";
                        if (IsInternalOnly.ToUpper() == "TRUE")
                            IsInternalOnly = "Yes";
                        else
                            IsInternalOnly = "No";
                        string subjectno = Convert.ToString(dssub.Tables[0].Rows[i]["subject_no"]);
                        string subtypeno = Convert.ToString(dssub.Tables[0].Rows[i]["subType_no"]);

                        drow = data.NewRow();
                        drow["S.No"] = Convert.ToString(i + 1); ;
                        drow["Subject Type"] = Convert.ToString(dssub.Tables[0].Rows[i]["subject_type"]);
                        drow["Subject Code"] = Convert.ToString(dssub.Tables[0].Rows[i]["subject_code"]);
                        drow["Subject Name"] = Convert.ToString(dssub.Tables[0].Rows[i]["subject_name"]);
                        drow["Tamil Subject Name"] = Convert.ToString(dssub.Tables[0].Rows[i]["TamilSubjectName"]);
                        drow["Acronym"] = Convert.ToString(dssub.Tables[0].Rows[i]["acronym"]);
                        drow["Min"] = Convert.ToString(minext);
                        drow["Max"] = Convert.ToString(maxext);
                        drow["Min "] = Convert.ToString(minint);
                        drow["Max "] = Convert.ToString(maxint);
                        drow["Min  "] = Convert.ToString(mintot);
                        drow["Max  "] = Convert.ToString(maxtot);
                        drow["No.Of Hrs/Week"] = Convert.ToString(dssub.Tables[0].Rows[i]["noofhrsperweek"]);
                        drow["Credit Points"] = Convert.ToString(cret);
                        drow["Elective Paper"] = elective;
                        drow["Language1"] = language1;
                        drow["Language2"] = language2;
                        drow["Course Code"] = Convert.ToString(dssub.Tables[0].Rows[i]["subject_code"]);
                        drow["Max Students"] = Convert.ToString(dssub.Tables[0].Rows[i]["max_stud"]);
                        drow["Elective"] = elective;
                        drow["IS Common Paper"] = comp_pap;
                        drow["Current"] = Convert.ToString(CurFee);
                        drow["Arrear"] = Convert.ToString(ArrFee);
                        drow["Written"] = Convert.ToString(WrittenMaxMark);
                        drow["Internal"] = Convert.ToString(IsInternalOnly);
                        drow["Subno"] = Convert.ToString(subjectno);
                        drow["Subtypeno"] = Convert.ToString(subtypeno);
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
                            if (rowIndex1 != 0 && rowIndex1 != 1)
                                grdsubject.Rows[rowIndex1].Cells[4].Font.Name = "Amudham";
                            grdsubject.Rows[rowIndex1].Cells[25].Visible = false;
                            grdsubject.Rows[rowIndex1].Cells[26].Visible = false;
                        }

                    }

                }
                else
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "No Record's Found!";
                    grdsubject.Visible = false;
                    divMainContents.Visible = false;
                    rptprint.Visible = false;
                }
            }
        }
        catch
        { }
    }


    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int i = 6; i < 26; i++)
                    if (i != 17)
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }


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
            LoadSubject();

            string subtypeno1 = grdsubject.Rows[rowIndex1].Cells[26].Text;

            ddl_subtype.SelectedIndex = ddl_subtype.Items.IndexOf(ddl_subtype.Items.FindByValue(subtypeno1));

            Textsubcode.Text = grdsubject.Rows[rowIndex1].Cells[2].Text;
            Textsubname.Text = grdsubject.Rows[rowIndex1].Cells[3].Text;
            string tamil = grdsubject.Rows[rowIndex1].Cells[4].Text;
            if (tamil != "" && tamil != "&nbsp;")
                Text_subtamil.Text = tamil;
            else
                Text_subtamil.Text = "";
            TextSubjectacr.Text = grdsubject.Rows[rowIndex1].Cells[5].Text;
            Textminint.Text = grdsubject.Rows[rowIndex1].Cells[6].Text;
            Textmaxint.Text = grdsubject.Rows[rowIndex1].Cells[7].Text;
            Textminext.Text = grdsubject.Rows[rowIndex1].Cells[8].Text;
            Textmaxext.Text = grdsubject.Rows[rowIndex1].Cells[9].Text;
            Textminmrk.Text = grdsubject.Rows[rowIndex1].Cells[10].Text;
            Textmaxmrk.Text = grdsubject.Rows[rowIndex1].Cells[11].Text;
            Texthrs.Text = grdsubject.Rows[rowIndex1].Cells[12].Text;
            TextCredit.Text = grdsubject.Rows[rowIndex1].Cells[13].Text;
            string Elect = grdsubject.Rows[rowIndex1].Cells[14].Text;
            if (Elect.ToUpper() == "YES")
                chkElectivepaper.Checked = true;
            else
                chkElectivepaper.Checked = false;

            string lang1 = grdsubject.Rows[rowIndex1].Cells[15].Text;
            string lang2 = grdsubject.Rows[rowIndex1].Cells[16].Text;
            if (lang1.ToUpper() == "YES")
                lang.Items[0].Selected = true;
            else
                lang.Items[0].Selected = false;
            if (lang2.ToUpper() == "YES")
                lang.Items[1].Selected = true;
            else
                lang.Items[1].Selected = false;

            Text_crsecode.Text = grdsubject.Rows[rowIndex1].Cells[17].Text;
            Textmaxstd.Text = grdsubject.Rows[rowIndex1].Cells[18].Text;
            string Comm = grdsubject.Rows[rowIndex1].Cells[20].Text;

            if (Comm.ToUpper() == "YES")
                Checkcompaper.Checked = true;
            else
                Checkcompaper.Checked = false;
            Textcurtfees.Text = grdsubject.Rows[rowIndex1].Cells[21].Text;
            TextArrear.Text = grdsubject.Rows[rowIndex1].Cells[22].Text;
            Textwritemax.Text = grdsubject.Rows[rowIndex1].Cells[23].Text;

            string inter = grdsubject.Rows[rowIndex1].Cells[24].Text;

            if (inter.ToUpper() == "YES")
                Checkinternal.Checked = true;
            else
                Checkinternal.Checked = false;

            string subno = grdsubject.Rows[rowIndex1].Cells[25].Text;
            Session["subno"] = subno;
            string subtypeno = grdsubject.Rows[rowIndex1].Cells[26].Text;
            Session["subtypeno"] = subtypeno;

        }
    }


    protected void btn_Add_Click(object sender, EventArgs e)
    {
        try
        {
            popupsubject.Visible = true;
            Text_subtamil.Font.Name = "Amudham";
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
            LoadSubject();

            Textsubcode.Text = "";
            Textsubname.Text = "";
            Text_subtamil.Text = "";
            TextSubjectacr.Text = "";
            Textminint.Text = "";
            Textmaxint.Text = "";
            Textminext.Text = "";
            Textmaxext.Text = "";
            Textminmrk.Text = "";
            Textmaxmrk.Text = "";
            Texthrs.Text = "";
            TextCredit.Text = "";
            lang.Items[0].Selected = false;
            lang.Items[1].Selected = false;
            Textcurtfees.Text = "";
            TextArrear.Text = "";
            Textwritemax.Text = "";
            Textmaxstd.Text = "";
            Text_crsecode.Text = "";
            Checkcompaper.Checked = false;
            chkElectivepaper.Checked = false;
            Checkinternal.Checked = false;
            Session["subno"] = null;
            Session["subtypeno"] = null;
        }
        catch
        {

        }
    }


    public void LoadSubject()
    {
        try
        {
            ddl_subtype.Items.Clear();
            string collegecode = string.Empty;
            string batch = string.Empty;
            string courseid = string.Empty;
            string dept = string.Empty;
            string sem = string.Empty;
            DataSet dssub = new DataSet();
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
                string subtype = "select subType_no,subject_type from sub_sem where syll_code=(select syll_code from syllabus_master where degree_code='" + dept + "' and semester='" + sem + "' and batch_year='" + batch + "' )";
                dssub.Clear();
                dssub = d2.select_method_wo_parameter(subtype, "Text");
                if (dssub.Tables.Count > 0 && dssub.Tables[0].Rows.Count > 0)
                {
                    ddl_subtype.DataSource = dssub;
                    ddl_subtype.DataTextField = "subject_type";
                    ddl_subtype.DataValueField = "subType_no";
                    ddl_subtype.DataBind();
                }
                else
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "Please Update the Subject Type";


                }
            }

        }

        catch
        {
        }

    }


    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupsubject.Visible = false;
    }

    protected void btn_errorclose_Click1(object sender, EventArgs e)
    {
        try
        {
            alertimg.Visible = false;
            lbl_alert.Text = "";

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

    }


    #region select_Subject

    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            bindbatch();
            binddegree1();
            bindbranch1();
            bindsem1();
            LoadSubject();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch1();
            binddegree1();
            bindsem1();
            LoadSubject();


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch1();
            bindsem1();
            LoadSubject();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

    }

    protected void ddlbranch_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            bindsem1();
            LoadSubject();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

    }

    protected void ddlsem_SelectedIndexChanged1(object sender, EventArgs e)
    {
        LoadSubject();
    }


    #endregion

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string collegecode = string.Empty;
            string batch = string.Empty;
            string courseid = string.Empty;
            string dept = string.Empty;
            string sem = string.Empty;
            string subtype = string.Empty;
            float miniinternal = 0;
            float maxiinternal = 0;
            float minextternal = 0;
            float maxexternal = 0;
            float totalminimum = 0;
            float totalmaximum = 0;
            float creditts = 0;
            float current = 0;
            float arrear = 0;
            float writtenmax = 0;
            string lang1 = "";
            string lang2 = "";
            string Elec = "0";
            string Com = "0";
            string Inter = "0";
            //double arr = 0;
            //double improve = 0;
            //double retotal = 0;
            //double revaluation = 0;
            DataSet dssub = new DataSet();
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
            if (ddl_subtype.Items.Count > 0)
                subtype = Convert.ToString(ddl_subtype.SelectedValue);

            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(courseid) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(subtype))
            {
                string syllcode = d2.GetFunction("select syll_code from syllabus_master where degree_code='" + dept + "' and semester='" + sem + "' and Batch_Year='" + batch + "'");
                if (syllcode == "" && syllcode == "0")
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "Please Update the Subject Type";
                    return;
                }

                string subacy = TextSubjectacr.Text;
                string subcode = Textsubcode.Text;
                string subname = Textsubname.Text;
                string subtamil = Text_subtamil.Text;
                string hrs = Texthrs.Text;
                string crecode = Text_crsecode.Text;
                if (lang.SelectedIndex == 0)
                    lang1 = "1";
                else
                    lang1 = "0";
                if (lang.SelectedIndex == 1)
                    lang2 = "1";
                else
                    lang2 = "0";

                if (chkElectivepaper.Checked)
                    Elec = "1";
                if (Checkcompaper.Checked)
                    Com = "1";
                if (Checkinternal.Checked)
                    Inter = "1";

                string maxstd = Textmaxstd.Text;
                float.TryParse(Convert.ToString(Textminint.Text), out miniinternal);
                float.TryParse(Convert.ToString(Textmaxint.Text), out maxiinternal);
                float.TryParse(Convert.ToString(Textminext.Text), out minextternal);
                float.TryParse(Convert.ToString(Textmaxext.Text), out maxexternal);
                float.TryParse(Convert.ToString(Textminmrk.Text), out totalminimum);
                float.TryParse(Convert.ToString(Textmaxmrk.Text), out totalmaximum);
                float.TryParse(Convert.ToString(TextCredit.Text), out creditts);

                float.TryParse(Convert.ToString(Textcurtfees.Text), out current);
                float.TryParse(Convert.ToString(TextArrear.Text), out arrear);
                float.TryParse(Convert.ToString(Textwritemax.Text), out writtenmax);
                int insup = 0;
                if (btn_save.Text.ToUpper() == "SAVE")
                {
                    string insqry = " if exists(select subject_no from subject where subject_code='" + subcode + "' and syll_code='" + syllcode + "') update subject set subType_no='" + subtype + "',acronym='" + subacy + "',subject_code='" + subcode + "',subject_name='" + subname + "',noofhrsperweek='" + hrs + "',subcourse_code='" + crecode + "',min_int_marks='" + miniinternal + "',max_int_marks='" + maxiinternal + "',min_ext_marks='" + minextternal + "',max_ext_marks='" + maxexternal + "',credit_points='" + creditts + "',mintotal='" + totalminimum + "',maxtotal='" + totalmaximum + "',Elective='" + Elec + "',isinternalonly='" + Inter + "',comp_pap='" + Com + "',language1='" + lang1 + "',language2='" + lang2 + "',MaxStud='" + maxstd + "',CurFee='" + current + "',ArrFee='" + arrear + "',writtenmaxmark='" + writtenmax + "',TamilSubjectName='" + subtamil + "'   where subject_code='" + subcode + "' and syll_code='" + syllcode + "' else insert into subject (syll_code,subType_no,acronym,subject_code,subject_name,noofhrsperweek,subcourse_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,credit_points,mintotal,maxtotal,Elective,isinternalonly,comp_pap,language1,language2,MaxStud,CurFee,ArrFee,writtenmaxmark) values ('" + syllcode + "','" + subtype + "','" + subacy + "','" + subcode + "','" + subname + "','" + hrs + "','" + crecode + "','" + miniinternal + "','" + maxiinternal + "','" + minextternal + "','" + maxexternal + "','" + creditts + "','" + totalminimum + "','" + totalmaximum + "','" + Elec + "','" + Inter + "','" + Com + "','" + lang1 + "','" + lang2 + "','" + maxstd + "','" + current + "','" + arrear + "','" + writtenmax + "')";
                    insup = d2.update_method_wo_parameter(insqry, "text");
                }
                if (btn_save.Text.ToUpper() == "UPDATE")
                {
                    if (Session["subno"] != "" && Session["subno"] != null)
                    {
                        string insqry = " update subject set subType_no='" + subtype + "',acronym='" + subacy + "',subject_code='" + subcode + "',subject_name='" + subname + "',noofhrsperweek='" + hrs + "',subcourse_code='" + crecode + "',min_int_marks='" + miniinternal + "',max_int_marks='" + maxiinternal + "',min_ext_marks='" + minextternal + "',max_ext_marks='" + maxexternal + "',credit_points='" + creditts + "',mintotal='" + totalminimum + "',maxtotal='" + totalmaximum + "',Elective='" + Elec + "',isinternalonly='" + Inter + "',comp_pap='" + Com + "',language1='" + lang1 + "',language2='" + lang2 + "',MaxStud='" + maxstd + "',CurFee='" + current + "',ArrFee='" + arrear + "',writtenmaxmark='" + writtenmax + "',TamilSubjectName='" + subtamil + "'   where subject_no='" + Convert.ToString(Session["subno"]) + "' and syll_code='" + syllcode + "'";
                        insup = d2.update_method_wo_parameter(insqry, "text");
                    }
                }
                if (insup > 0)
                {
                    alertimg.Visible = true;
                    if (btn_save.Text.ToUpper() == "SAVE")
                        lbl_alert.Text = "Saved Successfully";
                    if (btn_save.Text.ToUpper() == "UPDATE")
                        lbl_alert.Text = "Updated Successfully";
                    Textsubcode.Text = "";
                    Textsubname.Text = "";
                    Text_subtamil.Text = "";
                    TextSubjectacr.Text = "";
                    Textminint.Text = "";
                    Textmaxint.Text = "";
                    Textminext.Text = "";
                    Textmaxext.Text = "";
                    Textminmrk.Text = "";
                    Textmaxmrk.Text = "";
                    Texthrs.Text = "";
                    TextCredit.Text = "";
                    Textmaxstd.Text = "";
                    lang.Items[0].Selected = false;
                    lang.Items[1].Selected = false;
                    Textcurtfees.Text = "";
                    TextArrear.Text = "";
                    Textwritemax.Text = "";
                    Text_crsecode.Text = "";
                    Checkcompaper.Checked = false;
                    chkElectivepaper.Checked = false;
                    Checkinternal.Checked = false;
                }

            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Please Select All The Field!";

            }

        }
        catch
        {

        }

    }

    protected void btn_Delete_Click(object sender, EventArgs e)
    {
        divPopAlertNEW.Visible = true;
        divPopAlertContent.Visible = true;
        lblAlertMsgNEW.Text = "Do you want to delete the subjects ?";

    }

    protected void btn_yes_Click(object sender, EventArgs e)
    {
        try
        {
            string delqry = "delete from camarks where  subject_no='" + Session["subno"].ToString() + "'";
            delqry += "delete from exam_appl_details where  subject_no='" + Session["subno"].ToString() + "'";
            delqry += "delete from mark_entry where  subject_no='" + Session["subno"].ToString() + "'";
            delqry += "delete from Subject where  subject_no='" + Session["subno"].ToString() + "'";
            delqry += "delete from subjectChooser where  subject_no='" + Session["subno"].ToString() + "'";
            delqry += "delete from staff_selector where  subject_no='" + Session["subno"].ToString() + "'";
            int del = d2.update_method_wo_parameter(delqry, "Text");
            if (del > 0)
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Deleted SuccessFully";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

    }

    protected void btn_No_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlertNEW.Visible = false;
            divPopAlertContent.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "SubjectMaster"); }

    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popupsubject.Visible = false;

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
            string degreedetails = "Subject Master";
            string pagename = "subjectcreation.aspx";
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