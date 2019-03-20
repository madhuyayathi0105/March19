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
using System.Web.Services;
using System.Drawing;

public partial class Hm_StoreMasterNew : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable yearhash = new Hashtable();
    bool check = false;
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("invM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Inventory"].Trim();
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
            ////****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("inventoryindex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/InventoryMod/inventoryindex.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                rdb_nonacademic.Checked = true;
                //year();
                bindstoremaster();
                binddepartment();
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                btn_go_Click(sender, e);
            }
            lblvalidation1.Visible = false;
            errorlable.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        Newdiv.Visible = false;
    }

    [WebMethod]
    public static string CheckUserName(string StoreName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = StoreName;
            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct StoreName,StorePK from IM_StoreMaster  where StoreName ='" + user_name + "'");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }

            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    public void bindstoremaster()
    {
        try
        {
            ddl_storemaster.Items.Clear();
            ds.Clear();
            string strquery = "";
            strquery = "select StoreName,StorePK from IM_StoreMaster where CollegeCode='" + collegecode1 + "'";

            ds = d2.select_method_wo_parameter(strquery, "Text");

            // ds = d2.BindStore(collegecode1);


            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_storemaster.DataSource = ds;
                ddl_storemaster.DataTextField = "StoreName";
                ddl_storemaster.DataValueField = "StorePK";
                ddl_storemaster.DataBind();
            }
            ddl_storemaster.Items.Insert(0, "All");
        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct StoreName from IM_StoreMaster WHERE StoreName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    public void clear()
    {
        try
        {
            txt_storename.Text = "";
            txt_storeacr.Text = "";
            txt_startyear.Text = "";
        }
        catch
        {

        }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        btn_save.Visible = true;
        btn_update.Visible = false;
        btn_delete.Visible = false;
        poperrjs.Visible = true;
        SelectdptGrid.Visible = false;
        clear();
        //year();
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_adddeptment_Click(object sender, EventArgs e)
    {
        binddepartment();
        Newdiv.Visible = true;
        cb_selectall.Checked = false;
    }

    public void binddepartment()
    {
        try
        {
            string deptquery = "";
            //string deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + collegecode1 + "' order by Dept_Code ";
            if (rdb_academic.Checked == true)
            {
                deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + collegecode1 + "' and isacademic ='1' order by Dept_Code";
            }
            else if (rdb_nonacademic.Checked == true)
            {
                deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + collegecode1 + "' and isacademic ='0' order by Dept_Code";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dptgrid.DataSource = ds;
                dptgrid.DataBind();
                dptgrid.Visible = true;
            }
            else
            {
                dptgrid.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void rdb_academic_CheckedChanged(object sender, EventArgs e)
    {
        binddepartment();
    }
    protected void rdb_nonacademic_CheckedChanged(object sender, EventArgs e)
    {
        binddepartment();
    }

    protected void btn_deptexit_Click(object sender, EventArgs e)
    {
        try
        {
            Newdiv.Visible = false;
        }
        catch
        {

        }
    }
    protected void cb_selectAll_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_selectall.Checked == true)
            {
                if (dptgrid.Rows.Count > 0)
                {
                    for (int i = 0; i < dptgrid.Rows.Count; i++)
                    {
                        (dptgrid.Rows[i].FindControl("cbcheck") as CheckBox).Checked = true;
                    }
                }
            }
            if (cb_selectall.Checked == false)
            {
                if (dptgrid.Rows.Count > 0)
                {
                    for (int i = 0; i < dptgrid.Rows.Count; i++)
                    {
                        (dptgrid.Rows[i].FindControl("cbcheck") as CheckBox).Checked = false;
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_deptpartmentsave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DeptCode");
            dt.Columns.Add("DeptName");
            DataRow dr;
            if (dptgrid.Rows.Count > 0)
            {
                for (int ik = 0; ik < dptgrid.Rows.Count; ik++)
                {
                    if ((dptgrid.Rows[ik].FindControl("cbcheck") as CheckBox).Checked == true)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString((dptgrid.Rows[ik].FindControl("lbldeptcode") as Label).Text);
                        dr[1] = Convert.ToString((dptgrid.Rows[ik].FindControl("lbldeptname") as Label).Text);
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    SelectdptGrid.DataSource = dt;
                    SelectdptGrid.DataBind();
                    Newdiv.Visible = false;
                    SelectdptGrid.Visible = true;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select Any Record";
                }
            }

        }
        catch
        {

        }
    }
    protected void txtyear_Onchange(object sender, EventArgs e)
    {
        if (txt_startyear.Text.Trim() != "")
        {
            int year2 = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
            int txtyear = Convert.ToInt32(txt_startyear.Text);
            int oldyear = Convert.ToInt32(oldyeartxt.Text);
            if (oldyear <= txtyear && year2 >= txtyear)
            {

            }
            else
            {
                txt_startyear.Text = "";
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter Valid Year";
            }
        }
        else
        {
            txt_startyear.Text = "";
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Enter Valid Year";
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string storename = Convert.ToString(txt_storename.Text.First().ToString().ToUpper() + txt_storename.Text.Substring(1));
            string storeacr = Convert.ToString(txt_storeacr.Text).ToUpper();
            // storename = storename.ToUpperInvariant();
            storename = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(storename);

            string year = "";
            year = Convert.ToString(txt_startyear.Text);

            collegecode = Session["collegecode"].ToString();
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            if (storename.Trim() != "" && storeacr.Trim() != "")
            {
                if (SelectdptGrid.Rows.Count > 0)
                {
                    int stroecode = 0;
                    string laststorecode = d2.GetFunction("select StorePK from IM_StoreMaster where CollegeCode ='" + collegecode + "' order by StorePK desc");
                    if (laststorecode.Trim() != "")
                    {
                        stroecode = Convert.ToInt32(laststorecode);
                        stroecode++;
                    }
                    else
                    {
                        stroecode = 1;
                    }
                    int selectall = 0;

                    if (cb_selectall.Checked == true)
                    {
                        selectall = 1;
                    }
                    string insertstorequery = " insert into IM_StoreMaster (StoreAcr,StoreName,StoreStartYear,CollegeCode) values ('" + storeacr + "','" + storename + "','" + year + "','" + collegecode + "')";

                    //string insertstorequery = "insert into storemaster (access_date,access_time,store_code,store_acr,store_name,start_year,is_alldept,college_code) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + stroecode + "','" + storeacr + "','" + storename + "','" + year + "','" + selectall + "','" + collegecode + "')";
                    int inster = d2.update_method_wo_parameter(insertstorequery, "Text");
                    if (inster != 0)
                    {
                        for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
                        {
                            string deptcode = "";
                            deptcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbldeptcode") as Label).Text);
                            //string insertdeptquery = "insert into storedetails(access_date,access_time,dept_code,store_code,college_code) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + deptcode + "','" + stroecode + "','" + collegecode + "')";

                            string insertdeptquery = "insert into IM_StoreDeptDet(DeptCode,StoreFK) values ('" + deptcode + "','" + stroecode + "')";

                            int up = d2.update_method_wo_parameter(insertdeptquery, "Text");

                        }
                        bindstoremaster();
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        btn_addnew_Click(sender, e);
                        //btn_go_Click(sender, e);
                        //poperrjs.Visible = false;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select Department";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Fill all the Values";
            }
        }
        catch
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string selectquery = "";
            collegecode = Session["collegecode"].ToString();
            DataView dv = new DataView();

            if (txt_search.Text.Trim() != "")
            {

                selectquery = "select s.StorePK,s.CollegeCode,s.StoreAcr,s.StoreName,s.StoreStartYear,dt.Dept_Code,dt.Dept_Name from IM_StoreMaster s,IM_StoreDeptDet sd,Department dt where s.StorePK =sd.StoreFK and sd.DeptCode =dt.Dept_Code and s.CollegeCode ='" + collegecode + "' and s.StoreName='" + txt_search.Text + "' ";
                //selectquery = "select Store_Acr,s.Store_Code,Store_Name,dept_acronym,sd.Dept_Code  from StoreMaster s,StoreDetails sd,Department d where s.Store_Code =sd.Store_Code and d.Dept_Code =sd.Dept_Code and s.College_Code =sd.College_Code and s.College_Code = d.college_code and s.College_Code ='" + collegecode + "' and store_name='" + txt_search.Text + "' ";
                selectquery = selectquery + " select distinct StorePK,StoreName,StoreAcr ,StoreStartYear  from IM_StoreMaster where CollegeCode ='" + collegecode + "'  and StoreName='" + txt_search.Text + "'";
            }
            else
            {
                if (ddl_storemaster.SelectedItem.Text != "All")
                {
                    selectquery = "select s.StorePK,s.CollegeCode,s.StoreAcr,s.StoreName,s.StoreStartYear,dt.Dept_Code,dt.Dept_Name from IM_StoreMaster s,IM_StoreDeptDet sd,Department dt where s.StorePK =sd.StoreFK and sd.DeptCode =dt.Dept_Code and s.CollegeCode ='" + collegecode + "' and s.StorePK ='" + ddl_storemaster.SelectedItem.Value + "'";

                    //selectquery = "select Store_Acr,s.Store_Code,Store_Name,dept_acronym,sd.Dept_Code  from StoreMaster s,StoreDetails sd,Department d where s.Store_Code =sd.Store_Code and d.Dept_Code =sd.Dept_Code and s.College_Code =sd.College_Code and s.College_Code = d.college_code and s.College_Code ='" + collegecode + "' and s.Store_Code ='" + ddl_storemaster.SelectedItem.Value + "'";
                    selectquery = selectquery + " select distinct StorePK,StoreName,StoreAcr ,StoreStartYear  from IM_StoreMaster where CollegeCode ='" + collegecode + "' and StorePK ='" + ddl_storemaster.SelectedItem.Value + "' ";
                }
                else
                {
                    {
                        selectquery = "select s.StorePK,s.CollegeCode,s.StoreAcr,s.StoreName,s.StoreStartYear,dt.Dept_Code,dt.Dept_Name from IM_StoreMaster s,IM_StoreDeptDet sd,Department dt where s.StorePK =sd.StoreFK and sd.DeptCode =dt.Dept_Code and s.CollegeCode ='" + collegecode + "' ";
                        selectquery = selectquery + "  select distinct StorePK,StoreName,StoreAcr ,StoreStartYear  from IM_StoreMaster where CollegeCode ='" + collegecode + "' ";
                    }
                }
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[1].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].Columns.Count = 5;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[0].Width = 50;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Store Acronym";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Main Store Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[2].Width = 180;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Start Year";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[3].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 200;

                for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                {
                    string concate = "";
                    string concatecode = "";
                    ds.Tables[0].DefaultView.RowFilter = "StorePK ='" + Convert.ToString(ds.Tables[1].Rows[row]["StorePK"]) + "'";
                    dv = ds.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        for (int i = 0; i < dv.Count; i++)
                        {
                            if (concate == "")
                            {
                                concate = Convert.ToString(dv[i]["Dept_Name"]);
                                concatecode = Convert.ToString(dv[i]["Dept_Code"]);
                            }
                            else
                            {
                                concate = concate + " , " + Convert.ToString(dv[i]["Dept_Name"]);
                                concatecode = concatecode + " , " + Convert.ToString(dv[i]["Dept_Code"]);
                            }
                        }
                    }
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[1].Rows[row]["StoreAcr"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[1].Rows[row]["StorePK"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[1].Rows[row]["StoreName"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[row]["StoreStartYear"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(concate);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(concatecode);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                Fpspread1.Visible = true;
                errorlable.Visible = false;
                rptprint.Visible = true;
                div1.Visible = true;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            }
            else
            {
                errorlable.Visible = true;
                errorlable.Text = "No Records Found";
                div1.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Store Master Report";
            string pagename = "Hm_StoreMasterNew.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
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
        try
        {
            if (check == true)
            {
                poperrjs.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                if (activerow.Trim() != "")
                {
                    //year();
                    string storeacr = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string storename = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string Store_PK = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    Session["StorePK"] = Convert.ToString(Store_PK);
                    string year1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string depcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                    txt_storename.Text = Convert.ToString(storename);
                    txt_storeacr.Text = Convert.ToString(storeacr);
                    txt_startyear.Text = year1;
                    //if (year1.Trim() != "")
                    //{
                    //    int index = Convert.ToInt32(yearhash[Convert.ToString(year1)]);
                    //    ddl_startyear.SelectedIndex = index;

                    //}
                    if (depcode.Trim() != "" && depcode.Trim() != null)
                    {
                        string selectquery = "select  Dept_Code as DeptCode ,Dept_Name as DeptName from Department where Dept_Code in (" + depcode + ") and college_code ='" + collegecode + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            SelectdptGrid.DataSource = ds;
                            SelectdptGrid.DataBind();
                            SelectdptGrid.Visible = true;
                        }
                        else
                        {
                            ds = null;
                            SelectdptGrid.DataSource = ds;
                            SelectdptGrid.DataBind();
                            SelectdptGrid.Visible = false;
                        }
                    }
                    else
                    {
                        ds = null;
                        SelectdptGrid.DataSource = ds;
                        SelectdptGrid.DataBind();
                        SelectdptGrid.Visible = false;
                    }
                    btn_save.Visible = false;
                    btn_update.Visible = true;
                    btn_delete.Visible = true;
                }
            }
        }
        catch
        {
        }
    }

    //protected void year()
    //{
    //    ddl_startyear.Items.Clear();
    //    yearhash.Clear();
    //    int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
    //    for (int l = 0; l < 15; l++)
    //    {
    //        ddl_startyear.Items.Add(Convert.ToString(year));
    //        yearhash.Add(Convert.ToString(year), l);
    //        year--;
    //    }
    //    //ddl_startyear.Items.Insert(0, "Select");
    //}
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string storename = Convert.ToString(txt_storename.Text.First().ToString().ToUpper() + txt_storename.Text.Substring(1));
            string storeacr = Convert.ToString(txt_storeacr.Text);
            string year1 = "";
            //year1 = Convert.ToString(ddl_startyear.SelectedItem.Text);
            year1 = Convert.ToString(txt_startyear.Text);
            //if (year1.Trim() == "")
            //{
            //    year1 = "";
            //}
            collegecode = Session["collegecode"].ToString();
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            if (storename.Trim() != "" && storeacr.Trim() != "")
            {
                int stroecode = Convert.ToInt32(Session["StoreCode"]);
                int selectall = 0;

                if (cb_selectall.Checked == true)
                {
                    selectall = 1;
                }
                //string delete = " delete from StoreMaster where Store_Code ='" + stroecode + "'";
                string delete = " delete from IM_StoreDeptDet where StoreFK ='" + Convert.ToString(Session["StorePK"]) + "'";
                int upnow = d2.update_method_wo_parameter(delete, "Text");

                string insertstorequery = "update IM_StoreMaster set  StoreName ='" + storename + "', StoreStartYear='" + year1 + "', StoreAcr ='" + storeacr + "' where CollegeCode ='" + collegecode + "' and StorePK ='" + Convert.ToString(Session["StorePK"]) + "' ";

                //string insertstorequery = "insert into storemaster (access_date,access_time,store_code,store_acr,store_name,start_year,is_alldept,college_code) values ('" + dtaccessdate + "','" + dtaccesstime + "','" + stroecode + "','" + storeacr + "','" + storename + "','" + year1 + "','" + selectall + "','" + collegecode + "')";
                int inster = d2.update_method_wo_parameter(insertstorequery, "Text");
                if (inster != 0)
                {
                    for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
                    {
                        string deptcode = "";
                        deptcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbldeptcode") as Label).Text);
                        string insertdeptquery = "insert into IM_StoreDeptDet(DeptCode,StoreFK) values ('" + deptcode + "','" + Convert.ToString(Session["StorePK"]) + "')";
                        int up = d2.update_method_wo_parameter(insertdeptquery, "Text");

                    }
                    //year();
                    bindstoremaster();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Updated Successfully";
                    btn_go_Click(sender, e);
                    poperrjs.Visible = false;
                    //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Updated Sucessfully\");", true);

                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Fill all the Values";
                //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Fill all the Values\");", true);
            }
        }
        catch
        {

        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {
        }
    }
    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            int stroepk = Convert.ToInt32(Session["StorePK"]);
            string delete = " delete from IM_StoreMaster where StorePK ='" + stroepk + "'";
            delete = delete + " delete from IM_StoreDeptDet where StoreFK ='" + stroepk + "'";
            int upnow = d2.update_method_wo_parameter(delete, "Text");
            if (upnow != 0)
            {
                bindstoremaster();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                btn_go_Click(sender, e);
                poperrjs.Visible = false;
                //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Deleted Sucessfully\");", true);
            }
        }
        catch
        {
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {

        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        //surediv.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        poperrjs.Visible = true;
    }

    public object sender { get; set; }

    public EventArgs e { get; set; }
}