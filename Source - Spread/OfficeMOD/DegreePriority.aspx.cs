using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class DegreePriority : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Boolean flag_true = false;
    bool check = false;
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
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        if (ddlcollege.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
    }
    public void loadcollege()
    {
        ddlcollege.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollege);
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = loadDetails();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadSpread(ds);
        }
        else
        {
            FpSpread1.Visible = false;
            print.Visible = false;
            divpriority.Visible = false;
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "No Record found";
        }
    }

    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            string SelectQ = string.Empty;
            SelectQ = " select d.Degree_Code,c.Course_Name , dt.Dept_Name,d.college_code,c.course_id,d.dept_code,d.Dept_Priority from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }
    protected void loadSpread(DataSet ds)
    {
        try
        {
            #region design
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;

            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[2].Width = 350;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Set Priority";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            cb.AutoPostBack = true;
            #endregion

            #region value           
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                FpSpread1.Sheets[0].RowCount++;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Course_Name"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Course_id"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["dept_name"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = cb;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                double deptPr = 0;
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["Dept_Priority"]), out deptPr);
                if (deptPr!=0)
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Value = 1;
                }
                else
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = false;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(deptPr);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            }
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Visible = true;
            print.Visible = true;
            divpriority.Visible = true;
            FpSpread1.ShowHeaderSelection = false;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            #endregion
        }
        catch { }
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        check = true;
    }
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        if (flag_true == true)
        {
            FpSpread1.SaveChanges();
            string activrow = "";
            activrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            int hy_order = 0;
            for (int i = 0; i <= Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, actcol].Value);
                if (isval == 1)
                {

                    hy_order++;
                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                }
            }
            FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
        }
    }
    protected void FpSpread1_ButtonCommand(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        if (activecol == "3")
        {
            int act1 = Convert.ToInt32(activerow);
            int act2 = Convert.ToInt16(activecol);
            if (FpSpread1.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
            {
                flag_true = true;
                FpSpread1.Sheets[0].Cells[act1, act2 + 1].Text = "";
            }
            else
            {
                flag_true = false;
            }
        }
        FpSpread1.SaveChanges();

    }
    protected void btnSetPriority_Click(object sender, EventArgs e)
    {
        try
        {
            bool check = false;
            int insQ2 = d2.update_method_wo_parameter("update degree set dept_priority=null where  college_code='" + collegecode + "'", "Text");
            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    string priority = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text.Trim());
                    string courseId = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                    string degreecode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                    string clgcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag);
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        string insQ = "update degree set dept_priority='" + priority + "' where course_id='" + courseId + "' and degree_code='" + degreecode + "' and college_code='" + clgcode + "'";
                        int upd = d2.update_method_wo_parameter(insQ, "Text");
                        check = true;
                    }
                }
                if (check)
                {
                    lbl_alert.Text = "Priority Assigned";
                    imgdiv2.Visible = true;
                }
                else
                {
                    lbl_alert.Text = "Priority Not Assigned";
                    imgdiv2.Visible = true;
                }
            }
            else
            {
                lbl_alert.Text = "Priority Not Assigned";
                imgdiv2.Visible = true;
            }
        }
        catch { lbl_alert.Text = "Priority Not Assigned"; imgdiv2.Visible = true; }
    }
    protected void btnResetPriority_Click(object sender, EventArgs e)
    {
        try
        {
            bool check = false;
            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 3].Locked = false;
                    FpSpread1.Sheets[0].Cells[i, 3].Value = 0;
                    FpSpread1.Sheets[0].Cells[i, 4].Text = "";
                    check = true;
                }
            }
            FpSpread1.SaveChanges();
            if (check)
            {
                lbl_alert.Text = "Reset Successfully";
                imgdiv2.Visible = true;
            }
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
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Degree Priority Name";
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
            degreedetails = "Degree Priority";
            pagename = "DegreePriority.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    #endregion
    protected void btn_errorclose_Click(object seneder, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
}