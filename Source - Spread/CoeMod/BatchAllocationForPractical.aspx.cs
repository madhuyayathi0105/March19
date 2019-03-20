using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

public partial class BatchAllocationForPractical : System.Web.UI.Page
{
    SqlConnection con5 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con6 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    FarPoint.Web.Spread.ComboBoxCellType cb = new FarPoint.Web.Spread.ComboBoxCellType();
    Hashtable ht = new Hashtable();
    SqlConnection con;
    string usercode = "";
    string[] name;
    string Master1 = "";
    Boolean flag_true = false;
    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        con.Open();

    }
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
            if (!IsPostBack)
            {
                Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                SqlCommand mtcmd = new SqlCommand(Master1, setcon);
                mtrdr = mtcmd.ExecuteReader();
                {
                    if (mtrdr.HasRows)
                    {
                        while (mtrdr.Read())
                        {
                            if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Rollflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Regflag"] = "1";
                            }
                        }
                    }
                }
                DAccess2 da = new DAccess2();
                DataSet ds = new DataSet();
                ds = da.select_method_wo_parameter("bind_batch", "sp");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBatch.Items.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlBatch.Items.Add(ds.Tables[0].Rows[i]["batch_year"].ToString());
                    }
                }

                binddegree();

                bindbranch();

                bindSem();
                bindsections();

                ddlSubjectType.Items.Insert(0, new ListItem("All", "0"));
                ddlSubjectType.Items.Insert(1, new ListItem("Regular", "1"));
                ddlSubjectType.Items.Insert(2, new ListItem("Arear", "2"));


                ddlSession.Items.Insert(0, new ListItem("F.N", "0"));
                ddlSession.Items.Insert(1, new ListItem("A.N", "1"));

                Fpstudents.Sheets[0].RowCount = 1;
                Fpstudents.Sheets[0].ColumnCount = 6;
            }
            Fpstudents.Sheets[0].Columns[0].Width = 60;
            Fpstudents.Sheets[0].Columns[1].Width = 100;
            Fpstudents.Sheets[0].Columns[2].Width = 150;
            Fpstudents.Sheets[0].Columns[3].Width = 250;
            Fpstudents.Sheets[0].Columns[4].Width = 50;
            Fpstudents.Sheets[0].Columns[5].Width = 50;
            Fpstudents.Sheets[0].Columns[0].Locked = true;
            Fpstudents.Sheets[0].Columns[1].Locked = true;
            Fpstudents.Sheets[0].Columns[2].Locked = true;
            Fpstudents.Sheets[0].Columns[3].Locked = true;
            Fpstudents.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            Fpstudents.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            Fpstudents.Sheets[0].Columns[3].CellType = cb;
            Fpstudents.Sheets[0].RowHeader.Visible = false;
            Fpstudents.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpstudents.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll.No";
            Fpstudents.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg.No";
            Fpstudents.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
            Fpstudents.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Batch";
            Fpstudents.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstudents.Sheets[0].Columns[5].CellType = chkcell;
            Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            Fpstudents.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            Fpstudents.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpstudents.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpstudents.Sheets[0].DefaultStyle.Font.Bold = false;
            Fpstudents.CommandBar.Visible = false;
            //Fpstudents.RowHeader.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        binddegree();
        bindSem();
        bindbranch();
        bindsections();
        ddlSubject.Items.Clear();
        bindsubject();
        btnsave.Visible = false;
        Fpstudents.Visible = false;
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindSem();
        bindbranch();
        bindsections();
        ddlSubject.Items.Clear();
        bindsubject();
        btnsave.Visible = false;
        Fpstudents.Visible = false;
        lblerror.Visible = false;
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlSubject.Items.Clear();
        bindSem();
        bindsections();
        bindsubject();
        btnsave.Visible = false;
        Fpstudents.Visible = false;
        lblerror.Visible = false;
    }
    public void bindSem1()
    {

        Connection();
        DAccess2 das = new DAccess2();
        DataSet dss = new DataSet();
        ht.Clear();
        ht.Add("batch_year", ddlBatch.SelectedItem.Text.ToString());
        ht.Add("college_code", Session["collegecode"].ToString());
        ht.Add("degree_code", ddlBranch.SelectedValue.ToString());
        dss = das.select_method("bind_sem", ht, "sp");
        if (dss.Tables[0].Rows.Count > 0)
        {
            ddlSem.Enabled = true;
            ddlSem.Items.Clear();
            for (int i = 1; i <= Convert.ToInt16(dss.Tables[0].Rows[0]["duration"].ToString()); i++)
            {
                ddlSem.Items.Add(i.ToString());
            }
        }
        else
        {

            ddlSem.Enabled = false;
        }

    }

    public void bindSem()
    {
        Connection();
        DAccess2 das = new DAccess2();
        DataSet dss = new DataSet();
        ddlSem.Items.Clear();
        string duration = "";
        Boolean first_year = false;
        ht.Clear();

        ht.Add("degree_code", ddlBranch.SelectedValue.ToString());
        ht.Add("batch_year", ddlBatch.SelectedItem.Text.ToString());
        ht.Add("college_code", Session["collegecode"].ToString());

        dss = das.select_method("bind_sem", ht, "sp");
        int count3 = dss.Tables[0].Rows.Count;
        if (count3 > 0)
        {
            ddlSem.Enabled = true;
            duration = dss.Tables[0].Rows[0][0].ToString();
            first_year = Convert.ToBoolean(dss.Tables[0].Rows[0][1].ToString());
            for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
            {
                if (first_year == false)
                {
                    ddlSem.Items.Add(loop_val.ToString());
                }
                else if (first_year == true && loop_val != 2)
                {
                    ddlSem.Items.Add(loop_val.ToString());
                }

            }
        }
        else
        {
            count3 = dss.Tables[1].Rows.Count;
            if (count3 > 0)
            {
                ddlSem.Enabled = true;
                duration = dss.Tables[1].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(dss.Tables[1].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlSem.Items.Add(loop_val.ToString());
                    }

                }
            }
            else
            {
                ddlSem.Enabled = false;
            }
        }

    }


    public void bindbranch()
    {
        Connection();
        DAccess2 da1 = new DAccess2();
        DataSet ds1 = new DataSet();
        string singleuser = Session["single_user"].ToString();
        string group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        ht.Clear();
        ht.Add("course_id", ddlDegree.SelectedValue.ToString());
        ht.Add("college_code", Session["collegecode"].ToString());
        ht.Add("user_code", Session["usercode"].ToString());
        ht.Add("single_user", singleuser);
        ht.Add("group_code", group_user);
        ds1 = da1.select_method("bind_branch", ht, "sp");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ddlBranch.Enabled = true;
            ddlBranch.Items.Clear();
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                ddlBranch.Items.Insert(i, new ListItem(ds1.Tables[0].Rows[i]["dept_name"].ToString(), ds1.Tables[0].Rows[i]["degree_code"].ToString()));
            }
        }
        else
        {

            ddlBranch.Enabled = false;

        }


    }
    public void binddegree()
    {
        Connection();
        usercode = Session["usercode"].ToString();
        DAccess2 da1 = new DAccess2();
        DataSet ds1 = new DataSet();
        string singleuser = Session["single_user"].ToString();
        string group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        ht.Clear();
        ht.Add("college_code", Session["collegecode"].ToString());
        ht.Add("User_code", usercode);
        ht.Add("single_user", singleuser);
        ht.Add("group_code", group_user);
        ds1 = da1.select_method("bind_degree", ht, "sp");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ddlDegree.Enabled = true;
            ddlDegree.Items.Clear();
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                ddlDegree.Items.Insert(i, new ListItem(ds1.Tables[0].Rows[i]["course_name"].ToString(), ds1.Tables[0].Rows[i]["course_id"].ToString()));
            }
        }
        else
        {
            ddlDegree.Enabled = false;

        }
    }
    public void bindsections()
    {
        Connection();
        DAccess2 das = new DAccess2();
        DataSet dss = new DataSet();
        ht.Clear();
        ht.Add("batch_year", ddlBatch.SelectedItem.Text.ToString());
        ht.Add("degree_code", ddlBranch.SelectedValue.ToString());
        dss = das.select_method("bind_sec", ht, "sp");
        if (dss.Tables[0].Rows.Count > 0)
        {
            ddlSection.Enabled = true;
            ddlSection.Items.Clear();
            ddlSection.Items.Insert(0, new ListItem("All", "0"));
            for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
            {
                ddlSection.Items.Add(dss.Tables[0].Rows[i]["sections"].ToString());
            }
        }
        else
        {
            ddlSection.Items.Insert(0, new ListItem("All", "0"));
            ddlSection.Enabled = false;
        }
    }
    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubject.Items.Clear();
        bindsections();
        bindsubject();
        btnsave.Visible = false;
        Fpstudents.Visible = false;
        lblerror.Visible = false;
    }
    protected void ddlSubjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsubject();
        btnsave.Visible = false;
        Fpstudents.Visible = false;
    }
    protected void bindsubject()
    {
        if (ddlSubjectType.SelectedItem.Text.ToString() != "All")
        {
            DAccess2 da = new DAccess2();
            DataSet ds = new DataSet();
            ht.Clear();
            if (ddlSection.SelectedItem.Text.ToString() != "" && ddlSubjectType.SelectedItem.Text.ToString() != "" && ddlBatch.SelectedItem.Text.ToString() != "" && ddlSem.SelectedValue.ToString() != "" && ddlBranch.SelectedValue.ToString() != "")
            {
                ht.Add("CollegeCode", Session["collegecode"].ToString());
                ht.Add("Sections", ddlSection.SelectedItem.Text.ToString());
                ht.Add("Subtype", ddlSubjectType.SelectedItem.Text.ToString());
                ht.Add("BatchYear", ddlBatch.SelectedItem.Text.ToString());
                ht.Add("Semester", ddlSem.SelectedItem.Text.ToString());
                ht.Add("DegreeCode", ddlBranch.SelectedValue.ToString());
                ds = da.select_method("ProcBatchAllocationSubjectDetails", ht, "sp");
                ddlSubject.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSubject.Enabled = true;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlSubject.Items.Insert(i, new ListItem(ds.Tables[0].Rows[i]["SubjectName"].ToString(), ds.Tables[0].Rows[i]["SubjectNo"].ToString()));

                    }

                }
                else
                {

                    ddlSubject.Enabled = false;
                }
            }
        }

        else
        {

            DAccess2 da = new DAccess2();
            DataSet ds = new DataSet();
            ht.Clear();
            if (ddlSection.SelectedItem.Text.ToString() != "" && ddlSubjectType.SelectedItem.Text.ToString() != "" && ddlBatch.SelectedItem.Text.ToString() != "" && ddlSem.SelectedValue.ToString() != "" && ddlBranch.SelectedValue.ToString() != "")
            {
                ht.Add("CollegeCode", Session["collegecode"].ToString());
                ht.Add("Sections", ddlSection.SelectedItem.Text.ToString());
                ht.Add("Subtype", ddlSubjectType.SelectedItem.Text.ToString());
                ht.Add("BatchYear", ddlBatch.SelectedItem.Text.ToString());
                ht.Add("Semester", ddlSem.SelectedItem.Text.ToString());
                ht.Add("DegreeCode", ddlBranch.SelectedValue.ToString());
                ds = da.select_method("ProcBatchAllocationSubjectDetails", ht, "sp");
                ddlSubject.Items.Clear();
                int i = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlSubject.Items.Insert(i, new ListItem(ds.Tables[0].Rows[i]["SubjectName"].ToString(), ds.Tables[0].Rows[i]["SubjectNo"].ToString()));
                        //ddlSubject.DataSource = ds;
                        //ddlSubject.DataValueField = "SubjectNo";
                        //ddlSubject.DataTextField = "SubjectName";
                        //ddlSubject.DataBind();
                    }

                }

                if (ds.Tables[1].Rows.Count > 0)
                {

                    ddlSubject.Enabled = true;
                    int n = 0;
                    n = i;
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {

                        ddlSubject.Items.Insert(n, new ListItem(ds.Tables[1].Rows[j]["SubjectName"].ToString(), ds.Tables[1].Rows[j]["SubjectNo"].ToString()));
                        n = n + 1;

                    }

                }

                if (ds.Tables[0].Rows.Count < 1 && ds.Tables[1].Rows.Count < 1)
                {

                    ddlSubject.Enabled = false;
                }
                else
                {
                    ddlSubject.Enabled = true;

                }
            }

        }
    }
    protected void Fpstudents_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(Fpstudents.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    Fpstudents.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        Fpstudents.Visible = false;

        string batch = ddlBatch.SelectedValue.ToString();
        string degree_code = ddlBranch.SelectedValue.ToString();
        string sem = ddlSem.SelectedValue;
        string subjectno = ddlSubject.SelectedValue.ToString();
        string examdate = "";
        string examcode = "";
        string examdate1 = txtExamDate.Text;
        string datechangenew = "";
        string datechange = "";
        if (txtExamDate.Text != "")
        {
            string[] splitdate = examdate1.Split(new Char[] { '/' });
            string reqdate = splitdate[0].ToString();
            string reqmonth = splitdate[1].ToString();
            string reqyear = splitdate[2].ToString();
            datechange = reqmonth + "-" + reqdate + "-" + reqyear;
            datechangenew = reqdate + "-" + reqmonth + "-" + reqyear;
        }
        if (batch != "" && degree_code != "" && sem != "" && subjectno != "")
        {
            string examdatequery = "select convert(varchar(10),exam_date,105) as exam_date,ex.exam_code from exmtt ex,exmtt_det ed where ex.exam_code=ed.exam_code and ex.degree_code=" + degree_code + " and ex.batchto=" + batch + " and ex.semester=" + sem + " and ed.subject_no=" + subjectno + "";
            SqlDataAdapter daexamdatequery = new SqlDataAdapter(examdatequery, con5);
            DataSet dsexamdatequery = new DataSet();
            con5.Close();
            con5.Open();
            daexamdatequery.Fill(dsexamdatequery);

            if (dsexamdatequery.Tables[0].Rows.Count > 0)
            {
                examdate = Convert.ToString(dsexamdatequery.Tables[0].Rows[0]["exam_date"]);
                examcode = Convert.ToString(dsexamdatequery.Tables[0].Rows[0]["exam_code"]);
            }
            if (examdate != "")
            {
                if (txBatch.Text != "")
                {
                    if (txtExamDate.Text != "")
                    {
                        Connection();
                        int n = Convert.ToInt16(txBatch.Text.ToString());
                        name = new string[n + 1];
                        int mm = 0;
                        for (int j = 1; j <= n; j++)
                        {

                            name[j] = ("B" + j);
                            name[0] = " ";
                        }
                        string[] strcomo = new string[] { " " };
                        cb = new FarPoint.Web.Spread.ComboBoxCellType(name);
                        cb.AutoPostBack = true;

                        Hashtable ht = new Hashtable();
                        ht.Clear();
                        DAccess2 da = new DAccess2();
                        DataSet ds = new DataSet();
                        if (ddlSem.SelectedValue.ToString() != "" && ddlBranch.SelectedValue.ToString() != "" && ddlSection.SelectedItem.Text.ToString() != "" && ddlBatch.SelectedItem.Text.ToString() != "" && ddlSubject.SelectedValue.ToString() != "")
                        {
                            ht.Add("Semester", ddlSem.SelectedItem.Text.ToString());
                            ht.Add("CollegeCode", Session["collegecode"].ToString());
                            ht.Add("DegreeCode", ddlBranch.SelectedValue.ToString());
                            ht.Add("Sections", ddlSection.SelectedItem.Text.ToString());
                            ht.Add("BatchYear", ddlBatch.SelectedItem.Text.ToString());
                            ht.Add("SubjectNo", ddlSubject.SelectedValue.ToString());
                            ds = da.select_method("ProcBatchAllocationRegNoDetails", ht, "sp");
                            Fpstudents.Sheets[0].RowCount = 0;
                            Fpstudents.Sheets[0].RowCount = ds.Tables[0].Rows.Count + 1;
                            Fpstudents.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1 * 20;
                            Fpstudents.Sheets[0].SpanModel.Add(0, 0, 1, 5);
                            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            Fpstudents.Sheets[0].Cells[0, 5].CellType = chkcell1;
                            Fpstudents.Sheets[0].Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpstudents.Sheets[0].FrozenRowCount = 1;
                            chkcell1.AutoPostBack = true;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                btnsave.Visible = true;
                                Fpstudents.Visible = true;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    string batch1 = "";

                                    if (txtExamDate.Text != "")
                                    {
                                        string selectbatch = "select batch from batch_allocation_practical where batch_year=" + ddlBatch.SelectedItem.Text.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSem.SelectedValue.ToString() + " and exam_code=" + examcode + " and subject_no=" + ddlSubject.SelectedValue.ToString() + " and roll_no='" + ds.Tables[0].Rows[i]["RollNo"].ToString() + "' and subject_no=" + subjectno + "";
                                        SqlDataAdapter daselectbatch = new SqlDataAdapter(selectbatch, con5);
                                        DataSet dsselectbatch = new DataSet();
                                        con5.Close();
                                        con5.Open();
                                        daselectbatch.Fill(dsselectbatch);

                                        if (dsselectbatch.Tables[0].Rows.Count > 0)
                                        {
                                            batch1 = dsselectbatch.Tables[0].Rows[0]["batch"].ToString();
                                        }
                                    }
                                    Fpstudents.Sheets[0].Cells[i + 1, 0].Text = Convert.ToString(i + 1);
                                    Fpstudents.Sheets[0].Cells[i + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpstudents.Sheets[0].Cells[i + 1, 1].Text = ds.Tables[0].Rows[i]["RollNo"].ToString();
                                    Fpstudents.Sheets[0].Cells[i + 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    Fpstudents.Sheets[0].Cells[i + 1, 2].Text = ds.Tables[0].Rows[i]["RegNo"].ToString();
                                    Fpstudents.Sheets[0].Cells[i + 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    Fpstudents.Sheets[0].Cells[i + 1, 3].Text = ds.Tables[0].Rows[i]["StudentName"].ToString();
                                    Fpstudents.Sheets[0].Cells[i + 1, 4].CellType = cb;
                                    Fpstudents.Sheets[0].SetText(i + 1, 4, batch1);
                                }

                            }
                            int rowcount = Fpstudents.Sheets[0].RowCount;
                            Fpstudents.Sheets[0].RowCount = rowcount;
                            Fpstudents.Height = rowcount * 20;
                            if (Session["Rollflag"] == "0")
                            {
                                Fpstudents.Width = 560;
                                Fpstudents.Sheets[0].Columns[1].Visible = false;
                            }
                            if (Session["Regflag"] == "0")
                            {
                                Fpstudents.Width = 510;
                                Fpstudents.Sheets[0].Columns[2].Visible = false;
                                if (Session["Rollflag"] == "0")
                                {
                                    Fpstudents.Width = 410;
                                }
                            }
                        }
                    }
                    else
                    {
                        btnsave.Visible = false;
                        Fpstudents.Visible = false;
                        lblerror.Visible = true;
                        lblerror.Text = "Exam Date Not Filled";
                    }
                }
                else
                {
                    btnsave.Visible = false;
                    Fpstudents.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "No of Batches not Filled";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No of Batches is not Filled')", true);
                }
            }
            else
            {
                btnsave.Visible = false;
                Fpstudents.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Time Table not created for this Subject";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Time Table is not created for this Subject')", true);
            }
        }

    }
    protected void insertvalues()
    {
        lblerror.Visible = false;
        Fpstudents.SaveChanges();
        string batchyear = ddlBatch.SelectedValue.ToString();
        string degree_code = ddlBranch.SelectedValue.ToString();
        string sem = ddlSem.SelectedValue;
        string subjectno = ddlSubject.SelectedValue.ToString();
        string session = ddlSession.SelectedItem.Text;
        string examdate = txtExamDate.Text;
        string examdatefrmexmtt = "";
        string examcode = "";
        if (batchyear != "" && degree_code != "" && sem != "" && subjectno != "")
        {
            string examcodequery = "select ex.exam_code as examcode,convert(varchar(10),exam_date,105) as exam_date from exmtt ex,exmtt_det ed where ex.exam_code=ed.exam_code and ex.degree_code=" + degree_code + " and ex.batchto=" + batchyear + " and ex.semester=" + sem + " and ed.subject_no=" + subjectno + "";
            SqlDataAdapter daexamcodequery = new SqlDataAdapter(examcodequery, con5);
            DataSet dsexamcodequery = new DataSet();
            con5.Close();
            con5.Open();
            daexamcodequery.Fill(dsexamcodequery);

            if (dsexamcodequery.Tables[0].Rows.Count > 0)
            {
                examcode = Convert.ToString(dsexamcodequery.Tables[0].Rows[0]["examcode"]);
                examdatefrmexmtt = Convert.ToString(dsexamcodequery.Tables[0].Rows[0]["exam_date"]);
            }
        }

        if (examcode != "" & examdate != "")
        {
            int flag1 = 0;
            int flag = 0;
            string[] splitdate = examdate.Split(new Char[] { '/' });
            string reqdate = splitdate[0].ToString();
            string reqmonth = splitdate[1].ToString();
            string reqyear = splitdate[2].ToString();
            string datechange = reqmonth + "-" + reqdate + "-" + reqyear;
            string datechangenew = reqdate + "-" + reqmonth + "-" + reqyear;
            DateTime date1 = Convert.ToDateTime(datechange);
            string datenew = date1.ToString("dd");

            string[] splitdate1 = examdatefrmexmtt.Split(new Char[] { '-' });
            string reqdate1 = splitdate1[0].ToString();
            string reqmonth1 = splitdate1[1].ToString();
            string reqyear1 = splitdate1[2].ToString();
            string todatechange = reqmonth1 + "-" + reqdate1 + "-" + reqyear1;
            DateTime todate1 = Convert.ToDateTime(todatechange);
            string todatenew = todate1.ToString("dd");
            TimeSpan ts = date1.Subtract(todate1);
            int d = ts.Days;
            if (d >= 0)
            {
                string batch1 = "";
                for (int i = 1; i <= Fpstudents.Sheets[0].RowCount - 1; i++)
                {
                    int isval = 0;
                    string s = Fpstudents.Sheets[0].Cells[i, 5].Text;

                    isval = Convert.ToInt32(Fpstudents.Sheets[0].Cells[i, 5].Value);
                    if (isval == 1)
                    {
                        flag1 = 1;
                        batch1 = Convert.ToString(Fpstudents.Sheets[0].GetText(i, 4));
                        if (batch1 == null)
                        {
                            flag = 1;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('batch is not filled for selected students')", true);
                        }
                    }
                }
                if (flag1 == 0)
                {
                    Fpstudents.Visible = false;
                    btnsave.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "Select Students From the List";

                }
                if (flag == 0)
                {
                    string checkexamdatecontains = "select * from exmtt ex,exmtt_det ed where ex.exam_code=ed.exam_code and ex.degree_code=" + degree_code + " and ex.batchto=" + batchyear + " and ex.semester=" + sem + " and convert(varchar(10),ed.exam_date,105)='" + datechangenew + "'";
                    SqlDataAdapter dacheckexamdatecontains = new SqlDataAdapter(checkexamdatecontains, con5);
                    DataSet dscheckexamdatecontains = new DataSet();
                    con5.Close();
                    con5.Open();
                    dacheckexamdatecontains.Fill(dscheckexamdatecontains);
                    string rollno = "";
                    string batch = "";
                    if (dscheckexamdatecontains.Tables[0].Rows.Count > 0)
                    {
                        string checksubject_nocontains = "select * from exmtt ex,exmtt_det ed where ex.exam_code=ed.exam_code and ex.degree_code=" + degree_code + " and ex.batchto=" + batchyear + " and ex.semester=" + sem + " and convert(varchar(10),ed.exam_date,105)='" + datechangenew + "' and ed.subject_no=" + subjectno + "";
                        SqlDataAdapter dachecksubject_nocontains = new SqlDataAdapter(checksubject_nocontains, con5);
                        DataSet dschecksubject_nocontains = new DataSet();
                        con5.Close();
                        con5.Open();
                        dachecksubject_nocontains.Fill(dschecksubject_nocontains);
                        if (dschecksubject_nocontains.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 1; i <= Fpstudents.Sheets[0].RowCount - 1; i++)
                            {
                                int isval = 0;
                                string s = Fpstudents.Sheets[0].Cells[i, 5].Text;

                                isval = Convert.ToInt32(Fpstudents.Sheets[0].Cells[i, 5].Value);
                                if (isval == 1)
                                {
                                    rollno = Fpstudents.Sheets[0].Cells[i, 1].Text;
                                    batch = Convert.ToString(Fpstudents.Sheets[0].GetText(i, 4));
                                    if (batch == null)
                                    {
                                        batch = "-";
                                    }
                                    string selectquery = "Select * from batch_allocation_practical where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + sem + " and exam_code=" + examcode + "  and roll_no='" + rollno + "'  and subject_no=" + subjectno + " ";
                                    SqlCommand selectbatchcmd = new SqlCommand(selectquery, con5);
                                    con5.Close();
                                    con5.Open();
                                    SqlDataReader selectbatchreader;
                                    selectbatchreader = selectbatchcmd.ExecuteReader();
                                    if (selectbatchreader.HasRows)
                                    {
                                        Fpstudents.Visible = false;
                                        btnsave.Visible = false;
                                        lblerror.Visible = true;
                                        lblerror.Text = "Students Already Allocated for Some other subject";
                                    }
                                    else
                                    {
                                        string selectsubnoquery = "Select * from batch_allocation_practical where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + sem + " and exam_code=" + examcode + "  and roll_no='" + rollno + "' and exam_date='" + datechange + "' ";
                                        SqlCommand selectsubnoquerycmd = new SqlCommand(selectsubnoquery, con6);
                                        con6.Close();
                                        con6.Open();
                                        SqlDataReader selectsubnoqueryreader;
                                        selectsubnoqueryreader = selectsubnoquerycmd.ExecuteReader();
                                        if (selectsubnoqueryreader.HasRows)
                                        {
                                            Fpstudents.Visible = false;
                                            btnsave.Visible = false;
                                            lblerror.Visible = true;
                                            lblerror.Text = "Students Already Allocated for Some other subject";
                                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Already this Subject is alloted for students')", true);
                                            //string updatequery = "update batch_allocation_practical set batch='" + batch + "',exam_date='" + datechange + "', session='" + session + "'  where batch_year=" + batchyear + " and degree_code=" + degree_code + "  and semester=" + sem + " and exam_code=" + examcode + "and subject_no=" + subjectno + "  and roll_no='" + rollno + "'";

                                            //SqlCommand updatedummycmd = new SqlCommand(updatequery, con5);
                                            //con5.Close();
                                            //con5.Open();
                                            //updatedummycmd.ExecuteNonQuery();
                                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('updated')", true);
                                        }

                                        else
                                        {

                                            string insertquery = "insert into batch_allocation_practical (batch_year,degree_code,semester,Exam_code,subject_no,exam_date,session,Roll_no,batch) values(" + batchyear + "," + degree_code + "," + sem + "," + examcode + "," + subjectno + ",'" + datechange + "','" + session + "','" + rollno + "','" + batch + "')";
                                            SqlCommand createdummycmd = new SqlCommand(insertquery, con5);
                                            con5.Close();
                                            con5.Open();
                                            createdummycmd.ExecuteNonQuery();
                                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            btnsave.Visible = false;
                            Fpstudents.Visible = false;
                            lblerror.Visible = true;
                            lblerror.Text = "batch Allocated for this Subject So Pleaze Choose New Date";
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= Fpstudents.Sheets[0].RowCount - 1; i++)
                        {
                            int isval = 0;
                            string s = Fpstudents.Sheets[0].Cells[i, 5].Text;

                            isval = Convert.ToInt32(Fpstudents.Sheets[0].Cells[i, 5].Value);
                            if (isval == 1)
                            {
                                rollno = Fpstudents.Sheets[0].Cells[i, 1].Text;
                                batch = Convert.ToString(Fpstudents.Sheets[0].GetText(i, 4));
                                if (batch == "")
                                {
                                    batch = "-";
                                }
                                string selectquery = "Select * from batch_allocation_practical where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + sem + " and exam_code=" + examcode + "  and roll_no='" + rollno + "'  and subject_no=" + subjectno + " ";
                                SqlCommand selectbatchcmd = new SqlCommand(selectquery, con5);
                                con5.Close();
                                con5.Open();
                                SqlDataReader selectbatchreader;
                                selectbatchreader = selectbatchcmd.ExecuteReader();
                                if (selectbatchreader.HasRows)
                                {
                                    Fpstudents.Visible = false;
                                    btnsave.Visible = false;
                                    lblerror.Visible = true;
                                    lblerror.Text = "Students Already Allocated for Some other subject";
                                }
                                else
                                {
                                    string selectsubnoquery = "Select * from batch_allocation_practical where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + sem + " and exam_code=" + examcode + "  and roll_no='" + rollno + "' and exam_date='" + datechange + "' ";
                                    SqlCommand selectsubnoquerycmd = new SqlCommand(selectsubnoquery, con6);
                                    con6.Close();
                                    con6.Open();
                                    SqlDataReader selectsubnoqueryreader;
                                    selectsubnoqueryreader = selectsubnoquerycmd.ExecuteReader();
                                    if (selectsubnoqueryreader.HasRows)
                                    {
                                        Fpstudents.Visible = false;
                                        btnsave.Visible = false;
                                        lblerror.Visible = true;
                                        lblerror.Text = "Students Already Allocated for Some other subject";
                                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Already this Subject is alloted for students')", true);
                                        //string updatequery = "update batch_allocation_practical set batch='" + batch + "',exam_date='" + datechange + "', session='" + session + "'  where batch_year=" + batchyear + " and degree_code=" + degree_code + "  and semester=" + sem + " and exam_code=" + examcode + "and subject_no=" + subjectno + "  and roll_no='" + rollno + "'";

                                        //SqlCommand updatedummycmd = new SqlCommand(updatequery, con5);
                                        //con5.Close();
                                        //con5.Open();
                                        //updatedummycmd.ExecuteNonQuery();
                                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('updated')", true);
                                    }

                                    else
                                    {

                                        string insertquery = "insert into batch_allocation_practical (batch_year,degree_code,semester,Exam_code,subject_no,exam_date,session,Roll_no,batch) values(" + batchyear + "," + degree_code + "," + sem + "," + examcode + "," + subjectno + ",'" + datechange + "','" + session + "','" + rollno + "','" + batch + "')";
                                        SqlCommand createdummycmd = new SqlCommand(insertquery, con5);
                                        con5.Close();
                                        con5.Open();
                                        createdummycmd.ExecuteNonQuery();
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                                    }
                                }
                            }
                        }

                        //lblerror.Visible = true;
                        //lblerror.Text = "This Date is Allocated So Pleaze Choose New Date";
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('This Date is Allocated So Pleaze Choose New Date')", true);
                    }
                }
            }
            else
            {
                btnsave.Visible = false;
                Fpstudents.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "Date Should be greater than Starting date " + examdatefrmexmtt + "'";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Date should be greater than Exam Starting date "+examdatefrmexmtt +"')", true);
            }
        }
        else
        {
            btnsave.Visible = false;
            Fpstudents.Visible = false;
            lblerror.Visible = true;
            lblerror.Text = "Exam Date Not valid";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('exam date is not valid')", true);
        }
    }
    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnsave.Visible = false;
        Fpstudents.Visible = false;

    }
    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubject.Items.Clear();
        bindsubject();
        Fpstudents.Visible = false;
        btnsave.Visible = false;
        lblerror.Visible = false;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        insertvalues();
    }
    protected void txtExamDate_TextChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        btnsave.Visible = false;
        Fpstudents.Visible = false;
    }
    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        btnsave.Visible = false;
        Fpstudents.Visible = false;
    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

}