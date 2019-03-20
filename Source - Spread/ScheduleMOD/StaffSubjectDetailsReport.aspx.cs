using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class staffsubjecthoursreport : System.Web.UI.Page
{
    string grouporusercode = "", singleuser = "", group_user = "", usercode = "", collegecode = "", strquery = "";
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    int selectcount = 0;
    string batchval = "", degreeval = "", semval = "", sectval = "", subject_no = "";
    DataSet dssubject = new DataSet();
    Dictionary<DateTime, string> hat_holy = new Dictionary<DateTime, string>();
    Dictionary<string, int> dsubdic = new Dictionary<string, int>();
    int totconducthour = 0;
    Boolean splhr_flag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = "and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = "and user_code=" + Session["usercode"].ToString().Trim() + "";
            }
            usercode = Session["usercode"].ToString();

            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                fpsubjectdetails.Sheets[0].SheetName = " ";
                fpsubjectdetails.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                fpsubjectdetails.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
                fpsubjectdetails.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                fpsubjectdetails.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpsubjectdetails.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpsubjectdetails.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = Color.Black;
                fpsubjectdetails.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                fpsubjectdetails.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                fpsubjectdetails.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                fpsubjectdetails.Sheets[0].AllowTableCorner = true;
                //---------------page number

                fpsubjectdetails.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpsubjectdetails.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpsubjectdetails.Pager.Align = HorizontalAlign.Right;
                fpsubjectdetails.Pager.Font.Bold = true;
                fpsubjectdetails.Pager.Font.Name = "Book Antiqua";
                fpsubjectdetails.Pager.ForeColor = Color.DarkGreen;
                fpsubjectdetails.Pager.BackColor = Color.Beige;
                fpsubjectdetails.Pager.BackColor = Color.AliceBlue;
                fpsubjectdetails.Pager.PageCount = 5;
                fpsubjectdetails.CommandBar.Visible = false;

                fpsubjectdetails.SheetCorner.ColumnCount = 0;

                fpsubjectdetails.Sheets[0].ColumnHeader.RowCount = 1;
                fpsubjectdetails.Sheets[0].ColumnCount = 6;
                fpsubjectdetails.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpsubjectdetails.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Name";
                fpsubjectdetails.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                fpsubjectdetails.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total No of Hrs Taken";
                fpsubjectdetails.Sheets[0].ColumnHeader.Cells[0, 4].Text = "No of Hrs As Per Syllabus";
                fpsubjectdetails.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Remarks";
                fpsubjectdetails.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpsubjectdetails.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                fpsubjectdetails.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                fpsubjectdetails.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                fpsubjectdetails.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                fpsubjectdetails.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;

                fpsubjectdetails.Sheets[0].Columns[0].Width = 50;
                fpsubjectdetails.Sheets[0].Columns[1].Width = 150;
                fpsubjectdetails.Sheets[0].Columns[2].Width = 100;
                fpsubjectdetails.Sheets[0].Columns[3].Width = 100;
                fpsubjectdetails.Sheets[0].Columns[4].Width = 100;
                fpsubjectdetails.Sheets[0].Columns[5].Width = 100;

                fpsubjectdetails.Sheets[0].Columns[0].Locked = true;
                fpsubjectdetails.Sheets[0].Columns[1].Locked = true;
                fpsubjectdetails.Sheets[0].Columns[2].Locked = true;
                fpsubjectdetails.Sheets[0].Columns[3].Locked = true;
                fpsubjectdetails.Sheets[0].Columns[4].Locked = true;
                fpsubjectdetails.Sheets[0].Columns[5].Locked = true;

                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                fpsubjectdetails.Visible = false;
                BindCollege();
                BindDegree();
                for (int i = 0; i < chklsyear.Items.Count; i++)
                {
                    chklsyear.Items[i].Selected = true;
                }
                chkyear.Checked = true;
                btngo.Enabled = false;
                txtbranch.Enabled = false;
                txtdegree.Enabled = false;
                txtsec.Enabled = false;
                txtyear.Enabled = false;
                if (chklsdegree.Items.Count > 0)
                {
                    BindBranchMultiple(singleuser, group_user, collegecode, usercode);
                    btngo.Enabled = true;
                    txtbranch.Enabled = true;
                    txtdegree.Enabled = true;
                    txtsec.Enabled = true;
                    txtyear.Enabled = true;

                }
            }
        }
        catch (Exception ex) { }
    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void BindCollege()
    {
        try
        {
            if (!IsPostBack)
            {
                Session["QueryString"] = "";

                hat.Clear();
                hat.Add("column_field", grouporusercode.ToString());
                ds.Dispose();
                ds.Reset();
                ds = da.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = ds;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void BindDegree()
    {
        try
        {
            txtdegree.Text = "---Select---";
            selectcount = 0;
            chklsdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            collegecode = ddlcollege.SelectedValue.ToString();
            if (collegecode != "")
            {
                btngo.Enabled = true;
                txtbranch.Enabled = true;
                txtdegree.Enabled = true;
                txtsec.Enabled = true;
                txtyear.Enabled = true;
                ds.Dispose();
                ds.Reset();
                ds = da.BindDegree(singleuser, group_user, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklsdegree.DataSource = ds;
                    chklsdegree.DataTextField = "course_name";
                    chklsdegree.DataValueField = "course_id";
                    chklsdegree.DataBind();
                    chklsdegree.Items[0].Selected = true;
                    for (int i = 0; i < chklsdegree.Items.Count; i++)
                    {
                        chklsdegree.Items[i].Selected = true;
                        if (chklsdegree.Items[i].Selected == true)
                        {
                            selectcount += 1;
                        }
                        if (chklsdegree.Items.Count == selectcount)
                        {
                            chkdegree.Checked = true;
                        }
                    }
                }
            }
            else
            {
                btngo.Enabled = false;
                txtbranch.Enabled = false;
                txtdegree.Enabled = false;
                txtsec.Enabled = false;
                txtyear.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void BindBranchMultiple(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            txtbranch.Text = "---Select---";
            selectcount = 0;
            string course_id = "";
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                if (chklsdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklsdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklsdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklsbranch.Items.Clear();
            if (course_id != "")
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                ds.Dispose();
                ds.Reset();
                collegecode = ddlcollege.SelectedValue.ToString();
                ds = da.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklsbranch.DataSource = ds;
                    chklsbranch.DataTextField = "dept_name";
                    chklsbranch.DataValueField = "degree_code";
                    chklsbranch.DataBind();
                    chklsbranch.Items[0].Selected = true;
                    for (int i = 0; i < chklsbranch.Items.Count; i++)
                    {
                        chklsbranch.Items[i].Selected = true;
                        if (chklsbranch.Items[i].Selected == true)
                        {
                            selectcount += 1;
                        }
                        if (chklsbranch.Items.Count == selectcount)
                        {
                            chkbranch.Checked = true;
                        }
                    }
                }
                BindSectionDetail();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void BindSectionDetail()
    {
        try
        {
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            fpsubjectdetails.Visible = false;
            txtsec.Text = "---Select---";
            string batch = "";
            string branch = "";
            for (int i = 0; i < chklsyear.Items.Count; i++)
            {
                if (chklsyear.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = "" + chklsyear.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        batch = batch + "," + "" + chklsyear.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (batch != "")
            {
                batch = "and Current_Semester in (" + batch + ")";
            }

            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                if (chklsbranch.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "'" + chklsbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        branch = branch + "," + "'" + chklsbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (branch != "")
            {
                branch = "and degree_code in(" + branch + ")";
            }
            chklssec.Items.Clear();

            if (branch != "" && batch != "")
            {
                ds.Dispose();
                ds.Reset();
                chklssec.Items.Insert(0, " ");
                strquery = "Select distinct sections from Registration where cc=0 and delflag=0 and exam_flag<>'debar' " + batch + " " + branch + " and college_code=" + ddlcollege.SelectedValue.ToString() + " and sections<>'' and sections!='-1' and sections is not null";
                ds = da.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    chklssec.DataSource = ds;
                    chklssec.DataTextField = "sections";
                    chklssec.DataBind();
                    chklssec.Items.Insert(0, " ");
                    if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                    {
                        chklssec.Enabled = false;
                    }
                    else
                    {
                        chklssec.Enabled = true;
                        chklssec.SelectedIndex = chklssec.Items.Count - 2;
                        chklssec.Items[0].Selected = true;
                        for (int i = 0; i < chklssec.Items.Count; i++)
                        {
                            chklssec.Items[i].Selected = true;
                            if (chklssec.Items[i].Selected == true)
                            {
                                selectcount += 1;
                            }
                            if (chklssec.Items.Count == selectcount)
                            {
                                chksec.Checked = true;
                            }
                        }
                        chksec.Checked = true;
                    }
                }
                else
                {
                    chklssec.Items[0].Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDegree();
        BindBranchMultiple(singleuser, group_user, collegecode, usercode);
    }
    protected void chkyear_CheckedChanged(object sender, EventArgs e)
    {
        if (chkyear.Checked == true)
        {
            for (int i = 0; i < chklsyear.Items.Count; i++)
            {
                chklsyear.Items[i].Selected = true;
            }
            txtyear.Text = "Year (" + chklsyear.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsyear.Items.Count; i++)
            {
                chklsyear.Items[i].Selected = false;
            }
            txtyear.Text = "---Select---";
        }
        BindDegree();
        BindBranchMultiple(singleuser, group_user, collegecode, usercode);
    }
    public void chklsyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < chklsyear.Items.Count; i++)
        {
            if (chklsyear.Items[i].Selected == true)
            {
                selectcount++;
            }
        }
        if (selectcount == 0)
        {
            txtyear.Text = "---Select---";
            chkyear.Checked = false;
        }
        else if (selectcount == chklsyear.Items.Count)
        {
            txtyear.Text = "Year (" + chklsyear.Items.Count + ")";
            chkyear.Checked = true;
        }
        else
        {
            txtyear.Text = "Year (" + selectcount + ")";
            chkyear.Checked = false;
        }

        BindDegree();
        BindBranchMultiple(singleuser, group_user, collegecode, usercode);
    }
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdegree.Checked == true)
        {
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                chklsdegree.Items[i].Selected = true;
            }
            txtdegree.Text = "Degree (" + chklsdegree.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                chklsdegree.Items[i].Selected = false;
            }
            txtdegree.Text = "---Select---";
        }

        BindBranchMultiple(singleuser, group_user, collegecode, usercode);
    }
    protected void chklsdegree_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < chklsdegree.Items.Count; i++)
        {
            if (chklsdegree.Items[i].Selected == true)
            {
                selectcount++;
            }
        }
        if (selectcount == 0)
        {
            txtdegree.Text = "---Select---";
            chkdegree.Checked = false;
        }
        else if (selectcount == chklsdegree.Items.Count)
        {
            txtdegree.Text = "Degree (" + chklsdegree.Items.Count + ")";
            chkdegree.Checked = true;
        }
        else
        {
            txtdegree.Text = "Degree (" + selectcount + ")";
            chkdegree.Checked = false;
        }

        BindBranchMultiple(singleuser, group_user, collegecode, usercode);
    }
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbranch.Checked == true)
        {
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                chklsbranch.Items[i].Selected = true;
            }
            txtbranch.Text = "Branch (" + chklsbranch.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                chklsbranch.Items[i].Selected = false;
            }
            txtbranch.Text = "---Select---";
        }
        BindSectionDetail();
    }
    protected void chklsbranch_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < chklsbranch.Items.Count; i++)
        {
            if (chklsbranch.Items[i].Selected == true)
            {
                selectcount++;
            }
        }
        if (selectcount == 0)
        {
            txtbranch.Text = "---Select---";
            chkbranch.Checked = false;
        }
        else if (selectcount == chklsbranch.Items.Count)
        {
            txtbranch.Text = "Branch (" + chklsbranch.Items.Count + ")";
            chkbranch.Checked = true;
        }
        else
        {
            txtbranch.Text = "Branch (" + selectcount + ")";
            chkbranch.Checked = false;
        }
        BindSectionDetail();
    }
    protected void chksec_CheckedChanged(object sender, EventArgs e)
    {
        if (chksec.Checked == true)
        {
            for (int i = 0; i < chklssec.Items.Count; i++)
            {
                chklssec.Items[i].Selected = true;
            }
            txtsec.Text = "Sec (" + chklssec.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklssec.Items.Count; i++)
            {
                chklssec.Items[i].Selected = false;
            }
            txtsec.Text = "---Select---";
        }
    }
    protected void chklssec_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < chklssec.Items.Count; i++)
        {
            if (chklssec.Items[i].Selected == true)
            {
                selectcount++;
            }
        }
        if (selectcount == 0)
        {
            txtsec.Text = "---Select---";
            chksec.Checked = false;
        }
        else if (selectcount == chklsyear.Items.Count)
        {
            txtsec.Text = "Sec (" + chklssec.Items.Count + ")";
            chksec.Checked = true;
        }
        else
        {
            txtsec.Text = "Sec (" + selectcount + ")";
            chksec.Checked = false;
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            fpsubjectdetails.Sheets[0].RowCount = 0;
            string sem = "";
            string branch = "";
            hat.Clear();


            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                if (chklsbranch.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "'" + chklsbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        branch = branch + "," + "'" + chklsbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (branch != "")
            {
                branch = "and degree_code in(" + branch + ")";
            }
            string sections = "";
            for (int i = 0; i < chklssec.Items.Count; i++)
            {
                if (chklssec.Items[i].Selected == true)
                {
                    if (sections == "")
                    {
                        sections = "'" + chklssec.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        sections = sections + "," + "'" + chklssec.Items[i].Value.ToString() + "'";
                    }
                }

            }
            if (sections != "")
            {
                sections = "and sections in(" + sections + ")";
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            string querystring = "select rights from  special_hr_rights where " + grouporusercode + "";
            DataSet dsrights = da.select_method(querystring, hat, "Text");
            if (dsrights.Tables[0].Rows.Count > 0)
            {
                if (dsrights.Tables[0].Rows[0]["rights"].ToString().ToLower().Trim() == "true")
                {
                    splhr_flag = true;
                }
            }
            collegecode = ddlcollege.SelectedValue.ToString();
            int srno = 0;
            for (int bat = 0; bat < chklsyear.Items.Count; bat++)
            {
                if (chklsyear.Items[bat].Selected == true)
                {
                    sem = "" + chklsyear.Items[bat].Value.ToString() + "";
                    sem = "and Current_Semester in (" + sem + ")";
                    strquery = "Select distinct Batch_Year,degree_code,Current_Semester,sections from Registration where cc=0 and delflag=0 and exam_flag<>'debar' " + sem + " " + branch + " " + sections + " and college_code=" + collegecode + " order by degree_code,Current_Semester,Sections";
                    ds.Dispose();
                    ds.Reset();
                    ds = da.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        btnxl.Visible = true;
                        btnprintmaster.Visible = true;
                        Printcontrol.Visible = false;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        fpsubjectdetails.Visible = true;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            dsubdic.Clear();
                            batchval = ds.Tables[0].Rows[i]["batch_year"].ToString();
                            degreeval = ds.Tables[0].Rows[i]["degree_code"].ToString();
                            semval = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                            sectval = ds.Tables[0].Rows[i]["sections"].ToString();
                            string getval = batchval + '/' + degreeval + '/' + semval;
                            string getdegreebracnh = da.GetFunction("select c.Course_Name+'-'+de.Dept_Name from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and c.college_code=d.college_code and d.college_code=de.college_code and c.college_code=de.college_code and d.Degree_Code=" + degreeval + "");
                            string settext = "";
                            if (sectval != "" && sectval != null && sectval != "-1")
                            {
                                settext = chklsyear.Items[bat].Text.ToString() + " / " + batchval + " - " + getdegreebracnh + "- Sec - " + sectval;
                            }
                            else
                            {
                                settext = chklsyear.Items[bat].Text.ToString() + " / " + batchval + " - " + getdegreebracnh;
                            }
                            fpsubjectdetails.Sheets[0].RowCount++;
                            fpsubjectdetails.Sheets[0].Cells[fpsubjectdetails.Sheets[0].RowCount - 1, 0].Text = settext;
                            fpsubjectdetails.Sheets[0].Cells[fpsubjectdetails.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            fpsubjectdetails.Sheets[0].SpanModel.Add(fpsubjectdetails.Sheets[0].RowCount - 1, 0, 1, 9);
                            int startrow = fpsubjectdetails.Sheets[0].RowCount;
                            dssubject.Dispose();
                            dssubject.Reset();
                            dsubdic.Clear();
                            strquery = "Select S.Subject_Code,S.Subject_no,s.subject_name from Subject s,Sub_Sem ss,Syllabus_Master as SMas where SMas.Syll_Code = S.Syll_Code and SMas.Syll_Code = SS.Syll_Code and SS.Syll_Code = S.Syll_Code and S.SubType_no = SS.Subtype_no and SS.Promote_Count = 1 and SMas.Degree_Code =" + degreeval + " and SMas.Batch_Year =" + batchval + " and SMas.Semester = " + semval + " order by S.Subject_no, SS.SubType_No ";
                            dssubject = da.select_method_wo_parameter(strquery, "Text");
                            for (int sub = 0; sub < dssubject.Tables[0].Rows.Count; sub++)
                            {
                                srno++;
                                fpsubjectdetails.Sheets[0].RowCount++;
                                subject_no = dssubject.Tables[0].Rows[sub]["Subject_no"].ToString();
                                fpsubjectdetails.Sheets[0].Cells[fpsubjectdetails.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                fpsubjectdetails.Sheets[0].Cells[fpsubjectdetails.Sheets[0].RowCount - 1, 1].Text = dssubject.Tables[0].Rows[sub]["subject_name"].ToString();
                                fpsubjectdetails.Sheets[0].Cells[fpsubjectdetails.Sheets[0].RowCount - 1, 1].Tag = subject_no;

                                string staffname = "";
                                //  DataSet dsstaffname = da.select_method_wo_parameter("select sm.staff_name from staff_selector st,staffmaster sm where st.staff_code=sm.staff_code and st.subject_no=" + subject_no + "", "Text"); 
                                DataSet dsstaffname = da.select_method_wo_parameter("select sm.staff_name from staff_selector st,staffmaster sm where st.staff_code=sm.staff_code and st.subject_no=" + subject_no + " and Sections ='" + sectval + "'", "Text");  // added by jairam 14-11-2014
                                for (int stf = 0; stf < dsstaffname.Tables[0].Rows.Count; stf++)
                                {
                                    if (staffname == "")
                                    {
                                        staffname = dsstaffname.Tables[0].Rows[stf]["staff_name"].ToString();
                                    }
                                    else
                                    {
                                        staffname = staffname + ',' + dsstaffname.Tables[0].Rows[stf]["staff_name"].ToString();
                                    }
                                }

                                fpsubjectdetails.Sheets[0].Cells[fpsubjectdetails.Sheets[0].RowCount - 1, 2].Text = staffname;
                                if (!dsubdic.ContainsKey(subject_no))
                                {
                                    dsubdic.Add(subject_no, 0);
                                }

                            }
                            loaddetails();
                            for (int str = startrow; str < fpsubjectdetails.Sheets[0].RowCount; str++)
                            {
                                string subno = fpsubjectdetails.Sheets[0].Cells[str, 1].Tag.ToString();
                                if (dsubdic.ContainsKey(subno))
                                {
                                    totconducthour = dsubdic[subno];
                                    fpsubjectdetails.Sheets[0].Cells[str, 3].Text = totconducthour.ToString();
                                    string syllabuhrs = da.GetFunction("select SUM(noofhrs) from sub_unit_details where subject_no=" + subno + "");
                                    if (syllabuhrs.Trim() == "" || syllabuhrs == null)
                                    {
                                        syllabuhrs = "0";
                                    }
                                    fpsubjectdetails.Sheets[0].Cells[str, 4].Text = syllabuhrs;

                                }
                            }
                        }
                        fpsubjectdetails.Sheets[0].PageSize = fpsubjectdetails.Sheets[0].RowCount;
                    }
                    else
                    {
                        fpsubjectdetails.Visible = false;
                        errmsg.Text = "No Records Found";
                        errmsg.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    public void loaddetails()
    {
        try
        {
            DataSet ds_holi = new DataSet();
            string halforfull = "", mng = "", evng = "", holiday_sched_details = "", order = "", strDay = "", month_year = "", temp_hr_field = "";
            string value_holi_status = "", date_temp_field = "", dummy_date = "", full_hour = "", single_hour = "";
            string[] split_holiday_status = new string[1000];
            int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
            int no_of_hrs = 0, mng_hrs = 0;
            DataSet ds_alter = new DataSet();
            DataSet ds_period = new DataSet();
            Boolean check_alter = false;
            string strsec = "";
            if (sectval.Trim() != "")
            {
                strsec = "and sections='" + sectval + "'";

            }

            string strstartdate = "select CONVERT(nvarchar(15),start_date,101) as startdate,CONVERT(nvarchar(15),end_date,101) as enddate,No_of_hrs_per_day,no_of_hrs_I_half_day from seminfo sm,PeriodAttndSchedule p where p.degree_code=sm.degree_code and p.semester=sm.semester and sm.batch_year=" + batchval + " and p.degree_code=" + degreeval + " and p.semester=" + semval + "";
            DataSet dsgetdetails = da.select_method_wo_parameter(strstartdate, "Text");

            DateTime dtstart = Convert.ToDateTime(dsgetdetails.Tables[0].Rows[0]["startdate"].ToString());
            DateTime dtenddate = Convert.ToDateTime(dsgetdetails.Tables[0].Rows[0]["enddate"].ToString());

            string nhrs = dsgetdetails.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
            if (nhrs.Trim() != "" || nhrs != null)
            {
                nhrs = "0";
            }
            no_of_hrs = int.Parse(dsgetdetails.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString());
            mng_hrs = int.Parse(dsgetdetails.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
            if (no_of_hrs > 0)
            {

                if (splhr_flag == true)
                {
                    string strsplhrquery = "select subject_no from specialhr_details sd,specialhr_master sm where degree_code=" + degreeval + " and batch_year=" + batchval + " and semester=" + semval + " " + strsec + " and date between '" + dtstart + "' and '" + dtenddate + "'";
                    DataSet dssplhr = da.select_method_wo_parameter(strsplhrquery, "Text");
                    for (int sp = 0; sp < dssplhr.Tables[0].Rows.Count; sp++)
                    {
                        string subno = dssplhr.Tables[0].Rows[sp]["subject_no"].ToString();
                        if (dsubdic.ContainsKey(subno))
                        {
                            totconducthour = dsubdic[subno];
                            totconducthour++;
                            dsubdic[subno] = totconducthour;
                        }
                    }
                }

                hat.Clear();
                hat.Add("from_date", dtstart);
                hat.Add("to_date", dtenddate);
                hat.Add("degree_code", degreeval);
                hat.Add("sem", semval);
                hat.Add("coll_code", ddlcollege.SelectedValue.ToString());
                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dtstart.ToString() + "' and '" + dtenddate.ToString() + "' and degree_code=" + degreeval.ToString() + " and semester=" + semval.ToString() + "";
                DataSet dsholiday = new DataSet();
                dsholiday = da.select_method(sqlstr_holiday, hat, "Text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);
                ds_holi = da.select_method("HOLIDATE_DETAILS_FINE", hat, "sp");
                if (ds_holi.Tables[0].Rows.Count > 0)
                {
                    for (int holi = 0; holi < ds_holi.Tables[0].Rows.Count; holi++)
                    {

                        if (ds_holi.Tables[0].Rows[holi]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds_holi.Tables[0].Rows[holi]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds_holi.Tables[0].Rows[holi]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                        if (!hat_holy.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString())))
                        {
                            hat_holy.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString()), holiday_sched_details);
                        }
                    }
                }
                for (DateTime dt = dtstart; dt <= dtenddate; dt = dt.AddDays(1))
                {
                    if (!hat_holy.ContainsKey(dt))
                    {

                        if (!hat_holy.ContainsKey(dt))
                        {
                            hat_holy.Add(dt, "3*0*0");
                        }
                    }
                    value_holi_status = hat_holy[dt];
                    split_holiday_status = value_holi_status.Split('*');
                    if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                    {
                        split_holiday_status_1 = 1;
                        split_holiday_status_2 = no_of_hrs;
                    }
                    else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                    {
                        if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                        {
                            split_holiday_status_1 = mng_hrs + 1;
                            split_holiday_status_2 = no_of_hrs;
                        }
                        if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                        {
                            split_holiday_status_1 = 1;
                            split_holiday_status_2 = mng_hrs;
                        }
                    }
                    else if (split_holiday_status[0].ToString() == "0")//=================fulday holiday
                    {
                        split_holiday_status_1 = 0;
                        split_holiday_status_2 = 0;
                    }
                    if (split_holiday_status_1 == 0 && split_holiday_status_2 == 0)
                    {
                        dt = dt.AddDays(1);
                    }
                    else
                    {
                        ds_alter.Clear();
                        ds_alter.Dispose();
                        ds_alter.Reset();
                        string alterquery = "select  * from alternate_schedule where degree_code = " + degreeval + " and semester = " + semval + " and batch_year = " + batchval + " and FromDate ='" + dt.ToString() + "' " + strsec + " order by FromDate Desc";
                        ds_alter = da.select_method(alterquery, hat, "Text");

                        ds_period.Clear();
                        ds_period.Dispose();
                        ds_period.Reset();
                        string query = "select top 1 * from semester_schedule where degree_code = " + degreeval + " and semester = " + semval + " and batch_year = " + batchval + " and FromDate <='" + dt.ToString() + "' " + strsec + " order by FromDate Desc";
                        ds_period = da.select_method(query, hat, "Text");


                        if (ds_period.Tables[0].Rows.Count > 0)
                        {

                            if (no_of_hrs > 0)
                            {
                                dummy_date = dt.ToString();
                                string[] dummy_date_split = dummy_date.Split(' ');
                                string[] final_date_string = dummy_date_split[0].Split('/');
                                dummy_date = final_date_string[1].ToString() + "/" + final_date_string[0].ToString() + "/" + final_date_string[2].ToString();
                                month_year = ((Convert.ToInt16(final_date_string[2].ToString()) * 12) + (Convert.ToInt16(final_date_string[0].ToString()))).ToString();
                                if (order != "0")
                                {
                                    strDay = dt.ToString("ddd");
                                }
                                else
                                {
                                    strDay = findday(no_of_hrs, dtstart.ToString(), dt.ToString());
                                }
                                for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                {
                                    temp_hr_field = strDay + temp_hr;
                                    date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                    if (ds_alter.Tables[0].Rows.Count > 0)
                                    {
                                        for (int hasrow = 0; hasrow < ds_alter.Tables[0].Rows.Count; hasrow++)
                                        {
                                            full_hour = ds_alter.Tables[0].Rows[hasrow][temp_hr_field].ToString();
                                            if (full_hour.Trim() != "")
                                            {
                                                string[] split_full_hour = full_hour.Split(';');
                                                for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                {
                                                    single_hour = split_full_hour[semi_colon].ToString();
                                                    string[] split_single_hour = single_hour.Split('-');
                                                    if (dsubdic.ContainsKey(split_single_hour[0].ToString()))
                                                    {
                                                        check_alter = true;
                                                        totconducthour = dsubdic[split_single_hour[0].ToString()];
                                                        totconducthour++;
                                                        dsubdic[split_single_hour[0].ToString()] = totconducthour;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (check_alter == false)
                                    {
                                        full_hour = ds_period.Tables[0].Rows[0][temp_hr_field].ToString();
                                        if (full_hour.Trim() != "")
                                        {
                                            string[] split_full_hour_sem = full_hour.Split(';');
                                            for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                            {
                                                single_hour = split_full_hour_sem[semi_colon].ToString();
                                                string[] split_single_hour = single_hour.Split('-');
                                                if (dsubdic.ContainsKey(split_single_hour[0].ToString()))
                                                {
                                                    totconducthour = dsubdic[split_single_hour[0].ToString()];
                                                    totconducthour++;
                                                    dsubdic[split_single_hour[0].ToString()] = totconducthour;
                                                }
                                            }
                                        }
                                    }
                                    check_alter = false;
                                }
                            }

                        }

                        dt = dt.AddDays(1);
                    }

                }
            }
        }
        catch (Exception ex)
        {
        }
    }


    private string findday(int no, string sdate, string todate)//------------------find day order 
    {
        int order, holino;
        holino = 0;
        string day_order = "";
        string from_date = "";
        string fdate = "";
        int diff_work_day = 0;

        string[] spiltdate = todate.Split('/');
        todate = spiltdate[1] + '/' + spiltdate[0] + '/' + spiltdate[2];


        holino = int.Parse(da.GetFunction("select count(*) from holidaystudents where degree_code=" + degreeval.ToString() + " and semester=" + semval.ToString() + " and holiday_date between '" + sdate.ToString() + "' and  '" + todate.ToString() + "' and halforfull='0' and isnull(Not_include_dayorder,0)<>'1'"));//01.03.17 barath";"

        string quer = "select nodays from PeriodAttndSchedule where degree_code=" + degreeval.ToString() + " and semester=" + semval.ToString();
        string nodays = da.GetFunction(quer);
        if (nodays.Trim() == "" && nodays == null)
        {
            nodays = "0";
        }
        int no_days = Convert.ToInt32(nodays);
        DateTime dt1 = Convert.ToDateTime(todate.ToString());
        DateTime dt2 = Convert.ToDateTime(sdate.ToString());
        TimeSpan t = dt1.Subtract(dt2);
        int days = t.Days;

        diff_work_day = days - holino;
        order = Convert.ToInt16(diff_work_day.ToString()) % no_days;
        order = order + 1;//Added by srinath 12/9/2013
        if (order.ToString() == "0")
        {
            order = no;
        }
        if (order.ToString() == "1")
        {
            day_order = "mon";
        }
        else if (order.ToString() == "2")
        {
            day_order = "tue";
        }
        else if (order.ToString() == "3")
        {
            day_order = "wed";
        }
        else if (order.ToString() == "4")
        {
            day_order = "thu";
        }
        else if (order.ToString() == "5")
        {
            day_order = "fri";
        }
        else if (order.ToString() == "6")
        {
            day_order = "sat";
        }
        else if (order.ToString() == "7")
        {
            day_order = "sun";
        }
        return (day_order);

    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(fpsubjectdetails, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "Staff Subject Details Report";
        string pagename = "StaffSubjectDetailsReport.aspx";
        Printcontrol.loadspreaddetails(fpsubjectdetails, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

}