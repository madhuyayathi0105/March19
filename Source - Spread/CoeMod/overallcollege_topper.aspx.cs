using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;

public partial class overallcollege_topper : System.Web.UI.Page
{

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    DataSet dsfind = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            lbl_errmsg.Visible = false;
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                clear();
                bindcollege();
                BindBatch();
                BindDegree();
                BindBranchMultiple();
                loadmonth();
                bindexamyear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void loadmonth()
    {
        ddlmonth.Items.Clear();
        ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
        ddlmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
        ddlmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
        ddlmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
        ddlmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
        ddlmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
        ddlmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
        ddlmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
        ddlmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
        ddlmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
        ddlmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
        ddlmonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
    }
    protected void ddlclg_click(object sender, EventArgs e)
    {
        clear();
        loadmonth();
        bindexamyear();
        BindBatch();
        BindDegree();
        BindBranchMultiple();
    }

    public void BindBatch()
    {
        try
        {
            chklsbatch.Items.Clear();
            chkbatch.Checked = false;
            txtbatch.Text = "---Select---";
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();

                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                chkbatch.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    public void bindexamyear()
    {
        try
        {
            ddlyear.Items.Clear();
            string batchquery = "select distinct Exam_year  from Exam_Details order by  Exam_year asc ";
            DataSet dsbindexamyear = d2.select_method(batchquery, hat, "text ");
            if (dsbindexamyear.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = dsbindexamyear;
                ddlyear.DataTextField = "Exam_year";
                ddlyear.DataValueField = "Exam_year";
                ddlyear.DataBind();
                ddlyear.Items.Insert(0, " ");
            }
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            chklstdegree.Items.Clear();
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            collegecode = ddlclg.SelectedItem.Value;

            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();

                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                chkdegree.Checked = true;
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    public void BindBranchMultiple()
    {
        try
        {
            txtbranch.Text = "---Select---";
            chkbranch.Checked = false;
            chklstbranch.Items.Clear();
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            collegecode = ddlclg.SelectedValue.ToString();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                chkbranch.Checked = true;
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    protected void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(";"))
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
            ddlclg.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
            }
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtbatch.Text = "---Select---";
            chkbatch.Checked = false;
            int commcount = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == chklsbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }
            BindDegree();
            BindBranchMultiple();
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                txtbranch.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }
    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int commcount = 0;
            txtbranch.Text = "---Select---";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
            }
            BindBranchMultiple();
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + (commcount) + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }
            BindBranchMultiple();
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    protected void lbl_Logout(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }
    protected void ddlyear_onselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            string strsql = "select distinct Exam_Month  from Exam_Details  where exam_year='" + ddlyear.SelectedValue.ToString() + "'";
            ds = d2.select_method_wo_parameter(strsql, "Text");
            ddlmonth.Items.Clear();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int month = Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString());
                if (month == 1)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                }
                if (month == 2)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                }
                if (month == 3)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                }
                if (month == 4)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                }
                if (month == 5)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("May", "5"));
                }
                if (month == 6)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                }
                if (month == 7)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                }
                if (month == 8)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                }
                if (month == 9)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                }
                if (month == 10)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                }
                if (month == 11)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                }
                if (month == 12)
                {
                    ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                }
            }
            ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }

    protected void ddlmonth_onselected(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }
    protected void btn_go(object sender, EventArgs e)
    {
        try
        {
            clear();
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = Color.White;
            style2.BackColor = Color.Teal;
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            DataSet dsrank = new DataSet();
            Hashtable hatstutotal = new Hashtable();
            DataSet dsexam = new DataSet();
            DataView dvcount = new DataView();

            string batchcount = "";
            string sqlbatch = "";
            string branchcount = "";
            string sqlbranch = "";
            string exam_year = "";
            string exam_month = "";
            collegecode = ddlclg.SelectedItem.Value;

            for (int itemcount = 0; itemcount < chklsbatch.Items.Count; itemcount++)
            {
                if (chklsbatch.Items[itemcount].Selected == true)
                {
                    if (batchcount == "")
                    {
                        batchcount = "'" + chklsbatch.Items[itemcount].Value.ToString() + "'";
                    }
                    else
                    {
                        batchcount = batchcount + ",'" + chklsbatch.Items[itemcount].Value.ToString() + "'";
                    }
                }
            }

            if (batchcount != "")
            {
                sqlbatch = " and r.batch_year in(" + batchcount + ")";
            }
            else
            {
                lbl_errmsg.Visible = true;
                lbl_errmsg.Text = "Please Select The Batch And Then Proceed";
                return;
            }

            for (int itemcount = 0; itemcount < chklstbranch.Items.Count; itemcount++)
            {
                if (chklstbranch.Items[itemcount].Selected == true)
                {
                    if (branchcount == "")
                    {
                        branchcount = "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";

                    }
                    else
                    {
                        branchcount = branchcount + ",'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                    }
                }
            }
            if (branchcount != "")
            {
                sqlbranch = " and r.degree_code in(" + branchcount + ")";
            }
            else
            {
                lbl_errmsg.Visible = true;
                lbl_errmsg.Text = "Please Select The Degree and Branch And Then Proceed";
                return;
            }

            if (ddlyear.Items.Count > 0)
            {
                if (ddlyear.SelectedItem.ToString().Trim() != "")
                {
                    exam_year = ddlyear.SelectedValue.ToString();
                }
                else
                {
                    lbl_errmsg.Visible = true;
                    lbl_errmsg.Text = "Please Select The Exam Year And Then Proceed";
                    return;
                }
            }
            else
            {
                lbl_errmsg.Visible = true;
                lbl_errmsg.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }

            if (ddlmonth.Items.Count > 0)
            {
                if (ddlmonth.SelectedItem.ToString().Trim() != "")
                {
                    exam_month = ddlmonth.SelectedValue.ToString();
                }
                else
                {
                    lbl_errmsg.Visible = true;
                    lbl_errmsg.Text = "Please Select The Exam Month And Then Proceed";
                    return;
                }
            }
            else
            {
                lbl_errmsg.Visible = true;
                lbl_errmsg.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }

            Double cgpafrom = 0;
            Double cgpato = 0;
            string strfrange = txt_rangefrom.Text.ToString();
            if (strfrange.Trim() != "")
            {
                cgpafrom = Convert.ToDouble(strfrange);
            }
            else
            {
                lbl_errmsg.Visible = true;
                lbl_errmsg.Text = "Please Select The CGPA From Range And Then Proceed";
                return;
            }

            string strtrange = txt_to.Text.ToString();
            if (strtrange.Trim() != "")
            {
                cgpato = Convert.ToDouble(strtrange);
            }
            else
            {
                lbl_errmsg.Visible = true;
                lbl_errmsg.Text = "Please Select The CGPA To Range And Then Proceed";
                return;
            }

            if (cgpato < cgpafrom)
            {
                lbl_errmsg.Visible = true;
                lbl_errmsg.Text = "Please Enter The CGPA To Range Must Be Greater Then From Range";
                return;
            }

            string query = "select distinct r.stud_name,r.Reg_No,r.batch_year,dg.Acronym,course.Course_Name,ed.current_semester,r.sections,r.degree_code,d.dept_name,course.course_name,dg.course_id,r.mode ,r.roll_no,r.degree_code,r.mode from registration r,mark_entry me ,department d,course,degree dg,Exam_Details ed where me.roll_no=r.roll_no and r.degree_code=dg.degree_code and dg.course_id = course.course_id and dg.dept_code = d.dept_code and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.result='pass' and ed.Exam_Month='" + exam_month + "' and ed.Exam_year='" + exam_year + "' " + sqlbatch + " " + sqlbranch + " order by r.degree_code,r.Batch_Year";
            string getbranchvalue = "select distinct d.dept_acronym,c.Course_Name,dg.Degree_Code from registration r,mark_entry me ,department d,course c,degree dg,Exam_Details ed where me.roll_no=r.roll_no and r.degree_code=dg.degree_code and dg.course_id = c.course_id and dg.dept_code = d.dept_code and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and me.result='pass' and ed.Exam_Month='" + exam_month + "' and ed.Exam_year='" + exam_year + "' " + sqlbatch + " " + sqlbranch + " order by c.Course_Name,d.dept_acronym";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(query, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataSet dstable = d2.select_method("select * from sysobjects where name='tbl_Topperrank' and Type='U'", hat, "text ");
                if (dstable.Tables[0].Rows.Count == 0)
                {
                    int p = d2.insert_method("create table tbl_Topperrank (roll_no nvarchar(50),cgpa float (8),stud_name nvarchar(200),degree nvarchar(500),user_code nvarchar(25))", hat, "text");
                }
                else
                {
                    int p = d2.insert_method("IF not EXISTS (SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tbl_Topperrank' AND COLUMN_NAME = 'user_code') alter table tbl_Topperrank add user_code nvarchar(15)", hat, "text");
                }
                ds.Dispose();
                dsfind = d2.select_method("select name from sysobjects where xtype='p' and name='sp_ins_upd_topperrank' ", hat, "text");
                if (dsfind.Tables[0].Rows.Count == 0)
                {
                    string spcreation = " CREATE procedure sp_ins_upd_topperrank (@RollNumber varchar(50), @cgpa varchar(20), @stud_name varchar(20), @degree varchar(200) ,@user_code nvarchar(25))  as  declare @cou_nt  int set @cou_nt=(select count(Roll_no)from tbl_Topperrank where Roll_no=@RollNumber) if @cou_nt=0 BEGIN insert into tbl_Topperrank(Roll_no,cgpa,stud_name,degree,user_code) values (@RollNumber,@cgpa,@stud_name,@degree,@user_code) End Else BEGIN update  tbl_Topperrank set cgpa=@cgpa where Roll_no=@RollNumber and user_code=@user_code end";
                    int s = d2.insert_method(spcreation, hat, "Text");
                }
                else
                {
                    string spalter = " alter procedure sp_ins_upd_topperrank (@RollNumber varchar(50), @cgpa   varchar(20), @stud_name varchar(20), @degree varchar(200) ,@user_code nvarchar(25))    as  declare @cou_nt  int set @cou_nt=(select count(Roll_no)from tbl_Topperrank    where Roll_no=@RollNumber) if @cou_nt=0 BEGIN insert into tbl_Topperrank(Roll_no,   cgpa,stud_name,degree,user_code)values(@RollNumber,@cgpa,@stud_name,@degree,   @user_code) End Else BEGIN update  tbl_Topperrank set cgpa=@cgpa where    Roll_no=@RollNumber and user_code=@user_code End";
                    int gf = d2.insert_method(spalter, hat, "Text");
                }

                Boolean rowflag = false;
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "CGPA";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                string tempdegree = "";
                DataSet dsdegree = d2.select_method_wo_parameter(getbranchvalue, "text");
                int spancolumn = 0;
                Boolean valkflag = false;
                for (int d = 0; d < dsdegree.Tables[0].Rows.Count; d++)
                {
                    string strdegree = dsdegree.Tables[0].Rows[d]["dept_acronym"].ToString();
                    string Course = dsdegree.Tables[0].Rows[d]["Course_Name"].ToString();
                    if (tempdegree != Course || d == dsdegree.Tables[0].Rows.Count - 1)
                    {
                        if (tempdegree != "")
                        {
                            if (tempdegree == Course && d == dsdegree.Tables[0].Rows.Count - 1)
                            {
                                spancolumn++;
                                valkflag = true;
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Course;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = strdegree;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = dsdegree.Tables[0].Rows[d]["Degree_Code"].ToString();
                            }
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - spancolumn, 1, spancolumn);
                        }
                        spancolumn = 0;
                        tempdegree = Course;
                    }
                    if (valkflag == false)
                    {
                        spancolumn++;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Course;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = strdegree;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = dsdegree.Tables[0].Rows[d]["Degree_Code"].ToString();
                    }
                }

                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                for (int y = 0; y < chklsbatch.Items.Count; y++)
                {
                    if (chklsbatch.Items[y].Selected == true)
                    {
                        string batch = chklsbatch.Items[y].Text.ToString();
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Batch " + batch + "";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.AliceBlue;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                        int startow = FpSpread1.Sheets[0].RowCount;
                        for (Double dc = cgpato; dc >= cgpafrom; dc = dc - 0.5)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txt;
                            if (dc == cgpato)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ">" + dc.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = " and cgpa >" + dc + "";
                            }
                            else
                            {
                                Double doc1 = dc + 0.5;
                                Double doc2 = dc + 0.001;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dc + " - " + doc1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = " and cgpa between " + doc2 + " and " + doc1 + "";
                            }
                        }

                        for (int c = 1; c < FpSpread1.Sheets[0].ColumnCount - 1; c++)
                        {
                            string degreecode = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "Batch_year='" + batch + "' and degree_code='" + degreecode + "'";
                            DataView dvstud = ds.Tables[0].DefaultView;
                            for (int s = 0; s < dvstud.Count; s++)
                            {
                                string rollno = dvstud[s]["roll_no"].ToString();
                                string sem = dvstud[s]["current_semester"].ToString();
                                string mode = dvstud[s]["mode"].ToString();
                                string name = dvstud[s]["stud_name"].ToString();
                                string degreevalue = dvstud[s]["Acronym"].ToString();
                                int failcount = Convert.ToInt32(d2.GetFunction(" Select COUNT(*) from Mark_Entry,Subject where  Mark_Entry.Subject_No = Subject.Subject_No and roll_no='" + rollno + "' and result='fail' and result='Fail'  "));
                                if (failcount == 0)
                                {
                                    string cgpav = d2.Calculete_CGPA(rollno, sem, degreecode, batch, mode, collegecode);
                                    if (cgpav != "0" && cgpav != "" && cgpav != "-" && cgpav != "NaN")
                                    {
                                        Double num = 0;
                                        if (Double.TryParse(cgpav, out num))
                                        {
                                            hat.Clear();
                                            hat.Add("RollNumber", rollno);
                                            hat.Add("cgpa", cgpav.ToString());
                                            hat.Add("stud_name", name.ToString());
                                            hat.Add("degree", degreevalue.ToString());
                                            hat.Add("user_code", usercode.ToString());
                                            int o = d2.insert_method("sp_ins_upd_topperrank", hat, "sp");
                                        }
                                    }
                                }
                            }
                            for (int r = startow; r < FpSpread1.Sheets[0].RowCount; r++)
                            {
                                string valran = FpSpread1.Sheets[0].Cells[r, 0].Note;
                                string getstucount = d2.GetFunction("select count(roll_no) from tbl_Topperrank where user_code='" + usercode + "' " + valran + "");
                                if (getstucount.Trim() != "0" && getstucount.Trim() != "")
                                {
                                    rowflag = true;
                                }
                                FpSpread1.Sheets[0].Cells[r, c].CellType = txt;
                                FpSpread1.Sheets[0].Cells[r, c].Text = getstucount;
                                FpSpread1.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                            }
                            int p = d2.insert_method("Delete from tbl_Topperrank where user_code='" + usercode + "' ", hat, "text");
                        }
                    }
                }
                for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                {
                    int totalstudent = 0;
                    for (int c = 1; c < FpSpread1.Sheets[0].ColumnCount - 1; c++)
                    {
                        string rowsvalue = FpSpread1.Sheets[0].Cells[r, c].Text.ToString();
                        if (rowsvalue.Trim() != "")
                        {
                            totalstudent = totalstudent + Convert.ToInt32(rowsvalue);
                        }
                        FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].ColumnCount - 1].Text = totalstudent.ToString();
                        FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

                if (rowflag == true)
                {
                    FpSpread1.Visible = true;
                    lbl_rptname.Visible = true;
                    btn_excel.Visible = true;
                    btn_print.Visible = true;
                    txt_rpt.Visible = true;
                }
                else
                {
                    lbl_errmsg.Text = "No Records Found";
                    lbl_errmsg.Visible = true;
                }
            }
            else
            {
                lbl_errmsg.Text = "No Records Found";
                lbl_errmsg.Visible = true;
            }

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lbl_errmsg.Text = ex.ToString();
            lbl_errmsg.Visible = true;
        }
    }
    protected void btn_excelname(object sender, EventArgs e)
    {
        if (txt_rpt.Text == "")
        {
            lbl_reptnoname.Visible = true;
            lbl_reptnoname.Text = "Please Enter the Report Name";
        }
        else
        {
            lbl_reptnoname.Visible = false;
            lbl_reptnoname.Text = "";
            string reportname = txt_rpt.Text;
            d2.printexcelreport(FpSpread1, reportname);
        }
    }

    protected void btn_printcmn(object sender, EventArgs e)
    {

        lbl_reptnoname.Visible = false;
        lbl_reptnoname.Text = "";
        FpSpread1.Visible = true;
        string pagename = "overallcollege_topper.aspx";
        string degreedetails = "Overall College Topper List";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    public void clear()
    {
        lbl_errmsg.Visible = false;
        txt_rpt.Visible = false;
        txt_rpt.Text = "";
        lbl_rptname.Visible = false;
        btn_print.Visible = false;
        btn_excel.Visible = false;
        FpSpread1.Visible = false;
        Printcontrol.Visible = false;
    }
}