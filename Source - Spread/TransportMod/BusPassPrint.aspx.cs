//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Data;
//using System.Drawing;
//using System.Collections;
//using System.Text;
//using Gios.Pdf;
//using System.IO;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class BusPassPrint : System.Web.UI.Page
{

    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static byte roll = 0;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("TransportIndex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/TransportMod/TransportIndex.aspx");
                return;
            }
        }

        usercode = Session["usercode"].ToString();
        // collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            // setLabelText();
            bindCollege();
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            bindBtch();
            binddeg();
            binddept();
            //  bindsem();
            binddesg();
            bindstafdept();
            bindroute();
            bindvechileid();
            loadvechilestage();
            rblType_Selected(sender, e);
            Txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txtfromdate.Attributes.Add("readonly", "readonly");
            Txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txttodate.Attributes.Add("readonly", "readonly");
        }
        if (cblclg.Items.Count > 0)
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));


    }

    #region college
    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblclg.DataSource = ds;
            cblclg.DataTextField = "collname";
            cblclg.DataValueField = "college_code";
            cblclg.DataBind();
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    cblclg.Items[row].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        //bindheader();
        //loadpaid();
        //loadfinanceUser();
        //columnType();
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        // rblMemType_Selected(sender, e);
        //bindheader();
        //loadpaid();
        //loadfinanceUser();
        //columnType();
    }
    #endregion
  
    //student
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
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            string stream = "";
            //if (ddlstream.Items.Count > 0)
            //{
            //    if (ddlstream.SelectedItem.Text != "")
            //    {
            //        stream = ddlstream.SelectedItem.Text.ToString();
            //    }
            //}

            cbl_degree.Items.Clear();
            //  string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            //string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code in('" + collegecode + "')";
            string selqry = "select distinct  c.Course_Name  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code in('" + collegecode + "')";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_name";
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
            string batch2 = "";
            string degree = "";
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            batch2 = "";
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

            degree = "";
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
                        degree = degree + "'" + "," + "'" + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            // string collegecode = ddlcollege.SelectedItem.Value.ToString();
            if (batch2 != "" && degree != "")
            {
                // ds.Clear();
                //  ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                //string sel = "  select dt.Dept_Name,d.degree_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.Course_Id in('" + degree + "') and d.college_code in('" + collegecode + "')";
                //string strquery1 = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + degree + "') and degree.college_code in('" + collegecode + "')  and deptprivilages.Degree_code=degree.Degree_code ";
                ds = BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "dept_name";
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
            //bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            //bindsem();
        }
        catch { }
    }
    #endregion

    #region sem
    //protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
    //    }
    //    catch (Exception ex)
    //    { }
    //}
    //protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
    //    }
    //    catch (Exception ex)
    //    { }

    //}

    //protected void bindsem()
    //{
    //    try
    //    {
    //        cbl_sem.Items.Clear();
    //        cb_sem.Checked = false;
    //        txt_sem.Text = "--Select--";
    //        ds.Clear();
    //        string linkName = string.Empty;
    //        string cbltext = string.Empty;
    //        ds = d2.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_sem.DataSource = ds;
    //            cbl_sem.DataTextField = "TextVal";
    //            cbl_sem.DataValueField = "TextCode";
    //            cbl_sem.DataBind();

    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    cbltext = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                    txt_sem.Text = "" + linkName + "(" + cbltext + ")";
    //                else
    //                    txt_sem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
    //                cb_sem.Checked = true;
    //            }
    //        }
    //    }
    //    catch { }
    //}

    //protected void bindsem()
    //{
    //    try
    //    {
    //        string sem = "";
    //        string clgvalue = ddlcollege.SelectedItem.Value.ToString();
    //        if (rball.Checked == true)
    //        {
    //            string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code in('" + collegecode + "') order by len(textval),textval asc";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "TextVal";
    //                cbl_sem.DataValueField = "TextCode";
    //                cbl_sem.DataBind();
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                {
    //                    txt_sem.Text = "SemesterandYear(" + sem + ")";
    //                }
    //                else
    //                {
    //                    txt_sem.Text = "SemesterandYear(" + cbl_sem.Items.Count + ")";
    //                }
    //                cb_sem.Checked = true;
    //            }
    //        }
    //        else if (rbsem.Checked == true)
    //        {
    //            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code in('" + collegecode + "') order by len(textval),textval asc";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "TextVal";
    //                cbl_sem.DataValueField = "TextCode";
    //                cbl_sem.DataBind();
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                {
    //                    txt_sem.Text = "Semester(" + sem + ")";
    //                }
    //                else
    //                {
    //                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                }
    //                cb_sem.Checked = true;
    //                spsem.Text = "Semester";
    //            }
    //        }
    //        else if (rbyear.Checked == true)
    //        {
    //            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code in('" + collegecode + "') order by len(textval),textval asc";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "TextVal";
    //                cbl_sem.DataValueField = "TextCode";
    //                cbl_sem.DataBind();
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                {
    //                    txt_sem.Text = "Year(" + sem + ")";
    //                }
    //                else
    //                {
    //                    txt_sem.Text = "Year(" + cbl_sem.Items.Count + ")";
    //                }
    //                cb_sem.Checked = true;
    //                spsem.Text = "Year";
    //            }
    //        }
    //    }
    //    catch { }
    //}
    #endregion

    //staff
    #region desgination

    public void binddesg()
    {
        try
        {
            cbldesg.Items.Clear();
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            ds.Clear();
            string selqry = "select desig_code,desig_name from desig_master where collegeCode in('" + collegecode + "')";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldesg.DataSource = ds;
                cbldesg.DataTextField = "desig_name";
                cbldesg.DataValueField = "desig_code";
                cbldesg.DataBind();
                if (cbldesg.Items.Count > 0)
                {
                    for (int i = 0; i < cbldesg.Items.Count; i++)
                    {
                        cbldesg.Items[i].Selected = true;
                    }
                    txtdesg.Text = "Desgination(" + cbldesg.Items.Count + ")";
                    cbdesg.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cbdesg_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbdesg, cbldesg, txtdesg, "Desgination", "--Select--");
            //  binddept();
        }
        catch { }
    }
    protected void cbldesg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbdesg, cbldesg, txtdesg, "Desgination", "--Select--");
            // binddept();
        }
        catch { }
    }
    #endregion

    #region departemnt

    public void bindstafdept()
    {
        try
        {
            cblstafdept.Items.Clear();
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            ds.Clear();
            string selqry = "select distinct dept_code,dept_name from hrdept_master where college_code in('" + collegecode + "')";

            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstafdept.DataSource = ds;
                cblstafdept.DataTextField = "dept_name";
                cblstafdept.DataValueField = "dept_code";
                cblstafdept.DataBind();
                if (cblstafdept.Items.Count > 0)
                {
                    for (int i = 0; i < cblstafdept.Items.Count; i++)
                    {
                        cblstafdept.Items[i].Selected = true;
                    }
                    txtstafdept.Text = "Departemnt(" + cblstafdept.Items.Count + ")";
                    cbstafdept.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cbstafdept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbstafdept, cblstafdept, txtstafdept, "Department", "--Select--");
            //  binddept();
        }
        catch { }
    }
    protected void cblstafdept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbstafdept, cblstafdept, txtstafdept, "Department", "--Select--");
            // binddept();
        }
        catch { }
    }
    #endregion

    #region Route

    public void bindroute()
    {
        try
        {
            cblroute.Items.Clear();
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            ds.Clear();
            string selqry = "select distinct Route_ID from routemaster order by Route_ID";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblroute.DataSource = ds;
                cblroute.DataTextField = "Route_ID";
                cblroute.DataValueField = "Route_ID";
                cblroute.DataBind();
                if (cblroute.Items.Count > 0)
                {
                    for (int i = 0; i < cblroute.Items.Count; i++)
                    {
                        cblroute.Items[i].Selected = true;
                    }
                    txtroute.Text = "Route ID(" + cblroute.Items.Count + ")";
                    cbroute.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cbroute_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbroute, cblroute, txtroute, "Route", "--Select--");
            //  binddept();
            bindvechileid();
            loadvechilestage();
        }
        catch { }
    }
    protected void cblroute_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbroute, cblroute, txtroute, "Route", "--Select--");
            // binddept();
            loadvechilestage();
            bindvechileid();
        }
        catch { }
    }
    #endregion

    #region vechile id

    public void bindvechileid()
    {
        try
        {
            cblvechile.Items.Clear();
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string route = "";
            for (int i = 0; i < cblroute.Items.Count; i++)
            {
                if (cblroute.Items[i].Selected == true)
                {
                    if (route == "")
                    {
                        route = Convert.ToString(cblroute.Items[i].Value);
                    }
                    else
                    {
                        route += "','" + Convert.ToString(cblroute.Items[i].Value);
                    }
                }
            }
            ds.Clear();
            string selqry = "select * from vehicle_master  where route in('" + route + "') order by len(veh_id), Veh_ID ";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblvechile.DataSource = ds;
                cblvechile.DataTextField = "Veh_ID";
                cblvechile.DataValueField = "Veh_ID";
                cblvechile.DataBind();
                if (cblvechile.Items.Count > 0)
                {
                    for (int i = 0; i < cblvechile.Items.Count; i++)
                    {
                        cblvechile.Items[i].Selected = true;
                    }
                    txtvechile.Text = "Vechile ID(" + cblvechile.Items.Count + ")";
                    cbvechile.Checked = true;
                }
                loadvechilestage();
            }
            else
            {
                txtvechile.Text = "Select";
                cbvechile.Checked = false;
            }

        }
        catch { }
    }
    protected void cbvechile_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbvechile, cblvechile, txtvechile, "Vechile ID", "--Select--");
            //  binddept();
            loadvechilestage();
        }
        catch { }
    }
    protected void cblvechile_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbvechile, cblvechile, txtvechile, "Vechile ID", "--Select--");
            // binddept();
            loadvechilestage();
        }
        catch { }
    }
    #endregion

    #region Stage

    //public void bindstage()
    //{
    //    try
    //    {
    //        cblstage.Items.Clear();
    //        string clgvalue = ddlcollege.SelectedItem.Value.ToString();
    //        ds.Clear();
    //        string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
    //        //if (stream != "")
    //        //{
    //        //    selqry = selqry + " and type  in('" + stream + "')";
    //        //}
    //        ds = d2.select_method_wo_parameter(selqry, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cblstage.DataSource = ds;
    //            cblstage.DataTextField = "course_name";
    //            cblstage.DataValueField = "course_id";
    //            cblstage.DataBind();
    //            if (cblstage.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cblstage.Items.Count; i++)
    //                {
    //                    cblstage.Items[i].Selected = true;
    //                }
    //                txtstage.Text = "Stage(" + cblstage.Items.Count + ")";
    //                cbstage.Checked = true;
    //            }
    //        }

    //    }
    //    catch { }
    //}
    public void loadvechilestage()
    {
        string sqlquery = string.Empty;
        string filter = "";
        cblstage.Items.Clear();
        //   cblstage.Items.Insert(0, new ListItem("All", "-1"));
        string route = "";
        for (int i = 0; i < cblroute.Items.Count; i++)
        {
            if (cblroute.Items[i].Selected == true)
            {
                if (route == "")
                {
                    route = Convert.ToString(cblroute.Items[i].Value);
                }
                else
                {
                    route += "','" + Convert.ToString(cblroute.Items[i].Value);
                }
            }
        }
        string vechile = "";
        for (int i = 0; i < cblvechile.Items.Count; i++)
        {
            if (cblvechile.Items[i].Selected == true)
            {
                if (vechile == "")
                {
                    vechile = Convert.ToString(cblvechile.Items[i].Value);
                }
                else
                {
                    vechile += "','" + Convert.ToString(cblvechile.Items[i].Value);
                }
            }
        }

        if (route != "-1")
        {
            filter = " and v.Route in('" + route + "')";
        }
        if (vechile != "-1")
        {
            filter = filter + ' ' + "and r.Veh_ID in('" + vechile + "')";
        }

        sqlquery = "select distinct Stage_Name from routemaster r,vehicle_master v where Stage_Name is not null and Stage_Name<>'' and v.Veh_ID=r.Veh_ID " + filter + "";

        ds = d2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Boolean e1 = isNumeric(ds.Tables[0].Rows[i]["Stage_Name"].ToString(), System.Globalization.NumberStyles.Integer);
                if (e1)
                {
                    string Get_Stage = d2.GetFunction("select distinct Stage_Name from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    string Get_Stage_id = d2.GetFunction("select distinct Stage_id from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    cblstage.Items.Add(new ListItem(Get_Stage, Get_Stage_id));//Added By SRinath 8/10/2013
                }
                else
                {
                    cblstage.Items.Add(ds.Tables[0].Rows[i]["Stage_Name"].ToString());
                }

            }
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

    #region vehicle type

    //public void vehicleType()
    //{
    //    try
    //    {
    //        cblvehtype.Items.Clear();
    //        cbvehtype.Checked = false;
    //        txtvehtype.Text = "--Select--";
    //        cblvehtype.Items.Add(new ListItem("Own", "0"));
    //        cblvehtype.Items.Add(new ListItem("Dealer", "1"));
    //        if (cblroute.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cblvehtype.Items.Count; i++)
    //            {
    //                cblvehtype.Items[i].Selected = true;
    //            }
    //            txtvehtype.Text = "Vehicle Type(" + cblvehtype.Items.Count + ")";
    //            cbvehtype.Checked = true;
    //        }
    //    }
    //    catch { }
    //}
    //protected void cbvehtype_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    CallCheckboxChange(cbvehtype, cblvehtype, txtvehtype, "Vehicle Type", "--Select--");
    //}
    //protected void cblvehtype_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    CallCheckboxListChange(cbvehtype, cblvehtype, txtvehtype, "Vehicle Type", "--Select--");
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
    public DataSet BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        DataSet dsBranch = new DataSet();
        try
        {
            if (course_id.ToString().Trim() != "")
            {
                if (singleuser == "True")
                {

                    string strquery = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and course.course_name in('" + course_id + "') and degree.college_code in ('" + collegecode + "')  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
                    dsBranch = d2.select_method_wo_parameter(strquery, "Text");


                }
                else
                {

                    string strquery1 = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and course.course_name in(" + course_id + ") and degree.college_code in(" + collegecode + " ) and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc";
                    dsBranch = d2.select_method_wo_parameter(strquery1, "Text");
                }
            }

        }
        catch { }
        return dsBranch;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Reset();
        ds = loadDetails();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadSpreadDetails(ds);
        }
        else
        {
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            print.Visible = false;
            tdPrint.Visible = false;
            tdDate.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }
    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            string SelQ = string.Empty;
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            degree = getDegreeCode(degree, collegecode);
            string routeid = Convert.ToString(getCblSelectedValue(cblroute));
            string vechileid = Convert.ToString(getCblSelectedValue(cblvechile));
            string stageid = Convert.ToString(getCblSelectedValue(cblstage));
            string stafdesg = Convert.ToString(getCblSelectedValue(cbldesg));
            string stafdept = Convert.ToString(getCblSelectedValue(cblstafdept));
            string rptType = string.Empty;



            //Added by Rajasekar 12/06/2018
            string fromdate = string.Empty;
            string todate = string.Empty;
            fromdate = Txtfromdate.Text;
            todate = Txttodate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

            string HeaderFK = string.Empty;
            string LedgerFK = string.Empty;
            //string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            //DataSet dsAppNOFeeCat = new DataSet();
            string hdFK = string.Empty;
            string ldFK = string.Empty;
            DataSet dsBalAmt = new DataSet();
            string selQ = " select LinkValue,college_code from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code in('" + collegecode + "')";
            DataSet dsVal1 = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal1.Tables.Count > 0 && dsVal1.Tables[0].Rows.Count > 0)
            {
                for (int row1 = 0; row1 < dsVal1.Tables[0].Rows.Count; row1++)
                {
                    string linkValue = Convert.ToString(dsVal1.Tables[0].Rows[row1]["LinkValue"]);
                    string clgcode = Convert.ToString(dsVal1.Tables[0].Rows[row1]["college_code"]);
                    string[] leng = linkValue.Split(',');
                    if (leng.Length == 2)
                    {
                        hdFK = Convert.ToString(leng[0]);
                        HeaderFK += "'" + "," + "'" + hdFK;
                        ldFK = Convert.ToString(leng[1]);
                        LedgerFK += "'" + "," + "'" + ldFK;
                    }
                }
            }


            switch (rblType.SelectedIndex)
            {
                case 0:
                    //SelQ = "select distinct r.app_no, roll_no[Roll No],r.reg_no [Reg No],roll_admit[Admission No],stud_name[Student Name],vehid,route,sm.stage_name,r.college_code,r.seat_no,'1' as type,r.Current_Semester [Current_Semester] from registration r,routemaster rm,stage_master sm,vehicle_master vm where cast(rm.stage_name as int)=sm.stage_id and vm.route=rm.route_id and r.vehid=vm.veh_id and r.bus_routeid=rm.route_id and cast(r.boarding as int)=cast(rm.stage_name as int) and cast(r.boarding as int)=sm.stage_id AND   Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>''  and r.cc=0 and r.delflag=0 and r.exam_flag!='debar' ";
                    //if (!string.IsNullOrEmpty(batch))
                    //    SelQ += " and r.batch_year in('" + batch + "')";
                    //if (!string.IsNullOrEmpty(degree))
                    //    SelQ += " and r.degree_code in('" + degree + "')";

                    //if (!string.IsNullOrEmpty(batch))
                    //    SelQ += " and r.VehID in('" + vechileid + "')";
                    //if (!string.IsNullOrEmpty(degree))
                    //    SelQ += " and r.Boarding in('" + stageid + "')";
                    //if (!string.IsNullOrEmpty(batch))
                    //    SelQ += " and r.Bus_RouteID in('" + routeid + "')";
                    //if (ddlRptType.Items.Count > 0)
                    //{
                    //    if (ddlRptType.SelectedIndex == 0)
                    //    {
                    //        SelQ += " and r.roll_no in(select distinct roll_no from Print_Tracker) ";
                    //    }
                    //    else
                    //        SelQ += " and r.roll_no not in(select distinct roll_no from Print_Tracker)";
                    //}
                    SelQ = "select distinct r.app_no, roll_no[Roll No],r.reg_no [Reg No],roll_admit[Admission No],stud_name[Student Name],vehid,route,sm.stage_name,r.college_code,r.seat_no,'1' as type,r.Current_Semester [Current_Semester] from registration r,routemaster rm,stage_master sm,vehicle_master vm where cast(rm.stage_name as int)=sm.stage_id and vm.route=rm.route_id and r.vehid=vm.veh_id and r.bus_routeid=rm.route_id and cast(r.boarding as int)=cast(rm.stage_name as int) and cast(r.boarding as int)=sm.stage_id AND   Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>''  and r.cc=0 and r.delflag=0 and r.exam_flag!='debar' ";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and r.batch_year in('" + batch + "')";
                    if (!string.IsNullOrEmpty(degree))
                        SelQ += " and r.degree_code in('" + degree + "')";

                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and r.VehID in('" + vechileid + "')";
                    if (!string.IsNullOrEmpty(degree))
                        SelQ += " and r.Boarding in('" + stageid + "')";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and r.Bus_RouteID in('" + routeid + "')";
                    if (ddlRptType.Items.Count > 0)
                    {
                        if (ddlRptType.SelectedIndex == 0)
                        {
                            SelQ += " and r.roll_no in(select distinct roll_no from Print_Tracker) ";
                        }
                        else
                            SelQ += " and r.roll_no not in(select distinct roll_no from Print_Tracker)";
                    }
                    //SelQ += "and ft.App_No=r.App_No  and ft.headerfk in('" + HeaderFK + "') and ft.ledgerfk in('" + LedgerFK + "') order by ft.TransDate,ft.TransCode  ";//rajasekar 13/11/2018
                    break;
                //=================================//
                case 1:
                    SelQ = "select distinct sal.appl_id as app_no,stm.staff_code[Roll No],stm.staff_code [Reg No],stm.staff_code[Admission No],stm.staff_name[Student Name],vehid,route,sm.stage_name,stm.college_code,stm.seat_no,'2' as type from staffmaster stm ,stafftrans st ,hrdept_master hr ,desig_master dm,routemaster rm,stage_master sm,vehicle_master vm ,staff_appl_master sal where stm.appl_no=sal.appl_no and  cast(rm.stage_name as int)=sm.stage_id and vm.route=rm.route_id and stm.vehid=vm.veh_id and stm.bus_routeid=rm.route_id and cast(stm.boarding as int)=cast(rm.stage_name as int) and cast(stm.boarding as int)=sm.stage_id AND   Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' ";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and  dm.desig_code in('" + stafdesg + "')";
                    if (!string.IsNullOrEmpty(degree))
                        SelQ += "  and hr.dept_code in(' " + stafdept + "')";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and stm.VehID in('" + vechileid + "')";
                    if (!string.IsNullOrEmpty(degree))
                        SelQ += " and stm.Boarding in('" + stageid + "')";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and stm.Bus_RouteID in('" + routeid + "')";
                    if (ddlRptType.Items.Count > 0)
                    {
                        if (ddlRptType.SelectedIndex == 0)
                            SelQ += " and stm.staff_code in(select distinct roll_no from Print_Tracker)";
                        else
                            SelQ += " and stm.staff_code not in(select distinct roll_no from Print_Tracker)";
                    }
                    break;
                case 2:
                    SelQ = "select distinct r.app_no,roll_no[Roll No],r.reg_no [Reg No],roll_admit[Admission No],stud_name[Student Name],vehid,route,sm.stage_name,r.college_code,r.seat_no,'1' as type,r.Current_Semester [Current_Semester] from registration r,routemaster rm,stage_master sm,vehicle_master vm where cast(rm.stage_name as int)=sm.stage_id and vm.route=rm.route_id and r.vehid=vm.veh_id and r.bus_routeid=rm.route_id and cast(r.boarding as int)=cast(rm.stage_name as int) and cast(r.boarding as int)=sm.stage_id AND   Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' and r.cc=0 and r.delflag=0 and r.exam_flag!='debar' ";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and r.batch_year in('" + batch + "')";
                    if (!string.IsNullOrEmpty(degree))
                        SelQ += " and r.degree_code in('" + degree + "')";

                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and r.VehID in('" + vechileid + "')";
                    if (!string.IsNullOrEmpty(degree))
                        SelQ += " and r.Boarding in('" + stageid + "')";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and r.Bus_RouteID in('" + routeid + "')";
                    if (ddlRptType.Items.Count > 0)
                    {
                        if (ddlRptType.SelectedIndex == 0)
                            SelQ += " and r.roll_no in(select distinct roll_no from Print_Tracker)";
                        else
                            SelQ += " and r.roll_no not in(select distinct roll_no from Print_Tracker)";
                    }
                    SelQ += " union select distinct sal.appl_id as app_no,stm.staff_code[Roll No],stm.staff_code [Reg No],stm.staff_code[Admission No],stm.staff_name[Student Name],vehid,route,sm.stage_name,stm.college_code,stm.seat_no,'2' as type,'staff' as Current_Semester  from staffmaster stm ,stafftrans st ,hrdept_master hr ,desig_master dm,routemaster rm,stage_master sm,vehicle_master vm,staff_appl_master sal where stm.appl_no=sal.appl_no and  cast(rm.stage_name as int)=sm.stage_id and vm.route=rm.route_id and stm.vehid=vm.veh_id and stm.bus_routeid=rm.route_id and cast(stm.boarding as int)=cast(rm.stage_name as int) and cast(stm.boarding as int)=sm.stage_id AND   Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' ";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and  dm.desig_code in('" + stafdesg + "')";
                    if (!string.IsNullOrEmpty(degree))
                        SelQ += "  and hr.dept_code in( '" + stafdept + "')";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and stm.VehID in('" + vechileid + "')";
                    if (!string.IsNullOrEmpty(degree))
                        SelQ += " and stm.Boarding in('" + stageid + "')";
                    if (!string.IsNullOrEmpty(batch))
                        SelQ += " and stm.Bus_RouteID in('" + routeid + "')";
                    if (ddlRptType.Items.Count > 0)
                    {
                        if (ddlRptType.SelectedIndex == 0)
                            SelQ += " and stm.staff_code in(select distinct roll_no from Print_Tracker)";
                        else
                            SelQ += " and stm.staff_code not in(select distinct roll_no from Print_Tracker)";
                    }
                    break;
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { }
        return dsload;
    }
    protected void rblType_Selected(object sender, EventArgs e)
    {
        try
        {
            tdstaf.Visible = false;
            trstud.Visible = false;
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            print.Visible = false;
            tdPrint.Visible = false;
            tdDate.Visible = false;
            if (rblType.SelectedIndex == 0)
            {
                trstud.Visible = true;
            }
            else if (rblType.SelectedIndex == 1)
            {
                tdstaf.Visible = true;
            }

        }
        catch { }
    }
    protected ArrayList getColumn()
    {
        ArrayList arCol = new ArrayList();
        try
        {
            arCol.Add("Sno");
            arCol.Add("Select");
            arCol.Add("Roll No");
            arCol.Add("Reg No");
            arCol.Add("Admission No");
            arCol.Add("Student Name");
            arCol.Add("Route Id");
            arCol.Add("Vehicle Id");
            arCol.Add("Stage");
        }
        catch { }
        return arCol;
    }

    #region Added by saranya on 22/12/2017 for getting feecategory

    protected string semester(string CurSem, string collegecode)
    {
        string curFeeCat = string.Empty;
        string feecategory = string.Empty;
        string year = string.Empty;
        string YearWiseFee = string.Empty;
        string YearWiseFeecat = "";
        string CurSemVal = CurSem + " Semester";
        curFeeCat = d2.GetFunction("select textcode from textvaltable where textval='" + CurSemVal + "' and college_code ='" + collegecode + "'");
        if (feecategory == "")
            feecategory = curFeeCat;
        //else
        //    feecategory += "'" + "," + "'" + curFeeCat;
        //Year Wise
        switch (CurSem)
        {
            case "1":
            case "2":
                year = "1 Year";
                break;

            case "3":
            case "4":
                year = "2 Year";
                break;
            case "5":
            case "6":
                year = "3 Year";
                break;
            case "7":
            case "8":
                year = "4 Year";
                break;
        }
        YearWiseFeecat = d2.GetFunction("select distinct textcode from textvaltable where textval ='" + year + "' and college_code ='" + collegecode + "'");
        if (!YearWiseFee.Contains(year))
        {
            if (feecategory == "")
                feecategory = YearWiseFeecat;
            else
                feecategory += "'" + "," + "'" + YearWiseFeecat;
        }
        return feecategory;
    }
    #endregion

    protected void loadSpreadDetails(DataSet dsVal)
    {
        try
        {
            spreadDet.SaveChanges();
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = false;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            ArrayList arColumn = getColumn();
            string fromdate = string.Empty;
            string todate = string.Empty;
            fromdate = Txtfromdate.Text;
            todate = Txttodate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();


            foreach (string columN in arColumn)
            {
                spreadDet.Sheets[0].ColumnCount++;
                int colCnt = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = columN;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colCnt].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colCnt].Width = 40;
                switch (columN)
                {
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[colCnt].Width = 115;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[colCnt].Width = 115;
                        break;
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[colCnt].Width = 115;
                        break;
                    case "Stage":
                        spreadDet.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Left;
                        spreadDet.Sheets[0].Columns[colCnt].Width = 200;
                        break;

                    case "Student Name":
                        spreadDet.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Left;
                        spreadDet.Sheets[0].Columns[colCnt].Width = 250;
                        break;
                    case "Route Id":
                        spreadDet.Sheets[0].Columns[colCnt].Width = 80;
                        break;
                    case "Vehicle Id":
                        spreadDet.Sheets[0].Columns[colCnt].Width = 80;
                        break;
                }
            }
            spreadColumnVisible();
            int height = 0;
            int rowNum = 0;
            FarPoint.Web.Spread.CheckBoxCellType cbAll = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            cb.AutoPostBack = false;
            cbAll.AutoPostBack = true;
            FarPoint.Web.Spread.TextCellType txtRoll = new FarPoint.Web.Spread.TextCellType();
            bool boolCheck = false;

            //==========================Added by saranya on 21/12/2017========================================//
            string HeaderFK = string.Empty;
            string LedgerFK = string.Empty;
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            //DataSet dsAppNOFeeCat = new DataSet();
            string hdFK = string.Empty;
            string ldFK = string.Empty;
            DataSet dsBalAmt = new DataSet();
            string selQ = " select LinkValue,college_code from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code in('" + collegecode + "')";
            DataSet dsVal1 = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal1.Tables.Count > 0 && dsVal1.Tables[0].Rows.Count > 0)
            {
                for (int row1 = 0; row1 < dsVal1.Tables[0].Rows.Count; row1++)
                {
                    string linkValue = Convert.ToString(dsVal1.Tables[0].Rows[row1]["LinkValue"]);
                    string clgcode = Convert.ToString(dsVal1.Tables[0].Rows[row1]["college_code"]);
                    string[] leng = linkValue.Split(',');
                    if (leng.Length == 2)
                    {
                        hdFK = Convert.ToString(leng[0]);
                        HeaderFK += "'" + "," + "'" + hdFK;
                        ldFK = Convert.ToString(leng[1]);
                        LedgerFK += "'" + "," + "'" + ldFK;
                    }
                }
            }
            //===================================================================================//
            ArrayList app_noArr = new ArrayList();//Added by Rajasekar 12/06/2018
            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
            {
                //====================Added By Saranya on 21/12/2017==========================//
                Boolean freepass = false;
                string BalanceAmt = "0";
                double debitamt = 0;
                double totfeeamt=0;
                if (rblType.SelectedIndex == 0 || rblType.SelectedIndex == 2)
                {
                    string CurSem = Convert.ToString(dsVal.Tables[0].Rows[row]["Current_Semester"]);
                    if (CurSem != "staff")
                    {
                        string coll_code = Convert.ToString(dsVal.Tables[0].Rows[row]["College_Code"]);
                        string feecategory = semester(CurSem, coll_code);
                        string app_no = Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]);
                        string BalAmtQuery = "";

                        BalAmtQuery = "select distinct App_No from FT_FeeAllot where App_No='" + app_no + "' and headerfk in('" + HeaderFK + "') and ledgerfk in('" + LedgerFK + "')  and feecategory in ('" + feecategory + "')  and TotalAmount =0 and DeductAmout<>0"; //and feeamount=deductamount";

                        dsBalAmt = d2.select_method_wo_parameter(BalAmtQuery, "Text");//freepass
                        if (dsBalAmt.Tables[0].Rows.Count > 0)
                            freepass = true;

                        if (freepass != true)
                        {
                            //BalAmtQuery = "select fa.paidamount,ISNULL(fa.BalAmount,'0') as BalanceAmount from FT_FinDailyTransaction ft,FT_FeeAllot fa where fa.app_no='" + app_no + "' and ft.headerfk in('" + HeaderFK + "') and ft.ledgerfk in('" + LedgerFK + "')  and ft.feecategory in ('" + feecategory + "')  and ft.headerfk=fa.headerfk and ft.ledgerfk=fa.LedgerFK and ft.FeeCategory=fa.FeeCategory and fa.App_No=ft.App_No and ft.TransDate between '" + fromdate + "' and '" + todate + "'";

                            BalAmtQuery = "select fa.paidamount,ISNULL(fa.BalAmount,'0') as BalanceAmount,ft.Debit,fa.totalamount from FT_FinDailyTransaction ft,FT_FeeAllot fa where fa.app_no='" + app_no + "' and ft.headerfk in('" + HeaderFK + "') and ft.ledgerfk in('" + LedgerFK + "')  and ft.feecategory in ('" + feecategory + "')  and ft.headerfk=fa.headerfk and ft.ledgerfk=fa.LedgerFK and ft.FeeCategory=fa.FeeCategory and fa.App_No=ft.App_No and ft.TransDate <= '" + todate + "'";

                            dsBalAmt = d2.select_method_wo_parameter(BalAmtQuery, "Text");

                            if (dsBalAmt.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsBalAmt.Tables[0].Rows.Count; i++)
                                    debitamt += Convert.ToDouble(dsBalAmt.Tables[0].Rows[i]["Debit"]);

                                totfeeamt = Convert.ToDouble(dsBalAmt.Tables[0].Rows[0]["totalamount"]);
                            }

                        }
                             
                         
                  
                        if (BalanceAmt.Contains("."))
                        {
                            BalanceAmt = BalanceAmt.Remove((BalanceAmt.Length - 3), 3);
                        }
                         
                    }
                    else
                    {
                        BalanceAmt = "0";
                    }
                }
                else
                {
                    BalanceAmt = "0";
                }
                string app_no1 = Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]);
                if (!app_noArr.Contains(app_no1))//Added by Rajasekar 12/06/2018
                {

                    if (dsBalAmt.Tables.Count > 0 && dsBalAmt.Tables[0].Rows.Count > 0 && debitamt == totfeeamt)
                    {
                        if (BalanceAmt == "0")
                        {
                            app_noArr.Add(app_no1);//Added by Rajasekar 12/06/2018
                            //====================================================================//
                            int rowCnt = 0;
                            if (!boolCheck)
                            {
                                spreadDet.Sheets[0].RowCount++;
                                rowCnt = spreadDet.Sheets[0].RowCount - 1;
                                spreadDet.Sheets[0].Cells[rowCnt, 1].CellType = cbAll;
                                boolCheck = true;
                            }
                            height += 7;
                            spreadDet.Sheets[0].RowCount++;
                            rowCnt = spreadDet.Sheets[0].RowCount - 1;
                            spreadDet.Sheets[0].Cells[rowCnt, 0].Text = Convert.ToString(++rowNum);
                            spreadDet.Sheets[0].Cells[rowCnt, 0].Tag = Convert.ToString(dsVal.Tables[0].Rows[row]["college_code"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 0].Note = Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 1].CellType = cb;
                            spreadDet.Sheets[0].Cells[rowCnt, 1].Tag = Convert.ToString(dsVal.Tables[0].Rows[row]["seat_no"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 1].Note = Convert.ToString(dsVal.Tables[0].Rows[row]["type"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 2].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["Roll No"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 2].CellType = txtRoll;
                            spreadDet.Sheets[0].Cells[rowCnt, 3].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["Reg No"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 3].CellType = txtRoll;
                            spreadDet.Sheets[0].Cells[rowCnt, 4].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["Admission No"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 4].CellType = txtRoll;
                            spreadDet.Sheets[0].Cells[rowCnt, 5].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["Student Name"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 6].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["route"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 7].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["vehid"]);
                            spreadDet.Sheets[0].Cells[rowCnt, 8].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["stage_name"]);


                            if (freepass == true)//added by rajasekar 06/08/2018
                                spreadDet.Sheets[0].Rows[rowCnt].BackColor = Color.Green;
                            

                            //spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount);
                            //spreadDet.Sheets[0].Rows[rowCnt].BackColor = Color.Green;
                            //spreadDet.Sheets[0].Rows[rowCnt].ForeColor = Color.Black;
                            //spreadDet.Sheets[0].Rows[rowCnt].Font.Bold = true;
                        }
                    }
                }
            }


            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            txtexcelname.Text = "";
            if (height > 250)
                spreadDet.Height = height;
            else
                spreadDet.Height = 250;
            spreadDet.Visible = true;
            print.Visible = true;
            spreadDet.ShowHeaderSelection = false;
            spreadDet.SaveChanges();
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            tdPrint.Visible = true;
            tdDate.Visible = true;

        }
        catch { }
    }

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
            // lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            //string counterName = getCounterName(Convert.ToString(getCblSelectedValue(cbluser)));

            txtexcelname.Text = "";
            string degreedetails = string.Empty;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            //  degreedetails = "Bus Pass Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@' + "User/Counter : " + counterName;
            //  degreedetails = "Individual Student Daybook Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "User/Counter : " + counterName;
            pagename = "BusPassPrint.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    protected string getclgAcr(string collegecode)
    {
        string strAcr = string.Empty;
        try
        {
            StringBuilder clgAcr = new StringBuilder();
            string selQ = " select collname,college_code,coll_acronymn as acr from collinfo where college_code in('" + collegecode + "')";
            DataSet dsclg = d2.select_method_wo_parameter(selQ, "Text");
            if (dsclg.Tables.Count > 0 && dsclg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsclg.Tables[0].Rows.Count; row++)
                {
                    clgAcr.Append(Convert.ToString(dsclg.Tables[0].Rows[row]["acr"]) + ",");
                }
                if (clgAcr.Length > 0)
                    clgAcr.Remove(clgAcr.Length - 1, 1);
                strAcr = Convert.ToString(clgAcr);
            }
        }
        catch { strAcr = string.Empty; }
        return strAcr;
    }
    protected string getCounterName(string userId)
    {
        string strAcr = string.Empty;
        try
        {
            StringBuilder clgAcr = new StringBuilder();
            string selQ = " select distinct  user_id as acr,user_code from usermaster where fin_user='1' and user_code in('" + userId + "')";
            DataSet dsclg = d2.select_method_wo_parameter(selQ, "Text");
            if (dsclg.Tables.Count > 0 && dsclg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsclg.Tables[0].Rows.Count; row++)
                {
                    clgAcr.Append(Convert.ToString(dsclg.Tables[0].Rows[row]["acr"]) + ",");
                }
                if (clgAcr.Length > 0)
                    clgAcr.Remove(clgAcr.Length - 1, 1);
                strAcr = Convert.ToString(clgAcr);
            }
        }
        catch { strAcr = string.Empty; }
        return strAcr;
    }
    #endregion

    protected string getDegreeCode(string degreeName, string collegecode)
    {
        string getValue = string.Empty;
        try
        {
            string[] getVal = new string[0];
            string selQFK = "select  d.degree_code as code, c.Course_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code in('" + collegecode + "')  and dt.dept_name in('" + degreeName + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref getVal, getVal.Length + 1);
                    getVal[getVal.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["code"]);
                }
                getValue = string.Join("','", getVal);
            }
        }
        catch { getValue = string.Empty; }
        return getValue;
    }

    protected void spreadDet_ButtonCommand(object sender, EventArgs e)
    {
        try
        {
            string actrow = spreadDet.Sheets[0].ActiveRow.ToString();
            string actcol = spreadDet.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (spreadDet.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(spreadDet.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 1)
                    {
                        for (int i = 1; i < spreadDet.Sheets[0].RowCount; i++)
                        {
                            spreadDet.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 0)
                    {
                        for (int i = 1; i < spreadDet.Sheets[0].RowCount; i++)
                        {
                            spreadDet.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            getBusPass();
        }
        catch (Exception ex)
        {

        }
    }
    public void getBusPass()
    {
        //pdf();
        try
        {
            string checkvalue = "";
            if (checkok() == true)
            {
                DAccess2 da = new DAccess2();
                Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

                Font header = new Font("Arial", 7, FontStyle.Bold);
                Font header1 = new Font("Arial", 7, FontStyle.Bold);
                Font Fonthead = new Font("Arial", 2, FontStyle.Bold);
                Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
                Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
                Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
                Font Fontsmall = new Font("Arial", 7, FontStyle.Regular);
                Font Fontsmalll = new Font("Arial", 6, FontStyle.Regular);
                Font FontsmallBold = new Font("Arial", 6, FontStyle.Bold);
                Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
                Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
                spreadDet.SaveChanges();
                int count = 0;

                int x = 14;
                int y = 12; int w = 241; int h = 152; int x1 = 340; int y1 = 12; int w1 = 241; int h1 = 152;
                int liney = 0;
                int liney1 = 0;
                int linex = 0;
                int linew = 300;
                int lineh = 100;
                int clgx1 = 0; int clgy1 = 0; int clgw1 = 0; int clgh1 = 0; int clgy2 = 0;
                int clgy3 = 0; int clgy4 = 0;
                int headx = 0; int heady = 0; int headw = 0; int headh = 0;
                int hrc1 = 0; int hry = 0; int hrc2 = 0; int arc1 = 0; int hrc3 = 0; int arc3 = 0; int pnc = 0;
                int logoy = 0; int logow = 0; int studimg = 0; int imgy = 0; int backy = 0; int hold = 0;
                int dd = 0;
                int spreadCNt = 0;
                for (int i = 1; i < spreadDet.Sheets[0].RowCount; i++)
                {
                    checkvalue = Convert.ToString(spreadDet.Sheets[0].Cells[i, 1].Value);
                    if (checkvalue == "1")
                    {
                        spreadCNt++;
                    }
                }

                for (int i = 1; i < spreadDet.Sheets[0].RowCount; i++)
                {
                    checkvalue = Convert.ToString(spreadDet.Sheets[0].Cells[i, 1].Value);
                    if (checkvalue == "1")
                    {

                        string collegecode = Convert.ToString(spreadDet.Sheets[0].Cells[i, 0].Tag);
                        string busRoute = Convert.ToString(spreadDet.Sheets[0].Cells[i, 6].Text);
                        string vehilId = Convert.ToString(spreadDet.Sheets[0].Cells[i, 7].Text);
                        string stageName = Convert.ToString(spreadDet.Sheets[0].Cells[i, 8].Text);
                        string app_no = Convert.ToString(Convert.ToString(spreadDet.Sheets[0].Cells[i, 0].Note));
                        string rollNo = Convert.ToString(Convert.ToString(spreadDet.Sheets[0].Cells[i, 2].Text));
                        Session["pdfapp_no"] = Convert.ToString(app_no);
                        string seatNo = Convert.ToString(Convert.ToString(spreadDet.Sheets[0].Cells[i, 1].Tag));
                        string type = Convert.ToString(Convert.ToString(spreadDet.Sheets[0].Cells[i, 1].Note));
                        string Amt = string.Empty;
                        string rcptNo = string.Empty;
                        string date = string.Empty;

                        getTransportSetting(collegecode, app_no, ref  Amt, ref rcptNo, ref date);
                        //rcptNo = Convert.ToString(spreadDet.Sheets[0].Cells[i, 12].Text);
                        //date = Convert.ToString(spreadDet.Sheets[0].Cells[i, 13].Text);
                        string fromdate = txt_fromdate.Text;
                        string todate = txt_todate.Text;
                        // string[] frdate = fromdate.Split('/');
                        //if (frdate.Length == 3)
                        //    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                        //string[] tdate = todate.Split('/');
                        //if (tdate.Length == 3)
                        //    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();



                        string strquery = "Select * from collinfo where college_code='" + collegecode + "'";
                        DataSet ds = da.select_method_wo_parameter(strquery, "Text");
                        string university = "";
                        string collname = "";
                        string address1 = "";
                        string address2 = "";
                        string address3 = "";
                        string pincode = "";
                        string affliated = "";
                        string phone = "";
                        string fax = "";
                        string email = "";
                        string website = "";
                        string category = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                            category = ds.Tables[0].Rows[0]["category"].ToString();
                            address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                            address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                            phone = ds.Tables[0].Rows[0]["phoneno"].ToString();
                            fax = ds.Tables[0].Rows[0]["faxno"].ToString();
                            email = ds.Tables[0].Rows[0]["email"].ToString();
                            website = ds.Tables[0].Rows[0]["website"].ToString();
                        }

                        count++;
                        if (count == 1)
                        {
                            mypdfpage = mydocument.NewPage();
                        }


                        //string enrolltype = Convert.ToString(ddlenroll.SelectedItem.Value);
                        //string isenro = "";
                        //if (enrolltype.Trim() == "1")
                        //{
                        //    isenro = " and is_enroll ='1'";
                        //}
                        //else
                        //{
                        //    isenro = "";
                        //}
                        PdfTextArea ptc;
                        string query = string.Empty;
                        if (type == "1")
                        {
                            query = "select parentF_Mobile,app_formno,type, a.stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,DEPT_ACRONYM AS Dept_Name,r.batch_year,mother,parent_income,motherocc,mIncome,parent_occu,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,parent_pincodep,parent_statep,visualhandy,r.roll_no from applyn a,Degree d,Department dt,Course C ,registration r where a.app_no=r.app_no and isconfirm='1' and admission_status ='1' and selection_status ='1'   and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and  a.app_no='" + app_no + "'";
                            query += " select current_semester from registration where app_no='" + app_no + "'";
                        }
                        else
                        {
                            query = ("select distinct ''type, staffmaster.staff_code as app_formno, category_code,staffmaster.appl_no as sc,staffmaster.staff_name as stud_name,staffmaster.Bus_RouteID as BisID,staffmaster.VehID as VehID,staffmaster.Boarding as Boarding,''Course_Name,hrdept_master.dept_name AS Dept_Name,desig_master.desig_name from staffmaster,stafftrans,hrdept_master ,desig_master,staff_appl_master where staffmaster.appl_no= staff_appl_master.appl_no and  hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and staffmaster.college_code = '" + collegecode + "' And staffmaster.college_code = hrdept_master.college_code and desig_master.desig_code=stafftrans.desig_code and desig_master.collegecode=hrdept_master.college_code  and Bus_RouteID is not null and Bus_RouteID<>'' and VehID is not null and  VehID<>'' and Boarding is not null and Boarding<>'' AND Seat_No is not null and staff_appl_master.appl_id='" + app_no + "' order by staffmaster.staff_name");
                        }
                        DataSet ds1 = d2.select_method_wo_parameter(query, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            string currSem = string.Empty;
                            string oddOrEvenSem = string.Empty;
                            try
                            {
                                if (type == "1" && ds1.Tables[1].Rows.Count > 0)
                                    currSem = Convert.ToString(ds1.Tables[1].Rows[0]["current_semester"]);
                                if (!string.IsNullOrEmpty(currSem))
                                    currSem = getCurSem(currSem, ref oddOrEvenSem);
                            }
                            catch { currSem = "1 Year"; }
                            linex = 14;
                            linew = 300;
                            lineh = 100;
                            clgw1 = 132;
                            clgh1 = 27;
                            clgx1 = 68;
                            headw = 132; headh = 27; headx = 68; pnc = 190;
                            hrc1 = 85; hrc2 = 67;
                            #region ///..........  /// ..........id size...............

                            if (count == 1)
                            {
                                x = 14;
                                y = 12;
                                w = 241;
                                h = 152;
                                x1 = 340;
                                y1 = 12;
                                w1 = 241;
                                h1 = 152;
                                liney = 50;
                                liney1 = 140;
                                clgy1 = 13;
                                clgy2 = 20;
                                clgy3 = 27;
                                clgy4 = 34;
                                heady = 54;
                                hrc2 = 67;
                                arc1 = 130;
                                hrc3 = 170;
                                arc3 = 215;
                                logoy = 14;
                                studimg = 78;
                                imgy = 58;
                                backy = 14;
                                hold = 140;
                            }
                            else if (count == 2)
                            {
                                x = 14;
                                y = 221;
                                w = 241;
                                h = 152;
                                x1 = 340;
                                y1 = 221;
                                w1 = 241;
                                h1 = 152;
                                liney = 259;
                                liney1 = 349;
                                clgy1 = 222;
                                clgy2 = 229;
                                clgy3 = 236;
                                clgy4 = 243;
                                heady = 265;
                                backy = 221 + 2;
                                hrc2 = 278;
                                arc1 = 130;
                                hrc3 = 170;
                                arc3 = 215;
                                studimg = 221 + 66;
                                logoy = 224;
                                hold = 349;
                                imgy = 46 + 221;
                            }
                            else if (count == 3)
                            {
                                x = 14;
                                y = 430;
                                w = 241;
                                h = 152;
                                imgy = 46 + 430;
                                x1 = 340;
                                y1 = 430;
                                w1 = 241;
                                h1 = 152;
                                liney = 468;
                                liney1 = 558;
                                clgy1 = 431;
                                clgy2 = 438;
                                clgy3 = 445;
                                clgy4 = 452;
                                heady = 476;
                                studimg = 430 + 66;
                                hrc2 = 489;
                                arc1 = 130;
                                hrc3 = 170;
                                arc3 = 215;
                                logoy = 432;
                                hold = 558;
                                backy = 430 + 2;
                            }
                            else if (count == 4)
                            {
                                x = 14;
                                imgy = 46 + 639;
                                y = 639;
                                w = 241;
                                h = 152;
                                studimg = 639 + 66;
                                x1 = 340;
                                y1 = 639;
                                w1 = 241;
                                h1 = 152;
                                liney = 677;
                                liney1 = 767;
                                clgy1 = 640;
                                clgy2 = 647;
                                clgy3 = 654;
                                clgy4 = 661;
                                heady = 683;
                                backy = 639 + 2;
                                hrc2 = 696;
                                arc1 = 130;
                                hrc3 = 170;
                                arc3 = 215;
                                logoy = 642;
                                hold = 767;
                            }
                            #endregion

                            #region
                            ///.........................................
                            PdfArea pa1 = new PdfArea(mydocument, x, y, w, h);
                            PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                            mypdfpage.Add(pr3);

                            PdfArea pa2 = new PdfArea(mydocument, x1, y1, w1, h1);
                            PdfRectangle pr22 = new PdfRectangle(mydocument, pa2, Color.Black);
                            mypdfpage.Add(pr22);


                            PdfArea pa3 = new PdfArea(mydocument, x, liney1 + 10, 240, 13);
                            if (Convert.ToString(ds1.Tables[0].Rows[0]["type"]) == "DAY")
                            {
                                //PdfRectangle pr222 = new PdfRectangle(mydocument, pa3, Color.Maroon);
                                //pr222.Fill(Color.Maroon);
                                //mypdfpage.Add(pr222);   //21/11/2017 aruna
                            }
                            else if (Convert.ToString(ds1.Tables[0].Rows[0]["type"]) == "Evening")
                            {
                                //PdfRectangle pr222 = new PdfRectangle(mydocument, pa3, Color.Green);
                                //pr222.Fill(Color.Green);
                                //mypdfpage.Add(pr222);  //21/11/2017 aruna
                            }
                            else
                            {
                                //PdfRectangle pr222 = new PdfRectangle(mydocument, pa3, Color.Maroon);
                                //pr222.Fill(Color.Maroon);
                                //mypdfpage.Add(pr222);   //21/11/2017 aruna
                            }



                            //...................................................
                            #endregion

                            #region
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                            //                                   new PdfArea(mydocument, linex, liney - 15, linew, lineh), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                            //mypdfpage.Add(ptc);   //21/11/2017 aruna

                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                            //                              new PdfArea(mydocument, linex, liney1, linew, lineh), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                            //mypdfpage.Add(ptc);  //21/11/2017 aruna


                            #endregion

                            #region ///............... college details...............
                            ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, clgx1, clgy1 - 3, clgw1 + 30, clgh1), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(collname));// + " (" + category + ")"

                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, clgx1, clgy2 - 1, clgw1 + 30, clgh1), System.Drawing.ContentAlignment.MiddleCenter, address1 + "," + address3 + "," + pincode);
                            mypdfpage.Add(ptc);

                            /// 
                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                       new PdfArea(mydocument, clgx1, clgy3, clgw1, clgh1), System.Drawing.ContentAlignment.MiddleCenter, "ph:" + phone + "," + "Fax:" + fax);
                            //mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, clgx1, clgy3, clgw1 + 30, clgh1), System.Drawing.ContentAlignment.MiddleCenter, "Email:" + email);// + "," + "Website:" + website
                            mypdfpage.Add(ptc);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg"));
                                mypdfpage.Add(LogoImage, 25, logoy, 990);

                            }

                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                ds.Dispose();
                                ds.Reset();
                                ds = d2.select_method_wo_parameter("select logo1 from collinfo where college_code='" + collegecode + "' and logo1 is not null", "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                                mypdfpage.Add(LogoImage, 25, logoy, 990);
                            }
                            #endregion

                            string studname = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                            string stud_name = studname.Length.ToString();

                            #region /// ...............stud detail......................

                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, headx + 4, heady - 12, headw + 20, headh), System.Drawing.ContentAlignment.MiddleCenter, "BUS PASS");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, hrc1, hrc2 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "NAME");
                            mypdfpage.Add(ptc);

                            if (Convert.ToInt32(stud_name) < 28)
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, arc1 + 12, hrc2 - 15, headw + 30, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]));
                                mypdfpage.Add(ptc);
                            }
                            else
                            {
                                ptc = new PdfTextArea(Fontsmalll, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mydocument, arc1 + 12, hrc2 - 15, headw + 30, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]));
                                mypdfpage.Add(ptc);
                            }



                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, hrc1, hrc2 + 13 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "ROLL NO");
                            mypdfpage.Add(ptc);


                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, arc1 + 12, hrc2 + 13 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["roll_no"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, hrc1, hrc2 + 27 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "COURSE/YEAR");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, arc1 + 12, hrc2 + 27 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]) + "-" + Convert.ToString(ds1.Tables[0].Rows[0]["Dept_Name"]) + "/" + currSem);
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, hrc1, hrc2 + 41 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "BUS ROUTE");
                            mypdfpage.Add(ptc);
                            string txt = string.Empty;
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, arc1 + 12, hrc2 + 41 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + busRoute);
                            mypdfpage.Add(ptc);

                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                   new PdfArea(mydocument, 190, hrc2 + 50 + 12, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL");
                            //mypdfpage.Add(ptc);
                            #endregion

                            #region /// ...............stud detail back......................
                            if (rcptNo == "" && Amt == "" && date == "")
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, 350, backy, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "RECEIPT NO");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 410, backy, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": Free Bus Pass" );
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, 350, backy + 12, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "PAID AMOUNT");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 410, backy + 12, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": -");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 350, backy + 24, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "RECEIPT DATE");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 410, backy + 24, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": -");
                                mypdfpage.Add(ptc);

                            }
                            else
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, 350, backy, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "RECEIPT NO");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 410, backy, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + rcptNo);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, 350, backy + 12, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "PAID AMOUNT");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 410, backy + 12, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + Amt);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 350, backy + 24, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "RECEIPT DATE");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 410, backy + 24, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + date);
                                mypdfpage.Add(ptc);
                            }

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 350, backy + 35, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "STAGE");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 410, backy + 35, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + stageName);
                            mypdfpage.Add(ptc);


                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 350, backy + 46, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "BUS PASS NO");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 410, backy + 46, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + seatNo);
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 350, backy + 57, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "VALID FOR");
                            mypdfpage.Add(ptc);
                            if (cb_oddoreven.Checked == false)//added by rajasekar 11/10/2018
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 410, backy + 57, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + fromdate + " - " + todate);
                                mypdfpage.Add(ptc);
                            }
                            else
                            {

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 410, backy + 57, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + oddOrEvenSem);
                                mypdfpage.Add(ptc);
                            }
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, 350, backy + 68, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "BUS NO");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 410, backy + 68, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ": " + vehilId);
                            mypdfpage.Add(ptc);





                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 480, hold, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "AUTHORITY SIGNATORY");
                            mypdfpage.Add(ptc);
                            #endregion

                            #region // stud pht
                            string imgPhoto = string.Empty;

                            if (imgPhoto.Trim() == string.Empty)
                            {
                                string roll = d2.GetFunction("select app_no from applyn  where app_no='" + app_no + "'");
                                MemoryStream memoryStream = new MemoryStream();
                                ds.Dispose();
                                ds.Reset();
                                ds = d2.select_method_wo_parameter("select photo from stdphoto where app_no='" + roll + "'", "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["photo"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }

                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpg")))
                            {
                                imgPhoto = HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpg");
                                PdfImage studimg1 = mydocument.NewImage(imgPhoto);
                                mypdfpage.Add(studimg1, 25, studimg - 18, 520);
                            }



                            #endregion

                            #region Insert print table
                            d2.update_method_wo_parameter("if not exists(select roll_no from Print_Tracker where roll_no='" + rollNo + "') insert into Print_Tracker(roll_no,creteria,printed) values('" + rollNo + "','Bus Pass','1')", "Text");
                            #endregion

                            #region // principal sign
                            //if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg")))
                            //{
                            //    MemoryStream memoryStream = new MemoryStream();
                            //    ds.Dispose();
                            //    ds.Reset();
                            //    ds = d2.select_method_wo_parameter("select principal_sign from collinfo where college_code='" + collegecode + "' and principal_sign is not null", "Text");
                            //    if (ds.Tables[0].Rows.Count > 0)
                            //    {
                            //        byte[] file = (byte[])ds.Tables[0].Rows[0]["principal_sign"];
                            //        memoryStream.Write(file, 0, file.Length);
                            //        if (file.Length > 0)
                            //        {
                            //            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                            //            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                            //            thumb.Save(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                            //        }
                            //        memoryStream.Dispose();
                            //        memoryStream.Close();
                            //    }
                            //}
                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg")))
                            //{
                            //    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg"));
                            //    mypdfpage.Add(LogoImage, 190, hrc2 + 40, 990);
                            //}
                            #endregion
                        }

                        //if (count == 4 || spreadCNt <= 4)
                        //{
                        //    count = 0;
                        //    mypdfpage.SaveToDocument();
                        //    spreadCNt -= 4;
                        //}

                        if (count == 4)
                        {
                            count = 0;
                            mypdfpage.SaveToDocument();
                            spreadCNt -= 4;
                        }

                        if (spreadCNt < 4)
                        {
                            if (count == spreadCNt)
                            {
                                mypdfpage.SaveToDocument();
                            }

                        }

                    }
                }

                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "BusPass" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);

                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                    Response.Flush();
                    Response.End();
                }

            }


            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Any one Student!')", true);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "BusPass"); }
    }
    protected string getCurSem(string curSem, ref string oddOrEvenSem)
    {
        string curSemVal = string.Empty;
        try
        {
            switch (curSem)
            {
                case "1":
                case "2":
                    curSemVal = "1 Year";
                    break;
                case "3":
                case "4":
                    curSemVal = "2 Year";
                    break;
                case "5":
                case "6":
                    curSemVal = "3 Year";
                    break;
                case "7":
                case "8":
                    curSemVal = "4 Year";
                    break;
                case "9":
                case "10":
                    curSemVal = "5 Year";
                    break;
                case "11":
                case "12":
                    curSemVal = "6 Year";
                    break;
                default:
                    curSemVal = "1";
                    break;

            }
            oddOrEvenSem = "Odd Semster";
            if (Convert.ToInt32(curSem) % 2 == 0)
                oddOrEvenSem = "Even Semster";
        }
        catch { }
        return curSemVal;
    }

    protected void getTransDetails(string appNo)
    {
        try
        {
        }
        catch { }
    }

    protected void getTransportSetting(string collegecode, string appNo, ref string Amt, ref string rcptNo, ref string date)
    {
        try
        {
            string fromdate = string.Empty;
            string todate = string.Empty;
            fromdate = Txtfromdate.Text;
            todate = Txttodate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            
            StringBuilder sbFeeCat = new StringBuilder();
            string feeCat = "";
            string oddOrEvenSem = string.Empty;
            
           
            // string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
           // string batch = d2.GetFunction("select batch_year from registration where app_no='" + appNo + "'");

            string cursemOrYearVal = d2.GetFunction("select  current_semester from registration where app_no='" + appNo + "'");
            //string cursemOrYearVal = d2.GetFunction("select current_semester from registration where college_code='" + collegecode + "' and batch_year in('" + batch + "') ");

            string cursemOrYear = cursemOrYearVal + " Semester";
            string semfeecat = d2.GetFunction("select textcode from textvaltable where college_code='" + collegecode + "' and textval='" + cursemOrYear + "' ");

            //Added by Rajasekar
            sbFeeCat.Append(semfeecat).Append("','");
                     
            if (!string.IsNullOrEmpty(cursemOrYearVal))
                cursemOrYearVal = getCurSem(cursemOrYearVal, ref oddOrEvenSem);
            string yearwisefeecat = d2.GetFunction("select textcode from textvaltable where college_code='" + collegecode + "' and textval='" + cursemOrYearVal + "' ");
            sbFeeCat.Append(yearwisefeecat);
            feeCat = Convert.ToString(sbFeeCat);
           //=================================//
            
            string selQ = " select LinkValue,college_code from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code in('" + collegecode + "')";
            // selQ += " select distinct headerpk,headername,collegecode from fm_headermaster where collegecode in('" + collegecode + "')";
            // selQ += " select * from ft_feeallot where app_no='" + appNo + "'";
            //selQ += " select convert(varchar(10),transdate,103) as transdate,transcode,sum(debit) as debit,headerfk,ledgerfk  from ft_findailytransaction where app_no='" + appNo + "' and feecategory in('" + feeCat + "') and transdate between '" + fromdate + "' and '" + todate + "'  group by transdate,transcode,headerfk,ledgerfk";
            selQ += " select convert(varchar(10),transdate,103) as transdate,transcode,sum(debit) as debit,headerfk,ledgerfk  from ft_findailytransaction where app_no='" + appNo + "' and feecategory in('" + feeCat + "') and transdate <='" + todate + "'  group by transdate,transcode,headerfk,ledgerfk";

            DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    string linkValue = Convert.ToString(dsVal.Tables[0].Rows[row]["LinkValue"]);
                    string clgcode = Convert.ToString(dsVal.Tables[0].Rows[row]["college_code"]);
                    string[] leng = linkValue.Split(',');
                    if (leng.Length == 2)
                    {
                        string hdFK = Convert.ToString(leng[0]);
                        string ldFK = Convert.ToString(leng[1]);
                        if (dsVal.Tables[1].Rows.Count > 0)
                        {
                            dsVal.Tables[1].DefaultView.RowFilter = "headerfk='" + hdFK + "' and ledgerfk='" + ldFK + "'";
                            DataTable dtHdName = dsVal.Tables[1].DefaultView.ToTable();
                            if (dtHdName.Rows.Count > 0)
                            {

                                for (int i = 0; i < dtHdName.Rows.Count; i++)
                                {
                                    //double.TryParse(Convert.ToString(dtHdName.Compute("Sum(debit)", "")), out Amt);
                                    if (i == 0)
                                    {
                                        Amt=Convert.ToString(dtHdName.Rows[i]["debit"]);
                                        rcptNo = Convert.ToString(dtHdName.Rows[i]["transcode"]);
                                        date = Convert.ToString(dtHdName.Rows[i]["transdate"]);
                                    }
                                    else
                                    {
                                        Amt += ","+ Convert.ToString(dtHdName.Rows[i]["debit"]);
                                        rcptNo += "," + Convert.ToString(dtHdName.Rows[i]["transcode"]);
                                        date += "," + Convert.ToString(dtHdName.Rows[i]["transdate"]);
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

    public bool checkok()
    {
        bool check = false;
        spreadDet.SaveChanges();
        try
        {
            for (int i = 1; i < spreadDet.Sheets[0].Rows.Count; i++)
            {
                byte selval = Convert.ToByte(spreadDet.Sheets[0].Cells[i, 1].Value);
                if (selval == 1)
                {
                    check = true;
                }
            }
        }
        catch { }
        return check;
    }
    public string subjectcode(string textcri, string collegecode)
    {
        string subjec_no = "";
        try
        {
            DataSet ds23 = new DataSet();
            string select_subno = "select TextVal from textvaltable where TextCode ='" + textcri + "' and college_code ='" + collegecode + "' ";
            ds23.Clear();
            ds23 = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds23.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds23.Tables[0].Rows[0]["TextVal"]);
            }

        }
        catch
        {

        }
        return subjec_no;
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

    protected void spreadColumnVisible()
    {
        try
        {
            #region student wise
            if (roll == 0)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 1)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 2)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = false;

            }
            else if (roll == 3)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = false;
            }
            else if (roll == 4)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 5)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = false;
            }
            else if (roll == 6)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 7)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = true;
            }
            #endregion
        }
        catch { }
    }

    #endregion
}