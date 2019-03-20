using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;

public partial class HT_StaffRegistration : System.Web.UI.Page
{
    string user_code;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static ArrayList ItemListguest = new ArrayList();
    static ArrayList Itemindexguest = new ArrayList();
    Boolean Cellclick = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string college_code = "";
    string college = "";
    string course_id = string.Empty;
    static string Hostelcode = "";
    Hashtable hat4 = new Hashtable();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    bool check = false;
    bool checkguest = false;
    static string query = "";
    static string query1 = "";
    int count = 0;
    int flag = 0;
    string sqladd = "";
    static string mm = "";
    static string mmm = "";
    static string cln = "";
    string sql = "";
    string build = "";
    string floor = "";
    string flooor = "";
    string room = "";
    string[] fr;
    string[] address;
    string buildvalue1 = "";
    string build1 = "";
    string buildvalue2 = "";
    string build2 = "";
    string buildvalue3 = "";
    string build3 = "";
    string buildvalue4 = "";
    string build4 = "";
    string buildvalue6 = "";
    string build6 = "";
    string buildvalue7 = "";
    string build7 = "";
    string builldvalue5 = "";
    string builld5 = "";
    string build5 = "";
    string buildvalue8 = "";
    string build8 = "";
    string comm = "";
    string[] datesp;
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        lbl_validation.Text = "";
       // calvacatedate.StartDate = DateTime.Now;
       // caldisdate.StartDate = DateTime.Now;
        // CalendarExtender1.StartDate = DateTime.Now;//magesh 16.3.18
        CalendarExtender2.StartDate = DateTime.Now;
        //caladmin.StartDate = DateTime.Now;//magesh 16.3.18
        if (!IsPostBack)
        {
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            pheaderfilterguest.Visible = false;
            pcolumnorderguest.Visible = false;
            //cbl_roomlist_SelectedIndexChanged(sender,e);
            //search();
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;

            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.Visible = false;

            btn_pop1update.Visible = false;
            txt_staffnamesearch.Visible = true;

            bindroompopbuild();

            // clgbuild(bul);
            loadcollege();
            loadhostel();

            binddepartment();
            binddesignation();
            bindstafftype();

            loadcollegepopup();
            loadhostelpopup();

            loadcollegestaffpopup();
            bindstaffdepartmentpopup();

            txt_discontinuedate.Enabled = false;
            txt_vacatedate.Enabled = false;
            txt_pop1reason.Enabled = false;
            txt_discontinuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pop1admindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pop1admindate.Attributes.Add("readonly", "readonly");

            rdb_staffe.Checked = true;
            cb_include.Checked = true;
            cb_include_CheckedChanged(sender, e);
            //  CheckBox_column.Checked = true;
            //  CheckBox_column_CheckedChanged(sender, e);
            // btn_go_Click(sender, e);
            bindmessname();
            txt_admindate.Attributes.Add("readonly", "readonly");
            txt_admindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_vacatedateguest.Enabled = false;
            txt_vacatedateguest.Text = DateTime.Now.ToString("dd/MM/yyyy");

            Fpspread1.Width = 880;
            //magesh 12.3.18
            BindStudentType();
            BindgusttType();
            bindmessmaster();
            bindmessmaster1();
        }
    }
    protected void lb_logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch
        {
        }
    }

    public void bindmessmaster()
    {
        try
        {
            string selectQuery = d2.GetFunction("select MessMasterFK1 from HM_HostelMaster where HostelMasterPK='" + ddl_pop1hostelname.SelectedValue + "'");
            //string selectQuery1 =d2.GetFunction("select MessMasterPK from HM_MessMaster where MessMasterPK in(" + selectQuery + ") order by MessMasterPK asc");
            string[] spl = selectQuery.Split('-');
            if (spl.Length > 0)
            {
                string typ = string.Empty;
                if (spl.Count() > 0)
                {
                    for (int i = 0; i < spl.Count(); i++)
                    {
                        if (typ == "")
                        {
                            typ = "" + spl[i] + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + spl[i] + "";
                        }

                    }

                }
                selectQuery = ("select MessMasterPK,MessName from HM_MessMaster where MessMasterPK in('" + typ + "') order by MessMasterPK asc");
            }

          DataSet  dsmess = d2.select_method_wo_parameter(selectQuery, "text");
            // ddl_messmaster.Items.Clear();
            if (dsmess.Tables[0].Rows.Count > 0)
            {
                //magesh 20.6.18
                ddlmess.DataSource = dsmess;
                ddlmess.DataTextField = "MessName";
                ddlmess.DataValueField = "MessMasterPK";
                ddlmess.DataBind();
            }
            else
            {
                ddlmess.Items.Insert(0, "");
            }
            //    ddl_messmaster.DataSource = ds;
            //    ddl_messmaster.DataTextField = "MessName";
            //    ddl_messmaster.DataValueField = "MessMasterPK";
            //    ddl_messmaster.DataBind();
            //}
            //ddl_messmaster.Items.Insert(0, "Select");
        }
        catch
        {
            //ddl_messmaster.Items.Clear();
        }
    }

    public void bindmessmaster1()
    {
        try
        {
            string selectQuery = d2.GetFunction("select MessMasterFK1 from HM_HostelMaster where HostelMasterPK='" + ddl_messname.SelectedValue + "'");
            //string selectQuery1 =d2.GetFunction("select MessMasterPK from HM_MessMaster where MessMasterPK in(" + selectQuery + ") order by MessMasterPK asc");
            string[] spl = selectQuery.Split('-');
            if (spl.Length > 0)
            {
                string typ = string.Empty;
                if (spl.Count() > 0)
                {
                    for (int i = 0; i < spl.Count(); i++)
                    {
                        if (typ == "")
                        {
                            typ = "" + spl[i] + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + spl[i] + "";
                        }

                    }

                }
                selectQuery = ("select MessMasterPK,MessName from HM_MessMaster where MessMasterPK in('" + typ + "') order by MessMasterPK asc");
            }

          DataSet  dsmess = d2.select_method_wo_parameter(selectQuery, "text");
            // ddl_messmaster.Items.Clear();
            if (dsmess.Tables[0].Rows.Count > 0)
            {
                //magesh 20.6.18
                ddlmess1.DataSource = dsmess;
                ddlmess1.DataTextField = "MessName";
                ddlmess1.DataValueField = "MessMasterPK";
                ddlmess1.DataBind();
            }
            else
            {
                ddlmess1.Items.Insert(0, "");
            }
            //    ddl_messmaster.DataSource = ds;
            //    ddl_messmaster.DataTextField = "MessName";
            //    ddl_messmaster.DataValueField = "MessMasterPK";
            //    ddl_messmaster.DataBind();
            //}
            //ddl_messmaster.Items.Insert(0, "Select");
        }
        catch
        {
            //ddl_messmaster.Items.Clear();
        }
    }
    //magesh 21.6.18
    public void ddl_pop1hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if(check==false)
               idgeneration();
            bindmessmaster();
        }
        catch
        {
        }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddepartment();
        binddesignation();
        bindstafftype();
    }
    public void binddepartment()
    {
        try
        {
            ds.Clear();
            //string query = "";
            //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode1 + "'";
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            ds = d2.loaddepartment(clgcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_department.DataSource = ds;
                cbl_department.DataTextField = "dept_name";
                cbl_department.DataValueField = "dept_code";
                cbl_department.DataBind();
                if (cbl_department.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_department.Items.Count; i++)
                    {
                        cbl_department.Items[i].Selected = true;
                    }
                    txt_department.Text = "Department(" + cbl_department.Items.Count + ")";
                }
            }
            else
            {
                txt_department.Text = "--Select--";
            }
            for (int i = 0; i < cbl_department.Items.Count; i++)
            {
                cbl_department.Items[i].Selected = true;
                txt_department.Text = "Department(" + (cbl_department.Items.Count) + ")";
                cb_department.Checked = true;
            }
        }
        catch { }
    }
    protected void cb_department_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (cb_department.Checked == true)
            {
                for (int i = 0; i < cbl_department.Items.Count; i++)
                {
                    cbl_department.Items[i].Selected = true;
                }
                txt_department.Text = "Department(" + (cbl_department.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_department.Items.Count; i++)
                {
                    cbl_department.Items[i].Selected = false;
                }
                txt_department.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_department_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_department.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_department.Items.Count; i++)
            {
                if (cbl_department.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_department.Text = "--Select--";
                    cb_department.Checked = false;
                    build = cbl_department.Items[i].Text.ToString();
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
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_department.Items.Count)
            {
                txt_department.Text = "Department(" + seatcount + ")";
                cb_department.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_department.Text = "--Select--";
            }
            else
            {
                txt_department.Text = "Department(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void binddesignation()
    {
        try
        {
            ds.Clear();
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            ds = d2.loaddesignation(clgcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_designation.DataSource = ds;
                cbl_designation.DataTextField = "desig_name";
                cbl_designation.DataValueField = "desig_code";
                cbl_designation.DataBind();
                if (cbl_designation.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_designation.Items.Count; ro++)
                    {
                        cbl_designation.Items[ro].Selected = true;
                        cb_designation.Checked = true;
                    }
                    txt_designation.Text = "Designation(" + cbl_designation.Items.Count + ")";
                }
            }
        }
        catch
        {

        }
    }
    protected void cb_designation_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_designation.Checked == true)
            {
                for (int i = 0; i < cbl_designation.Items.Count; i++)
                {
                    cbl_designation.Items[i].Selected = true;
                }
                txt_designation.Text = "Department(" + (cbl_designation.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_designation.Items.Count; i++)
                {
                    cbl_designation.Items[i].Selected = false;
                }
                txt_designation.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_designation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_designation.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_designation.Items.Count; i++)
            {
                if (cbl_designation.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_designation.Text = "--Select--";
                    cb_designation.Checked = false;
                    build = cbl_designation.Items[i].Text.ToString();
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
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_designation.Items.Count)
            {
                txt_designation.Text = "Department(" + seatcount + ")";
                cb_designation.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_designation.Text = "--Select--";
            }
            else
            {
                txt_designation.Text = "Department(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void bindstafftype()
    {
        try
        {
            ds.Clear();
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            ds = d2.loadstafftype(clgcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stafftype.DataSource = ds;
                cbl_stafftype.DataTextField = "StfType";
                cbl_stafftype.DataValueField = "StfType";
                cbl_stafftype.DataBind();
                if (cbl_stafftype.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_stafftype.Items.Count; ro++)
                    {
                        cbl_stafftype.Items[ro].Selected = true;
                        cb_stafftype.Checked = true;
                    }
                    txt_stafftype.Text = "Staff Type(" + cbl_stafftype.Items.Count + ")";
                }
            }
        }
        catch
        {

        }

    }
    protected void cb_stafftype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (cb_stafftype.Checked == true)
            {
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = true;
                }
                txt_stafftype.Text = "Staff Type(" + (cbl_stafftype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = false;
                }
                txt_stafftype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_stafftype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_stafftype.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                if (cbl_stafftype.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_stafftype.Text = "--Select--";
                    cb_stafftype.Checked = false;
                    build = cbl_stafftype.Items[i].Text.ToString();
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
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_stafftype.Items.Count)
            {
                txt_stafftype.Text = "Staff Type(" + seatcount + ")";
                cb_stafftype.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_stafftype.Text = "--Select--";
            }
            else
            {
                txt_stafftype.Text = "Staff Type(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }

    }
    //public void loadhostel()
    //{
    //    try
    //    {
    //        ds = d2.BindHostel(collegecode1);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {

    //            cbl_hostelname.DataSource = ds;
    //            cbl_hostelname.DataTextField = "Hostel_Name";
    //            cbl_hostelname.DataValueField = "Hostel_code";
    //            cbl_hostelname.DataBind();
    //            //Hostelcode = cbl_hostelname.SelectedValue;

    //            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    //            {
    //                cbl_hostelname.Items[i].Selected = true;
    //                txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
    //                cb_hostelname.Checked = true;
    //            }

    //            string lochosname = "";
    //            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    //            {
    //                if (cbl_hostelname.Items[i].Selected == true)
    //                {
    //                    string hosname = cbl_hostelname.Items[i].Value.ToString();
    //                    if (lochosname == "")
    //                    {
    //                        lochosname = hosname;
    //                    }
    //                    else
    //                    {
    //                        lochosname = lochosname + "'" + "," + "'" + hosname;
    //                    }
    //                }
    //            }


    //            clgbuild(lochosname);
    //           // Hostelcode = lochosname;
    //        }
    //        else
    //        {
    //            cbl_hostelname.Items.Insert(0, "--Select--");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    ////protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    ////{
    ////    try
    ////    {
    ////        if (cb_hostelname.Checked == true)
    ////        {
    ////            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    ////            {
    ////                cbl_hostelname.Items[i].Selected = true;
    ////            }
    ////            txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";



    ////        }
    ////        else
    ////        {
    ////            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    ////            {
    ////                cbl_hostelname.Items[i].Selected = false;
    ////            }
    ////            txt_hostelname.Text = "--Select--";
    ////        }

    ////    }
    ////    catch (Exception ex)
    ////    {
    ////    }
    ////}
    //protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int seatcount = 0;
    //        cb_hostelname.Checked = false;

    //        string buildvalue = "";
    //        string build = "";
    //        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    //        {
    //            if (cbl_hostelname.Items[i].Selected == true)
    //            {
    //                seatcount = seatcount + 1;
    //                // txt_hostelname.Text = "--Select--";
    //                //  cb_hostelname.Checked = false;
    //                cb_buildingname.Checked = true;
    //                build = cbl_hostelname.Items[i].Text.ToString();
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
    //        clgbuild(buildvalue);
    //       // Hostelcode = buildvalue;
    //       // clgbuild(buildvalue);
    //        //clgfloorpop(buildvalue);
    //        if (seatcount == cbl_hostelname.Items.Count)
    //        {
    //            txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
    //            cb_hostelname.Checked = true;
    //        }
    //        else if (seatcount == 0)
    //        {
    //            txt_hostelname.Text = "--Select--";
    //        }
    //        else
    //        {
    //            txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }

    //}
    ////public void loadhostel()
    ////{
    ////    try
    ////    {

    ////        cbl_hostelname.Items.Clear();


    ////        ds = d2.BindHostel(collegecode1); 
    ////        if (ds.Tables[0].Rows.Count > 0)
    ////        {
    ////            cbl_hostelname.DataSource = ds;
    ////            cbl_hostelname.DataTextField = "Hostel_Name";
    ////            cbl_hostelname.DataValueField = "Hostel_code";
    ////            cbl_hostelname.DataBind();
    ////        }

    ////        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    ////        {
    ////            cbl_hostelname.Items[i].Selected = true;
    ////            txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
    ////            cb_hostelname.Checked = true;
    ////        }

    ////        string locbuild = "";
    ////        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    ////        {
    ////            if (cbl_hostelname.Items[i].Selected == true)
    ////            {
    ////                string builname = cbl_hostelname.Items[i].Text;
    ////                if (locbuild == "")
    ////                {
    ////                    locbuild = builname;
    ////                }
    ////                else
    ////                {
    ////                    locbuild = locbuild + "'" + "," + "'" + builname;
    ////                }
    ////            }
    ////        }
    ////        clgbuild(locbuild);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////    }
    ////}
    //protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_hostelname.Checked == true)
    //        {
    //            string buildvalue1 = "";
    //            string build1 = "";
    //            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    //            {
    //                if (cb_hostelname.Checked == true)
    //                {
    //                    cbl_hostelname.Items[i].Selected = true;
    //                    txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
    //                    //txt_floorname.Text = "--Select--";
    //                    build1 = cbl_hostelname.Items[i].Text.ToString();
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
    //           // Hostelcode = buildvalue1;
    //            clgbuild(buildvalue1);
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    //            {
    //                cbl_hostelname.Items[i].Selected = false;
    //                txt_hostelname.Text = "--Select--";
    //                cbl_buildingname.Items.Clear();
    //                cb_buildingname.Checked = false;
    //                txt_buildingname.Text = "--Select--";
    //                cbl_floorname.Items.Clear();
    //                cb_floorname.Checked = false;
    //                txt_floorname.Text = "--Select--";
    //                txt_roomname.Text = "--Select--";
    //                cb_roomname.Checked = false;
    //                cbl_roomname.Items.Clear();
    //            }
    //        }
    //        //  Button2.Focus();

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    ////protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    ////{
    ////    try
    ////    {
    ////        int seatcount = 0;
    ////        cb_hostelname.Checked = false;

    ////        string buildvalue = "";
    ////        string build = "";
    ////        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    ////        {
    ////            if (cbl_hostelname.Items[i].Selected == true)
    ////            {
    ////                seatcount = seatcount + 1;
    ////                // txt_floorname.Text = "--Select--";
    ////                cb_buildingname.Checked = true;
    ////                build = cbl_hostelname.Items[i].Text.ToString();
    ////                if (buildvalue == "")
    ////                {
    ////                    buildvalue = build;
    ////                }
    ////                else
    ////                {
    ////                    buildvalue = buildvalue + "'" + "," + "'" + build;

    ////                }

    ////            }
    ////        }
    ////        clgbuild(buildvalue1);
    ////        if (seatcount == cbl_hostelname.Items.Count)
    ////        {
    ////            txt_hostelname.Text = "Hostel(" + seatcount + ")";
    ////            cb_hostelname.Checked = true;
    ////        }
    ////        else if (seatcount == 0)
    ////        {
    ////            txt_hostelname.Text = "--Select--";
    ////        }
    ////        else
    ////        {
    ////            txt_hostelname.Text = "Hostel(" + seatcount + ")";
    ////        }
    ////        //  Button2.Focus();
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////    }

    ////}



    //public void clgbuild(string hostelname)
    //{
    //    try
    //    {
    //        cbl_buildingname.Items.Clear();
    //        string bul = "";
    //        string locbuild = "";

    //        bul = d2.GetBuildingCode(hostelname);
    //        ds = d2.BindBuilding(bul);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_buildingname.DataSource = ds;
    //            cbl_buildingname.DataTextField = "Building_Name";
    //            cbl_buildingname.DataValueField = "code";
    //            cbl_buildingname.DataBind();
    //        }

    //        for (int i = 0; i < cbl_buildingname.Items.Count; i++)
    //        {
    //            cbl_buildingname.Items[i].Selected = true;
    //            txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
    //            cb_buildingname.Checked = true;
    //        }
    //        for (int i = 0; i < cbl_buildingname.Items.Count; i++)
    //        {
    //            if (cbl_buildingname.Items[i].Selected == true)
    //            {
    //                string builname = cbl_buildingname.Items[i].Text;
    //                if (locbuild == "")
    //                {
    //                    locbuild = builname;
    //                }
    //                else
    //                {
    //                    locbuild = locbuild + "'" + "," + "'" + builname;
    //                }
    //            }
    //        }

    //        clgfloor(locbuild);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cbbuildname_CheckedChange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_buildingname.Checked == true)
    //        {
    //            string buildvalue1 = "";
    //            string build1 = "";
    //            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
    //            {
    //                if (cb_buildingname.Checked == true)
    //                {
    //                    cbl_buildingname.Items[i].Selected = true;
    //                    txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
    //                    //txt_floorname.Text = "--Select--";
    //                    build1 = cbl_buildingname.Items[i].Text.ToString();
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
    //            clgfloor(buildvalue1);
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
    //            {
    //                cbl_buildingname.Items[i].Selected = false;
    //                txt_buildingname.Text = "--Select--";
    //                cbl_floorname.Items.Clear();
    //                cb_floorname.Checked = false;
    //                txt_floorname.Text = "--Select--";
    //                txt_roomname.Text = "--Select--";
    //                cb_roomname.Checked = false;
    //                cbl_roomname.Items.Clear();
    //            }
    //        }
    //        //  Button2.Focus();

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cblbuildname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int seatcount = 0;
    //        cb_buildingname.Checked = false;

    //        string buildvalue = "";
    //        string build = "";
    //        for (int i = 0; i < cbl_buildingname.Items.Count; i++)
    //        {
    //            if (cbl_buildingname.Items[i].Selected == true)
    //            {
    //                seatcount = seatcount + 1;
    //                // txt_floorname.Text = "--Select--";
    //                cb_floorname.Checked = true;
    //                build = cbl_buildingname.Items[i].Text.ToString();
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
    //        clgfloor(buildvalue);
    //        if (seatcount == cbl_buildingname.Items.Count)
    //        {
    //            txt_buildingname.Text = "Building(" + seatcount + ")";
    //            cb_buildingname.Checked = true;
    //        }
    //        else if (seatcount == 0)
    //        {
    //            txt_buildingname.Text = "--Select--";
    //        }
    //        else
    //        {
    //            txt_buildingname.Text = "Building(" + seatcount + ")";
    //        }
    //        //  Button2.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //public void clgfloor(string buildname)
    //{
    //    try
    //    {
    //        //chklstfloorpo3.Items.Clear();
    //        cbl_floorname.Items.Clear();
    //        ds = d2.BindFloor(buildname);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_floorname.DataSource = ds;
    //            cbl_floorname.DataTextField = "Floor_Name";
    //            cbl_floorname.DataValueField = "Floor_Name";
    //            cbl_floorname.DataBind();

    //        }
    //        else
    //        {
    //            txt_floorname.Text = "--Select--";
    //        }
    //        //for selected floor
    //        for (int i = 0; i < cbl_floorname.Items.Count; i++)
    //        {
    //            cbl_floorname.Items[i].Selected = true;
    //            cb_floorname.Checked = true;
    //        }

    //        string locfloor = "";
    //        for (int i = 0; i < cbl_floorname.Items.Count; i++)
    //        {
    //            if (cbl_floorname.Items[i].Selected == true)
    //            {
    //                txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
    //                string flrname = cbl_floorname.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
    //                if (locfloor == "")
    //                {
    //                    locfloor = flrname;
    //                }
    //                else
    //                {
    //                    locfloor = locfloor + "'" + "," + "'" + flrname;
    //                }
    //            }

    //        }
    //        clgroom(locfloor, buildname);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cbfloorname_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_floorname.Checked == true)
    //        {
    //            string buildvalue1 = "";
    //            string build1 = "";
    //            string build2 = "";
    //            string buildvalue2 = "";

    //            if (cb_buildingname.Checked == true)
    //            {
    //                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
    //                {
    //                    build1 = cbl_buildingname.Items[i].Text.ToString();
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
    //            if (cb_floorname.Checked == true)
    //            {
    //                for (int j = 0; j < cbl_floorname.Items.Count; j++)
    //                {
    //                    cbl_floorname.Items[j].Selected = true;
    //                    txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
    //                    build2 = cbl_floorname.Items[j].Text.ToString();
    //                    if (buildvalue2 == "")
    //                    {
    //                        buildvalue2 = build2;
    //                    }
    //                    else
    //                    {
    //                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
    //                    }
    //                }
    //            }
    //            clgroom(buildvalue2, buildvalue1);
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_floorname.Items.Count; i++)
    //            {
    //                cbl_floorname.Items[i].Selected = false;
    //                txt_floorname.Text = "--Select--";
    //            }
    //            cb_roomname.Checked = false;
    //            cbl_roomname.Items.Clear();
    //            txt_roomname.Text = "--Select--";
    //        }
    //        //  Button2.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cblfloorname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int seatcount = 0;
    //        cb_floorname.Checked = false;
    //        string buildvalue1 = "";
    //        string build1 = "";
    //        string build2 = "";
    //        string buildvalue2 = "";
    //        for (int i = 0; i < cbl_buildingname.Items.Count; i++)
    //        {
    //            if (cbl_buildingname.Items[i].Selected == true)
    //            {
    //                build1 = cbl_buildingname.Items[i].Text.ToString();
    //                if (buildvalue1 == "")
    //                {
    //                    buildvalue1 = build1;
    //                }
    //                else
    //                {
    //                    buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
    //                }

    //            }
    //        }
    //        for (int i = 0; i < cbl_floorname.Items.Count; i++)
    //        {
    //            if (cbl_floorname.Items[i].Selected == true)
    //            {
    //                seatcount = seatcount + 1;
    //                build2 = cbl_floorname.Items[i].Text.ToString();
    //                if (buildvalue2 == "")
    //                {
    //                    buildvalue2 = build2;
    //                }
    //                else
    //                {
    //                    buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
    //                }
    //            }
    //        }
    //        clgroom(buildvalue2, buildvalue1);

    //        if (seatcount == cbl_floorname.Items.Count)
    //        {
    //            txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
    //            cb_floorname.Checked = true;
    //        }
    //        else if (seatcount == 0)
    //        {
    //            txt_floorname.Text = "--Select--";
    //        }
    //        else
    //        {
    //            txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
    //        }
    //        //   Button2.Focus();
    //        //  clgroom(buildvalue1, buildvalue2);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //public void clgroom(string floorname, string buildname)
    //{
    //    try
    //    {
    //        cbl_roomname.Items.Clear();
    //        ds = d2.BindRoom(floorname, buildname);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_roomname.DataSource = ds;
    //            cbl_roomname.DataTextField = "Room_Name";
    //            cbl_roomname.DataValueField = "Room_Name";
    //            cbl_roomname.DataBind();
    //        }
    //        else
    //        {
    //            txt_roomname.Text = "--Select--";
    //        }

    //        for (int i = 0; i < cbl_roomname.Items.Count; i++)
    //        {
    //            cbl_roomname.Items[i].Selected = true;
    //            txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
    //            cb_roomname.Checked = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cbroomname_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_roomname.Checked == true)
    //        {
    //            for (int i = 0; i < cbl_roomname.Items.Count; i++)
    //            {
    //                cbl_roomname.Items[i].Selected = true;
    //            }
    //            txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_roomname.Items.Count; i++)
    //            {
    //                cbl_roomname.Items[i].Selected = false;
    //            }
    //            txt_roomname.Text = "--Select--";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void cblroomname_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int seatcount = 0;
    //        cb_roomname.Checked = false;
    //        for (int i = 0; i < cbl_roomname.Items.Count; i++)
    //        {
    //            if (cbl_roomname.Items[i].Selected == true)
    //            {
    //                seatcount = seatcount + 1;
    //            }

    //        }
    //        if (seatcount == cbl_roomname.Items.Count)
    //        {
    //            txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
    //            cb_roomname.Checked = true;
    //        }
    //        else if (seatcount == 0)
    //        {
    //            txt_roomname.Text = "--Select--";
    //        }
    //        else
    //        {
    //            txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
    //        }
    //        //   Button2.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select staff_name  from staffmaster where resign =0 and settled =0  and staff_code not in (select Roll_No from Hostel_StudentDetails )  and staff_name like  '" + prefixText + "%' ";
        string query = "select staff_name  from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0 and s.appl_no = a.appl_no  and a.appl_id not in (select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )   and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffNamego(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select staff_name from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0 and s.appl_no = a.appl_no and a.appl_id in (select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0) and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and a.appl_id in(select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )  and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and a.appl_id not in(select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )  and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            DataView dv1 = new DataView();
            string sql = "";
            string hoscode = "";
            string degree = "";
            string semester = "";
            string stafftype = "";
            string designation = "";
            string buildingname = "";
            string floorname = "";
            string roomname = "";
            int index;
            string colno = "";
            int j = 0;

            //for department
            if (rdb_staffe.Checked == true)
            {
                for (int i = 0; i < cbl_department.Items.Count; i++)
                {
                    if (cbl_department.Items[i].Selected == true)
                    {
                        string degree1 = cbl_department.Items[i].Value.ToString();
                        if (degree == "")
                        {
                            degree = degree1;
                        }
                        else
                        {
                            degree = degree + "'" + "," + "'" + degree1;
                        }
                    }
                }
                //for floor
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    if (cbl_floorname.Items[i].Selected == true)
                    {
                        string floorname1 = cbl_floorname.Items[i].Value.ToString();
                        if (floorname == "")
                        {
                            floorname = floorname1;
                        }
                        else
                        {
                            floorname = floorname + "'" + "," + "'" + floorname1;
                        }
                    }
                }

                //for buildingname
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    if (cbl_buildingname.Items[i].Selected == true)
                    {
                        string buildingname1 = cbl_buildingname.Items[i].Value.ToString();
                        if (buildingname == "")
                        {
                            buildingname = buildingname1;
                        }
                        else
                        {
                            buildingname = buildingname + "'" + "," + "'" + buildingname1;
                        }
                    }
                }

                // for designation
                for (int i = 0; i < cbl_designation.Items.Count; i++)
                {
                    if (cbl_designation.Items[i].Selected == true)
                    {
                        string designation1 = cbl_designation.Items[i].Value.ToString();
                        if (designation == "")
                        {
                            designation = designation1;
                        }
                        else
                        {
                            designation = designation + "'" + "," + "'" + designation1;
                        }
                    }
                }

                //for hostelcode
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hoscode1 = cbl_hostelname.Items[i].Value.ToString();
                        if (hoscode == "")
                        {
                            hoscode = hoscode1;
                        }
                        else
                        {
                            hoscode = hoscode + "'" + "," + "'" + hoscode1;
                        }
                    }
                }

                //for staff type
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    if (cbl_stafftype.Items[i].Selected == true)
                    {
                        string stafftype1 = cbl_stafftype.Items[i].Value.ToString();
                        if (stafftype == "")
                        {
                            stafftype = stafftype1;
                        }
                        else
                        {
                            stafftype = stafftype + "'" + "," + "'" + stafftype1;
                        }
                    }
                }

                //for room
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    if (cbl_roomname.Items[i].Selected == true)
                    {
                        string roomname1 = cbl_roomname.Items[i].Value.ToString();
                        if (roomname == "")
                        {
                            roomname = roomname1;
                        }
                        else
                        {
                            roomname = roomname + "'" + "," + "'" + roomname1;
                        }
                    }
                }

                Hashtable columnhash = new Hashtable();
                columnhash.Clear();

                int colinc = 0;
                columnhash.Add("staff_code", "Staff Code");
                columnhash.Add("staff_name", "Name");
                columnhash.Add("desig_name", "Designation");
                columnhash.Add("dept_name", "Department");
                columnhash.Add("staffcategory", "Staff Type");
                columnhash.Add("Admin_Date", "Admit Date");
                columnhash.Add("HostelName", "Hostel Name");
                columnhash.Add("BuildingFK", "Building");
                columnhash.Add("FloorFK", "Floor");
                columnhash.Add("RoomFK", "Room");
                //  columnhash.Add("Room_Type", "Room Type");
                columnhash.Add("DiscontinueDate", "Discontinue");
                columnhash.Add("VacatedDate", "Vacated");
                columnhash.Add("Reason", "Reason");
                columnhash.Add("StudMessType", "StudMessType");
                columnhash.Add("id", "Staff Id");

                if (ItemList.Count == 0)
                {
                    ItemList.Add("staff_code");
                    ItemList.Add("staff_name");
                    ItemList.Add("desig_name");
                    ItemList.Add("dept_name");
                    ItemList.Add("id");
                    Fpspread1.Width = 880;

                }
                if (txt_staffcode.Text != "")
                {
                    sql = "select  hsd.APP_No,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code and  sm.staff_code='" + txt_staffcode.Text + "'  and hsd.HostelMasterFK in('" + hoscode + "') and latestrec=1 and ISNULL(IsVacated,'0')=0";//magesh 19.3.18 and latestrec=1
                }
                else if (txt_staffname.Text != "")
                {

                    sql = " hsd.APP_No,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code  and  sm.staff_name='" + txt_staffname.Text + "'  and hsd.HostelMasterFK in('" + hoscode + "') and latestrec=1 and ISNULL(IsVacated,'0')=0";//magesh 19.3.18 and latestrec=1
                }
                else
                {
                    sql = "select  hsd.APP_No,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code and hsd.BuildingFK in('" + buildingname + "') and hsd.FloorFK in('" + floorname + "') and RoomFK in('" + roomname + "')  and hsd.HostelMasterFK in('" + hoscode + "') and h.dept_code in('" + degree + "') and dm.desig_code in('" + designation + "') and dm.staffcategory in ('" + stafftype + "') and latestrec=1 and ISNULL(IsVacated,'0')=0";//magesh 19.3.18 and latestrec=1
                }
                sql = sql + " select Building_Name,Code  from Building_Master";
                sql = sql + " select Floor_Name,Floorpk  from Floor_Master";
                sql = sql + " select Room_Name,Roompk from Room_Detail";
                sql = sql + " select distinct co.MasterValue,co.MasterCode from CO_MasterValues co,HT_HostelRegistration hr,staffmaster r,staff_appl_master a where MasterCriteria='HSFVAC' and co.MasterCode=hr.Reason and a.appl_id =hr.APP_No";
                sql = sql + " select distinct co.MasterValue,co.MasterCode from CO_MasterValues co,HT_HostelRegistration hr,staffmaster r,staff_appl_master a where MasterCriteria='HSFDSC' and co.MasterCode=hr.Reason and a.appl_id =hr.APP_No";

                //select hsd.APP_No,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code

                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "TEXT");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pcolumnorder.Visible = true;
                    Divspread.Visible = true;
                    Fpspread1.Visible = true;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    //Fpspread1.Sheets[0].ColumnCount = 11;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = ItemList.Count + 1;
                    Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                    Fpspread1.Sheets[0].AutoPostBack = true;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            index = ItemList.IndexOf(Convert.ToString(colno));
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    int i;
                    int indRelDate = 0;
                    int indvacDate = 0;
                    int reason = 0;
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        Fpspread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[i, 0].Tag = ds.Tables[0].Rows[i]["APP_No"].ToString();

                        for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {

                                index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;


                                if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "DiscontinueDate")
                                {
                                    indRelDate = index + 1;
                                }
                                if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "VacatedDate")
                                {
                                    indvacDate = index + 1;
                                }
                                //if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "Reason")
                                //{
                                //    reason = index + 1;
                                //}
                                colno = Convert.ToString(ds.Tables[0].Columns[j]);
                                if (colno.Trim() != "BuildingFK")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "code in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[1].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Building_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Building_Name"]);
                                                    }
                                                }
                                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                        }
                                    }
                                }
                                if (colno.Trim() != "FloorFK")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = "Floorpk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[2].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Floor_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Floor_Name"]);
                                                    }
                                                }
                                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                        }

                                    }

                                }
                                if (colno.Trim() != "RoomFK")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[3].DefaultView.RowFilter = "Roompk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[3].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Room_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Room_Name"]);
                                                    }
                                                }
                                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                        }

                                    }

                                }
                                if (colno.Trim() != "Reason")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[4].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[4].DefaultView.RowFilter = "MasterCode in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[4].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["MasterValue"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["MasterValue"]);
                                                    }
                                                }
                                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                        }

                                    }

                                }
                                if (colno.Trim() != "Reason")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[5].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[5].DefaultView.RowFilter = "MasterCode in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[5].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["MasterValue"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["MasterValue"]);
                                                    }
                                                }
                                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    rptprint.Visible = true;
                    Fpspread1.Visible = true;
                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;
                    lbl_error.Visible = false;
                    //div1.Visible = true;
                    //lbl_error.Visible = false;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    //22.12.15 add

                    if (ItemList.Contains("DiscontinueDate"))
                    {
                        try
                        {
                            string reliveddate1 = "";
                            reliveddate1 = Convert.ToString(ds.Tables[0].Columns["DiscontinueDate"]);

                            for (int k = 0; k < Fpspread1.Rows.Count; k++)
                            {
                                reliveddate1 = Convert.ToString(ds.Tables[0].Rows[k]["DiscontinueDate"]);
                                string reliveddate = "01/01/1900";
                                if (reliveddate1 == reliveddate)
                                {
                                    Fpspread1.Sheets[0].Cells[k, indRelDate].Text = "";
                                    // Fpspread1.Sheets[0].Cells[k, indRelDate].Text = "";
                                }

                            }
                        }
                        catch { }
                    }
                    if (ItemList.Contains("VacatedDate"))
                    {
                        try
                        {
                            string vacatedate1 = "";
                            vacatedate1 = Convert.ToString(ds.Tables[0].Columns["VacatedDate"]);

                            for (int k = 0; k < Fpspread1.Rows.Count; k++)
                            {
                                vacatedate1 = Convert.ToString(ds.Tables[0].Rows[k]["VacatedDate"]);
                                string vacateddate = "01/01/1900";
                                if (vacatedate1 == vacateddate)
                                {
                                    Fpspread1.Sheets[0].Cells[k, indvacDate].Text = "";
                                }
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No records found";
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    Fpspread1.Visible = false;
                    Divspread.Visible = false;
                }
            }
            //
            if (rdb_gueste.Checked == true)
            {
                //for floor
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    if (cbl_floorname.Items[i].Selected == true)
                    {
                        string floorname1 = cbl_floorname.Items[i].Value.ToString();
                        if (floorname == "")
                        {
                            floorname = floorname1;
                        }
                        else
                        {
                            floorname = floorname + "'" + "," + "'" + floorname1;
                        }
                    }
                }

                //for buildingname
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    if (cbl_buildingname.Items[i].Selected == true)
                    {
                        string buildingname1 = cbl_buildingname.Items[i].Value.ToString();
                        if (buildingname == "")
                        {
                            buildingname = buildingname1;
                        }
                        else
                        {
                            buildingname = buildingname + "'" + "," + "'" + buildingname1;
                        }
                    }
                }

                // for designation
                //for hostelcode
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hoscode1 = cbl_hostelname.Items[i].Value.ToString();
                        if (hoscode == "")
                        {
                            hoscode = hoscode1;
                        }
                        else
                        {
                            hoscode = hoscode + "'" + "," + "'" + hoscode1;
                        }
                    }
                }

                //for staff type

                //for room
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    if (cbl_roomname.Items[i].Selected == true)
                    {
                        string roomname1 = cbl_roomname.Items[i].Value.ToString();
                        if (roomname == "")
                        {
                            roomname = roomname1;
                        }
                        else
                        {
                            roomname = roomname + "'" + "," + "'" + roomname1;
                        }
                    }
                }

                Hashtable columnhashguest = new Hashtable();
                columnhashguest.Clear();

                int colinc = 0;
                columnhashguest.Add("Admission_Date", "Admission Date");
                columnhashguest.Add("VenContactName", "Guest Name");
                columnhashguest.Add("VendorCompName", "Company Name");
                columnhashguest.Add("VenContactDesig", "Designation");
                columnhashguest.Add("VenContactDept", "Department");
                columnhashguest.Add("VendorAddress", "Address");
                columnhashguest.Add("VendorCity", "City");
                columnhashguest.Add("VendorDist", "District");
                columnhashguest.Add("VendorState", "State");
                columnhashguest.Add("VendorMobileNo", "Mobile Number");
                columnhashguest.Add("HostelName", "Hostel Name");
                columnhashguest.Add("BuildingFK", "Building");
                columnhashguest.Add("FloorFK", "Floor");
                columnhashguest.Add("RoomFK", "Room");
                //columnhashguest.Add("room_type", "Room Type");
                columnhashguest.Add("IsVacated", "Is Vacate");
                columnhashguest.Add("vacate_date", "Vacated Date");
                columnhashguest.Add("StudMessType", "Mess Type");
                columnhashguest.Add("id", "Guest Id");

                //columnhash.Add("Room_Type", "Room Type");
                //columnhash.Add("Relived_Date", "Relieved");
                // columnhashguest.Add("vacate_date", "Vacated");
                //columnhash.Add("Reason", "Reason");

                if (ItemListguest.Count == 0)
                {
                    ItemListguest.Add("Admission_Date");
                    ItemListguest.Add("VenContactName");
                    ItemListguest.Add("VendorCompName");
                    ItemListguest.Add("VenContactDesig");
                    ItemListguest.Add("id");

                }
                for (int i = 0; i <= 3; i++)
                {
                    cblcolumnorderguest.Items[i].Selected = true;
                    lnk_columnorderguest.Visible = true;
                    //tborder.Visible = true;
                }
                //  cblcolumnorderguest_SelectedIndexChanged(sender, e);

                if (hoscode.Trim() != "" && buildingname.Trim() != "" && floorname.Trim() != "" && roomname.Trim() != "")
                {
                    //sql = "select convert(varchar(10),Admission_Date ,103)as Admission_Date,Guest_Name,From_Company,Desig_Code,department,Guest_Address,Guest_City,district,State,MobileNo ,gr.Hostel_Code,hd.Hostel_Name,bm.Building_Name,Floor_Name,Room_Name,room_type,isvacate,CONVERT(varchar(10), vacate_date,103) as vacate_date,GuestCode  from Hostel_GuestReg gr,Hostel_Details hd,Building_Master bm where gr.Hostel_Code=hd.Hostel_code and bm.Building_Name=gr.Building_Name  and bm.Building_Name in('" + buildingname + "') and gr.college_code='" + ddl_collegename.SelectedItem.Value.ToString() + "' and Floor_Name in('" + floorname + "') and Room_Name in('" + roomname + "') and gr.Hostel_Code in('" + hoscode + "')  order by  Admission_Date ";
                    //sql = "select distinct convert(varchar(10),HostelAdmDate ,103)as Admission_Date,VenContactName,VendorCompName,VenContactDesig,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,gr.HostelMasterFK,hd.HostelName,BuildingFK,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No  from HT_HostelRegistration gr,HM_HostelMaster hd,CO_VendorMaster co,IM_VendorContactMaster im where gr.HostelMasterFK=hd.HostelMasterPK and co.VendorPK=im.VendorFK and im.VendorFK=gr.GuestVendorFK and gr.BuildingFK in('" + buildingname + "') and gr.FloorFK in('" + floorname + "') and gr.RoomFK in('" + roomname + "') and gr.HostelMasterFK in('" + hoscode + "')  order by  Admission_Date ";
                    //--Guest_Street,Guest_City,Guest_PinCode,Purpose,--hd.building_Code in('18')

                    sql = " select distinct convert(varchar(10),HostelAdmDate ,103)as Admission_Date,VenContactName,VendorCompName,VenContactDesig,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,gr.HostelMasterFK,hd.HostelName,BuildingFK,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK and gr.BuildingFK in('" + buildingname + "') and gr.FloorFK in('" + floorname + "') and gr.RoomFK in('" + roomname + "') and gr.HostelMasterFK in('" + hoscode + "') and isnull(IsVacated,'0')='0' order by  Admission_Date ";//and ISNULL(IsVacated,'0')=0
                    sql = sql + " select Building_Name,Code  from Building_Master";
                    sql = sql + " select Floor_Name,Floorpk  from Floor_Master";
                    sql = sql + " select Room_Name,Roompk from Room_Detail";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "TEXT");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        pcolumnorderguest.Visible = true;
                        Div4.Visible = true;
                        Fpspread2.Visible = true;
                        Fpspread2.Sheets[0].RowHeader.Visible = false;
                        //Fpspread2.Sheets[0].ColumnCount = 11;
                        Fpspread2.CommandBar.Visible = false;
                        Fpspread2.Sheets[0].RowCount = 0;
                        Fpspread2.Sheets[0].ColumnCount = 0;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread2.Sheets[0].RowHeader.Visible = false;
                        Fpspread2.Sheets[0].ColumnCount = ItemListguest.Count + 1;
                        Fpspread2.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                        Fpspread2.Sheets[0].AutoPostBack = true;

                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        int indvacDate = 0;
                        for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            colno = Convert.ToString(ds.Tables[0].Columns[j]);
                            if (ItemListguest.Contains(Convert.ToString(colno)))
                            {
                                index = ItemListguest.IndexOf(Convert.ToString(colno));
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhashguest[colno]);
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            Fpspread2.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[i, 0].Tag = ds.Tables[0].Rows[i]["APP_No"].ToString();

                            for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                if (ItemListguest.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                {
                                    index = ItemListguest.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                    Fpspread2.Sheets[0].Columns[index + 1].Width = 150;
                                    Fpspread2.Sheets[0].Columns[index + 1].Locked = true;
                                    Fpspread2.Sheets[0].Cells[i, index + 1].CellType = txt;
                                    Fpspread2.Sheets[0].Cells[i, index + 1].Font.Bold = false;
                                    Fpspread2.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                    if (ds.Tables[0].Columns[j].ToString() == "VendorDist")
                                    {
                                        string valdist = ds.Tables[0].Rows[i][j].ToString();
                                        string dist = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria = 'district' and mastercode='" + valdist + "'");
                                        Fpspread2.Sheets[0].Cells[i, index + 1].Text = dist;
                                    }

                                    else if (ds.Tables[0].Columns[j].ToString() == "VendorState")
                                    {

                                        string valstate = ds.Tables[0].Rows[i][j].ToString();
                                        string state = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria = 'State' and mastercode='" + valstate + "'");
                                        Fpspread2.Sheets[0].Cells[i, index + 1].Text = state;
                                    }
                                    else if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "vacate_date")
                                    {
                                        Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["vacate_date"]);
                                        indvacDate = index + 1;
                                    }
                                    else
                                    {
                                        Fpspread2.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                    }
                                    colno = Convert.ToString(ds.Tables[0].Columns[j]);
                                    if (colno.Trim() != "BuildingFK")
                                    {
                                    }
                                    else
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                            {
                                                ds.Tables[1].DefaultView.RowFilter = "code in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                                dv1 = ds.Tables[1].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    string buildvalue = "";
                                                    for (int r = 0; r < dv1.Count; r++)
                                                    {
                                                        if (buildvalue == "")
                                                        {
                                                            buildvalue = Convert.ToString(dv1[r]["Building_Name"]);
                                                        }
                                                        else
                                                        {
                                                            buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Building_Name"]);
                                                        }
                                                    }
                                                    Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                    Fpspread2.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                                }
                                            }

                                        }

                                    }
                                    if (colno.Trim() != "FloorFK")
                                    {
                                    }
                                    else
                                    {
                                        if (ds.Tables[2].Rows.Count > 0)
                                        {
                                            if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = "Floorpk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                                dv1 = ds.Tables[2].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    string buildvalue = "";
                                                    for (int r = 0; r < dv1.Count; r++)
                                                    {
                                                        if (buildvalue == "")
                                                        {
                                                            buildvalue = Convert.ToString(dv1[r]["Floor_Name"]);
                                                        }
                                                        else
                                                        {
                                                            buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Floor_Name"]);
                                                        }
                                                    }
                                                    Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                    Fpspread2.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                                }
                                            }

                                        }

                                    }
                                    if (colno.Trim() != "RoomFK")
                                    {
                                    }
                                    else
                                    {
                                        if (ds.Tables[3].Rows.Count > 0)
                                        {
                                            if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = "Roompk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                                dv1 = ds.Tables[3].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    string buildvalue = "";
                                                    for (int r = 0; r < dv1.Count; r++)
                                                    {
                                                        if (buildvalue == "")
                                                        {
                                                            buildvalue = Convert.ToString(dv1[r]["Room_Name"]);
                                                        }
                                                        else
                                                        {
                                                            buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Room_Name"]);
                                                        }
                                                    }
                                                    Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                    Fpspread2.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                                }
                                            }

                                        }

                                    }


                                }
                            }
                        }
                        rptprint.Visible = true;
                        Fpspread2.Visible = true;
                        pheaderfilterguest.Visible = true;
                        pcolumnorderguest.Visible = true;
                        lbl_error.Visible = false;
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        if (ItemListguest.Contains("vacate_date"))
                        {
                            try
                            {
                                string vacatedate1 = "";
                                vacatedate1 = Convert.ToString(ds.Tables[0].Columns["vacate_date"]);

                                for (int k = 0; k < Fpspread2.Rows.Count; k++)
                                {
                                    vacatedate1 = Convert.ToString(ds.Tables[0].Rows[k]["vacate_date"]);
                                    string vacateddate = "01/01/1900";
                                    if (vacatedate1 == vacateddate)
                                    {
                                        Fpspread2.Sheets[0].Cells[k, indvacDate].Text = "";
                                    }

                                }
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        rptprint.Visible = false;
                        //imgdiv2.Visible = true;
                        //lbl_erroralert.Text = "No records found";
                        //div1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No records found";
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        Fpspread2.Visible = false;
                        Div4.Visible = false;
                    }
                }
                else
                {
                    rptprint.Visible = false;
                    //imgdiv2.Visible = true;
                    //lbl_erroralert.Text = "No records found";
                    //div1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please select All Field";
                    pheaderfilterguest.Visible = false;
                    pcolumnorderguest.Visible = false;
                    Fpspread2.Visible = false;
                    Div4.Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    public string getstaffname(string staff)
    {
        string codestaff = d2.GetFunction("select Relived_Date  from Hostel_StudentDetails where Roll_No ='" + staff + "'");
        return codestaff;
    }
    public string gettextpop(string text)
    {
        string text_val = d2.GetFunction("select Vacated_Date  from Hostel_StudentDetails where Roll_No ='" + text + "'");
        return text_val;
    }
    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        popwindow1.Visible = true;
        btn_pop1save.Visible = true;
        btn_pop1exit.Visible = true;
        btn_pop1update.Visible = false;
        btn_pop1delete.Visible = false;
        btn_pop1exit1.Visible = false;
        clearpopup1();
        bindmessname();
        cb_discontinue_CheckedChanged(sender, e);
        cb_pop1vacate_CheckedChange(sender, e);
        btn_staffquestion.Enabled = true;
        btn_delguest.Visible = false;
        btn_updateguest.Visible = false;
        rdb_staff.Checked = false;
        rdb_guest.Checked = false;
        rdb_staff.Enabled = true;
        rdb_guest.Enabled = true;
        lbl_pop1collegename.Visible = false;
        ddl_pop1collegename.Visible = false;
        lbl_pop1hostelname.Visible = false;
        ddl_pop1hostelname.Visible = false;
        staff.Visible = false;
        lbl_pop1staffname.Visible = false;
        txt_pop1staffname.Visible = false;
        staffnamebtn.Visible = false;

        btn_staffquestion.Visible = false;
        lbl_pop1staffcode.Visible = false;
        txt_pop1staffcode.Visible = false;

        lbl_pop1department.Visible = false;
        txt_pop1department.Visible = false;
        lbl_pop1designation.Visible = false;

        txt_pop1designation.Visible = false;
        lbl_pop1dob.Visible = false;


        txt_pop1dob.Visible = false;
        lbl_pop1admindate.Visible = false;
        txt_pop1admindate.Visible = false;
        lbl_pop1messtype.Visible = false;
        txt_pop1roomno.Visible = false;
        roomno.Visible = false;
        btn_roomques.Visible = false;
        lbl_pop1messtype.Visible = false;

        ddlStudType.Visible = false;
        lbl_pop1building.Visible = false;
        txt_pop1building.Visible = false;

        lbl_pop1floor.Visible = false;
        txt_pop1floor.Visible = false;
        lbl_pop1roomtype.Visible = false;
        txt_pop1roomtype.Visible = false;
        lbl_pop1discontinue.Visible = false;
        cb_discontinue.Visible = false;
        lbl_pop1date.Visible = false;
        txt_discontinuedate.Visible = false;

        lbl_pop1reason.Visible = false;
        txt_pop1reason.Visible = false;
        lbl_pop1vacate.Visible = false;
        cb_pop1vacate.Visible = false;
        txt_vacatedateguest.Visible = false;
        btn_pop1save.Visible = false;
        btn_pop1exit.Visible = false;

        lbl_name4.Visible = false;
        txt_nameguest.Visible = false;
        guest.Visible = false;
        lbl_compname.Visible = false;
        txt_compname.Visible = false;
        lbl_desgn.Visible = false;
        txt_desgn.Visible = false;
        lbl_dep.Visible = false;

        txt_dep.Visible = false;
        // lbl_visit1.Visible = false;
        // txt_visit1.Visible = false;

        lbl_mno.Visible = false;
        txt_mno.Visible = false;
        //lbl_phno.Visible = false;

        //txt_phno.Visible = false;
        lbl_str.Visible = false;

        txt_str.Visible = false;
        lbl_cty.Visible = false;

        txt_cty.Visible = false;
        lbl_dis.Visible = false;
        txt_dis.Visible = false;


        lbl_stat.Visible = false;
        txt_stat.Visible = false;

        btn_saveguest.Visible = false;
        btn_exitguest.Visible = false;

        lbl_messname.Visible = false;
        ddl_messname.Visible = false;
        lbl_fromdate.Visible = false;
        txt_admindate.Visible = false;
        lbl_code.Visible = false;
        txt_code.Visible = false;
        ddlmess1.Visible = false;
        lbmess.Visible = false;
        ddlmess.Visible = true;
        Lblmess.Visible = true;
        lblid.Visible = true;
        txtid.Visible = true;
        Llid.Visible = false;
        txtid1.Visible = false;
        lbl_room.Visible = false;
        txt_room.Visible = false;
        roomnum.Visible = false;
        btn2.Visible = false;
        lbl_buildingguest.Visible = false;
        txt_building.Visible = false;
        lbl_floorguest.Visible = false;
        txt_floor.Visible = false;
        lbl_roomtype.Visible = false;
        txt_roomtype.Visible = false;

        lbl_vacate.Visible = false;
        cb_vacate.Visible = false;
        cb_vacate.Enabled = false;
        cb_vacate.Checked = false;
        lbl_vacatedate.Visible = false;
        txt_vacatedate.Visible = false;
        cb_discontinue.Enabled = false;
        cb_pop1vacate.Enabled = false;
        txt_vacatedateguest.Enabled = false;
        clear();
        bindcode();
        idgeneration();
        rdb_staff.Checked = true;
        rdb_staff_CheckedChanged(sender, e);
    }
    protected void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList[i].ToString();

                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cblcolumnorder.Items[0].Enabled = false;
                }

                tborder.Text = "";
                tborder.Visible = false;

            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {

                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {

                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }

            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }

            }
            tborder.Text = colname12;
            if (ItemList.Count == 11)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Cell_Click1(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void Fpspread_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                DataView dv1 = new DataView();
                string building = "";
                string floor = "";
                string roomnumber = "";
                lblGuestType.Visible = false;
                //rdb_NonVeg.Visible = false;
                ddlguest.Visible = false;

                popwindow1.Visible = true;
                cb_discontinue.Enabled = true;
                cb_discontinue.Checked = false;
                cb_pop1vacate.Enabled = true;
                cb_pop1vacate.Checked = false;
                txt_discontinuedate.Enabled = false;
                txt_vacatedate.Enabled = false;
                txt_pop1reason.Enabled = false;

                txt_discontinuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");


                rdb_staff.Checked = true;
                rdb_staff.Enabled = true;
                rdb_guest.Enabled = false;
                rdb_guest.Checked = false;
                lbl_pop1collegename.Visible = true;
                ddl_pop1collegename.Visible = true;
                lbl_pop1hostelname.Visible = true;
                ddl_pop1hostelname.Visible = true;
                staff.Visible = true;
                lbl_pop1staffname.Visible = true;
                txt_pop1staffname.Visible = true;
                staffnamebtn.Visible = true;

                btn_staffquestion.Visible = true;
                lbl_pop1staffcode.Visible = true;
                txt_pop1staffcode.Visible = true;

                lbl_pop1department.Visible = true;
                txt_pop1department.Visible = true;
                lbl_pop1designation.Visible = true;

                txt_pop1designation.Visible = true;
                lbl_pop1dob.Visible = true;


                txt_pop1dob.Visible = true;
                lbl_pop1admindate.Visible = true;
                txt_pop1admindate.Visible = true;
                lbl_pop1roomno.Visible = true;
                txt_pop1roomno.Visible = true;
                roomno.Visible = true;
                btn_roomques.Visible = true;
                lbl_pop1messtype.Visible = true;

                ddlStudType.Visible = true;
                lbl_pop1building.Visible = true;
                txt_pop1building.Visible = true;

                lbl_pop1floor.Visible = true;
                txt_pop1floor.Visible = true;
                lbl_pop1roomtype.Visible = true;
                txt_pop1roomtype.Visible = true;
                lbl_pop1discontinue.Visible = true;
                cb_discontinue.Visible = true;
                lbl_pop1date.Visible = true;
                txt_discontinuedate.Visible = true;

                lbl_pop1reason.Visible = true;
                txt_pop1reason.Visible = true;
                lbl_pop1vacate.Visible = true;
                cb_pop1vacate.Visible = true;
                txt_vacatedate.Visible = true;
                btn_pop1save.Visible = true;
                btn_pop1exit.Visible = true;
                btn_pop1save.Visible = false;
                btn_pop1exit.Visible = false;
                btn_updateguest.Visible = false;
                btn_delguest.Visible = false;
                btn_exitguest.Visible = false;
                btn_pop1update.Visible = true;
                btn_pop1delete.Visible = true;
                btn_pop1exit1.Visible = true;

                lbl_name4.Visible = false;
                txt_nameguest.Visible = false;
                guest.Visible = false;
                lbl_compname.Visible = false;
                txt_compname.Visible = false;
                lbl_desgn.Visible = false;
                txt_desgn.Visible = false;
                lbl_dep.Visible = false;

                txt_dep.Visible = false;
                // lbl_visit1.Visible = false;
                // txt_visit1.Visible = false;

                lbl_mno.Visible = false;
                txt_mno.Visible = false;
                //lbl_phno.Visible = false;

                //txt_phno.Visible = false;
                lbl_str.Visible = false;

                txt_str.Visible = false;
                lbl_cty.Visible = false;

                txt_cty.Visible = false;
                lbl_dis.Visible = false;
                txt_dis.Visible = false;

                lbl_stat.Visible = false;
                txt_stat.Visible = false;

                btn_saveguest.Visible = false;
                btn_exitguest.Visible = false;

                lbl_messname.Visible = false;
                ddl_messname.Visible = false;
                lbl_fromdate.Visible = false;
                txt_admindate.Visible = false;
                lbl_code.Visible = false;
                txt_code.Visible = false;
                ddlmess1.Visible = false;
                lbmess.Visible = false;
                ddlmess.Visible = true;
                Lblmess.Visible = true;
                lblid.Visible = true;
                txtid.Visible = true;
                Llid.Visible = false;
                txtid1.Visible = false;
                lbl_room.Visible = false;
                txt_room.Visible = false;
                roomnum.Visible = false;
                btn2.Visible = false;
                lbl_buildingguest.Visible = false;
                txt_building.Visible = false;
                lbl_floorguest.Visible = false;
                txt_floor.Visible = false;
                lbl_roomtype.Visible = false;
                txt_roomtype.Visible = false;

                lbl_vacate.Visible = false;
                cb_vacate.Visible = false;
                lbl_vacatedate.Visible = false;
                txt_vacatedateguest.Visible = false;
                btn_staffquestion.Enabled = false;

                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                //  collegecode1 = Session["collegecode"].ToString();
                if (activerow.Trim() != "" && activecol != "0")
                {
                    string staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                    // txt_pop1staffcode.Text = staffcode;
                    string collegecode = d2.GetFunction("select distinct hm.CollegeCode from HT_HostelRegistration ht,HM_HostelMaster hm where ht.APP_No='" + staffcode + "'");
                    loadcollegepopup();
                    ddl_pop1collegename.SelectedIndex = ddl_pop1collegename.Items.IndexOf(ddl_pop1collegename.Items.FindByValue(collegecode));

                    string hostelcode = d2.GetFunction("select HostelMasterFK from HT_HostelRegistration where APP_No='" + staffcode + "'");
                    loadhostelpopup();
                    ddl_pop1hostelname.SelectedIndex = ddl_pop1hostelname.Items.IndexOf(ddl_pop1hostelname.Items.FindByValue(hostelcode));

                    //string sql = "select Vacated,Relived,convert(varchar,convert(datetime,a.date_of_birth,103),103) as 'date_of_birth',hsd.StudMess_Type,hsd.Roll_No,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.Admin_Date,103),103) as 'Admin_Date',hd.Hostel_Name,hsd.Building_Name,hsd.Floor_Name,hsd.Room_Name,hsd.Room_Type,convert(varchar,convert(datetime,hsd.Relived_Date,103),103) as 'Relived_Date',convert(varchar,convert(datetime,hsd.Vacated_Date,103),103) as 'Vacated_Date',hsd.Reason from Hostel_StudentDetails hsd,staffmaster sm,Hostel_Details hd,desig_master dm,hrdept_master h,staff_appl_master a where hsd.Roll_No=sm.staff_code and hsd.Hostel_Code=hd.Hostel_code and a.appl_no =sm.appl_no and h.dept_code =a.dept_code and dm.desig_code =a.desig_code and settled=0 and resign =0 and hsd.Is_Staff=1 and hsd.Roll_No='" + staffcode + "'";
                    sql = "select IsVacated,IsDiscontinued,convert(varchar,convert(datetime,a.date_of_birth,103),103) as 'date_of_birth',hsd.StudMessType,hsd.APP_No,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.FloorFK,hsd.RoomFK,convert(varchar,convert(datetime,hsd.DiscontinueDate,103),103) as 'DiscontinueDate',convert(varchar,convert(datetime,hsd.VacatedDate,103),103) as 'VacatedDate',hsd.Reason,hsd.Messcode,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a where hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =a.dept_code and dm.desig_code =a.desig_code and settled=0 and resign =0 and hsd.MemType=2 and hsd.APP_No='" + staffcode + "'";
                    sql = sql + " select Building_Name,Code  from Building_Master";
                    sql = sql + " select Floor_Name,Floorpk  from Floor_Master";
                    sql = sql + " select Room_Name,Roompk from Room_Detail";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txt_pop1staffname.Text = Convert.ToString(ds.Tables[0].Rows[0]["Staff_Name"]);
                        txt_pop1staffcode.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"]);
                        txt_pop1department.Text = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"]);
                        txt_pop1designation.Text = Convert.ToString(ds.Tables[0].Rows[0]["staffcategory"]);
                        txt_pop1dob.Text = Convert.ToString(ds.Tables[0].Rows[0]["date_of_birth"]);
                        //    txt_pop1floor.Text = Convert.ToString(ds.Tables[0].Rows[0]["Floor_Name"]);
                        // txt_pop1reason.Text = Convert.ToString(ds.Tables[0].Rows[0]["Reason"]);
                        // txt_pop1roomno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]);
                        //  txt_pop1roomtype.Text = Convert.ToString(ds.Tables[0].Rows[0]["Room_Type"]);
                        // txt_pop1building.Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Name"]);
                        txt_pop1admindate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Admin_Date"]);
                        txt_discontinuedate.Text = Convert.ToString(ds.Tables[0].Rows[0]["DiscontinueDate"]);
                        txt_vacatedate.Text = Convert.ToString(ds.Tables[0].Rows[0]["VacatedDate"]);
                        txtid.Text =  Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                        int messtype = Convert.ToInt16(ds.Tables[0].Rows[0]["StudMessType"]);
                        ddlStudType.SelectedIndex = ddlStudType.Items.IndexOf(ddlStudType.Items.FindByValue(Convert.ToString(messtype + 1)));
                        ddl_pop1hostelname_SelectedIndexChanged(sender, e);
                        ddlmess.SelectedIndex = ddlmess.Items.IndexOf(ddlmess.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Messcode"])));
                        //if (messtype == 0)
                        //{
                        //    rbl_messtype.Items[0].Selected = true;
                        //    rbl_messtype.Items[1].Selected = false;
                        //}
                        //else if (messtype == 1)
                        //{
                        //    rbl_messtype.Items[0].Selected = false;
                        //    rbl_messtype.Items[1].Selected = true;
                        //}
                        building = ds.Tables[0].Rows[0]["BuildingFK"].ToString();
                        ViewState["Code"] = Convert.ToString(building);
                        if (building != "")
                        {
                            ds.Tables[1].DefaultView.RowFilter = "Code in (" + building + ")";
                            dv1 = ds.Tables[1].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int row = 0; row < dv1.Count; row++)
                                {
                                    build1 = Convert.ToString(dv1[row]["Building_Name"]);
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
                            txt_pop1building.Text = Convert.ToString(buildvalue1);
                        }
                        floor = ds.Tables[0].Rows[0]["FloorFK"].ToString();
                        ViewState["Floorpk"] = Convert.ToString(floor);
                        if (floor != "")
                        {
                            string build2 = "";
                            string buildvalue2 = "";
                            ds.Tables[2].DefaultView.RowFilter = "Floorpk in (" + floor + ")";
                            dv1 = ds.Tables[2].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int row = 0; row < dv1.Count; row++)
                                {
                                    build2 = Convert.ToString(dv1[row]["Floor_Name"]);
                                    if (buildvalue2 == "")
                                    {
                                        buildvalue2 = build2;
                                    }
                                    else
                                    {
                                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                                    }
                                }
                            }
                            txt_pop1floor.Text = Convert.ToString(buildvalue2);
                        }
                        roomnumber = ds.Tables[0].Rows[0]["RoomFK"].ToString();
                        ViewState["Roompk"] = Convert.ToString(roomnumber);
                        if (roomnumber != "")
                        {
                            string build3 = "";
                            string buildvalue3 = "";
                            ds.Tables[3].DefaultView.RowFilter = "Roompk in (" + roomnumber + ")";
                            dv1 = ds.Tables[3].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int row = 0; row < dv1.Count; row++)
                                {
                                    build3 = Convert.ToString(dv1[row]["Room_Name"]);
                                    if (buildvalue3 == "")
                                    {
                                        buildvalue3 = build3;
                                    }
                                    else
                                    {
                                        buildvalue3 = buildvalue3 + "'" + "," + "'" + build3;
                                    }
                                }
                            }
                            txt_pop1roomno.Text = Convert.ToString(buildvalue3);
                        }
                        string vacate = "";
                        vacate = Convert.ToString(ds.Tables[0].Rows[0]["IsVacated"]);
                        if (vacate != "Null" && vacate != "True")
                        {
                            cb_pop1vacate.Checked = false;
                            txt_vacatedate.Enabled = false;


                            //txt_pop1reason.Enabled = true;
                        }
                        else
                        {
                            cb_pop1vacate.Checked = true;
                            txt_vacatedate.Enabled = true;

                            // txt_pop1reason.Enabled = false;
                        }

                        string discon = "";
                        discon = Convert.ToString(ds.Tables[0].Rows[0]["IsDiscontinued"]);
                        if (discon != "Null" && discon != "True")
                        {
                            cb_discontinue.Checked = false;
                            txt_discontinuedate.Enabled = false;
                            // txt_pop1reason.Enabled = false;

                        }
                        else
                        {

                            cb_discontinue.Checked = true;
                            txt_discontinuedate.Enabled = true;
                            //  txt_pop1reason.Enabled = true;
                        }
                    }
                    string room = "select Room_Name,Room_type from Room_Detail where Room_Name='" + txt_pop1roomno.Text + "'";
                    ds = d2.select_method_wo_parameter(room, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string roomtype = ds.Tables[0].Rows[0]["Room_type"].ToString();
                        txt_pop1roomtype.Text = roomtype;
                    }
                    if (cb_discontinue.Checked == true)
                    {
                        string discontxt = "select co.MasterValue from CO_MasterValues co,HT_HostelRegistration hr,staffmaster r,staff_appl_master a where MasterCriteria='HSFDSC' and co.MasterCode=hr.Reason and a.appl_id =hr.APP_No and r.staff_code='" + txt_pop1staffcode.Text + "'";
                        ds = d2.select_method_wo_parameter(discontxt, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            string dis = ds.Tables[0].Rows[0]["MasterValue"].ToString();
                            txt_pop1reason.Text = dis;
                            txt_pop1reason.Enabled = true;

                        }
                    }
                    //else
                    //{
                    //    txt_pop1reason.Text = "";
                    //    txt_pop1reason.Enabled = false;
                    //}

                    if (cb_pop1vacate.Checked == true)
                    {
                        string vactxt = "select co.MasterValue from CO_MasterValues co,HT_HostelRegistration hr,staffmaster r,staff_appl_master a where MasterCriteria='HSFVAC' and co.MasterCode=hr.Reason and a.appl_id =hr.APP_No and r.staff_code='" + txt_pop1staffcode.Text + "'";
                        ds = d2.select_method_wo_parameter(vactxt, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            string vac = ds.Tables[0].Rows[0]["MasterValue"].ToString();
                            txt_pop1reason.Text = vac;
                            txt_pop1reason.Enabled = true;

                        }
                    }
                    //else
                    //{
                    //    txt_pop1reason.Text = "";
                    //    txt_pop1reason.Enabled = false;
                    //}
                }
                else
                {
                    popwindow1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Select any column from column order";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void imagebtnpop1close_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    public void loadcollegepopup()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_pop1collegename.DataSource = ds;
                ddl_pop1collegename.DataTextField = "collname";
                ddl_pop1collegename.DataValueField = "college_code";
                ddl_pop1collegename.DataBind();
            }
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch
        {
        }
    }
    public void loadhostelpopup()
    {
        try
        {
            ds.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            //string itemname = "select HostelMasterPK ,HostelName  from HM_HostelMaster  order by HostelMasterPK ";
            //ds = d2.select_method_wo_parameter(itemname, "Text");
            //magesh 21.6.18
            MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster  order by hostelname";
            ds = d2.select_method_wo_parameter(MessmasterFK, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_pop1hostelname.DataSource = ds;
                ddl_pop1hostelname.DataTextField = "HostelName";
                ddl_pop1hostelname.DataValueField = "HostelMasterPK";
                ddl_pop1hostelname.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    
    protected void btn_staff_question_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = true;
        Fpstaff.Visible = false;
        div1.Visible = false;
        btn_staffsave.Visible = false;
        btn_staffexit.Visible = false;
        lbl_errorsearch1.Text = "";
        txt_staffcodesearch.Text = "";
        txt_staffnamesearch.Text = "";
        bindstaffdepartmentpopup();
        lbl_errorsearch.Visible = false;

    }
    protected void btn_roomques_Click(object sender, EventArgs e)
    {

        bindroompopbuild();
        roomlookup.Visible = true;

        lblpop3err.Visible = false;
        search();

    }
    protected void rbl_messtype_RadiobtnChanged(object sender, EventArgs e)
    {
    }
    protected void cb_discontinue_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_discontinue.Checked == true)
        {
            txt_discontinuedate.Enabled = true;
            txt_pop1reason.Enabled = true;
            //21.12.15 add
            txt_discontinuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        else
        {
            txt_discontinuedate.Enabled = false;
            txt_pop1reason.Enabled = false;
        }
    }
    protected void cb_pop1vacate_CheckedChange(object sender, EventArgs e)
    {
        if (cb_pop1vacate.Checked == true)
        {
            txt_vacatedate.Enabled = true;
            //21.12.15 add
            txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pop1reason.Enabled = true;
        }
        else
        {
            txt_vacatedate.Enabled = false;
            txt_pop1reason.Enabled = false;
        }
    }
    protected void btn_pop1save_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string studmesstype = string.Empty;
            string staffcode = d2.GetFunction("select staff_code from staffmaster where staff_name='" + txt_pop1staffname.Text + "'");
            DateTime admindate = new DateTime();
            string firstdate = Convert.ToString(txt_pop1admindate.Text);

            string[] split = firstdate.Split('/');
            admindate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string building = "";
            string floor = "";
            string roomno = "";
            string applid = "";
            building = Convert.ToString(ViewState["Code"]);
            floor = Convert.ToString(ViewState["Floorpk"]);
            roomno = Convert.ToString(ViewState["Roompk"]);
            applid = Convert.ToString(ViewState["appl_id"]);
            //magesh 12.3.18
            int messtype = 0;
            int.TryParse(Convert.ToString(ddlStudType.SelectedValue), out messtype);
            studmesstype = Convert.ToString(messtype - 1);
            //string sql = "if exists (select * from HT_HostelRegistration where APP_No='" + applid + "' and MemType=2) update HT_HostelRegistration set HostelAdmDate='" + admindate + "',BuildingFK='" + building + "',FloorFK='" + floor + "',RoomFK='" + roomno + "',HostelMasterFK='" + ddl_pop1hostelname.SelectedItem.Value.ToString() + "',IsSuspend='0',IsVacated='0',VacatedDate='',SuspendDate='',StudMessType='" + rbl_messtype.SelectedItem.Value.ToString() + "',IsDiscontinued='0',DiscontinueDate='' where APP_No='" + applid + "' and MemType=2 else insert into HT_HostelRegistration(MemType,APP_No,HostelAdmDate,BuildingFK,FloorFK,RoomFK,IsVacated,VacatedDate,HostelMasterFK,IsSuspend,SuspendDate,StudMessType,IsDiscontinued,DiscontinueDate)  values('2','" + applid + "','" + admindate + "','" + building + "','" + floor + "','" + roomno + "','0','','" + ddl_pop1hostelname.SelectedItem.Value.ToString() + "','0','','" + rbl_messtype.SelectedItem.Value.ToString() + "','0','')";
            
            string sql = "if exists (select * from HT_HostelRegistration where APP_No='" + applid + "' and MemType=2) update HT_HostelRegistration set HostelAdmDate='" + admindate + "',BuildingFK='" + building + "',FloorFK='" + floor + "',RoomFK='" + roomno + "',HostelMasterFK='" + ddl_pop1hostelname.SelectedItem.Value.ToString() + "',IsSuspend='0',IsVacated='0',VacatedDate='',SuspendDate='',StudMessType='" + studmesstype + "',IsDiscontinued='0',DiscontinueDate='',Messcode='" + Convert.ToString(ddlmess.SelectedValue) + "',id='"+txtid.Text+"' where APP_No='" + applid + "' and MemType=2 else insert into HT_HostelRegistration(MemType,APP_No,HostelAdmDate,BuildingFK,FloorFK,RoomFK,IsVacated,VacatedDate,HostelMasterFK,IsSuspend,SuspendDate,StudMessType,IsDiscontinued,DiscontinueDate,Messcode,id)  values('2','" + applid + "','" + admindate + "','" + building + "','" + floor + "','" + roomno + "','0','','" + ddl_pop1hostelname.SelectedItem.Value.ToString() + "','0','','" + studmesstype + "','0','','" + Convert.ToString(ddlmess.SelectedValue) + "','"+txtid.Text+"')";
            int query = d2.update_method_wo_parameter(sql, "TEXT");
            if (query > 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Saved Successfully";
                clearpopup1();
                idgeneration();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_pop1exit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    public string dis(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {

        }
        return subjec_no;
    }
    public string vac(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {

        }
        return subjec_no;
    }
    protected void btn_pop1update_Click(object sender, EventArgs e)
    {
        try
        {
            string vecated = "";
            string discontinue = "";
            string getdaydiscon = "";
            string getdayvacate = "";
            string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string staffcode = d2.GetFunction("select staff_code from staffmaster where staff_name='" + txt_pop1staffname.Text + "'");

            string getdate = Convert.ToString(txt_pop1admindate.Text);
            string[] splitdate = getdate.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();

            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string admindate = dt.ToString("MM/dd/yyyy");

            if (cb_pop1vacate.Checked == true)
            {
                vecated = "1";
                string vacatedate = Convert.ToString(txt_vacatedate.Text);
                string[] splitdatevacate = vacatedate.Split('-');
                splitdatevacate = splitdatevacate[0].Split('/');
                DateTime dtvacate = new DateTime();
                if (splitdatevacate.Length > 0)
                {
                    dtvacate = Convert.ToDateTime(splitdatevacate[1] + "/" + splitdatevacate[0] + "/" + splitdatevacate[2]);
                }
                getdayvacate = dtvacate.ToString("MM/dd/yyyy");
            }
            else
            {
                vecated = "0";
                getdayvacate = "";
                //  txt_pop1reason.Text = "";
            }
            if (cb_discontinue.Checked == true)
            {
                discontinue = "1";
                string discondate = Convert.ToString(txt_discontinuedate.Text);
                string[] splitdatediscon = discondate.Split('-');
                splitdatediscon = splitdatediscon[0].Split('/');
                DateTime dtdiscon = new DateTime();
                if (splitdatediscon.Length > 0)
                {
                    dtdiscon = Convert.ToDateTime(splitdatediscon[1] + "/" + splitdatediscon[0] + "/" + splitdatediscon[2]);
                }
                getdaydiscon = dtdiscon.ToString("MM/dd/yyyy");
            }
            else
            {
                discontinue = "0";
                getdaydiscon = "";
                // txt_pop1reason.Text = "";
            }
            //magesh 12.3.18
            string studmesstype = string.Empty;
            int messtype = 0;
            int.TryParse(Convert.ToString(ddlStudType.SelectedValue), out messtype);
            studmesstype = Convert.ToString(messtype - 1);//magesh 12.3.18
            string sql = "";
            string building = "";
            string floor = "";
            string roomno = "";
            string applid = "";
            building = Convert.ToString(ViewState["Code"]);
            floor = Convert.ToString(ViewState["Floorpk"]);
            roomno = Convert.ToString(ViewState["Roompk"]);
            //applid = Convert.ToString(ViewState["appl_id"]);
            applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + staffcode + "' and sam.appl_no = sm.appl_no");
            ViewState["appl_id"] = Convert.ToString(applid);
            if (cb_discontinue.Checked == true)
            {
                string reasonds = "";
                string DC = "HSFDSC";
                string reasoncode = "";


                reasonds = Convert.ToString(txt_pop1reason.Text);
                reasoncode = dis(DC, reasonds);



                sql = "update HT_HostelRegistration set IsDiscontinued='" + discontinue + "',DiscontinueDate='" + getdaydiscon + "',Reason='" + reasoncode + "' where HostelMasterFK='" + ddl_pop1hostelname.SelectedValue + "' and APP_No ='" + applid + "' ";
                int query = d2.update_method_wo_parameter(sql, "TEXT");
                if (query > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Updated Successfully";
                    popwindow1.Visible = false;
                    btn_go_Click(sender, e);

                    //clearpopup1();
                }
            }
            else if (cb_pop1vacate.Checked == true)
            {
                string reasonds = "";
                string DC = "HSFVAC";
                string reasoncode = "";


                reasonds = Convert.ToString(txt_pop1reason.Text);
                reasoncode = vac(DC, reasonds);


                sql = "update HT_HostelRegistration set IsVacated='1',VacatedDate='" + getdayvacate + "',Reason='" + reasoncode + "' where HostelMasterFK='" + ddl_pop1hostelname.SelectedValue + "' and APP_No ='" + applid + "' ";
                int query = d2.update_method_wo_parameter(sql, "TEXT");
                if (query > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Updated Successfully";
                    popwindow1.Visible = false;

                    btn_go_Click(sender, e);
                    //clearpopup1();
                }
            }


            else
            {
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                string rollnum = "";

                rollnum = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);

                //string q1 = "select distinct r.Building_Name,r.Floor_Name,r.Room_Name,r.Room_type from Room_Detail r,Hostel_StudentDetails hd where hd.Roll_No='" + rollnum + "' and r.Building_Name=hd.Building_Name and r.Floor_Name=hd.Floor_Name and r.Room_Name=hd.Room_Name and r.Room_type=hd.Room_Type";
                string q1 = " select distinct r.Building_Name,r.Floor_Name,r.Room_Name,r.Room_type from Room_Detail r,HT_HostelRegistration hd,Floor_Master fm,Building_Master bm where hd.App_No='" + applid + "' and r.Roompk=hd.RoomFK and fm.Floorpk=hd.FloorFK and bm.Code=hd.BuildingFK";
                ds3.Clear();
                ds3 = d2.select_method_wo_parameter(q1, "text");
                string bulname = "";
                bulname = Convert.ToString(ds3.Tables[0].Rows[0]["Building_Name"].ToString());
                string flrname = "";
                flrname = Convert.ToString(ds3.Tables[0].Rows[0]["Floor_Name"].ToString());
                string roomname = "";
                roomname = Convert.ToString(ds3.Tables[0].Rows[0]["Room_Name"].ToString());
                string roomtype = "";
                roomtype = Convert.ToString(ds3.Tables[0].Rows[0]["Room_type"].ToString());
                string upalavl = " update Room_Detail set Avl_Student= Avl_Student - 1 where Room_type='" + roomtype + "' and Floor_Name='" + flrname + "' and Room_Name='" + roomname + "' and Building_Name='" + bulname + "'";
                int kalavl = d2.update_method_wo_parameter(upalavl, "text");

                string up = " update Room_Detail set Avl_Student= Avl_Student + 1 where Room_type='" + txt_pop1roomtype.Text + "' and Floor_Name='" + txt_pop1floor.Text + "' and Room_Name='" + txt_pop1roomno.Text + "' and Building_Name='" + txt_pop1building.Text + "'";
                int k = d2.update_method_wo_parameter(up, "text");

                //sql = "if exists (select * from Hostel_StudentDetails where Roll_No='" + staffcode + "' and Is_Staff=1) update Hostel_StudentDetails set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "',Admin_Date='" + admindate + "',Building_Name='" + txt_pop1building.Text + "',Floor_Name='" + txt_pop1floor.Text + "',Room_Name='" + txt_pop1roomno.Text + "',Relived='0',Relived_Date='',[Status]='0',Hostel_Code='" + ddl_pop1hostelname.SelectedItem.Value.ToString() + "',isNew='0',College_Code='" + ddl_pop1collegename.SelectedItem.Value.ToString() + "',Room_Type='" + txt_pop1roomtype.Text + "',Suspension='0',Vacated='0',Vacated_Date='',StudMess_Type='" + rbl_messtype.SelectedItem.Value.ToString() + "' where Roll_No='" + staffcode + "' and Is_Staff=1 else insert into Hostel_StudentDetails(Access_Date,Access_Time,Is_Staff,Roll_No,Admin_Date,Building_Name,Floor_Name,Room_Name,Relived,Relived_Date,Reason,[Status],Hostel_Code,isNew,College_Code,Room_Type,Suspension,Vacated,Vacated_Date,StudMess_Type)  values('" + dtaccessdate + "','" + dtaccesstime + "','1','" + staffcode + "','" + admindate + "','" + txt_pop1building.Text + "','" + txt_pop1floor.Text + "','" + txt_pop1roomno.Text + "','0','','','0','" + ddl_pop1hostelname.SelectedItem.Value.ToString() + "','0','" + ddl_pop1collegename.SelectedItem.Value.ToString() + "','" + txt_pop1roomtype.Text + "','0','0','','" + rbl_messtype.SelectedItem.Value.ToString() + "')";
                sql = "if exists (select * from HT_HostelRegistration where APP_No='" + applid + "' and MemType=2) update HT_HostelRegistration set HostelAdmDate='" + admindate + "',BuildingFK='" + building + "',FloorFK='" + floor + "',RoomFK='" + roomno + "',HostelMasterFK='" + ddl_pop1hostelname.SelectedItem.Value.ToString() + "',StudMessType='" + studmesstype + "',Messcode='" + Convert.ToString(ddlmess.SelectedValue) + "',id='"+txtid.Text+"' where APP_No='" + applid + "' and MemType=2 else insert into HT_HostelRegistration(MemType,APP_No,HostelAdmDate,BuildingFK,FloorFK,RoomFK,HostelMasterFK,StudMessType,Messcode,id)  values('2','" + applid + "','" + admindate + "','" + building + "','" + floor + "','" + roomno + "','" + ddl_pop1hostelname.SelectedItem.Value.ToString() + "','" + studmesstype + "','" + Convert.ToString(ddlmess.SelectedValue) + "','" + txtid.Text + "')";
                int query = d2.update_method_wo_parameter(sql, "TEXT");
                if (query > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Updated Successfully";

                    popwindow1.Visible = false;
                    btn_go_Click(sender, e);
                    //clearpopup1();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public string subjectcodenew(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {

        }
        return subjec_no;
    }
    protected void btn_pop1exit1_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    public void loadcollegestaffpopup()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college2.DataSource = ds;
                ddl_college2.DataTextField = "collname";
                ddl_college2.DataValueField = "college_code";
                ddl_college2.DataBind();
            }
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch
        {
        }
    }
    protected void ddl_college2_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            bindstaffdepartmentpopup();
        }
        catch
        {
        }
    }
    public void bindstaffdepartmentpopup()
    {
        try
        {
            ds.Clear();
            //string query = "";
            //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode1 + "'";
            string clgcode = "";
            if (ddl_college2.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_college2.SelectedItem.Value);
            }
            ds = d2.loaddepartment(clgcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_department3.DataSource = ds;
                ddl_department3.DataTextField = "dept_name";
                ddl_department3.DataValueField = "dept_code";
                ddl_department3.DataBind();

                ddl_department3.Items.Insert(0, "All");
            }

        }
        catch { }
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = false;
    }
    protected void ddl_searchbystaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_searchbystaff.SelectedItem.Text == "Staff Name")
        {
            txt_staffnamesearch.Visible = true;
            txt_staffcodesearch.Visible = false;
            txt_staffnamesearch.Text = "";

        }
        else if (ddl_searchbystaff.SelectedItem.Text == "Staff Code")
        {
            txt_staffcodesearch.Visible = true;
            txt_staffnamesearch.Visible = false;
            txt_staffnamesearch.Text = "";
        }
    }
    protected void btn_staffselectgo_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            int sno = 0;
            string sql = "";
            int rowcount;
            //Fpstaff.Visible = true;
            if (txt_staffnamesearch.Text != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 0)
                {
                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode=a.college_code and collegeCode='" + Convert.ToString(ddl_college2.SelectedItem.Value) + "' and s.Staff_name ='" + Convert.ToString(txt_staffnamesearch.Text) + "'   and a.appl_id not in (select app_no from HT_HostelRegistration  where MemType=2 and ISNULL(app_no,0)<>0) order by s.staff_code";
                }
            }
            else if (txt_staffcodesearch.Text.Trim() != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 1)
                {
                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode=a.college_code and collegeCode='" + Convert.ToString(ddl_college2.SelectedItem.Value) + "' and s.staff_code ='" + Convert.ToString(txt_staffcodesearch.Text) + "'  and a.appl_id not in (select app_no from HT_HostelRegistration  where MemType=2 and ISNULL(app_no,0)<>0) order by s.staff_code";
                }
            }
            else
            {
                if (ddl_department3.SelectedItem.Text == "All")
                {
                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode=a.college_code and collegeCode='" + Convert.ToString(ddl_college2.SelectedItem.Value) + "' and a.appl_id not in (select app_no from HT_HostelRegistration  where MemType=2 and ISNULL(app_no,0)<>0) order by s.staff_code";
                    //and a.appl_id not in (select app_no from HT_HostelRegistration  ) order by s.staff_code";
                }
                else
                {
                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code  and d.collegeCode=a.college_code and collegeCode='" + Convert.ToString(ddl_college2.SelectedItem.Value) + "' and h.dept_code in ('" + ddl_department3.SelectedItem.Value + "')  and a.appl_id not in (select app_no from HT_HostelRegistration  where MemType=2 and ISNULL(app_no,0)<>0)  order by s.staff_code";
                }
            }
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.SaveChanges();
            Fpstaff.SheetCorner.ColumnCount = 0;
            Fpstaff.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
            Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpstaff.Sheets[0].AutoPostBack = false;
            ds = d2.select_method_wo_parameter(sql, "Text");
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 5;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[0].Locked = true;
                Fpstaff.Columns[0].Width = 80;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[1].Locked = true;
                Fpstaff.Columns[1].Width = 100;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[2].Locked = true;
                Fpstaff.Columns[2].Width = 200;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[3].Locked = true;
                Fpstaff.Columns[3].Width = 250;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpstaff.Columns[4].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Width = 700;

                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    //Fpstaff.Sheets[0].RowCount++;
                    //name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    //code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();

                    Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
                    //Fpstaff.Sheets[0].Rows[Fpstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["appl_id"]);

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                // lbl_errorsearch.Visible = true;
                // lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch1.Visible = true;
                lbl_errorsearch1.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 345;
                Fpstaff.Width = 846;
                btn_staffsave.Visible = true;
                btn_staffexit.Visible = true;
                Fpstaff.Visible = true;
                div1.Visible = true;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();

            }
            else
            {
                Fpstaff.Visible = false;
                btn_staffsave.Visible = false;
                btn_staffexit.Visible = false;
                div1.Visible = false;
                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch1.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_staffsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_staffcodesearch.Text != "" || txt_staffnamesearch.Text != "" || ddl_searchbystaff.SelectedIndex != -1)
            {
                if (Fpstaff.Visible == true)
                {
                    popwindow1.Visible = true;
                    popupstaffcode1.Visible = false;
                    btn_pop1save.Visible = true;
                    btn_pop1exit.Visible = true;
                    btn_pop1update.Visible = false;
                    btn_pop1delete.Visible = false;
                    btn_pop1exit1.Visible = false;
                    string activerow = "";
                    string activecol = "";
                    string sql = "";
                    activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
                    string applid = "";
                    // if (activerow.Trim() != "")
                    //19.12.15 modify
                    if (activerow != Convert.ToString(-1))
                    {
                        //Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor = Color.DarkCyan;
                        string StaffCode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                        string applno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + StaffCode + "'");
                        applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + StaffCode + "' and sam.appl_no = sm.appl_no");
                        ViewState["appl_id"] = Convert.ToString(applid);
                        sql = "select convert(varchar,convert(datetime,date_of_birth,103),103) from staff_appl_master where appl_no='" + applno + "'";
                        string StaffDob = d2.GetFunction(sql);
                        string StaffName = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                        string StaffDepartment = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                        string StaffDesignation = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);

                        txt_pop1staffname.Text = StaffName;
                        txt_pop1staffcode.Text = StaffCode;
                        txt_pop1department.Text = StaffDepartment;
                        txt_pop1designation.Text = StaffDesignation;
                        txt_pop1dob.Text = StaffDob;
                    }
                    else
                    {
                        lbl_errorsearch.Visible = true;
                        lbl_errorsearch.Text = "Please Select Any Staff Name";
                        //  imgdiv2.Visible = true;
                        // lbl_erroralert.Text = "Please Select Any Staff Name";
                        popupstaffcode1.Visible = true;
                    }
                    txt_staffcodesearch.Text = "";
                    txt_staffnamesearch.Text = "";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "No records found";
                }
            }



        }
        catch (Exception ex)
        {
        }
    }
    protected void txt_pop1staffname_Text_Changed(object sender, EventArgs e)
    {
        string applid = "";
        string staffname = Convert.ToString(txt_pop1staffname.Text);
        //string q1 = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where staff_name='" + staffname + "' and s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code  and s.staff_code not in (select Roll_No from Hostel_StudentDetails ) ";
        string q1 = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where staff_name='" + staffname + "'  and s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code  and s.appl_no = a.appl_no  and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code  and a.appl_id  not in (select app_no from HT_HostelRegistration ) ";
        applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_name='" + staffname + "' and sam.appl_no = sm.appl_no");
        ViewState["appl_id"] = Convert.ToString(applid);
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string StaffName = Convert.ToString(txt_pop1staffname.Text);
            string StaffCode = Convert.ToString(ds.Tables[0].Rows[0][0]);
            string StaffDepartment = Convert.ToString(ds.Tables[0].Rows[0][3]);
            string StaffDesignation = Convert.ToString(ds.Tables[0].Rows[0][5]);

            string applno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + StaffCode + "'");
            sql = "select convert(varchar,convert(datetime,date_of_birth,103),103) from staff_appl_master where appl_no='" + applno + "' ";
            string StaffDob = d2.GetFunction(sql);

            txt_pop1staffname.Text = StaffName;
            txt_pop1staffcode.Text = StaffCode;
            txt_pop1department.Text = StaffDepartment;
            txt_pop1designation.Text = StaffDesignation;
            txt_pop1dob.Text = StaffDob;

        }
        //else
        //{
        //    imgdiv2.Visible = true;
        //    lbl_erroralert.Text = "This Staff Name Already Added";
        //    txt_pop1staffcode.Text = "";
        //    txt_pop1department.Text = "";
        //    txt_pop1designation.Text = "";
        //    txt_pop1dob.Text = "";
        //}
        if (txt_pop1staffname.Text == "")
        {

            txt_pop1staffcode.Text = "";
            txt_pop1department.Text = "";
            txt_pop1designation.Text = "";
            txt_pop1dob.Text = "";
        }



    }
    protected void btn_staffexit_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = false;
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void clearpopup1()
    {
        txt_pop1staffname.Text = txt_pop1department.Text = txt_pop1designation.Text = txt_pop1dob.Text = "";
        txt_pop1floor.Text = txt_pop1reason.Text = txt_pop1roomno.Text = txt_pop1roomtype.Text = txt_pop1building.Text = "";
        txt_pop1staffcode.Text = "";
        txt_pop1admindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        cb_pop1vacate.Checked = false;
        cb_discontinue.Checked = false;
        btn_staffquestion.Enabled = true;
    }
    public void bindroompopbuild()
    {
        try
        {
            cbl_pop3build.Items.Clear();
            string bul = "";
            string hostelpk = Convert.ToString(ddl_pop1hostelname.SelectedItem.Value);

            bul = d2.GetBuildingCode_inv(hostelpk);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_pop3build.DataSource = ds;
                cbl_pop3build.DataTextField = "Building_Name";
                cbl_pop3build.DataValueField = "code";
                cbl_pop3build.DataBind();
            }
            else
            {
                txt_pop3build.Text = "--Select--";
            }

            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                cbl_pop3build.Items[i].Selected = true;
                txt_pop3build.Text = "Building(" + (cbl_pop3build.Items.Count) + ")";
                cb_pop3build.Checked = true;
            }

            string locbuild = "";
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    string builname = cbl_pop3build.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            bindroompopfloor(locbuild);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_pop3build_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_pop3build.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_pop3build.Items.Count; i++)
                {
                    if (cb_pop3build.Checked == true)
                    {
                        cbl_pop3build.Items[i].Selected = true;
                        txt_pop3build.Text = "Build(" + (cbl_pop3build.Items.Count) + ")";
                        txt_pop3floor.Text = "--Select--";
                        txt_pop3roomtype.Text = "--Select--";
                        build1 = cbl_pop3build.Items[i].Text.ToString();
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
                clgfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_pop3build.Items.Count; i++)
                {
                    cbl_pop3build.Items[i].Selected = false;
                    txt_pop3build.Text = "--Select--";
                    cbl_pop3floor.ClearSelection();
                    cbl_pop3roomtype.ClearSelection();
                    cb_pop3floor.Checked = false;
                    cb_pop3roomtype.Checked = false;
                    txt_pop3floor.Text = "--Select--";
                    txt_pop3roomtype.Text = "--Select--";
                }
            }
            //  Button2.Focus();

        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_pop3build_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_pop3build.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_pop3floor.Text = "--Select--";
                    txt_pop3roomtype.Text = "--Select--";
                    cb_pop3floor.Checked = false;
                    cb_pop3roomtype.Checked = false;
                    build = cbl_pop3build.Items[i].Text.ToString();
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
            clgfloor(buildvalue);
            bindroompopfloor(buildvalue);
            if (seatcount == cbl_pop3build.Items.Count)
            {
                txt_pop3build.Text = "Build(" + seatcount + ")";
                cb_pop3build.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_pop3build.Text = "--Select--";
            }
            else
            {
                txt_pop3build.Text = "Build(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindroompopfloor(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_pop3floor.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_pop3floor.DataSource = ds;
                cbl_pop3floor.DataTextField = "Floor_Name";
                cbl_pop3floor.DataValueField = "Floorpk";
                cbl_pop3floor.DataBind();

            }
            else
            {
                txt_pop3floor.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {
                cbl_pop3floor.Items[i].Selected = true;
                cb_pop3floor.Checked = true;
            }

            string locfloor = "";
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {
                if (cbl_pop3floor.Items[i].Selected == true)
                {
                    txt_pop3floor.Text = "Floor(" + (cbl_pop3floor.Items.Count) + ")";
                    string flrname = cbl_pop3floor.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }

            }
            bindroompoproomtype(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_pop3floor_checkedchange(object sender, EventArgs e)
    {
        try
        {

            txt_pop3roomtype.Text = "--Select--";
            if (cb_pop3floor.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";

                if (cb_pop3build.Checked == true)
                {

                    for (int i = 0; i < cbl_pop3build.Items.Count; i++)
                    {

                        build1 = cbl_pop3build.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;

                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;

                        }
                    }
                    if (cb_pop3floor.Checked == true)
                    {
                        for (int j = 0; j < cbl_pop3floor.Items.Count; j++)
                        {
                            cbl_pop3floor.Items[j].Selected = true;
                            txt_pop3floor.Text = "Floor(" + (cbl_pop3floor.Items.Count) + ")";
                            txt_pop3roomtype.Text = "--Select--";
                            build2 = cbl_pop3floor.Items[j].Text.ToString();
                            if (buildvalue2 == "")
                            {

                                buildvalue2 = build2;
                            }
                            else
                            {

                                buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                            }
                        }
                    }
                }

                if (cb_pop3build.Checked == false)
                {

                    for (int i = 0; i < cbl_pop3build.Items.Count; i++)
                    {

                        build1 = cbl_pop3build.Items[i].Text.ToString();
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

                if (cb_pop3floor.Checked == true)
                {
                    for (int j = 0; j < cbl_pop3floor.Items.Count; j++)
                    {
                        cbl_pop3floor.Items[j].Selected = true;
                        txt_pop3floor.Text = "Floor(" + (cbl_pop3floor.Items.Count) + ")";
                        txt_pop3roomtype.Text = "--Select--";
                        build2 = cbl_pop3floor.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {

                            buildvalue2 = build2;
                        }
                        else
                        {

                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                bindroompoproomtype(buildvalue1, buildvalue2);
            }
            else
            {
                for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
                {
                    cbl_pop3floor.Items[i].Selected = false;
                    txt_pop3floor.Text = "--Select--";
                    cbl_pop3roomtype.ClearSelection();
                    cb_pop3roomtype.Checked = false;
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_pop3floor_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_pop3floor.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    build1 = cbl_pop3build.Items[i].Text.ToString();
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
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {

                if (cbl_pop3floor.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_pop3roomtype.Text = "--Select--";
                    build2 = cbl_pop3floor.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {

                        buildvalue2 = build2;
                    }
                    else
                    {

                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            bindroompoproomtype(buildvalue1, buildvalue2);
            if (seatcount == cbl_pop3floor.Items.Count)
            {
                txt_pop3floor.Text = "Floor(" + seatcount.ToString() + ")";
                cb_pop3floor.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_pop3floor.Text = "--Select--";
            }
            else
            {
                txt_pop3floor.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindroompoproomtype(string floor, string build)
    {
        try
        {
            if (floor != "" && build != "")
            {
                ds.Clear();
                ds = d2.BindRoomtype(floor, build);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_pop3roomtype.DataSource = ds;
                    cbl_pop3roomtype.DataTextField = "Room_Type";
                    cbl_pop3roomtype.DataValueField = "Room_Type";
                    cbl_pop3roomtype.DataBind();
                }
                else
                {
                    txt_pop3roomtype.Text = "--Select--";
                    cbl_pop3roomtype.ClearSelection();
                }
                for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
                {
                    cbl_pop3roomtype.Items[i].Selected = true;
                    txt_pop3roomtype.Text = "Room Type(" + (cbl_pop3roomtype.Items.Count) + ")";
                    cb_pop3roomtype.Checked = true;
                }
            }
            else
            {
                txt_pop3roomtype.Text = "--Select--";
                cbl_pop3roomtype.ClearSelection();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void cb_pop3roomtype_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_pop3roomtype.Checked == true)
            {
                for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
                {
                    cbl_pop3roomtype.Items[i].Selected = true;
                }
                txt_pop3roomtype.Text = "Room(" + (cbl_pop3roomtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
                {
                    cbl_pop3roomtype.Items[i].Selected = false;
                }
                txt_pop3roomtype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_pop3roomtype_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_pop3roomtype.Checked = false;
            for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
            {
                if (cbl_pop3roomtype.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == cbl_pop3roomtype.Items.Count)
            {
                txt_pop3roomtype.Text = "Room(" + seatcount.ToString() + ")";
                cb_pop3roomtype.Checked = true;
            }
            else if (seatcount == 0)
            {
                cbl_pop3roomtype.Text = "--Select--";
            }
            else
            {
                txt_pop3roomtype.Text = "Room(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_roomlookupsave_Click(object sender, EventArgs e)
    {
        try
        {

            //roomlookup.Visible = false;
            string activerow = "";
            string activecol = "";
            activerow = Froomspread.ActiveSheetView.ActiveRow.ToString();
            activecol = Froomspread.ActiveSheetView.ActiveColumn.ToString();
            if (Convert.ToInt32(activecol) != 0)
            {
                string purpose = Froomspread.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text;
                address = purpose.Split('-');
                purpose = address[0].ToString();
                string building = "";
                if (Froomspread.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor != Color.GreenYellow)
                {
                    txt_pop1roomno.Text = purpose;
                    // string build = Convert.ToString(cbl_pop3build.Items[0].Text);
                    // txt_pop1building.Text = build;
                    string building_value = Convert.ToString(Froomspread.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                    txt_pop1building.Text = building_value;
                    string buil = "";
                    building = Convert.ToString(Froomspread.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                    buil = d2.GetFunction("select Code  from Building_Master sm where  sm.Building_Name='" + building + "'");
                    ViewState["Code"] = Convert.ToString(buil);
                    string floorroom = Froomspread.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                    fr = floorroom.Split('-');
                    txt_pop1floor.Text = fr[0].ToString();
                    if (txt_pop1floor.Text == "")
                    {
                        txt_pop1floor.Text = fr[1].ToString();
                    }
                    string fl = "";

                    fl = d2.GetFunction("select Floorpk  from Floor_Master sm where  sm.Floor_Name='" + fr[0] + "'and Building_Name ='" + building + "'");
                    ViewState["Floorpk"] = Convert.ToString(fl);
                    string room = "select Room_Name,Room_type from Room_Detail where Room_Name='" + txt_pop1roomno.Text + "'";
                    ds = d2.select_method_wo_parameter(room, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string roomtype = ds.Tables[0].Rows[0]["Room_type"].ToString();
                        txt_pop1roomtype.Text = roomtype;
                        roomlookup.Visible = false;
                    }
                    if (txt_pop3roomtype.Text == "")
                    {
                        lblpop3err.Visible = true;
                        lblpop3err.Text = "Please Select  Room";
                        txt_pop1floor.Text = "";
                        txt_pop1building.Text = "";
                        txt_pop3roomtype.Text = "";
                    }
                    // string roomname = "";
                    string ro = "";
                    //  roomname = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    ro = d2.GetFunction("select Roompk  from Room_Detail sm where  sm.Room_Name='" + purpose + "' and Building_Name ='" + building + "'");
                    ViewState["Roompk"] = Convert.ToString(ro);
                }
                else
                {
                    lblpop3err.Visible = true;
                    lblpop3err.Text = "Please Select Unfilled Room";
                }
                if (Froomspread.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor != Color.GreenYellow)
                {
                    txt_pop1roomno.Text = purpose;

                    //string build = cbl_buildname.SelectedItem.Text.ToString();
                    string floorroom = Froomspread.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                    fr = floorroom.Split('-');
                    txt_pop1floor.Text = fr[0].ToString();
                    string fl = "";

                    fl = d2.GetFunction("select Floorpk  from Floor_Master sm where  sm.Floor_Name='" + fr[0] + "'and Building_Name ='" + building + "'");
                    ViewState["Floorpk"] = Convert.ToString(fl);
                    txt_pop1roomtype.Text = fr[1].ToString() + "-" + fr[2].ToString();
                    string ro = "";
                    //  roomname = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    ro = d2.GetFunction("select Roompk  from Room_Detail sm where  sm.Room_Name='" + purpose + "' and Building_Name ='" + building + "'");
                    ViewState["Roompk"] = Convert.ToString(ro);
                    if (txt_pop1roomno.Text == "")
                    {
                        lblpop3err.Visible = true;
                        lblpop3err.Text = "Please Select  Room";
                        txt_pop1floor.Text = "";
                        txt_pop1roomtype.Text = "";
                        txt_pop1building.Text = "";
                    }
                }
                else
                {
                    lblpop3err.Visible = true;
                    lblpop3err.Text = "Please Select Unfilled Room";
                }
            }
            else
            {
                lblpop3err.Visible = true;
                lblpop3err.Text = "Please Select Correct Room";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_roomlookupexit_Click(object sender, EventArgs e)
    {
        roomlookup.Visible = false;
        //cbl_buildname.Items.Clear();
        cbl_pop3floor.Items.Clear();
        cbl_pop3roomtype.Items.Clear();
        Froomspread.Visible = false;
    }
    public void btn_roomlookupgo_Click(object sender, EventArgs e)
    {
        search();
    }

    public void search()
    {
        try
        {
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Froomspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            toalrooms.Visible = true;
            totalvaccants.Visible = true;
            fill.Visible = true;
            partialfill.Visible = true;
            unfill.Visible = true;
            btn_roomlookupsave.Visible = true;
            btn_roomlookupexit.Visible = true;
            Froomspread.Sheets[0].ColumnCount = 0;
            Froomspread.Sheets[0].RowCount = 0;
            string hostelcode = Convert.ToString(ddl_pop1hostelname.SelectedItem.Value);
            string building = "";
            //  building = Convert.ToString(cbl_pop3build.SelectedItem.Text);
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    if (building == "")
                    {
                        building = "" + cbl_pop3build.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        building = building + "'" + "," + "'" + cbl_pop3build.Items[i].Text.ToString() + "";
                    }
                }
            }
            string vaccanttype = Convert.ToString(ddl_pop3vaccant.SelectedItem.Text);

            string floor = "";
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {
                if (cbl_pop3floor.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_pop3floor.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_pop3floor.Items[i].Text.ToString() + "";
                    }
                }
            }
            string roomtype0 = "";
            for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
            {
                if (cbl_pop3roomtype.Items[i].Selected == true)
                {
                    if (roomtype0 == "")
                    {
                        roomtype0 = "" + cbl_pop3roomtype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        roomtype0 = roomtype0 + "'" + "," + "'" + cbl_pop3roomtype.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (floor.Trim() != "" && roomtype0.Trim() != "")
            {
                // 24.02.16
                // string selectquery = "Select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student, ISNULL(m.Room_Cost,0) Room_Cost,h.HostelMasterPK,r.Building_Name FROM Room_Detail R left join RoomCost_Master m on r.Room_type = m.Room_Type  left join Building_Master b on b.Building_Name =r.Building_Name left join HM_HostelMaster h on h.collegecode =b.College_Code where R.Building_Name in ('" + building + "') and r.Room_Type in ('" + roomtype0 + "') and Floor_Name in ('" + floor + "') and h.HostelMasterPK ='" + hostelcode + "'";
                string bcode = d2.GetFunction(" select HostelBuildingFK  from HM_HostelMaster where HostelMasterPK ='" + hostelcode + "'");
                string selectquery = " select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student,r.Building_Name,b.College_Code from Building_Master B,Room_Detail R where b.Building_Name =r.Building_Name and b.College_Code =r.College_Code and b.Code in (" + bcode + ")";

                if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Filled")
                {
                    selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
                }
                else if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Un Filled")
                {
                    selectquery = selectquery + " AND R.Avl_Student = 0";
                }
                else if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Partially Filled")
                {
                    selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
                }

                selectquery = selectquery + " Select Distinct F.Floor_Name+' - '+Room_Type RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name in ('" + building + "') AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY F.Floor_Name+' - '+Room_Type";
                selectquery = selectquery + " select ISNULL(Room_Cost,0)as Room_Cost,Hostel_Code,Room_Type  from RoomCost_Master";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");

                int IntRoomLen = 0;
                int totalunfill = 0;
                int totalfill = 0;
                int totalpartialfill = 0;
                int totalvaccant = 0;
                string strRoomDetail = "";
                int colcnt = 0;
                Froomspread.Sheets[0].ColumnCount = 0;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Froomspread.CommandBar.Visible = false;
                    Froomspread.Sheets[0].RowHeader.Visible = false;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            Froomspread.Sheets[0].RowHeader.Visible = false;

                            Froomspread.Sheets[0].RowCount = Froomspread.Sheets[0].RowCount + 1;
                            colcnt = 0;

                            if (Froomspread.Sheets[0].ColumnCount - 1 < colcnt)
                            {
                                Froomspread.Sheets[0].ColumnCount++;
                            }

                            string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
                            string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
                            string alldetails = floorname + "-" + roomtype;

                            // string buildingname = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);

                            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                            Froomspread.Sheets[0].Columns[colcnt].CellType = textcel_type;
                            Froomspread.Sheets[0].Cells[i, colcnt].Text = alldetails;
                            // 29.10.15
                            //  FpSpread3.Sheets[0].Cells[i, colcnt].Tag = ds.Tables[0].Rows[colcnt]["Building_Name"].ToString();

                            Froomspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
                            Froomspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Froomspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Froomspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Froomspread.Sheets[0].Cells[i, 0].Font.Bold = true;
                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
                            DataView dv = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                //29.02.16
                                //FpSpread3.Sheets[0].Cells[i, colcnt].Tag = Convert.ToString(dv[0]["Building_Name"]);
                                int columncount = dv.Count;
                                for (int cnt = 0; cnt < dv.Count; cnt++)
                                {
                                    colcnt++;
                                    Froomspread.Sheets[0].Cells[i, cnt].Tag = Convert.ToString(dv[cnt]["Building_Name"]);
                                    string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]);// +Convert.ToString(dv[cnt]["Room_Cost"]);
                                    //24.02.16
                                    DataView cost = new DataView(); string rmcost = "";
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        for (int rmc = 0; rmc < ds.Tables[2].Rows.Count; rmc++)
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = "  Room_Type='" + roomtype + "'";//Hostel_Code='" + hostelcode + "' and//16.03.16
                                            cost = ds.Tables[2].DefaultView;

                                            if (cost.Count > 0)
                                            {
                                                // rmcost = Convert.ToString(cost[rmc]["Room_Cost"]);//16.04.16
                                                rmcost = Convert.ToString(cost[0]["Room_Cost"]);
                                            }
                                        }
                                    }
                                    if (rmcost.Trim() == "")
                                    {
                                        rmcost = "0";
                                    }
                                    s = s + rmcost;
                                    if (Froomspread.Sheets[0].ColumnCount - 1 < colcnt)
                                    {
                                        Froomspread.Sheets[0].ColumnCount = Froomspread.Sheets[0].ColumnCount + 1;
                                        Froomspread.Sheets[0].Columns[0].Locked = true;
                                        Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].Text = "Room Details";
                                        Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        Froomspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, Froomspread.Sheets[0].ColumnCount - 1);
                                        Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].Font.Bold = true;


                                    }
                                    if (cb_include.Checked == true)
                                    {
                                        Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                        Froomspread.Sheets[0].Columns[colcnt].Locked = true;


                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                        {
                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                            totalunfill = totalunfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                        {
                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                            totalpartialfill = totalpartialfill + 1;
                                        }

                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                        {
                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                            totalfill = totalfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                        {
                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                            totalpartialfill = totalpartialfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                        {
                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                            totalunfill = totalunfill + 1;
                                        }

                                        //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + rmcost.Length;

                                        totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                    }

                                    else
                                    {
                                        try
                                        {
                                            if (cb_include.Checked == false)
                                            {

                                                if (roomchecklist.Items[0].Selected == false && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
                                                {
                                                    Froomspread.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
                                                    {
                                                        Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);

                                                    if (IntRoomLen < strRoomDetail.Length)
                                                    {
                                                        IntRoomLen = strRoomDetail.Length;
                                                    }
                                                    Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
                                                    //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;

                                                }
                                            }

                                            if (cbl_roomlist.Items[0].Selected == true && cbl_roomlist.Items[1].Selected == false && cbl_roomlist.Items[2].Selected == false)
                                            {
                                                Froomspread.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }

                                                Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);

                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (cbl_roomlist.Items[0].Selected == true && cbl_roomlist.Items[1].Selected == true && cbl_roomlist.Items[2].Selected == false)
                                            {
                                                Froomspread.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (cbl_roomlist.Items[1].Selected == true && cbl_roomlist.Items[2].Selected == true && cbl_roomlist.Items[0].Selected == false)
                                            {
                                                Froomspread.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (cbl_roomlist.Items[0].Selected == true && cbl_roomlist.Items[2].Selected == true && cbl_roomlist.Items[1].Selected == false)
                                            {
                                                Froomspread.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + rmcost;// (dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (cbl_roomlist.Items[1].Selected == true && cbl_roomlist.Items[2].Selected == false && cbl_roomlist.Items[0].Selected == false)
                                            {
                                                Froomspread.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }

                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (cbl_roomlist.Items[2].Selected == true && cbl_roomlist.Items[1].Selected == false && cbl_roomlist.Items[0].Selected == false)
                                            {
                                                Froomspread.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (cbl_roomlist.Items[0].Selected == true && cbl_roomlist.Items[2].Selected == true && cbl_roomlist.Items[1].Selected == true)
                                            {
                                                chck1.Checked = true;
                                                Froomspread.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);

                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {

                                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

                                    for (int j = 1; j < Froomspread.Sheets[0].ColumnCount; j++)
                                    {
                                        totalvaccants.Text = " ";
                                        toalrooms.Text = " ";
                                        int totalroom = totalunfill + totalfill + totalpartialfill;
                                        toalrooms.Text = "Total No.of Rooms :" + totalroom;
                                        totalvaccants.Text = "Total No.of Vacant :" + totalvaccant;
                                        fill.Text = ("Filled(" + totalfill + ")");
                                        unfill.Text = ("UnFilled(" + totalunfill + ")");
                                        partialfill.Text = ("Partially Filled(" + totalpartialfill + ")");
                                    }
                                }
                                int height = 60;
                                {
                                    for (int j = 1; j < Froomspread.Sheets[0].RowCount; j++)
                                    {
                                        height = height + Froomspread.Sheets[0].Rows[j].Height;
                                    }
                                    Froomspread.Height = height;
                                    Froomspread.SaveChanges();
                                    Froomspread.Sheets[0].PageSize = Froomspread.Sheets[0].RowCount;
                                }
                                int width = 0;
                                if (Froomspread.Sheets[0].ColumnCount == 7)
                                {
                                    Froomspread.Sheets[0].Columns[0].Width = 400;
                                    for (int j = 1; j < Froomspread.Sheets[0].ColumnCount; j++)
                                    {
                                        width = width + Froomspread.Sheets[0].Columns[j].Width;

                                    }
                                    width = width + 400;

                                }
                                else if (Froomspread.Sheets[0].ColumnCount == 5)
                                {
                                    Froomspread.Sheets[0].Columns[0].Width = 800;
                                    for (int j = 1; j < Froomspread.Sheets[0].ColumnCount; j++)
                                    {
                                        width = width + Froomspread.Sheets[0].Columns[j].Width;

                                    }
                                    width = width + 800;
                                }
                                else
                                {
                                    width = 770;
                                }

                                Froomspread.Width = width;
                                Froomspread.SaveChanges();
                                Froomspread.Sheets[0].PageSize = Froomspread.Sheets[0].ColumnCount;
                            }
                        }
                        Froomspread.Visible = true;
                        tblStatus.Visible = true;
                    }
                }
                else
                {
                    Froomspread.Visible = false;
                    tblStatus.Visible = false;
                    lblpop3err.Visible = true;
                    lblpop3err.Text = "No Records Found";
                    btn_roomlookupsave.Visible = false;
                    btn_roomlookupexit.Visible = false;
                }
            }
            else
            {
                tblStatus.Visible = false;
                Froomspread.Visible = false;
                lblpop3err.Visible = true;
                lblpop3err.Text = "No Records Found";
                btn_pop3save.Visible = false;
                btn_pop3exit.Visible = false;
            }
        }
        catch
        {
        }
    }
    //public void search()
    //{
    //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //    darkstyle.ForeColor = Color.Black;
    //    Froomspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
    //    toalrooms.Visible = true;
    //    totalvaccants.Visible = true;
    //    fill.Visible = true;
    //    partialfill.Visible = true;
    //    unfill.Visible = true;
    //    btn_roomlookupsave.Visible = true;
    //    btn_roomlookupexit.Visible = true;
    //    Froomspread.Sheets[0].ColumnCount = 0;
    //    Froomspread.Sheets[0].RowCount = 0;
    //    string hostelcode = Convert.ToString(ddl_pop1hostelname.SelectedItem.Value);
    //    string building = "";
    //    //  building = Convert.ToString(cbl_pop3build.SelectedItem.Text);
    //    for (int i = 0; i < cbl_pop3build.Items.Count; i++)
    //    {
    //        if (cbl_pop3build.Items[i].Selected == true)
    //        {
    //            if (building == "")
    //            {
    //                building = "" + cbl_pop3build.Items[i].Text.ToString() + "";
    //            }
    //            else
    //            {
    //                building = building + "'" + "," + "'" + cbl_pop3build.Items[i].Text.ToString() + "";
    //            }
    //        }
    //    }
    //    string vaccanttype = Convert.ToString(ddl_pop3vaccant.SelectedItem.Text);

    //    string floor = "";
    //    for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
    //    {
    //        if (cbl_pop3floor.Items[i].Selected == true)
    //        {
    //            if (floor == "")
    //            {
    //                floor = "" + cbl_pop3floor.Items[i].Text.ToString() + "";
    //            }
    //            else
    //            {
    //                floor = floor + "'" + "," + "'" + cbl_pop3floor.Items[i].Text.ToString() + "";
    //            }
    //        }
    //    }
    //    string roomtype0 = "";
    //    for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
    //    {
    //        if (cbl_pop3roomtype.Items[i].Selected == true)
    //        {
    //            if (roomtype0 == "")
    //            {
    //                roomtype0 = "" + cbl_pop3roomtype.Items[i].Value.ToString() + "";
    //            }
    //            else
    //            {
    //                roomtype0 = roomtype0 + "'" + "," + "'" + cbl_pop3roomtype.Items[i].Value.ToString() + "";
    //            }
    //        }
    //    }


    //    if (floor.Trim() != "" && roomtype0.Trim() != "")
    //    {
    //        string builFK = d2.GetBuildingCode_inv(hostelcode);
    //        string selectquery = "Select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student, ISNULL(m.Room_Cost,0) Room_Cost,h.HostelMasterPK,r.Building_Name  FROM Room_Detail R left join RoomCost_Master m on r.Room_type = m.Room_Type  left join Building_Master b on b.Building_Name =r.Building_Name left join HM_HostelMaster h on h.collegecode =b.College_Code where R.Building_Name in ('" + building + "') and r.Room_Type in ('" + roomtype0 + "') and Floor_Name in ('" + floor + "') and h.HostelMasterPK ='" + hostelcode + "'";
    //        if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Filled")
    //        {
    //            selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
    //        }
    //        else if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Un Filled")
    //        {
    //            selectquery = selectquery + " AND R.Avl_Student = 0";
    //        }
    //        else if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Partialy Filled")
    //        {
    //            selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
    //        }

    //        selectquery = selectquery + " Select Distinct F.Floor_Name+' - '+Room_Type RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name in ('" + building + "') AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY F.Floor_Name+' - '+Room_Type";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(selectquery, "Text");

    //        int IntRoomLen = 0;
    //        int totalunfill = 0;
    //        int totalfill = 0;
    //        int totalpartialfill = 0;
    //        int totalvaccant = 0;
    //        string strRoomDetail = "";
    //        int colcnt = 0;
    //        Froomspread.Sheets[0].ColumnCount = 0;

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            Froomspread.CommandBar.Visible = false;
    //            Froomspread.Sheets[0].RowHeader.Visible = false;


    //            if (ds.Tables[1].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
    //                {
    //                    Froomspread.Sheets[0].RowHeader.Visible = false;

    //                    Froomspread.Sheets[0].RowCount = Froomspread.Sheets[0].RowCount + 1;
    //                    colcnt = 0;

    //                    if (Froomspread.Sheets[0].ColumnCount - 1 < colcnt)
    //                    {
    //                        Froomspread.Sheets[0].ColumnCount++;
    //                    }

    //                    string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
    //                    string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
    //                    string alldetails = floorname + "-" + roomtype;

    //                    FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
    //                    Froomspread.Sheets[0].Columns[colcnt].CellType = textcel_type;
    //                    Froomspread.Sheets[0].Cells[i, colcnt].Text = alldetails;

    //                    Froomspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
    //                    Froomspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //                    Froomspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
    //                    Froomspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //                    Froomspread.Sheets[0].Cells[i, 0].Font.Bold = true;
    //                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
    //                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
    //                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
    //                    DataView dv = new DataView();
    //                    ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
    //                    dv = ds.Tables[0].DefaultView;
    //                    if (dv.Count > 0)
    //                    {
    //                        Froomspread.Sheets[0].Cells[i, colcnt].Tag = Convert.ToString(dv[0]["Building_Name"]);
    //                        int columncount = dv.Count;
    //                        for (int cnt = 0; cnt < dv.Count; cnt++)
    //                        {
    //                            colcnt++;
    //                            string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]) + Convert.ToString(dv[cnt]["Room_Cost"]);
    //                            if (Froomspread.Sheets[0].ColumnCount - 1 < colcnt)
    //                            {
    //                                Froomspread.Sheets[0].ColumnCount = Froomspread.Sheets[0].ColumnCount + 1;
    //                                Froomspread.Sheets[0].Columns[0].Locked = true;
    //                                Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].Text = "Room Details";
    //                                Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
    //                                Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                                Froomspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, Froomspread.Sheets[0].ColumnCount - 1);
    //                                Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
    //                                Froomspread.Sheets[0].ColumnHeader.Cells[0, Froomspread.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                            }
    //                            if (cb_include.Checked == true)
    //                            {
    //                                Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
    //                                Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                {

    //                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                    totalunfill = totalunfill + 1;
    //                                }
    //                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                {
    //                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                    totalpartialfill = totalpartialfill + 1;
    //                                }

    //                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                {
    //                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                    totalfill = totalfill + 1;
    //                                }
    //                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                {
    //                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                    totalpartialfill = totalpartialfill + 1;
    //                                }
    //                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                {
    //                                    Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                    totalunfill = totalunfill + 1;
    //                                }

    //                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
    //                            }

    //                            else
    //                            {
    //                                try
    //                                {
    //                                    if (cb_include.Checked == false)
    //                                    {

    //                                        if (cbl_roomlist.Items[0].Selected == false && cbl_roomlist.Items[1].Selected == false && cbl_roomlist.Items[2].Selected == false)
    //                                        {
    //                                            Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                            {
    //                                                Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                                totalunfill = totalunfill + 1;
    //                                            }
    //                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                            {
    //                                                Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                                totalpartialfill = totalpartialfill + 1;
    //                                            }

    //                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                            {
    //                                                Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                                Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                                totalfill = totalfill + 1;

    //                                            }
    //                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                            {
    //                                                Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                                totalpartialfill = totalpartialfill + 1;
    //                                            }
    //                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
    //                                            {
    //                                                Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                                totalunfill = totalunfill + 1;
    //                                            }
    //                                            strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);

    //                                            if (IntRoomLen < strRoomDetail.Length)
    //                                            {
    //                                                IntRoomLen = strRoomDetail.Length;

    //                                            }

    //                                            Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;


    //                                        }
    //                                    }

    //                                    if (cbl_roomlist.Items[0].Selected == true && cbl_roomlist.Items[1].Selected == false && cbl_roomlist.Items[2].Selected == false)
    //                                    {
    //                                        Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }

    //                                        Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);

    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }


    //                                    else if (cbl_roomlist.Items[0].Selected == true && cbl_roomlist.Items[1].Selected == true && cbl_roomlist.Items[2].Selected == false)
    //                                    {
    //                                        Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }

    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }

    //                                    else if (cbl_roomlist.Items[1].Selected == true && cbl_roomlist.Items[2].Selected == true && cbl_roomlist.Items[0].Selected == false)
    //                                    {
    //                                        Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }

    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }

    //                                    else if (cbl_roomlist.Items[0].Selected == true && cbl_roomlist.Items[2].Selected == true && cbl_roomlist.Items[1].Selected == false)
    //                                    {
    //                                        Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Room_Cost"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }

    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }

    //                                    else if (cbl_roomlist.Items[1].Selected == true && cbl_roomlist.Items[2].Selected == false && cbl_roomlist.Items[0].Selected == false)
    //                                    {
    //                                        Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }

    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }
    //                                    else if (cbl_roomlist.Items[2].Selected == true && cbl_roomlist.Items[1].Selected == false && cbl_roomlist.Items[0].Selected == false)
    //                                    {
    //                                        Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Room_Cost"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }

    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }


    //                                    else if (cbl_roomlist.Items[0].Selected == true && cbl_roomlist.Items[2].Selected == true && cbl_roomlist.Items[1].Selected == true)
    //                                    {
    //                                        cb_include.Checked = true;
    //                                        Froomspread.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        Froomspread.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);

    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {

    //                                            Froomspread.Sheets[0].Cells[Froomspread.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }

    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;

    //                                    }
    //                                    totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
    //                                }

    //                                catch (Exception ex)
    //                                {
    //                                }
    //                            }

    //                            for (int j = 1; j < Froomspread.Sheets[0].ColumnCount; j++)
    //                            {
    //                                totalvaccants.Text = " ";
    //                                toalrooms.Text = " ";
    //                                int totalroom = totalunfill + totalfill + totalpartialfill;
    //                                toalrooms.Text = "Total No.of Rooms :" + totalroom;
    //                                totalvaccants.Text = "Total No.of Vacant :" + totalvaccant;
    //                                fill.Text = ("Filled(" + totalfill + ")");
    //                                unfill.Text = ("UnFilled(" + totalunfill + ")");
    //                                partialfill.Text = ("Partialy Filled(" + totalpartialfill + ")");
    //                            }

    //                        }

    //                        int height = 60;
    //                        {
    //                            for (int j = 1; j < Froomspread.Sheets[0].RowCount; j++)
    //                            {
    //                                height = height + Froomspread.Sheets[0].Rows[j].Height;
    //                            }
    //                            Froomspread.Height = height;
    //                            Froomspread.SaveChanges();
    //                            Froomspread.Sheets[0].PageSize = Froomspread.Sheets[0].RowCount;
    //                        }

    //                        int width = 0;
    //                        if (Froomspread.Sheets[0].ColumnCount == 7)
    //                        {
    //                            Froomspread.Sheets[0].Columns[0].Width = 400;
    //                            for (int j = 1; j < Froomspread.Sheets[0].ColumnCount; j++)
    //                            {
    //                                width = width + Froomspread.Sheets[0].Columns[j].Width;

    //                            }
    //                            width = width + 400;

    //                        }
    //                        else if (Froomspread.Sheets[0].ColumnCount == 5)
    //                        {
    //                            Froomspread.Sheets[0].Columns[0].Width = 800;
    //                            for (int j = 1; j < Froomspread.Sheets[0].ColumnCount; j++)
    //                            {
    //                                width = width + Froomspread.Sheets[0].Columns[j].Width;

    //                            }
    //                            width = width + 800;
    //                        }
    //                        else
    //                        {
    //                            width = 770;
    //                        }

    //                        Froomspread.Width = width;
    //                        Froomspread.SaveChanges();
    //                        Froomspread.Sheets[0].PageSize = Froomspread.Sheets[0].ColumnCount;
    //                    }

    //                }


    //                Froomspread.Visible = true;
    //                tblStatus.Visible = true;
    //                //lblpop3err.Visible = false;
    //                //lblpop3err.Text = "No Records Found";
    //            }
    //        }
    //        else
    //        {

    //            Froomspread.Visible = false;
    //            tblStatus.Visible = false;
    //            // imgdiv2.Visible = true;
    //            // lbl_erroralert.Text = "No records found";
    //            //22.12.15 add
    //            lblpop3err.Visible = true;
    //            lblpop3err.Text = "No Records Found";
    //            btn_roomlookupsave.Visible = false;
    //            btn_roomlookupexit.Visible = false;
    //        }
    //    }
    //    else
    //    {
    //        tblStatus.Visible = false;
    //        Froomspread.Visible = false;
    //        //  imgdiv2.Visible = true;
    //        // lbl_erroralert.Text = "No records found";
    //        //22.12.15 add
    //        lblpop3err.Visible = true;
    //        lblpop3err.Text = "No Records Found";
    //        btn_roomlookupsave.Visible = false;
    //        btn_roomlookupexit.Visible = false;
    //    }
    //}
    public void ddlhstlnamepop3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            mm = ddl_pop1hostelname.SelectedValue;


            txt_pop3roomtype.Text = "";
            txt_pop3build.Text = "";
            txt_pop3floor.Text = "";

        }
        catch (Exception ex)
        {

        }
    }
    protected void imagebtnpop3close_Click(object sender, EventArgs e)
    {
        roomlookup.Visible = false;
    }
    protected void ddl_pop3vaccant_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void cbl_roomlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbl_roomlist.Items[0].Selected == false)
            {
                cb_include.Checked = false;
            }
            if (cbl_roomlist.Items[1].Selected == false)
            {
                cb_include.Checked = false;
            }
            if (cbl_roomlist.Items[2].Selected == false)
            {
                cb_include.Checked = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_include_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_include.Checked == true)
            {
                for (int i = 0; i < cbl_roomlist.Items.Count; i++)
                {
                    cbl_roomlist.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < cbl_roomlist.Items.Count; i++)
                {
                    cbl_roomlist.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdb_staffe.Checked == true)
            {
                string reportname = txt_excelname.Text;
                if (reportname.ToString().Trim() != "")
                {
                    da.printexcelreport(Fpspread1, reportname);
                    lbl_validation.Visible = false;
                }
                else
                {
                    lbl_validation.Text = "Please Enter Your Report Name";
                    lbl_validation.Visible = true;
                    txt_excelname.Focus();
                }
            }
            else if (rdb_gueste.Checked == true)
            {
                string reportname = txt_excelname.Text;
                if (reportname.ToString().Trim() != "")
                {
                    da.printexcelreport(Fpspread2, reportname);
                    lbl_validation.Visible = false;
                }
                else
                {
                    lbl_validation.Text = "Please Enter Your Report Name";
                    lbl_validation.Visible = true;
                    txt_excelname.Focus();
                }

            }
        }
        catch
        {

        }

    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdb_staffe.Checked == true)
            {
                string degreedetails = "Hostel Staff / Guest Registration Detail Report";
                string pagename = "HT_StaffRegistration.aspx";
                Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else if (rdb_gueste.Checked == true)
            {
                string degreedetails = "Hostel Staff / Guest Registration Detail Report";
                string pagename = "HT_StaffRegistration.aspx";
                Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
        }
        catch
        {

        }

    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        if (check == true)
        {
            string activerow = "";
            string activecol = "";

            activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();

            if (activerow.Trim() != "")
            {
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 0].BackColor = Color.LightBlue;
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].BackColor = Color.LightBlue;
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].BackColor = Color.LightBlue;
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].BackColor = Color.LightBlue;
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].BackColor = Color.LightBlue;
            }
            


        }
    }
    protected void rdb_guest_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_guest.Checked == true)
        {
            //rdb_NonVeg.Visible = true;
            ddlguest.Visible = true;
            lblGuestType.Visible = true;

            idgeneration();
            lbl_name4.Visible = true;
            txt_nameguest.Visible = true;
            guest.Visible = true;
            lbl_compname.Visible = true;
            txt_compname.Visible = true;
            lbl_desgn.Visible = true;
            txt_desgn.Visible = true;
            lbl_dep.Visible = true;

            txt_dep.Visible = true;
            // lbl_visit1.Visible = true;
            // txt_visit1.Visible = true;

            lbl_mno.Visible = true;
            txt_mno.Visible = true;
            //lbl_phno.Visible = true;

            //txt_phno.Visible = true;
            lbl_str.Visible = true;

            txt_str.Visible = true;
            lbl_cty.Visible = true;

            txt_cty.Visible = true;
            lbl_dis.Visible = true;
            txt_dis.Visible = true;

            lbl_stat.Visible = true;
            txt_stat.Visible = true;

            btn_saveguest.Visible = true;
            btn_exitguest.Visible = true;

            lbl_messname.Visible = true;
            ddl_messname.Visible = true;
            lbl_fromdate.Visible = true;
            txt_admindate.Visible = true;
            lbl_code.Visible = true;
            txt_code.Visible = true;
            ddlmess1.Visible = true;
            lbmess.Visible = true;
            ddlmess.Visible = false;
            Lblmess.Visible = false;
            lblid.Visible = false;
            txtid.Visible = false;
            Llid.Visible = true;
            txtid1.Visible = true;
            lbl_room.Visible = true;
            txt_room.Visible = true;
            roomnum.Visible = true;
            btn2.Visible = true;
            lbl_buildingguest.Visible = true;
            txt_building.Visible = true;
            lbl_floorguest.Visible = true;
            txt_floor.Visible = true;
            lbl_roomtype.Visible = true;
            txt_roomtype.Visible = true;

            lbl_vacate.Visible = true;
            cb_vacate.Visible = true;
            lbl_vacatedate.Visible = true;
            txt_vacatedateguest.Visible = true;

            //

            lbl_pop1collegename.Visible = false;
            ddl_pop1collegename.Visible = false;
            lbl_pop1hostelname.Visible = false;
            ddl_pop1hostelname.Visible = false;
            staff.Visible = false;
            lbl_pop1staffname.Visible = false;
            txt_pop1staffname.Visible = false;
            staffnamebtn.Visible = false;

            btn_staffquestion.Visible = false;
            lbl_pop1staffcode.Visible = false;
            txt_pop1staffcode.Visible = false;

            lbl_pop1department.Visible = false;
            txt_pop1department.Visible = false;
            lbl_pop1designation.Visible = false;

            txt_pop1designation.Visible = false;
            lbl_pop1dob.Visible = false;


            txt_pop1dob.Visible = false;
            lbl_pop1admindate.Visible = false;
            txt_pop1admindate.Visible = false;
            lbl_pop1roomno.Visible = false;
            txt_pop1roomno.Visible = false;
            roomno.Visible = false;
            btn_roomques.Visible = false;
            lbl_pop1messtype.Visible = false;

            ddlStudType.Visible = false;
            lbl_pop1building.Visible = false;
            txt_pop1building.Visible = false;

            lbl_pop1floor.Visible = false;
            txt_pop1floor.Visible = false;
            lbl_pop1roomtype.Visible = false;
            txt_pop1roomtype.Visible = false;
            lbl_pop1discontinue.Visible = false;
            cb_discontinue.Visible = false;
            lbl_pop1date.Visible = false;
            txt_discontinuedate.Visible = false;

            lbl_pop1reason.Visible = false;
            txt_pop1reason.Visible = false;
            lbl_pop1vacate.Visible = false;
            cb_pop1vacate.Visible = false;
            txt_vacatedate.Visible = false;
            btn_pop1save.Visible = false;
            btn_pop1exit.Visible = false;
        }
        else
        {

        }


    }
    protected void rdb_staff_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_staff.Checked == true)
        {
            lblGuestType.Visible = false;
            //rdb_NonVeg.Visible = false;
            ddlguest.Visible = false;
            lbl_pop1collegename.Visible = true;
            ddl_pop1collegename.Visible = true;
            lbl_pop1hostelname.Visible = true;
            ddl_pop1hostelname.Visible = true;
            staff.Visible = true;
            lbl_pop1staffname.Visible = true;
            txt_pop1staffname.Visible = true;
            staffnamebtn.Visible = true;
            idgeneration();
            btn_staffquestion.Visible = true;
            lbl_pop1staffcode.Visible = true;
            txt_pop1staffcode.Visible = true;

            lbl_pop1department.Visible = true;
            txt_pop1department.Visible = true;
            lbl_pop1designation.Visible = true;

            txt_pop1designation.Visible = true;
            lbl_pop1dob.Visible = true;


            txt_pop1dob.Visible = true;
            lbl_pop1admindate.Visible = true;
            txt_pop1admindate.Visible = true;
            lbl_pop1roomno.Visible = true;
            txt_pop1roomno.Visible = true;
            roomno.Visible = true;
            btn_roomques.Visible = true;
            lbl_pop1messtype.Visible = true;

            ddlStudType.Visible = true;
            lbl_pop1building.Visible = true;
            txt_pop1building.Visible = true;

            lbl_pop1floor.Visible = true;
            txt_pop1floor.Visible = true;
            lbl_pop1roomtype.Visible = true;
            txt_pop1roomtype.Visible = true;
            lbl_pop1discontinue.Visible = true;
            cb_discontinue.Visible = true;
            lbl_pop1date.Visible = true;
            txt_discontinuedate.Visible = true;

            lbl_pop1reason.Visible = true;
            txt_pop1reason.Visible = true;
            lbl_pop1vacate.Visible = true;
            cb_pop1vacate.Visible = true;
            txt_vacatedate.Visible = true;
            btn_pop1save.Visible = true;
            btn_pop1exit.Visible = true;
            //

            lbl_name4.Visible = false;
            txt_nameguest.Visible = false;
            guest.Visible = false;
            lbl_compname.Visible = false;
            txt_compname.Visible = false;
            lbl_desgn.Visible = false;
            txt_desgn.Visible = false;
            lbl_dep.Visible = false;

            txt_dep.Visible = false;
            // lbl_visit1.Visible = false;
            // txt_visit1.Visible = false;

            lbl_mno.Visible = false;
            txt_mno.Visible = false;
            //lbl_phno.Visible = false;

            //txt_phno.Visible = false;
            lbl_str.Visible = false;

            txt_str.Visible = false;
            lbl_cty.Visible = false;

            txt_cty.Visible = false;
            lbl_dis.Visible = false;
            txt_dis.Visible = false;

            lbl_stat.Visible = false;
            txt_stat.Visible = false;

            btn_saveguest.Visible = false;
            btn_exitguest.Visible = false;

            lbl_messname.Visible = false;
            ddl_messname.Visible = false;
            lbl_fromdate.Visible = false;
            txt_admindate.Visible = false;
            lbl_code.Visible = false;
            txt_code.Visible = false;
            ddlmess1.Visible = false;
            lbmess.Visible = false;
            ddlmess.Visible = true;
            Lblmess.Visible = true;
            lblid.Visible = true;
            txtid.Visible = true;
            Llid.Visible = false;
            txtid1.Visible = false;
            lbl_room.Visible = false;
            txt_room.Visible = false;
            roomnum.Visible = false;
            btn2.Visible = false;
            lbl_buildingguest.Visible = false;
            txt_building.Visible = false;
            lbl_floorguest.Visible = false;
            txt_floor.Visible = false;
            lbl_roomtype.Visible = false;
            txt_roomtype.Visible = false;

            lbl_vacate.Visible = false;
            cb_vacate.Visible = false;
            lbl_vacatedate.Visible = false;
            txt_vacatedateguest.Visible = false;

        }
        else
        {

        }


    }
    protected void ddl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
                bindmessmaster1();
          
        }
        catch (Exception ex)
        {
        }
    }
    public void bindmessname()
    {
        try
        {


            ds.Clear();
            //string itemname = "select HostelMasterPK ,HostelName  from HM_HostelMaster  order by HostelMasterPK ";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(itemname, "Text");

            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);

            //magesh 21.6.18
            MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster  order by hostelname";
            ds = d2.select_method_wo_parameter(MessmasterFK, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_messname.DataSource = ds;
                ddl_messname.DataTextField = "HostelName";
                ddl_messname.DataValueField = "HostelMasterPK";
                ddl_messname.DataBind();
            }
            // ddl_messname.Items.Insert(0, "Select");
        }
        catch
        {
        }
    }

    public void btn2_Click(object sender, EventArgs e)
    {
        bindbuilding();
        //bindroompopfloor(buildname);
        //clgroomtype(floor, room);
        //Button4.Visible = false;
        //Button5.Visible = false;
        //Button6.Visible = false;
        btn_pop3save.Visible = false;
        btn_pop3exit.Visible = false;

        chck1.Checked = false;
        popwindow3.Visible = true;
        //ddl_pop3vaccant.SelectedItem.Text = "All";
        ddl_pop3vaccant.SelectedIndex = 0;
        roomchecklist.Items[0].Selected = false;
        roomchecklist.Items[1].Selected = false;
        roomchecklist.Items[2].Selected = false;

        FpSpread3.Visible = false;
        lblerr.Visible = false;
        tblStatusguest.Visible = false;
        toalroomsguest.Visible = false;
        totalvaccantsguest.Visible = false;
        fillguest.Visible = false;
        partialfillguest.Visible = false;
        unfillguest.Visible = false;
        searchguest();
    }
    public void bindbuilding()
    {
        try
        {


            cbl_build.Items.Clear();
            string bul = "";

            bul = d2.GetBuildingCode_inv(ddl_messname.SelectedItem.Value);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_build.DataSource = ds;
                cbl_build.DataTextField = "Building_Name";
                cbl_build.DataValueField = "code";
                cbl_build.DataBind();
            }
            else
            {
                txt_build.Text = "--Select--";
            }

            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                cbl_build.Items[i].Selected = true;
                txt_build.Text = "Building(" + (cbl_build.Items.Count) + ")";
                cb_build.Checked = true;
            }

            string locbuild = "";
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                if (cbl_build.Items[i].Selected == true)
                {
                    string builname = cbl_build.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            bindroompopfloorguest(locbuild);
        }
        catch
        {
        }
    }
    public void cb_build_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_build.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_build.Items.Count; i++)
                {
                    if (cb_build.Checked == true)
                    {
                        cbl_build.Items[i].Selected = true;
                        txt_build.Text = "Build(" + (cbl_build.Items.Count) + ")";
                        txt_floorguest.Text = "--Select--";
                        txt_roomtypeguest.Text = "--Select--";
                        build1 = cbl_build.Items[i].Text.ToString();
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
                bindroompopfloorguest(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_build.Items.Count; i++)
                {
                    cbl_build.Items[i].Selected = false;
                    txt_build.Text = "--Select--";
                    cbl_floor.ClearSelection();
                    cbl_roomtype.ClearSelection();
                    cb_floor.Checked = false;
                    cb_roomtype.Checked = false;
                    txt_floorguest.Text = "--Select--";
                    txt_roomtypeguest.Text = "--Select--";
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_build_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_build.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                if (cbl_build.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_floorguest.Text = "--Select--";
                    txt_roomtypeguest.Text = "--Select--";
                    cb_floor.Checked = false;
                    cb_roomtype.Checked = false;
                    build = cbl_build.Items[i].Text.ToString();
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
            bindroompopfloorguest(buildvalue);
            if (seatcount == cbl_build.Items.Count)
            {
                txt_build.Text = "Build(" + seatcount + ")";
                cb_build.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_build.Text = "--Select--";
            }
            else
            {
                txt_build.Text = "Build(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindroompopfloorguest(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_floor.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floor.DataSource = ds;
                cbl_floor.DataTextField = "Floor_Name";
                cbl_floor.DataValueField = "Floorpk";
                cbl_floor.DataBind();
            }
            else
            {
                txt_floorguest.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                cbl_floor.Items[i].Selected = true;
                cb_floor.Checked = true;
            }

            string locfloor = "";
            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected == true)
                {
                    txt_floorguest.Text = "Floor(" + (cbl_floor.Items.Count) + ")";
                    string flrname = cbl_floor.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }
            }
            clgroomtypeguest(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_floor_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_roomtypeguest.Text = "--Select--";
            if (cb_floor.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";

                if (cb_build.Checked == true)
                {
                    for (int i = 0; i < cbl_build.Items.Count; i++)
                    {
                        build1 = cbl_build.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                    if (cb_floor.Checked == true)
                    {
                        for (int j = 0; j < cbl_floor.Items.Count; j++)
                        {
                            cbl_floor.Items[j].Selected = true;
                            txt_floorguest.Text = "Floor(" + (cbl_floor.Items.Count) + ")";
                            txt_roomtypeguest.Text = "--Select--";
                            build2 = cbl_floor.Items[j].Text.ToString();
                            if (buildvalue2 == "")
                            {

                                buildvalue2 = build2;
                            }
                            else
                            {

                                buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                            }
                        }
                    }
                }
                if (cb_build.Checked == false)
                {

                    for (int i = 0; i < cbl_build.Items.Count; i++)
                    {

                        build1 = cbl_build.Items[i].Text.ToString();
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

                if (cb_floor.Checked == true)
                {
                    for (int j = 0; j < cbl_floor.Items.Count; j++)
                    {
                        cbl_floor.Items[j].Selected = true;
                        txt_floorguest.Text = "Floor(" + (cbl_floor.Items.Count) + ")";
                        txt_roomtypeguest.Text = "--Select--";
                        build2 = cbl_floor.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {

                            buildvalue2 = build2;
                        }
                        else
                        {

                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroomtypeguest(buildvalue1, buildvalue2);
            }
            else
            {
                for (int i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = false;
                    txt_floorguest.Text = "--Select--";
                    cbl_roomtype.ClearSelection();
                    cb_roomtype.Checked = false;
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_floor_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floor.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                if (cbl_build.Items[i].Selected == true)
                {
                    build1 = cbl_build.Items[i].Text.ToString();
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
            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_roomtypeguest.Text = "--Select--";
                    build2 = cbl_floor.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroomtypeguest(buildvalue1, buildvalue2);
            if (seatcount == cbl_floor.Items.Count)
            {
                txt_floorguest.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floor.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorguest.Text = "--Select--";
            }
            else
            {
                txt_floorguest.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void clgroomtypeguest(string floor, string room)
    {
        try
        {

            if (floor != "" && room != "")
            {
                ds.Clear();
                ds = d2.BindRoomtype(floor, room);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_roomtype.DataSource = ds;
                    cbl_roomtype.DataTextField = "Room_Type";
                    cbl_roomtype.DataValueField = "Room_Type";
                    cbl_roomtype.DataBind();
                }
                else
                {
                    txt_roomtypeguest.Text = "--Select--";
                    cbl_roomtype.ClearSelection();
                }
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    cbl_roomtype.Items[i].Selected = true;
                    txt_roomtypeguest.Text = "Room(" + (cbl_roomtype.Items.Count) + ")";
                    cb_roomtype.Checked = true;
                }
            }
            else
            {
                txt_roomtypeguest.Text = "--Select--";
                cbl_roomtype.ClearSelection();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_roomtype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomtype.Checked == true)
            {
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    cbl_roomtype.Items[i].Selected = true;
                }
                txt_roomtypeguest.Text = "Room(" + (cbl_roomtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    cbl_roomtype.Items[i].Selected = false;
                }
                txt_roomtypeguest.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_roomtype_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            //int seatcount = 0;
            //cb_roomtype.Checked = false;
            //for (int i = 0; i < cbl_roomtype.Items.Count; i++)
            //{
            //    if (cbl_roomtype.Items[i].Selected == true)
            //    {
            //        seatcount = seatcount + 1;
            //    }
            //}
            //if (seatcount == cbl_roomtype.Items.Count)
            //{
            //    txt_roomtypeguest.Text = "Room(" + seatcount.ToString() + ")";
            //    cb_roomtype.Checked = true;
            //}
            //else if (seatcount == 0)
            //{
            //    cbl_roomtype.Text = "--Select--";
            //}
            //else
            //{
            //    txt_roomtypeguest.Text = "Room(" + seatcount.ToString() + ")";
            //}
            //   Button2.Focus();
            int seatcount = 0;
            cb_roomtype.Checked = false;
            for (int i = 0; i < cbl_roomtype.Items.Count; i++)
            {
                if (cbl_roomtype.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount == cbl_roomtype.Items.Count)
            {
                txt_roomtypeguest.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomtype.Checked = true;
            }
            else if (seatcount == 0)
            {
                cbl_roomtype.Text = "--Select--";
            }
            else
            {
                txt_roomtypeguest.Text = "Room(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void chck1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chck1.Checked == true)
            {
                for (int i = 0; i < roomchecklist.Items.Count; i++)
                {
                    roomchecklist.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < roomchecklist.Items.Count; i++)
                {
                    roomchecklist.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void roomchecklist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (roomchecklist.Items[0].Selected == false)
            {
                chck1.Checked = false;
            }
            if (roomchecklist.Items[1].Selected == false)
            {
                chck1.Checked = false;
            }
            if (roomchecklist.Items[2].Selected == false)
            {
                chck1.Checked = false;
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void btn_gopop3_Click(object sender, EventArgs e)
    {
        try
        {
            btn_pop3save.Visible = true;
            btn_pop3exit.Visible = true;
            //Button4.Visible = false;
            //Button5.Visible = false;
            //Button6.Visible = false;
            searchguest();
        }
        catch { }
    }
    public void btn_pop3save_Click(object sender, EventArgs e)
    {
        try
        {

            string activerow = "";
            string activecol = "";
            activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
            if (Convert.ToInt32(activerow) != -1 && Convert.ToInt32(activecol) != -1)
            {
                if (Convert.ToInt32(activecol) != 0)
                {
                    //29.10.15
                    string purpose = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text;
                    string room1 = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                    string building_value = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                    if (purpose.Trim() != "")
                    {

                        address = purpose.Split('-');
                        purpose = address[0].ToString();

                        fr = room1.Split('-');
                        string rname = fr[0].ToString();
                        string rtype = "";
                        if (fr.Length == 3)
                        {
                            rtype = fr[1].ToString() + "-" + fr[2].ToString();
                        }
                        else
                        {
                            rtype = fr[1].ToString();
                        }
                        string building = "";

                        string build_name = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(0)].Tag);
                        string q = "select students_allowed,Avl_Student from Room_Detail where Room_Type='" + rtype + "' and Floor_Name='" + fr[0].ToString() + "' and Room_Name='" + purpose + "' and Building_Name='" + build_name + "'";
                        ds2.Clear();
                        ds2 = d2.select_method_wo_parameter(q, "text");
                        string comp1 = Convert.ToString(ds2.Tables[0].Rows[0]["students_allowed"].ToString());
                        string comp2 = Convert.ToString(ds2.Tables[0].Rows[0]["Avl_Student"].ToString());
                        if (comp2 == "")
                            comp2 = "0";

                        if (Convert.ToInt32(comp1) >= Convert.ToInt32(comp2) && Convert.ToInt32(comp1) != Convert.ToInt32(comp2))
                        {

                            if (FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor != Color.GreenYellow)
                            {
                                txt_room.Text = purpose;
                                // string build = Convert.ToString(cbl_build.Items[0].Text);
                                txt_building.Text = building_value;
                                string buil = "";
                                building = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                                buil = d2.GetFunction("select Code  from Building_Master sm where  sm.Building_Name='" + building + "'");
                                ViewState["Code"] = Convert.ToString(buil);
                                string floorroom = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                                fr = floorroom.Split('-');
                                txt_floor.Text = fr[0].ToString();
                                if (txt_pop1floor.Text == "")
                                {
                                    txt_floor.Text = fr[1].ToString();
                                }
                                string fl = "";

                                fl = d2.GetFunction("select Floorpk  from Floor_Master sm where  sm.Floor_Name='" + fr[0] + "'and Building_Name ='" + building + "'");
                                ViewState["Floorpk"] = Convert.ToString(fl);
                                string room = "select Room_Name,Room_type from Room_Detail where Room_Name='" + txt_room.Text + "'";
                                ds = d2.select_method_wo_parameter(room, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string roomtype = ds.Tables[0].Rows[0]["Room_type"].ToString();
                                    txt_roomtype.Text = roomtype;

                                    popwindow3.Visible = false;
                                }

                                string ro = "";

                                ro = d2.GetFunction("select Roompk  from Room_Detail sm where  sm.Room_Name='" + purpose + "' and Building_Name ='" + building + "'");
                                ViewState["Roompk"] = Convert.ToString(ro);
                                if (txt_roomtypeguest.Text == "")
                                {
                                    lblerr.Visible = true;
                                    lblerr.Text = "Please select  room";
                                    txt_floor.Text = "";
                                    txt_building.Text = "";
                                    txt_roomtypeguest.Text = "";
                                }

                            }
                            else
                            {
                                lblerr.Visible = true;
                                lblerr.Text = "Please select unfilled room";
                            }

                            if (FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor != Color.GreenYellow)
                            {
                                txt_room.Text = purpose;

                                //string build = cbl_buildname.SelectedItem.Text.ToString();
                                //TextBox3.Text = build;
                                string floorroom = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                                fr = floorroom.Split('-');
                                txt_floor.Text = fr[0].ToString();
                                txt_roomtype.Text = fr[1].ToString();
                                string fl = "";

                                fl = d2.GetFunction("select Floorpk  from Floor_Master sm where  sm.Floor_Name='" + fr[0] + "'and Building_Name ='" + building + "'");
                                ViewState["Floorpk"] = Convert.ToString(fl);
                                string NON = "NON";
                                if (txt_roomtype.Text == NON)
                                {
                                    txt_roomtype.Text = txt_roomtype.Text + "-AC";
                                    popwindow3.Visible = false;
                                }

                                if (txt_room.Text == "")
                                {
                                    lblerr.Visible = true;
                                    lblerr.Text = "Please select  room";
                                    txt_floor.Text = "";
                                    txt_roomtype.Text = "";
                                    txt_building.Text = "";
                                }

                                string ro = "";

                                ro = d2.GetFunction("select Roompk  from Room_Detail sm where  sm.Room_Name='" + purpose + "' and Building_Name ='" + building + "'");
                                ViewState["Roompk"] = Convert.ToString(ro);
                            }
                            else
                            {
                                lblerr.Visible = true;
                                lblerr.Text = "Please select unfilled room";
                            }

                        }
                        else
                        {
                            lblerr.Visible = true;
                            lblerr.Text = " Room fill please select other room";

                        }
                    }
                    else
                    {
                        lblerr.Visible = true;
                        lblerr.Text = "Please select correct room";
                    }
                }
                else
                {
                    lblerr.Visible = true;
                    lblerr.Text = "Please select correct room";
                }
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please select room";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop3exit_Click(object sender, EventArgs e)
    {
        popwindow3.Visible = false;
        //cbl_buildname.Items.Clear();
        cbl_floor.Items.Clear();
        cbl_roomtype.Items.Clear();
        FpSpread3.Visible = false;
    }
    public void searchguest()//16.04.16
    {
        try
        {
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            toalrooms.Visible = true;
            totalvaccants.Visible = true;
            fill.Visible = true;
            partialfill.Visible = true;
            unfill.Visible = true;

            btn_pop3save.Visible = true;
            btn_pop3exit.Visible = true;

            FpSpread3.Sheets[0].AutoPostBack = false;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.Sheets[0].RowCount = 0;
            string hostelcode = Convert.ToString(ddl_messname.SelectedItem.Value);
            string building = "";

            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    if (building == "")
                    {
                        building = "" + cbl_pop3build.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        building = building + "'" + "," + "'" + cbl_pop3build.Items[i].Text.ToString() + "";
                    }
                }
            }
            //  building = Convert.ToString(cbl_pop3build.SelectedItem.Text);
            string vaccanttype = Convert.ToString(ddl_pop3vaccant.SelectedItem.Text);

            string floor = "";
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {
                if (cbl_pop3floor.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_pop3floor.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_pop3floor.Items[i].Text.ToString() + "";
                    }
                }
            }
            string roomtype0 = "";
            for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
            {
                if (cbl_pop3roomtype.Items[i].Selected == true)
                {
                    if (roomtype0 == "")
                    {
                        roomtype0 = "" + cbl_pop3roomtype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        roomtype0 = roomtype0 + "'" + "," + "'" + cbl_pop3roomtype.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (floor.Trim() != "" && roomtype0.Trim() != "")
            {
                // 16.04.16
                // string selectquery = "Select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student, ISNULL(m.Room_Cost,0) Room_Cost,h.HostelMasterPK,r.Building_Name FROM Room_Detail R left join RoomCost_Master m on r.Room_type = m.Room_Type  left join Building_Master b on b.Building_Name =r.Building_Name left join HM_HostelMaster h on h.collegecode =b.College_Code where R.Building_Name in ('" + building + "') and r.Room_Type in ('" + roomtype0 + "') and Floor_Name in ('" + floor + "') and h.HostelMasterPK ='" + hostelcode + "'";
                string bcode = d2.GetFunction(" select HostelBuildingFK  from HM_HostelMaster where HostelMasterPK ='" + hostelcode + "'");
                string selectquery = " select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student,r.Building_Name,b.College_Code from Building_Master B,Room_Detail R where b.Building_Name =r.Building_Name and b.College_Code =r.College_Code and b.Code in (" + bcode + ")";
                if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Filled")
                {
                    selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
                }
                else if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Un Filled")
                {
                    selectquery = selectquery + " AND R.Avl_Student = 0";
                }
                else if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Partially Filled")
                {
                    selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
                }

                selectquery = selectquery + " Select Distinct F.Floor_Name+' - '+Room_Type RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name in ('" + building + "') AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY F.Floor_Name+' - '+Room_Type";
                selectquery = selectquery + " select ISNULL(Room_Cost,0)as Room_Cost,Hostel_Code,Room_Type  from RoomCost_Master";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");

                int IntRoomLen = 0;
                int totalunfill = 0;
                int totalfill = 0;
                int totalpartialfill = 0;
                int totalvaccant = 0;
                string strRoomDetail = "";
                int colcnt = 0;
                FpSpread3.Sheets[0].ColumnCount = 0;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread3.CommandBar.Visible = false;
                    FpSpread3.Sheets[0].RowHeader.Visible = false;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            FpSpread3.Sheets[0].RowHeader.Visible = false;

                            FpSpread3.Sheets[0].RowCount = FpSpread3.Sheets[0].RowCount + 1;
                            colcnt = 0;

                            if (FpSpread3.Sheets[0].ColumnCount - 1 < colcnt)
                            {
                                FpSpread3.Sheets[0].ColumnCount++;
                            }

                            string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
                            string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
                            string alldetails = floorname + "-" + roomtype;

                            // string buildingname = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);

                            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                            FpSpread3.Sheets[0].Columns[colcnt].CellType = textcel_type;
                            FpSpread3.Sheets[0].Cells[i, colcnt].Text = alldetails;
                            // 29.10.15
                            //  FpSpread3.Sheets[0].Cells[i, colcnt].Tag = ds.Tables[0].Rows[colcnt]["Building_Name"].ToString();

                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[i, 0].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
                            DataView dv = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                //29.02.16
                                //FpSpread3.Sheets[0].Cells[i, colcnt].Tag = Convert.ToString(dv[0]["Building_Name"]);
                                int columncount = dv.Count;
                                for (int cnt = 0; cnt < dv.Count; cnt++)
                                {
                                    colcnt++;
                                    FpSpread3.Sheets[0].Cells[i, cnt].Tag = Convert.ToString(dv[cnt]["Building_Name"]);
                                    string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]);// +Convert.ToString(dv[cnt]["Room_Cost"]);
                                    //24.02.16
                                    DataView cost = new DataView(); string rmcost = "";
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        for (int rmc = 0; rmc < ds.Tables[2].Rows.Count; rmc++)
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = "  Room_Type='" + roomtype + "'";//Hostel_Code='" + hostelcode + "' and
                                            cost = ds.Tables[2].DefaultView;

                                            if (cost.Count > 0)
                                            {
                                                rmcost = Convert.ToString(cost[0]["Room_Cost"]);
                                            }
                                        }
                                    }
                                    if (rmcost.Trim() == "")
                                    {
                                        rmcost = "0";
                                    }
                                    s = s + rmcost;
                                    if (FpSpread3.Sheets[0].ColumnCount - 1 < colcnt)
                                    {
                                        FpSpread3.Sheets[0].ColumnCount = FpSpread3.Sheets[0].ColumnCount + 1;
                                        FpSpread3.Sheets[0].Columns[0].Locked = true;
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "Room Details";
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, FpSpread3.Sheets[0].ColumnCount - 1);
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Bold = true;


                                    }
                                    if (chck1.Checked == true)
                                    {
                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;


                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                            totalunfill = totalunfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                            totalpartialfill = totalpartialfill + 1;
                                        }

                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                            totalfill = totalfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                            totalpartialfill = totalpartialfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                            totalunfill = totalunfill + 1;
                                        }

                                        //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + rmcost.Length;

                                        totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                    }

                                    else
                                    {
                                        try
                                        {
                                            if (chck1.Checked == false)
                                            {

                                                if (roomchecklist.Items[0].Selected == false && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
                                                {
                                                    FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);

                                                    if (IntRoomLen < strRoomDetail.Length)
                                                    {
                                                        IntRoomLen = strRoomDetail.Length;
                                                    }
                                                    FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
                                                    //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;

                                                }
                                            }

                                            if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }

                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);

                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[0].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + rmcost;// (dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }

                                            else if (roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == false && roomchecklist.Items[0].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }

                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == false && roomchecklist.Items[0].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == true)
                                            {
                                                chck1.Checked = true;
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);

                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }

                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {

                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }

                                    for (int j = 1; j < FpSpread3.Sheets[0].ColumnCount; j++)
                                    {
                                        totalvaccants.Text = " ";
                                        toalrooms.Text = " ";
                                        int totalroom = totalunfill + totalfill + totalpartialfill;
                                        toalrooms.Text = "Total No.of Rooms :" + totalroom;
                                        totalvaccants.Text = "Total No.of Vacant :" + totalvaccant;
                                        fill.Text = ("Filled(" + totalfill + ")");
                                        unfill.Text = ("UnFilled(" + totalunfill + ")");
                                        partialfill.Text = ("Partially Filled(" + totalpartialfill + ")");
                                    }
                                }
                                FpSpread3.SaveChanges();
                                FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].ColumnCount;
                                FpSpread3.Sheets[0].FrozenColumnCount = 1;
                                FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                            }
                        }
                        FpSpread3.Visible = true;
                        tblStatus.Visible = true;
                        lblpop3err.Visible = false;
                        lblpop3err.Text = "No Records Found";
                        btn_pop3save.Visible = true;
                        btn_pop3exit.Visible = true;
                    }
                }
                else
                {
                    FpSpread3.Visible = false;
                    tblStatus.Visible = false;
                    lblpop3err.Visible = true;
                    lblpop3err.Text = "No Records Found";
                    btn_pop3save.Visible = false;
                    btn_pop3exit.Visible = false;
                }
            }
            else
            {
                tblStatus.Visible = false;
                FpSpread3.Visible = false;
                lblpop3err.Visible = true;
                lblpop3err.Text = "No Records Found";
                btn_pop3save.Visible = false;
                btn_pop3exit.Visible = false;
            }
        }
        catch
        {
        }
    }

    //public void searchguest()
    //{
    //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //    darkstyle.ForeColor = Color.Black;
    //    FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

    //    toalroomsguest.Visible = true;
    //    totalvaccantsguest.Visible = true;
    //    fillguest.Visible = true;
    //    partialfillguest.Visible = true;
    //    unfillguest.Visible = true;

    //    btn_pop3save.Visible = true;
    //    btn_pop3exit.Visible = true;

    //    FpSpread3.Sheets[0].AutoPostBack = false;
    //    FpSpread3.Sheets[0].ColumnCount = 0;
    //    FpSpread3.Sheets[0].RowCount = 0;
    //    string hostelcode = Convert.ToString(ddl_messname.SelectedValue);
    //    string building = "";
    //    // building = Convert.ToString(cbl_build.SelectedItem.Text);
    //    for (int i = 0; i < cbl_build.Items.Count; i++)
    //    {
    //        if (cbl_build.Items[i].Selected == true)
    //        {
    //            if (building == "")
    //            {
    //                building = "" + cbl_build.Items[i].Text.ToString() + "";
    //            }
    //            else
    //            {
    //                building = building + "'" + "," + "'" + cbl_build.Items[i].Text.ToString() + "";
    //            }
    //        }
    //    }

    //    string vaccanttype = Convert.ToString(ddl_vacant.SelectedItem.Text);

    //    string floor = "";
    //    for (int i = 0; i < cbl_floor.Items.Count; i++)
    //    {
    //        if (cbl_floor.Items[i].Selected == true)
    //        {
    //            if (floor == "")
    //            {
    //                floor = "" + cbl_floor.Items[i].Text.ToString() + "";
    //            }
    //            else
    //            {
    //                floor = floor + "'" + "," + "'" + cbl_floor.Items[i].Text.ToString() + "";
    //            }
    //        }
    //    }
    //    string roomtype0 = "";
    //    for (int i = 0; i < cbl_roomtype.Items.Count; i++)
    //    {
    //        if (cbl_roomtype.Items[i].Selected == true)
    //        {
    //            if (roomtype0 == "")
    //            {
    //                roomtype0 = "" + cbl_roomtype.Items[i].Value.ToString() + "";
    //            }
    //            else
    //            {
    //                roomtype0 = roomtype0 + "'" + "," + "'" + cbl_roomtype.Items[i].Value.ToString() + "";
    //            }
    //        }
    //    }
    //    if (floor.Trim() != "" && roomtype0.Trim() != "")
    //    {
    //        string selectquery = "Select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student, ISNULL(m.Room_Cost,0) Room_Cost,h.HostelMasterPK,r.Building_Name FROM Room_Detail R left join RoomCost_Master m on r.Room_type = m.Room_Type  left join Building_Master b on b.Building_Name =r.Building_Name left join HM_HostelMaster h on h.collegecode =b.College_Code where R.Building_Name in ('" + building + "') and r.Room_Type in ('" + roomtype0 + "') and Floor_Name in ('" + floor + "') and h.HostelMasterPK ='" + hostelcode + "'";
    //        if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Filled")
    //        {
    //            selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
    //        }
    //        else if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Un Filled")
    //        {
    //            selectquery = selectquery + " AND R.Avl_Student = 0";
    //        }
    //        else if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Partially Filled")
    //        {
    //            selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
    //        }

    //        selectquery = selectquery + " Select Distinct F.Floor_Name+' - '+Room_Type RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name in ('" + building + "') AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY F.Floor_Name+' - '+Room_Type";

    //        //Select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student, ISNULL(m.Room_Cost,0) Room_Cost,h.HostelMasterPK,r.Building_Name  FROM Room_Detail R left join RoomCost_Master m on r.Room_type = m.Room_Type  left join Building_Master b on b.Building_Name =r.Building_Name left join HM_HostelMaster h on h.collegecode =b.College_Code where R.Building_Name in ('" + building + "') and r.Room_Type in ('" + roomtype0 + "') and Floor_Name in ('" + floor + "') and h.HostelMasterPK ='" + hostelcode + "'
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(selectquery, "Text");

    //        int IntRoomLen = 0;
    //        int totalunfill = 0;
    //        int totalfill = 0;
    //        int totalpartialfill = 0;
    //        int totalvaccant = 0;
    //        string strRoomDetail = "";
    //        int colcnt = 0;
    //        FpSpread3.Sheets[0].ColumnCount = 0;

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            FpSpread3.CommandBar.Visible = false;
    //            FpSpread3.Sheets[0].RowHeader.Visible = false;
    //            if (ds.Tables[1].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
    //                {
    //                    FpSpread3.Sheets[0].RowHeader.Visible = false;

    //                    FpSpread3.Sheets[0].RowCount = FpSpread3.Sheets[0].RowCount + 1;
    //                    colcnt = 0;

    //                    if (FpSpread3.Sheets[0].ColumnCount - 1 < colcnt)
    //                    {
    //                        FpSpread3.Sheets[0].ColumnCount++;
    //                    }

    //                    string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
    //                    string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
    //                    string alldetails = floorname + "-" + roomtype;

    //                    FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
    //                    FpSpread3.Sheets[0].Columns[colcnt].CellType = textcel_type;
    //                    FpSpread3.Sheets[0].Cells[i, colcnt].Text = alldetails;
    //                    // 29.10.15
    //                    //  FpSpread3.Sheets[0].Cells[i, colcnt].Tag = ds.Tables[0].Rows[colcnt]["Building_Name"].ToString();

    //                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
    //                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
    //                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //                    FpSpread3.Sheets[0].Cells[i, 0].Font.Bold = true;
    //                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
    //                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
    //                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
    //                    DataView dv = new DataView();
    //                    ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
    //                    dv = ds.Tables[0].DefaultView;
    //                    if (dv.Count > 0)
    //                    {

    //                        int columncount = dv.Count;
    //                        for (int cnt = 0; cnt < dv.Count; cnt++)
    //                        {
    //                            FpSpread3.Sheets[0].Cells[i, colcnt].Tag = Convert.ToString(dv[0]["Building_Name"]);
    //                            colcnt++;
    //                            //// 29.10.15
    //                            //FpSpread3.Sheets[0].Cells[i, cnt].Tag = ds.Tables[0].Rows[cnt]["Building_Name"].ToString();
    //                            string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]) + Convert.ToString(dv[cnt]["Room_Cost"]);
    //                            if (FpSpread3.Sheets[0].ColumnCount - 1 < colcnt)
    //                            {
    //                                FpSpread3.Sheets[0].ColumnCount = FpSpread3.Sheets[0].ColumnCount + 1;
    //                                FpSpread3.Sheets[0].Columns[0].Locked = true;
    //                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "Room Details";
    //                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
    //                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                                FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, FpSpread3.Sheets[0].ColumnCount - 1);
    //                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
    //                                FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Bold = true;


    //                            }
    //                            if (chck1.Checked == true)
    //                            {
    //                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
    //                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;


    //                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                {
    //                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                    totalunfill = totalunfill + 1;
    //                                }
    //                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                {
    //                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                    totalpartialfill = totalpartialfill + 1;
    //                                }

    //                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                {
    //                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                    totalfill = totalfill + 1;
    //                                }
    //                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                {
    //                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                    totalpartialfill = totalpartialfill + 1;
    //                                }
    //                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                {
    //                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                    totalunfill = totalunfill + 1;
    //                                }

    //                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
    //                            }

    //                            else
    //                            {
    //                                try
    //                                {
    //                                    if (chck1.Checked == false)
    //                                    {

    //                                        if (roomchecklist.Items[0].Selected == false && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
    //                                        {
    //                                            FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
    //                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                            {
    //                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                                totalunfill = totalunfill + 1;
    //                                            }
    //                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                            {
    //                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                                totalpartialfill = totalpartialfill + 1;
    //                                            }
    //                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                            {
    //                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                                totalfill = totalfill + 1;
    //                                            }
    //                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                            {
    //                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                                totalpartialfill = totalpartialfill + 1;
    //                                            }
    //                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
    //                                            {
    //                                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                                totalunfill = totalunfill + 1;
    //                                            }
    //                                            strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);

    //                                            if (IntRoomLen < strRoomDetail.Length)
    //                                            {
    //                                                IntRoomLen = strRoomDetail.Length;
    //                                            }
    //                                            FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                        }
    //                                    }

    //                                    if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
    //                                    {
    //                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }

    //                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);

    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }

    //                                    else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == false)
    //                                    {
    //                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }

    //                                    else if (roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[0].Selected == false)
    //                                    {
    //                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }

    //                                    else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == false)
    //                                    {
    //                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Room_Cost"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }

    //                                    else if (roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == false && roomchecklist.Items[0].Selected == false)
    //                                    {
    //                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }

    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }
    //                                    else if (roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == false && roomchecklist.Items[0].Selected == false)
    //                                    {
    //                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Room_Cost"]);
    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }
    //                                    else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == true)
    //                                    {
    //                                        chck1.Checked = true;
    //                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
    //                                        if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
    //                                        {
    //                                            IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
    //                                        }
    //                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);

    //                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }

    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
    //                                            totalfill = totalfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
    //                                        {
    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
    //                                            totalpartialfill = totalpartialfill + 1;
    //                                        }
    //                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
    //                                        {

    //                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
    //                                            totalunfill = totalunfill + 1;
    //                                        }
    //                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
    //                                    }
    //                                    totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                }
    //                            }

    //                            for (int j = 1; j < FpSpread3.Sheets[0].ColumnCount; j++)
    //                            {
    //                                totalvaccantsguest.Text = " ";
    //                                toalroomsguest.Text = " ";
    //                                int totalroom = totalunfill + totalfill + totalpartialfill;
    //                                toalroomsguest.Text = "Total No.of Rooms :" + totalroom;
    //                                totalvaccantsguest.Text = "Total No.of Vacant :" + totalvaccant;
    //                                fillguest.Text = ("Filled(" + totalfill + ")");
    //                                unfillguest.Text = ("UnFilled(" + totalunfill + ")");
    //                                partialfillguest.Text = ("Partially Filled(" + totalpartialfill + ")");
    //                            }
    //                        }

    //                        //int height = 60;
    //                        //{
    //                        //    for (int j = 1; j < FpSpread3.Sheets[0].RowCount; j++)
    //                        //    {
    //                        //        height = height + FpSpread3.Sheets[0].Rows[j].Height;
    //                        //    }
    //                        //    FpSpread3.Height = height;
    //                        //    FpSpread3.SaveChanges();
    //                        //    //FpSpread3.Sheets[0].FrozenColumnCount = 1;
    //                        //    FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
    //                        //}

    //                        //int width = 0;
    //                        //if (FpSpread3.Sheets[0].ColumnCount == 7)
    //                        //{
    //                        //    FpSpread3.Sheets[0].Columns[0].Width = 400;
    //                        //    for (int j = 1; j < FpSpread3.Sheets[0].ColumnCount; j++)
    //                        //    {
    //                        //        width = width + FpSpread3.Sheets[0].Columns[j].Width;

    //                        //    }
    //                        //    width = width + 400;
    //                        //}

    //                        //else if (FpSpread3.Sheets[0].ColumnCount == 5)
    //                        //{
    //                        //    FpSpread3.Sheets[0].Columns[0].Width = 800;
    //                        //    for (int j = 1; j < FpSpread3.Sheets[0].ColumnCount; j++)
    //                        //    {
    //                        //        width = width + FpSpread3.Sheets[0].Columns[j].Width;
    //                        //    }
    //                        //    width = width + 800;
    //                        //}
    //                        //else
    //                        //{
    //                        //    width = 770;
    //                        //}
    //                        //FpSpread3.Width = width;
    //                        FpSpread3.SaveChanges();
    //                        FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].ColumnCount;
    //                        FpSpread3.Sheets[0].FrozenColumnCount = 1;
    //                        FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
    //                    }
    //                }
    //                FpSpread3.Visible = true;
    //                tblStatusguest.Visible = true;
    //                lblerr.Visible = false;
    //                lblerr.Text = "No Records Found";
    //                btn_pop3save.Visible = true;
    //                btn_pop3exit.Visible = true;
    //            }
    //        }
    //        else
    //        {
    //            FpSpread3.Visible = false;
    //            tblStatusguest.Visible = false;
    //            lblerr.Visible = true;
    //            lblerr.Text = "No Records Found";
    //            btn_pop3save.Visible = false;
    //            btn_pop3exit.Visible = false;
    //        }
    //    }
    //    else
    //    {
    //        tblStatusguest.Visible = false;
    //        FpSpread3.Visible = false;
    //        lblerr.Visible = true;
    //        lblerr.Text = "Please Select All Field";
    //        btn_pop3save.Visible = false;
    //        btn_pop3exit.Visible = false;
    //    }
    //}
    protected void imagebtnpop3closeguest_Click(object sender, EventArgs e)
    {
        popwindow3.Visible = false;
    }

    public void cb_vacate_CheckedChange(object sender, EventArgs e)
    {
        if (cb_vacate.Checked == true)
        {
            txt_vacatedateguest.Enabled = true;
            txt_vacatedateguest.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        else
        {

            txt_vacatedateguest.Enabled = false;

        }
    }
    public void bindcode()
    {
        try
        {
            string newitemcode = "";
            string selectquery = "select VenAcr,VenStNo,VenSize,CustAcr,CustStNo,CustSize  from IM_CodeSettings";
            selectquery = selectquery + " select distinct VendorCode from CO_VendorMaster order by VendorCode desc";
            ds.Clear();
            ds = da.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[1].Rows.Count > 0)
            {
                string itemcode = Convert.ToString(ds.Tables[1].Rows[0]["VendorCode"]);
                string itemacr = Convert.ToString(ds.Tables[0].Rows[0]["VenAcr"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["VenSize"]);
                int len = itemacr.Length;
                itemcode = itemcode.Remove(0, len);

                int len1 = Convert.ToString(itemcode).Length;

                string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                //newitemcode = itemacr + "" + newnumber;
                //
                len = Convert.ToString(newnumber).Length;
                len1 = Convert.ToInt32(itemsize) - len;//5.11.15 // len1 = len1 - len;
                if (len1 == 2)
                {
                    newitemcode = "00" + newnumber;
                }
                else if (len1 == 1)
                {
                    newitemcode = "0" + newnumber;
                }
                else if (len1 == 4)
                {
                    newitemcode = "0000" + newnumber;
                }
                else if (len1 == 3)
                {
                    newitemcode = "000" + newnumber;
                }
                else if (len1 == 5)
                {
                    newitemcode = "00000" + newnumber;
                }
                else if (len1 == 6)
                {
                    newitemcode = "000000" + newnumber;
                }
                else
                {
                    newitemcode = Convert.ToString(newnumber);
                }
                if (newitemcode.Trim() != "")
                {
                    newitemcode = itemacr + "" + newitemcode;
                }

                //

            }
            else
            {
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["VenStNo"]);
                string itemacr = Convert.ToString(ds.Tables[0].Rows[0]["VenAcr"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["VenSize"]);


                string newnumber = Convert.ToString((Convert.ToInt32(itemstarno) + 1));
                int len = newnumber.Length;

                //itemstarno = itemstarno.Remove(0, len);

                //itemacr = Convert.ToString(itemstarno);

                string items = Convert.ToString(itemsize);
                int len1 = Convert.ToInt32(items);

                int size = len1 - len;

                if (size == 2)
                {
                    newitemcode = "00" + newnumber;
                }
                else if (size == 1)
                {
                    newitemcode = "0" + newnumber;
                }
                else if (size == 4)
                {
                    newitemcode = "0000" + newnumber;
                }
                else if (size == 3)
                {
                    newitemcode = "000" + newnumber;
                }
                else if (size == 5)
                {
                    newitemcode = "00000" + newnumber;
                }
                else if (size == 6)
                {
                    newitemcode = "000000" + newnumber;
                }
                else
                {
                    newitemcode = Convert.ToString(itemstarno);
                }
                newitemcode = Convert.ToString(itemacr) + "" + Convert.ToString(newitemcode);
                //newitemcode = Convert.ToString(ds.Tables[0].Rows[0]["VenAcr"]) + "" + Convert.ToString(ds.Tables[0].Rows[0]["VenStNo"]);
            }
            txt_code.Text = Convert.ToString(newitemcode);
        }
        catch
        {
        }
    }
    protected void btn_saveguest_Click(object sender, EventArgs e)
    {
        try
        {
            string dtaccessdate = "";
            string dtaccesstime = "";
            string date = "";
            string getday = "";
            string guestname = "";
            string guestaddress = "";
            string mobno = "";
            string company = "";
            string desi = "";
            string desicode = "";

            string builname = "";
            string floor = "";
            string room = "";
            string city = "";
            string dept = "";
            string deptcode = "";
            string dist = "";
            string distcode = "";
            string state = "";
            string statecode = "";
            string roomtype = "";
            dtaccessdate = DateTime.Now.ToString();
            dtaccesstime = DateTime.Now.ToLongTimeString();
            date = Convert.ToString(txt_admindate.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            getday = dt.ToString("MM/dd/yyyy");

            state = Convert.ToString(txt_stat.Text);
            statecode = subjectcode("State", state);

            desi = Convert.ToString(txt_desgn.Text);
            // desicode = subjectcode("Gudis", desi);
            dept = Convert.ToString(txt_dep.Text);
            //  deptcode = subjectcode("Gudep", dept);

            dist = Convert.ToString(txt_dis.Text);
            distcode = subjectcode("District", dist);


            guestname = Convert.ToString(txt_nameguest.Text);
            guestname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(guestname);
            guestaddress = Convert.ToString(txt_str.Text);
            guestaddress = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(guestaddress);
            mobno = Convert.ToString(txt_mno.Text);
            company = Convert.ToString(txt_compname.Text);
            company = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(company);


            builname = Convert.ToString(txt_building.Text);
            floor = Convert.ToString(txt_floor.Text);
            room = Convert.ToString(txt_room.Text);
            city = Convert.ToString(txt_cty.Text);
            city = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(city);


            roomtype = Convert.ToString(txt_roomtype.Text);
            roomtype = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(roomtype);

            string vendarcode = Convert.ToString(txt_code.Text);
            string hostelcode = Convert.ToString(ddl_messname.SelectedItem.Value);
            string query = "";
            string appnumber = "";
            string building = "";
            string floo = "";
            string roomno = "";//delsi

            //string expansetype = "";
            
            //if (rdb_veg.Checked == true)
            //{
            //    expansetype = "0";
            //}
            //else if (rdb_NonVeg.Checked == true)
            //{
            //    expansetype = "1";
            //}
            //magesh 12.3.18
            string studmesstype = string.Empty;
            int messtype = 0;
            int.TryParse(Convert.ToString(ddlguest.SelectedValue), out messtype);
            studmesstype = Convert.ToString(messtype - 1);//magesh 12.3.18



            building = Convert.ToString(ViewState["Code"]);
            floo = Convert.ToString(ViewState["Floorpk"]);
            roomno = Convert.ToString(ViewState["Roompk"]);
           

            string query1 = "";
            query1 = "insert into CO_VendorMaster(VendorAddress,VendorCompName,VendorCity,VendorType,VendorDist,VendorState,VendorCode)values('" + guestaddress + "','" + company + "','" + city + "','10','" + distcode + "','" + statecode + "','" + txt_code.Text + "')";
            int iv1 = d2.update_method_wo_parameter(query1, "Text");

            string venfk = d2.getvenpk(vendarcode);
            query = "insert into IM_VendorContactMaster(VenContactName,VendorMobileNo,VenContactDesig,VenContactDept,VendorFK) values('" + guestname + "','" + mobno + "','" + desi + "','" + dept + "','" + venfk + "')";
            int iv = d2.update_method_wo_parameter(query, "Text");


            appnumber = d2.GetFunction("select im.VendorContactPK from IM_VendorContactMaster im,CO_VendorMaster co where co.VendorType='10'  and im.VenContactName='" + guestname + "' and co.VendorCode='" + txt_code.Text + "' and co.VendorPK=im.VendorFK");
            ViewState["VendorContactPK"] = Convert.ToString(appnumber);
            string query2 = "";
            query2 = "insert into HT_HostelRegistration(BuildingFK,FloorFK,RoomFK,StudMessType,HostelMasterFK,HostelAdmDate,IsVacated,VacatedDate,GuestVendorType,MemType,APP_No,GuestVendorFK,Messcode,id)values('" + building + "','" + floo + "','" + roomno + "','" + studmesstype + "','" + hostelcode + "','" + dt.ToString("MM/dd/yyyy") + "','0','','10','3','" + appnumber + "','" + venfk + "','"+Convert.ToString(ddlmess1.SelectedValue)+"','"+txtid1.Text+"')";

            //string query = "insert into Hostel_GuestReg (Access_Date,Access_Time,Guest_Name,Guest_Address,MobileNo,From_Company,Desig_Code,Building_Name,Floor_Name ,Room_Name,Hostel_Code,college_code,Admission_Date,Guest_City,room_type,State,department,district,isvacate ,vacate_date ) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + guestname + "'  ,'" + guestaddress + "','" + mobno + "','" + company + "','" + desicode + "','" + builname + "','" + floor + "','" + room + "','" + hostelcode + "','" + collegecode1 + "'  ,'" + dt.ToString("MM/dd/yyyy") + "','" + city + "','" + roomtype + "','" + statecode + "','" + deptcode + "','" + distcode + "','0','')";
            int iv2 = d2.update_method_wo_parameter(query2, "Text");
            if (iv2 != 0 && iv != 0 && iv1 != 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Saved Successfully";

                clear();

                popwindow1.Visible = true;
                bindcode();
                // btn2_Click(sender, e);
                idgeneration();

            }
        }
        catch
        {
        }
    }
    public void clear()
    {
        bindmessname();
        //btn_delete.Visible = false;
        //btn_update.Visible = false;

        txt_nameguest.Text = "";
        //txt_phno.Text = "";
        // txt_code.Text = "";
        txt_compname.Text = "";
        txt_str.Text = "";
        txt_desgn.Text = "";
        txt_cty.Text = "";
        txt_dep.Text = "";
        txt_dis.Text = "";
        txt_mno.Text = "";
        txt_stat.Text = "";
        txt_room.Text = "";
        txt_building.Text = "";
        txt_floor.Text = "";
        txt_roomtype.Text = "";
        txt_admindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //22.12.15 add
        txt_discontinuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_vacatedateguest.Text = DateTime.Now.ToString("dd/MM/yyyy");



    }
    protected void btn_exitguest_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void rdb_staffe_Select(object sender, EventArgs e)
    {
        if (rdb_staffe.Checked == true)
        {
            stafftrue();
            pheaderfilterguest.Visible = false;
            pcolumnorderguest.Visible = false;
            Fpspread2.Visible = false;
            Div4.Visible = false;
            ////guestfalse();
            rptprint.Visible = false;

            //pheaderfilter.Visible = true;
            //pcolumnorder.Visible = true;
            //Fpspread1.Visible = true;
            //Divspread.Visible = true;
        }
    }
    protected void rdb_gueste_select(object sender, EventArgs e)
    {
        if (rdb_gueste.Checked == true)
        {
            guesttrue();
            // stafffalse();
            rptprint.Visible = false;

            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            Fpspread1.Visible = false;
            Divspread.Visible = false;

            //pheaderfilterguest.Visible = true;
            //pcolumnorderguest.Visible = true;


        }
    }
    protected void stafffalse()
    {
        lbl_collegename.Visible = false;
        ddl_collegename.Visible = false;
        lbl_department.Visible = false;

        txt_department.Visible = false;
        panel_department.Visible = false;
        lbl_designation.Visible = false;
        txt_designation.Visible = false;
        panel_designation.Visible = false;

        lbl_stafftype.Visible = false;
        txt_stafftype.Visible = false;
        panel_stafftype.Visible = false;
        lbl_hostelname.Visible = false;


        txt_hostelname.Visible = false;
        panel_hostelname.Visible = false;
        lbl_building.Visible = false;

        txt_buildingname.Visible = false;
        panel_building.Visible = false;
        lbl_floorname.Visible = false;
        txt_floorname.Visible = false;
        panel_floorname.Visible = false;

        lbl_roomname.Visible = false;
        txt_roomname.Visible = false;
        panel_roomname.Visible = false;
        lbl_staffname.Visible = false;


        txt_staffname.Visible = false;
        lbl_staffcode.Visible = false;
        txt_staffcode.Visible = false;
        lbl_searchbystaff.Visible = false;
        lbl_searchbystaffcode.Visible = false;

    }
    protected void stafftrue()
    {
        lbl_collegename.Visible = true;
        ddl_collegename.Visible = true;
        lbl_department.Visible = true;

        txt_department.Visible = true;
        panel_department.Visible = true;
        lbl_designation.Visible = true;
        txt_designation.Visible = true;
        panel_designation.Visible = true;

        lbl_stafftype.Visible = true;
        txt_stafftype.Visible = true;
        panel_stafftype.Visible = true;
        lbl_hostelname.Visible = true;


        txt_hostelname.Visible = true;
        panel_hostelname.Visible = true;
        lbl_building.Visible = true;

        txt_buildingname.Visible = true;
        panel_building.Visible = true;
        lbl_floorname.Visible = true;
        txt_floorname.Visible = true;
        panel_floorname.Visible = true;

        lbl_roomname.Visible = true;
        txt_roomname.Visible = true;
        panel_roomname.Visible = true;
        lbl_staffname.Visible = true;


        txt_staffname.Visible = true;
        lbl_staffcode.Visible = true;
        txt_staffcode.Visible = true;
        lbl_searchbystaff.Visible = true;
        lbl_searchbystaffcode.Visible = true;
        bindroompopbuild();


        loadcollege();
        loadhostel();
        binddepartment();
        binddesignation();
        bindstafftype();

    }

    protected void guestfalse()
    {
        lbl_collegename.Visible = true;
        ddl_collegename.Visible = true;
        lbl_department.Visible = true;

        txt_department.Visible = true;
        panel_department.Visible = true;
        lbl_designation.Visible = true;
        txt_designation.Visible = true;
        panel_designation.Visible = true;

        lbl_stafftype.Visible = true;
        txt_stafftype.Visible = true;
        panel_stafftype.Visible = true;
        lbl_hostelname.Visible = true;


        txt_hostelname.Visible = true;
        panel_hostelname.Visible = true;
        lbl_building.Visible = true;

        txt_buildingname.Visible = true;
        panel_building.Visible = true;
        lbl_floorname.Visible = true;
        txt_floorname.Visible = true;
        panel_floorname.Visible = true;

        lbl_roomname.Visible = true;
        txt_roomname.Visible = true;
        panel_roomname.Visible = true;
        lbl_staffname.Visible = true;


        txt_staffname.Visible = true;
        lbl_staffcode.Visible = true;
        txt_staffcode.Visible = true;
        lbl_searchbystaff.Visible = true;
        lbl_searchbystaffcode.Visible = true;
        bindroompopbuild();


        loadcollege();
        loadhostel();
        binddepartment();
        binddesignation();
        bindstafftype();

    }
    protected void guesttrue()
    {
        lbl_collegename.Visible = true;
        ddl_collegename.Visible = true;
        lbl_department.Visible = false;

        txt_department.Visible = false;
        panel_department.Visible = false;
        lbl_designation.Visible = false;
        txt_designation.Visible = false;
        panel_designation.Visible = false;

        lbl_stafftype.Visible = false;
        txt_stafftype.Visible = false;
        panel_stafftype.Visible = false;
        lbl_hostelname.Visible = true;


        txt_hostelname.Visible = true;
        panel_hostelname.Visible = true;
        lbl_building.Visible = true;

        txt_buildingname.Visible = true;
        panel_building.Visible = true;
        lbl_floorname.Visible = true;
        txt_floorname.Visible = true;
        panel_floorname.Visible = true;

        lbl_roomname.Visible = true;
        txt_roomname.Visible = true;
        panel_roomname.Visible = true;
        lbl_staffname.Visible = false;


        txt_staffname.Visible = false;
        lbl_staffcode.Visible = false;
        txt_staffcode.Visible = false;
        lbl_searchbystaff.Visible = false;
        lbl_searchbystaffcode.Visible = false;
        bindroompopbuild();


        loadcollege();
        loadhostel();
        // binddepartment();
        // binddesignation();
        // bindstafftype();

    }

    protected void CheckBox_columnguest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_columnguest.Checked == true)
            {
                ItemListguest.Clear();
                for (int i = 0; i < cblcolumnorderguest.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorderguest.Items[i].Selected = true;
                    lnk_columnorderguest.Visible = true;
                    ItemListguest.Add(cblcolumnorderguest.Items[i].Value.ToString());
                    Itemindexguest.Add(si);
                }
                lnk_columnorderguest.Visible = true;
                tborderguest.Visible = true;
                tborderguest.Text = "";
                int j = 0;
                for (int i = 0; i < ItemListguest.Count; i++)
                {
                    j = j + 1;
                    tborderguest.Text = tborderguest.Text + ItemListguest[i].ToString();

                    tborderguest.Text = tborderguest.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorderguest.Items.Count; i++)
                {
                    cblcolumnorderguest.Items[i].Selected = false;
                    lnk_columnorderguest.Visible = false;
                    ItemListguest.Clear();
                    Itemindexguest.Clear();
                    cblcolumnorderguest.Items[0].Enabled = false;
                }

                tborderguest.Text = "";
                tborderguest.Visible = false;

            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    protected void LinkButtonsremoveguest_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorderguest.ClearSelection();
            CheckBox_columnguest.Checked = false;
            lnk_columnorderguest.Visible = false;
            //cblcolumnorderguest.Items[0].Selected = true;
            ItemListguest.Clear();
            Itemindexguest.Clear();
            tborderguest.Text = "";
            tborderguest.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblcolumnorderguest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_columnguest.Checked = false;
            string value = "";
            int index;
            cblcolumnorderguest.Items[0].Selected = true;
            // cblcolumnorderguest.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorderguest.Items[index].Selected)
            {
                if (!Itemindexguest.Contains(sindex))
                {

                    ItemListguest.Add(cblcolumnorderguest.Items[index].Value.ToString());
                    Itemindexguest.Add(sindex);
                }
            }
            else
            {
                ItemListguest.Remove(cblcolumnorderguest.Items[index].Value.ToString());
                Itemindexguest.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorderguest.Items.Count; i++)
            {

                if (cblcolumnorderguest.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemListguest.Remove(cblcolumnorderguest.Items[i].Value.ToString());
                    Itemindexguest.Remove(sindex);
                }
            }

            lnk_columnorderguest.Visible = true;
            tborderguest.Visible = true;
            tborderguest.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemListguest.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemListguest[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemListguest[i].ToString() + "(" + (i + 1).ToString() + ")";
                }

            }
            tborderguest.Text = colname12;
            if (ItemListguest.Count == 11)
            {
                CheckBox_columnguest.Checked = true;
            }
            if (ItemListguest.Count == 0)
            {
                tborderguest.Visible = false;
                lnk_columnorderguest.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Cell_Clickguest(object sender, EventArgs e)
    {
        try
        {
            checkguest = true;
        }
        catch
        {
        }
    }
    protected void Fpspread2_render(object sender, EventArgs e)
    {
        try
        {
            if (checkguest == true)
            {

                popwindow1.Visible = true;
                //  txt_vacatedateguest.Enabled = true;
                // txt_vacatedateguest.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //rdb_NonVeg.Visible = true;
                ddlguest.Visible = true;
                lblGuestType.Visible = true;
                cb_vacate.Checked = false;
                rdb_staff.Checked = false;
                rdb_staff.Enabled = false;
                rdb_guest.Checked = true;
                rdb_guest.Enabled = true;
                btn_delguest.Visible = true;
                btn_updateguest.Visible = true;
                btn_saveguest.Visible = false;
                btn_exitguest.Visible = true;
                btn_pop1update.Visible = false;
                btn_pop1delete.Visible = false;
                btn_pop1exit1.Visible = false;
                lbl_name4.Visible = true;
                txt_nameguest.Visible = true;
                guest.Visible = true;
                lbl_compname.Visible = true;
                txt_compname.Visible = true;
                lbl_desgn.Visible = true;
                txt_desgn.Visible = true;
                lbl_dep.Visible = true;

                txt_dep.Visible = true;
                // lbl_visit1.Visible = true;
                // txt_visit1.Visible = true;

                lbl_mno.Visible = true;
                txt_mno.Visible = true;
                //lbl_phno.Visible = true;

                //txt_phno.Visible = true;
                lbl_str.Visible = true;

                txt_str.Visible = true;
                lbl_cty.Visible = true;

                txt_cty.Visible = true;
                lbl_dis.Visible = true;
                txt_dis.Visible = true;

                lbl_stat.Visible = true;
                txt_stat.Visible = true;

                //btn_saveguest.Visible = true;
                //btn_exitguest.Visible = true;

                lbl_messname.Visible = true;
                ddl_messname.Visible = true;
                lbl_fromdate.Visible = true;
                txt_admindate.Visible = true;
                lbl_code.Visible = true;
                txt_code.Visible = true;
                ddlmess1.Visible = true;
                lbmess.Visible = true;
                ddlmess.Visible = false;
                Lblmess.Visible = false;
                lblid.Visible = false;
                txtid.Visible = false;
                Llid.Visible = true;
                txtid1.Visible = true;
                lbl_room.Visible = true;
                txt_room.Visible = true;
                roomnum.Visible = true;
                btn2.Visible = true;
                lbl_buildingguest.Visible = true;
                txt_building.Visible = true;
                lbl_floorguest.Visible = true;
                txt_floor.Visible = true;
                lbl_roomtype.Visible = true;
                txt_roomtype.Visible = true;

                lbl_vacate.Visible = true;
                cb_vacate.Visible = true;
                lbl_vacatedate.Visible = true;
                txt_vacatedateguest.Visible = true;

                //

                lbl_pop1collegename.Visible = false;
                ddl_pop1collegename.Visible = false;
                lbl_pop1hostelname.Visible = false;
                ddl_pop1hostelname.Visible = false;
                staff.Visible = false;
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                staffnamebtn.Visible = false;

                btn_staffquestion.Visible = false;
                lbl_pop1staffcode.Visible = false;
                txt_pop1staffcode.Visible = false;

                lbl_pop1department.Visible = false;
                txt_pop1department.Visible = false;
                lbl_pop1designation.Visible = false;

                txt_pop1designation.Visible = false;
                lbl_pop1dob.Visible = false;


                txt_pop1dob.Visible = false;
                lbl_pop1admindate.Visible = false;
                txt_pop1admindate.Visible = false;
                lbl_pop1roomno.Visible = false;
                txt_pop1roomno.Visible = false;
                roomno.Visible = false;
                btn_roomques.Visible = false;
                lbl_pop1messtype.Visible = false;

                ddlStudType.Visible = false;
                lbl_pop1building.Visible = false;
                txt_pop1building.Visible = false;

                lbl_pop1floor.Visible = false;
                txt_pop1floor.Visible = false;
                lbl_pop1roomtype.Visible = false;
                txt_pop1roomtype.Visible = false;
                lbl_pop1discontinue.Visible = false;
                cb_discontinue.Visible = false;
                lbl_pop1date.Visible = false;
                txt_discontinuedate.Visible = false;

                lbl_pop1reason.Visible = false;
                txt_pop1reason.Visible = false;
                lbl_pop1vacate.Visible = false;
                cb_pop1vacate.Visible = false;
                txt_vacatedate.Visible = false;
                btn_pop1save.Visible = false;
                btn_pop1exit.Visible = false;
                cb_vacate.Enabled = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                DataView dv1 = new DataView();
                string building = "";
                string floor = "";
                string roomnumber = "";

                if (activerow.Trim() != "" && activecol != "0")
                {
                    string guestname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    txt_nameguest.Text = Convert.ToString(guestname);
                    //   string hostelcode = d2.GetFunction("select HostelMasterFK from HT_HostelRegistration,IM_VendorContactMaster where im.VenContactName='" + guestname + "' ");
                    //  bindmessname();
                    //  ddl_messname.SelectedIndex = ddl_messname.Items.IndexOf(ddl_messname.Items.FindByValue(hostelcode));

                    // ddl_messname.SelectedItem.Text = Convert.ToString(hostelcode);
                    //string sql = "select convert(varchar(10),Admission_Date ,103)as Admission_Date,Guest_Name,From_Company,Desig_Code,department,Guest_Address,Guest_City,district,State,MobileNo ,gr.Hostel_Code,hd.Hostel_Name,bm.Building_Name,Floor_Name,Room_Name,room_type,isvacate,CONVERT(varchar(10), vacate_date,103) as vacate_date,GuestCode  from Hostel_GuestReg gr,Hostel_Details hd,Building_Master bm where gr.Hostel_Code=hd.Hostel_code and bm.Building_Name=gr.Building_Name and gr.Guest_Name='" + guestname + "'";
                    string text = "select distinct HostelMasterFK,VendorCode,gr.StudMessType,convert(varchar(10),HostelAdmDate ,103)as Admission_Date,VenContactName,VendorCompName,VenContactDesig,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,gr.HostelMasterFK,hd.HostelName,BuildingFK,FloorFK,RoomFK,IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No,gr.Messcode,gr.id  from HT_HostelRegistration gr,HM_HostelMaster hd,CO_VendorMaster co,IM_VendorContactMaster im where gr.HostelMasterFK=hd.HostelMasterPK and co.VendorPK=im.VendorFK and im.VendorFK=gr.GuestVendorFK and im.VenContactName='" + guestname + "'";
                    text = text + " select Building_Name,Code  from Building_Master";
                    text = text + " select Floor_Name,Floorpk  from Floor_Master";
                    text = text + " select Room_Name,Roompk from Room_Detail";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(text, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_messname.SelectedIndex = ddl_messname.Items.IndexOf(ddl_messname.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"])));
                        txtid1.Text = Convert.ToString(ds.Tables[0].Rows[0]["id"]);

                        txt_admindate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Admission_Date"]);
                        txt_compname.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorCompName"]);
                        ddl_messname_SelectedIndexChanged(sender,e);

                        ddlmess1.SelectedIndex = ddlmess1.Items.IndexOf(ddlmess1.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Messcode"])));
                        int messtype = Convert.ToInt16(ds.Tables[0].Rows[0]["StudMessType"]);
                        //ddlStudType.SelectedIndex = ddlStudType.Items.IndexOf(ddlStudType.Items.FindByValue(Convert.ToString(messtype + 1)));
                        ddlguest.SelectedIndex = ddlguest.Items.IndexOf(ddlguest.Items.FindByValue(Convert.ToString(messtype+1)));
                        txt_str.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorAddress"]);
                        txt_cty.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorCity"]);

                        txt_mno.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorMobileNo"]);
                        // txt_building.Text = Convert.ToString(ds.Tables[0].Rows[0]["Building_Name"]);
                        // txt_floor.Text = Convert.ToString(ds.Tables[0].Rows[0]["Floor_Name"]);

                        // txt_room.Text = Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]);
                        building = ds.Tables[0].Rows[0]["BuildingFK"].ToString();
                        ViewState["Code"] = Convert.ToString(building);
                        if (building != "")
                        {
                            ds.Tables[1].DefaultView.RowFilter = "Code in (" + building + ")";
                            dv1 = ds.Tables[1].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int row = 0; row < dv1.Count; row++)
                                {
                                    build1 = Convert.ToString(dv1[row]["Building_Name"]);
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
                            txt_building.Text = Convert.ToString(buildvalue1);
                        }
                        floor = ds.Tables[0].Rows[0]["FloorFK"].ToString();
                        ViewState["Floorpk"] = Convert.ToString(floor);
                        if (floor != "")
                        {
                            string build2 = "";
                            string buildvalue2 = "";
                            ds.Tables[2].DefaultView.RowFilter = "Floorpk in (" + floor + ")";
                            dv1 = ds.Tables[2].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int row = 0; row < dv1.Count; row++)
                                {
                                    build2 = Convert.ToString(dv1[row]["Floor_Name"]);
                                    if (buildvalue2 == "")
                                    {
                                        buildvalue2 = build2;
                                    }
                                    else
                                    {
                                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                                    }
                                }
                            }
                            txt_floor.Text = Convert.ToString(buildvalue2);
                        }
                        roomnumber = ds.Tables[0].Rows[0]["RoomFK"].ToString();
                        ViewState["Roompk"] = Convert.ToString(roomnumber);
                        if (roomnumber != "")
                        {
                            string build3 = "";
                            string buildvalue3 = "";
                            ds.Tables[3].DefaultView.RowFilter = "Roompk in (" + roomnumber + ")";
                            dv1 = ds.Tables[3].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int row = 0; row < dv1.Count; row++)
                                {
                                    build3 = Convert.ToString(dv1[row]["Room_Name"]);
                                    if (buildvalue3 == "")
                                    {
                                        buildvalue3 = build3;
                                    }
                                    else
                                    {
                                        buildvalue3 = buildvalue3 + "'" + "," + "'" + build3;
                                    }
                                }
                            }
                            txt_room.Text = Convert.ToString(buildvalue3);
                        }


                        //  txt_roomtype.Text = Convert.ToString(ds.Tables[0].Rows[0]["room_type"]);
                        string vacate = "";
                        vacate = Convert.ToString(ds.Tables[0].Rows[0]["IsVacated"]);
                        if (vacate != "False")
                        {
                            cb_vacate.Checked = true;
                            txt_vacatedateguest.Enabled = true;
                        }
                        else
                        {
                            cb_vacate.Checked = false;
                            txt_vacatedateguest.Enabled = false;
                        }
                        txt_vacatedateguest.Text = Convert.ToString(ds.Tables[0].Rows[0]["vacate_date"]);
                        // string guscode = d2.GetFunction("select GuestCode from Hostel_GuestReg where Guest_Name='" + guestname + "'");
                        // txt_code.Text = Convert.ToString(guscode);
                        // txt_code.Text = Convert.ToString(ds.Tables[0].Rows[0]["GuestCode"]);
                        txt_code.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorCode"]);
                        txt_desgn.Text = Convert.ToString(ds.Tables[0].Rows[0]["VenContactDesig"]);
                        txt_dep.Text = Convert.ToString(ds.Tables[0].Rows[0]["VenContactDept"]);

                        //string val = Convert.ToString(ds.Tables[0].Rows[0]["Desig_Code"]);
                        //string des = d2.GetFunction("select TextVal from textvaltable where textcriteria = 'Gudis' and TextCode='" + val + "'");

                        //txt_desgn.Text = des;
                        //string valdept = Convert.ToString(ds.Tables[0].Rows[0]["department"]);
                        //string dept = d2.GetFunction("select TextVal from textvaltable where textcriteria = 'Gudep' and TextCode='" + valdept + "'");
                        //txt_dep.Text = dept;
                        string valdist = Convert.ToString(ds.Tables[0].Rows[0]["VendorDist"]);
                        string dist = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria = 'district' and mastercode='" + valdist + "'");
                        txt_dis.Text = dist;
                        string valstate = Convert.ToString(ds.Tables[0].Rows[0]["VendorState"]);
                        string state = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria = 'State' and mastercode='" + valstate + "'");
                        txt_stat.Text = state;

                    }
                    string room = "select Room_Name,Room_type from Room_Detail where Room_Name='" + txt_room.Text + "'";
                    ds = d2.select_method_wo_parameter(room, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string roomtype = ds.Tables[0].Rows[0]["Room_type"].ToString();
                        txt_roomtype.Text = roomtype;
                    }

                }

            }

        }
        catch
        {
        }
    }

    protected void btn_updateguest_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                string dtaccessdate = "";
                string dtaccesstime = "";
                string date = "";
                string getday = "";
                string guestname = "";
                string guestaddress = "";
                string mobno = "";
                string company = "";
                string desi = "";
                string desicode = "";
                string guestcode = "";
                string builname = "";
                string floor = "";
                string room = "";
                string city = "";
                string dept = "";
                string deptcode = "";
                string dist = "";
                string distcode = "";
                string state = "";
                string statecode = "";
                string roomtype = "";
                dtaccessdate = DateTime.Now.ToString();
                dtaccesstime = DateTime.Now.ToLongTimeString();
                date = Convert.ToString(txt_admindate.Text);
                string[] splitdate = date.Split('-');
                splitdate = splitdate[0].Split('/');
                DateTime dt = new DateTime();
                if (splitdate.Length > 0)
                {
                    dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                }
                getday = dt.ToString("MM/dd/yyyy");

                state = Convert.ToString(txt_stat.Text);
                statecode = subjectcode("State", state);

                desi = Convert.ToString(txt_desgn.Text);
                // desicode = subjectcode("Gudis", desi);
                dept = Convert.ToString(txt_dep.Text);
                //  deptcode = subjectcode("Gudep", dept);

                dist = Convert.ToString(txt_dis.Text);
                distcode = subjectcode("District", dist);
                if (rdb_staffe.Checked == true)
                {
                    ddlguest.Visible = false;
                    //rdb_veg.Visible = false;
                    lblGuestType.Visible = false;
                    ddlStudType.Visible = true;
                }
                if (rdb_gueste.Checked == true)
                {
                    ddlStudType.Visible = false;
                    ddlguest.Visible = true;
                    //rdb_veg.Visible = true;
                    lblGuestType.Visible = true;
                }


                guestname = Convert.ToString(txt_nameguest.Text);
                guestcode = Convert.ToString(txt_code.Text);
                guestname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(guestname);
                guestaddress = Convert.ToString(txt_str.Text);
                guestaddress = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(guestaddress);
                mobno = Convert.ToString(txt_mno.Text);
                company = Convert.ToString(txt_compname.Text);
                company = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(company);
                //desi = Convert.ToString(txt_desgn.Text);
                desicode = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(desicode);
                //  dept = Convert.ToString(txt_dep.Text);
                deptcode = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(deptcode);

                builname = Convert.ToString(txt_building.Text);
                floor = Convert.ToString(txt_floor.Text);
                room = Convert.ToString(txt_room.Text);
                city = Convert.ToString(txt_cty.Text);
                city = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(city);


                roomtype = Convert.ToString(txt_roomtype.Text);
                roomtype = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(roomtype);


                string hostelcode = Convert.ToString(ddl_messname.SelectedItem.Value);


                string vacatedate = Convert.ToString(txt_vacatedateguest.Text);
                string[] splitdatevacate = vacatedate.Split('-');
                splitdatevacate = splitdatevacate[0].Split('/');
                DateTime dtvacate = new DateTime();
                if (splitdatevacate.Length > 0)
                {
                    dtvacate = Convert.ToDateTime(splitdatevacate[1] + "/" + splitdatevacate[0] + "/" + splitdatevacate[2]);
                }
                string getdayvacate = dtvacate.ToString("MM/dd/yyyy");
                string vendarcode = Convert.ToString(txt_code.Text);
                string appnumber = "";
                string building = "";
                string floo = "";
                string r = "";
                //magesh 12.3.18
                //string expansetype = "";

                //if (rdb_veg.Checked == true)
                //{
                //    expansetype = "0";
                //}
                //else if (rdb_NonVeg.Checked == true)
                //{
                //    expansetype = "1";
                //}
                string studmesstype = string.Empty;
                int messtype = 0;

              
                int.TryParse(Convert.ToString(ddlguest.SelectedValue), out messtype);
                studmesstype = Convert.ToString(messtype - 1);//magesh 12.3.18

                building = Convert.ToString(ViewState["Code"]);
                floo = Convert.ToString(ViewState["Floorpk"]);
                r = Convert.ToString(ViewState["Roompk"]);

                if (cb_vacate.Checked == true)
                {
                    appnumber = d2.GetFunction("select im.VendorContactPK from IM_VendorContactMaster im,CO_VendorMaster co where co.VendorType='10'  and im.VenContactName='" + guestname + "' and co.VendorCode='" + txt_code.Text + "' and co.VendorPK=im.VendorFK");
                    ViewState["VendorContactPK"] = Convert.ToString(appnumber);

                    string query = "update HT_HostelRegistration set IsVacated='1',VacatedDate='" + getdayvacate + "'  where HostelMasterFK='" + hostelcode + "' and APP_No ='" + appnumber + "'and StudMessType='" + studmesstype + "'";

                    //MobileNo='" + mobno + "' and Guest_Name='" + guestname + "' and  MobileNo='" + mobno + "'  and Guest_Name='" + guestname + "' and
                    int iv = d2.update_method_wo_parameter(query, "Text");
                    if (iv != 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_erroralert.Text = "Updated Successfully";

                        clear();
                        popwindow1.Visible = false;

                        // btn2_Click(sender, e);

                    }
                }
                else
                {
                    string query = "";

                    string query1 = "";
                    



                    query1 = "if exists (select * from CO_VendorMaster where  VendorCode='" + txt_code.Text + "') update CO_VendorMaster set VendorAddress='" + guestaddress + "',VendorCompName='" + company + "',VendorCity='" + city + "',VendorType='10',VendorDist='" + distcode + "',VendorState='" + statecode + "' where VendorCode='" + txt_code.Text + "' else insert into CO_VendorMaster (VendorAddress,VendorCompName,VendorCity,VendorType,VendorDist,VendorState,VendorCode)values ('" + guestaddress + "','" + company + "','" + city + "','10','" + distcode + "','" + statecode + "','" + txt_code.Text + "')";
                    int iv1 = d2.update_method_wo_parameter(query1, "Text");

                    string venfk = d2.getvenpk(vendarcode);


                    query = "if exists (select * from IM_VendorContactMaster where  VendorFK='" + venfk + "')update IM_VendorContactMaster set VenContactName='" + guestname + "',VendorMobileNo='" + mobno + "',VenContactDesig='" + desi + "',VenContactDept='" + dept + "' where VendorFK='" + venfk + "' else insert into IM_VendorContactMaster(VenContactName,VendorMobileNo,VenContactDesig,VenContactDept,VendorFK) values('" + guestname + "','" + mobno + "','" + desi + "','" + dept + "','" + venfk + "')";
                    int iv = d2.update_method_wo_parameter(query, "Text");


                    appnumber = d2.GetFunction("select im.VendorContactPK from IM_VendorContactMaster im,CO_VendorMaster co where co.VendorType='10'  and im.VenContactName='" + guestname + "' and co.VendorCode='" + txt_code.Text + "' and co.VendorPK=im.VendorFK");
                    ViewState["VendorContactPK"] = Convert.ToString(appnumber);



                    string q1 = " select distinct r.Building_Name,r.Floor_Name,r.Room_Name,r.Room_type from Room_Detail r,HT_HostelRegistration hd,Floor_Master fm,Building_Master bm where hd.App_No='" + appnumber + "' and r.Roompk=hd.RoomFK and fm.Floorpk=hd.FloorFK and bm.Code=hd.BuildingFK";
                    ds3.Clear();
                    ds3 = d2.select_method_wo_parameter(q1, "text");
                    string bulname = "";
                    bulname = Convert.ToString(ds3.Tables[0].Rows[0]["Building_Name"].ToString());
                    string flrname = "";
                    flrname = Convert.ToString(ds3.Tables[0].Rows[0]["Floor_Name"].ToString());
                    string roomname = "";
                    roomname = Convert.ToString(ds3.Tables[0].Rows[0]["Room_Name"].ToString());
                    string roomtypeup = "";
                    roomtypeup = Convert.ToString(ds3.Tables[0].Rows[0]["Room_type"].ToString());
                    string upalavl = " update Room_Detail set Avl_Student= Avl_Student - 1 where Room_type='" + roomtypeup + "' and Floor_Name='" + flrname + "' and Room_Name='" + roomname + "' and Building_Name='" + bulname + "'";
                    int kalavl = d2.update_method_wo_parameter(upalavl, "text");

                    string up = " update Room_Detail set Avl_Student= Avl_Student + 1 where Room_type='" + txt_roomtype.Text + "' and Floor_Name='" + txt_floor.Text + "' and Room_Name='" + txt_room.Text + "' and Building_Name='" + txt_building.Text + "'";
                    int k = d2.update_method_wo_parameter(up, "text");
                    string query2 = "";


                    query2 = "if exists (select * from HT_HostelRegistration where  GuestVendorFK='" + venfk + "')update HT_HostelRegistration set BuildingFK='" + building + "',StudMessType='" + studmesstype + "',FloorFK='" + floo + "',RoomFK='" + r + "',HostelMasterFK='" + hostelcode + "',HostelAdmDate='" + dt.ToString("MM/dd/yyyy") + "',GuestVendorType='10',MemType='3',APP_No='" + appnumber + "',Messcode='" + Convert.ToString(ddlmess1.SelectedValue) + "',id='" + txtid1.Text + "' where GuestVendorFK='" + venfk + "' else insert into HT_HostelRegistration (BuildingFK,StudMessType,FloorFK,RoomFK,HostelMasterFK,HostelAdmDate,GuestVendorType,MemType,APP_No,GuestVendorFK,Messcode,id) values('" + building + "','" + studmesstype + "','" + floo + "','" + r + "','" + hostelcode + "','" + dt.ToString("MM/dd/yyyy") + "','10','3','" + appnumber + "','" + venfk + "','" + Convert.ToString(ddlmess1.SelectedValue) + "','"+txtid1.Text+"')";


                    int iv2 = d2.update_method_wo_parameter(query2, "Text");
                    if (iv2 != 0)
                    {
                        btn_go_Click(sender, e);
                        imgdiv2.Visible = true;
                        lbl_erroralert.Text = "Updated Successfully";

                        clear();
                        popwindow1.Visible = false;

                        // btn2_Click(sender, e);

                    }
                }
            }
            catch
            {

            }
        }
        catch
        {

        }
    }
    public void btn_delguest_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delguest.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to delete this Record?";

            }
        }
        catch
        {
        }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstate(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select TextVal,TextCode from textvaltable where textcriteria = 'State' and TextVal <>'' and TextVal like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getdesi(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select TextVal,TextCode from textvaltable where textcriteria = 'Gudis'  and TextVal <>'' and TextVal like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getdept(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select TextVal,TextCode from textvaltable where textcriteria = 'Gudep'  and TextVal <>'' and TextVal like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getdist(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select TextVal,TextCode from textvaltable where textcriteria = 'Dis' and TextVal <>'' and TextVal like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    //public string subjectcode(string textcri, string subjename)
    //{
    //    string subjec_no = "";
    //    try
    //    {

    //        subjename = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(subjename);
    //        string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode1 + " and TextVal='" + subjename + "'";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(select_subno, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //        }
    //        else
    //        {
    //            string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
    //            int result = d2.update_method_wo_parameter(insertquery, "Text");
    //            if (result != 0)
    //            {
    //                string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode1 + " and TextVal='" + subjename + "'";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(select_subno1, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //    return subjec_no;
    //}
    public string subjectcode(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            subjename = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(subjename);
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {

        }
        return subjec_no;
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        // btn_go_Click(sender, e);
        //clear1();
        clear();
        popwindow1.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        popwindow1.Visible = true;
    }
    public void delete()
    {
        try
        {
            surediv.Visible = false;
            string hostelcode = Convert.ToString(ddl_messname.SelectedItem.Value);
            string date = Convert.ToString(txt_admindate.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string getday = dt.ToString("MM/dd/yyyy");
            string mobno = Convert.ToString(txt_mno.Text);
            string guestcode = Convert.ToString(txt_code.Text);
            string appnumber = "";
            string guestname = "";
            guestname = Convert.ToString(txt_nameguest.Text);
            appnumber = d2.GetFunction("select im.VendorContactPK from IM_VendorContactMaster im,CO_VendorMaster co where co.VendorType='10'  and im.VenContactName='" + guestname + "' and co.VendorCode='" + txt_code.Text + "' and co.VendorPK=im.VendorFK");
            ViewState["VendorContactPK"] = Convert.ToString(appnumber);
            string query2 = "delete from HT_HostelRegistration where HostelMasterFK ='" + hostelcode + "' and APP_No='" + appnumber + "' ";
            int iv = d2.update_method_wo_parameter(query2, "Text");
            if (iv != 0)
            {



                imgdiv2.Visible = true;
                surediv.Visible = false;
                lbl_erroralert.Visible = true;
                lbl_erroralert.Text = "Deleted Successfully";

            }
        }
        catch
        {

        }
    }

    //public void des()
    //{
    //    string des = d2.GetFunction("select TextVal from textvaltable where textcriteria = 'Gudis'");
    //}


    protected void btn_sureyesstaff_Click(object sender, EventArgs e)
    {
        deletestaff();
        // btn_go_Click(sender, e);
        //clear1();
        //  clear();
        popwindow1.Visible = false;
    }
    protected void btn_surenostaff_Click(object sender, EventArgs e)
    {
        suredivstaff.Visible = false;
        imgdiv2.Visible = false;
        popwindow1.Visible = true;
    }
    public void deletestaff()
    {
        try
        {
            suredivstaff.Visible = false;
            string applid = "";

            string staffcode = d2.GetFunction("select staff_code from staffmaster where staff_name='" + txt_pop1staffname.Text + "'");
            applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + staffcode + "' and sam.appl_no = sm.appl_no");
            ViewState["appl_id"] = Convert.ToString(applid);
            string sql = "delete from HT_HostelRegistration where APP_No='" + applid + "' and MemType=2";
            int query = d2.update_method_wo_parameter(sql, "TEXT");
            if (query > 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Deleted Sucessfully";
                popwindow1.Visible = false;
                // btn_go_Click(sender, e);
            }
        }
        catch
        {

        }
    }
    public void btn_pop1delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_pop1delete.Text == "Delete")
            {
                suredivstaff.Visible = true;
                lbl_surestaff.Text = "Do you want to delete this Record?";

            }
        }
        catch
        {
        }
    }

    public void loadhostel()
    {
        try
        {
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds.Clear();
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            //string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelMasterPK ";
            // ds = d2.select_method_wo_parameter(itemname, "Text");

            //magesh 21.6.18
            MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster  order by hostelname";
            ds = d2.select_method_wo_parameter(MessmasterFK, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                //Hostelcode = cbl_hostelname.SelectedValue;

                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                    txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                    cb_hostelname.Checked = true;
                }

                string lochosname = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelname.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }

                clgbuild(lochosname);
            }
            else
            {
                cbl_hostelname.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_hostelname.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_hostelname.Text = "--Select--";
                    //  cb_hostelname.Checked = false;
                    cb_buildingname.Checked = true;
                    build = cbl_hostelname.Items[i].Text.ToString();
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
            clgbuild(buildvalue);
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_hostelname.Items.Count)
            {
                txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
                cb_hostelname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_hostelname.Text = "--Select--";
            }
            else
            {
                txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }

    }

    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cb_hostelname.Checked == true)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                        txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_hostelname.Items[i].Text.ToString();
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
                clgbuild(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                    txt_hostelname.Text = "--Select--";
                    cbl_buildingname.Items.Clear();
                    cb_buildingname.Checked = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
            //  Button2.Focus();

        }
        catch (Exception ex)
        {
        }
    }




    public void clgbuild(string hostelname)
    {
        try
        {
            // cbl_buildingname.Items.Clear();
            string bul = "";
            hostelname = "";

            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (bul == "")
                    {
                        bul = Convert.ToString(cbl_hostelname.Items[i].Value);
                    }
                    else
                    {

                        bul = bul + "'" + "," + "'" + Convert.ToString(cbl_hostelname.Items[i].Value);
                    }
                }
            }

            hostelname = d2.GetBuildingCode_inv(bul);
            ds = d2.BindBuilding(hostelname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildingname.DataSource = ds;
                cbl_buildingname.DataTextField = "Building_Name";
                cbl_buildingname.DataValueField = "code";
                cbl_buildingname.DataBind();
            }

            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                cbl_buildingname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                cb_buildingname.Checked = true;
            }

            string locbuild = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    string builname = cbl_buildingname.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            clgfloor(locbuild);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbbuildname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildingname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    if (cb_buildingname.Checked == true)
                    {
                        cbl_buildingname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildingname.Items[i].Text.ToString();
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
                clgfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    cbl_buildingname.Items[i].Selected = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
            //  Button2.Focus();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblbuildname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildingname.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_floorname.Text = "--Select--";
                    cb_floorname.Checked = true;
                    build = cbl_buildingname.Items[i].Text.ToString();
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
            clgfloor(buildvalue);
            if (seatcount == cbl_buildingname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildingname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void clgfloor(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_floorname.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "Floorpk";
                cbl_floorname.DataBind();

            }
            else
            {
                txt_floorname.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                cbl_floorname.Items[i].Selected = true;
                cb_floorname.Checked = true;
            }

            string locfloor = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                    string flrname = cbl_floorname.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }

            }
            clgroom(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbfloorname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";

                if (cb_buildingname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                    {
                        build1 = cbl_buildingname.Items[i].Text.ToString();
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
                if (cb_floorname.Checked == true)
                {
                    for (int j = 0; j < cbl_floorname.Items.Count; j++)
                    {
                        cbl_floorname.Items[j].Selected = true;
                        txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                        build2 = cbl_floorname.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {
                            buildvalue2 = build2;
                        }
                        else
                        {
                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroom(buildvalue2, buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                    txt_floorname.Text = "--Select--";
                }
                cb_roomname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblfloorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    build1 = cbl_buildingname.Items[i].Text.ToString();
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
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build2 = cbl_floorname.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroom(buildvalue2, buildvalue1);

            if (seatcount == cbl_floorname.Items.Count)
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floorname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorname.Text = "--Select--";
            }
            else
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
            //  clgroom(buildvalue1, buildvalue2);
        }
        catch (Exception ex)
        {
        }
    }
    public void clgroom(string floorname, string buildname)
    {
        try
        {
            cbl_roomname.Items.Clear();
            ds = d2.BindRoom(floorname, buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();
            }
            else
            {
                txt_roomname.Text = "--Select--";
            }

            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                cbl_roomname.Items[i].Selected = true;
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
                cb_roomname.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbroomname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomname.Checked == true)
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = true;
                }
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = false;
                }
                txt_roomname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblroomname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomname.Checked = false;
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == cbl_roomname.Items.Count)
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_roomname.Text = "--Select--";
            }
            else
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    //magesh 12.3.18
    protected void BindStudentType()
    {
        try
        {
            ddlStudType.Items.Clear();
            ds.Clear();
            string sql = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStudType.DataSource = ds;
                ddlStudType.DataTextField = "StudentTypeName";
                ddlStudType.DataValueField = "StudentType";
                ddlStudType.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void BindgusttType()
    {
        try
        {
            ddlguest.Items.Clear();
            ds.Clear();
            string sql = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlguest.DataSource = ds;
                ddlguest.DataTextField = "StudentTypeName";
                ddlguest.DataValueField = "StudentType";
                ddlguest.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void idgeneration()
    {
        try
        {
            string newitemcode = "";

            string ishostel = string.Empty;
            string memtype = string.Empty;
            string newins = string.Empty;
            string hos_code = string.Empty;
            string hos_code1 = string.Empty;
            string colcode = ddl_pop1collegename.SelectedValue;
            if (usercode != "")
            {
                newins = "select * from New_InsSettings where LinkName='hostelid generation' and user_code ='" + usercode + "' and college_code ='" + ddl_pop1collegename.SelectedValue + "'";
            }
            else
            {
                newins = "select * from New_InsSettings where LinkName='hostelid generation' and user_code ='" + group_user + "' and college_code ='" + ddl_pop1collegename.SelectedValue + "'";
            }
            ds = d2.select_method_wo_parameter(newins, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ishostel = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            }
            if (ishostel != "")
            {
                if (ishostel == "1")
                {
                    hos_code = Convert.ToString(ddl_pop1hostelname.SelectedValue);
                    hos_code1 = Convert.ToString(ddl_pop1hostelname.SelectedValue);

                    memtype = "3";
                }
                if (ishostel == "0")
                {
                    hos_code = Convert.ToString(ddl_pop1hostelname.SelectedValue);
                    hos_code1 = "0";
                    ishostel = "0";
                    if (rdb_staff.Checked == true)
                        memtype = "1";
                    if (rdb_guest.Checked == true)
                        memtype = "2";

                }


                string selectquery = "select idAcr,idStNo,idSize from Hostelidgeneration where college_code='" + colcode + "' and hostelcode='" + hos_code1 + "' and ishostel='" + ishostel + "' and memtype='" + memtype + "' order by FromDate desc";//where Latestrec =1"
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["idAcr"]);
                    string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["idStNo"]);
                    string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["idSize"]);
                    if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                    {
                        selectquery = " select distinct top (1) id  from HT_HostelRegistration where id like '" + Convert.ToString(itemacronym) + "[0-9]%'  and HostelMasterFK='" + hos_code + "' order by id desc";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                            string itemacr = Convert.ToString(itemacronym);
                            int len = itemacr.Length;
                            itemcode = itemcode.Remove(0, len);
                            int len1 = Convert.ToString(itemcode).Length;
                            string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                            len = Convert.ToString(newnumber).Length;
                            len1 = len1 - len;
                            if (len1 == 2)
                            {
                                newitemcode = "00" + newnumber;
                            }
                            else if (len1 == 1)
                            {
                                newitemcode = "0" + newnumber;
                            }
                            else if (len1 == 3)
                            {
                                newitemcode = "000" + newnumber;
                            }
                            else if (len1 == 4)
                            {
                                newitemcode = "0000" + newnumber;
                            }
                            else if (len1 == 5)
                            {
                                newitemcode = "00000" + newnumber;
                            }
                            else if (len1 == 6)
                            {
                                newitemcode = "000000" + newnumber;
                            }
                            else
                            {
                                newitemcode = Convert.ToString(newnumber);
                            }
                            if (newitemcode.Trim() != "")
                            {
                                newitemcode = itemacr + "" + newitemcode;
                            }
                        }
                        else
                        {
                            string itemacr = Convert.ToString(itemstarno);
                            int len = itemacr.Length;
                            string items = Convert.ToString(itemsize);
                            int len1 = Convert.ToInt32(items);
                            int size = len1 - len;
                            if (size == 2)
                            {
                                newitemcode = "00" + itemstarno;
                            }
                            else if (size == 1)
                            {
                                newitemcode = "0" + itemstarno;
                            }
                            else if (size == 3)
                            {
                                newitemcode = "000" + itemstarno;
                            }
                            else if (size == 4)
                            {
                                newitemcode = "0000" + itemstarno;
                            }
                            else if (size == 5)
                            {
                                newitemcode = "00000" + itemstarno;
                            }
                            else if (size == 6)
                            {
                                newitemcode = "000000" + itemstarno;
                            }
                            else
                            {
                                newitemcode = Convert.ToString(itemstarno);
                            }
                            newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                        }

                        if (rdb_guest.Checked == true)
                            txtid1.Text = Convert.ToString(newitemcode);
                        if (rdb_staff.Checked == true)
                            txtid.Text = Convert.ToString(newitemcode);
                        //poperrjs.Visible = true;
                        //btnsave.Visible = true;
                        //SelectdptGrid.Visible = false;
                        //btnupdate.Visible = false;
                        // btndelete.Visible = false;
                        // bindstore();
                        // bindunitddl();
                        // loadheadername();
                        //loadsubheadername();
                        // loaditem();
                        // bind_subheader();
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        //lbl_alert.Text = "Please Update Code Master";
                    }
                }
            }
            else
            {
                txtid1.Text = "";
                txtid.Text = "";
            }
        }
        catch
        {
        }
    }
   
}