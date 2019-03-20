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

public partial class InventoryMod_Student_Kit_Allotment : System.Web.UI.Page
{

    #region FieldDeclaration
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    int selDegree = 0;
    int selBranch = 0;
    int selSec = 0;
    int selCondo = 0;
    string newCollegeCode = string.Empty;
    string newBatchYear = string.Empty;
    string newDegreeCode = string.Empty;
    string newBranchCode = string.Empty;
    string newsemester = string.Empty;

    string qryCollege = string.Empty;
    string qryBatch = string.Empty;
    string qryDegree = string.Empty;
    string qryBranch = string.Empty;
    string qrySem = string.Empty;
    string qrySec = string.Empty;
    private string usercode;
    bool check = false;
    string semval = "";
    string sqlcmd = string.Empty;
    string txtcode = string.Empty;
    #endregion

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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                BindDegree();
                BindBatch();
                BindBranch();
                BindSection();
                checkSchoolSetting();
                ShowReport.Visible = false;
                spreadDet1.Visible = false;
                ShowReport1.Visible = false;
                spreadDet2.Visible = false;
            }
        }
        catch (Exception ex) { }
    }

    #region School_or_College
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
    #endregion

    #region College
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }

        }
        catch
        {
        }
    }
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindBatch();
            BindDegree();
            BindBranch();
            BindSection();
            ShowReport.Visible = false;
            ShowReport1.Visible = false;
            spreadDet1.Visible = false;
            spreadDet2.Visible = false;

        }
        catch
        {

        }

    }
    #endregion

    #region Batch
    public void BindBatch()
    {
        try
        {
            chklsbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    chklsbatch.DataSource = ds;
                    chklsbatch.DataTextField = "batch_year";
                    chklsbatch.DataValueField = "batch_year";
                    chklsbatch.DataBind();
                }

                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;

                }
                txtbatch.Text = lblBatch.Text + "(" + chklsbatch.Items.Count + ")";
                chkbatch.Checked = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(chkbatch, chklsbatch, txtbatch, lblBatch.Text, "--Select--");
        BindDegree();
        BindBranch();
        BindSection();
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(chkbatch, chklsbatch, txtbatch, lblBatch.Text, "--Select--");

        BindDegree();
        BindBranch();
        BindSection();
    }
    #endregion

    #region Degree
    public void BindDegree()
    {
        try
        {

            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Text = "-- Select --";
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", Convert.ToString(ddlCollege.SelectedValue).Trim());
            has.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", has, "sp");
            if (ds.Tables.Count > 0)
            {
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cblDegree.DataSource = ds;
                    cblDegree.DataTextField = "course_name";
                    cblDegree.DataValueField = "course_id";
                    cblDegree.DataBind();

                    foreach (ListItem li in cblDegree.Items)
                    {
                        li.Selected = true;
                    }
                    txtDegree.Text = "Degree" + "(" + cblDegree.Items.Count + ")";
                    chkDegree.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            int count = 0;
            if (chkDegree.Checked == true)
            {
                count++;
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = true;
                }
                txtDegree.Text = "Degree (" + (cblDegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = false;
                }
                txtDegree.Text = "-- Select --";
            }
            BindBranch();
            BindSection();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txtDegree.Text = "-- Select --";
            chkDegree.Checked = false;
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblDegree.Items.Count)
                {
                    chkDegree.Checked = true;
                }
                txtDegree.Text = "Degree (" + Convert.ToString(commcount) + ")";
            }
            BindBranch();
            BindSection();
        }
        catch (Exception ex)
        {

        }
    }
    #endregion


    #region Branch
    public void BindBranch()
    {
        try
        {

            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            txtBranch.Text = "-- Select --";
            hat.Clear();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);


            selDegree = 0;
            newDegreeCode = string.Empty;
            qryDegree = string.Empty;
            string coursecode = string.Empty;
            foreach (ListItem li in cblDegree.Items)
            {
                if (li.Selected)
                {
                    selDegree++;
                    if (string.IsNullOrEmpty(newDegreeCode.Trim()))
                    {
                        newDegreeCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newDegreeCode += ",'" + li.Value + "'";
                    }
                }
            }
            if (selDegree > 0)
            {
                coursecode = " and degree.course_id in(" + newDegreeCode + ")";

                string strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and course.college_code ='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and user_code='" + usercode + "' " + " " + coursecode + "";
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' and course.college_code ='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' " + "  " + coursecode + "";
                }
                ds = d2.select_method_wo_parameter(strquery, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "dept_name";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();

                    foreach (ListItem li in cblBranch.Items)
                    {
                        li.Selected = true;
                    }

                    txtBranch.Text = "Branch" + "(" + cblBranch.Items.Count + ")";
                    chkBranch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            int count = 0;
            if (chkBranch.Checked == true)
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    count++;
                    cblBranch.Items[i].Selected = true;
                }
                txtBranch.Text = "Branch (" + (cblBranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = false;
                }
                txtBranch.Text = "-- Select --";
            }
            BindSection();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txtBranch.Text = "-- Select --";
            chkBranch.Checked = false;
            for (int i = 0; i < cblBranch.Items.Count; i++)
            {
                if (cblBranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblBranch.Items.Count)
                {
                    chkBranch.Checked = true;
                }
                txtBranch.Text = "Branch (" + Convert.ToString(commcount) + ")";
            }
            BindSection();
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region Section
    public void BindSection()
    {
        cbl_section.Items.Clear();
        string q1 = "select distinct Sections from Registration where Sections<>'' order by Sections";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_section.DataSource = ds;
            cbl_section.DataTextField = "Sections";
            cbl_section.DataValueField = "Sections";
            cbl_section.DataBind();
            //cbl_section.Items.Insert(0, new ListItem(" ", " "));

        }
        for (int i = 0; i < cbl_section.Items.Count; i++)
        {
            cbl_section.Items[i].Selected = true;

        }
        txt_section.Text = lbl_section.Text + "(" + cbl_section.Items.Count + ")";
        cb_section.Checked = true;

    }

    protected void cb_section_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(cb_section, cbl_section, txt_section, "Section", "--Select--");

    }

    protected void cbl_section_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(cb_section, cbl_section, txt_section, "Section", "--Select--");

    }
    #endregion

    #region Go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            ds = getstudentdetails();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadspread(ds);
                ddl_feesetting_SelectedIndexChanged(sender, e);
            }
            else
            {

                alertimg.Visible = true;
                lbl_alert.Text = "No Records Found";

            }
        }
        catch
        {

        }


    }


    #endregion

    #region Fspread
    private DataSet getstudentdetails()
    {

        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string batch = string.Empty;
            string courseid = string.Empty;
            string dept = string.Empty;
            string sec = string.Empty;


            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (chklsbatch.Items.Count > 0)
                batch = Convert.ToString(d2.getCblSelectedValue(chklsbatch));
            if (cblDegree.Items.Count > 0)
                courseid = Convert.ToString(d2.getCblSelectedValue(cblDegree));
            if (cblBranch.Items.Count > 0)
                dept = Convert.ToString(d2.getCblSelectedValue(cblBranch));
            if (cbl_section.Items.Count > 0)
                sec = Convert.ToString(d2.getCblSelectedValue(cbl_section));


            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(courseid) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(sec))
            {
                selQ = " select distinct r.roll_no,r.Stud_Name,r.App_No  from registration r,degree de,course c,department dp where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.batch_year in('" + batch + "') and  c.Course_Id in('" + courseid + "')  and r.degree_code in('" + dept + "')  and r.Sections in('" + sec + "')  and  r.college_code='" + collegecode + "'  group by r.roll_no,r.Stud_Name,r.App_No order by  r.roll_no,r.Stud_Name,r.App_No ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");

            }
            #endregion
        }
        catch (Exception ex)
        { }

        return dsload;
    }

    public void loadspread(DataSet ds)
    {
        try
        {
            DataView dv = new DataView();
            DataSet dskit = new DataSet();
            spreadDet1.Sheets[0].RowCount = 1;
            spreadDet1.Sheets[0].ColumnCount = 5;
            spreadDet1.CommandBar.Visible = false;
            spreadDet1.Sheets[0].AutoPostBack = false;
            spreadDet1.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].Columns[0].Locked = true;
            spreadDet1.Columns[0].Width = 80;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Columns[1].Width = 80;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[2].Locked = true;
            spreadDet1.Columns[2].Width = 200;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[3].Locked = true;
            spreadDet1.Columns[3].Width = 250;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Kit Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[4].Locked = true;
            spreadDet1.Columns[4].Width = 100;

            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chk1.AutoPostBack = true;
            chkall.AutoPostBack = false;
            spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].CellType = chk1;
            spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

            int sno = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    spreadDet1.Sheets[0].RowCount++;
                    sno++;
                    string rollno = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]).Trim();
                    string stuname = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]).Trim();
                    string appno = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]).Trim();

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].CellType = chkall;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].CellType = txtCell;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Text = rollno;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Tag = appno;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Text = stuname;

                    string ktname = string.Empty;
                    string sqlkitname = "select distinct cm.MasterValue from IM_StudentKit_Details sd,IM_KitMaster km,CO_MasterValues cm  where sd.KitCode=km.KitCode and cm.CollegeCode=km.CollegeCode and cm.MasterCode=sd.KitCode and km.KitCode=cm.MasterCode and km.ItemCode=sd.ItemCode and Stu_AppNo='" + appno + "'";
                    dskit.Clear();
                    dskit = d2.select_method_wo_parameter(sqlkitname, "TEXT");
                    if (dskit.Tables.Count > 0 && dskit.Tables[0].Rows.Count > 0)
                    {
                        for (int krow = 0; krow < dskit.Tables[0].Rows.Count; krow++)
                        {

                            string kitname = Convert.ToString(dskit.Tables[0].Rows[krow]["MasterValue"]).Trim();
                            if (String.IsNullOrEmpty(ktname))
                                ktname = kitname;
                            else
                                ktname += "," + kitname;
                        }
                    }

                    if (ktname != "")
                        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Text = ktname;
                    else
                        spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Text = "";


                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;



                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].Locked = false;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Locked = true;
                    spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Locked = true;


                }
                spreadDet1.Sheets[0].PageSize = spreadDet1.Sheets[0].RowCount;
                spreadDet1.SaveChanges();
                ShowReport.Visible = true;
                spreadDet1.Visible = true;
                ShowReport1.Visible = false;
                spreadDet2.Visible = false;
                loadFeeSetting();


            }
        }

        catch
        {

        }

    }

    protected void spreadDet1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //Fpspread2.Visible = true;
        try
        {
            string actrow = spreadDet1.Sheets[0].ActiveRow.ToString();
            string actcol = spreadDet1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (spreadDet1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(spreadDet1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < spreadDet1.Sheets[0].RowCount; i++)
                        {
                            spreadDet1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < spreadDet1.Sheets[0].RowCount; i++)
                        {
                            spreadDet1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "Individual_StudentFeeStatus"); 
        }
    }

    #endregion

    #region Loadkit
    public void loadFeeSetting()
    {
        try
        {
            ddl_feesetting.Items.Clear();
            ddl_feesetting.Items.Add("Common");
            ddl_feesetting.Items.Add("Individual");

        }
        catch
        {
        }
    }


    protected void ddl_feesetting_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ShowReport1.Visible = false;
            spreadDet2.Visible = false;
            string ComOrInd = "";
            if (ddl_feesetting.SelectedIndex == 0)
            {
                ComOrInd = "0";
                ddl_Kitname.Visible = false;
                UpdatePanel6.Visible = true;
            }
            else
            {
                ComOrInd = "1";
                ddl_Kitname.Visible = true;
                UpdatePanel6.Visible = false;
            }
            loadkit(ComOrInd);
        }
        catch
        {

        }

    }

    protected void cb_kitname_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(cb_kitname, cbl_kitname, txt_kitname, "Kit Name", "--Select--");


    }

    protected void cbl_kitname_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(cb_kitname, cbl_kitname, txt_kitname, "Kit Name", "--Select--");


    }


    public void loadkit(string ComOrInd)
    {
        try
        {
            ddl_Kitname.Items.Clear();
            cbl_kitname.Items.Clear();
            string sql = "select distinct cm.MasterCode,cm.MasterValue from inventorykit ik,CO_MasterValues cm where ik.collegecode=cm.CollegeCode and ik.kitid=cm.MasterCode and MasterCriteria='kit' and cm.CollegeCode='" + ddlCollege.SelectedValue + "' and ik.CommonOrIndividual='" + ComOrInd + "'";
            //string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='kit' and CollegeCode ='" + ddlCollege.SelectedValue + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ComOrInd == "1")
                {
                    ddl_Kitname.DataSource = ds;
                    ddl_Kitname.DataTextField = "MasterValue";
                    ddl_Kitname.DataValueField = "MasterCode";
                    ddl_Kitname.DataBind();
                }
                else
                {
                    cbl_kitname.DataSource = ds;
                    cbl_kitname.DataTextField = "MasterValue";
                    cbl_kitname.DataValueField = "MasterCode";
                    cbl_kitname.DataBind();
                    for (int i = 0; i < cbl_kitname.Items.Count; i++)
                    {
                        cbl_kitname.Items[i].Selected = true;

                    }
                    txt_kitname.Text = lbl_kitename.Text + "(" + cbl_kitname.Items.Count + ")";
                    cb_kitname.Checked = true;
                }
            }

        }

        catch
        {
        }
    }

    protected void ddl_Kitname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ShowReport1.Visible = false;
            spreadDet2.Visible = false;
        }
        catch
        {

        }

    }


    #endregion

    #region Additem
    protected void btn_Add_item_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dskit = new DataSet();
            int chckval = 0;
            int cvalue = 0;
            if (spreadDet1.Rows.Count > 0)
            {
                spreadDet1.SaveChanges();
                for (int row = 0; row < spreadDet1.Sheets[0].RowCount; row++)
                {
                    chckval = Convert.ToInt32(spreadDet1.Sheets[0].Cells[row, 1].Value);
                    cvalue += chckval;
                }
            }
            if (cvalue > 0)
            {
                dskit = KititemDetails();
                if (dskit.Tables.Count > 0 && dskit.Tables[0].Rows.Count > 0)
                {
                    loadspreadkit(dskit);
                }
                else
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "No Records Found";

                }
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Pease Select The Student";
                return;
            }

        }
        catch
        {
        }

    }

    public DataSet KititemDetails()
    {
        DataSet dsloadDetails = new DataSet();
        try
        {
            #region get Value
            string collcode = string.Empty;
            string kitcode = string.Empty;
            string commonind = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddl_feesetting.SelectedIndex == 0)
            {
                if (cbl_kitname.Items.Count > 0)
                    kitcode = Convert.ToString(d2.getCblSelectedValue(cbl_kitname));
                commonind = "0";
            }
            else
            {
                if (ddl_Kitname.Items.Count > 0)
                    kitcode = Convert.ToString(ddl_Kitname.SelectedValue);
                commonind = "1";
            }

            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(kitcode))
            {
                string sqlkit = "select distinct i.ItemPK,i.ItemCode ,i.ItemName ,i.ItemUnit,sm.StorePK ,sm.StoreName,i.ItemHeaderName,t.MasterValue as ItemSubHeadername,km.KitCode from IM_StoreMaster sm,IM_ItemMaster i,CO_MasterValues t,IM_KitMaster km,inventorykit ik  where sm.StorePK=i.StoreFK and t.CollegeCode=sm.CollegeCode and t.MasterCode=i.subheader_code and t.CollegeCode=km.CollegeCode and i.ItemCode=km.ItemCode and ik.collegecode=t.CollegeCode and km.KitCode  in('" + kitcode + "') and  km.CollegeCode='" + collcode + "' and ik.CommonOrIndividual='" + commonind + "'  group by ItemPK,i.ItemCode,i.ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,t.MasterValue,km.KitCode order by i.ItemPK";
                dsloadDetails.Clear();
                dsloadDetails = d2.select_method_wo_parameter(sqlkit, "Text");

            }


            #endregion
        }

        catch
        { }

        return dsloadDetails;
    }

    public void loadspreadkit(DataSet ds1)
    {
        try
        {
            DataView dv = new DataView();
            spreadDet2.Sheets[0].RowCount = 0;
            spreadDet2.Sheets[0].ColumnCount = 5;
            spreadDet2.CommandBar.Visible = false;
            spreadDet2.Sheets[0].AutoPostBack = false;
            spreadDet2.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet2.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;

            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet2.Sheets[0].Columns[0].Locked = true;
            spreadDet2.Columns[0].Width = 80;

            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Header Name";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Columns[1].Width = 200;

            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].Columns[2].Locked = true;
            spreadDet2.Columns[2].Width = 200;

            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Measure";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].Columns[3].Locked = true;
            spreadDet2.Columns[3].Width = 80;


            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Qty";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet2.Sheets[0].Columns[4].Locked = false;
            spreadDet2.Columns[4].Width = 80;

            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.DoubleCellType num1 = new FarPoint.Web.Spread.DoubleCellType();
            num1.ErrorMessage = "Enter Only Numbers";
            //FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
            //FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            //chk1.AutoPostBack = true;
            //chkall.AutoPostBack = false;
            //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].CellType = chk1;
            //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

            int sno = 0;
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                {
                    spreadDet2.Sheets[0].RowCount++;
                    sno++;
                    string itemheadname = Convert.ToString(ds1.Tables[0].Rows[row]["ItemHeaderName"]).Trim();
                    string itemname = Convert.ToString(ds1.Tables[0].Rows[row]["ItemName"]).Trim();
                    string itemunit = Convert.ToString(ds1.Tables[0].Rows[row]["ItemUnit"]).Trim();
                    string itemno = Convert.ToString(ds1.Tables[0].Rows[row]["ItemCode"]).Trim();
                    string impk = Convert.ToString(ds1.Tables[0].Rows[row]["ItemPK"]).Trim();
                    string kitcode = Convert.ToString(ds1.Tables[0].Rows[row]["KitCode"]).Trim();

                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].CellType = num1;

                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].Text = itemheadname;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].Tag = kitcode;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].Text = itemname;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].Tag = itemno;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].Text = itemunit;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].Tag = impk;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].Text = "";


                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;



                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 1].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 2].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 3].Locked = true;
                    spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 4].Locked = false;


                }
                spreadDet2.Sheets[0].PageSize = spreadDet2.Sheets[0].RowCount;
                spreadDet2.SaveChanges();
                ShowReport1.Visible = true;
                spreadDet2.Visible = true;
            }
        }

        catch
        {

        }

    }

    #endregion

    #region ItemQtySave
    protected void btn_kititem_Save_Click(object sender, EventArgs e)
    {
        try
        {
            string stuappno = string.Empty;
            string itmcode = string.Empty;
            string Qty = string.Empty;
            string kitno = string.Empty;
            string itmpk = string.Empty;
            string Date = string.Empty;
            string header_id = string.Empty;
            string ledg_id = string.Empty;
            string kitamt = string.Empty;
            DataSet dskitfee = new DataSet();
            bool qty = false;
            bool finyear = false;
            int insertkitqty = 0;
            string kitnum = "";

            Date = DateTime.Now.ToString("MM/dd/yyyy");
            string CommOrInd = "";

            if (ddl_feesetting.SelectedIndex == 0)
            {
                if (cbl_kitname.Items.Count > 0)
                    kitno = Convert.ToString(d2.getCblSelectedValue(cbl_kitname));
                CommOrInd = "0";
            }
            else
            {
                if (ddl_Kitname.Items.Count > 0)
                    kitno = Convert.ToString(ddl_Kitname.SelectedValue);
                CommOrInd = "1";
            }
            //header and ledger
            string kitfeeallot = "select headerid,ledgerid,amt from inventorykit where collegecode='" + collegecode + "' and usercode='" + usercode + "' and kitid in('" + kitno + "') and CommonOrIndividual='" + CommOrInd + "'";
            dskitfee.Clear();
            dskitfee = d2.select_method_wo_parameter(kitfeeallot, "Text");
            if (dskitfee.Tables.Count > 0 && dskitfee.Tables[0].Rows.Count > 0)
            {
                header_id = Convert.ToString(dskitfee.Tables[0].Rows[0]["headerid"]);
                ledg_id = Convert.ToString(dskitfee.Tables[0].Rows[0]["ledgerid"]);
                kitamt = Convert.ToString(dskitfee.Tables[0].Rows[0]["amt"]);
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Please set Fees setting";
                return;
            }


            if (spreadDet1.Rows.Count > 0)
            {
                spreadDet1.SaveChanges();
                for (int row = 0; row < spreadDet1.Sheets[0].RowCount - 1; row++)
                {
                    int checkval = Convert.ToInt32(spreadDet1.Sheets[0].Cells[row, 1].Value);
                    if (checkval == 1)
                    {
                        check = true;
                        stuappno = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 2].Tag);
                        if (stuappno != "")

                        {
                            if (spreadDet2.Rows.Count > 0)
                            {
                                spreadDet2.SaveChanges();
                                for (int krow = 0; krow < spreadDet2.Sheets[0].RowCount; krow++)
                                {
                                    itmcode = Convert.ToString(spreadDet2.Sheets[0].Cells[krow, 2].Tag);
                                    itmpk = Convert.ToString(spreadDet2.Sheets[0].Cells[krow, 3].Tag);
                                    Qty = Convert.ToString(spreadDet2.Sheets[0].Cells[krow, 4].Text);
                                    kitnum = Convert.ToString(spreadDet2.Sheets[0].Cells[krow, 1].Tag);
                                    if (kitnum != "" && Qty != "")
                                    {
                                        string insertqtykit = "if exists (select * from IM_StudentKit_Details where ItemCode='" + itmcode + "' and KitCode='" + kitnum + "' and Stu_AppNo='" + stuappno + "' and itemfk='" + itmpk + "') Update IM_StudentKit_Details set  ItemCode='" + itmcode + "',KitCode='" + kitnum + "' where  ItemCode='" + itmcode + "' and KitCode='" + kitnum + "' and Stu_AppNo='" + stuappno + "' and itemfk='" + itmpk + "' else insert into IM_StudentKit_Details (Stu_AppNo,ItemCode,KitCode,Qty,Date,itemfk) values('" + stuappno + "','" + itmcode + "','" + kitnum + "','" + Qty + "','" + Date + "','" + itmpk + "')";
                                        insertkitqty = d2.update_method_wo_parameter(insertqtykit, "Text");
                                        qty = true;
                                    }
                                }

                            }
                            if (qty)
                            {
                                //FT_FeeAllot
                                string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode);
                                if (finYeaid.Trim() != "" && finYeaid.Trim() != "0")
                                {
                                    //fee Category
                                    string selsctfeecate = d2.GetFunction("select distinct current_semester from registration r where r.App_No='" + stuappno + "' and r.college_code in('" + collegecode + "')");

                                    if (checkSchoolSetting() == 0)
                                    {
                                        if (selsctfeecate == "1")
                                            semval = "Term 1";
                                        else if (selsctfeecate == "2")
                                            semval = "Term 2";
                                        else if (selsctfeecate == "3")
                                            semval = "Term 3";
                                        else if (selsctfeecate == "4")
                                            semval = "Term 4";
                                    }
                                    else
                                    {
                                        if (selsctfeecate == "1")
                                            semval = "1 Semester";
                                        if (selsctfeecate == "2")
                                            semval = "2 Semester";
                                        if (selsctfeecate == "3")
                                            semval = "3 Semester";
                                        if (selsctfeecate == "4")
                                            semval = "4 Semester";
                                        if (selsctfeecate == "5")
                                            semval = "5 Semester";
                                        if (selsctfeecate == "6")
                                            semval = "6 Semester";
                                        if (selsctfeecate == "7")
                                            semval = "7 Semester";
                                        if (selsctfeecate == "8")
                                            semval = "8 Semester";
                                        if (selsctfeecate == "9")
                                            semval = "9 Semester";

                                    }
                                    if (semval != "")
                                    {
                                        sqlcmd = d2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode + "'");
                                        if (sqlcmd != "0" && sqlcmd != "")
                                            txtcode = Convert.ToString(sqlcmd);
                                    }
                                    if (txtcode != "0")
                                    {
                                        string insertfeeallot = " if exists (select * from FT_FeeAllot where App_No ='" + stuappno + "' and LedgerFK='" + ledg_id + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + txtcode + "' and FinyearFk='" + finYeaid + "' and AllotDate='" + Date + "') update FT_FeeAllot set AllotDate='" + Date + "',MemType='1',FeeAmount='" + kitamt + "',TotalAmount ='" + kitamt + "' ,BalAmount ='" + kitamt + "'-isnull(PaidAmount,'0')   where App_No ='" + stuappno + "' and LedgerFK='" + ledg_id + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + txtcode + "' and FinyearFk='" + finYeaid + "' else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt)  values ('" + stuappno + "','" + ledg_id + "','" + header_id + "','" + finYeaid + "','" + Date + "','" + kitamt + "','" + txtcode + "','',0,0,'" + kitamt + "','" + kitamt + "','1','1',0,0)";
                                        insertkitqty = d2.update_method_wo_parameter(insertfeeallot, "Text");
                                        //finyear = true;
                                    }
                                }
                                else
                                {
                                    alertimg.Visible = true;
                                    lbl_alert.Text = "Please Select Finance Year";
                                    return;
                                }

                            }

                        }
                    }
                }
            }
            if (!check)
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Pease Select The Student";
                return;
            }

            if (insertkitqty > 0)
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                btn_go_Click(sender, e);
            }
            if (!qty)
            {
                alertimg.Visible = true;
                lbl_alert.Text = "Please Enter The Quantity";
                return;
            }
        }

        catch
        {


        }

    }
    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            alertimg.Visible = false;
        }
        catch
        {

        }
    }


}