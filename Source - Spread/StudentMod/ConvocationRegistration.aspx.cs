using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Configuration;
public partial class ConvocationRegistration : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
    static bool flage = false;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int chosedmode = 0;
    static int personmode = 0;
    static byte roll = 0;
    static string stcollegecode = string.Empty;
    Boolean Cellclick = false;
    static ArrayList colord = new ArrayList();

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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                if (ddlcollegename.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
                loadcollege();
                bindBtch();
                binddeg();
                binddept();
                bindtype();//Added by Saranya on 12/9/2018
                columnType();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
            }
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }

    #region college

    public void loadcollege()
    {
        ddlcollegename.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollegename);
    }

    protected void ddlcollegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            }
            bindBtch();
            binddeg();
            binddept();
            bindtype();
        }
        catch
        {
        }
    }
  
    #endregion

    #region batch

    public void bindBtch()
    {
        try
        {
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = lblbatch.Text + "(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
    }

    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
    }

    #endregion

    #region degree

    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            cbl_degree.Items.Clear();
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
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
                    txt_degrees.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degrees, lbldeg.Text, "--Select--");
        binddept();
    }

    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degrees, lbldeg.Text, "--Select--");
        binddept();
    }

    #endregion

    #region dept

    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    else
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                }
            }
            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    else
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                }
            }
            string collegecode = ddlcollegename.SelectedItem.Value.ToString();
            if (batch != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }
        }
        catch { }
    }

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
    }

    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
    }

    #endregion

    public void bindstate()
    {
        try
        {
            string statequry = "select TextCode,textval from textvaltable where TextCriteria like '%state%' and college_code=13 and textval<>''and textval<>'-' ";//and TextCriteria2='state1'
            ds.Clear();
            ds = d2.select_method_wo_parameter(statequry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_officestate.DataSource = ds;
                ddl_officestate.DataTextField = "textval";
                ddl_officestate.DataValueField = "TextCode";
                ddl_officestate.DataBind();
                ddlstate.DataSource = ds;
                ddlstate.DataTextField = "textval";
                ddlstate.DataValueField = "TextCode";
                ddlstate.DataBind();
            }
            ddlstate.Items.Insert(0, "Select");
            ddlstate.Items.Insert(ddlstate.Items.Count, "Others");
            ddl_officestate.Items.Insert(0, "Select");
            ddl_officestate.Items.Insert(ddl_officestate.Items.Count, "Others");
        }
        catch
        {
        }
    }

    public void bindgrid()
    {
        try
        {
            DataTable data = new DataTable();
            data.Columns.Add("S.No", typeof(string));
            data.Columns.Add("type", typeof(string));
            DataRow dar = null;
            dar = data.NewRow();
            dar[1] = "Depth of the Course content including project work, if any";
            data.Rows.Add(dar);
            dar = data.NewRow();
            dar[1] = "Extent of coverage of course";
            data.Rows.Add(dar);
            dar = data.NewRow();
            dar[1] = "Applicability-relevance  to real life situations";
            data.Rows.Add(dar);
            dar = data.NewRow();
            dar[1] = "Learning value (in terms of knowledge,  concepts, manual skills, analytical abilities and broadening  perspectives)";
            data.Rows.Add(dar);
            dar = data.NewRow();
            dar[1] = "Clarity and relevance of textual reading material";
            data.Rows.Add(dar);
            dar = data.NewRow();
            dar[1] = "Relevance of additional source material (Library)";
            data.Rows.Add(dar);
            dar = data.NewRow();
            dar[1] = "Extent of effort required by students";
            data.Rows.Add(dar);
            dar = data.NewRow();
            dar[1] = "Overall rating";
            data.Rows.Add(dar);
            feedbackgrid.DataSource = data;
            feedbackgrid.DataBind();
        }
        catch
        {
        }
    }

    //roll,reg,admission no  source college

    #region  add college

    public void loadcollegeAdd()
    {
        ddladdclg.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddladdclg);
        if (ddladdclg.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddladdclg.SelectedItem.Value);
            stcollegecode = Convert.ToString(ddladdclg.SelectedItem.Value);
        }
    }

    protected void ddladdclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddladdclg.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddladdclg.SelectedItem.Value);
                stcollegecode = Convert.ToString(ddladdclg.SelectedItem.Value);
            }
            loadfromsetting();
            clearDetails();
        }
        catch
        {
        }
    }
   
    #endregion

    public void loadfromsetting()
    {
        try
        {
            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");
            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + ddladdclg.SelectedValue + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + ddladdclg.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + ddladdclg.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + ddladdclg.SelectedValue + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(lst4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
            }
        }
        catch (Exception ex) { }
    }

    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    lblroll.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    lblroll.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    lblroll.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    lblroll.Text = "App No";
                    chosedmode = 3;
                    break;
            }
        }
        catch (Exception ex) { }
    }

    public void txt_roll_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool boolClear = false;
            string rollno = Convert.ToString(txt_roll.Text);
            if (!string.IsNullOrEmpty(rollno))
            {
                string appNo = getappNo(rollno);
                if (!string.IsNullOrEmpty(appNo) && appNo != "0")
                {
                    getStudentDet(appNo);
                }
                else
                {
                    clearDetails();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Student Not Valid')", true);
                }
            }
            else
            {
                clearDetails();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Student Not Valid')", true);
            }
        }
        catch (Exception ex) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            #region transfer
            if (personmode == 0)
            {
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_No like '" + prefixText + "%' and r.college_code='" + stcollegecode + "'  order by  Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Reg_No like '" + prefixText + "%' and r.college_code='" + stcollegecode + "'  order by  Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_admit like '" + prefixText + "%' and r.college_code='" + stcollegecode + "'  order by  Roll_admit asc";
                }
                else
                {
                    //if (admis == 2)
                    //{
                    //    query = "  select  top 100 app_formno from applyn a ,Registration r where a.app_no=r.App_No and admission_status =1 and selection_status=1 and isconfirm ='1' and DelFlag =0 and app_formno like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' and r.current_semester='1' order by  app_formno asc";
                    //}
                    //else
                    //{
                    query = "  select  top 100 app_formno from applyn where isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + stcollegecode + "'  order by  app_formno asc";
                    //}
                }
            }
            #endregion
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    protected string getappNo(string rollno)
    {
        string appNo = string.Empty;
        try
        {
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    appNo = d2.GetFunction("select app_no from registration where Roll_no='" + rollno + "' ");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    appNo = d2.GetFunction("select app_no from registration where reg_no='" + rollno + "' ");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    appNo = d2.GetFunction("select app_no from registration where roll_admit='" + rollno + "' ");
            }
            else
                appNo = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' ");
            if (appNo == "0")
            {
                appNo = d2.GetFunction("select app_no from registration where Roll_no='" + rollno + "' ");
                if (appNo == "0")
                    appNo = d2.GetFunction("select app_no from registration where reg_no='" + rollno + "' ");
                if (appNo == "0")
                    appNo = d2.GetFunction("select app_no from registration where roll_admit='" + rollno + "' ");
                if (appNo == "0")
                    appNo = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' ");
            }
        }
        catch { appNo = "0"; }
        return appNo;
    }

    protected void getStudentDet(string appNo, byte IsAlumni = 0)
    {
        try
        {
            string batch_year = "";
            //string inbatch_yearvalue = "";
            //string query = "select * from Master_Settings where settings='Alumnibatchyear'";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(query, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    batch_year = Convert.ToString(ds.Tables[0].Rows[0]["value"]);
            //    if (batch_year.Trim() != "" && batch_year.Trim() != null)
            //    {
            //        string[] splitvalue = batch_year.Split(',');
            //        if (splitvalue.Length > 0)
            //        {
            //            for (int ik = 0; ik <= splitvalue.GetUpperBound(0); ik++)
            //            {
            //                string firstvlaue = Convert.ToString(splitvalue[ik]);
            //                if (inbatch_yearvalue == "")
            //                    inbatch_yearvalue = firstvlaue;
            //                else
            //                    inbatch_yearvalue = inbatch_yearvalue + "'" + "," + "'" + firstvlaue;
            //            }
            //        }
            //    }
            //}
            if (appNo != "0")//inbatch_yearvalue.Trim() != ""//!string.IsNullOrEmpty(inbatch_yearvalue) &&
            {
                string selectquery = "select dt.Dept_Name ,c.Course_Name ,a.stud_name,a.app_no,r.Batch_Year,a.CampusReq,c.type,r.isalumni,a.sex,d.college_code,d.Duration,Exam_System,convert(varchar(10),dob,103) as dob,parent_name,stud_nametamil,parent_addressp,Student_Mobile,StuPer_Id,student_officename,student_designation,student_officeaddress,student_officeaddress2,student_officecity,student_officestate,student_officephone,Streetp,Cityp,parent_statep,parent_phnop,isalumni,convocation_Amount,convert(varchar(10),convocation_PaidDate,103)convocation_PaidDate,convocation_Remark from applyn a,Registration r,course c,Degree d,Department dt where a.app_no=r.App_No and (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and a.degree_code =d.Degree_Code and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and r.app_no='" + appNo + "'";// and r.batch_year in('" + inbatch_yearvalue + "')
                // and a.dob='" + dt.ToString("MM/dd/yyyy") + "' 
                // selectquery = selectquery + " and r.batch_year in ('" + inbatch_yearvalue + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtPaidDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtPaidDt.Attributes.Add("readonly", "readonly");
                    string isalumni = Convert.ToString(ds.Tables[0].Rows[0]["isalumni"]);
                    if (IsAlumni != 0)
                    {
                        if (isalumni == "True" || isalumni == "0")
                        {
                            isalumni = string.Empty;
                            confirm.Checked = true;
                            txtamount.Text = Convert.ToString(ds.Tables[0].Rows[0]["convocation_Amount"]);
                            txtPaidDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["convocation_PaidDate"]);
                            txtPaidDt.Attributes.Add("readonly", "readonly");
                            txtremark.Text = Convert.ToString(ds.Tables[0].Rows[0]["convocation_Remark"]);
                        }
                    }
                    if (isalumni.Trim() == null || isalumni.Trim() == "")
                    {
                        string dob = Convert.ToString(ds.Tables[0].Rows[0]["dob"]);
                        Session["sex"] = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                        if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
                            gen.InnerHtml = "Mr";
                        else
                            gen.InnerHtml = "Ms";
                        if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
                            genf.InnerHtml = "Mr";
                        else
                            genf.InnerHtml = "Ms";
                        string examtype = Convert.ToString(ds.Tables[0].Rows[0]["Exam_System"]);
                        string duration = Convert.ToString(ds.Tables[0].Rows[0]["Duration"]);
                        int yearcount = 0;
                        if (examtype.ToString().ToUpper() == "SEMESTER")
                            yearcount = Convert.ToInt32(duration) / 2;
                        else
                            yearcount = Convert.ToInt32(duration);
                        string sex = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                        Session["college_code"] = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                        Session["type"] = Convert.ToString(ds.Tables[0].Rows[0]["type"]);
                        Session["appno"] = Convert.ToString(ds.Tables[0].Rows[0]["App_No"]);
                        txt_studentname.Text = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
                        Session["student_Fullname"] = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
                        string degree_code = "";
                        degree_code = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
                        txt_degree.Text = degree_code;
                        txt_depatment.Text = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);
                        int batchyear = Convert.ToInt32(ds.Tables[0].Rows[0]["Batch_Year"]);
                        batchyear = batchyear + yearcount;
                        string batchYEar = Convert.ToString(ds.Tables[0].Rows[0]["Batch_Year"]);
                        txt_batchyear.Text = batchYEar + "-" + Convert.ToString(batchyear);
                        //added by sudhagar
                        //int bathYEAR = 0;
                        //int curYEar = 0;
                        //int.TryParse(Convert.ToString(DateTime.Now.ToString("yyyy")), out curYEar);
                        //int.TryParse(batchYEar, out bathYEAR);
                        //int passYEar = curYEar - bathYEAR;
                        txt_studNameTamil.Text = Convert.ToString(ds.Tables[0].Rows[0]["stud_nametamil"]);
                        txtfatherName.Text = Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]);
                        txtpassyear.Text = Convert.ToString(batchyear);
                        txtpassyear.Attributes.Add("readonly", "readonly");
                        txtemailid.Text = Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]);
                        txtmobileno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]);
                        txtaddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressp"]);
                        string streetp = Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]);//Streetp
                        txtaddressline2.Text = streetp.Contains("-") ? Convert.ToString(streetp.Split('-')[0]) : !string.IsNullOrEmpty(streetp) ? streetp : "";
                        txtaddressline3.Text = streetp.Contains("-") ? Convert.ToString(streetp.Split('-')[1]) : "";
                        // 
                        string getCity = d2.GetFunction("select textval from textvaltable where textcode='" + Convert.ToString(ds.Tables[0].Rows[0]["Cityp"]) + "' and textcriteria='city' and college_code='" + collegecode + "'");
                        if (getCity == "0")
                        {
                            getCity = Convert.ToString(ds.Tables[0].Rows[0]["Cityp"]);
                        }
                        txt_City.Text = Convert.ToString(getCity);
                        string state = Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]);
                        if (!string.IsNullOrEmpty(state))
                            ddlstate.SelectedIndex = ddlstate.Items.IndexOf(ddlstate.Items.FindByValue(state));  //parent_statep
                        string parPhonenum = Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]);
                        txt_residentstd.Text = parPhonenum.Contains("-") ? Convert.ToString(parPhonenum.Split('-')[0]) : "";//parent_phnop
                        txt_residentphone.Text = parPhonenum.Contains("-") ? Convert.ToString(parPhonenum.Split('-')[1]) : ""; //parent_phnop
                        txt_officename.Text = Convert.ToString(ds.Tables[0].Rows[0]["student_officename"]);//student_officename
                        txt_designation.Text = Convert.ToString(ds.Tables[0].Rows[0]["student_designation"]);//student_designation
                        txt_officeaddressline1.Text = Convert.ToString(ds.Tables[0].Rows[0]["student_officeaddress"]);//student_officeaddress
                        string offAddress = Convert.ToString(ds.Tables[0].Rows[0]["student_officeaddress2"]);
                        txt_officeaddressline2.Text = offAddress.Contains("-") ? Convert.ToString(offAddress.Split('-')[0]) : !string.IsNullOrEmpty(offAddress) ? offAddress : "";//student_officeaddress2
                        txt_officeaddressline3.Text = offAddress.Contains("-") ? Convert.ToString(offAddress.Split('-')[1]) : "";//student_officeaddress2
                        txt_officecity.Text = Convert.ToString(ds.Tables[0].Rows[0]["student_officecity"]);//student_officecity
                        string offPhonenum = Convert.ToString(ds.Tables[0].Rows[0]["student_officephone"]);
                        txt_officestd.Text = offPhonenum.Contains("-") ? Convert.ToString(offPhonenum.Split('-')[0]) : "";
                        txt_officephonenumber.Text = offPhonenum.Contains("-") ? Convert.ToString(offPhonenum.Split('-')[1]) : "";//student_officephone
                        string offState = Convert.ToString(ds.Tables[0].Rows[0]["student_officestate"]);
                        if (!string.IsNullOrEmpty(offState))
                            ddl_officestate.SelectedIndex = ddl_officestate.Items.IndexOf(ddl_officestate.Items.FindByValue(offState));  //p
                        //ddl_officestate    //student_officestate
                        txtpassmnth.Text = "4";
                        // maindiv.Visible = false;
                    }
                    else
                    {
                        //step7.Visible = false;
                        ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Yor are Already Register\");", true);
                    }
                }
                else
                {
                    step7.Visible = false;
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Not a Valid User\");", true);
                }
            }
        }
        catch
        {
        }
    }

    protected void clearDetails()
    {
        txt_roll.Text = string.Empty;
        txt_studentname.Text = string.Empty;
        txt_studNameTamil.Text = string.Empty;
        txt_degree.Text = string.Empty;
        txt_depatment.Text = string.Empty;
        txt_batchyear.Text = string.Empty;
        txtfatherName.Text = string.Empty;
        txtamount.Text = string.Empty;
        txtPaidDt.Text = string.Empty;
        txtpassmnth.Text = string.Empty;
        txtpassyear.Text = string.Empty;
        txtremark.Text = string.Empty;
        confirm.Checked = false;
        Cancel.Checked = false;
        txtaddress.Text = string.Empty;
        txtaddressline2.Text = string.Empty;
        txtaddressline3.Text = string.Empty;
        txt_City.Text = string.Empty;
        txtemailid.Text = string.Empty;
        txtmobileno.Text = string.Empty;
        txt_residentstd.Text = string.Empty;
        txt_residentphone.Text = string.Empty;
        txt_officename.Text = string.Empty;
        txt_designation.Text = string.Empty;
        txt_officeaddressline1.Text = string.Empty;
        txt_officeaddressline2.Text = string.Empty;
        txt_officeaddressline3.Text = string.Empty;
        txt_officecity.Text = string.Empty;
        txt_officestd.Text = string.Empty;
        txt_officephonenumber.Text = string.Empty;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        txt_roll.Enabled = true;
        clearDetails();
        loadcollegeAdd();
        loadfromsetting();
        rbl_rollno_OnSelectedIndexChanged(sender, e);
        bindgrid();
        bindstate();
        cbfeedback.Checked = false;
        div_addNew.Visible = true;
    }

    protected void imgAddNewclose_Click(object sender, EventArgs e)
    {
        div_addNew.Visible = false;
    }

    #region Add new Screen

    public string subjectcode(string textcri, string subjename, string collegecode)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode + " and TextVal='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
            else
            {
                string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + collegecode + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode + " and TextVal='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }

    protected void save_click(object sender, EventArgs e)
    {
        try
        {
            string appNo = getappNo(txt_roll.Text);
            if (!string.IsNullOrEmpty(appNo) && appNo != "0")
            {
                string collegecode = Convert.ToString(ddladdclg.SelectedValue);
                string emailid = Convert.ToString(txtemailid.Text);
                string mobileno = Convert.ToString(txtmobileno.Text);
                string address = Convert.ToString(txtaddress.Text);
                string secontaddress = Convert.ToString(txtaddressline2.Text) + "-" + Convert.ToString(txtaddressline3.Text);
                string city = Convert.ToString(txt_City.Text);
                string phone_number = Convert.ToString(txt_residentstd.Text) + "-" + Convert.ToString(txt_residentphone.Text);
                string officename = Convert.ToString(txt_officename.Text);
                string officedesig = Convert.ToString(txt_designation.Text);
                string officeaddress = Convert.ToString(txt_officeaddressline1.Text);
                string secondofficeaddress = Convert.ToString(txt_officeaddressline2.Text) + "-" + Convert.ToString(txt_officeaddressline3.Text);
                string officecity = Convert.ToString(txt_officecity.Text);
                string officephone_number = Convert.ToString(txt_officestd.Text) + "-" + Convert.ToString(txt_officephonenumber.Text);
                string txt = "";
                string state = "";
                if (ddlstate.SelectedItem.Text != "Others" && ddlstate.SelectedItem.Text != "Select")
                    state = ddlstate.SelectedItem.Value;
                else if (ddlstate.SelectedItem.Text != "Select")
                {
                    txt = "state";
                    string state_value = Convert.ToString(txt_state.Text);
                    if (state_value.Trim() != "")
                    {
                        state = subjectcode(txt, state_value, collegecode);
                    }
                }
                if (state.Trim() == "")
                    state = "0";
                string officestate = "";
                if (ddl_officestate.SelectedItem.Text != "Others" && ddl_officestate.SelectedItem.Text != "Select")
                    officestate = ddl_officestate.SelectedItem.Value;
                else if (ddl_officestate.SelectedItem.Text != "Select")
                {
                    txt = "state";
                    string state_value = Convert.ToString(txt_officestate.Text);
                    if (state_value.Trim() != "")
                        officestate = subjectcode(txt, state_value, collegecode);
                }
                if (officestate.Trim() == "")
                    officestate = "0";
                // string proffession = Convert.ToString(txtprofession.Text);
                string isalumni = "";
                if (confirm.Checked == true)
                    isalumni = "1";
                if (Cancel.Checked == true)
                    isalumni = "0";
                Session["Alumni"] = Convert.ToString(isalumni);
                string reMark = Convert.ToString(txtremark.Text);
                double payAmt = 0;
                double.TryParse(Convert.ToString(txtamount.Text), out payAmt);
                string date = Convert.ToString(txtPaidDt.Text);
                date = date.Split('/')[1] + "/" + date.Split('/')[0] + "/" + date.Split('/')[2];
                string valueadd = "";
                if (feedbackgrid.Rows.Count > 0)
                {
                    for (int jk = 0; jk < feedbackgrid.Rows.Count; jk++)
                    {
                        string start_value = "";
                        if ((feedbackgrid.Rows[jk].FindControl("rdbverygood") as RadioButton).Checked == true)
                            start_value = "3";
                        else if ((feedbackgrid.Rows[jk].FindControl("rdbgood") as RadioButton).Checked == true)
                            start_value = "2";
                        else if ((feedbackgrid.Rows[jk].FindControl("rdbsatisfactory") as RadioButton).Checked == true)
                            start_value = "1";
                        else if ((feedbackgrid.Rows[jk].FindControl("rdbunsatisfactoty") as RadioButton).Checked == true)
                            start_value = "0";
                        if (valueadd == "")
                            valueadd = start_value;
                        else
                            valueadd = valueadd + "-" + start_value;
                    }
                }
                #region old feeback
                //if (valueadd != "")
                //{
                //    string[] splitvalue = valueadd.Split('-');
                //    if (splitvalue.Length > 0)
                //    {
                //        string inserquery = "if not exists (select * from Studentfeedback where app_no ='" + appNo + "') insert into Studentfeedback values('" + appNo + "','" + Convert.ToString(splitvalue[0]) + "','" + Convert.ToString(splitvalue[1]) + "','" + Convert.ToString(splitvalue[2]) + "','" + Convert.ToString(splitvalue[3]) + "','" + Convert.ToString(splitvalue[4]) + "','" + Convert.ToString(splitvalue[5]) + "','" + Convert.ToString(splitvalue[6]) + "','" + Convert.ToString(splitvalue[7]) + "') else update Studentfeedback set param_1='" + Convert.ToString(splitvalue[0]) + "',param_2 ='" + Convert.ToString(splitvalue[1]) + "',param_3 ='" + Convert.ToString(splitvalue[2]) + "', param_4 ='" + Convert.ToString(splitvalue[3]) + "',param_5 ='" + Convert.ToString(splitvalue[4]) + "',param_6 ='" + Convert.ToString(splitvalue[5]) + "',param_7 ='" + Convert.ToString(splitvalue[6]) + "',param_8 ='" + Convert.ToString(splitvalue[7]) + "' where app_no ='" + appNo + "'";
                //        int res = d2.update_method_wo_parameter(inserquery, "Text");
                //    }
                //}
                #endregion
                string updatequery = "update applyn set StuPer_Id ='" + emailid + "' ,Student_Mobile ='" + mobileno + "' ,parent_addressp ='" + address + "',Streetp='" + secontaddress + "',Cityp='" + city + "',parent_statep='" + state + "',parent_phnop='" + phone_number + "',student_officename='" + officename + "',student_designation='" + officedesig + "',student_officeaddress='" + officeaddress + "',student_officeaddress2='" + secondofficeaddress + "',student_officecity='" + officecity + "',student_officestate='" + officestate + "',student_officephone='" + officephone_number + "',AlumniRegisterDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',convocation_Amount='" + payAmt + "',convocation_PaidDate='" + date + "',convocation_Remark='" + reMark + "',stud_nametamil=N'" + txt_studNameTamil.Text.Trim() + "' where college_code='" + collegecode + "' and app_no='" + appNo + "'";
                updatequery = updatequery + " update Registration set isalumni='" + isalumni + "' where college_code='" + collegecode + "' and App_No='" + appNo + "'";
                int a = d2.update_method_wo_parameter(updatequery, "Text");
                if (a != 0)
                {
                    //sendsms();
                    // sendmail();
                    clearDetails();
                    btnGo_Click(sender, e);
                    //Button1.Visible = true;
                    //Button2.Visible = true;
                    // flage = true;
                    if (txt_roll.Enabled == false)
                        div_addNew.Visible = false;
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Your Information Successfully Saved\");", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Student Not Valid')", true);
            }
        }
        catch
        {
        }
    }

    protected void click_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbfeedback.Checked == true)
            {
                step7.Visible = true;
                mptancet1.Show();
            }
            else
            {
                mptancet1.Hide();
            }
        }
        catch
        {
        }
    }

    protected void Ok_Click(object sender, EventArgs e)
    {
        try
        {
            mptancet1.Hide();
            if (flage == false)
            {
                Button1.Visible = true;
                Button2.Visible = true;
                cbfeedback.Checked = true;
            }
            else if (flage == true)
            {
                Button1.Visible = false;
                Button2.Visible = true;
                cbfeedback.Checked = true;
            }
        }
        catch
        {
        }
    }

    protected void Cancel_Click(object sender, EventArgs e)
    {
        mptancet1.Hide();
        Button1.Visible = false;
        cbfeedback.Checked = false;
    }

    protected void Exit_click(object sender, EventArgs e)
    {
        div_addNew.Visible = false;
    }

    #endregion

    #region Base Screen and report

    protected DataSet getDetails(string selectCol)
    {
        DataSet dsload = new DataSet();
        try
        {
            string collegecode = Convert.ToString(ddlcollegename.SelectedValue);
            string batchYEar = Convert.ToString(getCblSelectedValue(cbl_batch));
            string deptCode = Convert.ToString(getCblSelectedValue(cbl_dept));
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string selQ = "";
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string type = ddltype.SelectedItem.Text;
            if (!string.IsNullOrEmpty(deptCode))
            {
                //string selQ = "select r.roll_no[Roll No],r.reg_no[Reg No],r.roll_admit[Admission No],r.stud_name[Student Name(English)],a.stud_nametamil[Student Name(Tamil)],(select (c.course_name+'-'+dt.dept_name) from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code)[Department],convocation_Amount[Amount],convert(varchar(10),convocation_PaidDate,103)[Paid Date],convocation_Remark[Remark],StuPer_Id [Student Mail],Student_Mobile[Mobile No],parent_name[Father Name],case when isalumni='1' then 'YES' when isalumni='0' then 'NO' end [Attending Convocation],r.app_no from registration r,applyn a where r.app_no=a.app_no and r.batch_year in('" + batchYEar + "') and r.degree_code in('" + deptCode + "') and r.college_code='" + collegecode + "' and AlumniRegisterDate between '" + fromdate + "' and '" + todate + "'";
                //r.stud_name[Student Name(English)],a.stud_nametamil[Student Name(Tamil)],(select (c.course_name+'-'+dt.dept_name) from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code)[Department],convocation_Amount[Amount],convert(varchar(10),convocation_PaidDate,103)[Paid Date],convocation_Remark[Remark],StuPer_Id [Student Mail],Student_Mobile[Mobile No],parent_name[Father Name],case when isalumni='1' then 'YES' when isalumni='0' then 'NO' end [Attending Convocation],a.parent_addressP + '-'+ a.Streetp + '-' + a.cityp[Address],a.student_officename[Office Name],a.student_designation[Designation],student_officeaddress2 + '-' + student_officecity[Office Address]
                if (RbBoth.Checked == true)
                {
                    //abarna
                    selQ = "select r.roll_no[Roll No],r.reg_no[Reg No],r.roll_admit[Admission No]," + selectCol + ",r.app_no from registration r,applyn a,graduationadditional g where r.app_no=a.app_no and g.app_no=r.app_no and g.app_no=a.app_no and r.batch_year in('" + batchYEar + "') and r.degree_code in('" + deptCode + "') and r.college_code='" + collegecode + "' and r.AlumniRegisterDate between '" + fromdate + "' and '" + todate + "'";//modified by rajasekar 24/07/2018 
                }

                #region added by Saranya on 12/9/2018

                if (Rbapply.Checked == true)
                {
                    selQ = "select r.roll_no[Roll No],r.reg_no[Reg No],r.roll_admit[Admission No]," + selectCol + ",r.app_no from  applyn a,Registration r,Degree d,Department dt,course c,graduationadditional g where a.app_no =r.App_No and g.app_no=r.app_no and g.app_no=a.app_no and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='1' and a.degree_code=r.degree_code and r.degree_code in ('" + deptCode + "') and c.type='" + type + "' and r.AlumniRegisterDate between '" + fromdate + "' and '" + todate + "'  order by case sex when 2 then -1 else sex end desc ";
                }
                if (Rbnotapply.Checked == true)
                {
                    selQ = "select r.roll_no[Roll No],r.reg_no[Reg No],r.roll_admit[Admission No]," + selectCol + ",r.app_no from  applyn a,Registration r,Degree d,Department dt,course c,graduationadditional g where a.app_no =r.App_No and g.app_no=r.app_no and g.app_no=a.app_no and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='0' and a.degree_code=r.degree_code and r.degree_code in ('" + deptCode + "') and c.type='" + type + "' and r.AlumniRegisterDate between '" + fromdate + "' and '" + todate + "'  order by case sex when 2 then -1 else sex end desc";
                }

                #endregion

                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");
            }
        }
        catch { }
        return dsload;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        Printcontrolhed.Visible = false;
        ds.Clear();
        string selColumn = getSelectedColumn();//get selected column name developed by abarna
        ds = getDetails(selColumn);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //developed by abarna
            if (Rbnotapply.Checked == true)
            {
                loadNotAttendingDetails(ds);
            }
            else
            {
                loadStudentDetails(ds);
            }
        }
        else
        {
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            spreadNotAttend.Visible = false;
            print.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }

    protected void loadStudentDetails(DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            Hashtable htcol = new Hashtable();
            DataTable dtPaid = ds.Tables[0].DefaultView.ToTable();
            Hashtable htRealName = htcolumnHeaderValue();
            for (int row = 0; row < dtPaid.Columns.Count; row++)
            {
                string colName = Convert.ToString(dtPaid.Columns[row].ColumnName);
                spreadDet.Sheets[0].ColumnCount++;
                int col = spreadDet.Sheets[0].ColumnCount - 1;
                htcol.Add(colName, col);
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = colName;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Center;
                switch (colName.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[col].Width = 150;
                        admNo = Convert.ToInt32(col);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[col].Width = 110;
                        rollNo = Convert.ToInt32(col);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[col].Width = 110;
                        regNo = Convert.ToInt32(col);
                        boolroll = true;
                        break;
                    //case "Semester":
                    //    spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Center;
                    //    break;
                }
            }
            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Visible = false;
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            #endregion

            #region value
            int rowCnt = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            for (int row = 0; row < dtPaid.Rows.Count; row++)
            {
                spreadDet.Sheets[0].RowCount++;
                int rowSpCnt = spreadDet.Sheets[0].RowCount - 1;
                spreadDet.Sheets[0].Cells[rowSpCnt, 0].Tag = Convert.ToString(dtPaid.Rows[row]["app_no"]);
                spreadDet.Sheets[0].Cells[rowSpCnt, 1].Tag = Convert.ToString(dtPaid.Rows[row]["Roll No"]);
                spreadDet.Sheets[0].Cells[rowSpCnt, 0].Text = Convert.ToString(++rowCnt);
                for (int col = 0; col < dtPaid.Columns.Count; col++)
                {
                    string colName = Convert.ToString(dtPaid.Columns[col].ColumnName);
                    int ColCnt = 0;
                    // string viewcolName = Convert.ToString(htRealName[colName.Trim()]);
                    int.TryParse(Convert.ToString(htcol[colName]), out ColCnt);
                    string colValue = Convert.ToString(dtPaid.Rows[row][col]);
                    if (colValue.ToLower() == "select")
                    {
                        spreadDet.Sheets[0].Cells[rowSpCnt, ColCnt].Text = "";
                    }
                    else
                    {
                        spreadDet.Sheets[0].Cells[rowSpCnt, ColCnt].Text = colValue;
                    }
                    switch (colName.Trim())
                    {
                        case "Admission No":
                        case "Roll No":
                        case "Reg No":
                        case "Mobile No":
                            spreadDet.Sheets[0].Cells[rowSpCnt, ColCnt].CellType = txtroll;
                            break;
                    }
                }
            }
            spreadDet.SaveChanges();
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
            #endregion
        }
        catch { }
    }

    protected string getSelectedColumn()
    {
        string val = string.Empty;
        try
        {
            StringBuilder strCol = new StringBuilder();
            StringBuilder grpstrCol = new StringBuilder();
            Hashtable htcolumn = htcolumnValue();
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splVal = selQ.Split(',');
                    if (splVal.Length > 0)
                    {
                        for (int row = 0; row < splVal.Length; row++)
                        {
                            string tempSel = Convert.ToString(htcolumn[splVal[row].Trim()]);
                            //if (rdbtype.Items.Count > 0 && rdbtype.SelectedIndex == 2 && tempSel.Trim() == "a.app_formno")
                            //    tempSel = "r.roll_admit";
                            strCol.Append(tempSel + ",");
                            //if (tempSel != "sum(debit) as debit" && tempSel != "sum(credit) as credit")
                            //{
                            //    if (tempSel == "convert(varchar(10),transdate,103)as transdate")
                            //        tempSel = "transdate";
                            //    grpstrCol.Append(tempSel + ",");
                            //}
                        }
                    }
                }
                if (strCol.Length > 0)//&& grpstrCol.Length > 0
                {
                    strCol.Remove(strCol.Length - 1, 1);
                    val = Convert.ToString(strCol);
                    //grpstrCol.Remove(grpstrCol.Length - 1, 1);
                    //groupStr = Convert.ToString(grpstrCol);
                }
            }
        }
        catch { }
        return val;
    }

    #endregion

    #region Common Checkbox and Checkboxlist Event
    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
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
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
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
    #endregion

    #region roll,reg,admission setting
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
    protected void spreadColumnVisible(int rollNo, int regNo, int admNo)
    {
        try
        {
            #region
            if (roll == 0)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 1)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 2)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = false;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = false;
            }
            else if (roll == 3)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = false;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = false;
            }
            else if (roll == 4)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = false;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = false;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 5)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = false;
            }
            else if (roll == 6)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = false;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 7)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = false;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            #endregion
        }
        catch { }
    }
    #endregion

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
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
    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Convocation Registration Report" + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "DailyFeesCollectionReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch { }
    }
    #endregion

    /*07.09.17 update Popup */

    public void spreadDet_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;
    }

    protected void spreadDet_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick)
        {
            int activerow = 0;
            int activecol = 0;
            int.TryParse(Convert.ToString(spreadDet.ActiveSheetView.ActiveRow), out activerow);
            int.TryParse(Convert.ToString(spreadDet.ActiveSheetView.ActiveColumn), out activecol);
            if (activerow >= 0 && activecol >= 0)
            {
                clearDetails();
                loadcollegeAdd();
                loadfromsetting();
                rbl_rollno_OnSelectedIndexChanged(sender, e);
                bindgrid();
                bindstate();
                cbfeedback.Checked = false;
                string appNo = Convert.ToString(spreadDet.Sheets[0].Cells[activerow, 0].Tag);
                string rollNo = Convert.ToString(spreadDet.Sheets[0].Cells[activerow, 1].Tag);
                rbl_rollno.SelectedIndex = rbl_rollno.Items.IndexOf(rbl_rollno.Items.FindByValue("0"));
                txt_roll.Text = rollNo;
                txt_roll.Enabled = false;
                getStudentDet(appNo, 1);
                div_addNew.Visible = true;
            }
        }
    }

    #region colorder

    protected void lnkcolorder_Click(object sender, EventArgs e)
    {
        txtcolorder.Text = string.Empty;
        loadcolumnorder();
        columnType();
        // loadcolumns();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        //divcolorder.Visible = true;
    }

    public void loadcolumnorder()
    {
        cblcolumnorder.Items.Clear();
        cblcolumnorder.Items.Add(new ListItem("Student Name(Tamil)", "1"));
        cblcolumnorder.Items.Add(new ListItem("Student Name(English)", "2"));
        cblcolumnorder.Items.Add(new ListItem("Department", "3"));
        cblcolumnorder.Items.Add(new ListItem("Amount", "4"));
        cblcolumnorder.Items.Add(new ListItem("Paiddate", "5"));
        cblcolumnorder.Items.Add(new ListItem("Remark", "6"));
        cblcolumnorder.Items.Add(new ListItem("Email Id", "7"));
        cblcolumnorder.Items.Add(new ListItem("Mobile No", "8"));
        cblcolumnorder.Items.Add(new ListItem("Father Name", "9"));
        cblcolumnorder.Items.Add(new ListItem("Attending Convocation", "10"));
        cblcolumnorder.Items.Add(new ListItem("Address", "11"));
        cblcolumnorder.Items.Add(new ListItem("OfficeName", "12"));
        cblcolumnorder.Items.Add(new ListItem("OfficeDesignation", "13"));
        cblcolumnorder.Items.Add(new ListItem("OfficeAddress", "14"));
        cblcolumnorder.Items.Add(new ListItem("Gender", "15"));//Added by saranya on 12/9/2018
        //Added by saranya on 7/3/2019
        cblcolumnorder.Items.Add(new ListItem("Salary", "16"));
        cblcolumnorder.Items.Add(new ListItem("CampusSelection", "17"));
        cblcolumnorder.Items.Add(new ListItem("HallStudent", "18"));
        cblcolumnorder.Items.Add(new ListItem("HallName", "19"));
        cblcolumnorder.Items.Add(new ListItem("CollegeName", "20"));
        cblcolumnorder.Items.Add(new ListItem("Degree", "21"));
        cblcolumnorder.Items.Add(new ListItem("CollegeDepartment", "22"));
        //cblcolumnorder.Items.Add(new ListItem("Cut of Mark", "13"));
        //cblcolumnorder.Items.Add(new ListItem("Father Name", "14"));
        //cblcolumnorder.Items.Add(new ListItem("Father Mobile No", "15"));
        //cblcolumnorder.Items.Add(new ListItem("Semester", "16"));
        //cblcolumnorder.Items.Add(new ListItem("Batch Year", "17"));
    }

    protected Hashtable htcolumnValue()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            htcol.Add("Student Name(Tamil)", "a.stud_nametamil[Student Name(Tamil)]");
            htcol.Add("Student Name(English)", "r.stud_name[Student Name(English)]");
            htcol.Add("Department", "(select (c.course_name+'-'+dt.dept_name) from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code)[Department]");
            htcol.Add("Amount", "convocation_Amount[Amount]");
            htcol.Add("Paiddate", "convert(varchar(10),convocation_PaidDate,103)[Paid Date]");
            htcol.Add("Remark", "convocation_Remark[Remark]");
            htcol.Add("Email Id", "StuPer_Id [Student Mail]");
            htcol.Add("Mobile No", "Student_Mobile[Mobile No]");
            htcol.Add("Father Name", "parent_name[Father Name]");
            htcol.Add("Attending Convocation", "case when isalumni='1' then 'YES' when isalumni='0' then 'NO' end [Attending Convocation]");
            htcol.Add("Address", "a.parent_addressP + '-'+ a.Streetp + '-' + a.cityp[Address]");
            htcol.Add("OfficeName", "a.student_officename[Office Name]");
            htcol.Add("OfficeDesignation", "a.student_designation[Designation]");
            htcol.Add("OfficeAddress", "student_officeaddress2 + '-' + student_officecity[Office Address]");
            htcol.Add("Gender", "case when a.sex='0' then 'Male' when a.sex='1' then 'Female' end [Gender]");//Added by saranya on 12/9/2018
            //Added by saranya on 7/3/2019
            htcol.Add("Salary", "g.salary[Salary]");
            htcol.Add("CampusSelection", "case when g.campus_selected='0' then 'No' when g.campus_selected='1' then 'Yes' end [CampusSelection]");
            htcol.Add("HallStudent", "case when g.hallstudent='0' then 'No' when g.hallstudent='1' then 'Yes' end [HallStudent]");
            htcol.Add("HallName", "g.hallname[HallName]");
            htcol.Add("CollegeName", "g.collegename[CollegeName]");
            htcol.Add("Degree", "g.degree[Degree]");
            htcol.Add("CollegeDepartment", "g.department[CollegeDepartment]");
            //select r.roll_no[Roll No],r.reg_no[Reg No],r.roll_admit[Admission No],r.stud_name[Student Name(English)],,,,,,,,,,,,,
            //htcol.Add("Father Name", "a.parent_name");
            //htcol.Add("Father Mobile No", "a.parentf_mobile");
            //htcol.Add("Semester", "isnull(a.current_semester,0) as current_semester");//delsi2702
            //htcol.Add("Batch Year", "a.batch_year");
            //htcol.Add("Cut of Mark", "st.Cut_Of_Mark");
        }
        catch { }
        return htcol;
    }

    protected Hashtable htcolumnHeaderValue()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            //htcol.Add("sno", "SNo");
            //htcol.Add("sel", "Select");
            //htcol.Add("stview", "View");
            //htcol.Add("stud_name", "Student Name");
            //htcol.Add("dob", "DOB");
            //htcol.Add("app_formno", "Application ID");
            //htcol.Add("date_applied", "Application Date");
            //htcol.Add("degree_code", "Department");
            //htcol.Add("alterdegree_code", "Alternative Course");
            //htcol.Add("Religion", "Religion");
            //htcol.Add("Community", "Community");
            //htcol.Add("student_Mobile", "Mobile No");
            //htcol.Add("stuPer_id", "Email Id");
            //htcol.Add("collegecode", "Institute Name");
            //htcol.Add("percentage", "Percentage");
            //htcol.Add("Cut_Of_Mark", "Cut of Mark");
            //htcol.Add("roll_admit", "Admission No");
            //htcol.Add("parent_name", "Father Name");
            //htcol.Add("parentf_mobile", "Father Mobile No");
            //htcol.Add("current_semester", "Semester");
            //htcol.Add("batch_year", "Batch Year");
            htcol.Add("a.stud_nametamil", "Student Name(Tamil)");
            htcol.Add("a.stud_name", "Student Name(English)");
            htcol.Add("(select c.course_name+'-'+dt.dept_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( a.degree_code,0)) as degree_code", "Department");
            htcol.Add("a.convocation_Amount", "Amount");
            htcol.Add("convert(varchar(10),a.convocation_PaidDate,103) as convocation_PaidDate", "paiddate");
            htcol.Add("a.convocation_Remark", "Remark");
            htcol.Add("a.stuPer_id", "Email Id");
            htcol.Add("a.student_Mobile", "Mobile No");
            htcol.Add("a.parent_name", "Father Name");
            htcol.Add("case when isalumni='1' then 'YES' when isalumni='0' then 'NO' end [Attending Convocation]", "Attending Convocation");
            htcol.Add("a.parent_addressp", "Address");
            htcol.Add("student_officename", "OfficeName");
            htcol.Add("student_designation", "OfficeDesignation");
            htcol.Add("student_officeaddress", "OfficeAddress");
            htcol.Add("case when a.sex='0' then 'Male' when a.sex='1' then 'Female' end [Gender]", "Gender");//Added by saranya on 12/9/2018
            //Added by saranya on 7/3/2019
            htcol.Add("case when g.salary","Salary");
            htcol.Add("case when g.campus_selected='0' then 'No' when g.campus_selected='1' then 'Yes' end [CampusSelection]","CampusSelection");
            htcol.Add("case when g.hallstudent='0' then 'No' when g.hallstudent='1' then 'Yes' end [HallStudent]","HallStudent");
            htcol.Add("g.hallname","HallName");
            htcol.Add("g.collegename","CollegeName");
            htcol.Add("g.degree","Degree");
            htcol.Add("g.department", "CollegeDepartment");
        }
        catch { }
        return htcol;
    }

    protected void btncolorderOK_Click(object sender, EventArgs e)
    {
        // loadcolumns();
        divcolorder.Visible = true;
        if (getsaveColumnOrder())
        {
            divcolorder.Attributes.Add("Style", "display:none;");
        }
    }

    protected bool getsaveColumnOrder()
    {
        bool boolSave = false;
        try
        {
            string strText = string.Empty;
            if (cblcolumnorder.Items.Count > 0)
                strText = Convert.ToString(getCblSelectedTextwithout(cblcolumnorder));
            if (!string.IsNullOrEmpty(strText))
                strText = Convert.ToString(txtcolorder.Text);
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0" && !string.IsNullOrEmpty(strText))
            {
                string SelQ = " if exists (select * from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "')update New_InsSettings set linkvalue='" + strText + "' where  LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "' else insert into New_InsSettings(LinkName,linkvalue,user_code,college_code) values('" + linkName + "','" + strText + "','" + usercode + "','" + Usercollegecode + "')";
                int insQ = d2.update_method_wo_parameter(SelQ, "Text");
                boolSave = true;
            }
            if (!boolSave)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please select corresponding values!')", true);
            }
        }
        catch { }
        return boolSave;
    }

    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                {
                    colorder = true;
                }
            }
        }
        catch { }
        return colorder;
    }

    public void loadcolumns()
    {
        try
        {
            string linkname = "DFCR column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    colord.Clear();
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colord.Add(Convert.ToString(valuesplit[k]));
                                if (columnvalue == "")
                                    columnvalue = Convert.ToString(valuesplit[k]);
                                else
                                    columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }
            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                    if (columnvalue == "")
                        columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                    else
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,usercode,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                        string[] value1 = value.Split(',');
                        if (value1.Length > 0)
                        {
                            for (int i = 0; i < value1.Length; i++)
                            {
                                string val = value1[i].ToString();
                                for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                                {
                                    if (val == cblcolumnorder.Items[k].Value)
                                    {
                                        cblcolumnorder.Items[k].Selected = true;
                                        count++;
                                    }
                                    if (count == cblcolumnorder.Items.Count)
                                        cb_column.Checked = true;
                                    else
                                        cb_column.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    #region report type added dropdown

    //protected void btnAdd_OnClick(object sender, EventArgs e)
    //{
    //}

    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        selectReportType();
    }

    protected void btnDel_OnClick(object sender, EventArgs e)
    {
        deleteReportType();
    }

    //type save
    protected void btnaddtype_Click(object sender, EventArgs e)
    {
        try
        {
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string strDesc = Convert.ToString(txtdesc.Text);
            if (!string.IsNullOrEmpty(strDesc) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + strDesc + "' and MasterCriteria ='ConvocationDetails' and CollegeCode ='" + Usercollegecode + "') update CO_MasterValues set MasterValue ='" + strDesc + "' where MasterValue ='" + strDesc + "' and MasterCriteria ='ConvocationDetails' and CollegeCode ='" + Usercollegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + strDesc + "','ConvocationDetails','" + Usercollegecode + "')";
                int insert = d2.update_method_wo_parameter(sql, "Text");
                if (insert > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true); txtdesc.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter report type')", true);
            }
            columnType();
            divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        }
        catch { }
    }

    public void columnType()
    {
        string Usercollegecode = string.Empty;
        if (Session["collegecode"] != null)
            Usercollegecode = Convert.ToString(Session["collegecode"]);
        ddlreport.Items.Clear();
        ddlMainreport.Items.Clear();
        if (!string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='ConvocationDetails' and CollegeCode='" + Usercollegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlreport.DataSource = ds;
                ddlreport.DataTextField = "MasterValue";
                ddlreport.DataValueField = "MasterCode";
                ddlreport.DataBind();
                ddlreport.Items.Insert(0, new ListItem("Select", "0"));
                //main search filter
                ddlMainreport.DataSource = ds;
                ddlMainreport.DataTextField = "MasterValue";
                ddlMainreport.DataValueField = "MasterCode";
                ddlMainreport.DataBind();
                // ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlreport.Items.Insert(0, new ListItem("Select", "0"));
                ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }

    protected void selectReportType()
    {
        try
        {
            bool boolcheck = false;
            string getName = string.Empty;
            txtcolorder.Text = string.Empty;
            string strText = string.Empty;
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                getName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' ");
                if (!string.IsNullOrEmpty(getName) && getName != "0")
                {
                    string[] splName = getName.Split(',');
                    if (splName.Length > 0)
                    {
                        for (int sprow = 0; sprow < splName.Length; sprow++)
                        {
                            for (int flt = 0; flt < cblcolumnorder.Items.Count; flt++)
                            {
                                if (splName[sprow].Trim() == cblcolumnorder.Items[flt].Text.Trim())
                                {
                                    cblcolumnorder.Items[flt].Selected = true;
                                    boolcheck = true;
                                    // strText += cblcolumnorder.Items[flt].Text;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                txtcolorder.Text = string.Empty;
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                }
                cb_column.Checked = false;
            }
            if (boolcheck)
            {
                txtcolorder.Text = getName;
            }
        }
        catch { }
    }

    protected void deleteReportType()
    {
        int delMQ = 0;
        string Usercollegecode = string.Empty;
        if (Session["collegecode"] != null)
            Usercollegecode = Convert.ToString(Session["collegecode"]);
        string linkName = string.Empty;
        if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
            linkName = Convert.ToString(ddlreport.SelectedItem.Text);
        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            int delQ = 0;
            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "'", "Text")), out delQ);
            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete  from CO_MasterValues where MasterCriteria='ConvocationDetails' and mastervalue='" + linkName + "'  and collegecode='" + Usercollegecode + "'", "Text")), out delMQ);
        }
        if (delMQ > 0)
        {
            txtcolorder.Text = string.Empty;
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                cblcolumnorder.Items[i].Selected = false;
            }
            cb_column.Checked = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Failed')", true);
        columnType();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    }

    #endregion

    #endregion

    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }

    #region Added by Saranya on 12/9/2018

    public void bindtype()
    {
        try
        {
            string typequery = "select distinct type  from course where college_code =" + ddlcollegename.SelectedItem.Value + "";
            ds = d2.select_method_wo_parameter(typequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
            }
        }
        catch
        {

        }
    }

    protected void type_Change(object sender, EventArgs e)
    {
        try
        {
            binddeg();
            binddept();
        }
        catch
        {
        }
    }

    protected void loadNotAttendingDetails(DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadNotAttend.Sheets[0].RowCount = 0;
            spreadNotAttend.Sheets[0].ColumnCount = 0;
            spreadNotAttend.CommandBar.Visible = false;
            spreadNotAttend.Sheets[0].AutoPostBack = true;
            spreadNotAttend.Sheets[0].ColumnHeader.RowCount = 1;
            spreadNotAttend.Sheets[0].RowHeader.Visible = false;
            spreadNotAttend.Sheets[0].ColumnCount = 1;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadNotAttend.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadNotAttend.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            Hashtable htcol = new Hashtable();
            DataTable dtPaid = ds.Tables[0].DefaultView.ToTable();
            Hashtable htRealName = htcolumnHeaderValue();
            for (int row = 0; row < dtPaid.Columns.Count; row++)
            {
                string colName = Convert.ToString(dtPaid.Columns[row].ColumnName);
                spreadNotAttend.Sheets[0].ColumnCount++;
                int col = spreadNotAttend.Sheets[0].ColumnCount - 1;
                htcol.Add(colName, col);
                spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, col].Text = colName;
                spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadNotAttend.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                spreadNotAttend.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Center;
                switch (colName.Trim())
                {
                    case "Admission No":
                        spreadNotAttend.Sheets[0].Columns[col].Width = 150;
                        admNo = Convert.ToInt32(col);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadNotAttend.Sheets[0].Columns[col].Width = 110;
                        rollNo = Convert.ToInt32(col);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadNotAttend.Sheets[0].Columns[col].Width = 110;
                        regNo = Convert.ToInt32(col);
                        boolroll = true;
                        break;
                }
            }
            spreadNotAttend.Sheets[0].Columns[spreadNotAttend.Sheets[0].ColumnCount - 1].Visible = false;
            if (boolroll)
                spreadColumnVisible(rollNo, regNo, admNo);
            #endregion

            #region value
            int rowCnt = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            for (int row = 0; row < dtPaid.Rows.Count; row++)
            {
                spreadNotAttend.Sheets[0].RowCount++;
                int rowSpCnt = spreadNotAttend.Sheets[0].RowCount - 1;
                spreadNotAttend.Sheets[0].Cells[rowSpCnt, 0].Tag = Convert.ToString(dtPaid.Rows[row]["app_no"]);
                spreadNotAttend.Sheets[0].Cells[rowSpCnt, 1].Tag = Convert.ToString(dtPaid.Rows[row]["Roll No"]);
                spreadNotAttend.Sheets[0].Cells[rowSpCnt, 0].Text = Convert.ToString(++rowCnt);
                for (int col = 0; col < dtPaid.Columns.Count; col++)
                {
                    string colName = Convert.ToString(dtPaid.Columns[col].ColumnName);
                    int ColCnt = 0;
                    int.TryParse(Convert.ToString(htcol[colName]), out ColCnt);
                    spreadNotAttend.Sheets[0].Cells[rowSpCnt, ColCnt].Text = Convert.ToString(dtPaid.Rows[row][col]);
                    switch (colName.Trim())
                    {
                        case "Admission No":
                        case "Roll No":
                        case "Reg No":
                        case "Mobile No":
                            spreadNotAttend.Sheets[0].Cells[rowSpCnt, ColCnt].CellType = txtroll;
                            break;
                    }
                }
            }
            spreadNotAttend.SaveChanges();
            spreadNotAttend.Sheets[0].PageSize = spreadNotAttend.Sheets[0].RowCount;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadNotAttend.Visible = true;
            print.Visible = true;
            getPrintSettings();
            spreadNotAttend.SaveChanges();
            #endregion
        }
        catch { }
    }

    #endregion
}