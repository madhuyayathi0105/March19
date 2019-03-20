using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class Room_Allocation : System.Web.UI.Page
{
    string user_code;
    Boolean Cellclick = false;
    static ArrayList ItemList = new ArrayList();
    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds3 = new DataSet();
    DAccess2 queryObject = new DAccess2();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string selectQuery = string.Empty;

    string college = string.Empty;
    string batch = string.Empty;
    string degree1 = string.Empty;
    string dept = string.Empty;
    string subtype = string.Empty;
    string subname = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string college_code = "";

    string course_id = string.Empty;
    static string Hostelcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //string PageLogOut = "";
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
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        //// hat.Add("single_user", singleuser);
        // hat.Add("group_code", group_user);
        // hat.Add("college_code", collegecode);
        // hat.Add("user_code", usercode);
        if (!IsPostBack)
        {

            bindcollege();
            bindbatch();
            degree();
            bindbranch();
            bindsem();
            //bindbatch1();
            bindroom();
            binddeg();
            binddept();
            bindsub();
            bindsub1();
            FpSpread1.Visible = false;
            div_report.Visible = false;
            txt_pop1floor.Enabled = false;
            txt_pop1bul.Enabled = false;
            divdel.Visible = false;
            btn_go_Click(sender, e);
        }


    }
    public void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {


            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {

                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";

                    }
                }
                bindbranch();
                bindsem();

            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_dept.Text = "--Select--";
                    txt_sem.Text = "--Select--";
                    cbl_dept.ClearSelection();
                    cb_dept.Checked = false;
                    cbl_sem.ClearSelection();
                    cb_sem.Checked = false;
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            cb_degree.Checked = false;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            bindbranch();
            bindsem();
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_dept.Text = "--Select--";
                txt_sem.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }

        }
        catch (Exception ex)
        {
        }

    }
    public void degree()
    {
        try
        {
            user_code = Session["usercode"].ToString();
            college_code = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("college_code", college_code);
            hat.Add("user_code", user_code);
            ds1 = d2.select_method("bind_degree", hat, "sp");
            int count1 = ds1.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds1;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_degree.Items.Count; row++)
                    {
                        cbl_degree.Items[row].Selected = true;
                    }
                    txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch()
    {
        try
        {
            string branch = "";
            cbl_dept.Items.Clear();
            txt_dept.Text = "--Select--";
            txt_sem.Text = "--Select--";
            string commname = "";
            string build1 = "";
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {

                    if (cbl_degree.Items[i].Selected == true)
                    {
                        build1 = cbl_degree.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build1;
                        }
                        else
                        {
                            branch = branch + "," + build1;

                        }

                    }
                }

            }
            if (branch != "")
            {

                ds1 = d2.BindBranchMultiple(singleuser, group_user, branch, collegecode1, usercode);

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds1;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_dept.Items.Count; row++)
                        {
                            cbl_dept.Items[row].Selected = true;
                        }
                        txt_dept.Text = "Depatement(" + cbl_dept.Items.Count + ")";
                    }
                }
            }

        }

        catch (Exception ex)
        {
        }

    }

    public void cb_dept_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_dept.Checked == true)
            {
                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
                txt_dept.Text = "--Select--";
            }
            bindsem();

        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_dept.Text = "--Select--";
            cb_dept.Checked = false;

            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_dept.Items.Count)
            {
                txt_dept.Text = "Department(" + commcount.ToString() + ")";
                cb_dept.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_sem.Text = "--Select--";
            }
            else
            {
                txt_dept.Text = "Department(" + commcount.ToString() + ")";
            }
            bindsem();

        }

        catch (Exception ex)
        {
        }
    }
    public void cb_sem_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_sem.Checked == true)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
                txt_sem.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_sem.Text = "--Select--";
            cb_sem.Checked = false;

            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_sem.Items.Count)
            {
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
                cb_sem.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_dept.Text = "--Select--";
            }
            else
            {
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
            }

        }

        catch (Exception ex)
        {
        }
    }


    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            bindsub();
            string q = "";
            int i;
            string degcode = "";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    if (degcode == "")
                    {
                        degcode = "" + cbl_dept.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degcode = degcode + "'" + "," + "'" + cbl_dept.Items[i].Value.ToString() + "";
                    }
                }
            }

            string semcode = "";
            for (i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semcode == "")
                    {
                        semcode = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semcode = semcode + "'" + "," + "'" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }


            string batchcode = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batchcode == "")
                    {
                        batchcode = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        batchcode = batchcode + "'" + "," + "'" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (degcode != "" && batchcode != "" && batchcode != "")
            {
                q = "select distinct d.Degree_Code,(CONVERT(varchar, sr.Batch_Year)+' - '+c.Course_Name+' - '+dt.Dept_Name) as degree,s.subject_no,s.subject_name,Room_Name from SubwiseRoomAllot sr,Degree d,Department dt,Course c,subject s where s.subject_no =sr.subject_no and d.Degree_Code =sr.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.Degree_Code in ('" + degcode + "') and Batch_Year in ('" + batchcode + "') and Semester in ('" + semcode + "') and d.college_code ='" + collegecode1 + "'";
                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 4;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    darkstyle.HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;

                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    //FpSpread1.Sheets[0].ColumnCount = 4;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[0].Width = 100;
                    FpSpread1.Columns[1].Width = 300;


                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[2].Width = 350;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[3].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Room No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Columns[0].Locked = true;
                    FpSpread1.Columns[1].Locked = true;
                    FpSpread1.Columns[2].Locked = true;
                    FpSpread1.Columns[3].Locked = true;

                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        FpSpread1.Sheets[0].RowCount++;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(j + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[j]["degree"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[j]["Degree_Code"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[j]["subject_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[j]["subject_no"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[j]["Room_Name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[j]["Room_Name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;


                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    div_report.Visible = true;
                    lblerr.Visible = false;

                }
                else
                {
                    if (q != "")
                    {
                        ds = d2.select_method(q, hat, "Text");
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            FpSpread1.Sheets[0].Visible = false;
                            lblerr.Visible = true;
                            lblerr.Text = "No Records Found";
                            div_report.Visible = false;
                        }
                    }
                }
            }
            else
            {
                lblerr.Visible = true;
                FpSpread1.Visible = false;
                lblerr.Text = "Kindly Select All List";
                div_report.Visible = false;
            }
        }

        catch
        {
        }


    }
    public void btn_add_Click(object sender, EventArgs e)
    {


        bindbatch();
        binddeg();
        binddept();
        bindsem1();
        bindsub();
        bindroom();
        bindroomdetails();
        popwindow1.Visible = true;
        btn_pop1save.Visible = true;
        btn_pop1update.Visible = false;
        btn_pop1delete.Visible = false;
        btn_pop1exit.Visible = true;
        btn_pop1exit1.Visible = false;
        ddl_pop1college.Enabled = true;
        ddl_pop1batch.Enabled = true;
        txt_pop1dept.Enabled = true;
        txt_pop1degree.Enabled = true;
        txt_pop1sem.Enabled = true;
        btnclose.Visible = false;
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void imagebtnpop1close_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    public void ddl_pop1college_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void bindcollege()
    {
        try
        {
            ds1.Clear();
            ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds1 = d2.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds1;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();

                ddl_pop1college.DataSource = ds1;
                ddl_pop1college.DataTextField = "collname";
                ddl_pop1college.DataValueField = "college_code";
                ddl_pop1college.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindsub()
    {
        try
        {
            int i;

            string branch = "";
            string build = "";
            string build1 = "";
            string sem = "";
            string batch = "";
            ddl_pop1sub.Items.Clear();
            if (cbl_pop1dept.Items.Count > 0)
            {
                for (i = 0; i < cbl_pop1dept.Items.Count; i++)
                {

                    if (cbl_pop1dept.Items[i].Selected == true)
                    {
                        build = cbl_pop1dept.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "," + build;

                        }
                    }
                }
            }
            batch = Convert.ToString(ddl_pop1batch.SelectedItem.Value);
            if (cbl_pop1sem.Items.Count > 0)
            {
                for (i = 0; i < cbl_pop1sem.Items.Count; i++)
                {

                    if (cbl_pop1sem.Items[i].Selected == true)
                    {
                        build1 = cbl_pop1sem.Items[i].Value.ToString();
                        if (sem == "")
                        {
                            sem = build1;
                        }
                        else
                        {
                            sem = sem + "," + build1;

                        }
                    }
                }
            }

            if (branch != "" && batch.Trim() != "" && sem.Trim() != "")
            {


                ds1.Clear();
                string query = "select s.subject_name,subject_no from syllabus_master sy,subject s,sub_sem sm where sy.syll_code =sm.syll_code and sm.subType_no =s.subType_no and sy.degree_code in (" + branch + ") and sy.Batch_Year ='" + batch + "' and semester in (" + sem + ") and subject_no not in (select subject_no from SubwiseRoomAllot)";
                ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ddl_pop1sub.DataSource = ds1;
                    ddl_pop1sub.DataTextField = "subject_name";
                    ddl_pop1sub.DataValueField = "subject_no";
                    ddl_pop1sub.DataBind();
                }

            }
        }
        catch
        {

        }
    }
    public void bindroom()
    {
        try
        {
            ds1.Clear();
            string query = "select distinct Room_Name from Room_Detail where Room_Name not in (select Room_Name from SubwiseRoomAllot)";
            ds1 = d2.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_pop1roomno.DataSource = ds1;
                ddl_pop1roomno.DataTextField = "Room_Name";
                ddl_pop1roomno.DataValueField = "Room_Name";
                ddl_pop1roomno.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindroom1()
    {
        try
        {
            ds1.Clear();
            string query = "select distinct Room_Name from Room_Detail";
            ds1 = d2.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_pop1roomno.DataSource = ds1;
                ddl_pop1roomno.DataTextField = "Room_Name";
                ddl_pop1roomno.DataValueField = "Room_Name";
                ddl_pop1roomno.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindsub1()
    {
        int i;

        string branch = "";
        string build = "";
        string build1 = "";
        string sem = "";
        string batch = "";
        ddl_pop1sub.Items.Clear();
        if (cbl_pop1dept.Items.Count > 0)
        {
            for (i = 0; i < cbl_pop1dept.Items.Count; i++)
            {

                if (cbl_pop1dept.Items[i].Selected == true)
                {
                    build = cbl_pop1dept.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = build;
                    }
                    else
                    {
                        branch = branch + "," + build;

                    }
                }
            }
        }
        batch = Convert.ToString(ddl_pop1batch.SelectedItem.Value);
        if (cbl_pop1sem.Items.Count > 0)
        {
            for (i = 0; i < cbl_pop1sem.Items.Count; i++)
            {

                if (cbl_pop1sem.Items[i].Selected == true)
                {
                    build1 = cbl_pop1sem.Items[i].Value.ToString();
                    if (sem == "")
                    {
                        sem = build1;
                    }
                    else
                    {
                        sem = sem + "," + build1;

                    }
                }
            }
        }

        if (branch != "" && batch.Trim() != "" && sem.Trim() != "")
        {


            ds1.Clear();
            string query = "select s.subject_name,subject_no from syllabus_master sy,subject s,sub_sem sm where sy.syll_code =sm.syll_code and sm.subType_no =s.subType_no and sy.degree_code in (" + branch + ") and sy.Batch_Year ='" + batch + "' and semester in (" + sem + ")";
            //string query = "select s.subject_name,subject_no from syllabus_master sy,subject s,sub_sem sm where sy.syll_code =sm.syll_code and sm.subType_no =s.subType_no";
            ds1 = d2.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_pop1sub.DataSource = ds1;
                ddl_pop1sub.DataTextField = "subject_name";
                ddl_pop1sub.DataValueField = "subject_no";
                ddl_pop1sub.DataBind();
            }
        }
    }

    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        ds1.Clear();
        string branch = "";
        string build = "";
        string batch = "";
        if (cbl_dept.Items.Count > 0)
        {
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {

                if (cbl_dept.Items[i].Selected == true)
                {
                    build = cbl_dept.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = build;
                    }
                    else
                    {
                        branch = branch + "," + build;

                    }
                }
            }
        }
        build = "";
        if (cbl_batch.Items.Count > 0)
        {
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {

                if (cbl_batch.Items[i].Selected == true)
                {
                    build = cbl_batch.Items[i].Value.ToString();
                    if (batch == "")
                    {
                        batch = build;
                    }
                    else
                    {
                        batch = batch + "," + build;

                    }

                }
            }

        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            ds1 = d2.BindSem(branch, batch, collegecode1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string dur = Convert.ToString(ds1.Tables[0].Rows[i][0]);
                    if (dur.Trim() != "")
                    {
                        if (duration < Convert.ToInt32(dur))
                        {
                            duration = Convert.ToInt32(dur);
                        }
                    }
                }
            }
            if (duration != 0)
            {
                for (i = 1; i <= duration; i++)
                {
                    cbl_sem.Items.Add(Convert.ToString(i));
                }
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sem.Items.Count; row++)
                    {
                        cbl_sem.Items[row].Selected = true;
                    }
                    txt_sem.Text = "Sem(" + cbl_sem.Items.Count + ")";
                }
            }
        }

    }

    //public void bindsub1()
    //{
    //    string query = "select s.subject_name,subject_no from syllabus_master sy,subject s,sub_sem sm where sy.syll_code =sm.syll_code and sm.subType_no =s.subType_no ";
    //    ds1 = d2.select_method_wo_parameter(query, "Text");
    //    if (ds1.Tables[0].Rows.Count > 0)
    //    {
    //        ddl_pop1sub.DataSource = ds1;
    //        ddl_pop1sub.DataTextField = "subject_name";
    //        ddl_pop1sub.DataValueField = "subject_no";
    //        ddl_pop1sub.DataBind();
    //    }

    //}



    public void bindbatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            hat.Clear();
            // string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            ds1 = d2.BindBatch();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_pop1batch.DataSource = ds1;
                ddl_pop1batch.DataTextField = "Batch_Year";
                ddl_pop1batch.DataValueField = "Batch_Year";
                ddl_pop1batch.DataBind();

                cbl_batch.DataSource = ds1;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();


                if (cbl_batch.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_batch.Items.Count; row++)
                    {
                        cbl_batch.Items[row].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                }
            }



        }
        catch
        {
        }
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_batch.Text = "--Select--";
            cb_batch.Checked = false;

            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                }
            }
            if (commcount > 0)
            {
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void cb_batch_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_batch.Checked == true)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";


            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }




    public void ddl_pop1batch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void cb_pop1degree_checkedchange(object sender, EventArgs e)
    {
        try
        {


            if (cb_pop1degree.Checked == true)
            {
                for (int i = 0; i < cbl_pop1degree.Items.Count; i++)
                {

                    if (cb_pop1degree.Checked == true)
                    {
                        cbl_pop1degree.Items[i].Selected = true;
                        txt_pop1degree.Text = "Degree(" + (cbl_pop1degree.Items.Count) + ")";

                    }
                }
                binddept();
                bindsem1();
                bindsub();
            }
            else
            {
                for (int i = 0; i < cbl_pop1degree.Items.Count; i++)
                {
                    cbl_pop1degree.Items[i].Selected = false;
                    txt_pop1degree.Text = "--Select--";
                    txt_pop1dept.Text = "--Select--";
                    txt_pop1sem.Text = "--Select--";
                    cbl_pop1dept.ClearSelection();
                    cb_pop1dept.Checked = false;
                    cbl_pop1sem.ClearSelection();
                    cb_pop1sem.Checked = false;
                }
            }


        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_pop1degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            cb_pop1degree.Checked = false;
            for (int i = 0; i < cbl_pop1degree.Items.Count; i++)
            {
                if (cbl_pop1degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }

            if (seatcount == cbl_pop1degree.Items.Count)
            {
                txt_pop1degree.Text = "Degree(" + seatcount.ToString() + ")";
                cb_pop1degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_pop1degree.Text = "--Select--";
                txt_pop1dept.Text = "--Select--";
                txt_pop1sem.Text = "--Select--";
            }
            else
            {
                txt_pop1degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
            binddept();
            bindsem1();
            bindsub();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_pop1dept_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_pop1dept.Checked == true)
            {
                for (int i = 0; i < cbl_pop1dept.Items.Count; i++)
                {
                    if (cb_pop1dept.Checked == true)
                    {
                        cbl_pop1dept.Items[i].Selected = true;
                        txt_pop1dept.Text = "Department(" + (cbl_pop1dept.Items.Count) + ")";
                    }
                }
                bindsem1();
                bindsub();

            }


            else
            {
                for (int i = 0; i < cbl_pop1dept.Items.Count; i++)
                {
                    cbl_pop1dept.Items[i].Selected = false;
                }
                txt_pop1dept.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }

    }
    public void cbl_pop1dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_pop1dept.Text = "--Select--";
            cb_pop1dept.Checked = false;

            for (int i = 0; i < cbl_pop1dept.Items.Count; i++)
            {
                if (cbl_pop1dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_dept.Items.Count)
            {
                txt_pop1dept.Text = "Department(" + commcount.ToString() + ")";
                cb_pop1dept.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_pop1degree.Text = "--Select--";
                txt_pop1sem.Text = "--Select--";
            }
            else
            {
                txt_pop1dept.Text = "Department(" + commcount.ToString() + ")";
            }
            bindsem1();
            bindsub();
        }

        catch (Exception ex)
        {
        }
    }
    public void cb_pop1sem_checkedchange(object sender, EventArgs e)
    {

        try
        {

            if (cb_pop1sem.Checked == true)
            {
                for (int i = 0; i < cbl_pop1sem.Items.Count; i++)
                {
                    if (cb_pop1sem.Checked == true)
                    {
                        cbl_pop1sem.Items[i].Selected = true;
                        txt_pop1sem.Text = "Semester(" + (cbl_pop1sem.Items.Count) + ")";
                    }
                }
                bindsub();
            }
            else
            {
                for (int i = 0; i < cbl_pop1sem.Items.Count; i++)
                {
                    cbl_pop1sem.Items[i].Selected = false;
                }
                txt_pop1sem.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_pop1sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_pop1sem.Text = "--Select--";
            cb_pop1sem.Checked = false;

            for (int i = 0; i < cbl_pop1sem.Items.Count; i++)
            {
                if (cbl_pop1sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_pop1sem.Items.Count)
            {
                txt_pop1sem.Text = "Semester(" + commcount.ToString() + ")";
                cb_pop1sem.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_pop1degree.Text = "--Select--";
                txt_pop1dept.Text = "--Select--";
            }
            else
            {
                txt_pop1sem.Text = "Semester(" + commcount.ToString() + ")";
            }

            bindsub();
        }

        catch (Exception ex)
        {
        }
    }
    public void ddl_pop1sub_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch
        {

        }

    }

    public void binddept()
    {
        try
        {
            string branch = "";
            cbl_pop1dept.Items.Clear();
            txt_pop1dept.Text = "--Select--";
            txt_pop1sem.Text = "--Select--";
            string commname = "";
            string build1 = "";
            if (cbl_pop1degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_pop1degree.Items.Count; i++)
                {

                    if (cbl_pop1degree.Items[i].Selected == true)
                    {
                        build1 = cbl_pop1degree.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build1;
                        }
                        else
                        {
                            branch = branch + "," + build1;

                        }

                    }
                }

            }
            if (branch != "")
            {

                ds1 = d2.BindBranchMultiple(singleuser, group_user, branch, collegecode1, usercode);

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    cbl_pop1dept.DataSource = ds1;
                    cbl_pop1dept.DataTextField = "dept_name";
                    cbl_pop1dept.DataValueField = "degree_code";
                    cbl_pop1dept.DataBind();
                    if (cbl_pop1dept.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_pop1dept.Items.Count; row++)
                        {
                            cbl_pop1dept.Items[row].Selected = true;
                        }
                        txt_pop1dept.Text = "Depatement(" + cbl_pop1dept.Items.Count + ")";
                    }
                }
            }

        }

        catch (Exception ex)
        {
        }


    }
    public void binddeg()
    {
        try
        {
            //string query1 = "select distinct d.Course_Id,c.Course_Name  from syllabus_master sy,subject s,Degree d,Department dt,Course c where d.Degree_Code =sy.degree_code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and sy.syll_code =s.syll_code and subject_code ='" + ddl_pop1sub.SelectedValue + "'";
            //ds2 = d2.select_method_wo_parameter(query1, "Text");
            user_code = Session["usercode"].ToString();
            college_code = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("college_code", college_code);
            hat.Add("user_code", user_code);
            ds1 = d2.select_method("bind_degree", hat, "sp");
            int count1 = ds1.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_pop1degree.DataSource = ds1;
                cbl_pop1degree.DataTextField = "Course_Name";
                cbl_pop1degree.DataValueField = "Course_Id";
                cbl_pop1degree.DataBind();
                if (cbl_pop1degree.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_pop1degree.Items.Count; row++)
                    {
                        cbl_pop1degree.Items[row].Selected = true;
                    }
                    txt_pop1degree.Text = "Degree(" + cbl_pop1degree.Items.Count + ")";
                }
            }
        }
        catch
        {

        }
    }

    //public void bindbatch1()
    //{
    //    try
    //    {
    //        //string query2 = " select distinct Batch_Year  from syllabus_master sy,subject s,Degree d,Department dt,Course c where d.Degree_Code =sy.degree_code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and sy.syll_code =s.syll_code and subject_code ='" + ddl_pop1sub.SelectedValue + "'";

    //        //ds1 = d2.select_method_wo_parameter(query2, "Text");
    //        ds1 = d2.BindBatch();
    //        if (ds1.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_pop1batch.DataSource = ds1;
    //            ddl_pop1batch.DataTextField = "Batch_Year";
    //            ddl_pop1batch.DataValueField = "Batch_Year";
    //            ddl_pop1batch.DataBind();
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}
    public void bindsem1()
    {
        try
        {

            cbl_pop1sem.Items.Clear();
            txt_pop1sem.Text = "--Select--";
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds1.Clear();
            string branch = "";
            string build = "";
            string batch = "";
            if (cbl_pop1dept.Items.Count > 0)
            {
                for (i = 0; i < cbl_pop1dept.Items.Count; i++)
                {

                    if (cbl_pop1dept.Items[i].Selected == true)
                    {
                        build = cbl_pop1dept.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "," + build;

                        }
                    }
                }
            }
            batch = Convert.ToString(ddl_pop1batch.SelectedItem.Value);

            if (branch.Trim() != "" && batch.Trim() != "")
            {
                ds1 = d2.BindSem(branch, batch, collegecode1);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        string dur = Convert.ToString(ds1.Tables[0].Rows[i][0]);
                        if (dur.Trim() != "")
                        {
                            if (duration < Convert.ToInt32(dur))
                            {
                                duration = Convert.ToInt32(dur);
                            }
                        }
                    }
                }
                if (duration != 0)
                {
                    for (i = 1; i <= duration; i++)
                    {
                        cbl_pop1sem.Items.Add(Convert.ToString(i));
                    }
                    if (cbl_pop1sem.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_pop1sem.Items.Count; row++)
                        {
                            cbl_pop1sem.Items[row].Selected = true;
                        }
                        txt_pop1sem.Text = "Sem(" + cbl_pop1sem.Items.Count + ")";
                    }
                }
            }

        }
        catch
        {

        }
    }
    public void ddl_pop1roomno_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_pop1floor.Enabled = false;
        txt_pop1bul.Enabled = false;

        bindroomdetails();

    }

    public void bindroomdetails()
    {
        string query = "select Building_Name,Floor_Name  from Room_Detail where Room_Name ='" + ddl_pop1roomno.SelectedValue + "'";
        ds1 = d2.select_method_wo_parameter(query, "Text");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            string bul = ds1.Tables[0].Rows[0]["Building_Name"].ToString();
            txt_pop1bul.Text = bul;
            string flr = ds1.Tables[0].Rows[0]["Floor_Name"].ToString();
            txt_pop1floor.Text = flr;
        }
    }
    public void btn_pop1save_Click(object sender, EventArgs e)
    {
        try
        {
            divdel.Visible = false;
            int i;
            int j;
            bool saveflage = false;
            if (cbl_pop1dept.Items.Count > 0)
            {
                for (i = 0; i < cbl_pop1dept.Items.Count; i++)
                {
                    if (cbl_pop1dept.Items[i].Selected == true)
                    {
                        string degcode = Convert.ToString(cbl_pop1dept.Items[i].Value);

                        if (cbl_pop1sem.Items.Count > 0)
                        {
                            for (j = 0; j < cbl_pop1sem.Items.Count; j++)
                            {
                                if (cbl_pop1sem.Items[j].Selected == true)
                                {

                                    string semcode = Convert.ToString(cbl_pop1sem.Items[j].Value);
                                    string sub = Convert.ToString(ddl_pop1sub.SelectedItem.Value);
                                    //string qur = "select Subject_No from SubwiseRoomAllot";
                                    //ds = d2.select_method_wo_parameter(qur, "Text");
                                    //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    //{
                                    //    string s = Convert.ToString(ds.Tables[0].Rows[i][0]);

                                    //if (s != sub)
                                    //{
                                    //string query = "insert into SubwiseRoomAllot (Batch_Year,Degree_Code,Semester,Subject_No,Room_Name) values ('" + ddl_pop1batch.SelectedValue + "','" + degcode + "','" + semcode + "','" + ddl_pop1sub.SelectedValue + "','" + ddl_pop1roomno.SelectedValue + "')";
                                    string query = "if exists (select * from SubwiseRoomAllot where Degree_Code ='" + degcode + "' and Batch_Year ='" + ddl_pop1batch.SelectedValue + "' and Semester ='" + semcode + "' and Subject_No ='" + ddl_pop1sub.SelectedValue + "') update SubwiseRoomAllot set Room_Name ='" + ddl_pop1roomno.SelectedValue + "' where Degree_Code ='" + degcode + "' and Batch_Year ='" + ddl_pop1batch.SelectedValue + "' and Semester ='" + semcode + "' and Subject_No ='" + ddl_pop1sub.SelectedValue + "' and Floor_Name='" + txt_pop1floor.Text + "' and Building_Name='" + txt_pop1bul.Text + "'  else insert into SubwiseRoomAllot (Batch_Year,Degree_Code,Semester,Subject_No,Room_Name,Building_Name,Floor_Name) values ('" + ddl_pop1batch.SelectedValue + "','" + degcode + "','" + semcode + "','" + ddl_pop1sub.SelectedValue + "','" + ddl_pop1roomno.SelectedValue + "','" + txt_pop1bul.Text + "','" + txt_pop1floor.Text + "')";
                                    int value = d2.update_method_wo_parameter(query, "Text");
                                    if (value != 0)
                                    {
                                        saveflage = true;
                                    }
                                    //}
                                    else
                                    {
                                        saveflage = false;
                                    }
                                    //}
                                }
                            }
                        }

                    }
                }
            }
            if (saveflage == true)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Saved Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Subject Already Exist";
                popwindow1.Visible = true;
            }
        }
        catch
        {

        }
    }
    public void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        popwindow1.Visible = false;
        if (lblalerterr.Text == "Do You Want Delete This Record ?")
        {
            delete();
            imgdiv2.Visible = true;
            lblalerterr.Text = "Deleted Successfully";
            //imgdiv2.Visible = false;
            //popwindow1.Visible = false;
        }
    }
    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                //  FpSpread1.Sheets[0].Columns[1].Visible = false;
                d2.printexcelreport(FpSpread1, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }

        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }

    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Room Allocation Report";
            string pagename = "Room_Allocation.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop1exit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;

            popwindow1.Visible = true;
            btn_pop1save.Visible = false;
            btn_pop1exit.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (Cellclick == true)
        {
            try
            {
                btn_pop1save.Visible = false;
                btn_pop1exit.Visible = false;
                divdel.Visible = true;
                btn_pop1update.Visible = true;
                btn_pop1delete.Visible = true;
                btn_pop1exit1.Visible = true;


                string activerow = "";

                activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                string subject = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                string room = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string query = "select d.Degree_Code,dt.Dept_Name,c.Course_Id,c.Course_Name,s.Room_Name,r.Building_Name,r.Floor_Name,s.Semester,s.Batch_Year,sb.subject_no,sb.subject_name from SubwiseRoomAllot s,Degree d,Department dt,Course c,Room_Detail r,Subject sb where s.Degree_Code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and s.Subject_No =sb.subject_no and r.Room_Name =s.Room_Name and r.Building_Name =s.Building_Name and r.Floor_Name =s.Floor_Name and s.Degree_Code ='" + dept + "' and s.Subject_No ='" + subject + "' and s.Room_Name ='" + room + "'";
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string batch = Convert.ToString(ds.Tables[0].Rows[0]["Batch_Year"]);
                    string degree = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
                    // string dept = Convert.ToString(ds.Tables[0].Rows[0]["Degree_Code"]);
                    string sem = Convert.ToString(ds.Tables[0].Rows[0]["Semester"]);
                    string sub = Convert.ToString(ds.Tables[0].Rows[0]["subject_name"]);
                    string roomno = Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]);
                    string bul = Convert.ToString(ds.Tables[0].Rows[0]["Building_Name"]);
                    string flr = Convert.ToString(ds.Tables[0].Rows[0]["Floor_Name"]);
                    txt_pop1bul.Text = bul;
                    txt_pop1floor.Text = flr;

                    bindcollege();
                    bindbatch();
                    ddl_pop1batch.SelectedValue = batch;
                    binddeg();
                    ddl_pop1college.Enabled = false;
                    ddl_pop1batch.Enabled = false;
                    int count = 0;
                    for (int i = 0; i < cbl_pop1degree.Items.Count; i++)
                    {
                        if (cbl_pop1degree.Items[i].Text != degree)
                        {
                            cbl_pop1degree.Items[i].Selected = false;
                        }
                        else
                        {
                            cbl_pop1degree.Items[i].Selected = true;
                            count++;
                        }
                    }
                    txt_pop1degree.Text = "Degree(" + count + ")";
                    txt_pop1degree.Enabled = false;
                    count = 0;
                    binddept();
                    for (int i = 0; i < cbl_pop1dept.Items.Count; i++)
                    {
                        if (cbl_pop1dept.Items[i].Value != dept)
                        {
                            cbl_pop1dept.Items[i].Selected = false;
                        }
                        else
                        {
                            cbl_pop1dept.Items[i].Selected = true;
                            count++;
                        }
                    }
                    txt_pop1dept.Text = "Department(" + count + ")";
                    txt_pop1dept.Enabled = false;
                    count = 0;
                    bindsem1();
                    for (int i = 0; i < cbl_pop1sem.Items.Count; i++)
                    {
                        if (cbl_pop1sem.Items[i].Value != sem)
                        {
                            cbl_pop1sem.Items[i].Selected = false;
                        }
                        else
                        {
                            cbl_pop1sem.Items[i].Selected = true;
                            count++;
                        }
                    }
                    txt_pop1sem.Text = "Sem(" + count + ")";
                    txt_pop1sem.Enabled = false;
                    bindsub1();
                    ddl_pop1sub.SelectedValue = subject;
                    bindroom1();

                    ddl_pop1roomno.SelectedValue = roomno;


                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    public void btn_pop1delete_Click(object sender, EventArgs e)
    {
        try
        {

            // string del1 = "delete SubwiseRoomAllot where Subject_No ='" + ddl_pop1sub.SelectedItem.Value + "' and Room_Name='" + ddl_pop1roomno.SelectedItem.Text + "' and Building_Name='" + txt_pop1bul.Text + "' and Floor_Name='" + txt_pop1floor.Text + "'";
            //int j = d2.update_method_wo_parameter(del1, "Text");
            imgdiv2.Visible = true;
            lblalerterr.Text = "Do You Want Delete This Record ?";
            btnclose.Visible = true;
        }
        catch
        {

        }

    }
    public void delete()
    {
        string del1 = "delete SubwiseRoomAllot where Subject_No ='" + ddl_pop1sub.SelectedItem.Value + "' and Room_Name='" + ddl_pop1roomno.SelectedItem.Text + "' and Building_Name='" + txt_pop1bul.Text + "' and Floor_Name='" + txt_pop1floor.Text + "'";
        int j = d2.update_method_wo_parameter(del1, "Text");
    }

    public void btnclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void btn_pop1update_Click(object sender, EventArgs e)
    {
        //string query = "update SubwiseRoomAllot set Subject_No ='" + ddl_pop1sub.SelectedItem.Value + "',Room_Name='" + ddl_pop1roomno.SelectedItem.Text + "' , Building_Name='" + txt_pop1bul.Text + "' , Floor_Name='" + txt_pop1floor.Text + "' where Batch_Year ='" + ddl_pop1batch.SelectedValue + "',";

        try
        {
            divdel.Visible = false;
            int i;
            int j;
            bool saveflage = false;
            if (cbl_pop1dept.Items.Count > 0)
            {
                for (i = 0; i < cbl_pop1dept.Items.Count; i++)
                {
                    if (cbl_pop1dept.Items[i].Selected == true)
                    {
                        string degcode = Convert.ToString(cbl_pop1dept.Items[i].Value);

                        if (cbl_pop1sem.Items.Count > 0)
                        {
                            for (j = 0; j < cbl_pop1sem.Items.Count; j++)
                            {
                                if (cbl_pop1sem.Items[j].Selected == true)
                                {

                                    string semcode = Convert.ToString(cbl_pop1sem.Items[j].Value);
                                    string sub = Convert.ToString(ddl_pop1sub.SelectedItem.Value);

                                    string query = "update SubwiseRoomAllot set Subject_No ='" + ddl_pop1sub.SelectedItem.Value + "', Room_Name='" + ddl_pop1roomno.SelectedItem.Text + "' , Building_Name='" + txt_pop1bul.Text + "' , Floor_Name='" + txt_pop1floor.Text + "' where Batch_Year ='" + ddl_pop1batch.SelectedValue + "'and Degree_Code ='" + degcode + "' and Semester ='" + semcode + "'";
                                    int value = d2.update_method_wo_parameter(query, "Text");
                                    if (value != 0)
                                    {
                                        saveflage = true;
                                    }

                                    else
                                    {
                                        saveflage = false;
                                    }

                                }
                            }
                        }

                    }
                }
            }
            if (saveflage == true)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Updated Successfully";
            }
            else
            {
                //imgdiv2.Visible = true;
                //lblalerterr.Text = "Subject Already Exist";
                //popwindow1.Visible = true;
            }
        }
        catch
        {

        }

    }
    public void btn_pop1exit1_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
}