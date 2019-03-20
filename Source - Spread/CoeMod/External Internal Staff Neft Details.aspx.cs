using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.Configuration;

public partial class CoeMod_External_Internal_Staff_Neft_Details : System.Web.UI.Page
{
    static string examMonth = string.Empty;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static string examyear = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;
    string CollegeCode = string.Empty;
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    string selectQuery = string.Empty;
    Hashtable hat = new Hashtable();
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    DataTable dtFees = new DataTable();
    DataRow drCurrentRow;
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
            //****************************************************//
          
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            CollegeCode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                Bindcollege();
                bindyear();
                bindmonth();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Bindcollege()
    {
        try
        {
            string strUser = da.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlCollege.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = da.select_method_wo_parameter(selectQuery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }
    public void bindyear()
    {
        try
        {
            ddlYear1.Items.Clear();
            if (string.IsNullOrEmpty(Convert.ToString(ddlCollege.SelectedValue)))
            {
                return;
            }
            ds.Clear();
            ds = da.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear1.DataSource = ds;
                ddlYear1.DataTextField = "Exam_year";
                ddlYear1.DataValueField = "Exam_year";
                ddlYear1.DataBind();
            }
        }
        catch { }
    }
    public void bindmonth()
    {
        try
        {
            ddlMonth1.Items.Clear();
            ds.Clear();
            string year = ddlYear1.SelectedItem.Text;
            ds = da.Exammonth(year);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth1.DataSource = ds;
                ddlMonth1.DataValueField = "Exam_month";
                ddlMonth1.DataTextField = "monthName";
                ddlMonth1.DataBind();
            }
            degree();
            bindbranch1();
        }
        catch { }
    }
    public void degree()
    {
        try
        {
            cbl_degree.Items.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();

            string codevalues = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                codevalues = "and group_code='" + group_user + "'";
            }
            else
            {
                codevalues = "and user_code='" + usercode + "'";
            }
            string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code " + codevalues + " ";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlYear1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindmonth();
            examyear = Convert.ToString(ddlYear1.SelectedItem);
        }
        catch (Exception ex)
        {

        }
    }

    public void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
                       
                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                }
            }
            bindbranch1();
            cbl_subtype.Items.Clear();
            cbl_subject.Items.Clear();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree.Checked = false;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_degree.Text = "--Select--";
                   
                }
            }
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
                cb_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
            bindbranch1();
            cbl_subtype.Items.Clear();
            cbl_subject.Items.Clear();
        }
        catch (Exception ex)
        {
        }
    }


    public void bindbranch1()
    {
        try
        {
            cbldepartment.Items.Clear();
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

             string typ = "";
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbl_degree.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbl_degree.Items[i].Value + "";
                        }
                    }

                }
            }
            if (typ != "")
            {
                string deptquery="";
                if (usercode != "")
                 deptquery = " select distinct degree.degree_code,department.dept_name,degree.Acronym from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('"+typ+"') and degree.college_code='"+collegecode+"'  and deptprivilages.Degree_code=degree.Degree_code and user_code='"+usercode+"'";
                     else
               deptquery = " select distinct degree.degree_code,department.dept_name,degree.Acronym from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('"+typ+"') and degree.college_code='"+collegecode+"'  and deptprivilages.Degree_code=degree.Degree_code and group_code="+group_user+"";
             DataSet   ds = da.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldepartment.DataSource = ds;
                    cbldepartment.DataTextField = "dept_name";
                    cbldepartment.DataValueField = "degree_code";
                    cbldepartment.DataBind();
                }
            }
           
            subjecttypebind();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbdepartment_Change(object sender, EventArgs e)
    {
        try
        {

            if (cb_departemt.Checked == true)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    if (cb_departemt.Checked == true)
                    {
                        cbldepartment.Items[i].Selected = true;
                        txtdept.Text = "Department(" + (cbldepartment.Items.Count) + ")";

                    }
                }
            }
            else
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    cbldepartment.Items[i].Selected = false;
                    txtdept.Text = "--Select--";
                }
            }
            subjecttypebind();
            cbl_subtype.Items.Clear();
            cbl_subject.Items.Clear();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbldepartment_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_departemt.Checked = false;
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtdept.Text = "--Select--";

                }
            }
            if (seatcount == cbldepartment.Items.Count)
            {
                txtdept.Text = "Department(" + seatcount.ToString() + ")";
                cb_departemt.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtdept.Text = "--Select--";
                cb_departemt.Text = "--Select--";
            }
            else
            {
                txtdept.Text = "Department(" + seatcount.ToString() + ")";
            }
            subjecttypebind();
            cbl_subject.Items.Clear();
        }
        catch (Exception ex)
        {
        }
    }
    protected void subjectbind()
    {
        try
        {
            cbl_subject.Items.Clear();
            ds.Clear();
            string typeval = string.Empty;
          
               string typ = "";
            if (cbldepartment.Items.Count > 0)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    if (cbldepartment.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbldepartment.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbldepartment.Items[i].Value + "";
                        }
                    }

                }
            }
            string typ1 = "";
            if (cbl_subtype.Items.Count > 0)
            {
                for (int i = 0; i < cbl_subtype.Items.Count; i++)
                {
                    if (cbl_subtype.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cbl_subtype.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cbl_subtype.Items[i].Value + "";
                        }
                    }

                }
            }
            if (typ != "")
            {

                string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id  and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type in('" + typ1 + "') and d.degree_code in('" + typ + "') ";
                qeryss = qeryss + " union SELECT distinct ss.subject_type,s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id  and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and ss.subject_type in('" + typ1 + "') and d.degree_code in('" + typ + "')  and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by s.subject_name,s.subject_code desc";

                ds = da.select_method(qeryss, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_subject.DataSource = ds;
                    cbl_subject.DataTextField = "subnamecode";
                    cbl_subject.DataValueField = "subject_code";
                    cbl_subject.DataBind();
                }
            }
          
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            subjectbind();
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_subtype_Change(object sender, EventArgs e)
    {
        try
        {

            if (cb_subtype.Checked == true)
            {
                for (int i = 0; i < cbl_subtype.Items.Count; i++)
                {
                    if (cb_subtype.Checked == true)
                    {
                        cbl_subtype.Items[i].Selected = true;
                        txtsubtype.Text = "Type(" + (cbl_subtype.Items.Count) + ")";

                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_subtype.Items.Count; i++)
                {
                    cbl_subtype.Items[i].Selected = false;
                    txtsubtype.Text = "--Select--";
                }
            }
            subjectbind();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_subtype_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_subtype.Checked = false;
            for (int i = 0; i < cbl_subtype.Items.Count; i++)
            {
                if (cbl_subtype.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtsubtype.Text = "--Select--";

                }
            }
            if (seatcount == cbl_subtype.Items.Count)
            {
                txtsubtype.Text = "Type(" + seatcount.ToString() + ")";
                cb_subtype.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtsubtype.Text = "--Select--";
                cb_subtype.Text = "--Select--";
            }
            else
            {
                txtsubtype.Text = "Type(" + seatcount.ToString() + ")";
            }
            cbl_subject.Items.Clear();
            subjectbind();

        }
        catch (Exception ex)
        {
        }
    }
    protected void subjecttypebind()
    {
        try
        {
            cbl_subtype.Items.Clear();
              string typ = "";
            if (cbldepartment.Items.Count > 0)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    if (cbldepartment.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbldepartment.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbldepartment.Items[i].Value + "";
                        }
                    }

                }
            }
            if (typ != "")
            {
              
                ds.Clear();
                string qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id  and  ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and d.degree_code in('" + typ + "')  ";
                qeryss = qeryss + " union SELECT distinct ss.subject_type FROM subjectChooser sc,subject s,Registration r,Exam_Details ed,sub_sem ss,Degree d,Course c where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and sc.semester=ed.Current_Semester and s.subType_no=ss.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id  and ed.Exam_Month='" + ddlMonth1.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear1.SelectedItem.Text.ToString() + "' and d.degree_code in('" + typ + "') and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by ss.subject_type";


                ds = da.select_method(qeryss, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_subtype.DataSource = ds;
                    cbl_subtype.DataTextField = "subject_type";
                    cbl_subtype.DataBind();
                }
            }
            subjectbind();
        }
        catch (Exception ex)
        {
        }
    }

   
    public void cb_subject_Change(object sender, EventArgs e)
    {
        try
        {

            if (cb_subject.Checked == true)
            {
                for (int i = 0; i < cbl_subject.Items.Count; i++)
                {
                    if (cb_subject.Checked == true)
                    {
                        cbl_subject.Items[i].Selected = true;
                        txtsubject.Text = "Subject(" + (cbl_subject.Items.Count) + ")";

                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_subject.Items.Count; i++)
                {
                    cbl_subject.Items[i].Selected = false;
                    txtsubject.Text = "--Select--";
                }
            }
            SubSubject();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_subject_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_subject.Checked = false;
            for (int i = 0; i < cbl_subject.Items.Count; i++)
            {
                if (cbl_subject.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtsubject.Text = "--Select--";

                }
            }
            if (seatcount == cbl_subtype.Items.Count)
            {
                txtsubject.Text = "Subject(" + seatcount.ToString() + ")";
                cb_subject.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtsubject.Text = "--Select--";
                cb_subject.Text = "--Select--";
            }
            else
            {
                txtsubject.Text = "Subject(" + seatcount.ToString() + ")";
            }
            SubSubject();

        }
        catch (Exception ex)
        {
        }
    }
    protected void SubSubject()
    {
        try
        {
            cbl_subsubject.Items.Clear();
            string month = Convert.ToString(ddlMonth1.SelectedValue).Trim();
            string year = Convert.ToString(ddlYear1.SelectedValue).Trim();
            string valDegree =string.Empty;
            if (cbldepartment.Items.Count > 0)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    if (cbldepartment.Items[i].Selected == true)
                    {
                        if (valDegree == "")
                        {
                            valDegree = "" + cbldepartment.Items[i].Value + "";
                        }
                        else
                        {
                            valDegree = valDegree + "'" + "," + "'" + cbldepartment.Items[i].Value + "";
                        }
                    }

                }
            }
            string typ=string.Empty;
            if (cbl_subject.Items.Count > 0)
            {
                for (int i = 0; i < cbl_subject.Items.Count; i++)
                {
                    if (cbl_subject.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbl_subject.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbl_subject.Items[i].Value + "";
                        }
                    }

                }
            }
             if (typ != "")
             {
                 string SubSubjectQ = " select ss.SubPart,ss.SubSubjectID from COESubSubjectPartSettings ss,COESubSubjectPartMater sm where ss.id=sm.id and sm.DegreeCode='" + valDegree + "' and sm.ExamMonth='" + month + "' and sm.ExamYear='" + year + "'  and ss.SubCode in('" + typ + "')";
                 ds = da.select_method_wo_parameter(SubSubjectQ, "text");
                 if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                 {
                     cbl_subsubject.DataSource = ds;
                     cbl_subsubject.DataTextField = "SubPart";
                     cbl_subsubject.DataValueField = "SubSubjectID";
                     cbl_subsubject.DataBind();
                 }
             }
           
        }
        catch (Exception ex) { }
    }
    protected void ddlMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbl_subtype.Items.Clear();
            cbl_subject.Items.Clear();
            examMonth = Convert.ToString(ddlMonth1.SelectedItem);
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlbranch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbl_subtype.Items.Clear();
            cbl_subject.Items.Clear();
            subjecttypebind();
            
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow drs;
            Hashtable staffamo = new Hashtable();
            if (ItemList.Count == 0)
            {
                ItemList.Add("Staff Code/Name");
                ItemList.Add("Subject Code/Name");
                ItemList.Add("College_Name");
                ItemList.Add("Amount");
                ItemList.Add("Sign");
            }
            Hashtable columnhash = new Hashtable();
            columnhash.Clear();
            columnhash.Add("Staff Code/Name", "Staff Code/Name");
            columnhash.Add("Subject Code/Name", "Subject Code/Name");
            columnhash.Add("Exam Date", "Exam Date");
            columnhash.Add("College_Name", "College Name");
            columnhash.Add("Canditate Registred", "Canditate Registred");
            columnhash.Add("Canditate Examined", "Canditate Examined");
            columnhash.Add("Role", "Role");
            columnhash.Add("Distance In Km", "Distance In Km");
            columnhash.Add("Rem R.s", "Rem R.s");
            columnhash.Add("T.A R.S", "T.A R.S");
            columnhash.Add("D.A R.S", "D.A R.S");
            columnhash.Add("Acquittance", "Acquittance");
            columnhash.Add("IFSC Code", "IFSC Code");
            columnhash.Add("Account Number", "Account Number");
            columnhash.Add("Bank Name", "Bank Name");
            columnhash.Add("Amount", "Amount");
            columnhash.Add("Sign", "Sign");


              dt.Columns.Add("Staff Code/Name");
             dt.Columns.Add("Subject Code/Name");
             dt.Columns.Add("Exam Date");
             dt.Columns.Add("College Name");
             dt.Columns.Add("IFSC Code");
             dt.Columns.Add("Account Number");
             dt.Columns.Add("Bank Name");
             dt.Columns.Add("Canditate Registred");
             dt.Columns.Add( "Canditate Examined");
             dt.Columns.Add( "Role");
             dt.Columns.Add("Distance In Km");
             dt.Columns.Add("Rem R.s");
             dt.Columns.Add( "T.A R.S");
             dt.Columns.Add("D.A R.S");
             dt.Columns.Add("Total Amount");
             dt.Columns.Add("Acquittance");
             dt.Columns.Add("Sign");
            string typ = "";
            if (cbldepartment.Items.Count > 0)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    if (cbldepartment.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbldepartment.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbldepartment.Items[i].Value + "";
                        }
                    }

                }
            }
            string typ1 = string.Empty;
            if (cbl_subsubject.Items.Count > 0)
            {
                for (int i = 0; i < cbl_subsubject.Items.Count; i++)
                {
                    if (cbl_subsubject.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cbl_subsubject.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cbl_subsubject.Items[i].Value + "";
                        }
                    }

                }
            }
            string typ2 = string.Empty;
            if (cbl_subject.Items.Count > 0)
            {
                for (int i = 0; i < cbl_subject.Items.Count; i++)
                {
                    if (cbl_subject.Items[i].Selected == true)
                    {
                        if (typ2 == "")
                        {
                            typ2 = "" + cbl_subject.Items[i].Value + "";
                        }
                        else
                        {
                            typ2 = typ2 + "'" + "," + "'" + cbl_subject.Items[i].Value + "";
                        }
                    }

                }
            }
          
            drs = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string colno = Convert.ToString(dt.Columns[i]);

                int insdex = ItemList.IndexOf(Convert.ToString(colno));
                drs[colno] = colno;


            }
            dt.Rows.Add(drs);
            string minkmc=string.Empty;
            string minamou = string.Empty;
            string maxkmc = string.Empty;
            float fin_amo = 0;
            float fin_fixamo = 0;
            string maxamou = string.Empty;
            string colminkm=" select min_kilometer,min_amount,per_kilometer,per_amount  from Invigilator_Travel_setting where college_code='"+Convert.ToString(ddlCollege.SelectedValue)+"'";
               DataSet dsmin = da.select_method_wo_parameter(colminkm, "Text");
            if (dsmin.Tables.Count > 0 && dsmin.Tables[0].Rows.Count > 0)
            {
                minkmc=Convert.ToString(dsmin.Tables[0].Rows[0]["min_kilometer"]);
                minamou = Convert.ToString(dsmin.Tables[0].Rows[0]["min_amount"]);
                maxkmc = Convert.ToString(dsmin.Tables[0].Rows[0]["per_kilometer"]);
                maxamou = Convert.ToString(dsmin.Tables[0].Rows[0]["per_amount"]);
            }
           // string staff = "select distinct isnull(ExternalCode,'') as externalCode,ExamDate,ExamSession,eth.SubSubjectID,SubNo,s.subject_name,ea.exam_code from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,examtheorybatch eth  where  s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code       and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and subject_code in('" + typ2 + "' )  and    convert(varchar(10),ExamDate,105)>='" + txt_fromdate.Text + "' AND convert(varchar(10),ExamDate,105)<='" + txt_todate.Text + "' and ExternalCode<>'' and ExternalCode is not null   and ed.degree_code in (" + typ + ")    and ed.coll_code in (" + Convert.ToString(ddlCollege.SelectedValue) + ")  and eth.SubNo=s.subject_no    and eth.ExamCode=ea.exam_code    and eth.SubSubjectID in('" + typ1 + "')";
            //select distinct isnull(ExternalCode,'') as externalCode,ExamDate,ExamSession,eth.SubSubjectID from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,examtheorybatch eth,COESubSubjectPartMater co,COESubSubjectPartSettings cs  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code       and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and subject_code in('" + typ2 + "' )  and    convert(varchar(10),ExamDate,105)>='" + txt_fromdate.Text + "' AND convert(varchar(10),ExamDate,105)<='" + txt_todate.Text + "' and ExternalCode<>'' and ExternalCode is not null   and r.degree_code in (" + typ + ") and eth.appno=r.app_no   and r.college_code in (" + Convert.ToString(ddlCollege.SelectedValue) + ")  and eth.SubNo=s.subject_no and cs.SubCode=s.subject_code and eth.SubSubjectID=cs.SubSubjectID and co.id=cs.id and ed.Exam_Month=co.ExamMonth and ed.Exam_year=co.ExamYear and eth.ExamCode=ea.exam_code    and eth.SubSubjectID in('" + typ1 + "')";
            #region external
            string staff = "  select distinct isnull(ExternalCode,'') as externalCode, ed.exam_code from examtheorybatch eth,Exam_Details ed where eth.ExamCode=ed.exam_code and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and degree_code in (" + typ + ") and      convert(varchar(10),ExamDate,105)>='" + txt_fromdate.Text + "' AND convert(varchar(10),ExamDate,105)<='" + txt_todate.Text + "' and eth.SubSubjectID in('" + typ1 + "') and ExternalCode<>''";

            
            
            //select distinct isnull(ExternalCode,'') as externalCode,ExamDate,ExamSession,eth.SubSubjectID from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,examtheorybatch eth,COESubSubjectPartMater co,COESubSubjectPartSettings cs  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code       and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue)+ "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and subject_code in('" + typ2 + "' )  and    convert(varchar(10),ExamDate,105)>='" + txt_fromdate.Text + "' AND convert(varchar(10),ExamDate,105)<='" + txt_todate.Text + "' and ExternalCode<>'' and ExternalCode is not null   and r.degree_code in (" + typ + ") and eth.appno=r.app_no   and r.college_code in (" + Convert.ToString(ddlCollege.SelectedValue) + ")  and eth.SubNo=s.subject_no and cs.SubCode=s.subject_code and eth.SubSubjectID=cs.SubSubjectID and co.id=cs.id and ed.Exam_Month=co.ExamMonth and ed.Exam_year=co.ExamYear and eth.ExamCode=ea.exam_code    and eth.SubSubjectID in('" + typ1 + "')";


             
           // string staff = "select distinct isnull(ExternalCode,'') as externalCode,ExamDate,ExamSession,SubSubjectID  from examtheorybatch  where convert(varchar(10),ExamDate,105)>='" + txt_fromdate.Text + "' AND convert(varchar(10),ExamDate,105)<='" +txt_todate.Text + "' and ExternalCode<>'' and ExternalCode is not null";
         




            ds.Clear();
            ds = da.select_method_wo_parameter(staff, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    drs = dt.NewRow();
                    string exstaff = Convert.ToString(ds.Tables[0].Rows[m]["ExternalCode"]);
                    string[] spl = exstaff.Split(';');
                    if (spl.Length > 0)
                    {
                        for (int sp = 0; sp < spl.Length; sp++)
                        {
                           
                            string exstaffcode = spl[sp];
                            string[] splcode = exstaffcode.Split('-');

                            string staffname = "select college_name,coll_code,bank_name,ifsc_code,acc_no,daAmount  from external_staff where staff_code='" + splcode[0] + "'";
                            DataSet dsmstaffdetail = da.select_method_wo_parameter(staffname, "Text");
                            //ds.Tables[0].DefaultView.RowFilter ="ExternalCode='"+ds.Tables[0].Rows[m]["ExternalCode"]+"' "" + colmname + "='2' and interviewdate='" + fromdat + "' and  Course_Id='" + Convert.ToString(ds.Tables[1].Rows[i]["degree"]) + "' and degree_code='" + Convert.ToString(ds.Tables[1].Rows[i]["deptcode"]) + "'";
                            //               DataView dvStudentAttendance = ds.Tables[0].DefaultView;
                            string sqlda = "select distinct convert(varchar(10),ExamDate,105) as ExamDate,ExamSession,SubSubjectID,SubNo from  examtheorybatch  eth,Exam_Details ed where eth.ExamCode=ed.exam_code and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and degree_code in (" + typ + ") and      convert(varchar(10),ExamDate,105)>='" + txt_fromdate.Text + "' AND convert(varchar(10),ExamDate,105)<='" + txt_todate.Text + "' and   ExternalCode='" + spl[sp] + "' and eth.SubSubjectID in('" + typ1 + "') ";
                            DataSet dsm = da.select_method_wo_parameter(sqlda, "Text");
                            if (dsm.Tables.Count > 0 && dsm.Tables[0].Rows.Count > 0)
                            {
                                for (int ms = 0; ms < dsm.Tables[0].Rows.Count; ms++)
                                {
                                    float stamamount = 0;
                                    drs = dt.NewRow();
                                    drs["Staff Code/Name"] = spl[sp];

                                    string subname = da.GetFunction("select subject_name from subject where subject_no='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubNo"]) + "'");
                                    string subcode = da.GetFunction("select subject_Code from subject where subject_no='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubNo"]) + "'");
                                    drs["Subject Code/Name"] = subcode + '-' + subname;

                                    drs["Exam Date"] = Convert.ToString(dsm.Tables[0].Rows[ms]["ExamDate"]);

                                    
                                    string amount = da.GetFunction(" select Amount From InvigilationFeesSetting where ExamCode='" + Convert.ToString(ds.Tables[0].Rows[m]["exam_code"]) + "' and SubjectNo='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubNo"]) + "' and SubSubjectCode='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubSubjectID"]) + "'");
                                    drs["Rem R.s"] = amount;
                                    string toreg =da.GetFunction( "select COUNT(distinct roll_no) from exam_appl_details ead,exam_application ea,Exam_Details ed where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no  and ed.exam_code='" + Convert.ToString(ds.Tables[0].Rows[m]["exam_code"]) + "' and ead.subject_no='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubNo"]) + "' ");
                                    drs["Canditate Registred"] = toreg;
                                    float.TryParse(amount, out fin_fixamo);
                                    if (dsmstaffdetail.Tables.Count > 0 && dsmstaffdetail.Tables[0].Rows.Count > 0)
                                    {
                                      
                                        string collcode = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["coll_code"]);
                                        drs["college name"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["college_name"]);
                                        drs["IFSC Code"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["ifsc_code"]);
                                        drs["Bank Name"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["bank_name"]);
                                        drs["Account Number"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["acc_no"]);
                                        drs["D.A R.S"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["daAmount"]);
                                        string staff_amo = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["daAmount"]);
                                        string km = da.GetFunction("select Institution_km from textvaltable where TextCriteria='exins' and textval='" + Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["college_name"]) + "' and TextCode='" + collcode + "'");
                                        drs["Distance In Km"] = km;
                                        float inskm=0;
                                        float.TryParse(km,out inskm);
                                        float amstaff = 0;
                                        float.TryParse(staff_amo, out amstaff);
                                        stamamount += amstaff;
                                        float colmaxkmamo = 0;
                                        float.TryParse(maxamou, out colmaxkmamo);
                                        float colminkmamo = 0;
                                        float.TryParse(minamou, out colminkmamo);
                                        string min_km = da.GetFunction("select Institution_km from textvaltable where TextCriteria='exins' and textval='" + Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["college_name"]) + "' and TextCode='" + collcode + "'");
                                        float minkm=0;
                                        float.TryParse(minkmc,out minkm);
                                        if (inskm <= minkm)
                                        {
                                            drs["T.A R.S"] = minamou;
                                            stamamount += colminkmamo;
                                        }
                                        else
                                        {
                                            if (!staffamo.ContainsKey(spl[sp]))
                                            {
                                                float am = inskm - minkm;
                                                float perkm = am * colmaxkmamo;
                                                perkm += colminkmamo;
                                                drs["T.A R.S"] = perkm;
                                                staffamo.Add(spl[sp], perkm);
                                                stamamount += perkm;
                                            }
                                            else
                                            {
                                                drs["T.A R.S"] = '-';
                                            }
                                        }
                                    }

                                    string stucun = da.GetFunction("select  COUNT (AppNo) as AppNo  from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,examtheorybatch eth  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code       and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and subject_code in('" + typ2 + "' )  and    convert(varchar(10),ExamDate,105)='" + Convert.ToString(dsm.Tables[0].Rows[ms]["ExamDate"]) + "'    and r.degree_code in (" + typ + ") and eth.appno=r.app_no   and r.college_code in (" + Convert.ToString(ddlCollege.SelectedValue) + ")  and eth.SubNo=s.subject_no  and eth.ExamCode=ea.exam_code    and eth.SubSubjectID in('" + typ1 + "') and ExternalCode='" + spl[sp] + "' and SubSubjectID='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubSubjectID"]) + "' and SubNo='" + Convert.ToString(dsm.Tables[0].Rows[m]["SubNo"]) + "'");
                                    float.TryParse(stucun, out fin_amo);
                                    drs["Canditate Examined"] = stucun;
                                    drs["Role"] = "External";
                                    stamamount += fin_amo * fin_fixamo;
                                    drs["Total Amount"] = stamamount;
                                    //  select COUNT (AppNo) from  examtheorybatch where ExamDate='" + Convert.ToString(dsm.Tables[0].Rows[ms]["ExamDate"]) + "' and ExamSession='" + Convert.ToString(dsm.Tables[0].Rows[ms]["ExamSession"]) + "' and ExternalCode='" + Convert.ToString(ds.Tables[0].Rows[m]["ExternalCode"]) + "' and SubSubjectID='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubSubjectID"]) + "'");
                                    dt.Rows.Add(drs);

                                }
                            }




                            
                        }
                       
                    }
                }
            }
            #endregion
            #region internal
            string staffs = "  select distinct isnull(InternalCode,'') as externalCode, ed.exam_code from examtheorybatch eth,Exam_Details ed where eth.ExamCode=ed.exam_code and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and degree_code in (" + typ + ") and      convert(varchar(10),ExamDate,105)>='" + txt_fromdate.Text + "' AND convert(varchar(10),ExamDate,105)<='" + txt_todate.Text + "' and eth.SubSubjectID in('" + typ1 + "') and  isnull(InternalCode,'')<>''";

            ds.Clear();
          ds = da.select_method_wo_parameter(staffs, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    drs = dt.NewRow();
                    string exstaff = Convert.ToString(ds.Tables[0].Rows[m]["ExternalCode"]);
                    string[] spl = exstaff.Split(';');
                    if (spl.Length > 0)
                    {
                        for (int sp = 0; sp < spl.Length; sp++)
                        {

                            string exstaffcode = spl[sp];
                            string[] splcode = exstaffcode.Split('-');

                            string staffname = "select co.collname as college_name,co.college_code as coll_code,s.bank_name,s.ifsc_code,s.bankaccount as acc_no ,s.daAmount  from staffmaster s,collinfo co where staff_code='" + splcode[0] + "' and co.college_code=s.college_code";
                            DataSet dsmstaffdetail = da.select_method_wo_parameter(staffname, "Text");
                          
                            string sqlda = "select distinct convert(varchar(10),ExamDate,105) as ExamDate,ExamSession,SubSubjectID,SubNo from  examtheorybatch  eth,Exam_Details ed where eth.ExamCode=ed.exam_code and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and degree_code in (" + typ + ") and      convert(varchar(10),ExamDate,105)>='" + txt_fromdate.Text + "' AND convert(varchar(10),ExamDate,105)<='" + txt_todate.Text + "' and   InternalCode='" + spl[sp] + "' and eth.SubSubjectID in('" + typ1 + "') ";
                            DataSet dsm = da.select_method_wo_parameter(sqlda, "Text");
                            if (dsm.Tables.Count > 0 && dsm.Tables[0].Rows.Count > 0)
                            {
                                for (int ms = 0; ms < dsm.Tables[0].Rows.Count; ms++)
                                {
                                    float stamamount = 0;
                                    drs = dt.NewRow();
                                    drs["Staff Code/Name"] = spl[sp];

                                    string subname = da.GetFunction("select subject_name from subject where subject_no='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubNo"]) + "'");
                                    string subcode = da.GetFunction("select subject_Code from subject where subject_no='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubNo"]) + "'");
                                    drs["Subject Code/Name"] = subcode + '-' + subname;

                                    drs["Exam Date"] = Convert.ToString(dsm.Tables[0].Rows[ms]["ExamDate"]);


                                    string amount = da.GetFunction(" select Amount From InvigilationFeesSetting where ExamCode='" + Convert.ToString(ds.Tables[0].Rows[m]["exam_code"]) + "' and SubjectNo='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubNo"]) + "' and SubSubjectCode='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubSubjectID"]) + "'");
                                    drs["Rem R.s"] = amount;
                                    string toreg = da.GetFunction("select COUNT(distinct roll_no) from exam_appl_details ead,exam_application ea,Exam_Details ed where  ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no  and ed.exam_code='" + Convert.ToString(ds.Tables[0].Rows[m]["exam_code"]) + "' and ead.subject_no='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubNo"]) + "' ");
                                    drs["Canditate Registred"] = toreg;
                                    float.TryParse(amount, out fin_fixamo);
                                    if (dsmstaffdetail.Tables.Count > 0 && dsmstaffdetail.Tables[0].Rows.Count > 0)
                                    {

                                        string collcode = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["coll_code"]);
                                        drs["college name"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["college_name"]);
                                        drs["IFSC Code"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["ifsc_code"]);
                                        drs["Bank Name"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["bank_name"]);
                                        drs["Account Number"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["acc_no"]);
                                        drs["D.A R.S"] = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["daAmount"]);
                                        string staff_amo = Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["daAmount"]);
                                        string km = da.GetFunction("select Institution_km from textvaltable where TextCriteria='exins' and textval='" + Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["college_name"]) + "' and TextCode='" + collcode + "'");
                                        drs["Distance In Km"] = km;
                                        float inskm = 0;
                                        float.TryParse(km, out inskm);
                                        float amstaff = 0;
                                        float.TryParse(staff_amo, out amstaff);
                                        stamamount += amstaff;
                                        float colmaxkmamo = 0;
                                        float.TryParse(maxamou, out colmaxkmamo);
                                        float colminkmamo = 0;
                                        float.TryParse(minamou, out colminkmamo);
                                        string min_km = da.GetFunction("select Institution_km from textvaltable where TextCriteria='exins' and textval='" + Convert.ToString(dsmstaffdetail.Tables[0].Rows[0]["college_name"]) + "' and TextCode='" + collcode + "'");
                                        float minkm = 0;
                                        float.TryParse(minkmc, out minkm);
                                        if (inskm <= minkm)
                                        {
                                            drs["T.A R.S"] = minamou;
                                            stamamount += colminkmamo;
                                        }
                                        else
                                        {
                                            if (!staffamo.ContainsKey(spl[sp]))
                                            {
                                                float am = inskm - minkm;
                                                float perkm = am * colmaxkmamo;
                                                perkm += colminkmamo;
                                                drs["T.A R.S"] = perkm;
                                                staffamo.Add(spl[sp], perkm);
                                                stamamount += perkm;
                                            }
                                            else
                                            {
                                                drs["T.A R.S"] = '-';
                                            }
                                        }
                                    }

                                    string stucun =da.GetFunction( "select  COUNT (AppNo) as AppNo  from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,examtheorybatch eth  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code       and ed.Exam_Month='" + Convert.ToString(ddlMonth1.SelectedValue) + "' and ed.Exam_year='" + Convert.ToString(ddlYear1.SelectedValue) + "' and subject_code in('" + typ2 + "' )  and    convert(varchar(10),ExamDate,105)='" + Convert.ToString(dsm.Tables[0].Rows[ms]["ExamDate"]) + "'    and r.degree_code in (" + typ + ") and eth.appno=r.app_no   and r.college_code in (" + Convert.ToString(ddlCollege.SelectedValue) + ")  and eth.SubNo=s.subject_no  and eth.ExamCode=ea.exam_code    and eth.SubSubjectID in('" + typ1 + "') and InternalCode='" + spl[sp] + "' and SubSubjectID='" + Convert.ToString(dsm.Tables[0].Rows[ms]["SubSubjectID"]) + "' and SubNo='" + Convert.ToString(dsm.Tables[0].Rows[m]["SubNo"]) + "'");
                                    float.TryParse(stucun, out fin_amo);
                                    drs["Canditate Examined"] = stucun;
                                    drs["Role"] = "Internal";
                                    stamamount += fin_amo * fin_fixamo;
                                    drs["Total Amount"] = stamamount;
                                    dt.Rows.Add(drs);

                                }
                            }

                        }

                    }
                }
            }
            #endregion

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string colno = Convert.ToString(dt.Columns[j]);
                if (!ItemList.Contains(Convert.ToString(colno)))
                {
                    int insdex = ItemList.IndexOf(Convert.ToString(colno));
                    dt.Columns.Remove(Convert.ToString(colno));
                  
                    j--;
                }


            }
            if (dt.Rows.Count > 1)
            {
                gview.DataSource = dt;
                gview.DataBind();
                gview.Visible = true;
                div_report.Visible = true;
                #region span
                for (int i = gview.Rows.Count - 1; i >= 1; i--)
                {
                    GridViewRow row = gview.Rows[i];
                    GridViewRow previousRow = gview.Rows[i - 1];
                    for (int j = 0; j < row.Cells.Count; j++)
                    {

                        if (gview.Rows[0].Cells[j].Text == "Staff Code/Name" || gview.Rows[0].Cells[j].Text == "Subject Code/Name" || gview.Rows[0].Cells[j].Text == "Exam Date" || gview.Rows[0].Cells[j].Text == "College Name")
                        {
                            string date = row.Cells[j].Text;
                            string predate = previousRow.Cells[j].Text;
                            if (date == predate)
                            {
                                if (previousRow.Cells[j].RowSpan == 0)
                                {
                                    if (row.Cells[j].RowSpan == 0)
                                    {
                                        previousRow.Cells[j].RowSpan += 2;
                                    }
                                    else
                                    {
                                        previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                    }
                                    row.Cells[j].Visible = false;
                                }
                            }
                        }
                    }
                   
                }

                //for (int j = row.Cells.Count - 1; j >= 1; j--)
                //{
                //    GridViewRow rows = gview.Rows[0];
                //    GridViewRow previousRows = gview.Rows[0];
                //    GridViewRow previousRowss = gview.Rows[2];
                //    string date = gview.Rows[0].Cells[j].Text;
                //    string predate = gview.Rows[0].Cells[j - 1].Text;
                //}
                for (int m = gview.Rows.Count - 1; m >= 1; m--)
                {
                    GridViewRow rows = gview.Rows[m];
                    GridViewRow previousRows = gview.Rows[m];
                    GridViewRow previousRowss = gview.Rows[m];
                    //gview.Rows[m].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    //gview.Rows[m].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    //gview.Rows[m].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                    //gview.Rows[m].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                   
                   
                }

                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                //    string colno = Convert.ToString(dt.Columns[j]);
                //    if (!ItemList.Contains(Convert.ToString(colno)))
                //    {
                //        int insdex = ItemList.IndexOf(Convert.ToString(colno));
                //        dt.Columns.Remove(Convert.ToString(colno));
                //        gview.HeaderRow.Cells[j].Visible = false;
                //        j--;
                //    }


                //}
                RowHead(gview);
                #endregion span

            }
            else
            {

                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record";
                gview.Visible = false;
            }

        }
        catch
        {
        }

    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 1; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }
    public void cb_subsubject_Change(object sender, EventArgs e)
    {
        try
        {

            if (cb_subsubject.Checked == true)
            {
                for (int i = 0; i < cbl_subsubject.Items.Count; i++)
                {
                    if (cb_subsubject.Checked == true)
                    {
                        cbl_subsubject.Items[i].Selected = true;
                        txtsubsubject.Text = "Sub-Subject(" + (cbl_subsubject.Items.Count) + ")";

                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_subsubject.Items.Count; i++)
                {
                    cbl_subsubject.Items[i].Selected = false;
                    txtsubsubject.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_subsubject_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_subsubject.Checked = false;
            for (int i = 0; i < cbl_subsubject.Items.Count; i++)
            {
                if (cbl_subsubject.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtsubsubject.Text = "--Select--";

                }
            }
            if (seatcount == cbl_subsubject.Items.Count)
            {
                txtsubsubject.Text = "Sub-Subject(" + seatcount.ToString() + ")";
                cb_subsubject.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtsubsubject.Text = "--Select--";
                cb_subsubject.Text = "--Select--";
            }
            else
            {
                txtsubsubject.Text = "Sub-Subject(" + seatcount.ToString() + ")";
            }

        }
        catch (Exception ex)
        {
        }
    }
    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
              //  tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();
                }
              //  tborder.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    //cblcolumnorder.Items[0].Selected = true;
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
           // tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                //tborder.Text = tborder.Text + ItemList[i].ToString();
                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
            }
          //  tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = Session["usercode"].ToString();
            string degreedetails = "Student Strength Status Report";
            string pagename = "StudentStrengthStatusReport.aspx";
            NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
            ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
            NEWPrintMater1.Visible = true;
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(gview, report);
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
        catch (Exception ex) {  }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
}