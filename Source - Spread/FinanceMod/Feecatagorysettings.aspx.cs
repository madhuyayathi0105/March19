using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI;
using System.Configuration;

public partial class Feecatagorysettings : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;


    string isTermOrSemester = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string SelectQ = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();

                // ddl_Year_Selectedindex(sender, e);
                BindCollege();
                BindCollegename();
                bindBatch();
                bindYearWise();
                bindsem();
                bindTerm();
                bindyear();
                feecatagorymatch.Visible = false;
            }
        }
        catch (Exception ex) { }
    }

    public void bindBatch()
    {
        try
        {
            ds.Clear();
            ddlbatchYear.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatchYear.DataSource = ds;
                ddlbatchYear.DataTextField = "batch_Year";
                ddlbatchYear.DataBind();
            }
        }
        catch
        {

        }

    }
    public void bindyear()
    {
        try
        {
            ds.Clear();
            ddlYear.Items.Clear();
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            Year = Year - 1;
            for (int Y = 0; Y <= 3; Y++)
            {
                ddlYear.Items.Add(Convert.ToString(Year));
                Year++;
            }

        }
        catch
        {

        }

    }
    //protected void bindYearWise()
    //{

    //    string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + Convert.ToString(ddl_collegepop1.SelectedItem.Value) + "'";
    //    ds.Clear();
    //    ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        ddl_Year.DataSource = ds;
    //        ddl_Year.DataTextField = "textval";
    //        ddl_Year.DataValueField = "textcode";
    //        ddl_Year.DataBind();
    //    }
    //}
    public void ddl_Year_Selectedindex(object sender, EventArgs e)
    {
        //string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + Convert.ToString(ddl_collegepop1.SelectedItem.Value) + "'";
        //ds.Clear();
        //ds = d2.select_method_wo_parameter(SelectQ, "Text");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddl_Year.DataSource = ds;
        //    ddl_Year.DataTextField = "textval";
        //    ddl_Year.DataValueField = "textcode";
        //    ddl_Year.DataBind();
        //}
        // bindYearWise();

    }

    public void bindYearWise()
    {
        // SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + Convert.ToString(ddl_collegepop1.SelectedItem.Value) + "'";
        //ds.Clear();
        //ds = d2.select_method_wo_parameter(SelectQ, "Text");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddl_Year.DataSource = ds;
        //    ddl_Year.DataTextField = "textval";
        //    ddl_Year.DataValueField = "textcode";
        //    ddl_Year.DataBind();
        //}
        ddl_Year.Items.Clear();
        Dictionary<string, int> dtYear = new Dictionary<string, int>();
        dtYear.Add("1 Year", 1);
        dtYear.Add("2 Year", 2);
        dtYear.Add("3 Year", 3);
        dtYear.Add("4 Year", 4);
        dtYear.Add("5 Year", 5);
        foreach (KeyValuePair<string, int> dtVal in dtYear)
        {
            ddl_Year.Items.Add(new ListItem(Convert.ToString(dtVal.Key), Convert.ToString(dtVal.Value)));
        }

    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        string q1 = " select c.collname,dt.Dept_Name,textval,isnull(monthcode,'0') as monthcode from Fee_degree_match f,textvaltable t,collinfo c,degree d,department dt,Course co where t.TextCriteria='FEECA' and f.FeeCategory=t.TextCode and f.college_code=t.college_code and c.college_code=f.College_code and d.Degree_Code=f.Degree_code and co.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and co.college_code=d.college_code and d.college_code=dt.college_code and f.College_code=d.college_code and d.Degree_Code =f.Degree_code  and d.college_code ='" + ddlcollege.SelectedValue + "' order by f.College_code,d.Degree_Code,t.TextCode  ";

        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            q1 = "";
            q1 = "S.No-50/Institution Name-450/" + lbl_degree.Text + "-150/Feecatagory-150/Month-50";
            Fpreadheaderbindmethod(q1, FpSpread1, "false");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["collname"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["textval"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Rows[i].Locked = true;
                string month = getMonth(Convert.ToString(ds.Tables[0].Rows[i]["MonthCode"]));
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = month;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Rows[i].Locked = true;
            }
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        else
        {
            FpSpread1.Visible = false;
            lbl_error2.Text = "No Records Founds";
        }
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        feecatagorymatch.Visible = false;
        lbl_error1.Text = "";
    }
    protected void lnkyearMatch_click(object sender, EventArgs e)
    {
        popfine.Visible = true;
    }
    protected void cbSemester_CheckedChanged(object sender, EventArgs e)
    {
        if (cbSemester.Checked == true)
        {
            for (int i = 0; i < cblSemester.Items.Count; i++)
            {
                cblSemester.Items[i].Selected = true;
            }
            txtsemster.Text = "Semester (" + (cblSemester.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblSemester.Items.Count; i++)
            {
                cblSemester.Items[i].Selected = false;
            }
            txtsemster.Text = "--Select--";
        }

    }
    protected void cblSemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtsemster.Text = "--Select--";
        cbSemester.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cblSemester.Items.Count; i++)
        {
            if (cblSemester.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtsemster.Text = "Semester(" + commcount.ToString() + ")";
            if (commcount == cblSemester.Items.Count)
            {
                cbSemester.Checked = true;
            }
        }

    }
    protected void cbTerm_CheckedChanged(object sender, EventArgs e)
    {
        if (cbTerm.Checked == true)
        {
            for (int i = 0; i < cblTerm.Items.Count; i++)
            {
                cblTerm.Items[i].Selected = true;
            }
            txtTerm.Text = "Term(" + (cblTerm.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblTerm.Items.Count; i++)
            {
                cblTerm.Items[i].Selected = false;
            }
            txtTerm.Text = "--Select--";
        }

    }
    protected void cblTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtTerm.Text = "--Select--";
        cbTerm.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cblTerm.Items.Count; i++)
        {
            if (cblTerm.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtTerm.Text = "Term(" + commcount.ToString() + ")";
            if (commcount == cblTerm.Items.Count)
            {
                cbTerm.Checked = true;
            }
        }

    }
    protected void bindsem()
    {
        //try
        //{
        //    //string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
        //    cblSemester.Items.Clear();
        //    cbSemester.Checked = false;
        //    txtsemster.Text = "--Select--";
        //    ds.Clear();
        //    string linkName = string.Empty;
        //    string cbltext = string.Empty;
        //    //d2.featDegreeCode = featDegcode;
        //    ds = d2.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
        //    //ds = loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode);
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        cblSemester.DataSource = ds;
        //        cblSemester.DataTextField = "TextVal";
        //        cblSemester.DataValueField = "TextCode";
        //        cblSemester.DataBind();

        //        if (cblSemester.Items.Count > 0)
        //        {
        //            for (int i = 0; i < cblSemester.Items.Count; i++)
        //            {
        //                cblSemester.Items[i].Selected = true;
        //                cbltext = Convert.ToString(cblSemester.Items[i].Text);
        //            }
        //            if (cblSemester.Items.Count == 1)
        //                txtsemster.Text = "" + linkName + "(" + cbltext + ")";
        //            else
        //                txtsemster.Text = "" + linkName + "(" + cblSemester.Items.Count + ")";
        //            cbSemester.Checked = true;
        //        }
        //    }
        //}
        //catch { }
        string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + Convert.ToString(ddl_collegepop1.SelectedItem.Value) + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(SelectQ, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cblSemester.DataSource = ds;
            cblSemester.DataTextField = "textval";
            cblSemester.DataValueField = "textcode";
            cblSemester.DataBind();
        }
    }
    protected void bindTerm()
    {
        //try
        //{
        //    //string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
        //    cblTerm.Items.Clear();
        //    cbTerm.Checked = false;
        //    txtTerm.Text = "--Select--";
        //    ds.Clear();
        //    string linkName = string.Empty;
        //    string cbltext = string.Empty;
        //    //d2.featDegreeCode = featDegcode;
        //    ds = d2.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
        //    //ds = loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode);
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        cblTerm.DataSource = ds;
        //        cblTerm.DataTextField = "TextVal";
        //        cblTerm.DataValueField = "TextCode";
        //        cblTerm.DataBind();

        //        if (cblTerm.Items.Count > 0)
        //        {
        //            for (int i = 0; i < cblTerm.Items.Count; i++)
        //            {
        //                cblTerm.Items[i].Selected = true;
        //                cbltext = Convert.ToString(cblTerm.Items[i].Text);
        //            }
        //            if (cblTerm.Items.Count == 1)
        //                txtTerm.Text = "" + linkName + "(" + cbltext + ")";
        //            else
        //                txtTerm.Text = "" + linkName + "(" + cblTerm.Items.Count + ")";
        //            cbTerm.Checked = true;
        //        }
        //    }
        //}
        //catch { }
        string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Term%' and textval not like '-1%' and college_code ='" + Convert.ToString(ddl_collegepop1.SelectedItem.Value) + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(SelectQ, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cblTerm.DataSource = ds;
            cblTerm.DataTextField = "textval";
            cblTerm.DataValueField = "textcode";
            cblTerm.DataBind();
        }
    }

    //year wise setting added by abarna 11.10.2017
    protected void btn_savepop_click(object sender, EventArgs e)
    {
        if (txt_feecatagory.Text.Trim() != "")
        {
            string insert = "   if not exists(select*from textvaltable where college_code='" + Convert.ToString(ddl_collegepop.SelectedItem.Value) + "' and textval='" + txt_feecatagory.Text + "' and TextCriteria='FEECA')insert into textvaltable (TextVal,college_code,TextCriteria) values('" + txt_feecatagory.Text + "','" + Convert.ToString(ddl_collegepop.SelectedItem.Value) + "','FEECA')";
            int up = d2.update_method_wo_parameter(insert, "Text");
            if (up != 0)
            {
                lbl_error.Text = "Saved Successfully";
                txt_feecatagory.Text = "";
            }
        }
        btn_lnk_OnClick(sender, e);
    }

    protected void SaveFeeCat_click(object sender, EventArgs e)
    {
        try
        {
            //string collegeCode = string.Empty;
            Hashtable htSemCode = new Hashtable();
            Hashtable htPaidInsert = new Hashtable();
            bool boollSave = false;
            string colgCode = Convert.ToString(ddl_collegepop1.SelectedValue);
            string yearCode = Convert.ToString(ddl_Year.SelectedItem.Value);
            for (int row = 0; row < cblSemester.Items.Count; row++)
            {
                if (!cblSemester.Items[row].Selected)
                    continue;
                string semVal = Convert.ToString(cblSemester.Items[row].Value);
                htPaidInsert.Add("@collegeCode", colgCode);
                htPaidInsert.Add("@yearCode", yearCode);
                htPaidInsert.Add("@semesterTermCode", semVal);
                htPaidInsert.Add("@isTermOrSem", 1);
                int insert = d2.insert_method("uspInsertUpdateFeeCatYearMatch", htPaidInsert, "sp");
                htPaidInsert.Clear();
                if (insert != 0)
                    boollSave = true;
            }
            htPaidInsert.Clear();
            string semesterTermCode1 = Convert.ToString(getCblSelectedValue(cblTerm));
            for (int row = 0; row < cblTerm.Items.Count; row++)
            {
                if (!cblTerm.Items[row].Selected)
                    continue;
                string semVal = Convert.ToString(cblTerm.Items[row].Value);
                htPaidInsert.Add("@collegeCode", colgCode);
                htPaidInsert.Add("@yearCode", yearCode);
                htPaidInsert.Add("@semesterTermCode", semVal);
                htPaidInsert.Add("@isTermOrSem", 0);
                int insert = d2.insert_method("uspInsertUpdateFeeCatYearMatch", htPaidInsert, "sp");
                htPaidInsert.Clear();
                if (insert != 0)
                    boollSave = true;
            }
            if (boollSave)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

            }
        }
        catch { }
    }
    //year wise setting added by abarna 11.10.2017
    protected void GoFeeCat_click(object sender, EventArgs e)
    {
        gridviewSetting();
    }

    protected void btn_lnk_OnClick(object sender, EventArgs e)
    {
        feecatagorymatch.Visible = true;
        btn_savepop1.Visible = true;
        lblmonth.Visible = false;
        ddlmonth.Visible = false;
        btnmonth_save.Visible = false;
        BindFeecategory();
        bind_degree();
        ddl_feecatagory_selectedindex(sender, e);
        lbl_error1.Text = "";
    }
    protected void ddl_collegepop_Selectedindex(object sender, EventArgs e)
    {
        bind_degree();
    }
    protected void ddl_collegepop1_Selectedindex(object sender, EventArgs e)
    {//abarna
        bind_degree();
        bindYearWise();
        bindsem();
        bindTerm();
    }
    protected void ddl_feecatagory_selectedindex(object sender, EventArgs e)
    {
        string query = "  select FeeCategory,Degree_code from Fee_degree_match where FeeCategory='" + Convert.ToString(ddl_feecatagory.SelectedItem.Value) + "' and College_code='" + Convert.ToString(ddl_collegepop.SelectedItem.Value) + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        cbl_degree.ClearSelection();
        cb_degree.Checked = false;
        txt_degree.Text = "--Select--";
        if (ds.Tables[0].Rows.Count > 0)
        {
            int count = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //cbl_degree.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[i]["Degree_code"])).Selected = true;
                count++;
            }
            txt_degree.Text = lbl_degree.Text + "(" + count + ")";
        }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }
    void BindCollege()
    {
        try
        {
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

                ddl_collegepop.DataSource = ds;
                ddl_collegepop.DataTextField = "collname";
                ddl_collegepop.DataValueField = "college_code";
                ddl_collegepop.DataBind();
            }
        }
        catch
        {
        }
    }
    void BindCollegename()
    {
        try
        {
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

                ddl_collegepop1.DataSource = ds;
                ddl_collegepop1.DataTextField = "collname";
                ddl_collegepop1.DataValueField = "college_code";
                ddl_collegepop1.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void BindFeecategory()
    {
        if (ddl_collegepop.Items.Count > 0)
        {
            if (ddl_collegepop.SelectedItem.Value.Trim() != "")
            {
                string query = "  select textval,TextCode from textvaltable where TextCriteria ='FEECA' and college_code='" + Convert.ToString(ddl_collegepop.SelectedItem.Value) + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_feecatagory.DataSource = ds;
                    ddl_feecatagory.DataTextField = "textval";
                    ddl_feecatagory.DataValueField = "textcode";
                    ddl_feecatagory.DataBind();
                }
            }
        }
    }
    protected void bind_degree()
    {
        string rights = "";

        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
        {
            rights = "and group_code='" + group_user + "'";
        }
        else
        {
            rights = " and user_code='" + usercode + "'";
        }

        string query = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            cbl_degree.DataSource = ds;
            cbl_degree.DataTextField = "dept_name";
            cbl_degree.DataValueField = "degree_code";
            cbl_degree.DataBind();
        }
    }
    protected void btn_savepop1_click(object sender, EventArgs e)
    {
        if (cbl_degree.Items.Count > 0)
        {
            string q1 = ""; bool chk = false; int co = 0;

            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (co == 0)
                    {
                        q1 = " delete Fee_degree_match where College_code='" + ddl_collegepop.SelectedItem.Value.ToString() + "' and FeeCategory='" + Convert.ToString(ddl_feecatagory.SelectedItem.Value) + "' ";
                        int up1 = d2.update_method_wo_parameter(q1, "text");
                    }

                    q1 = "";
                    q1 = "if not exists(select * from Fee_degree_match where College_code='" + ddl_collegepop.SelectedItem.Value.ToString() + "' and FeeCategory='" + Convert.ToString(ddl_feecatagory.SelectedItem.Value) + "' and Degree_code='" + Convert.ToString(cbl_degree.Items[i].Value) + "') insert into Fee_degree_match(College_code,FeeCategory,Degree_code)values('" + ddl_collegepop.SelectedItem.Value.ToString() + "','" + Convert.ToString(ddl_feecatagory.SelectedItem.Value) + "','" + Convert.ToString(cbl_degree.Items[i].Value) + "')";
                    int up = d2.update_method_wo_parameter(q1, "text");
                    co++;
                    if (up != 0)
                    {
                        chk = true;
                    }
                }
            }
            if (chk == true)
            {
                lbl_error1.Text = "Saved Successfully";
            }
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;

    }
    protected void imagepopclose_click(object sender, EventArgs e)
    {
        popfine.Visible = false;
    }
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, "--Select--");
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text);
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
            int k = 0;
            string[] header = headername.Split('/');

            if (AutoPostBack.Trim().ToUpper() == "TRUE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = true;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (head.Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 50;
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 200;
                        }
                    }
                }
            }
            else if (AutoPostBack.Trim().ToUpper() == "FALSE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = false;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        string[] width = head.Split('-');
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (Convert.ToString(width[0]).Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error2.Text = ex.ToString();
        }
    }
    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lbl_degree);
        //lbl.Add(lbl_branch);
        //lbl.Add(lbl_sem);
        //lbl.Add(lbl_clg);
        fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);
        //fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    //added by sudhagar 08/12/2016  
    protected void btnlnkMonth_Click(object sender, EventArgs e)
    {
        feecatagorymatch.Visible = true;
        btn_savepop1.Visible = false;
        lblmonth.Visible = true;
        ddlmonth.Visible = true;
        btnmonth_save.Visible = true;
        //lblbatchYear.Visible = true;
        //ddlbatchYear.Visible = true;
        //lblYear.Visible = true;
        //ddlYear.Visible = true;
        BindFeecategory();
        bind_degree();
        BindMonth();
        lbl_error1.Text = "";
    }
    protected void BindMonth()
    {
        ddlmonth.Items.Clear();
        ddlmonth.Items.Add(new ListItem("JAN", "1"));
        ddlmonth.Items.Add(new ListItem("FEB", "2"));
        ddlmonth.Items.Add(new ListItem("MAR", "3"));
        ddlmonth.Items.Add(new ListItem("APR", "4"));
        ddlmonth.Items.Add(new ListItem("MAY", "5"));
        ddlmonth.Items.Add(new ListItem("JUN", "6"));
        ddlmonth.Items.Add(new ListItem("JUL", "7"));
        ddlmonth.Items.Add(new ListItem("AUG", "8"));
        ddlmonth.Items.Add(new ListItem("SEP", "9"));
        ddlmonth.Items.Add(new ListItem("OCT", "10"));
        ddlmonth.Items.Add(new ListItem("NOV", "11"));
        ddlmonth.Items.Add(new ListItem("DEC", "12"));
        ddlmonth.Items.Insert(0, "Select");
    }
    protected void btnmonth_save_Click(object seneder, EventArgs e)
    {
        lbl_error1.Text = "";
        saveDetails();
    }
    protected void saveDetails()
    {
        try
        {
            string month = string.Empty;
            if (ddlmonth.Items.Count > 0)
                month = Convert.ToString(ddlmonth.SelectedItem.Value);

            if (month != "Select" && cbl_degree.Items.Count > 0)
            {
                string q1 = ""; bool chk = false; int co = 0;
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        q1 = "if exists(select * from Fee_degree_match where College_code='" + ddl_collegepop.SelectedItem.Value.ToString() + "' and FeeCategory='" + Convert.ToString(ddl_feecatagory.SelectedItem.Value) + "' and Degree_code='" + Convert.ToString(cbl_degree.Items[i].Value) + "') update Fee_degree_match set Monthcode='" + month + "' where College_code='" + ddl_collegepop.SelectedItem.Value.ToString() + "' and FeeCategory='" + Convert.ToString(ddl_feecatagory.SelectedItem.Value) + "' and Degree_code='" + Convert.ToString(cbl_degree.Items[i].Value) + "' else insert into Fee_degree_match(College_code,FeeCategory,Degree_code,Monthcode)values('" + ddl_collegepop.SelectedItem.Value.ToString() + "','" + Convert.ToString(ddl_feecatagory.SelectedItem.Value) + "','" + Convert.ToString(cbl_degree.Items[i].Value) + "','" + month + "')";
                        int up = d2.update_method_wo_parameter(q1, "text");
                        co++;
                        if (up != 0)
                            chk = true;
                    }
                }
                if (chk == true)
                    lbl_error1.Text = "Saved Successfully";
            }
            else
                lbl_error1.Text = "Please Select Month";
        }
        catch { }
    }

    protected string getMonth(string monthcode)
    {
        string Month = string.Empty;
        try
        {
            switch (monthcode)
            {
                case "1":
                    Month = "JAN";
                    break;
                case "2":
                    Month = "FEB";
                    break;
                case "3":
                    Month = "MAR";
                    break;
                case "4":
                    Month = "APR";
                    break;
                case "5":
                    Month = "MAY";
                    break;
                case "6":
                    Month = "JUN";
                    break;
                case "7":
                    Month = "JUL";
                    break;
                case "8":
                    Month = "AUG";
                    break;
                case "9":
                    Month = "SEP";
                    break;
                case "10":
                    Month = "OCT";
                    break;
                case "11":
                    Month = "NOV";
                    break;
                case "12":
                    Month = "DEC";
                    break;
                default:
                    Month = "-";
                    break;

            }
        }
        catch { }
        return Month;
    }
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
    protected void gdattrpt_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gdReport.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gdReport.Rows[i];
                GridViewRow previousRow = gdReport.Rows[i - 1];
                for (int j = 0; j <= 2; j++)
                {
                    Label lnlname = new Label();
                    Label lnlname1 = new Label();
                    switch (j)
                    {
                        case 0:
                            lnlname = (Label)row.FindControl("lblsno");
                            lnlname1 = (Label)previousRow.FindControl("lblsno");
                            break;
                        case 1:
                            lnlname = (Label)row.FindControl("lblclg");
                            lnlname1 = (Label)previousRow.FindControl("lblclg");
                            break;
                        case 2:
                            lnlname = (Label)row.FindControl("lblYear");
                            lnlname1 = (Label)previousRow.FindControl("lblYear");
                            break;
                        //case 3:
                        //    lnlname = (Label)row.FindControl("lblbatch");
                        //    lnlname1 = (Label)previousRow.FindControl("lblbatch");
                        //    break;
                        //case 3:
                        //    lnlname = (Label)row.FindControl("lblSem");
                        //    lnlname1 = (Label)previousRow.FindControl("lblSem");
                        //    break;
                        case 5:
                            lnlname = (Label)row.FindControl("lblbutton");
                            lnlname1 = (Label)previousRow.FindControl("lblbutton");
                            break;
                    }
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                                previousRow.Cells[j].RowSpan += row.Cells[j].RowSpan + 2;
                            else
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
            for (int i = gdReport.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gdReport.Rows[i];
                GridViewRow previousRow = gdReport.Rows[i - 1];
                for (int j = 5; j <= 5; j++)
                {
                    Label lnlname = new Label();
                    Label lnlname1 = new Label();
                    switch (j)
                    {

                        case 5:
                            lnlname = (Label)row.FindControl("lblbutton");
                            lnlname1 = (Label)previousRow.FindControl("lblbutton");
                            break;
                    }
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                                previousRow.Cells[j].RowSpan += row.Cells[j].RowSpan + 2;
                            else
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void gdReport_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string value = "Delet$" + e.Row.RowIndex;

                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.gdReport, "Delet$" + e.Row.RowIndex);
            }
        }
        catch { }
    }
    protected void gridviewSetting()
    {
        gdReport.Visible = false;
        DataTable dtReport = new DataTable();
        string colgCode = Convert.ToString(ddl_collegepop1.SelectedValue);
        dtReport.Columns.Add("Sno");
        dtReport.Columns.Add("collegeStr");
        dtReport.Columns.Add("collegeVal");
        dtReport.Columns.Add("Year");
        // dtReport.Columns.Add("YearVal");
        dtReport.Columns.Add("semester");
        dtReport.Columns.Add("semesterVal");
        dtReport.Columns.Add("TermStr");
        dtReport.Columns.Add("TermVal");
        dtReport.Columns.Add("button");

        string selQ = "select t.Textval,c.collname,yearcode,f.collegeCode,semestertermcode,istermorsemester  from FeeCatagoryYearMatching f,textvaltable t,collinfo c where textcriteria='FEECA' and c.college_code ='" + colgCode + "' and t.textcode=f.SemesterTermcode and c.college_code=f.collegeCode";
        DataSet dsGrid = d2.select_method_wo_parameter(selQ, "Text");
        if (dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
        {
            DataTable dtMain = dsGrid.Tables[0].DefaultView.ToTable();
            DataTable dtYear = dsGrid.Tables[0].DefaultView.ToTable(true, "yearcode");
            DataTable dtSemOrTerm = dsGrid.Tables[0].DefaultView.ToTable(true, "istermorsemester", "yearcode");
            int rowCnt = 0;
            for (int Yr = 0; Yr < dtYear.Rows.Count; Yr++)
            {
                dtSemOrTerm.DefaultView.RowFilter = "yearcode='" + Convert.ToString(dtYear.Rows[Yr]["yearcode"]) + "'";
                DataTable dtSem = dtSemOrTerm.DefaultView.ToTable();
                if (dtSem.Rows.Count > 0)
                {
                    int rowCntCheck = 0;
                    // int tempFnlCnt = 0;
                    for (int sTr = 0; sTr < dtSem.Rows.Count; sTr++)
                    {
                        dtMain.DefaultView.RowFilter = "yearcode='" + Convert.ToString(dtYear.Rows[Yr]["yearcode"]) + "' and istermorsemester='" + Convert.ToString(dtSem.Rows[sTr]["istermorsemester"]) + "'";
                        DataTable dtMainD = dtMain.DefaultView.ToTable();
                        if (dtMainD.Rows.Count > 0)
                        {
                            int semTrm = 0;
                            int.TryParse(Convert.ToString(dtSem.Rows[sTr]["istermorsemester"]), out semTrm);

                            for (int Mn = 0; Mn < dtMainD.Rows.Count; Mn++)
                            {
                                DataRow dr;
                                if (semTrm == 1)
                                {
                                    dr = dtReport.NewRow();
                                    dr["Sno"] = Convert.ToString(++rowCnt);
                                    dr["collegeStr"] = Convert.ToString(dtMainD.Rows[Mn]["collname"]);
                                    dr["collegeVal"] = Convert.ToString(dtMainD.Rows[Mn]["collegeCode"]);
                                    dr["Year"] = Convert.ToString(dtMainD.Rows[Mn]["yearcode"]);
                                    //}
                                    //// dr["YearVal"] = Convert.ToString(dtYear.Rows[Yr][""]);
                                    //if (semTrm == 1)
                                    //{
                                    dr["semester"] = Convert.ToString(dtMainD.Rows[Mn]["Textval"]);
                                    dr["semesterVal"] = Convert.ToString(dtMainD.Rows[Mn]["semestertermcode"]);
                                    dr["button"] = Convert.ToString(dtMainD.Rows[Mn]["yearcode"]);
                                    dtReport.Rows.Add(dr);
                                    rowCntCheck++;
                                    //tempFnlCnt++;
                                }
                                else
                                {
                                    if (rowCntCheck == 0)
                                    {
                                        dr = dtReport.NewRow();
                                        dr["Sno"] = Convert.ToString(++rowCnt);
                                        dr["collegeStr"] = Convert.ToString(dtMainD.Rows[Mn]["collname"]);
                                        dr["collegeVal"] = Convert.ToString(dtMainD.Rows[Mn]["collegeCode"]);
                                        dr["Year"] = Convert.ToString(dtMainD.Rows[Mn]["yearcode"]);

                                        dr["TermStr"] = Convert.ToString(dtMainD.Rows[Mn]["Textval"]);
                                        dr["TermVal"] = Convert.ToString(dtMainD.Rows[Mn]["semestertermcode"]);
                                        dr["button"] = Convert.ToString(dtMainD.Rows[Mn]["yearcode"]);
                                        dtReport.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        int tblCount = 0;
                                        int.TryParse(Convert.ToString(dtReport.Rows.Count), out tblCount);
                                        int fnlCnt = tblCount - rowCntCheck;
                                        dtReport.Rows[fnlCnt]["TermStr"] = Convert.ToString(dtMainD.Rows[Mn]["Textval"]);
                                        dtReport.Rows[fnlCnt]["TermVal"] = Convert.ToString(dtMainD.Rows[Mn]["semestertermcode"]);
                                        rowCntCheck--;
                                    }
                                }

                            }
                        }

                    }
                }


            }
            if (dtReport.Rows.Count > 0)
            {
                gdReport.DataSource = dtReport;
                gdReport.DataBind();
                gdReport.Visible = true;
                popfine.Visible = true;
            }

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
        }

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        bool boollSave = false;
        int rowindex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
        if (gdReport.Rows.Count > 0)
        {
            int rowcnt = 0;
            foreach (GridViewRow gvpopro in gdReport.Rows)
            {
                if (rowindex == rowcnt)
                {

                    Hashtable htPaidInsert = new Hashtable();
                    Label Code = (Label)gvpopro.Cells[1].FindControl("lblclgVal");
                    Label Year = (Label)gvpopro.Cells[2].FindControl("lblYear");
                    htPaidInsert.Add("@collegeCode", Code.Text);
                    htPaidInsert.Add("@yearCode", Year.Text);
                    int delete = d2.insert_method("uspDeleteFeeCatYearMatching", htPaidInsert, "sp");

                    if (delete != 0)
                        boollSave = true;
                }

                if (boollSave)
                {

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Delete Successfully')", true);
                    gridviewSetting();
                }
                rowcnt++;
            }
        }
    }
}