using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class HourWise_PayProcess : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    InsproDirectAccess DirAccess = new InsproDirectAccess();
    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    FarPoint.Web.Spread.CheckBoxCellType CheckAll = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType CheckInd = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.DoubleCellType DoubleHrs = new FarPoint.Web.Spread.DoubleCellType();
    FarPoint.Web.Spread.DoubleCellType DoubleAmnt = new FarPoint.Web.Spread.DoubleCellType();

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
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
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

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }

            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                binddept();
                binddesig();
            }
            lblMainErr.Visible = false;
            lblsmserror.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        binddept();
        binddesig();
        Fpspread1.Visible = false;
        rprint.Visible = false;
        btnSave.Visible = false;
        lblMainErr.Visible = true;
    }

    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
        binddesig();
    }

    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
        binddesig();
    }

    protected void cbDesig_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbDesig, cblDesig, txtDesig, "Designation");
    }

    protected void cblDesig_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbDesig, cblDesig, txtDesig, "Designation");
    }

    protected void chkCommon_Change(object sender, EventArgs e)
    {
        txt_TotHrs.Text = "";
        txt_AmntHrs.Text = "";
        if (chkCommon.Checked)
        {
            lblHrs.Visible = true;
            txt_TotHrs.Visible = true;
            lblAmntHrs.Visible = true;
            txt_AmntHrs.Visible = true;
        }
        else
        {
            lblHrs.Visible = false;
            txt_TotHrs.Visible = false;
            lblAmntHrs.Visible = false;
            txt_AmntHrs.Visible = false;
        }
    }
    protected void chkstaff_Change(object sender, EventArgs e)
    {
       
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string DeptCode = string.Empty;
            string DesigCode = string.Empty;
            string MyDeptCode = string.Empty;
            string MyDesigCode = string.Empty;
            DeptCode = GetSelectedItemsValueAsString(cbl_dept);
            DesigCode = GetSelectedItemsValueAsString(cblDesig);

            if (String.IsNullOrEmpty(DeptCode))
            {
                Fpspread1.Visible = false;
                rprint.Visible = false;
                btnSave.Visible = false;
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Department!";
                return;
            }

            if (String.IsNullOrEmpty(DesigCode))
            {
                Fpspread1.Visible = false;
                rprint.Visible = false;
                btnSave.Visible = false;
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Designation!";
                return;
            }
            if (chkCommon.Checked)
            {
                if (String.IsNullOrEmpty(txt_TotHrs.Text.Trim()))
                {
                    Fpspread1.Visible = false;
                    rprint.Visible = false;
                    btnSave.Visible = false;
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Please Enter No.of Hours!";
                    return;
                }
                if (String.IsNullOrEmpty(txt_AmntHrs.Text.Trim()))
                {
                    Fpspread1.Visible = false;
                    rprint.Visible = false;
                    btnSave.Visible = false;
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Please Enter Amount/Hrs!";
                    return;
                }
            }
            MyDeptCode = "'" + DeptCode + "'";
            MyDesigCode = "'" + DesigCode + "'";
            if (chkstaff.Checked == true) /* poomalar 16.10.17 */
            {
                loadheaderstaffwise(MyDeptCode, MyDesigCode);
            }
            else
            {
                LoadHeader(MyDeptCode, MyDesigCode);
            }
        }
        catch { }
    }

    private void LoadHeader(string DeptCode, string DesigCode)
    {
        try
        {
            Fpspread1.Visible = false;
            rprint.Visible = false;
            lblMainErr.Visible = false;
            string collCode = Convert.ToString(ddlcollege.SelectedItem.Value);

            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 6;

            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Bold = true;
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.ForeColor = Color.Black;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpspread1.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "College";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Designation";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Hours";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Amount/Hrs";

            Fpspread1.Columns[0].Width = 75;
            Fpspread1.Columns[1].Width = 75;
            Fpspread1.Columns[2].Width = 250;
            Fpspread1.Columns[3].Width = 150;
            Fpspread1.Columns[4].Width = 75;
            Fpspread1.Columns[5].Width = 125;

            Fpspread1.Columns[0].Locked = true;
            Fpspread1.Columns[2].Locked = true;
            Fpspread1.Columns[3].Locked = true;
            Fpspread1.Columns[4].Locked = false;

            CheckAll.AutoPostBack = true;
            CheckInd.AutoPostBack = false;
            DoubleHrs.MaximumValue = 70;
            DoubleHrs.ErrorMessage = "Allow only Numerics & Max Hours is 70!";
            DoubleAmnt.ErrorMessage = "Allow only Numerics!";

            string selquery = "";

            //selquery = "select t.desig_code,t.dept_code,count(s.staff_code)as count from staffmaster s,stafftrans  t where s.staff_code =t.staff_code and t.latestrec ='1' and resign=0 and settled =0 and isnull(Discontinue,'0') ='0'  and s.college_code ='" + collCode + "' group by t.desig_code,t.dept_code";
            selquery = " select dept_code,desig_code,Tot_Hrs,Amnt_Per_Hrs from HourWise_PaySettings where college_code ='" + collCode + "' and isnull(PayType,0)='0' "; /* poomalar 23.10.17*/
            DataSet dsnew = new DataSet();
            dsnew.Clear();
            dsnew = d2.select_method_wo_parameter(selquery, "Text");
            int sno = 1;
            int rowcount = 0;
            if (cbl_dept.Items.Count > 0 && txt_dept.Text.Trim() != "--Select--")
            {
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = CheckAll;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;

                for (int st = 0; st < cbl_dept.Items.Count; st++)
                {
                    if (cbl_dept.Items[st].Selected == true)
                    {
                        rowcount = 0;
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dept.Items[st].Text);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 6);

                        string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dept.Items[st].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dept.Items[st].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dept.Items[st].Value) + "') or (dept_code='" + Convert.ToString(cbl_dept.Items[st].Value) + "'))";

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                            {
                                for (int jk = 0; jk < cblDesig.Items.Count; jk++)
                                {
                                    if (cblDesig.Items[jk].Selected == true)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[ik]["desig_code"]) == Convert.ToString(cblDesig.Items[jk].Value))
                                        {
                                            rowcount++;
                                            Fpspread1.Sheets[0].RowCount++;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = CheckInd;

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ddlcollege.SelectedItem.Text);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(cblDesig.Items[jk].Text);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(cblDesig.Items[jk].Value);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(cbl_dept.Items[st].Value);
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = DoubleHrs;
                                            if (dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                                            {
                                                DataView dvnew = new DataView();
                                                dsnew.Tables[0].DefaultView.RowFilter = " dept_code='" + Convert.ToString(cbl_dept.Items[st].Value) + "' and desig_code='" + Convert.ToString(cblDesig.Items[jk].Value) + "'";
                                                dvnew = dsnew.Tables[0].DefaultView;
                                                if (dvnew.Count > 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvnew[0]["Tot_Hrs"]);
                                                }
                                                else
                                                {
                                                    if (chkCommon.Checked && !String.IsNullOrEmpty(txt_TotHrs.Text.Trim()))
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(txt_TotHrs.Text.Trim());
                                                    else
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = "0";
                                                }
                                            }
                                            else
                                            {
                                                if (chkCommon.Checked && !String.IsNullOrEmpty(txt_TotHrs.Text.Trim()))
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(txt_TotHrs.Text.Trim());
                                                else
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = "0";
                                            }
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = DoubleAmnt;
                                            if (dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                                            {
                                                DataView dvmynew = new DataView();
                                                dsnew.Tables[0].DefaultView.RowFilter = " Dept_Code='" + Convert.ToString(cbl_dept.Items[st].Value) + "' and Desig_Code='" + Convert.ToString(cblDesig.Items[jk].Value) + "'";
                                                dvmynew = dsnew.Tables[0].DefaultView;
                                                if (dvmynew.Count > 0)
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvmynew[0]["Amnt_Per_Hrs"]);
                                                }
                                                else
                                                {
                                                    if (chkCommon.Checked && !String.IsNullOrEmpty(txt_AmntHrs.Text.Trim()))
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(txt_AmntHrs.Text.Trim());
                                                    else
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = "0";
                                                }
                                            }

                                            else
                                            {
                                                if (chkCommon.Checked && !String.IsNullOrEmpty(txt_AmntHrs.Text.Trim()))
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(txt_AmntHrs.Text.Trim());
                                                else
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = "0";
                                            }
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                                        }

                                    }
                                }
                            }
                        }




                        else
                        {
                            Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Remove();
                        }
                    }
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Visible = true;
                rprint.Visible = true;
                btnSave.Visible = true;
                lblMainErr.Visible = false;
            }
        }
        catch { }
    }

    private void loadheaderstaffwise(string DeptCode, string DesigCode) /* poomalar 16.10.17 */
    {
        try
        {

            Fpspread1.Visible = false;
            rprint.Visible = false;
            lblMainErr.Visible = false;
            string collCode = Convert.ToString(ddlcollege.SelectedItem.Value);

            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 7;

            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Bold = true;
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.ForeColor = Color.Black;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpspread1.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Code";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Hours";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Amount/Hrs";


            Fpspread1.Columns[0].Width = 50;
            Fpspread1.Columns[1].Width = 50;
            Fpspread1.Columns[2].Width = 200;
            Fpspread1.Columns[3].Width = 100;
            Fpspread1.Columns[4].Width = 200;
            Fpspread1.Columns[5].Width = 100;
            Fpspread1.Columns[6].Width = 100;


            Fpspread1.Columns[0].Locked = true;
            Fpspread1.Columns[2].Locked = true;
            Fpspread1.Columns[3].Locked = true;
            Fpspread1.Columns[4].Locked = true;

            CheckAll.AutoPostBack = true;
            CheckInd.AutoPostBack = false;
            DoubleHrs.MaximumValue = 70;
            DoubleHrs.ErrorMessage = "Allow only Numerics & Max Hours is 70!";
            DoubleAmnt.ErrorMessage = "Allow only Numerics!";
            string query = "";
            ReuasableMethods rs = new ReuasableMethods();
            string deptcodesel = rs.GetSelectedItemsValueAsString(cbl_dept);
            string desigcodesel = rs.GetSelectedItemsValueAsString(cblDesig);

            query = "select sm.staff_code,sm.staff_name,h.dept_name,desig.desig_name,st.stftype,sc.category_name,h.dept_code,desig.desig_code,sm.college_Code,sm.appl_no,isnull(Tot_Hrs,0) Tot_Hrs,isnull(Amnt_Per_Hrs,0) Amnt_Per_Hrs from staffmaster sm,hrdept_master h,desig_master desig,staffcategorizer sc,stafftrans st LEFT JOIN HourWise_PaySettings HW ON st.staff_code=hw.StaffCode and hw.dept_code=st.dept_code and hw.desig_code=hw.desig_code  where sm.staff_code=st.staff_code and sm.college_code=h.college_code and sm.college_code=desig.collegeCode and sm.college_code=sc.college_code and (isnull(st.stfnature,0)='1' or isnull(st.stfnature,'')='part') and st.dept_code=h.dept_code and st.desig_code=desig.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code='" + collCode + "' and h.dept_code in('" + deptcodesel + "') and desig.desig_code in('" + desigcodesel + "')";// and isnull(hw.PayType,0)='"+paytype+"'";
            DataSet dquery = new DataSet();


            dquery = d2.select_method_wo_parameter(query, "Text");
            int sno = 1;
            int rowcount = 0;
            if (dquery.Tables.Count > 0 && dquery.Tables[0].Rows.Count > 0)
            {

                if (cbl_dept.Items.Count > 0 && txt_dept.Text.Trim() != "--Select--")
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = CheckAll;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;

                    for (int st = 0; st < cbl_dept.Items.Count; st++)
                    {
                        if (cbl_dept.Items[st].Selected == true)
                        {
                            rowcount = 0;
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dept.Items[st].Text);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 7);

                            DataView dv = new DataView();
                            dquery.Tables[0].DefaultView.RowFilter = " dept_code='" + Convert.ToString(cbl_dept.Items[st].Value) + "'";
                            dv = dquery.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                for (int i = 0; i < dv.Count; i++)
                                {
                                    rowcount++;
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = CheckInd;

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[i]["staff_name"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[i]["staff_code"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[i]["desig_name"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dv[i]["desig_code"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(dv[i]["dept_code"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = DoubleHrs;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[i]["Tot_Hrs"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[i]["Amnt_Per_Hrs"]);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                } 
                            }
                            else
                            {
                                
                                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Remove();
                            }
                        }
                    }

                   
                }
            }
            else
            {
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Remove();
            }

            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Visible = true;
            rprint.Visible = true;
            btnSave.Visible = true;
            lblMainErr.Visible = false;

        }
        catch (Exception e)
        {
            e.ToString();
        }
    }


    protected void Fpspread1_command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[0, 1].Value);
            if (Check == 1)
            {
                for (int spr = 1; spr < Fpspread1.Sheets[0].RowCount; spr++)
                {
                    Fpspread1.Sheets[0].Cells[spr, 1].Value = 1;
                }
            }
            else
            {
                for (int spr = 1; spr < Fpspread1.Sheets[0].RowCount; spr++)
                {
                    Fpspread1.Sheets[0].Cells[spr, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string InsQ = "";
            int MyUpdCount = 0;
            string ClgCode = Convert.ToString(ddlcollege.SelectedValue);
            string Dept_Code = string.Empty;
            string Desig_Code = string.Empty;
            string staffcode = "";
            double Tot_Hrs = 0;
            double AmntPerHrs = 0;
            string myErrTxt = string.Empty;
            if (CheckSpr())
            {
                if (CheckSprVal(ref myErrTxt))
                {
                    Fpspread1.SaveChanges();
                    for (int myVal = 1; myVal < Fpspread1.Sheets[0].RowCount; myVal++)
                    {
                        InsQ = "";
                        Dept_Code = string.Empty;
                        Desig_Code = string.Empty;
                        Tot_Hrs = 0;
                        AmntPerHrs = 0;
                        byte paytype = 0;

                        byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[myVal, 1].Value);
                        if (Check == 1)
                        {
                            /* poomalar 16.10.17*/
                            if (chkstaff.Checked == false)
                            {
                                Dept_Code = Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 3].Note);
                                Desig_Code = Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 3].Tag);
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 4].Text), out Tot_Hrs);
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 5].Text), out AmntPerHrs);
                                paytype = 0;
                            }
                            if (chkstaff.Checked == true)
                            {
                                staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 3].Text);
                                Dept_Code = Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 4].Note);
                                Desig_Code = Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 4].Tag);
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 5].Text), out Tot_Hrs);
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 6].Text), out AmntPerHrs);
                                paytype = 1;
                            }
                            
                            if (!String.IsNullOrEmpty(Dept_Code) && !String.IsNullOrEmpty(Desig_Code) && Tot_Hrs != 0 && AmntPerHrs != 0)
                            {  /* poomalar 23.10.17*/
                                if (chkstaff.Checked == true)
                                {
                                    InsQ = "if exists (select * from HourWise_PaySettings where college_code='" + ClgCode + "' and dept_code='" + Dept_Code + "' and desig_code='" + Desig_Code + "' and Staffcode='" + staffcode + "') update HourWise_PaySettings set Tot_Hrs='" + Tot_Hrs + "',Amnt_Per_Hrs='" + AmntPerHrs + "'where college_code='" + ClgCode + "' and dept_code='" + Dept_Code + "' and desig_code='" + Desig_Code + "' and Staffcode='" + staffcode + "' and PayType='" + paytype + "'  else insert into HourWise_PaySettings (dept_code,desig_code,Tot_Hrs,Amnt_Per_Hrs,college_code,Staffcode,PayType) values ('" + Dept_Code + "','" + Desig_Code + "','" + Tot_Hrs + "','" + AmntPerHrs + "','" + ClgCode + "','" + staffcode + "','" + paytype + "')";
                                }
                                else
                                {
                                    InsQ = "if exists (select * from HourWise_PaySettings where college_code='" + ClgCode + "' and dept_code='" + Dept_Code + "' and desig_code='" + Desig_Code + "') update HourWise_PaySettings set Tot_Hrs='" + Tot_Hrs + "',Amnt_Per_Hrs='" + AmntPerHrs + "'where college_code='" + ClgCode + "' and dept_code='" + Dept_Code + "' and desig_code='" + Desig_Code + "' and PayType='" + paytype + "' else insert into HourWise_PaySettings (dept_code,desig_code,Tot_Hrs,Amnt_Per_Hrs,college_code,PayType) values ('" + Dept_Code + "','" + Desig_Code + "','" + Tot_Hrs + "','" + AmntPerHrs + "','" + ClgCode + "','" + paytype + "')";
                                }
                                int insCount = d2.update_method_wo_parameter(InsQ, "Text");
                                if (insCount > 0)
                                {
                                    MyUpdCount += 1;
                                }
                            }
                        }
                    }
                    if (MyUpdCount > 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully!";
                        btnGo_Click(sender, e);
                    }
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = myErrTxt;
                }
            }
            else /* poomalar 23.10.17*/
            {
                if (chkstaff.Checked == true)
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Please Select Any Staff!";
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Please Select Any Designation!";
                }
            }
        }
        catch { }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    private bool CheckSpr()
    {
        bool EntryFlag = false;
        try
        {
            Fpspread1.SaveChanges();
            for (int mySpr = 1; mySpr < Fpspread1.Sheets[0].RowCount; mySpr++)
            {
                byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[mySpr, 1].Value);
                if (Check == 1)
                    EntryFlag = true;
            }
        }
        catch { }
        return EntryFlag;
    }

    private bool CheckSprVal(ref string ErrText)
    {
        bool CheckFlag = true;
        try
        {
            string ClgCode = Convert.ToString(ddlcollege.SelectedValue);
            string Dept_Code = string.Empty;
            string Desig_Code = string.Empty;
            string DesigName = string.Empty;
            string DeptName = string.Empty;
            double tothrs = 0;
            double Amnthrs = 0;
            Fpspread1.SaveChanges();
            for (int mySpr = 1; mySpr < Fpspread1.Sheets[0].RowCount; mySpr++)
            {
                DesigName = string.Empty;
                DeptName = string.Empty;
                Dept_Code = string.Empty;
                Desig_Code = string.Empty;
                tothrs = 0;
                Amnthrs = 0;

                byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[mySpr, 1].Value);
                if (Check == 1)
                {
                    DesigName = Convert.ToString(Fpspread1.Sheets[0].Cells[mySpr, 3].Text);
                    Dept_Code = Convert.ToString(Fpspread1.Sheets[0].Cells[mySpr, 3].Note);
                    DeptName = d2.GetFunction("select Dept_Name from hrdept_master where Dept_Code='" + Dept_Code + "' and college_code='" + ClgCode + "'");
                    Desig_Code = Convert.ToString(Fpspread1.Sheets[0].Cells[mySpr, 3].Tag);
                    double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[mySpr, 4].Text), out tothrs);
                    double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[mySpr, 5].Text), out Amnthrs);

                    if (!String.IsNullOrEmpty(Dept_Code.Trim()) && !String.IsNullOrEmpty(Desig_Code.Trim()))
                    {
                        if (tothrs == 0)
                        {
                            ErrText = "Please Enter No.of Hours for '" + DesigName + "' Department of '" + DeptName + "'!";
                            CheckFlag = false;
                            return CheckFlag;
                        }
                        if (Amnthrs == 0)
                        {
                            ErrText = "Please Enter Amount/Hours for '" + DesigName + "' Department of '" + DeptName + "'!";
                            CheckFlag = false;
                            return CheckFlag;
                        }
                    }
                }
            }
        }
        catch { }
        return CheckFlag;
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Text = "Please Enter Your Report Name";
                lblsmserror.Visible = true;
                txtexcel.Focus();
            }
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string dptname = "HourWise - Payprocess Settings";
        string pagename = "HourWise_PayProcess.aspx";
        Printcontrol.loadspreaddetails(Fpspread1, pagename, dptname);
        Printcontrol.Visible = true;
    }

    private void bindcollege()
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
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }

    private void binddept()
    {
        cbl_dept.Items.Clear();
        txt_dept.Text = "--Select--";
        cb_dept.Checked = false;
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string selqry = "select Dept_Code,Dept_Name FROM hrdept_master where college_code='" + collcode + "' order by Dept_Name";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_dept.DataSource = ds;
            cbl_dept.DataTextField = "Dept_Name";
            cbl_dept.DataValueField = "Dept_Code";
            cbl_dept.DataBind();

            if (cbl_dept.Items.Count > 0)
            {
                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = "Department (" + cbl_dept.Items.Count + ")";
                cb_dept.Checked = true;
            }
        }
        binddesig();
    }

    private void binddesig()
    {
        cblDesig.Items.Clear();
        txtDesig.Text = "--Select--";
        cbDesig.Checked = false;
        Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
        dicgetcode.Clear();
        Dictionary<string, string> dicdescode = new Dictionary<string, string>();
        dicdescode.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        if (cbl_dept.Items.Count > 0)
        {
            for (int ik = 0; ik < cbl_dept.Items.Count; ik++)
            {
                if (cbl_dept.Items[ik].Selected == true)
                {
                    if (!dicgetcode.ContainsKey(Convert.ToString(cbl_dept.Items[ik].Value)))
                    {
                        string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dept.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dept.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dept.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbl_dept.Items[ik].Value) + "'))";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                            {
                                if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])))
                                {
                                    cblDesig.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])));
                                    dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]));
                                }
                            }
                        }
                        dicgetcode.Add(Convert.ToString(cbl_dept.Items[ik].Value), Convert.ToString(cbl_dept.Items[ik].Text));
                    }
                }
            }
        }
        if (cblDesig.Items.Count > 0)
        {
            for (int i = 0; i < cblDesig.Items.Count; i++)
            {
                cblDesig.Items[i].Selected = true;
            }
            txtDesig.Text = "Designation (" + cblDesig.Items.Count + ")";
            cbDesig.Checked = true;
        }
    }

    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Text));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + " (" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + " (" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }
    protected void btn_convenesexpClick(object sender, EventArgs e)
    {
        divconvenesexp.Visible = true;
    }
    protected void btn_convenesSaveClick(object sender, EventArgs e)
    {
        string Qry = " if exists (select * from new_inssettings where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and linkName='Parttime Staff Convenes Expance' and user_code='" + usercode + "')update new_inssettings set linkvalue='" + txt_conexp.Text + "',user_code='" + usercode + "' where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and linkName='Parttime Staff Convenes Expance' and user_code='" + usercode + "' else insert into new_inssettings (linkname,linkvalue,user_code,college_code) values('Parttime Staff Convenes Expance','" + txt_conexp.Text + "','" + usercode + "','" + Convert.ToString(ddlcollege.SelectedItem.Value) + "')";
        int insert = d2.update_method_wo_parameter(Qry, "text");
        if (insert != 0)
        {
            btn_convenesexitClick(sender, e);
        }
    }
    protected void btn_convenesexitClick(object sender, EventArgs e)
    {
        divconvenesexp.Visible = false;
    }
}