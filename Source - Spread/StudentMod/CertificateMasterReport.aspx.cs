using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;


public partial class CertificateMasterReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string collegecode = "";
    int i = 0;
    static int chosedmode = 0;
    string query = "";
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable hat = new Hashtable();
    bool Cellclick = false;
    bool Cellclick1 = false;
    Hashtable totalmode = new Hashtable();
    static string path1 = "";
    int getcc = 0;
    static string checkkk = "";

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
                setLabelText();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                loadcollegebase();
                loadstream();
                educationLevelbase();
                bindbatch();
                degree();
                bindbranch();
                bindcertificate();
                bindstatus();
                loadstream1();
                educationLevelbase1();
                degree1();
                bindbranch1();
                bindcertificate1();
                BindRoll();
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

    public void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadstream();
        degree();
        bindbranch();
        educationLevelbase();
        bindcertificate();

    }

    public void loadcollegebase()
    {
        try
        {
            ddlclg.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();

                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch
        { }
    }

    public void ddledu_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        degree();
        bindbranch();
    }

    protected void educationLevelbase()
    {
        try
        {
            ddledu.Items.Clear();
            collegecode = Convert.ToString(ddlclg.SelectedItem.Value);
            string SelectQ = "select distinct Edu_Level  from course  where  college_code='" + collegecode + "'  order by Edu_Level desc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelectQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddledu.DataSource = ds;
                ddledu.DataTextField = "Edu_Level";
                ddledu.DataValueField = "Edu_Level";
                ddledu.DataBind();
            }
        }
        catch { }
    }

    public void ddl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        degree();

    }

    public void bindbatch()
    {
        ddl_batch.Items.Clear();
        ds = d2.BindBatch();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_batch.DataSource = ds;
            ddl_batch.DataTextField = "batch_year";
            ddl_batch.DataValueField = "batch_year";
            ddl_batch.DataBind();

            ddl_pop_batch.DataSource = ds;
            ddl_pop_batch.DataTextField = "batch_year";
            ddl_pop_batch.DataValueField = "batch_year";
            ddl_pop_batch.DataBind();
        }
    }

    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldegree.Text, "--Select--");
        bindbranch();
        bindcertificate();
    }

    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldegree.Text, "--Select--");
        bindbranch();
        bindcertificate();
    }

    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, "--Select--");
    }

    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, "--Select--");
    }

    public void cb_stream_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_stream, cbl_stream, txt_stream, lblStr.Text, "--Select--");
        educationLevelbase();
        degree();

    }

    public void cbl_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_stream, cbl_stream, txt_stream, lblStr.Text, "--Select--");
        educationLevelbase();
        degree();
    }

    public void cb_certificate_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_certificate, cbl_certificate, txt_certificate, "Certificate", "--Select--");
    }

    public void cbl_certificate_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_certificate, cbl_certificate, txt_certificate, "Certificate", "--Select--");
    }

    public void cb_statusdetail_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_statusdetail, cbl_status, txt_status, "Status", "--Select--");
    }

    public void cbl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_statusdetail, cbl_status, txt_status, "Status", "--Select--");
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

    public void loadstream()
    {
        try
        {
            string stream = "";
            cbl_stream.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + ddlclg.SelectedItem.Value + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stream.DataSource = ds;
                cbl_stream.DataTextField = "type";
                cbl_stream.DataBind();
                if (cbl_stream.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stream.Items.Count; i++)
                    {
                        cbl_stream.Items[i].Selected = true;
                    }
                    txt_stream.Text = lblStr.Text + "(" + cbl_stream.Items.Count + ")";
                    cb_stream.Checked = true;
                    txt_stream.Enabled = true;
                }
                else
                {
                    txt_stream.Text = "--Select--";
                    cb_stream.Checked = false;
                    txt_stream.Enabled = false;
                }

            }
            else
            {
                txt_stream.Enabled = false;

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
            string edulvl = Convert.ToString(ddledu.SelectedItem.Value);


            string query = "";
            string type = rs.GetSelectedItemsValueAsString(cbl_stream);
            if (type != "")
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlclg.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "') and type in('" + type + "')";
            }
            else
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlclg.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "')";
            }
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
                    //    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //    {
                    cbl_degree.Items[0].Selected = true;
                }
                txt_degree.Text = lbldegree.Text + "(" + 1 + ")";
                // cb_degree.Checked = true;
                //}
                //else
                //{
                //    txt_degree.Text = "--Select--";
                //    cb_degree.Checked = false;
                //}

                string deg = "";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {

                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
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

    public void bindbranch()
    {
        try
        {
            string branch = rs.GetSelectedItemsValueAsString(cbl_degree);
            cbl_branch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlclg.SelectedItem.Value + "' ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlclg.SelectedItem.Value + "'";
            }
            {
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        //    for (int i = 0; i < cbl_branch.Items.Count; i++)
                        //    {
                        cbl_branch.Items[0].Selected = true;
                    }
                    txt_branch.Text = lbl_branch.Text + "(" + 1 + ")";
                    //}

                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    public void bindstatus()
    {
        string type = "";
        string[] statusname = { "Received", "Yet to be Received", "Issued", "Yet to be Return" };
        for (int i = 0; i < 4; i++)
        {

            cbl_status.Items.Add(new System.Web.UI.WebControls.ListItem(statusname[i], Convert.ToString(i + 1)));

        }
        if (cbl_status.Items.Count > 0)
        {
            for (int i = 0; i < cbl_status.Items.Count; i++)
            {
                cbl_status.Items[i].Selected = true;
                type = Convert.ToString(cbl_status.Items[i].Text);
            }
            if (cbl_status.Items.Count == 1)
            {
                txt_status.Text = "Status(" + type + ")";
            }
            else
            {
                txt_status.Text = "Status(" + cbl_status.Items.Count + ")";
            }
            cb_statusdetail.Checked = true;
        }
    }

    public void bindcertificate()
    {
        string que = "";
        cbl_certificate.Items.Clear();
        string courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
        que = "select distinct MasterCode,MasterValue from CO_MasterValues co,CertMasterDet cm where MasterCriteria='CertificateName' and CollegeCode='" + ddlclg.SelectedItem.Value + "' and cm.CertName=co.MasterCode and cm.CourseID in('" + courseid + "')";

        ds.Clear();
        ds = d2.select_method(que, hat, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_certificate.DataSource = ds;
            cbl_certificate.DataTextField = "MasterValue";
            cbl_certificate.DataValueField = "MasterCode";
            cbl_certificate.DataBind();
            if (cbl_certificate.Items.Count > 0)
            {

                cbl_certificate.Items[0].Selected = true;
            }
            txt_certificate.Text = "Certificate(" + 1 + ")";
        }
    }

    public void rdb_cumm_CheckedChanged(object sender, EventArgs e)
    {
    }

    public void rdb_detail_CheckedChanged(object sender, EventArgs e)
    {
    }

    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdb_cumm.Checked == true)
            {
                cummulativeGo();
            }
            else
            {
                detailGo();
            }
        }
        catch
        {
        }
    }

    public void cummulativeGo()
    {
        try
        {
            txt_excelname1.Text = "";
            txt_excelname.Text = "";
            int tot_strg = 0;
            int count = 0;
            int count1 = 0;
            Fpspread1.Visible = true;
            Fpspread2.Visible = false;
            lbl_headernamespd2.Visible = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = true;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
            Fpspread1.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            string batch = Convert.ToString(ddl_batch.SelectedItem.Value);
            string adddeg = rs.GetSelectedItemsValueAsString(cbl_branch);
            string courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
            string certificatename = rs.GetSelectedItemsText(cbl_certificate);
            string certificatevalue = rs.GetSelectedItemsValueAsString(cbl_certificate);
            string statusvalue = rs.GetSelectedItemsValue(cbl_status);
            string statusname = "";
            for (i = 0; i < cbl_status.Items.Count; i++)
            {
                if (cbl_status.Items[i].Selected == true)
                {
                    count1 = 1;
                    string addstatus = cbl_status.Items[i].Text.ToString();
                    string addstatus1 = cbl_status.Items[i].Value.ToString();
                    if (statusname == "")
                    {
                        statusname = addstatus;
                    }
                    else
                    {
                        statusname = statusname + "," + addstatus;
                    }
                }
            }

            query = "select COUNT(r.app_no)as TotalStrength,No_Of_seats,r.Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  from Registration r,applyn a, degree d,Department dt,Course C where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + batch + "')  and a.college_code='" + ddlclg.SelectedItem.Value + "' group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name ,r.Sections  ";

            query = query + "select COUNT(r.app_no)as TotalStrength,CertificateId,isIssued,IsReturn,isOrginal,isDuplicate,isIssuedDuplicate,isReturnDuplicate from StudCertDetails_New sd,Registration r where r.App_No=sd.App_no group by CertificateId,IsReturn,isOrginal,isDuplicate,isIssued,isIssuedDuplicate,isReturnDuplicate   ";
            query = query + "select COUNT(r.app_no)as TotalStrength,CertificateId,isOrginal,isDuplicate from StudCertDetails_New sd,Registration r where r.App_No=sd.App_no  group by CertificateId,isOrginal,isDuplicate  ";
            //query = query + " select * from Registration r,applyn a, degree d,Department dt,Course C,StudCertDetails_New S where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + batch + "')  and a.college_code='" + ddlclg.SelectedItem.Value + "' AND	r.App_No=s.App_no ";
            query = query + "select COUNT(r.app_no)as TotalStrength,CertificateId,isIssued,IsReturn,isOrginal,isDuplicate,isIssuedDuplicate,isReturnDuplicate,No_Of_seats,r.Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  from Registration r,applyn a, degree d,Department dt,Course C,StudCertDetails_New sd where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + batch + "')  and a.college_code='" + ddlclg.SelectedItem.Value + "' and  r.App_No=sd.App_no  group by No_Of_seats,r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name ,r.Sections ,CertificateId,isIssued,IsReturn,isOrginal,isDuplicate,isIssuedDuplicate,isReturnDuplicate";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (query == "")
            {
                Fpspread1.Visible = false;
                Fpspread2.Visible = false;
                lbl_err_stud.Visible = true;
                lbl_err_stud.Text = "Kindly Select All List ";
                lbl_headernamespd2.Visible = false;
                div_report1.Visible = false;
                return;
            }
            else
            {
                if (query != "")
                {
                    // ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread2.Visible = false;
                        Fpspread1.Visible = false;
                        lbl_err_stud.Visible = true;
                        lbl_err_stud.Text = "No Records Found";
                        lbl_headernamespd2.Visible = false;
                        div_report1.Visible = false;
                        return;
                    }
                    else
                    {
                        lbl_err_stud.Visible = false;
                        div_report1.Visible = false;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 3), 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 3), 1].Text = "Batch";
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 3), 2].Text = lbldegree.Text;
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 3), 3].Text = lbl_branch.Text;
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 3), 4].Text = "Total No Of Student";
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);

                            int ff = 0;
                            if (count1 == 1)
                            {
                                int cc = 4;
                                int j = 0;
                                int dd = 0;

                                int x = 0;

                                for (j = 0; j < cbl_certificate.Items.Count; j++)
                                {
                                    if (cbl_certificate.Items[j].Selected == true)
                                    {

                                        int d = 0;
                                        for (int jj = 0; jj < cbl_status.Items.Count; jj++)
                                        {
                                            if (cbl_status.Items[jj].Selected == true)
                                            {
                                                cc++;
                                                d++;

                                                Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 3), cc].Text = cbl_certificate.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = cbl_status.Items[jj].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Orginal";
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Orginal";
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + cbl_status.Items[jj].Value + "-" + "isOrginal";
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                                cc++;
                                                Fpspread1.Sheets[0].ColumnCount = cc + 1;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 3), cc].Text = cbl_certificate.Items[j].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Duplicate";
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = cbl_status.Items[jj].Text;
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + cbl_status.Items[jj].Value + "-" + "isDuplicate";
                                                Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Duplicate";
                                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, cc, 1, 2);

                                            }

                                        }

                                        if (x == 0)
                                        {
                                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, d * 2);
                                            x = 5;
                                        }
                                        else
                                        {
                                            if (d == 1)
                                            {
                                                x = x + 2;
                                            }
                                            else if (d == 2)
                                            {
                                                x = x + 4;
                                            }
                                            else if (d == 3)
                                            {
                                                x = x + 6;
                                            }
                                            else if (d == 4)
                                            {
                                                x = x + 8;
                                            }

                                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, x, 1, d * 2);
                                        }
                                        if (d == 4)
                                        {
                                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 5, 1, d - 2);
                                        }

                                    }
                                }

                                for (j = 0; j < cbl_certificate.Items.Count; j++)
                                {
                                    if (cbl_certificate.Items[j].Selected == true)
                                    {
                                        for (int h = 5; h <= cc; h++)
                                        {
                                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, h, 1, 2);
                                        }
                                    }
                                }

                                ff = cc;
                            }
                            DataView dv = new DataView();

                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread1.Sheets[0].RowCount++;
                                count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Column.Width = 300;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["TotalStrength"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Locked = true;
                                int totstrenth = Convert.ToInt32(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text);


                                if (tot_strg == 0)
                                {
                                    tot_strg = totstrenth;
                                }
                                else
                                {
                                    tot_strg = tot_strg + totstrenth;
                                }
                                if (count1 == 1)
                                {
                                    int cc = 4;
                                    for (int s = cc; s < ff; s++)
                                    {
                                        cc++;
                                        string values = "";
                                        string tag = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                        string[] tt = tag.Split('-');
                                        string statusval = tt[1];
                                        string org_dup = tt[2];
                                        string[] vall = statusval.Split(',');
                                        for (int ss = 0; ss < vall.Length; ss++)
                                        {
                                            values = newfunction(vall[ss]);
                                            if (statusval == "1")
                                            {
                                                if (org_dup == "isOrginal")
                                                {
                                                    ds.Tables[3].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' and isOrginal='True'";
                                                }
                                                else
                                                {
                                                    ds.Tables[3].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' and isDuplicate='True'";
                                                }
                                            }
                                            else if (statusval == "2")
                                            {
                                                if (org_dup == "isOrginal")
                                                {
                                                    ds.Tables[3].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' and isOrginal='False'";
                                                }
                                                else
                                                {
                                                    ds.Tables[3].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' and isDuplicate='False'";
                                                }
                                            }
                                            else if (statusval == "3")
                                            {
                                                if (org_dup == "isOrginal")
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' and isIssued='True'";
                                                }
                                                else
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' and isIssuedDuplicate='True'";
                                                }
                                            }
                                            else if (statusval == "4")
                                            {
                                                if (org_dup == "isOrginal")
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' and isIssued='False'";
                                                }
                                                else
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' and isIssuedDuplicate='False'";
                                                }
                                            }
                                            else
                                            {
                                                ds.Tables[1].DefaultView.RowFilter = " CertificateId='" + tt[0] + "' " + values + "";
                                            }
                                            if (statusval == "4" || statusval == "3")
                                            {
                                                dv = ds.Tables[1].DefaultView;
                                            }
                                            else
                                            {
                                                dv = ds.Tables[3].DefaultView;
                                            }
                                            if (dv.Count > 0)
                                            {
                                                string tot = Convert.ToString(dv[0]["TotalStrength"]);
                                                if (vall[ss] == "2")
                                                {
                                                    string totstg = Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text);
                                                    if (org_dup != "isOrginal")
                                                    {
                                                        tot = totstg;
                                                    }
                                                    else
                                                    {
                                                        int total = Convert.ToInt32(totstg) - Convert.ToInt32(tot);
                                                        tot = Convert.ToString(total);
                                                    }
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = tot;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;

                                                }
                                                else
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = tot;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Tag = tt[0];
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;

                                                }
                                                if (!totalmode.Contains(Convert.ToString(cc)))
                                                {

                                                    totalmode.Add(Convert.ToString(cc), Convert.ToString(tot));
                                                }
                                                else
                                                {
                                                    string getvalue = Convert.ToString(totalmode[Convert.ToString(cc)]);
                                                    if (getvalue.Trim() != "")
                                                    {
                                                        getvalue = getvalue + "," + tot;
                                                        totalmode.Remove(Convert.ToString(cc));
                                                        if (getvalue.Trim() != "")
                                                        {
                                                            totalmode.Add(Convert.ToString(cc), Convert.ToString(getvalue));
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                string totstg = "";
                                                if (vall[ss] == "2")
                                                {

                                                    string totstg1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text);
                                                    if (Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[(Fpspread1.Sheets[0].ColumnHeader.RowCount - 1), cc - 2]) == "Received")
                                                    {
                                                        totstg = Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc - 2].Text);
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc - 2].Font.Size = FontUnit.Medium;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                    }
                                                    else
                                                    {
                                                        if (org_dup == "isOrginal")
                                                        {
                                                            totstg = d2.GetFunction("select COUNT(r.app_no)as TotalStrength from StudCertDetails_New sd,Registration r where r.App_No=sd.App_no and  r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + batch + "')  and r.college_code='" + ddlclg.SelectedItem.Value + "' and CertificateId='" + tt[0] + "' and isOrginal='True'  group by CertificateId,isOrginal,isDuplicate");
                                                        }
                                                        else
                                                        {
                                                            totstg = d2.GetFunction("select COUNT(r.app_no)as TotalStrength from StudCertDetails_New sd,Registration r where r.App_No=sd.App_no and  r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + batch + "')  and r.college_code='" + ddlclg.SelectedItem.Value + "' and CertificateId='" + tt[0] + "' and isDuplicate='True'  group by CertificateId,isOrginal,isDuplicate");
                                                            totstg = Convert.ToString(Convert.ToInt32(totstg1) - Convert.ToInt32(totstg));
                                                        }
                                                    }
                                                    // totstg = Convert.ToString(Convert.ToInt32(totstg1) - Convert.ToInt32(totstg));
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = totstg;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                }
                                                else
                                                {
                                                    totstg = "0";
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Text = "0";
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Tag = tt[0];
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                }
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                                if (!totalmode.Contains(Convert.ToString(cc)))
                                                {

                                                    totalmode.Add(Convert.ToString(cc), Convert.ToString(totstg));
                                                }
                                                else
                                                {
                                                    string getvalue = Convert.ToString(totalmode[Convert.ToString(cc)]);
                                                    if (getvalue.Trim() != "")
                                                    {
                                                        getvalue = getvalue + "," + "0";
                                                        totalmode.Remove(Convert.ToString(cc));
                                                        if (getvalue.Trim() != "")
                                                        {
                                                            totalmode.Add(Convert.ToString(cc), Convert.ToString(getvalue));
                                                        }
                                                    }
                                                }
                                            }

                                        }

                                    }
                                    getcc = cc;
                                }
                                Fpspread1.Sheets[0].RowCount++;
                                Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Text = "Total";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Tag = "Total";
                                Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].BackColor = ColorTranslator.FromHtml("#80EDED");
                                Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Font.Bold = true;
                                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 4].ForeColor = Color.Maroon;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 4].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 4].Text = Convert.ToString(tot_strg);
                                Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 4].BackColor = ColorTranslator.FromHtml("#80EDED");
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                                if (totalmode.Count > 0)
                                {
                                    for (int r1 = 5; r1 <= getcc; r1++)
                                    {
                                        string totalvalue = Convert.ToString(totalmode[Convert.ToString(r1)]);
                                        if (totalvalue != "")
                                        {
                                            int gettotalvalue = 0;
                                            string[] spl = totalvalue.Split(',');
                                            for (int l = 0; l < spl.Length; l++)
                                            {
                                                int get_tot = Convert.ToInt32(spl[l]);
                                                if (gettotalvalue == 0)
                                                {
                                                    gettotalvalue = get_tot;
                                                }
                                                else
                                                {
                                                    gettotalvalue = gettotalvalue + get_tot;
                                                }
                                            }
                                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].Text = Convert.ToString(gettotalvalue);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, r1].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].ForeColor = ColorTranslator.FromHtml("#107532");
                                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].Font.Bold = true;
                                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), r1].BackColor = ColorTranslator.FromHtml("#80EDED");
                                        }
                                    }
                                }
                            }
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            Fpspread1.Width = 900;
                            Fpspread1.Height = 420;
                            Fpspread1.Visible = true;
                            div_report.Visible = false;
                            div_report1.Visible = true;
                        }
                    }
                }
            }

        }
        catch
        {
        }
    }

    public string newfunction(string val)
    {
        string text_val = "";
        if (val.Trim() == "4")
        {
            text_val = " and IsReturn='True' and isIssued='False'";
        }
        if (val.Trim() == "3")
        {
            text_val = " and isIssued='True' AND IsReturn='False'";
        }
        if (val.Trim() == "1")
        {
            text_val = "";
        }
        if (val.Trim() == "2")
        {
            text_val = " and isIssued='True' ";
        }
        return text_val;
    }

    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
            Cellclick1 = false;
        }
        catch
        {
        }
    }

    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            if (rdb_cumm.Checked == true)
            {
                fpspread1go();
            }
        }
    }

    public void fpspread1go()
    {
        try
        {
            txt_excelname1.Text = "";
            txt_excelname.Text = "";
            string activerow = "";
            string activecol = "";
            string header = "";
            string actval = "";
            int val = 0;
            int count = 0;
            string headertype = "";
            string headertype1 = "";
            checkkk = "1";
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string Batch_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            string course_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
            string dept_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
            if (dept_tagvalue == "Total")
            {
                dept_tagvalue = rs.GetSelectedItemsValueAsString(cbl_branch);
                Batch_tagvalue = Convert.ToString(ddl_batch.SelectedItem.Value);
            }
            if (Convert.ToInt32(activecol) <= 4)
            {
                header = "All";
                val = 0;
                Batch_tagvalue = Convert.ToString(ddl_batch.SelectedItem.Value);
                dept_tagvalue = rs.GetSelectedItemsValueAsString(cbl_branch);
            }
            else
            {
                val = 1;
                actval = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text);
                header = Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Text;
                headertype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Tag);
                headertype1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol)].Text);
            }
            Fpspread2.Sheets[0].Visible = true;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 2;
            Fpspread2.Sheets[0].ColumnCount = 6;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            string[] cert_val = headertype.Split('-');
            string name = header;
            if (val == 1)
            {
                if (cert_val[1] == "1" && cert_val[2] == "isOrginal")
                {
                    val = 11;
                    query = " select * from Registration r,StudCertDetails_New s , degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No =s.App_no and isOrginal  ='1' and CertificateId ='" + cert_val[0] + "'   ";
                }
                else if (cert_val[1] == "1" && cert_val[2] == "isDuplicate")
                {
                    val = 22;
                    query = " select * from Registration r,StudCertDetails_New s , degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No =s.App_no and isDuplicate  ='1' and CertificateId ='" + cert_val[0] + "'   ";
                }
                else if (cert_val[1] == "2" && cert_val[2] == "isOrginal")
                {
                    val = 33;
                    query = "select * from Registration r, degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and App_No not in(select app_no from StudCertDetails_New where isOrginal ='1' and CertificateId ='" + cert_val[0] + "')";
                }
                else if (cert_val[1] == "2" && cert_val[2] == "isDuplicate")
                {
                    val = 44;
                    query = "select * from Registration r, degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and App_No not in(select app_no from StudCertDetails_New where isDuplicate ='1' and CertificateId ='" + cert_val[0] + "')";
                }
                if (cert_val[1] == "3" && cert_val[2] == "isOrginal")
                {
                    val = 55;
                    query = " select * from Registration r,StudCertDetails_New s , degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No =s.App_no and isIssued  ='1' and CertificateId ='" + cert_val[0] + "'   ";
                }
                else if (cert_val[1] == "3" && cert_val[2] == "isDuplicate")
                {
                    val = 66;
                    query = " select * from Registration r,StudCertDetails_New s , degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No =s.App_no and IsReturn  ='1' and CertificateId ='" + cert_val[0] + "'   ";
                }
                else if (cert_val[1] == "4" && cert_val[2] == "isOrginal")
                {
                    val = 77;
                    query = " select * from Registration r,StudCertDetails_New s , degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No =s.App_no and isIssuedDuplicate  ='1' and CertificateId ='" + cert_val[0] + "'   ";
                }
                else if (cert_val[1] == "4" && cert_val[2] == "isDuplicate")
                {
                    val = 88;
                    query = " select * from Registration r,StudCertDetails_New s , degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No =s.App_no and isIssuedDuplicate  ='0' and CertificateId ='" + cert_val[0] + "'   ";
                }
            }
            else
            {
                val = 0;
                query = "select * from Registration r,degree d,Department dt,Course C where r.degree_code ='" + dept_tagvalue + "' and Batch_Year ='" + Batch_tagvalue + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  ";

            }
            query = query + "SELECT app_no,certificateno,(Select MasterValue FROM CO_MasterValues T WHERE CertificateId = t.MasterCode) CertificateId,case when isOrginal='1' then 'Yes' when isOrginal='0' then 'No'  end as isOrginal ,case when isDuplicate='1' then 'yes' when isDuplicate='0' then 'No' end as isDuplicate FROM StudCertDetails_New";

            ds.Clear();
            if (query == "")
            {
                Fpspread2.Visible = false;
                Label1.Visible = true;
                Label1.Text = "Kindly Select All List ";
                lbl_headernamespd2.Visible = false;
                div_report.Visible = false;
                return;
            }
            else
            {
                if (query != "")
                {
                    ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {

                        Fpspread2.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = "No Records Found";
                        div_report.Visible = false;
                        lbl_headernamespd2.Visible = false;
                        return;
                    }
                    else
                    {
                        Label1.Visible = false;
                        lbl_headernamespd2.Visible = false;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lbl_headernamespd2.Visible = true;
                            if (name == "All")
                            {
                                lbl_headernamespd2.Text = name;
                            }
                            else
                            {
                                lbl_headernamespd2.Text = name;
                            }
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 1].Text = "Roll No";
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 2].Text = "Student Name";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 2].Column.Width = 240;
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 3].Text = "Batch";
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 4].Text = lbldegree.Text;
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 5].Text = lbl_branch.Text;
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                            int ff = 0;
                            int cc = 5;
                            int j = 0;
                            int x = 0;
                            int d = 0;
                            if (val != 0)
                            {
                                if (cert_val[2] == "isOrginal")
                                {
                                    cc++;
                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = header;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Orginal";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Orginal";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isOrginal";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                }
                                if (cert_val[2] == "isDuplicate")
                                {
                                    cc++;
                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Duplicate";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isDuplicate";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Duplicate";
                                }
                                cc++;
                                Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Certificate No";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "certificateno";
                            }
                            else
                            {

                                for (j = 0; j < cbl_certificate.Items.Count; j++)
                                {
                                    if (cbl_certificate.Items[j].Selected == true)
                                    {

                                        cc++;
                                        Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = cbl_certificate.Items[j].Text;
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Orginal";
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Orginal";
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isOrginal";
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;

                                        cc++;
                                        Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Duplicate";
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isDuplicate";
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Duplicate";

                                        cc++;
                                        Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Certificate No";
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "certificateno";
                                        if (val == 0)
                                        {
                                            if (x == 0)
                                            {
                                                Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 3);
                                                x = 6;
                                            }
                                            else
                                            {
                                                x = x + 3;
                                                Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, x, 1, 3);
                                            }
                                        }
                                    }
                                }
                            }
                            cc++;

                            int sss = 6;
                            int ii = 0;



                            if (val != 0)
                            {
                                for (j = 0; j < cbl_certificate.Items.Count; j++)
                                {
                                    if (cbl_certificate.Items[j].Selected == true)
                                    {
                                        if (ii == 0)
                                        {
                                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, sss, 1, 2);
                                            ii++;
                                        }
                                        else
                                        {
                                            sss = sss + 2;
                                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, sss, 1, 2);

                                        }
                                    }
                                }
                            }
                            Fpspread2.Sheets[0].ColumnCount = cc + 1;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = "View";
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, cc, 2, 1);
                            ff = cc;
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Column.Width = 50;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Column.Width = 150;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Column.Width = 260;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Column.Width = 100;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Column.Width = 100;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Column.Width = 300;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                cc = 5;
                                DataView dv = new DataView();
                                string orginal = "";
                                string cert_no = "";
                                string duplicate = "";
                                string filename = "";
                                FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                                btn.Text = "View";
                                if (val == 33 || val == 44 || val == 0)
                                {
                                    if (val != 0)
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = " app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + Convert.ToString(cert_val[0]) + "'";
                                        dv = ds.Tables[1].DefaultView;

                                        if (dv.Count > 0)
                                        {
                                            orginal = Convert.ToString(dv[0]["isOrginal"]);
                                            duplicate = Convert.ToString(dv[0]["isDuplicate"]);
                                            cert_no = Convert.ToString(dv[0]["certificateno"]);
                                            filename = Convert.ToString(dv[0]["FileName"]);
                                            if (cert_val[2] == "isOrginal")
                                            {
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = orginal;
                                            }
                                            else
                                            {
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = duplicate;
                                            }
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Locked = true;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Locked = true;
                                            if (cert_no == "")
                                            {
                                                cert_no = "-";
                                            }
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = cert_no;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Tag = filename;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].CellType = btn;
                                        }
                                        else
                                        {
                                            if (val != 0)
                                            {
                                                if (cert_val[2] == "isOrginal")
                                                {
                                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = "No";
                                                }
                                                else
                                                {
                                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = "No";
                                                }
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = "-";
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].CellType = btn;
                                            }
                                            else
                                            {
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = "-";

                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = "No";
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].CellType = btn;
                                            }
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Locked = true;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Locked = true;

                                        }
                                    }
                                    else
                                    {

                                        for (j = 0; j < cbl_certificate.Items.Count; j++)
                                        {
                                            if (cbl_certificate.Items[j].Selected == true)
                                            {
                                                cc++;
                                                //ds.Tables[1].DefaultView.RowFilter = " app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + Convert.ToString(cbl_certificate.Items[j].Value) + "'";
                                                //dv = ds.Tables[1].DefaultView;
                                                orginal = d2.GetFunction(" select case when isOrginal='1' then 'Yes' when isOrginal='0' then 'No'  end as isOrginal from StudCertDetails_New where App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + Convert.ToString(cbl_certificate.Items[j].Value) + "'");
                                                duplicate = d2.GetFunction(" select case when isDuplicate='1' then 'yes' when isDuplicate='0' then 'No' end as isDuplicate from StudCertDetails_New where App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + Convert.ToString(cbl_certificate.Items[j].Value) + "'");
                                                cert_no = d2.GetFunction(" select certificateno from StudCertDetails_New where App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + Convert.ToString(cbl_certificate.Items[j].Value) + "'");
                                                filename = d2.GetFunction(" select Filename from StudCertDetails_New where App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + Convert.ToString(cbl_certificate.Items[j].Value) + "'");
                                                if (orginal == "0")
                                                {
                                                    orginal = "No";
                                                }
                                                if (duplicate == "0")
                                                {
                                                    duplicate = "No";
                                                }
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = orginal;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                                cc++;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = duplicate;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                                cc++;
                                                if (cert_no == "0" || cert_no == "")
                                                {
                                                    cert_no = "-";
                                                }
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = cert_no;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Tag = filename;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;


                                            }
                                        }

                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc + 1].CellType = btn;
                                    }


                                }
                                else
                                {
                                    if (val != 0)
                                    {

                                        orginal = Convert.ToString(ds.Tables[0].Rows[i]["isOrginal"]);
                                        if (orginal == "False")
                                        {
                                            orginal = "No";
                                        }
                                        else
                                        {
                                            orginal = "Yes";
                                        }
                                        duplicate = Convert.ToString(ds.Tables[0].Rows[i]["isDuplicate"]);
                                        if (duplicate == "False")
                                        {
                                            duplicate = "No";
                                        }
                                        else
                                        {
                                            duplicate = "Yes";
                                        }
                                        cert_no = Convert.ToString(ds.Tables[0].Rows[i]["certificateno"]);
                                        filename = Convert.ToString(ds.Tables[0].Rows[i]["FileName"]);
                                        if (cert_val[2] == "isOrginal")
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = orginal;
                                        }
                                        else
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(duplicate);
                                        }
                                        if (cert_no == "")
                                        {
                                            cert_no = "-";
                                        }
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(cert_no);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Tag = filename;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].CellType = btn;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Locked = true;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Locked = true;

                                    }

                                }

                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;


                            }
                            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                            Fpspread2.Width = 900;
                            Fpspread2.Height = 420;
                            Fpspread2.Visible = true;
                            div_report.Visible = true;
                        }
                    }
                }
            }
            checkkk = "1";
        }
        catch
        {
        }
    }

    public void detailGo()
    {

        try
        {
            lbl_err_stud.Visible = false;
            txt_excelname1.Text = "";
            txt_excelname.Text = "";
            checkkk = "2";
            int count = 0;
            int count1 = 0;
            Fpspread1.Visible = false;
            Fpspread3.Visible = false;
            lbl_headernamespd2.Visible = false;
            Fpspread2.Visible = true;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 2;
            Fpspread2.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            string batch = Convert.ToString(ddl_batch.SelectedItem.Value);
            string adddeg = rs.GetSelectedItemsValueAsString(cbl_branch);
            string statusname = "";
            for (i = 0; i < cbl_status.Items.Count; i++)
            {
                if (cbl_status.Items[i].Selected == true)
                {
                    count1 = 1;
                    string addstatus = cbl_status.Items[i].Text.ToString();
                    string addstatus1 = cbl_status.Items[i].Value.ToString();
                    if (statusname == "")
                    {
                        statusname = addstatus;
                    }
                    else
                    {
                        statusname = statusname + "," + addstatus;
                    }
                }
            }

            query = "select distinct s.certificateno,(Select MasterValue FROM CO_MasterValues T WHERE CertificateId = t.MasterCode) CertificateId,case when s.isOrginal='1' then 'Yes' when s.isOrginal='0' then 'No'  end as isOrginal ,case when s.isDuplicate='1' then 'yes' when s.isDuplicate='0' then 'No' end as isDuplicate, a.stud_name, r.app_no,r.Sections,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  from Registration r,applyn a, degree d,Department dt,Course C,StudCertDetails_New s,CertMasterDet cm where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No  and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + batch + "')  and a.college_code='" + ddlclg.SelectedItem.Value + "' and c.Course_Id=cm.CourseID  and s.App_no=r.App_No ";
            query = query + " select * from StudCertDetails_New ";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (query == "")
            {
                Fpspread2.Visible = false;
                Label1.Visible = true;
                div_report.Visible = false;
                lbl_err_stud.Text = "Kindly Select All List ";
                return;
            }
            else
            {
                if (query != "")
                {
                    ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {

                        Fpspread2.Visible = false;
                        Label1.Visible = true;
                        lbl_err_stud.Text = "No Records Found";
                        div_report.Visible = false;
                        return;
                    }
                    else
                    {
                        Label1.Visible = false;
                        div_report.Visible = false;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 1].Text = "Student Name";
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 2].Text = "Batch";
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 3].Text = lbldegree.Text;
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), 4].Text = lbl_branch.Text;
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                            int ff = 0;
                            int cc = 4;
                            int j = 0;
                            int d = 0;
                            int x = 0;
                            for (j = 0; j < cbl_certificate.Items.Count; j++)
                            {
                                if (cbl_certificate.Items[j].Selected == true)
                                {

                                    cc++;
                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = cbl_certificate.Items[j].Text;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Orginal";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Orginal";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isOrginal";

                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                    cc++;
                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Duplicate";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isDuplicate";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Duplicate";

                                    cc++;
                                    Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Certificate No";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "certificateno";
                                }

                                if (x == 0)
                                {
                                    Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 3);
                                    x = 5;
                                }
                                else
                                {
                                    x = x + 3;
                                    Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, x, 1, 3);
                                }
                            }

                            cc++;
                            Fpspread2.Sheets[0].ColumnCount = cc + 1;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[(Fpspread2.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = "View";
                            Fpspread2.Sheets[0].ColumnHeaderSpanModel.Add(0, cc, 2, 1);
                            ff = cc;
                            FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                            btn.Text = "View";
                            DataView dv = new DataView();
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Column.Width = 250;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Column.Width = 100;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Column.Width = 100;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Column.Width = 300;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Locked = true;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                cc = 4;
                                for (int s = cc; s < ff - 1; s++)
                                {
                                    cc++;

                                    string tag = Convert.ToString(Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                    string[] tt = tag.Split('-');
                                    string statusval = tt[1];
                                    string[] vall = statusval.Split(',');
                                    for (int ss = 0; ss < vall.Length; ss++)
                                    {

                                        ds.Tables[1].DefaultView.RowFilter = " app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + Convert.ToString(tt[0]) + "'";
                                        dv = ds.Tables[1].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            string orginal = Convert.ToString(dv[0][statusval]);
                                            string orginal1 = Convert.ToString(dv[0]["isOrginal"]);
                                            string duplicat1 = Convert.ToString(dv[0]["isDuplicate"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(ds.Tables[0].Rows[i][statusval]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                        }
                                        else
                                        {
                                            if (statusval != "certificateno")
                                            {
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = "No";
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Size = FontUnit.Medium;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Font.Name = "Book Antiqua";
                                            }
                                            else
                                            {
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = "-";
                                            }
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Locked = true;
                                        }
                                    }
                                }
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc + 1].CellType = btn;
                                getcc = cc;

                            }
                            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                            Fpspread2.Width = 900;
                            Fpspread2.Height = 420;
                            div_report.Visible = true;
                            div_report1.Visible = false;
                            Fpspread2.Visible = true;
                        }
                    }
                }
            }
            checkkk = "2";
        }
        catch
        {
        }
    }

    public void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            popview.Visible = true;
        }
        catch
        {
        }
    }

    public void btn_popclose_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
    }

    public void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadstream1();
        degree1();
        bindbranch1();
        educationLevelbase1();
        bindcertificate1();

    }

    public void cb_pop_stream_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_pop_stream, cbl_pop_stream, txt_pop_stream, lblpopStr.Text, "--Select--");
        educationLevelbase1();
        degree1();
    }

    public void cbl_pop_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_pop_stream, cbl_pop_stream, txt_pop_stream, lblpopStr.Text, "--Select--");
        educationLevelbase1();
        degree1();
    }

    public void ddl_pop_edu_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        degree1();
    }

    public void ddl_pop_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        degree1();
    }

    public void cb_pop_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_pop_degree, cbl_pop_degree, txt_pop_degree, lblpopDeg.Text, "--Select--");
        bindbranch1();
        bindcertificate1();
    }

    public void cbl_pop_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_pop_degree, cbl_pop_degree, txt_pop_degree, lblpopDeg.Text, "--Select--");
        bindbranch1();
        bindcertificate1();
    }

    public void cb_pop_branch_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_pop_branch, cbl_pop_branch, txt_pop_branch, lbl_pop_branch.Text, "--Select--");
    }

    public void cbl_pop_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_pop_branch, cbl_pop_branch, txt_pop_branch, lbl_pop_branch.Text, "--Select--");
    }

    public void cb_pop_certificate_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_pop_certificate, cbl_pop_certificate, txt_pop_certificate, "Certificate", "--Select--");
    }

    public void cbl_pop_certificate_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_pop_certificate, cbl_pop_certificate, txt_pop_certificate, "Certificate", "--Select--");
    }

    public void btn_pop_go_Click(object sender, EventArgs e)
    {
        addgo();
    }

    protected void educationLevelbase1()
    {
        try
        {
            ddl_pop_edu.Items.Clear();
            collegecode = Convert.ToString(ddl_college.SelectedItem.Value);
            string SelectQ = "select distinct Edu_Level  from course  where  college_code='" + collegecode + "'  order by Edu_Level desc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelectQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_pop_edu.DataSource = ds;
                ddl_pop_edu.DataTextField = "Edu_Level";
                ddl_pop_edu.DataValueField = "Edu_Level";
                ddl_pop_edu.DataBind();
            }
        }
        catch { }
    }

    public void loadstream1()
    {
        try
        {
            string stream = "";
            cbl_pop_stream.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + ddl_college.SelectedItem.Value + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_pop_stream.DataSource = ds;
                cbl_pop_stream.DataTextField = "type";
                cbl_pop_stream.DataBind();
                if (cbl_pop_stream.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_pop_stream.Items.Count; i++)
                    {
                        cbl_pop_stream.Items[i].Selected = true;
                    }
                    txt_pop_stream.Text = lblpopStr.Text + "(" + cbl_pop_stream.Items.Count + ")";
                    cb_pop_stream.Checked = true;
                    txt_pop_stream.Enabled = true;
                }
                else
                {
                    txt_pop_stream.Text = "--Select--";
                    cb_pop_stream.Checked = false;
                    txt_pop_stream.Enabled = false;
                }

            }
            else
            {
                txt_pop_stream.Enabled = false;

            }

        }
        catch
        {
        }

    }

    public void degree1()
    {
        try
        {
            string edulvl = Convert.ToString(ddl_pop_edu.SelectedItem.Value);


            string query = "";
            string type = rs.GetSelectedItemsValueAsString(cbl_pop_stream);
            if (type != "")
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddl_college.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "') and type in('" + type + "')";
            }
            else
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddl_college.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_pop_degree.DataSource = ds;
                cbl_pop_degree.DataTextField = "course_name";
                cbl_pop_degree.DataValueField = "course_id";
                cbl_pop_degree.DataBind();

                if (cbl_pop_degree.Items.Count > 0)
                {
                    //    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //    {
                    cbl_pop_degree.Items[0].Selected = true;
                }
                txt_pop_degree.Text = lblpopDeg.Text + "(" + 1 + ")";
                // cb_degree.Checked = true;
                //}
                //else
                //{
                //    txt_degree.Text = "--Select--";
                //    cb_degree.Checked = false;
                //}

                string deg = "";
                if (cbl_pop_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_pop_degree.Items.Count; i++)
                    {

                        if (cbl_pop_degree.Items[i].Selected == true)
                        {
                            string build = cbl_pop_degree.Items[i].Value.ToString();
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

            }
            else
            {
                txt_pop_degree.Text = "--Select--";
                cb_pop_degree.Checked = false;
                cbl_pop_degree.Items.Clear();
                txt_pop_branch.Text = "--Select--";
                cb_pop_branch.Checked = false;
                cbl_pop_branch.Items.Clear();

            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch1()
    {
        try
        {
            string branch = rs.GetSelectedItemsValueAsString(cbl_pop_degree);
            cbl_pop_branch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddl_college.SelectedItem.Value + "' ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddl_college.SelectedItem.Value + "'";
            }
            {
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_pop_branch.DataSource = ds;
                    cbl_pop_branch.DataTextField = "dept_name";
                    cbl_pop_branch.DataValueField = "degree_code";
                    cbl_pop_branch.DataBind();
                    if (cbl_pop_branch.Items.Count > 0)
                    {
                        //    for (int i = 0; i < cbl_branch.Items.Count; i++)
                        //    {
                        cbl_pop_branch.Items[0].Selected = true;
                    }
                    txt_pop_branch.Text = lbl_pop_branch.Text + "(" + 1 + ")";
                    //}

                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    public void bindcertificate1()
    {
        string que = "";
        string courseid = rs.GetSelectedItemsValueAsString(cbl_pop_degree);
        que = "select distinct MasterCode,MasterValue from CO_MasterValues co,CertMasterDet cm where MasterCriteria='CertificateName' and CollegeCode='" + ddl_college.SelectedItem.Value + "' and cm.CertName=co.MasterCode and cm.CourseID in('" + courseid + "')";
        ds.Clear();
        ds = d2.select_method(que, hat, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_pop_certificate.DataSource = ds;
            cbl_pop_certificate.DataTextField = "MasterValue";
            cbl_pop_certificate.DataValueField = "MasterCode";
            cbl_pop_certificate.DataBind();
            if (cbl_pop_certificate.Items.Count > 0)
            {

                cbl_pop_certificate.Items[0].Selected = true;
            }
            txt_pop_certificate.Text = "Certificate(" + 1 + ")";
        }
    }

    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);

        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
    }

    public void addgo()
    {
        try
        {
            int tot_strg = 0;
            int count = 0;
            int count1 = 0;
            int j = 0;
            Fpspread3.Sheets[0].Visible = true;
            Fpspread3.Sheets[0].RowHeader.Visible = false;
            Fpspread3.CommandBar.Visible = false;
            Fpspread3.Sheets[0].AutoPostBack = false;
            Fpspread3.Sheets[0].RowCount = 0;
            Fpspread3.Sheets[0].ColumnHeader.RowCount = 3;
            Fpspread3.Sheets[0].ColumnCount = 6;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            string batch = Convert.ToString(ddl_pop_batch.SelectedItem.Value);
            string adddeg = rs.GetSelectedItemsValueAsString(cbl_pop_branch);
            string courseid = rs.GetSelectedItemsValueAsString(cbl_pop_degree);
            string certificatename = rs.GetSelectedItemsText(cbl_pop_certificate);
            string certificatevalue = rs.GetSelectedItemsValueAsString(cbl_pop_certificate);
            string addsearch = "";
            if (txt_rollno.Text != "")
            {
                if (ddl_rollno.SelectedItem.Value == "1")
                {
                    addsearch = " and r.roll_no='" + txt_rollno.Text + "'";
                }
                else if (ddl_rollno.SelectedItem.Value == "2")
                {
                    addsearch = " and r.Reg_No='" + txt_rollno.Text + "'";
                }
                else if (ddl_rollno.SelectedItem.Value == "2")
                {
                    addsearch = " and a.app_formno='" + txt_rollno.Text + "'";
                }
                else
                {
                    addsearch = " and r.app_no='" + txt_rollno.Text + "'";
                }
            }
            else if (txt_studname.Text != "")
            {
                string[] name = Convert.ToString(txt_studname.Text).Split('-');

                addsearch = " and r.stud_name='" + name[0] + "'";
            }

            if (txt_rollno.Text == "" && txt_studname.Text == "")
            {
                query = "select distinct sd.app_no,sd.isIssued,sd.IsReturn,sd.isIssuedDuplicate,sd.isReturnDuplicate,sd.isOrginal,sd.isDuplicate,r.app_no,a.stud_name,r.roll_no ,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  from Registration r,applyn a, degree d,Department dt,Course C,CertMasterDet cm,StudCertDetails_New sd where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + batch + "') and a.college_code='" + ddl_college.SelectedItem.Value + "' and cm.CourseID in('" + courseid + "') and c.Course_Id=cm.CourseID and d.Course_Id=cm.CourseID and r.App_No=sd.App_no ";
            }
            else
            {
                query = "select distinct sd.app_no,sd.isIssued,sd.IsReturn,sd.isIssuedDuplicate,sd.isReturnDuplicate,sd.isOrginal,sd.isDuplicate,r.app_no,a.stud_name,r.roll_no ,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  from Registration r,applyn a, degree d,Department dt,Course C,CertMasterDet cm,StudCertDetails_New sd where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'  and a.college_code='" + ddl_college.SelectedItem.Value + "'  and c.Course_Id=cm.CourseID and d.Course_Id=cm.CourseID and r.App_No=sd.App_no " + addsearch + "  ";
            }
            query = query + "select * from StudCertDetails_New";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (query == "")
            {
                Fpspread3.Visible = false;
                Fpspread3.Visible = false;
                lbl_pop_error.Visible = true;
                lbl_pop_error.Text = "Kindly Select All List ";
                lbl_headernamespd2.Visible = false;
                btn_save.Visible = false;
                div_save.Visible = false;
                return;
            }
            else
            {
                if (query != "")
                {
                    ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread3.Visible = false;
                        Fpspread3.Visible = false;
                        lbl_pop_error.Visible = true;
                        lbl_pop_error.Text = "No Records Found";
                        lbl_headernamespd2.Visible = false;
                        btn_save.Visible = false;
                        div_save.Visible = false;
                        return;
                    }
                    else
                    {
                        lbl_pop_error.Visible = false;
                        div_save.Visible = false;
                        btn_save.Visible = false;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 0].Text = "S.No";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 1].Text = "Roll No";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 2].Text = "Student Name";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 3].Text = "Batch";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 4].Text = lblpopDeg.Text;
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 5].Text = lbl_pop_branch.Text;
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);
                            int ff = 0;
                            int cc = 5;

                            for (j = 0; j < cbl_pop_certificate.Items.Count; j++)
                            {
                                if (cbl_pop_certificate.Items[j].Selected == true)
                                {

                                    string rec = "";
                                    if (rdo_received.Checked == true)
                                    {
                                        rec = "Received";
                                        lbl_satff.Text = "Received By";

                                    }
                                    else
                                    {
                                        rec = "Issused";
                                        lbl_satff.Text = "Issused By";
                                    }
                                    cc++;
                                    Fpspread3.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), cc].Text = cbl_certificate.Items[j].Text;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = rec;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Orginal Certificate";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isOrginal";
                                    cc++;
                                    Fpspread3.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Duplicate Certificate";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isDuplicate";

                                    cc++;
                                    Fpspread3.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Orginal";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Orginal";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isOrginal";

                                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                    cc++;
                                    Fpspread3.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Duplicate";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isDuplicate";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Duplicate";



                                }
                            }
                            int sss = 6;
                            int ii = 0;
                            for (j = 0; j < cbl_pop_certificate.Items.Count; j++)
                            {
                                if (cbl_pop_certificate.Items[j].Selected == true)
                                {
                                    if (ii == 0)
                                    {
                                        Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, sss, 1, 4);
                                        Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(1, sss, 1, 4);
                                        ii++;
                                    }
                                    else
                                    {
                                        sss = sss + 4;
                                        Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, sss, 1, 4);
                                        Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(1, sss, 1, 4);
                                    }
                                }
                            }

                            ff = cc;
                            FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                            btn.Text = "View";
                            DataView dv = new DataView();
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread3.Sheets[0].RowCount++;
                                count++;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Column.Width = 50;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Column.Width = 250;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Column.Width = 300;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Locked = true;

                                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                cb.AutoPostBack = true;

                                cc = 5;
                                for (int s = cc; s < ff; s++)
                                {
                                    cc++;
                                    string tagtext = Convert.ToString(Fpspread3.Sheets[0].ColumnHeader.Cells[2, cc].Text);
                                    string tag = Convert.ToString(Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                    string[] tt = tag.Split('-');
                                    string statusval = tt[1];
                                    string orgtext = "";

                                    string[] vall = statusval.Split(',');
                                    for (int ss = 0; ss < vall.Length; ss++)
                                    {

                                        ds.Tables[1].DefaultView.RowFilter = " app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + Convert.ToString(tt[0]) + "'";
                                        dv = ds.Tables[1].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            string orginal = Convert.ToString(dv[0][statusval]);
                                            string orginal1 = Convert.ToString(dv[0]["isOrginal"]);
                                            string duplicat1 = Convert.ToString(dv[0]["isDuplicate"]);
                                            if (tagtext == "Orginal Certificate" || tagtext == "Duplicate Certificate")
                                            {
                                                if (orginal == "True")
                                                {
                                                    orgtext = "Yes";
                                                }
                                                else
                                                {
                                                    orgtext = "No";
                                                }

                                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Text = orgtext;
                                            }
                                            else
                                            {
                                                if (rdo_received.Checked == true)
                                                {
                                                    string returnorginal = Convert.ToString(dv[0]["isIssued"]);
                                                    string returndup = Convert.ToString(dv[0]["isIssuedDuplicate"]);
                                                    if (tagtext == "Orginal")
                                                    {
                                                        if ((orginal1 == "True" && (returnorginal == "False" || returnorginal == "")) || (orginal1 == "True" && (returnorginal == "True" || returnorginal == "")))
                                                        {
                                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Value = 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((duplicat1 == "True" && returndup == "False") || (duplicat1 == "True" && returndup == "True"))
                                                        {
                                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Value = 1;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string returnorginal = Convert.ToString(dv[0]["IsReturn"]);
                                                    string returndup = Convert.ToString(dv[0]["isReturnDuplicate"]);
                                                    if (tagtext == "Orginal")
                                                    {
                                                        if ((orginal1 == "True" && (returnorginal == "False" || returnorginal == "")) || (orginal1 == "True" && (returnorginal == "True")))
                                                        {

                                                        }
                                                        else
                                                        {
                                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Value = 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((duplicat1 == "True" && (returndup == "False" || returndup == "")) || (duplicat1 == "True" && returndup == "True"))
                                                        {
                                                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                        }
                                                        else
                                                        {
                                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Value = 1;
                                                        }
                                                    }
                                                }

                                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].CellType = cb;
                                            }
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            if (tagtext == "Orginal Certificate" || tagtext == "Duplicate Certificate")
                                            {
                                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Text = "No";
                                            }
                                            else
                                            {
                                                if (rdo_Issue.Checked == true)
                                                {
                                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].Locked = true;
                                                }
                                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].CellType = cb;
                                            }
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                }

                                getcc = cc;
                            }


                            Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                            Fpspread3.Width = 950;
                            Fpspread3.Height = 420;
                            Fpspread3.Visible = true;
                            div_save.Visible = true;
                            btn_save.Visible = true;
                        }
                    }
                }
            }

        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaff(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select distinct s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code, s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    public void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            query = "";
            string activerow = "";
            string activecol = "";
            string appno = "";
            string val = "";
            int s = 0;
            string checkvalue = "";
            string staffname = Convert.ToString(txt_staffsearch.Text);
            string[] ap1 = staffname.Split('-');
            DateTime datee = new DateTime();
            datee = TextToDate(txt_fromdate);
            string staff_appno = "";
            staff_appno = d2.GetFunction(" select sm.appl_id from staff_appl_master sm,staffmaster s where s.staff_code='" + ap1[3] + "' and s.appl_no=sm.appl_no");
            for (int sel = 0; sel < Fpspread3.Sheets[0].Rows.Count; sel++)
            {
                activerow = Fpspread3.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread3.Sheets[0].ColumnCount.ToString();
                if (Convert.ToInt32(activecol) > 5)
                {
                    for (int col = 1; col < Fpspread3.Sheets[0].Columns.Count; col++)
                    {
                        checkvalue = Convert.ToString(Fpspread3.Sheets[0].Cells[sel, col].Value);
                        if (checkvalue == "1")
                        {
                            if (Fpspread3.Sheets[0].Cells[sel, col].Locked == false)
                            {
                                val = Convert.ToString(Fpspread3.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(sel), Convert.ToInt32(col)].Tag);
                                string[] sp = val.Split('-');
                                string value = sp[1];

                                appno = Convert.ToString(Fpspread3.Sheets[0].Cells[sel, 1].Tag);
                                if (rdo_Issue.Checked == true)
                                {
                                    if (value == "isOrginal")
                                    {
                                        query = " update StudCertDetails_New set isIssued='1', IsReturn='0',isIssuedDuplicate='0',IssuedDate='" + datee + "', Issuedby='" + staff_appno + "' where app_no='" + appno + "' and certificateid='" + sp[0] + "'";
                                    }
                                    else
                                    {
                                        query = " update StudCertDetails_New set isIssuedDuplicate='1', isIssued='0',IsReturn='0',isIssuedDuplicatedate='" + datee + "', isIssuedDuplicateby='" + staff_appno + "' where app_no='" + appno + "' and certificateid='" + sp[0] + "'";
                                    }

                                }
                                else
                                {
                                    if (value == "isOrginal")
                                    {
                                        query = " update StudCertDetails_New set isReturnDuplicate='0',IsReturn='1',isIssued='0', RetrunDate='" + datee + "', Receivedby='" + staff_appno + "' where app_no='" + appno + "' and certificateid='" + sp[0] + "'";
                                    }
                                    else
                                    {
                                        query = " update StudCertDetails_New set isReturnDuplicate='1',isIssued='0',IsReturn='0', isReturnDuplicatedate='" + datee + "', isReturnDuplicateby='" + staff_appno + "' where app_no='" + appno + "' and certificateid='" + sp[0] + "'";
                                    }
                                }
                                s = d2.update_method_wo_parameter(query, "text");
                            }
                        }
                    }
                }

            }
            if (s == 1)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
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

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        if (chosedmode == 0)
        {
            query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        }
        else if (chosedmode == 1)
        {
            query = "select Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%'";
        }
        else if (chosedmode == 2)
        {
            query = "select app_formno from Registration r, applyn a where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.app_no=r.App_No and app_formno like '" + prefixText + "%'";
        }
        else
        {
            query = "select r.App_No from Registration r, applyn a where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.app_no=r.App_No and r.App_No like '" + prefixText + "%'";
        }
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    public void txt_studname_TextChanged(object sender, EventArgs e)
    {
        txt_rollno.Text = "";
        addgo();

    }

    public void txt_rollno_TextChanged(object sender, EventArgs e)
    {
        txt_studname.Text = "";
        addgo();

    }

    public void FpSpread2_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = false;
            Cellclick1 = true;
        }
        catch
        {
        }
    }

    public void FpSpread2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick1 == true)
        {
            //popview.Visible = true;
            //addgocellclik();
        }
    }

    public void addgocellclik()
    {
        try
        {
            int tot_strg = 0;
            int count = 0;
            int count1 = 0;
            int j = 0;
            string activerow = "";
            string activecol = "";
            Fpspread3.Sheets[0].Visible = true;
            Fpspread3.Sheets[0].RowHeader.Visible = false;
            Fpspread3.CommandBar.Visible = false;
            Fpspread3.Sheets[0].AutoPostBack = false;
            Fpspread3.Sheets[0].RowCount = 0;
            Fpspread3.Sheets[0].ColumnHeader.RowCount = 3;
            Fpspread3.Sheets[0].ColumnCount = 6;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string batch = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
            string courseid = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
            string adddeg = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
            string rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            string certificatename = rs.GetSelectedItemsText(cbl_certificate);
            string certificatevalue = rs.GetSelectedItemsValueAsString(cbl_certificate);
            query = "select distinct r.app_no,r.stud_name,r.roll_no ,r.degree_code,r.Batch_Year,r.Current_Semester,C.Course_Name,c.Course_Id ,Dt.Dept_Name  from Registration r,applyn a, degree d,Department dt,Course C,CertMasterDet cm,StudCertDetails_New sd where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + adddeg + "')and r.Batch_Year in('" + batch + "') and a.college_code='" + ddlclg.SelectedItem.Value + "' and cm.CourseID in('" + courseid + "') and c.Course_Id=cm.CourseID and d.Course_Id=cm.CourseID and r.App_No=sd.App_no and r.roll_no ='" + rollno + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (query == "")
            {
                Fpspread3.Visible = false;
                Fpspread3.Visible = false;
                lbl_pop_error.Visible = true;
                lbl_pop_error.Text = "Kindly Select All List ";
                lbl_headernamespd2.Visible = false;
                btn_save.Visible = false;
                div_save.Visible = false;
                return;
            }
            else
            {
                if (query != "")
                {
                    ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread3.Visible = false;
                        Fpspread3.Visible = false;
                        lbl_pop_error.Visible = true;
                        lbl_pop_error.Text = "No Records Found";
                        lbl_headernamespd2.Visible = false;
                        btn_save.Visible = false;
                        div_save.Visible = false;
                        return;
                    }
                    else
                    {
                        lbl_pop_error.Visible = false;
                        div_save.Visible = false;
                        btn_save.Visible = false;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                            Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 0].Text = "S.No";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 1].Text = "Roll No";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 2].Text = "Student Name";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 3].Text = "Batch";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 4].Text = "Course";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
                            Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), 5].Text = "Department";
                            Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);
                            int ff = 0;
                            int cc = 5;

                            for (j = 0; j < cbl_pop_certificate.Items.Count; j++)
                            {
                                if (cbl_pop_certificate.Items[j].Selected == true)
                                {

                                    string rec = "";
                                    if (rdo_received.Checked == true)
                                    {
                                        rec = "Received";
                                    }
                                    else
                                    {
                                        rec = "Issused";
                                    }
                                    cc++;
                                    Fpspread3.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 3), cc].Text = cbl_certificate.Items[j].Text;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 2), cc].Text = rec;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Orginal";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Orginal";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isOrginal";

                                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Locked = true;
                                    cc++;
                                    Fpspread3.Sheets[0].ColumnCount = cc + 1;
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Text = "Duplicate";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Tag = cbl_certificate.Items[j].Value + "-" + "isDuplicate";
                                    Fpspread3.Sheets[0].ColumnHeader.Cells[(Fpspread3.Sheets[0].ColumnHeader.RowCount - 1), cc].Tag = "Duplicate";

                                }
                            }
                            int sss = 6;
                            int ii = 0;
                            for (j = 0; j < cbl_pop_certificate.Items.Count; j++)
                            {
                                if (cbl_pop_certificate.Items[j].Selected == true)
                                {
                                    if (ii == 0)
                                    {
                                        Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, sss, 1, 2);
                                        Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(1, sss, 1, 2);
                                        ii++;
                                    }
                                    else
                                    {
                                        sss = sss + 2;
                                        Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, sss, 1, 2);
                                        Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(1, sss, 1, 2);
                                    }
                                }
                            }

                            ff = cc;
                            DataView dv = new DataView();
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread3.Sheets[0].RowCount++;
                                count++;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Locked = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Column.Width = 300;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 5].Locked = true;

                                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                cb.AutoPostBack = true;

                                cc = 5;
                                for (int s = cc; s < ff; s++)
                                {
                                    cc++;

                                    string tag = Convert.ToString(Fpspread3.Sheets[0].ColumnHeader.Cells[0, cc].Tag);
                                    string[] tt = tag.Split('-');
                                    string statusval = tt[1];

                                    string[] vall = statusval.Split(',');
                                    for (int ss = 0; ss < vall.Length; ss++)
                                    {

                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].CellType = cb; ;
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Center;
                                        string qq = d2.GetFunction("select isOrginal,isDuplicate from StudCertDetails_New where App_no='102978' and CertificateId in('')");
                                    }
                                }
                                getcc = cc;
                            }


                            Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                            Fpspread3.Width = 950;
                            Fpspread3.Height = 420;
                            Fpspread3.Visible = true;
                            div_save.Visible = true;
                            btn_save.Visible = true;
                        }
                    }
                }
            }


        }
        catch
        {
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
            lbl_norec.Visible = false;

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
            txt_excelname.Text = "";
        }

        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }

    }

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "";
            string pagename = "CertificateMasterReport.aspx";

            Printcontrol.loadspreaddetails(Fpspread2, pagename, attendance);


            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void txtexcelname1_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txt_excelname1.Visible = true;
            btn_Excel1.Visible = true;
            btn_printmaster1.Visible = true;
            lbl_reportname1.Visible = true;
            btn_Excel1.Focus();
            if (txt_excelname1.Text == "")
            {
                lbl_norec1.Visible = true;
            }
            else
            {
                lbl_norec1.Visible = false;
            }
            lbl_norec1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }

    public void btn_Excel1_click(object sender, EventArgs e)
    {
        try
        {

            string report = txt_excelname1.Text;
            if (report.ToString().Trim() != "")
            {

                d2.printexcelreport(Fpspread1, report);


                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
            }
            btn_Excel1.Focus();
            txt_excelname1.Text = "";
        }

        catch (Exception ex)
        {
            lbl_norec1.Text = ex.ToString();
        }
        txt_excelname1.Text = "";
    }

    public void btn_printmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "";
            string pagename = "CertificateMasterReport.aspx";

            Printcontrol.loadspreaddetails(Fpspread1, pagename, attendance);

            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        switch (Convert.ToUInt32(ddl_rollno.SelectedItem.Value))
        {
            case 1:
                txt_rollno.Attributes.Add("placeholder", "Roll No");
                chosedmode = 0;
                break;
            case 2:
                txt_rollno.Attributes.Add("placeholder", "Reg No");
                chosedmode = 1;
                break;
            case 3:
                txt_rollno.Attributes.Add("placeholder", "Admin No");
                chosedmode = 2;
                break;
            case 4:
                txt_rollno.Attributes.Add("placeholder", "App No");
                chosedmode = 3;
                break;

        }
    }

    public void BindRoll()
    {
        try
        {
            string[] roll = { "Roll No", "Reg No", "Admin No", "App No" };
            for (int i = 0; i < 9; i++)
            {

                ddl_rollno.Items.Add(new System.Web.UI.WebControls.ListItem(roll[i], Convert.ToString(i + 1)));

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void FpSpread2_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            string activerow1 = "";
            string activecol1 = "";
            string val = "";
            string app_no = "";
            string header = "";
            string certificatid = "";
            Fpspread2.SaveChanges();
            activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
            activerow1 = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol1 = Fpspread1.ActiveSheetView.ActiveColumn.ToString();

            string actrow = e.SheetView.ActiveRow.ToString();
            string actcol = e.SheetView.ActiveColumn.ToString();
            app_no = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(1)].Tag);
            string fileName = string.Empty;
            if (checkkk == "1")
            {
                header = Fpspread1.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(0), Convert.ToInt32(activecol1)].Text;
                if (Convert.ToInt32(activecol1) > 4)
                {
                    certificatid = d2.GetFunction("select MasterCode from CO_MasterValues where MasterValue='" + header + "' and CollegeCode='" + ddl_college.SelectedItem.Value + "'");
                    val = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(7)].Text;
                }


                int ss = Fpspread2.Sheets[0].ColumnCount - 1;
                if (Convert.ToInt32(activecol1) > 4)
                {
                    if (val == "-" || val == "")
                    {
                        lbl_alert.Text = "No Certificate Available";
                        imgdiv2.Visible = true;
                    }
                    else
                    {
                        path1 = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Tag);
                        string strquer = "SELECT FileName,attachdoc,Filetype FROM StudCertDetails_New WHERE App_no='" + app_no + "' and CertificateId='" + certificatid + "' and CertificateNo='" + val + "' and FileName='" + path1 + "'";
                        DataSet dsquery = d2.select_method(strquer, hat, "Text");
                        if (dsquery.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                            {

                                Response.ContentType = dsquery.Tables[0].Rows[i]["Filetype"].ToString();
                                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dsquery.Tables[0].Rows[i]["FileName"] + "\"");
                                Response.BinaryWrite((byte[])dsquery.Tables[0].Rows[i]["attachdoc"]);
                                Response.End();
                                Cellclick = false;
                            }
                        }
                        else
                        {
                            lbl_alert.Text = "File  Not Found ";
                            imgdiv2.Visible = true;
                        }
                    }

                }
                else
                {
                    viewfile.Visible = true;
                    int count = 0;
                    Fpspread4.Sheets[0].Visible = true;
                    Fpspread4.Sheets[0].RowHeader.Visible = false;
                    Fpspread4.CommandBar.Visible = false;
                    Fpspread4.Sheets[0].AutoPostBack = false;
                    Fpspread4.Sheets[0].RowCount = 0;
                    Fpspread4.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread4.Sheets[0].ColumnCount = 5;
                    FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle2.ForeColor = Color.Black;
                    darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                    Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                    string q = "select MasterCode,MasterValue,FileName,Filetype,attachdoc,CertificateNo from StudCertDetails_New s,CO_MasterValues c where App_no='" + app_no + "' and c.MasterCode=s.CertificateId ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q, "Text");

                    if (q == "")
                    {
                        Fpspread4.Visible = false;
                        Label2.Visible = true;
                        Label2.Text = "Kindly Select All List ";
                        btn_viewfileclose.Visible = false;
                        return;
                    }
                    else
                    {
                        if (q != "")
                        {
                            ds = d2.select_method(q, hat, "Text");
                            if (ds.Tables[0].Rows.Count == 0)
                            {

                                Fpspread4.Visible = false;
                                Label2.Visible = true;
                                Label2.Text = "No Records Found";
                                btn_viewfileclose.Visible = false;
                                return;
                            }
                            else
                            {
                                Label2.Visible = false;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                                    btn.Text = "View";
                                    Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                                    Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                                    Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                                    Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 0].Text = "S.No";
                                    Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 1].Text = "Certificate No";
                                    Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 2].Text = "Certificate Name";
                                    Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 3].Text = "File Name";
                                    Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 4].Text = "View";
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        Fpspread4.Sheets[0].RowCount++;
                                        count++;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Tag = app_no;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Locked = true;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["CertificateNo"]);
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 1].Locked = true;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Locked = true;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Locked = true;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Column.Width = 200;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Column.Width = 200;
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["MasterValue"]);
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MasterCode"]);
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["FileName"]);
                                        Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 4].CellType = btn;
                                    }
                                    Fpspread4.Visible = true;
                                    Fpspread4.Width = 700;
                                    btn_viewfileclose.Visible = true;
                                    Fpspread4.Height = 400;
                                }
                            }
                        }
                    }
                }
            }
            else
            {

                viewfile.Visible = true;
                int count = 0;
                Fpspread4.Sheets[0].Visible = true;
                Fpspread4.Sheets[0].RowHeader.Visible = false;
                Fpspread4.CommandBar.Visible = false;
                Fpspread4.Sheets[0].AutoPostBack = false;
                Fpspread4.Sheets[0].RowCount = 0;
                Fpspread4.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread4.Sheets[0].ColumnCount = 5;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                string q = "select MasterCode,MasterValue,FileName,Filetype,attachdoc,CertificateNo from StudCertDetails_New s,CO_MasterValues c where App_no='" + app_no + "' and c.MasterCode=s.CertificateId ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q, "Text");

                if (q == "")
                {
                    Fpspread4.Visible = false;
                    Label2.Visible = true;
                    Label2.Text = "Kindly Select All List ";
                    btn_viewfileclose.Visible = false;
                    return;
                }
                else
                {
                    if (q != "")
                    {
                        ds = d2.select_method(q, hat, "Text");
                        if (ds.Tables[0].Rows.Count == 0)
                        {

                            Fpspread4.Visible = false;
                            Label2.Visible = true;
                            Label2.Text = "No Records Found";
                            btn_viewfileclose.Visible = false;
                            return;
                        }
                        else
                        {
                            Label2.Visible = false;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                                btn.Text = "View";
                                Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                                Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                                Fpspread4.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                                Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 0].Text = "S.No";
                                Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 1].Text = "Certificate No";
                                Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 2].Text = "Certificate Name";
                                Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 3].Text = "File Name";
                                Fpspread4.Sheets[0].ColumnHeader.Cells[(Fpspread4.Sheets[0].ColumnHeader.RowCount - 1), 4].Text = "View";
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    Fpspread4.Sheets[0].RowCount++;
                                    count++;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Tag = app_no;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 0].Locked = true;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["CertificateNo"]);
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 1].Locked = true;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Locked = true;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Locked = true;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Column.Width = 200;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Column.Width = 200;
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["MasterValue"]);
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MasterCode"]);
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["FileName"]);
                                    Fpspread4.Sheets[0].Cells[Fpspread4.Sheets[0].RowCount - 1, 4].CellType = btn;
                                }
                                Fpspread4.Visible = true;
                                Fpspread4.Width = 700;
                                btn_viewfileclose.Visible = true;
                                Fpspread4.Height = 400;
                            }
                        }
                    }
                }
            }

        }
        catch
        {
        }
    }

    public void imgfileviewclose_Click(object sender, EventArgs e)
    {
        viewfile.Visible = false;
    }

    protected void FpSpread4_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            activerow = Fpspread4.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread4.ActiveSheetView.ActiveColumn.ToString();
            string fileName = string.Empty;
            string app_no = Convert.ToString(Fpspread4.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(0)].Tag);
            string certificatid = Convert.ToString(Fpspread4.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(2)].Tag);
            string val = Convert.ToString(Fpspread4.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            path1 = Convert.ToString(Fpspread4.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
            string strquer = "SELECT FileName,attachdoc,Filetype FROM StudCertDetails_New WHERE App_no='" + app_no + "' and CertificateId='" + certificatid + "' and CertificateNo='" + val + "' and FileName='" + path1 + "'";
            DataSet dsquery = d2.select_method(strquer, hat, "Text");
            if (dsquery.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                {

                    Response.ContentType = dsquery.Tables[0].Rows[i]["Filetype"].ToString();
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dsquery.Tables[0].Rows[i]["FileName"] + "\"");
                    Response.BinaryWrite((byte[])dsquery.Tables[0].Rows[i]["attachdoc"]);
                    Response.End();
                    Cellclick = false;
                }
            }
            else
            {
                lbl_alert.Text = "File  Not Found ";
                imgdiv2.Visible = true;
            }
        }
        catch
        {
        }
    }

    public void btn_viewfileclose_clik(object sender, EventArgs e)
    {
        viewfile.Visible = false;
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
        lbl.Add(lblclg);
        fields.Add(0);
        lbl.Add(lblStr);
        fields.Add(1);
        lbl.Add(lbldegree);
        fields.Add(2);
        lbl.Add(lbl_branch);
        fields.Add(3);
        //lbl.Add(lbl_org_sem);
        //fields.Add(4);

        lbl.Add(lbl_collg);
        fields.Add(0);
        lbl.Add(lblpopStr);
        fields.Add(1);
        lbl.Add(lblpopDeg);
        fields.Add(2);
        lbl.Add(lbl_pop_branch);
        fields.Add(3);


        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

}