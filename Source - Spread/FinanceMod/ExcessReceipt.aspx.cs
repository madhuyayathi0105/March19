//Code Started by Idhris : 21-07-2016
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;
using System.Web.UI;
using System.Text;
using Gios.Pdf;
using System.IO;
using System.Web;
using System.Collections;
using System.Configuration;

public partial class ExcessReceipt : System.Web.UI.Page
{
    int collegeCode = 0;
    int userCode = 0;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string collegecode = string.Empty;
    static int usercodestat = 0;
    static int choosedmode = 0;
    static int collegecodestat = 13;
    string usercode = string.Empty;
    DAccess2 DA = new DAccess2();
    DataSet ds = new DataSet();
    private delegate void delegateLoadInitials();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            setLabelText();
            collegeCode = Convert.ToInt32(Convert.ToString(Session["collegecode"]));
            userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
            usercodestat = userCode;
            usercode = Convert.ToString(Session["usercode"]);
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {

                delegateLoadInitials initialLoad = getInitials();
                initialLoad();
            }
            lbl_validation.Visible = false;
            updateClgCode();
            if (ddl_college.Items.Count > 0)
                collegecode = Convert.ToString(ddl_college.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }
    private delegateLoadInitials getInitials()
    {
        delegateLoadInitials initialLoad = new delegateLoadInitials(bindCollege);
        if (ddl_college.Items.Count > 0)
            collegecode = Convert.ToString(ddl_college.SelectedItem.Value);
        initialLoad += updateClgCode;
        initialLoad += bindType;
        initialLoad += bindbatch;
        initialLoad += binddegree;
        initialLoad += bindbranch;
        initialLoad += bindsem;
        initialLoad += bindsec;
        initialLoad += LoadFromSettings;
        return initialLoad;
    }
    public void bindCollege()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Clear();
            ddl_college.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            ds = DA.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
                if (ddl_college.Items.Count > 0)
                    collegecode = Convert.ToString(ddl_college.SelectedItem.Value);
            }
        }
        catch (Exception ex) { ddl_college.Items.Clear(); }
    }
    private void updateClgCode()
    {
        try
        {
            if (ddl_college.Items.Count > 0)
            {
                collegeCode = Convert.ToInt32(ddl_college.SelectedItem.Value);
            }
            else
            {
                collegeCode = 13;
            }
            collegecodestat = collegeCode;
            userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
        }
        catch { }
    }
    public void bindType()
    {
        try
        {
            lbl_stream.Text = useStreamShift();
            ddl_strm.Items.Clear();
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code='" + collegeCode + "'  order by type asc";

            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
                ddl_strm.Enabled = true;
            }
            else
            {
                ddl_strm.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }
    public void bindbatch()
    {
        try
        {
            //ddl_batch.Items.Clear();
            cbl_batch.Items.Clear();
            txt_batch.Text = "Batch";
            cb_batch.Checked = true;
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            DataSet ds = DA.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //ddl_batch.DataSource = ds;
                //ddl_batch.DataTextField = "batch_year";
                //ddl_batch.DataValueField = "batch_year";
                //ddl_batch.DataBind();

                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                CallCheckBoxChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
            }
        }
        catch (Exception ex) { }
    }
    public void binddegree()
    {
        try
        {
            //ddl_degree.Items.Clear();
            cbl_degree.Items.Clear();
            txt_degree.Text = "Degree";
            cb_degree.Checked = true;
            string stream = "";
            stream = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : "";

            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegeCode + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + userCode + " ";
            if (ddl_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //ddl_degree.DataSource = ds;
                //ddl_degree.DataTextField = "course_name";
                //ddl_degree.DataValueField = "course_id";
                //ddl_degree.DataBind();

                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
            }
        }
        catch (Exception ex) { }
    }
    public void bindbranch()
    {
        try
        {
            //ddl_branch.Items.Clear();
            cbl_branch.Items.Clear();
            txt_branch.Text = "Branch";
            cb_branch.Checked = true;
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
            // degree = GetSelectedItemsValueAsString(cbl_degree);//ddl_degree.Items.Count > 0 ? ddl_degree.SelectedValue : "";


            //string commname = "";
            //if (degree != "")
            //{
            //    commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + degree + "') and deptprivilages.Degree_code=degree.Degree_code ";
            //}
            //else
            //{
            //    commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            //}

            if (degree != "")
            {
                ds.Clear();
                ds = DA.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                // DataSet ds = DA.select_method_wo_parameter(commname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ddl_branch.DataSource = ds;
                    //ddl_branch.DataTextField = "dept_name";
                    //ddl_branch.DataValueField = "degree_code";
                    //ddl_branch.DataBind();
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void bindsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = DA.loadFeecategory(Convert.ToString(ddl_college.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "TextVal";
                cbl_sem.DataValueField = "TextCode";
                cbl_sem.DataBind();

                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_sem.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                        txt_sem.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txt_sem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }
    //public void bindsem()
    //{
    //    try
    //    {
    //        //ddl_sem.Items.Clear();
    //        cbl_sem.Items.Clear();
    //        cb_sem.Checked = true;
    //        txt_sem.Text = "Semester";
    //        string linkvalue = DA.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + userCode + "' and college_code ='" + collegeCode + "'");

    //        if (linkvalue != "")
    //        {
    //            DataSet dsSemYear = new DataSet();
    //            string query = "";
    //            string semyear = "select Linkvalue from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + userCode + "' and college_code ='" + collegeCode + "'";

    //            if (DA.GetFunction(semyear).Trim() == "1")
    //            {
    //                query = "selECT	* from textvaltable where TextCriteria ='FEECA' and (textval like '%Semester' or textval like '%Year')  and college_code=" + collegeCode + " order by len(textval),textval asc";
    //            }
    //            else
    //            {
    //                if (linkvalue == "0")
    //                {
    //                    query = "selECT	* from textvaltable where TextCriteria ='FEECA' and textval like '% semester' and college_code=" + collegeCode + " order by len(textval),textval asc";
    //                }
    //                else
    //                {
    //                    query = " selECT	* from textvaltable where TextCriteria ='FEECA' and textval like '% Year' and college_code=" + collegeCode + " order by len(textval),textval asc";
    //                }
    //            }
    //            dsSemYear = DA.select_method_wo_parameter(query, "Text");
    //            if (dsSemYear.Tables.Count > 0)
    //            {
    //                if (dsSemYear.Tables[0].Rows.Count > 0)
    //                {
    //                    cbl_sem.DataSource = dsSemYear;
    //                    cbl_sem.DataTextField = "TextVal";
    //                    cbl_sem.DataValueField = "TextCode";
    //                    cbl_sem.DataBind();

    //                    CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, "Sem/Year");
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}
    public void bindsec()
    {
        try
        {
            //ddl_sec.Items.Clear();
            cbl_sec.Items.Clear();
            cb_sec.Checked = true;
            txt_sec.Text = "Section";

            ListItem item = new ListItem("Empty", " ");

            string batch = "";
            batch = GetSelectedItemsValue(cbl_batch); //ddl_batch.Items.Count > 0 ? ddl_batch.SelectedValue : "0";
            string branch = "";
            branch = GetSelectedItemsValue(cbl_branch);//ddl_branch.Items.Count > 0 ? ddl_branch.SelectedValue : "0";
            DataSet dsSec = DA.BindSectionDetail(batch, branch);
            if (dsSec.Tables.Count > 0 && dsSec.Tables[0].Rows.Count > 0)
            {
                //ddl_sec.DataSource = dsSec;
                //ddl_sec.DataTextField = "sections";
                //ddl_sec.DataValueField = "sections";
                //ddl_sec.DataBind();

                cbl_sec.DataSource = dsSec;
                cbl_sec.DataTextField = "sections";
                cbl_sec.DataValueField = "sections";
                cbl_sec.DataBind();
                CallCheckBoxChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
            }
        }
        catch (Exception ex) { }
    }
    public void LoadFromSettings()
    {
        try
        {
            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + userCode + "' --and college_code in(" + collegeCode + ")";

            int save1 = Convert.ToInt32(DA.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + userCode + "' --and college_code in(" + collegeCode + ")";
            save1 = Convert.ToInt32(DA.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + userCode + "' --and college_code in(" + collegeCode + ")";
            save1 = Convert.ToInt32(DA.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + userCode + "' --and college_code in(" + collegeCode + ") ";
            save1 = Convert.ToInt32(DA.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                rbl_rollno.Items.Add(lst4);

            }

            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_SearchBy.Attributes.Add("placeholder", "Roll No");
                    choosedmode = 0;
                    break;
                case 1:
                    txt_SearchBy.Attributes.Add("placeholder", "Reg No");
                    choosedmode = 1;
                    break;
                case 2:
                    txt_SearchBy.Attributes.Add("placeholder", "Admin No");
                    choosedmode = 2;
                    break;
                case 3:
                    txt_SearchBy.Attributes.Add("placeholder", "App No");
                    choosedmode = 3;
                    break;
            }
        }
        catch (Exception ex) { }
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    protected void lb_LogOut_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { }
    }
    protected void ddl_college_OnIndexChange(object sender, EventArgs e)
    {
        bindType();
        bindbatch();
        binddegree();
        bindbranch();
        //bindsem();
        bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void ddl_batch_OnIndexChange(object sender, EventArgs e)
    {
        bindType();
        binddegree();
        bindbranch();
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void cb_batch_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
        bindType();
        binddegree();
        bindbranch();
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
        bindType();
        binddegree();
        bindbranch();
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void ddl_degree_OnIndexChange(object sender, EventArgs e)
    {
        bindbranch();
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void ddl_branch_OnIndexChange(object sender, EventArgs e)
    {
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void cb_branch_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);
        //bindsem();
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void ddl_sem_OnIndexChange(object sender, EventArgs e)
    {
        bindsec();
        btn_go_Click(sender, e);
    }
    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, "Sem/Year");
        btn_go_Click(sender, e);
    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_sem, cb_sem, txt_sem, "Sem/Year");
        btn_go_Click(sender, e);
    }
    protected void ddl_sec_OnIndexChange(object sender, EventArgs e)
    {
        btn_go_Click(sender, e);
    }
    protected void cb_sec_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
        btn_go_Click(sender, e);
    }
    protected void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
        btn_go_Click(sender, e);
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_SearchBy.Text = string.Empty;
        switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
        {
            case 0:
                txt_SearchBy.Attributes.Add("placeholder", "Roll No");
                choosedmode = 0;
                break;
            case 1:
                txt_SearchBy.Attributes.Add("placeholder", "Reg No");
                choosedmode = 1;
                break;
            case 2:
                txt_SearchBy.Attributes.Add("placeholder", "Admin No");
                choosedmode = 2;
                break;
            case 3:
                txt_SearchBy.Attributes.Add("placeholder", "App No");
                choosedmode = 3;
                break;
        }
        btn_go_Click(sender, e);
    }
    private string useStreamShift()
    {
        string useStrShft = "Stream";
        string streamcode = DA.GetFunction("select value from Master_Settings where settings='Stream/Shift Rights' and usercode='" + userCode + "'").Trim();

        if (streamcode == "" || streamcode == "0")
        {
            useStrShft = "Stream";
        }
        if (streamcode.Trim() == "1")
        {
            useStrShft = "Shift";
        }
        if (streamcode.Trim() == "2")
        {
            useStrShft = "Stream";
        }
        return useStrShft;
    }
    private ListItem getFeecategoryNEW(string Sem, string college_code, string user_code)
    {

        ListItem feeCategory = new ListItem();
        string linkvalue = DA.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + user_code + "' and college_code ='" + college_code + "'");
        DataSet dsFeecat = new DataSet();
        if (linkvalue == "0")
        {
            dsFeecat = DA.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + Sem + " Semester' and college_code=" + college_code + "", "Text");
        }
        else
        {
            string year = retYearForSem(Sem);
            dsFeecat = DA.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + year + " Year' and college_code=" + college_code + "", "Text");
        }
        if (dsFeecat.Tables.Count > 0 && dsFeecat.Tables[0].Rows.Count > 0)
        {
            feeCategory.Text = Convert.ToString(dsFeecat.Tables[0].Rows[0]["textval"]);
            feeCategory.Value = Convert.ToString(dsFeecat.Tables[0].Rows[0]["TextCode"]);
        }
        else
        {
            feeCategory.Text = " ";
            feeCategory.Value = "-1";
        }
        return feeCategory;
    }
    public string retYearForSem(string val)
    {
        string value = "1";
        if (val.Trim() == "1" || val.Trim() == "2")
        {
            value = "1";
        }
        if (val.Trim() == "3" || val.Trim() == "4")
        {
            value = "2";
        }
        if (val.Trim() == "5" || val.Trim() == "6")
        {
            value = "3";
        }
        if (val.Trim() == "7" || val.Trim() == "8")
        {
            value = "4";
        }
        if (val.Trim() == "9" || val.Trim() == "10")
        {
            value = "5";
        }
        return value;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetSearch(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //student query
            if (choosedmode == 0)
            {
                query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + " order by Roll_No asc";
            }
            else if (choosedmode == 1)
            {
                query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Reg_No asc";
            }
            else if (choosedmode == 2)
            {
                query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Roll_admit asc";
            }
            else if (choosedmode == 4)
            {
                query = "select  top 100 smart_serial_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and smart_serial_no like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by smart_serial_no asc";
            }
            else
            {
                byte studAppSHrtAdm = statStudentAppliedShorlistAdmit();
                string admStudFilter = "";
                switch (studAppSHrtAdm)
                {
                    case 0:
                        admStudFilter = " and isconfirm=1 ";
                        break;
                    case 1:
                        admStudFilter = " and isconfirm=1 and selection_status=1 ";
                        break;
                    case 2:
                        admStudFilter = " and isconfirm=1 and selection_status=1 and admission_status=1 ";
                        break;
                }
                query = "  select  top 100 app_formno from applyn where  app_formno like '" + prefixText + "%' and college_code=" + collegecodestat + " " + admStudFilter + "  order by app_formno asc";
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    private static byte statStudentAppliedShorlistAdmit()
    {
        DAccess2 DA = new DAccess2();
        string Q = "select LinkValue from New_InsSettings where LinkName='StudentAppliedShorlistAdmit' and user_code ='" + usercodestat + "' --and college_code ='" + collegecodestat + "'";
        byte moveVal = 0;
        byte.TryParse(DA.GetFunction(Q.Trim()), out moveVal);
        return moveVal;
    }
    private StringBuilder selectedSemester()
    {
        StringBuilder sbcursem = new StringBuilder();
        try
        {
            for (int f = 0; f < cbl_sem.Items.Count; f++)
            {
                if (cbl_sem.Items[f].Selected)
                {
                    string feecode = cbl_sem.Items[f].Text;
                    string[] fpsplit = feecode.Split(' ');
                    if (fpsplit.Length > 0)
                    {
                        string val = string.Empty;
                        if (fpsplit[1].Trim().ToUpper() == "YEAR")
                        {
                            switch (fpsplit[0].Trim())
                            {
                                case "1":
                                    val = "1','2";
                                    break;
                                case "2":
                                    val = "3','4";
                                    break;
                                case "3":
                                    val = "5','6";
                                    break;
                                case "4":
                                    val = "7','8";
                                    break;
                                case "5":
                                    val = "9','10";
                                    break;
                            }
                        }
                        else
                        {
                            val = fpsplit[0].Trim();
                        }
                        if (sbcursem.Length == 0)
                        {
                            sbcursem.Append(val);
                        }
                        else
                        {
                            sbcursem.Append("','" + val);
                        }
                    }
                }
            }
        }
        catch { }
        return sbcursem;
    }
    private string getExcessAmount(string app_no, string feecat)
    {
        //string Q = "select sum(isnull(el.ExcessAmt,0)-isnull(el.AdjAmt,0)) from ft_excessdet e,ft_excessledgerdet el where e.excessdetpk=el.excessdetfk and (isnull(el.ExcessAmt,0)-isnull(el.AdjAmt,0))>0  and e.app_no='" + app_no + "' ";
        string Q = "select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) from ft_excessdet where (isnull(ExcessAmt,0)-isnull(AdjAmt,0))>0  and app_no='" + app_no + "' and feecategory in (" + feecat + ") ";
        return DA.GetFunction(Q).Trim();
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_errormsg.Visible = false;
            lbl_Total.Visible = false;
            Printcontrol.Visible = false;
            rptprint.Visible = false;
            btnPrint.Visible = false;

            string selectquery;

            string branch = GetSelectedItemsValueAsString(cbl_branch);

            string degCode = GetSelectedItemsValueAsString(cbl_degree);

            string stream = ddl_strm.Enabled ? ddl_strm.Items.Count > 0 ? ddl_strm.SelectedItem.Text.Trim() : "" : "";

            string section = GetSelectedItemsText(cbl_sec);

            string batch_year = GetSelectedItemsValue(cbl_batch);

            string cusem = string.Empty;// selectedSemester().ToString();

            string feecat = GetSelectedItemsValue(cbl_sem);

            DataSet ds = new DataSet();

            string searchBytxt = txt_SearchBy.Text.Trim();
            if (searchBytxt != string.Empty)
            {
                //selectquery = "select r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,a.app_formno    from Registration r,applyn a,Degree d,Department dt,Course c where r.app_no=a.app_no and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' ";
                selectquery = "  select  r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,a.app_formno,SUM(isnull(ExcessAmt,0)-isnull(AdjAmt,0))  as Amt from Registration r,FT_ExcessDet f,Degree d,Department dt,Course c,applyn a where a.app_no =r.App_No and a.app_no =f.App_No and r.App_No =f.App_No and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and d.Degree_Code =r.degree_code and f.feecategory in (" + feecat + ") ";

                if (Convert.ToInt16(rbl_rollno.SelectedValue) == 0)
                {
                    selectquery += " and r.Roll_No='" + searchBytxt + "'  group by r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,r.Reg_No,r.App_No,c.type,C.Course_Name,dt.Dept_Name,r.Sections,a.app_formno,a.sex";
                }
                else if (Convert.ToInt16(rbl_rollno.SelectedValue) == 1)
                {
                    selectquery += " and r.Reg_No='" + searchBytxt + "'  group by r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,r.Reg_No,r.App_No,c.type,C.Course_Name,dt.Dept_Name,r.Sections,a.app_formno,a.sex";
                }
                else if (Convert.ToInt16(rbl_rollno.SelectedValue) == 2)
                {
                    selectquery += " and r.Roll_Admit='" + searchBytxt + "'  group by r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,r.Reg_No,r.App_No,c.type,C.Course_Name,dt.Dept_Name,r.Sections,a.app_formno,a.sex";
                }
                else if (Convert.ToInt16(rbl_rollno.SelectedValue) == 3)
                {
                    selectquery += " and a.app_formno='" + searchBytxt + "'  group by r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,r.Reg_No,r.App_No,c.type,C.Course_Name,dt.Dept_Name,r.Sections,a.app_formno,a.sex";
                }
                else
                {
                    selectquery = "";
                }
                ds = DA.select_method_wo_parameter(selectquery, "Text");
            }
            else
            {
                if (batch_year != string.Empty && degCode != string.Empty && branch != string.Empty && feecat != string.Empty)
                {
                    if (stream != string.Empty)
                    {
                        stream = " and c.type in ('" + stream + "')";
                    }
                    //if (cusem != string.Empty)
                    //{
                    //    cusem = " and r.current_semester in('" + cusem + "') ";
                    //}
                    //selectquery = "select r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,a.app_formno    from Registration r,applyn a,Degree d,Department dt,Course c,ft_excessdet f where r.app_no=a.app_no and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and f.app_no=r.app_no and (isnull(ExcessAmt,0)-isnull(AdjAMt,0))>0 and r.Batch_Year in(" + batch_year + ") and r.degree_code in ('" + branch + "')  and isnull(r.Sections,'') in ('" + section + "')   " + cusem + stream + "  order by Department,r.Sections,Gender asc";

                    selectquery = "  select  r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,a.app_formno,SUM(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as Amt from Registration r,FT_ExcessDet f,Degree d,Department dt,Course c,applyn a where a.app_no =r.App_No and a.app_no =f.App_No and r.App_No =f.App_No and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and d.Degree_Code =r.degree_code and f.feecategory in (" + feecat + ")  and r.Batch_Year in(" + batch_year + ") and r.degree_code in ('" + branch + "')  and isnull(r.Sections,'') in ('" + section + "')   " + cusem + stream + "  group by r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,r.Reg_No,r.App_No,c.type,C.Course_Name,dt.Dept_Name,r.Sections,a.app_formno,a.sex  order by Department,r.Sections,Gender asc";
                    ds = DA.select_method_wo_parameter(selectquery, "Text");
                }
            }


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                spreadStudList.Sheets[0].RowCount = 1;
                spreadStudList.Sheets[0].ColumnCount = 0;
                spreadStudList.Sheets[0].ColumnHeader.RowCount = 1;
                spreadStudList.CommandBar.Visible = false;
                spreadStudList.Sheets[0].ColumnCount = 11;

                spreadStudList.Sheets[0].RowHeader.Visible = false;
                spreadStudList.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                spreadStudList.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[0].Locked = true;
                spreadStudList.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[1].Width = 60;
                spreadStudList.Sheets[0].Columns[1].Locked = false;
                spreadStudList.Sheets[0].Cells[0, 1].CellType = chkall;
                spreadStudList.Sheets[0].Columns[1].Visible = true;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admission Number";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[2].Locked = true;
                spreadStudList.Columns[2].Width = 150;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[3].Locked = true;
                spreadStudList.Columns[3].Width = 150;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[4].Locked = true;
                spreadStudList.Columns[4].Width = 150;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Application No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[5].Locked = true;
                spreadStudList.Columns[5].Width = 150;

                spreadStudList.Sheets[0].Columns[2].Visible = false;
                spreadStudList.Sheets[0].Columns[3].Visible = false;
                spreadStudList.Sheets[0].Columns[4].Visible = false;
                spreadStudList.Sheets[0].Columns[5].Visible = false;
                if (Convert.ToInt16(rbl_rollno.SelectedValue) == 0)
                {
                    spreadStudList.Sheets[0].Columns[3].Visible = true;
                }
                else if (Convert.ToInt16(rbl_rollno.SelectedValue) == 1)
                {
                    spreadStudList.Sheets[0].Columns[4].Visible = true;
                }
                else if (Convert.ToInt16(rbl_rollno.SelectedValue) == 2)
                {
                    spreadStudList.Sheets[0].Columns[2].Visible = true;
                }
                else if (Convert.ToInt16(rbl_rollno.SelectedValue) == 3)
                {
                    spreadStudList.Sheets[0].Columns[5].Visible = true;
                }

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                spreadStudList.Sheets[0].Columns[6].Locked = true;
                spreadStudList.Columns[6].Width = 300;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Degree";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                spreadStudList.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                spreadStudList.Sheets[0].Columns[7].Locked = true;
                spreadStudList.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                spreadStudList.Columns[7].Width = 250;
                //spreadStudList.Sheets[0].Columns[7].Visible = false;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Section";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[8].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[8].Locked = true;
                spreadStudList.Columns[8].Width = 60;
                spreadStudList.Sheets[0].Columns[8].Visible = false;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Gender";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[9].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[9].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[9].Locked = true;
                spreadStudList.Columns[9].Width = 60;
                spreadStudList.Sheets[0].Columns[9].Visible = false;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Excess Amount";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[10].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[10].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
                spreadStudList.Sheets[0].Columns[10].Locked = true;
                spreadStudList.Columns[9].Width = 100;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtexAmt = new FarPoint.Web.Spread.TextCellType();

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string app_no = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
                    spreadStudList.Sheets[0].RowCount++;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 0].Tag = app_no;

                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 1].CellType = check;

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 2].CellType = txtRollAd;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 3].CellType = txtRollno;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 4].CellType = txtRegno;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 5].CellType = txtAppno;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["app_formno"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["Sections"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[row]["Gender"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 10].CellType = txtexAmt;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[row]["Amt"]);
                }
                spreadStudList.Visible = true;
                spreadStudList.Sheets[0].PageSize = spreadStudList.Sheets[0].RowCount;
                spreadStudList.Height = 320;
                spreadStudList.SaveChanges();
                rptprint.Visible = true;
                btnPrint.Visible = true;
                lbl_Total.Visible = true;
                lbl_Total.Text = "Total Number Of Students : " + (spreadStudList.Sheets[0].RowCount - 1);
            }
            else
            {
                spreadStudList.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            spreadStudList.Visible = false;
            lbl_errormsg.Visible = true;
            lbl_errormsg.Text = "No Records Found"; DA.sendErrorMail(ex, collegeCode.ToString(), "ExcessReceipt.aspx");
        }
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Excess Fees Report";
            string pagename = "Excess Receipt.aspx";
            Printcontrol.loadspreaddetails(spreadStudList, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                DA.printexcelreport(spreadStudList, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch (Exception ex) { }

    }
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch (Exception ex) { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private List<string> GetSelectedItemsValueList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Value);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetSelectedItemsTextList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Text);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetItemsValueList(CheckBoxList cblItems)
    {
        System.Collections.Generic.List<string> lsItems = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblItems.Items.Count; list++)
            {
                lsItems.Add(cblItems.Items[list].Value);
            }
        }
        catch { lsItems.Clear(); }
        return lsItems;
    }
    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        List<string> appNoList = new List<string>();
        if (checkedOK(ref appNoList))
        {
            string queryPrint = "select top 1 * from FM_RcptChlPrintSettings where collegecode ='" + collegeCode + "'";
            DataSet dsPri = new DataSet();
            dsPri = DA.select_method_wo_parameter(queryPrint, "Text");
            if (dsPri.Tables.Count > 0 && dsPri.Tables[0].Rows.Count > 0)
            {
                string feecat = GetSelectedItemsValue(cbl_sem);
                if (feecat != string.Empty)
                {
                    PrintReceipt(appNoList, dsPri.Tables[0], getFinYear(), feecat);
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Select Sem/Year";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Add Print Settings";
            }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select Students";
        }
    }
    protected void spreadStudList_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = spreadStudList.Sheets[0].ActiveRow.ToString();
            string actcol = spreadStudList.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (spreadStudList.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(spreadStudList.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < spreadStudList.Sheets[0].RowCount; i++)
                        {
                            spreadStudList.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < spreadStudList.Sheets[0].RowCount; i++)
                        {
                            spreadStudList.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }
    public bool checkedOK(ref List<string> appNoList)
    {
        bool Ok = false;
        spreadStudList.SaveChanges();
        for (int i = 1; i < spreadStudList.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(spreadStudList.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
                appNoList.Add(Convert.ToString(spreadStudList.Sheets[0].Cells[i, 0].Tag));
            }
        }
        return Ok;
    }
    private string getFinYear()
    {
        string finYearid = DA.getCurrentFinanceYear(usercode, collegeCode.ToString());
        return finYearid;
    }
    private void PrintReceipt(List<string> appNoList, DataTable dtPrintSet, string finYearid, string feecat)
    {
        int studCnt = appNoList.Count;
        int rcptCnt = 0;
        int notapCnt = 0;

        //Document Settings
        PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.A4);

        Font FontboldheadC = new Font("Arial", 15, FontStyle.Bold);
        Font Fontboldhead = new Font("Arial", 12, FontStyle.Bold);
        Font FontTableHead = new Font("Arial", 8, FontStyle.Bold);
        Font FontTable = new Font("Arial", 8, FontStyle.Bold);
        Font tamilFont = new Font("AMUDHAM.TTF", 10, FontStyle.Regular);

        bool createPDFOK = false;
        #region For Every selected Receipt

        foreach (string AppNo in appNoList)
        {

            double confirmAmtChk = 0;
            double.TryParse(getExcessAmount(AppNo, feecat), out confirmAmtChk); ;
            if (confirmAmtChk > 0)
            {
                //string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDNo,convert(varchar(10),DDDate,103) as DDDate,(select textval from textvaltable where textcode =DDBankCode) as Bank,DDBankBranch from ft_findailytransaction  where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                string chlnDet = "select l.ledgername,h.headername,el.Headerfk,el.Ledgerfk,isnull(el.ExcessAmt,0) as ExcAmt,isnull(el.AdjAmt,0) as AdjAmt,isnull(el.BalanceAmt,0) as BalAMt,el.Feecategory,el.FinYearFk,(select textval from textvaltable where textcode=el.feecategory) as semyear from ft_excessdet e,ft_excessledgerdet el,fm_ledgermaster l,fm_headermaster h where l.headerfk=h.headerpk and el.headerfk=h.headerpk and l.ledgerpk=el.ledgerfk and e.excessdetpk=el.excessdetfk and (isnull(el.ExcessAmt,0)-isnull(el.AdjAmt,0))>0  and e.app_no='" + AppNo + "' and e.feecategory in (" + feecat + ")";
                DataSet dsDet = DA.select_method_wo_parameter(chlnDet, "Text");
                if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                {
                    string rollno = string.Empty;
                    string studname = string.Empty;
                    string receiptno = string.Empty;
                    string name = string.Empty;
                    string app_formno = string.Empty;
                    string appnoNew = string.Empty;
                    string Regno = string.Empty;
                    string batchYrSem = string.Empty;

                    string rcptTime = DateTime.Now.ToLongTimeString();
                    string recptDt = DateTime.Now.Date.ToString("dd/MM/yyyy");

                    string mode = string.Empty;
                    string paymode = string.Empty;// Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                    string rcptType = string.Empty;//Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                    string modePaySng = string.Empty;
                    string dddates = string.Empty;//Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                    string ddnos = string.Empty;//Convert.ToString(dsDet.Tables[0].Rows[0]["DDNo"]);
                    string ddBanks = string.Empty;//Convert.ToString(dsDet.Tables[0].Rows[0]["Bank"]);
                    string ddBrans = string.Empty;//Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);
                    switch (paymode)
                    {
                        case "1":
                            mode = "Cash";
                            break;
                        case "2":
                            mode = "Cheque";
                            modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                            break;
                        case "3":
                            mode = "DD";
                            modePaySng = "\n\nDDNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                            break;
                        case "4":
                            mode = "Challan";
                            break;
                        case "5":
                            mode = "Online";
                            break;
                        default:
                            mode = string.Empty;
                            break;
                    }


                    string queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                    DataSet dsRollApp = new DataSet();
                    dsRollApp = DA.select_method_wo_parameter(queryRollApp, "Text");
                    if (dsRollApp.Tables.Count > 0 && dsRollApp.Tables[0].Rows.Count > 0)
                    {
                        rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                        app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                        appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                        Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                        studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                    }
                    name = rollno + "-" + studname;
                    //Print Region
                    try
                    {
                        #region Print Option For Receipt

                        PdfPage rcptpage = recptDoc.NewPage();
                        //Fields to print

                        #region Settings Input
                        //Header Div Values

                        byte collegeid = Convert.ToByte(dtPrintSet.Rows[0]["IsCollegeName"]);
                        byte address1 = Convert.ToByte(dtPrintSet.Rows[0]["IsCollegeAdd1"]);
                        byte address2 = Convert.ToByte(dtPrintSet.Rows[0]["IsCollegeAdd2"]);
                        byte address3 = Convert.ToByte(dtPrintSet.Rows[0]["IsCollegeAdd3"]);
                        byte city = Convert.ToByte(dtPrintSet.Rows[0]["IsCollegeDist"]);
                        byte state = Convert.ToByte(dtPrintSet.Rows[0]["IsCollegeState"]);

                        byte university = Convert.ToByte(dtPrintSet.Rows[0]["IsCollegeUniversity"]);
                        byte rightLogo = Convert.ToByte(dtPrintSet.Rows[0]["IsRightLogo"]);
                        byte leftLogo = Convert.ToByte(dtPrintSet.Rows[0]["IsLeftLogo"]);
                        byte time;
                        if (Convert.ToBoolean(Convert.ToString(dtPrintSet.Rows[0]["IsTime"])))
                        {
                            time = 1;
                        }
                        else
                        {
                            time = 0;
                        }
                        byte degACR = Convert.ToByte(dtPrintSet.Rows[0]["IsDegreeAcr"]);
                        byte degNam = Convert.ToByte(dtPrintSet.Rows[0]["IsDegreeName"]);
                        byte studnam = Convert.ToByte(dtPrintSet.Rows[0]["IsStudName"]);
                        byte year = Convert.ToByte(dtPrintSet.Rows[0]["IsYear"]);
                        byte semester = Convert.ToByte(dtPrintSet.Rows[0]["IsSemester"]);
                        byte regno = Convert.ToByte(dtPrintSet.Rows[0]["IsRegNo"]);
                        byte rolno = Convert.ToByte(dtPrintSet.Rows[0]["IsRollNo"]);
                        byte admno = Convert.ToByte(dtPrintSet.Rows[0]["IsAdminNo"]);

                        byte fathername = Convert.ToByte(dtPrintSet.Rows[0]["IsFatherName"]);
                        byte seattype = Convert.ToByte(dtPrintSet.Rows[0]["IsSeatType"]);
                        //byte setRollAsAdmin = Convert.ToByte(dtPrintSet.Rows[0]["rollas_adm"]);
                        byte boarding = Convert.ToByte(dtPrintSet.Rows[0]["IsBoarding"]);
                        byte mothername = Convert.ToByte(dtPrintSet.Rows[0]["IsMontherName"]);
                        string recptValid = Convert.ToString(dtPrintSet.Rows[0]["ValidDate"]);


                        //Body Div Values
                        //byte showAllFees = Convert.ToByte(dtPrintSet.Rows[0]["showallfee"]);
                        byte allotedAmt = Convert.ToByte(dtPrintSet.Rows[0]["IsAllotedAmt"]);
                        byte fineAmt = Convert.ToByte(dtPrintSet.Rows[0]["IsFineAmt"]);
                        byte balAmt = Convert.ToByte(dtPrintSet.Rows[0]["IsBalanceAmt"]);
                        byte semOrYear = Convert.ToByte(dtPrintSet.Rows[0]["IsSemYear"]);
                        byte prevPaidAmt = Convert.ToByte(dtPrintSet.Rows[0]["IsPrevPaid"]);
                        byte excessAmt = Convert.ToByte(dtPrintSet.Rows[0]["IsExcessAmt"]);
                        // byte totDetails = Convert.ToByte(dtPrintSet.Rows[0]["Total_Details"]);
                        byte fineInRow = Convert.ToByte(dtPrintSet.Rows[0]["IsFineinRow"]);
                        //byte totWTselectCol = Convert.ToByte(dtPrintSet.Rows[0]["TotalSelCol"]);
                        byte concession = Convert.ToByte(dtPrintSet.Rows[0]["IsConcession"]);
                        string concessionValue = string.Empty;
                        if (concession != 0)
                        {
                            concessionValue = Convert.ToString(dtPrintSet.Rows[0]["ConcessionName"]);
                        }


                        //Footer Div Values

                        byte studCopy = Convert.ToByte(dtPrintSet.Rows[0]["IsStudCopy"]);
                        byte officopy = Convert.ToByte(dtPrintSet.Rows[0]["IsOfficeCopy"]);
                        byte transCopy = Convert.ToByte(dtPrintSet.Rows[0]["IsTransportCopy"]);
                        byte narration = Convert.ToByte(dtPrintSet.Rows[0]["IsNarration"]);
                        if (narration != 0)
                        {
                            mode += modePaySng;
                        }
                        byte deduction = Convert.ToByte(dtPrintSet.Rows[0]["IsTotConcession"]);
                        byte forclgName = Convert.ToByte(dtPrintSet.Rows[0]["IsForCollegeName"]);
                        byte authSign = Convert.ToByte(dtPrintSet.Rows[0]["IsAuthSign"]);
                        byte validDate = Convert.ToByte(dtPrintSet.Rows[0]["IsValidUpto"]);
                        string authSignValue = string.Empty;
                        if (authSign != 0)
                        {
                            authSignValue = Convert.ToString(dtPrintSet.Rows[0]["AuthName"]);

                        }
                        byte studOffiCopy = Convert.ToByte(dtPrintSet.Rows[0]["PageType"]);
                        // byte dispModeWTcash = Convert.ToByte(dtPrintSet.Rows[0]["DisModeWithCash"]);
                        byte signFile = Convert.ToByte(dtPrintSet.Rows[0]["cashier_sign"]);

                        //if (signFile != 0)
                        //{
                        //if (FileUpload1.HasFile)
                        //{
                        //}                                                    
                        //}

                        #endregion

                        #region Students Input

                        string colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegeCode + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,isnull(r.sections,'') as sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegeCode + "";


                        string collegename = "";
                        string add1 = "";
                        string add2 = "";
                        string add3 = "";
                        string univ = "";
                        string deg = "";
                        string cursem = "";
                        string batyr = "";
                        string seatty = "";
                        string board = "";
                        string mothe = "";
                        string fathe = "";
                        string stream = "";
                        string section = "";
                        double deductionamt = 0;
                        DataSet ds = new DataSet();
                        ds = DA.select_method_wo_parameter(colquery, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (degACR == 0)
                                {
                                    deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                }
                                else
                                {
                                    deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                }
                                cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                section = Convert.ToString(ds.Tables[1].Rows[0]["sections"]);
                            }
                        }
                        #endregion

                        int pagelength = 1;
                        int rectHeight = 800;
                        if (studOffiCopy == 1)
                        {
                            pagelength = 2;
                            rectHeight = 380;
                        }
                        PdfPage rcptpageTran = recptDoc.NewPage();
                        int curY = 10;
                        int curX = 30;
                        for (int pl = 1; pl <= pagelength; pl++)
                        {

                            if (pl == 2)
                            {
                                curY = 420;
                            }
                            #region Receipt Header


                            //Rectangle Border
                            PdfArea rectArea = new PdfArea(recptDoc, 10, curY, 570, rectHeight);
                            PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                            rcptpage.Add(rectSpace);

                            //Header Images
                            //Line1
                            if (leftLogo != 0)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    rcptpage.Add(LogoImage, curX, curY, 450);
                                }
                            }
                            if (collegeid != 0)
                            {
                                curX = 120;
                                PdfTextArea clgText = new PdfTextArea(FontboldheadC, Color.Black, new PdfArea(recptDoc, curX, curY + 5, 350, 20), ContentAlignment.MiddleCenter, collegename);
                                rcptpage.Add(clgText);
                            }
                            if (rightLogo != 0)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    curX = 500;
                                    PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    rcptpage.Add(LogoImage1, curX, curY, 450);
                                }
                            }
                            //Line2
                            if (university != 0)
                            {
                                curY += 20;
                                curX = 120;
                                PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, univ);
                                rcptpage.Add(uniText);
                            }
                            //Line3
                            string jaiadd1 = "";
                            if (address1 != 0 || address2 != 0)
                            {
                                curX = 120;
                                curY += 15;
                                if (address2 != 0)
                                {
                                    jaiadd1 = add1 + " " + add2;
                                }
                                PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, jaiadd1);
                                rcptpage.Add(addText);
                            }
                            //Line4
                            if (address3 != 0)
                            {
                                curX = 120;
                                curY += 15;
                                PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add3);
                                rcptpage.Add(cityText);
                            }

                            curX = 280;
                            curY += 35;
                            //Text Area For Receipt
                            PdfTextArea headingText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX - 100, curY, 200, 30), ContentAlignment.MiddleCenter, "EXCESS RECEIPT");
                            rcptpage.Add(headingText);
                            int curX1 = 265;
                            int curX2 = 315;
                            curY += 21;
                            //PdfLine underLineRecpt = new PdfLine(recptDoc, new Point(curX1, curY), new Point(curX2, curY), Color.Black, 1);
                            //rcptpage.Add(underLineRecpt);

                            #endregion

                            #region Table 1
                            //Table1 Format 
                            PdfTable tableparts = recptDoc.NewTable(FontTableHead, 5, 9, 5);
                            tableparts.VisibleHeaders = false;

                            //Table1 Data
                            //Line 1
                            tableparts.Cell(0, 0).SetContent("Receipt No");
                            tableparts.Cell(0, 0).SetFont(FontTableHead);
                            tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tableparts.Cell(0, 1).SetContent(": -");
                            tableparts.Cell(0, 1).SetFont(FontTableHead);
                            tableparts.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                            if (time != 0)
                            {
                                tableparts.Cell(0, 3).SetContent("Time");
                                tableparts.Cell(0, 3).SetFont(FontTableHead);
                                tableparts.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                                tableparts.Cell(0, 4).SetContent(": " + rcptTime);
                                tableparts.Cell(0, 4).SetFont(FontTableHead);
                                tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(0, 4).ColSpan = 2;
                            }

                            tableparts.Cell(0, 6).SetContent("Date");
                            tableparts.Cell(0, 6).SetFont(FontTableHead);
                            tableparts.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tableparts.Cell(0, 7).SetContent(": " + recptDt);
                            tableparts.Cell(0, 7).SetFont(FontTableHead);
                            tableparts.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tableparts.Cell(0, 7).ColSpan = 2;

                            //Line2
                            int rowIn = 1;
                            int colIn = 0;

                            if (studnam != 0)
                            {
                                tableparts.Cell(rowIn, colIn).SetContent("Name : " + studname);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(" : " + studname);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;

                            }
                            if (colIn == 8)
                            {
                                colIn = 0;
                                rowIn++;
                            }

                            if (regno != 0)
                            {
                                tableparts.Cell(rowIn, colIn).SetContent("RegNo : " + Regno);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(": " + Regno);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;
                                // tableparts.Cell(rowIn, colIn).ColSpan = 2;

                            }
                            if (colIn == 8)
                            {
                                colIn = 0;
                                rowIn++;
                            }

                            if (rolno != 0)
                            {
                                tableparts.Cell(rowIn, colIn).SetContent("RollNo : " + rollno);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(": " + rollno);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;
                                //  tableparts.Cell(rowIn, colIn).ColSpan = 2;
                            }
                            if (colIn == 8)
                            {
                                colIn = 0;
                                rowIn++;
                            }

                            if (admno != 0)
                            {
                                tableparts.Cell(rowIn, colIn).SetContent("AdmissionNo : " + app_formno);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(": " + app_formno);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).ColSpan = 2;
                            }
                            if (colIn == 8)
                            {
                                colIn = 0;
                                rowIn++;
                            }

                            if (fathername != 0)
                            {
                                tableparts.Cell(rowIn, colIn).SetContent("Father's Name : " + fathe);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(" : " + fathe);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;
                                //tableparts.Cell(1, 4).ColSpan = 2;
                            }
                            if (colIn == 8)
                            {
                                colIn = 0;
                                rowIn++;
                            }

                            if (mothername != 0)
                            {
                                tableparts.Cell(rowIn, colIn).SetContent("Mother's Name : " + mothe);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(": " + mothe);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;
                                // tableparts.Cell(1, colIn).ColSpan = 2;
                            }

                            //Line 3
                            if (colIn == 8)
                            {
                                colIn = 0;
                                rowIn++;
                            }

                            string batYrSemHead = string.Empty;
                            string batYrSemCont = string.Empty;
                            if (degACR != 0)
                            {
                                batYrSemHead = "Degree/";
                                batYrSemCont = deg + "/";
                            }
                            if (year != 0)
                            {
                                batYrSemHead += "Yr/";
                                batYrSemCont += " " + romanLetter(returnYearforSem(cursem)) + "/";

                            }
                            if (semester != 0)
                            {
                                batYrSemHead += "Sem";
                                batYrSemCont += " " + romanLetter(cursem);
                                if (section.Trim() != string.Empty)
                                {
                                    batYrSemCont += "-" + section;
                                }
                            }
                            batYrSemHead = batYrSemHead.TrimEnd('/');
                            batYrSemCont = batYrSemCont.TrimEnd('/');

                            if (batYrSemHead != "")
                            {
                                tableparts.Cell(rowIn, colIn).SetContent(batYrSemHead + " : " + batYrSemCont);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(": " + batYrSemCont);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;
                                // tableparts.Cell(2, colIn).ColSpan = 2;
                            }
                            if (colIn == 8)
                            {
                                colIn = 0;
                                rowIn++;
                            }

                            if (seattype != 0)
                            {
                                tableparts.Cell(rowIn, colIn).SetContent("Seat Type : " + seatty);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(": " + seatty);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;
                                //tableparts.Cell(rowIn, 4).ColSpan = 2;
                            }
                            if (colIn == 8)
                            {
                                colIn = 0;
                                rowIn++;
                            }

                            if (boarding != 0)
                            {
                                tableparts.Cell(rowIn, colIn).SetContent("Boarding : " + board);
                                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                colIn++;
                                //tableparts.Cell(rowIn, colIn).SetContent(": " + board);
                                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                colIn++;
                                // tableparts.Cell(rowIn, colIn).ColSpan = 2;
                            }

                            curX = 15;
                            curY += 1;
                            PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 600, 200));
                            rcptpage.Add(addtabletopage1);

                            #endregion

                            #region Table 2
                            //Table2 Format

                            int rows = 1;

                            string selectQuery = "";

                            Hashtable htIndex = new Hashtable();
                            int hInsdx = 3;

                            //Table2 Header

                            int descWidth = 440;

                            if (semOrYear != 0)
                            {

                                htIndex.Add("semOrYear", hInsdx);
                                hInsdx++;
                                descWidth -= 100;
                            }


                            //if (allotedAmt != 0)
                            //{

                            //htIndex.Add("allotedAmt", hInsdx);
                            //hInsdx++;
                            //descWidth -= 70;
                            //}

                            //if (balAmt != 0)
                            //{

                            //htIndex.Add("balAmt", hInsdx);
                            //hInsdx++;
                            //descWidth -= 70;
                            //}
                            //if (prevPaidAmt != 0)
                            //{

                            //    htIndex.Add("prevPaidAmt", hInsdx);
                            //    hInsdx++;
                            //    descWidth -= 80;
                            //}

                            //if (concession != 0)
                            //{

                            //    htIndex.Add("concession", hInsdx);
                            //    hInsdx++;
                            //    descWidth -= 70;
                            //}

                            //Table2 Data

                            int sno = 0;
                            int indx = 0;
                            double totalamt = 0;
                            double balanamt = 0;
                            double curpaid = 0;
                            // double paidamount = 0;


                            string selHeadersQ = string.Empty;
                            DataSet dsHeaders = new DataSet();

                            //selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";

                            selHeadersQ = " select l.ledgername as DispName,h.headername,el.Headerfk,el.Ledgerfk,isnull(el.ExcessAmt,0) as FeeAmount,isnull(el.AdjAmt,0) as TakenAmt,(isnull(el.ExcessAmt,0)-isnull(el.AdjAmt,0)) as BalAmount,'0'  as DeductAmout, isnull(el.ExcessAmt,0) as TotalAmount, el.Feecategory, el.FinYearFk,(select textval from textvaltable where textcode=el.feecategory) as semyear from ft_excessdet e,ft_excessledgerdet el,fm_ledgermaster l,fm_headermaster h where l.headerfk=h.headerpk and el.headerfk=h.headerpk and l.ledgerpk=el.ledgerfk and e.excessdetpk=el.excessdetfk and (isnull(el.ExcessAmt,0)-isnull(el.AdjAmt,0))>0  and e.app_no='" + AppNo + "' and e.feecategory in (" + feecat + ") ";
                            if (selHeadersQ != string.Empty)
                            {
                                string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                dsHeaders.Clear();
                                dsHeaders = DA.select_method_wo_parameter(selHeadersQ, "Text");

                                if (dsHeaders.Tables.Count > 0 && dsHeaders.Tables[0].Rows.Count > 0)
                                {
                                    rows += dsHeaders.Tables[0].Rows.Count;
                                    PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, htIndex.Count + 3, 5);
                                    tableparts1.VisibleHeaders = false;
                                    tableparts1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    tableparts1.Cell(0, 0).SetContent("S.No");
                                    tableparts1.Cell(0, 0).SetFont(FontTableHead);
                                    tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tableparts1.Columns[0].SetWidth(30);

                                    tableparts1.Cell(0, 1).SetContent("Description");
                                    tableparts1.Cell(0, 1).SetFont(FontTableHead);
                                    tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tableparts1.Columns[1].SetWidth(descWidth);

                                    tableparts1.Cell(0, 2).SetContent("Advance Amount Rs");
                                    tableparts1.Cell(0, 2).SetFont(FontTableHead);
                                    tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tableparts1.Columns[2].SetWidth(150);

                                    for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                    {
                                        string disphdr = string.Empty;
                                        double allotamt0 = 0;
                                        double deductAmt0 = 0;
                                        double totalAmt0 = 0;
                                        double paidAmt0 = 0;
                                        double balAmt0 = 0;
                                        double creditAmt0 = 0;

                                        creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                        totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                        balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                        deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                        disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                        string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                        string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                        string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                        paidAmt0 = creditAmt0;

                                        feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["semyear"]);
                                        sno++;
                                        indx++;
                                        totalamt += Convert.ToDouble(totalAmt0);
                                        balanamt += Convert.ToDouble(balAmt0);
                                        curpaid += Convert.ToDouble(creditAmt0);

                                        deductionamt += Convert.ToDouble(deductAmt0);

                                        tableparts1.Cell(indx, 0).SetContent(sno);
                                        tableparts1.Cell(indx, 0).SetFont(FontTable);
                                        tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                                        tableparts1.Cell(indx, 1).SetContent(disphdr);
                                        tableparts1.Cell(indx, 1).SetFont(FontTable);
                                        tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                        //tableparts1.Cell(indx, 2).SetContent(creditAmt0);
                                        tableparts1.Cell(indx, 2).SetContent(balAmt0);
                                        tableparts1.Cell(indx, 2).SetFont(FontTable);
                                        tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);

                                        if (semOrYear != 0)
                                        {
                                            if (htIndex.Contains("semOrYear"))
                                            {
                                                int ind = Convert.ToInt32(htIndex["semOrYear"]);
                                                tableparts1.Cell(indx, ind).SetContent(Convert.ToString(feecatcode));
                                                tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                if (indx == 1)
                                                {
                                                    tableparts1.Cell(0, ind).SetContent("Category");
                                                    tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                    tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tableparts1.Columns[ind].SetWidth(100);
                                                }
                                            }

                                            // htIndex.Add("semOrYear", hInsdx);
                                        }


                                        if (allotedAmt != 0)
                                        {
                                            if (htIndex.Contains("allotedAmt"))
                                            {
                                                int ind = Convert.ToInt32(htIndex["allotedAmt"]);
                                                tableparts1.Cell(indx, ind).SetContent(totalAmt0);
                                                tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                if (indx == 1)
                                                {
                                                    tableparts1.Cell(0, ind).SetContent("Excess Amount");
                                                    tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                    tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tableparts1.Columns[ind].SetWidth(60);
                                                }
                                            }
                                        }

                                        if (balAmt != 0)
                                        {
                                            if (htIndex.Contains("balAmt"))
                                            {
                                                int ind = Convert.ToInt32(htIndex["balAmt"]);
                                                tableparts1.Cell(indx, ind).SetContent(balAmt0);
                                                tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                if (indx == 1)
                                                {
                                                    tableparts1.Cell(0, ind).SetContent("Balance Rs");
                                                    tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                    tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tableparts1.Columns[ind].SetWidth(60);
                                                }
                                            }
                                        }
                                        if (prevPaidAmt != 0)
                                        {
                                            if (htIndex.Contains("prevPaidAmt"))
                                            {
                                                int ind = Convert.ToInt32(htIndex["prevPaidAmt"]);
                                                tableparts1.Cell(indx, ind).SetContent(paidAmt0);
                                                tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                if (indx == 1)
                                                {
                                                    tableparts1.Cell(0, ind).SetContent("Already Paid Rs");
                                                    tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                    tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tableparts1.Columns[ind].SetWidth(70);
                                                }
                                            }

                                        }

                                        if (concession != 0)
                                        {
                                            if (htIndex.Contains("concession"))
                                            {
                                                int ind = Convert.ToInt32(htIndex["concession"]);
                                                tableparts1.Cell(indx, ind).SetContent(deductAmt0);
                                                tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                if (indx == 1)
                                                {
                                                    tableparts1.Cell(0, ind).SetContent("Deduction Rs");
                                                    tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                    tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tableparts1.Columns[ind].SetWidth(60);
                                                }
                                            }

                                        }
                                    }
                                    createPDFOK = true;

                                    curY += 5 + (int)addtabletopage1.Area.Height;
                                    PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 30, curY, 520, 600));
                                    rcptpage.Add(addtabletopage2);


                                    #region Table 3
                                    //Table3 Format
                                    PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 1, 5, 5);
                                    tableparts2.VisibleHeaders = false;
                                    tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    //Table3 Header              
                                    decimal totalamount = (decimal)totalamt;

                                    tableparts2.Cell(0, 0).SetContent("Total");
                                    tableparts2.Cell(0, 0).SetFont(FontTableHead);
                                    tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                    tableparts2.Cell(0, 0).ColSpan = 4;

                                    tableparts2.Cell(0, 4).SetContent("Rs. " + totalamount + "/-");
                                    tableparts2.Cell(0, 4).SetFont(FontTableHead);
                                    tableparts2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleRight);


                                    curY += (int)addtabletopage2.Area.Height + 5;
                                    PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 30, curY, 520, 50));
                                    rcptpage.Add(addtabletopage3);
                                    curY += 5 + (int)addtabletopage3.Area.Height;
                                    #endregion

                                    #region Receipt Footer


                                    if (deduction != 0)
                                    {
                                        PdfTextArea deducText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 130, curY, 200, 20), ContentAlignment.MiddleCenter, "Deduction Amount Rs. : " + deductionamt);
                                        rcptpage.Add(deducText);
                                    }
                                    //if (excessAmt != 0)
                                    //{
                                    //    PdfTextArea exText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 240, curY, 200, 20), ContentAlignment.MiddleCenter, "Excess Amount Rs. : " + excessRemaining(appnoNew));
                                    //    rcptpage.Add(exText);
                                    //}
                                    if (validDate != 0)
                                    {
                                        PdfTextArea valdtText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 370, curY, 200, 20), ContentAlignment.MiddleCenter, "Valid upto : " + "(" + recptValid + ")");
                                        rcptpage.Add(valdtText);
                                    }

                                    //Authorizer
                                    if (forclgName != 0)
                                    {
                                        curY += 15;
                                        PdfTextArea authorizeText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 350, curY, 250, 20), ContentAlignment.MiddleCenter, "For " + collegename);
                                        rcptpage.Add(authorizeText);
                                    }

                                    if (authSignValue.Trim() != "")
                                    {
                                        curY += 15;
                                        PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, authSignValue);
                                        rcptpage.Add(authorizeSignText);
                                    }
                                    else
                                    {
                                        curY += 15;
                                        PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, "Authorized Sign");
                                        rcptpage.Add(authorizeSignText);
                                    }

                                    PdfPage rcptpageOf = rcptpage.CreateCopy();


                                    if (transCopy != 0 && pl == 1)
                                    {
                                        int cuyy = curY;
                                        //if (authSign == 0)
                                        //{
                                        cuyy += 10;
                                        //}
                                        rcptpageTran = rcptpage.CreateCopy();
                                        PdfTextArea transCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, cuyy, 150, 20), ContentAlignment.MiddleCenter, "Transport Copy ");
                                        rcptpageTran.Add(transCopyText);


                                    }


                                    if (studCopy != 0 || studOffiCopy == 1)
                                    {
                                        //if (authSign == 0)
                                        //{
                                        curY += 10;
                                        //}
                                        string copy = "Student Copy ";
                                        if (pl == 2)
                                            copy = "Office Copy ";
                                        PdfTextArea studCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, copy);
                                        rcptpage.Add(studCopyText);
                                        if (pl == pagelength)
                                        {
                                            rcptpage.SaveToDocument();
                                        }
                                    }

                                    //save changes

                                    if (pl == pagelength)
                                    {
                                        if (officopy != 0 && studOffiCopy != 1)
                                        {
                                            PdfTextArea offCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, "Office Copy ");
                                            rcptpageOf.Add(offCopyText);
                                            rcptpageOf.SaveToDocument();

                                        }

                                    }

                                    if (transCopy != 0 && pl == pagelength)
                                    {
                                        rcptpageTran.SaveToDocument();
                                    }

                                    curY += 10;

                                    #endregion

                                }
                            }

                            #endregion


                        }

                        #endregion
                    }
                    catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExcessReceipt.aspx"); }
                    finally
                    {
                    }
                    createPDFOK = true;
                    rcptCnt++;
                }
                else
                {
                    notapCnt++;
                }
            }
            else
            {
                notapCnt++;
            }
        }
        #endregion
        #region To print the Receipt
        if (createPDFOK && rcptCnt > 0)
        {
            //Response Write
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                Response.Buffer = true;
                Response.Clear();
                recptDoc.SaveToFile(szPath + szFile);

                Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                imgAlert.Visible = true;
                lbl_alert.Text = "Receipt Generated";
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Server Path Not Found";
            }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Receipt Cannot Be Generated";
        }
        #endregion
    }
    private double excessRemaining(string appnoNew)
    {
        string excessamtQ = DA.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " ");

        double excessamtValue = 0;
        double.TryParse(excessamtQ, out excessamtValue);
        return excessamtValue;
    }
    public void isContainsDecimal(double myValue)
    {
        bool hasFractionalPart = (myValue - Math.Round(myValue) != 0);
    }
    public string returnIntegerPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 0)
        {
            strVal = strvalArr[0];
        }
        return strVal;
    }
    public string returnDecimalPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 1)
        {
            strVal = strvalArr[1];
            if (strVal.Length >= 2)
            {
                strVal = strVal.Substring(0, 2);
            }
            else
            {
                while (2 != strVal.Length)
                {
                    strVal = strVal + "0";
                }
            }
        }
        else
        {
            strVal = "00";
        }
        return strVal;
    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "I";
                    break;
                case "2":
                    romanLettervalue = "II";
                    break;
                case "3":
                    romanLettervalue = "III";
                    break;
                case "4":
                    romanLettervalue = "IV";
                    break;
                case "5":
                    romanLettervalue = "V";
                    break;
                case "6":
                    romanLettervalue = "VI";
                    break;
                case "7":
                    romanLettervalue = "VII";
                    break;
                case "8":
                    romanLettervalue = "VIII";
                    break;
                case "9":
                    romanLettervalue = "IX";
                    break;
                case "10":
                    romanLettervalue = "X";
                    break;
            }
        }
        return romanLettervalue;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakhs";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    public string DecimalToWords(decimal number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + DecimalToWords(Math.Abs(number));

        string words = "";

        int intPortion = (int)number;
        decimal fraction = (number - intPortion) * 100;
        int decPortion = (int)fraction;

        words = ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
        if (decPortion > 0)
        {
            words += " And ";
            words += ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
            words += " Paise ";
        }
        return words;
    }
    public string NumberToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }

        return words;
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }
    //Code Last modified by Idhris : 23-07-2016

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
        lbl.Add(lblCollege);
        lbl.Add(lbl_stream);
        lbl.Add(lbl_degree);
        lbl.Add(lbl_branch);
        lbl.Add(lbl_Sem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 04-10-2016 sudhagar
}