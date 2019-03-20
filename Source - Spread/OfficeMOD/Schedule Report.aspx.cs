using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

public partial class Schedule_Report : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //string PageLogOut = "";
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
        //            Response.Redirect(redrURI, false);
        //            return;
        //        }
        //    }
        //}
        //if (Session["collegecode"] == null)
        //{
        //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
        //    Response.Redirect(redrURI, false);
        //    return;
        //}
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            bindCompanyname();
            bindbatch();
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            gview.Visible = false;

        }
    }
    public void bindCompanyname()
    {
        try
        {
            gview.Visible = false;

            ds.Clear();
            cblcom.Items.Clear();
            string itemname = "select distinct CompanyPK, CompName from CompanyMaster  order by CompanyPK";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblcom.DataSource = ds;
                cblcom.DataTextField = "CompName";
                cblcom.DataValueField = "CompanyPK";
                cblcom.DataBind();


            }
            if (cblcom.Items.Count>0)
            {
                string buildvalue = string.Empty;
                string build = string.Empty;
            for (int i = 0; i < cblcom.Items.Count; i++)
            {
                if (cblcom.Items[i].Selected == true)
                {
                    build = cblcom.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            bindposition(buildvalue);
            bindedu(buildvalue);
        }
           
           
        }
        catch
        {
        }
    }
    public void chkcom_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (chkcom.Checked == true)
            {
                for (int i = 0; i < cblcom.Items.Count; i++)
                {
                    if (chkcom.Checked == true)
                    {
                        cblcom.Items[i].Selected = true;
                        txtcompany.Text = "Company(" + (cblcom.Items.Count) + ")";
                        build1 = cblcom.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < cblcom.Items.Count; i++)
                {
                    cblcom.Items[i].Selected = false;
                    txtcompany.Text = "--Select--";
                }
            }
            bindedu(buildvalue1);
            bindposition(buildvalue1);
        }
        catch (Exception ex)
        {
        }
    }
    public void cblcom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            int seatcount = 0;
            chkcom.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cblcom.Items.Count; i++)
            {
                if (cblcom.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtcompany.Text = "--Select--";
                    build = cblcom.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            if (seatcount == cbl_batch.Items.Count)
            {
                txtcompany.Text = "Company(" + seatcount.ToString() + ")";
                chkcom.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtcompany.Text = "--Select--";
                chkcom.Text = "--Select--";
            }
            else
            {
                txtcompany.Text = "Company(" + seatcount.ToString() + ")";
            }
            bindedu(buildvalue);
            bindposition(buildvalue);
        }
        catch (Exception ex)
        {
        }
    }
    public void drpcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
       // bindedu();
       // bindposition();
        gview.Visible = false;
    }
    public void bindbatch()
    {
        try
        {
            gview.Visible = false;
            cbl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                //ddl_batch1.SelectedIndex = 3;
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[0].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + 1 + ")";
                }
                else
                {
                    txt_batch.Text = "--Select--";
                    cb_batch.Checked = false;
                }
            }
        }
        catch
        {
        }
    }

    public void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (cb_batch.Checked == true)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cb_batch.Checked == true)
                    {
                        cbl_batch.Items[i].Selected = true;
                        txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                        build1 = cbl_batch.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                    txt_batch.Text = "--Select--";
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            int seatcount = 0;
            cb_batch.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_batch.Text = "--Select--";
                    build = cbl_batch.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            if (seatcount == cbl_batch.Items.Count)
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
                cb_batch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_batch.Text = "--Select--";
                cb_batch.Text = "--Select--";
            }
            else
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
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
            gview.Visible = false;
            cb_degree.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            string typ = "";
            if (cblcourse.Items.Count > 0)
            {
                for (int i = 0; i < cblcourse.Items.Count; i++)
                {
                    if (cblcourse.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cblcourse.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cblcourse.Items[i].Value + "";
                        }
                    }

                }
            }
            if (cblcom.Items.Count > 0)
            {

                for (int i = 0; i < cblcom.Items.Count; i++)
                {
                    if (cblcom.Items[i].Selected == true)
                    {
                        build = cblcom.Items[i].Value.ToString();
                        if (buildvalue == "")
                        {
                            buildvalue = build;
                        }
                        else
                        {
                            buildvalue = buildvalue + "'" + "," + "'" + build;
                        }
                    }
                }
            }
            if (typ != "" && buildvalue!="")
            {
                string deptquery = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + collegecode1 + "' and Edu_Level in('" + typ + "') ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldegree.DataSource = ds;
                    cbldegree.DataTextField = "Course_Name";
                    cbldegree.DataValueField = "Course_Id";
                    cbldegree.DataBind();
                }
                if (cbldegree.Items.Count > 0)
                {
                    string deu = "select distinct degree from IM_CompanyDept where CompanyFK in ('" + buildvalue + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deu, "Text");
                    int cun = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                        {
                            for (int i = 0; i < cbldegree.Items.Count; i++)
                            {

                                if (Convert.ToString(ds.Tables[0].Rows[m]["degree"]) == cbldegree.Items[i].Value)
                                {
                                    cun++;
                                    cbldegree.Items[i].Enabled = true;
                                    cbldegree.Items[i].Selected = true;
                                }
                                else
                                {
                                    if (cbldegree.Items[i].Selected != true)
                                        cbldegree.Items[i].Enabled = false;
                                }
                            }
                        }
                        txtdegree.Text = "Degree(" + cun + ")";
                    }
                    else
                    {
                        for (int i = 0; i < cbldegree.Items.Count; i++)
                        {
                            cbldegree.Items[i].Enabled = false;
                        }
                        txtdegree.Text = "--Select--";
                    }

                }
            }
            binddepartment();
        }
        catch
        {
        }
    }
    public void bindposition(string compk)
    {
        try
        {
            if (compk != "")
            {
                ds.Clear();
                cbldes.Items.Clear();
                string itemname = "select distinct MasterCode, MasterValue from CO_MasterValues where MasterCriteria ='Company Position' and MasterCode in(select composition from Company_datails where CompanyFK in('" + compk + "') ) order by MasterCode";
                ds.Clear();
                ds = d2.select_method_wo_parameter(itemname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldes.DataSource = ds;
                    cbldes.DataTextField = "MasterValue";
                    cbldes.DataValueField = "MasterCode";
                    cbldes.DataBind();


                }
            }
        }
        catch
        {
        }
    }
    public void chkdes_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (chkdes.Checked == true)
            {
                for (int i = 0; i < cbldes.Items.Count; i++)
                {
                    if (chkdes.Checked == true)
                    {
                        cbldes.Items[i].Selected = true;
                        txtdes.Text = "Description(" + (cbl_batch.Items.Count) + ")";
                        build1 = cbldes.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbldes.Items[i].Selected = false;
                    txtdes.Text = "--Select--";
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    public void cbldes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            int seatcount = 0;
            chkdes.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbldes.Items.Count; i++)
            {
                if (cbldes.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtdes.Text = "--Select--";
                    build = cbldes.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            if (seatcount == cbldes.Items.Count)
            {
                txtdes.Text = "Description(" + seatcount.ToString() + ")";
                chkdes.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtdes.Text = "--Select--";
                chkdes.Text = "--Select--";
            }
            else
            {
                txtdes.Text = "Description(" + seatcount.ToString() + ")";
            }

        }
        catch (Exception ex)
        {
        }
    }



    public void binddepartment()
    {
        try
        {
            gview.Visible = false;
            cb_departemt.Checked = false;
            string typ = "";
            string buildvalue = string.Empty;
            string build = string.Empty;
            if (cbldegree.Items.Count > 0)
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    if (cbldegree.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbldegree.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbldegree.Items[i].Value + "";
                        }
                    }

                }
            }
            if (cblcom.Items.Count > 0)
            {
                
                for (int i = 0; i < cblcom.Items.Count; i++)
                {
                    if (cblcom.Items[i].Selected == true)
                    {
                        build = cblcom.Items[i].Value.ToString();
                        if (buildvalue == "")
                        {
                            buildvalue = build;
                        }
                        else
                        {
                            buildvalue = buildvalue + "'" + "," + "'" + build;
                        }
                    }
                }
            }
            if (typ != "" && buildvalue!="")
            {
                string deptquery = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + typ + "') and  degree.college_code='" + collegecode1 + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldepartment.DataSource = ds;
                    cbldepartment.DataTextField = "dept_name";
                    cbldepartment.DataValueField = "degree_code";
                    cbldepartment.DataBind();
                }
                if (cbldepartment.Items.Count > 0)
                {
                    string deu = "select distinct deptcode from IM_CompanyDept where CompanyFK in('" + buildvalue + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deu, "Text");
                    int cun = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                        {
                            for (int i = 0; i < cbldepartment.Items.Count; i++)
                            {

                                if (Convert.ToString(ds.Tables[0].Rows[m]["deptcode"]) == cbldepartment.Items[i].Value)
                                {
                                    cun++;
                                    cbldepartment.Items[i].Enabled = true;
                                    cbldepartment.Items[i].Selected = true;
                                }
                                else
                                {
                                    if (cbldepartment.Items[i].Selected != true)
                                        cbldepartment.Items[i].Enabled = false;
                                }
                            }
                        }
                        txtdept.Text = "Branch(" + cun + ")";
                    }
                    else
                    {
                        for (int i = 0; i < cbldepartment.Items.Count; i++)
                        {
                            cbldepartment.Items[i].Enabled = false;

                        }
                        txtdept.Text = "--Select--";
                    }

                }

            }
        }
        catch
        {
        }
    }
    public void bindedu(string compk)
    {
        try
        {
            if (compk != "")
            {
                gview.Visible = false;
                string deptquery = " select distinct course.Edu_Level from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + collegecode1 + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblcourse.DataSource = ds;
                    cblcourse.DataTextField = "Edu_Level";
                    cblcourse.DataValueField = "Edu_Level";
                    cblcourse.DataBind();
                }
                if (cblcourse.Items.Count > 0)
                {
                    string deu = "select distinct course from IM_CompanyDept where CompanyFK in('" + compk + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deu, "Text");
                    int cun = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                        {
                            for (int i = 0; i < cblcourse.Items.Count; i++)
                            {

                                if (Convert.ToString(ds.Tables[0].Rows[m]["course"]) == cblcourse.Items[i].Value)
                                {
                                    cun++;
                                    cblcourse.Items[i].Enabled = true;
                                    cblcourse.Items[i].Selected = true;
                                }
                                else
                                {
                                    if (cblcourse.Items[i].Selected != true)
                                    {
                                        cblcourse.Items[i].Enabled = false;
                                    }
                                }
                            }
                        }
                        txtcourse.Text = "course(" + cun + ")";
                    }
                    else
                    {
                        for (int i = 0; i < cblcourse.Items.Count; i++)
                        {
                            cblcourse.Items[i].Enabled = false;
                        }
                        txtcourse.Text = "--Select--";
                    }

                }

                binddegree();
            }
        }
        catch
        {
        }

    }
    protected void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            if (cbldegree.Items.Count > 0)
            {
                int cun = 0;
                if (cb_degree.Checked == true)
                {
                    for (int i = 0; i < cbldegree.Items.Count; i++)
                    {
                        if (cbldegree.Items[i].Enabled == true)
                        {
                            cbldegree.Items[i].Selected = true;
                            cun++;
                        }
                    }
                   
                }
                else
                {
                    for (int i = 0; i < cbldegree.Items.Count; i++)
                    {
                        if (cbldegree.Items[i].Enabled == true)
                            cbldegree.Items[i].Selected = false;
                    }
                }
                txtdegree.Text = "Degree(" + cun + ")";
            }

            binddepartment();

        }
        catch
        {
        }
    }
    protected void cb_course_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            if (cblcourse.Items.Count > 0)
            {
                int cun = 0;
                if (cb_course.Checked == true)
                {
                    
                    for (int i = 0; i < cblcourse.Items.Count; i++)
                    {
                        if (cblcourse.Items[i].Enabled == true)
                        {
                            cblcourse.Items[i].Selected = true;
                            cun++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cblcourse.Items.Count; i++)
                    {
                        if (cblcourse.Items[i].Enabled == true)
                            cblcourse.Items[i].Selected = false;
                    }
                }
                txtcourse.Text = "course(" + cun + ")";
            }

            binddegree();

        }
        catch
        {
        }
    }
    protected void cbdepartment_Change(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            if (cbldepartment.Items.Count > 0)
            {
                int cun = 0;
                if (cb_departemt.Checked == true)
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        if (cbldepartment.Items[i].Enabled == true)
                        {
                            cun++;
                            cbldepartment.Items[i].Selected = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        if (cbldepartment.Items[i].Enabled == true)
                            cbldepartment.Items[i].Selected = false;
                    }
                }
                txtdept.Text = "Branch(" + cun + ")";
            }
        }
        catch
        {
        }
    }
    protected void cblcourse_ChekedChange(object sender, EventArgs e)
    {
        if (cblcourse.Items.Count > 0)
        {
            int cun = 0;
            for (int i = 0; i < cblcourse.Items.Count; i++)
            {
                if (cblcourse.Items[i].Selected == true)
                {
                    cun++;
                }
            }
            txtcourse.Text = "course(" + cun + ")";
        }
        binddegree();
        gview.Visible = false;
    }
    protected void cbldegree_ChekedChange(object sender, EventArgs e)
    {

        if (cbldegree.Items.Count > 0)
        {
            int cun = 0;
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    cun++;
                }
            }
            txtdegree.Text = "Degree(" + cun + ")";
        }

        binddepartment();
        gview.Visible = false;
    }
    protected void cbldepartment_ChekedChange(object sender, EventArgs e)
    {
        if (cbldepartment.Items.Count > 0)
        {
            int cun = 0;
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    cun++;
                }
            }
            txtdept.Text = "Branch(" + cun + ")";
        }

        gview.Visible = false;
    }
    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string fromDates = string.Empty;
            string toDates = string.Empty;
            bool isValidDate = false;
            bool isValidFromDate = false;
            bool isValidToDate = false;
            DateTime dtFromDates = new DateTime();
            DateTime dtToDates = new DateTime();
            fromDates = Convert.ToString(txt_fromdate.Text).Trim();
            toDates = Convert.ToString(txt_todate.Text).Trim();
            if (chkdate.Checked == true)
            {
                if (fromDates.Trim() != "")
                {
                    isValidDate = false;
                    isValidDate = DateTime.TryParseExact(fromDates.Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtFromDates);
                    isValidFromDate = isValidDate;
                    if (!isValidDate)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";

                        return;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Choose From Date";

                    return;

                }
                if (toDates.Trim() != "")
                {
                    isValidDate = false;
                    isValidDate = DateTime.TryParseExact(toDates.Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtToDates);
                    isValidToDate = isValidDate;
                    if (!isValidDate)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";

                        return;

                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Choose To Date";

                    return;
                }

                if (dtFromDates > dtToDates)
                {
                    lbl_alert.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                    imgdiv2.Visible = true;
                    return;
                }

            }
            string Batch_tagvalue = string.Empty;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    string addbatch1 = cbl_batch.Items[i].Value.ToString();
                    if (Batch_tagvalue == "")
                    {
                        Batch_tagvalue = addbatch1;
                    }
                    else
                    {
                        Batch_tagvalue = Batch_tagvalue + "'" + "," + "'" + addbatch1;
                    }
                }
            }
            string branch = string.Empty;
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    string branch1 = cbldepartment.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = branch1;
                    }
                    else
                    {
                        branch = branch + "'" + "," + "'" + branch1;
                    }
                }
            }
            string buildvalue = string.Empty;
            string build = string.Empty;
            if (cblcom.Items.Count > 0)
            {

                for (int i = 0; i < cblcom.Items.Count; i++)
                {
                    if (cblcom.Items[i].Selected == true)
                    {
                        build = cblcom.Items[i].Value.ToString();
                        if (buildvalue == "")
                        {
                            buildvalue = build;
                        }
                        else
                        {
                            buildvalue = buildvalue + "'" + "," + "'" + build;
                        }
                    }
                }
            }
            string position = string.Empty;
            string posi = string.Empty;
            if (cbldes.Items.Count > 0)
            {

                for (int i = 0; i < cbldes.Items.Count; i++)
                {
                    if (cbldes.Items[i].Selected == true)
                    {
                        posi = cbldes.Items[i].Value.ToString();
                        if (position == "")
                        {
                            position = posi;
                        }
                        else
                        {
                            position = position + "'" + "," + "'" + posi;
                        }
                    }
                }
            }
            #region details
           
                #region datatable
                DataRow drrow = null;
                DataTable dtTTDisp = new DataTable();
                dtTTDisp.Columns.Add("SNo.");
                dtTTDisp.Columns.Add("Date");
                dtTTDisp.Columns.Add("Time");
                dtTTDisp.Columns.Add("Company Name");
                dtTTDisp.Columns.Add("Deparment");
                dtTTDisp.Columns.Add("No Of Student");
                int y = dtTTDisp.Columns.Count;
                drrow = dtTTDisp.NewRow();
                drrow["SNo."] = "SNo.";
                drrow["Date"] = "Date";
                drrow["Time"] = "Time";
                drrow["Company Name"] = "Company Name";
                drrow["Deparment"] = "Deparment";
                drrow["No Of Student"] = "No Of Student";
                dtTTDisp.Rows.Add(drrow);
                #endregion
                string datequer = string.Empty;
                string qury = string.Empty;
             if (chkdate.Checked == true)
             {
                string fromdate = string.Empty;
                fromdate = txt_fromdate.Text;
                string[] spl = fromdate.Split('/');
                fromdate = Convert.ToString(Convert.ToString(spl[2]).Trim() + "-" + Convert.ToString(spl[1]).Trim() + "-" + Convert.ToString(spl[0]).Trim());
                string todate = string.Empty;
                todate = txt_todate.Text;
                string[] spls = todate.Split('/');
                todate = Convert.ToString(Convert.ToString(spls[2]).Trim() + "-" + Convert.ToString(spls[1]).Trim() + "-" + Convert.ToString(spls[0]).Trim());
                    datequer = "and cd.interviewdate between '" + fromdate + "' and '" + todate + "'";
                 }
             if (Batch_tagvalue != "" && buildvalue != "" && branch != "" && position!="")
                {

                    qury = "select r.Stud_Name,r.batch_year,r.Roll_No,r.Reg_No,r.App_No,cm.CompanyPK,Dept_Name,c.Course_Id,r.degree_code,dt.Dept_Code,c.course_name,r.Current_Semester,r.Sections,(select MasterValue from CO_MasterValues where MasterCode=cd.composition and MasterCriteria ='Company Position') as appposted,CONVERT(nvarchar,cd.interviewdate,103) as interviewdate  from  Company_StudentRegistration cr, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK in('" + buildvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk  and cd.interviewdate=cr.interviewdate " + datequer + " and r.degree_code in('" + branch + "') order by r.Roll_No";

                    qury = qury + "  select cm.CompanyPK,compname,CONVERT(nvarchar,interviewdate,103) as interviewdate,interviewtime,course,degree,deptcode,MasterValue,(select Dept_Name  from Department where dept_code in(select dept_code from degree where degree_code =deptcode) )as deptname from  CompanyMaster cm, Company_datails cd,IM_CompanyDept dp,CO_MasterValues co where dp.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK in('" + buildvalue + "') and deptcode in('" + branch + "') " + datequer + " and MasterCode=cd.composition and MasterCode in('" + position + "') and co.MasterCriteria ='Company Position' order by CONVERT(nvarchar,interviewdate,103) asc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(qury, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "No Record";
                    }
                    else
                    {
                        int cun = 0;
                        
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {

                                cun++;
                                drrow = dtTTDisp.NewRow();
                                drrow["SNo."] = cun;
                                drrow["Company Name"] = Convert.ToString(ds.Tables[1].Rows[i]["compname"]);
                                drrow["Date"] = Convert.ToString(ds.Tables[1].Rows[i]["interviewdate"]);
                                drrow["Time"] = Convert.ToString(ds.Tables[1].Rows[i]["interviewtime"]);
                                drrow["Deparment"] = Convert.ToString(ds.Tables[1].Rows[i]["deptname"]);
                                ds.Tables[0].DefaultView.RowFilter = "Course_Id='" + Convert.ToString(ds.Tables[1].Rows[i]["degree"]) + "' and degree_code='" + Convert.ToString(ds.Tables[1].Rows[i]["deptcode"]) + "'  and CompanyPK='" + Convert.ToString(ds.Tables[1].Rows[i]["CompanyPK"]) + "'   and interviewdate='" + Convert.ToString(ds.Tables[1].Rows[i]["interviewdate"]) + "'";
                                DataView   dvStudentAttendances = ds.Tables[0].DefaultView;
                                if (dvStudentAttendances.Count > 0)
                                {
                                    drrow["No Of Student"] = dvStudentAttendances.Count;
                                }

                                dtTTDisp.Rows.Add(drrow);
                        }

                        if (dtTTDisp.Rows.Count > 1)
                        {
                            gview.DataSource = dtTTDisp;
                            gview.DataBind();
                            gview.Visible = true;
                            div_report.Visible = true;
                            #region span
                            for (int i = gview.Rows.Count - 1; i >= 1; i--)
                            {
                                GridViewRow row = gview.Rows[i];
                                GridViewRow previousRow = gview.Rows[i - 1];
                                for (int j = 0; j < row.Cells.Count; j++)
                                {
                                    string date = row.Cells[j].Text;
                                    string predate = previousRow.Cells[j].Text;
                                    if (date == predate)
                                    {
                                        if (previousRow.Cells[j].RowSpan == 0)
                                        {
                                            if (row.Cells[j].RowSpan == 0)
                                            {
                                                previousRow.Cells[j].RowSpan += 2;
                                            }
                                            else
                                            {
                                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                            }
                                            row.Cells[j].Visible = false;
                                        }
                                    }
                                }
                                row.Cells[0].Visible = false;
                                gview.Rows[0].Cells[0].Visible = false;
                            }
                            RowHead(gview);
                            #endregion span
                        }
                        else
                        {

                            imgdiv2.Visible = true;
                            lbl_alert.Text = "No Record";
                            gview.Visible = false;
                        }
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select All Feild";
                    gview.Visible = false;
                }
            
            #endregion
        

        }
        catch
        {
        }
    }
    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 1; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        gview.Visible = false;
    }
  
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = null;
            string degreedetails = "Placement OverAll Report";
            string pagename = "Placement Report.aspx";
            NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
            ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
            NEWPrintMater1.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;

            if (report.ToString().Trim() != "")
            {

                d2.printexcelreportgrid(gview, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();

        }
        catch (Exception ex)
        {
            // lbl_norec.Text = ex.ToString();
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
        }
        catch (Exception ex) { }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void chkdate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdate.Checked == true)
            {
                txt_fromdate.Enabled = true;
                txt_todate.Enabled = true;
            }
            else
            {
                txt_fromdate.Enabled = false;
                txt_todate.Enabled = false;
            }
        }
        catch
        {
        }
    }


  
}