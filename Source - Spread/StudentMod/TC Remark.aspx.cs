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

public partial class StudentMod_TC_Remark : System.Web.UI.Page
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
                setLabelText();
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                bindRemark();
                bindconduct();

            }
            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
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
    {

    }
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
            //bindsec();
            // bindsem();
            divGrid.Visible = false;
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
            //bindsec();
            //  bindsem();
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
        //  lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        //  fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Remark";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;
    }

    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_tccertificateissuedate.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_tccertificateissuedate.SelectedItem.Value.ToString() + "' and MasterCriteria='remarks_tc' and collegecode='" + ddl_collegename.SelectedValue + "'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully";
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Selected";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
            }
            bindRemark();
        }
        catch { }
    }

    protected void btnplus1_Click(object sender, EventArgs e)
    {
        lbl_addgroup.Text = "Conduct"; txt_addgroup.Attributes.Add("maxlength", "10");
        txt_addgroup.Attributes.Add("placeholder", "");
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lblerror.Visible = false;
    }

    protected void btnminus1_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_generalconduct.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_generalconduct.SelectedItem.Value.ToString() + "' and MasterCriteria='conduct_tc' and collegecode='" + ddl_collegename.SelectedValue + "'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully";
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Selected";
                }
                bindconduct();
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
            }

        }
        catch { }
    }

    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);
            group = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(group);
            if (txt_addgroup.Text != "")
            {
                int insert = 0;
                if (lbl_addgroup.Text.Trim() == "Remark")
                {
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='remarks_tc' and CollegeCode='" + collegecode + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='remarks_tc' and CollegeCode='" + collegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','remarks_tc','" + collegecode + "')";
                    insert = d2.update_method_wo_parameter(sql, "Text");
                }
                else
                {
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='conduct_tc' and CollegeCode='" + collegecode + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='conduct_tc' and CollegeCode='" + collegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','conduct_tc','" + collegecode + "')";
                    insert = d2.update_method_wo_parameter(sql, "Text");
                }
                if (insert != 0)
                {
                    if (lbl_addgroup.Text.Trim() == "Remark")
                        bindRemark();
                    else
                        bindconduct();

                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved Successfully";
                    txt_addgroup.Text = "";
                    plusdiv.Visible = false;
                    panel_addgroup.Visible = false;
                }
                txt_addgroup.Text = "";
            }
            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Enter the Remark";
            }
        }
        catch
        {
        }
    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    protected void bindRemark()
    {
        try
        {
            ddl_tccertificateissuedate.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='remarks_tc' and CollegeCode ='" + ddl_collegename.SelectedValue + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_tccertificateissuedate.DataSource = ds;
                ddl_tccertificateissuedate.DataTextField = "MasterValue";
                ddl_tccertificateissuedate.DataValueField = "MasterCode";
                ddl_tccertificateissuedate.DataBind();
            }
            //ddl_tccertificateissuedate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch { }
    }

    protected void bindconduct()
    {
        try
        {
            ddl_generalconduct.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='conduct_tc' and CollegeCode ='" + ddl_collegename.SelectedValue + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_generalconduct.DataSource = ds;
                ddl_generalconduct.DataTextField = "MasterValue";
                ddl_generalconduct.DataValueField = "MasterCode";
                ddl_generalconduct.DataBind();
            }
            //ddl_generalconduct.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch { }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        btnsave.Visible = false;
        chkGridSelectAll.Visible = true;
        ds = getdetailstcreport();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadgrid(ds);
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
    
    private DataSet getdetailstcreport()
    {

        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string batch = string.Empty;
            string degree = string.Empty;

            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            if (cbl_batch.Items.Count > 0)
                batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            if (cbl_dept.Items.Count > 0)
                degree = Convert.ToString(getCblSelectedValue(cbl_dept));





            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree))
            {
                selQ = "select r.App_no, r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,a.remarks,(select distinct t.General_conduct  from  tc_details t where t.App_no=r.App_No ) as conduct from Registration r,applyn a where r.App_No=a.app_no and r.college_code='" + collegecode + "' and r.degree_code in ('" + degree + "') and  r.Batch_Year in('" + batch + "')";


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
            dt.Columns.Add("Name");
            dt.Columns.Add("Roll No");
            dt.Columns.Add("Reg No");
            dt.Columns.Add("Admission No");
            dt.Columns.Add("Remark");
            dt.Columns.Add("Conduct");


            DataRow drow;
            int rowcount = 0;
            string stream = Convert.ToString(ddlstream.SelectedItem.Text);



            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                drow = dt.NewRow();

                drow["SNo"] = Convert.ToString(++rowcount);
                drow["appno"] = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                drow["Name"] = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]);
                drow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);
                drow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]);
                drow["Admission No"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                drow["Remark"] = Convert.ToString(ds.Tables[0].Rows[row]["remarks"]);
                drow["Conduct"] = Convert.ToString(ds.Tables[0].Rows[row]["conduct"]);

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
                grid_Details.Columns[rollNo].Visible = true;
                grid_Details.Columns[regNo].Visible = true;
                grid_Details.Columns[admNo].Visible = true;
            }
            else if (roll == 1)
            {

                grid_Details.Columns[rollNo].Visible = true;
                grid_Details.Columns[regNo].Visible = true;
                grid_Details.Columns[admNo].Visible = true;
            }
            else if (roll == 2)
            {

                grid_Details.Columns[rollNo].Visible = true;
                grid_Details.Columns[regNo].Visible = false;
                grid_Details.Columns[admNo].Visible = false;

            }
            else if (roll == 3)
            {

                grid_Details.Columns[rollNo].Visible = false;
                grid_Details.Columns[regNo].Visible = true;
                grid_Details.Columns[admNo].Visible = false;
            }
            else if (roll == 4)
            {

                grid_Details.Columns[rollNo].Visible = false;
                grid_Details.Columns[regNo].Visible = false;
                grid_Details.Columns[admNo].Visible = true;
            }
            else if (roll == 5)
            {

                grid_Details.Columns[rollNo].Visible = true;
                grid_Details.Columns[regNo].Visible = true;
                grid_Details.Columns[admNo].Visible = false;
            }
            else if (roll == 6)
            {

                grid_Details.Columns[rollNo].Visible = false;
                grid_Details.Columns[regNo].Visible = true;
                grid_Details.Columns[admNo].Visible = true;
            }
            else if (roll == 7)
            {

                grid_Details.Columns[rollNo].Visible = true;
                grid_Details.Columns[regNo].Visible = false;
                grid_Details.Columns[admNo].Visible = true;
            }
            #endregion
        }
        catch { }
    }

    #endregion

    //protected void grid_Details_OnRowDataBound(object sender, EventArgs e)
    //{
    //    if (grid_Details.Rows.Count>0)
    //    {

    //        if (ddlreport.Items.Count > 0)
    //        {
    //            string strText = Convert.ToString(ddlreport.SelectedItem.Text);
    //            string strVal = Convert.ToString(ddlreport.SelectedItem.Text);
    //        }

    //    }    
    //}
    protected void grid_Details_DataBound(object sender, EventArgs e)
    {
        try
        {

            (grid_Details.Rows[0].FindControl("ddl_Remark") as DropDownList).Items.Clear();
            (grid_Details.Rows[0].FindControl("ddl_Conduct") as DropDownList).Items.Clear();

            if (grid_Details.Rows.Count > 0)
            {
                string strRemark = string.Empty;
                string strConduct = string.Empty;



                for (int a = 0; a < grid_Details.Rows.Count; a++)
                {
                    //if ((grid_Details.Rows[a].FindControl("lbl_cb") as CheckBox).Checked)
                    // continue;
                    if (ddl_tccertificateissuedate.Items.Count > 0)
                        strRemark = Convert.ToString(ddl_tccertificateissuedate.SelectedValue);

                    if (ddl_generalconduct.Items.Count > 0)
                        strConduct = Convert.ToString(ddl_generalconduct.SelectedValue);

                    string linkCriteria = string.Empty;
                    string linkCriteria1 = string.Empty;
                    linkCriteria = "remarks_tc";
                    string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='" + linkCriteria + "' and CollegeCode='" + ddl_collegename.SelectedValue + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (grid_Details.Rows[a].FindControl("ddl_Remark") as DropDownList).DataSource = ds;
                        (grid_Details.Rows[a].FindControl("ddl_Remark") as DropDownList).DataTextField = "MasterValue";
                        (grid_Details.Rows[a].FindControl("ddl_Remark") as DropDownList).DataValueField = "MasterCode";
                        (grid_Details.Rows[a].FindControl("ddl_Remark") as DropDownList).DataBind();

                        string strcode = Convert.ToString((grid_Details.Rows[a].FindControl("lbl_remark") as Label).Text);
                        if (!string.IsNullOrEmpty(strcode))
                            strRemark = strcode;

                        (grid_Details.Rows[a].FindControl("ddl_Remark") as DropDownList).SelectedIndex = (grid_Details.Rows[a].FindControl("ddl_Remark") as DropDownList).Items.IndexOf((grid_Details.Rows[a].FindControl("ddl_Remark") as DropDownList).Items.FindByValue(strRemark));
                    }

                    linkCriteria1 = "conduct_tc";
                    string query1 = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='" + linkCriteria1 + "' and CollegeCode='" + ddl_collegename.SelectedValue + "'";
                    ds.Reset();
                    ds = d2.select_method_wo_parameter(query1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (grid_Details.Rows[a].FindControl("ddl_Conduct") as DropDownList).DataSource = ds;
                        (grid_Details.Rows[a].FindControl("ddl_Conduct") as DropDownList).DataTextField = "MasterValue";
                        (grid_Details.Rows[a].FindControl("ddl_Conduct") as DropDownList).DataValueField = "MasterCode";
                        (grid_Details.Rows[a].FindControl("ddl_Conduct") as DropDownList).DataBind();

                        string strCodes = Convert.ToString((grid_Details.Rows[a].FindControl("lbl_conduct") as Label).Text);
                        if (!string.IsNullOrEmpty(strCodes))
                            strConduct = strCodes;
                        (grid_Details.Rows[a].FindControl("ddl_Conduct") as DropDownList).SelectedIndex = (grid_Details.Rows[a].FindControl("ddl_Conduct") as DropDownList).Items.IndexOf((grid_Details.Rows[a].FindControl("ddl_Conduct") as DropDownList).Items.FindByValue(strConduct));

                    }
                }
            }
        }

        catch
        {

        }
    }

    protected void grid_Details_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label strrm = (Label)e.Row.Cells[6].FindControl("lbl_remark");
                Label strct = (Label)e.Row.Cells[7].FindControl("lbl_conduct");
                ////string strrm = (Label)e.Row.Cells[6].FindControl("lbl_cb");
                ////string strct = (Label)e.Row.Cells[6].FindControl("lbl_cb");
                CheckBox cbsel = (CheckBox)e.Row.Cells[6].FindControl("lbl_cb");
                cbsel.Checked = false;
                if (strrm.Text != "")
                {
                    cbsel.Checked = true;
                    // (e.Row.Cells[6].FindControl("ddl_Remark") as DropDownList).SelectedIndex = (e.Row.Cells[6].FindControl("ddl_Remark") as DropDownList).Items.IndexOf((e.Row.Cells[6].FindControl("ddl_Remark") as DropDownList).Items.FindByValue(strrm.Text));


                }
            }
        }

        catch
        {

        }

    }

    //    if (rb_Journal.Checked)
    //    {
    //        e.Row.Cells[5].Text = "Receipt No";
    //        e.Row.Cells[6].Text = "Receipt Date";
    //        if (ddlJournalType.SelectedIndex == 0)
    //        {
    //            e.Row.Cells[7].Text = "Total Amount";
    //            e.Row.Cells[7].Visible = false;
    //            e.Row.Cells[8].Visible = false;
    //        }
    //        else if (ddlJournalType.SelectedIndex == 1)
    //        {
    //            e.Row.Cells[7].Text = "Total Amount";
    //            e.Row.Cells[7].Visible = true;
    //            e.Row.Cells[8].Visible = true;
    //        }
    //        else
    //        {
    //            e.Row.Cells[7].Text = "Total Amount";
    //            e.Row.Cells[7].Visible = false;
    //            e.Row.Cells[8].Visible = false;
    //        }
    //    }
    //}
    //if (e.Row.RowType == DataControlRowType.DataRow)
    //{
    //    e.Row.Cells[7].Visible = true;
    //    e.Row.Cells[8].Visible = true;
    //    if (rb_Journal.Checked)
    //    {
    //        //e.Row.Cells[7].Visible = false;
    //        //e.Row.Cells[8].Visible = false;
    //        if (ddlJournalType.SelectedIndex == 0)
    //        {

    //            e.Row.Cells[7].Visible = false;
    //            e.Row.Cells[8].Visible = false;
    //        }
    //        else if (ddlJournalType.SelectedIndex == 1)
    //        {

    //            e.Row.Cells[7].Visible = true;
    //            e.Row.Cells[8].Visible = true;
    //        }
    //        else
    //        {

    //            e.Row.Cells[7].Visible = false;
    //            e.Row.Cells[8].Visible = false;
    //        }
    //    }


    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkGridSelectAll.Checked)
            {
                foreach (GridViewRow row in grid_Details.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("lbl_cb");
                    if (!cbsel.Checked)
                        continue;
                    Label lbsno = (Label)row.FindControl("lbl_sno");
                    DropDownList lbremark = (DropDownList)row.FindControl("ddl_Remark");
                    DropDownList lbconduct = (DropDownList)row.FindControl("ddl_Conduct");
                    string strremark = "update applyn  set remarks='" + lbremark.SelectedValue + "' where app_no='" + lbsno.Text + "'";
                    strremark += " update Tc_details set General_conduct='" + lbconduct.SelectedValue + "' where App_no='" + lbsno.Text + "'";

                    d2.update_method_wo_parameter(strremark, "Text");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

                }
            }
            else
            {

                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select The Student')", true);

            }
        }
        catch
        {
        }

    }

}