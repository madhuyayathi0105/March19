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
public partial class HM_MonthlyMessBillReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsmnth = new DataSet();
    DAccess2 dmnth = new DAccess2();
    string build = "";
    string floor = "";
    string bul = "";
    string buildvalue = "";
    string batch = "";
    string degree = "";
    string exammonth = "";
    string examyear = "";
    string colg = "";
    string dept = "";
    int commcount;
    int i;
    int cout;
    int row;
    string college = "";
    string building = "";
    string hos = "";
    string hostel = "";
    string gender = "";
    string month = "";
    string year = "";
    static string Hostelcode = string.Empty;
    Boolean Cellclick = false;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static ArrayList ItemList1 = new ArrayList();
    static ArrayList Itemindex1 = new ArrayList();
    static ArrayList ItemListstu = new ArrayList();
    static ArrayList Itemindexstu = new ArrayList();
    static ArrayList ItemListguest = new ArrayList();
    static ArrayList Itemindexguest = new ArrayList();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
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
        collegecode1 = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pcolumnorder2.Visible = false;
            pheaderfilterstu.Visible = false;
            pcolumnorderstu.Visible = false;
            pheaderfilterguest.Visible = false;
            pcolumnorderguest.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
         //   bindhostelname();
            bindbuild();
            bindfloor();
            bindclg();
            bindBtch();
            binddeg();
            binddept();
            rdb_student.Checked = true;
            bindgender();
            bindmonth();
            bindyear();
            bindhostel();
            //  btn_go_Click(sender, e);
            //bindmonyear();
            //  rdb_common.Visible = false;
            //  rdb_indivual.Visible = false;
            rdb_fromat1.Checked = true;
            rdb_common.Enabled = false;
            rdb_indivual.Enabled = false;
            //  rdb_detailedwise.Checked = true;
            // rdb_student_CheckedChange(sender, e);
            // rdb_indivual_CheckedChange(sender, e);
            rdb_paid.Enabled = false;
            rdb_unpaid.Enabled = false;
            rdb_yettobepaid.Enabled = false;
        }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    //public void bindhostelname()
    //{
    //    try
    //    {
    //        //cbl_hostel.Items.Clear();
    //        ////ds = d2.BindHostel(collegecode1);14.10.15
    //        //ds = d2.BindMess(collegecode1);
    //        //if (ds.Tables[0].Rows.Count > 0)
    //        //{
    //        //    cbl_hostel.DataSource = ds;
    //        //    cbl_hostel.DataTextField = "MessName";
    //        //    cbl_hostel.DataValueField = "MessID";
    //        //    cbl_hostel.DataBind();
    //        //    if (cbl_hostel.Items.Count > 0)
    //        //    {
    //        //        for (i = 0; i < cbl_hostel.Items.Count; i++)
    //        //        {
    //        //            cbl_hostel.Items[i].Selected = true;
    //        //        }
    //        //        txt_hostelname.Text = "Mess Name(" + cbl_hostel.Items.Count + ")";
    //        //        cb_hostel.Checked = true;
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    txt_hostelname.Text = "--Select--";
    //        //    cb_hostel.Checked = false;
    //        //}
    //        ds.Clear();
    //        //string itemname = "select Hostel_code,Hostel_Name  from Hostel_Details order by Hostel_code";
    //        //ds = d2.select_method_wo_parameter(itemname, "Text");
    //        //ds = d2.Bindmess_inv(collegecode1);
    //        ds = d2.Bindmess_basedonrights(usercode, collegecode1);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_messname.DataSource = ds;
    //            ddl_messname.DataTextField = "MessName";
    //            ddl_messname.DataValueField = "MessMasterPK";
    //            ddl_messname.DataBind();
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}


    protected void ddl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           // bindbuild();
           // bindfloor();
            div1.Visible = false;
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            lbl_error.Visible = false;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pcolumnorder2.Visible = false;
            pheaderfilterstu.Visible = false;
            pcolumnorderstu.Visible = false;
            pheaderfilterguest.Visible = false;
            pcolumnorderguest.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    //protected void cb_hostel_CheckedChanged(object sender, EventArgs e)
    //{
    //    cout = 0;
    //    txt_hostelname.Text = "--Select--";
    //    if (cb_hostel.Checked == true)
    //    {
    //        cout++;
    //        for (i = 0; i < cbl_hostel.Items.Count; i++)
    //        {
    //            cbl_hostel.Items[i].Selected = true;
    //        }
    //        txt_hostelname.Text = "Mess Name(" + (cbl_hostel.Items.Count) + ")";
    //    }
    //    else
    //    {
    //        for (i = 0; i < cbl_hostel.Items.Count; i++)
    //        {
    //            cbl_hostel.Items[i].Selected = false;
    //        }
    //    }
    //    bindbuild();
    //    bindfloor();
    //}
    //protected void cbl_hostel_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    cb_hostel.Checked = false;
    //    commcount = 0;
    //    txt_hostelname.Text = "--Select--";
    //    for (i = 0; i < cbl_hostel.Items.Count; i++)
    //    {
    //        if (cbl_hostel.Items[i].Selected == true)
    //        {
    //            commcount = commcount + 1;
    //            cb_hostel.Checked = false;
    //        }
    //    }
    //    if (commcount > 0)
    //    {
    //        if (commcount == cbl_hostel.Items.Count)
    //        {
    //            cb_hostel.Checked = true;
    //        }
    //        txt_hostelname.Text = "Mess Name(" + commcount.ToString() + ")";
    //    }
    //    bindbuild();
    //    bindfloor();
    //}
    public void bindbuild()
    {
        try
        {
            cbl_building.Items.Clear();
            txt_building.Text = "---Select---";
            cb_building.Checked = false;
            build = "";
            if (cbl_hostelname.Items.Count > 0)//mess code 14.10.15
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_hostelname.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_hostelname.Items[i].Value);
                        }
                    }
                }
            }
          //  build = ddl_messname.SelectedItem.Value;
            bul = "";
            //
            //ds.Clear();
            //string q1 = " select  hostel_code from MessMaster m,MessDetail md where m.MessID =md.MessID and m.MessID in ('" + build + "')";
            //ds = d2.select_method_wo_parameter(q1,"Text");
            //if( )
            //{
            //}
          //  string buil1 = d2.Gethostelcode_inv(build);
            string buil1 = d2.GetBuildingCode_inv(build);
            ds = d2.BindBuilding(buil1);
            //
            //if (buil1 != "")
            //{
                //bul = queryObject.GetBuildingCode_inv(buil1);
                //ds = queryObject.BindBuilding(bul);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_building.DataSource = ds;
                    cbl_building.DataTextField = "Building_Name";
                    cbl_building.DataValueField = "code";
                    cbl_building.DataBind();
                    if (cbl_building.Items.Count > 0)
                    {
                        for (row = 0; row < cbl_building.Items.Count; row++)
                        {
                           // cbl_building.Items[row].Selected = true;
                        }
                        txt_building.Text = "Building Name(" + cbl_building.Items.Count + ")";
                       // cb_building.Checked = true;
                    }
                }
           // }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_building_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_building.Text = "--Select--";
            if (cb_building.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_building.Items.Count; i++)
                {
                    cbl_building.Items[i].Selected = true;
                }
                txt_building.Text = "Building Name(" + (cbl_building.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_building.Items.Count; i++)
                {
                    cbl_building.Items[i].Selected = false;
                }
            }
            bindfloor();
        }
        catch
        {
        }
    }
    protected void cbl_building_SelectedIndexChanged(object sender, EventArgs e)
    {
        i = 0;
        cb_building.Checked = false;
        commcount = 0;
        buildvalue = "";
        build = "";
        txt_building.Text = "--Select--";
        for (i = 0; i < cbl_building.Items.Count; i++)
        {
            if (cbl_building.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_building.Checked = false;
                build = cbl_building.Items[i].Text.ToString();
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
        if (commcount > 0)
        {
            if (commcount == cbl_building.Items.Count)
            {
                cb_building.Checked = true;
            }
            txt_building.Text = "Building Name(" + commcount.ToString() + ")";
        }
        bindfloor();
    }
    public void bindfloor()
    {
        try
        {
            cbl_floor.Items.Clear();
            txt_floor.Text = "---Select---";
            cb_floor.Checked = false;
            floor = "";
            if (cbl_building.Items.Count > 0)
            {
                for (i = 0; i < cbl_building.Items.Count; i++)
                {
                    if (cbl_building.Items[i].Selected == true)
                    {
                        if (floor == "")
                        {
                            floor = Convert.ToString(cbl_building.Items[i].Text);
                        }
                        else
                        {
                            floor = floor + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                        }
                    }
                }
            }
            if (floor != "")
            {
                ds = queryObject.BindFloor_new(floor);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_floor.DataSource = ds;
                    cbl_floor.DataTextField = "Floor_Name";
                    cbl_floor.DataValueField = "FloorPK";
                    cbl_floor.DataBind();
                    if (cbl_floor.Items.Count > 0)
                    {
                        for (row = 0; row < cbl_floor.Items.Count; row++)
                        {
                            cbl_floor.Items[row].Selected = true;
                        }
                        txt_floor.Text = "Floor Name(" + cbl_floor.Items.Count + ")";
                        cb_floor.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_floor_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_floor.Text = "--Select--";
            if (cb_floor.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = true;
                }
                txt_floor.Text = "Floor Name(" + (cbl_floor.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void cbl_floor_SelectedIndexChanged(object sender, EventArgs e)
    {
        i = 0;
        cb_floor.Checked = false;
        commcount = 0;
        txt_floor.Text = "---Select---";
        for (i = 0; i < cbl_floor.Items.Count; i++)
        {
            if (cbl_floor.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_floor.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_floor.Items.Count)
            {
                cb_floor.Checked = true;
            }
            txt_floor.Text = "Floor Name(" + commcount.ToString() + ")";
        }
    }
    //protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bindBtch();
    //        binddeg();
    //        binddept();
    //    }
    //    catch { }
    //}
    public void bindBtch()
    {
        try
        {
            cbl_batch.Items.Clear();
            cbl_year.Items.Clear();
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

                cbl_year.DataSource = ds;
                cbl_year.DataTextField = "batch_year";
                cbl_year.DataValueField = "batch_year";
                cbl_year.DataBind();
                string year = DateTime.Now.Year.ToString();
                string dmyear = d2.GetFunction("select    top 1 batch_year from applyn where batch_year<>'-1' and batch_year<>''order by batch_year desc");
                if (year != dmyear)
                    cbl_year.Items.Insert(0, year);
                if (cbl_batch.Items.Count > 0)
                {
                    for (i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
                if (cbl_year.Items.Count > 0)
                {
                    for (i = 0; i < cbl_year.Items.Count; i++)
                    {
                        cbl_year.Items[i].Selected = true;
                    }
                    txt_year.Text = "Batch(" + cbl_year.Items.Count + ")";
                    cb_year.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
            }
            binddeg();
            binddept();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_batch.Checked = false;
            commcount = 0;
            txt_batch.Text = "--Select--";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
            binddeg();
            binddept();
        }
        catch { }
    }
    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            batch = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }
            }
            string clgcode = "";
            for (i = 0; i < cbl_colg.Items.Count; i++)
            {
                if (cbl_colg.Items[i].Selected == true)
                {
                    if (clgcode == "")
                    {
                        clgcode = Convert.ToString(cbl_colg.Items[i].Value);
                    }
                    else
                    {
                        clgcode += "','" + Convert.ToString(cbl_colg.Items[i].Value);
                    }
                }
            }
            if (batch != "")
            {
                ds.Clear();
                //ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);//17.07.16
                ds = d2.select_method_wo_parameter(" select distinct d.course_id,c.course_name from degree d,course c,deptprivilages dp,Registration r where c.course_id=d.course_id and c.college_code = d.college_code and d.college_code in('" + clgcode + "') and dp.Degree_code=d.Degree_code and r.degree_code=d.Degree_Code and r.Batch_Year in('" + batch + "')", "text");
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
                        txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                        cb_degree.Checked = true;
                    }
                }
            }
        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
            }
            binddept();
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
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
            binddept();
        }
        catch { }
    }
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            batch = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }
            }
            degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value) + "";
                    }
                    else
                    {
                        degree += "','" + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }
            }
            if (batch != "" && degree != "")
            {
                ds.Clear();
                //ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode1, usercode);
                ds = d2.select_method_wo_parameter("select distinct d.degree_code,de.dept_name,d.Acronym  from degree d,Department de,course c,deptprivilages dp where c.course_id=d.course_id  and de.dept_code=d.dept_code and c.college_code = d.college_code and de.college_code = d.college_code and d.course_id in('" + degree + "') and dp.Degree_code=d.Degree_code", "Text");
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
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
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
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
            txt_dept.Text = "--Select--";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_dept.Items.Count)
                {
                    cb_dept.Checked = true;
                }
                txt_dept.Text = "Department(" + commcount.ToString() + ")";
            }
        }
        catch { }
    }
    public void bindgender()
    {
        try
        {
            cb_gender.Checked = false;
            txt_gender.Text = "Select All";
            if (cbl_colg.Items.Count > 0)
            {
                for (i = 0; i < cbl_gender.Items.Count; i++)
                {
                    cbl_gender.Items[i].Selected = true;
                }
                txt_gender.Text = "Gender(" + cbl_gender.Items.Count + ")";
                cb_gender.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_gender_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_gender.Text = "--Select--";
            if (cb_gender.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_gender.Items.Count; i++)
                {
                    cbl_gender.Items[i].Selected = true;
                }
                txt_gender.Text = "Gender(" + cbl_gender.Items.Count + ")";
            }
            else
            {
                for (i = 0; i < cbl_gender.Items.Count; i++)
                {
                    cbl_gender.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_gender_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_gender.Checked = false;
            commcount = 0;
            txt_floor.Text = "---Select---";
            for (i = 0; i < cbl_gender.Items.Count; i++)
            {
                if (cbl_gender.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_gender.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_gender.Items.Count)
                {
                    cb_gender.Checked = true;
                }
                txt_gender.Text = "Gender(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindmonth()
    {
        try
        {
            cb_month.Checked = false;
            txt_month.Text = "Select All";
            if (cbl_month.Items.Count > 0)
            {
                for (i = 0; i < cbl_month.Items.Count; i++)
                {
                    cbl_month.Items[i].Selected = true;
                }
                txt_month.Text = "Month(" + cbl_month.Items.Count + ")";
                cb_month.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_month_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_month.Text = "--Select--";
            if (cb_month.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_month.Items.Count; i++)
                {
                    cbl_month.Items[i].Selected = true;
                }
                txt_month.Text = "Month(" + cbl_month.Items.Count + ")";
            }
            else
            {
                for (i = 0; i < cbl_month.Items.Count; i++)
                {
                    cbl_month.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_month.Checked = false;
            commcount = 0;
            txt_month.Text = "---Select---";
            for (i = 0; i < cbl_month.Items.Count; i++)
            {
                if (cbl_month.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_month.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_month.Items.Count)
                {
                    cb_month.Checked = true;
                }
                txt_month.Text = "Month(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindyear()
    {
        try
        {
            cb_year.Checked = false;
            txt_year.Text = "Select All";
            if (cbl_year.Items.Count > 0)
            {
                for (i = 0; i < cbl_year.Items.Count; i++)
                {
                    cbl_year.Items[i].Selected = true;
                }
                txt_year.Text = "Year(" + cbl_year.Items.Count + ")";
                cb_year.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_year_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            cout = 0;
            txt_year.Text = "--Select--";
            if (cb_year.Checked == true)
            {
                cout++;
                for (i = 0; i < cbl_year.Items.Count; i++)
                {
                    cbl_year.Items[i].Selected = true;
                }
                txt_year.Text = "Year(" + cbl_year.Items.Count + ")";
            }
            else
            {
                for (i = 0; i < cbl_year.Items.Count; i++)
                {
                    cbl_year.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_year.Checked = false;
            commcount = 0;
            txt_year.Text = "---Select---";
            for (i = 0; i < cbl_year.Items.Count; i++)
            {
                if (cbl_year.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_year.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_year.Items.Count)
                {
                    cb_year.Checked = true;
                }
                txt_year.Text = "Year(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            ItemList.Clear();
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected)
                {
                    if (hos == "")
                    {
                        hos = "" + cbl_hostelname.Items[i].Value.ToString();
                    }
                    else
                    {
                        hos += "','" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_building.Items.Count; i++)
            {
                if (cbl_building.Items[i].Selected)
                {
                    if (building == "")
                    {
                        building = "" + cbl_building.Items[i].Value.ToString();
                    }
                    else
                    {
                        building += "','" + cbl_building.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_floor.Items[i].Value.ToString();
                    }
                    else
                    {
                        floor += "','" + cbl_floor.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_colg.Items.Count; i++)
            {
                if (cbl_colg.Items[i].Selected)
                {
                    if (colg == "")
                    {
                        colg = "" + cbl_colg.Items[i].Value.ToString();
                    }
                    else
                    {
                        colg += "','" + cbl_colg.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected)
                {
                    if (batch == "")
                    {
                        batch = "" + cbl_batch.Items[i].Text.ToString();
                    }
                    else
                    {
                        batch += "','" + cbl_batch.Items[i].Text.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected)
                {
                    if (degree == "")
                    {
                        degree = "" + cbl_degree.Items[i].Value.ToString();
                    }
                    else
                    {
                        degree += "','" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected)
                {
                    if (dept == "")
                    {
                        dept = "" + cbl_dept.Items[i].Value.ToString();
                    }
                    else
                    {
                        dept += "','" + cbl_dept.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_gender.Items.Count; i++)
            {
                if (cbl_gender.Items[i].Selected)
                {
                    if (gender == "")
                    {
                        gender = "" + cbl_gender.Items[i].Value.ToString();
                    }
                    else
                    {
                        gender += "','" + cbl_gender.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_month.Items.Count; i++)
            {
                if (cbl_month.Items[i].Selected)
                {
                    if (month == "")
                    {
                        month = "" + cbl_month.Items[i].Value.ToString();
                    }
                    else
                    {
                        month += "','" + cbl_month.Items[i].Value.ToString() + "";
                    }
                }
            }
            hostel = ddl_messname.SelectedItem.Value;
            string hostelcod = d2.Gethostelcode_inv(hostel);
            for (i = 0; i < cbl_year.Items.Count; i++)
            {
                if (cbl_year.Items[i].Selected)
                {
                    if (year == "")
                    {
                        year = "" + cbl_year.Items[i].Text.ToString();
                    }
                    else
                    {
                        year += "','" + cbl_year.Items[i].Text.ToString() + "";
                    }
                }
            }
            Printcontrol.Visible = false;
            int count = 0;
            if (rdb_fromat1.Checked == true && rdb_student.Checked == true)
            {
                #region formate1 student
                DataView dv1 = new DataView();
                Hashtable hat = new Hashtable();
                hat.Add("Roll_No", "Roll No");
                hat.Add("id", "Student Id");
                hat.Add("Stud_Name", "Student Name");
                hat.Add("Hostel_Name", "Hostel Name");
                hat.Add("Building_Name", "Building Name");
                hat.Add("Floor_Name", "Floor Name");
                hat.Add("Batch_Year", "Batch Year");
                hat.Add("degree", "Degree");
                hat.Add("sex", "Gender");
                hat.Add("BillMonth", "BillMonth");
                hat.Add("Bill_Year", "Bill_Year");
                hat.Add("total", "total");
                hat.Add("ExpanceGroupAmtTotal", "Expance");
                hat.Add("Additional_Amount", "Additional Amount");
                hat.Add("Rebete_days", "Calculate Days");
                hat.Add("Rebate_Amount", "Rebate Amount");
                hat.Add("Fixed_Amount", "Mess Bill Amount");//added by saranyadevi 13.3.2018
                hat.Add("Room_Name", "Room");//added by rajasekar 18.07.2018
                ds.Clear();
                if (hostel.Trim() != "" && month.Trim() != "" && year.Trim() != "" && building.Trim() != "" && floor.Trim() != "" && dept.Trim() != "" && gender.Trim() != "")
                {
                    //string monthlymessbillquery = "select distinct r.Roll_No ,r.Stud_Name,hs.HostelMasterFK as Hostel_Code,h.HostelName as Hostel_Name,b.Building_Name,f.Floor_Name,r.Batch_Year ,(C.Course_Name +' - '+dt.Dept_Name )as degree,m.MessMonth as BillMonth,m.MessYear as Bill_Year ,d.Degree_Code,a.sex,(md.MessAmount+md.RebateAmount+md.MessAdditonalAmt) as Fixed_Amount,md.MessAdditonalAmt as Additional_Amount,md.RebateAmount as Rebate_Amount , md.MessAmount as total,md.RebateDays as Rebete_days,hms.Rebate_Amount,ExpanceGroupAmtTotal,hs.studmesstype from HT_MessBillMaster m,HT_MessBillDetail md ,HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,applyn a,Building_Master b,Floor_Master f,HMessbill_StudDetails hms where m.MessBillMasterPK =md.MessBillMasterFK and h.HostelMasterPK=hs.HostelMasterFK  and hs.APP_No=md.App_No and r.App_No=md.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.App_No =a.app_no and hs.BuildingFK=b.Code and hs.FloorFK =f.FloorPK and hs.HostelMasterFK =h.HostelMasterPK";//md.App_No as Roll_No +ExpanceGroupAmtTotal -md.RebateAmount(md.MessAmount+md.MessAdditonalAmt) 

                    //string monthlymessbillquery = "select distinct r.Roll_No ,r.Stud_Name,hs.HostelMasterFK as Hostel_Code,h.HostelName as Hostel_Name,b.Building_Name,f.Floor_Name,r.Batch_Year ,(C.Course_Name +' - '+dt.Dept_Name )as degree,m.MessMonth as BillMonth,m.MessYear as Bill_Year ,d.Degree_Code,a.sex,(md.MessAmount+md.MessAdditonalAmt) as total,md.MessAdditonalAmt as Additional_Amount,md.RebateAmount as Rebate_Amount , md.MessAmount as Fixed_Amount,md.RebateDays as Rebete_days,hms.Rebate_Amount,ExpanceGroupAmtTotal,hs.studmesstype,hs.id from HT_MessBillMaster m,HT_MessBillDetail md ,HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,applyn a,Building_Master b,Floor_Master f,HMessbill_StudDetails hms where m.MessBillMasterPK =md.MessBillMasterFK and h.HostelMasterPK=hs.HostelMasterFK  and hs.APP_No=md.App_No and r.App_No=md.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.App_No =a.app_no and hs.BuildingFK=b.Code and hs.FloorFK =f.FloorPK and hs.HostelMasterFK =h.HostelMasterPK";//md.App_No as Roll_No +ExpanceGroupAmtTotal -md.RebateAmount(md.MessAmount+md.MessAdditonalAmt) 


                    string monthlymessbillquery = "select distinct r.Roll_No ,r.Stud_Name,hs.HostelMasterFK as Hostel_Code,h.HostelName as Hostel_Name,b.Building_Name,f.Floor_Name,r.Batch_Year ,(C.Course_Name +' - '+dt.Dept_Name )as degree,m.MessMonth as BillMonth,m.MessYear as Bill_Year ,d.Degree_Code,a.sex,(md.MessAmount+md.MessAdditonalAmt) as total,md.MessAdditonalAmt as Additional_Amount,md.RebateAmount as Rebate_Amount , md.MessAmount as Fixed_Amount,md.RebateDays as Rebete_days,hms.Rebate_Amount,ExpanceGroupAmtTotal,hs.studmesstype,hs.id,rd.Room_Name from HT_MessBillMaster m,HT_MessBillDetail md ,HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,applyn a,Building_Master b,Floor_Master f,HMessbill_StudDetails hms,Room_Detail rd where m.MessBillMasterPK =md.MessBillMasterFK and h.HostelMasterPK=hs.HostelMasterFK  and hs.APP_No=md.App_No and r.App_No=md.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.App_No =a.app_no and hs.BuildingFK=b.Code and hs.FloorFK =f.FloorPK and rd.Building_Name=b.Building_Name and rd.Floor_Name=f.Floor_Name and rd.RoomPK=hs.RoomFK and hs.HostelMasterFK =h.HostelMasterPK";//modified by rajasekar 18/07/2018
                    monthlymessbillquery = monthlymessbillquery + " and hs.BuildingFK in ('" + building + "') and hs.FloorFK in ('" + floor + "')";
                    monthlymessbillquery = monthlymessbillquery + " and r.college_code in ('" + colg + "') and r.degree_code in ('" + dept + "')";
                    monthlymessbillquery = monthlymessbillquery + "  and hs.HostelMasterFK in ('" + hos + "') and a.sex in ('" + gender + "') and hs.Messcode='" + Convert.ToString(ddl_messname.SelectedValue) + "'";
                    monthlymessbillquery = monthlymessbillquery + " and  m.MessMonth in('" + month + "') and m.MessYear in('" + year + "')";
                    monthlymessbillquery = monthlymessbillquery + " and ISNULL (IsSuspend,0) =0 and isnull(IsVacated,0)=0 and isnull(IsDiscontinued ,0)=0 and hs.HostelMasterFK =h.HostelMasterPK and hms.MessBill_Month=m.MessMonth and hms.MessBill_Year=m.MessYear and hms.Hostel_Code=m.MessMasterFK  and hms.MessType=hs.StudMessType and hms.memtype=hs.MemType ";//22.12.17 barath hms.rebate_days
                    ds = d2.select_method_wo_parameter(monthlymessbillquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = Itemindex.Count + 1;
                        FpSpread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[0].Width = 50;
                        for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                        {
                            if (cblcolumnorder.Items[i].Selected == true)
                            {
                                hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                                string colvalue = cblcolumnorder.Items[i].Text;
                                if (ItemList.Contains(colvalue) == false)
                                {
                                    ItemList.Add(cblcolumnorder.Items[i].Text);
                                }
                                tborder.Text = "";
                                for (int j = 0; j < ItemList.Count; j++)
                                {
                                    tborder.Text = tborder.Text + ItemList[j].ToString();
                                    tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";
                                }
                            }
                            cblcolumnorder.Items[0].Enabled = false;
                        }
                        if (ItemList.Count == 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                cblcolumnorder.Items[i].Selected = true;
                                hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                                string colvalue = cblcolumnorder.Items[i].Text;
                                if (ItemList.Contains(colvalue) == false)
                                {
                                    ItemList.Add(cblcolumnorder.Items[i].Text);
                                }
                                tborder.Text = "";
                                for (int j = 0; j < ItemList.Count; j++)
                                {
                                    tborder.Text = tborder.Text + ItemList[j].ToString();
                                    tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";
                                }
                            }
                        }
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].ColumnCount = 1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                        style2.Font.Size = 13;
                        style2.Font.Name = "Book Antiqua";
                        style2.Font.Bold = true;
                        style2.HorizontalAlign = HorizontalAlign.Center;
                        style2.ForeColor = Color.Black;
                        style2.BackColor = Color.AliceBlue;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        for (int i = 0; i < ItemList.Count; i++)
                        {
                            string value1 = ItemList[i].ToString();
                            int a = value1.Length;
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = ItemList[i].ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpSpread1.Sheets[0].RowCount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            count++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            int c = 0;
                            for (int j = 0; j < ItemList.Count; j++)
                            {
                                string k = Convert.ToString(ItemList[j].ToString());
                                string value = Convert.ToString(hat[k].ToString());
                                c++;
                                FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                FpSpread1.Sheets[0].Columns[2].CellType = textcel_type;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][value].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                                //14.10.15
                                if (value == Convert.ToString("BillMonth"))
                                {
                                    string BillMonth = ds.Tables[0].Rows[i][value].ToString();
                                    string billmonth = returnMonYear(BillMonth);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = billmonth;
                                }
                                //if (value == Convert.ToString("total"))//23.12.17
                                //{
                                //    double vegCount = 0;
                                //    if (Convert.ToString(ds.Tables[0].Rows[i]["studmesstype"]) == "1")
                                //        double.TryParse(Convert.ToString(d2.GetFunction(" select top(1) isnull(Per_Day_Amount,0)*isnull(No_Of_Days,0)as VegAmount from HMessbill_StudDetails sd,HT_MessBillDetail d where sd.MemType=d.MemType and MessBill_Month in('" + Convert.ToString(ds.Tables[0].Rows[i]["BillMonth"]) + "') and MessBill_Year in('" + Convert.ToString(ds.Tables[0].Rows[i]["Bill_Year"]) + "') and sd.memtype='1' and sd.MessType='0'")), out vegCount);
                                //    double total = 0;
                                //    double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["total"]), out total);
                                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = Convert.ToString(total + vegCount);
                                //}
                                if (value == Convert.ToString("sex"))
                                {
                                    string sex = ds.Tables[0].Rows[i][value].ToString();
                                    if (sex == "0")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = "Male";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    else if (sex == "1")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = "Female";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = "Both";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                }
                            }
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        div1.Visible = true;
                        FpSpread1.Visible = true;
                        // FpSpread1.SaveChanges();                  
                        pheaderfilter.Visible = true;
                        pcolumnorder.Visible = true;
                        div1.Visible = true;
                        lbl_error.Visible = false;
                        rptprint.Visible = true;
                        //lbl_error1.Visible = false;
                        pheaderfilter1.Visible = false;
                        pcolumnorder1.Visible = false;
                        pcolumnorder.Visible = true;
                        pcolumnorder2.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        div1.Visible = false;
                        lbl_error.Visible = true;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        lbl_error.Text = "No Record Found";
                        //lbl_error1.Visible = true;
                        rptprint.Visible = false;
                        pheaderfilter1.Visible = false;
                        pcolumnorder1.Visible = false;
                        pcolumnorder2.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                    }
                }
                else
                {
                    div1.Visible = false;
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    pheaderfilter1.Visible = false;
                    pcolumnorder1.Visible = false;
                    pcolumnorder2.Visible = false;
                    pheaderfilterstu.Visible = false;
                    pcolumnorderstu.Visible = false;
                    pheaderfilterguest.Visible = false;
                    pcolumnorderguest.Visible = false;
                    lbl_error.Text = "Please Select All Field";
                    FpSpread2.Visible = false;
                    div2.Visible = false;
                }
                #endregion
            }
            else if (rdb_fromat1.Checked == true && rdb_staff.Checked == true)
            {
                ItemList1.Clear();
                Hashtable columnhash = new Hashtable();
                columnhash.Add("Hostel_Name", "Hostel Name");
                columnhash.Add("APP_No", "APP NO");
                columnhash.Add("id", "Staff Id");
                columnhash.Add("appl_name", "Staff Name");
                //columnhash.Add("Guest_Address", "Guest Address");
                //columnhash.Add("MobileNo", "Mobile No");
                //columnhash.Add("From_Company", "From Company");
                columnhash.Add("Floor_Name", "Floor Name");
                columnhash.Add("Room_Name", "Room Name");
                // columnhash.Add("Admission_Date", "Admission Date");
                columnhash.Add("Building_Name", "Building Name");
                //columnhash.Add("Guest_Street", "Guest Street");
                //columnhash.Add("Guest_City", "Guest City");
                //columnhash.Add("Guest_PinCode", "Guest Pincode");
                //columnhash.Add("purpose", "Purpose");
                columnhash.Add("BillMonth", "BillMonth");
                columnhash.Add("Bill_Year", "Bill Year");
                columnhash.Add("total", "Total");
                columnhash.Add("Additional_Amount", "Additional Amount");
                columnhash.Add("Rebete_days", "Calculate Days");
                columnhash.Add("Rebate_Amount", "Rebate Amount");
                columnhash.Add("Fixed_Amount", "Mess Bill Amount");//added by saranyadevi 13.3.2018
                //columnhash.Add("Room_Name", "Room");//added by rajasekar 18.07.2018

                if (hostel.Trim() != "" && month.Trim() != "" && year.Trim() != "" && building.Trim() != "" && floor.Trim() != "" && dept.Trim() != "" && gender.Trim() != "" && hos.Trim()!="")
                {

                    string staff_new = "select distinct  gr.APP_No,appl_name,hd.HostelName as Hostel_Name,f.Floor_Name,r.Room_Name,gr.HostelMasterFK as Hostel_Code,bm.Building_Name,bm.Code,m.MessMonth as BillMonth,gr.id,m.MessYear as Bill_Year,(md.MessAmount+md.MessAdditonalAmt)  as  total,md.MessAdditonalAmt as Additional_Amount ,md.RebateAmount as Rebate_Amount,ExpanceGroupAmtTotal,md.MessAmount  as Fixed_Amount,md.RebateDays as Rebete_days from HT_MessBillMaster m,HT_MessBillDetail md ,HT_HostelRegistration gr,HM_HostelMaster hd,Building_Master bm,Floor_Master f,Room_Detail r ,HMessbill_StudDetails hms,staff_appl_master sam,staffmaster sm where m.MessBillMasterPK =md.MessBillMasterFK and gr.HostelMasterFK=hd.HostelMasterPK and gr.APP_No =md.App_No and bm.Code=gr.BuildingFK and gr.FloorFK=f.Floorpk and gr.RoomFK=r.Roompk and gr.BuildingFK in('" + building + "') and FloorFK in('" + floor + "')  and gr.HostelMasterFK in('" + hos + "') and  m.MessMonth in('" + month + "') and m.MessYear in('" + year + "') and gr.IsVacated='0' and hms.MessBill_Month=m.MessMonth and hms.MessBill_Year=m.MessYear and hms.memtype=gr.MemType and gr.MemType='2' and sam.appl_no = sm.appl_no and appl_id=gr.APP_No and gr.Messcode='" + Convert.ToString(ddl_messname.SelectedValue) + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(staff_new, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = Itemindex1.Count + 1;
                        FpSpread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[0].Width = 50;
                        for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                        {
                            if (cblcolumnorder2.Items[i].Selected == true)
                            {
                                columnhash.Add(cblcolumnorder2.Items[i].Text, cblcolumnorder2.Items[i].Value);
                                string colvalue = cblcolumnorder2.Items[i].Text;
                                if (ItemList1.Contains(colvalue) == false)
                                {
                                    ItemList1.Add(cblcolumnorder2.Items[i].Text);
                                }
                                tborder1.Text = "";
                                for (int j = 0; j < ItemList1.Count; j++)
                                {
                                    tborder1.Text = tborder1.Text + ItemList1[j].ToString();
                                    tborder1.Text = tborder1.Text + "(" + (j + 1).ToString() + ")  ";
                                }
                            }
                            cblcolumnorder2.Items[0].Enabled = false;
                        }
                        if (ItemList1.Count == 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                cblcolumnorder2.Items[i].Selected = true;
                                columnhash.Add(cblcolumnorder2.Items[i].Text, cblcolumnorder2.Items[i].Value);
                                string colvalue = cblcolumnorder2.Items[i].Text;
                                if (ItemList1.Contains(colvalue) == false)
                                {
                                    ItemList1.Add(cblcolumnorder2.Items[i].Text);
                                }
                                tborder1.Text = "";
                                for (int j = 0; j < ItemList1.Count; j++)
                                {
                                    tborder1.Text = tborder1.Text + ItemList1[j].ToString();
                                    tborder1.Text = tborder1.Text + "(" + (j + 1).ToString() + ")  ";
                                }
                            }
                        }
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].ColumnCount = 1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                        style2.Font.Size = 13;
                        style2.Font.Name = "Book Antiqua";
                        style2.Font.Bold = true;
                        style2.HorizontalAlign = HorizontalAlign.Center;
                        style2.ForeColor = Color.Black;
                        style2.BackColor = Color.AliceBlue;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        for (int i = 0; i < ItemList1.Count; i++)
                        {
                            string value1 = ItemList1[i].ToString();
                            int a = value1.Length;
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = ItemList1[i].ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpSpread1.Sheets[0].RowCount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            count++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            int c = 0;
                            for (int j = 0; j < ItemList1.Count; j++)
                            {
                                string k = Convert.ToString(ItemList1[j].ToString());
                                string value = Convert.ToString(columnhash[k].ToString());
                                c++;
                                //FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                //FpSpread1.Sheets[0].Columns[2].CellType = textcel_type;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][value].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                                if (value == Convert.ToString("BillMonth"))
                                {
                                    string BillMonth = ds.Tables[0].Rows[i][value].ToString();
                                    string billmonth = returnMonYear(BillMonth);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = billmonth;
                                }
                            }
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        div1.Visible = true;
                        FpSpread1.Visible = true;
                        // FpSpread1.SaveChanges();                  
                        pheaderfilter1.Visible = true;
                        pcolumnorder2.Visible = true;
                        lbl_error.Visible = false;
                        rptprint.Visible = true;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                    }
                   
 
                }
                #region magesh
                //FpSpread1.Visible = false;
                //div1.Visible = false;
                //lbl_error.Visible = true;
                //pheaderfilter.Visible = false;
                //pcolumnorder.Visible = false;
                //lbl_error.Text = "No Record Found";
                ////lbl_error1.Visible = true;
                //rptprint.Visible = false;
                //pheaderfilter1.Visible = false;
                //pcolumnorder1.Visible = false;
                //pheaderfilterstu.Visible = false;
                //pcolumnorderstu.Visible = false;
                //pheaderfilterguest.Visible = false;
                //pcolumnorderguest.Visible = false;
                //FpSpread2.Visible = false;
                //div2.Visible = false;
                #endregion
            }
            else if (rdb_fromat1.Checked == true && rdb_guest.Checked == true)
            {
                ItemList1.Clear();
                Hashtable columnhash = new Hashtable();
                columnhash.Add("Hostel_Name", "Hostel Name");
                columnhash.Add("Guest_Name", "Guest Name");
                columnhash.Add("id", "Guest Id");
                columnhash.Add("Guest_Address", "Guest Address");
                columnhash.Add("MobileNo", "Mobile No");
                columnhash.Add("From_Company", "From Company");
                columnhash.Add("Floor_Name", "Floor Name");
                columnhash.Add("Room_Name", "Room Name");
                // columnhash.Add("Admission_Date", "Admission Date");
                columnhash.Add("Building_Name", "Building Name");
                columnhash.Add("Guest_Street", "Guest Street");
                columnhash.Add("Guest_City", "Guest City");
                columnhash.Add("Guest_PinCode", "Guest Pincode");
                //columnhash.Add("purpose", "Purpose");
                columnhash.Add("BillMonth", "BillMonth");
                columnhash.Add("Bill_Year", "Bill Year");
                columnhash.Add("total", "Total");
                columnhash.Add("Additional_Amount", "Additional Amount");
                columnhash.Add("Rebete_days", "Calculate Days");
                columnhash.Add("Rebate_Amount", "Rebate Amount");
                columnhash.Add("Fixed_Amount", "Mess Bill Amount");//added by saranyadevi 13.3.2018

                if (hostel.Trim() != "" && month.Trim() != "" && year.Trim() != "" && building.Trim() != "" && floor.Trim() != "" && dept.Trim() != "" && gender.Trim() != "" && hos.Trim() != "")
                {
                    //string q1 = "select distinct hd.HostelName as Hostel_Name,ve.VendorName as Guest_Name,gr.GuestVendorFK as GuestCode,ve.VendorAddress as Guest_Address,MobileNo,ve.VendorCompName as From_Company ,f.Floor_Name,r.Room_Name,gr.HostelMasterFK as Hostel_Code,bm.Building_Name,bm.Code,ve.VendorStreet as Guest_Street,ve.VendorCity as Guest_City,ve.VendorPin as Guest_PinCode,m.MessMonth as BillMonth,m.MessYear as Bill_Year,(md.MessAmount+md.RebateAmount+md.MessAdditonalAmt)  as Fixed_Amount,md.MessAdditonalAmt as Additional_Amount ,md.RebateAmount as Rebate_Amount,md.MessAmount  as total,md.RebateDays as Rebete_days from HT_MessBillMaster m,HT_MessBillDetail md ,HT_HostelRegistration gr,HM_HostelMaster hd,Building_Master bm,CO_VendorMaster ve,Floor_Master f,Room_Detail r ,HMessbill_StudDetails hms where m.MessBillMasterPK =md.MessBillMasterFK and gr.HostelMasterFK=hd.HostelMasterPK and gr.APP_No =md.App_No and bm.Code=gr.BuildingFK and gr.FloorFK=f.Floorpk and gr.RoomFK=r.Roompk and gr.GuestVendorFK=ve.VendorPK and gr.BuildingFK in('" + building + "') and FloorFK in('" + floor + "')  and gr.HostelMasterFK in('" + hostelcod + "') and  m.MessMonth in('" + month + "') and m.MessYear in('" + year + "') and gr.IsVacated='0' and hms.MessBill_Month=m.MessMonth and hms.MessBill_Year=m.MessYear and hms.memtype=gr.MemType ";
                    //magesh 31.5.18
                    //string q1 = "select distinct hd.HostelName as Hostel_Name,ve.VendorName as Guest_Name,gr.GuestVendorFK as GuestCode,ve.VendorAddress as Guest_Address,MobileNo,ve.VendorCompName as From_Company ,f.Floor_Name,r.Room_Name,gr.HostelMasterFK as Hostel_Code,bm.Building_Name,bm.Code,ve.VendorStreet as Guest_Street,ve.VendorCity as Guest_City,ve.VendorPin as Guest_PinCode,m.MessMonth as BillMonth,m.MessYear as Bill_Year,(md.MessAmount+md.MessAdditonalAmt)  as total ,md.MessAdditonalAmt as Additional_Amount ,md.RebateAmount as Rebate_Amount,md.MessAmount  as Fixed_Amount,md.RebateDays as Rebete_days from HT_MessBillMaster m,HT_MessBillDetail md ,HT_HostelRegistration gr,HM_HostelMaster hd,Building_Master bm,CO_VendorMaster ve,Floor_Master f,Room_Detail r ,HMessbill_StudDetails hms where m.MessBillMasterPK =md.MessBillMasterFK and gr.HostelMasterFK=hd.HostelMasterPK and gr.APP_No =md.App_No and bm.Code=gr.BuildingFK and gr.FloorFK=f.Floorpk and gr.RoomFK=r.Roompk and gr.GuestVendorFK=ve.VendorPK and gr.BuildingFK in('" + building + "') and FloorFK in('" + floor + "')  and gr.HostelMasterFK in('" + hostelcod + "') and  m.MessMonth in('" + month + "') and m.MessYear in('" + year + "') and gr.IsVacated='0' and hms.MessBill_Month=m.MessMonth and hms.MessBill_Year=m.MessYear and hms.memtype=gr.MemType ";

                    string q1 = "select distinct hd.HostelName as Hostel_Name,vi.VenContactName as Guest_Name,gr.GuestVendorFK as GuestCode,ve.VendorAddress as Guest_Address,Vi.VendorMobileNo as MobileNo,ve.VendorCompName as From_Company ,f.Floor_Name,r.Room_Name,gr.HostelMasterFK as Hostel_Code,bm.Building_Name,bm.Code,ve.VendorStreet as Guest_Street,ve.VendorCity as Guest_City,ve.VendorPin as Guest_PinCode,m.MessMonth as BillMonth,m.MessYear as Bill_Year,(md.MessAmount+md.MessAdditonalAmt)  as total ,md.MessAdditonalAmt as Additional_Amount ,md.RebateAmount as Rebate_Amount,md.MessAmount  as Fixed_Amount,md.RebateDays as Rebete_days,gr.id from HT_MessBillMaster m,HT_MessBillDetail md ,HT_HostelRegistration gr,HM_HostelMaster hd,Building_Master bm,CO_VendorMaster ve,Floor_Master f,Room_Detail r ,HMessbill_StudDetails hms,IM_VendorContactMaster Vi where m.MessBillMasterPK =md.MessBillMasterFK and gr.HostelMasterFK=hd.HostelMasterPK and gr.APP_No =md.App_No and bm.Code=gr.BuildingFK and gr.FloorFK=f.Floorpk and gr.RoomFK=r.Roompk and gr.GuestVendorFK=ve.VendorPK and gr.BuildingFK in('" + building + "') and FloorFK in('" + floor + "')  and gr.HostelMasterFK in('" + hos + "') and  m.MessMonth in('" + month + "') and m.MessYear in('" + year + "') and gr.IsVacated='0' and hms.MessBill_Month=m.MessMonth and hms.MessBill_Year=m.MessYear and hms.memtype=gr.MemType and ve.VendorPK=vi.VendorFK  and vi.VendorContactPK=gr.APP_No and gr.Messcode='" + Convert.ToString(ddl_messname.SelectedValue) + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = Itemindex1.Count + 1;
                        FpSpread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Columns[0].Width = 50;
                        for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                        {
                            if (cblcolumnorder1.Items[i].Selected == true)
                            {
                                columnhash.Add(cblcolumnorder1.Items[i].Text, cblcolumnorder1.Items[i].Value);
                                string colvalue = cblcolumnorder1.Items[i].Text;
                                if (ItemList1.Contains(colvalue) == false)
                                {
                                    ItemList1.Add(cblcolumnorder1.Items[i].Text);
                                }
                                tborder1.Text = "";
                                for (int j = 0; j < ItemList1.Count; j++)
                                {
                                    tborder1.Text = tborder1.Text + ItemList1[j].ToString();
                                    tborder1.Text = tborder1.Text + "(" + (j + 1).ToString() + ")  ";
                                }
                            }
                            cblcolumnorder1.Items[0].Enabled = false;
                        }
                        if (ItemList1.Count == 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                cblcolumnorder1.Items[i].Selected = true;
                                columnhash.Add(cblcolumnorder1.Items[i].Text, cblcolumnorder1.Items[i].Value);
                                string colvalue = cblcolumnorder1.Items[i].Text;
                                if (ItemList1.Contains(colvalue) == false)
                                {
                                    ItemList1.Add(cblcolumnorder1.Items[i].Text);
                                }
                                tborder1.Text = "";
                                for (int j = 0; j < ItemList1.Count; j++)
                                {
                                    tborder1.Text = tborder1.Text + ItemList1[j].ToString();
                                    tborder1.Text = tborder1.Text + "(" + (j + 1).ToString() + ")  ";
                                }
                            }
                        }
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].ColumnCount = 1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                        style2.Font.Size = 13;
                        style2.Font.Name = "Book Antiqua";
                        style2.Font.Bold = true;
                        style2.HorizontalAlign = HorizontalAlign.Center;
                        style2.ForeColor = Color.Black;
                        style2.BackColor = Color.AliceBlue;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        for (int i = 0; i < ItemList1.Count; i++)
                        {
                            string value1 = ItemList1[i].ToString();
                            int a = value1.Length;
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = ItemList1[i].ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpSpread1.Sheets[0].RowCount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            count++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            int c = 0;
                            for (int j = 0; j < ItemList1.Count; j++)
                            {
                                string k = Convert.ToString(ItemList1[j].ToString());
                                string value = Convert.ToString(columnhash[k].ToString());
                                c++;
                                //FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                //FpSpread1.Sheets[0].Columns[2].CellType = textcel_type;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][value].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                                if (value == Convert.ToString("BillMonth"))
                                {
                                    string BillMonth = ds.Tables[0].Rows[i][value].ToString();
                                    string billmonth = returnMonYear(BillMonth);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = billmonth;
                                }
                            }
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        div1.Visible = true;
                        FpSpread1.Visible = true;
                        // FpSpread1.SaveChanges();                  
                        pheaderfilter1.Visible = true;
                        pcolumnorder1.Visible = true;
                        pcolumnorder2.Visible = false;
                        lbl_error.Visible = false;
                        rptprint.Visible = true;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        div1.Visible = false;
                        lbl_error.Visible = true;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        lbl_error.Text = "No Record Found";
                        //lbl_error1.Visible = true;
                        rptprint.Visible = false;
                        pheaderfilter1.Visible = false;
                        pcolumnorder1.Visible = false;
                        pcolumnorder2.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                    }
                }
                else
                {
                    div1.Visible = false;
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    pheaderfilter1.Visible = false;
                    pcolumnorder1.Visible = false;
                    pcolumnorder2.Visible = false;
                    pheaderfilterstu.Visible = false;
                    pcolumnorderstu.Visible = false;
                    pheaderfilterguest.Visible = false;
                    pcolumnorderguest.Visible = false;
                    lbl_error.Text = "Please Select All Field";
                    FpSpread2.Visible = false;
                    div2.Visible = false;
                }
            }
            else if (rdb_fromat2.Checked == true && rdb_student.Checked == true)
            {
                if (hostel.Trim() != "" && month.Trim() != "" && year.Trim() != "")
                {
                    string selectquerymnth = "";
                    if (txt_rollno.Text.Trim() != "")
                    {
                        string rnum = "";
                        rnum = Convert.ToString(txt_rollno.Text);
                        string[] splitrnum = rnum.Split('-');
                        string rno = "";
                        if (splitrnum.Length > 0)
                        {
                            rno = Convert.ToString(splitrnum[0] + splitrnum[1] + splitrnum[2] + splitrnum[3]);
                        }
                        //selectquerymnth = "  SELECT distinct r.Stud_Name,r.Roll_No, SUM ( BalanceAmt) as BalanceAmt,SUM(MessAmount) as Fixed_Amount, sum(MessAdditonalAmt)as Additional_Amount,GroupCode,GroupAmount,msn.MessName , MessYear,MessMonth,r.Roll_No,Per_Day_Amount FROM HT_MessBillMaster M,HT_MessBillDetail D,registration r,ft_excessdet t,Degree g,course c,Department p,HT_HostelRegistration hs,applyn a,HM_MessMaster msn,HMessbill_StudDetails hms WHERE a.app_no =r.App_No and  hs.APP_No =r.App_No and M.MessBillMasterPK = D.MessBillMasterFK and d.App_No = r.App_No and r.app_no = t.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.Dept_Code = p.Dept_Code and msn.MessMasterPK =m.MessMasterFK  and hms.MessBill_Month=m.MessMonth and MessMonth in ('" + month + "') and MessYear  in ('" + year + "')  and r.Roll_No ='" + splitrnum[0] + "' and hms.Hostel_Code=m.MessMasterFK and m.MessBillMasterPK=d.MessBillMasterFK and msn.MessMasterPK  in ('" + Convert.ToString(ddl_messname.SelectedItem.Value) + "') and hms.Hostel_Code=M.MessMasterFK  and m.MessMonth=hms.MessBill_Month group by MessMonth ,MessYear ,GroupCode,GroupAmount,MessName,Roll_No, Per_Day_Amount , Roll_No,r.Stud_Name";
                        selectquerymnth = " select r.Stud_Name,r.Roll_No,ft.BalAmount as BalanceAmt,FeeAmount as Fixed_Amount,MessAdditonalAmt as Additional_Amount, GroupCode,GroupAmount,GroupAmount,mm.MessName,mm.MessMasterPK, m.MessMonth,m.MessYear,hms.Per_Day_Amount from FT_FeeallotMonthly fm,FT_FeeAllot ft,HT_HostelRegistration  hs,HT_MessBillMaster m,HT_MessBillDetail d,Registration r,Degree g,course c,Department p,HM_MessMaster mm,HMessbill_StudDetails hms,applyn a where a.app_no=d.app_no and a.app_no=ft.app_no and r.app_no=a.app_no and hs.app_no=a.app_no and hms.Hostel_Code=m.MessMasterFK and hms.Hostel_Code=m.MessMasterFK and hms.MessBill_Month=m.MessMonth and hms.MessBill_Year=m.MessYear and mm.MessMasterPK=m.MessMasterFK and r.degree_code=g.Degree_Code and g.Course_Id=c.Course_Id and g.Dept_Code=p.Dept_Code and m.MessBillMasterPK=d.MessBillMasterFK and m.MessMonth=fm.AllotMonth and m.MessYear=fm.AllotYear and ft.App_No=hs.APP_No and d.App_No=ft.App_No and ft.App_No=hs.APP_No and r.App_No=hs.APP_No and r.App_No=ft.App_No  and ft.FeeAllotPK=fm.FeeAllotPK and m.MessMonth in('" + month + "') and hms.MessType=hs.StudMessType and d.memtype=hms.memtype and hms.memtype='1' and m.MessYear in ('" + year + "') and  m.MessMasterFK  in ('" + Convert.ToString(ddl_messname.SelectedItem.Value) + "') and r.Roll_No ='" + splitrnum[0] + "' and hs.HostelMasterFK in('" + hos + "')";
                    }
                    else
                    {
                        //selectquerymnth = "SELECT distinct r.Stud_Name,r.Roll_No, SUM ( BalanceAmt) as BalanceAmt,SUM( mess_amount) as Fixed_Amount, sum (MessAdditonalAmt)as Additional_Amount, GroupCode,GroupAmount,msn.MessMasterPK , msn.MessName ,m.MessMonth,m.MessYear,Per_Day_Amount  FROM  HT_MessBillMaster M,HT_MessBillDetail D,Registration r,FT_ExcessDet t,Degree g,course c,Department p,HT_HostelRegistration  hs,applyn a,HM_MessMaster msn,HMessbill_StudDetails hms WHERE a.app_no =r.App_No and  hs.APP_No =r.App_No and M.MessBillMasterPK = D.MessBillMasterFK and d.App_No = r.App_No and r.app_no = t.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.Dept_Code = p.Dept_Code and msn.MessMasterPK =m.MessMasterFK  and hms.MessBill_Month=m.MessMonth and MessMonth in ('" + month + "') and MessYear in ('" + year + "') and  msn.MessMasterPK  in ('" + Convert.ToString(ddl_messname.SelectedItem.Value) + "') and hms.Hostel_Code=m.MessMasterFK  ";
                        selectquerymnth = " select r.Stud_Name,r.Roll_No,ft.BalAmount as BalanceAmt,FeeAmount as Fixed_Amount,MessAdditonalAmt as Additional_Amount, GroupCode,GroupAmount,GroupAmount,mm.MessName,mm.MessMasterPK, m.MessMonth,m.MessYear,hms.Per_Day_Amount from FT_FeeallotMonthly fm,FT_FeeAllot ft,HT_HostelRegistration  hs,HT_MessBillMaster m,HT_MessBillDetail d,Registration r,Degree g,course c,Department p,HM_MessMaster mm,HMessbill_StudDetails hms,applyn a where a.app_no=d.app_no and a.app_no=ft.app_no and r.app_no=a.app_no and hs.app_no=a.app_no and hms.Hostel_Code=m.MessMasterFK and hms.Hostel_Code=m.MessMasterFK and hms.MessBill_Month=m.MessMonth and hms.MessBill_Year=m.MessYear and mm.MessMasterPK=m.MessMasterFK and r.degree_code=g.Degree_Code and g.Course_Id=c.Course_Id and g.Dept_Code=p.Dept_Code and m.MessBillMasterPK=d.MessBillMasterFK and m.MessMonth=fm.AllotMonth and m.MessYear=fm.AllotYear and ft.App_No=hs.APP_No and d.App_No=ft.App_No and ft.App_No=hs.APP_No and r.App_No=hs.APP_No and r.App_No=ft.App_No  and ft.FeeAllotPK=fm.FeeAllotPK and m.MessMonth in('" + month + "') and m.MessYear in ('" + year + "') and  m.MessMasterFK  in ('" + Convert.ToString(ddl_messname.SelectedItem.Value) + "') and hms.MessType=hs.StudMessType and hs.HostelMasterFK in('" + hos + "')";
                        if (dept.Trim() != "")
                        {
                            selectquerymnth = selectquerymnth + " and r.degree_code in ('" + dept + "') ";
                        }
                        if (batch.Trim() != "")
                        {
                            selectquerymnth = selectquerymnth + " and r.Batch_Year in ('" + batch + "') ";
                        }
                        if (building.Trim() != "")
                        {
                            selectquerymnth = selectquerymnth + " and hs.BuildingFK in ('" + building + "') ";
                        }
                        if (floor.Trim() != "")
                        {
                            selectquerymnth = selectquerymnth + " and hs.FloorFK in ('" + floor + "') ";
                        }
                        if (gender.Trim() != "")
                        {
                            selectquerymnth = selectquerymnth + " and a.sex in ('" + gender + "') ";
                        }
                        if (rdb_paid.Checked == true)
                        {
                            selectquerymnth += " and fm.AllotAmount=isnull(fm.PaidAmount,0)";
                        }
                        if (rdb_unpaid.Checked == true)
                        {
                            selectquerymnth += " and fm.AllotAmount>isnull(fm.PaidAmount,0) and fm.paidamount is null";
                        }
                        if (rdb_yettobepaid.Checked == true)
                        {
                            selectquerymnth += " and fm.AllotAmount>isnull(fm.PaidAmount,0) and fm.PaidAmount is not null";
                        }
                        selectquerymnth += " and d.memtype=hms.memtype and hms.memtype='1'  and hms.memtype=hs.MemType ";
                        selectquerymnth += " order by r.Stud_Name,m.MessMonth,m.MessYear ";
                    }
                    dsmnth.Clear();
                    dsmnth = dmnth.select_method_wo_parameter(selectquerymnth, "Text");
                    if (dsmnth.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 10;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[0].Width = 50;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Mess Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[3].Width = 100;//1
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Bill Year";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[4].Width = 100;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Bill Month";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Per day Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Opening Dues";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        //Added by saranyadevi 13.3.2018
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Mess Bill Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Additional Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        string query = "";
                        query = d2.GetFunction(" select distinct GroupCode  from HT_MessBillMaster where MessMonth in('" + month + "') and MessYear in('" + year + "') and MessMasterFK in('" + Convert.ToString(ddl_messname.SelectedItem.Value) + "')");
                        string query1 = "";
                        if (query.Trim() != "")
                        {
                            query1 = " select mastervalue,mastercode from co_mastervalues where mastercode in (" + query + ")";
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(query1, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds1.Tables[0].Rows[row]["mastervalue"]);
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds1.Tables[0].Rows[row]["mastercode"]);
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Closing Balance";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        for (int row = 0; row < dsmnth.Tables[0].Rows.Count; row++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = dsmnth.Tables[0].Rows[row]["Roll_No"].ToString();
                            if (txt_rollno.Text.Trim() != "")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dsmnth.Tables[0].Rows[row]["Roll_No"]);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsmnth.Tables[0].Rows[row]["Stud_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsmnth.Tables[0].Rows[row]["Roll_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsmnth.Tables[0].Rows[row]["MessName"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsmnth.Tables[0].Rows[row]["MessYear"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            string value = "";
                            value = Convert.ToString(dsmnth.Tables[0].Rows[row]["MessMonth"]);
                            if (value.Trim() != "")
                            {
                                string billmonth = returnMonYear(value);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = billmonth;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dsmnth.Tables[0].Rows[row]["Per_Day_Amount"]);//charges
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dsmnth.Tables[0].Rows[row]["BalanceAmt"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dsmnth.Tables[0].Rows[row]["Fixed_Amount"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dsmnth.Tables[0].Rows[row]["Additional_Amount"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                            int c = 10;
                            string getvalue = Convert.ToString(dsmnth.Tables[0].Rows[row]["GroupAmount"]);
                            if (getvalue.Trim() != "")
                            {
                                string[] groupcode = getvalue.Split(',');
                                foreach (string g in groupcode)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = Convert.ToString(g);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                    c++;
                                }
                            }
                            string groupamt = Convert.ToString(dsmnth.Tables[0].Rows[row]["GroupAmount"]);
                            decimal amt = 0;
                            double total = 0;
                            if (groupamt.Trim() != "")
                            {
                                string[] groupcode1 = groupamt.Split(',');
                                foreach (string g1 in groupcode1)
                                {
                                    amt += Convert.ToDecimal(g1);
                                }
                            }
                            string addamt = Convert.ToString(dsmnth.Tables[0].Rows[row]["Additional_Amount"]);
                            string bill = Convert.ToString(dsmnth.Tables[0].Rows[row]["Fixed_Amount"]);
                            total = Convert.ToDouble(addamt) + Convert.ToDouble(bill);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = Convert.ToString(total);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                            c++;
                            string opdays = Convert.ToString(dsmnth.Tables[0].Rows[row]["BalanceAmt"]);
                            double clobal = 0;
                            clobal = Convert.ToDouble(opdays) - Convert.ToDouble(total);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = Convert.ToString(Convert.ToDouble(Math.Round(clobal, 2)));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                        }
                        FpSpread1.Visible = true;
                        rptprint.Visible = true;
                        div1.Visible = true;
                        lbl_error.Visible = false;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        pheaderfilter1.Visible = false;
                        pcolumnorder1.Visible = false;
                        pcolumnorder2.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                        txt_rollno.Text = "";
                    }
                    else
                    {
                        div1.Visible = false;
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        pheaderfilter1.Visible = false;
                        pcolumnorder1.Visible = false;
                        pcolumnorder2.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        lbl_error.Text = "No Records Found";
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                        txt_rollno.Text = "";
                    }
                }
                else
                {
                    div1.Visible = false;
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    pheaderfilter1.Visible = false;
                    pcolumnorder1.Visible = false;
                    pcolumnorder2.Visible = false;
                    pheaderfilterstu.Visible = false;
                    pcolumnorderstu.Visible = false;
                    pheaderfilterguest.Visible = false;
                    pcolumnorderguest.Visible = false;
                    lbl_error.Text = "Please Select All Field";
                    FpSpread2.Visible = false;
                    div2.Visible = false;
                    txt_rollno.Text = "";
                }
            }
            else if (rdb_fromat2.Checked == true && rdb_staff.Checked == true)
            {
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = true;
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                lbl_error.Text = "No Record Found";
                //lbl_error1.Visible = true;
                rptprint.Visible = false;
                pheaderfilter1.Visible = false;
                pcolumnorder1.Visible = false;
                pcolumnorder2.Visible = false;
                pheaderfilterstu.Visible = false;
                pcolumnorderstu.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
            }
            //else if (rdb_fromat2.Checked == true && rdb_staff.Checked == true && rdb_indivual.Checked == true)
            //{
            //    FpSpread1.Visible = false;
            //    div1.Visible = false;
            //    lbl_error.Visible = true;
            //    pheaderfilter.Visible = false;
            //    pcolumnorder.Visible = false;
            //    lbl_error.Text = "No Record Found";
            //    //lbl_error1.Visible = true;
            //    rptprint.Visible = false;
            //    pheaderfilter1.Visible = false;
            //    pcolumnorder1.Visible = false;
            //    pheaderfilterstu.Visible = false;
            //    pcolumnorderstu.Visible = false;
            //}
            else if (rdb_fromat2.Checked == true && rdb_guest.Checked == true)
            {
                //ItemListguest.Clear();
                //Hashtable columnhash = new Hashtable();
                //columnhash.Add("Hostel_Name", "Guest Name");
                //columnhash.Add("Guest_Name", "Company Name");
                //columnhash.Add("Guest_Address", "Guest Code");
                //columnhash.Add("MobileNo", "Receipt No");
                //columnhash.Add("From_Company", "Date");
                //columnhash.Add("Floor_Name", "Amount");
                //columnhash.Add("Room_Name", "Opening Dues");
                //columnhash.Add("Admission_Date", "Days");
                //columnhash.Add("Building_Name", "Charges");
                //columnhash.Add("Guest_Street", "Bill");
                //columnhash.Add("Guest_City", "Total");
                ////columnhash.Add("Guest_PinCode", "Guest Pincode");
                ////columnhash.Add("purpose", "Purpose");
                ////columnhash.Add("BillMonth", "BillMonth");
                ////columnhash.Add("Bill_Year", "Bill Year");
                ////columnhash.Add("Fixed_Amount", "Fixed Amount");
                ////columnhash.Add("Additional_Amount", "Additional Amount");
                ////columnhash.Add("Rebete_days", "Calculate Days");
                ////columnhash.Add("Rebate_Amount", "Rebate Amount");
                ////columnhash.Add("total", "Total");
                //string q1 = "";
                //if (rdb_common.Checked == true && rdb_guest.Checked == true)
                //{
                //    q1 = "";
                //}
                //else if (rdb_indivual.Checked == true && rdb_guest.Checked == true)
                //{
                //}
                //ds.Clear();
                //ds = d2.select_method_wo_parameter(q1, "Text");
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    FpSpread1.Sheets[0].RowCount = 0;
                //    FpSpread1.Sheets[0].ColumnCount = 0;
                //    FpSpread1.CommandBar.Visible = false;
                //    FpSpread1.Sheets[0].AutoPostBack = true;
                //    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                //    FpSpread1.Sheets[0].RowHeader.Visible = false;
                //    FpSpread1.Sheets[0].ColumnCount = Itemindexguest.Count + 1;
                //    FpSpread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //    darkstyle.ForeColor = Color.White;
                //    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                //    FpSpread1.Columns[0].Width = 50;
                //    for (int i = 0; i < cbl_guest.Items.Count; i++)
                //    {
                //        if (cbl_guest.Items[i].Selected == true)
                //        {
                //            columnhash.Add(cbl_guest.Items[i].Text, cbl_guest.Items[i].Value);
                //            string colvalue = cbl_guest.Items[i].Text;
                //            if (ItemListguest.Contains(colvalue) == false)
                //            {
                //                ItemListguest.Add(cbl_guest.Items[i].Text);
                //            }
                //            txt_guestcol.Text = "";
                //            for (int j = 0; j < ItemListguest.Count; j++)
                //            {
                //                txt_guestcol.Text = txt_guestcol.Text + ItemListguest[j].ToString();
                //                txt_guestcol.Text = txt_guestcol.Text + "(" + (j + 1).ToString() + ")  ";
                //            }
                //        }
                //        cbl_guest.Items[0].Enabled = false;
                //    }
                //    if (ItemListguest.Count == 0)
                //    {
                //        for (int i = 0; i < 3; i++)
                //        {
                //            cbl_guest.Items[i].Selected = true;
                //            columnhash.Add(cbl_guest.Items[i].Text, cbl_guest.Items[i].Value);
                //            string colvalue = cbl_guest.Items[i].Text;
                //            if (ItemListguest.Contains(colvalue) == false)
                //            {
                //                ItemListguest.Add(cbl_guest.Items[i].Text);
                //            }
                //            txt_guestcol.Text = "";
                //            for (int j = 0; j < ItemListguest.Count; j++)
                //            {
                //                txt_guestcol.Text = txt_guestcol.Text + ItemListguest[j].ToString();
                //                txt_guestcol.Text = txt_guestcol.Text + "(" + (j + 1).ToString() + ")  ";
                //            }
                //        }
                //    }
                //    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                //    FpSpread1.Sheets[0].ColumnCount = 1;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                //    FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                //    style2.Font.Size = 13;
                //    style2.Font.Name = "Book Antiqua";
                //    style2.Font.Bold = true;
                //    style2.HorizontalAlign = HorizontalAlign.Center;
                //    style2.ForeColor = Color.Black;
                //    style2.BackColor = Color.AliceBlue;
                //    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                //    for (int i = 0; i < ItemListguest.Count; i++)
                //    {
                //        string value1 = ItemListguest[i].ToString();
                //        int a = value1.Length;
                //        FpSpread1.Sheets[0].ColumnCount++;
                //        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = ItemListguest[i].ToString();
                //        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                //        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                //        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                //    }
                //    FpSpread1.Sheets[0].RowCount = 0;
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        FpSpread1.Sheets[0].RowCount++;
                //        count++;
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                //        //int c = 0;
                //        //for (int j = 0; j < ItemListguest.Count; j++)
                //        //{
                //        //    string k = Convert.ToString(ItemListguest[j].ToString());
                //        //    string value = Convert.ToString(columnhash[k].ToString());
                //        //    c++;
                //        //    //FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                //        //    //FpSpread1.Sheets[0].Columns[2].CellType = textcel_type;
                //        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][value].ToString();
                //        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                //        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                //        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                //        //    if (value == Convert.ToString("BillMonth"))
                //        //    {
                //        //        string BillMonth = ds.Tables[0].Rows[i][value].ToString();
                //        //        string billmonth = returnMonYear(BillMonth);
                //        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = billmonth;
                //        //    }
                //        //}
                //    }
                //    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                //    div1.Visible = true;
                //    FpSpread1.Visible = true;
                //    // FpSpread1.SaveChanges();                  
                //    pheaderfilter1.Visible = false;
                //    pcolumnorder1.Visible = false;
                //    lbl_error.Visible = false;
                //    rptprint.Visible = true;
                //    pheaderfilter.Visible = false;
                //    pcolumnorder.Visible = false;
                //    pheaderfilterstu.Visible = false;
                //    pcolumnorderstu.Visible = false;
                //    pheaderfilterguest.Visible = true;
                //    pcolumnorderguest.Visible = true;
                //}
                //else
                //{
                //    FpSpread1.Visible = false;
                //    div1.Visible = false;
                //    lbl_error.Visible = true;
                //    pheaderfilter.Visible = false;
                //    pcolumnorder.Visible = false;
                //    lbl_error.Text = "No Record Found";
                //    //lbl_error1.Visible = true;
                //    rptprint.Visible = false;
                //    pheaderfilter1.Visible = false;
                //    pcolumnorder1.Visible = false;
                //    pheaderfilterstu.Visible = false;
                //    pcolumnorderstu.Visible = false;
                //    pheaderfilterguest.Visible = false;
                //    pcolumnorderguest.Visible = false;
                //}
                if (hostel.Trim() != "" && floor.Trim() != "" && building.Trim() != "" && batch.Trim() != "" && month.Trim() != "" && year.Trim() != "" && degree.Trim() != "")
                {
                    string selectquery = "";
                    if (rdb_common.Checked == true && rdb_guest.Checked == true)
                    {
                        //selectquery = "SELECT d.Roll_No,Stud_Name,Course_Name+'-'+Dept_Acronym Degree,BalanceAmt,Fixed_Amount,Additional_Amount,GroupCode,GroupAmount,d.Rebete_days ,Charges  FROM MessBill_Master M,MessBill_Detail D,registration r,ft_excessdet t,Degree g,course c,Department p WHERE M.messbillmasterid = D.MessBill_MasterCode and d.roll_no = r.Roll_No and r.app_no = t.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.Dept_Code = p.Dept_Code and billmonth in ('" + month + "') and bill_year in ('" + year + "')";
                        selectquery = "SELECT d.Roll_No,r.Stud_Name,Course_Name+'-'+Dept_Acronym Degree,BalanceAmt,Fixed_Amount,Additional_Amount,GroupCode,GroupAmount,d.Rebete_days ,Charges  FROM MessBill_Master M,MessBill_Detail D,registration r,ft_excessdet t,Degree g,course c,Department p,Hostel_StudentDetails hs,applyn a WHERE a.app_no =r.App_No and  hs.Roll_No =r.Roll_No and M.messbillmasterid = D.MessBill_MasterCode and d.roll_no = r.Roll_No and r.app_no = t.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.Dept_Code = p.Dept_Code and billmonth in ('" + month + "') and bill_year in ('" + year + "') and r.degree_code in ('" + degree + "') and r.Batch_Year in ('" + batch + "') and hs.Building_Name in ('" + building + "') and hs.Floor_Name in ('" + floor + "') and m.Hostel_Code in ('" + hostelcod + "') and a.sex in ('" + gender + "') ";
                    }
                    else if (rdb_indivual.Checked == true && rdb_guest.Checked == true)
                    {
                        selectquery = "SELECT d.Roll_No,Stud_Name,Course_Name+'-'+Dept_Acronym Degree,BalanceAmt,Fixed_Amount,Additional_Amount,GroupCode,GroupAmount,d.Rebete_days ,Charges  FROM MessBill_Master M,MessBill_Detail D,registration r,ft_excessdet t,Degree g,course c,Department p WHERE M.messbillmasterid = D.MessBill_MasterCode and d.roll_no = r.Roll_No and r.app_no = t.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.Dept_Code = p.Dept_Code and billmonth in ('" + month + "') and bill_year in ('" + year + "')  and d.Roll_No+'-'+stud_name+'-'+Course_Name+'-'+Dept_Name ='" + txt_rollno.Text + "'";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 9;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[0].Width = 50;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Guest Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[1].Width = 100;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Guest Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[2].Width = 200;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Company Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Opening Dues";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Days";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Charges";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Bill";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Additional Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        string query = "";
                        query = d2.GetFunction("select distinct GroupCode  from MessBill_Master where Bill_Year in ('" + year + "') and BillMonth in ('" + month + "') and Hostel_Code in ('" + hostelcod + "')");
                        //string groupcode1 = "";
                        //char[] delimiterChars = { ',' };
                        //string[] groupcode = query.Split(delimiterChars);
                        //foreach (string g in groupcode)
                        //{
                        string query1 = "";
                        if (query.Trim() != "")
                        {
                            query1 = "select TextVal,TextCode  from TextValTable where TextCode in (" + query + ")";
                            ds1.Clear();
                            ds1 = d2.select_method_wo_parameter(query1, "Text");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds1.Tables[0].Rows[row]["TextVal"]);
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds1.Tables[0].Rows[row]["TextCode"]);
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Closing Balance";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Guest_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["GuestCode"]);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["rpu"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["From_Company"]);
                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Stock_Value"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["BalanceAmt"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Rebete_days"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Charges"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Fixed_Amount"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["Additional_Amount"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            int c = 9;
                            string getvalue = Convert.ToString(ds.Tables[0].Rows[row]["GroupAmount"]);
                            if (getvalue.Trim() != "")
                            {
                                char[] delimiterChars = { ',' };
                                string[] groupcode = getvalue.Split(delimiterChars);
                                foreach (string g in groupcode)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = Convert.ToString(g);
                                    c++;
                                }
                            }
                            string groupamt = Convert.ToString(ds.Tables[0].Rows[row]["GroupAmount"]);
                            decimal amt = 0;
                            double total = 0;
                            if (groupamt.Trim() != "")
                            {
                                char[] delimiterChars = { ',' };
                                string[] groupcode1 = groupamt.Split(delimiterChars);
                                foreach (string g1 in groupcode1)
                                {
                                    amt += Convert.ToDecimal(g1);
                                }
                            }
                            string addamt = Convert.ToString(ds.Tables[0].Rows[row]["Additional_Amount"]);
                            string bill = Convert.ToString(ds.Tables[0].Rows[row]["Fixed_Amount"]);
                            total = Convert.ToDouble(addamt) + Convert.ToDouble(bill) + Convert.ToDouble(amt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = Convert.ToString(total);
                            c++;
                            string opdays = Convert.ToString(ds.Tables[0].Rows[row]["BalanceAmt"]);
                            double clobal = 0;
                            clobal = Convert.ToDouble(opdays) - Convert.ToDouble(total);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = Convert.ToString(clobal);
                        }
                        FpSpread1.Visible = true;
                        rptprint.Visible = true;
                        div1.Visible = true;
                        lbl_error.Visible = false;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        pheaderfilter1.Visible = false;
                        pcolumnorder1.Visible = false;
                        pcolumnorder2.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                    }
                    else
                    {
                        div1.Visible = false;
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        pheaderfilter1.Visible = false;
                        pcolumnorder1.Visible = false;
                        pcolumnorder2.Visible = false;
                        pheaderfilterstu.Visible = false;
                        pcolumnorderstu.Visible = false;
                        pheaderfilterguest.Visible = false;
                        pcolumnorderguest.Visible = false;
                        lbl_error.Text = "No Records Found";
                        FpSpread2.Visible = false;
                        div2.Visible = false;
                    }
                }
                else
                {
                    div1.Visible = false;
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    pheaderfilter1.Visible = false;
                    pcolumnorder1.Visible = false;
                    pcolumnorder2.Visible = false;
                    pheaderfilterstu.Visible = false;
                    pcolumnorderstu.Visible = false;
                    pheaderfilterguest.Visible = false;
                    pcolumnorderguest.Visible = false;
                    lbl_error.Text = "Please Select All Field";
                    FpSpread2.Visible = false;
                    div2.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void rdb_guest_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            //if (rdb_fromat1.Checked == true && rdb_guest.Checked == true)
            //{
            //    pheaderfilter1.Visible = true;
            //    pcolumnorder1.Visible = true;
            //    rptprint.Visible = false;
            //    div1.Visible = false;
            //    FpSpread1.Visible = false;
            //    pheaderfilter.Visible = false;
            //    pcolumnorder.Visible = false;
            //    pheaderfilterstu.Visible = false;
            //    pcolumnorderstu.Visible = false;
            //    pheaderfilterguest.Visible = false;
            //    pcolumnorderguest.Visible = false;
            //}
            //if (rdb_fromat2.Checked == true && rdb_guest.Checked == true)
            //{
            //    pheaderfilter.Visible = false;
            //    pcolumnorder.Visible = false;
            //    rptprint.Visible = false;
            //    div1.Visible = false;
            //    FpSpread1.Visible = false;
            //    pheaderfilter1.Visible = false;
            //    pcolumnorder1.Visible = false;
            //    pheaderfilterstu.Visible = false;
            //    pcolumnorderstu.Visible = false;
            //    pheaderfilterguest.Visible = true;
            //    pcolumnorderguest.Visible = true;
            //}
            if (rdb_fromat2.Checked == true && rdb_indivual.Checked == true && rdb_guest.Checked == true)
            {
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                txt_rollno.Visible = false;
                lbl_rollno.Visible = false;
                lbl_guestname.Visible = true;
                txt_guest.Visible = true;
                // indtrue();
                indfalse();
            }
        }
        catch { }
    }
    protected void rdb_student_CheckedChange(object sender, EventArgs e)
    {
        //    if (rdb_fromat1.Checked == true && rdb_student.Checked == true)
        //    {
        //        pheaderfilter.Visible = true;
        //        pcolumnorder.Visible = true;
        //        rptprint.Visible = false;
        //        div1.Visible = false;
        //        FpSpread1.Visible = false;
        //        pheaderfilter1.Visible = false;
        //        pcolumnorder1.Visible = false;
        //        pheaderfilterstu.Visible = false;
        //        pcolumnorderstu.Visible = false;
        //        pheaderfilterguest.Visible = false;
        //        pcolumnorderguest.Visible = false;
        //    }
        //    if (rdb_fromat2.Checked == true && rdb_student.Checked == true)
        //    {
        //        pheaderfilter.Visible = false;
        //        pcolumnorder.Visible = false;s
        //        rptprint.Visible = false;
        //        div1.Visible = false;
        //        FpSpread1.Visible = false;
        //        pheaderfilter1.Visible = false;
        //        pcolumnorder1.Visible = false;
        //        pheaderfilterstu.Visible = true;
        //        pcolumnorderstu.Visible = true;
        //        pheaderfilterguest.Visible = false;
        //        pcolumnorderguest.Visible = false;
        //    }
        if (rdb_fromat2.Checked == true && rdb_indivual.Checked == true && rdb_student.Checked == true)
        {
            txt_rollno.Visible = true;
            lbl_rollno.Visible = true;
            lbl_college.Visible = false;
            txt_colg.Visible = false;
            lbl_hostelname.Visible = false;
            ddl_messname.Visible = false;
            lbl_building.Visible = false;
            txt_building.Visible = false;
            lbl_floor.Visible = false;
            txt_floor.Visible = false;
            lbl_batch.Visible = false;
            txt_batch.Visible = false;
            lbl_degree.Visible = false;
            txt_degree.Visible = false;
            lbl_dept.Visible = false;
            txt_dept.Visible = false;
            lbl_gender.Visible = false;
            txt_gender.Visible = false;
            panel_batch.Visible = false;
            panel_colg.Visible = false;
            panel_degree.Visible = false;
            panel_dept.Visible = false;
            // panel_hostelname.Visible = false;
            panel_building.Visible = false;
            panel_gender.Visible = false;
            panel_floor.Visible = false;
            lbl_pop1staffname.Visible = false;
            txt_pop1staffname.Visible = false;
            lbl_guestname.Visible = false;
            txt_guest.Visible = false;
            rdb_detailedwise.Checked = true;
            rdb_monthwise.Checked = false;
        }
        else
        {
            //  rdb_indivual.Checked = false;
            //  rdb_student.Checked = false;
            txt_rollno.Visible = false;
            lbl_rollno.Visible = false;
            lbl_college.Visible = true;
            txt_colg.Visible = true;
            lbl_hostelname.Visible = true;
            ddl_messname.Visible = true;
            lbl_building.Visible = true;
            txt_building.Visible = true;
            lbl_floor.Visible = true;
            txt_floor.Visible = true;
            lbl_batch.Visible = true;
            txt_batch.Visible = true;
            lbl_degree.Visible = true;
            txt_degree.Visible = true;
            lbl_dept.Visible = true;
            txt_dept.Visible = true;
            lbl_gender.Visible = true;
            txt_gender.Visible = true;
            panel_batch.Visible = true;
            panel_colg.Visible = true;
            panel_degree.Visible = true;
            panel_dept.Visible = true;
            // panel_hostelname.Visible = true;
            panel_building.Visible = true;
            panel_gender.Visible = true;
            panel_floor.Visible = true;
            rdb_detailedwise.Checked = false;
        }
    }
    protected void btn_Excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lbl_validation1.Visible = false;
            }
            else
            {
                lbl_validation1.Text = "Please Enter Your Report Name";
                lbl_validation1.Visible = true;
                txt_excelname.Focus();
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
            string degreedetails = "Monthly Messbill Report";
            string pagename = "HM_MonthlyMessBillReport.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    public string returnMonYear(string numeral)
    {
        string monthyear = String.Empty;
        switch (numeral)
        {
            case "1":
                monthyear = "Jan";
                break;
            case "2":
                monthyear = "Feb";
                break;
            case "3":
                monthyear = "Mar";
                break;
            case "4":
                monthyear = "Apr";
                break;
            case "5":
                monthyear = "May";
                break;
            case "6":
                monthyear = "Jun";
                break;
            case "7":
                monthyear = "Jul";
                break;
            case "8":
                monthyear = "Aug";
                break;
            case "9":
                monthyear = "Sep";
                break;
            case "10":
                monthyear = "Oct";
                break;
            case "11":
                monthyear = "Nov";
                break;
            case "12":
                monthyear = "Dec";
                break;
        }
        return monthyear;
    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            cbl_colg.Items.Clear();
            cb_colg.Checked = false;
            txt_colg.Text = "Select All";
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_colg.DataSource = ds;
                cbl_colg.DataTextField = "collname";
                cbl_colg.DataValueField = "college_code";
                cbl_colg.DataBind();
                if (cbl_colg.Items.Count > 0)
                {
                    for (i = 0; i < cbl_colg.Items.Count; i++)
                    {
                        cbl_colg.Items[i].Selected = true;
                    }
                    txt_colg.Text = "College(" + cbl_colg.Items.Count + ")";
                    cb_colg.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_colg_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_colg.Text = "--Select--";
            if (cb_colg.Checked == true)
            {
                for (i = 0; i < cbl_colg.Items.Count; i++)
                {
                    cbl_colg.Items[i].Selected = true;
                }
                txt_colg.Text = "College(" + (cbl_colg.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_colg.Items.Count; i++)
                {
                    cbl_colg.Items[i].Selected = false;
                }
            }
            bindBtch();
            binddeg();
            binddept();
        }
        catch { }
    }
    protected void cbl_colg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_colg.Checked = false;
            commcount = 0;
            txt_colg.Text = "--Select--";
            for (i = 0; i < cbl_colg.Items.Count; i++)
            {
                if (cbl_colg.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_colg.Items.Count)
                {
                    cb_colg.Checked = true;
                }
                txt_colg.Text = "College(" + commcount.ToString() + ")";
            }
            bindBtch();
            binddeg();
            binddept();
        }
        catch { }
    }
    //column order
    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
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
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
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
                //tborder.Text = tborder.Text + ItemList[i].ToString();
                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();
                }
                tborder.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    //cblcolumnorder.Items[0].Selected = true;
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

    //magesh 11.4.18
    public void CheckBox1_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox1_column.Checked == true)
            {
                tborder2.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder2.Items[i].Selected = true;
                    lnk_columnorder2.Visible = true;
                    ItemList.Add(cblcolumnorder2.Items[i].Text.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder2.Visible = true;
                tborder2.Visible = true;
                tborder2.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();
                }
                tborder2.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                {
                    cblcolumnorder2.Items[i].Selected = false;
                    lnk_columnorder2.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    //cblcolumnorder.Items[0].Selected = true;
                }
                tborder2.Text = "";
                tborder2.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void LinkButtonsremove1_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder2.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder2.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder2.Text = "";
            tborder2.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void LinkButtonsremove_Click(object sender, EventArgs e)
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
    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string si = "";
            int j = 0;
            if (cb_column.Checked == true)
            {
                ItemList1.Clear();
                for (i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    //if (rdb_cumulative.Checked == true)
                    //{
                    //    ItemList1.Remove("Date");                    
                    //    Itemindex.Remove(si == "Date");
                    //    ItemList1.Remove("Description");
                    //    Itemindex.Remove(si == "Description");
                    //}
                    //else
                    //{
                    si = Convert.ToString(i);
                    cblcolumnorder1.Items[i].Selected = true;
                    lnk_columnorder1.Visible = true;
                    ItemList1.Add(cblcolumnorder1.Items[i].Value.ToString());
                    Itemindex1.Add(si);
                    //}
                }
                lnk_columnorder1.Visible = true;
                tborder1.Visible = true;
                tborder1.Text = "";
                for (i = 0; i < ItemList1.Count; i++)
                {
                    j = j + 1;
                    tborder1.Text = tborder1.Text + ItemList1[i].ToString();
                    tborder1.Text = tborder1.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    cblcolumnorder1.Items[i].Selected = false;
                    lnk_columnorder1.Visible = false;
                    ItemList1.Clear();
                    Itemindex1.Clear();
                    cblcolumnorder1.Items[0].Enabled = false;
                }
                tborder1.Text = "";
                tborder1.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void lb_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder1.ClearSelection();
            cb_column.Checked = false;
            lnk_columnorder1.Visible = false;
            //cblcolumnorder1.Items[0].Selected = true;
            ItemList1.Clear();
            Itemindex1.Clear();
            tborder1.Text = "";
            tborder1.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            cb_column.Checked = false;
            cblcolumnorder1.Items[0].Selected = true;
            cblcolumnorder1.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cblcolumnorder1.Items[index].Selected)
            {
                if (!Itemindex1.Contains(sindex))
                {
                    //if (tborder1.Text == "")
                    //{
                    //    ItemList1.Add("Roll No");
                    //}
                    //ItemList1.Add("Admission No");
                    //ItemList1.Add("Name");                   
                    ItemList1.Add(cblcolumnorder1.Items[index].Value.ToString());
                    Itemindex1.Add(sindex);
                }
            }
            else
            {
                ItemList1.Remove(cblcolumnorder1.Items[index].Value.ToString());
                Itemindex1.Remove(sindex);
            }
            for (i = 0; i < cblcolumnorder1.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder1.Items[0].Selected = true;
                //    cblcolumnorder1.Items[1].Selected = true;
                //    cblcolumnorder1.Items[2].Selected = true;
                //}
                if (cblcolumnorder1.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList1.Remove(cblcolumnorder1.Items[i].Value.ToString());
                    Itemindex1.Remove(sindex);
                }
            }
            lnk_columnorder1.Visible = true;
            tborder1.Visible = true;
            tborder1.Text = "";
            for (i = 0; i < ItemList1.Count; i++)
            {
                tborder1.Text = tborder1.Text + ItemList1[i].ToString();
                tborder1.Text = tborder1.Text + "(" + (i + 1).ToString() + ")  ";
            }
            if (ItemList1.Count == 22)
            {
                cb_column.Checked = true;
            }
            if (ItemList1.Count == 0)
            {
                tborder1.Visible = false;
                lnk_columnorder1.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void rdb_fromat1_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_fromat1.Checked == true)
            {
                rdb_common.Enabled = false;
                rdb_indivual.Enabled = false;
                indtrue();
                rdb_indivual.Checked = false;
                rdb_common.Checked = false;
                rdb_student.Checked = true;
                rdb_staff.Checked = false;
                rdb_guest.Checked = false;
                //  rdb_common.Visible = true;
                //  rdb_indivual.Visible = true;
                rdb_staff.Visible = true;
                rdb_guest.Visible = true;
                rdb_detailedwise.Visible = false;
                rdb_monthwise.Visible = false;
                rdb_detailedwise.Checked = false;
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = false;
                rptprint.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
                txt_rollno.Text = "";
                lbl_rollno.Visible = false;
                txt_rollno.Visible = false;
                rdb_paid.Enabled = false;
                rdb_unpaid.Enabled = false;
                rdb_yettobepaid.Enabled = false;
            }
            else
            {
                //  rdb_common.Visible = true;
                //   rdb_indivual.Visible = true;
                rdb_common.Enabled = true;
                rdb_indivual.Enabled = true;
                rdb_staff.Visible = false;
                rdb_guest.Visible = false;
                //  rdb_detailedwise.Visible = true;
                //  rdb_monthwise.Visible = true;
                //  rdb_detailedwise.Checked = true;
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = false;
                rptprint.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
                lbl_rollno.Visible = true;
                txt_rollno.Visible = true;
                txt_rollno.Text = "";
                rdb_paid.Enabled = true;
                rdb_unpaid.Enabled = true;
                rdb_yettobepaid.Enabled = true;
            }
        }
        catch
        {
        }
    }
    protected void rdb_detailedwise_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_detailedwise.Checked == true)
            {
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = false;
                rptprint.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
                txt_rollno.Text = "";
            }
        }
        catch
        {
        }
    }
    protected void rdb_monthwise_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_monthwise.Checked == true)
            {
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = false;
                rptprint.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
                txt_rollno.Text = "";
            }
            else
            {
            }
        }
        catch
        {
        }
    }
    protected void rdb_fromat2_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_fromat2.Checked == true)
            {
                //  rdb_common.Visible = true;
                //    rdb_indivual.Visible = true;
                rdb_common.Checked = true;
                rdb_common.Enabled = true;
                rdb_indivual.Enabled = true;
                rdb_staff.Visible = false;
                rdb_guest.Visible = false;
                // rdb_detailedwise.Visible = true;
                //rdb_monthwise.Visible = true;
                //rdb_detailedwise.Checked = true;
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = false;
                rptprint.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
                txt_rollno.Text = "";
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                lbl_rollno.Visible = true;
                txt_rollno.Visible = true;
                rdb_unpaid.Checked = true;
                rdb_paid.Enabled = true;
                rdb_unpaid.Enabled = true;
                rdb_yettobepaid.Enabled = true;
            }
            else
            {
                rdb_common.Visible = false;
                rdb_indivual.Visible = false;
                rdb_common.Enabled = false;
                rdb_indivual.Enabled = false;
                rdb_staff.Visible = true;
                rdb_guest.Visible = true;
                rdb_detailedwise.Visible = false;
                rdb_monthwise.Visible = false;
                rdb_detailedwise.Checked = false;
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = false;
                rptprint.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
                txt_rollno.Text = "";
                lbl_rollno.Visible = false;
                txt_rollno.Visible = false;
                rdb_unpaid.Checked = false;
                rdb_paid.Enabled = false;
                rdb_unpaid.Enabled = false;
                rdb_yettobepaid.Enabled = false;
            }
        }
        catch
        {
        }
    }
    protected void rdb_common_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_fromat2.Checked == true && rdb_common.Checked == true)
            {
                indtrue();
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = false;
                rptprint.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
            }
            else
            {
                indfalse();
            }
        }
        catch
        {
        }
    }
    protected void rdb_indivual_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_fromat2.Checked == true && rdb_indivual.Checked == true && rdb_student.Checked == true)
            {
                txt_rollno.Visible = true;
                lbl_rollno.Visible = true;
                lbl_college.Visible = false;
                txt_colg.Visible = false;
                lbl_hostelname.Visible = false;
                ddl_messname.Visible = false;
                lbl_building.Visible = false;
                txt_building.Visible = false;
                lbl_floor.Visible = false;
                txt_floor.Visible = false;
                lbl_batch.Visible = false;
                txt_batch.Visible = false;
                lbl_degree.Visible = false;
                txt_degree.Visible = false;
                lbl_dept.Visible = false;
                txt_dept.Visible = false;
                lbl_gender.Visible = false;
                txt_gender.Visible = false;
                panel_batch.Visible = false;
                panel_colg.Visible = false;
                panel_degree.Visible = false;
                panel_dept.Visible = false;
                // panel_hostelname.Visible = false;
                panel_building.Visible = false;
                panel_gender.Visible = false;
                panel_floor.Visible = false;
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                lbl_guestname.Visible = false;
                txt_guest.Visible = false;
                FpSpread1.Visible = false;
                div1.Visible = false;
                lbl_error.Visible = false;
                rptprint.Visible = false;
                FpSpread2.Visible = false;
                div2.Visible = false;
                rdb_detailedwise.Checked = true;
                rdb_monthwise.Checked = false;
            }
            else if (rdb_fromat2.Checked == true && rdb_indivual.Checked == true && rdb_staff.Checked == true)
            {
                lbl_pop1staffname.Visible = true;
                txt_pop1staffname.Visible = true;
                txt_rollno.Visible = false;
                lbl_rollno.Visible = false;
                lbl_guestname.Visible = false;
                txt_guest.Visible = false;
                indfalse();
            }
            else if (rdb_fromat2.Checked == true && rdb_indivual.Checked == true && rdb_guest.Checked == true)
            {
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                txt_rollno.Visible = false;
                lbl_rollno.Visible = false;
                lbl_guestname.Visible = true;
                txt_guest.Visible = true;
                indfalse();
                // indtrue();
            }
            else
            {
                // rdb_indivual.Checked = false;
                //  rdb_student.Checked = false;
                txt_rollno.Visible = false;
                lbl_rollno.Visible = false;
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                lbl_guestname.Visible = false;
                txt_guest.Visible = false;
                lbl_college.Visible = true;
                txt_colg.Visible = true;
                lbl_hostelname.Visible = true;
                ddl_messname.Visible = true;
                lbl_building.Visible = true;
                txt_building.Visible = true;
                lbl_floor.Visible = true;
                txt_floor.Visible = true;
                lbl_batch.Visible = true;
                txt_batch.Visible = true;
                lbl_degree.Visible = true;
                txt_degree.Visible = true;
                lbl_dept.Visible = true;
                txt_dept.Visible = true;
                lbl_gender.Visible = true;
                txt_gender.Visible = true;
                panel_batch.Visible = true;
                panel_colg.Visible = true;
                panel_degree.Visible = true;
                panel_dept.Visible = true;
                //  panel_hostelname.Visible = true;
                panel_building.Visible = true;
                panel_gender.Visible = true;
                panel_floor.Visible = true;
                rdb_detailedwise.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void indtrue()
    {
        txt_rollno.Visible = false;
        lbl_rollno.Visible = false;
        lbl_pop1staffname.Visible = false;
        txt_pop1staffname.Visible = false;
        lbl_guestname.Visible = false;
        txt_guest.Visible = false;
        lbl_college.Visible = true;
        txt_colg.Visible = true;
        lbl_hostelname.Visible = true;
        ddl_messname.Visible = true;
        lbl_building.Visible = true;
        txt_building.Visible = true;
        lbl_floor.Visible = true;
        txt_floor.Visible = true;
        lbl_batch.Visible = true;
        txt_batch.Visible = true;
        lbl_degree.Visible = true;
        txt_degree.Visible = true;
        lbl_dept.Visible = true;
        txt_dept.Visible = true;
        lbl_gender.Visible = true;
        txt_gender.Visible = true;
        panel_batch.Visible = true;
        panel_colg.Visible = true;
        panel_degree.Visible = true;
        panel_dept.Visible = true;
        //   panel_hostelname.Visible = true;
        panel_building.Visible = true;
        panel_gender.Visible = true;
        panel_floor.Visible = true;
    }
    public void indfalse()
    {
        lbl_college.Visible = false;
        txt_colg.Visible = false;
        lbl_hostelname.Visible = false;
        ddl_messname.Visible = false;
        lbl_building.Visible = false;
        txt_building.Visible = false;
        lbl_floor.Visible = false;
        txt_floor.Visible = false;
        lbl_batch.Visible = false;
        txt_batch.Visible = false;
        lbl_degree.Visible = false;
        txt_degree.Visible = false;
        lbl_dept.Visible = false;
        txt_dept.Visible = false;
        lbl_gender.Visible = false;
        txt_gender.Visible = false;
        panel_batch.Visible = false;
        panel_colg.Visible = false;
        panel_degree.Visible = false;
        panel_dept.Visible = false;
        // panel_hostelname.Visible = false;
        panel_building.Visible = false;
        panel_gender.Visible = false;
        panel_floor.Visible = false;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top 10 r.Roll_No+'-'+a.stud_name+'-'+c.Course_Name+'-'+dt.Dept_Name from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and r.Roll_No like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top 10 s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code, s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetguestName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct Guest_Name+'-'+From_Company+'-'+CONVERT(varchar(10), GuestCode) from Hostel_GuestReg  where Guest_Name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    protected void rdb_staff_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_fromat2.Checked == true && rdb_indivual.Checked == true && rdb_staff.Checked == true)
            {
                lbl_pop1staffname.Visible = true;
                txt_pop1staffname.Visible = true;
                txt_rollno.Visible = false;
                lbl_rollno.Visible = false;
                // indtrue();
                indfalse();
                lbl_guestname.Visible = false;
                txt_guest.Visible = false;
            }
            else
            {
                indtrue();
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
            }
            if (rdb_staff.Checked == true)
            {
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                rptprint.Visible = false;
                div1.Visible = false;
                FpSpread1.Visible = false;
                pheaderfilter1.Visible = true;
                pcolumnorder1.Visible = false;
                pcolumnorder2.Visible = false;
                pheaderfilterstu.Visible = false;
                pcolumnorderstu.Visible = false;
                pheaderfilterguest.Visible = false;
                pcolumnorderguest.Visible = false;
            }
        }
        catch
        {
        }
    }
    public void cb_stu_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string si = "";
            int j = 0;
            if (cb_stu.Checked == true)
            {
                ItemListstu.Clear();
                for (i = 0; i < cbl_stu.Items.Count; i++)
                {
                    //if (rdb_cumulative.Checked == true)
                    //{
                    //    ItemListstu.Remove("Date");                    
                    //    Itemindex.Remove(si == "Date");
                    //    ItemListstu.Remove("Description");
                    //    Itemindex.Remove(si == "Description");
                    //}
                    //else
                    //{
                    si = Convert.ToString(i);
                    cbl_stu.Items[i].Selected = true;
                    lnk_stu.Visible = true;
                    ItemListstu.Add(cbl_stu.Items[i].Value.ToString());
                    Itemindexstu.Add(si);
                    //}
                }
                lnk_stu.Visible = true;
                txt_stu.Visible = true;
                txt_stu.Text = "";
                for (i = 0; i < ItemListstu.Count; i++)
                {
                    j = j + 1;
                    txt_stu.Text = txt_stu.Text + ItemListstu[i].ToString();
                    txt_stu.Text = txt_stu.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (i = 0; i < cbl_stu.Items.Count; i++)
                {
                    cbl_stu.Items[i].Selected = false;
                    lnk_stu.Visible = false;
                    ItemListstu.Clear();
                    Itemindexstu.Clear();
                    cbl_stu.Items[0].Enabled = false;
                }
                txt_stu.Text = "";
                txt_stu.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_stu_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            cb_stu.Checked = false;
            cbl_stu.Items[0].Selected = true;
            cbl_stu.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cbl_stu.Items[index].Selected)
            {
                if (!Itemindexstu.Contains(sindex))
                {
                    //if (txt_stu.Text == "")
                    //{
                    //    ItemListstu.Add("Roll No");
                    //}
                    //ItemListstu.Add("Admission No");
                    //ItemListstu.Add("Name");                   
                    ItemListstu.Add(cbl_stu.Items[index].Value.ToString());
                    Itemindexstu.Add(sindex);
                }
            }
            else
            {
                ItemListstu.Remove(cbl_stu.Items[index].Value.ToString());
                Itemindexstu.Remove(sindex);
            }
            for (i = 0; i < cbl_stu.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cbl_stu.Items[0].Selected = true;
                //    cbl_stu.Items[1].Selected = true;
                //    cbl_stu.Items[2].Selected = true;
                //}
                if (cbl_stu.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemListstu.Remove(cbl_stu.Items[i].Value.ToString());
                    Itemindexstu.Remove(sindex);
                }
            }
            lnk_stu.Visible = true;
            txt_stu.Visible = true;
            txt_stu.Text = "";
            for (i = 0; i < ItemListstu.Count; i++)
            {
                txt_stu.Text = txt_stu.Text + ItemListstu[i].ToString();
                txt_stu.Text = txt_stu.Text + "(" + (i + 1).ToString() + ")  ";
            }
            if (ItemListstu.Count == 22)
            {
                cb_stu.Checked = true;
            }
            if (ItemListstu.Count == 0)
            {
                txt_stu.Visible = false;
                lnk_stu.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void lnk_stu_Click(object sender, EventArgs e)
    {
        try
        {
            cbl_stu.ClearSelection();
            cb_stu.Checked = false;
            lnk_stu.Visible = false;
            //cblcolumnorder1.Items[0].Selected = true;
            ItemListstu.Clear();
            Itemindexstu.Clear();
            txt_stu.Text = "";
            txt_stu.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    //
    public void cb_guest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string si = "";
            int j = 0;
            if (cb_guest.Checked == true)
            {
                ItemListstu.Clear();
                for (i = 0; i < cbl_guest.Items.Count; i++)
                {
                    //if (rdb_cumulative.Checked == true)
                    //{
                    //    ItemListstu.Remove("Date");                    
                    //    Itemindex.Remove(si == "Date");
                    //    ItemListstu.Remove("Description");
                    //    Itemindex.Remove(si == "Description");
                    //}
                    //else
                    //{
                    si = Convert.ToString(i);
                    cbl_guest.Items[i].Selected = true;
                    lnk_guest.Visible = true;
                    ItemListstu.Add(cbl_guest.Items[i].Value.ToString());
                    Itemindexstu.Add(si);
                    //}
                }
                lnk_guest.Visible = true;
                txt_guestcol.Visible = true;
                txt_guestcol.Text = "";
                for (i = 0; i < ItemListstu.Count; i++)
                {
                    j = j + 1;
                    txt_guestcol.Text = txt_guestcol.Text + ItemListstu[i].ToString();
                    txt_guestcol.Text = txt_guestcol.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (i = 0; i < cbl_guest.Items.Count; i++)
                {
                    cbl_guest.Items[i].Selected = false;
                    lnk_guest.Visible = false;
                    ItemListstu.Clear();
                    Itemindexstu.Clear();
                    cbl_guest.Items[0].Enabled = false;
                }
                txt_guestcol.Text = "";
                txt_guestcol.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_guest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            cb_guest.Checked = false;
            cbl_guest.Items[0].Selected = true;
            cbl_guest.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cbl_guest.Items[index].Selected)
            {
                if (!Itemindexstu.Contains(sindex))
                {
                    //if (txt_guestcol.Text == "")
                    //{
                    //    ItemListstu.Add("Roll No");
                    //}
                    //ItemListstu.Add("Admission No");
                    //ItemListstu.Add("Name");                   
                    ItemListstu.Add(cbl_guest.Items[index].Value.ToString());
                    Itemindexstu.Add(sindex);
                }
            }
            else
            {
                ItemListstu.Remove(cbl_guest.Items[index].Value.ToString());
                Itemindexstu.Remove(sindex);
            }
            for (i = 0; i < cbl_guest.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cbl_guest.Items[0].Selected = true;
                //    cbl_guest.Items[1].Selected = true;
                //    cbl_guest.Items[2].Selected = true;
                //}
                if (cbl_guest.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemListstu.Remove(cbl_guest.Items[i].Value.ToString());
                    Itemindexstu.Remove(sindex);
                }
            }
            lnk_guest.Visible = true;
            txt_guestcol.Visible = true;
            txt_guestcol.Text = "";
            for (i = 0; i < ItemListstu.Count; i++)
            {
                txt_guestcol.Text = txt_guestcol.Text + ItemListstu[i].ToString();
                txt_guestcol.Text = txt_guestcol.Text + "(" + (i + 1).ToString() + ")  ";
            }
            if (ItemListstu.Count == 22)
            {
                cb_guest.Checked = true;
            }
            if (ItemListstu.Count == 0)
            {
                txt_guestcol.Visible = false;
                lnk_guest.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void lnk_guest_Click(object sender, EventArgs e)
    {
        try
        {
            cbl_guest.ClearSelection();
            cb_guest.Checked = false;
            lnk_guest.Visible = false;
            //cblcolumnorder1.Items[0].Selected = true;
            ItemListstu.Clear();
            Itemindexstu.Clear();
            txt_guestcol.Text = "";
            txt_guestcol.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    //public void bindmonyear()
    //{
    //    try
    //    {
    //        college = "";
    //        batch = "";
    //        degree = "";
    //        dept = "";
    //        cbl_month.Items.Clear();
    //        cb_month.Checked = false;
    //        txt_month.Text = "---Select---";
    //        cbl_year.Items.Clear();
    //        cb_year.Checked = false;
    //        txt_year.Text = "---Select---";
    //            //college = Convert.ToString(ddl_college.SelectedValue);
    //            //batch = Convert.ToString(ddl_batch.SelectedValue);
    //            //degree = Convert.ToString(ddl_degree.SelectedValue);
    //        if (cbl_batch.Items.Count > 0)
    //        {
    //            for (i = 0; i < cbl_batch.Items.Count; i++)
    //            {
    //                if (cbl_batch.Items[i].Selected == true)
    //                {
    //                    if (batch == "")
    //                    {
    //                        batch= Convert.ToString(cbl_batch.Items[i].Value);
    //                    }
    //                    else
    //                    {
    //                        batch= batch + "','" + Convert.ToString(cbl_batch.Items[i].Value);
    //                    }
    //                }
    //            }
    //        }
    //            if (cbl_dept.Items.Count > 0)
    //            {
    //                for (i = 0; i < cbl_dept.Items.Count; i++)
    //                {
    //                    if (cbl_dept.Items[i].Selected == true)
    //                    {
    //                        if (dept == "")
    //                        {
    //                            dept = Convert.ToString(cbl_dept.Items[i].Value);
    //                        }
    //                        else
    //                        {
    //                            dept = dept + "','" + Convert.ToString(cbl_dept.Items[i].Value);
    //                        }
    //                    }
    //                }
    //            }
    //            selectQuery = "select distinct Exam_Month , Exam_year  from Exam_Details where batch_year in ('" + batch + "') and degree_code in ('" + dept + "') order by Exam_year asc";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(selectQuery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                exammonth = "";
    //                examyear = "";
    //                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                {
    //                    cbl_month.DataSource = ds;
    //                    cbl_month.DataTextField = "Exam_Month";
    //                    cbl_month.DataValueField = "Exam_Month";
    //                    cbl_month.DataBind();
    //                    cbl_year.DataSource = ds;
    //                    cbl_year.DataTextField = "Exam_year";
    //                    cbl_year.DataValueField = "Exam_year";
    //                    cbl_year.DataBind();
    //                }
    //                if (cbl_month.Items.Count > 0)
    //                {
    //                    for (i = 0; i < cbl_month.Items.Count; i++)
    //                    {
    //                        cbl_month.Items[i].Selected = true;
    //                    }
    //                    txt_month.Text = "Month(" + cbl_month.Items.Count + ")";
    //                    cb_month.Checked = true;
    //                }
    //                if (cbl_year.Items.Count > 0)
    //                {
    //                    for (i = 0; i < cbl_year.Items.Count; i++)
    //                    {
    //                        cbl_year.Items[i].Selected = true;
    //                    }
    //                    txt_year.Text = "Year(" + cbl_year.Items.Count + ")";
    //                    cb_year.Checked = true;
    //                }
    //            }
    //    }
    //    catch
    //    {
    //    }
    //} 
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch
        {
        }
    }
    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            for (i = 0; i < cbl_building.Items.Count; i++)
            {
                if (cbl_building.Items[i].Selected)
                {
                    if (building == "")
                    {
                        building = "" + cbl_building.Items[i].Text.ToString();
                    }
                    else
                    {
                        building += "','" + cbl_building.Items[i].Text.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_floor.Items[i].Text.ToString();
                    }
                    else
                    {
                        floor += "','" + cbl_floor.Items[i].Text.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_colg.Items.Count; i++)
            {
                if (cbl_colg.Items[i].Selected)
                {
                    if (colg == "")
                    {
                        colg = "" + cbl_colg.Items[i].Value.ToString();
                    }
                    else
                    {
                        colg += "','" + cbl_colg.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected)
                {
                    if (batch == "")
                    {
                        batch = "" + cbl_batch.Items[i].Text.ToString();
                    }
                    else
                    {
                        batch += "','" + cbl_batch.Items[i].Text.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected)
                {
                    if (degree == "")
                    {
                        degree = "" + cbl_degree.Items[i].Value.ToString();
                    }
                    else
                    {
                        degree += "','" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected)
                {
                    if (dept == "")
                    {
                        dept = "" + cbl_dept.Items[i].Value.ToString();
                    }
                    else
                    {
                        dept += "','" + cbl_dept.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_gender.Items.Count; i++)
            {
                if (cbl_gender.Items[i].Selected)
                {
                    if (gender == "")
                    {
                        gender = "" + cbl_gender.Items[i].Value.ToString();
                    }
                    else
                    {
                        gender += "','" + cbl_gender.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_month.Items.Count; i++)
            {
                if (cbl_month.Items[i].Selected)
                {
                    if (month == "")
                    {
                        month = "" + cbl_month.Items[i].Value.ToString();
                    }
                    else
                    {
                        month += "','" + cbl_month.Items[i].Value.ToString() + "";
                    }
                }
            }
            hostel = ddl_messname.SelectedItem.Value;
            string hostelcod = d2.Gethostelcode_inv(hostel);
            for (i = 0; i < cbl_year.Items.Count; i++)
            {
                if (cbl_year.Items[i].Selected)
                {
                    if (year == "")
                    {
                        year = "" + cbl_year.Items[i].Text.ToString();
                    }
                    else
                    {
                        year += "','" + cbl_year.Items[i].Text.ToString() + "";
                    }
                }
            }
            string activerow = "";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string yr = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
            // string mnth = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Value);
            string rnumb = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
            string mnth = d2.GetFunction("select billmonth from MessBill_Master where bill_year='" + yr + "'");
            try
            {
                string selectquery = "";
                if (rnumb.Trim() != "")
                {
                    //string rnum = "";
                    //rnum = Convert.ToString(txt_rollno.Text);
                    //string[] splitrnum = rnum.Split('-');
                    //string rno = "";
                    //if (splitrnum.Length > 0)
                    //{
                    //    rno = Convert.ToString(splitrnum[0] + splitrnum[1] + splitrnum[2] + splitrnum[3]);
                    //}
                    selectquery = "SELECT distinct d.Roll_No,Stud_Name,Course_Name+'-'+Dept_Acronym Degree,BalanceAmt,Fixed_Amount,Additional_Amount,GroupCode,GroupAmount,d.Rebete_days ,Charges  FROM MessBill_Master M,MessBill_Detail D,registration r,ft_excessdet t,Degree g,course c,Department p WHERE M.messbillmasterid = D.MessBill_MasterCode and d.roll_no = r.Roll_No and r.app_no = t.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.Dept_Code = p.Dept_Code and billmonth in ('" + mnth + "') and bill_year in ('" + yr + "')  and d.Roll_No ='" + rnumb + "'";
                }
                else
                {
                    selectquery = "SELECT distinct d.Roll_No,r.Stud_Name,Course_Name+'-'+Dept_Acronym Degree,BalanceAmt,Fixed_Amount,Additional_Amount,GroupCode,GroupAmount,d.Rebete_days ,Charges  FROM MessBill_Master M,MessBill_Detail D,registration r,ft_excessdet t,Degree g,course c,Department p,Hostel_StudentDetails hs,applyn a WHERE a.app_no =r.App_No and  hs.Roll_No =r.Roll_No and M.messbillmasterid = D.MessBill_MasterCode and d.roll_no = r.Roll_No and r.app_no = t.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.Dept_Code = p.Dept_Code and billmonth in ('" + mnth + "') and bill_year in ('" + yr + "')  and m.Hostel_Code in ('" + hostelcod + "') ";
                    if (dept.Trim() != "")
                    {
                        selectquery = selectquery + " and r.degree_code in ('" + dept + "') ";
                    }
                    if (batch.Trim() != "")
                    {
                        selectquery = selectquery + " and r.Batch_Year in ('" + batch + "') ";
                    }
                    if (building.Trim() != "")
                    {
                        selectquery = selectquery + " and hs.Building_Name in ('" + building + "') ";
                    }
                    if (floor.Trim() != "")
                    {
                        selectquery = selectquery + " and hs.Floor_Name in ('" + floor + "') ";
                    }
                    if (gender.Trim() != "")
                    {
                        selectquery = selectquery + " and a.sex in ('" + gender + "') ";
                    }
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread2.Sheets[0].RowCount = 0;
                    FpSpread2.Sheets[0].ColumnCount = 0;
                    FpSpread2.CommandBar.Visible = false;
                    FpSpread2.Sheets[0].AutoPostBack = true;
                    FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    FpSpread2.Sheets[0].ColumnCount = 9;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Columns[0].Width = 50;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Name";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Columns[1].Width = 100;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Columns[2].Width = 200;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Opening Dues";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Days";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Charges";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Bill";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Additional Amount";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    string query = "";
                    query = d2.GetFunction("select distinct GroupCode  from MessBill_Master where Bill_Year in ('" + year + "') and BillMonth in ('" + month + "') and Hostel_Code in ('" + hostelcod + "')");
                    //string groupcode1 = "";
                    //char[] delimiterChars = { ',' };
                    //string[] groupcode = query.Split(delimiterChars);
                    //foreach (string g in groupcode)
                    //{
                    string query1 = "";
                    if (query.Trim() != "")
                    {
                        query1 = "select TextVal,TextCode  from TextValTable where TextCode in (" + query + ")";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(query1, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                            {
                                FpSpread2.Sheets[0].ColumnCount++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds1.Tables[0].Rows[row]["TextVal"]);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds1.Tables[0].Rows[row]["TextCode"]);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Total";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Closing Balance";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Degree"]);
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["rpu"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Stock_Value"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["BalanceAmt"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Rebete_days"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Charges"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        string rebatedays = Convert.ToString(ds.Tables[0].Rows[row]["Rebete_days"]);
                        string charges = Convert.ToString(ds.Tables[0].Rows[row]["Charges"]);
                        double charge = 0;
                        if (charges.Trim() != "")
                        {
                            charge = Convert.ToDouble(charges);
                        }
                        else
                        {
                            charges = "0";
                            charge = Convert.ToDouble(charges);
                        }
                        double bills = 0;
                        if (charges != null)
                        {
                            bills = Convert.ToDouble(rebatedays) * Convert.ToDouble(charge);
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(bills);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["Additional_Amount"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                        int c = 9;
                        string getvalue = Convert.ToString(ds.Tables[0].Rows[row]["GroupAmount"]);
                        if (getvalue.Trim() != "")
                        {
                            char[] delimiterChars = { ',' };
                            string[] groupcode = getvalue.Split(delimiterChars);
                            foreach (string g in groupcode)
                            {
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, c].Text = Convert.ToString(g);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                c++;
                            }
                        }
                        string groupamt = Convert.ToString(ds.Tables[0].Rows[row]["GroupAmount"]);
                        decimal amt = 0;
                        double total = 0;
                        if (groupamt.Trim() != "")
                        {
                            char[] delimiterChars = { ',' };
                            string[] groupcode1 = groupamt.Split(delimiterChars);
                            foreach (string g1 in groupcode1)
                            {
                                amt += Convert.ToDecimal(g1);
                            }
                        }
                        string addamt = Convert.ToString(ds.Tables[0].Rows[row]["Additional_Amount"]);
                        string bill = Convert.ToString(ds.Tables[0].Rows[row]["Fixed_Amount"]);
                        total = Convert.ToDouble(addamt) + Convert.ToDouble(bill);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, c].Text = Convert.ToString(total);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                        c++;
                        string opdays = Convert.ToString(ds.Tables[0].Rows[row]["BalanceAmt"]);
                        double clobal = 0;
                        clobal = Convert.ToDouble(opdays) - Convert.ToDouble(total);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, c].Text = Convert.ToString(clobal);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                    }
                    FpSpread2.Visible = true;
                    rptprint.Visible = true;
                    div2.Visible = true;
                    lbl_error.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    pheaderfilter1.Visible = false;
                    pcolumnorder1.Visible = false;
                    pheaderfilterstu.Visible = false;
                    pcolumnorderstu.Visible = false;
                    pheaderfilterguest.Visible = false;
                    pcolumnorderguest.Visible = false;
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                }
            }
            catch
            {
            }
        }
    }
    protected void rdb_unpaid_CheckedChange(object sender, EventArgs e)
    {
        div1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        lbl_error.Visible = false;
    }
    protected void rdb_paid_CheckedChange(object sender, EventArgs e)
    {
        div1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        lbl_error.Visible = false;
    }
    protected void rdb_yettobepaid_CheckedChange(object sender, EventArgs e)
    {
        div1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        lbl_error.Visible = false;
    }
    public void cblcolumnorder2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            CheckBox1_column.Checked = false;
            cblcolumnorder2.Items[0].Selected = true;
            cblcolumnorder2.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cblcolumnorder2.Items[index].Selected)
            {
                if (!Itemindex1.Contains(sindex))
                {
                    //if (tborder1.Text == "")
                    //{
                    //    ItemList1.Add("Roll No");
                    //}
                    //ItemList1.Add("Admission No");
                    //ItemList1.Add("Name");                   
                    ItemList1.Add(cblcolumnorder2.Items[index].Value.ToString());
                    Itemindex1.Add(sindex);
                }
            }
            else
            {
                ItemList1.Remove(cblcolumnorder2.Items[index].Value.ToString());
                Itemindex1.Remove(sindex);
            }
            for (i = 0; i < cblcolumnorder2.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder1.Items[0].Selected = true;
                //    cblcolumnorder1.Items[1].Selected = true;
                //    cblcolumnorder1.Items[2].Selected = true;
                //}
                if (cblcolumnorder2.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList1.Remove(cblcolumnorder2.Items[i].Value.ToString());
                    Itemindex1.Remove(sindex);
                }
            }
            lnk_columnorder2.Visible = true;
            tborder2.Visible = true;
            tborder2.Text = "";
            for (i = 0; i < ItemList1.Count; i++)
            {
                tborder2.Text = tborder2.Text + ItemList1[i].ToString();
                tborder2.Text = tborder2.Text + "(" + (i + 1).ToString() + ")  ";
            }
            if (ItemList1.Count == 22)
            {
                CheckBox1_column.Checked = true;
            }
            if (ItemList1.Count == 0)
            {
                tborder2.Visible = false;
                lnk_columnorder2.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_hostelname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_building.Text = "--Select--";
            txt_floor.Text = "--Select--";
            if (cb_hostelname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                if (cb_hostelname.Checked == true)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        if (cb_hostelname.Checked == true)
                        {
                            cbl_hostelname.Items[i].Selected = true;
                            txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                            build1 = cbl_hostelname.Items[i].Value.ToString();
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
                    Hostelcode = buildvalue1;
                   // clgbuild(buildvalue1);
                    bindbuild();
                    bindmessmaster();
                }
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                    txt_hostelname.Text = "--Select--";
                    cbl_building.ClearSelection();
                    cbl_floor.ClearSelection();
                    cb_building.Checked = false;
                    cb_floor.Checked = false;
                }
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        cb_hostelname.Checked = false;
        int commcount = 0;
        string buildvalue = "";
        string build = "";
        txt_hostelname.Text = "--Select--";
        for (i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hostelname.Checked = false;
                ///new 22/08/15
                build = cbl_hostelname.Items[i].Value.ToString();
                if (buildvalue == "")
                {
                    buildvalue = build;
                }
                else
                {
                    buildvalue = buildvalue + "'" + "," + "'" + build;
                }
                bindbuild();
                bindmessmaster();
                Hostelcode = buildvalue;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_hostelname.Items.Count)
            {
                cb_hostelname.Checked = true;
            }
            txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
        }
    }

    protected void bindhostel()
    {
        try
        {
            cbl_hostelname.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            //magesh 21.6.18
            MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster  order by hostelname";
            ds = d2.select_method_wo_parameter(MessmasterFK, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
               // mm = cbl_hostelname.SelectedValue;
            }
            else
            {
                // cbl_hostelname.Items.Insert(0, "--Select--");
                txt_hostelname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }


    public void bindmessmaster()
    {
        try
        {

            string typ1 = string.Empty;
            if (cbl_hostelname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count ; i++)
                {
                    if (cbl_hostelname.Items[i].Selected==true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cbl_hostelname.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cbl_hostelname.Items[i].Value + "";
                        }
                    }
                   
                }
            }

            string selectQuery = d2.GetFunction("select MessMasterFK1 from HM_HostelMaster where HostelMasterPK in('" + typ1 + "')");
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

            ds = d2.select_method_wo_parameter(selectQuery, "text");
            // ddl_messmaster.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                //magesh 20.6.18
                ddl_messname.DataSource = ds;
                ddl_messname.DataTextField = "MessName";
                ddl_messname.DataValueField = "MessMasterPK";
                ddl_messname.DataBind();
            }
            else
            {
                //ddlmess.Items.Insert(0, "");
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


}
