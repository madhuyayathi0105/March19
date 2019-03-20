using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class TransportAndHostelAllotmentReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dsval = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static byte roll = 0;
    ArrayList colord = new ArrayList();
    bool deptacr = false;
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
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlcollege.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);

                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                loadStage();
                loadRoom();
                loadcolorder();
                rbtransport_Change(sender, e);
                DeptAcr();
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
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
        Response.Redirect("default.aspx", false);

    }



    #region college

    public void loadcollege()
    {
        try
        {
            ddlcollege.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        { }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                loadStage();
                loadRoom();
            }
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
                ddlstream.Items.Insert(0, "Both");
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
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            string stream = ddlstream.SelectedItem.Text.ToString();
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id   and d.college_code='" + clgvalue + "'";
            if (stream != "")
            {
                if (stream != "Both")
                    selqry += "  and type  in('" + stream + "')";
            }
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
        }
        catch { }
    }
    #endregion

    #region batch
    public void bindBtch()
    {
        try
        {
            int i = 0;
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
        }
        catch { }
    }
    #endregion

    #region degree


    public void binddeg()
    {
        try
        {
            int i = 0;
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
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            if (stream != "")
            {
                if (stream != "Both")
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
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
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
            int i = 0;
            txt_dept.Text = "---Select---";
            string batch2 = "";
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

            string degree = "";
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

            string collegecode = ddlcollege.SelectedItem.Value.ToString();
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
            bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            bindsem();
        }
        catch { }
    }
    #endregion

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        }
        catch (Exception ex)
        { }

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
            ds = d2.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
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

    //protected void bindsem()
    //{
    //    try
    //    {
    //        string sem = "";
    //        string clgvalue = ddlcollege.SelectedItem.Value.ToString();
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = d2.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                    cbl_sem.DataSource = ds;
    //                    cbl_sem.DataTextField = "TextVal";
    //                    cbl_sem.DataValueField = "TextCode";
    //                    cbl_sem.DataBind();
    //                }
    //                if (cbl_sem.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                    {
    //                        cbl_sem.Items[i].Selected = true;
    //                        sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                    }
    //                    if (cbl_sem.Items.Count == 1)
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + sem + ")";
    //                    }
    //                    else
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + cbl_sem.Items.Count + ")";
    //                    }
    //                    cb_sem.Checked = true;
    //                }

    //            }
    //            else
    //            {
    //                cbl_sem.Items.Clear();
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Semester(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Year(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Year(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}

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

    #region print
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
                if (rbtransport.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Transport Report Name";
                }
                else
                {
                    lblvalidation1.Text = "Please Enter Your Hostel Report Name";
                }
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
            degreedetails = "TransportAndHostelAllotmentReport";
            pagename = "TransportAndHostelAllotmentReport.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    #endregion

    #region stage
    public void loadStage()
    {
        string sqlquery = string.Empty;
        cblstage.Items.Clear();

        sqlquery = "    select distinct stage_name,stage_id from stage_master";
        ds = d2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblstage.DataSource = ds;
            cblstage.DataTextField = "stage_name";
            cblstage.DataValueField = "stage_id";
            cblstage.DataBind();
            if (cblstage.Items.Count > 0)
            {
                for (int i = 0; i < cblstage.Items.Count; i++)
                {
                    cblstage.Items[i].Selected = true;
                }
                txtstage.Text = "Stage(" + cblstage.Items.Count + ")";
                cbstage.Checked = true;
            }
        }
        else
        {
            txtstage.Text = "Select";
            cbstage.Checked = false;
        }
        // cblstage.SelectedIndex = 0;
    }
    protected void cbstage_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
            //  binddept();
        }
        catch { }
    }
    protected void cblstage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
            // binddept();
        }
        catch { }
    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    #endregion

    #region room

    public void loadRoom()
    {
        string sqlquery = string.Empty;
        cblroom.Items.Clear();
        sqlquery = " select distinct Room_type from RoomCost_Master where College_Code='" + collegecode + "'";

        //room_detail
        ds = d2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblroom.DataSource = ds;
            cblroom.DataTextField = "Room_type";
            cblroom.DataValueField = "Room_type";
            cblroom.DataBind();
            if (cblroom.Items.Count > 0)
            {
                for (int i = 0; i < cblroom.Items.Count; i++)
                {
                    cblroom.Items[i].Selected = true;
                }
                txtroom.Text = "Room(" + cblroom.Items.Count + ")";
                cbroom.Checked = true;
            }
        }
        else
        {
            txtroom.Text = "Select";
            cbroom.Checked = false;
        }
    }

    public void loadHostel()
    {
        string sqlquery = string.Empty;
        cblroom.Items.Clear();
        if (rbformat2.Checked == true)
            sqlquery = " select hostel_code as code,hostel_name as name from hostel_details where College_Code='" + collegecode + "'";
        else
            sqlquery = "select hostelmasterpk as code,hostelname as name from hm_hostelmaster";

        //room_detail
        ds = d2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblroom.DataSource = ds;
            cblroom.DataTextField = "name";
            cblroom.DataValueField = "code";
            cblroom.DataBind();
            if (cblroom.Items.Count > 0)
            {
                for (int i = 0; i < cblroom.Items.Count; i++)
                {
                    cblroom.Items[i].Selected = true;
                }
                txtroom.Text = "Hostel(" + cblroom.Items.Count + ")";
                cbroom.Checked = true;
            }
        }
        else
        {
            txtroom.Text = "Select";
            cbroom.Checked = false;
        }
    }
    protected void cbroom_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbhostel.Checked == true)
                CallCheckboxChange(cbroom, cblroom, txtroom, "Room", "--Select--");
            else
            {
                CallCheckboxChange(cbroom, cblroom, txtroom, "Hostel", "--Select--");
                loadHeader();
                loadLedger();
            }

        }
        catch { }
    }
    protected void cblroom_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbhostel.Checked == true)
                CallCheckboxListChange(cbroom, cblroom, txtroom, "Room", "--Select--");
            else
            {
                CallCheckboxListChange(cbroom, cblroom, txtroom, "Hostel", "--Select--");
                loadHeader();
                loadLedger();
            }

        }
        catch { }
    }
    #endregion

    #region header and ledger

    public void loadHeader()
    {
        try
        {
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            cblhtlhdr.Items.Clear();
            string hostelval = Convert.ToString(getCblSelectedValue(cblroom));
            string query = "";
            if (rbformat2.Checked == true)
                query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";
            else
                query = " select distinct hm.hosteladmfeeheaderfk,h.headername,h.headerpk from hm_hostelmaster hm,fm_headermaster h  where h.headerpk=hm.hosteladmfeeheaderfk and hostelmasterpk in('" + hostelval + "')";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblhtlhdr.DataSource = ds;
                cblhtlhdr.DataTextField = "HeaderName";
                cblhtlhdr.DataValueField = "HeaderPK";
                cblhtlhdr.DataBind();
                for (int i = 0; i < cblhtlhdr.Items.Count; i++)
                {
                    cblhtlhdr.Items[i].Selected = true;
                }
                txthtlhdr.Text = "Header(" + cblhtlhdr.Items.Count + ")";
                cbhtlhdr.Checked = true;
                loadLedger();
            }
        }
        catch
        {
        }
    }

    public void loadLedger()
    {
        try
        {
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            cblhtlldr.Items.Clear();
            string hed = "";
            for (int i = 0; i < cblhtlhdr.Items.Count; i++)
            {
                if (cblhtlhdr.Items[i].Selected == true)
                {
                    if (hed == "")
                    {
                        hed = cblhtlhdr.Items[i].Value.ToString();
                    }
                    else
                    {
                        hed = hed + "','" + "" + cblhtlhdr.Items[i].Value.ToString() + "";
                    }
                }
            }
            string query1 = "";
            if (rbformat2.Checked == true)
                query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            else
                query1 = " select distinct l.ledgername,l.ledgerpk,hosteladmfeeledgerfk from hm_hostelmaster hm,fm_ledgermaster l  where l.ledgerpk=hm.hosteladmfeeledgerfk and hm.hosteladmfeeheaderfk in ('" + hed + "')";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblhtlldr.DataSource = ds;
                cblhtlldr.DataTextField = "LedgerName";
                cblhtlldr.DataValueField = "LedgerPK";
                cblhtlldr.DataBind();
                for (int i = 0; i < cblhtlldr.Items.Count; i++)
                {
                    cblhtlldr.Items[i].Selected = true;
                }
                txthtlldr.Text = "Ledger(" + cblhtlldr.Items.Count + ")";
                cbhtlldr.Checked = true; ;

            }
            else
            {
                for (int i = 0; i < cblhtlldr.Items.Count; i++)
                {
                    cblhtlldr.Items[i].Selected = false;
                }
                txthtlldr.Text = "--Select--";
                cbhtlldr.Checked = false; ;
            }

        }
        catch
        {
        }
    }


    public void cbhtlhdr_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbhtlhdr, cblhtlhdr, txthtlhdr, "Header", "--Select--");
            loadLedger();
        }
        catch (Exception ex)
        { }
    }

    public void cblhtlhdr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbhtlhdr, cblhtlhdr, txthtlhdr, "Header", "--Select--");
            loadLedger();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbhtlldr_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbhtlldr, cblhtlldr, txthtlldr, "Ledger", "--Select--");

        }
        catch (Exception ex)
        { }
    }
    public void cblhtlldr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbhtlldr, cblhtlldr, txthtlldr, "Ledger", "--Select--");
        }
        catch (Exception ex)
        { }
    }

    #endregion

    #region Radio button event

    protected void rbtransport_Change(object sender, EventArgs e)
    {
        divcol.Visible = false;
        FpSpread1.Visible = false;
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";

        //
        tdstname.Visible = true;
        tdstvalue.Visible = true;
        tdrmname.Visible = false;
        tdrmvalue.Visible = false;

        //
        rb_hostel.Checked = false;
        rbhostel.Checked = false;
        rbhstlname.Checked = false;
        rbhostel.Enabled = false;
        rbhstlname.Enabled = false;
        //
        tdhtlhdr.Visible = false;
        tdhtlhdrval.Visible = false;
        tdhtlldr.Visible = false;
        tdhtlldrval.Visible = false;
        //
        tdrbs.Visible = false;
        tdpdsdt.Visible = false;
        loadcolorder();
        tdfmt.Visible = false;
    }
    protected void rb_hostel_Change(object sender, EventArgs e)
    {
        if (rb_hostel.Checked == true)
        {
            rbhostel.Checked = true;
            rbhostel_Change(sender, e);
            rbhstlname.Checked = false;
            rbhostel.Enabled = true;
            rbhstlname.Enabled = true;
            //
            tdhtlhdr.Visible = false;
            tdhtlhdrval.Visible = false;
            tdhtlldr.Visible = false;
            tdhtlldrval.Visible = false;
            loadcolorder();
            tdfmt.Visible = false;
        }
        else
        {
            rbhstlname.Checked = true;
            rbhostel.Checked = false;
        }

    }
    protected void rbhostel_Change(object sender, EventArgs e)
    {
        divcol.Visible = false;
        FpSpread1.Visible = false;
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
        //
        tdstname.Visible = false;
        tdstvalue.Visible = false;
        tdrmname.Visible = true;
        tdrmvalue.Visible = true;
        loadRoom();
        lblhtlname.Text = "Room Type";
        tdhtlhdr.Visible = false;
        tdhtlhdrval.Visible = false;
        tdhtlldr.Visible = false;
        tdhtlldrval.Visible = false;
        tdrbs.Visible = false;
        tdpdsdt.Visible = false;
        loadcolorder();
        tdfmt.Visible = false;
    }
    protected void rbhstlname_Change(object sender, EventArgs e)
    {
        loadHostel();
        lblhtlname.Text = "Hostel Type";
        tdhtlhdr.Visible = true;
        tdhtlhdrval.Visible = true;
        tdhtlldr.Visible = true;
        tdhtlldrval.Visible = true;
        loadHeader();
        loadLedger();
        tdrbs.Visible = true;
        tdpdsdt.Visible = true;
        loadcolorder();
        tdfmt.Visible = true;

    }

    protected void rbformat1_Change(object sender, EventArgs e)
    {
        loadHostel();
        loadHeader();
        loadLedger();
    }
    protected void rbformat2_Change(object sender, EventArgs e)
    {
        loadHostel();
        loadHeader();
        loadLedger();
    }

    protected void rbheader_Change(object sender, EventArgs e)
    {
        loadcolorder();
    }
    protected void rbledger_Change(object sender, EventArgs e)
    {
        loadcolorder();
    }

    protected void rbpaid_Change(object sender, EventArgs e)
    {
    }
    protected void rbunpaid_Change(object sender, EventArgs e)
    {
    }

    protected void rbboth_Change(object sender, EventArgs e)
    {

    }

    #endregion

    #region button go

    protected DataSet loadDataValue()
    {
        DataSet dsval = new DataSet();
        try
        {

            string SelQ = "";
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            string stageid = Convert.ToString(getCblSelectedValue(cblstage));
            string roomid = Convert.ToString(getCblSelectedValue(cblroom));
            string hedaderid = "";
            string ledgerid = "";
            string htlhdrid = Convert.ToString(getCblSelectedValue(cblhtlhdr));
            string htlldrid = Convert.ToString(getCblSelectedValue(cblhtlldr));
            ViewState["sem"] = sem;

            //transport header and ledger value
            string ledgersett = "select LinkValue from New_InsSettings where LinkName='TransportLedgerValue' and user_code='" + usercode + "' and college_code in('" + collegecode + "') ";
            ds = d2.select_method_wo_parameter(ledgersett, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                for (int hed = 0; hed < ds.Tables[0].Rows.Count; hed++)
                {
                    string value = Convert.ToString(ds.Tables[0].Rows[hed]["LinkValue"]);
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        if (hedaderid == "" && ledgerid == "")
                        {
                            hedaderid = valuesplit[0];
                            ledgerid = valuesplit[1];
                        }
                        else
                        {
                            hedaderid = hedaderid + "'" + "," + "'" + valuesplit[0];
                            ledgerid = ledgerid + "'" + "," + "'" + valuesplit[1];
                        }
                    }
                }
            }

            if (rbtransport.Checked == true)
            {
                #region transport
                SelQ = " select roll_no,roll_admit,reg_no,stud_name,Boarding,batch_year,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt,PaidAmount as paidamt,BalAmount balamt,FeeCategory,r.degree_code from FT_FeeAllot a,Registration r where a.App_No = r.App_No and r.college_code='" + collegecode + "' and isnull(IsCanceledStage,0)<>'1' ";
                if (batch != "")
                    SelQ += " and r.Batch_year in('" + batch + "')";

                if (degree != "")
                    SelQ += " and r.degree_code in('" + degree + "')";

                if (stageid != "")
                    SelQ += " and Boarding in('" + stageid + "')";

                if (ledgerid != "")
                    SelQ += " and LedgerFK in('" + ledgerid + "')";

                if (sem != "")
                    SelQ += "and FeeCategory in( '" + sem + "')";

                SelQ += " order by Roll_No,FeeCategory";

                SelQ = SelQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                SelQ = SelQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                SelQ += " select distinct Stage_Name from routemaster r,vehicle_master v where Stage_Name is not null and Stage_Name<>'' and v.Veh_ID=r.Veh_ID";
                #endregion
            }
            else
            {
                if (rbhostel.Checked == true)
                {
                    #region hostel Room

                    //SelQ = "   select distinct ht.app_no, rd.Room_Type,hm.HostelAdmFeeHeaderFK,hm.HostelAdmFeeLedgerFK from registration r,ht_hostelregistration ht,Room_detail rd,hm_hostelmaster hm where r.app_no=ht.app_no and ht.RoomFK=rd.roomPK and hm.hostelmasterPk=ht.hostelmasterfk and r.college_code='" + collegecode + "' ";

                    SelQ = "   select distinct ht.app_no, rd.Room_Type,hm.HostelAdmFeeHeaderFK,hm.HostelAdmFeeLedgerFK,((select Building_Name from Building_Master where code=BuildingFK)+'-'+ (select Floor_Name from Floor_Master where Floorpk=Floorfk)+'-'+(select Room_Name from Room_Detail where RoomFK=roompk))as stu_details from registration r,ht_hostelregistration ht,Room_detail rd,hm_hostelmaster hm where r.app_no=ht.app_no and ht.RoomFK=rd.roomPK and hm.hostelmasterPk=ht.hostelmasterfk and r.college_code='" + collegecode + "' ";
                    if (batch != "")
                        SelQ += " and r.Batch_year in('" + batch + "')";

                    if (degree != "")
                        SelQ += " and r.degree_code in('" + degree + "')";

                    if (roomid != "")
                        SelQ += " and rd.Room_Type in('" + roomid + "')";

                    //if (ledgerid != "")
                    //    SelQ += " and LedgerFK in('" + ledgerid + "')";

                    if (sem != "")
                        //SelQ += "and FeeCategory in( '" + sem + "')";
                        SelQ = SelQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                    SelQ = SelQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                    //SelQ += " select a.app_no,roll_admit, roll_no,reg_no,stud_name,Boarding,batch_year,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt,PaidAmount as paidamt,BalAmount balamt,FeeCategory,r.degree_code,headerfk,ledgerfk from FT_FeeAllot a,Registration r where a.App_No = r.App_No ";
                    SelQ += " select a.app_no,roll_admit, roll_no,reg_no,r.stud_name,Boarding,r.batch_year,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt,PaidAmount as paidamt,BalAmount balamt,FeeCategory,r.degree_code,headerfk,ledgerfk,ISNULL(ap.Student_Mobile,'') Student_Mobile from FT_FeeAllot a,Registration r,applyn ap where ap.app_no=r.App_No and a.App_No = r.App_No ";
                    if (batch != "")
                        SelQ += " and r.Batch_year in('" + batch + "')";

                    if (degree != "")
                        SelQ += " and r.degree_code in('" + degree + "')";
                    if (sem != "")
                        SelQ += "and FeeCategory in( '" + sem + "')";

                    SelQ += " select room_type,room_cost from roomcost_master where college_code='" + collegecode + "'";
                    #endregion
                }
                else
                {
                    string Hdrvalue = "";
                    string hdrgrpval = "";
                    if (rbheader.Checked == true)
                    {
                        Hdrvalue = ",f.headerfk as name";
                        hdrgrpval = ",f.headerfk";
                    }
                    else
                    {
                        Hdrvalue = ",f.ledgerfk as name";
                        hdrgrpval = ",f.ledgerfk";
                    }

                    string status = "";
                    if (rbpaid.Checked == true)
                        status = " having sum(isnull(f.paidamount,'0'))>'0'";
                    // status = " and( isnull(f.paidamount,'0')>0)";
                    else if (rbunpaid.Checked == true)
                        status = " having sum(isnull(f.balamount,'0'))>'0'";
                    //status = " and( isnull(f.paidamount,'0')='0' and balamount=totalamount)";
                    else
                        status = "";

                    if (rbformat2.Checked == true)
                    {
                        #region hostel

                        //magesh 11.4.18
                        //SelQ = " select r.app_no, r.roll_no,r.roll_admit,reg_no,stud_name,hs.hostel_code,batch_year,sum(FeeAmount) as feeamt,sum(DeductAmout) as concession ,sum(TotalAmount) as totamt,sum(PaidAmount) as paidamt,sum(BalAmount) balamt,FeeCategory,r.degree_code" + Hdrvalue + " from registration r, hostel_studentdetails hs,ft_feeallot f where r.roll_admit=hs.roll_admit and r.app_no=f.app_no   and r.college_code='" + collegecode + "'  ";
                        SelQ = " select r.app_no, r.roll_no,r.roll_admit,reg_no,r.stud_name,hs.hostel_code,(hs.Building_Name +'-'+ hs.Floor_Name +'-'+ Room_Name)as stu_details,r.batch_year,sum(FeeAmount) as feeamt,sum(DeductAmout) as concession ,sum(TotalAmount) as totamt,sum(PaidAmount) as paidamt,sum(BalAmount) balamt,FeeCategory,r.degree_code" + Hdrvalue + ",ISNULL(a.Student_Mobile,'') Student_Mobile from registration r,applyn a ,hostel_studentdetails hs,ft_feeallot f where  r.App_No=a.app_no and r.roll_admit=hs.roll_admit and r.app_no=f.app_no   and r.college_code='" + collegecode + "'  ";

                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";

                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";

                        if (roomid != "")
                            SelQ += " and hs.hostel_code in('" + roomid + "')";

                        if (sem != "")
                            SelQ += "and f.FeeCategory in( '" + sem + "')";

                        if (htlhdrid != "")
                            SelQ += " and f.headerfk in('" + htlhdrid + "')";

                        if (htlldrid != "")
                            SelQ += "and f.ledgerfk in( '" + htlldrid + "')";
                        //magesh 11.4.18
                        //SelQ += " group by r.app_no, r.roll_no,r.roll_admit,reg_no,stud_name,hs.hostel_code,batch_year,FeeCategory,r.degree_code" + hdrgrpval + " " + status + "";

                        SelQ += " group by r.app_no, r.roll_no,r.roll_admit,reg_no,r.stud_name,hs.hostel_code,hs.Building_Name, hs.Floor_Name, hs.Room_Name,r.batch_year,a.Student_Mobile,FeeCategory,r.degree_code" + hdrgrpval + " " + status + "";


                        SelQ = SelQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                        SelQ = SelQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                        SelQ += " select headerpk,headername ,ledgerpk,ledgername from fm_headermaster h, fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode='" + collegecode + "'";
                        SelQ += "  select hostel_code,hostel_name from hostel_details where college_code='" + collegecode + "'";
                        #endregion
                    }
                    else
                    {
                        #region hostel

                        //SelQ = "   select r.app_no, r.roll_no,r.roll_admit,reg_no,stud_name,hs.hostelmasterfk,batch_year,sum(FeeAmount) as feeamt,sum(DeductAmout) as concession ,sum(TotalAmount) as totamt,sum(PaidAmount) as paidamt,sum(BalAmount) balamt,FeeCategory,r.degree_code" + Hdrvalue + " from registration r, ht_hostelregistration hs,ft_feeallot f where r.app_no=hs.app_no and r.app_no=f.app_no and hs.app_no=f.app_no " + status + "  and r.college_code='" + collegecode + "'  ";
                        SelQ = "select r.app_no, r.roll_no,r.roll_admit,reg_no,r.stud_name,hs.hostelmasterfk,((select Building_Name from Building_Master where code=BuildingFK)+'-'+ (select Floor_Name from Floor_Master where Floorpk=Floorfk)+'-'+(select Room_Name from Room_Detail where RoomFK=roompk))as stu_details,r.batch_year,sum(FeeAmount) as feeamt,sum(DeductAmout) as concession ,sum(TotalAmount) as totamt,sum(PaidAmount) as paidamt,sum(BalAmount) balamt,FeeCategory,r.degree_code" + Hdrvalue + ",ISNULL(a.Student_Mobile,'') Student_Mobile from registration r,applyn a ,ht_hostelregistration hs,ft_feeallot f where r.App_No=a.app_no and r.app_no=hs.app_no and r.app_no=f.app_no and hs.app_no=f.app_no  and r.college_code='" + collegecode + "'  ";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";

                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";

                        if (roomid != "")
                            SelQ += " and hs.hostelmasterfk in('" + roomid + "')";

                        if (sem != "")
                            SelQ += "and f.FeeCategory in( '" + sem + "')";

                        if (htlhdrid != "")
                            SelQ += " and f.headerfk in('" + htlhdrid + "')";

                        if (htlldrid != "")
                            SelQ += "and f.ledgerfk in( '" + htlldrid + "')";
                        //SelQ += " group by r.app_no, r.roll_no,reg_no,r.roll_admit,stud_name,hs.hostelmasterfk,batch_year,FeeCategory,r.degree_code" + hdrgrpval + " " + status + "";

                        SelQ += " group by r.app_no, r.roll_no,reg_no,r.roll_admit,r.stud_name,hs.hostelmasterfk,BuildingFK,Floorfk,RoomFK,r.batch_year,a.Student_Mobile,FeeCategory,r.degree_code" + hdrgrpval + " " + status + "";


                        SelQ = SelQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                        SelQ = SelQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                        SelQ += " select headerpk,headername ,ledgerpk,ledgername from fm_headermaster h, fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode='" + collegecode + "'";
                        SelQ += " select hostelmasterpk,hostelname from hm_hostelmaster";
                        #endregion
                    }
                }
            }
            dsval.Clear();
            dsval = d2.select_method_wo_parameter(SelQ, "Text");

        }
        catch { }

        return dsval;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {

        dsval.Clear();
        dsval = loadDataValue();
        if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
        {
            if (rbtransport.Checked == true)
                loadSpreadTransportValues();
            else
            {
                if (rbhostel.Checked == true)
                    loadSpreadHostelRommValues();
                else
                    loadspreadHostelValues();
            }
        }
        else
        {
            FpSpread1.Visible = false;
            print.Visible = false;
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            divcol.Visible = false;
            lbl_alert.Text = "No Record Found";
        }

    }

    protected void loadSpreadTransportValues()
    {
        try
        {

            #region desgin
            bool flg = false;
            DeptAcr();
            RollAndRegSettings();
            loadcolumns();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 12;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].SelectionBackColor = Color.White;

            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[1].Visible = true;
            if (!colord.Contains("1"))
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;

            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            if (!colord.Contains("2"))
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                flg = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("3"))
            {
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                flg = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("4"))
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                flg = true;
            }

            //if (flg == true)
            //{
            //    if (roll == 0)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //    else if (roll == 1)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //    else if (roll == 2)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = false;
            //    }
            //    else if (roll == 3)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = false;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //}

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("5"))
            {
                FpSpread1.Sheets[0].Columns[5].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[5].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("6"))
            {
                FpSpread1.Sheets[0].Columns[6].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[6].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = lblsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("7"))
            {
                FpSpread1.Sheets[0].Columns[7].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[7].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Stage";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[8].Visible = true;
            if (!colord.Contains("8"))
            {
                FpSpread1.Sheets[0].Columns[8].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[8].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Allot";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("9"))
            {
                FpSpread1.Sheets[0].Columns[9].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[9].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("10"))
            {
                FpSpread1.Sheets[0].Columns[10].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[10].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Balance";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("11"))
            {
                FpSpread1.Sheets[0].Columns[11].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[11].Visible = true;
            }
            spreadColumnVisible(flg);

            #endregion

            #region values
            Hashtable gdtot = new Hashtable();
            DataView Dview = new DataView();
            DataView fee = new DataView();
            for (int sel = 0; sel < dsval.Tables[0].Rows.Count; sel++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["stud_name"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["roll_no"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["reg_no"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["roll_admit"]);

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["Batch_year"]);
                string Degreename = "";
                string Acrname = "";
                if (dsval.Tables[1].Rows.Count > 0)
                {
                    dsval.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dsval.Tables[0].Rows[sel]["Degree_code"]) + "'";
                    Dview = dsval.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                    {
                        Degreename = Convert.ToString(Dview[0]["degreename"]);
                        Acrname = Convert.ToString(Dview[0]["dept_acronym"]);
                    }
                }

                if (deptacr == true)
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Acrname);
                else
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Degreename);

                string TextName = "";
                if (dsval.Tables[2].Rows.Count > 0)
                {
                    dsval.Tables[2].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dsval.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    fee = dsval.Tables[2].DefaultView;
                    if (fee.Count > 0)
                        TextName = Convert.ToString(fee[0]["TextVal"]);
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = TextName;

                #region transport

                string stagename = d2.GetFunction(" select distinct Stage_Name from stage_master where Stage_id = '" + Convert.ToString(dsval.Tables[0].Rows[sel]["Boarding"]) + "'");
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = stagename;
                //allot              
                double allot = 0;
                double.TryParse(Convert.ToString(dsval.Tables[0].Rows[sel]["totamt"]), out allot);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(allot);
                if (!gdtot.Contains(9))
                    gdtot.Add(9, Convert.ToString(allot));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[9]), out total);
                    total += allot;
                    gdtot.Remove(9);
                    gdtot.Add(9, Convert.ToString(total));
                }
                //paid amount
                double paid = 0;
                double.TryParse(Convert.ToString(dsval.Tables[0].Rows[sel]["paidamt"]), out paid);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(paid);
                if (!gdtot.Contains(10))
                    gdtot.Add(10, Convert.ToString(paid));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[10]), out total);
                    total += paid;
                    gdtot.Remove(10);
                    gdtot.Add(10, Convert.ToString(total));
                }
                //balance
                double balance = 0;
                double.TryParse(Convert.ToString(dsval.Tables[0].Rows[sel]["balamt"]), out balance);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(balance);
                if (!gdtot.Contains(11))
                    gdtot.Add(11, Convert.ToString(balance));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[11]), out total);
                    total += balance;
                    gdtot.Remove(11);
                    gdtot.Add(11, Convert.ToString(total));
                }
                #endregion
            }
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
            #endregion

            #region Grandtotal
            double grandTotal = 0;
            FpSpread1.Sheets[0].PageSize = dsval.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.GreenYellow;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int i = 8; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                double.TryParse(Convert.ToString(gdtot[i]), out grandTotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
            }

            #endregion
            FpSpread1.Sheets[0].SelectionBackColor = Color.Green;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            divcol.Visible = true;
            FpSpread1.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            print.Visible = true;

        }
        catch { }
    }

    protected void loadSpreadHostelRommValues()
    {
        try
        {
            #region desgin
            bool flg = false;
            DeptAcr();
            RollAndRegSettings();
            loadcolumns();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 14;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].SelectionBackColor = Color.White;

            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[1].Visible = true;
            if (!colord.Contains("1"))
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;

            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            if (!colord.Contains("2"))
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                flg = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("3"))
            {
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                flg = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("4"))
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                flg = true;
            }

            //if (flg == true)
            //{
            //    if (roll == 0)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //    else if (roll == 1)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //    else if (roll == 2)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = false;
            //    }
            //    else if (roll == 3)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = false;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //}

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("5"))
            {
                FpSpread1.Sheets[0].Columns[5].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[5].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("6"))
            {
                FpSpread1.Sheets[0].Columns[6].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[6].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = lblsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("7"))
            {
                FpSpread1.Sheets[0].Columns[7].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[7].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Room";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[8].Visible = true;
            if (!colord.Contains("8"))
            {
                FpSpread1.Sheets[0].Columns[8].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[8].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Building Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("9"))
            {
                FpSpread1.Sheets[0].Columns[9].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[9].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Allot";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("10"))
            {
                FpSpread1.Sheets[0].Columns[10].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[10].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("11"))
            {
                FpSpread1.Sheets[0].Columns[11].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[11].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Balance";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[12].Visible = true;
            if (!colord.Contains("12"))
            {
                FpSpread1.Sheets[0].Columns[12].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[12].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Contact";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[13].Visible = true;
            if (!colord.Contains("13"))
            {
                FpSpread1.Sheets[0].Columns[13].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[13].Visible = true;
            }

            spreadColumnVisible(flg);

            #endregion

            #region values
            Hashtable gdtot = new Hashtable();
            DataView Dview = new DataView();
            DataView fee = new DataView();
            DataView dvrm = new DataView();
            DataView dvht = new DataView();
            bool save = false;
            int rowval = 0;
            for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
            {
                if (ViewState["sem"] != null)
                {
                    string feecatval = Convert.ToString(ViewState["sem"]);
                    if (dsval.Tables[3].Rows.Count > 0)
                    {
                        dsval.Tables[3].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsval.Tables[0].Rows[row]["App_no"]) + "' and headerfk='" + Convert.ToString(dsval.Tables[0].Rows[row]["HostelAdmFeeHeaderFK"]) + "' and ledgerfk='" + Convert.ToString(dsval.Tables[0].Rows[row]["HostelAdmFeeLedgerFK"]) + "' and FeeCategory in('" + feecatval + "')";
                        dvht = dsval.Tables[3].DefaultView;
                        if (dvht.Count > 0)
                        {
                            for (int sel = 0; sel < dvht.Count; sel++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                rowval++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvht[sel]["Stud_Name"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvht[sel]["roll_no"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvht[sel]["reg_no"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvht[sel]["roll_admit"]);

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvht[sel]["Batch_year"]);
                                string Degreename = "";
                                string Acrname = "";
                                if (dsval.Tables[1].Rows.Count > 0)
                                {
                                    dsval.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dvht[sel]["Degree_code"]) + "'";
                                    Dview = dsval.Tables[1].DefaultView;
                                    if (Dview.Count > 0)
                                    {
                                        Degreename = Convert.ToString(Dview[0]["degreename"]);
                                        Acrname = Convert.ToString(Dview[0]["dept_acronym"]);
                                    }
                                }

                                if (deptacr == true)
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Acrname);
                                else
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Degreename);

                                string TextName = "";
                                if (dsval.Tables[2].Rows.Count > 0)
                                {
                                    dsval.Tables[2].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dvht[sel]["FeeCategory"]) + "'";
                                    fee = dsval.Tables[2].DefaultView;
                                    if (fee.Count > 0)
                                        TextName = Convert.ToString(fee[0]["TextVal"]);
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = TextName;

                                #region room values

                                //allot              
                                double allot = 0;
                                double.TryParse(Convert.ToString(dvht[sel]["totamt"]), out allot);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(allot);
                                if (!gdtot.Contains(10))
                                    gdtot.Add(10, Convert.ToString(allot));
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(gdtot[10]), out total);
                                    total += allot;
                                    gdtot.Remove(10);
                                    gdtot.Add(10, Convert.ToString(total));
                                }


                                //
                                // Convert.ToString(dsval.Tables[0].Rows[row]["Room_Type"]);
                                string stagename = "";
                                string Detail = "";
                                if (dsval.Tables[4].Rows.Count > 0)
                                {
                                    dsval.Tables[4].DefaultView.RowFilter = "room_cost='" + Convert.ToString(allot) + "'";
                                    dvrm = dsval.Tables[4].DefaultView;
                                    if (dvrm.Count > 0)
                                        stagename = Convert.ToString(dvrm[0]["room_type"]);

                                    Detail = Convert.ToString(dsval.Tables[0].Rows[row]["stu_details"]);
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = stagename;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Detail;
                                string mobile_no = "";
                                mobile_no = Convert.ToString(dsval.Tables[0].Rows[sel]["Student_Mobile"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14].Text = mobile_no;
                                FpSpread1.Sheets[0].Columns[14].Width = 50;


                                //paid amount
                                double paid = 0;
                                double.TryParse(Convert.ToString(dvht[sel]["paidamt"]), out paid);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(paid);
                                if (!gdtot.Contains(11))
                                    gdtot.Add(11, Convert.ToString(paid));
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(gdtot[11]), out total);
                                    total += paid;
                                    gdtot.Remove(11);
                                    gdtot.Add(11, Convert.ToString(total));
                                }
                                //balance
                                double balance = 0;
                                double.TryParse(Convert.ToString(dvht[sel]["balamt"]), out balance);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(balance);
                                if (!gdtot.Contains(12))
                                    gdtot.Add(12, Convert.ToString(balance));
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(gdtot[12]), out total);
                                    total += balance;
                                    gdtot.Remove(12);
                                    gdtot.Add(12, Convert.ToString(total));
                                }

                                #endregion

                                save = true;
                            }
                        }
                    }
                }
            }
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
            #endregion
            if (save == true)
            {
                #region Grandtotal
                double grandTotal = 0;
                FpSpread1.Sheets[0].PageSize = dsval.Tables[0].Rows.Count + 1;
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.GreenYellow;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                for (int i = 9; i < FpSpread1.Sheets[0].ColumnCount; i++)
                {
                    double.TryParse(Convert.ToString(gdtot[i]), out grandTotal);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
                }

                #endregion
                FpSpread1.Sheets[0].SelectionBackColor = Color.Green;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
                divcol.Visible = true;
                FpSpread1.Visible = true;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                print.Visible = true;
            }
            else
            {
                divcol.Visible = false;
                FpSpread1.Visible = false;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                print.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }

    protected void loadspreadHostelValues()
    {
        try
        {

            #region desgin
            bool flg = false;
            DeptAcr();
            RollAndRegSettings();
            loadcolumns();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 15;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].SelectionBackColor = Color.White;

            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[1].Visible = true;
            if (!colord.Contains("1"))
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;

            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            if (!colord.Contains("2"))
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                flg = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("3"))
            {
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                flg = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("4"))
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                flg = true;
            }

            //if (flg == true)
            //{
            //    if (roll == 0)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //    else if (roll == 1)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //    else if (roll == 2)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = true;
            //        FpSpread1.Sheets[0].Columns[3].Visible = false;
            //    }
            //    else if (roll == 3)
            //    {
            //        FpSpread1.Sheets[0].Columns[2].Visible = false;
            //        FpSpread1.Sheets[0].Columns[3].Visible = true;
            //    }
            //}

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("5"))
            {
                FpSpread1.Sheets[0].Columns[5].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[5].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("6"))
            {
                FpSpread1.Sheets[0].Columns[6].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[6].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = lblsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("7"))
            {
                FpSpread1.Sheets[0].Columns[7].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[7].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Hostel";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[8].Visible = true;
            if (!colord.Contains("8"))
            {
                FpSpread1.Sheets[0].Columns[8].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[8].Visible = true;
            }
            string hdrname = "";
            if (rbheader.Checked == true)
                hdrname = "Header";
            else
                hdrname = "Ledger";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = hdrname;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("9"))
            {
                FpSpread1.Sheets[0].Columns[9].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[9].Visible = true;
            }

            //magesh 11.4.18
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Building Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("10"))
            {
                FpSpread1.Sheets[0].Columns[10].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[10].Visible = true;
            }
            //magesh 11.4.18

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Allot";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("11"))
            {
                FpSpread1.Sheets[0].Columns[11].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[11].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[12].Visible = true;
            if (!colord.Contains("12"))
            {
                FpSpread1.Sheets[0].Columns[12].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[12].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Balance";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[13].Visible = true;
            if (!colord.Contains("13"))
            {
                FpSpread1.Sheets[0].Columns[13].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[13].Visible = true;
            }
            //magesh 11.4.18
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Contact";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[14].Visible = true;
            if (!colord.Contains("14"))
            {
                FpSpread1.Sheets[0].Columns[14].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[14].Visible = true;
            }


            spreadColumnVisible(flg);
            #endregion

            #region values
            Hashtable gdtot = new Hashtable();
            DataView Dview = new DataView();
            DataView fee = new DataView();
            DataView hdr = new DataView();
            DataView dvhtl = new DataView();
            for (int sel = 0; sel < dsval.Tables[0].Rows.Count; sel++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["stud_name"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["roll_no"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["reg_no"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["roll_admit"]);

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsval.Tables[0].Rows[sel]["Batch_year"]);
                string Degreename = "";
                string Acrname = "";
                if (dsval.Tables[1].Rows.Count > 0)
                {
                    dsval.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dsval.Tables[0].Rows[sel]["Degree_code"]) + "'";
                    Dview = dsval.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                    {
                        Degreename = Convert.ToString(Dview[0]["degreename"]);
                        Acrname = Convert.ToString(Dview[0]["dept_acronym"]);
                    }
                }

                if (deptacr == true)
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Acrname);
                else
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Degreename);

                string TextName = "";
                if (dsval.Tables[2].Rows.Count > 0)
                {
                    dsval.Tables[2].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dsval.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    fee = dsval.Tables[2].DefaultView;
                    if (fee.Count > 0)
                        TextName = Convert.ToString(fee[0]["TextVal"]);
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = TextName;

                #region transport

                //hostel name
                string stagename = "";
                string Detail = "";
                if (dsval.Tables[4].Rows.Count > 0)
                {

                    if (rbformat2.Checked == true)
                    {
                        dsval.Tables[4].DefaultView.RowFilter = "hostel_code='" + Convert.ToString(dsval.Tables[0].Rows[sel]["hostel_code"]) + "'";
                        dvhtl = dsval.Tables[4].DefaultView;
                        if (dvhtl.Count > 0)
                            stagename = Convert.ToString(dvhtl[0]["hostel_name"]);
                        //magesh 11.4.18
                        Detail = Convert.ToString(dsval.Tables[0].Rows[sel]["stu_details"]);
                    }
                    else
                    {
                        dsval.Tables[4].DefaultView.RowFilter = "hostelmasterpk='" + Convert.ToString(dsval.Tables[0].Rows[sel]["hostelmasterfk"]) + "'";
                        dvhtl = dsval.Tables[4].DefaultView;
                        if (dvhtl.Count > 0)
                            stagename = Convert.ToString(dvhtl[0]["hostelname"]);
                        //magesh 11.4.18
                        Detail = Convert.ToString(dsval.Tables[0].Rows[sel]["stu_details"]);
                    }

                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = stagename;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Detail;
                FpSpread1.Sheets[0].Columns[10].Width = 50;

                //header or ledger name
                string hName = "";
                if (dsval.Tables[3].Rows.Count > 0)
                {
                    if (rbheader.Checked == true)
                    {
                        dsval.Tables[3].DefaultView.RowFilter = "headerPk='" + Convert.ToString(dsval.Tables[0].Rows[sel]["name"]) + "'";
                        hdr = dsval.Tables[3].DefaultView;
                        if (hdr.Count > 0)
                            hName = Convert.ToString(hdr[0]["headername"]);
                    }
                    else
                    {
                        dsval.Tables[3].DefaultView.RowFilter = "ledgerPK='" + Convert.ToString(dsval.Tables[0].Rows[sel]["name"]) + "'";
                        hdr = dsval.Tables[3].DefaultView;
                        if (hdr.Count > 0)
                            hName = Convert.ToString(hdr[0]["ledgername"]);
                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = hName;


                //allot              
                double allot = 0;
                double.TryParse(Convert.ToString(dsval.Tables[0].Rows[sel]["totamt"]), out allot);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(allot);
                if (!gdtot.Contains(11))
                    gdtot.Add(11, Convert.ToString(allot));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[11]), out total);
                    total += allot;
                    gdtot.Remove(11);
                    gdtot.Add(11, Convert.ToString(total));
                }
                //paid amount
                double paid = 0;
                double.TryParse(Convert.ToString(dsval.Tables[0].Rows[sel]["paidamt"]), out paid);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(paid);
                if (!gdtot.Contains(12))
                    gdtot.Add(12, Convert.ToString(paid));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[12]), out total);
                    total += paid;
                    gdtot.Remove(12);
                    gdtot.Add(12, Convert.ToString(total));
                }
                //balance
                double balance = 0;
                double.TryParse(Convert.ToString(dsval.Tables[0].Rows[sel]["balamt"]), out balance);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(balance);
                if (!gdtot.Contains(13))
                    gdtot.Add(13, Convert.ToString(balance));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[13]), out total);
                    total += balance;
                    gdtot.Remove(13);
                    gdtot.Add(13, Convert.ToString(total));
                }

                //magesh 11.4.18
                string mobile_no = "";
                mobile_no = Convert.ToString(dsval.Tables[0].Rows[sel]["Student_Mobile"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14].Text = mobile_no;
                FpSpread1.Sheets[0].Columns[14].Width = 50;
                //color change 
                if (rbboth.Checked == true)
                {
                    if (paid != 0 && balance != 0)
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].BackColor = ColorTranslator.FromHtml("#ff6666");
                    else if (paid == 0 && balance != 0)
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].BackColor = ColorTranslator.FromHtml("#ff6666");
                    else if (paid != 0 && balance == 0)
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].BackColor = ColorTranslator.FromHtml("#99ebff");
                }
                #endregion
            }
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(9, FarPoint.Web.Spread.Model.MergePolicy.Always);
            #endregion

            #region Grandtotal
            double grandTotal = 0;
            FpSpread1.Sheets[0].PageSize = dsval.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.GreenYellow;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int i = 10; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                double.TryParse(Convert.ToString(gdtot[i]), out grandTotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
            }

            #endregion
            FpSpread1.Sheets[0].SelectionBackColor = Color.Green;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            divcol.Visible = true;
            FpSpread1.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            print.Visible = true;

        }
        catch { }
    }
    #endregion

    #region Column order

    protected void loadcolorder()
    {
        cblcolorder.Items.Clear();
        if (rbtransport.Checked == true)
        {
            cblcolorder.Items.Add(new ListItem("Student Name", "1"));
            cblcolorder.Items.Add(new ListItem("Roll No", "2"));
            cblcolorder.Items.Add(new ListItem("Reg No", "3"));
            cblcolorder.Items.Add(new ListItem("Admission No", "4"));
            cblcolorder.Items.Add(new ListItem("Batch Year ", "5"));
            cblcolorder.Items.Add(new ListItem(lbldept.Text, "6"));
            cblcolorder.Items.Add(new ListItem(lblsem.Text, "7"));
            if (rbtransport.Checked == true)
                cblcolorder.Items.Add(new ListItem("Stage", "8"));
            //else
            //    cblcolorder.Items.Add(new ListItem("Room", "7"));

            cblcolorder.Items.Add(new ListItem("Allot", "9"));
            cblcolorder.Items.Add(new ListItem("Paid", "10"));
            cblcolorder.Items.Add(new ListItem("Balance", "11"));
        }
        else
        {
            if (rbhostel.Checked == true)
            {
                cblcolorder.Items.Add(new ListItem("Student Name", "1"));
                cblcolorder.Items.Add(new ListItem("Roll No", "2"));
                cblcolorder.Items.Add(new ListItem("Reg No", "3"));
                cblcolorder.Items.Add(new ListItem("Admission No", "4"));
                cblcolorder.Items.Add(new ListItem("Batch Year ", "5"));
                cblcolorder.Items.Add(new ListItem(lbldept.Text, "6"));
                cblcolorder.Items.Add(new ListItem(lblsem.Text, "7"));
                cblcolorder.Items.Add(new ListItem("Room", "8"));
                cblcolorder.Items.Add(new ListItem("Allot", "9"));
                cblcolorder.Items.Add(new ListItem("Paid", "10"));
                cblcolorder.Items.Add(new ListItem("Balance", "11"));
            }
            else
            {
                cblcolorder.Items.Add(new ListItem("Student Name", "1"));
                cblcolorder.Items.Add(new ListItem("Roll No", "2"));
                cblcolorder.Items.Add(new ListItem("Reg No", "3"));
                cblcolorder.Items.Add(new ListItem("Admission No", "4"));
                cblcolorder.Items.Add(new ListItem("Batch Year ", "5"));
                cblcolorder.Items.Add(new ListItem(lbldept.Text, "6"));
                cblcolorder.Items.Add(new ListItem(lblsem.Text, "7"));
                cblcolorder.Items.Add(new ListItem("Hostel", "8"));
                if (rbheader.Checked == true)
                    cblcolorder.Items.Add(new ListItem("Header", "9"));
                else
                    cblcolorder.Items.Add(new ListItem("Ledger", "9"));

                cblcolorder.Items.Add(new ListItem("Allot", "10"));
                cblcolorder.Items.Add(new ListItem("Paid", "11"));
                cblcolorder.Items.Add(new ListItem("Balance", "12"));
            }
        }

    }

    protected void cbcolorder_Changed(object sender, EventArgs e)
    {
        if (cbcolorder.Checked == true)
        {
            for (int i = 0; i < cblcolorder.Items.Count; i++)
            {
                cblcolorder.Items[i].Selected = true;
            }
        }
        else
        {

            for (int i = 0; i < cblcolorder.Items.Count; i++)
            {
                cblcolorder.Items[i].Selected = false;
            }
        }
    }

    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblcolorder.Items.Count; i++)
            {
                if (cblcolorder.Items[i].Selected == true)
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
            string linkname = "Transport and Hostel Allotment Report column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            //  string collegecode1 = ddlcollege.SelectedItem.Value.ToString();
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code in('" + collegecode + "') ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cblcolorder.Items.Count; i++)
                    {
                        if (cblcolorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolorder.Items[i].Value));
                            if (columnvalue == "")
                            {
                                columnvalue = Convert.ToString(cblcolorder.Items[i].Value);
                            }
                            else
                            {
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolorder.Items[i].Value);
                            }
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
                                {
                                    columnvalue = Convert.ToString(valuesplit[k]);
                                }
                                else
                                {
                                    columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cblcolorder.Items.Count; i++)
                {
                    cblcolorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolorder.Items[i].Value));
                    if (columnvalue == "")
                    {
                        columnvalue = Convert.ToString(cblcolorder.Items[i].Value);
                    }
                    else
                    {
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolorder.Items[i].Value);
                    }
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code in('" + collegecode + "') else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code in('" + collegecode + "') ";
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
                                for (int k = 0; k < cblcolorder.Items.Count; k++)
                                {
                                    if (val == cblcolorder.Items[k].Value)
                                    {
                                        cblcolorder.Items[k].Selected = true;
                                        count++;
                                    }
                                    if (count == cblcolorder.Items.Count)
                                    {
                                        cbcolorder.Checked = true;
                                    }
                                    else
                                    {
                                        cbcolorder.Checked = false;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
        catch { }
    }
    #endregion

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void DeptAcr()
    {
        string AcrVal = d2.GetFunction("select value from Master_Settings where settings='Finance Include Department Acronym'  and usercode='" + usercode + "'");
        if (AcrVal == "1")
            deptacr = true;
        else
            deptacr = false;

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
        lbl.Add(lbl_str1);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

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

    protected void spreadColumnVisible(bool flg)
    {
        try
        {
            //if (flg == true)
            //{
            if (roll == 0)
            {
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = false;
                FpSpread1.Columns[4].Visible = false;

            }
            else if (roll == 3)
            {
                FpSpread1.Columns[2].Visible = false;
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = false;
            }
            else if (roll == 4)
            {
                FpSpread1.Columns[2].Visible = false;
                FpSpread1.Columns[3].Visible = false;
                FpSpread1.Columns[4].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = false;
            }
            else if (roll == 6)
            {
                FpSpread1.Columns[2].Visible = false;
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = false;
                FpSpread1.Columns[4].Visible = true;
            }
            // }
        }
        catch { }
    }

    #endregion

    // last modified 04-10-2016 sudhagar
}