using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Configuration;

public partial class StudentMod_Studentdetreport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable hat1 = new Hashtable();
    static ArrayList ItemList_stud = new ArrayList();
    static ArrayList Itemindex_stud = new ArrayList();
    Hashtable addtotalhash = new Hashtable();
    static string loadval = "";
    static string colval = "";
    static string printval = "";
    Hashtable totalmode = new Hashtable();
    Hashtable newhash = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    static string columnname1 = "";
    ArrayList addcertificate = new ArrayList();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static Hashtable certdet = new Hashtable();
    static Hashtable pcmmark_hash = new Hashtable();
    static byte roll = 0;
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
            if (!IsPostBack)
            {
                setLabelText();
                BindCollege();
                bindbatch();
                edu_level();
                degree();
                bindbranch();
                bindsem();
                BindSectionDetail();

                columnordertype();
                ItemList_stud.Clear();


                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");

                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
            }
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    void BindCollege()
    {
        try
        {
            //string srisql = "select collname,college_code from collinfo";
            //ds = d2.select_method_wo_parameter(srisql, "Text");
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            cbl_clgname.Items.Clear();
            cbl_clgname.DataSource = ds;
            cbl_clgname.DataTextField = "collname";
            cbl_clgname.DataValueField = "college_code";
            cbl_clgname.DataBind();
            cb_clgname.Checked = true;
            CallCheckboxChange(cb_clgname, cbl_clgname, txt_clgname, lbl_clgname.Text, lbl_clgname.Text);
        }
        catch
        {
            CallCheckboxChange(cb_clgname, cbl_clgname, txt_clgname, lbl_clgname.Text, lbl_clgname.Text);
        }
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {

        edu_level();
        degree();
        bindbranch();
        bindbatch();
        bindsem();
        BindSectionDetail();
        columnordertype();
    }
    public void bindbatch()
    {
        try
        {
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
                degree();
            }
        }
        catch
        {
        }
    }
    public void degree()
    {
        try
        {
            string query, edulvl = "";
            cbl_degree.Items.Clear();

            edulvl = rs.GetSelectedItemsText(cbl_grad);


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
            string selColleges = rs.GetSelectedItemsValueAsString(cbl_clgname);

            query = "select distinct c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code in ('" + selColleges + "') and Edu_Level in ('" + edulvl + "')  " + rights + "";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_name";
                cbl_degree.DataBind();
                cb_degree.Checked = true;

            }
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, lbl_degree.Text);
        }
        catch (Exception ex)
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, lbl_degree.Text);
        }
    }
    public void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();
            string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
            string branch = rs.GetSelectedItemsText(cbl_degree);

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
            cb_branch.Checked = false;
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct department.dept_name  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and course.course_name in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code in ('" + collegeSel + "') " + rights + " ";
            }

            ds.Clear();

            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "dept_name";
                cbl_branch.DataBind();
                cb_branch.Checked = true;
            }
            CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, lbl_branch.Text);
        }
        catch
        {
            CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, lbl_branch.Text);
        }
    }
    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        ds.Clear();

        string build = "";
        string build1 = "";
        string batch = "";
        string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
        string branch = rs.GetSelectedItemsText(cbl_branch);

        if (ddl_batch.Items.Count > 0)
        {
            batch = ddl_batch.SelectedItem.Value;
        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            // ds = d2.BindSem(branch, batch, ddlcollege.SelectedItem.Value);
            string strsql1 = "select distinct duration,first_year_nonsemester  from degree d,department dt where dt.dept_code=d.dept_code and dept_name in ('" + branch + "') and d.college_code in ('" + collegeSel + "')";
            ds = d2.select_method_wo_parameter(strsql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
                    if (dur.Trim() != "")
                    {
                        if (duration < Convert.ToInt32(dur))
                        {
                            duration = Convert.ToInt32(dur);
                        }
                    }
                }
            }
            if (duration != 0)
            {
                for (i = 1; i <= duration; i++)
                {
                    cbl_sem.Items.Add(Convert.ToString(i));
                }
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sem.Items.Count; row++)
                    {
                        cbl_sem.Items[row].Selected = true;
                        cb_sem.Checked = true;
                    }
                    txt_sem.Text = "Sem(" + cbl_sem.Items.Count + ")";
                }
            }
        }
    }
    public void BindSectionDetail()
    {
        try
        {
            cbl_sec.Items.Clear();
            string batch = "";

            int i = 0;
            string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
            string branch = rs.GetSelectedItemsText(cbl_branch);

            if (ddl_batch.Items.Count > 0)
            {
                batch = ddl_batch.SelectedItem.Value;
            }
            string sqlquery = "select distinct sections from registration r,degree d, department dt where r.degree_code=d.degree_code and dt.dept_code=d.dept_code and  batch_year in('" + batch + "') and dept_name in('" + branch + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(sqlquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sec.DataSource = ds;
                cbl_sec.DataTextField = "sections";
                cbl_sec.DataValueField = "sections";
                cbl_sec.DataBind();
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sec.Items.Count; row++)
                    {
                        cbl_sec.Items[row].Selected = true;
                        cb_sec.Checked = true;
                    }
                    txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                }
                else
                {
                    txt_sec.Text = "--Select--";
                }
            }
            else
            {
                txt_sec.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void edu_level()
    {
        string st = "";
        string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);

        cbl_grad.Items.Clear();
        if (collegeSel != string.Empty)
        {
            st = "select distinct edu_level,priority from course where college_code in ('" + collegeSel + "') order by priority";

            ds = d2.select_method_wo_parameter(st, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_grad.DataSource = ds;
                cbl_grad.DataTextField = "edu_level";
                cbl_grad.DataValueField = "edu_level";
                cbl_grad.DataBind();
                cb_grad.Checked = true;

            }
        }
        CallCheckboxChange(cb_grad, cbl_grad, txt_grad, lbl_graduation.Text, lbl_graduation.Text);
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
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
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
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else if (commcount == 0)
            {
                //txt_degree.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
            bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }
    public void cb_sem_checkedchange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_sem.Text = "--Select--";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_sec_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_sec.Checked == true)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Section(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sec.Text = "--Select--";
            cb_sec.Checked = false;
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_sec.Items.Count)
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
                cb_sec.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_sec.Text = "--Select--";
            }
            else
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    protected void collegeCheckChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_clgname, cbl_clgname, txt_clgname, lbl_clgname.Text, lbl_clgname.Text);
        edu_level();
        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void collegeSelectedIndexChange(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_clgname, cbl_clgname, txt_clgname, "Institution Name");
        edu_level();
        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
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
    public void btndetailgo_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        Fpspread2.Visible = false;
        lbl_headernamespd2.Visible = false;
        imgbtn_columsetting.Visible = true;
        div_report.Visible = false;

        string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
        string branch = rs.GetSelectedItemsText(cbl_degree);

        string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudentStatus' and CollegeCode in ('" + collegeSel + "')";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddl_colord.SelectedItem.Text != "Select")
            {
                fpspread1go1();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Kindly Select Report Type";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Kindly Set Report Type";
        }
    }
    public void selectcolumnload()
    {
        string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
        string branch = rs.GetSelectedItemsText(cbl_degree);
        columnname1 = "";
        string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
        int cc = 0;
        string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code in ('" + collegeSel + "') ";
        ds = d2.select_method_wo_parameter(selcol1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int c = 0; c < 1; c++)//ds.Tables[0].Rows.Count
            {
                string value = Convert.ToString(ds.Tables[0].Rows[c]["LinkValue"]);
                if (value != "")
                {
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        for (int k = 0; k < valuesplit.Length; k++)
                        {
                            cc++;
                            colval = Convert.ToString(valuesplit[k]);
                            string c_name1 = columnload1(colval);
                            if (c_name1 != "")
                            {
                                if (columnname1 == "")
                                {
                                    columnname1 = c_name1;
                                }
                                else
                                {
                                    columnname1 = columnname1 + "," + c_name1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public string columnload1(string v)
    {
        string value = "";
        if (colval == "1")
        {
            value = "c.Course_Name";
        }
        if (colval == "2")
        {
            value = "Dt.Dept_Name";
        }
        if (colval == "3")
        {
            value = "a.Batch_Year";
        }
        if (colval == "4")
        {
            value = "a.Current_Semester";
        }
        if (colval == "5")
        {
            value = "a.parent_name";
        }
        if (colval == "6")
        {
            value = "CONVERT(VARCHAR(10),dob,103) as dob ";
        }
        if (colval == "7")
        {
            value = "(Select textval FROM textvaltable T WHERE convert(varchar,parent_occu) = convert(varchar,t.TextCode)) parent_occu";
        }
        if (colval == "8")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,mother_tongue) =  convert(varchar,t.TextCode)) mother_tongue";
        }
        if (colval == "9")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,religion) =  convert(varchar,t.TextCode)) religion";
        }
        if (colval == "10")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,citizen) =  convert(varchar,t.TextCode)) citizen";
        }
        if (colval == "11")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,community) =  convert(varchar,t.TextCode)) community";
        }
        if (colval == "12")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,caste) =  convert(varchar,t.TextCode)) caste";
        }
        if (colval == "13")
        {
            value = "case when isnull(TamilOrginFromAndaman,0)='0' then 'No' else 'Yes' end as  TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            value = "a.visualhandy";
        }
        if (colval == "15")
        {
            value = "a.first_graduate";
        }
        if (colval == "16")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,seattype) =  convert(varchar,t.TextCode)) seattype";
        }
        if (colval == "17")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,co_curricular) =  convert(varchar,t.TextCode)) co_curricular";
        }
        if (colval == "18")
        {
            value = "a.parent_addressP";
        }
        if (colval == "19")
        {
            value = "a.Streetp";
        }
        if (colval == "20")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,Cityp) =  convert(varchar,t.TextCode)) Cityp";
        }
        if (colval == "21")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,parent_statep) =  convert(varchar,t.TextCode))parent_statep";
        }
        if (colval == "22")
        {
            value = " Countryp";//(select textval from textvaltable where TextCode=Countryp)
        }
        if (colval == "23")
        {
            value = "a.Student_Mobile";
        }
        if (colval == "24")
        {
            value = "a.parent_pincodep";
        }
        if (colval == "25")
        {
            value = "a.parent_phnop";
        }
        if (colval == "26")
        {
            value = "case when isnull(MissionaryChild,0)='0' then 'No' else 'Yes' end as MissionaryChild";
        }
        if (colval == "27")
        {
            value = "a.missionarydisc";
        }
        if (colval == "35")
        {
            value = "ElectionID_No";
        }
        //if (colval == "28")
        //{
        //    value = "Institute_name";
        //}
        //if (colval == "29")
        //{
        //    value = "Part1Language";
        //}
        //if (colval == "30")
        //{
        //    value = "Part2Language";
        //}
        //if (colval == "31")
        //{
        //    value = "university_code";
        //}
        if (colval == "48")
        {
            value = "case when isnull(CampusReq,0)='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "49")
        {
            value = "case when isnull(handy,0)='0' then 'No' else 'Yes' end as handy";
        }
        if (colval == "50")
        {
            value = "case when  isnull(DistinctSport,0)='0' then 'No' else (Select textval FROM textvaltable T WHERE  convert(varchar,DistinctSport) =  convert(varchar,t.TextCode))   end as DistinctSport";
        }
        if (colval == "51")
        {
            value = "case when isnull(islearningdis,0)='0' then 'No' else 'Yes' end as islearningdis";
        }
        if (colval == "52")
        {
            value = "isdisabledisc";
        }
        if (colval == "53")
        {
            value = "case when isnull(isdisable,0)='0' then 'No' else 'Yes' end as isdisable";
        }
        //if (colval == "54")
        //{
        //    value = "r.Stud_Name";
        //}
        //if (colval == "55")
        //{
        //    value = "r.Roll_No";
        //}
        if (colval == "56")
        {
            value = "StuPer_Id";
        }
        //if (colval == "57")
        //{
        //    value = "r.Reg_No";
        //}
        if (colval == "58")
        {
            value = " roll_admit";
        }
        if (colval == "59")
        {
            value = "app_formno";
        }
        if (colval == "60")
        {
            value = " Sections";
        }
        if (colval == "61")
        {
            value = "case when sex='0' then 'Male' else 'Female' end as sex";
        }
        if (colval == "62")
        {
            value = "(Select textval FROM textvaltable T WHERE  convert(varchar,bldgrp) =  convert(varchar,t.TextCode)) bldgrp";
        }
        if (colval == "63")
        {
            value = "r.stud_type";
        }
        if (colval == "64")
        {
            value = "case when isnull(IsExService,0)='0' then 'No' else 'Yes' end as IsExService";
        }
        if (colval == "65")
        {
            value = "case when isnull(CampusReq,0)='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "66")
        {
            value = "case when isnull(isdonar,0)='0' then 'No' else 'Yes' end as isdonar";
        }
        if (colval == "67")
        {
            value = "case when isnull(ReserveCategory,0)='0' then 'No' else 'Yes' end as  ReserveCategory";
        }
        if (colval == "68")
        {
            value = "case when isnull(EconBackword,0)='0' then 'No' else 'Yes' end as EconBackword";
        }
        if (colval == "69")
        {
            value = "case when isnull(parentoldstud,0)='0' then 'No' else 'Yes' end as parentoldstud";
        }
        if (colval == "70")
        {
            value = "case when isnull(IsDrivingLic,0)='0' then 'No' else 'Yes' end as IsDrivingLic";
        }
        if (colval == "71")
        {
            value = "Driving_details";
        }
        if (colval == "72")
        {
            value = "case when isnull(tutionfee_waiver,0)='0' then 'No' else 'Yes' end as tutionfee_waiver";
        }
        if (colval == "73")
        {
            value = "case when isnull(IsInsurance,0)='0' then 'No' else 'Yes' end as IsInsurance";
        }
        if (colval == "74")
        {
            value = "ExsRank";
        }
        if (colval == "75")
        {
            value = "ExSPlace";
        }
        if (colval == "76")
        {
            value = "ExsNumber";
        }
        if (colval == "77")
        {
            value = "Insurance_Amount";
        }
        if (colval == "78")
        {
            value = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            value = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            value = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            value = "CONVERT(VARCHAR(10),date_applied,103) as date_applied";
        }
        if (colval == "82")
        {
            value = "alter_mobileno";
        }
        if (colval == "83")
        {
            value = " (select textval from textvaltable where convert(varchar,TextCode)=convert(varchar,SubCaste)) SubCaste ";
        }
        if (colval == "84")
        {
            value = "(Select textval FROM textvaltable T WHERE convert(varchar,parent_income) = convert(varchar,t.TextCode)) parent_income";
        }
        if (colval == "85")
        {
            value = "parentF_Mobile";
        }
        if (colval == "86")
        {
            value = "parentidp";
        }
        if (colval == "87")
        {
            value = "mother";
        }
        if (colval == "88")
        {
            value = "(Select textval FROM textvaltable T WHERE convert(varchar,mIncome) = convert(varchar,t.TextCode)) mIncome";
        }
        if (colval == "89")
        {
            value = "parentM_Mobile";
        }
        if (colval == "90")
        {
            value = "emailM";
        }
        if (colval == "91")
        {
            value = "guardian_name";
        }
        if (colval == "92")
        {
            value = "guardian_mobile";
        }
        if (colval == "93")
        {
            value = "gurdian_email";
        }
        if (colval == "94")
        {
            value = "place_birth";
        }
        if (colval == "95")
        {
            value = "Aadharcard_no";
        }
        if (colval == "96")
        {
            value = "(Select textval FROM textvaltable T WHERE convert(varchar,motherocc) = convert(varchar,t.TextCode)) motherocc";
        }
        if (colval == "108")
        {
            value = "a.parent_addressC";
        }
        if (colval == "109")
        {
            value = "a.Streetc";
        }
        if (colval == "110")
        {
            value = "a.Cityc";
        }
        if (colval == "111")
        {
            value = "(Select textval FROM textvaltable T WHERE convert(varchar,parent_statec) = convert(varchar,t.TextCode)) parent_statec";
        }
        if (colval == "112")
        {
            value = "   Countryc";//(select textval from textvaltable where TextCode=Countryc)
        }
        if (colval == "113")
        {
            value = "a.parent_pincodec";
        }
        if (colval == "122")
        {
            value = " Boarding";
        }
        if (colval == "123")
        {
            value = "vehid";
        }
        if (colval == "43")
        {
            value = "case when r.Mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' when r.mode='4' then 'IrRegular' end Mode ";
        }
        //26.08.16
        if (colval == "124")
        {
            value = "StuPer_Id";
        }
        if (colval == "125")
        {
            value = "idmark";
        }
        if (colval == "126")
        {
            value = " Quota";
        }
        if (colval == "127")
        {
            value = " convert(varchar(10), fatherdob,103) fatherdob";
        }
        if (colval == "128")
        {
            value = " FocDesign";
        }
        if (colval == "129")
        {
            value = " FocDept";
        }
        if (colval == "130")
        {
            value = " FocDetails";
        }
        if (colval == "131")
        {
            value = " Fat_off_addressP";
        }
        if (colval == "132")
        {
            value = " Fat_off_street";
        }
        if (colval == "133")
        {
            value = " ftown";
        }
        if (colval == "134")
        {
            value = "  fcity";// (select textval from textvaltable where TextCode=fcity)
        }
        if (colval == "135")
        {
            value = "  Fat_off_state";//(select textval from textvaltable where TextCode=Fat_off_state)
        }
        if (colval == "136")
        {
            value = " Fat_off_country";//(select textval from textvaltable where TextCode=Fat_off_country) 
        }
        if (colval == "137")
        {
            value = " Fat_off_pincode";
        }
        //mother
        if (colval == "138")
        {
            value = " convert(varchar(10), motherdob,103) motherdob";
        }
        if (colval == "139")
        {
            value = " MocDesign";
        }
        if (colval == "140")
        {
            value = " MocDept";
        }
        if (colval == "141")
        {
            value = " MocDetails";
        }
        if (colval == "142")
        {
            value = " mot_off_address1";
        }
        if (colval == "143")
        {
            value = " mot_off_address2";
        }
        if (colval == "144")
        {
            value = " mtown";
        }
        if (colval == "145")
        {
            value = "  mcity";//(select textval from textvaltable where TextCode=mcity)
        }
        if (colval == "146")
        {
            value = "  mot_off_state";//(select textval from textvaltable where TextCode=mot_off_state)
        }
        if (colval == "147")
        {
            value = "  mot_off_country";//(select textval from textvaltable where TextCode=mot_off_country)
        }
        if (colval == "148")
        {
            value = " mot_off_pincode";
        }
        //guardian
        if (colval == "149")
        {
            value = " convert(varchar(10), Guardiandob,103) Guardiandob";
        }
        if (colval == "150")
        {
            value = " GocDesign";
        }
        if (colval == "151")
        {
            value = " GocDept";
        }
        if (colval == "152")
        {
            value = " GocDetails";
        }
        if (colval == "153")
        {
            value = " gur_off_address1";
        }
        if (colval == "154")
        {
            value = " gur_off_address2";
        }
        if (colval == "155")
        {
            value = " gtown";
        }
        if (colval == "156")
        {
            value = "  gcity";//(select textval from textvaltable where TextCode=gcity)
        }
        if (colval == "157")
        {
            value = " gur_off_state";//(select textval from textvaltable where TextCode=gur_off_state) 
        }
        if (colval == "158")
        {
            value = "  gur_off_country";//(select textval from textvaltable where TextCode=gur_off_country)
        }
        if (colval == "159")
        {
            value = " gur_off_pincode";
        }
        if (colval == "162")
        {
            value = " (select Reg_No from Vehicle_Master where Veh_ID=ISNULL(r.VehID,0))Vehicle_Reg_No ";
        }
        if (colval == "163")
        {
            value = " (select Veh_Type from Vehicle_Master where Veh_ID=ISNULL(r.VehID,0))Veh_Type ";
        }
        if (colval == "164")
        {
            value = "LastTCNo";
        }
        if (colval == "165")
        {
            value = " convert(varchar(10), LastTCDate,103) LastTCDate";
        }
        if (colval == "32")
        {
            value = "spouse_name";
        }
        if (colval == "33")
        {
            value = "convert(varchar(10), adm_date,103) adm_date";
        }
        if (colval == "34")
        {
            value = "cf.collname";
        }
        if (colval == "36")
        {
            value = " case when isnull(direct_refer ,0)=0 then 'Direct' when isnull(direct_refer ,0)=1 then 'Staff' when isnull(direct_refer ,0)=3 then 'Student' when isnull(direct_refer ,0)=2 then 'Other' end  as  direct_refer";
        }
        if (colval == "37")
        {
            value = " case when isnull(direct_refer ,0)=1 then  (select staff_name from staffmaster where staff_code=refer_stcode) when isnull(direct_refer ,0)=3 then  (select stud_name from applyn where convert(varchar,app_no)=refer_stcode) end  refer_stcode";//modified abarna 18.06.2018
        }

        return value;
    }
    public void fpspread1go1()
    {
        try
        {
            string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
            string branchSel = rs.GetSelectedItemsText(cbl_degree);

            RollAndRegSettings();
            string orderStr = string.Empty;
            if (roll == 0)
                orderStr = " Order by roll_no,reg_no,roll_admit ";
            else if (roll == 1)
                orderStr = " Order by roll_no,reg_no,roll_admit ";
            else if (roll == 2)
                orderStr = " Order by roll_no ";
            else if (roll == 3)
                orderStr = " Order by reg_no ";
            else if (roll == 4)
                orderStr = " Order by roll_admit ";
            else if (roll == 5)
                orderStr = " Order by roll_no,reg_no ";
            else if (roll == 6)
                orderStr = " Order by reg_no,roll_admit ";
            else if (roll == 7)
                orderStr = " Order by roll_no,roll_admit ";
            lbl_headernamespd2.Visible = true;
            Fpspread2.Visible = true;
            div_report.Visible = true;
            int val = 0;
            int count = 0;
            int i = 0;
            string header = "";
            string sectionvalue = "";
            string Batch_tagvalue = "";
            string dept_tagvalue = "";
            string sem_tagvalue = "";
            string sec_tagvalue = "";
            Batch_tagvalue = Convert.ToString(ddl_batch.SelectedItem.Value);
            dept_tagvalue = returnwithsinglecodevalue(cbl_branch);
            sem_tagvalue = returnwithsinglecodevalue(cbl_sem);
            sec_tagvalue = returnwithsinglecodevalue(cbl_sec);
            if (sec_tagvalue != "")
            {
                sectionvalue = " AND ISNULL( r.Sections,'') in('','" + sec_tagvalue + "')";
            }
            else
            {
                sectionvalue = "";
            }
            if (dept_tagvalue.Trim() != "" && dept_tagvalue.Trim() != "" && sem_tagvalue.Trim() != "")
            {
                Fpspread2.Sheets[0].Visible = true;
                Fpspread2.Sheets[0].RowHeader.Visible = false;
                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].AutoPostBack = true;
                Fpspread2.Sheets[0].RowCount = 0;
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.Sheets[0].ColumnCount = 3;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                loadlcolumns();
                string query = "";
                columnname1 = "";
                selectcolumnload();
                if (columnname1 != "")
                {
                    columnname1 = "," + columnname1;
                }
                string selectquery = "";
                if (ddlOrderby.SelectedIndex == 0)
                {
                    selectquery += "  order by d.Degree_Code,isnull(r.Sections,''),ltrim (r.stud_name) asc ";
                }
                else if (ddlOrderby.SelectedIndex == 1)
                {
                    selectquery += "  order by d.Degree_Code,isnull(r.Sections,''),r.adm_date,ltrim (r.stud_name) asc ";
                }
                else
                {
                    if (rblGen.SelectedIndex == 0)
                    {
                        selectquery += "  order by d.Degree_Code,isnull(r.Sections,''),a.sex,ltrim (r.stud_name) asc ";
                    }
                    else if (rblGen.SelectedIndex == 1)
                    {
                        selectquery += "  order by d.Degree_Code,isnull(r.Sections,''),a.sex desc,ltrim (r.stud_name) asc ";
                    }
                }
                string ccc = "";
                string debar = "";
                string disc = "";
                string commondist = "";
                if (chkinclude.Checked == true)
                {
                    #region include discontinue
                    if (cblinclude.Items.Count > 0)
                    {
                        for (int k = 0; k < cblinclude.Items.Count; k++)
                        {
                            if (cblinclude.Items[k].Selected == true)
                            {
                                if (cblinclude.Items[k].Value == "1")
                                {
                                    ccc = " r.cc=1";
                                }
                                if (cblinclude.Items[k].Value == "2")
                                {
                                    debar = "  r.Exam_Flag like '%debar'";
                                }
                                if (cblinclude.Items[k].Value == "3")
                                {
                                    disc = "  r.DelFlag=1";
                                }
                            }
                        }
                        if (cb_onlydis.Checked == false)
                        {
                            if (ccc != "" && debar == "" && disc == "")
                                commondist = " and (" + ccc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
                            if (ccc == "" && debar != "" && disc == "")
                                commondist = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                            if (ccc == "" && debar == "" && disc != "")
                                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                            if (ccc != "" && debar != "" && disc == "")
                                commondist = " and (" + ccc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                            if (ccc == "" && debar != "" && disc != "")
                                commondist = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";
                            if (ccc != "" && debar == "" && disc != "")
                                commondist = " and (" + ccc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";
                            else if (ccc == "" && debar == "" && disc == "")
                                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
                            if (ccc != "" && debar != "" && disc != "")
                                commondist = "";
                        }
                        if (cb_onlydis.Checked == true)
                        {
                            if (debar.Trim() != "" && disc.Trim() != "" && ccc.Trim() != "")
                            {
                                commondist = " and (" + ccc + " or " + debar + " or " + disc + ")";
                            }
                            else if (debar.Trim() != "" && ccc.Trim() != "")
                            {
                                commondist = " and (" + ccc + " or " + debar + ")";
                            }
                            else if (disc.Trim() != "" && ccc.Trim() != "")
                            {
                                commondist = " and (" + ccc + " or " + disc + ")";
                            }
                            else if (debar.Trim() != "" && disc.Trim() != "")//01.03.17 barath
                            {
                                commondist = " and  (" + debar + " or " + disc + ")";
                            }
                            else if (ccc.Trim() != "")
                            {
                                commondist = " and (" + ccc + ")";
                            }
                            else if (debar.Trim() != "")
                            {
                                commondist = " and (" + debar + ")";
                            }
                            else if (disc.Trim() != "")
                            {
                                commondist = " and (" + disc + ")";
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    commondist = " and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR'";
                }
                //transfer students
                if (!cbTransfer.Checked)
                {
                    //and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR'
                    query = " select r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no " + columnname1 + " from Registration r,applyn a,degree d,Department dt,Course C,collinfo cf where cf.college_code=r.college_code and r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1'  and dt.dept_name in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and r.college_code in('" + collegeSel + "')";
                }
                else
                {
                    query = " select r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no " + columnname1 + " from Registration r,applyn a,degree d,Department dt,Course C,collinfo cf,ST_Student_Transfer st where cf.college_code=r.college_code and r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no=st.appno and a.app_no=st.appno and isconfirm ='1' and admission_status ='1'  and dt.dept_name in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and r.college_code in('" + collegeSel + "')";
                }
                if (sem_tagvalue.Trim() != "")
                {
                    query = query + " and  r.Current_Semester in('" + sem_tagvalue + "') ";
                }
                if (cbDateWise.Checked && !cbTransfer.Checked)
                {
                    string[] fromdate = txt_fromdate.Text.Split('/');
                    string[] todate = txt_todate.Text.Split('/');
                    query += " and (r.Adm_Date >='" + (fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2]) + "' and r.Adm_Date<='" + (todate[1] + "/" + todate[0] + "/" + todate[2]) + "') ";
                }
                else if (cbDateWise.Checked && cbTransfer.Checked)
                {
                    string[] fromdate = txt_fromdate.Text.Split('/');
                    string[] todate = txt_todate.Text.Split('/');

                    query += " and (st.Transferdate >='" + (fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2]) + "' and st.Transferdate<='" + (todate[1] + "/" + todate[0] + "/" + todate[2]) + "') ";
                }
                query = query + "" + commondist + sectionvalue + selectquery + "";
                //query = query + " sELECT instaddress,instpincode,(Select textval FROM textvaltable T WHERE isnull(Xmedium,0) = convert(varchar(20), t.TextCode)) Xmedium,(Select textval FROM textvaltable T WHERE isnull(medium,0) = convert(varchar(20), t.TextCode)) medium,percentage,securedmark,totalmark,passyear,passmonth,case when Vocational_stream='0' then 'No' else 'Yes' end as Vocational_stream,markPriority,Cut_Of_Mark ,a.App_No,uni_state,type_semester,(Select textval FROM textvaltable T WHERE TextCode= isnull(university_code,0)) university_code,ISNULL(pt.TExtVal,'') Part2Language,(Select textval FROM textvaltable T WHERE Part1Language = t.TextCode) Part1Language,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear FROM Stud_prev_details P INNER JOIN applyn A ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language  Where  p.app_no = a.app_no and IsConfirm='1' and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and a.college_code in ('" + collegeSel + "') ";

                query = query + "sELECT instaddress,instpincode,(Select textval FROM textvaltable T WHERE isnull(Xmedium,0) = convert(varchar(20), t.TextCode)) Xmedium,(Select textval FROM textvaltable T WHERE isnull(medium,0) = convert(varchar(20), t.TextCode)) medium,percentage,securedmark,totalmark,passyear,passmonth,case when Vocational_stream='0' then 'No' else 'Yes' end as Vocational_stream,markPriority,Cut_Of_Mark ,a.App_No,uni_state,type_semester,(Select textval FROM textvaltable T WHERE TextCode= isnull(university_code,0)) university_code,ISNULL(pt.TExtVal,'') Part2Language,(Select textval FROM textvaltable T WHERE Part1Language = t.TextCode) Part1Language,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear FROM (applyn A INNER JOIN Stud_prev_details P   ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language) INNER JOIN (Degree d inner join department dt on d.Dept_Code=dt.Dept_Code ) on a.degree_code = d.Degree_Code  Where  p.app_no = a.app_no and IsConfirm='1' and dt.Dept_Name in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and a.college_code in ('" + collegeSel + "') ";
                if (sem_tagvalue.Trim() != "")
                {
                    query = query + " and  a.Current_Semester in('" + sem_tagvalue + "') ";
                }
                query = query + " select * from StudCertDetails_New s,applyn a,degree d,department dt where a.degree_code =d.degree_code and dt.dept_code=d.dept_code and  a.App_No=s.App_No  and dt.dept_name in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and a.college_code in ('" + collegeSel + "')";
                if (sem_tagvalue.Trim() != "")
                {
                    query = query + " and  a.Current_Semester in('" + sem_tagvalue + "') ";
                }
                query = query + " select s.App_No ,AccFor,AccNo,DebitCardNo,IFSCCode,(select textval from textvaltable where TextCode=isnull(s.BankName,0)) BankName,Branch,BankAdd,MICRCode from studbankdet s,applyn a,degree d,department dt where a.degree_code =d.degree_code and dt.dept_code=d.dept_code and a.App_No=s.App_No   and dt.dept_name in ('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and a.college_code in ('" + collegeSel + "')  ";
                if (sem_tagvalue.Trim() != "")
                {
                    query = query + " and  a.Current_Semester in('" + sem_tagvalue + "') ";
                }
                query = query + " select * from stud_relation s,applyn a,degree d,department dt where a.degree_code =d.degree_code and dt.dept_code=d.dept_code and a.App_No=s.application_no   and dt.dept_name in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and a.college_code in ('" + collegeSel + "') ";
                if (sem_tagvalue.Trim() != "")
                {
                    query = query + " and  a.Current_Semester in('" + sem_tagvalue + "') ";
                }
                query = query + " select TextCode,textval from TextValTable where TextCriteria in ('state','coun','city')";
                query = query + " select HostelName,APP_No from HT_HostelRegistration hr,HM_HostelMaster hm where hr.HostelMasterFK=hm.HostelMasterPK ";
                query = query + " SELECT s.app_no,registerno,max_marks,pass_month,pass_year,noofattempt,acual_marks,psubjectno from Stud_prev_details s,perv_marks_history p where s.course_entno =p.course_entno";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "text");
                lblerror.Visible = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    div_report.Visible = true;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Column.Visible = false;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "AppNo";
                    int cc = 2;
                    int j = 0;
                    DataSet dss = new DataSet();
                    string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
                    string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code in ('" + collegeSel + "') ";
                    selcol1 += "  select distinct CertName,MasterValue from CertMasterDet c,CO_MasterValues co where c.CertName=co.MasterCode and MasterCriteria='CertificateName' and collegecode in ('" + collegeSel + "') and categorytype='6'";
                    selcol1 += " select App_no,CertificateId from StudCertDetails_New ";
                    selcol1 += " select LinkValue from New_InsSettings where LinkName='SubjectPriority' and user_code ='" + usercode + "' and college_code  in ('" + collegeSel + "')";
                    dss.Clear();
                    dss = d2.select_method_wo_parameter(selcol1, "Text");
                    //Hashtable certdet = new Hashtable();
                    foreach (DataRow dr in dss.Tables[1].Rows)
                    {
                        if (!certdet.Contains(Convert.ToString(dr["CertName"])))
                            certdet.Add(Convert.ToString(dr["CertName"]), Convert.ToString(dr["MasterValue"]));
                    }
                    if (dss.Tables[3].Rows.Count > 0)
                    {
                        string[] LinkValue = Convert.ToString(dss.Tables[3].Rows[0]["LinkValue"]).Split(',');
                        string ss = "  select textval,textcode from textvaltable where textcriteria='subje' and college_code in ('" + collegeSel + "') and TextCode in ('" + Convert.ToString(dss.Tables[3].Rows[0]["LinkValue"]).Replace(",", "','") + "') ";
                        DataSet pcm_ds = new DataSet();
                        pcm_ds = d2.select_method_wo_parameter(ss, "text");
                        if (pcm_ds.Tables[0].Rows.Count > 0)
                            foreach (DataRow dr in pcm_ds.Tables[0].Rows)
                            {
                                if (!pcmmark_hash.Contains(Convert.ToString(dr["textcode"])))
                                    pcmmark_hash.Add(Convert.ToString(dr["textcode"]), Convert.ToString(dr["textval"]));
                            }
                    }
                    if (dss.Tables.Count > 0)
                    {
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            for (int c = 0; c < 1; c++)//dss.Tables[0].Rows.Count
                            {
                                string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                                if (value != "")
                                {
                                    string[] valuesplit = value.Split(',');
                                    if (valuesplit.Length > 0)
                                    {
                                        for (int k = 0; k < valuesplit.Length; k++)
                                        {
                                            colval = Convert.ToString(valuesplit[k]);
                                            loadtext();
                                            if (loadval.Trim().ToUpper() != "")
                                            {
                                                cc++;
                                                Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = loadval;
                                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = printval;
                                            }
                                            else
                                            {
                                                cc++;
                                                Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                                if (certdet.Contains(colval))
                                                {
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = Convert.ToString(certdet[colval]);
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Note = "Certificate";
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = colval;
                                                }
                                                if (pcmmark_hash.Contains(colval))
                                                {
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = Convert.ToString(pcmmark_hash[colval]);
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Note = "PCM";
                                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = colval;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            Fpspread2.Visible = false;
                            div_report.Visible = false;
                            lbl_headernamespd2.Visible = false;
                            return;
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Set Column Order";
                        Fpspread2.Visible = false;
                        div_report.Visible = false;
                        lbl_headernamespd2.Visible = false;
                        return;
                    }
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        count++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]); ;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Locked = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Locked = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        string admi_status = Convert.ToString(ds.Tables[0].Rows[i]["Admission_Status"]);
                        string delflag = Convert.ToString(ds.Tables[0].Rows[i]["DelFlag"]);
                        string examflg = Convert.ToString(ds.Tables[0].Rows[i]["Exam_Flag"]);
                        string coursecomp = Convert.ToString(ds.Tables[0].Rows[i]["CC"]);
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#F2C77D");
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#F2C77D");
                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#F2C77D");
                        cc = 2;
                        string text = "";
                        DataView dv = new DataView();
                        DataView dv1 = new DataView();
                        string linkname = Convert.ToString(ddl_colord.SelectedItem.Text);
                        DataSet dscol = new DataSet();
                        for (int k = 3; k < Fpspread2.Sheets[0].ColumnCount; k++)
                        {
                            cc++;
                            string col = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                            if (col == "type_semester" || col == "Institute_name" || col == "isgrade" || col == "Part1Language" || col == "Part2Language" || col == "university_code" || col == "instaddress" || col == "Xmedium" || col == "medium" || col == "percentage" || col == "securedmark" || col == "totalmark" || col == "passyear" || col == "passmonth" || col == "Vocational_stream" || col == "markPriority" || col == "Cut_Of_Mark" || col == "instpincode")
                            {
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "'";
                                        dv = ds.Tables[1].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            text = Convert.ToString(dv[0][col]);
                                        }
                                        else
                                        {
                                            text = "";
                                        }
                                    }
                                }
                                else
                                {
                                    text = "";
                                }
                            }
                            else if (col == "AccNo" || col == "DebitCardNo" || col == "IFSCCode" || col == "BankName" || col == "Branch")
                            {
                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    ds.Tables[3].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                    dv1 = ds.Tables[3].DefaultView;
                                    if (dv1.Count > 0)
                                    {
                                        text = Convert.ToString(dv1[0][col]);
                                    }
                                    else
                                    {
                                        text = "";
                                    }
                                }
                                else
                                {
                                    text = "";
                                }
                            }
                            else if (col == "name_roll" || col == "relationship" || col == "isstaff")
                            {
                                if (ds.Tables[4].Rows.Count > 0)
                                {
                                    ds.Tables[4].DefaultView.RowFilter = "application_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                    dv1 = ds.Tables[4].DefaultView;
                                    if (dv1.Count > 0)
                                    {
                                        if (col == "isstaff")
                                        {
                                            text = Convert.ToString(dv1[0][col]);
                                            if (text == "0")
                                            {
                                                text = "Student";
                                            }
                                            else
                                            {
                                                text = "Staff";
                                            }
                                        }
                                        else
                                        {
                                            text = Convert.ToString(dv1[0][col]);
                                        }
                                    }
                                    else
                                    {
                                        text = "";
                                    }
                                }
                                else
                                {
                                    text = "";
                                }
                            }
                            else if (col == "Hostel Name")
                            {
                                if (ds.Tables[6].Rows.Count > 0)
                                {
                                    ds.Tables[6].DefaultView.RowFilter = "APP_No='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                    dv1 = ds.Tables[6].DefaultView;
                                    if (dv1.Count > 0)
                                    {
                                        text = Convert.ToString(dv1[0]["HostelName"]);
                                    }
                                    else
                                    {
                                        text = "";
                                    }
                                }
                                else
                                {
                                    text = "";
                                }
                            }
                            else
                            {
                                string Note = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Note);
                                string tag = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                if (Note.Trim() == "")
                                {
                                    text = Convert.ToString(ds.Tables[0].Rows[i][col]);
                                }
                                if (Note == "Certificate")
                                {
                                    if (dss.Tables[2].Rows.Count > 0)
                                    {
                                        DataView cert_dv = new DataView();
                                        dss.Tables[2].DefaultView.RowFilter = " app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' and CertificateId='" + tag + "'";
                                        cert_dv = dss.Tables[2].DefaultView;
                                        if (cert_dv.Count > 0)
                                        {
                                            text = "Yes";
                                        }
                                    }
                                    else { text = "No"; }
                                }
                                if (Note == "PCM")
                                {
                                    if (ds.Tables[7].Rows.Count > 0)
                                    {
                                        DataView dvm_dv = new DataView();
                                        ds.Tables[7].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' and psubjectno='" + tag + "'";
                                        dvm_dv = ds.Tables[7].DefaultView;
                                        if (dvm_dv.Count > 0)
                                        {
                                            text = Convert.ToString(dvm_dv[0]["acual_marks"]);
                                        }
                                        else
                                        {
                                            text = "";
                                        }
                                    }
                                }
                            }
                            if (col == "visualhandy")
                            {
                                if (text == "0")
                                {
                                    text = "No";
                                }
                                else if (text == "1")
                                {
                                    text = "Yes";
                                }
                            }
                            if (col == "first_graduate")
                            {
                                if (text == "0")
                                {
                                    text = "No";
                                }
                                else if (text == "1")
                                {
                                    text = "Yes";
                                }
                            }
                            if (col == "Countryp" || col == "Countryc" || col == "Fat_off_state" || col == "Fat_off_country" || col == "fcity" || col == "mot_off_state" || col == "mot_off_country" || col == "mcity" || col == "gur_off_state" || col == "gur_off_country" || col == "gcity")
                            {
                                if (ds.Tables[5].Rows.Count > 0)
                                {
                                    string colval = Convert.ToString(ds.Tables[0].Rows[i]["" + col + ""]);
                                    if (colval.Trim() == "")
                                        colval = "0";
                                    bool checkno = false;
                                    checkno = checknumber(colval);
                                    if (checkno == true)
                                    {
                                        ds.Tables[5].DefaultView.RowFilter = "Textcode = '" + colval + "'";
                                        dv1 = ds.Tables[5].DefaultView;
                                        if (dv1.Count > 0)
                                        {
                                            text = Convert.ToString(dv1[0]["textval"]);
                                        }
                                        else
                                        {
                                            text = "";
                                        }
                                    }
                                    else
                                    {
                                        text = colval;
                                    }
                                }
                                else
                                {
                                    text = "";
                                }
                            }
                            if (text == "0")
                            {
                                text = "";
                            }
                            if (text == "")
                            {
                                text = "";
                            }
                            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].CellType = txt;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = text;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Column.Width = 180;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                            if (col == "Current_Semester")
                            {
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                            }
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].BackColor = ColorTranslator.FromHtml("#F2C77D");
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].VerticalAlign = VerticalAlign.Middle;
                        }
                        if (delflag == "1")
                        {
                            Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                            Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].ForeColor = Color.IndianRed;
                        }
                        if (examflg.ToLower() == "debar")
                        {
                            Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                            Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].ForeColor = Color.Blue;
                        }
                        if (coursecomp == "1" || coursecomp == "True")
                        {
                            Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                            Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].ForeColor = Color.Green;
                        }
                    }
                    Fpspread2.Columns[1].Visible = false;
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Visible = true;
                    imgbtn_columsetting.Visible = true;
                    ds.Clear();
                    ds.Dispose();
                    GC.SuppressFinalize(this);
                }
                else
                {
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Records Found";
                    Fpspread2.Visible = false;
                    div_report.Visible = false;
                    lbl_headernamespd2.Visible = false;
                }
            }
            else
            {
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                lbl_alert.Text = " Please Select All Fields";
                Fpspread2.Visible = false;
                div_report.Visible = false;
                lbl_headernamespd2.Visible = false;
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = ex.ToString();
            Fpspread2.Visible = false;
            div_report.Visible = false;
            lbl_headernamespd2.Visible = false;
        }
    }
    protected void btnok_click(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedItem.Text != "Select")
        {
            if (txtcolumn.Text.Trim() != "")
            {
                poppernew.Visible = false;
                savecolumnorder();
                //fpspread1go1();
                lblalerterr.Visible = false;
            }
            else
            {
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please select atleast one colunm then proceed!";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Select Report Type";
        }
    }
    public void imgbtn_all_Click(object sender, EventArgs e)
    {
        ddl_coltypeadd.SelectedIndex = 0;
        poppernew.Visible = true;
        load();
        lb_selectcolumn.ClearSelection();
        txtcolumn.Text = "";
        ItemList.Clear();
        Itemindex.Clear();
        txtcolumn.Height = 100;
    }
    protected void btnclose_click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }
    public void load()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Student Name", "54"));
        lb_selectcolumn.Items.Add(new ListItem("Roll No", "55"));
        lb_selectcolumn.Items.Add(new ListItem("Reg No", "57"));
        lb_selectcolumn.Items.Add(new ListItem("Admission No", "58"));
        lb_selectcolumn.Items.Add(new ListItem("Application No", "59"));
        lb_selectcolumn.Items.Add(new ListItem("Applied Date", "81"));
        lb_selectcolumn.Items.Add(new ListItem("Admitted Date", "33"));//08.05.17
        lb_selectcolumn.Items.Add(new ListItem("College Name", "34"));//08.05.17
        lb_selectcolumn.Items.Add(new ListItem("Batch", "3"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_degree.Text, "1"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_branch.Text, "2"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_org_sem.Text, "4"));
        lb_selectcolumn.Items.Add(new ListItem("Section", "60"));
        lb_selectcolumn.Items.Add(new ListItem("SeatType", "16"));
        lb_selectcolumn.Items.Add(new ListItem("Student Type", "63"));
        lb_selectcolumn.Items.Add(new ListItem("Hostel Name", "161"));
        lb_selectcolumn.Items.Add(new ListItem("Student EmailID", "124"));
        lb_selectcolumn.Items.Add(new ListItem("Identification Mark", "125"));
        lb_selectcolumn.Items.Add(new ListItem("Quota", "126"));
        lb_selectcolumn.Items.Add(new ListItem("Mode", "43"));
        lb_selectcolumn.Items.Add(new ListItem("Boarding", "122"));
        lb_selectcolumn.Items.Add(new ListItem("Vehicle Id", "123"));
        lb_selectcolumn.Items.Add(new ListItem("Vehicle Type", "162"));
        lb_selectcolumn.Items.Add(new ListItem("Vehicle Reg No", "163"));
        lb_selectcolumn.Items.Add(new ListItem("Gender", "61"));
        lb_selectcolumn.Items.Add(new ListItem("DOB", "6"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Group", "62"));
        lb_selectcolumn.Items.Add(new ListItem("Father Name", "5"));
        lb_selectcolumn.Items.Add(new ListItem("Father Income", "84"));
        lb_selectcolumn.Items.Add(new ListItem("Father Occupation", "7"));
        lb_selectcolumn.Items.Add(new ListItem("Father Mob No", "85"));
        lb_selectcolumn.Items.Add(new ListItem("Father Email Id", "86"));
        lb_selectcolumn.Items.Add(new ListItem("Father DOB", "127"));
        lb_selectcolumn.Items.Add(new ListItem("Father Designation", "128"));
        lb_selectcolumn.Items.Add(new ListItem("Father Department", "129"));
        lb_selectcolumn.Items.Add(new ListItem("Father Office", "130"));
        lb_selectcolumn.Items.Add(new ListItem("Father Office Address", "131"));
        lb_selectcolumn.Items.Add(new ListItem("Father Street", "132"));
        lb_selectcolumn.Items.Add(new ListItem("Father Town", "133"));
        lb_selectcolumn.Items.Add(new ListItem("Father City", "134"));
        lb_selectcolumn.Items.Add(new ListItem("Father State", "135"));
        lb_selectcolumn.Items.Add(new ListItem("Father Country", "136"));
        lb_selectcolumn.Items.Add(new ListItem("Father Pincode", "137"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Name", "87"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Income", "88"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Occupation", "96"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Mob No", "89"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Email Id", "90"));
        lb_selectcolumn.Items.Add(new ListItem("Mother DOB", "138"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Designation", "139"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Department", "140"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Office", "141"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Office Address", "142"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Street", "143"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Town", "144"));
        lb_selectcolumn.Items.Add(new ListItem("Mother City", "145"));
        lb_selectcolumn.Items.Add(new ListItem("Mother State", "146"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Country", "147"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Pincode", "148"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Name", "91"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Email Id", "92"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Mob No", "93"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian DOB", "149"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Designation", "150"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Department", "151"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Office", "152"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Office Address", "153"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Street", "154"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Town", "155"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian City", "156"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian State", "157"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Country", "158"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Pincode", "159"));
        lb_selectcolumn.Items.Add(new ListItem("Place Of Birth", "94"));
        lb_selectcolumn.Items.Add(new ListItem("Adhaar Card No", "95"));
        lb_selectcolumn.Items.Add(new ListItem("Voter ID", "35"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Tongue", "8"));
        lb_selectcolumn.Items.Add(new ListItem("Religion", "9"));
        lb_selectcolumn.Items.Add(new ListItem("Community", "11"));
        lb_selectcolumn.Items.Add(new ListItem("Caste", "12"));
        lb_selectcolumn.Items.Add(new ListItem("Sub Caste", "83"));
        lb_selectcolumn.Items.Add(new ListItem("Citizen", "10"));
        lb_selectcolumn.Items.Add(new ListItem("TamilOrginFromAndaman", "13"));
        lb_selectcolumn.Items.Add(new ListItem("Ex-serviceman", "64"));
        lb_selectcolumn.Items.Add(new ListItem("Rank", "74"));
        lb_selectcolumn.Items.Add(new ListItem("Place", "75"));
        lb_selectcolumn.Items.Add(new ListItem("Number", "76"));
        lb_selectcolumn.Items.Add(new ListItem("IsDisable", "53"));
        lb_selectcolumn.Items.Add(new ListItem("VisualHandy", "14"));
        lb_selectcolumn.Items.Add(new ListItem("Residency", "48"));
        lb_selectcolumn.Items.Add(new ListItem("Physically challange", "49"));
        lb_selectcolumn.Items.Add(new ListItem("Learning Disability", "51"));
        lb_selectcolumn.Items.Add(new ListItem("Other Disability", "52"));
        lb_selectcolumn.Items.Add(new ListItem("Sports", "50"));
        lb_selectcolumn.Items.Add(new ListItem("First Graduate", "15"));
        lb_selectcolumn.Items.Add(new ListItem("MissionaryChild", "26"));
        lb_selectcolumn.Items.Add(new ListItem("missionarydisc", "27"));
        lb_selectcolumn.Items.Add(new ListItem("Hostel accommodation", "65"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Donor", "66"));
        lb_selectcolumn.Items.Add(new ListItem("Reserved Caste", "67"));
        lb_selectcolumn.Items.Add(new ListItem("Economic Backward", "68"));
        lb_selectcolumn.Items.Add(new ListItem("Parents Old Student", "69"));
        lb_selectcolumn.Items.Add(new ListItem("Driving License", "70"));
        lb_selectcolumn.Items.Add(new ListItem("License No", "71"));
        lb_selectcolumn.Items.Add(new ListItem("Tuition Fee Waiver", "72"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance", "73"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance Amount", "77"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance InsBy", "78"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance Nominee", "79"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance NominRelation", "80"));
        lb_selectcolumn.Items.Add(new ListItem("Address", "18"));
        lb_selectcolumn.Items.Add(new ListItem("Street", "19"));
        lb_selectcolumn.Items.Add(new ListItem("City", "20"));
        lb_selectcolumn.Items.Add(new ListItem("State", "21"));
        lb_selectcolumn.Items.Add(new ListItem("Country", "22"));
        lb_selectcolumn.Items.Add(new ListItem("PinCode", "24"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Address", "108"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Street", "109"));
        lb_selectcolumn.Items.Add(new ListItem("Communication City", "110"));
        lb_selectcolumn.Items.Add(new ListItem("Communication State", "111"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Country", "112"));
        lb_selectcolumn.Items.Add(new ListItem("Communication PinCode", "113"));
        lb_selectcolumn.Items.Add(new ListItem("Student Mobile", "23"));
        lb_selectcolumn.Items.Add(new ListItem("Alternate Mob No", "82"));
        lb_selectcolumn.Items.Add(new ListItem("Student EmailId", "56"));
        lb_selectcolumn.Items.Add(new ListItem("Parent Phone No", "25"));
        lb_selectcolumn.Items.Add(new ListItem("Curricular", "17"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Name", "28"));
        lb_selectcolumn.Items.Add(new ListItem("Institution Address", "97"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Pincode", "160"));
        //lb_selectcolumn.Items.Add(new ListItem("Institute State", "161"));
        //institute pincode,institude state,
        lb_selectcolumn.Items.Add(new ListItem("X Medium", "98"));
        lb_selectcolumn.Items.Add(new ListItem("X11 Medium", "99"));
        lb_selectcolumn.Items.Add(new ListItem("Part1 Language", "29"));
        lb_selectcolumn.Items.Add(new ListItem("Part2 Language", "30"));
        lb_selectcolumn.Items.Add(new ListItem("Percentage", "100"));
        lb_selectcolumn.Items.Add(new ListItem("Secured Mark", "101"));
        lb_selectcolumn.Items.Add(new ListItem("Total Mark", "102"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Month", "103"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Year", "104"));
        lb_selectcolumn.Items.Add(new ListItem("Vocational Stream", "105"));
        lb_selectcolumn.Items.Add(new ListItem("Mark Priority", "106"));
        lb_selectcolumn.Items.Add(new ListItem("Cut Of Mark", "107"));
        lb_selectcolumn.Items.Add(new ListItem("University Name", "31"));
        lb_selectcolumn.Items.Add(new ListItem("Last TC No", "164"));
        lb_selectcolumn.Items.Add(new ListItem("Last TC Date", "165"));
        //lb_selectcolumn.Items.Add(new ListItem("12th TC", "32"));
        //lb_selectcolumn.Items.Add(new ListItem("10th MS", "33"));
        //lb_selectcolumn.Items.Add(new ListItem("12th MS", "34"));
        //lb_selectcolumn.Items.Add(new ListItem("Community Certificate No", "35"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Provisional No", "36"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Consolidate", "35"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Degree", "38"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma- No of Semester", "39"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Provisional No", "40"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Consolidate", "41"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Degree", "42"));
        //lb_selectcolumn.Items.Add(new ListItem("UG- No of Semester", "43"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Provisional No", "44"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Consolidate", "45"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Degree", "46"));
        //lb_selectcolumn.Items.Add(new ListItem("PG- No of Semester", "47"));
        lb_selectcolumn.Items.Add(new ListItem("A/C No", "114"));
        lb_selectcolumn.Items.Add(new ListItem("DebitCard No", "115"));
        lb_selectcolumn.Items.Add(new ListItem("IFSCCode", "116"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Name", "117"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Branch", "118"));
        lb_selectcolumn.Items.Add(new ListItem("Relation Name", "119"));
        lb_selectcolumn.Items.Add(new ListItem("RelationShip", "120"));
        lb_selectcolumn.Items.Add(new ListItem("Student/Staff", "121"));
        lb_selectcolumn.Items.Add(new ListItem("Spouse Name", "32"));
        lb_selectcolumn.Items.Add(new ListItem("Refer By", "36"));
        lb_selectcolumn.Items.Add(new ListItem("Referred Person ", "37"));

        string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
        string branchSel = rs.GetSelectedItemsText(cbl_degree);

        //08.05.17 barath
        string cert_q = " select distinct CertName,MasterValue from CertMasterDet c,CO_MasterValues co where c.CertName=co.MasterCode and MasterCriteria='CertificateName' and collegecode in ('" + collegeSel + "') and categorytype='6'";
        cert_q += " select LinkValue from New_InsSettings where LinkName='SubjectPriority' and user_code ='" + usercode + "' and college_code  in ('" + collegeSel + "')";
        DataSet cert_ds = new DataSet();
        cert_ds = d2.select_method_wo_parameter(cert_q, "text");
        if (cert_ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in cert_ds.Tables[0].Rows)
            {
                lb_selectcolumn.Items.Add(new ListItem(Convert.ToString(dr["MasterValue"]), Convert.ToString(dr["CertName"])));
            }
        }
        if (cert_ds.Tables[1].Rows.Count > 0)
        {
            string[] pcmsub = Convert.ToString(cert_ds.Tables[1].Rows[0][0]).Split(',');
            if (pcmsub.Length > 1)
            {
                foreach (string pcmsubject in pcmsub)
                {
                    string subjectname = d2.GetFunction(" select textval,textcode from textvaltable where textcriteria='subje' and college_code in ('" + collegeSel + "') and TextCode='" + pcmsubject + "'");
                    lb_selectcolumn.Items.Add(new ListItem(subjectname, pcmsubject));
                }
            }
        }
    }
    public void loadlcolumns()
    {
        try
        {
            string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
            string branchSel = rs.GetSelectedItemsText(cbl_degree);

            string linkname = "StudentStrengthCommon column order settings";
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code in ('" + collegeSel + "') ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colval = Convert.ToString(valuesplit[k]);
                                loadtext();
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
    public void savecolumnorder()
    {
        string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
        string branchSel = rs.GetSelectedItemsText(cbl_degree);
        string columnvalue = "";
        string linkname = Convert.ToString(ddl_coltypeadd.SelectedItem.Text);
        string val = "";
        if (txtcolumn.Text.Trim() != "")
        {
            if (ItemList.Count > 0)
            {
                for (int i = 0; i < ItemList.Count; i++)
                {
                    val = Convert.ToString(lb_selectcolumn.Items.FindByText(ItemList[i].ToString()).Value);
                    if (columnvalue == "")
                    {
                        columnvalue = val;
                    }
                    else
                    {
                        columnvalue = columnvalue + ',' + val;
                    }
                }
            }
        }
        for (int clgI = 0; clgI < cbl_clgname.Items.Count; clgI++)
        {
            if (cbl_clgname.Items[clgI].Selected)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code in ('" + (cbl_clgname.Items[clgI].Value) + "')  ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "'  and college_code='" + (cbl_clgname.Items[clgI].Value) + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + (cbl_clgname.Items[clgI].Value) + "')";
                int clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
        }
    }
    public void loadtext()
    {
        loadval = ""; printval = "";
        if (colval == "1")
        {
            loadval = lbl_degree.Text;
            printval = "Course_Name";
        }
        if (colval == "2")
        {
            loadval = lbl_branch.Text;
            printval = "Dept_Name";
        }
        if (colval == "3")
        {
            loadval = "Batch";
            printval = "Batch_Year";
        }
        if (colval == "4")
        {
            loadval = lbl_org_sem.Text;
            printval = "Current_Semester";
        }
        if (colval == "5")
        {
            loadval = "Father Name";
            printval = "parent_name";
        }
        if (colval == "6")
        {
            loadval = "DOB";
            printval = "dob";
        }
        if (colval == "7")
        {
            loadval = "Father Occupation";
            printval = "parent_occu";
        }
        if (colval == "8")
        {
            loadval = "Mother Tongue";
            printval = "mother_tongue";
        }
        if (colval == "9")
        {
            loadval = "Religion";
            printval = "religion";
        }
        if (colval == "10")
        {
            loadval = "Citizen";
            printval = "citizen";
        }
        if (colval == "11")
        {
            loadval = "Community";
            printval = "community";
        }
        if (colval == "12")
        {
            loadval = "Caste";
            printval = "caste";
        }
        if (colval == "13")
        {
            loadval = "TamilOrginFromAndaman";
            printval = "TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            loadval = "VisualHandy";
            printval = "visualhandy";
        }
        if (colval == "15")
        {
            loadval = "First Graduate";
            printval = "first_graduate";
        }
        if (colval == "16")
        {
            loadval = "SeatType";
            printval = "seattype";
        }
        if (colval == "17")
        {
            loadval = "Curricular";
            printval = "co_curricular";
        }
        if (colval == "18")
        {
            loadval = "Address";
            printval = "parent_addressP";
        }
        if (colval == "19")
        {
            loadval = "Street";
            printval = "Streetp";
        }
        if (colval == "20")
        {
            loadval = "City";
            printval = "cityp";
        }
        if (colval == "21")
        {
            loadval = "State";
            printval = "parent_statep";
        }
        if (colval == "22")
        {
            loadval = "Country";
            printval = "Countryp";
        }
        if (colval == "23")
        {
            loadval = "Student Mobile";
            printval = "Student_Mobile";
        }
        if (colval == "24")
        {
            loadval = "PinCode";
            printval = "parent_pincodep";
        }
        if (colval == "25")
        {
            loadval = "Parent Phone No";
            printval = "parent_phnop";
        }
        if (colval == "26")
        {
            loadval = "MissionaryChild";
            printval = "MissionaryChild";
        }
        if (colval == "27")
        {
            loadval = "missionarydisc";
            printval = "missionarydisc";
        }
        if (colval == "28")
        {
            loadval = "Institute Name";
            printval = "Institute_name";
        }
        if (colval == "29")
        {
            loadval = "Part1 Language";
            printval = "Part1Language";
        }
        if (colval == "30")
        {
            loadval = "Part2 Language";
            printval = "Part2Language";
        }
        if (colval == "31")
        {
            loadval = "University Name";
            printval = "university_code";
        }
        if (colval == "35")
        {
            loadval = "Voter ID";
            printval = "ElectionID_No";
        }
        if (colval == "36")
        {
            loadval = "Refer By";
            printval = "direct_refer";
        }
        if (colval == "37")
        {
            loadval = "Refer Person";
            printval = "refer_stcode";
        }
        if (colval == "48")
        {
            loadval = "Residency";
            printval = "CampusReq";
        }
        if (colval == "49")
        {
            loadval = "Physically challange";
            printval = "handy";
        }
        if (colval == "50")
        {
            printval = "DistinctSport";
            loadval = "Sports";
        }
        if (colval == "51")
        {
            printval = "islearningdis";
            loadval = "Learning Disability";
        }
        if (colval == "52")
        {
            printval = "isdisabledisc";
            loadval = "Other Disability";
        }
        if (colval == "53")
        {
            loadval = "IsDisable";
            printval = "isdisable";
        }
        if (colval == "54")
        {
            loadval = "Student Name";
            printval = "stud_name";
        }
        if (colval == "55")
        {
            loadval = "Roll No";
            printval = "Roll_no";
        }
        if (colval == "56")
        {
            loadval = "Student EmailId";
            printval = "StuPer_Id";
        }
        if (colval == "57")
        {
            loadval = "Reg No";
            printval = "reg_no";
        }
        if (colval == "58")
        {
            loadval = "Admission No";
            printval = "roll_admit";
        }
        if (colval == "59")
        {
            loadval = "Application No";
            printval = "app_formno";
        }
        if (colval == "60")
        {
            loadval = "Section";
            printval = "sections";
        }
        if (colval == "61")
        {
            loadval = "Gender";
            printval = "sex";
        }
        if (colval == "62")
        {
            loadval = "Blood Group";
            printval = "bldgrp";
        }
        if (colval == "63")
        {
            loadval = "Student Type";
            printval = "stud_type";
        }
        if (colval == "64")
        {
            loadval = "Ex-serviceman";
            printval = "IsExService";
        }
        if (colval == "65")
        {
            loadval = "Hostel accommodation";
            printval = "CampusReq";
        }
        if (colval == "66")
        {
            loadval = "Blood Donor";
            printval = "isdonar";
        }
        if (colval == "67")
        {
            loadval = "Reserved Caste";
            printval = "ReserveCategory";
        }
        if (colval == "68")
        {
            loadval = "Economic Backward";
            printval = "EconBackword";
        }
        if (colval == "69")
        {
            loadval = "Parents Old Student";
            printval = "parentoldstud";
        }
        if (colval == "70")
        {
            loadval = "Driving License";
            printval = "IsDrivingLic";
        }
        if (colval == "71")
        {
            loadval = "License No";
            printval = "Driving_details";
        }
        if (colval == "72")
        {
            loadval = "Tuition Fee Waiver";
            printval = "tutionfee_waiver";
        }
        if (colval == "73")
        {
            loadval = "Insurance";
            printval = "IsInsurance";
        }
        if (colval == "74")
        {
            loadval = "Rank";
            printval = "ExsRank";
        }
        if (colval == "75")
        {
            loadval = "Place";
            printval = "ExSPlace";
        }
        if (colval == "76")
        {
            loadval = "Number";
            printval = "ExsNumber";
        }
        if (colval == "77")
        {
            loadval = "Insurance Amount";
            printval = "Insurance_Amount";
        }
        if (colval == "78")
        {
            loadval = "Insurance InsBy";
            printval = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            loadval = "Insurance Nominee";
            printval = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            loadval = "Insurance NominRelation";
            printval = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            loadval = "Applied Date";
            printval = "date_applied";
        }
        if (colval == "82")
        {
            loadval = "Alternate Mob No";
            printval = "alter_mobileno";
        }
        if (colval == "83")
        {
            loadval = "Sub Caste";
            printval = "SubCaste";
        }
        if (colval == "84")
        {
            loadval = "Father Income";
            printval = "parent_income";
        }
        if (colval == "85")
        {
            loadval = "Father Mob No";
            printval = "parentF_Mobile";
        }
        if (colval == "86")
        {
            loadval = "Father Email Id";
            printval = "parentidp";
        }
        if (colval == "87")
        {
            loadval = "Mother Name";
            printval = "mother";
        }
        if (colval == "88")
        {
            loadval = "Mother Income";
            printval = "mIncome";
        }
        if (colval == "89")
        {
            loadval = "Mother Mob No";
            printval = "parentM_Mobile";
        }
        if (colval == "90")
        {
            loadval = "Mother Email Id";
            printval = "emailM";
        }
        if (colval == "91")
        {
            loadval = "Guardian Name";
            printval = "guardian_name";
        }
        if (colval == "92")
        {
            loadval = "Guardian Mob No";
            printval = "guardian_mobile";
        }
        if (colval == "93")
        {
            loadval = "Guardian Email Id";
            printval = "gurdian_email";
        }
        if (colval == "94")
        {
            loadval = "Place Of Birth";
            printval = "place_birth";
        }
        if (colval == "95")
        {
            loadval = "Adhaar Card No";
            printval = "Aadharcard_no";
        }
        if (colval == "96")
        {
            loadval = "Mother Occupation";
            printval = "motherocc";
        }
        if (colval == "97")
        {
            loadval = "Institution Address";
            printval = "instaddress";
        }
        if (colval == "98")
        {
            loadval = "X Medium";
            printval = "Xmedium";
        }
        if (colval == "99")
        {
            loadval = "X11 Medium";
            printval = "medium";
        }
        if (colval == "100")
        {
            loadval = "Percentage";
            printval = "percentage";
        }
        if (colval == "101")
        {
            loadval = "Secured Mark";
            printval = "securedmark";
        }
        if (colval == "102")
        {
            printval = "totalmark";
            loadval = "Total Mark";
        }
        if (colval == "103")
        {
            loadval = "Pass Month";
            printval = "passmonth";
        }
        if (colval == "104")
        {
            loadval = "Pass Year";
            printval = "passyear";
        }
        if (colval == "105")
        {
            loadval = "Vocational Stream";
            printval = "Vocational_stream";
        }
        if (colval == "106")
        {
            loadval = "Mark Priority";
            printval = "markPriority";
        }
        if (colval == "107")
        {
            loadval = "Cut Of Mark";
            printval = "Cut_Of_Mark";
        }
        if (colval == "108")
        {
            loadval = "Communication Address";
            printval = "parent_addressc";
        }
        if (colval == "109")
        {
            loadval = "Communication Street";
            printval = "Streetc";
        }
        if (colval == "110")
        {
            loadval = "Communication City";
            printval = "cityc";
        }
        if (colval == "111")
        {
            loadval = "Communication State";
            printval = "parent_statec";
        }
        if (colval == "112")
        {
            loadval = "Communication Country";
            printval = "Countryc";
        }
        if (colval == "113")
        {
            printval = "parent_pincodec";
            loadval = "Communication PinCode";
        }
        if (colval == "114")
        {
            loadval = "A/C No";
            printval = "AccNo";
        }
        if (colval == "115")
        {
            printval = "DebitCardNo";
            loadval = "DebitCard No";
        }
        if (colval == "116")
        {
            loadval = "IFSCCode";
            printval = "IFSCCode";
        }
        if (colval == "117")
        {
            loadval = "Bank Name";
            printval = "BankName";
        }
        if (colval == "118")
        {
            printval = "Branch";
            loadval = "Branch";
        }
        if (colval == "119")
        {
            printval = "name_roll";
            loadval = "Relation Name";
        }
        if (colval == "120")
        {
            printval = "relationship";
            loadval = "RelationShip";
        }
        if (colval == "121")
        {
            printval = "isstaff";
            loadval = "Student/Staff";
        }
        if (colval == "122")
        {
            printval = "Boarding";
            loadval = "Boarding";
        }
        if (colval == "123")
        {
            printval = "vehid";
            loadval = "Vehicle Id";
        }
        if (colval == "43")
        {
            printval = "Mode";
            loadval = "Mode";
        }
        //26.08.16
        if (colval == "124")
        {
            printval = "StuPer_Id";
            loadval = "Student EmailID";
        }
        if (colval == "125")
        {
            printval = "idmark";
            loadval = "Identification Mark";
        }
        if (colval == "126")
        {
            printval = "Quota";
            loadval = "Quota";
        }
        if (colval == "127")
        {
            printval = "fatherdob";
            loadval = "Father DOB";
        }
        if (colval == "128")
        {
            printval = "FocDesign";
            loadval = "Father Designation";
        }
        if (colval == "129")
        {
            printval = "FocDept";
            loadval = "Father Department";
        }
        if (colval == "130")
        {
            printval = "FocDetails";
            loadval = "Father Office";
        }
        if (colval == "131")
        {
            printval = "Fat_off_addressP";
            loadval = "Father Office Address";
        }
        if (colval == "132")
        {
            printval = "Fat_off_street";
            loadval = "Father Street";
        }
        if (colval == "133")
        {
            printval = "ftown";
            loadval = "Father Town";
        }
        if (colval == "134")
        {
            printval = "fcity";
            loadval = "Father City";
        }
        if (colval == "135")
        {
            printval = "Fat_off_state";
            loadval = "Father State";
        }
        if (colval == "136")
        {
            printval = "Fat_off_country";
            loadval = "Father Country";
        }
        if (colval == "137")
        {
            printval = "Fat_off_pincode";
            loadval = "Father Pincode";
        }
        //mother
        if (colval == "138")
        {
            printval = "motherdob";
            loadval = "Mother DOB";
        }
        if (colval == "139")
        {
            printval = "MocDesign";
            loadval = "Mother Designation";
        }
        if (colval == "140")
        {
            printval = "MocDept";
            loadval = "Mother Department";
        }
        if (colval == "141")
        {
            printval = "MocDetails";
            loadval = "Mother Office";
        }
        if (colval == "142")
        {
            printval = "mot_off_address1";
            loadval = "Mother Office Address";
        }
        if (colval == "143")
        {
            printval = "mot_off_address2";
            loadval = "Mother Street";
        }
        if (colval == "144")
        {
            printval = "mtown";
            loadval = "Mother Town";
        }
        if (colval == "145")
        {
            printval = "mcity";
            loadval = "Mother City";
        }
        if (colval == "146")
        {
            printval = "mot_off_state";
            loadval = "Mother State";
        }
        if (colval == "147")
        {
            printval = "mot_off_country";
            loadval = "Mother Country";
        }
        if (colval == "148")
        {
            printval = "mot_off_pincode";
            loadval = "Mother Pincode";
        }
        //guardian
        if (colval == "149")
        {
            printval = "Guardiandob";
            loadval = "Guardian DOB";
        }
        if (colval == "150")
        {
            printval = "GocDesign";
            loadval = "Guardian Designation";
        }
        if (colval == "151")
        {
            printval = "GocDept";
            loadval = "Guardian Department";
        }
        if (colval == "152")
        {
            printval = "GocDetails";
            loadval = "Guardian Office";
        }
        if (colval == "153")
        {
            printval = "gur_off_address1";
            loadval = "Guardian Office Address";
        }
        if (colval == "154")
        {
            printval = "gur_off_address2";
            loadval = "Guardian Street";
        }
        if (colval == "155")
        {
            printval = "gtown";
            loadval = "Guardian Town";
        }
        if (colval == "156")
        {
            printval = "gcity";
            loadval = "Guardian City";
        }
        if (colval == "157")
        {
            printval = "gur_off_state";
            loadval = "Guardian State";
        }
        if (colval == "158")
        {
            printval = "gur_off_country";
            loadval = "Guardian Country";
        }
        if (colval == "159")
        {
            printval = "gur_off_pincode";
            loadval = "Guardian Pincode";
        }
        if (colval == "160")
        {
            printval = "instpincode";
            loadval = "Institute Pincode";
        }
        if (colval == "161")
        {
            printval = "Hostel Name";
            loadval = "Hostel Name";
        }
        if (colval == "162")
        {
            printval = "Vehicle_Reg_No";
            loadval = "Vehicle Reg No";
        }
        if (colval == "163")
        {
            printval = "Veh_Type";
            loadval = "Vehicle Type";
        }
        if (colval == "164")
        {
            printval = "LastTCNo";
            loadval = "Last TC No";
        }
        if (colval == "165")
        {
            printval = "LastTCDate";
            loadval = "Last TC Date";
        }
        if (colval == "32")
        {
            printval = "spouse_name";
            loadval = "Spouse Name";
        }
        if (colval == "33")
        {
            printval = "adm_date";
            loadval = "Admitted Date";
        }
        if (colval == "34")
        {
            printval = "collname";
            loadval = "College Name";
        }


    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Student Strength Report";
            string pagename = "strengthreport.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, attendance);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread2, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {
            lbl_norec.Visible = true;
            lbl_norec.Text = ex.ToString();
        }
    }
    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            if (txt_excelname.Text == "")
            {
                lbl_norec.Visible = true;
            }
            else
            {
                lbl_norec.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void clear()
    {
        //Fpspread1.Visible = false;
        lbl_headernamespd2.Visible = false;
        imgbtn_columsetting.Visible = false;
        Fpspread2.Visible = false;
        div_report.Visible = false;
    }
    public void columnordertype()
    {
        ddl_colord.Items.Clear();
        ddl_coltypeadd.Items.Clear();
        string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
        string branchSel = rs.GetSelectedItemsText(cbl_degree);
        string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudentStatus' and CollegeCode in ('" + collegeSel + "')";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_colord.DataSource = ds;
            ddl_colord.DataTextField = "MasterValue";
            ddl_colord.DataValueField = "MasterCode";
            ddl_colord.DataBind();
            ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
            ddl_coltypeadd.DataSource = ds;
            ddl_coltypeadd.DataTextField = "MasterValue";
            ddl_coltypeadd.DataValueField = "MasterCode";
            ddl_coltypeadd.DataBind();
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    public void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }
    public void btn_deltype_OnClick(object sender, EventArgs e)
    {
        string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
        string branchSel = rs.GetSelectedItemsText(cbl_degree);

        if (ddl_coltypeadd.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
        else if (ddl_coltypeadd.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any record";
        }
        else if (ddl_coltypeadd.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='StudentStatus' and CollegeCode in ('" + collegeSel + "' )";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                txtcolumn.Text = "";
                ItemList.Clear();
                Itemindex.Clear();
                lb_selectcolumn.ClearSelection();
                lbl_alert.Text = "Deleted Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No records found";
            }
            columnordertype();
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }

    public void btndescpopadd_Click(object sender, EventArgs e)
    {
        if (txt_description11.Text != "")
        {
            int insert = 0;
            for (int clgI = 0; clgI < cbl_clgname.Items.Count; clgI++)
            {

                if (cbl_clgname.Items[clgI].Selected)
                {
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudentStatus' and CollegeCode ='" + cbl_clgname.Items[clgI].Value + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudentStatus' and CollegeCode ='" + cbl_clgname.Items[clgI].Value + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','StudentStatus','" + cbl_clgname.Items[clgI].Value + "')";
                    insert += d2.update_method_wo_parameter(sql, "TEXT");
                }
            }
            if (insert != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Added Successfully";
                txt_description11.Text = "";
                //imgdiv33.Visible = false;           
            }
        }
        else
        {
            imgdiv2.Visible = true;
            pnl2.Visible = true;
            lbl_alert.Text = "Enter the description";
        }
        columnordertype();
    }
    public void btndescpopexit_Click(object sender, EventArgs e)
    {
        panel_description11.Visible = false;
        imgdiv33.Visible = false;
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
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
        }
        catch { }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {

            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (int sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (int sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    public void viewcolumorder()
    {
        try
        {
            string collegeSel = rs.GetSelectedItemsValueAsString(cbl_clgname);
            string branchSel = rs.GetSelectedItemsText(cbl_degree);
            lb_selectcolumn.ClearSelection();
            txtcolumn.Text = "";
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                string q = "select LinkValue from New_InsSettings where LinkName='" + ddl_coltypeadd.SelectedItem.Text + "' and college_code in ('" + collegeSel + "')";
                q += "  select distinct CertName,MasterValue from CertMasterDet c,CO_MasterValues co where c.CertName=co.MasterCode and MasterCriteria='CertificateName' and collegecode in ('" + collegeSel + "') and categorytype='6'";
                ds.Clear();
                q += " select LinkValue from New_InsSettings where LinkName='SubjectPriority' and user_code ='" + usercode + "' and college_code  in ('" + collegeSel + "')";
                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        if (!certdet.Contains(Convert.ToString(dr["CertName"])))
                            certdet.Add(Convert.ToString(dr["CertName"]), Convert.ToString(dr["MasterValue"]));
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        string[] LinkValue = Convert.ToString(ds.Tables[2].Rows[0]["LinkValue"]).Split(',');
                        string ss = "  select textval,textcode from textvaltable where textcriteria='subje' and college_code in ('" + collegeSel + "') and TextCode in ('" + Convert.ToString(ds.Tables[2].Rows[0]["LinkValue"]).Replace(",", "','") + "') ";
                        DataSet pcm_ds = new DataSet();
                        pcm_ds = d2.select_method_wo_parameter(ss, "text");
                        if (pcm_ds.Tables[0].Rows.Count > 0)
                            foreach (DataRow dr in pcm_ds.Tables[0].Rows)
                            {
                                if (!pcmmark_hash.Contains(Convert.ToString(dr["textcode"])))
                                    pcmmark_hash.Add(Convert.ToString(dr["textcode"]), Convert.ToString(dr["textval"]));
                            }
                    }
                    string vall = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                    string[] sp = vall.Split(',');
                    if (sp.Length > 50)
                        txtcolumn.Height = 250;
                    else
                        txtcolumn.Height = 100;
                    for (int y = 0; y < sp.Length; y++)
                    {
                        colval = sp[y];
                        loadtext();
                        lb_selectcolumn.Items.FindByValue(colval).Selected = true;
                        if (loadval.Trim() == "")
                        {
                            if (certdet.Contains(colval))
                            {
                                loadval = Convert.ToString(certdet[colval]);
                            }
                            if (pcmmark_hash.Contains(colval))
                            {
                                loadval = Convert.ToString(pcmmark_hash[colval]);
                            }
                        }
                        if (!Itemindex.Contains(colval))
                        {
                            ItemList.Add(loadval);
                            Itemindex.Add(colval);
                        }
                        if (txtcolumn.Text.Trim() == "")
                            txtcolumn.Text = loadval + "(" + (y + 1) + ")";
                        else
                            txtcolumn.Text = txtcolumn.Text + "," + loadval + "(" + (y + 1) + ")";
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void ddl_coltypeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedIndex != 0)
        {
            viewcolumorder();
        }
        else
        {
            Itemindex.Clear();
            ItemList.Clear();
            lb_selectcolumn.ClearSelection();
            txtcolumn.Text = "";
            txtcolumn.Height = 100;
        }
    }
    protected void ddl_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        degree();
        edu_level();
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        degree();
        bindsem();
        BindSectionDetail();
    }
    protected void ddl_graduation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            degree();
            bindbranch();
            bindsem();
            BindSectionDetail();
        }
        catch { }
    }
    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        lb_selectcolumn.ClearSelection();
        txtcolumn.Text = "";
        txtcolumn.Height = 100;
    }
    protected void lb_selectcolumn_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            string value = "";
            int index;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (lb_selectcolumn.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(lb_selectcolumn.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(lb_selectcolumn.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < lb_selectcolumn.Items.Count; i++)
            {
                if (lb_selectcolumn.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(lb_selectcolumn.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnordr.Visible = true;
            txtcolumn.Visible = true;
            txtcolumn.Text = "";
            for (int i = 0; i < lb_selectcolumn.Items.Count; i++)
            {
                if (txtcolumn.Text == "")
                {
                    txtcolumn.Text = ItemList[i].ToString() + "(" + (i + 1) + ")";
                }
                else
                {
                    txtcolumn.Text = txtcolumn.Text + "," + ItemList[i].ToString() + "(" + (i + 1) + ")";
                }
            }
            if (ItemList.Count > 50)
                txtcolumn.Height = 250;
            else
                txtcolumn.Height = 100;
            if (ItemList.Count == 0)
            {
                txtcolumn.Visible = false;
                lnk_columnordr.Visible = false;
            }
        }
        catch { }
    }
    protected void LinkButtonselectall_Click(object sender, EventArgs e)
    {
        try
        {
            ItemList.Clear();
            for (int i = 0; i < lb_selectcolumn.Items.Count; i++)
            {
                string si = Convert.ToString(i);
                lb_selectcolumn.Items[i].Selected = true;
                ItemList.Add(lb_selectcolumn.Items[i].Text.ToString());
                Itemindex.Add(si);
            }
            txtcolumn.Visible = true;
            txtcolumn.Text = "";
            lnk_columnordr.Visible = true;
            txtcolumn.Text = "";
            for (int i = 0; i < lb_selectcolumn.Items.Count; i++)
            {
                if (txtcolumn.Text != "")
                {
                    txtcolumn.Text = txtcolumn.Text + "," + ItemList[i].ToString();
                }
                else
                {
                    txtcolumn.Text = txtcolumn.Text + ItemList[i].ToString();
                }
            }
            if (lb_selectcolumn.Items.Count > 50)
                txtcolumn.Height = 250;
            else
                txtcolumn.Height = 100;
        }
        catch
        { }
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
    protected string returnwithsinglecodetext(CheckBoxList cb)
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
    protected bool checknumber(string numbers)
    {
        bool valchk = false;
        Regex regex = new Regex("^[0-9]+$");
        if (regex.IsMatch(numbers))
        {
            valchk = true;
        }
        return valchk;
    }
    protected void ddl_degree_Selectedindexchange(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void ddlOrderby_OnIndexChange(object sender, EventArgs e)
    {
        switch (ddlOrderby.SelectedIndex)
        {
            case 0:
            case 1:
                // spanGen.Visible = false;
                rblGen.Visible = false;
                break;
            case 2:
                //spanGen.Visible = true;
                rblGen.Visible = true;
                rblGen.SelectedIndex = 0;
                break;
        }
    }
    protected void rblGen_Indexchange(object sender, EventArgs e)
    {
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
        lbl.Add(lbl_clgname);
        fields.Add(0);
        //lbl.Add(lbl_Stream);
        //fields.Add(1);
        lbl.Add(lbl_degree);
        fields.Add(2);
        lbl.Add(lbl_branch);
        fields.Add(3);
        lbl.Add(lbl_org_sem);
        fields.Add(4);
        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    protected void cbinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");
        }
        catch { }
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");
        }
        catch { }
    }
    protected void chkinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkinclude.Checked == true)
        {
            divcolor.Visible = true;
            cb_onlydis.Enabled = true;
            cb_onlydis.Checked = false;
            txtinclude.Enabled = true;
            LoadIncludeSetting();
            cbTransfer.Enabled = false;
        }
        else
        {
            divcolor.Visible = false;
            cb_onlydis.Enabled = false;
            txtinclude.Enabled = false;
            cb_onlydis.Checked = false;
            cblinclude.Items.Clear();
            cbTransfer.Enabled = true;
        }
    }
    private void LoadIncludeSetting()
    {
        try
        {
            cblinclude.Items.Clear();
            cblinclude.Items.Add(new ListItem("Course Completed", "1"));
            cblinclude.Items.Add(new ListItem("Debar", "2"));
            cblinclude.Items.Add(new ListItem("Discontinue", "3"));
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    cblinclude.Items[i].Selected = true;
                }
                cbinclude.Checked = true;
                txtinclude.Text = "Include Settings(" + cblinclude.Items.Count + ")";
            }
        }
        catch { }
    }

    #region roll,reg,admission no settings
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }
    #endregion

    //16-05-2017
    protected void checkDate(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = Convert.ToDateTime(txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2]);
            DateTime todate = Convert.ToDateTime(txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2]);

            if (fromdate <= todate)
            {
            }
            else
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('From Date Should Not Exceed To Date')", true);
                imgdiv2.Visible = true;
                lbl_alert.Text = "From Date Should Not Exceed To Date";
            }
        }
        catch { }
    }

    //18-05-2017
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, lbl_degree.Text);

        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, "");

        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void cb_grad_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_grad, cbl_grad, txt_grad, lbl_graduation.Text, lbl_graduation.Text);

        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
    }
    protected void cbl_grad_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_grad, cbl_grad, txt_grad, lbl_graduation.Text, "");
        degree();
        bindbranch();
        bindsem();
        BindSectionDetail();
    }

    //transfer added by sudhagar 30.05.2017
    protected void cbTransfer_Changed(object sender, EventArgs e)
    {
        if (cbTransfer.Checked)
            chkinclude.Enabled = false;
        else
            chkinclude.Enabled = true;
    }

}