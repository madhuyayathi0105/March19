using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using InsproDataAccess;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Globalization;

public partial class LibraryMod_cumm_issued_report : System.Web.UI.Page
{

    DataTable dtCommon = new DataTable();
    DataSet ds = new DataSet();
    DataSet rsbok = new DataSet();
    DAccess2 d2 = new DAccess2();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    InsproDirectAccess dir = new InsproDirectAccess();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();

    string Sql = string.Empty;
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string qryCollege = string.Empty;
    DataTable cumisuue = new DataTable();
    static int dept_count = 0;
    static int lib_count = 0;
    DataRow dr;
    DataRow dr1;
    DataRow dr3;

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
          
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
          
            if (!IsPostBack)
            {
                Bindcollege();
                Bindreporttype();
                getLibPrivil();
                Binddept();
                Bindtype();
                txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch
        {
        }

    }

    #region college

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
            }
        }
        catch
        {
        }
    }

    #endregion

    protected void btn_go_Click(object sender, EventArgs e)
    {

        if (ddlreporttype.SelectedIndex == 0)
        {
            //loadSpread_Header();
            // Assign_dept();
            search_dept();
        }
        else if (ddlreporttype.SelectedIndex == 1)
        {
            // loadSpread_Header();

            search_date();
            if (!cbfrom.Checked)
            {
                lblAlertMsg.Text = "Select the date";
                divPopAlert.Visible = true;
            }
            if (cumisuue.Rows.Count > 0)
            {
                grdCumIssReport.DataSource = cumisuue;
                grdCumIssReport.DataBind();
                grdCumIssReport.Visible = true;
            }
        }
        else if (ddlreporttype.SelectedIndex == 2)
        {
            // loadSpread_Header();
            Search_Subject();
        }

    }

    #region Assign_date

    public void Assign_date()
    {
        try
        {
            if (txt_fromdate1.Text != null && txt_todate1.Text != null)
            {
                int fd, fm, fy, td, tm, ty = 0;
                string fdate = txt_fromdate1.Text;
                string tdate = txt_todate1.Text;
                string[] tmp1 = fdate.Split(new Char[] { '/' });
                fd = Convert.ToInt32(tmp1[0]);
                fm = Convert.ToInt32(tmp1[1]);
                fy = Convert.ToInt32(tmp1[2]);
                string[] tmp2 = tdate.Split(new Char[] { '/' });
                td = Convert.ToInt32(tmp2[0]);
                tm = Convert.ToInt32(tmp2[1]);
                ty = Convert.ToInt32(tmp2[2]);
                tdate = tmp2[1] + "/" + tmp2[0] + "/" + tmp2[2];
                DateTime fdt = new DateTime(fy, fm, fd, 0, 0, 0);
                DateTime tdt = new DateTime(ty, tm, td, 0, 0, 0);
                int rowcnt = 0;

                //DateTime fdate = Convert.ToDateTime(txt_fromdate1.Text);
                //DateTime tdate = Convert.ToDateTime(txt_todate1.Text);
                while (fdt < tdt)
                {
                    cumisuue.Rows[rowcnt]["Date"] = Convert.ToString(fdt);

                    if (fm == 2)
                    {
                        if (fd >= 28)
                        {
                            fm++;
                            fd = 1;
                        }
                        else if (fd < 29)
                            fd++;
                    }
                    else if (fm != 2)
                    {
                        if (fd < 30)
                            fd++;
                        if (fd >= 30)
                        {
                            fm++;
                            fd = 1;
                        }
                    }
                    if (fm > 12)
                        fy++;
                    fdt = new DateTime(fy, fm, fd, 0, 0, 0);
                    rowcnt++;
                }

            }
        }
        catch
        {
        }
    }

    #endregion

    public void search_dept()
    {
        try
        {
            int IntTotBooks = 0;
            int IntTotNBook = 0;
            int IntGTotal = 0;
            int IntGTStudBooks = 0;
            int IntGTStudNBooks = 0;
            int IntGTStaffBooks = 0;
            int IntGTStaffNBooks = 0;
            String Str_Date = string.Empty;
            String Str_LibCode = string.Empty;
            String Str_DeptCode = string.Empty;
            String Str_Type = string.Empty;

            cumisuue.Columns.Add("Department", typeof(string));
            cumisuue.Columns.Add("Student(Books)", typeof(string));
            cumisuue.Columns.Add("Staff(Books)", typeof(string));
            cumisuue.Columns.Add("Total(Books)", typeof(string));
            cumisuue.Columns.Add("Student(Non Books)", typeof(string));
            cumisuue.Columns.Add("Staff(Non Books)", typeof(string));
            cumisuue.Columns.Add("Total(Non Books)", typeof(string));
            cumisuue.Columns.Add("Total", typeof(string));

            if (cbfrom.Checked == true)
            {
                string fdate = txt_fromdate1.Text;
                string tdate = txt_todate1.Text;
                string[] tmp1 = fdate.Split(new Char[] { '/' });
                fdate = tmp1[1] + "/" + tmp1[0] + "/" + tmp1[2];
                string[] tmp2 = tdate.Split(new Char[] { '/' });
                tdate = tmp2[1] + "/" + tmp2[0] + "/" + tmp2[2];
                Str_Date = " AND B.Borrow_Date Between '" + fdate + "' AND '" + tdate + "' ";
            }
            else
                Str_Date = "";
            if (ddl_type.Text == "Returned")
                Str_Type = " AND B.Return_Flag = 1";
            else if (ddl_type.Text == "Not Returned")
                Str_Type = " AND B.Return_Flag = 0";
            else
                Str_Type = "";
            string vallib = string.Empty;
            if (chklstlib.Items.Count > 0)
                vallib = rs.getCblSelectedValue(chklstlib);
            if (!string.IsNullOrEmpty(vallib))
                Str_LibCode = " AND B.Lib_Code IN('" + vallib + "')";

            string dep = string.Empty;
            string dep_name = string.Empty;
            if (chklstdept.Items.Count > 0)
            {
                dep = rs.GetSelectedItemsValue(chklstdept);
                dep_name = rs.GetSelectedItemsText(chklstdept);
            }
            IntTotBooks = 0;
            IntTotNBook = 0;
            IntGTotal = 0;
            string dept_code = string.Empty;
            int IntGTotalBooks = 0;
            int IntGTotalNonBooks = 0;
            int IntGrandTotal = 0;

            for (int deptChk = 0; deptChk < chklstdept.Items.Count; deptChk++)
            {
                if (chklstdept.Items[deptChk].Selected)
                {
                    Str_DeptCode = " In (" + chklstdept.Items[deptChk].Value + ")  ";
                    Sql = "SELECT COUNT(*) TotStudBook" + " " + ",D.dept_acronym FROM Borrow B,Registration R,Degree G,Department D,Library L" + " " + "Where b.roll_no = r.roll_no And r.degree_code = g.degree_code" + " " + "AND G.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + " " + "AND Return_Type = 'BOK' AND Is_Staff = 0" + " " + "AND G.Dept_Code " + Str_DeptCode + Str_Date + Str_LibCode + Str_Type + " " + "  GROUP BY G.Dept_Code,D.dept_acronym";

                    DataSet DS = d2.select_method_wo_parameter(Sql, "Text");
                    int rcnt = DS.Tables[0].Rows.Count;
                    dr = cumisuue.NewRow();
                    dr["Department"] = Convert.ToString(chklstdept.Items[deptChk].Text);
                    IntTotBooks = 0;
                    IntTotNBook = 0;
                    IntGTotal = 0;
                    if (rcnt > 0)
                    {
                        dr["Student(Books)"] = Convert.ToString(DS.Tables[0].Rows[0]["TotStudBook"]);
                        IntTotBooks = IntTotBooks + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                        IntGTotal = IntGTotal + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                        IntGTStudBooks = IntGTStudBooks + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                    }
                    else
                    {
                        dr["Student(Books)"] = "0";
                        IntTotBooks = IntTotBooks + 0;
                    }
                    Sql = "SELECT COUNT(*) TotStaffBook" + " " + " from Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L" + " " + "Where b.roll_no = M.Staff_Code  And M.Staff_Code = T.Staff_Code " + "" + "AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + " " + "AND Return_Type = 'BOK' AND Is_Staff = 1" + " " + "AND T.Dept_Code " + Str_DeptCode + "AND T.Latestrec = 1" + " " + Str_Date + Str_LibCode + Str_Type + " " + " GROUP BY T.Dept_Code";

                    DataSet DS1 = d2.select_method_wo_parameter(Sql, "Text");
                    int rcnt1 = DS1.Tables[0].Rows.Count;
                    if (rcnt1 > 0)
                    {
                        dr["Staff(Books)"] = Convert.ToString(DS1.Tables[0].Rows[0]["TotStaffBook"]);
                        IntTotBooks = IntTotBooks + (Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]));
                        IntGTotal = IntGTotal + (Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]));
                        IntGTStaffBooks = IntGTStaffBooks + (Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]));
                    }
                    else
                    {
                        dr["Staff(Books)"] = "0";
                        IntTotBooks = IntTotBooks + 0;
                    }
                    //******************//

                    Sql = "SELECT COUNT(*) TotStudNBook" + " " + "FROM Borrow B,Registration R,Degree G,Department D,Library L" + " " + "Where b.roll_no = r.roll_no  And r.degree_code = g.degree_code" + " " + "AND G.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + " " + "AND Return_Type = 'NBM' AND Is_Staff = 0" + " " + "AND G.Dept_Code " + Str_DeptCode + " " + Str_Date + Str_LibCode + " " + " GROUP BY G.Dept_Code";

                    DataSet DS2 = d2.select_method_wo_parameter(Sql, "Text");
                    int rcnt2 = DS2.Tables[0].Rows.Count;

                    if (rcnt2 > 0)
                    {
                        dr["Student(Non Books)"] = Convert.ToString(DS2.Tables[0].Rows[0]["TotStudNBook"]);
                        IntTotNBook = IntTotNBook + (Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudNBook"]));
                        IntGTotal = IntGTotal + (Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudNBook"]));
                        IntGTStudNBooks = IntGTStudNBooks + (Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudNBook"]));
                    }
                    else
                    {
                        dr["Student(Non Books)"] = "0";
                        IntTotNBook = IntTotNBook + 0;
                    }
                    //******************//

                    Sql = "SELECT COUNT(*) TotStaffNBook" + " " + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L" + " " + "Where b.roll_no = M.Staff_Code  And M.Staff_Code = T.Staff_Code " + " " + "AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + " " + "AND Return_Type = 'NBM' AND Is_Staff = 1" + " " + "AND T.Dept_Code " + Str_DeptCode + " " + "AND T.Latestrec = 1" + " " + Str_Date + Str_LibCode + Str_Type + " " + " GROUP BY T.Dept_Code";

                    DataSet DS3 = d2.select_method_wo_parameter(Sql, "Text");
                    int rcnt3 = DS3.Tables[0].Rows.Count;
                    if (rcnt3 > 0)
                    {
                        dr["Staff(Non Books)"] = Convert.ToString(DS3.Tables[0].Rows[0]["TotStaffNBook"]);

                        IntTotNBook = IntTotNBook + (Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]));
                        IntGTotal = IntGTotal + (Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]));
                        IntGTStaffNBooks = IntGTStaffNBooks + (Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]));
                    }
                    else
                    {
                        dr["Staff(Non Books)"] = "0";
                        IntTotNBook = IntTotNBook + 0;
                    }
                    IntGTotalBooks = IntGTotalBooks + IntTotBooks;
                    IntGTotalNonBooks = IntGTotalNonBooks + IntTotNBook;
                    IntGrandTotal = IntGrandTotal + IntGTotal;
                    dr["Total(Books)"] = Convert.ToString(IntTotBooks);
                    dr["Total(Non Books)"] = Convert.ToString(IntTotNBook);
                    dr["Total"] = Convert.ToString(IntGTotal);
                    cumisuue.Rows.Add(dr);
                }
            }
            dr = cumisuue.NewRow();
            dr["Department"] = "Total";
            dr["Student(Books)"] = Convert.ToString(IntGTStudBooks);
            dr["Staff(Books)"] = Convert.ToString(IntGTStaffBooks);
            dr["Total(Books)"] = Convert.ToString(IntGTotalBooks);
            dr["Student(Non Books)"] = Convert.ToString(IntGTStudNBooks);
            dr["Staff(Non Books)"] = Convert.ToString(IntGTStaffNBooks);
            dr["Total(Non Books)"] = Convert.ToString(IntGTotalNonBooks);
            dr["Total"] = Convert.ToString(IntGrandTotal);
            cumisuue.Rows.Add(dr);
            grdCumIssReport.DataSource = cumisuue;
            grdCumIssReport.DataBind();
            grdCumIssReport.Visible = true;
            print.Visible = true;
        }
        catch (Exception ex) { }
    }

    protected void grdCumIssReport_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ddlreporttype.SelectedIndex == 0 || ddlreporttype.SelectedIndex == 1)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    public void search_date()
    {
        try
        {
            int IntTotBooks = 0;
            int IntTotNBook = 0;
            int IntGTotal = 0;
            int IntGTStudBooks = 0;
            int IntGTStudNBooks = 0;
            int IntGTStaffBooks = 0;
            int IntGTStaffNBooks = 0;
            String Str_Date = string.Empty;
            String Str_LibCode = string.Empty;
            String Str_DeptCode = string.Empty;
            String Str_Type = string.Empty;
            string fdate = string.Empty;
            string tdate = string.Empty;
            int IntGTotalBooks = 0;
            int IntGTotalNonBooks = 0;
            int IntGrandTotal = 0;
            if (cbfrom.Checked == true)
            {
                fdate = txt_fromdate1.Text;
                tdate = txt_todate1.Text;
                string[] tmp1 = fdate.Split(new Char[] { '/' });
                if (tmp1.Length == 3)
                    fdate = tmp1[1] + "-" + tmp1[0] + "-" + tmp1[2];
                string[] tmp2 = tdate.Split(new Char[] { '/' });
                if (tmp2.Length == 3)
                    tdate = tmp2[1] + "-" + tmp2[0] + "-" + tmp2[2];
                Str_Date = " AND B.Borrow_Date Between '" + fdate + "' AND '" + tdate + "' ";
            }
            else
                Str_Date = "";

            if (ddl_type.Text == "Returned")
                Str_Type = " AND B.Return_Flag = 1";
            else if (ddl_type.Text == "Not Returned")
                Str_Type = " AND B.Return_Flag = 0";
            else
                Str_Type = "";

            string vallib = string.Empty;
            if (chklstlib.Items.Count > 0)
                vallib = rs.getCblSelectedValue(chklstlib);

            if (!string.IsNullOrEmpty(vallib))
                Str_LibCode = " AND B.Lib_Code IN('" + vallib + "')";
            string dep = string.Empty;
            string dep_name = string.Empty;
            if (chklstdept.Items.Count > 0)
            {
                dep = rs.GetSelectedItemsValue(chklstdept);
                dep_name = rs.GetSelectedItemsText(chklstdept);
            }
            if (!string.IsNullOrEmpty(dep))
                Str_DeptCode = " " + dep + "  ";
            IntTotBooks = 0;
            IntTotNBook = 0;
            IntGTotal = 0;
            string dept_code = string.Empty;

            cumisuue.Columns.Add("Date", typeof(string));
            cumisuue.Columns.Add("Student(Books)", typeof(string));
            cumisuue.Columns.Add("Staff(Books)", typeof(string));
            cumisuue.Columns.Add("Total(Books)", typeof(string));
            cumisuue.Columns.Add("Student(Non Books)", typeof(string));
            cumisuue.Columns.Add("Staff(Non Books)", typeof(string));
            cumisuue.Columns.Add("Total(Non Books)", typeof(string));
            cumisuue.Columns.Add("Total", typeof(string));
            for (DateTime k = Convert.ToDateTime(fdate); k <= Convert.ToDateTime(tdate); )
            {
                IntTotBooks = 0;
                IntTotNBook = 0;
                IntGTotal = 0;
                dr3 = cumisuue.NewRow();
                Sql = "SELECT COUNT(*) TotStudBook" + " " + " FROM Borrow B,Registration R,Degree G,Department D,Library L" + " " + "Where b.roll_no = r.roll_no And r.degree_code = g.degree_code" + " " + "AND G.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + " " + "AND Return_Type = 'BOK' AND B.Borrow_Date='" + k + "' AND Is_Staff = 0" + " " + "" + Str_LibCode + Str_Type;
                if (Str_DeptCode != null)
                    Sql = Sql + " " + "AND G.Dept_Code IN " + "(" + Str_DeptCode + ")";
                Sql = Sql + " " + " GROUP BY Borrow_Date";

                DataSet DS = d2.select_method_wo_parameter(Sql, "Text");
                int rcnt = DS.Tables[0].Rows.Count;
                string dateval = Convert.ToString(k);
                string[] datedt = dateval.Split(new Char[] { '/' });
                if (datedt.Length == 3)
                    dateval = datedt[1] + "-" + datedt[0] + "-" + datedt[2];
                dr3["Date"] = dateval.Split(' ')[0];
                if (rcnt > 0)
                {
                    dr3["Student(Books)"] = Convert.ToString(DS.Tables[0].Rows[0]["TotStudBook"]);
                    IntTotBooks = IntTotBooks + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                    IntGTotal = IntGTotal + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                    IntGTStudBooks = IntGTStudBooks + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                }
                else
                {
                    dr3["Student(Books)"] = "0";
                    IntTotBooks = IntTotBooks + 0;
                }
                Sql = "SELECT COUNT(*) TotStaffBook" + " " + "FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Degree G , Library L" + " " + "Where b.roll_no = M.Staff_Code And M.Staff_Code = T.Staff_Code " + "" + " AND G.Dept_Code = D.Dept_Code AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + " " + "AND Return_Type = 'BOK' AND B.Borrow_Date='" + k + "' AND Is_Staff = 1 AND T.Latestrec =1" + " " + " " + " " + Str_LibCode + Str_Type;

                if (Str_DeptCode != null)
                    Sql = Sql + " " + "AND G.Dept_Code IN " + "(" + Str_DeptCode + ")";
                Sql = Sql + " " + " GROUP BY Borrow_Date";
                DataSet DS1 = d2.select_method_wo_parameter(Sql, "Text");
                int rcnt1 = DS1.Tables[0].Rows.Count;
                if (rcnt1 > 0)
                {
                    dr3["Staff(Books)"] = Convert.ToString(DS1.Tables[0].Rows[0]["TotStaffBook"]);
                    IntTotBooks = IntTotBooks + (Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]));
                    IntGTotal = IntGTotal + (Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]));
                    IntGTStaffBooks = IntGTStaffBooks + (Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]));
                }
                else
                {
                    dr3["Staff(Books)"] = "0";
                    IntTotBooks = IntTotBooks + 0;
                }

                Sql = "SELECT COUNT(*) TotStudNBook" + " " + "FROM Borrow B,Registration R,Degree G,Department D,Library L" + " " + "Where b.roll_no = r.roll_no  And r.degree_code = g.degree_code" + " " + "AND G.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + " " + "AND Return_Type = 'NBM' AND B.Borrow_Date='" + k + "' AND Is_Staff = 0" + " " + "" + " " + Str_LibCode + Str_Type;
                if (Str_DeptCode != null)
                    Sql = Sql + " " + "AND G.Dept_Code IN " + "(" + Str_DeptCode + ")";
                Sql = Sql + " " + " GROUP BY Borrow_Date";
                DataSet DS2 = d2.select_method_wo_parameter(Sql, "Text");
                int rcnt2 = DS2.Tables[0].Rows.Count;
                if (rcnt2 > 0)
                {
                    dr3["Student(Non Books)"] = Convert.ToString(DS2.Tables[0].Rows[0]["TotStudBook"]);
                    IntTotNBook = IntTotNBook + (Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudBook"]));
                    IntGTotal = IntGTotal + (Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudBook"]));
                    IntGTStudNBooks = IntGTStudNBooks + (Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudBook"]));
                }
                else
                {
                    dr3["Student(Non Books)"] = "0";
                    IntTotNBook = IntTotNBook + 0;
                }

                Sql = "SELECT COUNT(*) TotStaffNBook" + " " + "FROM Borrow B,StaffMaster M,Degree G,StaffTrans T,HrDept_Master D,Library L" + " " + "Where b.roll_no = M.Staff_Code  And M.Staff_Code = T.Staff_Code " + " " + " AND G.Dept_Code = D.Dept_Code AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + " " + "AND Return_Type = 'NBM' AND B.Borrow_Date='" + k + "' AND Is_Staff = 1" + " " + " " + " " + Str_LibCode + Str_Type;

                if (Str_DeptCode != null)
                    Sql = Sql + " " + "AND G.Dept_Code IN " + "(" + Str_DeptCode + ")";
                Sql = Sql + " " + " GROUP BY Borrow_Date";
                DataSet DS3 = d2.select_method_wo_parameter(Sql, "Text");
                int rcnt3 = DS3.Tables[0].Rows.Count;
                if (rcnt3 > 0)
                {
                    dr3["Staff(Non Books)"] = Convert.ToString(DS3.Tables[0].Rows[0]["TotStaffNBook"]);
                    IntTotNBook = IntTotNBook + (Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]));
                    IntGTotal = IntGTotal + (Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]));
                    IntGTStaffNBooks = IntGTStaffNBooks + (Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]));
                }
                else
                {
                    dr3["Staff(Non Books)"] = "0";
                    IntTotNBook = IntTotNBook + 0;
                }
                IntGTotalBooks = IntGTotalBooks + IntTotBooks;
                IntGTotalNonBooks = IntGTotalNonBooks + IntTotNBook;
                IntGrandTotal = IntGrandTotal + IntGTotal;
                dr3["Total(Books)"] = Convert.ToString(IntTotBooks);
                dr3["Total(Non Books)"] = Convert.ToString(IntTotNBook);
                dr3["Total"] = Convert.ToString(IntGTotal);
                cumisuue.Rows.Add(dr3);
                DateTime DueDate = k.AddDays(1);
                k = DueDate;
            }
            dr = cumisuue.NewRow();
            dr["Date"] = "Total";
            dr["Student(Books)"] = Convert.ToString(IntGTStudBooks);
            dr["Staff(Books)"] = Convert.ToString(IntGTStaffBooks);
            dr["Total(Books)"] = Convert.ToString(IntGTotalBooks);
            dr["Student(Non Books)"] = Convert.ToString(IntGTStudNBooks);
            dr["Staff(Non Books)"] = Convert.ToString(IntGTStaffNBooks);
            dr["Total(Non Books)"] = Convert.ToString(IntGTotalNonBooks);
            dr["Total"] = Convert.ToString(IntGrandTotal);
            cumisuue.Rows.Add(dr);
            grdCumIssReport.DataSource = cumisuue;
            grdCumIssReport.DataBind();
            grdCumIssReport.Visible = true;
            print.Visible = true;
        }
        catch (Exception ex) { }
    }

    public void Search_Subject()
    {
        int IntTotBooks = 0;
        int IntTotNBook = 0;
        int IntGTotal = 0;
        int IntGTStudBooks = 0;
        int IntGTStudNBooks = 0;
        int IntGTStaffBooks = 0;
        int IntGTStaffNBooks = 0;
        String Str_Date = string.Empty;
        String Str_LibCode = string.Empty;
        String Str_DeptCode = string.Empty;
        String Str_Type = string.Empty;
        int IntGTotalBooks = 0;
        int IntGTotalNonBooks = 0;
        int IntGrandTotal = 0;
        try
        {
            if (cbfrom.Checked == true)
            {
                string fdate = txt_fromdate1.Text;
                string tdate = txt_todate1.Text;
                string[] tmp1 = fdate.Split(new Char[] { '/' });
                fdate = tmp1[1] + "/" + tmp1[0] + "/" + tmp1[2];
                string[] tmp2 = tdate.Split(new Char[] { '/' });
                tdate = tmp2[1] + "/" + tmp2[0] + "/" + tmp2[2];

                Str_Date = " AND B.Borrow_Date Between '" + fdate + "' AND '" + tdate + "' ";

            }
            else
                Str_Date = "";
            if (ddl_type.Text == "Returned")

                Str_Type = " AND B.Return_Flag = 1";

            else if (ddl_type.Text == "Not Returned")

                Str_Type = " AND B.Return_Flag = 0";
            else
                Str_Type = "";
            string vallib = string.Empty;
            if (chklstlib.Items.Count > 0)
                vallib = rs.getCblSelectedValue(chklstlib);

            if (!string.IsNullOrEmpty(vallib))
                Str_LibCode = " AND B.Lib_Code IN('" + vallib + "')";
            String dep = string.Empty;
            if (chklstdept.Items.Count > 0)
                dep = rs.getCblSelectedValue(chklstdept);
            if (!string.IsNullOrEmpty(dep))
                Str_DeptCode = Str_DeptCode + "('" + dep + "')";

            Sql = "SELECT DISTINCT Subject FROM BookDetails B  WHERE 1=1  " + "" + Str_LibCode;

            rsbok = d2.select_method_wo_parameter(Sql, "Text");
            cumisuue.Columns.Add("Subject", typeof(string));
            cumisuue.Columns.Add("Student(Books)", typeof(string));
            cumisuue.Columns.Add("Staff(Books)", typeof(string));
            cumisuue.Columns.Add("Total(Books)", typeof(string));
            cumisuue.Columns.Add("Student(Non Books)", typeof(string));
            cumisuue.Columns.Add("Staff(Non Books)", typeof(string));
            cumisuue.Columns.Add("Total(Non Books)", typeof(string));
            cumisuue.Columns.Add("Total", typeof(string));
            if (rsbok.Tables[0].Rows.Count > 0)
            {
                int temp = 0;
                for (int j = 0; j < rsbok.Tables[0].Rows.Count; j++)
                {
                    IntTotBooks = 0;
                    IntTotNBook = 0;
                    IntGTotal = 0;
                    dr1 = cumisuue.NewRow();
                    string sub = "";
                    String dpt_code = Convert.ToString(rsbok.Tables[0].Rows[j]["Subject"]).Trim();
                    if (dpt_code.Contains(" ' "))
                    {
                        sub = "";
                    }
                    else
                    {
                        sub = "AND Subject ='" + dpt_code + "'";
                    }
                    dr1["Subject"] = dpt_code;
                    Sql = "SELECT COUNT(*) TotStudBook" + "" + "  FROM  Borrow B,Registration R,Degree G,Department D,Library L,BookDetails M " + "" + " Where B.Acc_No = M.Acc_No AND b.roll_no = r.roll_no And r.degree_code = g.degree_code" + "" + " AND G.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + "" + " AND Return_Type = 'BOK' AND Is_Staff = 0" + " " + sub + " " + "  " + "" + Str_Date + Str_LibCode + Str_Type + "" + " GROUP BY Subject ";

                    DataSet DS = d2.select_method_wo_parameter(Sql, "Text");
                    int rcnt = DS.Tables[0].Rows.Count;
                    if (rcnt > 0)
                    {
                        dr1["Student(Books)"] = Convert.ToString(DS.Tables[0].Rows[0]["TotStudBook"]);
                        IntTotBooks = IntTotBooks + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                        IntGTotal = IntGTotal + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                        IntGTStudBooks = IntGTStudBooks + (Convert.ToInt32(DS.Tables[0].Rows[0]["TotStudBook"]));
                    }
                    else
                    {
                        dr1["Student(Books)"] = "0";
                        IntTotBooks = IntTotBooks + 0;
                    }

                    Sql = "SELECT COUNT(*) TotStaffBook" + "" + " FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L,BookDetails K " + "" + " Where B.Acc_No = K.Acc_No AND b.roll_no = M.Staff_Code  And M.Staff_Code = T.Staff_Code " + "" + " AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + "" + " AND Return_Type = 'BOK' AND Is_Staff = 1" + "" + " " + sub + " " + "" + " AND T.Latestrec = 1" + "" + Str_Date + Str_LibCode + Str_Type + "" + " GROUP BY Subject ";

                    DataSet DS1 = d2.select_method_wo_parameter(Sql, "Text");
                    int rcnt1 = DS1.Tables[0].Rows.Count;
                    if (rcnt1 > 0)
                    {
                        dr1["Staff(Books)"] = Convert.ToString(DS1.Tables[0].Rows[0]["TotStaffBook"]);
                        IntTotBooks = IntTotBooks + Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]);
                        IntGTotal = IntGTotal + Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]);
                        IntGTStaffBooks = IntGTStaffBooks + Convert.ToInt32(DS1.Tables[0].Rows[0]["TotStaffBook"]);
                    }
                    else
                    {
                        dr1["Staff(Books)"] = "0";
                        IntTotBooks = IntTotBooks + 0;
                    }
                    Sql = "SELECT COUNT(*) TotStudNBook" + "" + " FROM Borrow B,Registration R,Degree G,Department D,Library L,BookDetails M" + "" + " Where  B.Acc_No = M.Acc_No and b.roll_no = r.roll_no And r.degree_code = g.degree_code" + "" + " AND G.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + "" + " AND Return_Type = 'NBM' AND Is_Staff = 0  " + sub + "" + "" + Str_Date + Str_LibCode + Str_Type + "" + " GROUP BY G.Dept_Code,Subject";

                    DataSet DS2 = d2.select_method_wo_parameter(Sql, "Text");
                    int rcnt2 = DS2.Tables[0].Rows.Count;
                    if (rcnt2 > 0)
                    {
                        dr1["Student(Non Books)"] = Convert.ToString(DS2.Tables[0].Rows[0]["TotStudNBook"]);
                        IntTotNBook = IntTotNBook + Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudNBook"]);
                        IntGTotal = IntGTotal + Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudNBook"]);
                        IntGTStudNBooks = IntGTStudNBooks + Convert.ToInt32(DS2.Tables[0].Rows[0]["TotStudNBook"]);
                    }
                    else
                    {
                        dr1["Student(Non Books)"] = "0";
                        IntTotNBook = IntTotNBook + 0;
                    }
                    Sql = "SELECT COUNT(*) TotStaffNBook" + "" + " FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D,Library L,BookDetails bk" + "" + " Where B.Acc_No = bk.Acc_No and b.roll_no = M.Staff_Code  And M.Staff_Code = T.Staff_Code " + "" + " AND T.Dept_Code = D.Dept_Code AND B.Lib_Code = L.Lib_Code" + "" + " AND Return_Type = 'NBM' " + sub + " AND Is_Staff = 1" + "" + " AND T.Dept_Code =" + "'" + dpt_code + "'" + "" + " AND T.Latestrec = 1" + "" + Str_Date + Str_LibCode + Str_Type + "" + " GROUP BY T.Dept_Code";

                    DataSet DS3 = d2.select_method_wo_parameter(Sql, "Text");
                    int rcnt3 = DS3.Tables[0].Rows.Count;
                    if (rcnt3 > 0)
                    {
                        dr1["Staff(Non Books)"] = Convert.ToString(DS3.Tables[0].Rows[1]["TotStaffNBook"]);
                        IntTotNBook = IntTotNBook + Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]);
                        IntGTotal = IntGTotal + Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]);
                        IntGTStaffNBooks = IntGTStaffNBooks + Convert.ToInt32(DS3.Tables[0].Rows[0]["TotStaffNBook"]);
                    }
                    else
                    {
                        dr1["Staff(Non Books)"] = "0";
                        IntTotNBook = IntTotNBook + 0;
                    }
                    IntGTotalBooks = IntGTotalBooks + IntTotBooks;
                    IntGTotalNonBooks = IntGTotalNonBooks + IntTotNBook;
                    IntGrandTotal = IntGrandTotal + IntGTotal;
                    dr1["Total(Books)"] = Convert.ToString(IntTotBooks);
                    dr1["Total(Non Books)"] = Convert.ToString(IntTotNBook);
                    dr1["Total"] = Convert.ToString(IntGTotal);
                    cumisuue.Rows.Add(dr1);
                }
            }
            dr = cumisuue.NewRow();
            dr["Student(Books)"] = Convert.ToString(IntGTStudBooks);
            dr["Staff(Books)"] = Convert.ToString(IntGTStaffBooks);
            dr["Total(Books)"] = Convert.ToString(IntGTotalBooks);
            dr["Student(Non Books)"] = Convert.ToString(IntGTStudNBooks);
            dr["Staff(Non Books)"] = Convert.ToString(IntGTStaffNBooks);
            dr["Total(Non Books)"] = Convert.ToString(IntGTotalNonBooks);
            dr["Total"] = Convert.ToString(IntGrandTotal);
            cumisuue.Rows.Add(dr);
            grdCumIssReport.DataSource = cumisuue;
            grdCumIssReport.DataBind();
            grdCumIssReport.Visible = true;
            print.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    #region Print

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Cummulative Report";
            string pagename = "cumm_issued_report.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdCumIssReport, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "cumm_issued_report"); }
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdCumIssReport, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #endregion

    #region Library

    public void BindLibrary(string LibCollection)
    {
        try
        {
            chklstlib.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(lib, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstlib.DataSource = ds;
                    chklstlib.DataTextField = "lib_name";
                    chklstlib.DataValueField = "lib_code";
                    chklstlib.DataBind();
                    //chklstlib.Items.Insert(0, "All");
                }
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
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlCollege.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
            if (singleUser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + userCode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupUserCode.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
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
                        ds = d2.select_method_wo_parameter(sql, "text");
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

            BindLibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region dept

    public void Binddept()
    {
        try
        {
            Hashtable has = new Hashtable();
            chklstdept.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(College))
            {
                has.Add("collegecode", College);
                ds.Clear();
                String dep = "select distinct Dept_Name,dept_code from Department where college_code = '" + College + "'";
                ds = d2.select_method_wo_parameter(dep, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstdept.DataSource = ds;
                    chklstdept.DataTextField = "Dept_Name";
                    chklstdept.DataValueField = "dept_code";
                    chklstdept.DataBind();
                    //chklstdept.Items.Insert(0, "All");
                }
            }
        }
        catch
        {
        }

    }

    #endregion

    #region reporttype

    public void Bindreporttype()
    {
        try
        {
            //ddlreporttype.Items.Add("General");
            ddlreporttype.Items.Add("Departmentwise");
            ddlreporttype.Items.Add("Date Wise");
            ddlreporttype.Items.Add("SubjectWise");

        }
        catch
        {
        }
    }

    #endregion

    #region type

    public void Bindtype()
    {
        try
        {
            ddl_type.Items.Add("Both");
            ddl_type.Items.Add("Returned");
            ddl_type.Items.Add("Not Returned");
            //ddlreporttype.Items.Add("Lost Books");

        }
        catch
        {
        }
    }

    #endregion

    protected void cbfrom_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbfrom.Checked)
            {
                txt_fromdate1.Enabled = true;
                txt_todate1.Enabled = true;

            }
            else
            {
                txt_fromdate1.Enabled = false;
                txt_todate1.Enabled = false;
            }
        }
        catch
        {
        }

    }

    //public void loadSpread_Header()
    //{
    //    try
    //    {

    //        spreadDet1.Sheets[0].RowCount = 0;
    //        spreadDet1.Sheets[0].ColumnCount = 9;
    //        spreadDet1.CommandBar.Visible = false;
    //        spreadDet1.Sheets[0].AutoPostBack = true;
    //        spreadDet1.Sheets[0].ColumnHeader.RowCount = 1;
    //        spreadDet1.Sheets[0].RowHeader.Visible = false;
    //        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //        darkstyle.ForeColor = Color.White;
    //        spreadDet1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
    //        darkstyle.Font.Name = "Book Antiqua";
    //        darkstyle.Font.Size = FontUnit.Medium;
    //        darkstyle.HorizontalAlign = HorizontalAlign.Center;
    //        darkstyle.VerticalAlign = VerticalAlign.Middle;
    //        if (ddlreporttype.SelectedIndex == 0)
    //        {
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
    //            spreadDet1.Sheets[0].Columns[0].Width = 20;


    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
    //            //spreadDet1.Sheets[0].Columns[1].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
    //            spreadDet1.Sheets[0].Columns[1].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student(Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
    //            spreadDet1.Sheets[0].Columns[2].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff (Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
    //            spreadDet1.Sheets[0].Columns[3].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total(Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
    //            spreadDet1.Sheets[0].Columns[4].Width = 20;


    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
    //            spreadDet1.Sheets[0].Columns[5].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
    //            spreadDet1.Sheets[0].Columns[6].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Locked = true;
    //            spreadDet1.Sheets[0].Columns[7].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Locked = true;
    //            spreadDet1.Sheets[0].Columns[8].Width = 20;

    //        }
    //        else if (ddlreporttype.SelectedIndex == 1)
    //        {
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
    //            spreadDet1.Sheets[0].Columns[0].Width = 20;


    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
    //            //spreadDet1.Sheets[0].Columns[1].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
    //            spreadDet1.Sheets[0].Columns[1].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student(Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
    //            spreadDet1.Sheets[0].Columns[2].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff (Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
    //            spreadDet1.Sheets[0].Columns[3].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total(Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
    //            spreadDet1.Sheets[0].Columns[4].Width = 20;


    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
    //            spreadDet1.Sheets[0].Columns[5].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
    //            spreadDet1.Sheets[0].Columns[6].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Locked = true;
    //            spreadDet1.Sheets[0].Columns[1].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Locked = true;
    //            spreadDet1.Sheets[0].Columns[1].Width = 20;



    //        }
    //        else if (ddlreporttype.SelectedIndex == 2)
    //        {


    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
    //            spreadDet1.Sheets[0].Columns[0].Width = 20;


    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
    //            //spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
    //            //spreadDet1.Sheets[0].Columns[1].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
    //            spreadDet1.Sheets[0].Columns[1].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student(Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
    //            spreadDet1.Sheets[0].Columns[2].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff (Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
    //            spreadDet1.Sheets[0].Columns[3].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total(Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
    //            spreadDet1.Sheets[0].Columns[4].Width = 20;


    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
    //            spreadDet1.Sheets[0].Columns[5].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
    //            spreadDet1.Sheets[0].Columns[6].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total(Non Books)";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Locked = true;
    //            spreadDet1.Sheets[0].Columns[7].Width = 20;

    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total";
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Left;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].VerticalAlign = VerticalAlign.Bottom;
    //            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Locked = true;
    //            spreadDet1.Sheets[0].Columns[8].Width = 20;

    //        }

    //        FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
    //        //int sno = 0;
    //        spreadDet1.Sheets[0].Rows.Count++;//added by rajasekar 22/06/2018
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].CellType = txtCell;
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].CellType = txtCell;
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].CellType = txtCell;
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].CellType = txtCell;
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].CellType = txtCell;
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 6].CellType = txtCell;
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 7].CellType = txtCell;
    //        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 8].CellType = txtCell;
    //       // spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 9].CellType = txtCell;

    //        spreadDet1.SaveChanges();
    //        spreadDet1.Visible = false;
    //    }
    //    catch
    //    {
    //    }
    //}

    //#region Assign_dept
    //public void Assign_dept()
    //{
    //    if (txtdep.Text != null || txtdep.Text!= "---Select---")
    //    {
    //        int count = 0;
    //        for (count = 0; count < chklstdept.Items.Count; count++)
    //        {

    //            if (chklstdept.Items[count].Selected == true)
    //            {
    //                   spreadDet1.Sheets[0].RowCount++;
    //                   spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].Text = chklstdept.Items[count].ToString();
    //                   spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
    //            }
    //        }
    //    }
    //    else
    //        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "('Please select Department')", true); 

    //}
    //#endregion

    protected void chksdept_CheckedChanged(object sender, EventArgs e)
    {
        if (chksdept.Checked == true)
        {
            for (int i = 0; i < chklstdept.Items.Count; i++)
            {
                chklstdept.Items[i].Selected = true;
            }
            txtdep.Text = "Department(" + (chklstdept.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklstdept.Items.Count; i++)
            {
                chklstdept.Items[i].Selected = false;
            }
            txtdep.Text = "---Select---";
        }
    }

    protected void chklstdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        department.Focus();
        chksdept.Checked = false;
        txtdep.Text = "---Select---";
        int deptcnt = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstdept.Items.Count; i++)
        {
            if (chklstdept.Items[i].Selected == true)
            {
                value = chklstdept.Items[i].Text;
                code = chklstdept.Items[i].Value.ToString();
                deptcnt = deptcnt + 1;
            }
        }
        if (deptcnt > 0)
        {
            txtdep.Text = "Department(" + deptcnt.ToString() + ")";
            if (deptcnt == chklstdept.Items.Count)
            {
                chksdept.Checked = true;
            }
        }
        dept_count = deptcnt;
        //BindTest(strbatch, strbranch);
    }

    protected void chklstlib_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddllibrary.Focus();
        chklib.Checked = false;

        txtlib.Text = "---Select---";
        int libcnt = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstlib.Items.Count; i++)
        {
            if (chklstlib.Items[i].Selected == true)
            {
                value = chklstlib.Items[i].Text;
                code = chklstlib.Items[i].Value.ToString();
                libcnt = libcnt + 1;
            }
        }
        if (libcnt > 0)
        {
            txtlib.Text = "Library(" + libcnt.ToString() + ")";
            if (libcnt == chklstlib.Items.Count)
            {
                chklib.Checked = true;
            }
        }
        lib_count = libcnt;
        //BindTest(strbatch, strbranch);
    }

    protected void chklib_CheckedChanged(object sender, EventArgs e)
    {
        if (chklib.Checked == true)
        {
            for (int i = 0; i < chklstlib.Items.Count; i++)
            {
                chklstlib.Items[i].Selected = true;
            }
            txtlib.Text = "Library(" + (chklstlib.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklstlib.Items.Count; i++)
            {
                chklstlib.Items[i].Selected = false;
            }
            txtlib.Text = "---Select---";
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }

        catch (Exception ex)
        {

        }
    }
}