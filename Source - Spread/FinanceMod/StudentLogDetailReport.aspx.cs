using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Net;
using System.IO;
using System.Configuration;

public partial class StudentLogDetailReport : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    ArrayList colord = new ArrayList();
    bool cellClick = false;
    bool Cellclick = false;
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
                    collegecode = ddlcollege.SelectedItem.Value.ToString();
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                BindSem();
                // bindsem();
                bindsec();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
            }
            if (ddlcollege.Items.Count > 0)
                collegecode = ddlcollege.SelectedItem.Value.ToString();
            imgdiv2.Attributes.Add("style", "display: none;");
            divtempsecond.Attributes.Add("style", "display: none;");
        }
        catch (Exception ex) { }
    }
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ddlcollege.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = da.select_method_wo_parameter(Query, "Text");
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
    }
    #region stream

    public void loadstrm()
    {
        try
        {
            ddltype.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode + "' and type<>''";
            ds.Clear();
            ds = da.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataValueField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
            }
            else
            {
                ddltype.Enabled = false;
            }
            binddeg();
        }
        catch
        { }
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddeg();
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
            ds = da.BindBatch();
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
        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
        binddeg();
        binddept();
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
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
            string stream = "";
            if (ddltype.Items.Count > 0)
                stream = ddltype.SelectedItem.Text.ToString();
            cbl_degree.Items.Clear();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode + "'";
            if (stream != "")
                selqry = selqry + " and type  in('" + stream + "')";
            ds = da.select_method_wo_parameter(selqry, "Text");
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
            txt_dept.Text = "---Select---";
            string batch2 = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch2 == "")
                        batch2 = Convert.ToString(cbl_batch.Items[i].Text);
                    else
                        batch2 += "','" + Convert.ToString(cbl_batch.Items[i].Text);
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
            if (batch2 != "" && degree != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
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
        // bindsem();       
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        //  bindsem();      
    }
    #endregion
    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lbldept.Text, "--Select--");
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
    }
    private void BindSem()
    {
        try
        {
            string strbatchyear = "0";
            string mystrbatchyear = "0";
            if (cbl_batch.Items.Count > 0)
                strbatchyear = getCblSelectedValue(cbl_batch);
            string strbranch = "0";
            string mystrbranch = "0";
            if (cbl_dept.Items.Count > 0)
                strbranch = getCblSelectedValue(cbl_dept);
            if (strbatchyear.Trim() == "" || strbatchyear.Trim() == "0")
                mystrbatchyear = "0";
            else
                mystrbatchyear = "'" + strbatchyear + "'";
            if (strbranch.Trim() == "" || strbranch.Trim() == "0")
                mystrbranch = "0";
            else
                mystrbranch = "'" + strbranch + "'";
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = da.BindSem(mystrbranch, mystrbatchyear, collegecode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                        cbl_sem.Items.Add(i.ToString());
                    else if (first_year == true && i != 2)
                        cbl_sem.Items.Add(i.ToString());
                    cbl_sem.Items[i - 1].Selected = true;
                }
                if (cbl_sem.Items.Count > 0)
                {
                    cb_sem.Checked = true;
                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
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
            ds = da.loadFeecategory(collegecode, usercode, ref linkName);
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
    #endregion
    #region sec
    public void bindsec()
    {
        try
        {
            cbl_sect.Items.Clear();
            txt_sect.Text = "---Select---";
            cb_sect.Checked = false;
            string build = "";
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_sem.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_sem.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "")
            {
                ds = da.BindSectionDetailmult(collegecode);
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

    #region button go
    protected DataSet getDatasetValues()
    {
        DataSet dsval = new DataSet();
        try
        {
            string batch = string.Empty;
            string degcode = string.Empty;
            string sem = string.Empty;
            string sec = string.Empty;
            collegecode = Convert.ToString(ddlcollege.SelectedValue);
            batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            degcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sect));

            string tempFrdt = string.Empty;
            string temptodt = string.Empty;
            string fromdate = Convert.ToString(txt_fromdate.Text);
            string todate = Convert.ToString(txt_todate.Text);
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                tempFrdt = frdate[0].ToString() + "/" + frdate[1].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                temptodt = tdate[0].ToString() + "/" + tdate[1].ToString() + "/" + tdate[2].ToString();
            }
            string selQ = string.Empty;
            // selQ = " select r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,r.batch_year,r.current_semester,(c.course_name+'-'+dt.dept_name) as deptname,(c.course_name+'-'+dt.dept_acronym) as deptacr from registration r,degree d,course c,department dt  where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and r.college_code=d.college_code and  r.degree_code in('') and r.current_semester in('') and r.batch_year in('') and d.college_code=''";
            selQ = " select count(r.roll_no) as roll_no,count(r.reg_no) as reg_no,count(r.roll_admit) as roll_admit,r.degree_code,r.batch_year,(c.course_name+'-'+dt.dept_name) as deptname,(c.course_name+'-'+dt.dept_acronym) as deptacr from registration r,degree d,course c,department dt  where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and r.college_code=d.college_code and r.batch_year in('" + batch + "') and  r.degree_code in('" + degcode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  and d.college_code='" + collegecode + "' group by r.degree_code,r.batch_year,c.course_name,dt.dept_name,dt.dept_acronym order by r.batch_year,r.degree_code asc";
            //log in count
            selQ += " select  count(distinct r.roll_no) as roll_no,count( distinct r.reg_no) as reg_no,count(distinct r.roll_admit) as roll_admit,r.degree_code,r.batch_year from registration r,logindetails ld  where  staff_code=r.roll_no and ld.flag='2' and r.batch_year in('" + batch + "') and  r.degree_code in('" + degcode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  and r.college_code='" + collegecode + "' and convert(varchar(10),ld.dateandtime,103) between '" + tempFrdt + "' and '" + temptodt + "' group by r.degree_code,r.batch_year";
            //online payment try
            selQ += " select  count(distinct r.roll_no) as roll_no,count( distinct r.reg_no) as reg_no,count(distinct r.roll_admit) as roll_admit,r.degree_code,r.batch_year from registration r,OnlineFeeTransactionMaster ot,OnlineFeeTransaction ots  where  r.app_no=ot.appno and ot.transpk=ots.transfk and r.college_code=ot.collegecode and r.batch_year in('" + batch + "') and  r.degree_code in('" + degcode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  and r.college_code='" + collegecode + "' and ot.Transdate between '" + fromdate + "' and '" + todate + "' and isnull(ots.paidstatus,'0')='0' group by r.degree_code,r.batch_year";
            //online payment paid
            selQ += " select  count(distinct r.roll_no) as roll_no,count( distinct r.reg_no) as reg_no,count(distinct r.roll_admit) as roll_admit,r.degree_code,r.batch_year from registration r,ft_findailytransaction f  where  r.app_no=f.app_no and r.batch_year in('" + batch + "') and  r.degree_code in('" + degcode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  and r.college_code='" + collegecode + "' and f.paymode='5' and f.Transdate between '" + fromdate + "' and '" + todate + "' group by r.degree_code,r.batch_year";
            //challan print
            selQ += " select  count(distinct r.roll_no) as roll_no,count( distinct r.reg_no) as reg_no,count(distinct r.roll_admit) as roll_admit,r.degree_code,r.batch_year from registration r,ft_challandet c  where  r.app_no=c.app_no and isnull(isconfirmed,'0')='0' and r.batch_year in('" + batch + "') and  r.degree_code in('" + degcode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  and r.college_code='" + collegecode + "' and c.challandate between '" + fromdate + "' and '" + todate + "' group by r.degree_code,r.batch_year";
            //challan confirmed
            selQ += " select  count(distinct r.roll_no) as roll_no,count( distinct r.reg_no) as reg_no,count(distinct r.roll_admit) as roll_admit,r.degree_code,r.batch_year from registration r,ft_challandet c  where  r.app_no=c.app_no and isnull(isconfirmed,'0')='1' and r.batch_year in('" + batch + "') and  r.degree_code in('" + degcode + "') and r.current_semester in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','')  and r.college_code='" + collegecode + "' and c.challandate between '" + fromdate + "' and '" + todate + "' group by r.degree_code,r.batch_year";
            dsval.Clear();
            dsval = da.select_method_wo_parameter(selQ, "Text");
        }
        catch { }
        return dsval;
    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        bool boolcheck = false;
        ds.Clear();
        ds = getDatasetValues();
        if (ds.Tables.Count > 0)
        {
            DataTable dtstud = dtload(ds);
            if (dtstud.Rows.Count > 0)
                loadSpreadDetails(dtstud);
            else
                boolcheck = true;
        }
        else
            boolcheck = true;
        if (boolcheck)
        {
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            print.Visible = false;
            lbl_alert.Text = "No Record Found";
            imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
        }
    }

    protected DataTable dtload(DataSet ds)
    {
        DataTable dtstud = new DataTable();
        try
        {
            dtstud.Columns.Add("SNo");
            dtstud.Columns.Add("Batch");
            dtstud.Columns.Add("Dept");
            dtstud.Columns.Add("Deptcode");
            dtstud.Columns.Add("Total_Stud");
            dtstud.Columns.Add("Total_Log");
            dtstud.Columns.Add("Total_On_Try");
            dtstud.Columns.Add("Total_On_Paid");
            dtstud.Columns.Add("Total_Chl_Print");
            dtstud.Columns.Add("Total_Chl_Confirm");
            DataRow drstud;
            if (dtstud.Columns.Count > 0)
            {
                #region
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    int studCnt = 0;
                    drstud = dtstud.NewRow();
                    string degcode = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                    string batch = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
                    drstud["SNo"] = Convert.ToString(row + 1);
                    drstud["Batch"] = batch;
                    drstud["Dept"] = Convert.ToString(ds.Tables[0].Rows[row]["deptacr"]);
                    drstud["Deptcode"] = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                    drstud["Total_Stud"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "degree_code='" + degcode + "' and batch_year='" + batch + "'";
                        DataView dvCnt = ds.Tables[1].DefaultView;
                        if (dvCnt.Count > 0)
                            int.TryParse(Convert.ToString(dvCnt[0]["roll_no"]), out studCnt);
                    }
                    drstud["Total_Log"] = Convert.ToString(studCnt); studCnt = 0;

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ds.Tables[2].DefaultView.RowFilter = "degree_code='" + degcode + "' and batch_year='" + batch + "'";
                        DataView dvCnt = ds.Tables[2].DefaultView;
                        if (dvCnt.Count > 0)
                            int.TryParse(Convert.ToString(dvCnt[0]["roll_no"]), out studCnt);
                    }
                    drstud["Total_On_Try"] = Convert.ToString(studCnt); studCnt = 0;

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        ds.Tables[3].DefaultView.RowFilter = "degree_code='" + degcode + "' and batch_year='" + batch + "'";
                        DataView dvCnt = ds.Tables[3].DefaultView;
                        if (dvCnt.Count > 0)
                            int.TryParse(Convert.ToString(dvCnt[0]["roll_no"]), out studCnt);
                    }
                    drstud["Total_On_Paid"] = Convert.ToString(studCnt); studCnt = 0;

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        ds.Tables[4].DefaultView.RowFilter = "degree_code='" + degcode + "' and batch_year='" + batch + "'";
                        DataView dvCnt = ds.Tables[4].DefaultView;
                        if (dvCnt.Count > 0)
                            int.TryParse(Convert.ToString(dvCnt[0]["roll_no"]), out studCnt);
                    }
                    drstud["Total_Chl_Print"] = Convert.ToString(studCnt); studCnt = 0;

                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        ds.Tables[5].DefaultView.RowFilter = "degree_code='" + degcode + "' and batch_year='" + batch + "'";
                        DataView dvCnt = ds.Tables[5].DefaultView;
                        if (dvCnt.Count > 0)
                            int.TryParse(Convert.ToString(dvCnt[0]["roll_no"]), out studCnt);
                    }
                    drstud["Total_Chl_Confirm"] = Convert.ToString(studCnt); studCnt = 0;
                    dtstud.Rows.Add(drstud);
                }
                #endregion
            }
        }
        catch { }
        return dtstud;
    }

    protected void loadSpreadDetails(DataTable dtstud)
    {
        try
        {
            #region design
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 9;
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

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblbatch.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbldept.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total No Of Students";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total No Of Students(loged)";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total No Of Students(Online Payment Not Paid)";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total No Of Students(Online Payment Paid)";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total No Of Students(Challan Not Paid)";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total No Of Students(Challan Paid)";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            #endregion
            #region value
            int height = 0;
            Hashtable grandtotal = new Hashtable();
            ArrayList arbatch = new ArrayList();
            string degreeCode = string.Empty;
            string batchYear = string.Empty;
            for (int row = 0; row < dtstud.Rows.Count; row++)
            {
                double totStudCnt = 0;
                double totLogCnt = 0;
                double totOnTryCnt = 0;
                double totOnPaidCnt = 0;
                double totChlCnt = 0;
                double totChlConfCnt = 0;
                string batch = string.Empty;
                height += 10;

                batch = Convert.ToString(dtstud.Rows[row]["Batch"]);
                batchYear = Convert.ToString(dtstud.Rows[row]["Batch"]);
                #region every batch year total
                if (!arbatch.Contains(batch))
                {
                    if (grandtotal.Count > 0)
                    {
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Tag = degreeCode;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Note = batchYear;
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.White;
                        double grandvalues = 0;
                        for (int j = 3; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(grandtotal[j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        }
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].Tag = "-3";
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Tag = "-4";
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 5].Tag = "-5";
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 6].Tag = "-6";
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 7].Tag = "-7";
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 8].Tag = "-8";
                        grandtotal.Clear();
                        degreeCode = string.Empty;
                    }
                    arbatch.Add(batch);
                }
                #endregion
                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = batch;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtstud.Rows[row]["Dept"]);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dtstud.Rows[row]["Deptcode"]);
                if (degreeCode == string.Empty)
                    degreeCode = Convert.ToString(dtstud.Rows[row]["Deptcode"]);
                else
                    degreeCode += "','" + Convert.ToString(dtstud.Rows[row]["Deptcode"]);
                double.TryParse(Convert.ToString(dtstud.Rows[row]["Total_Stud"]), out totStudCnt);
                double.TryParse(Convert.ToString(dtstud.Rows[row]["Total_Log"]), out totLogCnt);
                double.TryParse(Convert.ToString(dtstud.Rows[row]["Total_On_Try"]), out totOnTryCnt);
                double.TryParse(Convert.ToString(dtstud.Rows[row]["Total_On_Paid"]), out totOnPaidCnt);
                double.TryParse(Convert.ToString(dtstud.Rows[row]["Total_Chl_Print"]), out totChlCnt);
                double.TryParse(Convert.ToString(dtstud.Rows[row]["Total_Chl_Confirm"]), out totChlConfCnt);

                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(totStudCnt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Tag = "-3";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(totLogCnt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Tag = "-4";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totOnTryCnt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Tag = "-5";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(totOnPaidCnt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Tag = "-6";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(totChlCnt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Tag = "-7";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(totChlConfCnt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Tag = "-8";

                #region total
                if (!grandtotal.ContainsKey(3))
                    grandtotal.Add(3, Convert.ToString(totStudCnt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[3]), out amount);
                    amount += totStudCnt;
                    grandtotal.Remove(3);
                    grandtotal.Add(3, Convert.ToString(amount));
                }
                if (!grandtotal.ContainsKey(4))
                    grandtotal.Add(4, Convert.ToString(totLogCnt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[4]), out amount);
                    amount += totLogCnt;
                    grandtotal.Remove(4);
                    grandtotal.Add(4, Convert.ToString(amount));
                }
                if (!grandtotal.ContainsKey(5))
                    grandtotal.Add(5, Convert.ToString(totOnTryCnt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[5]), out amount);
                    amount += totOnTryCnt;
                    grandtotal.Remove(5);
                    grandtotal.Add(5, Convert.ToString(amount));
                }
                if (!grandtotal.ContainsKey(6))
                    grandtotal.Add(6, Convert.ToString(totOnPaidCnt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[6]), out amount);
                    amount += totOnPaidCnt;
                    grandtotal.Remove(6);
                    grandtotal.Add(6, Convert.ToString(amount));
                }
                if (!grandtotal.ContainsKey(7))
                    grandtotal.Add(7, Convert.ToString(totChlCnt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[7]), out amount);
                    amount += totChlCnt;
                    grandtotal.Remove(7);
                    grandtotal.Add(7, Convert.ToString(amount));
                }
                if (!grandtotal.ContainsKey(8))
                    grandtotal.Add(8, Convert.ToString(totChlConfCnt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[8]), out amount);
                    amount += totChlConfCnt;
                    grandtotal.Remove(8);
                    grandtotal.Add(8, Convert.ToString(amount));
                }
                #endregion
            }
            spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadDet.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            #endregion
            #region Total
            // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Tag = degreeCode;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Note = batchYear;
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            double grandvalue = 0;
            for (int j = 3; j < spreadDet.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
            }
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].Tag = "-3";
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Tag = "-4";
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 5].Tag = "-5";
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 6].Tag = "-6";
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 7].Tag = "-7";
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 8].Tag = "-8";
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            spreadDet.Height = height;
            spreadDet.SaveChanges();
        }
        catch { }
    }

    #endregion

    #region cell click
    protected void spreadDet_Click(object sender, EventArgs e)
    {
        cellClick = true;
    }
    protected void spreadDet_Render(object sender, EventArgs e)
    {
        try
        {
            if (cellClick)
            {
                string activerow = spreadDet.ActiveSheetView.ActiveRow.ToString();
                string activecol = spreadDet.ActiveSheetView.ActiveColumn.ToString();
                int rowCnt = 0;
                int colCnt = 0;
                int.TryParse(activerow, out rowCnt);
                int.TryParse(activecol, out colCnt);
                if (rowCnt != -1 && colCnt > 2)
                {
                    string textStr = Convert.ToString(spreadDet.Sheets[0].Cells[rowCnt, 0].Text);
                    if (textStr.Trim() != "Total")
                    {
                        string batch = Convert.ToString(spreadDet.Sheets[0].Cells[rowCnt, 1].Text);
                        string degree = Convert.ToString(spreadDet.Sheets[0].Cells[rowCnt, 2].Tag);
                        int identityCol = 0;
                        int.TryParse(Convert.ToString(spreadDet.Sheets[0].Cells[rowCnt, colCnt].Tag), out identityCol);
                        DataSet dsDet = loadClickValues(batch, degree, identityCol);
                        if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                        {
                            bindLabelString(identityCol);
                            bool boolCheck = loadBasicStudDetils(dsDet);
                            if (boolCheck)
                            {
                                divpur.Visible = true;
                                // spreadSms.Visible = true;
                                divbtn.Visible = true;
                                txtmessage.Visible = true;
                                btnsendsms.Visible = true;
                                txtmessage.Text = string.Empty;
                                loadPurpose();
                                ddlpurpose_SelectedIndexChanged(sender, e);
                            }
                        }
                        else
                        {
                            imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                            lbl_alert.Text = "No Record Found!";
                        }
                        #region old
                        //if (identityCol == -3)
                        //{
                        //    //total student count
                        //    DataSet dsDet = loadClickValues(batch, degree, identityCol);
                        //    if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                        //    {
                        //        loadBasicStudDetils(dsDet);
                        //    }
                        //}
                        //if (identityCol == -4)
                        //{
                        //    //total log student count
                        //    DataSet dsDet = loadClickValues(batch, degree, identityCol);
                        //    if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                        //    {
                        //        loadBasicStudDetils(dsDet);
                        //    }
                        //}
                        //if (identityCol == -5)
                        //{
                        //    //total online payment try student count
                        //}
                        //if (identityCol == -6)
                        //{
                        //    //total online payment paid student count
                        //}
                        //if (identityCol == -7)
                        //{
                        //    //total challan print student count
                        //}
                        //if (identityCol == -8)
                        //{
                        //    //total challan confirm student count
                        //}
                        #endregion
                    }
                    else if (textStr.Trim() == "Total")
                    {
                        string batch = Convert.ToString(spreadDet.Sheets[0].Cells[rowCnt, 0].Note);
                        string degree = Convert.ToString(spreadDet.Sheets[0].Cells[rowCnt, 0].Tag);
                        int identityCol = 0;
                        int.TryParse(Convert.ToString(spreadDet.Sheets[0].Cells[rowCnt, colCnt].Tag), out identityCol);
                        DataSet dsDet = loadClickValues(batch, degree, identityCol);
                        if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                        {
                            bindLabelString(identityCol);
                            bool boolCheck = loadBasicStudDetils(dsDet);
                            if (boolCheck)
                            {
                                divpur.Visible = true;
                                //  spreadSms.Visible = true;
                                divbtn.Visible = true;
                                txtmessage.Visible = true;
                                btnsendsms.Visible = true;
                                txtmessage.Text = string.Empty;
                                loadPurpose();
                                ddlpurpose_SelectedIndexChanged(sender, e);
                            }
                        }
                    }

                    else
                    {
                        imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                        lbl_alert.Text = "Please Click Valid Columns";
                    }
                }
                else
                {
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Please Click Valid Columns";
                }
            }
        }
        catch { }
    }

    protected DataSet loadClickValues(string batch, string degree, int funType)
    {
        DataSet dsval = new DataSet();
        try
        {
            string tempFrdt = string.Empty;
            string temptodt = string.Empty;
            string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            collegecode = Convert.ToString(ddlcollege.SelectedValue);
            string fromdate = Convert.ToString(txt_fromdate.Text);
            string todate = Convert.ToString(txt_todate.Text);
            string[] frdate = fromdate.Split('/');

            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                tempFrdt = frdate[0].ToString() + "/" + frdate[1].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                temptodt = tdate[0].ToString() + "/" + tdate[1].ToString() + "/" + tdate[2].ToString();
            }
            string selQ = string.Empty;
            if (funType == -3)
            {
                selQ = " select r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,r.batch_year,r.current_semester,(c.course_name+'-'+dt.dept_name) as deptname,(c.course_name+'-'+dt.dept_acronym) as deptacr,r.app_no from registration r,degree d,course c,department dt  where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and r.college_code=d.college_code and  r.degree_code in('" + degree + "') and r.current_semester in('" + sem + "') and r.batch_year in('" + batch + "') and d.college_code='" + collegecode + "'";
            }
            else if (funType == -4)
            {
                selQ = " select distinct   r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,r.batch_year ,r.current_semester,r.app_no from registration r,logindetails ld  where  staff_code=r.roll_no and ld.flag='2' and r.batch_year in('" + batch + "') and  r.degree_code in('" + degree + "') and r.current_semester in('" + sem + "')   and r.college_code='" + collegecode + "' and convert(varchar(10),ld.dateandtime,103) between '" + tempFrdt + "' and '" + temptodt + "'";
            }
            else if (funType == -5)
            {
                selQ += " select  distinct   r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,r.batch_year ,r.current_semester,r.app_no from registration r,OnlineFeeTransactionMaster ot  where  r.app_no=ot.appno and r.college_code=ot.collegecode and r.batch_year in('" + batch + "') and  r.degree_code in('" + degree + "') and r.current_semester in('" + sem + "')  and r.college_code='" + collegecode + "' and ot.Transdate between '" + fromdate + "' and '" + todate + "' ";
            }
            else if (funType == -6)
            {
                selQ += " select  distinct   r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,r.batch_year ,r.current_semester,r.app_no from registration r,ft_findailytransaction f  where  r.app_no=f.app_no and r.batch_year in('" + batch + "') and  r.degree_code in('" + degree + "') and r.current_semester in('" + sem + "')  and r.college_code='" + collegecode + "' and f.paymode='5' and f.Transdate between '" + fromdate + "' and '" + todate + "' ";
            }
            else if (funType == -7)
            {
                selQ += " select  distinct   r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,r.batch_year ,r.current_semester,r.app_no from registration r,ft_challandet c  where  r.app_no=c.app_no and isnull(isconfirmed,'0')='0' and r.batch_year in('" + batch + "') and  r.degree_code in('" + degree + "') and r.current_semester in('" + sem + "')  and r.college_code='" + collegecode + "' and c.challandate between '" + fromdate + "' and '" + todate + "' ";
            }
            else if (funType == -8)
            {
                selQ += " select   distinct   r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,r.batch_year ,r.current_semester,r.app_no from registration r,ft_challandet c  where  r.app_no=c.app_no and isnull(isconfirmed,'0')='1' and r.batch_year in('" + batch + "') and  r.degree_code in('" + degree + "') and r.current_semester in('" + sem + "')   and r.college_code='" + collegecode + "' and c.challandate between '" + fromdate + "' and '" + todate + "' ";
            }
            selQ += " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
            dsval.Clear();
            dsval = da.select_method_wo_parameter(selQ, "Text");
        }
        catch { }
        return dsval;
    }

    protected bool loadBasicStudDetils(DataSet ds)
    {
        bool boolCheck = false;
        try
        {
            #region
            RollAndRegSettings();
            spreadStud.Sheets[0].RowCount = 0;
            spreadStud.Sheets[0].ColumnCount = 0;
            spreadStud.CommandBar.Visible = false;
            spreadStud.Sheets[0].AutoPostBack = false;
            spreadStud.Sheets[0].ColumnHeader.RowCount = 1;
            spreadStud.Sheets[0].RowHeader.Visible = false;
            spreadStud.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadStud.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtadmit = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
            cb.AutoPostBack = false;
            cball.AutoPostBack = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[0].Width = 40;

            spreadStud.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[1].Width = 60;

            spreadStud.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            spreadStud.Sheets[0].Columns[2].Width = 280;

            spreadStud.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            spreadStud.Sheets[0].Columns[3].Width = 100;

            spreadStud.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[4].Width = 100;

            spreadStud.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Admission No";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[5].Width = 100;

            spreadStud.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Batch";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;

            spreadStud.Sheets[0].ColumnHeader.Cells[0, 7].Text = lbldept.Text;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            spreadStud.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadStud.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            spreadColumnVisible();
            #endregion
            #region value
            bool boolrow = false;
            int height = 0;
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                if (!boolrow)
                {
                    spreadStud.Sheets[0].RowCount++;
                    spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 1].CellType = cball;
                    boolrow = true;
                }
                height += 10;
                spreadStud.Sheets[0].RowCount++;
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 1].CellType = cb;
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]);
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]);
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
                string deptName = string.Empty;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(ds.Tables[0].Rows[row]["Degree_code"]) + "' ";
                    DataView dnew = ds.Tables[1].DefaultView;
                    if (dnew.Count > 0)
                        deptName = Convert.ToString(dnew[0]["dept_acronym"]);
                }
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 7].Text = deptName;
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_code"]);
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 4].CellType = txtreg;
                spreadStud.Sheets[0].Cells[spreadStud.Sheets[0].RowCount - 1, 5].CellType = txtadmit;
                boolCheck = true;
            }
            spreadStud.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadStud.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadStud.Sheets[0].PageSize = spreadStud.Sheets[0].RowCount;
            spreadStud.Visible = true;
            // spreadStud.Height = height;
            spreadStud.ShowHeaderSelection = false;
            spreadStud.SaveChanges();
            divStud.Visible = true;
            #endregion
        }
        catch { }
        return boolCheck;
    }
    protected void spreadStud_Command(object sender, EventArgs e)
    {
        try
        {
            spreadStud.SaveChanges();
            string actrow = spreadStud.Sheets[0].ActiveRow.ToString();
            if (actrow.Trim() == "0")
            {
                if (spreadStud.Sheets[0].RowCount > 0)
                {
                    int Val = 0;
                    int.TryParse(Convert.ToString(spreadStud.Sheets[0].Cells[0, 1].Value), out Val);
                    if (Val == 1)
                    {
                        for (int i = 1; i < spreadStud.Sheets[0].RowCount; i++)
                        {
                            spreadStud.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    else
                    {
                        for (int i = 1; i < spreadStud.Sheets[0].RowCount; i++)
                        {
                            spreadStud.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }

                }
            }
        }
        catch { }
    }
    protected void bindLabelString(int identityCol)
    {
        if (identityCol == -3)
            spname.InnerHtml = "Total Student Details";
        else if (identityCol == -4)
            spname.InnerHtml = "Total Student Details(loged)";
        else if (identityCol == -5)
            spname.InnerHtml = "Total Student Details(Online Payment Not Paid)";
        else if (identityCol == -6)
            spname.InnerHtml = "Total Student Details(Online Payment Paid)";
        else if (identityCol == -7)
            spname.InnerHtml = "Total Student Details(Challan Not Paid)";
        else if (identityCol == -8)
            spname.InnerHtml = "Total Student Details(Challan Paid)";
    }
    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void imagebtnpopsscode_Click(object sender, EventArgs e)
    {
        divStud.Visible = false;
    }

    #region Button sms Send
    protected void btnsendsms_Click(object sender, EventArgs e)
    {
        try
        {
            bool check = false;
            string appno = "";
            string degcode = "";
            string messag = Convert.ToString(txtmessage.Text);
            spreadStud.SaveChanges();
            if (!string.IsNullOrEmpty(messag) && spreadStud.Rows.Count > 1)
            {
                string collName = da.GetFunction("Select collname from collinfo where college_code='" + collegecode + "'");
                string RightsCode = da.GetFunction("select value from master_settings where settings='Send Sms Right' and usercode='" + usercode + "'");
                string smsUserId = da.GetFunction("select SMS_User_ID from Track_Value where college_code='" + collegecode + "'");
                if (!string.IsNullOrEmpty(RightsCode) && RightsCode != "0" && !string.IsNullOrEmpty(smsUserId) && smsUserId != "0")
                {
                    for (int sel = 0; sel < spreadStud.Sheets[0].Rows.Count; sel++)
                    {
                        if (sel == 0)
                            continue;
                        string value = Convert.ToString(spreadStud.Sheets[0].Cells[sel, 1].Value);
                        if (value == "1")
                        {
                            appno = Convert.ToString(spreadStud.Sheets[0].Cells[sel, 1].Tag);
                            degcode = Convert.ToString(spreadStud.Sheets[0].Cells[sel, 7].Tag);
                            if (appno != "" && appno != "0" && degcode != "" && degcode != "0" && messag != "")
                            {
                                SmsRights(appno, collegecode, degcode, messag, collName, RightsCode, smsUserId);
                                check = true;
                            }
                        }
                    }
                    if (check == false)
                    {
                        imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                        lbl_alert.Text = "Please Select Any One Student";
                    }
                }
                else
                {
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Please Set the Rights To Send Sms";
                    return;
                }
            }
            else
            {
                imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                lbl_alert.Text = "Please Select Any One Template";
            }
        }
        catch { }
    }
    protected void SmsRights(string appno, string collegecode, string Degcode, string MSGs, string collName, string RightsCode, string smsUserId)
    {
        try
        {
            bool check = false;
            string selMblno = "";
            if (RightsCode == "1")
            {
                string[] splival = RightsCode.Split(',');
                for (int sel = 0; sel < splival.Length; sel++)
                {
                    if (splival[sel] == "1")
                    {
                        //mblno =da.GetFunction("select Student_Mobile  from applyn where app_no='" + appno + "'"); 
                        selMblno = da.GetFunction("select Student_Mobile  from applyn where app_no='" + appno + "'");
                        //  selMblno = "8608759542";
                        if (selMblno != "0")
                        {
                            sendsms(appno, selMblno, MSGs, smsUserId);
                            check = true;
                        }
                    }
                    if (splival[sel] == "2")
                    {
                        // mblno =da.GetFunction("select parentF_Mobile  from applyn where app_no='" + appno + "'");
                        selMblno = da.GetFunction("select parentF_Mobile  from applyn where app_no='" + appno + "'");
                        // selMblno = "8608759542";
                        if (selMblno != "0")
                        {
                            sendsms(appno, selMblno, MSGs, smsUserId);
                            check = true;
                        }
                    }
                    if (splival[sel] == "3")
                    {
                        // mblno =da.GetFunction("select parentM_Mobile  from applyn where app_no='" + appno + "'");
                        selMblno = da.GetFunction("select parentM_Mobile  from applyn where app_no='" + appno + "'");
                        // selMblno = "8608759542";
                        if (selMblno != "0")
                        {
                            sendsms(appno, selMblno, MSGs, smsUserId);
                            check = true;
                        }
                    }
                }
                if (check == true)
                {
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "SMS Send Successfully";
                }
            }
            else
            {
                imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                lbl_alert.Text = "Please Set the Rights To Send Sms";
                return;
            }
        }
        catch { }
    }

    public void sendsms(string app, string mblno, string Msg, string smsUserId)
    {
        try
        {
            string SenderID = "";
            string Password = "";
            string todaydate = System.DateTime.Now.ToString("dd/MM/yyyy");
            string[] splitdate = todaydate.Split('/');
            DateTime dt1 = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            if (!string.IsNullOrEmpty(smsUserId))
            {
                string getval = da.GetUserapi(smsUserId);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {
                    SenderID = Convert.ToString(spret[0]);
                    Password = Convert.ToString(spret[0]);
                }
                string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mblno + "&text=" + Msg + "&priority=ndnd&stype=normal";
                string isst = "0";
                smsreport(strpath, isst, dt1, mblno, Msg);
            }
        }
        catch
        {

        }
    }

    public void smsreport(string uril, string isstaff, DateTime dt1, string phone, string msg)
    {
        try
        {
            string phoneno = phone;
            string message = msg;
            string date = dt1.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = "";
            groupmsgid = strvel;
            int sms = 0;
            string smsreportinsert = "";

            smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date)values( '" + phoneno + "','" + groupmsgid + "','" + message + "','" + collegecode + "','" + isstaff + "','" + date + "')";
            sms = da.update_method_wo_parameter(smsreportinsert, "Text");

        }
        catch (Exception ex)
        { }
    }
    #endregion

    //sms template adding and deleting
    #region Sms template add and delete
    //purpose dropdown load
    public void loadPurpose()
    {
        try
        {
            ddlpurpose.Items.Clear();
            ddlpurposemsg.Items.Clear();
            string strpurposename = "select PurposePK,purpose,temp_code from FT_sms_purpose where college_code = '" + collegecode + "'";
            DataSet dsPur = da.select_method_wo_parameter(strpurposename, "Text");
            if (dsPur.Tables.Count > 0 && dsPur.Tables[0].Rows.Count > 0)
            {
                ddlpurpose.DataSource = dsPur;
                ddlpurpose.DataTextField = "Purpose";
                ddlpurpose.DataValueField = "PurposePK";
                ddlpurpose.DataBind();
                // ddlpurpose.Items.Insert(0, "Select");

                ddlpurposemsg.DataSource = dsPur;
                ddlpurposemsg.DataTextField = "Purpose";
                ddlpurposemsg.DataValueField = "PurposePK";
                ddlpurposemsg.DataBind();
                //ddlpurposemsg.Items.Insert(0, "Select");
            }
        }
        catch
        { }
    }

    //selected index changed
    protected void ddlpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadPurposeDetails();
    }
    protected void loadPurposeDetails()
    {
        try
        {
            spreadSms.Visible = false;
            spreadSms.Sheets[0].RowCount = 1;
            spreadSms.Sheets[0].ColumnCount = 2;
            spreadSms.Columns[1].Width = 900;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadSms.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            spreadSms.Sheets[0].ColumnHeaderVisible = false;
            spreadSms.Sheets[0].SheetCorner.Columns[0].Visible = false;
            spreadSms.Sheets[0].AutoPostBack = true;

            //lblpurpose1.Visible = true;
            ddlpurpose.Visible = true;
            spreadSms.Sheets[0].RowCount = 1;
            spreadSms.Sheets[0].ColumnCount = 2;
            spreadSms.Columns[1].Width = 900;

            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].Text = "S.No";
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].Locked = true;
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#000000");

            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].Text = "Template";
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].Locked = true;
            spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            string selQ = string.Empty;
            string getPurpose = string.Empty;
            if (ddlpurpose.Items.Count > 0)
                getPurpose = Convert.ToString(ddlpurpose.SelectedValue);
            if (!string.IsNullOrEmpty(getPurpose))
            {
                selQ = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from FT_sms_purpose sp,FT_sms_template st where sp.PurposePK=st.temp_code and sp.college_code='" + collegecode + "' and st.temp_code='" + getPurpose + "'";
                ds = da.select_method_wo_parameter(selQ, "Text");
            }
            //else
            //{
            //    selQ = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from FT_sms_template where temp_code = " + getPurpose + "";
            //    ds = da.select_method_wo_parameter(selQ, "Text");
            //}
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    spreadSms.Sheets[0].RowCount++;
                    spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);

                    spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    spreadSms.Sheets[0].Cells[spreadSms.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                }
                spreadSms.Sheets[0].PageSize = spreadSms.Sheets[0].RowCount;
                spreadSms.ShowHeaderSelection = false;
                spreadSms.SaveChanges();
                spreadSms.Visible = true;
            }
        }
        catch
        { }
    }
    //add template 
    protected void btnaddtemplate_Click(object sender, EventArgs e)
    {
        try
        {
            // spreadSms.Visible = true;
            // UpdatePanel1.Visible = true;
            // UpdatePanel2.Visible = true;
            // divempedit.Visible = true;
            templatepanel.Visible = true;
            lblpurpose.Visible = true;
            btnplus.Visible = true;
            btnminus.Visible = true;
            ddlpurpose.Visible = true;
            txtpurposemsg.Visible = true;
            btnsave.Visible = true;
            btnexit.Visible = true;
            lblerror.Visible = false;
            loadPurpose();
        }
        catch
        {

        }
    }

    //button plus detail add
    protected void btnplus_Click(object sender, EventArgs e)
    {
        try
        {
            lblpurposecaption.Visible = true;
            txtpurposecaption.Visible = true;
            txtpurposecaption.Text = "";
            btnpurposeadd.Visible = true;
            btnpurposeexit.Visible = true;
        }
        catch
        {
        }
    }

    //button minus delete the details
    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            string getPurpose = string.Empty;
            if (ddlpurposemsg.Items.Count > 0)
                getPurpose = Convert.ToString(ddlpurposemsg.SelectedValue);
            if (!string.IsNullOrEmpty(getPurpose))
            {
                string strdelpurpose = "Delete from FT_sms_purpose where PurposePK = '" + ddlpurposemsg.SelectedValue + "'";
                int upd = da.update_method_wo_parameter(strdelpurpose, "Text");
                if (upd > 0)
                {
                    loadPurpose();
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Purpose deleted Successfully";
                }
                else
                {
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Purpose deleted Failed";
                }
            }
            else
            {
                imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                lbl_alert.Text = "Purpose deleted Failed";
            }
        }
        catch
        { }
    }

    //delete the template 
    protected void btndeletetemplate_Click(object sender, EventArgs e)
    {
        try
        {            //Cellclick = true;
            bool booCheck = false;
            spreadSms.SaveChanges();
            string txtmsg = Convert.ToString(txtmessage.Text);
            if (!string.IsNullOrEmpty(txtmsg) && spreadSms.Sheets[0].RowCount > 0)
            {
                string activerow = "";
                string activecol = "";
                activerow = spreadSms.ActiveSheetView.ActiveRow.ToString();
                activecol = spreadSms.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1)
                {
                    string msg = spreadSms.Sheets[0].GetText(ar, 1);
                    string strdeletequery = "delete   FT_sms_template where Template='" + msg + "'";
                    int vvv = da.update_method_wo_parameter(strdeletequery, "Text");
                    if (vvv == 1)
                    {
                        txtmessage.Text = "";
                        loadPurpose();
                        ddlpurpose_SelectedIndexChanged(sender, e);
                        imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                        lbl_alert.Text = "Delete Template Succefully";
                    }
                    else
                        booCheck = true;
                }
                else
                    booCheck = true;
                if (booCheck)
                {
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Delete Template  failed";
                }
            }
            else
            {
                imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                lbl_alert.Text = "Please Select Any One Template";
            }
        }
        catch
        { }
    }

    //button save
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string txtmsg = Convert.ToString(txtpurposemsg.Text);
            if (!string.IsNullOrEmpty(txtmsg))
            {
                int i = 0;
                string strsavequery = "insert into FT_sms_template (temp_code,Template,college_code)values( '" + ddlpurposemsg.SelectedValue.ToString() + "','" + txtmsg + "','" + collegecode + "')";
                i = da.update_method_wo_parameter(strsavequery, "Text");
                if (i == 1)
                {
                    divempedit.Attributes.Add("Style", "display:none");
                    loadPurpose();
                    loadPurposeDetails();
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Template added Succefully";
                }
                else
                {
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Template added failed";
                }
            }
            else
            {
                imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                lbl_alert.Text = "Please Enter Reason";
            }
        }
        catch
        { }
    }

    //button exit
    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            divempedit.Attributes.Add("Style", "display:none");
            templatepanel.Visible = false;
            divtempsecond.Attributes.Add("style", "display: none;");
            loadPurpose();
        }
        catch
        {
        }
    }
    //spread function

    protected void FpSpread2_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;
    }
    protected void FpSpread2_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (Cellclick)
            {
                string activerow = string.Empty;
                string activecol = string.Empty;
                activerow = spreadSms.ActiveSheetView.ActiveRow.ToString();
                activecol = spreadSms.ActiveSheetView.ActiveColumn.ToString();
                int ar; int ac;
                int.TryParse(activerow, out ar);
                int.TryParse(activecol, out ac);
                if (ar != -1)
                    txtmessage.Text = spreadSms.Sheets[0].GetText(ar, 1);
                Cellclick = false;
            }
        }
        catch
        { }
    }

    //purpose adding the details
    protected void btnpurposeadd_Click(object sender, EventArgs e)
    {
        try
        {
            string strtxtpurpose = Convert.ToString(txtpurposecaption.Text);
            if (!string.IsNullOrEmpty(strtxtpurpose))
            {
                string strinsertpurpose = "insert into FT_sms_purpose (Purpose,college_code) values ( '" + strtxtpurpose + "','" + collegecode + "')";
                int upd = da.update_method_wo_parameter(strinsertpurpose, "Text");
                if (upd > 0)
                {
                    loadPurpose();
                    ddlpurposemsg.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(strtxtpurpose));
                    ddlpurpose.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(strtxtpurpose));
                    divtempsecond.Attributes.Add("style", "display: none;");
                    divempedit.Attributes.Add("Style", "height: 48em;z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;top: 0; left: 0px;display:block");
                    templatepanel.Visible = true;
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Purpose added Successfully";
                }
                else
                {
                    imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                    lbl_alert.Text = "Purpose added failed";
                }
            }
            else
            {
                imgdiv2.Attributes.Add("style", "height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;display: block;");
                lbl_alert.Text = "Please Enter the Purpose";
            }
            txtpurposecaption.Text = string.Empty;
        }
        catch
        { }
    }
    protected void btnpurposeexit_Click(object sender, EventArgs e)
    {
        divempedit.Attributes.Add("Style", "height: 48em;z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;top: 0; left: 0px;display:block");
        templatepanel.Enabled = true;
        divtempsecond.Attributes.Add("style", "display: none;");
    }

    protected void txtpurposemsg_TextChanged(object sender, EventArgs e)
    {

    }

    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtmessage.Text = string.Empty;
        loadPurpose();
        ddlpurpose_SelectedIndexChanged(sender, e);
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
                da.printexcelreport(spreadDet, reportname);
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
            degreedetails = "Daily Fees Structure Report" + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "DailyFeesCollectionReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
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
            dsl = da.select_method_wo_parameter(Master1, "text");
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
            if (roll == 0)
            {
                spreadStud.Columns[3].Visible = true;
                spreadStud.Columns[4].Visible = true;
                spreadStud.Columns[5].Visible = true;
            }
            else if (roll == 1)
            {
                spreadStud.Columns[3].Visible = true;
                spreadStud.Columns[4].Visible = true;
                spreadStud.Columns[5].Visible = true;
            }
            else if (roll == 2)
            {
                spreadStud.Columns[3].Visible = true;
                spreadStud.Columns[4].Visible = false;
                spreadStud.Columns[5].Visible = false;

            }
            else if (roll == 3)
            {
                spreadStud.Columns[3].Visible = false;
                spreadStud.Columns[4].Visible = true;
                spreadStud.Columns[5].Visible = false;
            }
            else if (roll == 4)
            {
                spreadStud.Columns[3].Visible = false;
                spreadStud.Columns[4].Visible = false;
                spreadStud.Columns[5].Visible = true;
            }
            else if (roll == 5)
            {
                spreadStud.Columns[3].Visible = true;
                spreadStud.Columns[4].Visible = true;
                spreadStud.Columns[5].Visible = false;
            }
            else if (roll == 6)
            {
                spreadStud.Columns[3].Visible = false;
                spreadStud.Columns[4].Visible = true;
                spreadStud.Columns[5].Visible = true;
            }
            else if (roll == 7)
            {
                spreadStud.Columns[3].Visible = true;
                spreadStud.Columns[4].Visible = false;
                spreadStud.Columns[5].Visible = true;
            }
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

        lbl.Add(lblclg);
        lbl.Add(lbltype);
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
}