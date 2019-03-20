using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using System.Configuration;

public partial class StudentMod_CertificateEntry : System.Web.UI.Page
{
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    DataSet dscol = new DataSet();
    Hashtable grandtotal = new Hashtable();
    string batch2 = "";
    string degree = "";
    int i;
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
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            sessstream = Convert.ToString(Session["streamcode"]);
            //  lbl_str1.Text = sessstream;
            //lbl_str2.Text = sessstream;
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                usercode = Session["group_code"].ToString();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                usercode = Session["usercode"].ToString();
            }
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    #region college
    public void loadcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddl_collegename.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            loadstrm();
            bindBtch();
            binddeg();
            binddept();
            divGrid.Visible = false;


        }
        catch
        {
        }
    }
    #endregion

    #region stream

    public void loadstrm()
    {
        try
        {
            cbl_stream.Items.Clear();
            cb_stream.Checked = false;
            txt_stream.Text = "---Select---";
            ds.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode1 + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_stream.DataSource = ds;
                cbl_stream.DataTextField = "type";
                cbl_stream.DataValueField = "type";
                cbl_stream.DataBind();
            }
            if (cbl_stream.Items.Count > 0)
            {
                for (i = 0; i < cbl_stream.Items.Count; i++)
                {
                    cbl_stream.Items[i].Selected = true;
                }
                txt_stream.Text = "Stream(" + cbl_stream.Items.Count + ")";
                cb_stream.Checked = true;
            }


        }
        catch
        {

        }
    }
    protected void cb_stream_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_stream, cbl_stream, txt_stream, "Stream", "--Select--");
            bindBtch();
            binddeg();
            binddept();
            divGrid.Visible = false;

        }
        catch { }
    }
    protected void cbl_stream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_stream, cbl_stream, txt_stream, "Stream", "--Select--");
            bindBtch();
            binddeg();
            binddept();
            divGrid.Visible = false;

        }
        catch { }
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
                    for (i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddeg();
            binddept();
            divGrid.Visible = false;


        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddeg();
            binddept();
            divGrid.Visible = false;
        }
        catch { }
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
            string stream = "";
            stream = Convert.ToString(getCblSelectedText(cbl_stream));
            //if (cbl_stream.Items.Count > 0)
            //{
            //    if (cbl_stream.SelectedItem.Text != "")
            //    {
            //        stream = cbl_stream.SelectedItem.Text.ToString();
            //    }
            //}

            cbl_degree.Items.Clear();
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            ds.Reset();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
            divGrid.Visible = false;

        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
            divGrid.Visible = false;

        }
        catch { }
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
            batch2 = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch2 == "")
                    {
                        batch2 = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch2 += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            string collegecode = ddl_collegename.SelectedItem.Value.ToString();
            if (batch2 != "" && degree != "")
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
                        for (i = 0; i < cbl_dept.Items.Count; i++)
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
        try
        {
            CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            divGrid.Visible = false;
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
            divGrid.Visible = false;
        }
        catch { }
    }
    #endregion

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

        lbl.Add(lbl_collegename);
        lbl.Add(lbl_str1);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);

        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

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

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            btnsave.Visible = false;
            ds = getdetailscertificatereport();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                RollAndRegSettings();
                loadgrid(ds);
            }
            else
            {

                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);

            }
        }
        catch { }
    }

    private DataSet getdetailscertificatereport()
    {

        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string batch = string.Empty;
            string degree = string.Empty;
            string stream = string.Empty;
            string branch = string.Empty;


            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            if (cbl_stream.Items.Count > 0)
                stream = Convert.ToString(getCblSelectedValue(cbl_stream));
            if (cbl_batch.Items.Count > 0)
                batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            if (cbl_degree.Items.Count > 0)
                degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            //if (cbl_dept.Items.Count > 0)
            //    branch = Convert.ToString(getCblSelectedValue(cbl_dept));

            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(stream) && !string.IsNullOrEmpty(degree))
            {
                //selQ = " select distinct r.App_no, cast(r.Roll_No as varchar(100)) as Roll_No,cast(r.Reg_No as varchar(100)) as Reg_No,cast(r.Roll_Admit as varchar(100)) as Roll_Admit,r.Stud_Name,(select top 1 CAST(registration_no as varchar(25))+'/'+cast(upper(convert(varchar(3),DateAdd(month,cast(passmonth as int),-1))) as varchar(50))+' '+cast(passyear as varchar(50)) from stud_prev_details sp where sp.app_no=r.app_no and ISNULL(registration_no,'')<>'' and ISNULL(passmonth,'0')<>'0' and ISNULL(passyear,'0')<>'0' and sp.app_no=a.app_no and r.App_No=a.app_no) as YearSession,(select top 1 cast(sum(acual_marks) as varchar)+'/'+ cast(sum(max_marks) as varchar) acual_marks from perv_marks_history ph where course_entno in(select course_entno from Stud_prev_details sp where app_no=ph.app_no and r.App_No=sp.app_no and ph.app_no=r.App_No and ph.app_no=a.app_no and sp.app_no=a.app_no) group by ph.registerno, ph.pass_month,ph.pass_year,ph.app_no) as TotalMark,sp.marksheetno from Registration r,applyn a left join Stud_prev_details sp on sp.app_no=a.app_no where r.App_No=a.app_no and  r.college_code='" + collegecode + "'  and r.degree_code in ('" + degree + "') and  r.Batch_Year in('" + batch + "')and DelFlag=0 and CC=0 and Exam_Flag<>'Debar' order by r.App_No,Roll_No,Reg_No,Roll_Admit,r.Stud_Name";
                selQ = " select distinct r.App_no, cast(r.Roll_No as varchar(100)) as Roll_No,cast(r.Reg_No as varchar(100)) as Reg_No,cast(r.Roll_Admit as varchar(100)) as Roll_Admit,r.Stud_Name,(select top 1 CAST(registration_no as varchar(25))+'/'+cast(upper(convert(varchar(3),DateAdd(month,cast(passmonth as int),-1))) as varchar(50))+' '+cast(passyear as varchar(50)) from stud_prev_details sp where sp.app_no=r.app_no and ISNULL(registration_no,'')<>'' and ISNULL(passmonth,'0')<>'0' and ISNULL(passyear,'0')<>'0' and sp.app_no=a.app_no and r.App_No=a.app_no) as YearSession,(select top 1 cast(sum(acual_marks) as varchar)+'/'+ cast(sum(max_marks) as varchar) acual_marks from perv_marks_history ph where course_entno in(select course_entno from Stud_prev_details sp where r.App_No=sp.app_no and sp.app_no=a.app_no) group by ph.registerno, ph.pass_month,ph.pass_year,ph.app_no) as TotalMark,sp.marksheetno from Registration r,applyn a left join Stud_prev_details sp on sp.app_no=a.app_no where r.App_No=a.app_no and  r.college_code='" + collegecode + "'  and r.degree_code in ('" + degree + "') and  r.Batch_Year in('" + batch + "')and DelFlag=0 and CC=0 and Exam_Flag<>'Debar' order by r.App_No,Roll_No,Reg_No,Roll_Admit,r.Stud_Name";


                //selQ = "select distinct r.App_no, r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,CONVERT(varchar(20),r.Batch_Year)+'-'+c.Course_Name+'-'+dt.dept_acronym+'-'+CONVERT(varchar(20),r.Current_Semester) as deptname,sp.marksheetno  from applyn a,Course c,Degree dg,Department dt,Registration r left join Stud_prev_details sp on sp.app_no=r.App_No where r.App_No=a.app_no and r.college_code='" + collegecode + "' and r.degree_code in ('" + degree + "') and dg.Degree_Code=r.degree_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and  r.Batch_Year in('" + batch + "')and DelFlag=0 and CC=0 and Exam_Flag<>'Debar' order by r.App_No,r.Roll_No,r.Reg_No,r.Roll_Admit";

                //select distinct r.App_no, r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,CONVERT(varchar(20),r.Batch_Year)+'-'+c.Course_Name+'-'+dt.dept_acronym+'-'+CONVERT(varchar(20),r.Current_Semester) as deptname,sp.marksheetno  from applyn a,Course c,Degree dg,Department dt,Registration r left join Stud_prev_details sp on sp.app_no=r.App_No where r.App_No=a.app_no and r.college_code='" + collegecode + "' and r.degree_code in ('" + degree + "') and dg.Degree_Code=r.degree_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and  r.Batch_Year in('" + batch + "')and DelFlag=0 and CC=0 and Exam_Flag<>'Debar' order by r.App_No,r.Roll_No,r.Reg_No,r.Roll_Admit


                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");

            }
            #endregion
        }
        catch (Exception ex)
        { }

        return dsload;
    }

    private void loadgrid(DataSet ds)
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("appno");
            dt.Columns.Add("Student Name");
            dt.Columns.Add("Roll No");
            dt.Columns.Add("Reg No");
            dt.Columns.Add("Admission No");
            dt.Columns.Add("YearSession");
            dt.Columns.Add("TotalMark");
            dt.Columns.Add("12th Certificate No");

            DataRow drow;
            int rowcount = 0;

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                drow = dt.NewRow();

                drow["SNo"] = Convert.ToString(++rowcount);
                drow["appno"] = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                drow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);
                drow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]);
                drow["Admission No"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                drow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]);
                drow["YearSession"] = Convert.ToString(ds.Tables[0].Rows[row]["YearSession"]);
                drow["TotalMark"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalMark"]);
                drow["12th Certificate No"] = Convert.ToString(ds.Tables[0].Rows[row]["marksheetno"]);


                dt.Rows.Add(drow);
            }
            if (dt.Rows.Count > 0)
            {

                grid_Details.DataSource = dt;
                grid_Details.DataBind();
                divGrid.Visible = true;
                btnsave.Visible = true;
            }

        }

        catch
        {
        }

    }

    protected void gridReport_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            e.Row.Cells[0].Width = 50;
            e.Row.Cells[1].Width = 100;
            e.Row.Cells[2].Width = 100;
            e.Row.Cells[3].Width = 100;
            //e.Row.Cells[4].Width = 300;
            //e.Row.Cells[5].Width = 300;
            //e.Row.Cells[6].Width = 300;



            #region
            if (roll == 0)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;


            }
            else if (roll == 1)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;


            }
            else if (roll == 2)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;


            }
            else if (roll == 3)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;


            }
            else if (roll == 4)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = true;


            }
            else if (roll == 5)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;

            }
            else if (roll == 6)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;


            }
            else if (roll == 7)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = true;


            }
            #endregion
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

            #region
            if (roll == 0)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;


            }
            else if (roll == 1)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;


            }
            else if (roll == 2)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;

            }
            else if (roll == 3)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;


            }
            else if (roll == 4)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = true;


            }
            else if (roll == 5)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;


            }
            else if (roll == 6)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;


            }
            else if (roll == 7)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = true;

            }
            #endregion
        }
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolCheck = false;
            foreach (GridViewRow row in grid_Details.Rows)
            {
                Label lbsno = (Label)row.FindControl("lbl_sno");
                TextBox lbl_CertificateNo = (TextBox)row.FindControl("txt_CertificateNo");
                string Certificateno = string.Empty;
                string strcertificateno = string.Empty;
                Certificateno = lbl_CertificateNo.Text;
                if (Certificateno != "0" && !string.IsNullOrEmpty(Certificateno))
                {
                    strcertificateno += "update Stud_prev_details  set marksheetno='" + lbl_CertificateNo.Text + "' where app_no='" + lbsno.Text + "';";
                    int updaVal = d2.update_method_wo_parameter(strcertificateno, "Text");
                    if (updaVal > 0)
                        boolCheck = true;
                }
            }
            if (boolCheck)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
            }
        }
        catch
        {
        }
    }

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
    #endregion

}