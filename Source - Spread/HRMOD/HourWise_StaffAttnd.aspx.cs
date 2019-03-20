using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class HourWise_StaffAttnd : System.Web.UI.Page
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
                loadstafftype();
                loadcategory();
                txtFrmDt.Attributes.Add("readonly", "readonly");
                txtToDt.Attributes.Add("readonly", "readonly");
                txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");

                calFrmDt.EndDate = DateTime.Now;
                calToDt.EndDate = DateTime.Now;
            }
            lblMainErr.Visible = false;
            lblsmserror.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void txtFrmDt_Change(object sender, EventArgs e)
    {
        if (GetDayFrstDate(txtFrmDt.Text.Trim()) > GetDayFrstDate(txtToDt.Text.Trim()))
        {
            txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblMainErr.Visible = true;
            lblMainErr.Text = "From Date Should be less than or equal to To Date!";
            Fpspread1.Visible = false;
            lblNote.Visible = false;
            btnSave.Visible = false;
            rprint.Visible = false;
        }
    }

    protected void txtToDt_Change(object sender, EventArgs e)
    {
        if (GetDayFrstDate(txtFrmDt.Text.Trim()) > GetDayFrstDate(txtToDt.Text.Trim()))
        {
            txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblMainErr.Visible = true;
            lblMainErr.Text = "To Date Should be Greater than or equal to From Date!";
            Fpspread1.Visible = false;
            lblNote.Visible = false;
            btnSave.Visible = false;
            rprint.Visible = false;
        }
    }

    private DateTime GetDayFrstDate(string Date)
    {
        DateTime MyDt = new DateTime();
        try
        {
            string[] splDt = Date.Split('/');
            MyDt = Convert.ToDateTime(splDt[1] + "/" + splDt[0] + "/" + splDt[2]);
        }
        catch { }
        return MyDt;
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        binddept();
        binddesig();
        loadstafftype();
        loadcategory();
        lblMainErr.Visible = false;
        Fpspread1.Visible = false;
        lblNote.Visible = false;
        btnSave.Visible = false;
        rprint.Visible = false;
    }

    protected void cbDept_Change(object sender, EventArgs e)
    {
        chkchange(cbDept, cblDept, txtDept, "Department");
    }

    protected void cblDept_Change(object sender, EventArgs e)
    {
        chklstchange(cbDept, cblDept, txtDept, "Department");
    }

    protected void cbDesig_Change(object sender, EventArgs e)
    {
        chkchange(cbDesig, cblDesig, txtDesig, "Designation");
    }

    protected void cblDesig_Change(object sender, EventArgs e)
    {
        chklstchange(cbDesig, cblDesig, txtDesig, "Designation");
    }

    protected void cbStfType_Change(object sender, EventArgs e)
    {
        chkchange(cbStfType, cblStfType, txtStfType, "Staff Type");
    }

    protected void cblStfType_Change(object sender, EventArgs e)
    {
        chklstchange(cbStfType, cblStfType, txtStfType, "Staff Type");
    }

    protected void cbStfCat_Change(object sender, EventArgs e)
    {
        chkchange(cbStfCat, cblStfCat, txtStfCat, "Staff Category");
    }

    protected void cblStfCat_Change(object sender, EventArgs e)
    {
        chklstchange(cbStfCat, cblStfCat, txtStfCat, "Staff Category");
    }

    protected void Fpspread1_command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[0, 1].Value);
            if (Check == 1)
            {
                for (int s = 0; s < Fpspread1.Sheets[0].Rows.Count; s++)
                {
                    Fpspread1.Sheets[0].Cells[s, 1].Value = 1;
                }
            }
            else
            {
                for (int s = 0; s < Fpspread1.Sheets[0].Rows.Count; s++)
                {
                    Fpspread1.Sheets[0].Cells[s, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            LoadHeader();
        }
        catch { }
    }

    private void LoadHeader()
    {
        lblNote.Visible = false;
        Fpspread1.Visible = false;
        rprint.Visible = false;
        lblMainErr.Visible = false;
        btnSave.Visible = false;

        string collCode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string DeptCode = string.Empty;
        string DesigCode = string.Empty;
        string StfType = string.Empty;
        string StfCat = string.Empty;
        string MyDeptCode = string.Empty;
        string MyDesigCode = string.Empty;
        string MyStfType = string.Empty;
        string MyStfCat = string.Empty;

        DataView dvnew = new DataView();
        DataView dvnew1 = new DataView();
        DeptCode = GetSelectedItemsValueAsString(cblDept);
        DesigCode = GetSelectedItemsValueAsString(cblDesig);
        StfType = GetSelectedItemsText(cblStfType);
        StfCat = GetSelectedItemsValueAsString(cblStfCat);

        MyDeptCode = "'" + DeptCode + "'";
        MyDesigCode = "'" + DesigCode + "'";
        MyStfType = "'" + StfType + "'";
        MyStfCat = "'" + StfCat + "'";

        DateTime dtFrm = GetDayFrstDate(txtFrmDt.Text.Trim());
        DateTime dtTo = GetDayFrstDate(txtToDt.Text.Trim());
        DateTime dtTempFrm = dtFrm;
        DateTime dtTempTo = dtTo;// delsi0606 added or isnull(st.stfnature,'')='part')
        string SelQ = "select Tot_Hrs,Amnt_Per_Hrs,dept_code,desig_Code,college_code from HourWise_PaySettings where dept_code in(" + MyDeptCode + ") and desig_code in(" + MyDesigCode + ") and college_code='" + collCode + "'";
        SelQ = SelQ + " select sm.staff_code,sm.staff_name,h.dept_name,desig.desig_name,st.stftype,sc.category_name,h.dept_code,desig.desig_code,sm.college_Code,sm.appl_no from staffmaster sm,stafftrans st,hrdept_master h,desig_master desig,staffcategorizer sc where sm.staff_code=st.staff_code and sm.college_code=h.college_code and sm.college_code=desig.collegeCode and sm.college_code=sc.college_code and (isnull(st.stfnature,0)='1' or isnull(st.stfnature,'')='part') and st.dept_code=h.dept_code and st.desig_code=desig.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code='" + collCode + "'";
        if (!String.IsNullOrEmpty(DeptCode))
            SelQ = SelQ + " and h.dept_code in(" + MyDeptCode + ")";
        if (!String.IsNullOrEmpty(DesigCode))
            SelQ = SelQ + " and desig.desig_code in(" + MyDesigCode + ")";
        if (!String.IsNullOrEmpty(StfType))
            SelQ = SelQ + " and st.stftype in(" + MyStfType + ")";
        if (!String.IsNullOrEmpty(StfCat))
            SelQ = SelQ + " and sc.category_code in(" + MyStfCat + ")";
        SelQ = SelQ + " select Appl_ID,Staff_Code,WorkingDate,WorkingHour from Hour_Staff_Attnd where MONTH(WorkingDate) in('" + dtFrm.Month + "','" + dtTo.Month + "') and YEAR(WorkingDate) in('" + dtFrm.Year + "','" + dtTo.Year + "')";
        ds.Clear();
        ds = d2.select_method_wo_parameter(SelQ, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {
            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 2;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 9;

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
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Designation";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff Type";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Staff Category";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Hours";

            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

            Fpspread1.Columns[0].Width = 75;
            Fpspread1.Columns[1].Width = 75;
            Fpspread1.Columns[2].Width = 150;
            Fpspread1.Columns[3].Width = 200;
            Fpspread1.Columns[4].Width = 180;
            Fpspread1.Columns[5].Width = 180;
            Fpspread1.Columns[6].Width = 150;
            Fpspread1.Columns[7].Width = 150;
            Fpspread1.Columns[8].Width = 75;

            Fpspread1.Columns[0].Locked = true;
            Fpspread1.Columns[2].Locked = true;
            Fpspread1.Columns[3].Locked = true;
            Fpspread1.Columns[4].Locked = true;
            Fpspread1.Columns[5].Locked = true;
            Fpspread1.Columns[6].Locked = true;
            Fpspread1.Columns[7].Locked = true;
            Fpspread1.Columns[8].Locked = true;

            while (dtTempFrm <= dtTempTo)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dtTempFrm.ToString("dd/MM/yyyy"));
                dtTempFrm = dtTempFrm.AddDays(1);
            }
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Working Hours";
            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, Fpspread1.Sheets[0].ColumnCount - 9);
            dtTempFrm = dtFrm;
            dtTempTo = dtTo;

            CheckAll.AutoPostBack = true;
            CheckInd.AutoPostBack = false;
            DoubleHrs.MaximumValue = 10;
            DoubleHrs.ErrorMessage = "Allow only Numerics & Max Hours is less than or Equal to Total Hours!";

            Fpspread1.Sheets[0].RowCount++;
            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = CheckAll;
            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Value = 0;
            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

            int slno = 0;
            for (int ro = 0; ro < ds.Tables[0].Rows.Count; ro++)
            {
                ds.Tables[1].DefaultView.RowFilter = " dept_code='" + Convert.ToString(ds.Tables[0].Rows[ro]["dept_code"]) + "' and desig_code='" + Convert.ToString(ds.Tables[0].Rows[ro]["desig_code"]) + "' and college_code='" + Convert.ToString(ds.Tables[0].Rows[ro]["college_code"]) + "'";
                dvnew = ds.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    for (int dv = 0; dv < dvnew.Count; dv++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        slno += 1;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(slno);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = CheckInd;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Value = 0;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        string Appl_ID = "";
                        Appl_ID = d2.GetFunction("select appl_id from staff_appl_master where appl_no='" + Convert.ToString(dvnew[dv]["appl_no"]) + "' and college_code='" + collCode + "'");
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvnew[dv]["staff_code"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(Appl_ID).Trim();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvnew[dv]["staff_name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvnew[dv]["dept_name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvnew[dv]["desig_name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dvnew[dv]["stftype"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dvnew[dv]["category_name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[ro]["Tot_Hrs"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                        int col = 9;
                        double DblHRs = 0;
                        while (dtTempFrm <= dtTempTo)
                        {
                            col += 1;
                            ds.Tables[2].DefaultView.RowFilter = " Appl_ID='" + Appl_ID + "' and Staff_Code='" + Convert.ToString(dvnew[dv]["staff_code"]) + "' and WorkingDate='" + dtTempFrm.ToString("MM/dd/yyyy") + "'";
                            dvnew1 = ds.Tables[2].DefaultView;
                            if (dvnew1.Count > 0)
                            {
                                double.TryParse(Convert.ToString(dvnew1[0]["WorkingHour"]), out DblHRs);
                                DblHRs = Math.Round(DblHRs, 0, MidpointRounding.AwayFromZero);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString(DblHRs);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - 1].Text = "";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col - 1].CellType = DoubleHrs;
                            dtTempFrm = dtTempFrm.AddDays(1);
                        }
                        dtTempFrm = dtFrm;
                        dtTempTo = dtTo;
                    }
                }
            }
            Fpspread1.SaveChanges();
            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            Fpspread1.Sheets[0].FrozenRowCount = 1;
            Fpspread1.Sheets[0].FrozenColumnCount = 4;
            Fpspread1.Visible = true;
            lblNote.Visible = true;
            btnSave.Visible = true;
            rprint.Visible = true;
            lblMainErr.Visible = false;
        }
        else
        {
            lblMainErr.Visible = true;
            lblMainErr.Text = "No Record(s) Found!";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string MyErrTxt = string.Empty;
            string StaffCode = string.Empty;
            string Appl_ID = string.Empty;
            string WorkDate = string.Empty;
            double WorkHrs = 0;
            string InsQ = string.Empty;
            int InsCount = 0;

            if (CheckSelSpr())
            {
                if (CheckSpr(ref MyErrTxt))
                {
                    Fpspread1.SaveChanges();
                    for (int ro = 1; ro < Fpspread1.Sheets[0].RowCount; ro++)
                    {
                        StaffCode = string.Empty;
                        Appl_ID = string.Empty;
                        byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[ro, 1].Value);
                        if (Check == 1)
                        {
                            StaffCode = Convert.ToString(Fpspread1.Sheets[0].Cells[ro, 2].Text);
                            Appl_ID = Convert.ToString(Fpspread1.Sheets[0].Cells[ro, 2].Tag);
                            for (int col = 9; col <= Fpspread1.Sheets[0].ColumnCount - 1; col++)
                            {
                                WorkDate = string.Empty;
                                WorkHrs = 0;
                                WorkDate = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                                double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[ro, col].Text), out WorkHrs);
                                InsQ = "if exists (select * from Hour_Staff_Attnd where Appl_ID='" + Appl_ID + "' and Staff_Code='" + StaffCode + "' and WorkingDate='" + GetDayFrstDate(WorkDate).ToString("MM/dd/yyyy") + "') update Hour_Staff_Attnd set WorkingHour='" + WorkHrs + "' where Appl_ID='" + Appl_ID + "' and Staff_Code='" + StaffCode + "' and WorkingDate='" + GetDayFrstDate(WorkDate).ToString("MM/dd/yyyy") + "' else insert into Hour_Staff_Attnd (Appl_ID,Staff_Code,WorkingDate,WorkingHour) values ('" + Appl_ID + "','" + StaffCode + "','" + GetDayFrstDate(WorkDate).ToString("MM/dd/yyyy") + "','" + WorkHrs + "')";
                                int UpdCount = d2.update_method_wo_parameter(InsQ, "Text");
                                if (UpdCount > 0)
                                    InsCount++;
                            }
                        }
                    }
                    if (InsCount > 0)
                    {
                        alertpopwindow.Visible = true;
                        lblMainErr.Visible = false;
                        lblalerterr.Text = "Saved Successfully!";
                        btnGo_Click(sender, e);
                    }
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = MyErrTxt;
                }
            }
            else
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any Staff!";
            }
        }
        catch { }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    private bool CheckSelSpr()
    {
        bool EntryFlag = false;
        try
        {
            Fpspread1.SaveChanges();
            for (int ch = 1; ch < Fpspread1.Sheets[0].Rows.Count; ch++)
            {
                byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[ch, 1].Value);
                if (Check == 1)
                {
                    EntryFlag = true;
                    return EntryFlag;
                }
            }
        }
        catch { }
        return EntryFlag;
    }

    private bool CheckSpr(ref string Myerr)
    {
        bool ErrFlag = true;
        double TotHrs = 0;
        double WrkHrs = 0;
        try
        {
            Fpspread1.SaveChanges();
            for (int sp = 1; sp < Fpspread1.Sheets[0].Rows.Count; sp++)
            {
                TotHrs = 0;
                byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[sp, 1].Value);
                if (Check == 1)
                {
                    double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[sp, 8].Text), out TotHrs);
                    for (int mycol = 9; mycol <= Fpspread1.Sheets[0].ColumnCount - 1; mycol++)
                    {
                        WrkHrs = 0;
                        if (String.IsNullOrEmpty(Convert.ToString(Fpspread1.Sheets[0].Cells[sp, mycol].Text)))
                        {
                            Myerr = "Please Enter the Working Hours for '" + Convert.ToString(Fpspread1.Sheets[0].Cells[sp, 3].Text) + "' on (" + Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, mycol].Text) + ")!";
                            ErrFlag = false;
                            return ErrFlag;
                        }
                        else
                        {
                            double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[sp, mycol].Text), out WrkHrs);
                            if (WrkHrs > TotHrs)
                            {
                                Myerr = "Working Hours should be less than or Equal to Total Hours for '" + Convert.ToString(Fpspread1.Sheets[0].Cells[sp, 3].Text) + "' on (" + Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, mycol].Text) + ")!";
                                ErrFlag = false;
                                return ErrFlag;
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return ErrFlag;
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
        string dptname = "HourWise - Staff Attendance";
        string pagename = "HourWise_StaffAttnd.aspx";
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
        try
        {
            ds.Clear();
            cblDept.Items.Clear();
            txtDept.Text = "--Select--";
            cbDept.Checked = false;
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string SelQ = "select dept_code,dept_name from hrdept_master where college_code='" + collcode + "'";
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDept.DataSource = ds;
                cblDept.DataTextField = "dept_name";
                cblDept.DataValueField = "dept_code";
                cblDept.DataBind();

                if (cblDept.Items.Count > 0)
                {
                    for (int ik = 0; ik < cblDept.Items.Count; ik++)
                    {
                        cblDept.Items[ik].Selected = true;
                    }
                    txtDept.Text = "Department (" + Convert.ToString(cblDept.Items.Count) + ")";
                    cbDept.Checked = true;
                }
            }
        }
        catch { }
    }

    private void binddesig()
    {
        try
        {
            ds.Clear();
            cblDesig.Items.Clear();
            txtDesig.Text = "--Select--";
            cbDesig.Checked = false;
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string SelQ = "select desig_code,desig_name from desig_master where collegeCode='" + collcode + "'";
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDesig.DataSource = ds;
                cblDesig.DataTextField = "desig_name";
                cblDesig.DataValueField = "desig_code";
                cblDesig.DataBind();

                if (cblDesig.Items.Count > 0)
                {
                    for (int ik = 0; ik < cblDesig.Items.Count; ik++)
                    {
                        cblDesig.Items[ik].Selected = true;
                    }
                    txtDesig.Text = "Designation (" + Convert.ToString(cblDesig.Items.Count) + ")";
                    cbDesig.Checked = true;
                }
            }
        }
        catch { }
    }

    private void loadstafftype()
    {
        try
        {
            ds.Clear();
            cblStfType.Items.Clear();
            txtStfType.Text = "--Select--";
            cbStfType.Checked = false;
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblStfType.DataSource = ds;
                cblStfType.DataTextField = "stftype";
                cblStfType.DataBind();
                if (cblStfType.Items.Count > 0)
                {
                    for (int i = 0; i < cblStfType.Items.Count; i++)
                    {
                        cblStfType.Items[i].Selected = true;
                    }
                    txtStfType.Text = "StaffType (" + cblStfType.Items.Count + ")";
                    cbStfType.Checked = true;
                }
            }
        }
        catch { }
    }

    private void loadcategory()
    {
        try
        {
            ds.Clear();
            cblStfCat.Items.Clear();
            txtStfCat.Text = "--Select--";
            cbStfCat.Checked = false;
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + collcode + "' ";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblStfCat.DataSource = ds;
                cblStfCat.DataTextField = "category_Name";
                cblStfCat.DataValueField = "category_code";
                cblStfCat.DataBind();
                cblStfCat.Visible = true;
                if (cblStfCat.Items.Count > 0)
                {
                    for (int i = 0; i < cblStfCat.Items.Count; i++)
                    {
                        cblStfCat.Items[i].Selected = true;
                    }
                    txtStfCat.Text = "Category (" + cblStfCat.Items.Count + ")";
                    cbStfCat.Checked = true;
                }
            }
        }
        catch { }
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
}