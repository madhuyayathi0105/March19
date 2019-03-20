using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

public partial class Placement_Report : System.Web.UI.Page
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
            // interviewround();
            bindedu();
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
          //  binddate();
            gview.Visible = false;
          //  btnsave.Visible = false;
        }
    }
    public void bindCompanyname()
    {
        try
        {
            gview.Visible = false;
          //  btnsave.Visible = false;
            ds.Clear();
            drpcompany.Items.Clear();
            string itemname = "select distinct CompanyPK, CompName from CompanyMaster  order by CompanyPK";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                drpcompany.DataSource = ds;
                drpcompany.DataTextField = "CompName";
                drpcompany.DataValueField = "CompanyPK";
                drpcompany.DataBind();


            }
            bindedu();
        }
        catch
        {
        }
    }
    //public void binddate()
    //{
    //    try
    //    {
    //        gview.Visible = false;
    //       // btnsave.Visible = false;
    //        ddldate.Items.Clear();
    //        string datebind = "select convert(varchar, interviewdate, 103) as interviewdate  from Company_datails where CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "'";
    //        DataSet dsdate = new DataSet();
    //        dsdate = d2.select_method_wo_parameter(datebind, "text");
    //        if (dsdate.Tables[0].Rows.Count > 0)
    //        {
    //            ddldate.DataSource = dsdate;
    //            ddldate.DataTextField = "interviewdate";
    //            ddldate.DataValueField = "interviewdate";
    //            ddldate.DataBind();
    //        }
    //    }
    //    catch
    //    {
    //    }

    //}
    public void drpcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindedu();
       // binddate();
        gview.Visible = false;
       // btnsave.Visible = false;
    }
    public void bindbatch()
    {
        try
        {
            gview.Visible = false;
           // btnsave.Visible = false;
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
                    // txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    txt_batch.Text = "Batch(" + 1 + ")";
                    //cb_batch.Checked = true;
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
           // btnsave.Visible = false;
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
         //   btnsave.Visible = false;
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
        //    btnsave.Visible = false;
            cb_degree.Checked = false;
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
            if (typ != "")
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
                    string deu = "select distinct degree from IM_CompanyDept where CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "'";
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

    public void binddepartment()
    {
        try
        {
            gview.Visible = false;
          //  btnsave.Visible = false;
            cb_departemt.Checked = false;
            string typ = "";
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
            if (typ != "")
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
                    string deu = "select distinct deptcode from IM_CompanyDept where CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "'";
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
    public void bindedu()
    {
        try
        {
         //   btnsave.Visible = false;
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
                string deu = "select distinct course from IM_CompanyDept where CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "'";
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
        catch
        {
        }

    }
    protected void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        try
        {
          //  btnsave.Visible = false;
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
         //   btnsave.Visible = false;
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
                            cun++;
                            cblcourse.Items[i].Selected = true;
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
         //   btnsave.Visible = false;
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
            if (cb_course.Checked == true)
            {
                for (int i = 0; i < cblcourse.Items.Count; i++)
                {
                    if (cblcourse.Items[i].Selected == true)
                    {
                        cun++;
                    }
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
    protected void cbl_accept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cblaccept.Items.Count > 0)
        {
            int cun = 0;
            for (int i = 0; i < Cblaccept.Items.Count; i++)
            {
                if (Cblaccept.Items[i].Selected == true)
                {
                    cun++;
                }
            }
            txaccept.Text = "Shortlist(" + cun + ")";
        }

        gview.Visible = false;
    }
    protected void cb_accept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
         //   btnsave.Visible = false;
            gview.Visible = false;
            if (Cblaccept.Items.Count > 0)
            {
                int cun = 0;
                if (cb_accept.Checked == true)
                {
                    for (int i = 0; i < Cblaccept.Items.Count; i++)
                    {
                        cun++;
                        Cblaccept.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < Cblaccept.Items.Count; i++)
                    {
                        cun = 0;
                        Cblaccept.Items[i].Selected = false;
                    }
                }

                txaccept.Text = "Shortlist(" + cun + ")";
            }
        }
        catch
        {
        }
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
            #region details
            if (rdbdetails.Checked == true)
            {
                #region datatable
                DataRow drrow = null;
                DataTable dtTTDisp = new DataTable();
                dtTTDisp.Columns.Add("App_no");
                dtTTDisp.Columns.Add("SNo.");
                dtTTDisp.Columns.Add("Roll No");
                dtTTDisp.Columns.Add("Reg No");
                dtTTDisp.Columns.Add("Student Name");
                dtTTDisp.Columns.Add("Batch");
                dtTTDisp.Columns.Add("Section");
                dtTTDisp.Columns.Add("Semester");
                dtTTDisp.Columns.Add("Applied Post");
                int y = dtTTDisp.Columns.Count;
                drrow = dtTTDisp.NewRow();
                drrow["App_no"] = "App_no";
                drrow["SNo."] = "SNo.";
                drrow["Roll No"] = "Roll No";
                drrow["Reg No"] = "Reg No";
                drrow["Student Name"] = "Student Name";
                drrow["Batch"] = "Batch";
                drrow["Semester"] = "Semester";
                drrow["Section"] = "Section";
                drrow["Applied Post"] = "Applied Post";
                dtTTDisp.Rows.Add(drrow);
                #endregion
                string qury = string.Empty;
                string datequer = string.Empty;
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
              
                if (Batch_tagvalue != "" && drpcompany.SelectedValue != "" && branch != "")
                {
                    if (rdbShortlist.Checked == true)
                    {
                        string shoqyery = string.Empty;
                        if (Chkshort.Checked == true)
                        {
                            if (Cblaccept.Items[0].Selected == true && Cblaccept.Items[1].Selected == true && Cblaccept.Items[2].Selected == true)
                                shoqyery = "";
                            else if (Cblaccept.Items[0].Selected == true && Cblaccept.Items[1].Selected == true)
                                shoqyery = "and  Shortlist_flag='1' ";
                            else if (Cblaccept.Items[0].Selected == true && Cblaccept.Items[2].Selected == true)
                                shoqyery = "and ((Shortlist_flag='1' and stu_desire<>'2') or Shortlist_flag=0) ";
                            else if (Cblaccept.Items[1].Selected == true && Cblaccept.Items[2].Selected == true)
                                shoqyery = "and (Shortlist_flag='1'  or Shortlist_flag=0) and  stu_desire='2'";

                            else if (Cblaccept.Items[0].Selected == true)
                                shoqyery = "and  Shortlist_flag='1' and stu_desire='1'";
                            else if (Cblaccept.Items[1].Selected == true)
                                shoqyery = "and  Shortlist_flag='1' and stu_desire='2'";
                            else if (Cblaccept.Items[2].Selected == true)
                                shoqyery = "and  Shortlist_flag='0' and stu_desire='2'";
                        }
                        else
                        {
                            shoqyery ="and  Shortlist_flag='1'";
                        }
                        qury = "select r.Stud_Name,r.batch_year,r.Roll_No,r.Reg_No,r.App_No,Dept_Name,c.course_name,r.Current_Semester,r.Sections,(select MasterValue from CO_MasterValues where MasterCode=cd.composition and MasterCriteria ='Company Position') as appposted from  Company_StudentRegistration cr, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk " + datequer + " and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') " + shoqyery + " order by r.Roll_No,r.batch_year";
                    }
                    else if (rdbApplied.Checked == true || rdbnotApplied.Checked == true)
                    {

                        qury = "select r.Stud_Name,r.batch_year,r.Roll_No,r.Reg_No,r.App_No,Dept_Name,c.Course_Id,r.degree_code,c.course_name,r.Current_Semester,r.Sections,(select MasterValue from CO_MasterValues where MasterCode=cd.composition and MasterCriteria ='Company Position') as appposted,* from  Company_StudentRegistration cr,Cm_Attendance ca, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk " + datequer + "  and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') and ca.App_No=cr.APP_No and ca.CompanyFK=cm.CompanyPK  order by r.Roll_No,r.batch_year,c.Course_Id,r.degree_code";
                        qury = qury + "  select cm.CompanyPK,compname,CONVERT(nvarchar,interviewdate,103) as interviewdate,interviewtime,course,degree,deptcode,(select Dept_Name  from Department where dept_code in(select dept_code from degree where degree_code =deptcode) )as deptname from  CompanyMaster cm, Company_datails cd,IM_CompanyDept dp  where dp.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK in('" + Convert.ToString(drpcompany.SelectedValue) + "') and deptcode in('" + branch + "') " + datequer + " order by CONVERT(nvarchar,interviewdate,103) asc,degree,deptcode";
                    }
                    else
                    {
                        qury = "select r.Stud_Name,r.batch_year,r.Roll_No,r.Reg_No,r.App_No,Dept_Name,c.course_name,r.Current_Semester,r.Sections,(select MasterValue from CO_MasterValues where MasterCode=cd.composition and MasterCriteria ='Company Position') as appposted,cd.rounds,* from  Company_StudentRegistration cr,Cm_Studentselection ca, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk " + datequer + "  and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') and ca.App_No=cr.APP_No and ca.CompanyFK=cm.CompanyPK and cd.interviewdate=ca.interviewdate  order by r.Roll_No";
                    }
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
                        if (rdbShortlist.Checked == true)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {

                                cun++;
                                drrow = dtTTDisp.NewRow();
                                if (i == 0)
                                {
                                    drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                    dtTTDisp.Rows.Add(drrow);
                                }
                                else
                                {

                                    if (Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]) != Convert.ToString(ds.Tables[0].Rows[i - 1]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]))
                                    {
                                        drrow = dtTTDisp.NewRow();
                                        drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                        dtTTDisp.Rows.Add(drrow);
                                    }
                                }
                                drrow = dtTTDisp.NewRow();
                                drrow["App_no"] = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                                drrow["SNo."] = cun;
                                drrow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                drrow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                drrow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]);
                                drrow["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                                drrow["Section"] = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                drrow["Applied Post"] = Convert.ToString(ds.Tables[0].Rows[i]["appposted"]);
                                dtTTDisp.Rows.Add(drrow);
                            }
                        }
                        else if (rdbApplied.Checked == true || rdbnotApplied.Checked == true)
                        {
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                               
                               string  fromdates = Convert.ToString(ds.Tables[1].Rows[i]["interviewdate"]);
                                string dates = string.Empty;
                                dates = fromdates;
                                string[] sp = dates.Split('/');
                                string fromdat = sp[1] + '/' + sp[0] + '/' + sp[2];
                                int getdate = 0;
                                int.TryParse(sp[0], out getdate);
                                if (getdate < 10)
                                {
                                    String startOfString = sp[0].Remove(0, 1);
                                    sp[0] = startOfString;
                                }
                                string colmname = "D" + Convert.ToString(sp[0]);
                                if (rdbApplied.Checked == true)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "" + colmname + "='1' and interviewdate='" + fromdat + "' and  Course_Id='" + Convert.ToString(ds.Tables[1].Rows[i]["degree"]) + "' and degree_code='" + Convert.ToString(ds.Tables[1].Rows[i]["deptcode"]) + "'";
                                    DataView dvStudentAttendance = ds.Tables[0].DefaultView;
                                    Hashtable degrees = new Hashtable();
                                    if (dvStudentAttendance.Count > 0)
                                    {
                                        for (int m = 0; m < dvStudentAttendance.Count; m++)
                                        {
                                            cun++;
                                            drrow = dtTTDisp.NewRow();
                                            if (m == 0)
                                            {
                                                drrow["SNo."] =fromdates+'-'+  Convert.ToString(dvStudentAttendance[m]["course_name"]) + '-' + Convert.ToString(dvStudentAttendance[m]["Dept_Name"]);
                                                degrees.Add(cun, Convert.ToString(dvStudentAttendance[m]["course_name"]) + '-' + Convert.ToString(dvStudentAttendance[m]["Dept_Name"]));
                                                dtTTDisp.Rows.Add(drrow);
                                            }
                                            else
                                            {
                                                if (!degrees.ContainsValue( Convert.ToString(dvStudentAttendance[m]["course_name"]) + '-' + Convert.ToString(dvStudentAttendance[m]["Dept_Name"])))
                                                {

                                                    drrow = dtTTDisp.NewRow();
                                                    drrow["SNo."] = fromdates + '-' + Convert.ToString(dvStudentAttendance[m]["course_name"]) + '-' + Convert.ToString(dvStudentAttendance[m]["Dept_Name"]);
                                                    dtTTDisp.Rows.Add(drrow);

                                                }
                                            }
                                            drrow = dtTTDisp.NewRow();
                                            drrow["App_no"] = Convert.ToString(dvStudentAttendance[m]["App_No"]);
                                            drrow["SNo."] = cun;
                                            drrow["Student Name"] = Convert.ToString(dvStudentAttendance[m]["Stud_Name"]);
                                            drrow["Roll No"] = Convert.ToString(dvStudentAttendance[m]["Roll_No"]);
                                            drrow["Reg No"] = Convert.ToString(dvStudentAttendance[m]["Reg_No"]);
                                            drrow["Batch"] = Convert.ToString(dvStudentAttendance[m]["Batch_year"]);
                                            drrow["Semester"] = Convert.ToString(dvStudentAttendance[m]["Current_Semester"]);
                                            drrow["Section"] = Convert.ToString(dvStudentAttendance[m]["Sections"]);
                                            drrow["Applied Post"] = Convert.ToString(dvStudentAttendance[m]["appposted"]);

                                            dtTTDisp.Rows.Add(drrow);
                                        }
                                    }
                                }
                                else if (rdbnotApplied.Checked == true)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "" + colmname + "='2' and interviewdate='" + fromdat + "' and  Course_Id='" + Convert.ToString(ds.Tables[1].Rows[i]["degree"]) + "' and degree_code='" + Convert.ToString(ds.Tables[1].Rows[i]["deptcode"]) + "'";
                                    DataView dvStudentAttendance = ds.Tables[0].DefaultView;
                                    Hashtable degrees = new Hashtable();
                                    if (dvStudentAttendance.Count > 0)
                                    {
                                        for (int m = 0; m < dvStudentAttendance.Count; m++)
                                        {
                                            cun++;
                                            drrow = dtTTDisp.NewRow();
                                            if (m == 0)
                                            {
                                                drrow["SNo."] = fromdates + '-' + Convert.ToString(dvStudentAttendance[m]["course_name"]) + '-' + Convert.ToString(dvStudentAttendance[m]["Dept_Name"]);
                                                degrees.Add(cun, Convert.ToString(dvStudentAttendance[m]["course_name"]) + '-' + Convert.ToString(dvStudentAttendance[m]["Dept_Name"]));
                                                dtTTDisp.Rows.Add(drrow);
                                            }
                                            else
                                            {
                                                if (!degrees.ContainsValue(Convert.ToString(dvStudentAttendance[m]["course_name"]) + '-' + Convert.ToString(dvStudentAttendance[0]["Dept_Name"])))
                                                {

                                                    drrow = dtTTDisp.NewRow();
                                                    drrow["SNo."] = fromdates + '-' + Convert.ToString(dvStudentAttendance[m]["course_name"]) + '-' + Convert.ToString(dvStudentAttendance[m]["Dept_Name"]);
                                                    dtTTDisp.Rows.Add(drrow);

                                                }
                                            }
                                            drrow = dtTTDisp.NewRow();
                                            drrow["App_no"] = Convert.ToString(dvStudentAttendance[m]["App_No"]);
                                            drrow["SNo."] = cun;
                                            drrow["Student Name"] = Convert.ToString(dvStudentAttendance[m]["Stud_Name"]);
                                            drrow["Roll No"] = Convert.ToString(dvStudentAttendance[m]["Roll_No"]);
                                            drrow["Reg No"] = Convert.ToString(dvStudentAttendance[m]["Reg_No"]);
                                            drrow["Batch"] = Convert.ToString(dvStudentAttendance[m]["Batch_year"]);
                                            drrow["Semester"] = Convert.ToString(dvStudentAttendance[m]["Current_Semester"]);
                                            drrow["Section"] = Convert.ToString(dvStudentAttendance[m]["Sections"]);
                                            drrow["Applied Post"] = Convert.ToString(dvStudentAttendance[m]["appposted"]);

                                            dtTTDisp.Rows.Add(drrow);
                                        }
                                    }
                                }
                            }
                        }
                        else if (rdbSelected.Checked == true)
                        {
                            int m = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {

                                drrow = dtTTDisp.NewRow();
                                string round = Convert.ToString(ds.Tables[0].Rows[i]["rounds"]);
                                if (Convert.ToString(ds.Tables[0].Rows[i]["R" + round]) == "1")
                                {
                                    if (m == 0)
                                    {
                                        drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                        dtTTDisp.Rows.Add(drrow);
                                        m++;
                                    }
                                    else
                                    {

                                        if (Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]) != Convert.ToString(ds.Tables[0].Rows[i - 1]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]))
                                        {
                                            drrow = dtTTDisp.NewRow();
                                            drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                            dtTTDisp.Rows.Add(drrow);
                                        }
                                    }
                                    cun++;
                                    drrow = dtTTDisp.NewRow();
                                    drrow["App_no"] = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                                    drrow["SNo."] = cun;
                                    drrow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                    drrow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                    drrow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                    drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]);
                                    drrow["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                                    drrow["Section"] = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                    drrow["Applied Post"] = Convert.ToString(ds.Tables[0].Rows[i]["appposted"]);
                                    dtTTDisp.Rows.Add(drrow);
                                }

                            }
                        }
                        else
                        {
                            if (chkinclu.Checked == true)
                            {
                                int m = 0;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    drrow = dtTTDisp.NewRow();
                                    string round = Convert.ToString(ds.Tables[0].Rows[i]["rounds"]);
                                    int getround = ddlround.Items.IndexOf(ddlround.Items.FindByText(Convert.ToString(ddlround.SelectedValue))); ;
                                    //  string getround = Convert.ToString(ddlround.SelectedValue);
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["R" + round]) != "1")
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i]["R" + getround]) == "1")
                                        {
                                            if (m == 0)
                                            {
                                                drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                                dtTTDisp.Rows.Add(drrow);
                                                m++;
                                            }
                                            else
                                            {

                                                if (Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]) != Convert.ToString(ds.Tables[0].Rows[i - 1]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]))
                                                {
                                                    drrow = dtTTDisp.NewRow();
                                                    drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                                    dtTTDisp.Rows.Add(drrow);
                                                }
                                            }
                                            cun++;
                                            drrow = dtTTDisp.NewRow();
                                            drrow["App_no"] = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                                            drrow["SNo."] = cun;
                                            drrow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                            drrow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                            drrow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                            drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]);
                                            drrow["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                                            drrow["Section"] = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                            drrow["Applied Post"] = Convert.ToString(ds.Tables[0].Rows[i]["appposted"]);
                                            dtTTDisp.Rows.Add(drrow);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int m = 0;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    drrow = dtTTDisp.NewRow();
                                    string round = Convert.ToString(ds.Tables[0].Rows[i]["rounds"]);
                                    int getround = ddlround.Items.IndexOf(ddlround.Items.FindByText(Convert.ToString(ddlround.SelectedValue))); ;
                                    //  string getround = Convert.ToString(ddlround.SelectedValue);
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["R" + round]) != "1")
                                    {
                                       
                                            if (m == 0)
                                            {
                                                drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                                dtTTDisp.Rows.Add(drrow);
                                                m++;
                                            }
                                            else
                                            {

                                                if (Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]) != Convert.ToString(ds.Tables[0].Rows[i - 1]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]))
                                                {
                                                    drrow = dtTTDisp.NewRow();
                                                    drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                                    dtTTDisp.Rows.Add(drrow);
                                                }
                                            }
                                            cun++;
                                            drrow = dtTTDisp.NewRow();
                                            drrow["App_no"] = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                                            drrow["SNo."] = cun;
                                            drrow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                            drrow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                            drrow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                            drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]);
                                            drrow["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                                            drrow["Section"] = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                            drrow["Applied Post"] = Convert.ToString(ds.Tables[0].Rows[i]["appposted"]);
                                            dtTTDisp.Rows.Add(drrow);
                                        
                                    }
                                }
                            }
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

                                //for (int j = row.Cells.Count - 1; j >= 1; j--)
                                //{
                                //    GridViewRow rows = gview.Rows[0];
                                //    GridViewRow previousRows = gview.Rows[0];
                                //    GridViewRow previousRowss = gview.Rows[2];
                                //    string date = gview.Rows[0].Cells[j].Text;
                                //    string predate = gview.Rows[0].Cells[j - 1].Text;
                                //}
                                for (int m = gview.Rows.Count - 1; m >= 1; m--)
                                {
                                    GridViewRow rows = gview.Rows[m];
                                    GridViewRow previousRows = gview.Rows[m];
                                    GridViewRow previousRowss = gview.Rows[m];
                                    gview.Rows[m].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                    string cellte = gview.Rows[m].Cells[1].Text;
                                    if (!Convert.ToString(cellte).All(char.IsNumber))
                                    {
                                        gview.Rows[m].Cells[1].ColumnSpan = gview.Rows[m].Cells.Count;

                                        gview.Rows[m].Cells[1].ColumnSpan = gview.Rows[m].Cells.Count;
                                        for (int j = 2; j < gview.Rows[m].Cells.Count; j++)
                                        {
                                            gview.Rows[m].Cells[0].Visible = false;
                                            gview.Rows[m].Cells[j].Visible = false;
                                        }
                                    }
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
            }
            #endregion 
            #region cumulative
            else
            {
                #region datatable
                DataRow drrow = null;
                DataTable dtTTDisp = new DataTable();
                dtTTDisp.Columns.Add("SNo.");
                dtTTDisp.Columns.Add("Batch");
                dtTTDisp.Columns.Add("Degree");
                dtTTDisp.Columns.Add("Branch");
                dtTTDisp.Columns.Add("Section");
                dtTTDisp.Columns.Add("Semester");
                dtTTDisp.Columns.Add("Shortlist");
                dtTTDisp.Columns.Add("Applied");
                dtTTDisp.Columns.Add("NotApplied");
                dtTTDisp.Columns.Add("Selected");
                dtTTDisp.Columns.Add("Not Selected");
                int y = dtTTDisp.Columns.Count;
                drrow = dtTTDisp.NewRow();
                drrow["Selected"] = "Selected";
                drrow["SNo."] = "SNo.";
                drrow["Not Selected"] = "Not Selected";
                drrow["Applied"] = "Attened";
                drrow["NotApplied"] = "Not Attened";
                drrow["Shortlist"] = "Shortlist";
                drrow["Batch"] = "Batch";
                drrow["Degree"] = "Degree";
                drrow["Branch"] = "Branch";
                drrow["Semester"] = "Semester";
                drrow["Section"] = "Section";
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
                if (Batch_tagvalue != "" && drpcompany.SelectedValue != ""  && branch != "")
                {
                    qury = "select COUNT( r.app_no)as TotalStrength,r.degree_code,r.Batch_Year,r.Current_Semester,r.Sections,C.Course_Name,C.Course_Id ,Dt.Dept_Name from  Company_StudentRegistration cr, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk " + datequer + " and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') and Shortlist_flag='1' group by r.Batch_Year, r.degree_code ,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,C.Course_Id,r.Sections";
                    qury = qury + " select r.Stud_Name,r.batch_year,r.Roll_No,r.Reg_No,r.App_No,Dept_Name,c.course_name,c.Course_Id,r.degree_code,r.Current_Semester,r.Sections,(select MasterValue from CO_MasterValues where MasterCode=cd.composition and MasterCriteria ='Company Position') as appposted,* from  Company_StudentRegistration cr,Cm_Attendance ca, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk " + datequer + "  and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') and ca.App_No=cr.APP_No and ca.CompanyFK=cm.CompanyPK  order by r.Roll_No";

                    // qury = qury + " select COUNT(distinct r.app_no)as TotalStrength,r.degree_code,r.Sections,r.Batch_Year,r.Current_Semester,C.Course_Name,C.Course_Id ,Dt.Dept_Name from  Company_StudentRegistration cr,Cm_Attendance ca, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk and cd.interviewdate between '" + fromdate + "' and '" + todate + "' and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') and ca.App_No=cr.APP_No and ca.CompanyFK=cm.CompanyPK and " + colmname + "='1' group by r.degree_code ,r.Batch_Year,r.Current_Semester,C.Course_Name ,Dt.Dept_Name,r.Sections,C.Course_Id";


                    //  qury = qury + " select * from  Company_StudentRegistration cr, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd,Cm_Studentselection cs where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk and cd.interviewdate between '" + fromdate + "' and '" + todate + "' and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') and cm.CompanyPK=cm.CompanyPK";

                    qury = qury + " select r.Stud_Name,r.batch_year,r.Roll_No,r.Reg_No,r.App_No,Dept_Name,c.course_name,r.Current_Semester,r.Sections,(select MasterValue from CO_MasterValues where MasterCode=cd.composition and MasterCriteria ='Company Position') as appposted,cd.rounds,* from  Company_StudentRegistration cr,Cm_Studentselection ca, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk " + datequer + " and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') and ca.App_No=cr.APP_No and ca.CompanyFK=cm.CompanyPK and cd.interviewdate=ca.interviewdate  order by r.Roll_No";
                    qury = qury + "  select cm.CompanyPK,compname,CONVERT(nvarchar,interviewdate,103) as interviewdate,interviewtime,course,degree,deptcode,(select Dept_Name  from Department where dept_code in(select dept_code from degree where degree_code =deptcode) )as deptname from  CompanyMaster cm, Company_datails cd,IM_CompanyDept dp  where dp.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK in('" + Convert.ToString(drpcompany.SelectedValue) + "') and deptcode in('" + branch + "') " + datequer + " order by CONVERT(nvarchar,interviewdate,103) asc";

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
                        int shorcun = 0; 
                        int attedcun = 0; 
                        int selectcun = 0; int notselect = 0; int notattedcun = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            #region shortlist
                            cun++;
                            drrow = dtTTDisp.NewRow();
                            drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                            drrow["SNo."] = cun;
                            drrow["Degree"] = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                            drrow["Branch"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]);
                            drrow["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                            drrow["Section"] = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                            ds.Tables[0].DefaultView.RowFilter = "Course_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]) + "' and Dept_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]) + "' and Batch_year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "'";
                            DataView dvStudentAttendance = ds.Tables[0].DefaultView;
                            if (dvStudentAttendance.Count > 0)
                            {
                                drrow["Shortlist"] = Convert.ToString(dvStudentAttendance[0]["TotalStrength"]);
                                shorcun += Convert.ToInt32(dvStudentAttendance[0]["TotalStrength"]);
                            }
                            else
                                drrow["Shortlist"] = 0;
                            #endregion
                            #region Attened
                            DataView dvStudentAttendances = new DataView();
                            DataView studatten = new DataView();
                            DataView selected = new DataView();
                            DataView notselected = new DataView();
                            int cunt = 0;
                            ds.Tables[3].DefaultView.RowFilter = "degree='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]) + "' and deptcode='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "'";
                            studatten = ds.Tables[3].DefaultView;
                            if (studatten.Count>0)
                            {
                            for (int m = 0; m < studatten.Count; m++)
                            {

                                string fromdates = Convert.ToString(studatten[m]["interviewdate"]);
                                string dates = string.Empty;
                                dates = fromdates;
                                string[] sp = dates.Split('/');
                                string fromdat = sp[1] + '/' + sp[0] + '/' + sp[2];
                                int getdate = 0;
                                int.TryParse(sp[0], out getdate);
                                if (getdate < 10)
                                {
                                    String startOfString = sp[0].Remove(0, 1);
                                    sp[0] = startOfString;
                                }
                                string colmname = "D" + Convert.ToString(sp[0]);
                                ds.Tables[1].DefaultView.RowFilter = "Course_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]) + "' and Dept_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]) + "' and Batch_year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' and " + colmname + "='1'  and interviewdate='" + fromdat + "'";
                                dvStudentAttendances = ds.Tables[1].DefaultView;
                                cunt += dvStudentAttendances.Count;
                            }
                            }
                            if (cunt > 0)
                            {
                                drrow["Applied"] = cunt;
                                attedcun += cunt;
                            }
                            else
                                drrow["Applied"] = 0;

                            #endregion
                            #region Not Attened
                            DataView dvStudentAttend = new DataView();
                            DataView studattens = new DataView();
                          
                            int cunts = 0;
                            ds.Tables[3].DefaultView.RowFilter = "degree='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"]) + "' and deptcode='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "'";
                            studattens = ds.Tables[3].DefaultView;
                            if (studattens.Count > 0)
                            {
                                for (int m = 0; m < studattens.Count; m++)
                                {

                                    string fromdates = Convert.ToString(studattens[m]["interviewdate"]);
                                    string dates = string.Empty;
                                    dates = fromdates;
                                    string[] sp = dates.Split('/');
                                    string fromdat = sp[1] + '/' + sp[0] + '/' + sp[2];
                                    int getdate = 0;
                                    int.TryParse(sp[0], out getdate);
                                    if (getdate < 10)
                                    {
                                        String startOfString = sp[0].Remove(0, 1);
                                        sp[0] = startOfString;
                                    }
                                    string colmname = "D" + Convert.ToString(sp[0]);
                                    ds.Tables[1].DefaultView.RowFilter = "Course_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]) + "' and Dept_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]) + "' and Batch_year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' and " + colmname + "='2'  and interviewdate='" + fromdat + "'";
                                    dvStudentAttend = ds.Tables[1].DefaultView;
                                    cunts += dvStudentAttend.Count;
                                }
                            }
                            if (cunts > 0)
                            {
                                drrow["NotApplied"] = cunts;
                                notattedcun += cunts;
                            }
                            else
                                drrow["NotApplied"] = 0;

                            #endregion
                            #region selected
                            ds.Tables[2].DefaultView.RowFilter = "Course_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]) + "' and Dept_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]) + "' and Batch_year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' ";
                            DataView getround = new DataView();
                            getround = ds.Tables[2].DefaultView;
                            if (getround.Count > 0)
                            {
                                string round = Convert.ToString(getround[0]["rounds"]);

                                ds.Tables[2].DefaultView.RowFilter = "Course_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]) + "' and Dept_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]) + "' and Batch_year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' and R" + round + "='1'";
                                selected = ds.Tables[2].DefaultView;
                                if (selected.Count > 0)
                                {
                                    drrow["Selected"] = selected.Count;
                                    selectcun += selected.Count;
                                }
                                ds.Tables[2].DefaultView.RowFilter = "Course_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]) + "' and Dept_Name='" + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]) + "' and Batch_year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[i]["Sections"]) + "' and isnull( R" + round + ",'2')='2'";
                                notselected = ds.Tables[2].DefaultView;
                                if (notselected.Count > 0)
                                {
                                    drrow["Not Selected"] = notselected.Count;
                                    notselect += notselected.Count;
                                }
                            }
                            #endregion
                            dtTTDisp.Rows.Add(drrow);
                        }
                        drrow = dtTTDisp.NewRow();
                        drrow["Batch"] = "Total";
                        drrow["Shortlist"] = shorcun;
                        drrow["Applied"] = attedcun;
                        drrow["NotApplied"] = notattedcun;
                        drrow["Selected"] = selectcun;
                        drrow["Not Selected"] = notselect;
                        dtTTDisp.Rows.Add(drrow);

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
                                for (int j = 0; j < row.Cells.Count - 5; j++)
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

                                for (int m = gview.Rows.Count - 1; m >= 1; m--)
                                {

                                    GridViewRow rows = gview.Rows[m];
                                    GridViewRow previousRows = gview.Rows[m];
                                    GridViewRow previousRowss = gview.Rows[m];
                                    gview.Rows[m].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[9].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[m].Cells[10].HorizontalAlign = HorizontalAlign.Center;
                                    string cellte = gview.Rows[m].Cells[1].Text;

                                    if (!Convert.ToString(cellte).All(char.IsNumber))
                                    {
                                        gview.Rows[m].Cells[1].ColumnSpan = gview.Rows[m].Cells.Count - 6;
                                        gview.Rows[m].Cells[1].HorizontalAlign = HorizontalAlign.Right;
                                        gview.Rows[m].Cells[1].BackColor = Color.DarkSeaGreen;
                                        gview.Rows[m].Cells[9].BackColor = Color.DarkSeaGreen;
                                        gview.Rows[m].Cells[10].BackColor = Color.DarkSeaGreen;
                                        gview.Rows[m].Cells[6].BackColor = Color.DarkSeaGreen;
                                        gview.Rows[m].Cells[7].BackColor = Color.DarkSeaGreen;
                                        gview.Rows[m].Cells[8].BackColor = Color.DarkSeaGreen;
                                        gview.Rows[m].Cells[1].ColumnSpan = gview.Rows[m].Cells.Count - 6;
                                        for (int j = 2; j < gview.Rows[m].Cells.Count - 5; j++)
                                        {
                                            gview.Rows[m].Cells[0].Visible = false;
                                            gview.Rows[m].Cells[j].Visible = false;
                                        }
                                    }
                                }
                              
                            RowHead(gview);
                            #endregion span

                        }
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select All Feild";
                }
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
    protected void rdbcun_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbdetails.Checked != true)
            {
                cun.Visible = false;
                lblroun.Visible = false;
                lblroun1.Visible = false;
                Td1.Visible = false;
                Td2.Visible = false;
                
            }
            else
            {
                cun.Visible = true;
                
                if (rdbnotSelected.Checked == true)
                {
                    lblroun.Visible = true;
                    lblroun1.Visible = true;
                    Td1.Visible = false;
                    Td2.Visible = false;
                }
                else if (rdbShortlist.Checked == true)
                {
                    lblroun.Visible = false;
                    lblroun1.Visible = false;
                    Td1.Visible = true;
                    Td2.Visible = true;
                }
                else
                {
                    lblroun.Visible = false;
                    lblroun1.Visible = false;
                    Td1.Visible = false;
                    Td2.Visible = false;
                }

            }
        }
        catch
        {
        }
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
    protected void rdbnotSelected_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbnotSelected.Checked == true)
            {
                lblroun.Visible = true;
                lblroun1.Visible = true;
                Td1.Visible = false;
                Td2.Visible = false;
                interviewround();
            }
            else if (rdbShortlist.Checked == true)
            {
                Td1.Visible = true;
                Td2.Visible = true;
                lblroun.Visible = false;
                lblroun1.Visible = false;
            }
            else
            {
                lblroun.Visible = false;
                lblroun1.Visible = false;
                Td1.Visible = false;
                Td2.Visible = false;
            }
        }
        catch
        {
        }
    }
    //public void cb_round_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        gview.Visible = false;
    //        string buildvalue1 = string.Empty;
    //        string build1 = string.Empty;
    //        if (Cbround.Checked == true)
    //        {
    //            for (int i = 0; i < Cblround.Items.Count; i++)
    //            {
    //                if (Cbround.Checked == true)
    //                {
    //                    Cblround.Items[i].Selected = true;
    //                    txtround.Text = "Batch(" + (Cblround.Items.Count) + ")";
    //                    build1 = Cblround.Items[i].Value.ToString();
    //                    if (buildvalue1 == "")
    //                    {
    //                        buildvalue1 = build1;
    //                    }
    //                    else
    //                    {
    //                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < Cblround.Items.Count; i++)
    //            {
    //                Cblround.Items[i].Selected = false;
    //                txtround.Text = "--Select--";
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //public void cbl_round_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        gview.Visible = false;
    //        int seatcount = 0;
    //        Cbround.Checked = false;
    //        string buildvalue = string.Empty;
    //        string build = string.Empty;
    //        for (int i = 0; i < Cblround.Items.Count; i++)
    //        {
    //            if (Cblround.Items[i].Selected == true)
    //            {
    //                seatcount = seatcount + 1;
    //                txtround.Text = "--Select--";
    //                build = Cblround.Items[i].Value.ToString();
    //                if (buildvalue == "")
    //                {
    //                    buildvalue = build;
    //                }
    //                else
    //                {
    //                    buildvalue = buildvalue + "'" + "," + "'" + build;
    //                }
    //            }
    //        }
    //        if (seatcount == Cblround.Items.Count)
    //        {
    //            txtround.Text = "Batch(" + seatcount.ToString() + ")";
    //            Cbround.Checked = true;
    //        }
    //        else if (seatcount == 0)
    //        {
    //            txtround.Text = "--Select--";
    //            Cbround.Text = "--Select--";
    //        }
    //        else
    //        {
    //            txtround.Text = "Batch(" + seatcount.ToString() + ")";
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void interviewround()
    {
        try
        {
            gview.Visible = false;
            ds.Clear();
            ddlround.Items.Clear();
            string datequer = string.Empty;
            Hashtable hs = new Hashtable();
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

            string itemname = "select * from com_interviewmode where CompanyFK='" + drpcompany.SelectedValue + "' " + datequer + "";
            DataSet insround = new DataSet();
            insround = d2.select_method_wo_parameter(itemname, "text");
            int num=0;
            if (insround.Tables[0].Rows.Count > 0)
            {
                int cun = 0;
                for (int i = 0; i < insround.Tables[0].Rows.Count; i++)
                {
                    string rounds = Convert.ToString(insround.Tables[0].Rows[i]["mode"]);
                    if (rounds != "")
                    {
                        string[] spl = rounds.Split(',');
                        if (spl.Length > 0)
                        {
                            for (int m = 0; m < spl.Length; m++)
                            {
                                num++;
                                if (num == 1)
                                {
                                    hs.Add(num, spl[m]);
                                    if (!hs.ContainsValue(spl[m]))
                                        hs.Add(num, spl[m]);

                                    string posi = d2.GetFunction("  select MasterValue from CO_MasterValues where MasterCode ='" + spl[m] + "' and MasterCriteria ='Mode Of Interview' ");
                                    ddlround.Items.Insert(cun, posi);
                                    cun++;
                                }
                                else if (!hs.ContainsValue(spl[m]))
                                {
                                    
                                    
                                        hs.Add(num, spl[m]);

                                        string posi = d2.GetFunction("  select MasterValue from CO_MasterValues where MasterCode ='" + spl[m] + "' and MasterCriteria ='Mode Of Interview' ");
                                        ddlround.Items.Insert(cun, posi);
                                        cun++;
                                    
                                }
                            }
                        }
                    }
                }
                ddlround.Items.Insert(0, "select");
            }
            else
            {
                ddlround.Items.Insert(0, "select");
            }

        }
        catch
        {
        }
    }
    protected void chkinclu_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkinclu.Checked == true)
                ddlround.Enabled = true;
            else
                ddlround.Enabled = false;
        }
        catch
        {
        }
    }
    protected void Chkshort_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chkshort.Checked == true)
                txaccept.Enabled = true;
            else
                txaccept.Enabled = false;
        }
        catch
        {
        }
    }
}