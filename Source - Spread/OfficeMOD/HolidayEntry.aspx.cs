using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;


public partial class HolidayEntry : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    int i = 0;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string popclg1 = string.Empty;
    string popclg2 = string.Empty;
    string popclg3 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    Boolean Cellclick_applied = false;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
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
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            loadcollege();
            if (ddl_collegename.Items.Count > 0)
                collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            binddepts(collegecode1);//delsi1406
            binddeptss(collegecode1);
            loadstaftype();
            LoadLibrary();
            mess();
            Hostel();
            cbdegree.Checked = true;
            cbdegree_Changed(sender, e);
            rbstud.Checked = true;
            rbstud_Changed(sender, e);
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Attributes.Add("readonly", "readonly");
        }
        if (ddl_collegename.Items.Count > 0)
            collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
    }

    #region college

    public void loadcollege1()
    {
        try
        {
            //ds.Clear();
            //ds = d2.BindCollege();
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddl_collegename.DataSource = ds;
            //    ddl_collegename.DataTextField = "collname";
            //    ddl_collegename.DataValueField = "college_code";
            //    ddl_collegename.DataBind();

            //    ddl_popclg1.DataSource = ds;
            //    ddl_popclg1.DataTextField = "collname";
            //    ddl_popclg1.DataValueField = "college_code";
            //    ddl_popclg1.DataBind();

            //    ddl_popclg2.DataSource = ds;
            //    ddl_popclg2.DataTextField = "collname";
            //    ddl_popclg2.DataValueField = "college_code";
            //    ddl_popclg2.DataBind();

            //    ddl_popclg3.DataSource = ds;
            //    ddl_popclg3.DataTextField = "collname";
            //    ddl_popclg3.DataValueField = "college_code";
            //    ddl_popclg3.DataBind();
            //}
        }
        catch { }
    }

    public void loadcollege()//delsi1302
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                columnfield = " and group_code='" + group_code + "'";
            else
                columnfield = " and user_code='" + Session["usercode"] + "'";
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddl_collegename.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.Enabled = true;
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();

                ddl_popclg1.DataSource = ds;
                ddl_popclg1.DataTextField = "collname";
                ddl_popclg1.DataValueField = "college_code";
                ddl_popclg1.DataBind();

                ddl_popclg2.DataSource = ds;
                ddl_popclg2.DataTextField = "collname";
                ddl_popclg2.DataValueField = "college_code";
                ddl_popclg2.DataBind();

                ddl_popclg3.DataSource = ds;
                ddl_popclg3.DataTextField = "collname";
                ddl_popclg3.DataValueField = "college_code";
                ddl_popclg3.DataBind();

            }
        }
        catch (Exception e) { }
    }





    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_collegename.Items.Count > 0)
            collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
        bindBtch();
        binddeg();
        binddept();
        bindsem();
        loadstaftype();
        LoadLibrary();
        FpSpreadbase.Visible = false;
        print.Visible = false;
    }

    protected void ddl_popclg1_change(object sender, EventArgs e)
    {
        if (ddl_popclg1.Items.Count > 0)
            popclg1 = Convert.ToString(ddl_popclg1.SelectedValue);
        addnewbindBtch();
        addnewbinddeg();
        addnewbinddept();
        addnewbindsem();
    }

    protected void ddl_popclg2_change(object sender, EventArgs e)
    {
        if (ddl_popclg2.Items.Count > 0)
            popclg2 = Convert.ToString(ddl_popclg2.SelectedValue);
        binddepts(popclg2);
        loadstafftype();
    }

    protected void ddl_popclg3_change(object sender, EventArgs e)
    {
        if (ddl_popclg3.Items.Count > 0)
            popclg3 = Convert.ToString(ddl_popclg3.SelectedValue);
        LoadLibrary();
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
            int newcount = 0;
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
                        if (i == 0)
                        {
                            newcount++;
                            cbl_batch.Items[i].Selected = true;
                        }
                    }
                    txt_batch.Text = "Batch(" + newcount + ")";
                    cb_batch.Checked = false;
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
        bindsem();
    }

    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
        binddeg();
        binddept();
        bindsem();
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
            int newcount = 0;
            string collcode = Convert.ToString(ddl_collegename.SelectedValue);

            cbl_degree.Items.Clear();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collcode + "'";
            if (stream != "")
                selqry = selqry + " and type  in('" + stream + "')";
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
                        if (i == 0)
                        {
                            newcount++;
                            cbl_degree.Items[i].Selected = true;
                        }
                    }
                    txt_degree.Text = "Degree(" + newcount + ")";
                    cb_degree.Checked = false;
                }
            }
            binddept();
        }
        catch { }
    }

    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, "Degree", "--Select--");
        binddept();
        bindsem();
    }

    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, "Degree", "--Select--");
        binddept();
        bindsem();
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
            string degree = "";
            int i = 0;
            int newcount = 0;
            string collcode = Convert.ToString(ddl_collegename.SelectedValue);

            batch2 = getCblSelectedText(cbl_batch);

            degree = getCblSelectedValue(cbl_degree);

            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, "'" + degree + "'", collcode, usercode);
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
                            if (i == 0)
                            {
                                newcount++;
                                cbl_dept.Items[i].Selected = true;
                            }
                        }
                        txt_dept.Text = "Department(" + newcount + ")";
                        cb_dept.Checked = false;
                    }
                }
            }
            bindsem();
        }
        catch { }
    }

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        bindsem();
    }

    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        bindsem();
    }

    #endregion

    #region sem

    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
    }

    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
    }

    protected void bindsem()
    {
        try
        {
            int semcount = 0;
            string collcode = Convert.ToString(ddl_collegename.SelectedValue);

            string getcount = d2.GetFunction("select MAX(NDurations) as Sem from Ndegree where college_code='" + collcode + "'");
            if (getcount.Trim() != "" && getcount.Trim() != "0")
            {
                cbl_sem.Items.Clear();
                Int32.TryParse(getcount, out semcount);
                for (int ik = 1; ik <= semcount; ik++)
                {
                    cbl_sem.Items.Add(new ListItem(Convert.ToString(ik), Convert.ToString(ik)));
                }
                cbl_sem.DataBind();
                if (cbl_sem.Items.Count > 0)
                {
                    for (i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                    }
                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

    #region Staffcatagory

    protected void loadstaftype()
    {
        try
        {
            ds.Clear();
            int newcount = 0;
            cblstfcat.Items.Clear();
            string collcode = Convert.ToString(ddl_collegename.SelectedValue);
            // string item = "select distinct stftype,category_code from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and stftype is not null and stftype<>'' and college_code = '" + collcode + "' order by category_code";
            // string item = "select distinct category_name,category_code from staffcategorizer where  college_code='" + collcode + "'";
            string item = "select distinct category_name,c.category_code from staffcategorizer c,stafftrans st where st.category_code=c.category_code and college_code='" + collcode + "'";


            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstfcat.DataSource = ds;
                cblstfcat.DataTextField = "category_name";
                cblstfcat.DataValueField = "category_code";
                cblstfcat.DataBind();
                if (cblstfcat.Items.Count > 0)
                {
                    for (int i = 0; i < cblstfcat.Items.Count; i++)
                    {
                        if (i == 0)
                        {
                            newcount++;
                            cblstfcat.Items[i].Selected = true;
                        }
                    }
                    txtstfcat.Text = "StaffType (" + newcount + ")";
                    cbstfcat.Checked = false;
                }
            }
            else
            {
                txtstfcat.Text = "--Select--";
                cbstfcat.Checked = false;
            }
        }
        catch { }
    }

    protected void cbstfcat_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbstfcat, cblstfcat, txtstfcat, "Staff Type", "--Select--");
    }

    protected void cblstfcat_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbstfcat, cblstfcat, txtstfcat, "Staff Type", "--Select--");
    }

    #endregion

    #region Load Library

    protected void LoadLibrary()
    {
        try
        {
            ds.Clear();
            int newcount = 0;
            cbllbrary.Items.Clear();
            string collcode = Convert.ToString(ddl_collegename.SelectedValue);
            string item = "select lib_code,lib_name from library where college_code='" + collcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbllbrary.DataSource = ds;
                cbllbrary.DataTextField = "lib_name";
                cbllbrary.DataValueField = "lib_code";
                cbllbrary.DataBind();
                if (cbllbrary.Items.Count > 0)
                {
                    for (int i = 0; i < cbllbrary.Items.Count; i++)
                    {
                        if (i == 0)
                        {
                            newcount++;
                            cbllbrary.Items[i].Selected = true;
                        }
                    }
                    txtlbr.Text = "Library (" + newcount + ")";
                    cblbrary.Checked = false;
                }
            }
            else
            {
                txtlbr.Text = "--Select--";
                cblbrary.Checked = false;
            }
        }
        catch { }
    }

    protected void cblbrary_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cblbrary, cbllbrary, txtlbr, "Library", "--Select--");
    }

    protected void cbllbrary_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cblbrary, cbllbrary, txtlbr, "Library", "--Select--");
    }

    #endregion

    //Added by Saranyadevi 12.2.2018
    #region mess

    public void mess()
    {
        try
        {

            cbl_mess.Items.Clear();
            cb_mess.Checked = false;
            txt_mess.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            string Query = "select MessMasterPK,MessName,MessAcr from HM_MessMaster order by MessMasterPK asc";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_mess.DataSource = ds;
                cbl_mess.DataTextField = "MessName";
                cbl_mess.DataValueField = "MessMasterPK";
                cbl_mess.DataBind();
                if (cbl_mess.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_mess.Items.Count; i++)
                    {
                        cbl_mess.Items[i].Selected = true;
                    }
                    txt_mess.Text = "Mess(" + cbl_mess.Items.Count + ")";
                    cb_mess.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_mess_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxChange(cb_mess, cbl_mess, txt_mess, "Mess", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_mess_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxListChange(cb_mess, cbl_mess, txt_mess, "mess", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    #endregion


    #region Hostel


    public void Hostel()
    {
        try
        {

            cbl_hostel.Items.Clear();
            cb_hostel.Checked = false;
            txt_hostel.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            string Query = "select HostelMasterPK,HostelName  from HM_HostelMaster";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostel.DataSource = ds;
                cbl_hostel.DataTextField = "HostelName";
                cbl_hostel.DataValueField = "HostelMasterPK";
                cbl_hostel.DataBind();
                if (cbl_hostel.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostel.Items.Count; i++)
                    {
                        cbl_hostel.Items[i].Selected = true;
                    }
                    txt_hostel.Text = "Hostel(" + cbl_hostel.Items.Count + ")";
                    cb_hostel.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_hostel_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxChange(cb_hostel, cbl_hostel, txt_hostel, "Hostel", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_hostel_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxListChange(cb_hostel, cbl_hostel, txt_hostel, "Hostel", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }


    #endregion

    // end  by saranyadevi
    #region Checkbox event

    protected void cbdate_Changed(object sender, EventArgs e)
    {
        if (cbdate.Checked == true)
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

    protected void cbtype_Changed(object sender, EventArgs e)
    {
        if (cbtype.Checked == true)
        {
            rbhalf.Enabled = true;
            rbfull.Enabled = true;
            rbmnghalf.Enabled = false;
            rbevehalf.Enabled = false;
            rbhalf.Checked = false;
            rbfull.Checked = false;
            rbmnghalf.Checked = false;
            rbevehalf.Checked = false;
        }
        else
        {
            rbhalf.Enabled = false;
            rbfull.Enabled = false;
            rbmnghalf.Enabled = false;
            rbevehalf.Enabled = false;
            rbhalf.Checked = false;
            rbfull.Checked = false;
            rbmnghalf.Checked = false;
            rbevehalf.Checked = false;
        }
    }

    protected void cbdegree_Changed(object sender, EventArgs e)
    {
        if (cbdegree.Checked == true)
        {
            txt_batch.Enabled = true;
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
            txt_sem.Enabled = true;
        }
        else
        {
            txt_batch.Enabled = false;
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
            txt_sem.Enabled = false;
        }
    }

    protected void cbcatg_Changed(object sender, EventArgs e)
    {
        if (cbcatg.Checked == true)
            txtstfcat.Enabled = true;
        else
            txtstfcat.Enabled = false;
    }

    protected void cblbr_Changed(object sender, EventArgs e)
    {
        if (cblbr.Checked == true)
            txtlbr.Enabled = true;
        else
            txtlbr.Enabled = false;
    }

    protected void rbhalf_Changed(object sender, EventArgs e)
    {
        rbmnghalf.Enabled = true;
        rbevehalf.Enabled = true;
    }

    protected void rbfull_Changed(object sender, EventArgs e)
    {
        rbmnghalf.Enabled = false;
        rbevehalf.Enabled = false;
    }

    protected void rbstud_Changed(object sender, EventArgs e)
    {
        divstud.Visible = true;
        divstf.Visible = false;
        divlbr.Visible = false;
        divhos.Visible = false;
        divmess.Visible = false;
        cbdegree.Visible = true;
        cbcatg.Visible = false;
        cblbr.Visible = false;
        //
        cbdegree.Checked = true;
        cbdate.Checked = false;
        cbtype.Checked = false;

        cbdegree_Changed(sender, e);
        cbdate_Changed(sender, e);
        cbtype_Changed(sender, e);

        bindBtch();
        binddeg();
        binddept();

        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        divspread.Visible = false;
        print.Visible = false;
    }

    protected void rbstaff_Changed(object sender, EventArgs e)
    {
        divstud.Visible = false;
        divstf.Visible = true;
        divlbr.Visible = false;
        divhos.Visible = false;
        divmess.Visible = false;
        cbdegree.Visible = false;
        cbcatg.Visible = true;
        cblbr.Visible = false;
        //
        cbcatg.Checked = true;
        cbdate.Checked = false;
        cbtype.Checked = false;

        cbcatg_Changed(sender, e);
        cbdate_Changed(sender, e);
        cbtype_Changed(sender, e);
        loadstaftype();

        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        divspread.Visible = false;
        print.Visible = false;
    }

    protected void rblbr_Changed(object sender, EventArgs e)
    {
        divstud.Visible = false;
        divstf.Visible = false;
        divlbr.Visible = true;
        divhos.Visible = false;
        divmess.Visible = false;
        cbdegree.Visible = false;
        cbcatg.Visible = false;
        cblbr.Visible = true;
        //
        cblbr.Checked = true;
        cbdate.Checked = false;
        cbtype.Checked = false;

        cblbr_Changed(sender, e);
        cbdate_Changed(sender, e);
        cbtype_Changed(sender, e);
        LoadLibrary();

        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        divspread.Visible = false;
        print.Visible = false;
    }


    //Aded by saranyadevi 13.2.2018
    protected void rblhos_Changed(object sender, EventArgs e)
    {
        divstud.Visible = false;
        divstf.Visible = false;
        divlbr.Visible = false;
        divhos.Visible = true;
        divmess.Visible = false;
        cbdegree.Visible = false;
        cbcatg.Visible = false;
        cblbr.Visible = false;
        //
        cblbr.Checked = false;
        cbdate.Checked = false;
        cbtype.Checked = false;

        cblbr_Changed(sender, e);
        cbdate_Changed(sender, e);
        cbtype_Changed(sender, e);

        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        divspread.Visible = false;
        print.Visible = false;

    }

    protected void rblmess_Changed(object sender, EventArgs e)
    {
        divstud.Visible = false;
        divstf.Visible = false;
        divlbr.Visible = false;
        divhos.Visible = false;
        divmess.Visible = true;
        cbdegree.Visible = false;
        cbcatg.Visible = false;
        cblbr.Visible = false;
        //
        cblbr.Checked = false;
        cbdate.Checked = false;
        cbtype.Checked = false;

        cblbr_Changed(sender, e);
        cbdate_Changed(sender, e);
        cbtype_Changed(sender, e);

        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        divspread.Visible = false;
        print.Visible = false;
    }
    #endregion

    #region go and addnew

    protected DataSet LoadDatasetValues()
    {
        DataSet dsload = new DataSet();
        try
        {
            int cbvalue = 0;
            int holidaytype = 0;
            string fromdate = "";
            string todate = "";
            string type = "";
            string degree = "";
            string feecat = "";
            string stftype = "";

            string libry = "";
            string SelectQ = "";
            int cbdateval = 0;
            int cbtypeval = 0;
            int cbdegreval = 0;
            string deptCode = string.Empty;
            string halfmng = "";
            string halfeve = "";
            int halfval = 0;
            string Messcode = string.Empty;
            string Hostelcode = string.Empty;
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
            }
            if (rbstud.Checked == true)
            {
                cbvalue = 1;
                if (cbdegree.Checked == true)
                {
                    cbdegreval = 1;
                    degree = Convert.ToString(getCblSelectedValue(cbl_dept));
                    feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
                }
            }
            else if (rbstaff.Checked == true)
            {
                cbvalue = 2;
                if (cbcatg.Checked == true)
                {
                    cbdegreval = 1;
                    stftype = Convert.ToString(getCblSelectedValue(cblstfcat));
                    deptCode = Convert.ToString(getCblSelectedValue(cbldepartment));
                }
            }
            else if (rblbr.Checked == true)
            {
                cbvalue = 3;
                if (cblbr.Checked == true)
                {
                    cbdegreval = 1;
                    libry = Convert.ToString(getCblSelectedValue(cbllbrary));
                }
            }
            else if (rblhos.Checked == true)//added by saranyadevi 13.2.2018
            {
                cbvalue = 4;
                holidaytype = 0;
                if (cbl_hostel.Items.Count > 0)
                    Hostelcode = Convert.ToString(d2.getCblSelectedValue(cbl_hostel));

            }
            else if (rblmess.Checked == true)//added by saranyadevi 13.2.2018
            {
                cbvalue = 5;
                holidaytype = 1;
                if (cbl_mess.Items.Count > 0)
                    Messcode = Convert.ToString(d2.getCblSelectedValue(cbl_mess));

            }
            if (cbdate.Checked == true)
            {
                cbdateval = 1;
                fromdate = Convert.ToString(txt_fromdate.Text);
                todate = Convert.ToString(txt_todate.Text);
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                {
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                }
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                {
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                }
            }
            if (cbtype.Checked == true)
            {
                cbtypeval = 1;
                if (rbhalf.Checked == true)
                {
                    type = "1";
                    if (rbmnghalf.Checked == true)
                    {
                        halfval = 1;
                        halfmng = "1";
                        halfeve = "0";
                    }
                    else
                    {
                        halfval = 1;
                        halfeve = "1";
                        halfmng = "0";
                    }
                }
                else if (rbfull.Checked == true)
                {
                    halfval = 0;
                    type = "0";
                    halfmng = "0";
                    halfeve = "0";
                }
            }

            if (cbvalue == 1)
            {
                #region stud
                SelectQ = "select degree_code,CONVERT(varchar(10),holiday_date,103) as holidaydate,holiday_desc,semester,halforfull,morning,evening from holidayStudents where ";
                // holyid
                if (degree != "")
                    SelectQ = SelectQ + "degree_code in('" + degree + "')";
                if (feecat != "")
                    SelectQ = SelectQ + " and semester in('" + feecat + "')";
                if (type != "")
                {
                    if (cbdegreval == 1)
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + " and  halforfull in('" + type + "') and morning in('" + halfmng + "') and evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " and  halforfull in('" + type + "')";
                    }
                    else
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + "   halforfull in('" + type + "') and morning in('" + halfmng + "') and evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " halforfull in('" + type + "')";
                    }
                }
                if (fromdate != "" && todate != "")
                {
                    if (cbtypeval == 1)
                        SelectQ = SelectQ + " and holiday_date between '" + fromdate + "' and '" + todate + "'";
                    else
                        SelectQ = SelectQ + " holiday_date between '" + fromdate + "' and '" + todate + "'";
                }
                SelectQ += " order by holiday_date desc";
                SelectQ = SelectQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "' order by Degree_Code";
                #endregion
            }
            else if (cbvalue == 2)
            {
                #region staff
                SelectQ = "select category_code,dept_code,CONVERT(varchar(10),holiday_date,103) as holidaydate,holiday_desc,halforfull,morning,evening from holidayStaff where ";//added deptcode 1406
                if (stftype != "" && deptCode != "")
                    SelectQ = SelectQ + "category_code in('" + stftype + "') and dept_code in('" + deptCode + "') and college_Code='" + collegecode1 + "'";//delsi added Collegecode
                if (type != "")
                {
                    if (cbdegreval == 1)
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + " and  halforfull in('" + type + "') and morning in('" + halfmng + "') and evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " and  halforfull in('" + type + "')";
                    }
                    else
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + " halforfull in('" + type + "') and morning in('" + halfmng + "') and evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " halforfull in('" + type + "')";
                    }
                }
                if (fromdate != "" && todate != "")
                {
                    if (cbtypeval == 1 && (stftype.Trim() != "" || type.Trim() != ""))
                        SelectQ = SelectQ + " and holiday_date between '" + fromdate + "' and '" + todate + "'";
                    else
                        SelectQ = SelectQ + " and holiday_date between '" + fromdate + "' and '" + todate + "'";
                }
                SelectQ += " order by holiday_date,dept_code desc";
                //     SelectQ = SelectQ + " select stftype,category_code from  stafftrans t ,staffmaster m where m.staff_code = t.staff_code and stftype is not null and stftype<>'' and college_code = '" + collegecode1 + "' order by category_code";
                //"category_code in() and holiday_date between '' and '' and halforfull='' ";
                //SelectQ = SelectQ + " select c.category_name,c.category_code from  stafftrans t ,staffmaster m,staffcategorizer c where m.staff_code = t.staff_code and t.category_code=c.category_code and m.college_code=c.college_code and m.college_code = '" + collegecode1 + "'  order by c.category_code";


                SelectQ = SelectQ + " select c.category_name,c.category_code,hdm.dept_code,hdm.dept_name from  stafftrans t ,staffmaster m,staffcategorizer c,hrdept_master hdm where m.staff_code = t.staff_code and t.category_code=c.category_code and m.college_code=c.college_code and t.dept_code=hdm.dept_code and m.college_code=hdm.college_code and m.college_code = '" + collegecode1 + "' and hdm.dept_code in('" + deptCode + "')  order by hdm.dept_name,c.category_code";

                #endregion
            }
            else if (cbvalue == 3)
            {
                #region library

                SelectQ = "select Lib_Code, CONVERT(varchar(10),Holiday_Date,103) as holidaydate ,HalfOrFull,Morning,Evening,Holiday_Desc,College_Code from holiday_Library where  ";
                if (libry != "")
                    SelectQ = SelectQ + "Lib_Code in('" + libry + "')";
                if (type != "")
                {
                    if (cbdegreval == 1)
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + " and  HalfOrFull in('" + type + "') and Morning in('" + halfmng + "') and Evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " and  HalfOrFull in('" + type + "')";
                    }
                    else
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + "   HalfOrFull in('" + type + "') and Morning in('" + halfmng + "') and Evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " HalfOrFull in('" + type + "')";
                    }
                }
                if (fromdate != "" && todate != "")
                {
                    if (cbtypeval == 1 && (libry.Trim() != "" || type.Trim() != ""))
                        SelectQ = SelectQ + " and Holiday_Date between '" + fromdate + "' and '" + todate + "'";
                    else
                        SelectQ = SelectQ + " Holiday_Date between '" + fromdate + "' and '" + todate + "'";
                }
                SelectQ += " order by holiday_date desc";
                SelectQ = SelectQ + " select * from library where college_code='" + collegecode1 + "' order by Lib_Code";
                #endregion
            }
            else if (cbvalue == 4)
            {
                #region Hostel
                SelectQ = "select MessCode, CONVERT(varchar(10),HolidayDate,103) as holidaydate ,IsHalfDay,Morning,Evening,HolidayDescription from HT_Holidays where HolidayType='" + holidaytype + "'   ";
                if (Hostelcode != "")
                    SelectQ = SelectQ + "and MessCode in('" + Hostelcode + "')";
                if (type != "")
                {
                    if (cbdegreval == 1)
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + " and  IsHalfDay in('" + type + "') and Morning in('" + halfmng + "') and Evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " and  IsHalfDay in('" + type + "')";
                    }
                    else
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + " and  IsHalfDay in('" + type + "') and Morning in('" + halfmng + "') and Evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " and IsHalfDay in('" + type + "')";
                    }
                }
                if (fromdate != "" && todate != "")
                {
                    if (cbtypeval == 1 && (libry.Trim() != "" || type.Trim() != ""))
                        SelectQ = SelectQ + " and HolidayDate between '" + fromdate + "' and '" + todate + "'";
                    else
                        SelectQ = SelectQ + " HolidayDate between '" + fromdate + "' and '" + todate + "'";
                }
                SelectQ += " order by holidaydate desc";
                SelectQ = SelectQ + " select * from HM_HostelMaster";
                #endregion
            }
            else if (cbvalue == 5)
            {
                #region Mess
                SelectQ = "select MessCode, CONVERT(varchar(10),HolidayDate,103) as holidaydate ,IsHalfDay,Morning,Evening,HolidayDescription from HT_Holidays where HolidayType='" + holidaytype + "'   ";
                if (Messcode != "")
                    SelectQ = SelectQ + "and MessCode in('" + Messcode + "')";
                if (type != "")
                {
                    if (cbdegreval == 1)
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + " and  IsHalfDay in('" + type + "') and Morning in('" + halfmng + "') and Evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " and  IsHalfDay in('" + type + "')";
                    }
                    else
                    {
                        if (halfval == 1)
                            SelectQ = SelectQ + " and IsHalfDay in('" + type + "') and Morning in('" + halfmng + "') and Evening in('" + halfeve + "')";
                        else
                            SelectQ = SelectQ + " and  IsHalfDay in('" + type + "')";
                    }
                }
                if (fromdate != "" && todate != "")
                {
                    if (cbtypeval == 1 && (libry.Trim() != "" || type.Trim() != ""))
                        SelectQ = SelectQ + " and HolidayDate between '" + fromdate + "' and '" + todate + "'";
                    else
                        SelectQ = SelectQ + " HolidayDate between '" + fromdate + "' and '" + todate + "'";
                }
                SelectQ += " order by holidaydate desc";
                SelectQ = SelectQ + " select * from HM_MessMaster";
                #endregion
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = LoadDatasetValues();
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (rbstud.Checked == true)
                    LoadStudValues();
                else if (rbstaff.Checked == true)
                    LoadStaffValues();
                else if (rblbr.Checked == true)
                    LoadLibraryValues();
                else if (rblhos.Checked == true)
                    LoadHostelValues();
                else if (rblmess.Checked == true)
                    LoadMessValues();
            }
            else
            {
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                divspread.Visible = false;
                FpSpreadbase.Visible = false;
                print.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }

    protected bool checkedOK()
    {
        bool OK = false;
        FpSpreadbase.SaveChanges();
        try
        {
            for (int ik = 0; ik < FpSpreadbase.Sheets[0].Rows.Count; ik++)
            {
                byte check = Convert.ToByte(FpSpreadbase.Sheets[0].Cells[ik, 1].Value);
                if (check == 1)
                {
                    OK = true;
                }
            }
        }
        catch { }
        return OK;
    }

    protected void Fpspreadbase_UpdateCommand(Object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpSpreadbase.SaveChanges();
            byte check = Convert.ToByte(FpSpreadbase.Sheets[0].Cells[0, 1].Value);
            if (check == 1)
            {
                for (int ik = 0; ik < FpSpreadbase.Sheets[0].Rows.Count; ik++)
                {
                    FpSpreadbase.Sheets[0].Cells[ik, 1].Value = 1;
                }
            }
            else
            {
                for (int ik = 0; ik < FpSpreadbase.Sheets[0].Rows.Count; ik++)
                {
                    FpSpreadbase.Sheets[0].Cells[ik, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    protected void LoadStudValues()
    {
        try
        {
            #region design

            FpSpreadbase.Sheets[0].RowCount = 0;
            FpSpreadbase.Sheets[0].ColumnCount = 0;
            FpSpreadbase.CommandBar.Visible = false;
            FpSpreadbase.Sheets[0].AutoPostBack = false;
            FpSpreadbase.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpreadbase.Sheets[0].RowHeader.Visible = false;
            FpSpreadbase.Sheets[0].ColumnCount = 9;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.CheckBoxCellType cbsel = new FarPoint.Web.Spread.CheckBoxCellType();
            cbsel.AutoPostBack = false;
            FarPoint.Web.Spread.CheckBoxCellType cbselall = new FarPoint.Web.Spread.CheckBoxCellType();
            cbselall.AutoPostBack = true;
            FpSpreadbase.Sheets[0].FrozenRowCount = 1;

            //  FarPoint.Web.Spread.DoubleCellType chaltxt = new FarPoint.Web.Spread.DoubleCellType();
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Course";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[2].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Semester";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[3].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Date";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Type";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Morning";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Evening";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[7].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Description";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[8].Locked = true;

            if (rbhalf.Checked == true)
            {
                if (rbmnghalf.Checked == true)
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = true;
                    FpSpreadbase.Sheets[0].Columns[7].Visible = false;
                }
                else
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = false;
                    FpSpreadbase.Sheets[0].Columns[7].Visible = true;
                }
            }
            else
            {
                FpSpreadbase.Width = 863;
                FpSpreadbase.Sheets[0].Columns[6].Visible = false;
                FpSpreadbase.Sheets[0].Columns[7].Visible = false;
            }

            FpSpreadbase.Sheets[0].Columns[0].Width = 50;
            FpSpreadbase.Sheets[0].Columns[1].Width = 75;
            FpSpreadbase.Sheets[0].Columns[2].Width = 280;
            FpSpreadbase.Sheets[0].Columns[3].Width = 95;
            FpSpreadbase.Sheets[0].Columns[4].Width = 100;
            FpSpreadbase.Sheets[0].Columns[5].Width = 75;
            FpSpreadbase.Sheets[0].Columns[6].Width = 75;
            FpSpreadbase.Sheets[0].Columns[7].Width = 75;
            FpSpreadbase.Sheets[0].Columns[8].Width = 180;

            #endregion

            #region values
            DataView Dview = new DataView();
            string Degreename = "";

            FpSpreadbase.Sheets[0].RowCount++;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbselall;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;

            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                FpSpreadbase.Sheets[0].RowCount++;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbsel;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(ds.Tables[0].Rows[sel]["Degree_code"]) + "'";
                    Dview = ds.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                    {
                        Degreename = Convert.ToString(Dview[0]["degreename"]);
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Text = Degreename;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["Degree_code"]);
                    }
                }

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["semester"]);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sel]["holidaydate"]);
                string type = Convert.ToString(ds.Tables[0].Rows[sel]["halforfull"]);
                if (type.ToUpper() == "FALSE")
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Full";
                else
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Half";

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                string morng = Convert.ToString(ds.Tables[0].Rows[sel]["morning"]);
                string eveng = Convert.ToString(ds.Tables[0].Rows[sel]["evening"]);
                if (type.ToUpper() == "TRUE")
                {
                    if (morng.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Evening";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Morning";

                    if (eveng.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = "Morning";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = "Evening";
                }
                else
                {
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = "";
                }

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[sel]["holiday_desc"]);

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
            }

            FpSpreadbase.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);

            #endregion

            #region visible

            FpSpreadbase.Sheets[0].PageSize = FpSpreadbase.Sheets[0].RowCount;
            FpSpreadbase.SaveChanges();
            divspread.Visible = true;
            FpSpreadbase.Visible = true;
            FpSpreadbase.Height = 380;
            FpSpreadbase.ShowHeaderSelection = false;
            print.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";

            #endregion
        }
        catch { }
    }

    protected void LoadStaffValues()
    {
        try
        {
            #region design

            FpSpreadbase.Sheets[0].RowCount = 0;
            FpSpreadbase.Sheets[0].ColumnCount = 0;
            FpSpreadbase.CommandBar.Visible = false;
            FpSpreadbase.Sheets[0].AutoPostBack = false;
            FpSpreadbase.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpreadbase.Sheets[0].RowHeader.Visible = false;
            // FpSpreadbase.Sheets[0].ColumnCount = 8;
            FpSpreadbase.Sheets[0].ColumnCount = 9;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            //  FarPoint.Web.Spread.DoubleCellType chaltxt = new FarPoint.Web.Spread.DoubleCellType();

            FarPoint.Web.Spread.CheckBoxCellType cbsel = new FarPoint.Web.Spread.CheckBoxCellType();
            cbsel.AutoPostBack = false;
            FarPoint.Web.Spread.CheckBoxCellType cbselall = new FarPoint.Web.Spread.CheckBoxCellType();
            cbselall.AutoPostBack = true;
            FpSpreadbase.Sheets[0].FrozenRowCount = 1;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";//added delsi1406
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[2].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Catagory";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[3].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Date";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            FpSpreadbase.Sheets[0].Columns[4].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Type";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Morning";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Evening";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[7].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Description";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[8].Locked = true;


            if (rbhalf.Checked == true)
            {
                if (rbmnghalf.Checked == true)
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = true;
                    FpSpreadbase.Sheets[0].Columns[7].Visible = false;
                }
                else
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = false;
                    FpSpreadbase.Sheets[0].Columns[7].Visible = true;
                }
            }
            else
            {
                FpSpreadbase.Width = 863;
                FpSpreadbase.Sheets[0].Columns[6].Visible = false;
                FpSpreadbase.Sheets[0].Columns[7].Visible = false;
            }
            FpSpreadbase.Sheets[0].Columns[0].Width = 50;
            FpSpreadbase.Sheets[0].Columns[1].Width = 75;
            FpSpreadbase.Sheets[0].Columns[2].Width = 250;
            FpSpreadbase.Sheets[0].Columns[3].Width = 280;
            FpSpreadbase.Sheets[0].Columns[4].Width = 100;
            FpSpreadbase.Sheets[0].Columns[5].Width = 70;
            FpSpreadbase.Sheets[0].Columns[6].Width = 70;
            FpSpreadbase.Sheets[0].Columns[7].Width = 70;
            FpSpreadbase.Sheets[0].Columns[8].Width = 180;
            #endregion

            #region values
            DataView Dview = new DataView();
            string name = "";
            string departmentName = string.Empty;
            int sno = 0;

            FpSpreadbase.Sheets[0].RowCount++;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbselall;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;

            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                string deptCode = Convert.ToString(ds.Tables[0].Rows[sel]["dept_code"]);
                string catcode = Convert.ToString(ds.Tables[0].Rows[sel]["category_code"]);

                string checkdept_code = d2.GetFunction("select distinct dept_code from stafftrans st,staffmaster sm where sm.staff_code=st.staff_code and sm.college_code='" + ddl_collegename.SelectedItem.Value + "' and dept_code='" + deptCode + "'");
                string checkcategory_code = d2.GetFunction("select distinct category_code  from stafftrans where dept_code='" + deptCode + "' and category_code='" + catcode + "'");


                if (checkdept_code.Trim().ToString() != "0" && checkcategory_code != "0")
                {
                    sno++;
                    FpSpreadbase.Sheets[0].RowCount++;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbsel;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "category_code='" + Convert.ToString(ds.Tables[0].Rows[sel]["category_code"]) + "'  and dept_code='" + Convert.ToString(ds.Tables[0].Rows[sel]["dept_code"]) + "'";
                        Dview = ds.Tables[1].DefaultView;
                        if (Dview.Count > 0)
                        {
                            departmentName = Convert.ToString(Dview[0]["dept_Name"]);
                            name = Convert.ToString(Dview[0]["category_name"]);
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(departmentName);
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["dept_code"]);


                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Text = name;
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["category_code"]);
                        }
                    }
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sel]["holidaydate"]);
                    string type = Convert.ToString(ds.Tables[0].Rows[sel]["halforfull"]);
                    if (type.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Full";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Half";

                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                    string morng = Convert.ToString(ds.Tables[0].Rows[sel]["morning"]);
                    string eveng = Convert.ToString(ds.Tables[0].Rows[sel]["evening"]);
                    if (type.ToUpper() == "TRUE")
                    {
                        if (morng.ToUpper() == "FALSE")
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Evening";
                        else
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Morning";

                        if (eveng.ToUpper() == "FALSE")
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = "Morning";
                        else
                            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = "Evening";
                    }
                    else
                    {
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "";
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = "";
                    }

                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[sel]["holiday_desc"]);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                }
            }

            FpSpreadbase.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
            #endregion

            #region visible

            FpSpreadbase.Sheets[0].PageSize = FpSpreadbase.Sheets[0].RowCount;
            FpSpreadbase.SaveChanges();
            divspread.Visible = true;
            FpSpreadbase.Visible = true;
            FpSpreadbase.Height = 380;
            FpSpreadbase.ShowHeaderSelection = false;
            print.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";

            #endregion
        }
        catch { }
    }

    protected void LoadLibraryValues()
    {
        try
        {
            #region design

            FpSpreadbase.Sheets[0].RowCount = 0;
            FpSpreadbase.Sheets[0].ColumnCount = 0;
            FpSpreadbase.CommandBar.Visible = false;
            FpSpreadbase.Sheets[0].AutoPostBack = true;
            FpSpreadbase.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpreadbase.Sheets[0].RowHeader.Visible = false;
            FpSpreadbase.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            //  FarPoint.Web.Spread.DoubleCellType chaltxt = new FarPoint.Web.Spread.DoubleCellType();

            FarPoint.Web.Spread.CheckBoxCellType cbsel = new FarPoint.Web.Spread.CheckBoxCellType();
            cbsel.AutoPostBack = false;
            FarPoint.Web.Spread.CheckBoxCellType cbselall = new FarPoint.Web.Spread.CheckBoxCellType();
            cbselall.AutoPostBack = true;
            FpSpreadbase.Sheets[0].FrozenRowCount = 1;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Library";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[2].Locked = true;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpreadbase.Sheets[0].Columns[3].Locked = true;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Category";//"Type";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Morning";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Evening";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Text = " Description";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[7].Locked = true;

            if (rbhalf.Checked == true)
            {
                if (rbmnghalf.Checked == true)
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[5].Visible = true;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = false;
                }
                else
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[5].Visible = false;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = true;
                }
            }
            else
            {
                FpSpreadbase.Width = 863;
                FpSpreadbase.Sheets[0].Columns[5].Visible = false;
                FpSpreadbase.Sheets[0].Columns[6].Visible = false;
            }
            FpSpreadbase.Sheets[0].Columns[0].Width = 50;
            FpSpreadbase.Sheets[0].Columns[1].Width = 75;
            FpSpreadbase.Sheets[0].Columns[2].Width = 280;
            FpSpreadbase.Sheets[0].Columns[3].Width = 100;
            FpSpreadbase.Sheets[0].Columns[4].Width = 95;
            FpSpreadbase.Sheets[0].Columns[5].Width = 70;
            FpSpreadbase.Sheets[0].Columns[6].Width = 70;
            FpSpreadbase.Sheets[0].Columns[7].Width = 250;
            #endregion

            #region values
            DataView Dview = new DataView();
            string name = "";

            FpSpreadbase.Sheets[0].RowCount++;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbselall;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;

            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                FpSpreadbase.Sheets[0].RowCount++;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbsel;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "Lib_Code='" + Convert.ToString(ds.Tables[0].Rows[sel]["Lib_Code"]) + "'";
                    Dview = ds.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                    {
                        name = Convert.ToString(Dview[0]["lib_name"]);
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Text = name;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["Lib_Code"]);
                    }
                }

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["holidaydate"]);
                string type = Convert.ToString(ds.Tables[0].Rows[sel]["HalfOrFull"]);
                if (type.ToUpper() == "FALSE")
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = "Full";
                else
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = "Half";

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                string morng = Convert.ToString(ds.Tables[0].Rows[sel]["Morning"]);
                string eveng = Convert.ToString(ds.Tables[0].Rows[sel]["Evening"]);
                if (type.ToUpper() == "TRUE")
                {
                    if (morng.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Evening";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Morning";

                    if (eveng.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Morning";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Evening";
                }
                else
                {
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "";
                }

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Holiday_Desc"]);

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

            }

            FpSpreadbase.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);

            #endregion

            #region visible

            FpSpreadbase.Sheets[0].PageSize = FpSpreadbase.Sheets[0].RowCount;
            FpSpreadbase.SaveChanges();
            divspread.Visible = true;
            FpSpreadbase.Visible = true;
            FpSpreadbase.Height = 380;
            FpSpreadbase.ShowHeaderSelection = false;
            print.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";

            #endregion
        }
        catch { }
    }

    #region Hostel

    protected void LoadHostelValues()
    {
        try
        {
            #region design

            FpSpreadbase.Sheets[0].RowCount = 0;
            FpSpreadbase.Sheets[0].ColumnCount = 0;
            FpSpreadbase.CommandBar.Visible = false;
            FpSpreadbase.Sheets[0].AutoPostBack = false;
            FpSpreadbase.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpreadbase.Sheets[0].RowHeader.Visible = false;
            FpSpreadbase.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            //  FarPoint.Web.Spread.DoubleCellType chaltxt = new FarPoint.Web.Spread.DoubleCellType();

            FarPoint.Web.Spread.CheckBoxCellType cbsel = new FarPoint.Web.Spread.CheckBoxCellType();
            cbsel.AutoPostBack = false;
            FarPoint.Web.Spread.CheckBoxCellType cbselall = new FarPoint.Web.Spread.CheckBoxCellType();
            cbselall.AutoPostBack = true;
            FpSpreadbase.Sheets[0].FrozenRowCount = 1;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hostel";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[2].Locked = true;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpreadbase.Sheets[0].Columns[3].Locked = true;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Type";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Morning";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Evening";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Text = " Description";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[7].Locked = true;

            if (rbhalf.Checked == true)
            {
                if (rbmnghalf.Checked == true)
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[5].Visible = true;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = false;
                }
                else
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[5].Visible = false;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = true;
                }
            }
            else
            {
                FpSpreadbase.Width = 863;
                FpSpreadbase.Sheets[0].Columns[5].Visible = false;
                FpSpreadbase.Sheets[0].Columns[6].Visible = false;
            }
            FpSpreadbase.Sheets[0].Columns[0].Width = 50;
            FpSpreadbase.Sheets[0].Columns[1].Width = 75;
            FpSpreadbase.Sheets[0].Columns[2].Width = 280;
            FpSpreadbase.Sheets[0].Columns[3].Width = 100;
            FpSpreadbase.Sheets[0].Columns[4].Width = 95;
            FpSpreadbase.Sheets[0].Columns[5].Width = 70;
            FpSpreadbase.Sheets[0].Columns[6].Width = 70;
            FpSpreadbase.Sheets[0].Columns[7].Width = 250;
            #endregion

            #region values
            DataView Dview = new DataView();
            string name = "";

            FpSpreadbase.Sheets[0].RowCount++;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbselall;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;

            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                FpSpreadbase.Sheets[0].RowCount++;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbsel;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "HostelMasterPK='" + Convert.ToString(ds.Tables[0].Rows[sel]["MessCode"]) + "'";
                    Dview = ds.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                    {
                        name = Convert.ToString(Dview[0]["HostelName"]);
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Text = name;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["MessCode"]);
                    }
                }

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["holidaydate"]);
                string type = Convert.ToString(ds.Tables[0].Rows[sel]["IsHalfDay"]);
                if (type.ToUpper() == "FALSE")
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = "Full";
                else
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = "Half";

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                string morng = Convert.ToString(ds.Tables[0].Rows[sel]["Morning"]);
                string eveng = Convert.ToString(ds.Tables[0].Rows[sel]["Evening"]);
                if (type.ToUpper() == "TRUE")
                {
                    if (morng.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Evening";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Morning";

                    if (eveng.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Morning";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Evening";
                }
                else
                {
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "";
                }

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[sel]["HolidayDescription"]);

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

            }

            FpSpreadbase.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);

            #endregion

            #region visible

            FpSpreadbase.Sheets[0].PageSize = FpSpreadbase.Sheets[0].RowCount;
            FpSpreadbase.SaveChanges();
            divspread.Visible = true;
            FpSpreadbase.Visible = true;
            FpSpreadbase.Height = 380;
            FpSpreadbase.ShowHeaderSelection = false;
            print.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";

            #endregion
        }
        catch
        {

        }


    }
    #endregion

    #region Mess
    protected void LoadMessValues()
    {
        try
        {
            #region design

            FpSpreadbase.Sheets[0].RowCount = 0;
            FpSpreadbase.Sheets[0].ColumnCount = 0;
            FpSpreadbase.CommandBar.Visible = false;
            FpSpreadbase.Sheets[0].AutoPostBack = false;
            FpSpreadbase.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpreadbase.Sheets[0].RowHeader.Visible = false;
            FpSpreadbase.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            //  FarPoint.Web.Spread.DoubleCellType chaltxt = new FarPoint.Web.Spread.DoubleCellType();

            FarPoint.Web.Spread.CheckBoxCellType cbsel = new FarPoint.Web.Spread.CheckBoxCellType();
            cbsel.AutoPostBack = false;
            FarPoint.Web.Spread.CheckBoxCellType cbselall = new FarPoint.Web.Spread.CheckBoxCellType();
            cbselall.AutoPostBack = true;
            FpSpreadbase.Sheets[0].FrozenRowCount = 1;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Mess";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[2].Locked = true;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpreadbase.Sheets[0].Columns[3].Locked = true;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Type";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Morning";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Evening";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].Locked = true;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Text = " Description";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            FpSpreadbase.Sheets[0].Columns[7].Locked = true;

            if (rbhalf.Checked == true)
            {
                if (rbmnghalf.Checked == true)
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[5].Visible = true;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = false;
                }
                else
                {
                    FpSpreadbase.Width = 930;
                    FpSpreadbase.Sheets[0].Columns[5].Visible = false;
                    FpSpreadbase.Sheets[0].Columns[6].Visible = true;
                }
            }
            else
            {
                FpSpreadbase.Width = 863;
                FpSpreadbase.Sheets[0].Columns[5].Visible = false;
                FpSpreadbase.Sheets[0].Columns[6].Visible = false;
            }
            FpSpreadbase.Sheets[0].Columns[0].Width = 50;
            FpSpreadbase.Sheets[0].Columns[1].Width = 75;
            FpSpreadbase.Sheets[0].Columns[2].Width = 280;
            FpSpreadbase.Sheets[0].Columns[3].Width = 100;
            FpSpreadbase.Sheets[0].Columns[4].Width = 95;
            FpSpreadbase.Sheets[0].Columns[5].Width = 70;
            FpSpreadbase.Sheets[0].Columns[6].Width = 70;
            FpSpreadbase.Sheets[0].Columns[7].Width = 250;
            #endregion

            #region values
            DataView Dview = new DataView();
            string name = "";

            FpSpreadbase.Sheets[0].RowCount++;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbselall;
            FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;

            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                FpSpreadbase.Sheets[0].RowCount++;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].CellType = cbsel;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Value = 0;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "MessMasterPK='" + Convert.ToString(ds.Tables[0].Rows[sel]["MessCode"]) + "'";
                    Dview = ds.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                    {
                        name = Convert.ToString(Dview[0]["MessName"]);
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Text = name;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["MessCode"]);
                    }
                }

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["holidaydate"]);
                string type = Convert.ToString(ds.Tables[0].Rows[sel]["IsHalfDay"]);
                if (type.ToUpper() == "FALSE")
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = "Full";
                else
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = "Half";

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                string morng = Convert.ToString(ds.Tables[0].Rows[sel]["Morning"]);
                string eveng = Convert.ToString(ds.Tables[0].Rows[sel]["Evening"]);
                if (type.ToUpper() == "TRUE")
                {
                    if (morng.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Evening";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "Morning";

                    if (eveng.ToUpper() == "FALSE")
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Morning";
                    else
                        FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "Evening";
                }
                else
                {
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = "";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = "";
                }

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[sel]["HolidayDescription"]);

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

            }

            FpSpreadbase.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);

            #endregion

            #region visible

            FpSpreadbase.Sheets[0].PageSize = FpSpreadbase.Sheets[0].RowCount;
            FpSpreadbase.SaveChanges();
            divspread.Visible = true;
            FpSpreadbase.Visible = true;
            FpSpreadbase.Height = 380;
            FpSpreadbase.ShowHeaderSelection = false;
            print.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";

            #endregion
        }
        catch
        {

        }

    }
    #endregion

    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        divaddnew.Visible = true;
        txtdes.Text = "";

        addnewtxtfrdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        addnewtxttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        addnewtxtfrdt.Attributes.Add("readonly", "readonly");
        addnewtxttodt.Attributes.Add("readonly", "readonly");
        addnewtxtfrdt.Enabled = true;
        addnewtxttodt.Enabled = true;

        if (rbstud.Checked == true)
        {
            trstud.Visible = true;
            trstf.Visible = false;
            trlbr.Visible = false;
            trhos.Visible = false;
            trleave.Visible = true;
            addnewrbstud.Checked = true;
            addnewrbstaff.Checked = false;
            addnewrblbr.Checked = false;
            addnewrbhos.Checked = false;
            addnewrbstud_Changed(sender, e);
            addnewbindBtch();
            addnewbinddeg();
            addnewbinddept();
            addnewbindsem();
        }
        else if (rbstaff.Checked == true)
        {
            trstud.Visible = false;
            trstf.Visible = true;
            trlbr.Visible = false;
            trhos.Visible = false;
            trleave.Visible = true;
            addnewrbstud.Checked = false;
            addnewrbstaff.Checked = true;
            addnewrblbr.Checked = false;
            addnewrbhos.Checked = false;
            loadstafftype();
            LoadDays();
        }
        else if (rblbr.Checked == true)
        {
            trstud.Visible = false;
            trstf.Visible = false;
            trlbr.Visible = true;
            trleave.Visible = true;
            trhos.Visible = false;
            addnewrbstud.Checked = false;
            addnewrbstaff.Checked = false;
            addnewrblbr.Checked = true;
            addnewrbhos.Checked = false;
            addnewLoadLibrary();
        }
        else if (rblhos.Checked == true)
        {
            trstud.Visible = false;
            trstf.Visible = false;
            trlbr.Visible = false;
            trleave.Visible = true;
            trhos.Visible = true;
            addnewrbstud.Checked = false;
            addnewrbstaff.Checked = false;
            addnewrblbr.Checked = false;
            addnewrbhos.Checked = true;
            chk_mess.Checked = true;
            chk_hostel.Checked = false;
            lblmess.Visible = true;
            anmess.Visible = true;
            lblhostel.Visible = false;
            anhostel.Visible = false;
            bindmess();
            bindHostel();
        }
        else if (rblmess.Checked == true)
        {
            trstud.Visible = false;
            trstf.Visible = false;
            trlbr.Visible = false;
            trleave.Visible = true;
            trhos.Visible = true;
            addnewrbstud.Checked = false;
            addnewrbstaff.Checked = false;
            addnewrblbr.Checked = false;
            addnewrbhos.Checked = true;
            chk_mess.Checked = true;
            chk_hostel.Checked = false;
            lblmess.Visible = true;
            anmess.Visible = true;
            lblhostel.Visible = false;
            anhostel.Visible = false;
            bindmess();
            bindHostel();
        }

    }

    #endregion

    #region Lookup

    public void addnewbindBtch()
    {
        try
        {
            addnewcblyr.Items.Clear();
            addnewcbyr.Checked = false;
            addnewtxtyr.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                addnewcblyr.DataSource = ds;
                addnewcblyr.DataTextField = "batch_year";
                addnewcblyr.DataValueField = "batch_year";
                addnewcblyr.DataBind();
                if (addnewcblyr.Items.Count > 0)
                {
                    for (i = 0; i < addnewcblyr.Items.Count; i++)
                    {
                        addnewcblyr.Items[i].Selected = true;
                    }
                    addnewtxtyr.Text = "Batch(" + addnewcblyr.Items.Count + ")";
                    addnewcbyr.Checked = true;
                }
            }
            addnewbindsem();
        }
        catch { }
    }

    protected void addnewcbyr_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(addnewcbyr, addnewcblyr, addnewtxtyr, "Batch", "--Select--");
            addnewbindsem();
        }
        catch { }
    }

    protected void addnewcblyr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(addnewcbyr, addnewcblyr, addnewtxtyr, "Batch", "--Select--");
            addnewbindsem();
        }
        catch { }
    }

    public void addnewbinddeg()
    {
        try
        {
            addnewcbldegree.Items.Clear();
            addnewcbdegree.Checked = false;
            addnewtxtdegree.Text = "---Select---";
            string stream = "";
            string collcode = Convert.ToString(ddl_popclg1.SelectedValue);

            addnewcbldegree.Items.Clear();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collcode + "'";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                addnewcbldegree.DataSource = ds;
                addnewcbldegree.DataTextField = "course_name";
                addnewcbldegree.DataValueField = "course_id";
                addnewcbldegree.DataBind();
                if (addnewcbldegree.Items.Count > 0)
                {
                    for (i = 0; i < addnewcbldegree.Items.Count; i++)
                    {
                        addnewcbldegree.Items[i].Selected = true;
                    }
                    addnewtxtdegree.Text = "Degree(" + addnewcbldegree.Items.Count + ")";
                    addnewcbdegree.Checked = true;
                }
            }
            addnewbinddept();
            addnewbindsem();
        }
        catch { }
    }

    protected void addnewcbdegree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(addnewcbdegree, addnewcbldegree, addnewtxtdegree, "Degree", "--Select--");
            addnewbinddept();
            addnewbindsem();
        }
        catch { }
    }

    protected void addnewcbldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(addnewcbdegree, addnewcbldegree, addnewtxtdegree, "Degree", "--Select--");
            addnewbinddept();
            addnewbindsem();
        }
        catch { }
    }

    public void addnewbinddept()
    {
        try
        {
            addnewcbldept.Items.Clear();
            addnewcbdept.Checked = false;
            addnewtxtdept.Text = "---Select---";
            string degree = "";
            int i = 0;

            string collcode = Convert.ToString(ddl_popclg1.SelectedValue);
            degree = getCblSelectedValue(addnewcbldegree);

            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, "'" + degree + "'", collcode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    addnewcbldept.DataSource = ds;
                    addnewcbldept.DataTextField = "dept_name";
                    addnewcbldept.DataValueField = "degree_code";
                    addnewcbldept.DataBind();
                    if (addnewcbldept.Items.Count > 0)
                    {
                        for (i = 0; i < addnewcbldept.Items.Count; i++)
                        {
                            addnewcbldept.Items[i].Selected = true;
                        }
                        addnewtxtdept.Text = "Department(" + addnewcbldept.Items.Count + ")";
                        addnewcbdept.Checked = true;
                    }
                }
                addnewbindsem();
            }
        }
        catch { }
    }

    protected void addnewcbdept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(addnewcbdept, addnewcbldept, addnewtxtdept, "Department", "--Select--");
            addnewbindsem();
        }
        catch { }
    }

    protected void addnewcbldept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(addnewcbdept, addnewcbldept, addnewtxtdept, "Department", "--Select--");
            addnewbindsem();
        }
        catch { }
    }

    protected void addnewbindsem()
    {
        try
        {
            string batch2 = "";
            string dept = "";

            batch2 = getCblSelectedText(addnewcblyr);

            dept = getCblSelectedValue(addnewcbldept);

            string collcode = Convert.ToString(ddl_popclg1.SelectedValue);

            string selq = "select distinct Current_Semester from Registration where degree_code in('" + dept + "') and Batch_Year in('" + batch2 + "') and college_code='" + collcode + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' order by Current_Semester";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                addnewcblsem.Items.Clear();
                addnewcblsem.DataSource = ds;
                addnewcblsem.DataTextField = "Current_Semester";
                addnewcblsem.DataValueField = "Current_Semester";
                addnewcblsem.DataBind();
                if (cbl_sem.Items.Count > 0)
                {
                    for (i = 0; i < addnewcblsem.Items.Count; i++)
                    {
                        addnewcblsem.Items[i].Selected = true;
                    }
                    addnewtxtsem.Text = "Semester(" + addnewcblsem.Items.Count + ")";
                    addnewcbsem.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void addnewcbsem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(addnewcbsem, addnewcblsem, addnewtxtsem, "Semester", "--Select--");
    }

    protected void addnewcblsem_sssOnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(addnewcbsem, addnewcblsem, addnewtxtsem, "Semester", "--Select--");
    }

    protected void loadstafftype()
    {
        try
        {
            ds.Clear();
            addnewcblstafftype.Items.Clear();
            string collcode = Convert.ToString(ddl_popclg2.SelectedValue);
            // string item = "select distinct stftype,category_code  from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and stftype is not null and stftype<>'' and college_code = '" + collcode + "'";

            // string item = "select distinct category_name,category_code from staffcategorizer where  college_code='" + collcode + "'";

            string item = "select distinct category_name,c.category_code from staffcategorizer c,stafftrans st where st.category_code=c.category_code and college_code='" + collcode + "'";

            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                addnewcblstafftype.DataSource = ds;
                addnewcblstafftype.DataTextField = "category_name";
                addnewcblstafftype.DataValueField = "category_code";
                addnewcblstafftype.DataBind();
                if (addnewcblstafftype.Items.Count > 0)
                {
                    for (int i = 0; i < addnewcblstafftype.Items.Count; i++)
                    {
                        addnewcblstafftype.Items[i].Selected = true;
                    }
                    addnewtxtstftype.Text = "StaffType (" + addnewcblstafftype.Items.Count + ")";
                    addnewcbstafftype.Checked = true;
                }
            }
            else
            {
                addnewtxtstftype.Text = "--Select--";
                addnewcbstafftype.Checked = false;
            }
        }
        catch { }
    }

    protected void addnewcbstafftype_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(addnewcbstafftype, addnewcblstafftype, addnewtxtstftype, "Staff Type", "--Select--");
    }

    protected void addnewcblstafftype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(addnewcbstafftype, addnewcblstafftype, addnewtxtstftype, "Staff Type", "--Select--");
    }

    protected void LoadDays()
    {
        try
        {
            addnewcbllvedy.Items.Clear();
            addnewcbllvedy.Items.Add(new ListItem("Sunday", "1"));
            addnewcbllvedy.Items.Add(new ListItem("Monday", "2"));
            addnewcbllvedy.Items.Add(new ListItem("Tuesday", "3"));
            addnewcbllvedy.Items.Add(new ListItem("Wednesday", "4"));
            addnewcbllvedy.Items.Add(new ListItem("thursday", "5"));
            addnewcbllvedy.Items.Add(new ListItem("Friday", "6"));
            addnewcbllvedy.Items.Add(new ListItem("Saturday", "7"));
            for (int i = 0; i < addnewcbllvedy.Items.Count; i++)
            {
                addnewcbllvedy.Items[i].Selected = true;
            }
            addnewcblvedy.Checked = true;
            addnewtxtlvedy.Text = "Leave Days (" + addnewcbllvedy.Items.Count + ")";
        }
        catch { }
    }

    protected void addnewcblvedy_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(addnewcblvedy, addnewcbllvedy, addnewtxtlvedy, "Leave Days", "--Select--");
    }

    protected void addnewcbllvedy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(addnewcblvedy, addnewcbllvedy, addnewtxtlvedy, "Leave Days", "--Select--");
    }

    protected void addnewLoadLibrary()
    {
        try
        {
            ds.Clear();
            addnewcbllbr.Items.Clear();
            string collcode = Convert.ToString(ddl_popclg3.SelectedValue);
            string item = "select lib_code,lib_name from library where college_code='" + collcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                addnewcbllbr.DataSource = ds;
                addnewcbllbr.DataTextField = "lib_name";
                addnewcbllbr.DataValueField = "lib_code";
                addnewcbllbr.DataBind();
                if (addnewcbllbr.Items.Count > 0)
                {
                    for (int i = 0; i < addnewcbllbr.Items.Count; i++)
                    {
                        addnewcbllbr.Items[i].Selected = true;
                    }
                    addnewtxtlbr.Text = "Library (" + addnewcbllbr.Items.Count + ")";
                }
            }
            else
            {
                addnewtxtlbr.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void addnewcblbr_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(addnewcblbr, addnewcbllbr, addnewtxtlbr, "Library", "--Select--");
    }

    protected void addnewcbllbr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(addnewcblbr, addnewcbllbr, addnewtxtlbr, "Library", "--Select--");
    }

    protected void imgaddmew_Click(object sender, EventArgs e)
    {
        divaddnew.Visible = false;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int cbvalue = 0;
            string degreecode = "";
            string feecat = "";
            string sem = "";
            string stftype = "";
            string stafftype = "";
            string leavedys = "";
            string libry = "";
            string leave = "";
            string descr = "";
            string mng = "";
            string eve = "";
            string messorhostel = "";
            string messhostel = "";
            string mess = "";
            string hostel = "";
            string dept_Code = string.Empty;

            bool value = false;
            int sel = 0;
            int savecount = 0;
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string fromdate = Convert.ToString(addnewtxtfrdt.Text);
            string todate = Convert.ToString(addnewtxttodt.Text);
            DateTime dtfr = new DateTime();
            DateTime dt1 = new DateTime();
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                dtfr = Convert.ToDateTime(fromdate);
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                dt1 = Convert.ToDateTime(todate);
            }

            if (addnewrbhalf.Checked == true)
            {
                leave = "1";
                if (rbmng.Checked == true)
                {
                    mng = "1";
                    eve = "0";
                }
                else
                {
                    eve = "1";
                    mng = "0";
                }
            }
            else if (addnewrbfull.Checked == true)
            {
                leave = "0";
                mng = "1";
                eve = "1";

                if (addnewrbhos.Checked == true)
                {

                    leave = "0";
                    mng = "0";
                    eve = "0";
                }

            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Leave Type!";
                return;
            }

            if (txtdes.Text.Trim() != "")
            {
                descr = Convert.ToString(txtdes.Text);
            }
            string SelectQ = "";
            if (addnewrbstud.Checked == true)
            {
                string collcode = Convert.ToString(ddl_popclg1.SelectedValue);

                for (sel = 0; sel < addnewcbldept.Items.Count; sel++)
                {
                    if (addnewcbldept.Items[sel].Selected == true)
                    {
                        degreecode = Convert.ToString(addnewcbldept.Items[sel].Value);
                        for (int k = 0; k < addnewcblsem.Items.Count; k++)
                        {
                            if (addnewcblsem.Items[k].Selected == true)
                            {
                                feecat = Convert.ToString(addnewcblsem.Items[k].Value);
                                DateTime dt = dtfr;
                                if (ViewState["date_desc"] == null)//delsi2205
                                {
                                    while (dt <= dt1)
                                    {
                                        SelectQ = "if exists (select * from holidayStudents where degree_code='" + degreecode + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "' and semester='" + feecat + "' and college_code='" + collcode + "') update holidayStudents set halforfull='" + leave + "',holiday_desc='" + descr + "',morning='" + mng + "',evening='" + eve + "' where degree_code='" + degreecode + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "' and semester='" + feecat + "' and college_code='" + collcode + "' else insert into holidayStudents (degree_code,holiday_date,holiday_desc,semester,halforfull,morning,evening,college_code) values('" + degreecode + "','" + dt.ToString("MM/dd/yyyy") + "','" + descr + "','" + feecat + "','" + leave + "','" + mng + "','" + eve + "','" + collcode + "')";
                                        savecount = d2.update_method_wo_parameter(SelectQ, "Text");
                                        value = true;
                                        dt = dt.AddDays(1);
                                    }
                                }
                                else if (ViewState["date_desc"] != null)
                                {

                                    Hashtable getdatedesc = (Hashtable)ViewState["date_desc"];

                                    foreach (DictionaryEntry val in getdatedesc)
                                    {
                                        string dateval = Convert.ToString(val.Key);
                                        string descval = Convert.ToString(val.Value);

                                        string[] splitdate = dateval.Split('/');
                                        string datedb = Convert.ToString(splitdate[1]) + "/" + Convert.ToString(splitdate[0]) + "/" + Convert.ToString(splitdate[2]);

                                        DateTime date_val = Convert.ToDateTime(datedb);

                                        SelectQ = "if exists (select * from holidayStudents where degree_code='" + degreecode + "' and holiday_date='" + date_val.ToString("MM/dd/yyyy") + "' and semester='" + feecat + "' and college_code='" + collcode + "') update holidayStudents set halforfull='" + leave + "',holiday_desc='" + descval + "',morning='" + mng + "',evening='" + eve + "' where degree_code='" + degreecode + "' and holiday_date='" + date_val.ToString("MM/dd/yyyy") + "' and semester='" + feecat + "' and college_code='" + collcode + "' else insert into holidayStudents (degree_code,holiday_date,holiday_desc,semester,halforfull,morning,evening,college_code) values('" + degreecode + "','" + date_val.ToString("MM/dd/yyyy") + "','" + descval + "','" + feecat + "','" + leave + "','" + mng + "','" + eve + "','" + collcode + "')";
                                        savecount = d2.update_method_wo_parameter(SelectQ, "Text");
                                        value = true;


                                    }

                                }
                            }
                        }
                    }
                }

                //codehere

            }
            else if (addnewrbstaff.Checked == true)
            {
                string collcode = Convert.ToString(ddl_popclg2.SelectedValue);
                for (int j = 0; j < cbl_Department.Items.Count; j++)
                {
                    if (cbl_Department.Items[j].Selected == true)
                    {
                        dept_Code = Convert.ToString(cbl_Department.Items[j].Value);

                        for (sel = 0; sel < addnewcblstafftype.Items.Count; sel++)
                        {
                            if (addnewcblstafftype.Items[sel].Selected == true)
                            {
                                stftype = Convert.ToString(addnewcblstafftype.Items[sel].Value);
                                stafftype = Convert.ToString(addnewcblstafftype.Items[sel].Text);
                                for (int k = 0; k < addnewcbllvedy.Items.Count; k++)
                                {
                                    if (addnewcbllvedy.Items[k].Selected == true)
                                    {
                                        leavedys = Convert.ToString(addnewcbllvedy.Items[k].Value);
                                        DateTime dt = dtfr;
                                        if (ViewState["date_desc"] == null)
                                        {
                                            while (dt <= dt1)
                                            {
                                                //SelectQ = "if exists (select * from holidayStaff where category_code='" + stftype + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "' and college_code='" + collcode + "' ) update holidayStaff set halforfull='" + leave + "',morning='" + mng + "',evening='" + eve + "',holiday_desc='" + descr + "',StfType='" + stafftype + "' where category_code='" + stftype + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "' and college_code='" + collcode + "'  else insert into holidayStaff(category_code,holiday_date,holiday_desc,college_code,halforfull,morning,evening,StfType) values('" + stftype + "','" + dt.ToString("MM/dd/yyyy") + "','" + descr + "','" + collcode + "','" + leave + "','" + mng + "','" + eve + "','" + stafftype + "')";//delsi1406
                                                // savecount = d2.update_method_wo_parameter(SelectQ, "Text");
                                                // value = true;


                                                SelectQ = "if exists (select * from holidayStaff where category_code='" + stftype + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "' and college_code='" + collcode + "' and dept_code='" + dept_Code + "' ) update holidayStaff set halforfull='" + leave + "',morning='" + mng + "',evening='" + eve + "',holiday_desc='" + descr + "',StfType='" + stafftype + "',dept_code='" + dept_Code + "',category_code='" + stftype + "' where category_code='" + stftype + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "' and college_code='" + collcode + "' and dept_code='" + dept_Code + "'  else insert into holidayStaff(category_code,holiday_date,holiday_desc,college_code,halforfull,morning,evening,StfType,dept_code) values('" + stftype + "','" + dt.ToString("MM/dd/yyyy") + "','" + descr + "','" + collcode + "','" + leave + "','" + mng + "','" + eve + "','" + stafftype + "','" + dept_Code + "')";

                                                savecount = d2.update_method_wo_parameter(SelectQ, "Text");
                                                value = true;
                                                dt = dt.AddDays(1);
                                            }
                                        }
                                        else if (ViewState["date_desc"] != null)
                                        {
                                            Hashtable getdatedesc = (Hashtable)ViewState["date_desc"];

                                            foreach (DictionaryEntry val in getdatedesc)
                                            {
                                                string dateval = Convert.ToString(val.Key);
                                                string descval = Convert.ToString(val.Value);

                                                string[] splitdate = dateval.Split('/');
                                                string datedb = Convert.ToString(splitdate[1]) + "/" + Convert.ToString(splitdate[0]) + "/" + Convert.ToString(splitdate[2]);

                                                DateTime date_val = Convert.ToDateTime(datedb);
                                                SelectQ = "if exists (select * from holidayStaff where category_code='" + stftype + "' and holiday_date='" + date_val.ToString("MM/dd/yyyy") + "' and college_code='" + collcode + "' and dept_code='" + dept_Code + "' ) update holidayStaff set halforfull='" + leave + "',morning='" + mng + "',evening='" + eve + "',holiday_desc='" + descval + "',StfType='" + stafftype + "',dept_code='" + dept_Code + "',category_code='" + stftype + "' where category_code='" + stftype + "' and holiday_date='" + date_val.ToString("MM/dd/yyyy") + "' and college_code='" + collcode + "' and dept_code='" + dept_Code + "'  else insert into holidayStaff(category_code,holiday_date,holiday_desc,college_code,halforfull,morning,evening,StfType,dept_code) values('" + stftype + "','" + date_val.ToString("MM/dd/yyyy") + "','" + descval + "','" + collcode + "','" + leave + "','" + mng + "','" + eve + "','" + stafftype + "','" + dept_Code + "')";

                                                savecount = d2.update_method_wo_parameter(SelectQ, "Text");
                                                value = true;



                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (addnewrblbr.Checked == true)
            {
                string collcode = Convert.ToString(ddl_popclg3.SelectedValue);
                for (sel = 0; sel < addnewcbllbr.Items.Count; sel++)
                {
                    if (addnewcbllbr.Items[sel].Selected == true)
                    {
                        libry = Convert.ToString(addnewcbllbr.Items[sel].Value);
                        DateTime dt = dtfr;
                        while (dt <= dt1)
                        {
                            SelectQ = "if exists (select * from Holiday_Library where Lib_Code='" + libry + "' and Holiday_Date='" + dt.ToString("MM/dd/yyyy") + "'  and College_code='" + collcode + "') update Holiday_Library set Holiday_Date='" + dt.ToString("MM/dd/yyyy") + "' , HalfOrFull='" + leave + "' , Morning='" + mng + "' , Evening='" + eve + "',Holiday_Desc='" + descr + "' where Lib_Code='" + libry + "' and College_Code='" + collcode + "' and Holiday_Date='" + dt.ToString("MM/dd/yyyy") + "' else insert into Holiday_Library (Holiday_Date,HalfOrFull,Morning,Evening,Holiday_Desc,Lib_Code,College_Code) values('" + dt.ToString("MM/dd/yyyy") + "','" + leave + "','" + mng + "','" + eve + "','" + descr + "','" + libry + "','" + collcode + "')";
                            savecount = d2.update_method_wo_parameter(SelectQ, "Text");
                            value = true;
                            dt = dt.AddDays(1);
                        }
                    }
                }
            }
            else if (addnewrbhos.Checked == true)
            {
                if (chk_mess.Checked == false && chk_hostel.Checked == false)
                {

                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please Select Mess Or Hostel Holiday";
                    return;
                }

                if (chk_mess.Checked == true)
                {
                    messorhostel = "1";
                    DateTime dt = dtfr;
                    while (dt <= dt1)
                    {
                        if (cblmess.Items.Count > 0)
                        {
                            messhostel = Convert.ToString(d2.getCblSelectedValue(cblmess));

                            if (messhostel != "")
                            {
                                string meho = "','";
                                string[] split = messhostel.Split(meho.ToCharArray());
                                if (split.Length > 0)
                                {

                                    for (int i = 0; i < split.Length; i++)
                                    {
                                        if (!string.IsNullOrEmpty(split[i]))
                                        {
                                            mess = split[i];
                                            //mess = mess.Remove(mess.Length - 3, 3);
                                            if (!string.IsNullOrEmpty(mess))
                                            {
                                                SelectQ = "if exists (select * from HT_Holidays where  HolidayDate='" + dt.ToString("MM/dd/yyyy") + "' and HolidayType='" + messorhostel + "' and MessCode='" + mess + "') update HT_Holidays set HolidayDate='" + dt.ToString("MM/dd/yyyy") + "' ,HolidayType='" + messorhostel + "',MessCode ='" + mess + "',HolidayForHostler='1',HolidayForDayscholar='1',HolidayForStaff='1' where HolidayDate='" + dt.ToString("MM/dd/yyyy") + "' and HolidayType='" + messorhostel + "' and MessCode='" + mess + "' else insert into HT_Holidays (HolidayDate,HolidayType,MessCode,HolidayForHostler,HolidayForDayscholar,HolidayForStaff) values('" + dt.ToString("MM/dd/yyyy") + "','" + messorhostel + "','" + mess + "','1','1','1')";
                                                savecount = d2.update_method_wo_parameter(SelectQ, "Text");

                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alert.Visible = true;
                                lbl_alert.Text = "Please Select Any Mess Name";
                                return;
                            }

                        }
                        value = true;
                        dt = dt.AddDays(1);
                    }
                }
                if (chk_hostel.Checked == true)
                {
                    if (cblhostel.Items.Count == 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Please Select Any Hostel Name";
                        return;
                    }
                    messorhostel = "0";
                    DateTime dt = dtfr;
                    while (dt <= dt1)
                    {
                        if (cblhostel.Items.Count > 0)
                        {
                            messhostel = Convert.ToString(d2.getCblSelectedValue(cblhostel));
                            if (messhostel != "")
                            {
                                string meho = "','";
                                string[] split1 = messhostel.Split(meho.ToCharArray());

                                if (split1.Length > 0)
                                {
                                    for (int i = 0; i < split1.Length; i++)
                                    {
                                        if (!string.IsNullOrEmpty(split1[i]))
                                        {
                                            hostel = split1[i];
                                            //mess = mess.Remove(mess.Length - 3, 3);
                                            if (!string.IsNullOrEmpty(hostel))
                                            {

                                                SelectQ = "if exists (select * from HT_Holidays where  HolidayDate='" + dt.ToString("MM/dd/yyyy") + "' and HolidayType='" + messorhostel + "' and MessCode='" + hostel + "') update HT_Holidays set HolidayDate='" + dt.ToString("MM/dd/yyyy") + "' ,HolidayType='" + messorhostel + "',MessCode ='" + hostel + "',HolidayForHostler='1',HolidayForDayscholar='1',HolidayForStaff='1' where HolidayDate='" + dt.ToString("MM/dd/yyyy") + "' and HolidayType='" + messorhostel + "' and MessCode='" + hostel + "' else insert into HT_Holidays (HolidayDate,HolidayType,MessCode,HolidayForHostler,HolidayForDayscholar,HolidayForStaff) values('" + dt.ToString("MM/dd/yyyy") + "','" + messorhostel + "','" + hostel + "','1','1','1')";
                                                savecount = d2.update_method_wo_parameter(SelectQ, "Text");
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alert.Visible = true;
                                lbl_alert.Text = "Please Select Any Mess Name";
                                return;
                            }
                        }
                        value = true;
                        dt = dt.AddDays(1);
                    }
                }

            }
            if (value == true)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Saved Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please fill the Correct Values";
            }
        }
        catch { }
    }

    protected void btndel_Click(object sender, EventArgs e)//delsisref
    {
        try
        {
            FpSpreadbase.SaveChanges();
            if (checkedOK())
            {
                int delcount = 0;
                string collcode = Convert.ToString(ddl_collegename.SelectedValue);
                if (rbstud.Checked == true)
                {
                    for (int ik = 1; ik < FpSpreadbase.Sheets[0].Rows.Count; ik++)
                    {
                        byte newcheck = Convert.ToByte(FpSpreadbase.Sheets[0].Cells[ik, 1].Value);
                        if (newcheck == 1)
                        {
                            string degreecode = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 2].Tag);
                            string sem = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 3].Text);
                            string holdate = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 4].Text);
                            DateTime dt = new DateTime();
                            if (holdate.Trim() != "")
                            {
                                string[] spldat = holdate.Split('/');
                                dt = Convert.ToDateTime(spldat[1] + "/" + spldat[0] + "/" + spldat[2]);
                            }
                            string delq = "Delete from holidayStudents where degree_code='" + degreecode + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "' and semester='" + sem + "' and college_Code='" + collcode + "'";
                            delcount = d2.update_method_wo_parameter(delq, "Text");
                        }
                    }
                }
                else if (rbstaff.Checked == true)
                {
                    for (int ik = 1; ik < FpSpreadbase.Sheets[0].Rows.Count; ik++)
                    {
                        byte newcheck = Convert.ToByte(FpSpreadbase.Sheets[0].Cells[ik, 1].Value);
                        if (newcheck == 1)
                        {
                            string deptCode = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 2].Tag);
                            string catcode = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 3].Tag);
                            string stafftype = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 3].Text);
                            string holdate = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 4].Text);
                            DateTime dt = new DateTime();
                            if (holdate.Trim() != "")
                            {
                                string[] spldat = holdate.Split('/');
                                dt = Convert.ToDateTime(spldat[1] + "/" + spldat[0] + "/" + spldat[2]);
                            }
                            //string delq = "Delete from holidayStaff where category_code='" + catcode + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "'  and college_Code='" + collcode + "'";//and StfType='" + stafftype + "'


                            string delq = "Delete from holidayStaff where category_code='" + catcode + "' and dept_code='" + deptCode + "' and holiday_date='" + dt.ToString("MM/dd/yyyy") + "'  and college_Code='" + collcode + "' ";
                            delcount = d2.update_method_wo_parameter(delq, "Text");
                        }
                    }
                }
                else if (rblbr.Checked == true)
                {
                    for (int ik = 1; ik < FpSpreadbase.Sheets[0].Rows.Count; ik++)
                    {
                        byte newcheck = Convert.ToByte(FpSpreadbase.Sheets[0].Cells[ik, 1].Value);
                        if (newcheck == 1)
                        {
                            string libcode = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 2].Tag);
                            //string stafftype = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 2].Text);
                            string holdate = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 3].Text);
                            DateTime dt = new DateTime();
                            if (holdate.Trim() != "")
                            {
                                string[] spldat = holdate.Split('/');
                                dt = Convert.ToDateTime(spldat[1] + "/" + spldat[0] + "/" + spldat[2]);
                            }
                            string delq = "Delete from Holiday_Library where Holiday_Date='" + dt.ToString("MM/dd/yyyy") + "' and Lib_Code='" + libcode + "' and college_Code='" + collcode + "'";
                            delcount = d2.update_method_wo_parameter(delq, "Text");
                        }
                    }
                }
                //Added By Saranyadevi 13.2.2018
                else if (rblhos.Checked == true)
                {
                    for (int ik = 1; ik < FpSpreadbase.Sheets[0].Rows.Count; ik++)
                    {
                        byte newcheck = Convert.ToByte(FpSpreadbase.Sheets[0].Cells[ik, 1].Value);
                        if (newcheck == 1)
                        {
                            string hoscode = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 2].Tag);
                            //string stafftype = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 2].Text);
                            string holdate = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 3].Text);
                            DateTime dt = new DateTime();
                            if (holdate.Trim() != "")
                            {
                                string[] spldat = holdate.Split('/');
                                dt = Convert.ToDateTime(spldat[1] + "/" + spldat[0] + "/" + spldat[2]);
                            }
                            string delq = "Delete from HT_Holidays where HolidayDate='" + dt.ToString("MM/dd/yyyy") + "' and MessCode='" + hoscode + "'  and HolidayType='0'";
                            delcount = d2.update_method_wo_parameter(delq, "Text");
                        }
                    }
                }
                else if (rblmess.Checked == true)
                {
                    for (int ik = 1; ik < FpSpreadbase.Sheets[0].Rows.Count; ik++)
                    {
                        byte newcheck = Convert.ToByte(FpSpreadbase.Sheets[0].Cells[ik, 1].Value);
                        if (newcheck == 1)
                        {
                            string mescode = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 2].Tag);
                            //string stafftype = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 2].Text);
                            string holdate = Convert.ToString(FpSpreadbase.Sheets[0].Cells[ik, 3].Text);
                            DateTime dt = new DateTime();
                            if (holdate.Trim() != "")
                            {
                                string[] spldat = holdate.Split('/');
                                dt = Convert.ToDateTime(spldat[1] + "/" + spldat[0] + "/" + spldat[2]);
                            }
                            string delq = "Delete from HT_Holidays where HolidayDate='" + dt.ToString("MM/dd/yyyy") + "' and MessCode='" + mescode + "'  and HolidayType='1'";
                            delcount = d2.update_method_wo_parameter(delq, "Text");
                        }
                    }
                }//End By Saranyadevi 13.2.2018
                if (delcount > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Deleted Successfully!";

                    btngo_Click(sender, e);
                    if (rbstud.Checked == true)
                    {
                        LoadStudValues();
                    }
                    else if (rbstaff.Checked == true)
                    {
                        LoadStaffValues();
                    }
                    else
                    {
                        LoadLibrary();
                    }
                    if (FpSpreadbase.Sheets[0].RowCount == 1)
                    {
                        FpSpreadbase.Visible = false;
                        print.Visible = false;
                    }
                    else
                    {
                        FpSpreadbase.Visible = true;
                        print.Visible = true;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Records Not Deleted";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any one Item!";
            }
        }
        catch { }
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        divaddnew.Visible = false;
    }

    protected void addnewrbhalf_Changed(object sender, EventArgs e)
    {
        if (addnewrbhalf.Checked == true)
        {
            rbmng.Enabled = true;
            rbeve.Enabled = true;
        }

    }

    protected void addnewrbfull_Changed(object sender, EventArgs e)
    {
        if (addnewrbfull.Checked == true)
        {
            rbmng.Enabled = false;
            rbeve.Enabled = false;
        }

    }




    #region look up stud,staff,library,Hostel

    protected void addnewrbstud_Changed(object sender, EventArgs e)
    {
        trstud.Visible = true;
        trstf.Visible = false;
        trlbr.Visible = false;
        trleave.Visible = true;
        trhos.Visible = false;
        addnewrbhalf.Enabled = true;
        //
        loadcollege();
        ddl_popclg1.SelectedIndex = 0;
        addnewbindBtch();
        addnewbinddeg();
        addnewbinddept();
        addnewbindsem();
        addnewtxtfrdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        addnewtxttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtdes.Text = "";
        addnewrbhalf.Checked = false;
        //------- commented and added by Deepali on 9.4.18
        //addnewrbfull.Checked = false;    
        addnewrbfull.Checked = true;
        addnewrbfull_Changed(sender, e);
        //--------------
    }

    protected void addnewrbstaff_Changed(object sender, EventArgs e)
    {
        trstud.Visible = false;
        trstf.Visible = true;
        trlbr.Visible = false;
        trleave.Visible = true;
        trhos.Visible = false;
        addnewrbhalf.Enabled = true;
        loadcollege();
        ddl_popclg2.SelectedIndex = 0;
        loadstafftype();
        LoadDays();
        addnewtxtfrdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        addnewtxttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtdes.Text = "";
        addnewrbhalf.Checked = false;
        //------- commented and added by Deepali on 9.4.18
        //addnewrbfull.Checked = false;    
        addnewrbfull.Checked = true;
        addnewrbfull_Changed(sender, e);
        //--------------
    }

    protected void addnewrblbr_Changed(object sender, EventArgs e)
    {
        trstud.Visible = false;
        trstf.Visible = false;
        trlbr.Visible = true;
        trhos.Visible = false;
        trleave.Visible = true;
        addnewrbhalf.Enabled = true;
        //
        loadcollege();
        ddl_popclg3.SelectedIndex = 0;
        addnewLoadLibrary();
        addnewtxtfrdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        addnewtxttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtdes.Text = "";
        addnewrbhalf.Checked = false;
        //------- commented and added by Deepali on 9.4.18
        //addnewrbfull.Checked = false;    
        addnewrbfull.Checked = true;
        addnewrbfull_Changed(sender, e);
        //--------------
    }


    //Added By Saranyadevi 12.2.2018
    #region Hostel
    protected void addnewrbhos_Changed(object sender, EventArgs e)
    {
        trstud.Visible = false;
        trstf.Visible = false;
        trlbr.Visible = false;
        trleave.Visible = true;
        trhos.Visible = true;
        addnewrbhalf.Enabled = false;
        //
        bindmess();
        bindHostel();
        addnewtxtfrdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        addnewtxttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtdes.Text = "";
        addnewrbhalf.Checked = false;
        //------- commented and added by Deepali on 9.4.18
        //addnewrbfull.Checked = false;    
        addnewrbfull.Checked = true;
        addnewrbfull_Changed(sender, e);
        //--------------
    }

    #region mess

    public void bindmess()
    {
        try
        {
            cblmess.Items.Clear();
            cbmess.Checked = false;
            txtmess.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            string Query = "select MessMasterPK,MessName,MessAcr from HM_MessMaster order by MessMasterPK asc";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblmess.DataSource = ds;
                cblmess.DataTextField = "MessName";
                cblmess.DataValueField = "MessMasterPK";
                cblmess.DataBind();
                if (cblmess.Items.Count > 0)
                {
                    for (int i = 0; i < cblmess.Items.Count; i++)
                    {
                        cblmess.Items[i].Selected = true;
                    }
                    txtmess.Text = "Mess(" + cblmess.Items.Count + ")";
                    cbmess.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbmess_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxChange(cbmess, cblmess, txtmess, "Mess", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    protected void cblmess_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxListChange(cbmess, cblmess, txtmess, "mess", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }


    #endregion


    #region Hostel
    public void bindHostel()
    {

        try
        {
            cblhostel.Items.Clear();
            cbhostel.Checked = false;
            txthostel.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            string Query = "select HostelMasterPK,HostelName  from HM_HostelMaster";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblhostel.DataSource = ds;
                cblhostel.DataTextField = "HostelName";
                cblhostel.DataValueField = "HostelMasterPK";
                cblhostel.DataBind();
                if (cblhostel.Items.Count > 0)
                {
                    for (int i = 0; i < cblhostel.Items.Count; i++)
                    {
                        cblhostel.Items[i].Selected = true;
                    }
                    txthostel.Text = "Hostel(" + cblhostel.Items.Count + ")";
                    cbhostel.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbhostel_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxChange(cbhostel, cblhostel, txthostel, "Hostel", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }

    protected void cblhostel_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxListChange(cbhostel, cblhostel, txthostel, "Hostel", "--Select--");

        }
        catch (Exception ex)
        {
        }
    }



    #endregion



    protected void chk_mess_Changed(object sender, EventArgs e)
    {

        try
        {
            if (chk_mess.Checked == true)
            {
                lblmess.Visible = true;
                anmess.Visible = true;
            }
            else
            {
                lblmess.Visible = false;
                anmess.Visible = false;
            }
        }
        catch
        {
        }

    }

    protected void chk_hostel_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chk_hostel.Checked == true)
            {
                lblhostel.Visible = true;
                anhostel.Visible = true;
            }
            else
            {
                lblhostel.Visible = false;
                anhostel.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void ddl_mess_change(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }

    protected void ddl_hostel_change(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }

    #endregion
    #endregion

    #endregion

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
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
            else
            {
                txt.Text = deft;
                cb.Checked = false;
            }
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
                d2.printexcelreport(FpSpreadbase, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                if (rbstud.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Student Holiday Report Name";
                }
                else if (rbstaff.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Staff Holiday Report Name";
                }
                else if (rblbr.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Library Holiday Report Name";
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
            degreedetails = "Holidays Report";
            pagename = "HolidayEntry.aspx";
            Printcontrolhed.loadspreaddetails(FpSpreadbase, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    #endregion

    #region SemAndYear Setting

    protected string feecatValue(string value)
    {
        string semval = "";
        string type = "";
        try
        {
            string strtype = d2.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode1 + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
            if (strtype == "1")
                type = "Yearly";
            else
                type = "Semester";

            if (type == "Yearly")
            {
                if (value == "1" || value == "2")
                    semval = "1 Year";

                else if (value == "3" || value == "4")
                    semval = "2 Year";

                else if (value == "5" || value == "6")
                    semval = "3 Year";

                else if (value == "7" || value == "8")
                    semval = "4 Year";

            }
            else if (type == "Semester")
            {
                if (value == "1")
                    semval = "1 Semester";

                else if (value == "2")
                    semval = "2 Semester";

                else if (value == "3")
                    semval = "3 Semester";

                else if (value == "4")
                    semval = "4 Semester";

                else if (value == "5")
                    semval = "5 Semester";

                else if (value == "6")
                    semval = "6 Semester";

                else if (value == "7")

                    semval = "7 Semester";

                else if (value == "8")
                    semval = "8 Semester";

                else if (value == "9")
                    semval = "9 Semester";
            }

        }
        catch { }
        return semval;
    }

    #endregion

    protected void lbtn_Leav_Click(object sender, EventArgs e)
    {
        try
        {
            popLeavedays.Visible = true;
            // Fp_holiday.Visible = true;
            // string fromdate = Convert.ToString(addnewtxtfrdt.Text);
            // string todate = Convert.ToString(addnewtxttodt.Text);

            // txt_from.Text = Convert.ToString(fromdate);
            // txt_from.Enabled = false;

            //  txt_to.Text = Convert.ToString(todate);
            // txt_to.Enabled = false;
            btn_save2.Visible = true;
            lbl_displayHoliday.Visible = true;
            if (addnewrbhalf.Checked == true && addnewrbhalf.Enabled == true)
            {
                lbl_displayHoliday.Text = "Half Day";
            }
            else if (addnewrbfull.Checked == true && addnewrbfull.Enabled == true)
            {
                lbl_displayHoliday.Text = "Full Day";

            }
            lbl_error3.Visible = false;
            lbl_error3.Text = "";


            // txt_from.Text = DateTime.Now.ToString("dd/MM/yyyy");
            // txt_to.Text = DateTime.Now.ToString("dd/MM/yyyy");



            Fp_holiday.Visible = true;
            Fp_holiday.Sheets[0].RowCount = 0;
            Fp_holiday.SaveChanges();
            Fp_holiday.SheetCorner.ColumnCount = 0;
            Fp_holiday.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fp_holiday.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fp_holiday.ActiveSheetView.SelectionBackColor = Color.Coral;
            Fp_holiday.Sheets[0].AutoPostBack = false;
            Fp_holiday.Sheets[0].ColumnHeader.RowCount = 1;
            Fp_holiday.Sheets[0].RowHeader.Visible = false;
            Fp_holiday.Sheets[0].ColumnCount = 4;
            Fp_holiday.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fp_holiday.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

            string fromdate = Convert.ToString(addnewtxtfrdt.Text);


            string todate = Convert.ToString(addnewtxttodt.Text);
            int sno = 0;
            DateTime dtfr = new DateTime();
            DateTime dt1 = new DateTime();
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                dtfr = Convert.ToDateTime(fromdate);
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                dt1 = Convert.ToDateTime(todate);
            }

            FarPoint.Web.Spread.CheckBoxCellType cbsel = new FarPoint.Web.Spread.CheckBoxCellType();
            cbsel.AutoPostBack = false;
            FarPoint.Web.Spread.CheckBoxCellType cbselall = new FarPoint.Web.Spread.CheckBoxCellType();
            cbselall.AutoPostBack = true;
            Fp_holiday.Sheets[0].FrozenRowCount = 1;


            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fp_holiday.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fp_holiday.Sheets[0].Columns[0].Locked = true;

            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fp_holiday.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Date";
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fp_holiday.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            Fp_holiday.Sheets[0].Columns[2].Locked = true;


            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Description";
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fp_holiday.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            Fp_holiday.Sheets[0].Columns[3].Width = 312;

            DateTime dt = dtfr;

            if (dt >= dt1)
            {
                popLeavedays.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select From Date And To Date Correctly!";
                Fp_holiday.Visible = false;
                return;

            }
            while (dt <= dt1)
            {
                sno++;
                Fp_holiday.Sheets[0].RowCount++;
                Fp_holiday.Sheets[0].Cells[Fp_holiday.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                Fp_holiday.Sheets[0].Cells[Fp_holiday.Sheets[0].RowCount - 1, 1].CellType = cbsel;
                Fp_holiday.Sheets[0].Cells[Fp_holiday.Sheets[0].RowCount - 1, 1].Value = 0;
                string getdate = Convert.ToString(dt);
                string displaydate = string.Empty;
                string[] datesec = getdate.Split(' ');
                string dateval = Convert.ToString(datesec[0]);
                string[] frmdate = dateval.Split('/');
                if (frmdate.Length == 3)
                {
                    displaydate = frmdate[1].ToString() + "/" + frmdate[0].ToString() + "/" + frmdate[2].ToString();

                }

                Fp_holiday.Sheets[0].Cells[Fp_holiday.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(displaydate);

                dt = dt.AddDays(1);

            }
            btn_save2.Visible = true;
            Fp_holiday.Sheets[0].PageSize = Fp_holiday.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
        }



    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popLeavedays.Visible = false;
    }



    public void btnsav_Click(object sender, EventArgs e)
    {
        Hashtable dateDesc = new Hashtable();
        Fp_holiday.SaveChanges();
        if (checked_OK())
        {
            for (int ik = 0; ik < Fp_holiday.Sheets[0].Rows.Count; ik++)
            {
                byte newcheck = Convert.ToByte(Fp_holiday.Sheets[0].Cells[ik, 1].Value);
                if (newcheck == 1)
                {
                    string date_val = Convert.ToString(Fp_holiday.Sheets[0].Cells[ik, 2].Text);

                    string desc = Convert.ToString(Fp_holiday.Sheets[0].Cells[ik, 3].Text);
                    dateDesc.Add(Convert.ToString(date_val), Convert.ToString(desc));

                }
            }

            ViewState["date_desc"] = dateDesc as Hashtable;
            lbl_error3.Visible = false;
            lbl_error3.Text = "";

            addnewtxtfrdt.Enabled = false;
            addnewtxttodt.Enabled = false;



            popLeavedays.Visible = false;



        }
        else
        {
            lbl_error3.Visible = true;
            lbl_error3.Text = "You Haven't Selected Any Date!";


        }
    }
    //  public void btnGoleave_Click(object sender, EventArgs e)
    //  {
    //  try
    // {
    //    Fp_holiday.Visible = true;
    //    Fp_holiday.Sheets[0].RowCount = 0;
    //    Fp_holiday.SaveChanges();
    //    Fp_holiday.SheetCorner.ColumnCount = 0;
    //    Fp_holiday.CommandBar.Visible = false;
    //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //    darkstyle.ForeColor = Color.White;
    //    Fp_holiday.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
    //    Fp_holiday.ActiveSheetView.SelectionBackColor = Color.Coral;
    //    Fp_holiday.Sheets[0].AutoPostBack = false;
    //    Fp_holiday.Sheets[0].ColumnHeader.RowCount = 1;
    //    Fp_holiday.Sheets[0].RowHeader.Visible = false;
    //    Fp_holiday.Sheets[0].ColumnCount = 4;
    //    Fp_holiday.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
    //    Fp_holiday.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

    //    string fromdate = Convert.ToString(txt_from.Text);


    //    string todate = Convert.ToString(txt_to.Text);
    //    int sno = 0;
    //    DateTime dtfr = new DateTime();
    //    DateTime dt1 = new DateTime();
    //    string[] frdate = fromdate.Split('/');
    //    if (frdate.Length == 3)
    //    {
    //        fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
    //        dtfr = Convert.ToDateTime(fromdate);
    //    }
    //    string[] tdate = todate.Split('/');
    //    if (tdate.Length == 3)
    //    {
    //        todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
    //        dt1 = Convert.ToDateTime(todate);
    //    }

    //    FarPoint.Web.Spread.CheckBoxCellType cbsel = new FarPoint.Web.Spread.CheckBoxCellType();
    //    cbsel.AutoPostBack = false;
    //    FarPoint.Web.Spread.CheckBoxCellType cbselall = new FarPoint.Web.Spread.CheckBoxCellType();
    //    cbselall.AutoPostBack = true;
    //    Fp_holiday.Sheets[0].FrozenRowCount = 1;


    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //    Fp_holiday.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //    Fp_holiday.Sheets[0].Columns[0].Locked = true;

    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //    Fp_holiday.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Date";
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
    //    Fp_holiday.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
    //    Fp_holiday.Sheets[0].Columns[2].Locked = true;


    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Description";
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
    //    Fp_holiday.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
    //    Fp_holiday.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
    //    Fp_holiday.Sheets[0].Columns[3].Width = 312;

    //    DateTime dt = dtfr;
    //    if (dt >= dt1)
    //    {

    //        imgdiv2.Visible = true;
    //        lbl_alert.Visible = true;
    //        lbl_alert.Text = "Please Select From Date And To Date Correctly!";
    //        Fp_holiday.Visible = false;
    //        return;

    //    }

    //    while (dt <= dt1)
    //    {
    //        sno++;
    //        Fp_holiday.Sheets[0].RowCount++;
    //        Fp_holiday.Sheets[0].Cells[Fp_holiday.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
    //        Fp_holiday.Sheets[0].Cells[Fp_holiday.Sheets[0].RowCount - 1, 1].CellType = cbsel;
    //        Fp_holiday.Sheets[0].Cells[Fp_holiday.Sheets[0].RowCount - 1, 1].Value = 0;
    //        string getdate = Convert.ToString(dt);
    //        string displaydate = string.Empty;
    //        string[] datesec = getdate.Split(' ');
    //        string dateval = Convert.ToString(datesec[0]);
    //        string[] frmdate = dateval.Split('/');
    //        if (frmdate.Length == 3)
    //        {
    //            displaydate = frmdate[1].ToString() + "/" + frmdate[0].ToString() + "/" + frmdate[2].ToString();

    //        }

    //        Fp_holiday.Sheets[0].Cells[Fp_holiday.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(displaydate);

    //        dt = dt.AddDays(1);

    //    }
    //    btn_save2.Visible = true;
    //    Fp_holiday.Sheets[0].PageSize = Fp_holiday.Sheets[0].RowCount;

    //}
    //catch (Exception ex)
    //{


    //}
    //  }


    protected void Fp_holiday_UpdateCommand(Object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Fp_holiday.SaveChanges();
            byte check = Convert.ToByte(Fp_holiday.Sheets[0].Cells[0, 1].Value);
            if (check == 1)
            {
                for (int ik = 0; ik < Fp_holiday.Sheets[0].Rows.Count; ik++)
                {
                    Fp_holiday.Sheets[0].Cells[ik, 1].Value = 1;
                }
            }
            else
            {
                for (int ik = 0; ik < Fp_holiday.Sheets[0].Rows.Count; ik++)
                {
                    Fp_holiday.Sheets[0].Cells[ik, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    protected bool checked_OK()
    {
        bool OK = false;
        Fp_holiday.SaveChanges();
        try
        {
            for (int ik = 0; ik < Fp_holiday.Sheets[0].Rows.Count; ik++)
            {
                byte check = Convert.ToByte(Fp_holiday.Sheets[0].Cells[ik, 1].Value);
                if (check == 1)
                {
                    OK = true;
                }
            }
        }
        catch { }
        return OK;
    }

    public void binddepts(string scollege)//delsi1406
    {
        try
        {
            int height = 0;
            ds = d2.loaddepartment(scollege);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Department.DataSource = ds;
                cbl_Department.DataTextField = "dept_name";
                cbl_Department.DataValueField = "Dept_Code";
                cbl_Department.DataBind();
            }
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                cbl_Department.Items[i].Selected = true;
                height++;
            }
            txt_Department.Text = "Department(" + cbl_Department.Items.Count + ")";
            cb_Department.Checked = true;
            if (height > 10)
            {
                panel_Department.Height = 300;
            }
            else
            {
                panel_Department.Height = 150;
            }
        }
        catch (Exception e)
        {
        }
    }


    protected void cb_Department_CheckedChanged(object sender, EventArgs e)//delsi1406
    {
        if (cb_Department.Checked == true)
        {
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                cbl_Department.Items[i].Selected = true;
                txt_Department.Text = "Department(" + (cbl_Department.Items.Count) + ")";
            }
            panel_Department.Focus();
        }
        else
        {
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                cbl_Department.Items[i].Selected = false;
                txt_Department.Text = "---Select---";
            }
        }
    }


    protected void cbl_Department_SelectedIndexChanged(object sender, EventArgs e)//delsi1406
    {
        panel_Department.Focus();
        int desigcount = 0;
        for (int i = 0; i < cbl_Department.Items.Count; i++)
        {
            if (cbl_Department.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_Department.Text = "Department(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_Department.Text = "---Select---";
        }
        cb_Department.Checked = false;
    }

    public void binddeptss(string scollege)//delsi1406
    {
        try
        {
            int height = 0;
            ds = d2.loaddepartment(scollege);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldepartment.DataSource = ds;
                cbldepartment.DataTextField = "dept_name";
                cbldepartment.DataValueField = "Dept_Code";
                cbldepartment.DataBind();
            }
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                cbldepartment.Items[i].Selected = true;
                height++;
            }
            txtdept.Text = "Department(" + cbldepartment.Items.Count + ")";
            cbdepartment.Checked = true;
            if (height > 10)
            {
                panel_Department.Height = 300;
            }
            else
            {
                panel_Department.Height = 150;
            }
        }
        catch (Exception e)
        {
        }
    }




    protected void cb_dept_CheckedChanged(object sender, EventArgs e)//delsi1406
    {
        if (cbdepartment.Checked == true)
        {
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                cbldepartment.Items[i].Selected = true;
                txtdept.Text = "Department(" + (cbldepartment.Items.Count) + ")";
            }
            pnldept.Focus();
        }
        else
        {
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                cbldepartment.Items[i].Selected = false;
                txtdept.Text = "---Select---";
            }
        }
    }



    protected void cbl_dept_SelectedIndexChanged(object sender, EventArgs e)//delsi1406
    {
        pnldept.Focus();
        int desigcount = 0;
        for (int i = 0; i < cbldepartment.Items.Count; i++)
        {
            if (cbldepartment.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txtdept.Text = "Department(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txtdept.Text = "---Select---";
        }
        cbdepartment.Checked = false;
    }
}