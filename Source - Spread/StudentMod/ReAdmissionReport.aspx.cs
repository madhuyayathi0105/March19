using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Collections;
using System.Configuration;

public partial class StudentMod_ReAdmissionReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    ReuasableMethods rs = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    string branch = string.Empty;
    string college_code = string.Empty;
    string q1 = string.Empty;

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
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            setLabelText();
            lbl_clgname.Text = lbl_clgT.Text;
            lbl_degree.Text = lbl_degreeT.Text;
            lbl_branch.Text = lbl_branchT.Text;
            //lbl_org_sem.Text = lbl_semT.Text;
            if (!IsPostBack)
            {
                //RA_txtdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                //RA_txtdate.Attributes.Add("readonly", "readonly");
                txtfromdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txtfromdate.Attributes.Add("readonly", "readonly");
                txttodate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txttodate.Attributes.Add("readonly", "readonly");
                BindCollege();
                bindbatch();
                degree();
                bindbranch(branch);
                //bindsem();
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master = "select * from Master_Settings where settings in('Roll No','Register No','Admission No') " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                if (ds.Tables.Count > 0)
                {
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
                        //  if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            //Session["Admissionflag"] = "1";
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    #region Filter event and bind methods

    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        degree();
        FpSpread1.Visible = false;
        //bindsem();
    }

    public void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindsem();
        FpSpread1.Visible = false;
    }

    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = lbl_degreeT.Text + "(" + (cbl_degree.Items.Count) + ")";
                        build1 = cbl_degree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                bindbranch(buildvalue1);
                //bindsem();
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cb_branch.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            int seatcount = 0;
            cb_degree.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            bindbranch(buildvalue);
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = lbl_degreeT.Text + "(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
            //bindsem();
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = lbl_branchT.Text + "(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            //bindsem();
        }
        catch
        {
        }
    }

    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_branch.Items.Count)
            {
                txt_branch.Text = lbl_branchT.Text + "(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else
            {
                txt_branch.Text = lbl_branchT.Text + "(" + commcount.ToString() + ")";
            }
            //bindsem();
        }
        catch
        {
        }
    }

    //public void cb_sem_checkedchange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int cout = 0;
    //        txt_sem.Text = "--Select--";
    //        if (cb_sem.Checked == true)
    //        {
    //            cout++;
    //            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //            {
    //                cbl_sem.Items[i].Selected = true;
    //            }
    //            txt_sem.Text = lbl_semT.Text + "(" + (cbl_sem.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //            {
    //                cbl_sem.Items[i].Selected = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        cb_sem.Checked = false;
    //        int commcount = 0;
    //        txt_sem.Text = "--Select--";
    //        for (int i = 0; i < cbl_sem.Items.Count; i++)
    //        {
    //            if (cbl_sem.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //                cb_sem.Checked = false;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            if (commcount == cbl_sem.Items.Count)
    //            {
    //                cb_sem.Checked = true;
    //            }
    //            txt_sem.Text = lbl_semT.Text + "(" + commcount.ToString() + ")";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    public void degree()
    {
        try
        {
            college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            string query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    //for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //{
                    cbl_degree.Items[0].Selected = true;
                    //}
                    txt_degree.Text = lbl_degreeT.Text + " (" + 1 + ")";
                    cb_degree.Checked = true;
                }
                else
                {
                    txt_degree.Text = "--Select--";
                    cb_degree.Checked = false;
                }
                string build = "";
                string deg = "";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                bindbranch(deg);
            }
            else
            {
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch(string branch)
    {
        try
        {
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            cbl_branch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + " ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                if (cbl_branch.Items.Count > 0)
                {
                    //for (int i = 0; i < cbl_branch.Items.Count; i++)
                    //{
                    cbl_branch.Items[0].Selected = true;
                    //}
                    txt_branch.Text = lbl_branchT.Text + "(" + 1 + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    //public void bindsem()
    //{
    //    RA_ddlsem.Items.Clear();
    //    cbl_sem.Items.Clear();
    //    txt_sem.Text = "--Select--";
    //    int duration = 0;
    //    int i = 0;
    //    ds.Clear();
    //    string branch = "";
    //    string build = "";
    //    string batch = "";
    //    if (cbl_branch.Items.Count > 0)
    //    {
    //        for (i = 0; i < cbl_branch.Items.Count; i++)
    //        {
    //            if (cbl_branch.Items[i].Selected == true)
    //            {
    //                build = cbl_branch.Items[i].Value.ToString();
    //                if (branch == "")
    //                {
    //                    branch = build;
    //                }
    //                else
    //                {
    //                    branch = branch + "," + build;
    //                }
    //            }
    //        }
    //    }
    //    if (ddl_batch.Items.Count > 0)
    //    {
    //        batch = Convert.ToString(ddl_batch.SelectedItem.Value);
    //    }
    //    //batch = build;
    //    if (branch.Trim() != "" && batch.Trim() != "")
    //    {
    //        // ds = d2.BindSem(branch, batch, ddlcollege.SelectedItem.Value);
    //        string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + branch + ") and college_code=" + ddlcollege.SelectedItem.Value + " order by Duration desc";
    //        ds = d2.select_method_wo_parameter(strsql1, "text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            int dur = 0;
    //            int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out dur);
    //            for (i = 1; i <= dur; i++)
    //            {
    //                RA_ddlsem.Items.Add(new ListItem(Convert.ToString(i) + " - " + lbl_semT.Text, Convert.ToString(i)));
    //                cbl_sem.Items.Add(Convert.ToString(i));
    //                cbl_sem.Items[i - 1].Selected = true;
    //                cb_sem.Checked = true;
    //            }
    //            txt_sem.Text = lbl_org_sem.Text + "(" + cbl_sem.Items.Count + ")";
    //        }
    //    }
    //}

    void BindCollege()
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
            Hashtable hat1 = new Hashtable();
            hat1.Clear();
            hat1.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat1, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            ddl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
        }
        catch
        {
        }
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lbl_clgT);
        lbl.Add(lbl_degreeT);
        lbl.Add(lbl_branchT);
        lbl.Add(lbl_semT);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        lbl.Add(lbl_semT);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    #endregion

    protected void btn_go_OnClick(object sender, EventArgs e)
    {
        try
        {
            #region Order by

            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            string strorder = ",len(r.roll_no)";
            if (orderby_Setting == "0")
            {
                strorder = ",len(r.roll_no)";
            }
            else if (orderby_Setting == "1")
            {
                strorder = ",len(r.Reg_No)";
            }
            else if (orderby_Setting == "2")
            {
                strorder = ",r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = ",len(r.roll_no),len(r.Reg_No),r.stud_name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = ",len(r.roll_no),len(r.Reg_No)";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = ",len(r.Reg_No),r.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = ",len(r.roll_no),r.Stud_Name";
            }

            #endregion
            //FarPoint.Web.Spread.TextCellType txt;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_degree);
            string deptcode = rs.GetSelectedItemsValueAsString(cbl_branch);
            string batchyear = ddl_batch.SelectedItem.Text;
            //string sem = rs.GetSelectedItemsValueAsString(cbl_sem);
            DateTime FromDate = new DateTime();
            DateTime ToDate = new DateTime();
            string[] fromdate = txtfromdate.Text.Split('/');
            string[] todate = txttodate.Text.Split('/');
            FromDate = Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2]);
            ToDate = Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2]);
            
            if (ddlCatogery.SelectedValue.ToString() == "1")
            {

                q1 = "select distinct R.Stud_Name ,r.Roll_no,R.Reg_No, Convert(varchar(10),R.batch_year)+' - '+ Convert(varchar(10),c.Course_name)+' - '+de.Acronym+' - '+Convert(varchar(10),R.current_semester)  as Degreedet,Convert(varchar(20),re.readm_date,103) as ReadmittedDate ,Convert(varchar(10), re.newbatch_year) +' - ' + Convert(varchar(10),re.Readm_Semester) +' SEM' as ReadmittedSemester  from Readmission re,REgistration R,Degree de,Department dep,Course c where R.App_No=re.App_No and c.Course_id=de.Course_id and de.college_code=c.college_code and dep.Dept_code=de.Dept_code and dep.college_code=de.college_code and R.college_code=dep.college_code and R.degree_code=de.Degree_code and c.Course_id IN ('" + degreecode + "') and R.batch_year in ('" + batchyear + "') and dep.dept_code in('" + deptcode + "') and re.Dis_Date  between '" + FromDate.ToString("MM/dd/yyyy") + "' and '" + ToDate.ToString("MM/dd/yyyy") + "' and REadmitreason='2' ";   //REadmitreason 1 for Prolong Absent Students
            }
            else if (ddlCatogery.SelectedValue.ToString() == "2")
            {

                q1 = "select distinct R.Stud_Name ,r.Roll_no,R.Reg_No, Convert(varchar(10),R.batch_year)+' - '+ Convert(varchar(10),c.Course_name)+' - '+de.Acronym+' - '+Convert(varchar(10),R.current_semester)  as Degreedet,Convert(varchar(20),re.readm_date,103) as ReadmittedDate ,Convert(varchar(10), re.newbatch_year) +' - ' + Convert(varchar(10),re.Readm_Semester) +' SEM' as ReadmittedSemester  from Readmission re,REgistration R,Degree de,Department dep,Course c where R.App_No=re.App_No and c.Course_id=de.Course_id and de.college_code=c.college_code and dep.Dept_code=de.Dept_code and dep.college_code=de.college_code and R.college_code=dep.college_code and R.degree_code=de.Degree_code and c.Course_id IN ('" + degreecode + "') and R.batch_year in ('" + batchyear + "') and dep.dept_code in('" + deptcode + "') and re.Dis_Date  between '" + FromDate.ToString("MM/dd/yyyy") + "' and '" + ToDate.ToString("MM/dd/yyyy") + "'  and REadmitreason='1' ";  //REadmitreason 1 for Discontinued Students
            }

            //q1 += " order by " + strorder.TrimStart(',') + " ";
            if (deptcode.Trim() != "" && degreecode.Trim() != "")  //&& sem.Trim() != ""
            {
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    //rs.Fpreadheaderbindmethod("S.No-50/Student Name-220/Roll No-130/Reg No-150/Degree Details-180/Readmitted Date-120/Semester-140/", FpSpread1, "FALSE");
                    rs.Fpreadheaderbindmethod("S.No/Student Name/Roll No/Reg No/Degree Details/Readmitted Date/Semester/", FpSpread1, "TRUE");

                    FpSpread1.Sheets[0].Rows.Count++;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count - 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count - 1);
                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["degree_code"]);

                        if (Convert.ToString(Session["Rollflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        if (Convert.ToString(Session["Regflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[4].Visible = true;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["Stud_Name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["Roll_no"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["Reg_No"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dr["Degreedet"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dr["ReadmittedDate"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dr["ReadmittedSemester"]);

                        FarPoint.Web.Spread.TextCellType txtclType = new FarPoint.Web.Spread.TextCellType();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = txtclType;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Columns[7].Visible = false;

                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    lbl_error.Visible = false;
                    print.Visible = true;
                }
                else
                {
                    FpSpread1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Record Founds";
                }
            }
            else
            {
                FpSpread1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch { }
    }

    protected void ddlCatogery_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // lblvalidation1.Text = "";
            string clgAcr = dirAcc.selectScalarString("select Coll_Acronymn from collInfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'");
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Re-Admission Report \n" + clgAcr + '@' + " Date   : " + txtfromdate.Text + " To " + txttodate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@';
            pagename = "ReAdmissionProcess.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails, 0, Convert.ToString(Session["usercode"]));
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    protected void btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            //lblvalidation1.Text = "";
            string clgAcr = dirAcc.selectScalarString("select Coll_Acronymn from collInfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'");
            //string updateqry = "update TextValTable set TextVal='0' where TextCriteria='PMS' and college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            //int i = dirAcc.updateData(updateqry);
            // txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "ReAdmissionProcess Report\n" + clgAcr + '@' + " Date   : " + txtfromdate.Text + " To " + txttodate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
            pagename = "ReAdmissionProcess.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails, 1, Convert.ToString(Session["usercode"]));
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

}