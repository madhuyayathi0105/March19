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

public partial class LibraryMod_InvoiceReport : System.Web.UI.Page
{
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string groupusercode = string.Empty;
    string clgcode = string.Empty;
    static int selected_acc_sub_wise = 0;
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable ht = new Hashtable();
    TimeSpan ts;
    DAccess2 dacces2 = new DAccess2();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("LibraryHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/LibraryMod/LibraryHome.aspx");
                    return;
                }
            }
           
                usercollegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupusercode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
          
            if (!IsPostBack)
            {
                bindclg();
                getLibPrivil();
                binddept();
                txtfrom.Attributes.Add("readonly", "readonly");
                txtfrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtto.Attributes.Add("readonly", "readonly");
                txtto.Text = DateTime.Now.ToString("MM/dd/yyyy");
            }
        }
        catch
        {
        }
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
                ddlclg.SelectedIndex = 0;
            }
        }


        catch
        {
        }
    }

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlclg.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            if (singleuser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + usercode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupusercode.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(sql, "text");
                    }
                    if (groupUser.Length > 1)
                    {
                        for (int i = 0; i < groupUser.Length; i++)
                        {
                            GrpUserVal = groupUser[i];
                            if (!GrpCode.Contains(GrpUserVal))
                            {
                                if (GrpCode == "")
                                    GrpCode = GrpUserVal;
                                else
                                    GrpCode = GrpCode + "','" + GrpUserVal;
                            }
                        }
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code in ('" + GrpCode + "')";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(sql, "text");
                    }
                }
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                libcodecollection = "WHERE lib_code IN (-1)";
                goto aa;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string codeCollection = Convert.ToString(ds.Tables[0].Rows[i]["lib_code"]);
                    if (!hsLibcode.Contains(codeCollection))
                    {
                        hsLibcode.Add(codeCollection, "LibCode");
                        if (libcodecollection == "")
                            libcodecollection = codeCollection;
                        else
                            libcodecollection = libcodecollection + "','" + codeCollection;
                    }
                }
            }
            //libcodecollection = Left(libcodecollection, Len(libcodecollection) - 1);
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;
            bindlibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void bindlibrary(string libcode)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string college = Convert.ToString(ddlclg.SelectedValue);

            if (!string.IsNullOrEmpty(college))
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + college + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib_name, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataBind();
                }

            }

        }
        catch
        {
        }
    }

    public void binddept()
    {
        try
        {
            string college = Convert.ToString(ddlclg.SelectedValue);
            string selectQuery = "select distinct(dept_name) from journal_dept,bookdetails where bookdetails.dept_code=journal_dept.dept_name and bookdetails.lib_code = journal_dept.lib_code and journal_dept.college_code =" + college + " and  bookdetails.lib_code='" + ddllibrary.SelectedValue + "' order by dept_name";

            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds;
                ddldept.DataTextField = "dept_name";
                ddldept.DataValueField = "dept_name";
                ddldept.DataBind();



            }
            ddldept.Items.Insert(0, "All");
        }
        catch (Exception ex)
        {

        }
    }

    # region Getrno1

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno1(string prefixText)
    {
        List<string> name = new List<string>();

        try
        {

            string query = "";

            WebService ws = new WebService();

            {
                string txtval = string.Empty;

                if (selected_acc_sub_wise == 0)
                {

                    query = "select distinct bill_no from bookdetails where bill_no like '" + prefixText + "%'  order by bill_no";
                }


            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    # endregion

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            double grangreantotal = 0;
            string qry = string.Empty;
            string librcode = string.Empty;
            int sno = 0;
            string colgcode = Convert.ToString(ddlclg.SelectedValue);
            string libraryname = Convert.ToString(ddllibrary.SelectedItem).Trim();
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                librcode = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }
            string BillNo = "";
            if (txtsearch1.Text != "")
            {
                BillNo = " and Bill_No='" + txtsearch1.Text + "'";
            }

            if (ddldept.Text == "All")
                qry = "select CONVERT(varchar(20),bill_date,103) bill_date,bill_no,publisher,count(acc_no) as noofbooks,sum(convert(float,price)) as amount from bookdetails where bookdetails.Lib_Code='" + librcode + "' and bill_date between '" + txtfrom.Text + "' and '" + txtto.Text + "' " + BillNo + " group by bill_no,publisher,bill_date order by bill_date";
            else
                qry = "select CONVERT(varchar(20),bill_date,103) bill_date,bill_no,publisher,count(acc_no) as noofbooks,sum(convert(float,price)) as amount from bookdetails where bookdetails.Lib_Code='" + librcode + "' and bookdetails.dept_code='" + ddldept.Text + "' and bill_date between '" + txtfrom.Text + "' and '" + txtto.Text + "' " + BillNo + " group by bill_no,publisher,bill_date order by bill_date";
            //qry = "select bill_date,bill_no,publisher,acc_no as noofbooks,price as amount from bookdetails where Lib_Code='" + librcode + "' and bill_date between '" + txtfrom.Text + "' and '" + txtto.Text + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            int previewbok = 0;
            int grandprev = 0;
            DataTable dt = new DataTable();
            DataRow drow;
            dt.Columns.Add("SNo");
            dt.Columns.Add("Date");
            dt.Columns.Add("Invoice No");
            dt.Columns.Add("Publishers Name And Address");
            dt.Columns.Add("No Of Books");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Grand Total Books");
            dt.Columns.Add("Grand Total");


           
           
            int rowcount = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                drow = dt.NewRow();

                drow["SNo"] = Convert.ToString(++rowcount);
                drow["Date"] = Convert.ToString(ds.Tables[0].Rows[i]["bill_date"]);
                drow["Invoice No"] = Convert.ToString(ds.Tables[0].Rows[i]["bill_no"]);
                drow["Publishers Name And Address"] = Convert.ToString(ds.Tables[0].Rows[i]["publisher"]);
                drow["No Of Books"] = Convert.ToString(ds.Tables[0].Rows[i]["noofbooks"]);
                drow["Amount"] = Convert.ToString(ds.Tables[0].Rows[i]["amount"]);
                previewbok += Convert.ToInt32(ds.Tables[0].Rows[i]["noofbooks"]);
                grandprev += Convert.ToInt32(ds.Tables[0].Rows[i]["amount"]);

                drow["Grand Total Books"] = Convert.ToString(previewbok);
                dt.Rows.Add(drow);

              
                if (dt.Rows.Count > 0)
                {
                    grid_Details.DataSource = dt;
                    grid_Details.DataBind();
                    divtable.Visible = true;
                    btn_printmaster.Visible = true;
                    btn_Excel.Visible = true;
                    txt_excelname.Visible = true;
                    div_report.Visible = true;
                    lbl_reportname.Visible = true;

                   
                }
                else
                {
                    divtable.Visible = false;
                    grid_Details.Visible = false;
                    divPopupAlert.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Records Found";
                    btn_printmaster.Visible = false;
                    btn_Excel.Visible = false;
                    txt_excelname.Visible = false;
                    div_report.Visible = false;
                    lbl_reportname.Visible = false;
                }
            }
        }
        catch
        {
        }
    }

  
    
    protected void grdManualExit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void grdManualExit_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grid_Details.PageIndex = e.NewPageIndex;
        btngo_click(sender, e);
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    #region print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(grid_Details, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch
        {

        }
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Book Circulation Report";
            string pagename = "book_circulation_report.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol1.loadspreaddetails(grid_Details, pagename, attendance, 0, ss,"1");
            Printcontrol1.Visible = true;
            
        }
        catch { }
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
        catch { }
    }

  
    public override void VerifyRenderingInServerForm(Control control)
    { }
    #endregion

    protected void ddllib_selectedIndexchanged(object sender, EventArgs e)
    {
        binddept();
    }

    protected void ddlclg_selectedIndexchanged(object sender, EventArgs e)
    {
        getLibPrivil();
    }
}