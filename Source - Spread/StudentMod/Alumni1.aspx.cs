using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;

public partial class Alumni1 : System.Web.UI.Page
{
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
    ReuasableMethods rs = new ReuasableMethods();
    private string usercode;
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
            // collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            sessstream = Convert.ToString(Session["streamcode"]);
            if (!IsPostBack)
            {
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                    collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                bindsec();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                getPrintSettings();
            }
        }
        catch (Exception ex) { }
    }

    #region college
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ddl_collegename.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
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
    { }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }
    #endregion
   
    #region stream

    public void loadstrm()
    {
        try
        {
            ddlstream.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.Enabled = true;
            }
            else
            {
                ddlstream.Enabled = false;
            }
            binddeg();
        }
        catch
        { }
    }
    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            string stream = ddlstream.SelectedItem.Text.ToString();
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type  in('" + stream + "') and d.college_code='" + clgvalue + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Course_Name";
                cbl_degree.DataValueField = "Course_Id";
                cbl_degree.DataBind();
            }
            for (int j = 0; j < cbl_degree.Items.Count; j++)
            {
                cbl_degree.Items[j].Selected = true;
                cb_degree.Checked = true;
            }

            txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
            binddept();
            bindsec();
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
            //   ds = d2.BindBatch();
            string selqry = "select distinct batch_year from Registration where CC=1  and college_code='" + collegecode + "' order by batch_year desc";
            ds = d2.select_method_wo_parameter(selqry, "Text");
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
            bindsec();
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
            bindsec();
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
            if (ddlstream.Items.Count > 0)
            {
                if (ddlstream.SelectedItem.Text != "")
                {
                    stream = ddlstream.SelectedItem.Text.ToString();
                }
            }

            cbl_degree.Items.Clear();
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            ds.Clear();
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
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
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
            bindsec();
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
            bindsec();
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
            string batch2 = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
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

            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
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
        try
        {
            CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            bindsec();
            // bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
            bindsec();
            //  bindsem();
        }
        catch { }
    }
    #endregion
    
    #region sec
    public void bindsec()
    {
        try
        {
            string batch2 = "";
            string strbranch = "";
            cbl_sect.Items.Clear();
            txt_sect.Text = "---Select---";
            cb_sect.Checked = false;
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();

            if (cbl_batch.Items.Count > 0)
                batch2 = rs.getCblSelectedValue(cbl_batch);
            if (cbl_dept.Items.Count > 0)
                strbranch = rs.getCblSelectedValue(cbl_dept);

            if (clgvalue != "")
            {
                //ds = d2.BindSectionDetailmult(clgvalue);
                string sec = "select distinct isnull(sections,'') as sections  from registration where batch_year in('" + batch2 + "') and degree_code in('" + strbranch + "')  and college_code='" + clgvalue + "'   and delflag=0 and exam_flag<>'Debar' order by sections";//and sections is not null and ltrim(sections)<>''  and sections<>'-1' 
                ds.Clear();
                ds = d2.select_method_wo_parameter(sec, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sect.DataSource = ds;
                    cbl_sect.DataTextField = "sections";
                    cbl_sect.DataValueField = "sections";
                    cbl_sect.DataBind();
                    if (cbl_sect.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sect.Items.Count; row++)
                        {
                            cbl_sect.Items[row].Selected = true;
                        }
                        txt_sect.Text = "Section(" + cbl_sect.Items.Count + ")";
                        cb_sect.Checked = true;
                    }

                }
            }
            else
            {
                cb_sect.Checked = false;
                txt_sect.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }
    protected void cb_sect_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sect, cbl_sect, txt_sect, "Section", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sect, cbl_sect, txt_sect, "Section", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    #endregion
    
    public void btngo_Click(object sender, EventArgs e)
    {

        ds = getdetailsalumnireport();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadspread(ds);
        }
        else
        {

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);

        }


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
    
    private DataSet getdetailsalumnireport()
    {

        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string batch = string.Empty;
            string degree = string.Empty;
            string sec = string.Empty;

            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            if (cbl_batch.Items.Count > 0)
                batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            if (cbl_dept.Items.Count > 0)
                degree = Convert.ToString(getCblSelectedValue(cbl_dept));

            if (cbl_sect.Items.Count > 0)
                sec = Convert.ToString(rs.getCblSelectedValue(cbl_sect));


            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree))
            {
                selQ = " select r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.batch_year,(select c.type from degree d,course c,department dt where d.course_id =c.course_id and d.dept_code=dt.dept_code and d.college_code=r.college_code and r.degree_code=d.degree_code)as course,(select c.course_name from degree d,course c,department dt where d.course_id =c.course_id and d.dept_code=dt.dept_code and d.college_code=r.college_code and r.degree_code=d.degree_code)as Branch,(select dt.dept_name from degree d,course c,department dt where d.course_id =c.course_id and d.dept_code=dt.dept_code and d.college_code=r.college_code and r.degree_code=d.degree_code) as deptname,case when param_1=0 then 'UnSatisfactory' when param_1=1 then 'Satisfactory' when param_1=2 then 'Good' when param_1=3 then 'Very Good' end param_1,case when param_2=0 then 'UnSatisfactory' when param_2=1 then 'Satisfactory' when param_2=2 then 'Good' when param_2=3 then 'Very Good' end param_2,case when param_3=0 then 'UnSatisfactory' when param_3=1 then 'Satisfactory' when param_3=2 then 'Good' when param_3=3 then 'Very Good' end param_3,case when param_4=0 then 'UnSatisfactory' when param_4=1 then 'Satisfactory' when param_4=2 then 'Good' when param_4=3 then 'Very Good' end param_4,case when param_5=0 then 'UnSatisfactory' when param_5=1 then 'Satisfactory' when param_5=2 then 'Good' when param_5=3 then 'Very Good' end param_5,case when param_6=0 then 'UnSatisfactory' when param_6=1 then 'Satisfactory' when param_6=2 then 'Good' when param_6=3 then 'Very Good' end param_6,case when param_7=0 then 'UnSatisfactory' when param_7=1 then 'Satisfactory' when param_7=2 then 'Good' when param_7=3 then 'Very Good' end param_7,case when param_8=0 then 'UnSatisfactory' when param_8=1 then 'Satisfactory' when param_8=2 then 'Good' when param_8=3 then 'Very Good' end param_8 from registration r,applyn a,studentfeedback s where r.app_no=a.app_no and r.app_no=s.app_no and a.app_no=s.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in ('" + degree + "') and isnull(r.sections,'') in('" + sec + "') and cc=1 and (isalumni=1 or isalumni=0)";
                if (cbdate.Checked)
                    selQ += "   and AlumnregisterDate between'" + fromdate + "' and '" + todate + "' ";

                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");

            }
            #endregion
        }
        catch (Exception ex)
        { }

        return dsload;
    }
    
    //private void loadgrid(DataSet ds)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt.Columns.Add("SNo");
    //        dt.Columns.Add("Roll No");
    //        dt.Columns.Add("Reg No");
    //        dt.Columns.Add("Addmission No");
    //        dt.Columns.Add("Student Name");
    //        dt.Columns.Add("Course");
    //        dt.Columns.Add("Stream");
    //        dt.Columns.Add("BatchYear");
    //        dt.Columns.Add("Branch");
    //        dt.Columns.Add("Department");
    //        dt.Columns.Add("Q1");
    //        dt.Columns.Add("Q2");
    //        dt.Columns.Add("Q3");
    //        dt.Columns.Add("Q4");
    //        dt.Columns.Add("Q5");
    //        dt.Columns.Add("Q6");
    //        dt.Columns.Add("Q7");
    //        dt.Columns.Add("Q8");
    //        DataRow drow;
    //        int rowcount = 0;
    //        string stream = Convert.ToString(ddlstream.SelectedItem.Text);
    //        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
    //        {
    //            drow = dt.NewRow();
    //            drow["SNo"] = Convert.ToString(++rowcount);
    //            drow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);
    //            drow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]);
    //            drow["Addmission No"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
    //            drow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]);
    //            drow["Course"] = Convert.ToString(ds.Tables[0].Rows[row]["course"]);
    //            drow["Stream"] = stream;
    //            drow["BatchYear"] = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
    //            drow["Branch"] = Convert.ToString(ds.Tables[0].Rows[row]["Branch"]);
    //            drow["Department"] = Convert.ToString(ds.Tables[0].Rows[row]["deptname"]);
    //            drow["Q1"] = Convert.ToString(ds.Tables[0].Rows[row]["param_1"]);
    //            drow["Q2"] = Convert.ToString(ds.Tables[0].Rows[row]["param_2"]);
    //            drow["Q3"] = Convert.ToString(ds.Tables[0].Rows[row]["param_3"]);
    //            drow["Q4"] = Convert.ToString(ds.Tables[0].Rows[row]["param_4"]);
    //            drow["Q5"] = Convert.ToString(ds.Tables[0].Rows[row]["param_5"]);
    //            drow["Q6"] = Convert.ToString(ds.Tables[0].Rows[row]["param_6"]);
    //            drow["Q7"] = Convert.ToString(ds.Tables[0].Rows[row]["param_7"]);
    //            drow["Q8"] = Convert.ToString(ds.Tables[0].Rows[row]["param_8"]);
    //            dt.Rows.Add(drow);
    //        }

    //        if (dt.Rows.Count > 0)
    //        {
    //            grid_Details.DataSource = dt;
    //            grid_Details.DataBind();
    //            divGrid.Visible = true;
    //        }

    //    }
    //    catch
    //    {
    //    }
    //}
    
    private void loadspread(DataSet ds)
    {
        try
        {
            RollAndRegSettings();
            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("Roll No");
            dt.Columns.Add("Reg No");
            dt.Columns.Add("Admission No");
            dt.Columns.Add("Student Name");
            //dt.Columns.Add("Course");
            dt.Columns.Add("BatchYear");
            dt.Columns.Add("Stream");
            dt.Columns.Add("Branch");
            dt.Columns.Add("Department");
            dt.Columns.Add("Q1");
            dt.Columns.Add("Q2");
            dt.Columns.Add("Q3");
            dt.Columns.Add("Q4");
            dt.Columns.Add("Q5");
            dt.Columns.Add("Q6");
            dt.Columns.Add("Q7");
            dt.Columns.Add("Q8");
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            bool boolroll = false;
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            for (int row = 0; row < dt.Columns.Count; row++)
            {

                spreadDet.Sheets[0].ColumnCount++;
                string col = Convert.ToString(dt.Columns[row].ColumnName);
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                switch (col)
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[row].Width = 150;
                        admNo = Convert.ToInt32(row);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[row].Width = 110;
                        rollNo = Convert.ToInt32(row);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[row].Width = 110;
                        regNo = Convert.ToInt32(row);
                        boolroll = true;
                        break;
                    //case "Batch Year":
                    //   spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //    break;

                }

            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);

            string stream = Convert.ToString(ddlstream.SelectedItem.Text);
            DataRow drow;
            int rowcount = 0;

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                spreadDet.Sheets[0].RowCount++;
                for (int col = 0; col < dt.Columns.Count; col++)
                {

                    if (col == 0)
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(++rowcount);
                    else
                    {
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][col - 1]);
                    }
                }

            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            divspread.Visible = true;
            print.Visible = true;
        }

        catch { }
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


    protected void spreadColumnVisible(int rollNo, int regNo, int admNo)
    {
        try
        {
            #region
            if (roll == 0)
            {
                spreadDet.Columns[rollNo].Visible = true;
                spreadDet.Columns[regNo].Visible = true;
                spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 1)
            {

                spreadDet.Columns[rollNo].Visible = true;
                spreadDet.Columns[regNo].Visible = true;
                spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 2)
            {

                spreadDet.Columns[rollNo].Visible = true;
                spreadDet.Columns[regNo].Visible = false;
                spreadDet.Columns[admNo].Visible = false;

            }
            else if (roll == 3)
            {

                spreadDet.Columns[rollNo].Visible = false;
                spreadDet.Columns[regNo].Visible = true;
                spreadDet.Columns[admNo].Visible = false;
            }
            else if (roll == 4)
            {

                spreadDet.Columns[rollNo].Visible = false;
                spreadDet.Columns[regNo].Visible = false;
                spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 5)
            {

                spreadDet.Columns[rollNo].Visible = true;
                spreadDet.Columns[regNo].Visible = true;
                spreadDet.Columns[admNo].Visible = false;
            }
            else if (roll == 6)
            {

                spreadDet.Columns[rollNo].Visible = false;
                spreadDet.Columns[regNo].Visible = true;
                spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 7)
            {

                spreadDet.Columns[rollNo].Visible = true;
                spreadDet.Columns[regNo].Visible = false;
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
                // lblvalidation1.Visible = false;
            }
            else
            {
                // lblvalidation1.Text = "Please Enter Your  Report Name";
                //  lblvalidation1.Visible = true;
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
            degreedetails = "Student Alumni  Report";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "Alumni1.aspx";
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
}