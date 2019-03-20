using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Text;
using System.Configuration;

public partial class StudentMod_ReAdmissionProcess : System.Web.UI.Page
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
                RA_txtdate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                RA_txtdate.Attributes.Add("readonly", "readonly");
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
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Admissionflag"] = "1";
                        }
                    }
                }
                ddl_searchtype.Items.Clear();
                //ddlAppFormat.Items.Add(new ListItem("Readmission", "0"));
                ddl_searchtype.Items.Add(new ListItem("Roll No", "0"));
                ddl_searchtype.Items.Add(new ListItem("Reg No", "1"));
                ddl_searchtype.Items.Add(new ListItem("Admission No", "2"));
                ddl_searchtype.Items.Add(new ListItem("App No", "3"));
                ddl_searchtype.Items.Add(new ListItem("Student Name", "4"));
                txt_searchappno.Attributes.Add("placeholder", "Roll No");
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
                RA_ddlbatch.DataSource = ds;
                RA_ddlbatch.DataTextField = "batch_year";
                RA_ddlbatch.DataValueField = "batch_year";
                RA_ddlbatch.DataBind();
            }
        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getappfrom(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //"Roll No", "0"
        //"Reg No", "1"
        //"Admission No", "2"
        //"App No", "3"
        int SEARCHTYPE = 0;
        int.TryParse(contextKey, out SEARCHTYPE);
        string type = "";
        switch (SEARCHTYPE)
        {
            case 0:
                type = "r.roll_no";
                break;
            case 1:
                type = "r.reg_no";
                break;
            case 2:
                type = "r.Roll_Admit";
                break;
            case 3:
                type = "r.app_no";
                break;
            case 4:
                type = "r.stud_name";
                break;
        }
        string query = " select " + type + "  from applyn a,Registration r where a.app_no=r.App_No  and " + type + " like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
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

    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        pop_readmissiondet.Visible = false;
    }

    protected void ddl_searchtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        loadsearchtype();
    }

    protected void loadsearchtype()
    {
        switch (Convert.ToUInt32(ddl_searchtype.SelectedItem.Value))
        {
            case 0:
                txt_searchappno.Attributes.Add("placeholder", "Roll No");
                break;
            case 1:
                txt_searchappno.Attributes.Add("placeholder", "Reg No");
                break;
            case 2:
                txt_searchappno.Attributes.Add("placeholder", "Admission No");
                break;
            case 3:
                txt_searchappno.Attributes.Add("placeholder", "App No");
                break;
            case 4:
                txt_searchappno.Attributes.Add("placeholder", "Student Name");
                break;
        }
    }

    protected string searchFilterType()
    {
        string type = "";
        switch (Convert.ToUInt32(ddl_searchtype.SelectedValue))
        {
            case 0:
                type = "r.roll_no";
                break;
            case 1:
                type = "r.reg_no";
                break;
            case 2:
                type = "r.Roll_Admit";
                break;
            case 3:
                type = "r.app_no";
                break;
            case 4:
                type = "r.stud_name";
                break;
        }
        return type;
    }

    #endregion

    #region Button Events

    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

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
                strorder = ",r.Reg_No";
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

            string deptcode = rs.GetSelectedItemsValueAsString(cbl_degree);
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
            //string sem = rs.GetSelectedItemsValueAsString(cbl_sem);
            DateTime FromDate = new DateTime();
            DateTime ToDate = new DateTime();
            string[] fromdate = txtfromdate.Text.Split('/');
            string[] todate = txttodate.Text.Split('/');
            FromDate = Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2]);
            ToDate = Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2]);

            if (ddlCatogery.SelectedValue.ToString() == "1")
            {
                //and r.current_semester in('" + sem + "') removed on 13/12/2017 by prabha 
                //Semester dropdown removed hence current semester condition has also been removed
                q1 = " select distinct r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections,convert(varchar(10),Discontinue_Date,103)Discontinue_Date from applyn a, degree d,Department dt,Course C,Registration r left join Discontinue ds on r.App_No=ds.app_no where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  (delflag=1 or exam_flag= 'DEBAR') and isnull(r.ProlongAbsent,'0')=0 and r.degree_code in ('" + degreecode + "') and r.batch_year in ('" + ddl_batch.SelectedItem.Value + "')  and r.college_code in('" + ddlcollege.SelectedItem.Value + "')  and Discontinue_Date between '" + FromDate.ToString("MM/dd/yyyy") + "' and '" + ToDate.ToString("MM/dd/yyyy") + "'";
            }
            else if (ddlCatogery.SelectedValue.ToString() == "2")
            {
                //and r.current_semester in('" + sem + "') removed on 13/12/2017 by prabha 
                //Semester dropdown removed hence current semester condition has also been removed
                q1 = " select r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections,convert(varchar(10),Discontinue_Date,103)Discontinue_Date from applyn a, degree d,Department dt,Course C, Registration r left join Discontinue ds on r.App_No=ds.app_no where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  (delflag=1 or exam_flag= 'DEBAR') and r.ProlongAbsent<>0 and r.degree_code in ('" + degreecode + "')  and r.batch_year in ('" + ddl_batch.SelectedItem.Value + "') and r.college_code in('" + ddlcollege.SelectedItem.Value + "')  and Discontinue_Date between '" + FromDate.ToString("MM/dd/yyyy") + "' and '" + ToDate.ToString("MM/dd/yyyy") + "' ";
            }

            if (!string.IsNullOrEmpty(txt_searchappno.Text))
            {
                if (ddlCatogery.SelectedValue.ToString() == "1")
                {
                    //and r.current_semester in('" + sem + "') removed on 13/12/2017 by prabha 
                    //Semester dropdown removed hence current semester condition has also been removed
                    q1 = " select r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections,convert(varchar(10),Discontinue_Date,103)Discontinue_Date from applyn a, degree d,Department dt,Course C,Registration r left join Discontinue ds on r.App_No=ds.app_no where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  (delflag=1 or exam_flag= 'DEBAR') and isnull(r.ProlongAbsent,'0')=0 and r.college_code in('" + ddlcollege.SelectedItem.Value + "')  and Discontinue_Date between '" + FromDate.ToString("MM/dd/yyyy") + "' and '" + ToDate.ToString("MM/dd/yyyy") + "'";
                }
                else if (ddlCatogery.SelectedValue.ToString() == "2")
                {
                    //and r.current_semester in('" + sem + "') removed on 13/12/2017 by prabha 
                    //Semester dropdown removed hence current semester condition has also been removed
                    q1 = " select r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections,convert(varchar(10),Discontinue_Date,103)Discontinue_Date from applyn a, degree d,Department dt,Course C, Registration r left join Discontinue ds on r.App_No=ds.app_no where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  (delflag=1 or exam_flag= 'DEBAR') and r.ProlongAbsent<>0  and r.college_code in('" + ddlcollege.SelectedItem.Value + "')  and Discontinue_Date between '" + FromDate.ToString("MM/dd/yyyy") + "' and '" + ToDate.ToString("MM/dd/yyyy") + "' ";
                }
            }
            string type = searchFilterType();
            if (txt_searchappno.Text.Trim() != "")
                q1 += " and " + type + "='" + txt_searchappno.Text.Trim() + "'";
            q1 += " order by " + strorder.TrimStart(',') + " ";
            if (deptcode.Trim() != "" && degreecode.Trim() != "")  //&& sem.Trim() != ""
            {
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    rs.Fpreadheaderbindmethod("S.No-50/Select-50/Discontinue Date-100/Roll No-150/Reg No-150/Admission No-150/Student Name-200/Gender-120/Section-70/" + lbl_degreeT.Text + "-250/" + lbl_semT.Text + "-100", FpSpread1, "FALSE");
                    FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                    cb.AutoPostBack = false;
                    cball.AutoPostBack = true;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cball;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count - 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cb;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dr["App_No"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["degree_code"]);
                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                        FpSpread1.Sheets[0].Columns[5].Visible = false;
                        if (Convert.ToString(Session["Rollflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        if (Convert.ToString(Session["Regflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[4].Visible = true;
                        if (Convert.ToString(Session["Admissionflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[5].Visible = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["Discontinue_Date"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["roll_no"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dr["Reg_No"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dr["Roll_Admit"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dr["stud_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dr["sex"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dr["sections"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dr["Course_Name"]) + " - " + Convert.ToString(dr["Dept_Name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dr["Current_Semester"]);

                        FarPoint.Web.Spread.TextCellType txtclType = new FarPoint.Web.Spread.TextCellType();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = txtclType;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
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
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
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

    protected void btn_save_OnClick(object sender, EventArgs e)
    {
        lblstuddetails.Visible = false;
        string degree = string.Empty;
        string semester = string.Empty;
        string studdetails = string.Empty;
        FpSpread1.SaveChanges();
        int SelectedRow = 0;
        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
        {
           string value = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value);
            if (value == "1")
            {
                SelectedRow++;
                degree = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 9].Text).Trim();//Added By Saranyadevi 21.2.2018
                semester = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Text).Trim();
            }
        }
        string batch = Convert.ToString(ddl_batch.SelectedItem.Value).Trim();//Added By Saranyadevi 21.2.2018
        studdetails = batch + "-" + degree + "-" + semester;
        
        if (SelectedRow > 0)
        {

            pop_readmissiondet.Visible = true;
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
            if (ddlCatogery.SelectedValue.ToString() == "2")
            {
                
                RA_ddlbatch.Enabled = true;  //modified on 15/12/2017
                RA_ddlsem.Enabled = true;
            }
            if (ddlCatogery.SelectedValue.ToString() == "1")
            {
                RA_ddlbatch.Enabled = true;
                RA_ddlsem.Enabled = true;
            }
            if (SelectedRow == 1) //Added By Saranyadevi 21.2.2018
            {
                lblstuddetails.Visible = true;
                lblstuddetails.Text = studdetails;
            }
            else
            {
                lblstuddetails.Visible = false;
            }

        }
        else
        {
            lblalerterr.Text = "Please Select Atleast One Student and then Proceed";
            alertpopwindow.Visible = true;
        }
    }

    protected void btn_ReadmissionSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string value = string.Empty;
            StringBuilder SaveQuery = new StringBuilder();
            StringBuilder updateQuery = new StringBuilder();
            ds.Clear();
            ds.Dispose();
            q1 = " select Discontinue_Date,Reason,app_no from Discontinue ";
            ds = d2.select_method_wo_parameter(q1, "text");

            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                value = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (value == "1")
                {
                    string app_no = Convert.ToString(FpSpread1.Sheets[0].GetTag(Convert.ToInt32(i), 1)).Trim();
                    string degree = Convert.ToString(FpSpread1.Sheets[0].GetTag(Convert.ToInt32(i), 2)).Trim();
                    string batchYear = Convert.ToString(ddl_batch.SelectedItem.Value);
                    string Sem = FpSpread1.Sheets[0].GetText(Convert.ToInt32(i), 10);
                    string RAsem = Convert.ToString(RA_ddlsem.SelectedItem.Value);
                    string RABatch = Convert.ToString(RA_ddlbatch.SelectedItem.Value);
                    DateTime RAdate = new DateTime();
                    if (RA_txtdate.Text.Trim() != "")
                    {
                        string[] splitdate = RA_txtdate.Text.Split('/');
                        RAdate = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                    }
                    ds.Tables[0].DefaultView.RowFilter = " app_no='" + app_no + "' ";
                    DataView discontinueDet = ds.Tables[0].DefaultView;
                    string DiscontinueDate = string.Empty;
                    string DisReason = string.Empty;
                    if (discontinueDet.Count > 0)
                    {
                        DiscontinueDate = Convert.ToString(discontinueDet[0]["Discontinue_Date"]);
                        DisReason = Convert.ToString(discontinueDet[0]["Reason"]);
                    }
                    if (ddlCatogery.SelectedValue.ToString() == "2")//Prolong Absent
                    {
                        SaveQuery.Append(" insert into Readmission(App_no,Readm_date,Readm_Semester,batch_year,newbatch_year,Dis_Semester,Dis_Date,Dis_Reason,Appr_Lrno,Appr_Date,Catogery)values('" + app_no + "','" + RAdate.ToString("MM/dd/yyyy") + "','" + Sem + "','" + batchYear + "','" + batchYear + "','" + Sem + "','" + DiscontinueDate + "','" + DisReason + "','" + RA_txtremark.Text.Trim() + "','" + RAdate.ToString("MM/dd/yyyy") + "','" + ddlCatogery.SelectedValue.ToString() + "')");
                    }
                    else if (ddlCatogery.SelectedValue.ToString() == "1")//Discontinue
                    {
                        SaveQuery.Append(" insert into Readmission(App_no,Readm_date,Readm_Semester,batch_year,newbatch_year,Dis_Semester,Dis_Date,Dis_Reason,Appr_Lrno,Appr_Date,Catogery)values('" + app_no + "','" + RAdate.ToString("MM/dd/yyyy") + "','" + RAsem + "','" + batchYear + "','" + RABatch + "','" + Sem + "','" + DiscontinueDate + "','" + DisReason + "','" + RA_txtremark.Text.Trim() + "','" + RAdate.ToString("MM/dd/yyyy") + "','" + ddlCatogery.SelectedValue.ToString() + "')");
                        SaveQuery.Append(" update Registration SET current_semester='" + RAsem + "' where App_No='" + app_no + "'"); 
                    }
                    updateQuery.Append("update Registration SET DelFlag=0,ProlongAbsent=0 where App_No='" + app_no + "'");
                    updateQuery.Append("update APPLYN SET Admission_Status=1 where App_No='" + app_no + "'");
                }
            }
            string SaveQry = Convert.ToString(SaveQuery);
            string updateQry = Convert.ToString(updateQuery);
            if (!string.IsNullOrEmpty(SaveQry) && !string.IsNullOrEmpty(updateQry))
            {
                int saveReadmission = d2.update_method_wo_parameter(SaveQry, "text");
                int updateReg = d2.update_method_wo_parameter(updateQry, "text");
                if (saveReadmission != 0 && updateReg != 0)
                {
                    lblalerterr.Text = "Updated Successfully";
                    alertpopwindow.Visible = true;
                    pop_readmissiondet.Visible = false;
                    btn_go_OnClick(sender, e);
                }
            }
            else
            {
                lblalerterr.Text = "Please select any one student";
                alertpopwindow.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Readmission Process"); 
        }
    }

    #endregion

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
            degreedetails = "ReAdmissionProcess Report \n" + clgAcr + '@' + " Date   : " + txtfromdate.Text + " To " + txttodate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@';
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