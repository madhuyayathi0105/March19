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
using System.Data.SqlClient;
using System.Configuration;
using wc = System.Web.UI.WebControls;


public partial class attndmastersettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    DAccess2 da = new DAccess2();
    //  string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = string.Empty;
    Hashtable has = new Hashtable();
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    string userCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string collegeCode = string.Empty;
    string qryCollege = string.Empty;
    string qryBatch = string.Empty;

    ReuasableMethods rs = new ReuasableMethods();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("MasterWizardMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MasterWizardMod/MasterWizardMenuIndex.aspx");
                    return;
                }
            }
            if (!IsPostBack)
            {
                bindclg();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                loadSem();
                BindSectionDetail();
                loadgrid();


            }
        }
        catch
        {
        }
    }

    public void loadgrid()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        string val = "select * from AttMasterSetting where CollegeCode='" + ddlclg.SelectedValue.ToString() + "'; select lock_hr,markatt_from,markatt_to from attendance_hrlock where college_code='" + ddlclg.SelectedValue.ToString() + "' and lockstatus=1";
        ds = da.select_method_wo_parameter(val, "text");

        dt.Columns.Add("leavetype");
        dt.Columns.Add("lblleaveval");
        dt.Columns.Add("status");
        dt.Columns.Add("displayinreport");
        dr = dt.NewRow();
        dr["leavetype"] = "Present";
        dr["lblleaveval"] = "1";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='1'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Absent";
        dr["lblleaveval"] = "2";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='2'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "On Duty";
        dr["lblleaveval"] = "3";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='3'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Sports On duty";
        dr["lblleaveval"] = "5";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='5'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "NSS";
        dr["lblleaveval"] = "6";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='6'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Medical Leave";
        dr["lblleaveval"] = "4";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='4'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Not Joined";
        dr["lblleaveval"] = "8";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='8'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Susupended";
        dr["lblleaveval"] = "9";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='9'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Leave";
        dr["lblleaveval"] = "10";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='10'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "NCC";
        dr["lblleaveval"] = "11";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='11'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Hour Suspension";
        dr["lblleaveval"] = "12";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='12'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Paper Presentation";
        dr["lblleaveval"] = "13";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='13'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Sympossiom OD";
        dr["lblleaveval"] = "14";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='14'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Cultural OD";
        dr["lblleaveval"] = "15";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='15'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Other OD";
        dr["lblleaveval"] = "16";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='16'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["leavetype"] = "Late";
        dr["lblleaveval"] = "17";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = " leaveCode='17'";
            DataView dv = ds.Tables[0].DefaultView;
            string calc = string.Empty;
            string disprpt = string.Empty;
            if (dv.Count > 0)
            {
                calc = Convert.ToString(dv[0]["CalcFlag"]);
                disprpt = Convert.ToString(dv[0]["DispText"]);
            }
            dr["status"] = calc;
            dr["displayinreport"] = disprpt;

        }
        dt.Rows.Add(dr);

        gridview1.DataSource = dt;
        gridview1.DataBind();


        DataTable dt1 = new DataTable();
        DataRow dr1 = null;

        dt1.Columns.Add("lockHour");
        dt1.Columns.Add("fromhour");
        dt1.Columns.Add("tohour");
        if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {
            for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
            {
                dr1 = dt1.NewRow();
                dr1["lockHour"] = Convert.ToString(ds.Tables[1].Rows[k]["lock_hr"]);
                dr1["fromhour"] = Convert.ToString(ds.Tables[1].Rows[k]["markatt_from"]);
                dr1["tohour"] = Convert.ToString(ds.Tables[1].Rows[k]["markatt_to"]);
                dt1.Rows.Add(dr1);
            }
        }


        gridview3.DataSource = dt1;
        gridview3.DataBind();
        gridview1.Visible = true;
        gridview3.Visible = true;
    }

    public void bindclg()
    {
        try
        {

            ddlclg.Items.Clear();
            string columnfield = string.Empty;
            string group_user = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            ddlclg.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = dsprint;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.Items.Add(" ");

            }

        }
        catch
        {
        }
    }

    #region bindheaders
    public void BindRightsBaseBatch()
    {
        try
        {
            DataSet dsBatch = new DataSet();
            userCode = string.Empty;
            string groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCode = string.Empty;
            ds.Clear();
            chkBatch.Checked = false;
            cblBatch.Items.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlclg.Items.Count > 0 && ddlclg.Visible)
            {
                collegeCode = Convert.ToString(ddlclg.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollege = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollege = " and r.college_code in(" + collegeCode + ")";
            }

            dsBatch.Clear();
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            qryBatch = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatch.Count > 0)
                    qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
            }
            string batchquery = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollege))
            {
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1  " + qryCollege + qryBatch + " order by r.Batch_Year desc";//and r.cc='0' and delflag='0' and exam_flag<>'debar'
                //ds.Clear();
                ds = da.select_method_wo_parameter(batchquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBatch.DataSource = ds;
                    cblBatch.DataTextField = "Batch_Year";
                    cblBatch.DataValueField = "Batch_Year";
                    cblBatch.DataBind();

                    checkBoxListselectOrDeselect(cblBatch, true);
                    CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblbatchyr.Text, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void binddegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            txt_degree.Text = "---Select---";


            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegeCode = ddlclg.SelectedValue.ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegeCode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
            }
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                cb_degree.Checked = true;
            }
        }
        catch
        {

        }
    }
    public void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();

            cb_branch.Checked = false;
            txt_branch.Text = "---Select---";

            string degree = "";
            for (int i1 = 0; i1 < cbl_degree.Items.Count; i1++)
            {
                if (cbl_degree.Items[i1].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i1].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i1].Value);
                    }
                }

            }

            if (cbl_degree.Items.Count > 0)
            {
                string degreecode = cbl_degree.SelectedValue.ToString();
                if (degreecode.Trim() != "")
                {
                    ds.Clear();
                    ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), degree, collegeCode = ddlclg.SelectedValue.ToString(), Session["usercode"].ToString());
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_branch.DataSource = ds;
                        cbl_branch.DataTextField = "dept_name";
                        cbl_branch.DataValueField = "degree_code";
                        cbl_branch.DataBind();
                    }
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                        cb_branch.Checked = true;
                    }
                }
            }



        }
        catch
        {

        }
    }
    public void loadSem()
    {
        ds.Clear();
        collegeCode = string.Empty;
        string valBatch = string.Empty;
        string valDegree = string.Empty;
        if (ddlclg.Items.Count > 0)
            collegeCode = ddlclg.SelectedValue.ToString().Trim();
        if (cblBatch.Items.Count > 0)
            valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
        if (cbl_branch.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(cbl_branch);


        string SelSem = string.Empty;
        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
        {
            SelSem = "select distinct r.current_semester from Registration r where  college_code='" + collegeCode + "' and batch_year in('" + valBatch + "')  and   CC='0' and DelFlag='0' and Exam_Flag<>'debar'   order by r.current_semester ";
            ds = da.select_method_wo_parameter(SelSem, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "current_semester";
                cbl_sem.DataValueField = "current_semester";
                cbl_sem.DataBind();
                checkBoxListselectOrDeselect(cbl_sem, true);
                CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
            }
        }

    }

    public void BindSectionDetail()
    {
        DataSet ds = new DataSet();
        // ddlSec.Items.Clear();
        if (cbl_branch.Items.Count > 0 && cbl_branch.Items.Count > 0)
        {
            string branch = string.Empty;
            string batch = string.Empty;
            if (cblBatch.Items.Count > 0)
                batch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cbl_branch.Items.Count > 0)
                branch = rs.GetSelectedItemsValueAsString(cbl_branch);
            //  string branch = Convert.ToString(ddlBranch.SelectedValue).Trim();
            //  string batch = Convert.ToString(ddlBatch.SelectedValue).Trim();
            string query = "select distinct sections from registration where batch_year in ('" + Convert.ToString(batch).Trim() + "') and degree_code in ('" + Convert.ToString(branch).Trim() + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblsection.DataSource = ds;
            cblsection.DataTextField = "sections";
            cblsection.DataValueField = "sections";
            cblsection.DataBind();
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() == "")
            {
                txtsection.Enabled = false;
                //  RequiredFieldValidator5.Visible = false;
            }
            else
            {
                txtsection.Enabled = true;
                //RequiredFieldValidator5.Visible = true;
            }
        }
        else
        {
            txtsection.Enabled = false;
            //   RequiredFieldValidator5.Visible = false;
        }
    }

    #endregion

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblleave = (e.Row.FindControl("lblstatus") as Label);
            string leavetypeval = lblleave.Text;
            DropDownList ddlstat = (e.Row.FindControl("ddlstatus") as DropDownList);

            ddlstat.Items.Insert(0, "Present");
            ddlstat.Items.Insert(1, "Absent");
            ddlstat.Items.Insert(2, "Not Consider");
            ddlstat.Items.Insert(3, " ");


            //string val = da.GetFunction("select CalcFlag from AttMasterSetting where CollegeCode='" + ddlclg.SelectedValue.ToString() + "' and LeaveCode='" + leavetypeval + "'");
            string leavetypetxt = string.Empty;
            if (leavetypeval == "0")
                leavetypetxt = "Present";
            else if (leavetypeval == "1")
                leavetypetxt = "Absent";
            else if (leavetypeval == "2")
                leavetypetxt = "Not Consider";

            if (string.IsNullOrEmpty(leavetypetxt))
                ddlstat.Items[3].Selected = true;
            else
                ddlstat.Items.FindByText(leavetypetxt).Selected = true;
        }
    }

    protected void gridview3_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DropDownList ddlhour = (e.Row.FindControl("ddllockhours") as DropDownList);
            DropDownList ddlfromhour = (e.Row.FindControl("ddlfromhour") as DropDownList);
            DropDownList ddltohour = (e.Row.FindControl("ddltohour") as DropDownList);

            for (int i = 1; i < 11; i++)
            {
                ddlhour.Items.Insert(i - 1, Convert.ToString(i));
                ddlfromhour.Items.Insert(i - 1, Convert.ToString(i));
                ddltohour.Items.Insert(i - 1, Convert.ToString(i));
            }
            ddlhour.Items.Insert(10, "0");
            ddlfromhour.Items.Insert(10, "0");
            ddltohour.Items.Insert(10, "0");
            ddlhour.Items.Insert(11," ");
            ddlfromhour.Items.Insert(11, " ");
            ddltohour.Items.Insert(11, " ");

            Label lblhr = e.Row.FindControl("lbllockhour") as Label;
            string hour = lblhr.Text;
            if (string.IsNullOrEmpty(hour))
                ddlhour.Items[11].Selected = true;
            else
                ddlhour.Items.FindByText(hour).Selected = true;

            Label lblfrmhr = e.Row.FindControl("lblfromhour") as Label;
            string fromhour = lblfrmhr.Text;
            if (string.IsNullOrEmpty(fromhour))
                ddlfromhour.Items[11].Selected = true;
            else
                ddlfromhour.Items.FindByText(fromhour).Selected = true;

            Label lbltohr = e.Row.FindControl("lbltohour") as Label;
            string tohour = lbltohr.Text;
            if (string.IsNullOrEmpty(tohour))
                ddltohour.Items[11].Selected = true;
            else
                ddltohour.Items.FindByText(tohour).Selected = true;



        }
    }

    protected void ddlclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        loadgrid();
        BindRightsBaseBatch();
        binddegree();
        bindbranch();
        loadSem();
        BindSectionDetail();
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }
    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblbatchyr.Text, "--Select--");
            binddegree();
            bindbranch();
            loadSem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblbatchyr.Text, "--Select--");
            binddegree();
            bindbranch();
            loadSem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
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
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
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

    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            // cb_branch.Checked = false;
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                txt_degree.Text = "Degree(" + commcount.ToString() + ")";
            }
            bindbranch();
            loadSem();
            BindSectionDetail();
        }
        catch { }
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {

                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
            }
            bindbranch();
            loadSem();
            BindSectionDetail();

        }
        catch { }
    }

    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // clear();
            int i = 0;
            cb_branch.Checked = false;
            int commcount = 0;
            txt_branch.Text = "--Select--";
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }

            loadSem();
            BindSectionDetail();


        }
        catch { }
    }
    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            //clear();
            txt_branch.Text = "--Select--";
            if (cb_branch.Checked == true)
            {

                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Department(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
            }
            loadSem();
            BindSectionDetail();


        }
        catch { }
    }

    protected void cbsection_checkedchange(object sender, EventArgs e)
    {
        txtsection.Text = "--Select--";
        if (cbsection.Checked == true)
        {

            for (int i = 0; i < cblsection.Items.Count; i++)
            {
                cblsection.Items[i].Selected = true;
            }
            txtsection.Text = "Section(" + (cblsection.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblsection.Items.Count; i++)
            {
                cblsection.Items[i].Selected = false;
            }
        }


    }
    protected void cblsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        cbsection.Checked = false;
        int commcount = 0;
        txtsection.Text = "--Select--";
        for (i = 0; i < cblsection.Items.Count; i++)
        {
            if (cblsection.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblsection.Items.Count)
            {
                cbsection.Checked = true;
            }
            txtsection.Text = "Section(" + commcount.ToString() + ")";
        }

    }

    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");

    }

    protected void cb_sem_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");


    }


    protected void ddlsection_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            //divPopAlertbackvolume.Visible = true;
            //divPopAlertback.Visible = true;
            string batch = string.Empty;
            string semester = string.Empty;
            string branch = string.Empty;
            string section = string.Empty;
            string section1 = string.Empty;
            for (int i = 0; i < cblBatch.Items.Count; i++)
            {
                if (cblBatch.Items[i].Selected == true)
                {
                    if (string.IsNullOrEmpty(batch))
                        batch = "'" + cblBatch.Items[i].Text.ToString() + "'";

                    else
                        batch = batch + ",'" + cblBatch.Items[i].Text.ToString() + "'";
                }
            }
            for (int j1 = 0; j1 < cbl_branch.Items.Count; j1++)
            {
                if (cbl_branch.Items[j1].Selected == true)
                {
                    if (string.IsNullOrEmpty(branch))
                        branch = "'" + cbl_branch.Items[j1].Value.ToString() + "'";
                    else
                        branch = branch + ",'" + cbl_branch.Items[j1].Value.ToString() + "'";
                }
            }
            for (int j = 0; j < cbl_sem.Items.Count; j++)
            {
                if (cbl_sem.Items[j].Selected == true)
                {
                    if (string.IsNullOrEmpty(semester))
                        semester = "'" + cbl_sem.Items[j].Text.ToString() + "'";
                    else
                        semester = semester + ",'" + cbl_sem.Items[j].Text.ToString() + "'";
                }
            }
            if (txtsection.Enabled == true)
            {
                for (int j2 = 0; j2 < cblsection.Items.Count; j2++)
                {
                    if (cblsection.Items[j2].Selected == true)
                    {
                        if (string.IsNullOrEmpty(section))
                            section = "'" + cblsection.Items[j2].Text.ToString() + "'";
                        else
                            section = section + ",'" + cblsection.Items[j2].Text.ToString() + "'";
                    }
                }
                section1 = " and section in (" + section + ")";
            }


            //string qry = "select distinct r.degree_code,Batch_Year,Current_Semester,d.Dept_Name,dd.degree_code,r.Sections,c.Course_Name from Registration r,Department d,course c,Degree dd where r.degree_code=dd.Degree_Code and d.Dept_Code=dd.Dept_Code and c.Course_Id=dd.Course_Id and r.batch_year in (" + batch + ") and r.degree_code in (" + branch + ") and r.current_semester in (" + semester + ") order by Batch_Year";
            //DataSet ds1 = da.select_method_wo_parameter(qry, "text");

            //DataTable dt = new DataTable();
            //DataRow dr = null;
            //dt.Columns.Add("SNo");
            //dt.Columns.Add("Batch Year");
            //dt.Columns.Add("Course");
            //dt.Columns.Add("Department");
            //dt.Columns.Add("deptcode");
            //dt.Columns.Add("Semester");
            //dt.Columns.Add("Section");
            //int sno = 0;
            //if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            //{
            //    for (int l = 0; l < ds1.Tables[0].Rows.Count; l++)
            //    {
            //        sno++;
            //        dr = dt.NewRow();
            //        dr["SNo"] = Convert.ToString(sno);
            //        dr["Batch Year"] = Convert.ToString(ds1.Tables[0].Rows[l]["Batch_Year"]);
            //        dr["Course"] = Convert.ToString(ds1.Tables[0].Rows[l]["Course_Name"]);
            //        dr["Department"] = Convert.ToString(ds1.Tables[0].Rows[l]["Dept_Name"]);
            //        dr["deptcode"] = Convert.ToString(ds1.Tables[0].Rows[l]["degree_code"]);
            //        dr["Semester"] = Convert.ToString(ds1.Tables[0].Rows[l]["Current_Semester"]);
            //        dr["Section"] = Convert.ToString(ds1.Tables[0].Rows[l]["Sections"]);
            //        dt.Rows.Add(dr);
            //    }
            //    gridview2.DataSource = dt;
            //    gridview2.DataBind();
            //    gridview2.Visible = true;
            //}
            string val = "select lock_hr,markatt_from,markatt_to from attendance_hrlock where college_code='" + ddlclg.SelectedValue.ToString() + "' and lockstatus=1 and batch_year in (" + batch + ") and degree_code in (" + branch + ") and semester in (" + semester + ") " + section1 + "";
            DataSet ds1 = da.select_method_wo_parameter(val, "text");

            DataTable dt1 = new DataTable();
            DataRow dr1 = null;

            dt1.Columns.Add("lockHour");
            dt1.Columns.Add("fromhour");
            dt1.Columns.Add("tohour");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                {
                    dr1 = dt1.NewRow();
                    dr1["lockHour"] = Convert.ToString(ds1.Tables[0].Rows[k]["lock_hr"]);
                    dr1["fromhour"] = Convert.ToString(ds1.Tables[0].Rows[k]["markatt_from"]);
                    dr1["tohour"] = Convert.ToString(ds1.Tables[0].Rows[k]["markatt_to"]);
                    dt1.Rows.Add(dr1);
                }
            }
            else
            {
                dr1 = dt1.NewRow();
                  dr1["lockHour"] ="0";
                    dr1["fromhour"] = "0";
                    dr1["tohour"] = "0";
                dt1.Rows.Add(dr1);
            }


            gridview3.DataSource = dt1;
            gridview3.DataBind();
            gridview3.Visible = true;

        }
        catch
        {
        }
    }
    protected void btnok_onClick(object sender, EventArgs e)
    {
        try
        {
            bool saveflag = false;
            string degcode = string.Empty;
            string batchyr = string.Empty;
            string sems = string.Empty;
            string sections = string.Empty;
            foreach (GridViewRow gr in gridview2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbselect");
                if (chk.Checked == true)
                {
                    Label degc = gr.FindControl("lbldegc") as Label;
                    Label batyr = gr.FindControl("lblbatchyr") as Label;
                    Label sem = gr.FindControl("lblsems") as Label;
                    Label sec = gr.FindControl("lblsect") as Label;

                    if (string.IsNullOrEmpty(degcode))
                        degcode = degc.Text;
                    else
                        degcode = degcode + "," + degc.Text;
                    if (string.IsNullOrEmpty(batchyr))
                        batchyr = batyr.Text;
                    else
                        batchyr = batchyr + "," + batyr.Text;
                    if (string.IsNullOrEmpty(sems))
                        sems = sem.Text;
                    else
                        sems = sems + "," + sem.Text;
                    if (string.IsNullOrEmpty(sections))
                        sections = sec.Text;
                    else
                        sections = sections + "," + sec.Text;

                }
            }
            lbldegcd.Text = degcode;
            lblbatchyr1.Text = batchyr;
            lblsemester1.Text = sems;
            lblsections1.Text = sections;
            divPopAlertbackvolume.Visible = false;
            divPopAlertback.Visible = false;
            div4.Visible = true;
            div5.Visible = true;
            Label7.Visible = true;
            Label7.Text = "Saved";
        }
        catch
        {
        }
    }
    protected void btnexit_onClick(object sender, EventArgs e)
    {
        divPopAlertbackvolume.Visible = false;
        divPopAlertback.Visible = false;
    }
    protected void btnsave_onClick(object sender, EventArgs e)
    {
        try
        {
            int instval = 0;
            foreach (GridViewRow gr in gridview1.Rows)
            {
                Label lblleavetype = gr.FindControl("lblleavetype") as Label;
                string leavetyp = lblleavetype.Text;
                Label lblleaveval = gr.FindControl("lblleaveval") as Label;
                string leavecode = lblleaveval.Text;
                DropDownList ddlstatus = gr.FindControl("ddlstatus") as DropDownList;
                string status = ddlstatus.Text;
                string calcflag = string.Empty;
                if (status == "Present")
                    calcflag = "0";
                else if (status == "Absent")
                    calcflag = "1";
                else if (status == "Not Consider")
                    calcflag = "2";
                TextBox txtdisplay = gr.FindControl("txtdisplay") as TextBox;
                string display = txtdisplay.Text;
                if (!string.IsNullOrEmpty(display))
                {

                    string qry = "if exists (select * from AttMasterSetting where CollegeCode='" + ddlclg.SelectedValue.ToString() + "' and LeaveCode='" + leavecode + "')update AttMasterSetting set CalcFlag='" + calcflag + "' ,DispText='" + display + "' where CollegeCode='" + ddlclg.SelectedValue.ToString() + "' and LeaveCode='" + leavecode + "'   else insert into AttMasterSetting(LeaveCode, CalcFlag,DispText,CollegeCode) values('" + leavecode + "','" + calcflag + "','" + display + "','" + ddlclg.SelectedValue.ToString() + "')";
                    instval = da.update_method_wo_parameter(qry, "text");
                }

            }

            string batch=string.Empty;
            string branch=string.Empty;
            string semester=string.Empty;
            string section=string.Empty;
           
            foreach(GridViewRow grd in gridview3.Rows)
            {
                DropDownList hr = grd.FindControl("ddllockhours") as DropDownList;
                string hour = hr.Text;
                DropDownList frhr = grd.FindControl("ddlfromhour") as DropDownList;
                string fromhour = frhr.Text;
                DropDownList tohr = grd.FindControl("ddltohour") as DropDownList;
                string tohour = tohr.Text;
               
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    if (cblBatch.Items[i].Selected == true)
                    {
                        batch = cblBatch.Items[i].Text.ToString();
                        for (int j = 0; j < cbl_branch.Items.Count; j++)
                        {
                            if (cbl_branch.Items[j].Selected == true)
                            {
                                branch = cbl_branch.Items[j].Value.ToString() ;
                                for (int jm = 0; jm < cbl_sem.Items.Count; jm++)
                                {
                                    if (cbl_sem.Items[jm].Selected == true)
                                    {
                                        semester =  cbl_sem.Items[jm].Text.ToString() ;
                                        if (txtsection.Enabled == true)
                                        {
                                            for (int j1 = 0; j1 < cblsection.Items.Count; j1++)
                                            {
                                                if (cblsection.Items[j1].Selected == true)
                                                {
                                                    if (string.IsNullOrEmpty(section))
                                                        section =  cblsection.Items[j1].Text.ToString() ;
                                                    string insqry = "if exists (select * from attendance_hrlock where college_code='" + ddlclg.SelectedValue.ToString() + "' and degree_code  ='" + branch + "' and batch_year ='" + batch + "' and semester ='" + semester + "' and section ='" + section + "' and lockstatus='1')update attendance_hrlock set lockstatus='1' ,locktype='2',lock_hr='" + hour + "',markatt_from='" + fromhour + "',markatt_to='" + tohour + "' where college_code='" + ddlclg.SelectedValue.ToString() + "' and degree_code ='" + branch + "'  and batch_year ='" + batch + "' and semester='" + semester + "' and section ='" + section + "'   else insert into attendance_hrlock(college_code, lock_hr,markatt_from,markatt_to,degree_code,batch_year,semester,section,lockstatus,locktype) values('" + ddlclg.SelectedValue.ToString() + "','" + hour + "','" + fromhour + "','" + tohour + "','" + branch + "','" + batch + "','" + semester + "','" + section + "','1','2')";
                                                    int savefg = da.update_method_wo_parameter(insqry, "text");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string insqry = "if exists (select * from attendance_hrlock where college_code='" + ddlclg.SelectedValue.ToString() + "' and degree_code  ='" + branch + "' and batch_year ='" + batch + "' and semester ='" + semester + "'  and lockstatus='1')update attendance_hrlock set lockstatus='1' ,locktype='2',lock_hr='" + hour + "',markatt_from='" + fromhour + "',markatt_to='" + tohour + "' where college_code='" + ddlclg.SelectedValue.ToString() + "' and degree_code ='" + branch + "'  and batch_year ='" + batch + "' and semester='" + semester + "'    else insert into attendance_hrlock(college_code, lock_hr,markatt_from,markatt_to,degree_code,batch_year,semester,lockstatus,locktype) values('" + ddlclg.SelectedValue.ToString() + "','" + hour + "','" + fromhour + "','" + tohour + "','" + branch + "','" + batch + "','" + semester + "','1','2')";
                                            int savefg = da.update_method_wo_parameter(insqry, "text");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (instval > 0)
            {
                div4.Visible = true;
                div5.Visible = true;
                Label7.Visible = true;
                Label7.Text = "Saved Successfully";
            }
            else
            {
                div4.Visible = true;
                div5.Visible = true;
                Label7.Visible = true;
                Label7.Text = "Not Saved";
            }
        }
        catch
        {
        }
    }
    protected void btnaltok_Click(object sender, EventArgs e)
    {
        div4.Visible = false;
        div5.Visible = false;
    }

    protected void btnaddnewrow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt_newrow = new DataTable();
            DataRow dr = null;
      
            dt_newrow.Columns.Add("Lockhour");
            dt_newrow.Columns.Add("fromhour");
            dt_newrow.Columns.Add("tohour");
            for (int i = 0; i < gridview3.Rows.Count; i++)
            {
                dr = dt_newrow.NewRow();

                DropDownList top = (DropDownList)gridview3.Rows[i].Cells[0].FindControl("ddllockhours");
                string topic = top.Text;
                dr["lockHour"] = topic;

                DropDownList labtop = (DropDownList)gridview3.Rows[i].Cells[1].FindControl("ddlfromhour");
                string laptopi = labtop.Text;
                dr["fromhour"] = laptopi;

                DropDownList descp = (DropDownList)gridview3.Rows[i].Cells[2].FindControl("ddltohour");
                string desc = descp.Text;
                dr["tohour"] = desc;

                dt_newrow.Rows.Add(dr);
            }
            ViewState["CurrentTable"] = dt_newrow;


            dr = dt_newrow.NewRow();

            dr["lockHour"] = "";
            dr["fromhour"] = "";
            dr["tohour"] = "";
           
            dt_newrow.Rows.Add(dr);

            gridview3.DataSource = dt_newrow;
            gridview3.DataBind();
        }
        catch
        {
        }
    }

}