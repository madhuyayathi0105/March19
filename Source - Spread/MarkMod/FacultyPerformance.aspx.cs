using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class FacultyPerformance : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    Hashtable hat = new Hashtable();
    ArrayList arinternal = new ArrayList();
    ArrayList arexternal = new ArrayList();
    Boolean resultflag = false;
    string collegecode = "";
    string course_id = "";
    string dept_id = "";
    int commcount = 0;
    string usercode = "";
    Boolean checkflag = false;

    #region "Loading Details"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Request.FilePath.Contains("CAMHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/MarkMod/CAMHome.aspx");
                return;
            }
        }
        errmsg.Visible = false;
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            chktopbelow.Checked = false;
            chktopbelow_CheckedChanged(sender, e);
            btnprint.Visible = false;
            txtexcelname.Visible = false;
            btnexcel.Visible = false;
            txtexcelname.Text = "";
            FpReport.Visible = false;
            FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;

            FpEntry.Visible = false;
            lblexcel.Visible = false;
            txtxl.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            FpEntry.Sheets[0].AutoPostBack = true;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Name = "Book Antiqua";
            style.HorizontalAlign = HorizontalAlign.Center;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpEntry.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpEntry.Sheets[0].AllowTableCorner = true;
            //lblrange.Visible = false;
            //txtrange.Visible = false;

            FpReport.Width = 830;
            // FpReport.Width = 830;
            FpReport.Sheets[0].AutoPostBack = true;
            FpReport.CommandBar.Visible = true;
            FpReport.Sheets[0].SheetName = " ";
            FpReport.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FpReport.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpReport.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            FpReport.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpReport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpReport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpReport.Sheets[0].DefaultStyle.Font.Bold = false;

            FpReport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpReport.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpReport.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpReport.Sheets[0].AllowTableCorner = true;

            //---------------page number

            FpReport.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            FpReport.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            FpReport.Pager.Align = HorizontalAlign.Right;
            FpReport.Pager.Font.Bold = true;
            FpReport.Pager.Font.Name = "Book Antiqua";
            FpReport.Pager.ForeColor = System.Drawing.Color.DarkGreen;
            FpReport.Pager.BackColor = System.Drawing.Color.Beige;
            FpReport.Pager.BackColor = System.Drawing.Color.AliceBlue;
            FpReport.Pager.PageCount = 100;


            BindDepartment();
            BindDesignation();
            BindStaff();
            baindyear();
            errmsg.Visible = false;
            rbtop.Checked = false;
            rbbelow.Checked = false;
        }
    }

    //Load Designation
    public void BindDesignation()
    {
        ds.Dispose();
        ds.Reset();
        ds = d2.binddesi(collegecode);
        chklsdesign.DataSource = ds;
        chklsdesign.DataValueField = "desig_code";
        chklsdesign.DataTextField = "desig_name";
        chklsdesign.DataBind();
    }

    //Load Department
    public void BindDepartment()
    {
        ds.Dispose();
        ds.Reset();
        ds = d2.loaddepartment(collegecode);
        chklsdept.DataSource = ds;
        chklsdept.DataTextField = "dept_name";
        chklsdept.DataValueField = "Dept_Code";
        chklsdept.DataBind();
    }

    //Load Staff
    public void BindStaff()
    {
        chklsstaff.Items.Clear();
        txtstaff.Text = "---Select---";
        chkstaff.Checked = false;
        course_id = "";
        dept_id = "";
        for (int i = 0; i < chklsdesign.Items.Count; i++)
        {
            if (chklsdesign.Items[i].Selected == true)
            {
                if (course_id == "")
                {
                    course_id = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                }
                else
                {
                    course_id = "" + course_id + "," + "'" + chklsdesign.Items[i].Value.ToString() + "'";
                }
            }
        }


        for (int i = 0; i < chklsdept.Items.Count; i++)
        {
            if (chklsdept.Items[i].Selected == true)
            {
                if (dept_id == "")
                {
                    dept_id = "'" + chklsdept.Items[i].Value.ToString() + "'";
                }
                else
                {
                    dept_id = dept_id + "," + "'" + chklsdept.Items[i].Value.ToString() + "'";
                }
            }
        }
        if (dept_id == "" || dept_id == null)
        {
            dept_id = "''";
        }

        if (course_id == "" || course_id == null)
        {
            course_id = "''";
        }
        ds.Dispose();
        ds.Reset();
        ds = d2.bindstaffnme(collegecode, course_id, dept_id);
        if (ds.Tables[0].Rows.Count > 0)
        {
            chklsstaff.Enabled = true;
            chklsstaff.DataSource = ds;
            chklsstaff.DataTextField = "staffnamecode"; //gowthaman 26july2013 "staff_name";
            chklsstaff.DataValueField = "staff_code";
            chklsstaff.DataBind();
        }
        else
        {
            chklsstaff.Items.Clear();
            chklsstaff.Enabled = false;
        }
    }

    //Load Year
    public void baindyear()
    {
        ds.Dispose();
        ds.Reset();
        ds = d2.BindBatch();
        ddlbatch.DataSource = ds;
        ddlbatch.DataValueField = "batch_year";
        ddlbatch.DataTextField = "batch_year";
        ddlbatch.DataBind();
        ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
        int yeard = Convert.ToInt32(ddlbatch.SelectedValue.ToString());
        yeard++;
        ddlbatch.Items.Add(Convert.ToString(yeard));
    }

    //protected void rblrange_SelectedChange(object sender, EventArgs e)
    //{
    //    if (rblrange.SelectedValue == "1")
    //    {
    //        lblrange.Text = "Top";
    //        lblrange.Visible = false;
    //        txtrange.Visible = true;
    //    }
    //    if (rblrange.SelectedValue == "2")
    //    {
    //        lblrange.Text = "Below";
    //        lblrange.Visible = true;
    //        txtrange.Visible = true;
    //    }
    //}
    protected void chklsdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        commcount = 0;
        txtdept.Text = "---Select---";
        chkdept.Checked = false;
        for (int i = 0; i < chklsdept.Items.Count; i++)
        {
            if (chklsdept.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtdept.Text = "Dept(" + commcount.ToString() + ")";
            if (chklsdept.Items.Count == commcount)
            {
                chkdept.Checked = true;
            }
        }
        BindStaff();
    }
    protected void chkdept_ChekedChange(object sender, EventArgs e)
    {
        if (chkdept.Checked == true)
        {
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                chklsdept.Items[i].Selected = true;
            }
            txtdept.Text = "Dept(" + chklsdept.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                chklsdept.Items[i].Selected = false;
            }
            txtdept.Text = "--Select--";
        }
        BindStaff();
    }
    protected void chkdesign_ChekedChange(object sender, EventArgs e)
    {
        if (chkdesign.Checked == true)
        {
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                chklsdesign.Items[i].Selected = true;
            }
            txtdesign.Text = "Desgn(" + chklsdesign.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                chklsdesign.Items[i].Selected = false;
            }
            txtdesign.Text = "--Select--";
        }
        BindStaff();
    }
    protected void chklsdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtdesign.Text = "--Select--";
        chkdesign.Checked = false;
        commcount = 0;
        for (int i = 0; i < chklsdesign.Items.Count; i++)
        {
            if (chklsdesign.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtdesign.Text = "Desgn(" + commcount.ToString() + ")";
            if (chklsdesign.Items.Count == commcount)
            {
                chkdesign.Checked = true;
            }
        }
        BindStaff();
    }
    protected void chkstaff_ChekedChange(object sender, EventArgs e)
    {
        if (chkstaff.Checked == true)
        {
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                chklsstaff.Items[i].Selected = true;
            }
            txtstaff.Text = "Staff(" + chklsstaff.Items.Count.ToString() + ")";
        }
        else
        {
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                chklsstaff.Items[i].Selected = false;
            }
            txtstaff.Text = "--Select--";
        }
    }
    protected void chklsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtstaff.Text = "--Select--";
        chkstaff.Checked = false;
        for (int i = 0; i < chklsstaff.Items.Count; i++)
        {
            if (chklsstaff.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtstaff.Text = "Staff(" + commcount.ToString() + ")";
            if (chklsstaff.Items.Count == commcount)
            {
                chkstaff.Checked = true;
            }
        }
    }

    protected void rbbelow_CheckedChange(object sender, EventArgs e)
    {
        lblrange.Text = "Below";
        lblrange.Visible = true;
        txtrange.Visible = true;
    }
    protected void rbtop_CheckedChange(object sender, EventArgs e)
    {
        lblrange.Text = "Top";
        lblrange.Visible = true;
        txtrange.Visible = true;
    }


    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            btnprint.Visible = false;
            txtexcelname.Visible = false;
            btnexcel.Visible = false;
            txtexcelname.Text = "";
            lblnorec.Visible = false;
            errmsg.Visible = false;
            FpEntry.Visible = false;
            lblexcel.Visible = false;
            txtxl.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            FpReport.Visible = false;
            Printcontrol.Visible = false;
            FpEntry.Sheets[0].ColumnCount = 0;
            ds.Dispose();
            ds = d2.select_method("select * from sysobjects where name='tbl_staff_topper' and Type='U'", hat, "text ");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //int q = d2.insert_method("drop table tbl_staff_topper", hat, "text");
                int p = d2.insert_method("IF not EXISTS (SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tbl_staff_topper' AND COLUMN_NAME = 'user_code') alter table tbl_staff_topper add user_code nvarchar(25)", hat, "text");
                //int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int)", hat, "text");
            }
            else
            {
                int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int,user_code nvarchar(25))", hat, "text");
            }

            int strdelexistval = d2.update_method_wo_parameter("delete from tbl_staff_topper where user_code='" + usercode + "'", "Text");
            if (ChkIndividual.Checked == false)
            {
                if (ddlorder.SelectedItem.Text == "Dept.")
                {
                    ds.Dispose();
                    string staffcode = "";
                    for (int i = 0; i < chklsstaff.Items.Count; i++)
                    {
                        if (chklsstaff.Items[i].Selected == true)
                        {
                            if (staffcode == "")
                            {
                                staffcode = "'" + chklsstaff.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                staffcode = " " + staffcode + ",'" + chklsstaff.Items[i].Value.ToString() + "'";
                            }
                        }
                    }
                    if (staffcode != "")
                    {


                        staffcode = staffcode.ToString().Trim();

                        if (ddlexam.SelectedIndex.ToString() == "1")
                        {
                            loadreport(staffcode);
                        }
                        else if (ddlexam.SelectedIndex.ToString() == "2")
                        {
                            loadexternal(staffcode);
                        }
                        else
                        {
                            loadexternal(staffcode);
                            loadreport(staffcode);
                        }

                        loadHeader();

                        ds.Dispose();
                        ds.Reset();

                        string loadquery = "";
                        if (ddlexam.SelectedIndex.ToString() == "1")
                        {
                            FpEntry.Sheets[0].Columns[4].Visible = true;
                            FpEntry.Sheets[0].Columns[5].Visible = true;
                            FpEntry.Sheets[0].Columns[6].Visible = false;
                            FpEntry.Sheets[0].Columns[7].Visible = false;
                            //gowthaman 26july2013 ===============================
                            //if (rbbelow.Checked == true)
                            //    loadquery = "select rank() over(order by (sum(in_pass)/sum(in_appear) * 100) asc) as rank,(sum(in_pass)/sum(in_appear) * 100) as internalpass, (sum(in_fail)/sum(in_appear) * 100 ) as internalfail,sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 group by staff_code,degree,staff_name";
                            //else
                            //    loadquery = "select rank() over(order by (sum(in_pass)/sum(in_appear) * 100) desc) as rank,(sum(in_pass)/sum(in_appear) * 100) as internalpass, (sum(in_fail)/sum(in_appear) * 100 ) as internalfail,sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 group by staff_code,degree,staff_name";
                            if (rbbelow.Checked == true)
                                loadquery = "select rank() over(order by isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) asc) as rank,    isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) as internalpass,   isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0) as internalfail,  sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 and user_code='" + usercode + "' group by staff_code,degree,staff_name";
                            else
                                loadquery = "select rank() over(order by isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) desc) as rank,    isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) as internalpass,   isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0) as internalfail,  sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 and user_code='" + usercode + "' group by staff_code,degree,staff_name";

                            //=====================================================

                        }
                        else if (ddlexam.SelectedIndex.ToString() == "2")
                        {
                            FpEntry.Sheets[0].Columns[4].Visible = false;
                            FpEntry.Sheets[0].Columns[5].Visible = false;
                            FpEntry.Sheets[0].Columns[6].Visible = true;
                            FpEntry.Sheets[0].Columns[7].Visible = true;
                            //gowthaman 26july2013 ===============================
                            //if (rbbelow.Checked == true)
                            //    loadquery = "select rank() over(order by (sum(ext_pass)/sum(ext_appear) * 100) asc) as rank,(sum(ext_pass)/sum(ext_appear) * 100) as externalpass, (sum(ext_fail)/sum(ext_appear) * 100 ) as externalfail,sum(ext_pass) as pass,sum(ext_appear) as externalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=1 group by staff_code,degree,staff_name";
                            //else
                            //    loadquery = "select rank() over(order by (sum(ext_pass)/sum(ext_appear) * 100) desc) as rank,(sum(ext_pass)/sum(ext_appear) * 100) as externalpass, (sum(ext_fail)/sum(ext_appear) * 100 ) as externalfail,sum(ext_pass) as pass,sum(ext_appear) as externalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=1 group by staff_code,degree,staff_name";
                            if (rbbelow.Checked == true)
                                loadquery = "select rank() over(order by isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0) asc) as rank,    isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0) as externalpass,   isnull((sum(ext_fail)/nullif(sum(ext_appear),0)* 100),0) as externalfail,  sum(ext_pass) as pass,sum(ext_appear) as externalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=1 group by staff_code,degree,staff_name";
                            else
                                loadquery = "select rank() over(order by isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0) desc) as rank, isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0) as externalpass,   isnull((sum(ext_fail)/nullif(sum(ext_appear),0)* 100),0) as externalfail,  sum(ext_pass) as pass,sum(ext_appear) as externalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=1 group by staff_code,degree,staff_name";

                            //====================================================
                        }
                        else
                        {
                            FpEntry.Sheets[0].Columns[4].Visible = true;
                            FpEntry.Sheets[0].Columns[5].Visible = true;
                            FpEntry.Sheets[0].Columns[6].Visible = true;
                            FpEntry.Sheets[0].Columns[7].Visible = true;

                            //gowthaman 26july2013 ===============================
                            //if (rbbelow.Checked == true)
                            //    loadquery = "select rank() over(order by ((convert(decimal(16,4),(isnull(sum(ext_pass)/sum(ext_appear) * 50,'0')))) +convert(decimal(16,4),(isnull((sum(in_pass)/sum(in_appear) * 50),'0')))) asc) as rank,(isnull((sum(ext_pass)/sum(ext_appear) * 100),'0'))as externalpass, (isnull((sum(ext_fail)/sum(ext_appear) * 100) ,'0')) as externalfail,(isnull((sum(in_pass)/sum(in_appear) * 100),'0')) as internalpass, (isnull((sum(in_fail)/sum(in_appear) * 100 ),'0')) as internalfail,staff_code,degree,staff_name from tbl_staff_topper  group by staff_code,degree,staff_name";
                            //else                        
                            //    loadquery = "select rank() over(order by ((convert(decimal(16,4),(isnull(sum(ext_pass)/sum(ext_appear) * 50,'0')))) +convert(decimal(16,4),(isnull((sum(in_pass)/sum(in_appear) * 50),'0')))) desc) as rank,(isnull((sum(ext_pass)/sum(ext_appear) * 100),'0'))as externalpass, (isnull((sum(ext_fail)/sum(ext_appear) * 100) ,'0')) as externalfail,(isnull((sum(in_pass)/sum(in_appear) * 100),'0')) as internalpass, (isnull((sum(in_fail)/sum(in_appear) * 100 ),'0')) as internalfail,staff_code,degree,staff_name from tbl_staff_topper  group by staff_code,degree,staff_name";
                            if (rbbelow.Checked == true)
                                loadquery = "select rank() over(order by ((convert(decimal(16,4),(isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 50),0)))) +convert(decimal(16,4),(isnull((sum(in_pass)/nullif(sum(in_appear),0)* 50),0)))) asc) as rank,(isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0))as externalpass, (isnull((sum(ext_fail)/nullif(sum(ext_appear),0)* 100),0)) as externalfail,(isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0)) as internalpass, (isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0)) as internalfail,staff_code,degree,staff_name from tbl_staff_topper and user_code='" + usercode + "' group by staff_code,degree,staff_name";
                            else
                                loadquery = "select rank() over(order by ((convert(decimal(16,4),(isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 50),0)))) +convert(decimal(16,4),(isnull((sum(in_pass)/nullif(sum(in_appear),0)* 50),0)))) desc) as rank,(isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0))as externalpass, (isnull((sum(ext_fail)/nullif(sum(ext_appear),0)* 100),0)) as externalfail,(isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0)) as internalpass, (isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0)) as internalfail,staff_code,degree,staff_name from tbl_staff_topper and user_code='" + usercode + "' group by staff_code,degree,staff_name";

                        }

                        ds = d2.select_method(loadquery, hat, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Double intpass = 0;
                                Double intfail = 0;
                                Double extpass = 0;
                                Double extfail = 0;
                                string department = ds.Tables[0].Rows[i]["degree"].ToString();
                                string staff = ds.Tables[0].Rows[i]["staff_code"].ToString();
                                string staffname = ds.Tables[0].Rows[i]["Staff_name"].ToString();
                                int rank = Convert.ToInt32(ds.Tables[0].Rows[i]["rank"]);
                                int range = 0;

                                string namequery = "select d.desig_name from desig_master d,stafftrans st where st.desig_code=d.desig_code  and st.staff_code='" + staff + "'";

                                DataSet dsname = d2.select_method(namequery, hat, "Text ");
                                string designation = dsname.Tables[0].Rows[0]["desig_name"].ToString();

                                if (txtrange.Text != "")
                                {
                                    range = Convert.ToInt32(txtrange.Text);

                                    if (ddlexam.SelectedIndex.ToString() == "1")
                                    {
                                        if (rank <= range)
                                        {
                                            resultflag = true;
                                            intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                            intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                            intpass = Math.Round(intpass, 2);
                                            intfail = Math.Round(intfail, 2);
                                            FpEntry.Sheets[0].RowCount++;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = department;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = designation;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = staff;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = staffname;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = intpass.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = intfail.ToString();
                                        }
                                    }
                                    else if (ddlexam.SelectedIndex.ToString() == "2")
                                    {
                                        if (rank <= range)
                                        {
                                            resultflag = true;
                                            extpass = Convert.ToDouble(ds.Tables[0].Rows[i]["externalpass"].ToString());
                                            extfail = Convert.ToDouble(ds.Tables[0].Rows[i]["externalfail"].ToString());
                                            extpass = Math.Round(extpass, 2);
                                            extfail = Math.Round(extfail, 2);
                                            FpEntry.Sheets[0].RowCount++;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = department;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = designation;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = staff;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = staffname;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = extpass.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = extfail.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (rank <= range)
                                        {
                                            intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                            intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                            extpass = Convert.ToDouble(ds.Tables[0].Rows[i]["externalpass"].ToString());
                                            extfail = Convert.ToDouble(ds.Tables[0].Rows[i]["externalfail"].ToString());
                                            if (intpass.ToString().Trim() == "")
                                            {
                                                intpass = 0;
                                            }
                                            if (intfail.ToString().Trim() == "")
                                            {
                                                intfail = 0;
                                            }
                                            if (extpass.ToString().Trim() == "")
                                            {
                                                extpass = 0;
                                            }
                                            if (extfail.ToString().Trim() == "")
                                            {
                                                extfail = 0;
                                            }
                                            resultflag = true;
                                            intpass = Math.Round(intpass, 2);
                                            intfail = Math.Round(intfail, 2);
                                            extpass = Math.Round(extpass, 2);
                                            extfail = Math.Round(extfail, 2);
                                            FpEntry.Sheets[0].RowCount++;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = department;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = designation;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = staff;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = staffname;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = intpass.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = intfail.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = extpass.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = extfail.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Note = "0";
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Note = "0";
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Note = "1";
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Note = "1";
                                        }
                                    }

                                }
                                else
                                {
                                    if (ddlexam.SelectedIndex.ToString() == "1")
                                    {
                                        resultflag = true;
                                        intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                        intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                        intpass = Math.Round(intpass, 2);
                                        intfail = Math.Round(intfail, 2);
                                        FpEntry.Sheets[0].RowCount++;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = department;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = designation;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = staff;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = staffname;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = intpass.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = intfail.ToString();
                                    }
                                    else if (ddlexam.SelectedIndex.ToString() == "2")
                                    {
                                        resultflag = true;
                                        extpass = Convert.ToDouble(ds.Tables[0].Rows[i]["externalpass"].ToString());
                                        extfail = Convert.ToDouble(ds.Tables[0].Rows[i]["externalfail"].ToString());
                                        extpass = Math.Round(extpass, 2);
                                        extfail = Math.Round(extfail, 2);
                                        FpEntry.Sheets[0].RowCount++;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = department;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = designation;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = staff;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = staffname;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = extpass.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = extfail.ToString();
                                    }
                                    else
                                    {
                                        resultflag = true;
                                        intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                        intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                        extpass = Convert.ToDouble(ds.Tables[0].Rows[i]["externalpass"].ToString());
                                        extfail = Convert.ToDouble(ds.Tables[0].Rows[i]["externalfail"].ToString());
                                        if (intpass.ToString().Trim() == "")
                                        {
                                            intpass = 0;
                                        }
                                        if (intfail.ToString().Trim() == "")
                                        {
                                            intfail = 0;
                                        }
                                        if (extpass.ToString().Trim() == "")
                                        {
                                            extpass = 0;
                                        }
                                        if (extfail.ToString().Trim() == "")
                                        {
                                            extfail = 0;
                                        }

                                        intpass = Math.Round(intpass, 2);
                                        intfail = Math.Round(intfail, 2);
                                        extpass = Math.Round(extpass, 2);
                                        extfail = Math.Round(extfail, 2);
                                        FpEntry.Sheets[0].RowCount++;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = department;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = designation;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = staff;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = staffname;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = intpass.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = intfail.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = extpass.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = extfail.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Note = "0";
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Note = "0";
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Note = "1";
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Note = "1";
                                    }
                                }

                            }
                        }

                        else
                        {
                            FpEntry.Visible = false;
                            lblexcel.Visible = false;
                            txtxl.Visible = false;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                            errmsg.Visible = true;
                            errmsg.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        FpEntry.Visible = false;
                        lblexcel.Visible = false;
                        txtxl.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Staff";
                        return;
                    }
                }
                else
                {//Added By VENKAT 16/8/2014==============================================
                    btnprint.Visible = false;
                    txtexcelname.Visible = false;
                    btnexcel.Visible = false;
                    txtexcelname.Text = "";
                    lblnorec.Visible = false;
                    errmsg.Visible = false;
                    FpEntry.Visible = false;
                    lblexcel.Visible = false;
                    txtxl.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    FpReport.Visible = false;

                    ds.Dispose();
                    string staffcode = "";
                    for (int i = 0; i < chklsstaff.Items.Count; i++)
                    {
                        if (chklsstaff.Items[i].Selected == true)
                        {
                            if (staffcode == "")
                            {
                                staffcode = "'" + chklsstaff.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                staffcode = " " + staffcode + ",'" + chklsstaff.Items[i].Value.ToString() + "'";
                            }
                        }
                    }
                    if (staffcode != "")
                    {
                        //ds.Dispose();
                        //ds = d2.select_method("select * from sysobjects where name='tbl_staff_topper' and Type='U'", hat, "text ");
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    //int q = d2.insert_method("drop table tbl_staff_topper", hat, "text");
                        //    //int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int)", hat, "text");
                        ////}
                        ////else
                        ////{
                        //    int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int,user_code nvarchar(25))", hat, "text");
                        //}

                        staffcode = staffcode.ToString().Trim();

                        if (ddlexam.SelectedIndex.ToString() == "1")
                        {
                            loadreport(staffcode);
                        }
                        else if (ddlexam.SelectedIndex.ToString() == "2")
                        {
                            loadexternal(staffcode);
                        }
                        else
                        {
                            loadexternal(staffcode);
                            loadreport(staffcode);
                        }

                        loadHeader();

                        ds.Dispose();
                        ds.Reset();
                        //---------------Remove By M.SakthiPriya 20/12/2014-----------------------
                        //string[] code = staffcode.Split(',');
                        //for (int cd = 0; cd < code.Length; cd++)
                        //{
                        //    string stf = code[cd];
                        //------------------------End-----------------------------
                        string loadquery = "";
                        if (ddlexam.SelectedIndex.ToString() == "1")
                        {
                            FpEntry.Sheets[0].Columns[4].Visible = true;
                            FpEntry.Sheets[0].Columns[5].Visible = true;
                            FpEntry.Sheets[0].Columns[6].Visible = false;
                            FpEntry.Sheets[0].Columns[7].Visible = false;
                            //gowthaman 26july2013 ===============================
                            //if (rbbelow.Checked == true)
                            //    loadquery = "select rank() over(order by (sum(in_pass)/sum(in_appear) * 100) asc) as rank,(sum(in_pass)/sum(in_appear) * 100) as internalpass, (sum(in_fail)/sum(in_appear) * 100 ) as internalfail,sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 group by staff_code,degree,staff_name";
                            //else
                            //    loadquery = "select rank() over(order by (sum(in_pass)/sum(in_appear) * 100) desc) as rank,(sum(in_pass)/sum(in_appear) * 100) as internalpass, (sum(in_fail)/sum(in_appear) * 100 ) as internalfail,sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 group by staff_code,degree,staff_name";
                            if (rbbelow.Checked == true)
                                loadquery = "select rank() over(order by isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) asc) as rank,    isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) as internalpass,   isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0) as internalfail,  sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 and staff_code in(" + staffcode + ") and user_code='" + usercode + "' group by staff_code,degree,staff_name";//Modify By M.SakthiPriya 20/12/2014
                            else
                                loadquery = "select rank() over(order by isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) desc) as rank,    isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) as internalpass,   isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0) as internalfail,  sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 and staff_code in(" + staffcode + ") and user_code='" + usercode + "' group by staff_code,degree,staff_name";//Modify By M.SakthiPriya 20/12/2014

                            //=====================================================

                        }
                        else if (ddlexam.SelectedIndex.ToString() == "2")
                        {
                            FpEntry.Sheets[0].Columns[4].Visible = false;
                            FpEntry.Sheets[0].Columns[5].Visible = false;
                            FpEntry.Sheets[0].Columns[6].Visible = true;
                            FpEntry.Sheets[0].Columns[7].Visible = true;
                            //gowthaman 26july2013 ===============================
                            //if (rbbelow.Checked == true)
                            //    loadquery = "select rank() over(order by (sum(ext_pass)/sum(ext_appear) * 100) asc) as rank,(sum(ext_pass)/sum(ext_appear) * 100) as externalpass, (sum(ext_fail)/sum(ext_appear) * 100 ) as externalfail,sum(ext_pass) as pass,sum(ext_appear) as externalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=1 group by staff_code,degree,staff_name";
                            //else
                            //    loadquery = "select rank() over(order by (sum(ext_pass)/sum(ext_appear) * 100) desc) as rank,(sum(ext_pass)/sum(ext_appear) * 100) as externalpass, (sum(ext_fail)/sum(ext_appear) * 100 ) as externalfail,sum(ext_pass) as pass,sum(ext_appear) as externalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=1 group by staff_code,degree,staff_name";
                            if (rbbelow.Checked == true)
                                loadquery = "select rank() over(order by isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0) asc) as rank,    isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0) as externalpass,   isnull((sum(ext_fail)/nullif(sum(ext_appear),0)* 100),0) as externalfail,  sum(ext_pass) as pass,sum(ext_appear) as externalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=1 and staff_code in(" + staffcode + ") and user_code='" + usercode + "' group by staff_code,degree,staff_name";//Modify By M.SakthiPriya 20/12/2014
                            else
                                loadquery = "select rank() over(order by isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0) desc) as rank, isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0) as externalpass,   isnull((sum(ext_fail)/nullif(sum(ext_appear),0)* 100),0) as externalfail,  sum(ext_pass) as pass,sum(ext_appear) as externalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=1 and staff_code in(" + staffcode + ") and user_code='" + usercode + "' group by staff_code,degree,staff_name";//Modify By M.SakthiPriya 20/12/2014

                            //====================================================
                        }
                        else
                        {
                            FpEntry.Sheets[0].Columns[4].Visible = true;
                            FpEntry.Sheets[0].Columns[5].Visible = true;
                            FpEntry.Sheets[0].Columns[6].Visible = true;
                            FpEntry.Sheets[0].Columns[7].Visible = true;

                            //gowthaman 26july2013 ===============================
                            //if (rbbelow.Checked == true)
                            //    loadquery = "select rank() over(order by ((convert(decimal(16,4),(isnull(sum(ext_pass)/sum(ext_appear) * 50,'0')))) +convert(decimal(16,4),(isnull((sum(in_pass)/sum(in_appear) * 50),'0')))) asc) as rank,(isnull((sum(ext_pass)/sum(ext_appear) * 100),'0'))as externalpass, (isnull((sum(ext_fail)/sum(ext_appear) * 100) ,'0')) as externalfail,(isnull((sum(in_pass)/sum(in_appear) * 100),'0')) as internalpass, (isnull((sum(in_fail)/sum(in_appear) * 100 ),'0')) as internalfail,staff_code,degree,staff_name from tbl_staff_topper  group by staff_code,degree,staff_name";
                            //else                        
                            //    loadquery = "select rank() over(order by ((convert(decimal(16,4),(isnull(sum(ext_pass)/sum(ext_appear) * 50,'0')))) +convert(decimal(16,4),(isnull((sum(in_pass)/sum(in_appear) * 50),'0')))) desc) as rank,(isnull((sum(ext_pass)/sum(ext_appear) * 100),'0'))as externalpass, (isnull((sum(ext_fail)/sum(ext_appear) * 100) ,'0')) as externalfail,(isnull((sum(in_pass)/sum(in_appear) * 100),'0')) as internalpass, (isnull((sum(in_fail)/sum(in_appear) * 100 ),'0')) as internalfail,staff_code,degree,staff_name from tbl_staff_topper  group by staff_code,degree,staff_name";
                            if (rbbelow.Checked == true)
                                loadquery = "select rank() over(order by ((convert(decimal(16,4),(isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 50),0)))) +convert(decimal(16,4),(isnull((sum(in_pass)/nullif(sum(in_appear),0)* 50),0)))) asc) as rank,(isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0))as externalpass, (isnull((sum(ext_fail)/nullif(sum(ext_appear),0)* 100),0)) as externalfail,(isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0)) as internalpass, (isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0)) as internalfail,staff_code,degree,staff_name from tbl_staff_topper where staff_code in(" + staffcode + ") and user_code='" + usercode + "' group by staff_code,degree,staff_name";//Modify By M.SakthiPriya 20/12/2014
                            else
                                loadquery = "select rank() over(order by ((convert(decimal(16,4),(isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 50),0)))) +convert(decimal(16,4),(isnull((sum(in_pass)/nullif(sum(in_appear),0)* 50),0)))) desc) as rank,(isnull((sum(ext_pass)/nullif(sum(ext_appear),0)* 100),0))as externalpass, (isnull((sum(ext_fail)/nullif(sum(ext_appear),0)* 100),0)) as externalfail,(isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0)) as internalpass, (isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0)) as internalfail,staff_code,degree,staff_name from tbl_staff_topper where staff_code in(" + staffcode + ") and user_code='" + usercode + "' group by staff_code,degree,staff_name";//Modify By M.SakthiPriya 20/12/2014

                        }

                        ds = d2.select_method(loadquery, hat, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Double intpass = 0;
                                Double intfail = 0;
                                Double extpass = 0;
                                Double extfail = 0;
                                string department = ds.Tables[0].Rows[i]["degree"].ToString();
                                string staff = ds.Tables[0].Rows[i]["staff_code"].ToString();
                                string staffname = ds.Tables[0].Rows[i]["Staff_name"].ToString();
                                int rank = Convert.ToInt32(ds.Tables[0].Rows[i]["rank"]);
                                int range = 0;

                                string namequery = "select d.desig_name from desig_master d,stafftrans st where st.desig_code=d.desig_code  and st.staff_code='" + staff + "'";

                                DataSet dsname = d2.select_method(namequery, hat, "Text ");
                                string designation = dsname.Tables[0].Rows[0]["desig_name"].ToString();

                                if (txtrange.Text != "")
                                {
                                    range = Convert.ToInt32(txtrange.Text);

                                    if (ddlexam.SelectedIndex.ToString() == "1")
                                    {
                                        if (rank <= range)
                                        {
                                            resultflag = true;
                                            intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                            intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                            intpass = Math.Round(intpass, 2);
                                            intfail = Math.Round(intfail, 2);
                                            FpEntry.Sheets[0].RowCount++;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = staff;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staffname;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = designation;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = department;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = intpass.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = intfail.ToString();
                                        }
                                    }
                                    else if (ddlexam.SelectedIndex.ToString() == "2")
                                    {
                                        if (rank <= range)
                                        {
                                            resultflag = true;
                                            extpass = Convert.ToDouble(ds.Tables[0].Rows[i]["externalpass"].ToString());
                                            extfail = Convert.ToDouble(ds.Tables[0].Rows[i]["externalfail"].ToString());
                                            extpass = Math.Round(extpass, 2);
                                            extfail = Math.Round(extfail, 2);
                                            FpEntry.Sheets[0].RowCount++;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = staff;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staffname;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = designation;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = department;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = extpass.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = extfail.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (rank <= range)
                                        {
                                            resultflag = true;
                                            intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                            intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                            extpass = Convert.ToDouble(ds.Tables[0].Rows[i]["externalpass"].ToString());
                                            extfail = Convert.ToDouble(ds.Tables[0].Rows[i]["externalfail"].ToString());
                                            if (intpass.ToString().Trim() == "")
                                            {
                                                intpass = 0;
                                            }
                                            if (intfail.ToString().Trim() == "")
                                            {
                                                intfail = 0;
                                            }
                                            if (extpass.ToString().Trim() == "")
                                            {
                                                extpass = 0;
                                            }
                                            if (extfail.ToString().Trim() == "")
                                            {
                                                extfail = 0;
                                            }
                                            resultflag = true;
                                            intpass = Math.Round(intpass, 2);
                                            intfail = Math.Round(intfail, 2);
                                            extpass = Math.Round(extpass, 2);
                                            extfail = Math.Round(extfail, 2);
                                            FpEntry.Sheets[0].RowCount++;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = staff;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staffname;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = designation;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = department;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = intpass.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = intfail.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = extpass.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = extfail.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Note = "0";
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Note = "0";
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Note = "1";
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Note = "1";
                                        }
                                    }

                                }
                                else
                                {
                                    if (ddlexam.SelectedIndex.ToString() == "1")
                                    {
                                        resultflag = true;
                                        intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                        intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                        intpass = Math.Round(intpass, 2);
                                        intfail = Math.Round(intfail, 2);
                                        FpEntry.Sheets[0].RowCount++;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = staff;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staffname;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = designation;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = department;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = intpass.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = intfail.ToString();
                                    }
                                    else if (ddlexam.SelectedIndex.ToString() == "2")
                                    {
                                        resultflag = true;
                                        extpass = Convert.ToDouble(ds.Tables[0].Rows[i]["externalpass"].ToString());
                                        extfail = Convert.ToDouble(ds.Tables[0].Rows[i]["externalfail"].ToString());
                                        extpass = Math.Round(extpass, 2);
                                        extfail = Math.Round(extfail, 2);
                                        FpEntry.Sheets[0].RowCount++;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = staff;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staffname;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = designation;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = department;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = extpass.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = extfail.ToString();
                                    }
                                    else
                                    {
                                        intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                        intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                        extpass = Convert.ToDouble(ds.Tables[0].Rows[i]["externalpass"].ToString());
                                        extfail = Convert.ToDouble(ds.Tables[0].Rows[i]["externalfail"].ToString());
                                        if (intpass.ToString().Trim() == "")
                                        {
                                            intpass = 0;
                                        }
                                        if (intfail.ToString().Trim() == "")
                                        {
                                            intfail = 0;
                                        }
                                        if (extpass.ToString().Trim() == "")
                                        {
                                            extpass = 0;
                                        }
                                        if (extfail.ToString().Trim() == "")
                                        {
                                            extfail = 0;
                                        }
                                        resultflag = true;
                                        intpass = Math.Round(intpass, 2);
                                        intfail = Math.Round(intfail, 2);
                                        extpass = Math.Round(extpass, 2);
                                        extfail = Math.Round(extfail, 2);
                                        FpEntry.Sheets[0].RowCount++;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = staff;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staffname;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = designation;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = department;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = intpass.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Text = intfail.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = extpass.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = extfail.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Note = "0";
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 5].Note = "0";
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Note = "1";
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Note = "1";
                                        FpEntry.Sheets[0].RowHeader.Visible = true;
                                    }
                                }

                            }
                            FpEntry.Sheets[0].RowHeader.Visible = true;
                        }

                        //else
                        //{
                        //    FpEntry.Visible = false;
                        //    lblexcel.Visible = false;
                        //    txtxl.Visible = false;
                        //    btnxl.Visible = false;
                        //    btnprintmaster.Visible = false;
                        //    errmsg.Visible = true;
                        //    errmsg.Text = "No Records Found";
                        //}
                    }
                    // }
                    else
                    {

                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Staff";
                        return;
                    }
                }

                FpEntry.Sheets[0].PageSize = FpEntry.Sheets[0].RowCount;
            }     //=====================================END======================================
            else
            {
                if (ddlorder.SelectedItem.Text == "Dept.")
                {
                    int sno = 0;
                    string examquery = "";
                    btnprint.Visible = false;
                    txtexcelname.Visible = false;
                    btnexcel.Visible = false;
                    txtexcelname.Text = "";
                    lblnorec.Visible = false;
                    errmsg.Visible = false;
                    FpEntry.Visible = false;
                    lblexcel.Visible = false;
                    txtxl.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    FpReport.Visible = false;
                    DataSet dssubmark = new DataSet();
                    DataSet dsoverallmark = new DataSet();
                    DataView dvmark = new DataView();
                    double subintappear = 0;
                    double subintpass = 0;
                    double overalllastperc = 0;
                    int overalllastcount = 0;
                    ds.Dispose();
                    string staffcode = "";
                    for (int i = 0; i < chklsstaff.Items.Count; i++)
                    {
                        if (chklsstaff.Items[i].Selected == true)
                        {
                            if (staffcode == "")
                            {
                                staffcode = "'" + chklsstaff.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                staffcode = " " + staffcode + ",'" + chklsstaff.Items[i].Value.ToString() + "'";
                            }
                        }
                    }
                    if (staffcode != "")
                    {
                        //ds.Dispose();
                        //ds = d2.select_method("select * from sysobjects where name='tbl_staff_topper' and Type='U'", hat, "text ");
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    int q = d2.insert_method("drop table tbl_staff_topper", hat, "text");
                        //    int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int)", hat, "text");
                        //}
                        //else
                        //{
                        //    int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int)", hat, "text");
                        //}

                        staffcode = staffcode.ToString().Trim();

                        loadreport(staffcode);

                        loadindividualheader();
                        FpEntry.Visible = true;

                        ds.Dispose();
                        ds.Reset();

                        string loadquery = "";


                        if (rbbelow.Checked == true)
                            loadquery = "select rank() over(order by isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) asc) as rank,    isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) as internalpass,   isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0) as internalfail,  sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 and user_code='" + usercode + "' group by staff_code,degree,staff_name";
                        else
                            loadquery = "select rank() over(order by isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) desc) as rank,    isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) as internalpass,   isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0) as internalfail,  sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 and user_code='" + usercode + "' group by staff_code,degree,staff_name";


                        // }
                        //Added By Srinath 5/3/2014
                        if (loadquery.Trim() == "")
                        {
                            FpEntry.Visible = false;
                            btnxl.Visible = false;
                            txtxl.Visible = false;
                            lblexcel.Visible = false;
                            btnprintmaster.Visible = false;
                            errmsg.Visible = true;
                            errmsg.Text = "Plaese Select Exam Type as Internal";
                            return;
                        }
                        else
                        {

                            ds = d2.select_method(loadquery, hat, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    Double intpass = 0;
                                    Double intfail = 0;
                                    Double extpass = 0;
                                    Double extfail = 0;
                                    string department = ds.Tables[0].Rows[i]["degree"].ToString();
                                    string staff = ds.Tables[0].Rows[i]["staff_code"].ToString();
                                    string staffname = ds.Tables[0].Rows[i]["Staff_name"].ToString();
                                    int rank = Convert.ToInt32(ds.Tables[0].Rows[i]["rank"]);
                                    int range = 0;

                                    string namequery = "select d.desig_name from desig_master d,stafftrans st where st.desig_code=d.desig_code  and st.staff_code='" + staff + "'";

                                    DataSet dsname = d2.select_method(namequery, hat, "Text ");
                                    string designation = dsname.Tables[0].Rows[0]["desig_name"].ToString();

                                    if (txtrange.Text != "")
                                    {
                                        range = Convert.ToInt32(txtrange.Text);

                                        //if (ddlexam.SelectedIndex.ToString() == "1")
                                        //{
                                        if (rank <= range)
                                        {
                                            sno++;
                                            intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                            intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                            intpass = Math.Round(intpass, 2);
                                            intfail = Math.Round(intfail, 2);
                                            FpEntry.Sheets[0].RowCount++;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = department;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = designation;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = staff;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = staffname;


                                            examquery = "select distinct ROW_NUMBER() OVER(ORDER BY degree DESC) AS Row,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail from tbl_staff_topper where isexternal=0 and staff_code='" + staff + "' and user_code='" + usercode + "'";
                                            dsoverallmark = d2.select_method(examquery, hat, "Text ");
                                            dssubmark = d2.select_method(examquery, hat, "Text ");
                                            int lastcol = 0;
                                            for (int col = 5; col < FpEntry.Sheets[0].ColumnCount; col++)
                                            {
                                                lastcol = col;
                                                string testname = FpEntry.Sheets[0].ColumnHeader.Cells[1, col].Text;
                                                string subjectname = "";
                                                string percenatge = "";
                                                if (testname != "")
                                                {
                                                    dssubmark.Tables[0].DefaultView.RowFilter = "internal_exam_type='" + testname + "'";
                                                    dvmark = dssubmark.Tables[0].DefaultView;
                                                    if (dvmark.Count > 0)
                                                    {
                                                        overalllastcount++;
                                                        double passpercentage = 0;
                                                        double roundpasspercentage = 0;
                                                        subjectname = dvmark[0]["subject"].ToString();
                                                        subintappear = Convert.ToDouble(dvmark[0]["in_appear"].ToString());
                                                        subintpass = Convert.ToDouble(dvmark[0]["in_pass"].ToString());
                                                        passpercentage = (subintpass / subintappear) * 100;
                                                        roundpasspercentage = Math.Round(passpercentage, 2);
                                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col].Text = subjectname.ToString();
                                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col + 1].Text = roundpasspercentage.ToString();
                                                        overalllastperc = overalllastperc + roundpasspercentage;
                                                    }
                                                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col + 1].Text = "-";
                                                    //col = col + 2;

                                                }

                                            }
                                            double totalroundperc = 0;
                                            if (overalllastcount != 0)
                                            {
                                                double totalper = overalllastperc / overalllastcount;
                                                totalroundperc = Math.Round(totalper, 2);
                                            }
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, lastcol].Text = intpass.ToString();
                                        }
                                    }
                                    else
                                    {
                                        overalllastperc = 0;
                                        overalllastcount = 0;
                                        sno++;
                                        intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                        intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                        intpass = Math.Round(intpass, 2);
                                        intfail = Math.Round(intfail, 2);
                                        FpEntry.Sheets[0].RowCount++;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = department;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = designation;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = staff;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = staffname;

                                        examquery = "select distinct ROW_NUMBER() OVER(ORDER BY degree DESC) AS Row,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail from tbl_staff_topper where isexternal=0 and staff_code='" + staff + "' and user_code='" + usercode + "'";
                                        dsoverallmark = d2.select_method(examquery, hat, "Text ");
                                        dssubmark = d2.select_method(examquery, hat, "Text ");
                                        int lastcol = 0;
                                        for (int col = 5; col < FpEntry.Sheets[0].ColumnCount; col++)
                                        {
                                            lastcol = col;
                                            string testname = FpEntry.Sheets[0].ColumnHeader.Cells[1, col].Text;
                                            string subjectname = "";
                                            string percenatge = "";
                                            if (testname != "")
                                            {
                                                dssubmark.Tables[0].DefaultView.RowFilter = "internal_exam_type='" + testname + "'";
                                                dvmark = dssubmark.Tables[0].DefaultView;
                                                if (dvmark.Count > 0)
                                                {
                                                    overalllastcount++;
                                                    double passpercentage = 0;
                                                    double roundpasspercentage = 0;
                                                    subjectname = dvmark[0]["subject"].ToString();
                                                    subintappear = Convert.ToDouble(dvmark[0]["in_appear"].ToString());
                                                    subintpass = Convert.ToDouble(dvmark[0]["in_pass"].ToString());
                                                    passpercentage = (subintpass / subintappear) * 100;
                                                    roundpasspercentage = Math.Round(passpercentage, 2);
                                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col].Text = subjectname.ToString();
                                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col + 1].Text = roundpasspercentage.ToString();
                                                    overalllastperc = overalllastperc + roundpasspercentage;
                                                }
                                            }

                                        }
                                        double totalroundperc = 0;
                                        if (overalllastcount != 0)
                                        {
                                            double totalper = overalllastperc / overalllastcount;
                                            totalroundperc = Math.Round(totalper, 2);
                                        }
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, lastcol].Text = intpass.ToString();
                                    }
                                }
                            }
                        }//==============End



                    }
                    else
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Staff";
                    }
                }
                else//Added By VENKAT 16/8/2014==============================================================
                {
                    int sno = 0;
                    string examquery = "";
                    btnprint.Visible = false;
                    txtexcelname.Visible = false;
                    btnexcel.Visible = false;
                    txtexcelname.Text = "";
                    lblnorec.Visible = false;
                    errmsg.Visible = false;
                    FpEntry.Visible = false;
                    lblexcel.Visible = false;
                    txtxl.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    FpReport.Visible = false;
                    DataSet dssubmark = new DataSet();
                    DataSet dsoverallmark = new DataSet();
                    DataView dvmark = new DataView();
                    double subintappear = 0;
                    double subintpass = 0;
                    double overalllastperc = 0;
                    int overalllastcount = 0;
                    ds.Dispose();
                    string staffcode = "";
                    for (int i = 0; i < chklsstaff.Items.Count; i++)
                    {
                        if (chklsstaff.Items[i].Selected == true)
                        {
                            if (staffcode == "")
                            {
                                staffcode = "'" + chklsstaff.Items[i].Value.ToString() + "'";
                            }
                            else
                            {
                                staffcode = " " + staffcode + ",'" + chklsstaff.Items[i].Value.ToString() + "'";
                            }
                        }
                    }
                    if (staffcode != "")
                    {
                        //ds.Dispose();
                        //ds = d2.select_method("select * from sysobjects where name='tbl_staff_topper' and Type='U'", hat, "text ");
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    int q = d2.insert_method("drop table tbl_staff_topper", hat, "text");
                        //    int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int)", hat, "text");
                        //}
                        //else
                        //{
                        //    int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int)", hat, "text");
                        //}

                        staffcode = staffcode.ToString().Trim();

                        loadreport(staffcode);

                        loadindividualheader();
                        FpEntry.Visible = true;
                        //-----------------------------Remove
                        //string[] code = staffcode.Split(',');
                        //for (int cn = 0; cn < code.Length; cn++)
                        //{
                        ds.Dispose();
                        ds.Reset();
                        // string stf = code[cn];
                        string loadquery = "";


                        if (rbbelow.Checked == true)
                            loadquery = "select rank() over(order by isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) asc) as rank,    isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) as internalpass,   isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0) as internalfail,  sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 and staff_code in(" + staffcode + ") and user_code='" + usercode + "' group by staff_code,degree,staff_name";//Modify By M.SakthiPriya 20/12/2014
                        else
                            loadquery = "select rank() over(order by isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) desc) as rank,    isnull((sum(in_pass)/nullif(sum(in_appear),0)* 100),0) as internalpass,   isnull((sum(in_fail)/nullif(sum(in_appear),0)* 100),0) as internalfail,  sum(in_pass) as pass,sum(in_appear) as internalpass,staff_code,degree,staff_name from tbl_staff_topper where isexternal=0 and staff_code in(" + staffcode + ") and user_code='" + usercode + "' group by staff_code,degree,staff_name";//Modify By M.SakthiPriya 20/12/2014




                        if (loadquery.Trim() == "")
                        {
                            FpEntry.Visible = false;
                            btnxl.Visible = false;
                            txtxl.Visible = false;
                            lblexcel.Visible = false;
                            btnprintmaster.Visible = false;
                            errmsg.Visible = true;
                            errmsg.Text = "Plaese Select Exam Type as Internal";
                            return;
                        }
                        else
                        {
                            string stfcode = "";
                            ds = d2.select_method(loadquery, hat, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    Double intpass = 0;
                                    Double intfail = 0;
                                    Double extpass = 0;
                                    Double extfail = 0;
                                    string department = ds.Tables[0].Rows[i]["degree"].ToString();
                                    string staff = ds.Tables[0].Rows[i]["staff_code"].ToString();
                                    string staffname = ds.Tables[0].Rows[i]["Staff_name"].ToString();
                                    int rank = Convert.ToInt32(ds.Tables[0].Rows[i]["rank"]);
                                    int range = 0;

                                    string namequery = "select d.desig_name from desig_master d,stafftrans st where st.desig_code=d.desig_code  and st.staff_code in(" + staffcode + ")";

                                    DataSet dsname = d2.select_method(namequery, hat, "Text ");
                                    string designation = dsname.Tables[0].Rows[0]["desig_name"].ToString();

                                    if (txtrange.Text != "")
                                    {
                                        range = Convert.ToInt32(txtrange.Text);

                                        if (rank <= range)
                                        {
                                            resultflag = true;

                                            intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                            intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                            intpass = Math.Round(intpass, 2);
                                            intfail = Math.Round(intfail, 2);
                                            FpEntry.Sheets[0].RowCount++;

                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staff;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staff;
                                            // added by sridhar  04 sep 2014==========* Start   *====================
                                            if (stfcode != staff)
                                            {
                                                sno++;
                                                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                FpEntry.Sheets[0].Columns[0].Locked = true;
                                                stfcode = staff;
                                            }
                                            // added by sridhar  04 sep 2014==========* End   *====================

                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = staffname;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = designation;
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = department;


                                            examquery = "select distinct ROW_NUMBER() OVER(ORDER BY degree DESC) AS Row,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail from tbl_staff_topper where isexternal=0 and staff_code='" + staff + "' and user_code='" + usercode + "'";
                                            dsoverallmark = d2.select_method(examquery, hat, "Text ");
                                            dssubmark = d2.select_method(examquery, hat, "Text ");
                                            int lastcol = 0;
                                            for (int col = 5; col < FpEntry.Sheets[0].ColumnCount; col++)
                                            {
                                                lastcol = col;
                                                string testname = FpEntry.Sheets[0].ColumnHeader.Cells[1, col].Text;
                                                string subjectname = "";
                                                string percenatge = "";
                                                if (testname != "")
                                                {
                                                    dssubmark.Tables[0].DefaultView.RowFilter = "internal_exam_type='" + testname + "' and degree='" + department + "'"; // added by sridhar 04 sep 2014
                                                    dvmark = dssubmark.Tables[0].DefaultView;
                                                    if (dvmark.Count > 0)
                                                    {
                                                        overalllastcount++;
                                                        double passpercentage = 0;
                                                        double roundpasspercentage = 0;
                                                        subjectname = dvmark[0]["subject"].ToString();
                                                        subintappear = Convert.ToDouble(dvmark[0]["in_appear"].ToString());
                                                        subintpass = Convert.ToDouble(dvmark[0]["in_pass"].ToString());
                                                        passpercentage = (subintpass / subintappear) * 100;
                                                        roundpasspercentage = Math.Round(passpercentage, 2);
                                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col].Text = subjectname.ToString();
                                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col + 1].Text = roundpasspercentage.ToString();
                                                        overalllastperc = overalllastperc + roundpasspercentage;
                                                    }
                                                    //FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col + 1].Text = "-";
                                                    //col = col + 2;

                                                }

                                            }
                                            double totalroundperc = 0;
                                            if (overalllastcount != 0)
                                            {
                                                double totalper = overalllastperc / overalllastcount;
                                                totalroundperc = Math.Round(totalper, 2);
                                            }
                                            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, lastcol].Text = intpass.ToString();
                                        }
                                    }
                                    else
                                    {
                                        resultflag = true;
                                        overalllastperc = 0;
                                        overalllastcount = 0;
                                        sno++;
                                        intpass = Convert.ToDouble(ds.Tables[0].Rows[i]["internalpass"].ToString());
                                        intfail = Convert.ToDouble(ds.Tables[0].Rows[i]["internalfail"].ToString());
                                        intpass = Math.Round(intpass, 2);
                                        intfail = Math.Round(intfail, 2);
                                        FpEntry.Sheets[0].RowCount++;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 1].Text = staff;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 2].Text = staffname;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = designation;
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = department;

                                        examquery = "select distinct ROW_NUMBER() OVER(ORDER BY degree DESC) AS Row,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail from tbl_staff_topper where isexternal=0 and staff_code='" + staff + "' and user_code='" + usercode + "'";
                                        dsoverallmark = d2.select_method(examquery, hat, "Text ");
                                        dssubmark = d2.select_method(examquery, hat, "Text ");
                                        int lastcol = 0;
                                        for (int col = 5; col < FpEntry.Sheets[0].ColumnCount; col++)
                                        {
                                            lastcol = col;
                                            string testname = FpEntry.Sheets[0].ColumnHeader.Cells[1, col].Text;
                                            string subjectname = "";
                                            string percenatge = "";
                                            if (testname != "")
                                            {
                                                dssubmark.Tables[0].DefaultView.RowFilter = "internal_exam_type='" + testname + "'";
                                                dvmark = dssubmark.Tables[0].DefaultView;
                                                if (dvmark.Count > 0)
                                                {
                                                    overalllastcount++;
                                                    double passpercentage = 0;
                                                    double roundpasspercentage = 0;
                                                    subjectname = dvmark[0]["subject"].ToString();
                                                    subintappear = Convert.ToDouble(dvmark[0]["in_appear"].ToString());
                                                    subintpass = Convert.ToDouble(dvmark[0]["in_pass"].ToString());
                                                    passpercentage = (subintpass / subintappear) * 100;
                                                    roundpasspercentage = Math.Round(passpercentage, 2);
                                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col].Text = subjectname.ToString();
                                                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, col + 1].Text = roundpasspercentage.ToString();
                                                    overalllastperc = overalllastperc + roundpasspercentage;
                                                }
                                            }

                                        }
                                        double totalroundperc = 0;
                                        if (overalllastcount != 0)
                                        {
                                            double totalper = overalllastperc / overalllastcount;
                                            totalroundperc = Math.Round(totalper, 2);
                                        }
                                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, lastcol].Text = intpass.ToString();
                                    }
                                }
                            }
                        }//==============End

                    }

                 //   }
                    else
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Staff";
                    }

                }//======================================END=======================================================
            }
            if (resultflag == false)
            {
                FpEntry.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            else
            {
                FpEntry.Sheets[0].PageSize = FpEntry.Sheets[0].RowCount;
            }
            strdelexistval = d2.update_method_wo_parameter("delete from tbl_staff_topper where user_code='" + usercode + "'", "Text");
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }

    }
    public void loadindividualheader()
    {
        FpEntry.Sheets[0].RowHeader.Visible = false;
        FpEntry.Visible = true;
        lblexcel.Visible = true;
        txtxl.Visible = true;
        btnxl.Visible = true;
        btnprintmaster.Visible = true;
        FpEntry.Sheets[0].ColumnCount = 5;
        FpEntry.Sheets[0].ColumnHeader.RowCount = 3;
        FpEntry.Sheets[0].RowCount = 0;

        //gowthaman 26july2013====================================================
        FpEntry.Sheets[0].SheetCorner.ColumnCount = 1;
        FpEntry.Sheets[0].SheetCorner.RowCount = 3;
        FarPoint.Web.Spread.Cell acell;
        acell = FpEntry.Sheets[0].SheetCorner.Cells[0, 0];
        acell.ColumnSpan = 0;
        acell.RowSpan = 3;
        acell.Text = "S.No";
        acell.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
        //=========================================================================

        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Faculty Performance";
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 8);

        FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Sl.No";
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 2, 1);
        if (ddlorder.SelectedItem.Text == "Dept.")
        {
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Department";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Designation";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Staff Code";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Staff Name";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 4, 2, 1);
        }
        else
        {
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Staff Code";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Staff Name";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Designation";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Department";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 4, 2, 1);
        }

        FpEntry.Sheets[0].Columns[0].Width = 100;
        FpEntry.Sheets[0].Columns[1].Width = 100;
        FpEntry.Sheets[0].Columns[2].Width = 100;
        FpEntry.Sheets[0].Columns[3].Width = 150;
        FpEntry.Sheets[0].Columns[4].Width = 100;

        FpEntry.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
        FpEntry.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        FpEntry.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
        FpEntry.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
        FpEntry.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;



        FpEntry.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpEntry.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

        for (int test = 0; test < arinternal.Count; test++)
        {
            FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 2;
            int col = FpEntry.Sheets[0].ColumnCount - 2;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, col].Text = arinternal[test].ToString();
            FpEntry.Sheets[0].ColumnHeader.Cells[1, col].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, col].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, col].Font.Size = FontUnit.Medium;
            // FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, col, 1, 2);
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, col, 1, 2);
            int col1 = FpEntry.Sheets[0].ColumnCount - 1;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, col].Text = "Subject Name";
            FpEntry.Sheets[0].ColumnHeader.Cells[2, col].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[2, col].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, col].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, col1].Text = "%";
            FpEntry.Sheets[0].ColumnHeader.Cells[2, col1].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[2, col1].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[2, col1].Font.Size = FontUnit.Medium;

        }
        FpEntry.Sheets[0].ColumnCount++;
        int overallcol = FpEntry.Sheets[0].ColumnCount - 1;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, overallcol].Text = "Overall %";
        FpEntry.Sheets[0].ColumnHeader.Cells[1, overallcol].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[1, overallcol].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, overallcol].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, overallcol, 2, 1);

    }
    public void loadHeader()
    {
        FpEntry.Visible = true;
        lblexcel.Visible = true;
        txtxl.Visible = true;
        btnxl.Visible = true;
        btnprintmaster.Visible = true;
        FpEntry.Sheets[0].ColumnCount = 8;
        FpEntry.Sheets[0].ColumnHeader.RowCount = 3;
        FpEntry.Sheets[0].RowCount = 0;

        //gowthaman 26july2013====================================================
        FpEntry.Sheets[0].SheetCorner.ColumnCount = 1;
        FpEntry.Sheets[0].SheetCorner.RowCount = 3;
        FarPoint.Web.Spread.Cell acell;
        acell = FpEntry.Sheets[0].SheetCorner.Cells[0, 0];
        acell.ColumnSpan = 0;
        acell.RowSpan = 3;
        acell.Text = "S.No";
        acell.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
        //=========================================================================

        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Faculty Performance";
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 8);
        if (ddlorder.SelectedItem.Text == "Dept.")
        {
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Department";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Designation";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Staff Code";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Staff Name";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 2, 1);
        }
        else //Added by venkat 16/8/2014====================================================
        {
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Staff Code";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Staff Name";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Designation";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 2, 1);

            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Department";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Name = "Book Antiqua";
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Bold = true;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 2, 1);
        }//=======================================END=============================================
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Internal";
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 4].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 4, 1, 2);

        FpEntry.Sheets[0].ColumnHeader.Cells[2, 4].Text = "Pass %";
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 4].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 4].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 4].Font.Bold = true;

        FpEntry.Sheets[0].ColumnHeader.Cells[2, 5].Text = "Fail %";
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 5].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 5].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 5].Font.Bold = true;

        FpEntry.Sheets[0].ColumnHeader.Cells[1, 6].Text = "External";
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 6].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 6].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[1, 6].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 6, 1, 2);

        FpEntry.Sheets[0].ColumnHeader.Cells[2, 6].Text = "Pass %";
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 6].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 6].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 6].Font.Size = FontUnit.Medium;

        FpEntry.Sheets[0].ColumnHeader.Cells[2, 7].Text = "Fail %";
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 7].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 7].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[2, 7].Font.Size = FontUnit.Medium;

        FpEntry.Sheets[0].Columns[0].Width = 100;
        FpEntry.Sheets[0].Columns[1].Width = 100;
        FpEntry.Sheets[0].Columns[2].Width = 100;
        FpEntry.Sheets[0].Columns[3].Width = 150;
        FpEntry.Sheets[0].Columns[4].Width = 100;
        FpEntry.Sheets[0].Columns[5].Width = 75;
        FpEntry.Sheets[0].Columns[6].Width = 75;
        FpEntry.Sheets[0].Columns[7].Width = 75;
        FpEntry.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
        FpEntry.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        FpEntry.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
        FpEntry.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
        FpEntry.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;


        FpEntry.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpEntry.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpEntry.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
    }

    public void loadreport(string staffcode)
    {
        try
        {
            ds.Dispose();

            string academicyear = ddlbatch.SelectedValue.ToString();

            string staffqury = "select distinct  len(ss.staff_code),ss.staff_code,sm.staff_name,sy.degree_code,sy.batch_year,sy.semester,ss.sections,sy.syll_code,ss.subject_no,s.subtype_no,s.subject_name,si.start_date,si.end_date,cer.criteria,exa.exam_code,cer.min_mark,dept.dept_name from staff_selector ss,subject s,syllabus_master sy,seminfo si,sub_sem sb,staffmaster sm,exam_type exa,criteriaforinternal cer,registration reg,department dept,degree deg  where  ss.staff_code=sm.staff_code and s.subject_no=ss.subject_no and s.syll_code=sy.syll_code and ss.batch_year=sy.batch_year and si.degree_code=sy.degree_code and si.semester=sy.semester and si.batch_year=sy.batch_year and sb.syll_code=s.syll_code and sb.subtype_no=s.subtype_no and sb.promote_count=1";
            staffqury = " " + staffqury + "and sy.syll_code=cer.syll_code and sy.batch_year=exa.batch_year and exa.subject_no=ss.subject_no and cer.criteria_no=exa.criteria_no and exa.sections=ss.sections and reg.batch_year=sy.batch_year and reg.degree_code=sy.degree_code and reg.current_semester=sy.semester and reg.cc=0 and reg.delflag=0  and reg.exam_flag<>'debar' and dept.dept_code=deg.dept_code and deg.degree_code= sy.degree_code and sb.Lab in(1,0) and '" + academicyear + "' between year(start_date) and year(end_date) and ss.staff_code in(" + staffcode + ") order by len(ss.staff_code),ss.staff_code,sy.degree_code,sy.batch_year,sy.semester,sy.syll_code,ss.subject_no,s.subtype_no,s.subject_name";
            hat.Clear();
            DataSet dsreoprt = d2.select_method(staffqury, hat, "Text");
            if (dsreoprt.Tables[0].Rows.Count > 0)
            {
                for (int repo = 0; repo < dsreoprt.Tables[0].Rows.Count; repo++)
                {
                    string staffname = dsreoprt.Tables[0].Rows[repo]["staff_name"].ToString();
                    string staff = dsreoprt.Tables[0].Rows[repo]["staff_code"].ToString();
                    string subject = dsreoprt.Tables[0].Rows[repo]["subject_name"].ToString();
                    string degreecode = dsreoprt.Tables[0].Rows[repo]["degree_code"].ToString();
                    string subjectno = dsreoprt.Tables[0].Rows[repo]["subject_no"].ToString();
                    string batch = dsreoprt.Tables[0].Rows[repo]["batch_year"].ToString();
                    string syllcode = dsreoprt.Tables[0].Rows[repo]["syll_code"].ToString();
                    string sections = dsreoprt.Tables[0].Rows[repo]["sections"].ToString();
                    string semester = dsreoprt.Tables[0].Rows[repo]["semester"].ToString();
                    string examname = dsreoprt.Tables[0].Rows[repo]["criteria"].ToString();

                    if (arinternal.Contains(examname) == false)
                    {
                        arinternal.Add(examname);
                    }
                    string examcode = dsreoprt.Tables[0].Rows[repo]["exam_code"].ToString();
                    string minmarks = dsreoprt.Tables[0].Rows[repo]["min_mark"].ToString();
                    string departmentvalue = dsreoprt.Tables[0].Rows[repo]["dept_name"].ToString();
                    string sp_section = "";// added by sridhar aug 2014


                    if (sections.ToString().Trim() == "-1" || sections.ToString().Trim() == "" || sections.ToString().Trim() == null || sections.ToString().Trim() == "All")
                    {
                        sections = "";
                        sp_section = "";// added by sridhar aug 2014
                    }
                    else
                    {
                        sp_section = sections;// added by sridhar aug 2014
                        sections = "and r.sections='" + sections + "'";

                    }
                    string totalstudent = "";
                    string staffvaluequery = "select distinct count(s.roll_no) as total from subjectchooser s,registration r where r.roll_no=s.roll_no and subject_no='" + subjectno + "' and r.batch_year=" + batch + " and r.current_semester=s.semester and s.semester=" + semester + " and r.degree_code=" + degreecode + " " + sections + "";
                    hat.Clear();
                    DataSet dsstaff = d2.select_method(staffvaluequery, hat, "Text");
                    if (dsstaff.Tables[0].Rows.Count > 0)
                    {
                        totalstudent = dsstaff.Tables[0].Rows[0]["total"].ToString();
                    }

                    hat.Clear();
                    hat.Add("exam_code", examcode);
                    hat.Add("min_marks", minmarks);
                    hat.Add("section", sp_section);
                    DataSet dsexamdetails = d2.select_method("Proc_All_Subject_Details", hat, "sp");
                    if (dsexamdetails.Tables[0].Rows.Count > 0)
                    {
                        string appear = dsexamdetails.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                        string passcount = dsexamdetails.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                        string failcount = dsexamdetails.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();

                        //inser internal
                        string insertallexam = "insert into tbl_staff_topper (staff_code,staff_name,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail,isExternal,user_code) values ";
                        insertallexam = "" + insertallexam + " ('" + staff + "','" + staffname + "','" + departmentvalue + "','" + subject + "','" + examname + "'," + totalstudent + "," + appear + "," + passcount + "," + failcount + ",0,'" + usercode + "')";
                        int value = d2.insert_method(insertallexam, hat, "Text");
                    }
                }
            }

        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void loadexternal(string staffcode)
    {
        hat.Clear();
        ds.Dispose();
        string academicyearvalue = ddlbatch.SelectedValue.ToString();
        ds.Dispose();
        ds = d2.select_method("if exists(select name from sysobjects where xtype='p' and name='Sp_External_Student_Details' )drop proc Sp_External_Student_Details", hat, "text ");
        int s = d2.insert_method("Create procedure Sp_External_Student_Details @subject_no int,@Exam_code int as Begin select count(roll_no) as Present_count from mark_entry where result<>'AAA' and exam_code=@Exam_code and subject_no=@subject_no select count(roll_no) as Absent_count from mark_entry where result='AAA' and exam_code=@Exam_code and subject_no=@subject_no select count(roll_no) as Pass_Count from mark_entry where result='Pass' and exam_code=@Exam_code and subject_no=@subject_no select count(roll_no) As Fail_Count_With_AB from mark_entry where result !='Pass' and exam_code=@Exam_code and subject_no=@subject_no select count(roll_no) As Fail_Count_Without_AB from mark_entry where result!='pass' and result !='AAA' and exam_code=@Exam_code and subject_no=@subject_no select count(roll_no) as Total from mark_entry where exam_code=@Exam_code and subject_no=@subject_no end", hat, "Text");
        ds.Dispose();

        //string externalstaffquery = "select distinct len(ss.staff_code),ss.staff_code,sm.staff_name,sy.degree_code,sy.batch_year,sy.semester,ss.sections,sy.syll_code,ss.subject_no,s.subtype_no,s.subject_name,si.start_date,si.end_date,dept.dept_name,exa.exam_code,exa.exam_month,exa.exam_year,st.dept_code from staff_selector ss,subject s,syllabus_master sy,seminfo si,sub_sem sb,staffmaster sm,registration reg,stafftrans st,department dept,exam_details exa where  ss.staff_code=sm.staff_code and s.subject_no=ss.subject_no and s.syll_code=sy.syll_code and ss.batch_year=sy.batch_year and si.degree_code=sy.degree_code and si.semester=sy.semester and si.batch_year=sy.batch_year and sb.syll_code=s.syll_code and sb.subtype_no=s.subtype_no and sb.promote_count=1";
        //externalstaffquery = "" + externalstaffquery + " and reg.batch_year=sy.batch_year and reg.degree_code=sy.degree_code and reg.current_semester=sy.semester and reg.batch_year=exa.batch_year and exa.degree_code=reg.degree_code and reg.current_semester=exa.current_semester and reg.cc=0 and reg.delflag=0  and reg.exam_flag<>'debar'  and dept.dept_code= st.dept_code and st.staff_code=ss.staff_code and sb.Lab in(1,0) and '" + academicyearvalue + "' between year(start_date) and year(end_date) and ss.staff_code in(" + staffcode + ")   order by len(ss.staff_code),ss.staff_code,sy.degree_code,sy.batch_year, sy.semester,sy.syll_code,ss.subject_no,s.subtype_no,s.subject_name";

        string externalstaffquery = "select distinct len(ss.staff_code),ss.staff_code,sm.staff_name,sy.degree_code,sy.batch_year,sy.semester,ss.sections,sy.syll_code,ss.subject_no,s.subtype_no,s.subject_name,si.start_date,si.end_date,dept.dept_name,exa.exam_code,exa.exam_month,exa.exam_year,st.dept_code from staff_selector ss,subject s,syllabus_master sy,seminfo si,sub_sem sb,staffmaster sm,registration reg,stafftrans st,department dept,exam_details exa where  ss.staff_code=sm.staff_code and s.subject_no=ss.subject_no and s.syll_code=sy.syll_code and ss.batch_year=sy.batch_year and si.degree_code=sy.degree_code and si.semester=sy.semester and si.batch_year=sy.batch_year and sb.syll_code=s.syll_code and sb.subtype_no=s.subtype_no and sb.promote_count=1";
        externalstaffquery = "" + externalstaffquery + " and reg.batch_year=sy.batch_year and reg.degree_code=sy.degree_code  and reg.batch_year=exa.batch_year and exa.degree_code=reg.degree_code  and reg.cc=0 and reg.delflag=0  and reg.exam_flag<>'debar' and sy.semester=exa.current_semester and dept.dept_code= st.dept_code and st.staff_code=ss.staff_code and sb.Lab in(1,0) and '" + academicyearvalue + "' between year(start_date) and year(end_date) and ss.staff_code in(" + staffcode + ")   order by len(ss.staff_code),ss.staff_code,sy.degree_code,sy.batch_year, sy.semester,sy.syll_code,ss.subject_no,s.subtype_no,s.subject_name";


        DataSet dsexterstaff = d2.select_method(externalstaffquery, hat, "Text");

        if (dsexterstaff.Tables[0].Rows.Count > 0)
        {
            for (int extr = 0; extr < dsexterstaff.Tables[0].Rows.Count; extr++)
            {
                string examcode = dsexterstaff.Tables[0].Rows[extr]["exam_code"].ToString();
                string subjectno = dsexterstaff.Tables[0].Rows[extr]["subject_no"].ToString();
                string Staffcode = dsexterstaff.Tables[0].Rows[extr]["staff_code"].ToString();
                string staffname = dsexterstaff.Tables[0].Rows[extr]["staff_name"].ToString();
                string degree = dsexterstaff.Tables[0].Rows[extr]["dept_name"].ToString();
                string subject = dsexterstaff.Tables[0].Rows[extr]["subject_name"].ToString();
                string exammonth = dsexterstaff.Tables[0].Rows[extr]["Exam_month"].ToString();
                string examyear = dsexterstaff.Tables[0].Rows[extr]["Exam_Year"].ToString();

                if (exammonth == "1")
                    exammonth = "Jan";
                else if (exammonth == "2")
                    exammonth = "Feb";
                else if (exammonth == "3")
                    exammonth = "Mar";
                else if (exammonth == "4")
                    exammonth = "Apr";
                else if (exammonth == "5")
                    exammonth = "May";
                else if (exammonth == "6")
                    exammonth = "Jun";
                else if (exammonth == "7")
                    exammonth = "Jul";
                else if (exammonth == "8")
                    exammonth = "Aug";
                else if (exammonth == "9")
                    exammonth = "Sep";
                else if (exammonth == "10")
                    exammonth = "Oct";
                else if (exammonth == "11")
                    exammonth = "Nov";
                else if (exammonth == "12")
                    exammonth = "Dec";

                hat.Clear();
                hat.Add("Exam_code", examcode);
                hat.Add("Subject_no", subjectno);
                DataSet dsexterdetail = d2.select_method("Sp_External_Student_Details", hat, "sp");
                if (dsexterdetail.Tables[0].Rows.Count > 0)
                {
                    string Total = dsexterdetail.Tables[5].Rows[0]["Total"].ToString();
                    string Pass = dsexterdetail.Tables[2].Rows[0]["Pass_Count"].ToString();
                    string Fail = dsexterdetail.Tables[3].Rows[0]["Fail_Count_With_AB"].ToString();
                    string Appear = dsexterdetail.Tables[0].Rows[0]["Present_count"].ToString();

                    int totalcount = Convert.ToInt32(Total) + Convert.ToInt32(Pass) + Convert.ToInt32(Fail) + Convert.ToInt32(Appear);
                    if (totalcount != 0)
                    {
                        //insert External
                        string insertallexam = "insert into tbl_staff_topper (staff_code,staff_name,degree,subject,external_exam_type,ext_total,ext_appear,ext_pass,ext_fail,isExternal,user_code) values ";
                        insertallexam = "" + insertallexam + " ('" + Staffcode + "','" + staffname + "','" + degree + "','" + subject + "','" + exammonth + " / " + examyear + "'," + Total + "," + Appear + "," + Pass + "," + Fail + ",1,'" + usercode + "')";
                        int value = d2.insert_method(insertallexam, hat, "Text");
                    }
                }

            }
        }

    }

    protected void FpEntry_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        checkflag = true;
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)// added by gowtham july'25
    {
        Session["column_header_row_count"] = 2;
        string deg_details = string.Empty;
        string degree_pdf = string.Empty;
        string header = string.Empty;
        deg_details = "Faculty Performance";

        string degreedetails = string.Empty;
        degreedetails = deg_details;
        string pagename = "FacultyPerformance.aspx";
        Printcontrol.loadspreaddetails(FpReport, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnexcel_Click(object sender, EventArgs e)// added by gowtham july'25
    {
        try
        {

            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                norecordlbl.Visible = false;
                d2.printexcelreport(FpReport, reportname);
                txtexcelname.Text = "";
            }
            else
            {
                norecordlbl.Text = "Please Enter Your Report Name";
                norecordlbl.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void FpEntry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (checkflag == true)
            {
                lblnorec.Visible = false;
                string activerow = "";
                string activecol = "";
                string code = "";
                string Name = "";
                activerow = FpEntry.ActiveSheetView.ActiveRow.ToString();
                activecol = FpEntry.ActiveSheetView.ActiveColumn.ToString();
                if (ddlorder.SelectedItem.Text == "Dept.")
                {
                    code = FpEntry.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                    Name = FpEntry.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                }
                else if (ChkIndividual.Checked == true) // added by sridhar 04 sep 2014
                {
                    code = FpEntry.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                    Name = FpEntry.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;

                }
                else
                {
                    code = FpEntry.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                    Name = FpEntry.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                }

                string loadquery = "";
                if (ddlexam.SelectedIndex.ToString() == "1")
                {
                    loadquery = "select distinct ROW_NUMBER() OVER(ORDER BY degree DESC) AS Row,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail from tbl_staff_topper where isexternal=0 and staff_code='" + code + "' and user_code='" + usercode + "'";

                }
                else if (ddlexam.SelectedIndex.ToString() == "2")
                {
                    loadquery = "select  ROW_NUMBER() OVER(ORDER BY degree DESC) AS Row,degree,subject,external_exam_type,ext_total,ext_appear,ext_pass,ext_fail from tbl_staff_topper where isexternal=1 and staff_code='" + code + "' and user_code='" + usercode + "'";
                }
                else
                {
                    string external = FpEntry.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Note;
                    loadquery = "select distinct ROW_NUMBER() OVER(ORDER BY degree DESC) AS Row, degree,subject,isnull(internal_exam_type,'-') as internalexam,isnull(in_total,'0') as intotal,isnull(in_appear,'0') as inappear,isnull(in_pass,'0') as intpass,isnull(in_fail,'0') as isnull,isnull(external_exam_type,'-') as externalexam,isnull(ext_total,'0') as exttotal,isnull(ext_appear,'0') as extappear,isnull(ext_pass,'0') as extpass ,isnull(ext_fail,'0') as extfail from tbl_staff_topper where staff_code='" + code + "' and user_code='" + usercode + "'";
                }
                if (loadquery != "")
                {
                    DataSet dsload = d2.select_method(loadquery, hat, "Text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        btnprint.Visible = true;
                        txtexcelname.Visible = true;
                        btnexcel.Visible = true;
                        FpReport.Visible = true;
                        FpReport.Sheets[0].ColumnCount = 8;
                        FpReport.Sheets[0].ColumnHeader.RowCount = 2;
                        FpReport.Sheets[0].RowCount = 0;
                        FpReport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "" + Name + "     Performance";
                        FpReport.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpReport.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                        FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 8);

                        FpReport.Sheets[0].ColumnHeader.Cells[1, 0].Text = "S.No";
                        FpReport.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Degree";
                        FpReport.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Subject";
                        FpReport.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Exam Type";
                        FpReport.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Total";
                        FpReport.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Apper ";
                        FpReport.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Pass ";
                        FpReport.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Fail";

                        FpReport.Sheets[0].ColumnHeader.Rows[1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpReport.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                        FpReport.Sheets[0].Columns[0].Width = 50;
                        FpReport.Sheets[0].Columns[1].Width = 150;
                        FpReport.Sheets[0].Columns[2].Width = 150;
                        FpReport.Sheets[0].Columns[3].Width = 80;
                        FpReport.Sheets[0].Columns[4].Width = 80;
                        FpReport.Sheets[0].Columns[5].Width = 80;
                        FpReport.Sheets[0].Columns[6].Width = 80;
                        FpReport.Sheets[0].Columns[7].Width = 80;

                        FpReport.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                        FpReport.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                        FpReport.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                        FpReport.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                        FpReport.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                        FpReport.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                        FpReport.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                        FpReport.Sheets[0].Columns[7].Font.Name = "Book Antiqua";


                        FpReport.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpReport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpReport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                        FpReport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                        FpReport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                        FpReport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                        FpReport.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                        FpReport.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;


                        FpReport.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
                        FpReport.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;

                        FpReport.Sheets[0].Columns[0].Locked = true;
                        FpReport.Sheets[0].Columns[1].Locked = true;
                        FpReport.Sheets[0].Columns[2].Locked = true;
                        FpReport.Sheets[0].Columns[3].Locked = true;
                        FpReport.Sheets[0].Columns[4].Locked = true;
                        FpReport.Sheets[0].Columns[5].Locked = true;
                        FpReport.Sheets[0].Columns[6].Locked = true;
                        FpReport.Sheets[0].Columns[7].Locked = true;

                        if (ddlexam.SelectedIndex == 0)
                        {
                            FpReport.Sheets[0].ColumnCount = FpReport.Sheets[0].ColumnCount + 5;
                            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, FpReport.Sheets[0].ColumnCount);
                            FpReport.Sheets[0].ColumnHeader.Cells[1, 8].Text = "External Exam";
                            FpReport.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Total";
                            FpReport.Sheets[0].ColumnHeader.Cells[1, 10].Text = "Appear";
                            FpReport.Sheets[0].ColumnHeader.Cells[1, 11].Text = "Pass";
                            FpReport.Sheets[0].ColumnHeader.Cells[1, 12].Text = "Fail";
                            FpReport.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
                            FpReport.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                            FpReport.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                            FpReport.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
                            FpReport.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpReport.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpReport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpReport.SaveChanges();

                        btnprint.Visible = true;
                        txtexcelname.Visible = true;
                        btnexcel.Visible = true;
                        FpReport.Visible = true;
                        FpReport.DataSource = dsload;
                        FpReport.DataBind();
                    }
                    else
                    {
                        btnprint.Visible = false;
                        txtexcelname.Visible = false;
                        btnexcel.Visible = false;
                        txtexcelname.Text = "";
                        FpReport.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = " No Record Found";
                    }
                }
                int rowcount = FpReport.Sheets[0].RowCount;
                FpReport.Height = 700;
                FpReport.Sheets[0].PageSize = 25 + (rowcount * 20);
                FpReport.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    protected void chktopbelow_CheckedChanged(object sender, EventArgs e)
    {
        if (chktopbelow.Checked == true)
        {
            rbtop.Checked = true;
            rbtop.Enabled = true;
            rbbelow.Enabled = true;
            rbbelow.Checked = false;
            txtrange.Enabled = true;
            txtrange.Text = "";
            if (rbtop.Checked == true)
            {
                lblrange.Text = "Top";
            }
            else if (rbbelow.Checked == true)
            {
                lblrange.Text = "Below";

            }
        }
        else
        {
            rbtop.Checked = false;
            rbtop.Enabled = false;
            rbbelow.Enabled = false;
            rbbelow.Checked = false;
            txtrange.Text = "";
            txtrange.Enabled = false;
            if (rbtop.Checked == true)
            {
                lblrange.Text = "Top";
            }
            else if (rbbelow.Checked == true)
            {
                lblrange.Text = "Below";

            }

        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtxl.Text;

            if (reportname.ToString().Trim() != "")
            {
                lbl_error.Visible = false;
                d2.printexcelreport(FpEntry, reportname);

            }
            else
            {
                lbl_error.Text = "Please Enter Your Report Name";
                lbl_error.Visible = true;
            }
            txtxl.Text = "";
            reportname = "";
        }
        catch (Exception ex)
        {
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
        }
    }
    protected void ChkIndividual_CheckedChanged(object sender, EventArgs e)
    {
        ddlexam.Items.Clear();
        if (ChkIndividual.Checked == true)
        {
            ddlexam.Items.Add("Internal");

        }
        else
        {
            ddlexam.Items.Add("All");
            ddlexam.Items.Add("Internal");
            ddlexam.Items.Add("External");
        }
    }

    protected void btnprintmasterr_Click(object sender, EventArgs e)
    {
        string filter = "";
        Session["column_header_row_count"] = FpEntry.Sheets[0].ColumnHeader.RowCount;
        string batch = string.Empty;
        string deg = string.Empty;
        string brnch = string.Empty;
        string sel_seattype = string.Empty;
        string degreedetails = string.Empty;

        if (chktopbelow.Checked == true)
        {
            if (rbtop.Checked == true)
            {
                filter = "@Top :" + txtrange.Text;
            }
            else if (rbbelow.Checked == true)
            {
                filter = "@Below :" + txtrange.Text;
            }
        }

        degreedetails = "Faculty Performance @" + "Academic Year:" + ddlbatch.SelectedItem.Text + "@Exam Type :" + ddlexam.SelectedItem.Text + filter;// +"@Branch :" + brnch + "@Seat Type :" + sel_seattype;

        string pagename = "FacultyPerformance.aspx";

        Printcontrol.loadspreaddetails(FpEntry, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}