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
using System.Configuration;

public partial class Gatepass_CodeSettings : System.Web.UI.Page
{
    #region Field Declaration

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt;
    int row;
    int i;
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string[] split;
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    bool fromDropDown = false;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("ReqM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Request"].Trim();
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

            if (!Request.FilePath.Contains("RequestHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/RequestMOD/RequestHome.aspx");
                    return;
                }
            }

            //****************************************************//

            txt_frmdate.Attributes.Add("readonly", "readonly");
            //  calfrmdate.EndDate = DateTime.Now;
            //calfrmdate.StartDate = DateTime.Now;
            //calfromdate.EndDate = DateTime.Now;
            if (!IsPostBack)
            {
                txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                bindcollege();
                txt_frmdate.Attributes.Add("readonly", "readonly");
                BindGridview();
                loadOldSetting();
                loadOldSetting1();
                // bindLibrary();
            }

        }
        catch
        { }
    }

    public void loadOldSetting()
    {
        try
        {
            dt = new DateTime();
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }


            string selectPrevDate = "select distinct CONVERT(varchar(10), FromDate,103) as date from gatepass_no where College_Code='" + clgcode + "' order by date desc";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(selectPrevDate, "Text");
            //ddl_PrevDate.Items.Clear();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_PrevDate.DataSource = ds1;
                ddl_PrevDate.DataTextField = "date";
                ddl_PrevDate.DataBind();
            }

            string selectquery = "select top 1 * from gatepass_no where College_Code='" + clgcode + "' order by FromDate desc";

            if (fromDropDown)
            {
                if (ddl_PrevDate.Items.Count > 0)
                {
                    split = ddl_PrevDate.SelectedItem.Text.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                    selectquery = "select top 1 * from gatepass_no where College_Code='" + clgcode + "' and FromDate='" + dt.ToString("MM/dd/yyyy") + "' order by FromDate desc";
                }
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < grid_prev.Rows.Count; i++)
                {
                    string val = hi.Value;
                    string val1 = hi1.Value;
                    string val2 = hi2.Value;

                    val = ds.Tables[0].Rows[0]["gatepassAcr"].ToString();
                    val1 = ds.Tables[0].Rows[0]["gatepassStNo"].ToString();
                    val2 = ds.Tables[0].Rows[0]["gatepassSize"].ToString();

                    TextBox acr = (TextBox)grid_prev.Rows[i].FindControl("txt_acronym1");
                    TextBox start = (TextBox)grid_prev.Rows[i].FindControl("txt_startno1");
                    TextBox size = (TextBox)grid_prev.Rows[i].FindControl("txt_size1");

                    acr.Text = val;
                    start.Text = val1;
                    size.Text = val2;
                }
            }
            else
            {
                BindGridview();
            }
        }
        catch { }
    }

    public void loadOldSetting1()
    {
        try
        {
            dt = new DateTime();
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string m = ddl_PrevDate.SelectedItem.Text;

            //string selectPrevDate = "select distinct CONVERT(varchar(10), FromDate,103) as date from gatepass_no where College_Code='" + clgcode + "' order by date desc";
            //ds1.Clear();
            //ds1 = d2.select_method_wo_parameter(selectPrevDate, "Text");
            ////ddl_PrevDate.Items.Clear();
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
            //    ddl_PrevDate.DataSource = ds1;
            //    ddl_PrevDate.DataTextField = "date";
            //    ddl_PrevDate.DataBind();
            //}

            string selectquery = "select top 1 * from gatepass_no where College_Code='" + clgcode + "' order by FromDate desc";

            if (fromDropDown)
            {
                if (ddl_PrevDate.SelectedItem.Text != "")
                {
                    split = ddl_PrevDate.SelectedItem.Text.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                    selectquery = "select top 1 * from gatepass_no where College_Code='" + clgcode + "' and FromDate='" + dt.ToString("MM/dd/yyyy") + "' order by FromDate desc";
                }
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < grid_prev.Rows.Count; i++)
                {
                    string val = hi.Value;
                    string val1 = hi1.Value;
                    string val2 = hi2.Value;

                    val = ds.Tables[0].Rows[0]["gatepassAcr"].ToString();
                    val1 = ds.Tables[0].Rows[0]["gatepassStNo"].ToString();
                    val2 = ds.Tables[0].Rows[0]["gatepassSize"].ToString();

                    TextBox acr = (TextBox)grid_prev.Rows[i].FindControl("txt_acronym1");
                    TextBox start = (TextBox)grid_prev.Rows[i].FindControl("txt_startno1");
                    TextBox size = (TextBox)grid_prev.Rows[i].FindControl("txt_size1");

                    acr.Text = val;
                    start.Text = val1;
                    size.Text = val2;
                }
            }
            else
            {
                BindGridview();
            }
        }
        catch { }
    }

    //protected void bindLibrary()
    //{
    //    try
    //    {
    //       // ddllibrary.Items.Clear();
    //        ds.Clear();
    //        string College = ddl_collegename.SelectedValue.ToString();
    //        string SelectQ = string.Empty;
    //        if (!string.IsNullOrEmpty(College))
    //        {
    //            dicQueryParameter.Clear();
    //            dicQueryParameter.Add("CollegeCode", Convert.ToString(College));
    //            ds = storeAcc.selectDataSet("[GetLibrary]", dicQueryParameter);
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                ddllibrary.DataSource = ds;
    //                ddllibrary.DataTextField = "lib_name";
    //                ddllibrary.DataValueField = "lib_code";
    //                ddllibrary.DataBind();

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //d2.sendErrorMail(ex, College, "Library_Card_Master");
    //    }
    //}
    public void BindGridview()
    {
        ArrayList addnew = new ArrayList();
        addnew.Add("Item Header Code");

        ug_grid.Visible = true;
        grid_prev.Visible = true;

        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummay5");
        DataRow dr;
        for (row = 0; row < addnew.Count; row++)
        {
            dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = Convert.ToString(addnew[row]);
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            ug_grid.DataSource = dt;
            ug_grid.DataBind();
            grid_prev.DataSource = dt;
            grid_prev.DataBind();

        }
        txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_prvdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    public void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string clgcode = "";
            string library = "";

            if (ddl_collegename.Items.Count > 0)

                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);

            //if (ddllibrary.Items.Count > 0)
            //    library = Convert.ToString(ddllibrary.SelectedValue);
            string firstdate = Convert.ToString(txt_frmdate.Text);
            dt = new DateTime();
            split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
            DateTime date = dt.Date;
            DateTime currdate = DateTime.Now.Date;
            string currtime = DateTime.Now.ToLongTimeString();
            //TextBox acr = (TextBox)ug_grid.Rows[0].FindControl("txt_acronym");
            //TextBox start = (TextBox)ug_grid.Rows[0].FindControl("txt_startno");
            //TextBox size = (TextBox)ug_grid.Rows[0].FindControl("txt_size");

            ////acr.Text = val;
            ////start.Text = val1;
            ////size.Text = val2;


            //string getval =  acr.Text ;
            //string getval1 = start.Text ;
            //string getval2 = size.Text;



            string getval = hid.Value;
            string getval1 = hid1.Value;
            string getval2 = hid2.Value;

            //string insertquery = "INSERT INTO gatepass_no(FromDate,FromTime,gatepassAcr,gatepassStNo,gatepassSize,Rcpt_LastNo,LatestRec,College_Code) VALUES ( '" + dt.ToString("MM/dd/yyyy") + "','" + currtime + "','" + dt.ToString("MM/dd/yyyy") + "','" + getval + "','" + getval1 + "','" + getval2 + "','" + getval1 + "',1,'" + clgcode + "' )";
            string insertquery = "if exists(select*from gatepass_no where FromDate='" + dt.ToString("MM/dd/yyyy") + "' and college_code='" + clgcode + "')update gatepass_no set FromTime='" + currtime + "' ,gatepassAcr ='" + getval + "',gatepassStNo='" + getval1 + "',gatepassSize='" + getval2 + "' where College_Code='" + clgcode + "' and FromDate='" + dt.ToString("MM/dd/yyyy") + "'else  INSERT INTO gatepass_no(FromDate,FromTime,gatepassAcr,gatepassStNo,gatepassSize,College_Code) VALUES ( '" + dt.ToString("MM/dd/yyyy") + "','" + currtime + "','" + getval + "','" + getval1 + "','" + getval2 + "','" + clgcode + "' )";
            int inst = d2.update_method_wo_parameter(insertquery, "Text");

            if (inst != 0)
            {
                loadOldSetting();
                imgdiv2.Visible = true;
                lbl_alerterr.Visible = true;
                lbl_alerterr.Text = "Code Settings Saved Sucessfully";

            }




        }
        catch { }

    }
    public void grid_prev_Bound(object sender, GridViewRowEventArgs e)
    {


    }
    public void grid_prev_Bound0(object sender, GridViewRowEventArgs e)
    {

    }
    public void OnDataBound(object sender, EventArgs e)
    {


    }
    public void OnDataBound0(object sender, EventArgs e)
    {
    }
    protected void ddl_PrevDate_OnSelectedIndexChange(object sender, EventArgs e)
    {
        fromDropDown = true;
        loadOldSetting1();
    }
    protected void ddl_librarySelectedindexchange(object sender, EventArgs e)
    {

    }
    protected void txt_frmdate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtdateerr.Visible = false;
            string dateTime = txt_frmdate.Text.Split('/')[1] + "/" + txt_frmdate.Text.Split('/')[0] + "/" + txt_frmdate.Text.Split('/')[2];
            DateTime dt = new DateTime();
            dt = DateTime.Now.Date;
            DateTime dt2 = Convert.ToDateTime(dateTime);


            if (dt2 < dt)
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Date Must be Current Date";
            }
            else if (dt2 > dt)
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Date Must be Current Date";
            }
            else
            {
                imgdiv2.Visible = false;
                lbl_alerterr.Visible = false;
                txtdateerr.Visible = false;
                //Mainpage.Visible = true;
                //btn_save.Visible = true;
                //btn_reset.Visible = true;
                //btn_exit.Visible = true;
                //ug_grid.Visible = true;
                //old_grid.Visible = true;
                //div1.Visible = true;
            }
        }
        catch
        {

        }
    }
    protected void ddl_collegeSelectedindexchange(object sender, EventArgs e)
    {
        fromDropDown = true;
        loadOldSetting();
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        // Response.Redirect("~/Hostel.aspx");
    }
    protected void btn_help_Click(object sender, EventArgs e)
    {
        // Response.Redirect("~/Hostel.aspx");
    }
    protected void btn_reset_Click(object sender, EventArgs e)
    {
        clearGridview();
    }
    public void clearGridview()
    {
        ArrayList addnew = new ArrayList();

        ug_grid.Visible = true;

        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummay5");
        DataRow dr;
        for (row = 0; row < addnew.Count; row++)
        {
            dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = Convert.ToString(addnew[row]);
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            ug_grid.DataSource = dt;
            ug_grid.DataBind();
        }
        txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_prvdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void bindcollege()
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
        }
        catch
        {
        }
    }

}