using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class DepartmentVissionMission : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
            binddeg();
            binddept();
        }
        if (ddlcollege.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
    }
    public void loadcollege()
    {
        ddlcollege.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollege);
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        binddeg();
        binddept();
    }
    #region degree


    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";

            cbl_degree.Items.Clear();
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    #endregion

    #region dept
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            string collegecode = ddlcollege.SelectedItem.Value.ToString();
            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
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
            CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            // bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
            //  bindsem();
        }
        catch { }
    }
    #endregion

    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            string degreecode = Convert.ToString(getCblSelectedValue(cbl_dept));
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(degreecode))
            {
                SelectQ = " select d.Degree_Code,c.Course_Name , dt.Dept_Name,d.college_code,c.course_id,d.dept_code,d.deg_vission,d.deg_mission from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "' and d.degree_code in('" + degreecode + "') and isnull(d.deg_vission,'')<>'' and isnull(d.deg_mission,'')<>'' ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(SelectQ, "Text");
            }
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
            FpSpread1.Sheets[0].AutoPostBack = true;

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

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vision";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[3].Width = 500;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Mission";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[4].Width = 500;
            //  FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            //cb.AutoPostBack = true;
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

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["deg_vission"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["deg_mission"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            }
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Visible = true;
            print.Visible = true;
            FpSpread1.ShowHeaderSelection = false;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            #endregion
        }
        catch { }
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
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "No Record found";
        }
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        check = true;
    }
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        if (check == true)
        {
            FpSpread1.SaveChanges();
            ddlcollegeadd.Items.Clear();
            ddldegreeadd.Items.Clear();
            ddldeptadd.Items.Clear();
            string activrow = string.Empty;
            string activecol = string.Empty;
            activrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (!string.IsNullOrEmpty(activrow) && activrow != "-1")
            {
                int actrow = Convert.ToInt32(activrow);
                string clgcode = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 0].Tag);
                string courseId = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 1].Tag);
                string strCourse = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 1].Text);
                string degId = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 2].Tag);
                string strDeg = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 2].Text);
                string vission = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 3].Text);
                string mission = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 4].Text);
                string getclgname = d2.GetFunction("select collname from collinfo where college_code='" + clgcode + "'");
                ddlcollegeadd.Items.Add(new ListItem(getclgname, clgcode));
                ddldegreeadd.Items.Add(new ListItem(strCourse, courseId));
                ddldeptadd.Items.Add(new ListItem(strDeg, degId));
                txtvission.Text = vission;
                txtmission.Text = mission;
                btnsave.Text = "Update";
                divadd.Visible = true;
            }
        }
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        loadcollegeadd();
        binddegadd();
        binddeptadd();
        txtvission.Text = string.Empty;
        txtmission.Text = string.Empty;
        btnsave.Text = "Save";
        divadd.Visible = true;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string strVission = Convert.ToString(txtvission.Text);
        string strMission = Convert.ToString(txtmission.Text);
        bool check = false;
        collegecode = Convert.ToString(ddlcollegeadd.SelectedItem.Value);
        string courseid = Convert.ToString(ddldegreeadd.SelectedItem.Value);
        string degreecode = Convert.ToString(ddldeptadd.SelectedItem.Value);
        if (!string.IsNullOrEmpty(courseid) && !string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(strVission) && !string.IsNullOrEmpty(strMission) && !string.IsNullOrEmpty(collegecode))
        {
            string updQ = "  update degree set deg_vission='" + strVission + "',deg_mission='" + strMission + "' where course_id='" + courseid + "' and degree_code='" + degreecode + "' and college_code='" + collegecode + "'";
            int upd = d2.update_method_wo_parameter(updQ, "Text");
            check = true;
        }
        if (check)
        {
            txtvission.Text = string.Empty;
            txtmission.Text = string.Empty;
            if (btnsave.Text.Trim() == "Update")
            {
                divadd.Visible = false;
                btngo_Click(sender, e);
            }
            imgdiv2.Visible = true;
            lbl_alert.Text = btnsave.Text + "d Successfully";
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtvission.Text = string.Empty;
        txtmission.Text = string.Empty;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void imgclose_Click(object sender, EventArgs e)
    {
        divadd.Visible = false;
    }


    #region add
    public void loadcollegeadd()
    {
        ddlcollegeadd.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollegeadd);
    }
    public void binddegadd()
    {
        try
        {
            ddldegreeadd.Items.Clear();
            string clgvalue = ddlcollegeadd.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegreeadd.DataSource = ds;
                ddldegreeadd.DataTextField = "course_name";
                ddldegreeadd.DataValueField = "course_id";
                ddldegreeadd.DataBind();
            }

        }
        catch { }
    }
    public void binddeptadd()
    {
        try
        {
            ddldeptadd.Items.Clear();
            string degree = "";
            if (ddldegreeadd.Items.Count > 0)
                degree = Convert.ToString(ddldegreeadd.SelectedItem.Value);
            string colgcode = ddlcollegeadd.SelectedItem.Value.ToString();
            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, colgcode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldeptadd.DataSource = ds;
                    ddldeptadd.DataTextField = "dept_name";
                    ddldeptadd.DataValueField = "degree_code";
                    ddldeptadd.DataBind();
                }
            }

        }
        catch { }
    }
    protected void ddlcollegeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegadd();
        binddeptadd();

    }
    protected void ddldegreeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddeptadd();
    }


    #endregion

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
                lblvalidation1.Text = "Please Enter Your Department Vision Mission Report";
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
            pagename = "DepartmentVissionMission.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    #endregion

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

    protected string  getdegDetails(string degreecode, ref string Vision, ref string Mission)
    {
      //  string Vision = string.Empty;
       // string Mission = string.Empty;
        string deptName = string.Empty;
        string SelectQ = " select d.Degree_Code,c.Course_Name , dt.Dept_Name,d.college_code,c.course_id,d.dept_code,d.deg_vission,d.deg_mission from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "' and d.degree_code='" + degreecode + "'";
        DataSet dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
        {
             deptName = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Name"]);
            string strVission = Convert.ToString(dsload.Tables[0].Rows[0]["deg_vission"]);
            string strMission = Convert.ToString(dsload.Tables[0].Rows[0]["deg_mission"]);
            string[] sptVission = strVission.Split('$');
            if (sptVission.Length > 0)
            {
                for (int row = 0; row < sptVission.Length; row++)
                {
                    if (Vision == string.Empty)
                        Vision = "&nbsp;&nbsp;" + sptVission[row];
                    else
                        Vision += "\n" + sptVission[row];
                }
            }
            string[] sptMission = strMission.Split('$');
            if (sptMission.Length > 0)
            {
                for (int row = 0; row < sptMission.Length; row++)
                {
                    if (Mission == string.Empty)
                        Mission = "&nbsp;&nbsp;" + sptMission[row];
                    else
                        Mission += "\n" + sptMission[row];
                }
            }
        }
        return deptName;
    }
}