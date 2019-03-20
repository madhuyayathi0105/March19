/*
 * Author : Mohamed Idhris Sheik Dawood
 * Date Created : 02-01-2017
 * */

using System;
using InsproDataAccess;
using System.Data;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class OfficeMOD_AdminUserSetting : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    InsproDirectAccess dirAccess = new InsproDirectAccess();
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
            loadCollege();
            collegecode = ddlCollege.Items.Count > 0 ? ddlCollege.SelectedValue.ToString() : "13";
            loadSpread(ddlCollege.SelectedValue);
        }
        else
        {
            collegecode = ddlCollege.Items.Count > 0 ? ddlCollege.SelectedValue.ToString() : "13";
        }
    }
    //Load college and operation
    private void loadCollege()
    {
        try
        {
            DataTable dtCollege = new DataTable();
            string selectQ = "select collname,college_code from collinfo";
            dtCollege = dirAccess.selectDataTable(selectQ);

            ddlCollege.Items.Clear();
            if (dtCollege.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCollege;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch { }
    }
    private string getPrevUser(string collegeCode)
    {
        string selectQ = "select user_code from New_InsSettings where LinkName ='AdminUserSettingForStudentLogin' and college_code='" + collegeCode + "'";
        string prevUserCode = dirAccess.selectScalarString(selectQ);
        return prevUserCode;
    }
    protected void ddlCollege_IndexChanged(object sender, EventArgs e)
    {
        loadSpread(ddlCollege.SelectedValue);
    }
    //Spread Load
    private void loadSpread(string collegeCode)
    {
        try
        {
            Fpspread1.Visible = false;
            rptprint.Visible = false;
            lblvalidation1.Visible = false;
            Printcontrol.Visible = false;
            txtexcelname.Text = string.Empty;
            lblRecNotFound.Visible = true;

            string prevUser = getPrevUser(collegeCode);
            string selectQ = "select user_code,USER_ID,Full_Name, Description, SingleUser, is_staff, staff_code ,case when ISNULL(is_staff,'0')='1' then 'Yes' else 'No' end  as  isstaff,case when ISNULL(SingleUser,'0')='1' then 'Yes' else 'No' end as IsSingleUser  from UserMaster where college_code='" + collegeCode + "' ";
            DataTable dtUser = dirAccess.selectDataTable(selectQ);

            if (dtUser.Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 8;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[0].Width = 40;
                Fpspread1.Columns[0].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "User ID";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 160;
                Fpspread1.Columns[1].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Full Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[2].Width = 200;
                Fpspread1.Columns[2].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Description";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Column.Width = 150;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[3].Width = 200;
                Fpspread1.Columns[3].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Single User";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 100;
                Fpspread1.Columns[4].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Staff";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[5].Width = 60;
                Fpspread1.Columns[5].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff Code";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[6].Width = 100;
                Fpspread1.Columns[6].Locked = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[7].Width = 60;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                for (int row = 0; row < dtUser.Rows.Count; row++)
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtUser.Rows[row]["USER_ID"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dtUser.Rows[row]["user_code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtUser.Rows[row]["Full_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtUser.Rows[row]["Description"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtUser.Rows[row]["IsSingleUser"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dtUser.Rows[row]["SingleUser"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dtUser.Rows[row]["isstaff"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dtUser.Rows[row]["is_staff"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dtUser.Rows[row]["staff_code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].CellType = chkall;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    if (prevUser.Trim() == Convert.ToString(dtUser.Rows[row]["user_code"]).Trim())
                    {
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Value = 1;
                        for (int k = 0; k < Fpspread1.Columns.Count; k++)
                        {
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].BackColor = ColorTranslator.FromHtml("#00CC00");
                        }
                    }
                }
                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Width = 950;
                Fpspread1.Height = 400;

                rptprint.Visible = true;
                lblRecNotFound.Visible = false;
            }
        }
        catch { }
    }
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string activerow = "";
            string activecol = "";
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            int countnew = 0;

            if (activecol == "7")
            {
                for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
                {
                    if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[Convert.ToInt32(i), Convert.ToInt32(activecol)].Value) == 1)
                    {
                        countnew++;
                    }
                    else
                    {
                        for (int k = 0; k < Fpspread1.Columns.Count; k++)
                        {
                            Fpspread1.Sheets[0].Cells[Convert.ToInt32(i), k].BackColor = ColorTranslator.FromHtml("White");
                        }
                    }
                }
            }
            if (countnew > 1)
            {
                countnew--;
                Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Value = 0;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Only One User Should Be Selected')", true);
            }
        }
        catch
        {
        }
    }
    //Save User Code
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string selectedUserCode = string.Empty;
            for (int spreadI = 0; spreadI < Fpspread1.Rows.Count; spreadI++)
            {
                if (Convert.ToByte(Fpspread1.Sheets[0].Cells[spreadI, 7].Value) == 1)
                {
                    selectedUserCode = Convert.ToString(Fpspread1.Sheets[0].Cells[spreadI, 1].Tag).Trim();
                }
            }

            if (selectedUserCode != string.Empty)
            {
                string insUpQ = "if exists (select user_code from New_InsSettings where LinkName ='AdminUserSettingForStudentLogin' and college_code='" + collegecode + "') update New_InsSettings set user_code='" + selectedUserCode + "' where LinkName ='AdminUserSettingForStudentLogin' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code) values ('AdminUserSettingForStudentLogin','1','" + selectedUserCode + "','" + collegecode + "' )";
                if (dirAccess.updateData(insUpQ) > 0)
                {
                    loadSpread(ddlCollege.SelectedValue);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select User!')", true);
            }
        }
        catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later!')", true); }
    }
    //Print and Excel Report
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                DAccess2 DA = new DAccess2();
                DA.printexcelreport(Fpspread1, reportname);
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Admin User Settings";
            string pagename = "AdminUserSetting.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }
    }
    //Last Modified 02-01-2017
}