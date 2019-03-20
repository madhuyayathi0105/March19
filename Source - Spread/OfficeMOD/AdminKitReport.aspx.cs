using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class OfficeMOD_AdminKitReport : System.Web.UI.Page
{
    string collegecode = string.Empty;

    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    DataSet dscol = new DataSet();
    Hashtable grandtotal = new Hashtable();
    private string usercode;
    static byte roll = 0;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
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
        // collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {
            loadcollege();
            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);

            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            Puser.Visible = false;
            getPrintSettings();
        }

    }
    #region college
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ddl_collegename.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        divspread.Visible = false;
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }
    #endregion

    #region popup
    protected void btnuse_Click(object sender, EventArgs e)
    {
        try
        {
            Puser.Visible = true;
            chkincludegroup.Checked = false;
            txtusersearch.Text = "";
            loaduser();
        }
        catch (Exception ex)
        {
            lblperrmsg.Visible = true;
            lblperrmsg.Text = ex.ToString();
        }
    }

    protected void chkincludegroup_Checked(object sender, EventArgs e)
    {
        loaduser();
    }

    protected void btnuserok_Click(object sender, EventArgs e)
    {
        try
        {

            Fpuser.SaveChanges();



            for (int r = 0; r < Fpuser.Sheets[0].Rows.Count; r++)
            {
                int isval = Convert.ToInt32(Fpuser.Sheets[0].Cells[r, 3].Value);
                if (isval == 1)
                {
                    string strquery = "select  User_id,Full_Name from UserMaster where User_id='" + Fpuser.Sheets[0].Cells[r, 1].Text.ToString() + "'";
                    DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string uname = ds.Tables[0].Rows[0]["user_id"].ToString();
                        string funame = ds.Tables[0].Rows[0]["full_name"].ToString();


                        txtusername.Text = uname;



                    }

                    Puser.Visible = false;

                }
            }


        }
        catch (Exception ex)
        {
            lblperrmsg.Visible = true;
            lblperrmsg.Text = ex.ToString();
        }
    }
    protected void btnuseexit_Click(object sender, EventArgs e)
    {
        try
        {
            Puser.Visible = false;
        }
        catch (Exception ex)
        {
            lblperrmsg.Visible = true;
            lblperrmsg.Text = ex.ToString();
        }
    }
    public void loaduser()
    {
        try
        {
            Fpuser.Sheets[0].ColumnCount = 4;
            Fpuser.Sheets[0].RowCount = 0;
            Fpuser.SheetCorner.ColumnCount = 0;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpuser.Sheets[0].Columns[0].Width = 50;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 1].Text = "User Name";
            Fpuser.Sheets[0].Columns[1].Width = 200;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Full Name";
            Fpuser.Sheets[0].Columns[2].Width = 200;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            Fpuser.Sheets[0].Columns[3].Width = 50;
            Fpuser.CommandBar.Visible = false;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            string chkgroup = " and (isnull(group_code,'')='' or group_code='0' or group_code='-1')";
            if (chkincludegroup.Checked == true)
            {
                chkgroup = "";
            }

            string strusename = "";
            if (txtusersearch.Text.ToString().Trim() != "")
            {
                strusename = " and user_id like '" + txtusersearch.Text.ToString() + "%'";
            }

            string strquery = "select User_code,user_id,Full_Name from usermaster where college_code='" + ddl_collegename.SelectedValue.ToString() + "' " + chkgroup + " " + strusename + " order by user_id";


            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Fpuser.Sheets[0].RowCount++;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].Text = Fpuser.Sheets[0].RowCount.ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].CellType = txt;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["user_id"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[0].Rows[i]["User_code"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].CellType = txt;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Full_Name"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].CellType = chk;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

            }
            Fpuser.Width = 518;
            Fpuser.Sheets[0].PageSize = Fpuser.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblperrmsg.Visible = true;
            lblperrmsg.Text = ex.ToString();
        }
    }
    #endregion
    protected void btngo_Click(object sender, EventArgs e)
    {
        ds = getdetailsAdminreport();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadspread(ds);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }

    private DataSet getdetailsAdminreport()
    {

        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;


            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedValue);


            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                //fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();  //existing
                fromdate = frdate[2].ToString() + "-" + frdate[1].Trim() + "-" + frdate[0].Trim();   //modified by prabha on feb 09 2018
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                //todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();    //existing
                todate = tdate[2].ToString() + "-" + tdate[1].Trim() + "-" + tdate[0].Trim();   //modified by prabha on feb 09 2018
            string selQ = string.Empty;
            string username = txtusername.Text;

            if (!string.IsNullOrEmpty(collegecode))
            {
//selQ = "select CONVERT(varchar(20),ul.in_Time,103) InDate,CONVERT(varchar(8),ul.in_Time,108) InTime,ul.in_Time,CONVERT(varchar(8),ul.out_time,103) OutDate,CONVERT(varchar(5),ul.out_time,108) OutTime,ul.out_time,um.User_id,u.UsrAction,u.Details,u.ctrNam,case when UsrAction=0 then 'Login' when UsrAction=1 then 'Save' when UsrAction=2 then 'Update' when UsrAction=3 then 'Delete'  end UsrAction1,isnull(u.IpAddress,'0') as IpAddress from usermaster um,usereelog ul left join userlog u on ul.Entry_Code=u.Entry_Code where ul.User_Code=um.User_code and UsrAction in(0,1,2,3) and college_code='" + collegecode + "' and ul.DOA between'" + fromdate + "' and '" + todate + "' and um.User_id='" + username + "' order by ul.in_Time ";
               selQ = "select CONVERT(varchar(20),ul.in_Time,103) InDate,CONVERT(varchar(8),ul.in_Time,108) InTime,ul.in_Time,CONVERT(varchar(8),ul.out_time,103) OutDate,CONVERT(varchar(8),ul.out_time,108) OutTime,ul.out_time,um.User_id,u.UsrAction,u.Details,u.ctrNam,case when UsrAction=0 then 'Login' when UsrAction=1 then 'Save' when UsrAction=2 then 'Update' when UsrAction=3 then 'Delete'  end UsrAction1,isnull(u.IpAddress,'0') as IpAddress,u.version from usermaster um,usereelog ul left join userlog u on ul.Entry_Code=u.Entry_Code where ul.User_Code=um.User_code and UsrAction in(0,1,2,3) and college_code='" + collegecode + "' and ul.DOA between'" + fromdate + "' and '" + todate + "' and um.User_id='" + username + "' order by ul.in_Time ";//Altered by mashumathi 19/04/2018

                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");

            }

            //string degreeDetails = "select c.course_name,dt.Dept_Name from  course c,Degree d,Department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code";

            #endregion
        }
        catch (Exception ex)
        { }

        return dsload;
    }

    private void loadspread(DataSet ds)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("Date");
            dt.Columns.Add("Login Time");
            dt.Columns.Add("LogOut Time");
            dt.Columns.Add("User Name");
            dt.Columns.Add("Activity");
            dt.Columns.Add("Ip Address");//Added by saranya on 11/04/2018
            dt.Columns.Add("Details");
            dt.Columns.Add("Activity Details");
           dt.Columns.Add("Version");//Added by madhumathi 19/04/2018
            dt.Columns.Add("Total Hours");   //Added by Mullai


            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            for (int row = 0; row < dt.Columns.Count; row++)
            {
                spreadDet.Sheets[0].ColumnCount++;
                string col = Convert.ToString(dt.Columns[row].ColumnName);
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            }

            DataRow drow;
            int rowcount = 0;

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                spreadDet.Sheets[0].RowCount++;
                //*****added by Mullai
                TimeSpan ts;
                string log_in_time = Convert.ToString(ds.Tables[0].Rows[row]["InTime"]).Trim();
                string log_out_time = Convert.ToString(ds.Tables[0].Rows[row]["OutTime"]).Trim();
                DateTime di = Convert.ToDateTime(log_in_time);
                DateTime dot = Convert.ToDateTime(log_out_time);
                ts = dot - di;
                string tot_hours = Convert.ToString(ts);
              
                string degreeDetails = Convert.ToString(ds.Tables[0].Rows[row]["Details"]).Trim();
                string date1=Convert.ToString(ds.Tables[0].Rows[row]["InDate"]).Trim();
                string adNAme = Convert.ToString(ds.Tables[0].Rows[row]["User_id"]).Trim();
                string activity = Convert.ToString(ds.Tables[0].Rows[row]["UsrAction1"]).Trim();
                string ipadd = Convert.ToString(ds.Tables[0].Rows[row]["IpAddress"]).Trim();
                string actdetails = Convert.ToString(ds.Tables[0].Rows[row]["CtrNam"]).Trim();
                string versi = Convert.ToString(ds.Tables[0].Rows[row]["version"]).Trim();
                string[] split = degreeDetails.Split(':');
                if (split.Length > 0)
                {
                    string degCode = split[0];
                    int val = 0;
                    if (int.TryParse(degCode, out val))
                    {
                        string deptName = d2.GetFunctionv("select c.course_name+' - '+dt.Dept_Name from  course c,Degree d,Department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code='" + degCode + "'");
                        split[0] = deptName;
                    }
                }

                degreeDetails = string.Join(":", split);
                //for (int col = 0; col < dt.Columns.Count; col++)
                //{ 
                spreadDet.Sheets[0].ColumnCount = 11;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowcount);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(date1);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(log_in_time);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(log_out_time);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(adNAme);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(activity);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ipadd);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(degreeDetails);

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(actdetails);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(versi);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(tot_hours);
                ///////////****

                    ////if (col == 0)
                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(++rowcount);
                    //else if (col == 2)

                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][1]);
                    //else if (col == 3)

                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][4]);

                   


                    //else if (col == 4)

                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][6]);
                    //else if (col == 5)

                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][10]);

                    ////=====Added by saranya on 11/04/2018====//
                    //else if (col == 6)
                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][11]);
                    ////===================//

                    //else if (col == 7)
                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(degreeDetails);

                    //else if (col == 8)
                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][12]);
                    ////=============Added by madhumathi  19/04/2018===============//
                    ////else if (col == 9)
                    //// spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][13]);//==================/
                    //else if(col==9)
                        

                    //   spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(tot_hours);

                    //else
                    //{
                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][col - 1]);
                    //}
               // }

            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            divspread.Visible = true;
            print.Visible = true;

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
                // lblvalidation1.Visible = false;
            }
            else
            {
                // lblvalidation1.Text = "Please Enter Your  Report Name";
                //  lblvalidation1.Visible = true;
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
            degreedetails = "Admin Kit Report";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "AdminKitReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch { }
    }

    #endregion

    #region Added by saranya on 11/04/2018 for view details

    protected void btnView_Click(object sender, EventArgs e)
    {
        ds = getViewdetailsAdminreport();

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadspread(ds);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }

    private DataSet getViewdetailsAdminreport()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                //fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();  //existing
                fromdate = frdate[2].ToString() + "-" + frdate[1].Trim() + "-" + frdate[0].Trim();   //modified by prabha on feb 09 2018
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                //todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();    //existing
                todate = tdate[2].ToString() + "-" + tdate[1].Trim() + "-" + tdate[0].Trim();   //modified by prabha on feb 09 2018
            string selQ = string.Empty;
            string username = txtusername.Text;

            if (!string.IsNullOrEmpty(collegecode))
            {
              //  selQ = "select CONVERT(varchar(20),ul.in_Time,103) InDate,CONVERT(varchar(8),ul.in_Time,108) InTime,ul.in_Time,CONVERT(varchar(8),ul.out_time,103) OutDate,CONVERT(varchar(5),ul.out_time,108) OutTime,ul.out_time,um.User_id,u.UsrAction,u.Details,u.ctrNam,case when UsrAction=0 then 'Login' when UsrAction=1 then 'Save' when UsrAction=2 then 'Update' when UsrAction=3 then 'Delete'  end UsrAction1,isnull(u.IpAddress,'0') as IpAddress from usermaster um,usereelog ul left join userlog u on ul.Entry_Code=u.Entry_Code where ul.User_Code=um.User_code and UsrAction in(0,1,2,3) and college_code='" + collegecode + "' and ul.DOA between'" + fromdate + "' and '" + todate + "' order by ul.in_Time ";//and um.User_id='" + username + "'

                selQ = "select CONVERT(varchar(20),ul.in_Time,103) InDate,CONVERT(varchar(8),ul.in_Time,108) InTime,ul.in_Time,CONVERT(varchar(8),ul.out_time,103) OutDate,CONVERT(varchar(5),ul.out_time,108) OutTime,ul.out_time,um.User_id,u.UsrAction,u.Details,u.ctrNam,case when UsrAction=0 then 'Login' when UsrAction=1 then 'Save' when UsrAction=2 then 'Update' when UsrAction=3 then 'Delete'  end UsrAction1,isnull(u.IpAddress,'0') as IpAddress,u.version from usermaster um,usereelog ul left join userlog u on ul.Entry_Code=u.Entry_Code where ul.User_Code=um.User_code and UsrAction in(0,1,2,3) and college_code='" + collegecode + "' and ul.DOA between'" + fromdate + "' and '" + todate + "' order by ul.in_Time ";//Altered by mashumathi 19/04/2018
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");
            }          
            #endregion
        }
        catch (Exception ex)
        { }
        return dsload;
    }

    #endregion
}